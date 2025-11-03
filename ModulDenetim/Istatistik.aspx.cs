using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using Portal.Base;

namespace Portal.ModulDenetim
{
    public partial class Istatistik : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(Sabitler.DENETIM_ISTATISTIK))
                {
                    return;
                }

                YillariDoldur();
                VerileriYukle();
            }
        }

        #region Dropdown Metodları

        private void YillariDoldur()
        {
            try
            {
                ddlYil.Items.Clear();
                int mevcutYil = DateTime.Now.Year;

                for (int yil = 2019; yil <= mevcutYil + 1; yil++)
                {
                    ddlYil.Items.Add(new ListItem(yil.ToString(), yil.ToString()));
                }

                SetSafeDropDownValue(ddlYil, mevcutYil.ToString());
            }
            catch (Exception ex)
            {
                LogError("Yıllar doldurulurken hata", ex);
                ShowToast("Yıllar yüklenirken hata oluştu.", "danger");
            }
        }

        #endregion

        #region Veri Yükleme Metodları

        private void VerileriYukle()
        {
            try
            {
                WidgetVerileriniYukle();
            }
            catch (Exception ex)
            {
                LogError("Widget verileri yüklenirken hata", ex);
                ShowToast($"Widget hatası: {ex.Message}", "danger");
            }

            try
            {
                AylikDenetimleriYukle();
            }
            catch (Exception ex)
            {
                LogError("Aylık denetimler yüklenirken hata", ex);
                ShowToast($"Aylık denetim hatası: {ex.Message}", "danger");
            }

            try
            {
                GrafikleriYukle();
            }
            catch (Exception ex)
            {
                LogError("Grafikler yüklenirken hata", ex);
                ShowToast($"Grafik hatası: {ex.Message}", "danger");
            }
        }

        private void WidgetVerileriniYukle()
        {
            int secilenYil = Convert.ToInt32(ddlYil.SelectedValue);

            string queryIsletme = @"SELECT COUNT(id) FROM denetimisletme 
                                   WHERE DenetimTuru = @DenetimTuru AND YEAR(DenetimTarihi) = @Yil";

            string queryArac = @"SELECT COUNT(id) FROM denetimtasit 
                                WHERE DenetimTuru = @DenetimTuru AND YEAR(DenetimTarihi) = @Yil";

            DataTable dtYolcuIsletme = ExecuteDataTable(queryIsletme, CreateParameters(
                ("@DenetimTuru", Sabitler.YOLCU_TASIMACILIGI),
                ("@Yil", secilenYil)
            ));
            int yolcuIsletmeSayisi = dtYolcuIsletme.Rows.Count > 0 ? Convert.ToInt32(dtYolcuIsletme.Rows[0][0]) : 0;
            lblYolcuIsletme.Text = yolcuIsletmeSayisi.ToString();

            DataTable dtYolcuArac = ExecuteDataTable(queryArac, CreateParameters(
                ("@DenetimTuru", Sabitler.YOLCU_TASIMACILIGI),
                ("@Yil", secilenYil)
            ));
            int yolcuAracSayisi = dtYolcuArac.Rows.Count > 0 ? Convert.ToInt32(dtYolcuArac.Rows[0][0]) : 0;
            lblYolcuArac.Text = yolcuAracSayisi.ToString();

            DataTable dtEsyaIsletme = ExecuteDataTable(queryIsletme, CreateParameters(
                ("@DenetimTuru", Sabitler.ESYA_TASIMACILIGI),
                ("@Yil", secilenYil)
            ));
            int esyaIsletmeSayisi = dtEsyaIsletme.Rows.Count > 0 ? Convert.ToInt32(dtEsyaIsletme.Rows[0][0]) : 0;
            lblEsyaIsletme.Text = esyaIsletmeSayisi.ToString();

            DataTable dtEsyaArac = ExecuteDataTable(queryArac, CreateParameters(
                ("@DenetimTuru", Sabitler.ESYA_TASIMACILIGI),
                ("@Yil", secilenYil)
            ));
            int esyaAracSayisi = dtEsyaArac.Rows.Count > 0 ? Convert.ToInt32(dtEsyaArac.Rows[0][0]) : 0;
            lblEsyaArac.Text = esyaAracSayisi.ToString();

            DataTable dtTmIsletme = ExecuteDataTable(queryIsletme, CreateParameters(
                ("@DenetimTuru", Sabitler.TEHLIKELI_MADDE),
                ("@Yil", secilenYil)
            ));
            int tmIsletmeSayisi = dtTmIsletme.Rows.Count > 0 ? Convert.ToInt32(dtTmIsletme.Rows[0][0]) : 0;
            lblTmIsletme.Text = tmIsletmeSayisi.ToString();

            DataTable dtTmArac = ExecuteDataTable(queryArac, CreateParameters(
                ("@DenetimTuru", Sabitler.TEHLIKELI_MADDE),
                ("@Yil", secilenYil)
            ));
            int tmAracSayisi = dtTmArac.Rows.Count > 0 ? Convert.ToInt32(dtTmArac.Rows[0][0]) : 0;
            lblTmArac.Text = tmAracSayisi.ToString();

            string queryUzakDenetim = @"SELECT SUM(AracSayisi) as Sayi, 
                                       SUM(UygunsuzAracSayisi) as Kontrol, 
                                       SUM(YBOlmayanAracSayisi + YBKayitliOlmayanAracSayisi) as Toplam 
                                       FROM denetimuzak WHERE YEAR(Tarih) = @Yil";

            DataTable dtUzak = ExecuteDataTable(queryUzakDenetim, CreateParameters(("@Yil", secilenYil)));
            if (dtUzak.Rows.Count > 0 && dtUzak.Rows[0]["Sayi"] != DBNull.Value)
            {
                lblUzakArac.Text = dtUzak.Rows[0]["Sayi"].ToString();
                lblUzakCeza.Text = dtUzak.Rows[0]["Kontrol"].ToString() + " - " + dtUzak.Rows[0]["Toplam"].ToString();
            }
            else
            {
                lblUzakArac.Text = "0";
                lblUzakCeza.Text = "0 - 0";
            }

            int toplamIsletme = yolcuIsletmeSayisi + esyaIsletmeSayisi + tmIsletmeSayisi;
            int toplamArac = yolcuAracSayisi + esyaAracSayisi + tmAracSayisi;

            lblToplamIsletme.Text = "Toplam İşletme Denetimi: " + toplamIsletme.ToString();
            lblToplamArac.Text = "Toplam Araç Denetimi: " + toplamArac.ToString();
        }

        private void AylikDenetimleriYukle()
        {
            int secilenYil = Convert.ToInt32(ddlYil.SelectedValue);

            string queryAylikIsletme = @"
                SELECT A1.*, YILLIKTOPLAM 
                FROM (
                    SELECT DenetimTuru As [Denetim Türü], 
                        ISNULL([1], 0) AS OCAK, ISNULL([2], 0) AS ŞUBAT, 
                        ISNULL([3], 0) AS MART, ISNULL([4], 0) AS NİSAN, 
                        ISNULL([5], 0) AS MAYIS, ISNULL([6], 0) AS HAZİRAN, 
                        ISNULL([7], 0) AS TEMMUZ, ISNULL([8], 0) AS AĞUSTOS, 
                        ISNULL([9], 0) AS EYLÜL, ISNULL([10], 0) AS EKİM, 
                        ISNULL([11], 0) AS KASIM, ISNULL([12], 0) AS ARALIK 
                    FROM (
                        SELECT DenetimTuru, MONTH(DenetimTarihi) AS AY, COUNT(id) AS YILLIKTOPLAM 
                        FROM denetimisletme 
                        WHERE YEAR(DenetimTarihi) = @Yil 
                        GROUP BY DenetimTuru, MONTH(DenetimTarihi)
                    ) AS DATA 
                    PIVOT(SUM(YILLIKTOPLAM) FOR AY IN([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS PIVOTTABLE
                ) AS A1  
                LEFT OUTER JOIN (
                    SELECT DenetimTuru, COUNT(id) AS YILLIKTOPLAM 
                    FROM denetimisletme 
                    WHERE YEAR(DenetimTarihi) = @Yil 
                    GROUP BY DenetimTuru
                ) AS A2 ON A1.[Denetim Türü] = A2.DenetimTuru  
                UNION ALL 
                SELECT 'GENEL TOPLAM' AS TOPLAM, 
                    SUM(A.Ocak), SUM(A.Subat), SUM(A.Mart), SUM(A.Nisan), 
                    SUM(A.Mayis), SUM(A.Haziran), SUM(A.Temmuz), SUM(A.Agustos), 
                    SUM(A.Eylul), SUM(A.Ekim), SUM(A.Kasim), SUM(A.Aralik), 
                    (SELECT COUNT(id) FROM denetimisletme WHERE YEAR(DenetimTarihi) = @Yil) 
                FROM (
                    SELECT DenetimTuru, 
                        ISNULL([1], 0) AS Ocak, ISNULL([2], 0) AS Subat, 
                        ISNULL([3], 0) AS Mart, ISNULL([4], 0) AS Nisan,
                        ISNULL([5], 0) AS Mayis, ISNULL([6], 0) AS Haziran, 
                        ISNULL([7], 0) AS Temmuz, ISNULL([8], 0) AS Agustos, 
                        ISNULL([9], 0) AS Eylul, ISNULL([10], 0) AS Ekim, 
                        ISNULL([11], 0) AS Kasim, ISNULL([12], 0) AS Aralik 
                    FROM (
                        SELECT DenetimTuru, MONTH(DenetimTarihi) AS AY, COUNT(id) AS YILLIKTOPLAM 
                        FROM denetimisletme 
                        WHERE YEAR(DenetimTarihi) = @Yil 
                        GROUP BY DenetimTuru, MONTH(DenetimTarihi)
                    ) AS DATA 
                    PIVOT(SUM(YILLIKTOPLAM) FOR AY IN([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS PIVOTTABLE
                ) AS A";

            DataTable dtAylikIsletme = ExecuteDataTable(queryAylikIsletme, CreateParameters(("@Yil", secilenYil)));
            gvAylikIsletme.DataSource = dtAylikIsletme;
            gvAylikIsletme.DataBind();

            string queryAylikTasit = @"
                SELECT A1.*, YILLIKTOPLAM 
                FROM (
                    SELECT DenetimTuru As [Denetim Türü], 
                        ISNULL([1], 0) AS OCAK, ISNULL([2], 0) AS ŞUBAT, 
                        ISNULL([3], 0) AS MART, ISNULL([4], 0) AS NİSAN, 
                        ISNULL([5], 0) AS MAYIS, ISNULL([6], 0) AS HAZİRAN, 
                        ISNULL([7], 0) AS TEMMUZ, ISNULL([8], 0) AS AĞUSTOS, 
                        ISNULL([9], 0) AS EYLÜL, ISNULL([10], 0) AS EKİM, 
                        ISNULL([11], 0) AS KASIM, ISNULL([12], 0) AS ARALIK 
                    FROM (
                        SELECT DenetimTuru, MONTH(DenetimTarihi) AS AY, COUNT(id) AS YILLIKTOPLAM 
                        FROM denetimtasit 
                        WHERE YEAR(DenetimTarihi) = @Yil 
                        GROUP BY DenetimTuru, MONTH(DenetimTarihi)
                    ) AS DATA 
                    PIVOT(SUM(YILLIKTOPLAM) FOR AY IN([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS PIVOTTABLE
                ) AS A1  
                LEFT OUTER JOIN (
                    SELECT DenetimTuru, COUNT(id) AS YILLIKTOPLAM 
                    FROM denetimtasit 
                    WHERE YEAR(DenetimTarihi) = @Yil 
                    GROUP BY DenetimTuru
                ) AS A2 ON A1.[Denetim Türü] = A2.DenetimTuru  
                UNION ALL 
                SELECT 'GENEL TOPLAM' AS TOPLAM, 
                    SUM(A.Ocak), SUM(A.Subat), SUM(A.Mart), SUM(A.Nisan), 
                    SUM(A.Mayis), SUM(A.Haziran), SUM(A.Temmuz), SUM(A.Agustos), 
                    SUM(A.Eylul), SUM(A.Ekim), SUM(A.Kasim), SUM(A.Aralik), 
                    (SELECT COUNT(id) FROM denetimtasit WHERE YEAR(DenetimTarihi) = @Yil) 
                FROM (
                    SELECT DenetimTuru, 
                        ISNULL([1], 0) AS Ocak, ISNULL([2], 0) AS Subat, 
                        ISNULL([3], 0) AS Mart, ISNULL([4], 0) AS Nisan,
                        ISNULL([5], 0) AS Mayis, ISNULL([6], 0) AS Haziran, 
                        ISNULL([7], 0) AS Temmuz, ISNULL([8], 0) AS Agustos, 
                        ISNULL([9], 0) AS Eylul, ISNULL([10], 0) AS Ekim, 
                        ISNULL([11], 0) AS Kasim, ISNULL([12], 0) AS Aralik 
                    FROM (
                        SELECT DenetimTuru, MONTH(DenetimTarihi) AS AY, COUNT(id) AS YILLIKTOPLAM 
                        FROM denetimtasit 
                        WHERE YEAR(DenetimTarihi) = @Yil 
                        GROUP BY DenetimTuru, MONTH(DenetimTarihi)
                    ) AS DATA 
                    PIVOT(SUM(YILLIKTOPLAM) FOR AY IN([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS PIVOTTABLE
                ) AS A";

            DataTable dtAylikTasit = ExecuteDataTable(queryAylikTasit, CreateParameters(("@Yil", secilenYil)));
            gvAylikTasit.DataSource = dtAylikTasit;
            gvAylikTasit.DataBind();
        }

        private void GrafikleriYukle()
        {
            int secilenYil = Convert.ToInt32(ddlYil.SelectedValue);

            string queryIsletmeIl = @"SELECT il, COUNT(id) as Sayi 
                                     FROM denetimisletme 
                                     WHERE YEAR(DenetimTarihi) = @Yil 
                                     GROUP BY il 
                                     ORDER BY il ASC";
            DataTable dtIsletmeIl = ExecuteDataTable(queryIsletmeIl, CreateParameters(("@Yil", secilenYil)));
            GrafikDoldur(ChartIsletmeIl, dtIsletmeIl, false);

            string queryAracIl = @"SELECT il, COUNT(id) as Sayi 
                                  FROM denetimtasit 
                                  WHERE YEAR(DenetimTarihi) = @Yil 
                                  GROUP BY il 
                                  ORDER BY il ASC";
            DataTable dtAracIl = ExecuteDataTable(queryAracIl, CreateParameters(("@Yil", secilenYil)));
            GrafikDoldur(ChartAracIl, dtAracIl, false);

            string queryIsletmeTur = @"SELECT DenetimTuru, COUNT(id) as Sayi 
                                      FROM denetimisletme 
                                      WHERE YEAR(DenetimTarihi) = @Yil 
                                      GROUP BY DenetimTuru 
                                      ORDER BY DenetimTuru ASC";
            DataTable dtIsletmeTur = ExecuteDataTable(queryIsletmeTur, CreateParameters(("@Yil", secilenYil)));
            GrafikDoldur(ChartIsletmeTur, dtIsletmeTur, false);

            string queryTasitTur = @"SELECT DenetimTuru, COUNT(id) as Sayi 
                                    FROM denetimtasit 
                                    WHERE YEAR(DenetimTarihi) = @Yil 
                                    GROUP BY DenetimTuru 
                                    ORDER BY DenetimTuru ASC";
            DataTable dtTasitTur = ExecuteDataTable(queryTasitTur, CreateParameters(("@Yil", secilenYil)));
            GrafikDoldur(ChartTasitTur, dtTasitTur, false);

            string queryIsletmePersonel = @"SELECT pp.Personel1 as Personel, SUM(Adet) as Adet 
                                           FROM (
                                               SELECT Personel1, COUNT(id) as Adet 
                                               FROM denetimisletme 
                                               WHERE YEAR(DenetimTarihi) = @Yil 
                                               GROUP BY Personel1 
                                               UNION ALL 
                                               SELECT Personel2, COUNT(id) as Adet 
                                               FROM denetimisletme 
                                               WHERE YEAR(DenetimTarihi) = @Yil 
                                               GROUP BY Personel2
                                           ) pp 
                                           WHERE pp.Personel1 != '' 
                                           GROUP BY pp.Personel1 
                                           ORDER BY Adet DESC";
            DataTable dtIsletmePersonel = ExecuteDataTable(queryIsletmePersonel, CreateParameters(("@Yil", secilenYil)));
            GrafikDoldur(ChartIsletmePersonel, dtIsletmePersonel, true);

            string queryTasitPersonel = @"SELECT pp.Personel1 as Personel, SUM(Adet) as Adet 
                                         FROM (
                                             SELECT Personel1, COUNT(id) as Adet 
                                             FROM denetimtasit 
                                             WHERE YEAR(DenetimTarihi) = @Yil 
                                             GROUP BY Personel1 
                                             UNION ALL 
                                             SELECT Personel2, COUNT(id) as Adet 
                                             FROM denetimtasit 
                                             WHERE Personel2 != '' AND YEAR(DenetimTarihi) = @Yil 
                                             GROUP BY Personel2
                                         ) pp 
                                         WHERE pp.Personel1 != '' 
                                         GROUP BY pp.Personel1 
                                         ORDER BY Adet DESC";
            DataTable dtTasitPersonel = ExecuteDataTable(queryTasitPersonel, CreateParameters(("@Yil", secilenYil)));
            GrafikDoldur(ChartTasitPersonel, dtTasitPersonel, true);
        }

        private void GrafikDoldur(Chart chart, DataTable dt, bool personelGrafik = false)
        {
            if (dt.Rows.Count == 0) return;
            if (chart.Series.Count == 0) return;

            chart.Series[0].Points.Clear();

            string xField = personelGrafik ? "Personel" : dt.Columns[0].ColumnName;
            string yField = personelGrafik ? "Adet" : "Sayi";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string xValue = dt.Rows[i][xField].ToString();
                int yValue = Convert.ToInt32(dt.Rows[i][yField]);
                chart.Series[0].Points.AddXY(xValue, yValue);
            }

            if (personelGrafik && chart.ChartAreas.Count > 0)
            {
                chart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                chart.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                chart.ChartAreas[0].AxisY.LabelStyle.Angle = 0;
                chart.Series[0].LabelAngle = -90;
            }
            else if (chart.ChartAreas.Count > 0)
            {
                chart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
            }
        }

        #endregion

        #region Event Handlers

        protected void ddlYil_SelectedIndexChanged(object sender, EventArgs e)
        {
            VerileriYukle();
        }

        protected void btnExcelIsletme_Click(object sender, EventArgs e)
        {
            if (gvAylikIsletme.Rows.Count == 0)
            {
                ShowToast("Export edilecek veri bulunamadı.", "warning");
                return;
            }

            try
            {
                string dosyaAdi = $"AylikIsletmeDenetimi_{ddlYil.SelectedValue}_{DateTime.Now:yyyyMMdd}.xls";
                ExportGridViewToExcel(gvAylikIsletme, dosyaAdi);
                LogInfo($"Aylık işletme denetimi Excel'e aktarıldı: {dosyaAdi}");
            }
            catch (Exception ex)
            {
                LogError("Excel export hatası", ex);
                ShowToast("Excel dosyası oluşturulurken hata oluştu.", "danger");
            }
        }

        protected void btnExcelTasit_Click(object sender, EventArgs e)
        {
            if (gvAylikTasit.Rows.Count == 0)
            {
                ShowToast("Export edilecek veri bulunamadı.", "warning");
                return;
            }

            try
            {
                string dosyaAdi = $"AylikTasitDenetimi_{ddlYil.SelectedValue}_{DateTime.Now:yyyyMMdd}.xls";
                ExportGridViewToExcel(gvAylikTasit, dosyaAdi);
                LogInfo($"Aylık taşıt denetimi Excel'e aktarıldı: {dosyaAdi}");
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