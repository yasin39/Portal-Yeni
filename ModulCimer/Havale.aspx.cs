using Portal.Base;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Portal.ModulCimer
{
    public partial class Havale : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckPermission(Sabitler.CIMER_PERSONEL))
                return;

            if (!IsPostBack)
            {
                // ==> Session'dan toast mesajı varsa göster
                if (Session["ToastMessage"] != null && Session["ToastType"] != null)
                {
                    ShowToast(Session["ToastMessage"].ToString(), Session["ToastType"].ToString());
                    Session.Remove("ToastMessage");
                    Session.Remove("ToastType");
                }
                Listele();
            }
        }

        private void Listele()
        {
            string query = @"
                SELECT * FROM cimer_basvurular 
                WHERE Son_Kullanici = @Kullanici AND Onay_Durumu = '0' 
                ORDER BY id DESC";

            var parameters = CreateParameters(
                ("@Kullanici", CurrentUserName)
            );

            DataTable dt = ExecuteDataTable(query, parameters);
            GridView1.DataSource = dt;
            GridView1.DataBind();

            // Kullanıcı dropdown'larını yükle
            YükleKullaniciDropdown();
            YükleFirmaDropdown();
        }

        private void YükleKullaniciDropdown()
        {
            string personelTipi = Session["Ptipi"].ToString();
            string whereClause = personelTipi == "0" ? "AND Personel_Tipi = '1'" : "";

            string query = $@"
                SELECT Adi_Soyadi FROM kullanici
                WHERE Durum = 'Aktif' {whereClause}
                ORDER BY Adi_Soyadi ASC";

            PopulateDropDownList(ddlKullanici, query, "Adi_Soyadi", "Adi_Soyadi", true);
            PopulateDropDownList(ddlOnaylayici, query, "Adi_Soyadi", "Adi_Soyadi", true);
        }

        private void YükleFirmaDropdown()
        {
            string query = "SELECT Firma_Unvan FROM cimer_firmalar ORDER BY Firma_Unvan ASC";
            DataTable dt = ExecuteDataTable(query);
            ddlFirmalar.DataSource = dt;
            ddlFirmalar.DataTextField = "Firma_Unvan";
            ddlFirmalar.DataValueField = "Firma_Unvan";
            ddlFirmalar.DataBind();
            ddlFirmalar.Items.Insert(0, new ListItem("", ""));
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedId = Convert.ToInt32(GridView1.SelectedDataKey.Value);
            string basvuruNo = GridView1.SelectedRow.Cells[2].Text; // Basvuru_No sütunu

            txtBasvuruNo.Text = basvuruNo;
            txtBasvuruNoCevap.Text = basvuruNo;

            // Mevcut verileri yükle cevap paneli için
            string query = "SELECT * FROM cimer_basvurular WHERE id = @Id";
            var parameters = CreateParameters(("@Id", selectedId));
            DataTable dt = ExecuteDataTable(query, parameters);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                txtYapilanIslem.Text = row["Yapilan_İslem"]?.ToString() ?? "";
                txtSonIslem.Text = row["Son_Yapilan_islem"]?.ToString() ?? "";
                SetSafeDropDownValue(ddlFirmalar, row["Sikayet_Edilen_Firma"]?.ToString() ?? "");
                SetSafeDropDownValue(ddlDurum, row["Sonuc"]?.ToString() ?? "");
                SetSafeDropDownValue(ddlBekleme, row["Bekleme_Durumu"]?.ToString() ?? "");
            }

            pnlHavale.Visible = true;
            pnlCevapla.Visible = false;
            pnlGecmis.Visible = false;
        }

        protected void btnHavaleEt_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (string.IsNullOrWhiteSpace(ddlKullanici.SelectedValue) || ddlKullanici.SelectedValue == "")
            {
                ShowToast("Lütfen havale edilecek personeli seçiniz.", "warning");
                return;
            }

            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Hareket ekle
                            Helpers.InsertCimerMovement(conn, transaction, txtBasvuruNo.Text, CurrentUserName,
                                ddlKullanici.SelectedValue, txtAciklama.Text, 0, "Havale Edildi");

                            // Başvuruyu güncelle
                            string updateQuery = @"
                                UPDATE cimer_basvurular 
                                SET Onay_Durumu = '0', Son_Kullanici = @YeniKullanici, 
                                    Guncelleyen_Kullanici = @Guncelleyen, Guncelleme_Tarihi = @Tarih 
                                WHERE Basvuru_No = @BasvuruNo";

                            var updateParams = CreateParameters(
                                ("@YeniKullanici", ddlKullanici.SelectedValue),
                                ("@Guncelleyen", CurrentUserName),
                                ("@Tarih", DateTime.Now),
                                ("@BasvuruNo", txtBasvuruNo.Text)
                            );

                            ExecuteNonQueryWithTransaction(conn, transaction, updateQuery, updateParams);

                            transaction.Commit();
                            LogInfo($"Başvuru {txtBasvuruNo.Text} {ddlKullanici.SelectedValue} kullanıcısına havale edildi.");
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                // ==> Session'a toast mesajı kaydet ve redirect yap
                Session["ToastMessage"] = "Başvuru başarıyla havale edildi.";
                Session["ToastType"] = "success";
                Response.Redirect("Havale.aspx");
            }
            catch (Exception ex)
            {
                LogError("Havale işlemi hatası", ex);
                ShowErrorAndRedirect("Havale işlemi sırasında hata oluştu.", "Havale.aspx");
            }
        }

        protected void btnCevapYaz_Click(object sender, EventArgs e)
        {
            pnlHavale.Visible = false;
            pnlCevapla.Visible = true;
            pnlGecmis.Visible = false;
        }

        protected void btnKaydetCevap_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Hareket ekle
                            Helpers.InsertCimerMovement(conn, transaction, txtBasvuruNoCevap.Text, CurrentUserName,
                                ddlOnaylayici.SelectedValue, "", 1, "Cevap Verildi. Onaya Çıkarıldı.");

                            // Başvuruyu güncelle
                            string updateQuery = @"
                                UPDATE cimer_basvurular 
                                SET Bekleme_Durumu = @Bekleme, Son_Yapilan_islem = @SonIslem, 
                                    Yapilan_İslem = @YapilanIslem, Sonuc = @Sonuc, 
                                    Sikayet_Edilen_Firma = @Firma, Onay_Durumu = '1', 
                                    Son_Kullanici = @YeniKullanici, Guncelleyen_Kullanici = @Guncelleyen, 
                                    Guncelleme_Tarihi = @Tarih 
                                WHERE Basvuru_No = @BasvuruNo";

                            var updateParams = CreateParameters(
                                ("@Bekleme", ddlBekleme.SelectedValue),
                                ("@SonIslem", txtSonIslem.Text),
                                ("@YapilanIslem", txtYapilanIslem.Text),
                                ("@Sonuc", ddlDurum.SelectedValue),
                                ("@Firma", ddlFirmalar.SelectedValue),
                                ("@YeniKullanici", ddlOnaylayici.SelectedValue),
                                ("@Guncelleyen", CurrentUserName),
                                ("@Tarih", DateTime.Now),
                                ("@BasvuruNo", txtBasvuruNoCevap.Text)
                            );

                            ExecuteNonQueryWithTransaction(conn, transaction, updateQuery, updateParams);

                            transaction.Commit();
                            LogInfo($"Başvuru {txtBasvuruNoCevap.Text} cevaplandı ve onaya gönderildi.");
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                Session["ToastMessage"] = "Başvuruya başarıyla cevap verildi.";
                Session["ToastType"] = "success";
                Response.Redirect("Havale.aspx");
            }
            catch (Exception ex)
            {
                LogError("Cevap kaydetme hatası", ex);
                ShowErrorAndRedirect("Cevap kaydetme sırasında hata oluştu.", "Havale.aspx");
            }
        }

        protected void btnIade_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Hareket ekle
                            Helpers.InsertCimerMovement(conn, transaction, txtBasvuruNo.Text, CurrentUserName,
                                CurrentUserName, txtAciklama.Text, 3, "Başvuru İade/Sevk Edildi.");

                            // Başvuruyu güncelle
                            string updateQuery = @"
                                UPDATE cimer_basvurular 
                                SET Sonuc = 'İade Edildi', Onay_Durumu = '3' 
                                WHERE Basvuru_No = @BasvuruNo";

                            var updateParams = CreateParameters(("@BasvuruNo", txtBasvuruNo.Text));

                            ExecuteNonQueryWithTransaction(conn, transaction, updateQuery, updateParams);

                            transaction.Commit();
                            LogInfo($"Başvuru {txtBasvuruNo.Text} iade edildi.");
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                Session["ToastMessage"] = "Başvuru başarıyla iade edildi.";
                Session["ToastType"] = "success";
                Response.Redirect("Havale.aspx");
            }
            catch (Exception ex)
            {
                LogError("İade işlemi hatası", ex);
                ShowErrorAndRedirect("İade işlemi sırasında hata oluştu.", "Havale.aspx");
            }
        }

        protected void btnGecmis_Click(object sender, EventArgs e)
        {
            pnlHavale.Visible = false;
            pnlCevapla.Visible = false;
            pnlGecmis.Visible = true;

            string query = @"
                SELECT Sevk_Eden, Teslim_Alan, Tarih, Aciklama, islem_Aciklama 
                FROM cimer_basvuru_hareketleri 
                WHERE Basvuru_id = @BasvuruNo 
                ORDER BY id DESC";

            var parameters = CreateParameters(("@BasvuruNo", txtBasvuruNo.Text));
            DataTable dt = ExecuteDataTable(query, parameters);
            GridViewGecmis.DataSource = dt;
            GridViewGecmis.DataBind();
        }

        protected void btnGecmisKapat_Click(object sender, EventArgs e)
        {
            pnlGecmis.Visible = false;
            pnlHavale.Visible = true;
        }

        protected void btnTum_Click(object sender, EventArgs e)
        {
            // Tüm başvuruları listele (koşulsuz, ama mevcut kullanıcıya ait)
            Listele(); // Aynı query, değişiklik yok
        }

    }
}