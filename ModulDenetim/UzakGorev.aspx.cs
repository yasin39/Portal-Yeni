using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Base;

namespace Portal.ModulDenetim
{
    public partial class UzakGorev : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(Sabitler.DENETIM_UZAK_GOREV))
                {
                    return;
                }

                AcikGörevleriYukle();
            }
        }

        private void AcikGörevleriYukle()
        {
            try
            {
                string personelAdi = CurrentUserName;

                if (string.IsNullOrEmpty(personelAdi))
                {
                    ShowToast("Oturum bilgisi bulunamadı.", "danger");
                    Response.Redirect("~/Login.aspx");
                    return;
                }

                string query = @"
                    SELECT 
                        id,
                        Tarih,
                        AracSayisi,
                        UygunsuzAracSayisi,
                        YBOlmayanAracSayisi,
                        YBKayitliOlmayanAracSayisi,
                        AtananPersonel,
                        Durum,
                        Aciklama
                    FROM denetimuzak 
                    WHERE AtananPersonel = @PersonelAdi 
                        AND Durum = @Durum
                    ORDER BY Tarih ASC";

                var parametreler = CreateParameters(
                    ("@PersonelAdi", personelAdi),
                    ("@Durum", Sabitler.ACIK)
                );

                DataTable dt = ExecuteDataTable(query, parametreler);

                UzakGorevGrid.DataSource = dt;
                UzakGorevGrid.DataBind();

                KayitSayisiniGuncelle(dt.Rows.Count);
                PanelDetay.Visible = false;
            }
            catch (Exception ex)
            {
                LogError("Açık görevler yüklenirken hata", ex);
                ShowToast("Veriler yüklenirken bir hata oluştu.", "danger");
            }
        }


        private void TumGörevleriYukle()
        {
            try
            {
                string personelAdi = CurrentUserName;

                if (string.IsNullOrEmpty(personelAdi))
                {
                    ShowToast("Oturum bilgisi bulunamadı.", "danger");
                    Response.Redirect("~/Login.aspx");
                    return;
                }

                string query = @"
                    SELECT 
                        id,
                        Tarih,
                        AracSayisi,
                        UygunsuzAracSayisi,
                        YBOlmayanAracSayisi,
                        YBKayitliOlmayanAracSayisi,
                        AtananPersonel,
                        Durum,
                        Aciklama
                    FROM denetimuzak 
                    WHERE AtananPersonel = @PersonelAdi
                    ORDER BY Tarih DESC";

                var parametreler = CreateParameters(("@PersonelAdi", personelAdi));

                DataTable dt = ExecuteDataTable(query, parametreler);

                UzakGorevGrid.DataSource = dt;
                UzakGorevGrid.DataBind();

                KayitSayisiniGuncelle(dt.Rows.Count);
                PanelDetay.Visible = false;

                ShowToast($"Toplam {dt.Rows.Count} görev listelendi.", "info");
            }
            catch (Exception ex)
            {
                LogError("Tüm görevler yüklenirken hata", ex);
                ShowToast("Veriler yüklenirken bir hata oluştu.", "danger");
            }
        }


        protected void UzakGorevGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow selectedRow = UzakGorevGrid.SelectedRow;

                txtTarih.Text = selectedRow.Cells[1].Text;
                txtAracSayisi.Text = System.Web.HttpUtility.HtmlDecode(selectedRow.Cells[2].Text);
                txtUygunsuzArac.Text = System.Web.HttpUtility.HtmlDecode(selectedRow.Cells[3].Text);
                txtYBOlmayanArac.Text = System.Web.HttpUtility.HtmlDecode(selectedRow.Cells[4].Text);
                txtYBKayitliOlmayan.Text = System.Web.HttpUtility.HtmlDecode(selectedRow.Cells[5].Text);

                if (selectedRow.Cells.Count > 8)
                {
                    txtAciklama.Text = System.Web.HttpUtility.HtmlDecode(selectedRow.Cells[8].Text);
                }

                PanelDetay.Visible = true;
            }
            catch (Exception ex)
            {
                LogError("Grid satır seçimi hatası", ex);
                ShowToast("Kayıt yüklenirken hata oluştu.", "danger");
            }
        }

        /// <summary>
        /// Açık görevler butonu
        /// </summary>
        protected void btnAcikGorevler_Click(object sender, EventArgs e)
        {
            AcikGörevleriYukle();
        }

        /// <summary>
        /// Tüm görevler butonu
        /// </summary>
        protected void btnTumGorevler_Click(object sender, EventArgs e)
        {
            TumGörevleriYukle();
        }

        /// <summary>
        /// Kaydet butonu - Görev durumunu Kapalı yap
        /// </summary>
        protected void btnKaydet_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                ShowToast("Lütfen zorunlu alanları doldurunuz.", "warning");
                return;
            }
            try
            {
                if (UzakGorevGrid.SelectedIndex == -1)
                {
                    ShowToast("Lütfen bir kayıt seçiniz.", "warning");
                    return;
                }

                int kayitId = Convert.ToInt32(UzakGorevGrid.SelectedDataKey.Value);
                string guncelleyenKullanici = CurrentUserName;

                string query = @"
                    UPDATE denetimuzak 
                    SET 
                        UygunsuzAracSayisi = @UygunsuzArac,
                        YBOlmayanAracSayisi = @YBOlmayan,
                        YBKayitliOlmayanAracSayisi = @YBKayitsiz,
                        Durum = @Durum,
                        Aciklama = @Aciklama,
                        DenetimKayitTarihi = @DenetimTarihi,
                        GuncelleyenKullanici = @GuncelleyenKullanici,
                        GuncellemeTarihi = @GuncellemeTarihi
                    WHERE id = @KayitId";

                var parametreler = CreateParameters(
                    ("@UygunsuzArac", txtUygunsuzArac.Text),
                    ("@YBOlmayan", txtYBOlmayanArac.Text),
                    ("@YBKayitsiz", txtYBKayitliOlmayan.Text),
                    ("@Durum", Sabitler.KAPALI),
                    ("@Aciklama", txtAciklama.Text),
                    ("@DenetimTarihi", DateTime.Now),
                    ("@GuncelleyenKullanici", guncelleyenKullanici),
                    ("@GuncellemeTarihi", DateTime.Now),
                    ("@KayitId", kayitId)
                );

                int etkilenenSatir = ExecuteNonQuery(query, parametreler);

                if (etkilenenSatir > 0)
                {
                    LogInfo($"Uzak görev güncellendi. ID: {kayitId}");
                    ShowToast("Görev başarıyla kapatıldı ve kaydedildi.", "success");

                    AcikGörevleriYukle();
                    PanelDetay.Visible = false;
                }
                else
                {
                    ShowToast("Kayıt güncellenirken bir sorun oluştu.", "warning");
                }
            }
            catch (Exception ex)
            {
                LogError("Kayıt güncelleme hatası", ex);
                ShowToast("Kayıt kaydedilirken hata oluştu.", "danger");
            }
        }

        /// <summary>
        /// Vazgeç butonu
        /// </summary>
        protected void btnVazgec_Click(object sender, EventArgs e)
        {
            PanelDetay.Visible = false;
            UzakGorevGrid.SelectedIndex = -1;
        }

        /// <summary>
        /// Excel'e aktar
        /// </summary>
        protected void btnExcelAktar_Click(object sender, EventArgs e)
        {
            if (UzakGorevGrid.Rows.Count == 0)
            {
                ShowToast("Export edilecek veri bulunamadı.", "warning");
                return;
            }

            try
            {              

                ExportGridViewToExcel(UzakGorevGrid, "UzakGorevler_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                LogInfo("Uzak görevler Excel'e aktarıldı.");
             
            }
            catch (Exception ex)
            {
                LogError("Excel export hatası", ex);
                ShowToast("Excel dosyası oluşturulurken hata oluştu.", "danger");
            }
        }

        /// <summary>
        /// Excel render için gerekli override
        /// </summary>
        public override void VerifyRenderingInServerForm(Control control)
        {
        }

        /// <summary>
        /// Kayıt sayısını badge'de göster
        /// </summary>
        private void KayitSayisiniGuncelle(int kayitSayisi)
        {
            if (kayitSayisi > 0)
            {
                lblKayitSayisi.Text = $"{kayitSayisi} kayıt";
                lblKayitSayisi.CssClass = "badge bg-primary";
            }
            else
            {
                lblKayitSayisi.Text = "Kayıt yok";
                lblKayitSayisi.CssClass = "badge bg-secondary";
            }
        }
    }
}