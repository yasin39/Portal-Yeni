using Portal.Base;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Portal.ModulPersonel
{
    public partial class GenelIzin : BasePage
    {
        private string SecilenYil
        {
            get { return DdlYil.SelectedValue; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(Sabitler.Personel))
                {
                    return;
                }

                YillariYukle();
                PersonelIzinleriniYukle();
            }
        }

        private void YillariYukle()
        {
            int MevcutYil = DateTime.Now.Year;
            DdlYil.Items.Clear();

            for (int i = MevcutYil - 2; i <= MevcutYil + 1; i++)
            {
                ListItem item = new ListItem(i.ToString(), i.ToString());
                if (i == MevcutYil)
                {
                    item.Selected = true;
                }
                DdlYil.Items.Add(item);
            }
        }

        private void PersonelIzinleriniYukle(string AramaMetni = "", string IzinTuru = "")
        {
            try
            {
                string Query = BuildQueryWithFilters(AramaMetni, IzinTuru);
                var Parametreler = CreateParameters(("@Yil", SecilenYil));

                if (!string.IsNullOrEmpty(AramaMetni))
                {
                    Parametreler.Add(CreateParameter("@Arama", "%" + AramaMetni + "%"));
                }

                DataTable PersonelVerileri = ExecuteDataTable(Query, Parametreler);

                PersonelIzinGrid.DataSource = PersonelVerileri;
                PersonelIzinGrid.DataBind();

                KayitSayisiniGuncelle(PersonelVerileri.Rows.Count);

                if (PersonelVerileri.Rows.Count == 0)
                {
                    ShowToast("Arama kriterlerine uygun kayıt bulunamadı.", "info");
                }
            }
            catch (Exception ex)
            {
                LogError("Personel izinleri yüklenirken hata", ex);
                ShowToast("Veriler yüklenirken bir hata oluştu.", "danger");
            }
        }

        private string BuildQueryWithFilters(string AramaMetni, string IzinTuru)
        {
            string BaseQuery = @"
                SELECT 
                    pp.Resim,
                    pp.SicilNo,
                    pp.Adi,
                    pp.Soyad,
                    ISNULL(rr.Toplam_Rapor, 0) as Toplam_Rapor,
                    ISNULL(ii.Toplam_Saatlik, 0) as Toplam_Saatlik,
                    ISNULL(mm.Toplam_Mazeret, 0) as Toplam_Mazeret,
                    ISNULL(hh.Toplam_Hastane, 0) as Toplam_Hastane,
                    ISNULL(yy.Toplam_Yillik, 0) as Toplam_Yillik,
                    ISNULL(aa.Toplam, 0) as Toplam,
                    ISNULL(pp.Devredenizin, 0) as Devredenizin,
                    ISNULL(pp.cariyilizni, 0) as cariyilizni,
                    ISNULL(pp.toplamizin, 0) as Kalanizin
                FROM 
                    (SELECT p.SicilNo, p.Resim, p.Adi, p.Soyad, 
                            p.Devredenizin, p.cariyilizni, p.toplamizin 
                     FROM personel p 
                     WHERE p.Durum = 'Aktif') pp
                LEFT JOIN 
                    (SELECT Sicil_No, SUM(izin_Suresi) as Toplam 
                     FROM personel_izin 
                     WHERE YEAR(izne_Baslama_Tarihi) = @Yil 
                     GROUP BY Sicil_No) aa ON pp.SicilNo = aa.Sicil_No
                LEFT JOIN 
                    (SELECT Sicil_No, SUM(izin_Suresi) as Toplam_Yillik 
                     FROM personel_izin 
                     WHERE YEAR(izne_Baslama_Tarihi) = @Yil 
                           AND izin_turu = 'Yıllık İzin' 
                     GROUP BY Sicil_No) yy ON pp.SicilNo = yy.Sicil_No
                LEFT JOIN 
                    (SELECT Sicil_No, SUM(izin_Suresi) as Toplam_Rapor 
                     FROM personel_izin 
                     WHERE YEAR(izne_Baslama_Tarihi) = @Yil 
                           AND izin_turu = 'Rapor' 
                     GROUP BY Sicil_No) rr ON pp.SicilNo = rr.Sicil_No
                LEFT JOIN 
                    (SELECT Sicil_No, SUM(izin_Suresi) as Toplam_Saatlik 
                     FROM personel_izin 
                     WHERE YEAR(izne_Baslama_Tarihi) = @Yil 
                           AND izin_turu = 'Saatlik izin' 
                     GROUP BY Sicil_No) ii ON pp.SicilNo = ii.Sicil_No
                LEFT JOIN 
                    (SELECT Sicil_No, SUM(izin_Suresi) as Toplam_Mazeret 
                     FROM personel_izin 
                     WHERE YEAR(izne_Baslama_Tarihi) = @Yil 
                           AND izin_turu = 'Mazeret İzni' 
                     GROUP BY Sicil_No) mm ON pp.SicilNo = mm.Sicil_No
                LEFT JOIN 
                    (SELECT Sicil_No, SUM(izin_Suresi) as Toplam_Hastane 
                     FROM personel_izin 
                     WHERE YEAR(izne_Baslama_Tarihi) = @Yil 
                           AND izin_turu = 'Hastane İzni' 
                     GROUP BY Sicil_No) hh ON pp.SicilNo = hh.Sicil_No";

            string WhereClause = " WHERE 1=1";

            if (!string.IsNullOrEmpty(AramaMetni))
            {
                WhereClause += @" AND (pp.Adi LIKE @Arama 
                                      OR pp.Soyad LIKE @Arama 
                                      OR pp.SicilNo LIKE @Arama 
                                      OR (pp.Adi + ' ' + pp.Soyad) LIKE @Arama)";
            }

            if (!string.IsNullOrEmpty(IzinTuru))
            {
                switch (IzinTuru)
                {
                    case "Yıllık İzin":
                        WhereClause += " AND yy.Toplam_Yillik > 0";
                        break;
                    case "Rapor":
                        WhereClause += " AND rr.Toplam_Rapor > 0";
                        break;
                    case "Saatlik izin":
                        WhereClause += " AND ii.Toplam_Saatlik > 0";
                        break;
                    case "Mazeret İzni":
                        WhereClause += " AND mm.Toplam_Mazeret > 0";
                        break;
                    case "Hastane İzni":
                        WhereClause += " AND hh.Toplam_Hastane > 0";
                        break;
                }
            }

            return BaseQuery + WhereClause + " ORDER BY pp.Adi, pp.Soyad ASC";
        }

        private void KayitSayisiniGuncelle(int KayitSayisi)
        {
            if (KayitSayisi > 0)
            {
                LblKayitSayisi.Text = $"{KayitSayisi} Kayıt";
                LblKayitSayisi.CssClass = "badge bg-primary fs-6";
            }
            else
            {
                LblKayitSayisi.Text = "Kayıt Yok";
                LblKayitSayisi.CssClass = "badge bg-secondary fs-6";
            }
        }

        protected void DdlYil_SelectedIndexChanged(object sender, EventArgs e)
        {
            string AramaMetni = TxtArama.Text.Trim();
            string IzinTuru = DdlIzinTuru.SelectedValue;
            PersonelIzinleriniYukle(AramaMetni, IzinTuru);
        }

        protected void BtnAra_Click(object sender, EventArgs e)
        {
            string AramaMetni = TxtArama.Text.Trim();
            string IzinTuru = DdlIzinTuru.SelectedValue;

            if (string.IsNullOrEmpty(AramaMetni) && string.IsNullOrEmpty(IzinTuru))
            {
                ShowToast("Lütfen arama metni girin veya izin türü seçin.", "warning");
                return;
            }

            PersonelIzinleriniYukle(AramaMetni, IzinTuru);
        }

        protected void BtnTumunuListele_Click(object sender, EventArgs e)
        {
            TxtArama.Text = string.Empty;
            DdlIzinTuru.SelectedIndex = 0;
            PersonelIzinleriniYukle();
            ShowToast("Tüm kayıtlar listelendi.", "success");
        }

        protected void BtnExcelAktar_Click(object sender, EventArgs e)
        {
            if (PersonelIzinGrid.Rows.Count == 0)
            {
                ShowToast("Excel'e aktarılacak veri bulunamadı.", "warning");
                return;
            }

            try
            {
                string DosyaAdi = $"PersonelIzinRaporu_{SecilenYil}_{DateTime.Now:yyyyMMdd_HHmmss}.xls";
                ExportGridViewToExcel(PersonelIzinGrid, DosyaAdi);
                LogInfo($"Personel izin raporu Excel'e aktarıldı: {DosyaAdi}");
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
    }
}