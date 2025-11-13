using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Base;

namespace Portal.ModulYonetici
{
    public partial class KullaniciYonetim : BasePage
    {
        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(900))
                    return;

                LoadKullanicilar();
                LoadYetkiDropdown();
                LoadYetkiMatrisi();
            }
        }

        #endregion

        #region Data Loading

        /// <summary>
        /// Kullanıcı listesini yükle
        /// </summary>
        private void LoadKullanicilar()
        {
            try
            {
                string query = @"SELECT Sicil_No, Adi_Soyadi, Kullanici_Turu, Personel_Tipi, 
                                       Birim, Mail_Adresi, Durum 
                                FROM kullanici 
                                ORDER BY Adi_Soyadi ASC";

                DataTable dt = ExecuteDataTable(query);

                gvKullanicilar.DataSource = dt;
                gvKullanicilar.DataBind();

                LogInfo($"Kullanıcı listesi yüklendi: {dt.Rows.Count} kayıt");
            }
            catch (Exception ex)
            {
                LogError("Kullanıcılar yüklenirken hata", ex);
                ShowToast("Kullanıcılar yüklenirken hata oluştu.", "danger");
            }
        }

        /// <summary>
        /// Yetki dropdown'unu yükle
        /// </summary>
        private void LoadYetkiDropdown()
        {
            try
            {
                string query = @"SELECT DISTINCT Yetki, Yetki_No 
                                FROM yetki 
                                ORDER BY Yetki ASC";

                DataTable dt = ExecuteDataTable(query);

                ddlYetkiSec.Items.Clear();
                ddlYetkiSec.Items.Insert(0, new ListItem("-- Yetki Seçiniz --", ""));

                foreach (DataRow row in dt.Rows)
                {
                    string yetkiAdi = row["Yetki"].ToString();
                    string yetkiNo = row["Yetki_No"].ToString();
                    ddlYetkiSec.Items.Add(new ListItem($"{yetkiAdi} ({yetkiNo})", yetkiNo));
                }

                LogInfo("Yetki dropdown yüklendi.");
            }
            catch (Exception ex)
            {
                LogError("Yetki dropdown yüklenirken hata", ex);
                ShowToast("Yetki listesi yüklenirken hata oluştu.", "danger");
            }
        }

        /// <summary>
        /// Kullanıcı detayını yükle
        /// </summary>
        private void LoadKullaniciDetay(string sicilNo)
        {
            try
            {
                // Kullanıcı bilgilerini getir
                string queryKullanici = @"SELECT Sicil_No, Adi_Soyadi, Kullanici_Turu, Durum, Mail_Adresi 
                                         FROM kullanici 
                                         WHERE Sicil_No = @SicilNo";

                var parameters = CreateParameters(("@SicilNo", sicilNo));
                DataTable dtKullanici = ExecuteDataTable(queryKullanici, parameters);

                if (dtKullanici.Rows.Count > 0)
                {
                    DataRow row = dtKullanici.Rows[0];

                    lblDetaySicilNo.Text = row["Sicil_No"].ToString();
                    lblDetayAdSoyad.Text = row["Adi_Soyadi"].ToString();
                    lblDetayKullaniciTuru.Text = row["Kullanici_Turu"].ToString();
                    lblDetayDurum.Text = row["Durum"].ToString();
                    lblDetayMail.Text = string.IsNullOrEmpty(row["Mail_Adresi"].ToString())
                        ? "Kayıtlı değil" : row["Mail_Adresi"].ToString();

                    pnlKullaniciDetay.Visible = true;
                    divDetayBos.Visible = false;

                    // Kullanıcının yetkilerini yükle
                    LoadKullaniciYetkileri(sicilNo);
                }
            }
            catch (Exception ex)
            {
                LogError("Kullanıcı detayı yüklenirken hata", ex);
                ShowToast("Kullanıcı detayı yüklenirken hata oluştu.", "danger");
            }
        }

        /// <summary>
        /// Kullanıcının yetkilerini yükle
        /// </summary>
        private void LoadKullaniciYetkileri(string sicilNo)
        {
            try
            {
                string query = @"SELECT Yetki, Yetki_No 
                                FROM yetki 
                                WHERE Sicil_No = @SicilNo 
                                ORDER BY Yetki_No ASC";

                var parameters = CreateParameters(("@SicilNo", sicilNo));
                DataTable dt = ExecuteDataTable(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    rptKullaniciYetkileri.DataSource = dt;
                    rptKullaniciYetkileri.DataBind();
                    pnlYetkiListesi.Visible = true;
                    lblYetkiYok.Visible = false;
                }
                else
                {
                    pnlYetkiListesi.Visible = false;
                    lblYetkiYok.Visible = true;
                }

                LogInfo($"Kullanıcı yetkileri yüklendi: {sicilNo} - {dt.Rows.Count} yetki");
            }
            catch (Exception ex)
            {
                LogError("Kullanıcı yetkileri yüklenirken hata", ex);
            }
        }

        /// <summary>
        /// Yetkiye göre kullanıcıları yükle
        /// </summary>
        private void LoadYetkiyeGoreKullanicilar(string yetkiNo)
        {
            try
            {
                string query = @"SELECT k.Sicil_No, k.Adi_Soyadi, k.Durum 
                                FROM kullanici k
                                INNER JOIN yetki y ON k.Sicil_No = y.Sicil_No
                                WHERE y.Yetki_No = @YetkiNo
                                ORDER BY k.Adi_Soyadi ASC";

                var parameters = CreateParameters(("@YetkiNo", yetkiNo));
                DataTable dt = ExecuteDataTable(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    rptYetkiyeGoreKullanicilar.DataSource = dt;
                    rptYetkiyeGoreKullanicilar.DataBind();
                    lblYetkiKullaniciSayisi.Text = dt.Rows.Count.ToString();
                    pnlYetkiyeGoreKullanicilar.Visible = true;
                    divYetkiyeGoreBos.Visible = false;
                }
                else
                {
                    pnlYetkiyeGoreKullanicilar.Visible = false;
                    divYetkiyeGoreBos.Visible = true;
                }

                LogInfo($"Yetkiye göre kullanıcılar yüklendi: {yetkiNo} - {dt.Rows.Count} kullanıcı");
            }
            catch (Exception ex)
            {
                LogError("Yetkiye göre kullanıcılar yüklenirken hata", ex);
                ShowToast("Kullanıcı listesi yüklenirken hata oluştu.", "danger");
            }
        }

        /// <summary>
        /// Yetki matrisini yükle ve HTML table oluştur
        /// </summary>
        private void LoadYetkiMatrisi()
        {
            try
            {
                // Tüm kullanıcıları getir
                string queryKullanicilar = @"SELECT Sicil_No, Adi_Soyadi 
                                            FROM kullanici 
                                            WHERE Durum = 'Aktif' 
                                            ORDER BY Adi_Soyadi ASC";
                DataTable dtKullanicilar = ExecuteDataTable(queryKullanicilar);

                // Tüm yetkileri getir
                string queryYetkiler = @"SELECT DISTINCT Yetki_No, Yetki 
                                        FROM yetki 
                                        ORDER BY Yetki_No ASC";
                DataTable dtYetkiler = ExecuteDataTable(queryYetkiler);

                // Mevcut yetki atamalarını getir
                string queryMevcutYetkiler = @"SELECT Sicil_No, Yetki_No 
                                              FROM yetki";
                DataTable dtMevcutYetkiler = ExecuteDataTable(queryMevcutYetkiler);

                if (dtKullanicilar.Rows.Count == 0 || dtYetkiler.Rows.Count == 0)
                {
                    ltMatrisTable.Text = "<p class='text-center text-muted'>Kullanıcı veya yetki bulunamadı.</p>";
                    pnlMatris.Visible = true;
                    divMatrisBos.Visible = false;
                    return;
                }

                // HTML Table oluştur
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<table class='table table-bordered matrix-table'>");

                // Header
                sb.AppendLine("<thead><tr>");
                sb.AppendLine("<th class='user-name-cell'>Kullanıcı</th>");
                foreach (DataRow yetki in dtYetkiler.Rows)
                {
                    string yetkiKisaAd = yetki["Yetki"].ToString();
                    if (yetkiKisaAd.Length > 20)
                        yetkiKisaAd = yetkiKisaAd.Substring(0, 17) + "...";

                    sb.AppendLine($"<th title='{yetki["Yetki"]}'>{yetkiKisaAd}<br/><small>({yetki["Yetki_No"]})</small></th>");
                }
                sb.AppendLine("</tr></thead>");

                // Body
                sb.AppendLine("<tbody>");
                foreach (DataRow kullanici in dtKullanicilar.Rows)
                {
                    string sicilNo = kullanici["Sicil_No"].ToString();
                    string adSoyad = kullanici["Adi_Soyadi"].ToString();

                    sb.AppendLine("<tr>");
                    sb.AppendLine($"<td class='user-name-cell'><strong>{adSoyad}</strong><br/><small class='text-muted'>{sicilNo}</small></td>");

                    foreach (DataRow yetki in dtYetkiler.Rows)
                    {
                        string yetkiNo = yetki["Yetki_No"].ToString();

                        // Bu kullanıcının bu yetkisi var mı?
                        bool yetkiVar = false;
                        foreach (DataRow mevcutYetki in dtMevcutYetkiler.Rows)
                        {
                            if (mevcutYetki["Sicil_No"].ToString() == sicilNo &&
                                mevcutYetki["Yetki_No"].ToString() == yetkiNo)
                            {
                                yetkiVar = true;
                                break;
                            }
                        }

                        string checkedAttr = yetkiVar ? "checked" : "";
                        string inputName = $"chk_{sicilNo}_{yetkiNo}";

                        sb.AppendLine($"<td><input type='checkbox' class='matrix-checkbox form-check-input' name='{inputName}' id='{inputName}' {checkedAttr} /></td>");
                    }

                    sb.AppendLine("</tr>");
                }
                sb.AppendLine("</tbody>");
                sb.AppendLine("</table>");

                ltMatrisTable.Text = sb.ToString();
                pnlMatris.Visible = true;
                divMatrisBos.Visible = false;

                LogInfo("Yetki matrisi yüklendi.");
            }
            catch (Exception ex)
            {
                LogError("Yetki matrisi yüklenirken hata", ex);
                ShowToast("Yetki matrisi yüklenirken hata oluştu.", "danger");
            }
        }

        #endregion

        #region Button Events

        /// <summary>
        /// Detay butonuna tıklandığında
        /// </summary>
        protected void btnDetayGit_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                string sicilNo = btn.CommandArgument;

                LoadKullaniciDetay(sicilNo);

                // Detay sekmesine geç (Client-side script ile)
                string script = @"
                    var detayTab = new bootstrap.Tab(document.getElementById('detay-tab'));
                    detayTab.show();
                ";
                ScriptManager.RegisterStartupScript(this, GetType(), "switchTab", script, true);

                ShowToast("Kullanıcı detayı yüklendi.", "info");
            }
            catch (Exception ex)
            {
                LogError("Detay gösterme hatası", ex);
                ShowToast("Detay yüklenirken hata oluştu.", "danger");
            }
        }

        /// <summary>
        /// Yetki seçildiğinde o yetkiye sahip kullanıcıları göster
        /// </summary>
        protected void ddlYetkiSec_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlYetkiSec.SelectedIndex > 0)
            {
                string yetkiNo = ddlYetkiSec.SelectedValue;
                LoadYetkiyeGoreKullanicilar(yetkiNo);
            }
            else
            {
                pnlYetkiyeGoreKullanicilar.Visible = false;
                divYetkiyeGoreBos.Visible = true;
            }
        }

        /// <summary>
        /// Yetki matrisini kaydet
        /// </summary>
        protected void btnMatrisKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                int eklenenSayisi = 0;
                int silinenSayisi = 0;

                // Tüm kullanıcıları ve yetkileri getir
                string queryKullanicilar = @"SELECT Sicil_No FROM kullanici WHERE Durum = 'Aktif'";
                DataTable dtKullanicilar = ExecuteDataTable(queryKullanicilar);

                string queryYetkiler = @"SELECT DISTINCT Yetki_No, Yetki FROM yetki";
                DataTable dtYetkiler = ExecuteDataTable(queryYetkiler);

                foreach (DataRow kullanici in dtKullanicilar.Rows)
                {
                    string sicilNo = kullanici["Sicil_No"].ToString();

                    foreach (DataRow yetki in dtYetkiler.Rows)
                    {
                        string yetkiNo = yetki["Yetki_No"].ToString();
                        string yetkiAdi = yetki["Yetki"].ToString();
                        string inputName = $"chk_{sicilNo}_{yetkiNo}";

                        // Checkbox değerini al
                        bool checkboxChecked = Request.Form[inputName] != null;

                        // Veritabanında bu yetki var mı?
                        string queryKontrol = @"SELECT COUNT(*) 
                                               FROM yetki 
                                               WHERE Sicil_No = @SicilNo AND Yetki_No = @YetkiNo";
                        var parametersKontrol = CreateParameters(
                            ("@SicilNo", sicilNo),
                            ("@YetkiNo", yetkiNo)
                        );
                        DataTable dtKontrol = ExecuteDataTable(queryKontrol, parametersKontrol);
                        bool yetkiVarDB = Convert.ToInt32(dtKontrol.Rows[0][0]) > 0;

                        // Checkbox işaretli ama DB'de yok -> Ekle
                        if (checkboxChecked && !yetkiVarDB)
                        {
                            string queryEkle = @"INSERT INTO yetki (Sicil_No, Yetki, Yetki_No) 
                                                VALUES (@SicilNo, @Yetki, @YetkiNo)";
                            var parametersEkle = CreateParameters(
                                ("@SicilNo", sicilNo),
                                ("@Yetki", yetkiAdi),
                                ("@YetkiNo", yetkiNo)
                            );
                            ExecuteNonQuery(queryEkle, parametersEkle);
                            eklenenSayisi++;
                        }
                        // Checkbox işaretsiz ama DB'de var -> Sil
                        else if (!checkboxChecked && yetkiVarDB)
                        {
                            string querySil = @"DELETE FROM yetki 
                                               WHERE Sicil_No = @SicilNo AND Yetki_No = @YetkiNo";
                            var parametersSil = CreateParameters(
                                ("@SicilNo", sicilNo),
                                ("@YetkiNo", yetkiNo)
                            );
                            ExecuteNonQuery(querySil, parametersSil);
                            silinenSayisi++;
                        }
                    }
                }

                LogInfo($"Yetki matrisi kaydedildi: {eklenenSayisi} eklendi, {silinenSayisi} silindi");
                ShowToast($"İşlem tamamlandı: {eklenenSayisi} yetki eklendi, {silinenSayisi} yetki kaldırıldı.", "success");

                // Matrisi yenile
                LoadYetkiMatrisi();
            }
            catch (Exception ex)
            {
                LogError("Yetki matrisi kaydedilirken hata", ex);
                ShowToast("Yetki matrisi kaydedilirken hata oluştu.", "danger");
            }
        }

        /// <summary>
        /// Yetki matrisini yenile
        /// </summary>
        protected void btnMatrisYenile_Click(object sender, EventArgs e)
        {
            LoadYetkiMatrisi();
            ShowToast("Yetki matrisi yenilendi.", "info");
        }

        #endregion
    }
}
