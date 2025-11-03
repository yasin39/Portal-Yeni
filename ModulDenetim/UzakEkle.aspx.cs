using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Portal.Base;

namespace Portal.ModulDenetim
{
    public partial class UzakEkle : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(Sabitler.DENETIM_UZAK_GIRIS))
                {
                    return;
                }

                PersonelleriYukle();
            }
        }

        #region Veri Yükleme

        private void PersonelleriYukle()
        {
            try
            {
                Helpers.LoadActivePersonnel(ddlPersonel);
            }
            catch (Exception ex)
            {
                LogError("Personeller yüklenirken hata", ex);
                ShowToast("Personeller yüklenirken hata oluştu.", "danger");
            }
        }

        #endregion

        #region Button Events

        protected void btnKaydet_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                string kullaniciAdi = CurrentUserName;
                if (string.IsNullOrEmpty(kullaniciAdi))
                {
                    ShowToast("Oturum bilgisi bulunamadı. Lütfen tekrar giriş yapınız.", "danger");
                    return;
                }

                string query = @"
                    INSERT INTO denetimuzak 
                    (Tarih, AracSayisi, AtananPersonel, Durum, Aciklama, KayitTarihi, KayitKullanici) 
                    VALUES 
                    (@Tarih, @AracSayisi, @AtananPersonel, @Durum, @Aciklama, @KayitTarihi, @KayitKullanici)";

                var parameters = CreateParameters(
                    ("@Tarih", txtTarih.Text),
                    ("@AracSayisi", txtAracSayisi.Text),
                    ("@AtananPersonel", ddlPersonel.SelectedValue),
                    ("@Durum", ddlIslemDurum.SelectedValue),
                    ("@Aciklama", txtAciklama.Text),
                    ("@KayitTarihi", DateTime.Now),
                    ("@KayitKullanici", kullaniciAdi)
                );

                int sonuc = ExecuteNonQuery(query, parameters);

                if (sonuc > 0)
                {
                    LogInfo($"Uzaktan denetim kaydı eklendi: {ddlPersonel.SelectedValue} - {txtTarih.Text}");
                    ShowToast("Kayıt başarıyla eklendi.", "success");
                    FormTemizle();
                }
                else
                {
                    ShowToast("Kayıt eklenirken bir hata oluştu.", "danger");
                }
            }
            catch (Exception ex)
            {
                LogError("Kayıt eklenirken hata", ex);
                ShowToast("Kayıt eklenirken hata oluştu.", "danger");
            }
        }

        protected void btnBul_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKayitBul.Text))
            {
                ShowToast("Lütfen kayıt ID giriniz.", "warning");
                return;
            }

            try
            {
                lblHata.Text = string.Empty;

                string query = @"
                    SELECT * FROM denetimuzak 
                    WHERE id = @KayitId";

                var parameters = CreateParameters(("@KayitId", txtKayitBul.Text));

                DataTable dt = ExecuteDataTable(query, parameters);

                if (dt.Rows.Count == 0)
                {
                    lblHata.Text = "Aranan kayıt bulunamadı.";
                    ShowToast("Kayıt bulunamadı.", "warning");
                    return;
                }

                DataRow row = dt.Rows[0];

                txtTarih.Text = Convert.ToDateTime(row["Tarih"]).ToString("yyyy-MM-dd");
                txtAracSayisi.Text = row["AracSayisi"].ToString();

                SetSafeDropDownValue(ddlPersonel, row["AtananPersonel"].ToString());
                SetSafeDropDownValue(ddlIslemDurum, row["Durum"].ToString());

                txtAciklama.Text = row["Aciklama"].ToString();

                divUygunsuzArac.Visible = true;
                divCezaliArac.Visible = true;
                divYBKayitliOlmayan.Visible = true;

                txtUygunsuzArac.Text = row["UygunsuzAracSayisi"].ToString();
                txtCezaliArac.Text = row["YBOlmayanAracSayisi"].ToString();
                txtYBKayitliOlmayan.Text = row["YBKayitliOlmayanAracSayisi"].ToString();

                txtKayitBul.ReadOnly = true;
                btnVazgec.Visible = true;
                btnKaydet.Visible = false;
                btnGuncelle.Visible = true;

                ShowToast("Kayıt bulundu.", "info");
            }
            catch (Exception ex)
            {
                LogError("Kayıt arama hatası", ex);
                ShowToast("Kayıt aranırken hata oluştu.", "danger");
            }
        }

        protected void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                string kullaniciAdi = CurrentUserName;
                if (string.IsNullOrEmpty(kullaniciAdi))
                {
                    ShowToast("Oturum bilgisi bulunamadı. Lütfen tekrar giriş yapınız.", "danger");
                    return;
                }

                string query = @"
                    UPDATE denetimuzak 
                    SET Tarih = @Tarih, 
                        AracSayisi = @AracSayisi, 
                        AtananPersonel = @AtananPersonel, 
                        Durum = @Durum, 
                        Aciklama = @Aciklama, 
                        GuncellemeTarihi = @GuncellemeTarihi, 
                        GuncelleyenKullanici = @GuncelleyenKullanici 
                    WHERE id = @KayitId";

                var parameters = CreateParameters(
                    ("@Tarih", txtTarih.Text),
                    ("@AracSayisi", txtAracSayisi.Text),
                    ("@AtananPersonel", ddlPersonel.SelectedValue),
                    ("@Durum", ddlIslemDurum.SelectedValue),
                    ("@Aciklama", txtAciklama.Text),
                    ("@GuncellemeTarihi", DateTime.Now),
                    ("@GuncelleyenKullanici", kullaniciAdi),
                    ("@KayitId", txtKayitBul.Text)
                );

                int sonuc = ExecuteNonQuery(query, parameters);

                if (sonuc > 0)
                {
                    LogInfo($"Uzaktan denetim kaydı güncellendi: ID={txtKayitBul.Text}");
                    ShowToast("Kayıt başarıyla güncellendi.", "success");
                    System.Threading.Thread.Sleep(1000);
                    Response.Redirect("UzakEkle.aspx", false);
                }
                else
                {
                    ShowToast("Kayıt güncellenirken bir hata oluştu.", "danger");
                }
            }
            catch (Exception ex)
            {
                LogError("Kayıt güncellenirken hata", ex);
                ShowToast("Kayıt güncellenirken hata oluştu.", "danger");
            }
        }

        protected void btnVazgec_Click(object sender, EventArgs e)
        {
            Response.Redirect("UzakEkle.aspx", false);
        }

        #endregion

        #region Helper Methods

        private void FormTemizle()
        {
            ClearFormControls(txtTarih, txtAracSayisi, txtAciklama, ddlPersonel, ddlIslemDurum);
        }

        #endregion
    }
}