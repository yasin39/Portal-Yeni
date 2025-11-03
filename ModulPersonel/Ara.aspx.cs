using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Base;

namespace Portal.ModulPersonel
{
    public partial class Ara : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(Sabitler.Personel))
                {
                    return;
                }
                Doldur();
            }
        }

        private void Doldur()
        {
            // Aktif personeli yükle
            string query = "SELECT * FROM personel WHERE Durum = @Durum ORDER BY Adi ASC";
            var parameters = new List<SqlParameter> { CreateParameter("@Durum", Sabitler.Aktif) };
            DataTable dt = ExecuteDataTable(query, parameters);
            gvPersonelAra.DataSource = dt;
            gvPersonelAra.DataBind();

            // Toplam aktif personel sayısı
            query = "SELECT COUNT(*) FROM personel WHERE Durum = @Durum";
            parameters = new List<SqlParameter> { CreateParameter("@Durum", Sabitler.Aktif) };
            object result = ExecuteScalar(query, parameters);
            int toplamSayi = result != null ? Convert.ToInt32(result) : 0;
            lblToplamSayi.Text = toplamSayi.ToString();

            // Dropdown'ları doldur
            // Unvan
            query = "SELECT Unvan FROM personel_unvan ORDER BY Unvan ASC";
            PopulateDropDownList(ddlUnvan, query, "Unvan", "Unvan", false); // Hepsi zaten ekli

            // Mesleki Unvan (DISTINCT ile)
            query = "SELECT DISTINCT ISNULL(MeslekiUnvan, '') AS MeslekiUnvan FROM personel WHERE MeslekiUnvan IS NOT NULL ORDER BY MeslekiUnvan ASC";
            PopulateDropDownList(ddlMeslekiUnvan, query, "MeslekiUnvan", "MeslekiUnvan", false);

            // Sendika
            query = "SELECT Sendika_Adi FROM personel_sendika ORDER BY Sendika_Adi ASC";
            PopulateDropDownList(ddlSendika, query, "Sendika_Adi", "Sendika_Adi", false);

            // Birim
            query = "SELECT Sube_Adi FROM subeler ORDER BY Sube_Adi ASC";
            PopulateDropDownList(ddlBirim, query, "Sube_Adi", "Sube_Adi", false);

            // Statik dropdown'ları doldur
            ddlStatu.Items.Add(new ListItem("Memur"));
            ddlStatu.Items.Add(new ListItem("İşçi"));
            ddlStatu.Items.Add(new ListItem(Sabitler.FirmaPersoneli));
            ddlStatu.Items.Add(new ListItem(Sabitler.IskurIsciTYP));
            ddlStatu.Items.Add(new ListItem("Sözleşmeli Personel (4-B)"));
            ddlStatu.Items.Add(new ListItem("İşçi (375 KHK)"));

            ddlCalismaDurumu.Items.Add(new ListItem(Sabitler.KadroluAktifCalisan));
            ddlCalismaDurumu.Items.Add(new ListItem(Sabitler.GeciciGorevliAktifCalisan));
            ddlCalismaDurumu.Items.Add(new ListItem(Sabitler.GeciciGorevdePasifCalisan));
            ddlCalismaDurumu.Items.Add(new ListItem(Sabitler.FirmaPersoneli));
            ddlCalismaDurumu.Items.Add(new ListItem(Sabitler.IskurIsciTYP));

            ddlOgrenim.Items.Add(new ListItem("Lise"));
            ddlOgrenim.Items.Add(new ListItem("Ön Lisans"));
            ddlOgrenim.Items.Add(new ListItem("Lisans"));
            ddlOgrenim.Items.Add(new ListItem("Yüksek Lisans"));
            ddlOgrenim.Items.Add(new ListItem("Doktora"));

            ddlKanGrubu.Items.Add(new ListItem("0 Rh+"));
            ddlKanGrubu.Items.Add(new ListItem("0 Rh-"));
            ddlKanGrubu.Items.Add(new ListItem("A Rh+"));
            ddlKanGrubu.Items.Add(new ListItem("A Rh-"));
            ddlKanGrubu.Items.Add(new ListItem("B Rh+"));
            ddlKanGrubu.Items.Add(new ListItem("B Rh-"));
            ddlKanGrubu.Items.Add(new ListItem("AB Rh+"));
            ddlKanGrubu.Items.Add(new ListItem("AB Rh-"));

            ddlMedeniHal.Items.Add(new ListItem("Bekar"));
            ddlMedeniHal.Items.Add(new ListItem("Evli"));
            ddlMedeniHal.Items.Add(new ListItem("Boşanmış"));
            ddlMedeniHal.Items.Add(new ListItem("Dul"));

            ddlCinsiyet.Items.Add(new ListItem("Erkek"));
            ddlCinsiyet.Items.Add(new ListItem("Kadın"));

            ddlDurum.Items.Add(new ListItem(Sabitler.Aktif));
            ddlDurum.Items.Add(new ListItem("Pasif"));
        }

        protected void btnAra_Click(object sender, EventArgs e)
        {
            AramaYap();
        }

        private void AramaYap()
        {
            var parameters = new List<SqlParameter>();
            string query = "SELECT * FROM personel WHERE 1=1";

            if (!string.IsNullOrWhiteSpace(txtSicilNo.Text))
            {
                query += " AND SicilNo = @SicilNo";
                parameters.Add(CreateParameter("@SicilNo", txtSicilNo.Text.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(txtTcKimlikNo.Text))
            {
                query += " AND TcKimlikNo = @TcKimlikNo";
                parameters.Add(CreateParameter("@TcKimlikNo", txtTcKimlikNo.Text.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(txtAdi.Text))
            {
                query += " AND Adi LIKE @Adi";
                parameters.Add(CreateParameter("@Adi", "%" + txtAdi.Text.Trim() + "%"));
            }

            if (!string.IsNullOrWhiteSpace(txtSoyad.Text))
            {
                query += " AND Soyad LIKE @Soyad";
                parameters.Add(CreateParameter("@Soyad", "%" + txtSoyad.Text.Trim() + "%"));
            }

            if (ddlUnvan.SelectedValue != "Hepsi")
            {
                query += " AND Unvan = @Unvan";
                parameters.Add(CreateParameter("@Unvan", ddlUnvan.SelectedValue));
            }

            if (ddlMeslekiUnvan.SelectedValue != "Hepsi")
            {
                query += " AND MeslekiUnvan = @MeslekiUnvan";
                parameters.Add(CreateParameter("@MeslekiUnvan", ddlMeslekiUnvan.SelectedValue));
            }

            if (ddlStatu.SelectedValue != "Hepsi")
            {
                query += " AND Statu = @Statu";
                parameters.Add(CreateParameter("@Statu", ddlStatu.SelectedValue));
            }

            if (ddlCalismaDurumu.SelectedValue != "Hepsi")
            {
                query += " AND CalismaDurumu = @CalismaDurumu";
                parameters.Add(CreateParameter("@CalismaDurumu", ddlCalismaDurumu.SelectedValue));
            }

            if (ddlOgrenim.SelectedValue != "Hepsi")
            {
                query += " AND Ogrenim_Durumu = @Ogrenim";
                parameters.Add(CreateParameter("@Ogrenim", ddlOgrenim.SelectedValue));
            }

            if (ddlKanGrubu.SelectedValue != "Hepsi")
            {
                query += " AND KanGrubu = @KanGrubu";
                parameters.Add(CreateParameter("@KanGrubu", ddlKanGrubu.SelectedValue));
            }

            if (ddlMedeniHal.SelectedValue != "Hepsi")
            {
                query += " AND MedeniHali = @MedeniHali";
                parameters.Add(CreateParameter("@MedeniHali", ddlMedeniHal.SelectedValue));
            }

            if (ddlCinsiyet.SelectedValue != "Hepsi")
            {
                query += " AND Cinsiyet = @Cinsiyet";
                parameters.Add(CreateParameter("@Cinsiyet", ddlCinsiyet.SelectedValue));
            }

            if (ddlSendika.SelectedValue != "Hepsi")
            {
                query += " AND Sendika = @Sendika";
                parameters.Add(CreateParameter("@Sendika", ddlSendika.SelectedValue));
            }

            if (ddlBirim.SelectedValue != "Hepsi")
            {
                query += " AND GorevYaptigiBirim = @Birim";
                parameters.Add(CreateParameter("@Birim", ddlBirim.SelectedValue));
            }

            if (ddlDurum.SelectedValue != "Hepsi")
            {
                query += " AND Durum = @Durum";
                parameters.Add(CreateParameter("@Durum", ddlDurum.SelectedValue));
            }

            if (!string.IsNullOrWhiteSpace(txtTarih.Text))
            {
                query += " AND KurumaBaslamaTarihi <= @Tarih AND (istenAyrilisTarihi >= @Tarih OR istenAyrilisTarihi IS NULL)";
                parameters.Add(CreateParameter("@Tarih", DateTime.Parse(txtTarih.Text)));
            }

            query += " ORDER BY Adi ASC";

            try
            {
                DataTable dt = ExecuteDataTable(query, parameters);
                gvPersonelAra.DataSource = dt;
                gvPersonelAra.DataBind();

                int bulunanSayi = dt.Rows.Count;
                lblBulunanSayi.Text = bulunanSayi.ToString();

                LogInfo($"Personel arama tamamlandı: {bulunanSayi} kayıt bulundu.");
            }
            catch (Exception ex)
            {
                LogError("Arama hatası", ex);
                ShowError("Arama sırasında bir hata oluştu. Lütfen kriterleri kontrol ediniz.");
                lblBulunanSayi.Text = "0";
            }
        }

        protected void btnTemizle_Click(object sender, EventArgs e)
        {
            txtSicilNo.Text = string.Empty;
            txtTcKimlikNo.Text = string.Empty;
            txtAdi.Text = string.Empty;
            txtSoyad.Text = string.Empty;
            txtTarih.Text = string.Empty;

            ResetDropDownLists(ddlUnvan, ddlMeslekiUnvan, ddlStatu, ddlCalismaDurumu,
                              ddlOgrenim, ddlKanGrubu, ddlMedeniHal, ddlCinsiyet,
                              ddlSendika, ddlBirim, ddlDurum);

            lblBulunanSayi.Text = string.Empty;
            Doldur(); // Initial veriyi yeniden yükle
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (gvPersonelAra.Rows.Count == 0)
            {
                ShowToast("Export edilecek veri bulunamadı.","danger");
                return;
            }
            ExportGridViewToExcel(gvPersonelAra, "personel_ara_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            // GridView export için gerekli
        }
    }
}