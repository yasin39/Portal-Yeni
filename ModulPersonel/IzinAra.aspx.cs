using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Base;

namespace Portal.ModulPersonel
{
    public partial class IzinAra : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(Sabitler.Personel))
                {
                    return;
                }

                BugunIzindekilerListele();
                IstatistikleriGuncelle();
            }
        }

        #region İstatistikler

        private void IstatistikleriGuncelle()
        {
            try
            {
                string bugun = DateTime.Now.Date.ToString("yyyy-MM-dd");

                string query = @"
            SELECT 
                COUNT(*) as ToplamIzinli,
                SUM(CASE WHEN izin_turu = N'Yıllık İzin' THEN 1 ELSE 0 END) as YillikIzin,
                SUM(CASE WHEN izin_turu = N'Rapor' THEN 1 ELSE 0 END) as Rapor,
                SUM(CASE WHEN izin_turu IN (N'Saatlik izin', N'Mazeret İzni') THEN 1 ELSE 0 END) as DigerIzin
            FROM personel_izin 
            WHERE izne_Baslama_Tarihi <= @Bugun 
            AND izin_Bitis_Tarihi >= @Bugun";

                var parametreler = CreateParameters(("@Bugun", bugun));
                DataTable dt = ExecuteDataTable(query, parametreler);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    lblToplamIzinli.Text = row["ToplamIzinli"].ToString();
                    lblYillikIzin.Text = row["YillikIzin"].ToString();
                    lblRaporlu.Text = row["Rapor"].ToString();
                    lblDigerIzin.Text = row["DigerIzin"].ToString();
                }
            }
            catch (Exception ex)
            {
                LogError("İstatistik güncelleme hatası", ex);
                ShowToast("İstatistikler güncellenirken hata oluştu.", "danger");
            }
        }

        #endregion

        #region Bugün İzinde Olanlar

        private void BugunIzindekilerListele()
        {
            try
            {
                string bugun = DateTime.Now.Date.ToString("yyyy-MM-dd");

                string query = @"
                    SELECT 
                        pi.id,
                        pi.Sicil_No,
                        pi.Adi_Soyadi,
                        ISNULL(p.GorevYaptigiBirim, 'Belirtilmemiş') as GorevYaptigiBirim,
                        pi.Statu,
                        pi.izin_turu,
                        pi.izin_Suresi,
                        pi.izne_Baslama_Tarihi,
                        pi.izin_Bitis_Tarihi,
                        pi.Goreve_Baslama_Tarihi,
                        pi.Aciklama,
                        pi.Kayit_Tarihi,
                        pi.Kayit_Kullanici
                    FROM personel_izin pi
                    LEFT JOIN personel p ON p.SicilNo = pi.Sicil_No
                    WHERE pi.izne_Baslama_Tarihi <= @Bugun 
                    AND pi.izin_Bitis_Tarihi >= @Bugun
                    ORDER BY p.GorevYaptigiBirim ASC, pi.Adi_Soyadi ASC";

                var parametreler = CreateParameters(("@Bugun", bugun));
                DataTable dt = ExecuteDataTable(query, parametreler);

                BugunIzinlilerGrid.DataSource = dt;
                BugunIzinlilerGrid.DataBind();

                lblBugunSayisi.Text = dt.Rows.Count.ToString();

                if (dt.Rows.Count > 0 && BugunIzinlilerGrid.FooterRow != null)
                {
                    BugunIzinlilerGrid.FooterRow.Cells[0].Text = "TOPLAM";
                    BugunIzinlilerGrid.FooterRow.Cells[0].ColumnSpan = 2;
                    BugunIzinlilerGrid.FooterRow.Cells[1].Visible = false;
                    BugunIzinlilerGrid.FooterRow.Cells[2].Text = $"{dt.Rows.Count} personel bugün izinlidir.";
                    BugunIzinlilerGrid.FooterRow.Cells[2].ColumnSpan = 6;
                    BugunIzinlilerGrid.FooterRow.Cells[3].Visible = false;
                    BugunIzinlilerGrid.FooterRow.Cells[4].Visible = false;
                    BugunIzinlilerGrid.FooterRow.Cells[5].Visible = false;
                    BugunIzinlilerGrid.FooterRow.Cells[6].Visible = false;
                    BugunIzinlilerGrid.FooterRow.Cells[7].Visible = false;
                    BugunIzinlilerGrid.FooterRow.Font.Bold = true;
                }

                LogInfo($"Bugün izinde olan {dt.Rows.Count} personel listelendi.");
            }
            catch (Exception ex)
            {
                LogError("Bugün izinde olanları listeleme hatası", ex);
                ShowToast("İzinli personeller listelenirken hata oluştu.", "danger");
            }
        }

        #endregion

        #region Personel Arama

        protected void btnSicilAra_Click(object sender, EventArgs e)
        {
            PersonelBilgisiGetir();
        }

        protected void txtSicilNo_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtSicilNo.Text))
            {
                PersonelBilgisiGetir();
            }
        }

        private void PersonelBilgisiGetir()
        {
            try
            {
                string sicilNo = txtSicilNo.Text.Trim();

                if (string.IsNullOrEmpty(sicilNo))
                {
                    ShowToast("Lütfen sicil numarası giriniz.", "warning");
                    return;
                }

                string query = @"
                    SELECT 
                        SicilNo, 
                        TcKimlikNo, 
                        Adi, 
                        Soyad, 
                        Statu, 
                        Resim 
                    FROM personel 
                    WHERE SicilNo = @SicilNo";

                var parametreler = CreateParameters(("@SicilNo", sicilNo));
                DataTable dt = ExecuteDataTable(query, parametreler);

                if (dt.Rows.Count == 0)
                {
                    pnlPersonelBilgi.Visible = false;
                    ShowToast("Personel bulunamadı. Lütfen sicil numarasını kontrol ediniz.", "warning");
                    return;
                }

                DataRow row = dt.Rows[0];
                lblSicilNo.Text = row[Sabitler.SicilNo].ToString();
                lblAdSoyad.Text = $"{row[Sabitler.Adi]} {row[Sabitler.Soyad]}";
                lblStatu.Text = row[Sabitler.Statu].ToString();
                imgPersonel.ImageUrl = row["Resim"].ToString();

                pnlPersonelBilgi.Visible = true;

                PersonelToplamIzinleriniGetir(sicilNo);

                LogInfo($"Personel bilgisi getirildi: {lblAdSoyad.Text} ({sicilNo})");
            }
            catch (Exception ex)
            {
                LogError("Personel bilgisi getirme hatası", ex);
                ShowToast("Personel bilgisi alınırken hata oluştu.", "danger");
            }
        }

        private void PersonelToplamIzinleriniGetir(string sicilNo)
        {
            try
            {
                string query = @"
                    SELECT 
                        ISNULL(SUM(CASE WHEN izin_turu = N'Yıllık İzin' THEN izin_Suresi ELSE 0 END), 0) as ToplamYillik,
                        ISNULL(SUM(CASE WHEN izin_turu = N'Rapor' THEN izin_Suresi ELSE 0 END), 0) as ToplamRapor,
                        ISNULL(SUM(CASE WHEN izin_turu = N'Saatlik izin' THEN izin_Suresi ELSE 0 END), 0) as ToplamSaatlik,
                        ISNULL(SUM(CASE WHEN izin_turu = N'Mazeret İzni' THEN izin_Suresi ELSE 0 END), 0) as ToplamMazeret
                    FROM personel_izin 
                    WHERE Sicil_No = @SicilNo";

                var parametreler = CreateParameters(("@SicilNo", sicilNo));
                DataTable dt = ExecuteDataTable(query, parametreler);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    lblToplamYillik.Text = Convert.ToDouble(row["ToplamYillik"]).ToString("0.#");
                    lblToplamRapor.Text = Convert.ToDouble(row["ToplamRapor"]).ToString("0.#");
                    lblToplamSaatlik.Text = Convert.ToDouble(row["ToplamSaatlik"]).ToString("0.#");
                    lblToplamMazeret.Text = Convert.ToDouble(row["ToplamMazeret"]).ToString("0.#");
                }
            }
            catch (Exception ex)
            {
                LogError("Personel toplam izin hesaplama hatası", ex);
            }
        }

        #endregion

        #region Detaylı Arama

        protected void btnAra_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT * FROM personel_izin WHERE 1=1 ";

                if (!string.IsNullOrWhiteSpace(txtAdSoyad.Text))
                {
                    query += " AND Adi_Soyadi LIKE @AdSoyad";
                }

                if (!string.IsNullOrWhiteSpace(txtSicilNo.Text))
                {
                    query += " AND Sicil_No = @SicilNo";
                }

                if (!string.IsNullOrWhiteSpace(ddlIzinTuru.SelectedValue))
                {
                    query += " AND izin_turu = @IzinTuru";
                }

                if (!string.IsNullOrWhiteSpace(txtBaslangicTarihi.Text))
                {
                    query += " AND izne_Baslama_Tarihi >= @BaslangicTarihi";
                }

                if (!string.IsNullOrWhiteSpace(txtBitisTarihi.Text))
                {
                    query += " AND izin_Bitis_Tarihi <= @BitisTarihi";
                }

                query += " ORDER BY id DESC";

                var parametreler = CreateParameters(
                    ("@AdSoyad", $"%{txtAdSoyad.Text}%"),
                    ("@SicilNo", txtSicilNo.Text),
                    ("@IzinTuru", ddlIzinTuru.SelectedValue),
                    ("@BaslangicTarihi", txtBaslangicTarihi.Text),
                    ("@BitisTarihi", txtBitisTarihi.Text)
                );

                DataTable dt = ExecuteDataTable(query, parametreler);

                AramaSonuclariGrid.DataSource = dt;
                AramaSonuclariGrid.DataBind();

                lblAramaSayisi.Text = dt.Rows.Count.ToString();

                if (dt.Rows.Count > 0)
                {
                    ShowToast($"{dt.Rows.Count} kayıt bulundu.", "success");
                }
                else
                {
                    ShowToast("Arama kriterlerine uygun kayıt bulunamadı.", "info");
                }

                LogInfo($"İzin araması yapıldı. Sonuç: {dt.Rows.Count} kayıt");
            }
            catch (Exception ex)
            {
                LogError("İzin arama hatası", ex);
                ShowToast("Arama sırasında hata oluştu.", "danger");
            }
        }

        protected void btnTemizle_Click(object sender, EventArgs e)
        {
            txtSicilNo.Text = string.Empty;
            txtAdSoyad.Text = string.Empty;
            ddlIzinTuru.SelectedIndex = 0;
            txtBaslangicTarihi.Text = string.Empty;
            txtBitisTarihi.Text = string.Empty;

            pnlPersonelBilgi.Visible = false;

            AramaSonuclariGrid.DataSource = null;
            AramaSonuclariGrid.DataBind();
            lblAramaSayisi.Text = "0";

            ShowToast("Arama kriterleri temizlendi.", "info");
        }

        #endregion

        #region GridView Events

        protected void AramaSonuclariGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                AramaSonuclariGrid.PageIndex = e.NewPageIndex;
                btnAra_Click(sender, e);
            }
            catch (Exception ex)
            {
                LogError("Sayfa değiştirme hatası", ex);
                ShowToast("Sayfa değiştirilirken hata oluştu.", "danger");
            }
        }

        #endregion

        #region Excel Export

        protected void btnExcelAktar_Click(object sender, EventArgs e)
        {
            if (AramaSonuclariGrid.Rows.Count == 0)
            {
                ShowToast("Export edilecek veri bulunamadı.", "warning");
                return;
            }

            try
            {
                ExportGridViewToExcel(AramaSonuclariGrid, "PersonelIzinArama_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xls");
                LogInfo("İzin arama sonuçları Excel'e aktarıldı.");
            }
            catch (Exception ex)
            {
                LogError("Excel export hatası", ex);
                ShowToast("Excel dosyası oluşturulurken hata oluştu.", "danger");
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
        }

        #endregion
    }
}