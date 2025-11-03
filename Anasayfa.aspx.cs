using Portal.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;


namespace Portal
{
    public partial class Anasayfa : BasePage
    {
        private WeatherService weatherService;

        protected void Page_Load(object sender, EventArgs e)
        {
            weatherService = new WeatherService();

            if (!IsPostBack)
            {
                ProfilGetir();
                DuyurularDoldur();
                DogumGunleriDoldur();
                DenetimGorevleriDoldur();
                AktifIzinlerDoldur();

                LoadCityDropdown();

                LoadWeatherWidget("Ankara");
            }
        }

        private void LoadCityDropdown()
        {
            Dictionary<string, int> cities = weatherService.GetCityList();

            // Şehir isimlerini List'e çevir ve sırala
            List<string> cityNames = new List<string>();
            foreach (string cityName in cities.Keys)
            {
                cityNames.Add(cityName);
            }
            cityNames.Sort(); // Alfabetik sıralama


            ddlCity.DataSource = cityNames;
            ddlCity.DataBind();

            ddlCity.SelectedValue = "Ankara";
        }


        private void LoadWeatherWidget(string cityName)
        {
            string weatherJson = weatherService.GetWeatherData(cityName);


            // JSON'u Hidden field'a yaz (JavaScript için)
            hfWeatherData.Value = weatherJson;
        }

        protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCity = ddlCity.SelectedValue;
            LoadWeatherWidget(selectedCity);
        }

        private void ProfilGetir()
        {
            // Session kontrolü
            if (Session["Kturu"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            lblPersonelAdi.Text = CurrentUserName;

            string sicilNo = Session["Sicil"].ToString();
            if (sicilNo.Length < 2)
            {
                ShowError("Geçersiz sicil numarası.");
                return;
            }
            sicilNo = sicilNo.Substring(2);
            string yil = DateTime.Now.Year.ToString();

            // Kullanılan izinleri sorgula (saniye cinsinden, ama labels'ta saat'e çevir)
            string query = @"
                SELECT 
                    yy.Sicil_No, 
                    ISNULL(yy.Toplam_Yillik, 0) as Toplam_Yillik,
                    ISNULL(rr.Toplam_Rapor, 0) as Toplam_Rapor, 
                    ISNULL(ii.Toplam_Saatlik, 0) as Toplam_Saatlik, 
                    ISNULL(mm.Toplam_Mazeret, 0) as Toplam_Mazeret 
                FROM (
                    SELECT y.Sicil_No, SUM(y.izin_Suresi) as Toplam_Yillik 
                    FROM personel_izin y 
                    WHERE Sicil_No = @SicilNo AND izin_turu = 'Yıllık İzin' AND YEAR(izne_Baslama_Tarihi) = @Yil 
                    GROUP BY Sicil_No
                ) yy 
                FULL JOIN (
                    SELECT r.Sicil_No, SUM(r.izin_Suresi) as Toplam_Rapor 
                    FROM personel_izin r 
                    WHERE Sicil_No = @SicilNo AND izin_turu = 'Rapor' AND YEAR(izne_Baslama_Tarihi) = @Yil 
                    GROUP BY Sicil_No
                ) rr ON yy.Sicil_No = rr.Sicil_No 
                FULL JOIN (
                    SELECT i.Sicil_No, SUM(i.izin_Suresi) as Toplam_Saatlik 
                    FROM personel_izin i 
                    WHERE Sicil_No = @SicilNo AND izin_turu = 'Saatlik izin' AND YEAR(izne_Baslama_Tarihi) = @Yil  
                    GROUP BY Sicil_No
                ) ii ON yy.Sicil_No = ii.Sicil_No 
                FULL JOIN (
                    SELECT m.Sicil_No, SUM(m.izin_Suresi) as Toplam_Mazeret 
                    FROM personel_izin m 
                    WHERE Sicil_No = @SicilNo AND izin_turu = 'Mazeret İzni' AND YEAR(izne_Baslama_Tarihi) = @Yil  
                    GROUP BY Sicil_No
                ) mm ON yy.Sicil_No = mm.Sicil_No 
                WHERE yy.Sicil_No = @SicilNo OR rr.Sicil_No = @SicilNo OR ii.Sicil_No = @SicilNo OR mm.Sicil_No = @SicilNo";

            var parameters = CreateParameters(
                ("@SicilNo", sicilNo),
                ("@Yil", yil)
            );

            DataTable dtIzinler = ExecuteDataTable(query, parameters);
            grdKullanilanIzinler.DataSource = dtIzinler;
            grdKullanilanIzinler.DataBind();

            // Saat'e çevir ve formatla (saniye / 3600, trailing zeros kaldır)
            if (grdKullanilanIzinler.Rows.Count > 0)
            {
                lblYillikIzin.Text = FormatDuration(grdKullanilanIzinler.Rows[0].Cells[1].Text ?? "0");
                lblRaporIzin.Text = FormatDuration(grdKullanilanIzinler.Rows[0].Cells[2].Text ?? "0");
                lblSaatlikIzin.Text = FormatDuration(grdKullanilanIzinler.Rows[0].Cells[3].Text ?? "0");
                lblMazeretIzin.Text = FormatDuration(grdKullanilanIzinler.Rows[0].Cells[4].Text ?? "0");
            }
            else
            {
                lblYillikIzin.Text = "0";
                lblRaporIzin.Text = "0";
                lblSaatlikIzin.Text = "0";
                lblMazeretIzin.Text = "0";
            }
        }

        private string FormatDuration(string input)
        {
            if (double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out double seconds))
            {
                double hours = seconds / 3600.0;
                return hours.ToString("F1", CultureInfo.CurrentCulture).TrimEnd('0').TrimEnd('.'); // F1 for 1 decimal, remove trailing 0/decimal
            }
            return "0";
        }

        public string FormatDurationForGrid(object value)
        {
            if (value != null && double.TryParse(value.ToString(), out double seconds))
            {
                double hours = seconds / 3600.0;
                return hours.ToString("F1", CultureInfo.CurrentCulture).TrimEnd('0').TrimEnd('.');
            }
            return "0";
        }

        private void DuyurularDoldur()
        {
            string query = @"
                SELECT * FROM duyuru 
                WHERE Durum = 'Aktif' 
                AND Baslama_Tarihi <= @Bugun 
                AND Bitis_Tarihi >= @Bugun 
                ORDER BY id DESC";

            var parameters = CreateParameters(
                ("@Bugun", DateTime.Now.Date)
            );

            DataTable dtDuyurular = ExecuteDataTable(query, parameters);
            rptDuyurular.DataSource = dtDuyurular;
            rptDuyurular.DataBind();

            if (dtDuyurular.Rows.Count == 0)
            {
                lblNoDuyuru.Visible = true;
            }
        }

        private void DogumGunleriDoldur()
        {
            string query = "SELECT Adi, Soyad FROM personel WHERE DAY(DogumTarihi) = DAY(GETDATE()) AND MONTH(DogumTarihi) = MONTH(GETDATE())";

            DataTable dtDogum = ExecuteDataTable(query);
            rptDogumGunleri.DataSource = dtDogum;
            rptDogumGunleri.DataBind();

            if (dtDogum.Rows.Count == 0)
            {
                lblNoDogumGunu.Visible = true;
            }
        }

        private void DenetimGorevleriDoldur()
        {
            string query = @"
                SELECT * FROM yolluk 
                WHERE BaslamaTarihi <= @Bugun 
                AND BitisTarihi >= @Bugun 
                ORDER BY il ASC";

            var parameters = CreateParameters(
                ("@Bugun", DateTime.Now.Date)
            );

            DataTable dtDenetim = ExecuteDataTable(query, parameters);
            grdDenetimGorevleri.DataSource = dtDenetim;
            grdDenetimGorevleri.DataBind();

            if (dtDenetim.Rows.Count > 0)
            {
                grdDenetimGorevleri.ShowFooter = true;
                grdDenetimGorevleri.FooterRow.Cells[0].Text = $"Toplam Aktif Görev: {dtDenetim.Rows.Count}";
                grdDenetimGorevleri.FooterRow.Cells[0].ColumnSpan = grdDenetimGorevleri.Columns.Count;
                grdDenetimGorevleri.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                grdDenetimGorevleri.FooterRow.Cells[0].CssClass = "bg-light font-weight-bold py-2 neutral-footer";
                for (int i = 1; i < grdDenetimGorevleri.FooterRow.Cells.Count; i++)
                {
                    grdDenetimGorevleri.FooterRow.Cells[i].Visible = false;
                }
            }
        }

        private void AktifIzinlerDoldur()
        {
            string query = @"
                SELECT Adi_Soyadi, izin_turu, izin_Suresi, Goreve_Baslama_Tarihi 
                FROM personel_izin  
                WHERE izne_Baslama_Tarihi <= @Bugun 
                AND izin_Bitis_Tarihi >= @Bugun 
                ORDER BY Adi_Soyadi ASC";

            var parameters = CreateParameters(
                ("@Bugun", DateTime.Now.Date)
            );

            DataTable dtIzinler = ExecuteDataTable(query, parameters);
            grdAktifIzinler.DataSource = dtIzinler;
            grdAktifIzinler.DataBind();

            int izinlisayi = dtIzinler.Rows.Count;
            if (izinlisayi > 0)
            {
                grdAktifIzinler.ShowFooter = true;
                grdAktifIzinler.FooterRow.Cells[0].Text = "Toplam";
                grdAktifIzinler.FooterRow.Cells[1].Text = izinlisayi.ToString();
                grdAktifIzinler.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;
            }
        }
    }
}