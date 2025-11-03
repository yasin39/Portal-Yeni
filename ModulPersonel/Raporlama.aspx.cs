using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Portal.Base;
using Portal;
using System.Web.UI;

namespace ModulPersonel
{
    public partial class PersonelRaporla : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(Sabitler.Personel))
                {
                    return;
                }

                LoadData();
            }
        }

        private void LoadData()
        {
            try
            {
                LoadStatistics();
                LoadPersonelDagilimGrid();
                LoadBirimDagilimGrid();
                LoadChartData();
            }
            catch (Exception ex)
            {
                LogError("PersonelRaporla LoadData hatası", ex);
                ShowToast("Veriler yüklenirken bir hata oluştu.", "danger");
            }
        }

        private void LoadStatistics()
        {
            string queryKadroluAktif = @"
                SELECT COUNT(*) 
                FROM personel 
                WHERE CalismaDurumu='Kadrolu Aktif Çalışan' 
                AND Statu !='Firma Personeli' 
                AND Durum='Aktif'";

            string queryGeciciAktif = @"
                SELECT COUNT(*) 
                FROM personel 
                WHERE CalismaDurumu='Geçici Görevli Aktif Çalışan' 
                AND Durum='Aktif'";

            string queryFirmaTYP = @"
                SELECT COUNT(*) 
                FROM personel 
                WHERE (Statu='Firma Personeli' OR Statu='İşkur İşçi (TYP)') 
                AND Durum='Aktif'";

            string queryKadroluGecici = @"
                SELECT COUNT(*) 
                FROM personel 
                WHERE CalismaDurumu='Geçici Görevde Pasif Çalışan' 
                AND Durum='Aktif'";

            string queryToplam = @"
                SELECT COUNT(*) 
                FROM personel 
                WHERE Durum='Aktif'";

            int KadroluAktifSayisi = Convert.ToInt32(ExecuteScalar(queryKadroluAktif));
            int GeciciAktifSayisi = Convert.ToInt32(ExecuteScalar(queryGeciciAktif));
            int FirmaTYPSayisi = Convert.ToInt32(ExecuteScalar(queryFirmaTYP));
            int KadroluGeciciSayisi = Convert.ToInt32(ExecuteScalar(queryKadroluGecici));
            int ToplamSayisi = Convert.ToInt32(ExecuteScalar(queryToplam));
            int ToplamAktifSayisi = KadroluAktifSayisi + GeciciAktifSayisi + FirmaTYPSayisi;

            lblKadroluAktif.Text = KadroluAktifSayisi.ToString();
            lblGeciciAktif.Text = GeciciAktifSayisi.ToString();
            lblFirmaTYP.Text = FirmaTYPSayisi.ToString();
            lblToplamAktif.Text = ToplamAktifSayisi.ToString();
            lblKadroluGecici.Text = KadroluGeciciSayisi.ToString();
            lblToplamPersonel.Text = ToplamSayisi.ToString();
        }

        private void LoadPersonelDagilimGrid()
        {
            string query = @"
                SELECT 
                    hh.Unvan as Unvan, 
                    ISNULL(tkp.[tk.Kadrolu], 0) as Toplam_Kadrolu,
                    ISNULL(kk.[kp.Kadrolu], 0) as KadroluGiden,
                    ISNULL(pp.[p.Kadrolu], 0) as Kadrolu,
                    ISNULL(gg.[gp.Kadrolu], 0) as Gecici_Gelen,
                    ISNULL(ff.[f.Kadrolu], 0) as Firma,
                    ISNULL(typ.[ty.Kadrolu], 0) as TYP,
                    ISNULL(pp.[p.Kadrolu], 0) + ISNULL(gg.[gp.Kadrolu], 0) + ISNULL(ff.[f.Kadrolu], 0) + ISNULL(typ.[ty.Kadrolu], 0) as Toplam_Aktif
                FROM 
                    (SELECT h.Unvan, COUNT(*) As[h.Kadrolu] FROM personel h WHERE Durum='Aktif' GROUP BY Unvan) hh
                FULL JOIN 
                    (SELECT p.Unvan, COUNT(*) As[p.Kadrolu] FROM personel p WHERE Durum='Aktif' AND CalismaDurumu='Kadrolu Aktif Çalışan' GROUP BY Unvan) pp ON pp.Unvan = hh.Unvan
                FULL JOIN 
                    (SELECT kp.Unvan, COUNT(*) As[kp.Kadrolu] FROM personel kp WHERE Durum='Aktif' AND CalismaDurumu='Geçici Görevde Pasif Çalışan' GROUP BY Unvan) kk ON hh.Unvan = kk.Unvan
                FULL JOIN
                    (SELECT tk.Unvan, COUNT(*) As[tk.Kadrolu] FROM personel tk WHERE Durum='Aktif' AND CalismaDurumu !='Geçici Görevli Aktif Çalışan' AND CalismaDurumu !='Firma personeli' AND CalismaDurumu !='İşkur İşçi (TYP)' GROUP BY Unvan) tkp ON hh.Unvan = tkp.Unvan
                FULL JOIN
                    (SELECT gp.Unvan, COUNT(*) As[gp.Kadrolu] FROM personel gp WHERE Durum='Aktif' AND CalismaDurumu='Geçici Görevli Aktif Çalışan' GROUP BY Unvan) gg ON hh.Unvan = gg.Unvan
                FULL JOIN
                    (SELECT f.Unvan, COUNT(*) As[f.Kadrolu] FROM personel f WHERE Durum='Aktif' AND CalismaDurumu='Firma personeli' GROUP BY Unvan) ff ON hh.Unvan = ff.Unvan
                FULL JOIN
                    (SELECT ty.Unvan, COUNT(*) As[ty.Kadrolu] FROM personel ty WHERE Durum='Aktif' AND CalismaDurumu='İşkur İşçi (TYP)' GROUP BY Unvan) typ ON hh.Unvan = typ.Unvan
                ORDER BY hh.Unvan ASC";

            DataTable PersonelTablosu = ExecuteDataTable(query);
            PersonelDagılımGrid.DataSource = PersonelTablosu;
            PersonelDagılımGrid.DataBind();

            if (PersonelDagılımGrid.Rows.Count > 0)
            {
                CalculateGridFooter();
            }
        }

        private void CalculateGridFooter()
        {
            int ToplamKadrolu = 0;
            int KadroluGiden = 0;
            int Kadrolu = 0;
            int GeciciGelen = 0;
            int Firma = 0;
            int TYP = 0;
            int ToplamAktif = 0;

            foreach (GridViewRow row in PersonelDagılımGrid.Rows)
            {
                ToplamKadrolu += Convert.ToInt32(row.Cells[1].Text);
                KadroluGiden += Convert.ToInt32(row.Cells[2].Text);
                Kadrolu += Convert.ToInt32(row.Cells[3].Text);
                GeciciGelen += Convert.ToInt32(row.Cells[4].Text);
                Firma += Convert.ToInt32(row.Cells[5].Text);
                TYP += Convert.ToInt32(row.Cells[6].Text);
                ToplamAktif += Convert.ToInt32(row.Cells[7].Text);
            }

            PersonelDagılımGrid.FooterRow.Cells[0].Text = "Toplam " + PersonelDagılımGrid.Rows.Count + " Adet Unvan";
            PersonelDagılımGrid.FooterRow.Cells[1].Text = ToplamKadrolu.ToString();
            PersonelDagılımGrid.FooterRow.Cells[2].Text = KadroluGiden.ToString();
            PersonelDagılımGrid.FooterRow.Cells[3].Text = Kadrolu.ToString();
            PersonelDagılımGrid.FooterRow.Cells[4].Text = GeciciGelen.ToString();
            PersonelDagılımGrid.FooterRow.Cells[5].Text = Firma.ToString();
            PersonelDagılımGrid.FooterRow.Cells[6].Text = TYP.ToString();
            PersonelDagılımGrid.FooterRow.Cells[7].Text = ToplamAktif.ToString();
        }

        private void LoadBirimDagilimGrid()
        {
            string query = @"
                SELECT 
                    GorevYaptigiBirim as Birim_Adi, 
                    COUNT(*) As Personel_Sayisi
                FROM personel 
                WHERE Durum='Aktif' 
                AND CalismaDurumu!='Geçici Görevde Pasif Çalışan' 
                GROUP BY GorevYaptigiBirim 
                ORDER BY GorevYaptigiBirim ASC";

            DataTable BirimTablosu = ExecuteDataTable(query);
            BirimDagilimGrid.DataSource = BirimTablosu;
            BirimDagilimGrid.DataBind();
        }

        private void LoadChartData()
        {
            string queryKadro = @"
                SELECT Unvan, COUNT(*) As Unvan_Sayisi
                FROM personel 
                WHERE Durum='Aktif' 
                AND CalismaDurumu!='Geçici Görevde Pasif Çalışan'
                GROUP BY Unvan 
                ORDER BY Unvan ASC";

            string querySendika = @"
                SELECT Sendika, COUNT(*) As Sendika_Sayisi
                FROM personel 
                WHERE Durum='Aktif' 
                GROUP BY Sendika 
                ORDER BY Sendika ASC";

            string queryEngel = @"
                SELECT EngelDurumu, COUNT(*) As Engel_Sayisi
                FROM personel 
                WHERE Durum='Aktif' 
                GROUP BY EngelDurumu 
                ORDER BY EngelDurumu ASC";

            DataTable KadroData = ExecuteDataTable(queryKadro);
            DataTable SendikaData = ExecuteDataTable(querySendika);
            DataTable EngelData = ExecuteDataTable(queryEngel);

            hdnKadroData.Value = ConvertDataTableToJson(KadroData, "Unvan", "Unvan_Sayisi");
            hdnSendikaData.Value = ConvertDataTableToJson(SendikaData, "Sendika", "Sendika_Sayisi");
            hdnEngelData.Value = ConvertDataTableToJson(EngelData, "EngelDurumu", "Engel_Sayisi");
        }

        private string ConvertDataTableToJson(DataTable dt, string labelColumn, string valueColumn)
        {
            var labels = new List<string>();
            var values = new List<int>();

            foreach (DataRow row in dt.Rows)
            {
                labels.Add(row[labelColumn].ToString());
                values.Add(Convert.ToInt32(row[valueColumn]));
            }

            return $"{{\"labels\":{Newtonsoft.Json.JsonConvert.SerializeObject(labels)},\"values\":{Newtonsoft.Json.JsonConvert.SerializeObject(values)}}}";
        }

        protected void btnExcelExport_Click(object sender, EventArgs e)
        {
            try
            {
                ExportGridViewToExcel(PersonelDagılımGrid, "PersonelDagilimRaporu.xls");
            }
            catch (Exception ex)
            {
                LogError("Excel export hatası", ex);
                ShowToast("Excel dosyası oluşturulurken bir hata oluştu.", "danger");
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
        }
    }
}