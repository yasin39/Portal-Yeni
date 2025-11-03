using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Base;

namespace Portal.ModulTehlikeliMadde
{
    public partial class FirmaKayit : BasePage
    {
        

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckPermission(Sabitler.TEHLIKELI_MADDE_KAYIT))
            {
                Response.Redirect("~/Anasayfa.aspx");
                return;
            }

            if (!IsPostBack)
            {
                DropDownlariYukle();
                FirmalariListele();
            }
        }

        #endregion

        #region Dropdown Yükleme Metodları

        private void DropDownlariYukle()
        {
            SehirleriYukle();
            YetkiBelgeleriYukle();
            FaaliyetAlanlariYukle();
        }

        private void SehirleriYukle()
        {
            Helpers.LoadProvinces(ddlIl, "Seçiniz...");
        }

        private void IlceleriYukle(string ilAdi)
        {
            if (string.IsNullOrEmpty(ilAdi))
            {
                ddlIlce.Items.Clear();
                ddlIlce.Items.Add(new ListItem("Seçiniz...", ""));
                return;
            }

            Helpers.LoadDistricts(ddlIlce, ilAdi, "Seçiniz...");
        }

        private void YetkiBelgeleriYukle()
        {
            string sorgu = "SELECT Belge_Adi FROM yetki_belgeleri ORDER BY Belge_Adi ASC";

            using (SqlConnection baglanti = GetConnection())
            {
                baglanti.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sorgu, baglanti);
                DataTable tablo = new DataTable();
                adapter.Fill(tablo);

                ddlYetkiBelgesi.Items.Clear();
                ddlYetkiBelgesi.Items.Add(new ListItem("Seçiniz...", ""));

                foreach (DataRow satir in tablo.Rows)
                {
                    ddlYetkiBelgesi.Items.Add(new ListItem(satir["Belge_Adi"].ToString(), satir["Belge_Adi"].ToString()));
                }
            }

            ddlBelgeNo.Items.Clear();
            ddlBelgeNo.Items.Add(new ListItem("Seçiniz...", ""));
        }

        private void FaaliyetAlanlariYukle()
        {
            string sorgu = "SELECT FaaliyetAdi FROM tmfaaliyetalanlari ORDER BY FaaliyetAdi ASC";

            using (SqlConnection baglanti = GetConnection())
            {
                baglanti.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sorgu, baglanti);
                DataTable tablo = new DataTable();
                adapter.Fill(tablo);

                ddlFaaliyetAlani.Items.Clear();
                ddlFaaliyetAlani.Items.Add(new ListItem("Seçiniz...", ""));

                foreach (DataRow satir in tablo.Rows)
                {
                    ddlFaaliyetAlani.Items.Add(new ListItem(satir["FaaliyetAdi"].ToString(), satir["FaaliyetAdi"].ToString()));
                }
            }
        }

        #endregion

        #region GridView Metodları

        private void FirmalariListele()
        {
            try
            {
                string sorgu = @"SELECT TOP 20 id, Unet, Unvan, Adres, il, ilce, BelgeTuru, BelgeSeriNo, 
                                Statu, FaaliyetTuru, Durum, Aciklama, KayitKullanici, KayitTarihi, 
                                GuncelleyenKullanici, GuncellemeTarihi 
                                FROM tmistatistik 
                                ORDER BY id DESC";

                using (SqlConnection baglanti = GetConnection())
                {
                    baglanti.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(sorgu, baglanti);
                    DataTable tablo = new DataTable();
                    adapter.Fill(tablo);

                    gvFirmalar.DataSource = tablo;
                    gvFirmalar.DataBind();

                    lblKayitSayisi.Text = tablo.Rows.Count + " kayıt";
                }
            }
            catch (Exception ex)
            {
                LogError("Firma listesi yüklenirken hata", ex);
                ShowToast("Firma listesi yüklenirken hata oluştu.", "danger");
            }
        }

        protected void gvFirmalar_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvFirmalar.PageIndex = e.NewPageIndex;
                FirmalariListele();
            }
            catch (Exception ex)
            {
                LogError("Sayfa değiştirme hatası", ex);
                ShowToast("Sayfa değiştirme sırasında hata oluştu.", "danger");
            }
        }

        #endregion

        #region Button Click Events

        protected void btnAra_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKayitNo.Text))
            {
                ShowToast("Lütfen kayıt numarası giriniz.", "warning");
                return;
            }

            KayitBul(Convert.ToInt32(txtKayitNo.Text));
        }

        protected void btnYeniKayit_Click(object sender, EventArgs e)
        {
            FormuTemizle();
            ShowToast("Yeni kayıt için formu doldurunuz.", "info");
        }

        protected void btnKaydet_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            if (BelgeNoKontrol(ddlBelgeNo.SelectedValue, 0))
            {
                lblBelgeUyari.Text = "Bu belge numarası ile kayıt zaten mevcut!";
                lblBelgeUyari.Visible = true;
                ShowToast("Bu belge numarası ile kayıt zaten eklenmiş.", "warning");
                return;
            }

            try
            {
                string sorgu = @"INSERT INTO tmistatistik 
                                (Unet, Unvan, Adres, ilce, il, Statu, BelgeTuru, BelgeSeriNo, 
                                FaaliyetTuru, Durum, Aciklama, KayitTarihi, KayitKullanici) 
                                VALUES 
                                (@Unet, @Unvan, @Adres, @Ilce, @Il, @Statu, @BelgeTuru, @BelgeSeriNo, 
                                @FaaliyetTuru, @Durum, @Aciklama, @KayitTarihi, @KayitKullanici)";

                var parametreler = CreateParameters(
                    ("@Unet", txtUnetNo.Text),
                    ("@Unvan", txtUnvan.Text),
                    ("@Adres", txtAdres.Text),
                    ("@Ilce", ddlIlce.SelectedValue),
                    ("@Il", ddlIl.SelectedValue),
                    ("@Statu", ddlStatu.SelectedValue),
                    ("@BelgeTuru", ddlYetkiBelgesi.SelectedValue),
                    ("@BelgeSeriNo", ddlBelgeNo.SelectedValue),
                    ("@FaaliyetTuru", ddlFaaliyetAlani.SelectedValue),
                    ("@Durum", ddlBelgeDurumu.SelectedValue),
                    ("@Aciklama", txtAciklama.Text),
                    ("@KayitTarihi", DateTime.Now),
                    ("@KayitKullanici",CurrentUserName)
                );

                int sonuc = ExecuteNonQuery(sorgu, parametreler);

                if (sonuc > 0)
                {
                    ShowToast("Kayıt başarıyla eklendi.", "success");
                    LogInfo($"Yeni firma kaydı eklendi: {txtUnvan.Text}");
                    FormuTemizle();
                    FirmalariListele();
                }
                else
                {
                    ShowToast("Kayıt eklenirken bir sorun oluştu.", "danger");
                }
            }
            catch (Exception ex)
            {
                LogError("Kayıt eklerken hata", ex);
                ShowToast("Kayıt eklenirken hata oluştu.", "danger");
            }
        }

        protected void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            int secilenID = Convert.ToInt32(hfSecilenKayitID.Value); // ✅ DEĞIŞIKLIK

            if (secilenID == 0)
            {
                ShowToast("Güncellenecek kayıt bulunamadı.", "warning");
                return;
            }

            if (BelgeNoKontrol(ddlBelgeNo.SelectedValue, secilenID))
            {
                lblBelgeUyari.Text = "Bu belge numarası başka bir kayıtta kullanılıyor!";
                lblBelgeUyari.Visible = true;
                ShowToast("Bu belge numarası başka bir kayıtta kullanılıyor.", "warning");
                return;
            }

            try
            {
                string sorgu = @"UPDATE tmistatistik SET 
                                Unet=@Unet, Unvan=@Unvan, Adres=@Adres, ilce=@Ilce, il=@Il, 
                                Statu=@Statu, BelgeTuru=@BelgeTuru, BelgeSeriNo=@BelgeSeriNo, 
                                FaaliyetTuru=@FaaliyetTuru, Durum=@Durum, Aciklama=@Aciklama, 
                                GuncellemeTarihi=@GuncellemeTarihi, GuncelleyenKullanici=@GuncelleyenKullanici 
                                WHERE id=@ID";

                var parametreler = CreateParameters(
                    ("@Unet", txtUnetNo.Text),
                    ("@Unvan", txtUnvan.Text),
                    ("@Adres", txtAdres.Text),
                    ("@Ilce", ddlIlce.SelectedValue),
                    ("@Il", ddlIl.SelectedValue),
                    ("@Statu", ddlStatu.SelectedValue),
                    ("@BelgeTuru", ddlYetkiBelgesi.SelectedValue),
                    ("@BelgeSeriNo", ddlBelgeNo.SelectedValue),
                    ("@FaaliyetTuru", ddlFaaliyetAlani.SelectedValue),
                    ("@Durum", ddlBelgeDurumu.SelectedValue),
                    ("@Aciklama", txtAciklama.Text),
                    ("@GuncellemeTarihi", DateTime.Now),
                    ("@GuncelleyenKullanici", CurrentUserName),
                    ("@ID", secilenID)
                );

                int sonuc = ExecuteNonQuery(sorgu, parametreler);

                if (sonuc > 0)
                {
                    ShowToast("Kayıt başarıyla güncellendi.", "success");
                    LogInfo($"Firma kaydı güncellendi: {txtUnvan.Text} (ID: {secilenID})");
                    FormuTemizle();
                    FirmalariListele();
                }
                else
                {
                    ShowToast("Kayıt güncellenirken bir sorun oluştu.", "danger");
                }
            }
            catch (Exception ex)
            {
                LogError("Kayıt güncellerken hata", ex);
                ShowToast("Kayıt güncellenirken hata oluştu.", "danger");
            }
        }

        protected void btnVazgec_Click(object sender, EventArgs e)
        {
            FormuTemizle();
            ShowToast("İşlemden vazgeçildi.", "info");
        }

        protected void btnExcelAktar_Click(object sender, EventArgs e)
        {
            if (gvFirmalar.Rows.Count == 0)
            {
                ShowToast("Export edilecek veri bulunamadı.", "warning");
                return;
            }

            try
            {
                ExportGridViewToExcel(gvFirmalar, "TehlikeliMaddeFirmalar_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                LogInfo("Tehlikeli madde firmaları Excel'e aktarıldı.");
            }
            catch (Exception ex)
            {
                LogError("Excel export hatası", ex);
                ShowToast("Excel dosyası oluşturulurken hata oluştu.", "danger");
            }
        }

        #endregion

        #region DropDown Change Events

        protected void ddlIl_SelectedIndexChanged(object sender, EventArgs e)
        {
            IlceleriYukle(ddlIl.SelectedValue);
        }

        protected void ddlBelgeNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlBelgeNo.SelectedValue))
            {
                lblBelgeUyari.Visible = false;
                return;
            }

            int secilenID = Convert.ToInt32(hfSecilenKayitID.Value); // ✅ DEĞIŞIKLIK

            if (BelgeNoKontrol(ddlBelgeNo.SelectedValue, secilenID))
            {
                lblBelgeUyari.Text = "Bu belge numarası ile kayıt zaten mevcut!";
                lblBelgeUyari.Visible = true;
            }
            else
            {
                lblBelgeUyari.Visible = false;
            }
        }

        #endregion

        #region Helper Methods

        private void KayitBul(int kayitID)
        {
            try
            {
                string sorgu = @"SELECT * FROM tmistatistik WHERE id=@ID";
                var parametreler = CreateParameters(("@ID", kayitID));

                using (SqlConnection baglanti = GetConnection())
                {
                    baglanti.Open();
                    SqlCommand komut = new SqlCommand(sorgu, baglanti);
                    komut.Parameters.AddRange(parametreler.ToArray());

                    SqlDataReader okuyucu = komut.ExecuteReader();

                    if (okuyucu.Read())
                    {
                        hfSecilenKayitID.Value = kayitID.ToString(); // ✅ DEĞIŞIKLIK

                        txtUnetNo.Text = okuyucu["Unet"].ToString();
                        txtUnvan.Text = okuyucu["Unvan"].ToString();
                        txtAdres.Text = okuyucu["Adres"].ToString();
                        SetSafeDropDownValue(ddlYetkiBelgesi, okuyucu["BelgeTuru"].ToString());

                        BelgeNolariniYukle();
                        SetSafeDropDownValue(ddlBelgeNo, okuyucu["BelgeSeriNo"].ToString());

                        SetSafeDropDownValue(ddlIl, okuyucu["il"].ToString());
                        IlceleriYukle(okuyucu["il"].ToString());
                        SetSafeDropDownValue(ddlIlce, okuyucu["ilce"].ToString());

                        SetSafeDropDownValue(ddlStatu, okuyucu["Statu"].ToString());
                        SetSafeDropDownValue(ddlFaaliyetAlani, okuyucu["FaaliyetTuru"].ToString());
                        SetSafeDropDownValue(ddlBelgeDurumu, okuyucu["Durum"].ToString());
                        txtAciklama.Text = okuyucu["Aciklama"].ToString();

                        btnKaydet.Visible = false;
                        btnGuncelle.Visible = true;
                        btnVazgec.Visible = true;

                        ShowToast("Kayıt bulundu ve forma yüklendi.", "success");
                    }
                    else
                    {
                        ShowToast("Kayıt bulunamadı.", "warning");
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("Kayıt bulma hatası", ex);
                ShowToast("Kayıt bulunurken hata oluştu.", "danger");
            }
        }

        private void BelgeNolariniYukle()
        {
            string sorgu = "SELECT DISTINCT BelgeSeriNo FROM tmistatistik WHERE BelgeSeriNo IS NOT NULL AND BelgeSeriNo != '' ORDER BY BelgeSeriNo ASC";

            using (SqlConnection baglanti = GetConnection())
            {
                baglanti.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sorgu, baglanti);
                DataTable tablo = new DataTable();
                adapter.Fill(tablo);

                ddlBelgeNo.Items.Clear();
                ddlBelgeNo.Items.Add(new ListItem("Seçiniz...", ""));

                foreach (DataRow satir in tablo.Rows)
                {
                    ddlBelgeNo.Items.Add(new ListItem(satir["BelgeSeriNo"].ToString(), satir["BelgeSeriNo"].ToString()));
                }
            }
        }

        private bool BelgeNoKontrol(string belgeNo, int mevcutKayitID)
        {
            if (string.IsNullOrEmpty(belgeNo)) return false;

            try
            {
                string sorgu = @"SELECT COUNT(*) FROM tmistatistik 
                                WHERE BelgeSeriNo=@BelgeNo AND id!=@ID";
                var parametreler = CreateParameters(
                    ("@BelgeNo", belgeNo),
                    ("@ID", mevcutKayitID)
                );

                using (SqlConnection baglanti = GetConnection())
                {
                    baglanti.Open();
                    SqlCommand komut = new SqlCommand(sorgu, baglanti);
                    komut.Parameters.AddRange(parametreler.ToArray());

                    int kayitSayisi = Convert.ToInt32(komut.ExecuteScalar());
                    return kayitSayisi > 0;
                }
            }
            catch (Exception ex)
            {
                LogError("Belge no kontrol hatası", ex);
                return false;
            }
        }

        private void FormuTemizle()
        {
            ClearFormControls(txtKayitNo, txtUnetNo, txtUnvan, txtAdres, txtAciklama,
                             ddlYetkiBelgesi, ddlIl, ddlStatu, ddlFaaliyetAlani, ddlBelgeDurumu);

            ddlBelgeNo.Items.Clear();
            ddlBelgeNo.Items.Add(new ListItem("Seçiniz...", ""));
            ddlIlce.Items.Clear();
            ddlIlce.Items.Add(new ListItem("Seçiniz...", ""));

            lblBelgeUyari.Visible = false;

            btnKaydet.Visible = true;
            btnGuncelle.Visible = false;
            btnVazgec.Visible = false;

            hfSecilenKayitID.Value = "0"; // ✅ DEĞIŞIKLIK
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
        }

        #endregion
    }
}