using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal;
using Portal.Base;

namespace ModulYonetici
{
    public partial class KullaniciIslem : BasePage
    {
        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(900))
                    return;

                LoadKullanicilar();
            }
        }

        #endregion

        #region Data Loading

        private void LoadKullanicilar()
        {
            try
            {
                string query = @"SELECT Sicil_No, Adi_Soyadi, Kullanici_Turu, Personel_Tipi, 
                                       Birim, Mail_Adresi, Durum 
                                FROM kullanici 
                                ORDER BY Adi_Soyadi ASC";

                DataTable dt = ExecuteDataTable(query);

                if (dt.Rows.Count > 0)
                {
                    rptKullanicilar.DataSource = dt;
                    rptKullanicilar.DataBind();
                    lblMesaj.Visible = false;
                }
                else
                {
                    rptKullanicilar.DataSource = null;
                    rptKullanicilar.DataBind();
                    lblMesaj.Text = "Kayıtlı kullanıcı bulunamadı.";
                    lblMesaj.Visible = true;
                }
            }
            catch (Exception ex)
            {
                LogError("Kullanıcılar yüklenirken hata", ex);
                ShowToast("Kullanıcılar yüklenirken hata oluştu.", "danger");
            }
        }

        #endregion

        #region Button Events

        protected void btnKaydet_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                string sicilNo = txtSicilNo.Text.Trim();

                if (KullaniciVarMi(sicilNo))
                {
                    ShowToast("Bu sicil numarasına sahip kullanıcı zaten mevcut!", "warning");
                    return;
                }

                string hashedParola = Helpers.HashPassword(txtParola.Text);

                string query = @"INSERT INTO kullanici 
                                (Sicil_No, Adi_Soyadi, Kullanici_Turu, Parola, Durum, 
                                 Birim, Personel_Tipi, Mail_Adresi) 
                                VALUES 
                                (@SicilNo, @AdiSoyadi, @KullaniciTuru, @Parola, @Durum, 
                                 @Birim, @PersonelTipi, @MailAdresi)";

                var parameters = CreateParameters(
                    ("@SicilNo", sicilNo),
                    ("@AdiSoyadi", txtAdiSoyadi.Text.Trim()),
                    ("@KullaniciTuru", ddlKullaniciTuru.SelectedValue),
                    ("@Parola", hashedParola),
                    ("@Durum", ddlDurum.SelectedValue),
                    ("@Birim", txtBirim.Text.Trim()),
                    ("@PersonelTipi", ddlPersonelTipi.SelectedValue),
                    ("@MailAdresi", txtMailAdresi.Text.Trim())
                );

                ExecuteNonQuery(query, parameters);

                ShowToast($"Kullanıcı başarıyla kaydedildi: {txtAdiSoyadi.Text}", "success");
                LogInfo($"Yeni kullanıcı eklendi: {sicilNo} - {txtAdiSoyadi.Text}");

                ClearForm();
                LoadKullanicilar();
            }
            catch (Exception ex)
            {
                LogError("Kullanıcı kaydedilirken hata", ex);
                ShowToast("Kullanıcı kaydedilirken hata oluştu.", "danger");
            }
        }

        protected void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                string sicilNo = txtSicilNo.Text.Trim();
                string query;

                if (string.IsNullOrEmpty(txtParola.Text))
                {
                    query = @"UPDATE kullanici SET 
                             Adi_Soyadi = @AdiSoyadi,
                             Kullanici_Turu = @KullaniciTuru,
                             Durum = @Durum,
                             Birim = @Birim,
                             Personel_Tipi = @PersonelTipi,
                             Mail_Adresi = @MailAdresi
                             WHERE Sicil_No = @SicilNo";

                    var parameters = CreateParameters(
                        ("@SicilNo", sicilNo),
                        ("@AdiSoyadi", txtAdiSoyadi.Text.Trim()),
                        ("@KullaniciTuru", ddlKullaniciTuru.SelectedValue),
                        ("@Durum", ddlDurum.SelectedValue),
                        ("@Birim", txtBirim.Text.Trim()),
                        ("@PersonelTipi", ddlPersonelTipi.SelectedValue),
                        ("@MailAdresi", txtMailAdresi.Text.Trim())
                    );

                    ExecuteNonQuery(query, parameters);
                }
                else
                {
                    string hashedParola = Helpers.HashPassword(txtParola.Text);

                    query = @"UPDATE kullanici SET 
                             Adi_Soyadi = @AdiSoyadi,
                             Kullanici_Turu = @KullaniciTuru,
                             Parola = @Parola,
                             Durum = @Durum,
                             Birim = @Birim,
                             Personel_Tipi = @PersonelTipi,
                             Mail_Adresi = @MailAdresi
                             WHERE Sicil_No = @SicilNo";

                    var parameters = CreateParameters(
                        ("@SicilNo", sicilNo),
                        ("@AdiSoyadi", txtAdiSoyadi.Text.Trim()),
                        ("@KullaniciTuru", ddlKullaniciTuru.SelectedValue),
                        ("@Parola", hashedParola),
                        ("@Durum", ddlDurum.SelectedValue),
                        ("@Birim", txtBirim.Text.Trim()),
                        ("@PersonelTipi", ddlPersonelTipi.SelectedValue),
                        ("@MailAdresi", txtMailAdresi.Text.Trim())
                    );

                    ExecuteNonQuery(query, parameters);
                }

                ShowToast($"Kullanıcı başarıyla güncellendi: {txtAdiSoyadi.Text}", "success");
                LogInfo($"Kullanıcı güncellendi: {sicilNo}");

                ClearForm();
                LoadKullanicilar();
            }
            catch (Exception ex)
            {
                LogError("Kullanıcı güncellenirken hata", ex);
                ShowToast("Kullanıcı güncellenirken hata oluştu.", "danger");
            }
        }

        protected void btnVazgec_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        protected void btnYenile_Click(object sender, EventArgs e)
        {
            LoadKullanicilar();
            ShowToast("Liste yenilendi.", "info");
        }


        //protected void btnExcel_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string query = @"SELECT Sicil_No AS [Sicil No], 
        //                               Adi_Soyadi AS [Adı Soyadı], 
        //                               Kullanici_Turu AS [Kullanıcı Türü], 
        //                               Personel_Tipi AS [Personel Tipi],
        //                               Birim,
        //                               Mail_Adresi AS [Mail Adresi],
        //                               Durum 
        //                        FROM kullanici 
        //                        ORDER BY Adi_Soyadi ASC";

        //        DataTable dt = ExecuteDataTable(query);

        //        if (dt.Rows.Count == 0)
        //        {
        //            ShowToast("Export edilecek veri bulunamadı.", "warning");
        //            return;
        //        }

        //        ExportToExcel(dt, "Kullanici_Listesi", "Kullanıcı Listesi");
        //        LogInfo("Kullanıcı listesi Excel'e aktarıldı");
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError("Excel export hatası", ex);
        //        ShowToast("Excel dosyası oluşturulurken hata oluştu.", "danger");
        //    }
        //}

        #endregion

        #region Repeater Events

        protected void rptKullanicilar_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string sicilNo = e.CommandArgument.ToString();

            if (e.CommandName == "Duzenle")
            {
                LoadKullaniciForEdit(sicilNo);
            }
            else if (e.CommandName == "SifreSifirla")
            {
                ResetPassword(sicilNo);
            }
        }

        #endregion

        #region Helper Methods

        private bool KullaniciVarMi(string sicilNo)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM kullanici WHERE Sicil_No = @SicilNo";
                var parameters = CreateParameters(("@SicilNo", sicilNo));

                DataTable dt = ExecuteDataTable(query, parameters);
                int count = Convert.ToInt32(dt.Rows[0][0]);

                return count > 0;
            }
            catch (Exception ex)
            {
                LogError("Kullanıcı varlık kontrolü hatası", ex);
                return false;
            }
        }

        private void LoadKullaniciForEdit(string sicilNo)
        {
            try
            {
                string query = @"SELECT * FROM kullanici WHERE Sicil_No = @SicilNo";
                var parameters = CreateParameters(("@SicilNo", sicilNo));

                DataTable dt = ExecuteDataTable(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    txtSicilNo.Text = row["Sicil_No"].ToString();
                    txtSicilNo.Enabled = false;

                    txtAdiSoyadi.Text = row["Adi_Soyadi"].ToString();
                    txtMailAdresi.Text = row["Mail_Adresi"].ToString();
                    txtBirim.Text = row["Birim"].ToString();

                    ddlKullaniciTuru.SelectedValue = row["Kullanici_Turu"].ToString();
                    ddlPersonelTipi.SelectedValue = row["Personel_Tipi"].ToString();
                    ddlDurum.SelectedValue = row["Durum"].ToString();

                    txtParola.Text = string.Empty;
                    rfvParola.Enabled = false;

                    SetFormModeUpdate(btnKaydet, btnGuncelle, null, btnVazgec);

                    ShowToast($"Düzenleme modu: {row["Adi_Soyadi"]}", "info");
                }
            }
            catch (Exception ex)
            {
                LogError("Kullanıcı yüklenirken hata", ex);
                ShowToast("Kullanıcı bilgileri yüklenirken hata oluştu.", "danger");
            }
        }

        private void ResetPassword(string sicilNo)
        {
            try
            {
                string yeniParola = "Ankara2025!";
                string hashedParola = Helpers.HashPassword(yeniParola);

                string query = @"UPDATE kullanici SET 
                                Parola = @Parola,
                                SifreDegistirmeZorla = 1
                                WHERE Sicil_No = @SicilNo";

                var parameters = CreateParameters(
                    ("@Parola", hashedParola),
                    ("@SicilNo", sicilNo)
                );

                ExecuteNonQuery(query, parameters);

                string kullaniciAdi = GetKullaniciAdi(sicilNo);
                ShowToast($"Kullanıcı şifresi sıfırlandı: {kullaniciAdi} - Yeni Şifre: {yeniParola}", "success");
                LogInfo($"Şifre sıfırlandı: {sicilNo}");
            }
            catch (Exception ex)
            {
                LogError("Şifre sıfırlama hatası", ex);
                ShowToast("Şifre sıfırlanırken hata oluştu.", "danger");
            }
        }

        private string GetKullaniciAdi(string sicilNo)
        {
            try
            {
                string query = "SELECT Adi_Soyadi FROM kullanici WHERE Sicil_No = @SicilNo";
                var parameters = CreateParameters(("@SicilNo", sicilNo));

                DataTable dt = ExecuteDataTable(query, parameters);

                if (dt.Rows.Count > 0)
                    return dt.Rows[0]["Adi_Soyadi"].ToString();

                return sicilNo;
            }
            catch
            {
                return sicilNo;
            }
        }

        private void ClearForm()
        {
            txtSicilNo.Text = string.Empty;
            txtSicilNo.Enabled = true;

            txtAdiSoyadi.Text = string.Empty;
            txtParola.Text = string.Empty;
            txtMailAdresi.Text = string.Empty;
            txtBirim.Text = string.Empty;

            ddlKullaniciTuru.SelectedIndex = 0;
            ddlPersonelTipi.SelectedIndex = 0;
            ddlDurum.SelectedValue = "Aktif";

            rfvParola.Enabled = true;

            SetFormModeInsert(btnKaydet, btnGuncelle, null, btnVazgec);
        }

        #endregion
    }
}