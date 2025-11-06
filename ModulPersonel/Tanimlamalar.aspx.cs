using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Base;

namespace Portal.ModulPersonel
{
    public partial class Tanimlamalar : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(Sabitler.Personel))
                {
                    return;
                }
                TablolariDoldur();
            }
        }

        /// <summary>
        /// Page_PreRender olayı, tüm buton tıklama olaylarından (örn: btnSendikaEkle_Click)
        /// SONRA çalışır. Bu, postback sonrası hangi tabın aktif olacağına
        /// karar vermek için doğru yerdir.
        /// </summary>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            // Sadece PostBack durumlarında çalışır
            if (IsPostBack)
            {
                // Gizli alandan aktif tabın ID'sini (örn: '#sendika') oku
                string aktifTabId = hdnAktifTab.Value;

                if (!string.IsNullOrEmpty(aktifTabId))
                {
                    // Bootstrap 5 JS API'sini çağıran bir script oluştur
                    // Bu script, 'data-bs-target' özelliği gizli alandaki değere eşit olan
                    // butonu bulur ve programatik olarak o tabı 'gösterir'.
                    string script = $@"
                        var tabButton = document.querySelector('button[data-bs-target=""{aktifTabId}""]');
                        if (tabButton) {{
                            var tab = new bootstrap.Tab(tabButton);
                            tab.show();
                        }}";

                    // Oluşturulan script'i sayfa yüklendikten sonra çalışması için kaydet
                    // (MasterPage'inizde bir <asp:ScriptManager> olduğunu varsayıyoruz)
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "AktifTabScript", script, true);
                }
            }
        }

        #region Veri Yükleme

        private void TablolariDoldur()
        {
            KurumlariYukle();
            SendikalariYukle();
            UnvanlariYukle();
            BirimleriYukle();
        }

        private void KurumlariYukle()
        {
            try
            {
                string query = "SELECT id, Kurum_Adi FROM personel_kurum ORDER BY Kurum_Adi ASC";
                DataTable dt = ExecuteDataTable(query);
                gvKurum.DataSource = dt;
                gvKurum.DataBind();
            }
            catch (Exception ex)
            {
                LogError("Kurumları yüklerken hata", ex);
                ShowToast("Kurumlar yüklenirken hata oluştu.", "danger");
            }
        }

        private void SendikalariYukle()
        {
            try
            {
                string query = "SELECT id, Sendika_Adi FROM personel_sendika ORDER BY Sendika_Adi ASC";
                DataTable dt = ExecuteDataTable(query);
                gvSendika.DataSource = dt;
                gvSendika.DataBind();
            }
            catch (Exception ex)
            {
                LogError("Sendikaları yüklerken hata", ex);
                ShowToast("Sendikalar yüklenirken hata oluştu.", "danger");
            }
        }

        private void UnvanlariYukle()
        {
            try
            {
                string query = "SELECT id, Unvan FROM personel_unvan ORDER BY Unvan ASC";
                DataTable dt = ExecuteDataTable(query);
                gvUnvan.DataSource = dt;
                gvUnvan.DataBind();
            }
            catch (Exception ex)
            {
                LogError("Ünvanları yüklerken hata", ex);
                ShowToast("Ünvanlar yüklenirken hata oluştu.", "danger");
            }
        }

        private void BirimleriYukle()
        {
            try
            {
                string query = "SELECT Id, Sube_Adi FROM subeler ORDER BY Sube_Adi ASC";
                DataTable dt = ExecuteDataTable(query);
                gvBirim.DataSource = dt;
                gvBirim.DataBind();
            }
            catch (Exception ex)
            {
                LogError("Birimleri yüklerken hata", ex);
                ShowToast("Birimler yüklenirken hata oluştu.", "danger");
            }
        }

        #endregion

        #region Kurum İşlemleri

        protected void btnKurumEkle_Click(object sender, EventArgs e)
        {
            try
            {
                string kurumAdi = txtKurum.Text.Trim();

                if (string.IsNullOrEmpty(kurumAdi))
                {
                    ShowToast("Kurum adı boş olamaz.", "warning");
                    return;
                }

                string query = "INSERT INTO personel_kurum (Kurum_Adi) VALUES (@KurumAdi)";
                var parametreler = CreateParameters(("@KurumAdi", kurumAdi));

                int sonuc = ExecuteNonQuery(query, parametreler);

                if (sonuc > 0)
                {
                    ShowToast("Kurum başarıyla eklendi.", "success");
                    txtKurum.Text = string.Empty;
                    KurumlariYukle();
                    LogInfo($"Yeni kurum eklendi: {kurumAdi}");
                }
            }
            catch (Exception ex)
            {
                LogError("Kurum eklerken hata", ex);
                ShowToast("Kurum eklenirken hata oluştu.", "danger");
            }
        }

        protected void btnKurumGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvKurum.SelectedIndex < 0)
                {
                    ShowToast("Lütfen güncellenecek kaydı seçiniz.", "warning");
                    return;
                }

                int kurumId = Convert.ToInt32(gvKurum.SelectedDataKey.Value);
                string kurumAdi = txtKurum.Text.Trim();

                if (string.IsNullOrEmpty(kurumAdi))
                {
                    ShowToast("Kurum adı boş olamaz.", "warning");
                    return;
                }

                string query = "UPDATE personel_kurum SET Kurum_Adi = @KurumAdi WHERE id = @KurumId";
                var parametreler = CreateParameters(
                    ("@KurumAdi", kurumAdi),
                    ("@KurumId", kurumId)
                );

                int sonuc = ExecuteNonQuery(query, parametreler);

                if (sonuc > 0)
                {
                    ShowToast("Kurum başarıyla güncellendi.", "success");
                    KurumGuncellemeyiIptalEt();
                    KurumlariYukle();
                    LogInfo($"Kurum güncellendi: {kurumAdi} (ID: {kurumId})");
                }
            }
            catch (Exception ex)
            {
                LogError("Kurum güncellerken hata", ex);
                ShowToast("Kurum güncellenirken hata oluştu.", "danger");
            }
        }

        protected void btnKurumIptal_Click(object sender, EventArgs e)
        {
            KurumGuncellemeyiIptalEt();
        }

        protected void gvKurum_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow secilenSatir = gvKurum.SelectedRow;
                txtKurum.Text = HttpUtility.HtmlDecode(secilenSatir.Cells[1].Text);

                btnKurumEkle.Visible = false;
                btnKurumGuncelle.Visible = true;
                btnKurumIptal.Visible = true;
            }
            catch (Exception ex)
            {
                LogError("Kurum seçerken hata", ex);
                ShowToast("Kayıt seçilirken hata oluştu.", "danger");
            }
        }

        private void KurumGuncellemeyiIptalEt()
        {
            txtKurum.Text = string.Empty;
            btnKurumEkle.Visible = true;
            btnKurumGuncelle.Visible = false;
            btnKurumIptal.Visible = false;
            gvKurum.SelectedIndex = -1;
        }

        #endregion

        #region Sendika İşlemleri

        protected void btnSendikaEkle_Click(object sender, EventArgs e)
        {
            try
            {
                string sendikaAdi = txtSendika.Text.Trim();

                if (string.IsNullOrEmpty(sendikaAdi))
                {
                    ShowToast("Sendika adı boş olamaz.", "warning");
                    return;
                }

                string query = "INSERT INTO personel_sendika (Sendika_Adi) VALUES (@SendikaAdi)";
                var parametreler = CreateParameters(("@SendikaAdi", sendikaAdi));

                int sonuc = ExecuteNonQuery(query, parametreler);

                if (sonuc > 0)
                {
                    ShowToast("Sendika başarıyla eklendi.", "success");
                    txtSendika.Text = string.Empty;
                    SendikalariYukle();
                    LogInfo($"Yeni sendika eklendi: {sendikaAdi}");
                }
            }
            catch (Exception ex)
            {
                LogError("Sendika eklerken hata", ex);
                ShowToast("Sendika eklenirken hata oluştu.", "danger");
            }
        }

        protected void btnSendikaGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvSendika.SelectedIndex < 0)
                {
                    ShowToast("Lütfen güncellenecek kaydı seçiniz.", "warning");
                    return;
                }

                int sendikaId = Convert.ToInt32(gvSendika.SelectedDataKey.Value);
                string sendikaAdi = txtSendika.Text.Trim();

                if (string.IsNullOrEmpty(sendikaAdi))
                {
                    ShowToast("Sendika adı boş olamaz.", "warning");
                    return;
                }

                string query = "UPDATE personel_sendika SET Sendika_Adi = @SendikaAdi WHERE id = @SendikaId";
                var parametreler = CreateParameters(
                    ("@SendikaAdi", sendikaAdi),
                    ("@SendikaId", sendikaId)
                );

                int sonuc = ExecuteNonQuery(query, parametreler);

                if (sonuc > 0)
                {
                    ShowToast("Sendika başarıyla güncellendi.", "success");
                    SendikaGuncellemeyiIptalEt();
                    SendikalariYukle();
                    LogInfo($"Sendika güncellendi: {sendikaAdi} (ID: {sendikaId})");
                }
            }
            catch (Exception ex)
            {
                LogError("Sendika güncellerken hata", ex);
                ShowToast("Sendika güncellenirken hata oluştu.", "danger");
            }
        }

        protected void btnSendikaIptal_Click(object sender, EventArgs e)
        {
            SendikaGuncellemeyiIptalEt();
        }

        protected void gvSendika_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow secilenSatir = gvSendika.SelectedRow;
                txtSendika.Text = HttpUtility.HtmlDecode(secilenSatir.Cells[1].Text);

                btnSendikaEkle.Visible = false;
                btnSendikaGuncelle.Visible = true;
                btnSendikaIptal.Visible = true;
            }
            catch (Exception ex)
            {
                LogError("Sendika seçerken hata", ex);
                ShowToast("Kayıt seçilirken hata oluştu.", "danger");
            }
        }

        private void SendikaGuncellemeyiIptalEt()
        {
            txtSendika.Text = string.Empty;
            btnSendikaEkle.Visible = true;
            btnSendikaGuncelle.Visible = false;
            btnSendikaIptal.Visible = false;
            gvSendika.SelectedIndex = -1;
        }

        #endregion

        #region Ünvan İşlemleri

        protected void btnUnvanEkle_Click(object sender, EventArgs e)
        {
            try
            {
                string unvan = txtUnvan.Text.Trim();

                if (string.IsNullOrEmpty(unvan))
                {
                    ShowToast("Ünvan boş olamaz.", "warning");
                    return;
                }

                string query = "INSERT INTO personel_unvan (Unvan) VALUES (@Unvan)";
                var parametreler = CreateParameters(("@Unvan", unvan));

                int sonuc = ExecuteNonQuery(query, parametreler);

                if (sonuc > 0)
                {
                    ShowToast("Ünvan başarıyla eklendi.", "success");
                    txtUnvan.Text = string.Empty;
                    UnvanlariYukle();
                    LogInfo($"Yeni ünvan eklendi: {unvan}");
                }
            }
            catch (Exception ex)
            {
                LogError("Ünvan eklerken hata", ex);
                ShowToast("Ünvan eklenirken hata oluştu.", "danger");
            }
        }

        protected void btnUnvanGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvUnvan.SelectedIndex < 0)
                {
                    ShowToast("Lütfen güncellenecek kaydı seçiniz.", "warning");
                    return;
                }

                int unvanId = Convert.ToInt32(gvUnvan.SelectedDataKey.Value);
                string unvan = txtUnvan.Text.Trim();

                if (string.IsNullOrEmpty(unvan))
                {
                    ShowToast("Ünvan boş olamaz.", "warning");
                    return;
                }

                string query = "UPDATE personel_unvan SET Unvan = @Unvan WHERE id = @UnvanId";
                var parametreler = CreateParameters(
                    ("@Unvan", unvan),
                    ("@UnvanId", unvanId)
                );

                int sonuc = ExecuteNonQuery(query, parametreler);

                if (sonuc > 0)
                {
                    ShowToast("Ünvan başarıyla güncellendi.", "success");
                    UnvanGuncellemeyiIptalEt();
                    UnvanlariYukle();
                    LogInfo($"Ünvan güncellendi: {unvan} (ID: {unvanId})");
                }
            }
            catch (Exception ex)
            {
                LogError("Ünvan güncellerken hata", ex);
                ShowToast("Ünvan güncellenirken hata oluştu.", "danger");
            }
        }

        protected void btnUnvanIptal_Click(object sender, EventArgs e)
        {
            UnvanGuncellemeyiIptalEt();
        }

        protected void gvUnvan_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow secilenSatir = gvUnvan.SelectedRow;
                txtUnvan.Text = HttpUtility.HtmlDecode(secilenSatir.Cells[1].Text);

                btnUnvanEkle.Visible = false;
                btnUnvanGuncelle.Visible = true;
                btnUnvanIptal.Visible = true;
            }
            catch (Exception ex)
            {
                LogError("Ünvan seçerken hata", ex);
                ShowToast("Kayıt seçilirken hata oluştu.", "danger");
            }
        }

        private void UnvanGuncellemeyiIptalEt()
        {
            txtUnvan.Text = string.Empty;
            btnUnvanEkle.Visible = true;
            btnUnvanGuncelle.Visible = false;
            btnUnvanIptal.Visible = false;
            gvUnvan.SelectedIndex = -1;
        }

        #endregion

        #region Birim İşlemleri

        protected void btnBirimEkle_Click(object sender, EventArgs e)
        {
            try
            {
                string birimAdi = txtBirim.Text.Trim();

                if (string.IsNullOrEmpty(birimAdi))
                {
                    ShowToast("Birim adı boş olamaz.", "warning");
                    return;
                }

                string query = "INSERT INTO subeler (Sube_Adi) VALUES (@BirimAdi)";
                var parametreler = CreateParameters(("@BirimAdi", birimAdi));

                int sonuc = ExecuteNonQuery(query, parametreler);

                if (sonuc > 0)
                {
                    ShowToast("Birim başarıyla eklendi.", "success");
                    txtBirim.Text = string.Empty;
                    BirimleriYukle();
                    LogInfo($"Yeni birim eklendi: {birimAdi}");
                }
            }
            catch (Exception ex)
            {
                LogError("Birim eklerken hata", ex);
                ShowToast("Birim eklenirken hata oluştu.", "danger");
            }
        }

        protected void btnBirimGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvBirim.SelectedIndex < 0)
                {
                    ShowToast("Lütfen güncellenecek kaydı seçiniz.", "warning");
                    return;
                }

                int birimId = Convert.ToInt32(gvBirim.SelectedDataKey.Value);
                string birimAdi = txtBirim.Text.Trim();

                if (string.IsNullOrEmpty(birimAdi))
                {
                    ShowToast("Birim adı boş olamaz.", "warning");
                    return;
                }

                string query = "UPDATE subeler SET Sube_Adi = @BirimAdi WHERE Id = @BirimId";
                var parametreler = CreateParameters(
                    ("@BirimAdi", birimAdi),
                    ("@BirimId", birimId)
                );

                int sonuc = ExecuteNonQuery(query, parametreler);

                if (sonuc > 0)
                {
                    ShowToast("Birim başarıyla güncellendi.", "success");
                    BirimGuncellemeyiIptalEt();
                    BirimleriYukle();
                    LogInfo($"Birim güncellendi: {birimAdi} (ID: {birimId})");
                }
            }
            catch (Exception ex)
            {
                LogError("Birim güncellerken hata", ex);
                ShowToast("Birim güncellenirken hata oluştu.", "danger");
            }
        }

        protected void btnBirimIptal_Click(object sender, EventArgs e)
        {
            BirimGuncellemeyiIptalEt();
        }

        protected void gvBirim_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow secilenSatir = gvBirim.SelectedRow;
                txtBirim.Text = HttpUtility.HtmlDecode(secilenSatir.Cells[1].Text);

                btnBirimEkle.Visible = false;
                btnBirimGuncelle.Visible = true;
                btnBirimIptal.Visible = true;
            }
            catch (Exception ex)
            {
                LogError("Birim seçerken hata", ex);
                ShowToast("Kayıt seçilirken hata oluştu.", "danger");
            }
        }

        private void BirimGuncellemeyiIptalEt()
        {
            txtBirim.Text = string.Empty;
            btnBirimEkle.Visible = true;
            btnBirimGuncelle.Visible = false;
            btnBirimIptal.Visible = false;
            gvBirim.SelectedIndex = -1;
        }

        #endregion
    }
}