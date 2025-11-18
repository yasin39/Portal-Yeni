using Portal.Base;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Portal.ModulGorev
{
    public partial class GorevRapor : BasePage
    {
        #region SQL Sorguları      

        /// <summary>
        /// Araç görev listesini getirir (filtrelemeli)
        /// </summary>
        private const string SqlGetGorevListesi = @"
            SELECT 
                Gorev_Id,
                Gorevli_Arac,
                Adi_Soyadi,
                CONVERT(VARCHAR(10), Goreve_Cikis_Tarihi, 103) as Goreve_Cikis_Tarihi,
                CONVERT(VARCHAR(10), Gorevden_Donus_Tarihi, 103) as Gorevden_Donus_Tarihi,
                Cikis_Km,
                Donus_Km,
                Yapilan_Km,
                Ort_Yak_Tuk,
                Tuketilen_Yakit,
                Gorevlendiren_Birim,
                Goreve_Gidilen_Yer,
                Aciklama
            FROM aracgorev
            WHERE 1=1
                AND (@Plaka = 'Hepsi' OR Gorevli_Arac = @Plaka)
                AND (@Sube = 'Hepsi' OR Gorevlendiren_Birim = @Sube)
                AND (@BaslangicTarihi IS NULL OR CONVERT(DATE, Goreve_Cikis_Tarihi, 23) >= @BaslangicTarihi)
                AND (@BitisTarihi IS NULL OR CONVERT(DATE, Goreve_Cikis_Tarihi, 23) <= @BitisTarihi)
            ORDER BY Goreve_Cikis_Tarihi DESC";

        /// <summary>
        /// Tüm araç görevlerini getirir
        /// </summary>
        private const string SqlGetTumGorevler = @"
            SELECT 
                Gorev_Id,
                Gorevli_Arac,
                Adi_Soyadi,
                CONVERT(VARCHAR(10), Goreve_Cikis_Tarihi, 103) as Goreve_Cikis_Tarihi,
                CONVERT(VARCHAR(10), Gorevden_Donus_Tarihi, 103) as Gorevden_Donus_Tarihi,
                Cikis_Km,
                Donus_Km,
                Yapilan_Km,
                Ort_Yak_Tuk,
                Tuketilen_Yakit,
                Gorevlendiren_Birim,
                Goreve_Gidilen_Yer,
                Aciklama
            FROM aracgorev
            ORDER BY Goreve_Cikis_Tarihi DESC";

        /// <summary>
        /// Araç listesini getirir
        /// </summary>
        private const string SqlGetAraclar = @"
            SELECT Arac_Plakasi, Adi_Soyadi, Ort_Yakit_Tuk 
            FROM araclar 
            ORDER BY Arac_Plakasi ASC";

        /// <summary>
        /// Şube listesini getirir
        /// </summary>
        private const string SqlGetSubeler = @"
            SELECT Sube_Adi 
            FROM subeler 
            ORDER BY Sube_Adi ASC";

        /// <summary>
        /// Seçilen plakaya göre araç bilgilerini getirir
        /// </summary>
        private const string SqlGetAracBilgi = @"
            SELECT Adi_Soyadi, Ort_Yakit_Tuk 
            FROM araclar 
            WHERE Arac_Plakasi = @Plaka";

        /// <summary>
        /// Görev kaydını siler
        /// </summary>
        private const string SqlDeleteGorev = @"
            DELETE FROM aracgorev 
            WHERE Gorev_Id = @GorevId";

        /// <summary>
        /// Görev kaydını günceller
        /// </summary>
        private const string SqlUpdateGorev = @"
            UPDATE aracgorev 
            SET 
                Gorevli_Arac = @Plaka,
                Adi_Soyadi = @AdiSoyadi,
                Goreve_Cikis_Tarihi = @CikisTarihi,
                Gorevden_Donus_Tarihi = @DonusTarihi,
                Cikis_Km = @CikisKm,
                Donus_Km = @DonusKm,
                Yapilan_Km = @YapilanKm,
                Ort_Yak_Tuk = @OrtYakitTuk,
                Tuketilen_Yakit = @TuketilenYakit,
                Gorevlendiren_Birim = @Sube,
                Goreve_Gidilen_Yer = @GidilenYer,
                Aciklama = @Aciklama,
                Kullanici = @Kullanici
            WHERE Gorev_Id = @GorevId";

        /// <summary>
        /// İstatistik verileri (Toplam sayı, KM, Yakıt)
        /// </summary>
        private const string SqlGetIstatistikler = @"
            SELECT 
                COUNT(*) as ToplamGorev,
                ISNULL(SUM(Yapilan_Km), 0) as ToplamKm,
                ISNULL(SUM(Tuketilen_Yakit), 0) as ToplamYakit
            FROM aracgorev
            WHERE 1=1
                AND (@Plaka = 'Hepsi' OR Gorevli_Arac = @Plaka)
                AND (@Sube = 'Hepsi' OR Gorevlendiren_Birim = @Sube)
                AND (@BaslangicTarihi IS NULL OR CONVERT(DATE, Goreve_Cikis_Tarihi, 23) >= @BaslangicTarihi)
                AND (@BitisTarihi IS NULL OR CONVERT(DATE, Goreve_Cikis_Tarihi, 23) <= @BitisTarihi)";

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (!CheckPermission(Sabitler.GOREV_TAKIP_SISTEMI))
                //{
                //    return;
                //}

                AracVeSubeleriYukle();
                GorevVerileriniYukle();
                IstatistikleriGuncelle();
            }
        }

        #region Veri Yükleme Metodları

        private void AracVeSubeleriYukle()
        {
            try
            {                
                DataTable dtAraclar = ExecuteDataTable(SqlGetAraclar);

                ddlPlaka.Items.Clear();
                ddlPlaka.Items.Add(new ListItem("Hepsi", "Hepsi"));
                ddlPlaka.DataSource = dtAraclar;
                ddlPlaka.DataTextField = "Arac_Plakasi";
                ddlPlaka.DataValueField = "Arac_Plakasi";
                ddlPlaka.DataBind();

                ddlPlakaGuncelle.Items.Clear();
                ddlPlakaGuncelle.DataSource = dtAraclar;
                ddlPlakaGuncelle.DataTextField = "Arac_Plakasi";
                ddlPlakaGuncelle.DataValueField = "Arac_Plakasi";
                ddlPlakaGuncelle.DataBind();
                
                DataTable dtSubeler = ExecuteDataTable(SqlGetSubeler);

                ddlSube.Items.Clear();
                ddlSube.Items.Add(new ListItem("Hepsi", "Hepsi"));
                ddlSube.DataSource = dtSubeler;
                ddlSube.DataTextField = "Sube_Adi";
                ddlSube.DataValueField = "Sube_Adi";
                ddlSube.DataBind();

                ddlSubeGuncelle.Items.Clear();
                ddlSubeGuncelle.Items.Add(new ListItem("Seçiniz...", ""));
                ddlSubeGuncelle.DataSource = dtSubeler;
                ddlSubeGuncelle.DataTextField = "Sube_Adi";
                ddlSubeGuncelle.DataValueField = "Sube_Adi";
                ddlSubeGuncelle.DataBind();
            }
            catch (Exception ex)
            {
                LogError("Araç ve şube verileri yüklenirken hata", ex);
                ShowToast("Veriler yüklenirken hata oluştu.", "danger");
            }
        }

        private void GorevVerileriniYukle(bool filtreliArama = false)
        {
            try
            {
                DataTable dt;

                if (filtreliArama)
                {
                    string plaka = ddlPlaka.SelectedValue;
                    string sube = ddlSube.SelectedValue;
                    DateTime? baslangicTarihi = string.IsNullOrEmpty(txtBaslangicTarihi.Text) ?
                        (DateTime?)null : DateTime.Parse(txtBaslangicTarihi.Text);
                    DateTime? bitisTarihi = string.IsNullOrEmpty(txtBitisTarihi.Text) ?
                        (DateTime?)null : DateTime.Parse(txtBitisTarihi.Text);

                    var parametreler = CreateParameters(
                        ("@Plaka", plaka),
                        ("@Sube", sube),
                        ("@BaslangicTarihi", baslangicTarihi),
                        ("@BitisTarihi", bitisTarihi)
                    );

                    // Değişiklik: GorevSorgu.GetGorevListesi yerine yerel SqlGetGorevListesi kullanıldı.
                    dt = ExecuteDataTable(SqlGetGorevListesi, parametreler);
                }
                else
                {
                    // Değişiklik: GorevSorgu.GetTumGorevler yerine yerel SqlGetTumGorevler kullanıldı.
                    dt = ExecuteDataTable(SqlGetTumGorevler);
                }

                GorevlerGrid.DataSource = dt;
                GorevlerGrid.DataBind();

                KayitSayisiniGuncelle(dt.Rows.Count);
            }
            catch (Exception ex)
            {
                LogError("Görev verileri yüklenirken hata", ex);
                ShowToast("Veriler yüklenirken hata oluştu.", "danger");
            }
        }

        private void IstatistikleriGuncelle(bool filtreliHesaplama = false)
        {
            try
            {
                DataTable dt;

                if (filtreliHesaplama)
                {
                    string plaka = ddlPlaka.SelectedValue;
                    string sube = ddlSube.SelectedValue;
                    DateTime? baslangicTarihi = string.IsNullOrEmpty(txtBaslangicTarihi.Text) ?
                        (DateTime?)null : DateTime.Parse(txtBaslangicTarihi.Text);
                    DateTime? bitisTarihi = string.IsNullOrEmpty(txtBitisTarihi.Text) ?
                        (DateTime?)null : DateTime.Parse(txtBitisTarihi.Text);

                    var parametreler = CreateParameters(
                        ("@Plaka", plaka),
                        ("@Sube", sube),
                        ("@BaslangicTarihi", baslangicTarihi),
                        ("@BitisTarihi", bitisTarihi)
                    );

                    // Değişiklik: GorevSorgu.GetIstatistikler yerine yerel SqlGetIstatistikler kullanıldı.
                    dt = ExecuteDataTable(SqlGetIstatistikler, parametreler);
                }
                else
                {
                    var parametreler = CreateParameters(
                        ("@Plaka", "Hepsi"),
                        ("@Sube", "Hepsi"),
                        ("@BaslangicTarihi", null),
                        ("@BitisTarihi", null)
                    );
                    
                    dt = ExecuteDataTable(SqlGetIstatistikler, parametreler);
                }

                if (dt.Rows.Count > 0)
                {
                    lblToplamGorev.Text = dt.Rows[0]["ToplamGorev"].ToString();
                    lblToplamKm.Text = Convert.ToDecimal(dt.Rows[0]["ToplamKm"]).ToString("N2");
                    lblToplamYakit.Text = Convert.ToDecimal(dt.Rows[0]["ToplamYakit"]).ToString("N2");
                }
            }
            catch (Exception ex)
            {
                LogError("İstatistikler güncellenirken hata", ex);
            }
        }

        #endregion

        #region Button Event Metodları

        protected void btnAra_Click(object sender, EventArgs e)
        {
            try
            {
                GorevVerileriniYukle(true);
                IstatistikleriGuncelle(true);
                pnlGuncelleme.CssClass = "update-panel";
                ShowToast("Filtreleme başarıyla yapıldı.", "info");
                LogInfo($"Görev arama yapıldı. Plaka: {ddlPlaka.SelectedValue}, Şube: {ddlSube.SelectedValue}");
            }
            catch (Exception ex)
            {
                LogError("Arama sırasında hata", ex);
                ShowToast("Arama sırasında hata oluştu.", "danger");
            }
        }

        protected void btnTumunuListele_Click(object sender, EventArgs e)
        {
            try
            {
                ddlPlaka.SelectedValue = "Hepsi";
                ddlSube.SelectedValue = "Hepsi";
                txtBaslangicTarihi.Text = string.Empty;
                txtBitisTarihi.Text = string.Empty;

                GorevVerileriniYukle(false);
                IstatistikleriGuncelle(false);
                pnlGuncelleme.CssClass = "update-panel";

                ShowToast("Tüm görevler listelendi.", "info");
            }
            catch (Exception ex)
            {
                LogError("Tümünü listele sırasında hata", ex);
                ShowToast("Listeleme sırasında hata oluştu.", "danger");
            }
        }

        protected void btnExcelAktar_Click(object sender, EventArgs e)
        {
            if (GorevlerGrid.Rows.Count == 0)
            {
                ShowToast("Export edilecek veri bulunamadı.", "warning");
                return;
            }

            try
            {
                ExportGridViewToExcel(GorevlerGrid, "AracGorevRapor_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                LogInfo("Araç görev raporu Excel'e aktarıldı.");
            }
            catch (Exception ex)
            {
                LogError("Excel export hatası", ex);
                ShowToast("Excel dosyası oluşturulurken hata oluştu.", "danger");
            }
        }

        #endregion

        #region GridView Event Metodları

        protected void GorevlerGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow seciliSatir = GorevlerGrid.SelectedRow;

                txtGorevId.Text = GorevlerGrid.DataKeys[seciliSatir.RowIndex].Value.ToString();
                SetSafeDropDownValue(ddlPlakaGuncelle, HttpUtility.HtmlDecode(seciliSatir.Cells[2].Text));
                txtAdiSoyadi.Text = HttpUtility.HtmlDecode(seciliSatir.Cells[3].Text);

                string cikisTarihi = HttpUtility.HtmlDecode(seciliSatir.Cells[4].Text);
                string donusTarihi = HttpUtility.HtmlDecode(seciliSatir.Cells[5].Text);

                if (DateTime.TryParse(cikisTarihi, out DateTime dtCikis))
                    txtCikisTarihi.Text = dtCikis.ToString("yyyy-MM-dd");

                if (DateTime.TryParse(donusTarihi, out DateTime dtDonus))
                    txtDonusTarihi.Text = dtDonus.ToString("yyyy-MM-dd");

                txtCikisKm.Text = HttpUtility.HtmlDecode(seciliSatir.Cells[6].Text);
                txtDonusKm.Text = HttpUtility.HtmlDecode(seciliSatir.Cells[7].Text);
                txtYapilanKm.Text = HttpUtility.HtmlDecode(seciliSatir.Cells[8].Text);
                txtOrtYakitTuk.Text = HttpUtility.HtmlDecode(seciliSatir.Cells[9].Text);
                txtTuketilenYakit.Text = HttpUtility.HtmlDecode(seciliSatir.Cells[10].Text);
                SetSafeDropDownValue(ddlSubeGuncelle, HttpUtility.HtmlDecode(seciliSatir.Cells[11].Text));
                txtGidilenYer.Text = HttpUtility.HtmlDecode(seciliSatir.Cells[12].Text);
                txtAciklama.Text = HttpUtility.HtmlDecode(seciliSatir.Cells[13].Text);

                pnlGuncelleme.CssClass = "update-panel show";

                ShowToast("Görev kaydı seçildi.", "info");
            }
            catch (Exception ex)
            {
                LogError("Kayıt seçimi sırasında hata", ex);
                ShowToast("Kayıt seçimi sırasında hata oluştu.", "danger");
            }
        }

        #endregion

        #region Güncelleme ve Silme Metodları

        protected void btnGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                YakitHesapla();

                var parametreler = CreateParameters(
                    ("@GorevId", int.Parse(txtGorevId.Text)),
                    ("@Plaka", ddlPlakaGuncelle.SelectedValue),
                    ("@AdiSoyadi", txtAdiSoyadi.Text),
                    ("@CikisTarihi", DateTime.Parse(txtCikisTarihi.Text)),
                    ("@DonusTarihi", DateTime.Parse(txtDonusTarihi.Text)),
                    ("@CikisKm", decimal.Parse(txtCikisKm.Text)),
                    ("@DonusKm", decimal.Parse(txtDonusKm.Text)),
                    ("@YapilanKm", decimal.Parse(txtYapilanKm.Text)),
                    ("@OrtYakitTuk", decimal.Parse(txtOrtYakitTuk.Text.Replace('.', ','))),
                    ("@TuketilenYakit", decimal.Parse(txtTuketilenYakit.Text.Replace('.', ','))),
                    ("@Sube", ddlSubeGuncelle.SelectedValue),
                    ("@GidilenYer", txtGidilenYer.Text),
                    ("@Aciklama", txtAciklama.Text),
                    ("@Kullanici", CurrentUserName ?? "Sistem")
                );

                // Değişiklik: GorevSorgu.UpdateGorev yerine yerel SqlUpdateGorev kullanıldı.
                int etkilenenSatir = ExecuteNonQuery(SqlUpdateGorev, parametreler);

                if (etkilenenSatir > 0)
                {
                    GorevVerileriniYukle();
                    IstatistikleriGuncelle();
                    pnlGuncelleme.CssClass = "update-panel";

                    ShowToast("Görev kaydı başarıyla güncellendi.", "success");
                    LogInfo($"Görev güncellendi. ID: {txtGorevId.Text}");
                }
                else
                {
                    ShowToast("Kayıt güncellenemedi.", "warning");
                }
            }
            catch (Exception ex)
            {
                LogError("Güncelleme sırasında hata", ex);
                ShowToast("Güncelleme sırasında hata oluştu.", "danger");
            }
        }

        protected void btnSil_Click(object sender, EventArgs e)
        {
            try
            {
                var parametreler = CreateParameters(("@GorevId", int.Parse(txtGorevId.Text)));

                // Değişiklik: GorevSorgu.DeleteGorev yerine yerel SqlDeleteGorev kullanıldı.
                int etkilenenSatir = ExecuteNonQuery(SqlDeleteGorev, parametreler);

                if (etkilenenSatir > 0)
                {
                    GorevVerileriniYukle();
                    IstatistikleriGuncelle();
                    pnlGuncelleme.CssClass = "update-panel";

                    ShowToast("Görev kaydı başarıyla silindi.", "success");
                    LogInfo($"Görev silindi. ID: {txtGorevId.Text}");
                }
                else
                {
                    ShowToast("Kayıt silinemedi.", "warning");
                }
            }
            catch (Exception ex)
            {
                LogError("Silme sırasında hata", ex);
                ShowToast("Silme sırasında hata oluştu.", "danger");
            }
        }

        protected void btnVazgec_Click(object sender, EventArgs e)
        {
            pnlGuncelleme.CssClass = "update-panel";
            FormTemizle();
        }

        #endregion

        #region Helper Metodları

        protected void ddlPlakaGuncelle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var parametreler = CreateParameters(("@Plaka", ddlPlakaGuncelle.SelectedValue));

                // Değişiklik: GorevSorgu.GetAracBilgi yerine yerel SqlGetAracBilgi kullanıldı.
                DataTable dt = ExecuteDataTable(SqlGetAracBilgi, parametreler);

                if (dt.Rows.Count > 0)
                {
                    txtAdiSoyadi.Text = dt.Rows[0]["Adi_Soyadi"].ToString();
                    txtOrtYakitTuk.Text = dt.Rows[0]["Ort_Yakit_Tuk"].ToString();
                }
            }
            catch (Exception ex)
            {
                LogError("Araç bilgisi yüklenirken hata", ex);
            }
        }

        protected void txtDonusKm_TextChanged(object sender, EventArgs e)
        {
            YakitHesapla();
        }

        private void YakitHesapla()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtCikisKm.Text) && !string.IsNullOrEmpty(txtDonusKm.Text))
                {
                    decimal cikisKm = decimal.Parse(txtCikisKm.Text);
                    decimal donusKm = decimal.Parse(txtDonusKm.Text);
                    decimal yapilanKm = donusKm - cikisKm;

                    txtYapilanKm.Text = yapilanKm.ToString();

                    if (!string.IsNullOrEmpty(txtOrtYakitTuk.Text))
                    {
                        decimal ortYakitTuk = decimal.Parse(txtOrtYakitTuk.Text.Replace('.', ','));
                        decimal tuketilenYakit = yapilanKm * ortYakitTuk / 100;
                        txtTuketilenYakit.Text = tuketilenYakit.ToString("N2");
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("Yakıt hesaplama hatası", ex);
            }
        }

        private void FormTemizle()
        {
            ClearFormControls(txtGorevId, txtAdiSoyadi, txtOrtYakitTuk, txtCikisTarihi, txtDonusTarihi,
                             txtCikisKm, txtDonusKm, txtYapilanKm, txtTuketilenYakit, txtGidilenYer, txtAciklama,
                             ddlPlakaGuncelle, ddlSubeGuncelle);
        }

        private void KayitSayisiniGuncelle(int kayitSayisi)
        {
            lblKayitSayisi.Text = kayitSayisi > 0 ? $"{kayitSayisi} kayıt" : "Kayıt yok";
            lblKayitSayisi.CssClass = kayitSayisi > 0 ? "badge bg-primary ms-2" : "badge bg-secondary ms-2";
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            // GridView'ı Excel'e aktarırken 'Control ... must be placed inside a form tag' 
            // hatasını önlemek için bu metodun içi boş bırakılır.
        }

        #endregion
    }
}