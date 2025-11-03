using Portal.Base;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web.UI;

namespace Portal.ModulCimer
{
    public partial class Incelenen : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckPermission(Sabitler.CIMER_PERSONEL))
            {
                return;
            }

            if (!IsPostBack)
            {
                Yukle();
            }
        }

        private void Yukle()
        {
            try
            {
                string query = @"
                    SELECT id, Basvuru_No, Basvuru_Tarihi, TC_No, Adi_Soyadi, Tel_No, Mail, Adres, 
                           Basvuru_Metni, Basvuru_Ek, Yapilan_İslem, Son_Yapilan_islem, 
                           Sikayet_Edilen_Firma, Guncelleyen_Kullanici, Guncelleme_Tarihi, Bekleme_Durumu
                    FROM cimer_basvurular 
                    WHERE Guncelleyen_Kullanici = @Kullanici AND Bekleme_Durumu = 'Evet' 
                    ORDER BY id ASC";

                var parameters = CreateParameters(
                    ("@Kullanici", CurrentUserName)
                );

                DataTable dt = ExecuteDataTable(query, parameters);
                GridViewBasvurular.DataSource = dt;
                GridViewBasvurular.DataBind();

                // Firma dropdown yükle
                var firmaQuery = "SELECT Firma_Unvan FROM cimer_firmalar ORDER BY Firma_Unvan ASC";
                DataTable firmaDt = ExecuteDataTable(firmaQuery, null);
                ddlFirmalar.DataSource = firmaDt;
                ddlFirmalar.DataTextField = "Firma_Unvan";
                ddlFirmalar.DataValueField = "Firma_Unvan";
                ddlFirmalar.DataBind();

                // Onay kullanıcısı dropdown yükle
                string personelTipi = Session["Ptipi"]?.ToString() ?? "0";
                string whereClause = personelTipi == "0" ? "AND Personel_Tipi = '1'" : "";
                string onayQuery = $@"
                    SELECT Adi_Soyadi FROM kullanici 
                    WHERE Durum = 'Aktif' {whereClause} 
                    ORDER BY Adi_Soyadi ASC";

                DataTable onayDt = ExecuteDataTable(onayQuery, null);
                ddlOnayKullanici.DataSource = onayDt;
                ddlOnayKullanici.DataTextField = "Adi_Soyadi";
                ddlOnayKullanici.DataValueField = "Adi_Soyadi";
                ddlOnayKullanici.DataBind();
            }
            catch (Exception ex)
            {
                LogError("Başvuru yükleme hatası", ex);
                ShowErrorAndRedirect("Veriler yüklenirken hata oluştu. Lütfen tekrar deneyin.", "~/Anasayfa.aspx");
            }
        }

        protected void GridViewBasvurular_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int selectedId = Convert.ToInt32(GridViewBasvurular.SelectedDataKey.Value);
                string query = @"
                    SELECT Basvuru_No, Basvuru_Metni, Yapilan_İslem, Son_Yapilan_islem, Sikayet_Edilen_Firma
                    FROM cimer_basvurular 
                    WHERE id = @Id";

                var parameters = CreateParameters(
                    ("@Id", selectedId)
                );

                DataTable dt = ExecuteDataTable(query, parameters);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtBasvuruNo.Text = row["Basvuru_No"].ToString();
                    txtBasvuruMetni.Text = row["Basvuru_Metni"].ToString();
                    txtYapilanIslem.Text = row["Yapilan_İslem"].ToString();
                    txtSonYapilanIslem.Text = row["Son_Yapilan_islem"].ToString();
                    ddlFirmalar.SelectedValue = row["Sikayet_Edilen_Firma"].ToString();

                    pnlDetay.Attributes["class"] = "detail-panel show";
                    pnlTarihce.Attributes["class"] = "history-panel";
                }
            }
            catch (Exception ex)
            {
                LogError("Başvuru seçimi hatası", ex);
                ShowError("Başvuru detayları yüklenirken hata oluştu.");
            }
        }

        protected void btnKaydet_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid || string.IsNullOrEmpty(txtBasvuruNo.Text)) return;

            SqlTransaction transaction = null;
            try
            {
                using (SqlConnection conn = GetOpenConnection())
                {
                    transaction = conn.BeginTransaction();

                    // Hareket ekle
                    Helpers.InsertCimerMovement(conn, transaction, txtBasvuruNo.Text, CurrentUserName,
                        ddlOnayKullanici.SelectedValue, txtAciklama.Text, 4, ddlDurum.SelectedValue);

                    // Başvuru güncelle
                    string updateQuery = @"
                        UPDATE cimer_basvurular 
                        SET Son_Yapilan_islem = @SonIslem, Sonuc = @Sonuc, Guncelleyen_Kullanici = @Guncelleyen, 
                            Guncelleme_Tarihi = @GuncTarih, Onay_Durumu = '1', Son_Kullanici = @SonKullanici 
                        WHERE Basvuru_No = @BasvuruId";

                    var updateParams = CreateParameters(
                        ("@SonIslem", txtSonYapilanIslem.Text),
                        ("@Sonuc", ddlDurum.SelectedValue),
                        ("@Guncelleyen", CurrentUserName),
                        ("@GuncTarih", DateTime.Now),
                        ("@SonKullanici", ddlOnayKullanici.SelectedValue),
                        ("@BasvuruId", txtBasvuruNo.Text)
                    );

                    ExecuteNonQueryWithTransaction(conn, transaction, updateQuery, updateParams);

                    transaction.Commit();

                    LogInfo($"Başvuru güncellendi ve harekete eklendi: {txtBasvuruNo.Text} - {ddlDurum.SelectedValue}");

                    ScriptManager.RegisterStartupScript(this, GetType(), "success", $"alert('İşlem kaydedildi ve onaya gönderildi.'); window.location='Incelenen.aspx';", true);
                }
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                LogError("Başvuru güncelleme hatası", ex);
                ShowError("Kaydetme sırasında hata oluştu. Lütfen tekrar deneyin.");
            }
        }

        protected void btnTarihce_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBasvuruNo.Text)) return;

            try
            {
                string query = @"
                    SELECT Sevk_Eden, Teslim_Alan, Tarih, Aciklama, islem_Aciklama 
                    FROM cimer_basvuru_hareketleri 
                    WHERE Basvuru_id = @BasvuruId 
                    ORDER BY id DESC";

                var parameters = CreateParameters(
                    ("@BasvuruId", txtBasvuruNo.Text)
                );

                DataTable dt = ExecuteDataTable(query, parameters);

                StringBuilder html = new StringBuilder();
                html.Append("<div class='table-responsive'><table class='table table-bordered table-striped'><thead><tr>");
                html.Append("<th>Sevk Eden</th><th>Teslim Alan</th><th>Tarih</th><th>Açıklama</th><th>İşlem</th>");
                html.Append("</tr></thead><tbody>");

                foreach (DataRow row in dt.Rows)
                {
                    html.Append("<tr>");
                    html.Append($"<td>{row["Sevk_Eden"]}</td>");
                    html.Append($"<td>{row["Teslim_Alan"]}</td>");
                    html.Append($"<td>{FormatDateTimeTurkish(Convert.ToDateTime(row["Tarih"]))}</td>");
                    html.Append($"<td>{row["Aciklama"]}</td>");
                    html.Append($"<td>{row["islem_Aciklama"]}</td>");
                    html.Append("</tr>");
                }

                html.Append("</tbody></table></div>");
                litTarihce.Text = html.ToString();

                pnlDetay.Attributes["class"] = "detail-panel";
                pnlTarihce.Attributes["class"] = "history-panel show";
            }
            catch (Exception ex)
            {
                LogError("Tarihçe yükleme hatası", ex);
                ShowError("Tarihçe yüklenirken hata oluştu.");
            }
        }

        protected void btnKapat_Click(object sender, EventArgs e)
        {
            pnlDetay.Attributes["class"] = "detail-panel";
            pnlTarihce.Attributes["class"] = "history-panel";
            Yukle();
        }

        protected void btnTarihceKapat_Click(object sender, EventArgs e)
        {
            pnlTarihce.Attributes["class"] = "history-panel";
            pnlDetay.Attributes["class"] = "detail-panel show";
        }       

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewBasvurular.AllowPaging = false;
                Yukle(); // Yeniden yükle sayfalama olmadan

                // Gizli sütunları göster Excel için
                GridViewBasvurular.Columns[3].Visible = true;
                GridViewBasvurular.Columns[4].Visible = true;
                GridViewBasvurular.Columns[5].Visible = true;
                GridViewBasvurular.Columns[6].Visible = true;
                GridViewBasvurular.Columns[7].Visible = true;

                ExportGridViewToExcel(GridViewBasvurular, "CimerTakipEdilen.xls");
            }
            catch (Exception ex)
            {
                LogError("Excel export hatası", ex);
                ShowError("Excel export sırasında hata oluştu.");
            }
        }
    }
}