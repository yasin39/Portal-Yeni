using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Base;

namespace Portal.ModulCimer
{
    public partial class Istatistik : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(Sabitler.CIMER_PERSONEL))
                {
                    return;
                }

                YillariDoldur();
                int mevcutYil = DateTime.Now.Year;
                SetSafeDropDownValue(ddlYil, mevcutYil.ToString());
                IstatistikleriYukle(mevcutYil);
            }
        }

        private void YillariDoldur()
        {
            int baslangicYili = 2017;
            int bitisYili = DateTime.Now.Year + 5; // Gelecek 5 yıl dahil

            for (int yil = baslangicYili; yil <= bitisYili; yil++)
            {
                ddlYil.Items.Add(new ListItem(yil.ToString(), yil.ToString()));
            }
        }

        protected void ddlYil_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.TryParse(ddlYil.SelectedValue, out int seciliYil))
            {
                IstatistikleriYukle(seciliYil);
            }
        }

        private void IstatistikleriYukle(int yil)
        {
            try
            {
                // Firma dağılımı için chart verisi
                string queryFirma = @"
                    SELECT Sikayet_Edilen_Firma, COUNT(*) AS Sayi 
                    FROM cimer_basvurular 
                    WHERE YEAR(Kayit_Tarihi) = @Yil 
                    GROUP BY Sikayet_Edilen_Firma 
                    ORDER BY Sayi DESC";

                var firmaParams = CreateParameters(("@Yil", yil));
                DataTable dtFirma = ExecuteDataTable(queryFirma, firmaParams);

                // Chart'ı doldur
                chartSirketDagilim.Series[0].Points.Clear();
                foreach (DataRow row in dtFirma.Rows)
                {
                    string firma = row["Sikayet_Edilen_Firma"].ToString();
                    int sayi = Convert.ToInt32(row["Sayi"]);
                    chartSirketDagilim.Series[0].Points.AddXY(firma, sayi);
                }
                chartSirketDagilim.ChartAreas[0].AxisX.Interval = 1; // Her etiketi göster

                // Devam eden başvurular (Onay_Durumu != '3')
                string queryDevam = @"
                    SELECT COUNT(*) AS Sayi 
                    FROM cimer_basvurular 
                    WHERE Onay_Durumu != '3' AND YEAR(Kayit_Tarihi) = @Yil";

                var devamParams = CreateParameters(("@Yil", yil));
                object devamSayiObj = ExecuteScalar(queryDevam, devamParams);
                int devamSayi = Convert.ToInt32(devamSayiObj ?? 0);
                lblDevamEden.Text = devamSayi.ToString();

                // Cevaplanan (Onay_Durumu = '3')
                string queryCevap = @"
                    SELECT COUNT(*) AS Sayi 
                    FROM cimer_basvurular 
                    WHERE Onay_Durumu = '3' AND YEAR(Kayit_Tarihi) = @Yil";

                var cevapParams = CreateParameters(("@Yil", yil));
                object cevapSayiObj = ExecuteScalar(queryCevap, cevapParams);
                int cevapSayi = Convert.ToInt32(cevapSayiObj ?? 0);
                lblCevaplanan.Text = cevapSayi.ToString();

                // Toplam (devam + cevaplanan)
                int toplam = devamSayi + cevapSayi;
                lblToplam.Text = toplam.ToString();

                // İkinci kez cevaplanan (Son_Yapilan_islem IS NOT NULL)
                string queryIkinci = @"
                    SELECT COUNT(*) AS Sayi 
                    FROM cimer_basvurular 
                    WHERE Son_Yapilan_islem IS NOT NULL AND YEAR(Kayit_Tarihi) = @Yil";

                var ikinciParams = CreateParameters(("@Yil", yil));
                object ikinciSayiObj = ExecuteScalar(queryIkinci, ikinciParams);
                int ikinciSayi = Convert.ToInt32(ikinciSayiObj ?? 0);
                lblIkinciKez.Text = ikinciSayi.ToString();

                LogInfo($"İstatistikler yüklendi - Yıl: {yil}, Toplam: {toplam}");
            }
            catch (Exception ex)
            {
                LogError("İstatistik yükleme hatası", ex);
                ShowErrorAndRedirect("İstatistikler yüklenirken bir hata oluştu. Lütfen yöneticiyle iletişime geçiniz.", "~/Anasayfa.aspx");
            }
        }
    }
}