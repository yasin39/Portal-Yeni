using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI.WebControls;
using Portal.Base;


namespace Portal.ModulBelgeTakip
{
    public partial class SonrakiCeza : BasePage
    {
        // ==> DEĞİŞİKLİK: SQL sorguları KayitTekrarSorgu.cs dosyasından buraya taşındı.
        #region SQL Sorguları
        private const string SqlGetFirmaByVergiNo = @"
            SELECT ID, FIRMA_ADI, VERGI_NUMARASI, FIRMA_ADRESI 
            FROM FIRMALAR 
            WHERE VERGI_NUMARASI LIKE @VergiNo";

        private const string SqlInsertDenetim = @"
            INSERT INTO DENETIMLER 
            (FIRMA_ID, CEZAKESEN_PERSONEL, DENETIM_TARIHI, BELGE_TIPI, 
             MAKBUZ_NO, DENETIM_TIPI)
            VALUES 
            (@FirmaId, @Personel, @DenetimTarihi, @BelgeTipi, 
             @MakbuzNo, 'TekrarEden')";

        private const string SqlUpdateFirmaTebligTarihi = @"
            UPDATE FIRMALAR 
            SET SONCEZA_TEBLIG_TARIHI = @TebligTarihi
            WHERE ID = @FirmaId";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                //if (!CheckPermission(Sabitler.BELGE_TAKIP_FIRMALAR))
                //{
                //    return;
                //}

                InitializePage();
            }
        }

        /// <summary>
        /// Sayfa ilk yüklendiğinde dropdownları doldurur
        /// </summary>
        private void InitializePage()
        {
            LoadPersonelDropDown();
            LoadBelgeTurleri();
            PanelDenetim.Visible = false;
        }

        /// <summary>
        /// Personel dropdown'ını doldurur
        /// BasePage.PopulateDropDownList kullanıldı
        /// </summary>
        private void LoadPersonelDropDown()
        {
            try
            {
                string query = @"SELECT ID, ADSOYAD FROM DenetimPersonel WHERE IsActive = 1 ORDER BY ADSOYAD";

                PopulateDropDownList(DdlPersonel, query, "ADSOYAD", "ID", addDefault: true);
                DdlPersonel.Items[0].Text = "Personel Seçiniz";
                DdlPersonel.Items[0].Value = "-1";
            }
            catch (Exception ex)
            {
                ShowToast("Personel listesi yüklenirken hata oluştu.", "danger");
                LogError("LoadPersonelDropDown hatası", ex);
            }
        }

        /// <summary>
        /// Belge türleri dropdown'ını doldurur
        /// ==> BasePage fonksiyonları kullanıldı
        /// </summary>
        private void LoadBelgeTurleri()
        {
            try
            {
                string query = @"
                    SELECT ID, BELGE_AD
                    FROM BELGELER
                    WHERE IsActive = 1
                    ORDER BY BELGE_AD";

                PopulateDropDownList(DdlBelgeTuru, query, "BELGE_AD", "ID", addDefault: true);
                DdlBelgeTuru.Items[0].Text = "Belge Türü Seçiniz";
                DdlBelgeTuru.Items[0].Value = "-1";
            }
            catch (Exception ex)
            {
                ShowToast("Belge türleri yüklenirken hata oluştu.", "danger");
                LogError("LoadBelgeTurleri hatası", ex);
            }
        }

        /// <summary>
        /// Firma arama butonu click eventi
        /// ==> BasePage fonksiyonları kullanıldı
        /// </summary>
        protected void BtnAra_Click(object sender, EventArgs e)
        {
            try
            {
                var parameters = CreateParameters(
                    ("@VergiNo", $"%{TxtVergiNo.Text.Trim()}%")
                );

                // ==> DEĞİŞİKLİK: Sorgu referansı, bu sınıftaki 'SqlGetFirmaByVergiNo' değişkeni olarak güncellendi.
                DataTable dt = ExecuteDataTable(SqlGetFirmaByVergiNo, parameters);

                FirmaGrid.DataSource = dt;
                FirmaGrid.DataBind();

                if (dt.Rows.Count == 0)
                {
                    ShowToast("Firma bulunamadı.", "warning");
                }
                else
                {
                    ShowToast($"{dt.Rows.Count} firma bulundu.", "info");
                }
            }
            catch (Exception ex)
            {
                ShowToast("Firma aranırken bir hata oluştu.", "danger");
                LogError("BtnAra_Click hatası", ex);
            }
        }

        /// <summary>
        /// GridView'de firma seçildiğinde çalışır
        /// </summary>
        protected void FirmaGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FirmaGrid.SelectedIndex != -1)
            {
                LblFirmaID.Text = FirmaGrid.SelectedDataKey.Value.ToString();
                LblFirmaAdi.Text = FirmaGrid.SelectedRow.Cells[1].Text;
                UpdateGridViewButtons();
                PanelDenetim.Visible = true;
            }
        }

        /// <summary>
        /// GridView butonlarının görünümünü günceller
        /// </summary>
        private void UpdateGridViewButtons()
        {
            foreach (GridViewRow row in FirmaGrid.Rows)
            {
                var selectButton = (LinkButton)row.Cells[4].Controls[0];
                selectButton.Text = row.RowIndex == FirmaGrid.SelectedIndex
                    ? "<i class='fas fa-check me-1'></i>Seçildi"
                    : "Seç";
                selectButton.CssClass = row.RowIndex == FirmaGrid.SelectedIndex
                    ? "btn btn-success btn-sm"
                    : "btn btn-outline-primary btn-sm";
            }
        }

        /// <summary>
        /// Kaydet butonu click eventi
        /// ==> Manuel transaction ile BasePage fonksiyonları kullanıldı
        /// </summary>
        protected void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            try
            {
                CultureInfo trCulture = new CultureInfo("tr-TR");
                DateTime secilenTarih = DateTime.Parse(HdnSelectedDate.Value, trCulture);

                int result = SaveRecord(secilenTarih);

                if (result > 0)
                {
                    ShowToast("Ceza kaydı başarıyla oluşturuldu.", "success");
                    LogInfo($"Ceza kaydı oluşturuldu. Firma ID: {LblFirmaID.Text}");
                    ClearForm();
                    FirmaGrid.DataSource = null;
                    FirmaGrid.DataBind();
                    PanelDenetim.Visible = false;
                }
                else
                {
                    ShowToast("Kayıt yapılamadı.", "danger");
                }
            }
            catch (SqlException ex) when (ex.Message.Contains("CHK_DenetimTarihi"))
            {
                ShowToast("Denetim tarihi geçerli değil. Lütfen geçerli bir tarih seçin.", "danger");
                LogError("BtnKaydet_Click - Tarih hatası", ex);
            }
            catch (Exception ex)
            {
                ShowToast($"Kayıt sırasında bir hata oluştu: {ex.Message}", "danger");
                LogError("BtnKaydet_Click hatası", ex);
            }
        }

        /// <summary>
        /// Tarih validasyonu (server-side)
        /// </summary>
        protected void CustomValidatorTarih_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!string.IsNullOrEmpty(TxtDate.Text))
            {
                CultureInfo trCulture = new CultureInfo("tr-TR");
                if (DateTime.TryParseExact(TxtDate.Text, "dd.MM.yyyy", trCulture, DateTimeStyles.None, out DateTime secilenTarih))
                {
                    args.IsValid = secilenTarih <= DateTime.Now;
                    if (!args.IsValid)
                    {
                        ((CustomValidator)source).ErrorMessage = "Denetim tarihi gelecekte olamaz.";
                    }
                }
                else
                {
                    args.IsValid = false;
                    ((CustomValidator)source).ErrorMessage = "Geçerli bir tarih giriniz.";
                }
            }
            else
            {
                args.IsValid = false;
                ((CustomValidator)source).ErrorMessage = "Denetim tarihi zorunludur.";
            }
        }

        /// <summary>
        /// Kaydı veritabanına kaydeder (Transaction ile)
        /// ==> BasePage.GetConnection() ve manuel transaction kullanıldı
        /// </summary>
        private int SaveRecord(DateTime denetimTarihi)
        {
            int affectedRows = 0;

            // ==> BasePage.GetConnection() kullanıldı
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Denetim kaydı ekleme
                        // ==> DEĞİŞİKLİK: Sorgu referansı, bu sınıftaki 'SqlInsertDenetim' değişkeni olarak güncellendi.
                        using (SqlCommand cmd = new SqlCommand(SqlInsertDenetim, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@FirmaId", LblFirmaID.Text);
                            cmd.Parameters.AddWithValue("@Personel", DdlPersonel.SelectedValue);
                            cmd.Parameters.AddWithValue("@DenetimTarihi", denetimTarihi);
                            cmd.Parameters.AddWithValue("@BelgeTipi", DdlBelgeTuru.SelectedValue);
                            cmd.Parameters.AddWithValue("@MakbuzNo", TxtCezaMakbuzNo.Text.Trim());
                            affectedRows = cmd.ExecuteNonQuery();
                        }

                        // Firma tebliğ tarihi güncelleme
                        // ==> DEĞİŞİKLİK: Sorgu referansı, bu sınıftaki 'SqlUpdateFirmaTebligTarihi' değişkeni olarak güncellendi.
                        using (SqlCommand cmd = new SqlCommand(SqlUpdateFirmaTebligTarihi, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@TebligTarihi", denetimTarihi);
                            cmd.Parameters.AddWithValue("@FirmaId", LblFirmaID.Text);
                            affectedRows += cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        LogInfo($"Transaction başarılı. Etkilenen satır: {affectedRows}");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogError("SaveRecord - Transaction rollback", ex);
                        throw;
                    }
                }
            }

            return affectedRows;
        }

        /// <summary>
        /// Formu temizler
        /// </summary>
        private void ClearForm()
        {
            ClearFormControls(TxtVergiNo, TxtDate, TxtCezaMakbuzNo, DdlPersonel, DdlBelgeTuru);
            HdnSelectedDate.Value = string.Empty;
            LblFirmaID.Text = string.Empty;
            LblFirmaAdi.Text = string.Empty;
        }
    }
}