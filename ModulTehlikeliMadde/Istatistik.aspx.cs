using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Portal.Base;

namespace Portal.ModulTehlikeliMadde
{
    public partial class Istatistik : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(Sabitler.TEHLIKELI_MADDE_KAYIT))
                    return;

                LoadIlDropdown();
                LoadIstatistik();
            }
        }

        private void LoadIlDropdown()
        {
            try
            {
                string sorgu = @"SELECT sehir 
                               FROM sehirler 
                               WHERE Bolge_Dahilimi = 1 
                               ORDER BY sehir ASC";

                DataTable illerTablo = ExecuteDataTable(sorgu);

                ddlIl.Items.Clear();
                ddlIl.Items.Insert(0, new ListItem("Hepsi", "Hepsi"));

                foreach (DataRow satir in illerTablo.Rows)
                {
                    ddlIl.Items.Add(new ListItem(satir["sehir"].ToString()));
                }
            }
            catch (Exception ex)
            {
                LogError("İl dropdown yükleme hatası", ex);
                ShowToast("İl listesi yüklenirken hata oluştu.", "error");
            }
        }

        private void LoadIstatistik()
        {
            try
            {
                string secilenIl = ddlIl.SelectedValue;

                if (secilenIl == "Hepsi")
                {
                    LoadTumIllerIstatistik();
                }
                else
                {
                    LoadTekIlIstatistik(secilenIl);
                }
            }
            catch (Exception ex)
            {
                LogError("İstatistik yükleme hatası", ex);
                ShowToast("İstatistik yüklenirken hata oluştu.", "error");
            }
        }

        private void LoadTumIllerIstatistik()
        {
            string sorgu = @"
                SELECT 
                    aa.FaaliyetTuru, 
                    ISNULL(ank.Adet, 0) AS Ankara, 
                    ISNULL(kon.Adet, 0) AS Konya, 
                    ISNULL(esk.Adet, 0) AS Eskişehir, 
                    ISNULL(kay.Adet, 0) AS Kayseri, 
                    ISNULL(nev.Adet, 0) AS Nevşehir, 
                    ISNULL(kır.Adet, 0) AS Kırşehir, 
                    ISNULL(kırk.Adet, 0) AS Kırıkkale, 
                    ISNULL(aks.Adet, 0) AS Aksaray, 
                    ISNULL(can.Adet, 0) AS Çankırı,
                    (ISNULL(ank.Adet, 0) + ISNULL(kon.Adet, 0) + ISNULL(esk.Adet, 0) + 
                     ISNULL(kay.Adet, 0) + ISNULL(nev.Adet, 0) + ISNULL(kır.Adet, 0) + 
                     ISNULL(kırk.Adet, 0) + ISNULL(aks.Adet, 0) + ISNULL(can.Adet, 0)) AS TOPLAM
                FROM (
                    SELECT FaaliyetTuru 
                    FROM tmistatistik 
                    WHERE Durum = @Durum 
                    GROUP BY FaaliyetTuru
                ) aa
                LEFT JOIN (
                    SELECT FaaliyetTuru, COUNT(id) AS Adet 
                    FROM tmistatistik 
                    WHERE Durum = @Durum AND il = 'Ankara' 
                    GROUP BY FaaliyetTuru
                ) ank ON aa.FaaliyetTuru = ank.FaaliyetTuru
                LEFT JOIN (
                    SELECT FaaliyetTuru, COUNT(id) AS Adet 
                    FROM tmistatistik 
                    WHERE Durum = @Durum AND il = 'Konya' 
                    GROUP BY FaaliyetTuru
                ) kon ON aa.FaaliyetTuru = kon.FaaliyetTuru
                LEFT JOIN (
                    SELECT FaaliyetTuru, COUNT(id) AS Adet 
                    FROM tmistatistik 
                    WHERE Durum = @Durum AND il = 'Eskişehir' 
                    GROUP BY FaaliyetTuru
                ) esk ON aa.FaaliyetTuru = esk.FaaliyetTuru
                LEFT JOIN (
                    SELECT FaaliyetTuru, COUNT(id) AS Adet 
                    FROM tmistatistik 
                    WHERE Durum = @Durum AND il = 'Kayseri' 
                    GROUP BY FaaliyetTuru
                ) kay ON aa.FaaliyetTuru = kay.FaaliyetTuru
                LEFT JOIN (
                    SELECT FaaliyetTuru, COUNT(id) AS Adet 
                    FROM tmistatistik 
                    WHERE Durum = @Durum AND il = 'Nevşehir' 
                    GROUP BY FaaliyetTuru
                ) nev ON aa.FaaliyetTuru = nev.FaaliyetTuru
                LEFT JOIN (
                    SELECT FaaliyetTuru, COUNT(id) AS Adet 
                    FROM tmistatistik 
                    WHERE Durum = @Durum AND il = 'Kırşehir' 
                    GROUP BY FaaliyetTuru
                ) kır ON aa.FaaliyetTuru = kır.FaaliyetTuru
                LEFT JOIN (
                    SELECT FaaliyetTuru, COUNT(id) AS Adet 
                    FROM tmistatistik 
                    WHERE Durum = @Durum AND il = 'Kırıkkale' 
                    GROUP BY FaaliyetTuru
                ) kırk ON aa.FaaliyetTuru = kırk.FaaliyetTuru
                LEFT JOIN (
                    SELECT FaaliyetTuru, COUNT(id) AS Adet 
                    FROM tmistatistik 
                    WHERE Durum = @Durum AND il = 'Aksaray' 
                    GROUP BY FaaliyetTuru
                ) aks ON aa.FaaliyetTuru = aks.FaaliyetTuru
                LEFT JOIN (
                    SELECT FaaliyetTuru, COUNT(id) AS Adet 
                    FROM tmistatistik 
                    WHERE Durum = @Durum AND il = 'Çankırı' 
                    GROUP BY FaaliyetTuru
                ) can ON aa.FaaliyetTuru = can.FaaliyetTuru

                UNION ALL

                SELECT 
                    'Genel Toplam' AS FaaliyetTuru,
                    (SELECT COUNT(id) FROM tmistatistik WHERE Durum = @Durum AND il = 'Ankara') AS Ankara,
                    (SELECT COUNT(id) FROM tmistatistik WHERE Durum = @Durum AND il = 'Konya') AS Konya,
                    (SELECT COUNT(id) FROM tmistatistik WHERE Durum = @Durum AND il = 'Eskişehir') AS Eskişehir,
                    (SELECT COUNT(id) FROM tmistatistik WHERE Durum = @Durum AND il = 'Kayseri') AS Kayseri,
                    (SELECT COUNT(id) FROM tmistatistik WHERE Durum = @Durum AND il = 'Nevşehir') AS Nevşehir,
                    (SELECT COUNT(id) FROM tmistatistik WHERE Durum = @Durum AND il = 'Kırşehir') AS Kırşehir,
                    (SELECT COUNT(id) FROM tmistatistik WHERE Durum = @Durum AND il = 'Kırıkkale') AS Kırıkkale,
                    (SELECT COUNT(id) FROM tmistatistik WHERE Durum = @Durum AND il = 'Aksaray') AS Aksaray,
                    (SELECT COUNT(id) FROM tmistatistik WHERE Durum = @Durum AND il = 'Çankırı') AS Çankırı,
                    (SELECT COUNT(id) FROM tmistatistik WHERE Durum = @Durum) AS TOPLAM";

            var parametreler = CreateParameters(("@Durum", "Geçerli"));
            DataTable istatistikTablo = ExecuteDataTable(sorgu, parametreler);

            IstatistikGrid.DataSource = istatistikTablo;
            IstatistikGrid.DataBind();

            CreateGrafik(istatistikTablo, "Hepsi");
        }

        private void LoadTekIlIstatistik(string ilAdi)
        {
            string sorgu = @"
                SELECT 
                    FaaliyetTuru, 
                    COUNT(id) AS Adet 
                FROM tmistatistik 
                WHERE Durum = @Durum AND il = @IlAdi 
                GROUP BY FaaliyetTuru 
                ORDER BY FaaliyetTuru";

            var parametreler = CreateParameters(
                ("@Durum", "Geçerli"),
                ("@IlAdi", ilAdi)
            );

            DataTable istatistikTablo = ExecuteDataTable(sorgu, parametreler);

            if (istatistikTablo.Rows.Count > 0)
            {
                IstatistikGrid.DataSource = istatistikTablo;
                IstatistikGrid.DataBind();

                CreateGrafik(istatistikTablo, ilAdi);
            }
            else
            {
                IstatistikGrid.DataSource = null;
                IstatistikGrid.DataBind();

                ShowToast($"{ilAdi} ili için veri bulunamadı.", "warning");

                ClientScript.RegisterStartupScript(this.GetType(), "GrafikTemizle",
                    "CreateChart([], []);", true);
            }
        }

        private void CreateGrafik(DataTable veriTablosu, string ilAdi)
        {
            try
            {
                if (veriTablosu.Rows.Count == 0)
                    return;

                DataTable grafikVeri = veriTablosu.Copy();

                if (ilAdi == "Hepsi")
                {
                    DataRow genelToplamSatir = grafikVeri.AsEnumerable()
                        .FirstOrDefault(r => r["FaaliyetTuru"].ToString() == "Genel Toplam");

                    if (genelToplamSatir != null)
                    {
                        grafikVeri.Rows.Remove(genelToplamSatir);
                    }

                    var etiketlerArray = grafikVeri.AsEnumerable()
                        .Select(r => r["FaaliyetTuru"].ToString())
                        .ToArray();

                    var verilerArray = grafikVeri.AsEnumerable()
                        .Select(r => Convert.ToInt32(r["TOPLAM"]))
                        .ToArray();

                    string etiketlerJson = string.Join(",", etiketlerArray.Select(e => $"'{EscapeJavaScript(e)}'"));
                    string verilerJson = string.Join(",", verilerArray);

                    string script = $"CreateChart([{etiketlerJson}], [{verilerJson}]);";
                    ClientScript.RegisterStartupScript(this.GetType(), "GrafikOlustur", script, true);
                }
                else
                {
                    var etiketlerArray = grafikVeri.AsEnumerable()
                        .Select(r => r["FaaliyetTuru"].ToString())
                        .ToArray();

                    var verilerArray = grafikVeri.AsEnumerable()
                        .Select(r => Convert.ToInt32(r["Adet"]))
                        .ToArray();

                    string etiketlerJson = string.Join(",", etiketlerArray.Select(e => $"'{EscapeJavaScript(e)}'"));
                    string verilerJson = string.Join(",", verilerArray);

                    string script = $"CreateChart([{etiketlerJson}], [{verilerJson}]);";
                    ClientScript.RegisterStartupScript(this.GetType(), "GrafikOlustur", script, true);
                }
            }
            catch (Exception ex)
            {
                LogError("Grafik oluşturma hatası", ex);
            }
        }

        private string EscapeJavaScript(string metin)
        {
            if (string.IsNullOrEmpty(metin))
                return string.Empty;

            return metin.Replace("'", "\\'").Replace("\"", "\\\"");
        }

        protected void ddlIl_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadIstatistik();
        }
    }
}