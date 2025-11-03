using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Base;

namespace Portal.ModulDenetim
{
    public partial class UzakRapor : BasePage
    {
        #region SQL Sorguları

        // DenetimSorgu.cs dosyasından buraya taşınan sorgular.
        // Sorguları sabit (const) string olarak tanımlamak, hem performanslıdır 
        // hem de kodun içinde değişmeyeceklerini garanti eder.

        /// <summary>
        /// Aktif personelleri getirir
        /// </summary>
        private const string SqlGetAktifPersoneller = @"
            SELECT 
                Adi + ' ' + Soyad as Kisi 
            FROM personel 
            WHERE Durum = 'Aktif' 
                AND CalismaDurumu != 'Geçici Görevde Pasif Çalışan' 
                AND Statu = 'Memur' 
            ORDER BY Adi ASC";

        /// <summary>
        /// Tüm uzaktan denetim kayıtlarını getirir (Durumu 'Açık' olanlar)
        /// </summary>
        private const string SqlGetTumDenetimler = @"
            SELECT 
                id,
                Tarih,
                AracSayisi,
                UygunsuzAracSayisi,
                YBOlmayanAracSayisi,
                YBKayitliOlmayanAracSayisi,
                DenetimKayitTarihi,
                AtananPersonel,
                Durum,
                Aciklama
            FROM denetimuzak 
            WHERE Durum = 'Açık' 
            ORDER BY Tarih ASC";

        /// <summary>
        /// Filtrelere göre uzaktan denetim kayıtlarının temel sorgusu
        /// </summary>
        private const string SqlGetFiltreliDenetimler = @"
            SELECT 
                id,
                Tarih,
                AracSayisi,
                UygunsuzAracSayisi,
                YBOlmayanAracSayisi,
                YBKayitliOlmayanAracSayisi,
                DenetimKayitTarihi,
                AtananPersonel,
                Durum,
                Aciklama
            FROM denetimuzak 
            WHERE 1=1"; // Filtreler bu WHERE koşuluna eklenecek

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(Sabitler.DENETIM_UZAK_RAPOR))
                {
                    return;
                }

                PersonelleriYukle();
                DenetimleriYukle();
            }
        }

        #region Veri Yükleme

        private void PersonelleriYukle()
        {
            try
            {                
                DataTable dt = ExecuteDataTable(SqlGetAktifPersoneller);

                ddlPersonel.Items.Clear();
                ddlPersonel.Items.Add(new ListItem("Hepsi", "Hepsi"));
                ddlPersonel.DataSource = dt;
                ddlPersonel.DataTextField = "Kisi";
                ddlPersonel.DataValueField = "Kisi";
                ddlPersonel.DataBind();
            }
            catch (Exception ex)
            {
                LogError("Personeller yüklenirken hata", ex);
                ShowToast("Personeller yüklenirken hata oluştu.", "danger");
            }
        }

        private void DenetimleriYukle()
        {
            try
            {                
                DataTable dt = ExecuteDataTable(SqlGetTumDenetimler);
                gvDenetimler.DataSource = dt;
                gvDenetimler.DataBind();
                KayitSayisiniGuncelle(dt.Rows.Count);
            }
            catch (Exception ex)
            {
                LogError("Denetimler yüklenirken hata", ex);
                ShowToast("Denetimler yüklenirken hata oluştu.", "danger");
            }
        }

        private void FiltreliDenetimleriYukle()
        {
            try
            {                
                string sqlSorgu = SqlGetFiltreliDenetimler;
                var parametreler = new System.Collections.Generic.List<SqlParameter>();

                if (ddlPersonel.SelectedValue != "Hepsi")
                {
                    sqlSorgu += " AND AtananPersonel = @Personel";
                    parametreler.Add(CreateParameter("@Personel", ddlPersonel.SelectedValue));
                }

                if (ddlDurum.SelectedValue != "Hepsi")
                {
                    sqlSorgu += " AND Durum = @Durum";
                    parametreler.Add(CreateParameter("@Durum", ddlDurum.SelectedValue));
                }

                if (!string.IsNullOrEmpty(txtBaslangicTarihi.Text))
                {
                    sqlSorgu += " AND CONVERT(DATE, Tarih, 23) >= @BaslangicTarihi";
                    parametreler.Add(CreateParameter("@BaslangicTarihi", txtBaslangicTarihi.Text));
                }

                if (!string.IsNullOrEmpty(txtBitisTarihi.Text))
                {
                    sqlSorgu += " AND CONVERT(DATE, Tarih, 23) <= @BitisTarihi";
                    parametreler.Add(CreateParameter("@BitisTarihi", txtBitisTarihi.Text));
                }

                sqlSorgu += " ORDER BY Tarih DESC";

                DataTable dt = ExecuteDataTable(sqlSorgu, parametreler);
                gvDenetimler.DataSource = dt;
                gvDenetimler.DataBind();

                KayitSayisiniGuncelle(dt.Rows.Count);
                lblSonucBilgisi.Text = $"Filtreleme sonucu: {dt.Rows.Count} kayıt bulundu.";
                lblSonucBilgisi.Visible = true;
            }
            catch (Exception ex)
            {
                LogError("Filtreleme sırasında hata", ex);
                ShowToast("Filtreleme sırasında hata oluştu.", "danger");
            }
        }

        #endregion

        #region Button Events

        protected void btnAra_Click(object sender, EventArgs e)
        {
            FiltreliDenetimleriYukle();
        }

        protected void btnTumunuListele_Click(object sender, EventArgs e)
        {
            //ddlPersonel.SelectedValue = "Hepsi";
            //ddlDurum.SelectedValue = "Hepsi";
            txtBaslangicTarihi.Text = string.Empty;
            txtBitisTarihi.Text = string.Empty;
            lblSonucBilgisi.Visible = false;
            DenetimleriYukle();
            ShowToast("Filtreler temizlendi ve tüm kayıtlar listelendi.", "info");
        }

        protected void btnExcelAktar_Click(object sender, EventArgs e)
        {
            if (gvDenetimler.Rows.Count == 0)
            {
                ShowToast("Export edilecek veri bulunamadı.", "warning");
                return;
            }

            try
            {
                ExportGridViewToExcel(gvDenetimler, "DenetimUzakRapor_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                LogInfo("Uzak denetim raporu Excel'e aktarıldı.");
            }
            catch (Exception ex)
            {
                LogError("Excel export hatası", ex);
                ShowToast("Excel dosyası oluşturulurken hata oluştu.", "danger");
            }
        }

        #endregion

        #region Helper Methods

        private void KayitSayisiniGuncelle(int kayitSayisi)
        {
            lblKayitSayisi.Text = kayitSayisi > 0
                ? $"{kayitSayisi} kayıt"
                : "Kayıt yok";
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            // GridView'ı Excel'e aktarırken 'Control ... must be placed inside a form tag' 
            // hatasını önlemek için bu metodun içi boş bırakılır.
        }

        #endregion
    }
}