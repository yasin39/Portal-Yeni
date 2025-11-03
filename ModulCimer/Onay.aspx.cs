using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Base;

namespace Portal.ModulCimer
{
    public partial class Onay : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(Sabitler.CIMER_PERSONEL))
                {
                    return;
                }
                Listele();
            }
        }

        private void Listele()
        {
            try
            {
                string query = @"
                    SELECT * FROM cimer_basvurular 
                    WHERE Son_Kullanici = @SonKullanici AND Onay_Durumu = @OnayDurumu 
                    ORDER BY id DESC";

                var parameters = CreateParameters(
                    ("@SonKullanici", CurrentUserName),
                    ("@OnayDurumu", Sabitler.ONAY_BEKLIYOR.ToString())
                );

                DataTable dt = ExecuteDataTable(query, parameters);
                GridView1.DataSource = dt;
                GridView1.DataBind();

                // Kullanıcı dropdown'ını doldur
                ddlKullanici.Visible = true;
                rfvKullanici.Enabled = true;
                if (Session["Ptipi"].ToString() == Sabitler.NORMAL_PERSONEL.ToString())
                {
                    ddlKullanici.Visible = false;
                    rfvKullanici.Enabled = false;
                }
                else
                {
                    string queryUsers = "SELECT Adi_Soyadi FROM kullanici WHERE Durum = 'Aktif' ORDER BY Adi_Soyadi ASC";
                    PopulateDropDownList(ddlKullanici, queryUsers, "Adi_Soyadi", "Adi_Soyadi", true);
                }

                LogInfo($"CİMER onay listesi yüklendi. Kayıt sayısı: {dt.Rows.Count}");
            }
            catch (Exception ex)
            {
                LogError("Listele hatası", ex);
                ShowErrorAndRedirect("Başvurular listelenirken hata oluştu.", "~/Anasayfa.aspx");
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (GridView1.SelectedDataKey == null || GridView1.SelectedDataKey.Values["id"] == null)
                {
                    throw new InvalidOperationException("Seçili satır bulunamadı veya ID değeri yok.");
                }

                int selectedId = Convert.ToInt32(GridView1.SelectedDataKey.Values["id"]);

                // Seçili kaydın detaylarını yükle
                string query = @"
                    SELECT Basvuru_No, Basvuru_Metni, Yapilan_İslem, Son_Yapilan_islem, Sonuc 
                    FROM cimer_basvurular 
                    WHERE id = @Id";

                var parameters = CreateParameters(("@Id", selectedId));
                DataTable dt = ExecuteDataTable(query, parameters);

                if (dt.Rows.Count == 0)
                {
                    throw new InvalidOperationException("Seçili başvuru bulunamadı.");
                }

                DataRow row = dt.Rows[0];
                lblBasvuruNo.Text = row["Basvuru_No"].ToString();
                txtBasvuruNo.Text = row["Basvuru_No"].ToString();
                txtAciklama.Text = string.Empty;
                txtYapilanIslem.Text = row["Yapilan_İslem"]?.ToString() ?? string.Empty;
                txtSonYapilanIslem.Text = row["Son_Yapilan_islem"]?.ToString() ?? string.Empty;
                txtSurecDurum.Text = row["Sonuc"]?.ToString() ?? string.Empty;

                txtBasvuruMetniFull.Text = row["Basvuru_Metni"].ToString();

                pnlApproval.Visible = true;
                pnlHistory.Visible = false;

                LogInfo($"Başvuru seçildi: ID={selectedId}, No={row["Basvuru_No"]}");
            }
            catch (Exception ex)
            {
                LogError("Grid seçimi hatası", ex);
                ShowErrorAndRedirect($"Başvuru seçilirken hata oluştu: {ex.Message}", Request.RawUrl);
            }
        }

        private void EvrakGecmis(string basvuruId)
        {
            try
            {
                lblTable.Text = Helpers.BuildDocumentHistoryHtml(basvuruId);
                LogInfo($"Evrak geçmişi gösterildi: Başvuru {basvuruId}");
            }
            catch (Exception ex)
            {
                LogError("Evrak geçmişi hatası", ex);
                lblTable.Text = "<p class='text-danger'>Geçmiş yüklenirken hata oluştu.</p>";
            }
        }

        protected void Hareket_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBasvuruNo.Text))
            {
                ShowToast("Önce bir başvuru seçiniz.","warning");
                return;
            }
            EvrakGecmis(txtBasvuruNo.Text);
            pnlHistory.Visible = true;
            pnlApproval.Visible = false;
        }

        protected void GecmisKapat_Click(object sender, EventArgs e)
        {
            pnlHistory.Visible = false;
            pnlApproval.Visible = true;
        }

        protected void Onayla_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid || string.IsNullOrEmpty(txtBasvuruNo.Text)) return;

            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Hareket tablosuna ekle
                            string teslimAlan = CurrentUserName == "Kemal YILMAZ" ? "Kayıt Kullanıcısı" : ddlKullanici.SelectedValue;
                            Helpers.InsertCimerMovement(conn, transaction, txtBasvuruNo.Text, CurrentUserName,
                                teslimAlan, txtAciklama.Text, 1, "Onaylandı");

                            // Ana tabloyu güncelle
                            string updateQuery;
                            List<SqlParameter> paramsUpdate = new List<SqlParameter>
                            {
                                CreateParameter("@BasvuruId", txtBasvuruNo.Text),
                                CreateParameter("@Tarih", DateTime.Now),
                                CreateParameter("@Guncelleyen", CurrentUserName)
                            };

                            if (Session["Ptipi"].ToString() == Sabitler.NORMAL_PERSONEL.ToString())
                            {
                                updateQuery = @"
                                    UPDATE cimer_basvurular 
                                    SET Onay_Durumu = @OnayDurumuSon, Son_Kullanici = Kayit_Kullanici, 
                                        Guncelleme_Tarihi = @Tarih, Guncelleyen_Kullanici = @Guncelleyen
                                    WHERE Basvuru_No = @BasvuruId";
                                paramsUpdate.Add(CreateParameter("@OnayDurumuSon", Sabitler.BITTI.ToString()));
                            }
                            else
                            {
                                updateQuery = @"
                                    UPDATE cimer_basvurular 
                                    SET Onay_Durumu = @OnayDurumuBekle, Son_Kullanici = @SonKullanici, 
                                        Guncelleme_Tarihi = @Tarih, Guncelleyen_Kullanici = @Guncelleyen
                                    WHERE Basvuru_No = @BasvuruId";
                                paramsUpdate.Add(CreateParameter("@OnayDurumuBekle", Sabitler.ONAY_BEKLIYOR.ToString()));
                                paramsUpdate.Add(CreateParameter("@SonKullanici", ddlKullanici.SelectedValue));
                            }

                            ExecuteNonQueryWithTransaction(conn, transaction, updateQuery, paramsUpdate);

                            transaction.Commit();
                            LogInfo($"CİMER başvuru onaylandı: {txtBasvuruNo.Text} - {teslimAlan}");
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                // Paneli gizle ve listeyi yenile
                pnlApproval.Visible = false;
                Listele();
                ShowToast("Başvuru başarıyla onaylandı.", "success");
            }
            catch (Exception ex)
            {
                LogError("Onay hatası", ex);
                ShowToast("Onay işlemi sırasında hata oluştu.", "danger");
            }
        }

        protected void Iade_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid || string.IsNullOrEmpty(txtBasvuruNo.Text)) return;

            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Hareket tablosuna ekle
                            string teslimAlan = ddlKullanici.Visible ? ddlKullanici.SelectedValue : CurrentUserName;
                            Helpers.InsertCimerMovement(conn, transaction, txtBasvuruNo.Text, CurrentUserName,
                                teslimAlan, txtAciklama.Text, 0, "İade Edildi");

                            // Ana tabloyu güncelle
                            string updateQuery = @"
                                UPDATE cimer_basvurular 
                                SET Onay_Durumu = @OnayDurumuIade, Son_Kullanici = @SonKullanici, 
                                    Guncelleme_Tarihi = @Tarih, Guncelleyen_Kullanici = @Guncelleyen
                                WHERE Basvuru_No = @BasvuruId";

                            List<SqlParameter> paramsUpdate = new List<SqlParameter>
                            {
                                CreateParameter("@OnayDurumuIade", Sabitler.HAVALE.ToString()),
                                CreateParameter("@SonKullanici", teslimAlan),
                                CreateParameter("@Tarih", DateTime.Now),
                                CreateParameter("@BasvuruId", txtBasvuruNo.Text),
                                CreateParameter("@Guncelleyen", CurrentUserName)
                            };

                            ExecuteNonQueryWithTransaction(conn, transaction, updateQuery, paramsUpdate);

                            transaction.Commit();
                            LogInfo($"CİMER başvuru iade edildi: {txtBasvuruNo.Text} - {teslimAlan}");
                            ShowToast($"CİMER başvuru iade edildi: {txtBasvuruNo.Text} - {teslimAlan}","success");
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                // Paneli gizle ve listeyi yenile
                pnlApproval.Visible = false;
                Listele();
                ShowToast("Başvuru başarıyla iade edildi.", "success");
            }
            catch (Exception ex)
            {
                LogError("İade hatası", ex);
                ShowErrorAndRedirect("İade işlemi sırasında hata oluştu.", Request.RawUrl);
            }
        }

        protected void ExceleAktar_Click(object sender, EventArgs e)
        {
            if (GridView1.Rows.Count == 0)
            {
                ShowToast("Export edilecek veri bulunamadı.","danger");
                return;
            }
            ExportGridViewToExcel(GridView1, "CimerOnaylar.xls");
            
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            // GridView Excel render için gerekli
        }

        //  Helper method: Kısa metin döndür (GridView için)
        protected string GetShortText(object text, int maxLength)
        {
            if (text == null || text == DBNull.Value)
                return "-";

            string fullText = text.ToString().Trim();

            if (string.IsNullOrEmpty(fullText))
                return "-";

            if (fullText.Length <= maxLength)
                return fullText;

            return fullText.Substring(0, maxLength) + "... <span class='text-truncated'>[devamı]</span>";
        }

        //  Helper method: Tooltip için orta uzunlukta metin döndür
        protected string GetTooltipText(object text)
        {
            if (text == null || text == DBNull.Value)
                return "Başvuru metni bulunmamaktadır.";

            string fullText = text.ToString().Trim();

            if (string.IsNullOrEmpty(fullText))
                return "Başvuru metni bulunmamaktadır.";

            // Tooltip için max 300 karakter + HTML break
            if (fullText.Length <= 300)
                return fullText.Replace("\n", "<br/>");

            return fullText.Substring(0, 300).Replace("\n", "<br/>") + "...<br/><em>Tam metin için satırı seçiniz</em>";
        }

    }
}