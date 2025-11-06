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
            DoldurDropDownlar();
        }

        //** Tüm dropdown'ları doldurma işlemi tek metoda alındı ve tekrar kontrolü eklendi
        private void DoldurDropDownlar()
        {
            // Unvan
            string query = "SELECT Unvan FROM personel_unvan ORDER BY Unvan ASC";
            PopulateDropDownList(ddlUnvan, query, "Unvan", "Unvan", false);
            PopulateDropDownList(ddlModalUnvan, query, "Unvan", "Unvan", true); //** Modal için de doldur

            // Mesleki Unvan (DISTINCT ile)
            query = "SELECT DISTINCT ISNULL(MeslekiUnvan, '') AS MeslekiUnvan FROM personel WHERE MeslekiUnvan IS NOT NULL ORDER BY MeslekiUnvan ASC";
            PopulateDropDownList(ddlMeslekiUnvan, query, "MeslekiUnvan", "MeslekiUnvan", false);

            // Sendika
            query = "SELECT Sendika_Adi FROM personel_sendika ORDER BY Sendika_Adi ASC";
            PopulateDropDownList(ddlSendika, query, "Sendika_Adi", "Sendika_Adi", false);

            // Birim
            query = "SELECT Sube_Adi FROM subeler ORDER BY Sube_Adi ASC";
            PopulateDropDownList(ddlBirim, query, "Sube_Adi", "Sube_Adi", false);

            //** Statik dropdown'ları doldur - TEKRAR KONTROLÜ EKLENDI
            // Statü
            if (ddlStatu.Items.Count <= 1) //** "Hepsi" zaten var, diğerleri yoksa ekle
            {
                ddlStatu.Items.Add(new ListItem("Memur"));
                ddlStatu.Items.Add(new ListItem("İşçi"));
                ddlStatu.Items.Add(new ListItem(Sabitler.FirmaPersoneli));
                ddlStatu.Items.Add(new ListItem(Sabitler.IskurIsciTYP));
                ddlStatu.Items.Add(new ListItem("Sözleşmeli Personel (4-B)"));
                ddlStatu.Items.Add(new ListItem("İşçi (375 KHK)"));
            }

            // Çalışma Durumu
            if (ddlCalismaDurumu.Items.Count <= 1) //** Tekrar kontrolü
            {
                ddlCalismaDurumu.Items.Add(new ListItem(Sabitler.KadroluAktifCalisan));
                ddlCalismaDurumu.Items.Add(new ListItem(Sabitler.GeciciGorevliAktifCalisan));
                ddlCalismaDurumu.Items.Add(new ListItem(Sabitler.GeciciGorevdePasifCalisan));
                ddlCalismaDurumu.Items.Add(new ListItem(Sabitler.FirmaPersoneli));
                ddlCalismaDurumu.Items.Add(new ListItem(Sabitler.IskurIsciTYP));
            }

            // Öğrenim
            if (ddlOgrenim.Items.Count <= 1) //** Tekrar kontrolü
            {
                ddlOgrenim.Items.Add(new ListItem("Lise"));
                ddlOgrenim.Items.Add(new ListItem("Ön Lisans"));
                ddlOgrenim.Items.Add(new ListItem("Lisans"));
                ddlOgrenim.Items.Add(new ListItem("Yüksek Lisans"));
                ddlOgrenim.Items.Add(new ListItem("Doktora"));
            }

            // Kan Grubu
            if (ddlKanGrubu.Items.Count <= 1) //** Tekrar kontrolü
            {
                ddlKanGrubu.Items.Add(new ListItem("0 Rh+"));
                ddlKanGrubu.Items.Add(new ListItem("0 Rh-"));
                ddlKanGrubu.Items.Add(new ListItem("A Rh+"));
                ddlKanGrubu.Items.Add(new ListItem("A Rh-"));
                ddlKanGrubu.Items.Add(new ListItem("B Rh+"));
                ddlKanGrubu.Items.Add(new ListItem("B Rh-"));
                ddlKanGrubu.Items.Add(new ListItem("AB Rh+"));
                ddlKanGrubu.Items.Add(new ListItem("AB Rh-"));
            }

            // Medeni Hal
            if (ddlMedeniHal.Items.Count <= 1) //** Tekrar kontrolü
            {
                ddlMedeniHal.Items.Add(new ListItem("Bekar"));
                ddlMedeniHal.Items.Add(new ListItem("Evli"));
                ddlMedeniHal.Items.Add(new ListItem("Boşanmış"));
                ddlMedeniHal.Items.Add(new ListItem("Dul"));
            }

            // Cinsiyet
            if (ddlCinsiyet.Items.Count <= 1) //** Tekrar kontrolü
            {
                ddlCinsiyet.Items.Add(new ListItem("Erkek"));
                ddlCinsiyet.Items.Add(new ListItem("Kadın"));
            }

            // Durum
            if (ddlDurum.Items.Count <= 1) //** Tekrar kontrolü
            {
                ddlDurum.Items.Add(new ListItem(Sabitler.Aktif));
                ddlDurum.Items.Add(new ListItem("Pasif"));
            }
        }

        //** GridView RowCommand eventi - Güncelle butonuna tıklandığında modal açılır
        protected void gvPersonelAra_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Guncelle")
            {
                int personelId = Convert.ToInt32(e.CommandArgument);
                PersonelBilgileriniYukle(personelId);
                ScriptManager.RegisterStartupScript(this, GetType(), "openModal", "openModal();", true);
            }
        }

        //** Seçilen personelin bilgilerini modal'a yükler
        private void PersonelBilgileriniYukle(int personelId)
        {
            try
            {
                string query = "SELECT * FROM personel WHERE Personelid = @Id";
                var parameters = new List<SqlParameter> { CreateParameter("@Id", personelId) };
                DataTable dt = ExecuteDataTable(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    hfPersonelId.Value = personelId.ToString();
                    txtModalTcNo.Text = row["TcKimlikNo"].ToString();
                    txtModalAdi.Text = row["Adi"].ToString();
                    txtModalSoyadi.Text = row["Soyad"].ToString();
                    txtModalSicilNo.Text = row["SicilNo"].ToString();

                    //** Dropdown seçimini ayarla
                    if (ddlModalUnvan.Items.FindByText(row["Unvan"].ToString()) != null)
                    {
                        ddlModalUnvan.SelectedValue = row["Unvan"].ToString();
                    }

                    txtModalMeslekiUnvan.Text = row["MeslekiUnvan"].ToString();
                    txtModalKadroDerece.Text = row["KadroDerece"].ToString();
                    txtModalCepTel.Text = row["CepTelefonu"].ToString();
                    txtModalMail.Text = row["MailAdresi"].ToString();
                    txtModalEvTel.Text = row["EvTelefonu"].ToString();
                    txtModalAdres.Text = row["Adres"].ToString();
                }
            }
            catch (Exception ex)
            {
                LogError("Personel bilgileri yükleme hatası", ex);
                ShowError("Personel bilgileri yüklenirken bir hata oluştu.");
            }
        }

        //** Modal'daki Kaydet butonuna tıklandığında güncelleme yapar
        protected void btnModalKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                int personelId = Convert.ToInt32(hfPersonelId.Value);

                string query = @"UPDATE personel SET 
                    TcKimlikNo = @TcKimlikNo,
                    Adi = @Adi,
                    Soyad = @Soyad,
                    SicilNo = @SicilNo,
                    Unvan = @Unvan,
                    MeslekiUnvan = @MeslekiUnvan,
                    KadroDerece = @KadroDerece,
                    CepTelefonu = @CepTelefonu,
                    MailAdresi = @MailAdresi,
                    EvTelefonu = @EvTelefonu,
                    Adres = @Adres
                    WHERE Personelid = @Id";

                var parameters = new List<SqlParameter>
                {
                    CreateParameter("@TcKimlikNo", txtModalTcNo.Text.Trim()),
                    CreateParameter("@Adi", txtModalAdi.Text.Trim()),
                    CreateParameter("@Soyad", txtModalSoyadi.Text.Trim()),
                    CreateParameter("@SicilNo", txtModalSicilNo.Text.Trim()),
                    CreateParameter("@Unvan", ddlModalUnvan.SelectedValue),
                    CreateParameter("@MeslekiUnvan", txtModalMeslekiUnvan.Text.Trim()),
                    CreateParameter("@KadroDerece", txtModalKadroDerece.Text.Trim()),
                    CreateParameter("@CepTelefonu", txtModalCepTel.Text.Trim()),
                    CreateParameter("@MailAdresi", txtModalMail.Text.Trim()),
                    CreateParameter("@EvTelefonu", txtModalEvTel.Text.Trim()),
                    CreateParameter("@Adres", txtModalAdres.Text.Trim()),
                    CreateParameter("@Id", personelId)
                };

                int result = ExecuteNonQuery(query, parameters);

                if (result > 0)
                {
                    LogInfo($"Personel güncellendi: ID={personelId}, Ad={txtModalAdi.Text} {txtModalSoyadi.Text}");
                    ShowToast("Personel bilgileri başarıyla güncellendi.","success");
                    AramaYap(); //** GridView'i yeniden yükle
                }
                else
                {
                    ShowToast("Güncelleme işlemi başarısız oldu.","danger");
                }
            }
            catch (Exception ex)
            {
                LogError("Personel güncelleme hatası", ex);
                ShowError("Güncelleme sırasında bir hata oluştu.");
            }
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
                ShowToast("Export edilecek veri bulunamadı.", "danger");
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