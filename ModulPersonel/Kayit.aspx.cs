using Portal.Base;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Portal.ModulPersonel
{
    public partial class Kayit : BasePage
    {        
        private bool isUpdateMode = false;
        private string currentTcKimlikNo;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckPermission(Sabitler.Personel))
                return;

            if (!IsPostBack)
            {
                DoldurDropDownListleri();
                btnGuncelle.Visible = false; // Varsayılan olarak ekle modu
                ViewState["CurrentTcKimlikNo"] = string.Empty;  // ViewState kullan
            }
            else
            {
                currentTcKimlikNo = ViewState["CurrentTcKimlikNo"]?.ToString() ?? string.Empty;
            }
        }

        private void DoldurDropDownListleri()
        {
            // Ünvanlar
            string queryUnvan = "SELECT Unvan FROM personel_unvan ORDER BY Unvan ASC";
            PopulateDropDownList(ddlUnvan, queryUnvan, "Unvan", "Unvan", true);

            // Sendikalar
            string querySendika = "SELECT Sendika_Adi FROM personel_sendika ORDER BY Sendika_Adi ASC";
            ddlSendika.Items.Clear();
            PopulateDropDownList(ddlSendika, querySendika, "Sendika_Adi", "Sendika_Adi", true);

            // Kurumlar (Gelen/Giden)
            string queryKurum = "SELECT Kurum_Adi FROM personel_kurum ORDER BY Kurum_Adi ASC";
            ddlGeciciGelenKurum.Items.Clear();
            PopulateDropDownList(ddlGeciciGelenKurum, queryKurum, "Kurum_Adi", "Kurum_Adi", true);
            ddlGeciciGidenKurum.Items.Clear();
            PopulateDropDownList(ddlGeciciGidenKurum, queryKurum, "Kurum_Adi", "Kurum_Adi", true);

            // Birimler (Şubeler)
            string queryBirim = "SELECT Sube_Adi FROM subeler ORDER BY Sube_Adi ASC";
            ddlGorevYaptigiBirim.Items.Clear();
            PopulateDropDownList(ddlGorevYaptigiBirim, queryBirim, "Sube_Adi", "Sube_Adi", true);
        }

        protected void txtTcKimlikNo_TextChanged(object sender, EventArgs e)
        {
            
            string tc = txtTcKimlikNo.Text.Trim();
            if (!string.IsNullOrEmpty(tc) && tc.Length == 11 && IsNumeric(tc))
            {
                lblTcValidation.Text = "TC Kimlik No geçerli.";
                lblTcValidation.CssClass = "text-success small";               
            }
            else
            {
                lblTcValidation.Text = "TC Kimlik No 11 haneli olmalı.";
                lblTcValidation.CssClass = "text-danger small";
            }
        }

        private bool IsNumeric(string value)
        {
            return long.TryParse(value, out _);
        }
        
        protected void btnBilgileriGetir_Click(object sender, EventArgs e)
        {
            string tc = txtTcKimlikNo.Text.Trim();          


            if (string.IsNullOrEmpty(tc))
            {
                ShowToast("Lütfen TC Kimlik No giriniz.","warning");
                return;
            }

            string query = @"
                SELECT * FROM personel 
                WHERE TcKimlikNo = @TcKimlikNo";

            var parameters = CreateParameters(("@TcKimlikNo", tc));

            DataTable dt = ExecuteDataTable(query, parameters);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                // Alanları doldur
                txtAdi.Text = row[Sabitler.Adi].ToString();
                txtSoyad.Text = row[Sabitler.Soyad].ToString();
                txtDogumYeri.Text = row["DogumYeri"].ToString();
                txtDogumTarihi.Text = row["DogumTarihi"].ToString();               
                txtSicilNo.Text = row[Sabitler.SicilNo].ToString();                
                txtIlkIseGirisTarihi.Text = row["ilkisegiristarihi"].ToString();
                txtKurumaBaslamaTarihi.Text = row[Sabitler.KurumaBaslamaTarihi].ToString();               
                txtKadroDerece.Text = row["KadroDerece"].ToString();                
                txtMeslekiUnvan.Text = row[Sabitler.MeslekiUnvan].ToString();
                txtIstenAyrilisTarihi.Text = row[Sabitler.IstenAyrilisTarihi].ToString();
                txtIstenAyrilmaSebebi.Text = row["istenAyrilmaSebebi"].ToString();
                txtGGorevBaslangic.Text = row["GGorevBaslangicTarihi"].ToString();
                txtGGorevBitis.Text = row["GGorevBitisTarihi"].ToString();                
                txtDevredenIzin.Text = row[Sabitler.DevredenIzin].ToString();
                txtCariIzin.Text = row[Sabitler.CariIzin].ToString();
                txtCepTelefonu.Text = row["CepTelefonu"].ToString();
                txtMailAdresi.Text = row["MailAdresi"].ToString();
                txtEvTelefonu.Text = row["EvTelefonu"].ToString();
                txtDahiliTelefon.Text = row["Dahili"].ToString();
                txtAdres.Text = row["Adres"].ToString();
                txtAcilDurumKisi.Text = row["AcilDurumdaAranacakKisi"].ToString();
                txtAcilCep.Text = row["AcilCep"].ToString();                
                txtEmeklilikTarihi.Text = row["EmeklilikTarihi"].ToString();
                txtYaslilikAyligiTarihi.Text = row["YaslilikAylikTarihi"].ToString();
                txtEngelAciklama.Text = row["EngelAciklama"].ToString();
                txtEmeklilikAciklama.Text = row["EmeklilikAciklama"].ToString();

                // Güvenli Dropdown Setleme Fonksiyonu 
                // Normal olanlar (varsayılan "Seçiniz...")
                SetSafeDropDownValue(ddlCinsiyet, row[Sabitler.Cinsiyet]?.ToString());
                SetSafeDropDownValue(ddlDurum, row[Sabitler.Statu]?.ToString());
                SetSafeDropDownValue(ddlUnvan, row[Sabitler.Unvan]?.ToString());
                SetSafeDropDownValue(ddlGorevYaptigiBirim, row[Sabitler.GorevYaptigiBirim]?.ToString());
                SetSafeDropDownValue(ddlOgrenimDurumu, row[Sabitler.Ogrenim_Durumu]?.ToString());
                SetSafeDropDownValue(ddlGeciciGelenKurum, row["GeciciGelenPersonelKurumu"]?.ToString());
                SetSafeDropDownValue(ddlGeciciGidenKurum, row["GeciciGidenPersonelKurumu"]?.ToString());
                SetSafeDropDownValue(ddlKanGrubu, row[Sabitler.KanGrubu]?.ToString());
                SetSafeDropDownValue(ddlMedeniHali, row[Sabitler.MedeniHali]?.ToString());
                SetSafeDropDownValue(ddlAskerlikDurumu, row["AskerlikDurumu"]?.ToString());
                SetSafeDropDownValue(ddlSendika, row[Sabitler.Sendika]?.ToString());

                // Özel varsayılanlar
                SetSafeDropDownValue(ddlCalismaDurumu, row[Sabitler.Durum]?.ToString(), "Aktif");
                SetSafeDropDownValue(ddlEngelDurumu, row["EngelDurumu"]?.ToString(), "Yok");

                isUpdateMode = true;
                btnEkle.Visible = false;
                btnGuncelle.Visible = true;
                currentTcKimlikNo = tc;
                ViewState["CurrentTcKimlikNo"] = tc;  //  ViewState'e kaydet

                ShowToast("Personel bilgileri başarıyla getirildi.", "success");
                LogInfo($"Personel bilgileri getirildi: {txtAdi.Text} {txtSoyad.Text} (TC: {tc})");
            }
            else
            {
                isUpdateMode = false;
                ShowToast("Personel Bulunamadı", "warning");
                ShowToast("TC Kimlik No ile personel bulunamadı.","warning");
            }
        }

        protected void btnEkle_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                using (SqlConnection conn = GetOpenConnection())
                {
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            string query = @"
                                INSERT INTO personel (
                                    TcKimlikNo, Adi, Soyad, DogumYeri, DogumTarihi, Cinsiyet, Durum, SicilNo, 
                                    Unvan, Statu, ilkisegiristarihi, KurumaBaslamaTarihi, GorevYaptigiBirim, CalismaDurumu,
                                    GeciciGelenPersonelKurumu, GeciciGidenPersonelKurumu, istenAyrilisTarihi, istenAyrilmaSebebi,
                                    GGorevBaslangicTarihi, GGorevBitisTarihi, CepTelefonu, MailAdresi, EvTelefonu, Adres,
                                    AcilDurumdaAranacakKisi, AcilCep, KanGrubu, MedeniHali, AskerlikDurumu, EngelDurumu,
                                    EngelAciklama, Sendika, MeslekiUnvan, KadroDerece, Ogrenim_Durumu,
                                    EmeklilikTarihi, YaslilikAylikTarihi, EmeklilikAciklama, Dahili,
                                    Devredenizin, cariyilizni, KayitTarihi, KayitKullanici
                                ) VALUES (
                                    @TcKimlikNo, @Adi, @Soyad, @DogumYeri, @DogumTarihi, @Cinsiyet, @Durum, @SicilNo,
                                    @Unvan, @Statu, @IlkIseGirisTarihi, @KurumaBaslamaTarihi, @GorevYaptigiBirim, @CalismaDurumu,
                                    @GeciciGelenKurum, @GeciciGidenKurum, @IstenAyrilisTarihi, @IstenAyrilmaSebebi,
                                    @GGorevBaslangic, @GGorevBitis, @CepTelefonu, @MailAdresi, @EvTelefonu, @Adres,
                                    @AcilDurumKisi, @AcilCep, @KanGrubu, @MedeniHali, @AskerlikDurumu, @EngelDurumu,
                                    @EngelAciklama, @Sendika, @MeslekiUnvan, @KadroDerece, @OgrenimDurumu,
                                    @EmeklilikTarihi, @YaslilikAyligiTarihi, @EmeklilikAciklama, @Dahili,
                                    @DevredenIzin, @CariIzin, @KayitTarihi, @KayitKullanici
                                )";

                            var parameters = CreateParameters(
                                ("@TcKimlikNo", txtTcKimlikNo.Text.Trim()),
                                ("@Adi", txtAdi.Text.Trim()),
                                ("@Soyad", txtSoyad.Text.Trim()),
                                ("@DogumYeri", txtDogumYeri.Text.Trim()),
                                ("@DogumTarihi", string.IsNullOrEmpty(txtDogumTarihi.Text) ? (object)DBNull.Value : txtDogumTarihi.Text),
                                ("@Cinsiyet", ddlCinsiyet.SelectedValue),
                                ("@Durum", ddlCalismaDurumu.SelectedValue),
                                ("@SicilNo", txtSicilNo.Text.Trim()),
                                ("@Unvan", ddlUnvan.SelectedValue),
                                ("@Statu", ddlDurum.SelectedValue),
                                ("@IlkIseGirisTarihi", string.IsNullOrEmpty(txtIlkIseGirisTarihi.Text) ? (object)DBNull.Value : txtIlkIseGirisTarihi.Text),
                                ("@KurumaBaslamaTarihi", string.IsNullOrEmpty(txtKurumaBaslamaTarihi.Text) ? (object)DBNull.Value : txtKurumaBaslamaTarihi.Text),
                                ("@GorevYaptigiBirim", ddlGorevYaptigiBirim.SelectedValue),
                                ("@CalismaDurumu", ddlCalismaDurumu.SelectedValue),
                                ("@GeciciGelenKurum", ddlGeciciGelenKurum.SelectedValue ?? (object)DBNull.Value),
                                ("@GeciciGidenKurum", ddlGeciciGidenKurum.SelectedValue ?? (object)DBNull.Value),
                                ("@IstenAyrilisTarihi", string.IsNullOrEmpty(txtIstenAyrilisTarihi.Text) ? (object)DBNull.Value : txtIstenAyrilisTarihi.Text),
                                ("@IstenAyrilmaSebebi", txtIstenAyrilmaSebebi.Text.Trim()),
                                ("@GGorevBaslangic", string.IsNullOrEmpty(txtGGorevBaslangic.Text) ? (object)DBNull.Value : txtGGorevBaslangic.Text),
                                ("@GGorevBitis", string.IsNullOrEmpty(txtGGorevBitis.Text) ? (object)DBNull.Value : txtGGorevBitis.Text),
                                ("@CepTelefonu", txtCepTelefonu.Text.Trim()),
                                ("@MailAdresi", txtMailAdresi.Text.Trim()),
                                ("@EvTelefonu", txtEvTelefonu.Text.Trim()),
                                ("@Adres", txtAdres.Text.Trim()),
                                ("@AcilDurumKisi", txtAcilDurumKisi.Text.Trim()),
                                ("@AcilCep", txtAcilCep.Text.Trim()),
                                ("@KanGrubu", ddlKanGrubu.SelectedValue ?? (object)DBNull.Value),
                                ("@MedeniHali", ddlMedeniHali.SelectedValue ?? (object)DBNull.Value),
                                ("@AskerlikDurumu", ddlAskerlikDurumu.SelectedValue ?? (object)DBNull.Value),
                                ("@EngelDurumu", ddlEngelDurumu.SelectedValue),
                                ("@EngelAciklama", txtEngelAciklama.Text.Trim()),
                                ("@Sendika", ddlSendika.SelectedValue ?? (object)DBNull.Value),
                                ("@MeslekiUnvan", txtMeslekiUnvan.Text.Trim()),                                
                                ("@KadroDerece", txtKadroDerece.Text.Trim()),
                                ("@OgrenimDurumu", ddlOgrenimDurumu.SelectedValue ?? (object)DBNull.Value),
                                ("@EmeklilikTarihi", string.IsNullOrEmpty(txtEmeklilikTarihi.Text) ? (object)DBNull.Value : txtEmeklilikTarihi.Text),
                                ("@YaslilikAyligiTarihi", string.IsNullOrEmpty(txtYaslilikAyligiTarihi.Text) ? (object)DBNull.Value : txtYaslilikAyligiTarihi.Text),
                                ("@EmeklilikAciklama", txtEmeklilikAciklama.Text.Trim()),
                                ("@Dahili", txtDahiliTelefon.Text.Trim()),
                                ("@DevredenIzin", string.IsNullOrEmpty(txtDevredenIzin.Text) ? (object)DBNull.Value : txtDevredenIzin.Text),
                                ("@CariIzin", string.IsNullOrEmpty(txtCariIzin.Text) ? (object)DBNull.Value : txtCariIzin.Text),                               
                                ("@KayitTarihi", DateTime.Now),
                                ("@KayitKullanici", CurrentUserName)
                            );

                            int result = ExecuteNonQueryWithTransaction(conn, transaction,query, parameters);

                            if (result > 0)
                            {
                                transaction.Commit();                               
                                LogInfo($"Yeni personel eklendi: {txtAdi.Text} {txtSoyad.Text} (TC: {txtTcKimlikNo.Text}) by {CurrentUserName}");
                                ShowToast($"Personel başarıyla eklendi :{txtAdi.Text} {txtSoyad.Text} (TC: {txtTcKimlikNo.Text}) ", "success");
                            }
                            else
                            {
                                transaction.Rollback();
                                ShowError("Kayıt sırasında hata oluştu.");
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            LogError("Personel ekleme hatası", ex);
                            ShowError("Kayıt sırasında hata oluştu. Lütfen tekrar deneyin.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("Personel ekleme genel hatası", ex);
                ShowError("Sistem hatası oluştu.");
            }
        }

        protected void btnGuncelle_Click(object sender, EventArgs e)
        {
            LogInfo($"Page.IsValid: {Page.IsValid}, currentTcKimlikNo: '{currentTcKimlikNo}'");

            if (!Page.IsValid)
            {
                ShowToast("Form validation hatası: Zorunlu alanları kontrol edin.","warning");
                return;
            }
            if (string.IsNullOrEmpty(currentTcKimlikNo))
            {
                ShowToast("Güncellenecek personel TC'si bulunamadı. Lütfen 'Bilgileri Getir' butonunu kullanın.","warning");
                return;
            }

            if (!Page.IsValid || string.IsNullOrEmpty(currentTcKimlikNo)) return;

            try
            {
                using (SqlConnection conn = GetOpenConnection())
                {
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            string query = @"
                                UPDATE personel SET 
                                    Adi = @Adi, Soyad = @Soyad, DogumYeri = @DogumYeri, DogumTarihi = @DogumTarihi, 
                                    Cinsiyet = @Cinsiyet, Durum = @Durum, SicilNo = @SicilNo, Unvan = @Unvan, 
                                    Statu = @Statu, ilkisegiristarihi = @IlkIseGirisTarihi, KurumaBaslamaTarihi = @KurumaBaslamaTarihi, 
                                    GorevYaptigiBirim = @GorevYaptigiBirim, CalismaDurumu = @CalismaDurumu,
                                    GeciciGelenPersonelKurumu = @GeciciGelenKurum, GeciciGidenPersonelKurumu = @GeciciGidenKurum, 
                                    istenAyrilisTarihi = @IstenAyrilisTarihi, istenAyrilmaSebebi = @IstenAyrilmaSebebi,
                                    GGorevBaslangicTarihi = @GGorevBaslangic, GGorevBitisTarihi = @GGorevBitis, 
                                    CepTelefonu = @CepTelefonu, MailAdresi = @MailAdresi, EvTelefonu = @EvTelefonu, 
                                    Adres = @Adres, AcilDurumdaAranacakKisi = @AcilDurumKisi, AcilCep = @AcilCep, 
                                    KanGrubu = @KanGrubu, MedeniHali = @MedeniHali, AskerlikDurumu = @AskerlikDurumu, 
                                    EngelDurumu = @EngelDurumu, EngelAciklama = @EngelAciklama, Sendika = @Sendika, 
                                    MeslekiUnvan = @MeslekiUnvan, KadroDerece = @KadroDerece, 
                                    Ogrenim_Durumu = @OgrenimDurumu, EmeklilikTarihi = @EmeklilikTarihi, 
                                    YaslilikAylikTarihi = @YaslilikAyligiTarihi, EmeklilikAciklama = @EmeklilikAciklama, 
                                    Dahili = @Dahili, Devredenizin = @DevredenIzin, cariyilizni = @CariIzin, 
                                    SonGuncellemeTarihi = @SonGuncellemeTarihi, 
                                    SonGuncellemeKullanici = @SonGuncellemeKullanici
                                WHERE TcKimlikNo = @TcKimlikNo";

                            var parameters = CreateParameters(
                                ("@TcKimlikNo", currentTcKimlikNo),
                                ("@Adi", txtAdi.Text.Trim()),
                                ("@Soyad", txtSoyad.Text.Trim()),
                                ("@DogumYeri", txtDogumYeri.Text.Trim()),
                                ("@DogumTarihi", string.IsNullOrEmpty(txtDogumTarihi.Text) ? (object)DBNull.Value : txtDogumTarihi.Text),
                                ("@Cinsiyet", ddlCinsiyet.SelectedValue),
                                ("@Durum", ddlCalismaDurumu.SelectedValue),
                                ("@SicilNo", txtSicilNo.Text.Trim()),
                                ("@Unvan", ddlUnvan.SelectedValue),
                                ("@Statu", ddlDurum.SelectedValue),
                                ("@IlkIseGirisTarihi", string.IsNullOrEmpty(txtIlkIseGirisTarihi.Text) ? (object)DBNull.Value : txtIlkIseGirisTarihi.Text),
                                ("@KurumaBaslamaTarihi", string.IsNullOrEmpty(txtKurumaBaslamaTarihi.Text) ? (object)DBNull.Value : txtKurumaBaslamaTarihi.Text),
                                ("@GorevYaptigiBirim", ddlGorevYaptigiBirim.SelectedValue),
                                ("@CalismaDurumu", ddlCalismaDurumu.SelectedValue),
                                ("@GeciciGelenKurum", ddlGeciciGelenKurum.SelectedValue ?? (object)DBNull.Value),
                                ("@GeciciGidenKurum", ddlGeciciGidenKurum.SelectedValue ?? (object)DBNull.Value),
                                ("@IstenAyrilisTarihi", string.IsNullOrEmpty(txtIstenAyrilisTarihi.Text) ? (object)DBNull.Value : txtIstenAyrilisTarihi.Text),
                                ("@IstenAyrilmaSebebi", txtIstenAyrilmaSebebi.Text.Trim()),
                                ("@GGorevBaslangic", string.IsNullOrEmpty(txtGGorevBaslangic.Text) ? (object)DBNull.Value : txtGGorevBaslangic.Text),
                                ("@GGorevBitis", string.IsNullOrEmpty(txtGGorevBitis.Text) ? (object)DBNull.Value : txtGGorevBitis.Text),
                                ("@CepTelefonu", txtCepTelefonu.Text.Trim()),
                                ("@MailAdresi", txtMailAdresi.Text.Trim()),
                                ("@EvTelefonu", txtEvTelefonu.Text.Trim()),
                                ("@Adres", txtAdres.Text.Trim()),
                                ("@AcilDurumKisi", txtAcilDurumKisi.Text.Trim()),
                                ("@AcilCep", txtAcilCep.Text.Trim()),
                                ("@KanGrubu", ddlKanGrubu.SelectedValue ?? (object)DBNull.Value),
                                ("@MedeniHali", ddlMedeniHali.SelectedValue ?? (object)DBNull.Value),
                                ("@AskerlikDurumu", ddlAskerlikDurumu.SelectedValue ?? (object)DBNull.Value),
                                ("@EngelDurumu", ddlEngelDurumu.SelectedValue),
                                ("@EngelAciklama", txtEngelAciklama.Text.Trim()),
                                ("@Sendika", ddlSendika.SelectedValue ?? (object)DBNull.Value),
                                ("@MeslekiUnvan", txtMeslekiUnvan.Text.Trim()),                                
                                ("@KadroDerece", txtKadroDerece.Text.Trim()),
                                ("@OgrenimDurumu", ddlOgrenimDurumu.SelectedValue ?? (object)DBNull.Value),
                                ("@EmeklilikTarihi", string.IsNullOrEmpty(txtEmeklilikTarihi.Text) ? (object)DBNull.Value : txtEmeklilikTarihi.Text),
                                ("@YaslilikAyligiTarihi", string.IsNullOrEmpty(txtYaslilikAyligiTarihi.Text) ? (object)DBNull.Value : txtYaslilikAyligiTarihi.Text),
                                ("@EmeklilikAciklama", txtEmeklilikAciklama.Text.Trim()),
                                ("@Dahili", txtDahiliTelefon.Text.Trim()),
                                ("@DevredenIzin", string.IsNullOrEmpty(txtDevredenIzin.Text) ? (object)DBNull.Value : txtDevredenIzin.Text),
                                ("@CariIzin", string.IsNullOrEmpty(txtCariIzin.Text) ? (object)DBNull.Value : txtCariIzin.Text),                                
                                ("@SonGuncellemeTarihi", DateTime.Now),
                                ("@SonGuncellemeKullanici", CurrentUserName)
                            );

                            int result = ExecuteNonQueryWithTransaction(conn, transaction, query, parameters);
                            LogInfo($"UPDATE result: {result}");  // Debug log

                            if (result > 0)
                            {
                                transaction.Commit();
                                LogInfo($"Personel güncellendi: {txtAdi.Text} {txtSoyad.Text} (TC: {currentTcKimlikNo}) by {CurrentUserName}");
                                ShowSuccessAndRedirect("Personel başarıyla güncellendi.", "Kayit.aspx");
                            }
                            else
                            {
                                transaction.Rollback();
                                ShowError("Güncelleme sırasında hata oluştu: Eşleşen kayıt bulunamadı.");
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            LogError("Personel güncelleme hatası", ex);
                            ShowError("Güncelleme sırasında hata oluştu. Lütfen tekrar deneyin.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("Personel güncelleme genel hatası", ex);
                ShowError("Sistem hatası oluştu.");
            }
        }

        protected void btnVazgec_Click(object sender, EventArgs e)
        {
            Response.Redirect("Kayit.aspx", false);
        }
        
    }
}