using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Base;
using System.Globalization;

namespace Portal.ModulGorev
{
    public partial class PersonelRapor : BasePage
    {
        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(Sabitler.GOREV_TAKIP_SISTEMI)) return;

                DropDownlariDoldur();
                GorevVerileriniYukle();
            }
        }

        #endregion

        #region DropDown Doldurma

        private void DropDownlariDoldur()
        {
            PersonelleriYukle();
            IlleriYukle();
        }

        private void PersonelleriYukle()
        {
            try
            {
                Helpers.LoadActivePersonnel(ddlPersonel, "Hepsi");
                if (ddlPersonel.Items.Count > 0 && ddlPersonel.Items[0].Text == "Hepsi")
                {
                    // Helper'ın eklediği (muhtemelen Value="0" olan) öğeyi kaldır
                    ddlPersonel.Items.RemoveAt(0);
                    // Value="Hepsi" olan doğru öğeyi ekle
                    ddlPersonel.Items.Insert(0, new ListItem("Hepsi", "Hepsi"));
                }
            }
            catch (Exception ex)
            {
                LogError("Personeller yüklenirken hata", ex);
                ShowToast("Personeller yüklenirken hata oluştu.", "danger");
            }
        }

        private void IlleriYukle()
        {
            try
            {
                Helpers.LoadProvinces(ddlIl, "Hepsi");
                // Duplicate "Hepsi" eklenmesin - Helpers.LoadProvinces zaten ekliyor
                if (ddlIl.Items.Count > 0 && ddlIl.Items[0].Text == "Hepsi")
                {
                    // Helper'ın eklediği (muhtemelen Value="0" olan) öğeyi kaldır
                    ddlIl.Items.RemoveAt(0);
                    // Value="Hepsi" olan doğru öğeyi ekle
                    ddlIl.Items.Insert(0, new ListItem("Hepsi", "Hepsi"));
                }
            }
            catch (Exception ex)
            {
                LogError("İller yüklenirken hata", ex);
                ShowToast("İller yüklenirken hata oluştu.", "danger");
            }
        }

        #endregion

        #region Veri Yükleme

        private void GorevVerileriniYukle(bool filtreliMi = false)
        {
            try
            {
                object baslangicTarihiParam = DBNull.Value;
                object bitisTarihiParam = DBNull.Value;
                string tarihFormati = "d/M/yyyy"; 

                if (filtreliMi)
                {
                    // Başlangıç Tarihini güvenli bir şekilde DateTime nesnesine çevir
                    if (!string.IsNullOrEmpty(txtBaslangicTarihi.Text))
                    {
                        if (DateTime.TryParseExact(txtBaslangicTarihi.Text, tarihFormati,
                            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime basTarih))
                        {
                            baslangicTarihiParam = basTarih.Date;
                        }
                        else
                        {
                            // Kullanıcıya geçersiz format uyarısı ver ve işlemi durdur
                            ShowToast("Başlangıç tarihi formatı geçersiz (gg.aa.yyyy olmalı).", "warning");
                            return;
                        }
                    }

                    // Bitiş Tarihini güvenli bir şekilde DateTime nesnesine çevir
                    if (!string.IsNullOrEmpty(txtBitisTarihi.Text))
                    {
                        if (DateTime.TryParseExact(txtBitisTarihi.Text, tarihFormati,
                            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime bitTarih))
                        {
                            bitisTarihiParam = bitTarih.Date;
                        }
                        else
                        {
                            // Kullanıcıya geçersiz format uyarısı ver ve işlemi durdur
                            ShowToast("Bitiş tarihi formatı geçersiz (gg.aa.yyyy olmalı).", "warning");
                            return;
                        }
                    }
                }
                // --- DEĞİŞİKLİK SONU ---

                string query = "SELECT TOP 50 * FROM yolluk WHERE 1=1 ";

                if (filtreliMi)
                {
                    if (ddlIl.SelectedValue != "Hepsi")
                        query += " AND il = @Il";

                    if (ddlPersonel.SelectedValue != "Hepsi")
                        query += " AND AdiSoyadi = @Personel";

                    // --- DEĞİŞİKLİK BAŞLANGICI: Güvenli SQL Sorgusu ---

                    // Sadece tarih geçerliyse (DBNull değilse) sorguya ekle
                    if (baslangicTarihiParam != DBNull.Value)
                        // CONVERT yerine TRY_CONVERT kullanarak veritabanındaki bozuk veriden etkilenme
                        // Style 23 = 'yyyy-mm-dd' (Eğer DB'deki format bu değilse 104 'dd.mm.yyyy' deneyin)
                        query += " AND TRY_CONVERT(DATE, BaslamaTarihi, 23) >= @BaslangicTarihi";

                    if (bitisTarihiParam != DBNull.Value)
                        query += " AND TRY_CONVERT(DATE, BitisTarihi, 23) <= @BitisTarihi";

                    // --- DEĞİŞİKLİK SONU ---
                }

                query += " ORDER BY id DESC";

                var parameters = CreateParameters(
                    ("@Il", ddlIl.SelectedValue),
                    ("@Personel", ddlPersonel.SelectedValue),
                    // Parametreye string yerine ayrıştırılmış DateTime veya DBNull nesnesini gönder
                    ("@BaslangicTarihi", baslangicTarihiParam),
                    ("@BitisTarihi", bitisTarihiParam)
                );

                DataTable dt = ExecuteDataTable(query, parameters);

                GorevlerGrid.DataSource = dt;
                GorevlerGrid.DataBind();

                int kayitSayisi = dt.Rows.Count;
                KayitSayisiniGuncelle(kayitSayisi);

                if (kayitSayisi > 0 && GorevlerGrid.FooterRow != null)
                {
                    GorevlerGrid.FooterRow.Cells[0].Text = "Toplam";
                    GorevlerGrid.FooterRow.Cells[1].Text = kayitSayisi.ToString();
                    GorevlerGrid.FooterRow.Cells[1].Font.Bold = true;
                }

                if (filtreliMi)
                {
                    lblSonucBilgisi.Text = $"{kayitSayisi} kayıt bulundu";
                    lblSonucBilgisi.Visible = true;
                }
            }
            catch (Exception ex)
            {
                LogError("Veri yükleme hatası", ex);
                ShowToast("Veriler yüklenirken hata oluştu.", "danger");
            }
        }

        #endregion

        #region Button Events

        protected void btnAra_Click(object sender, EventArgs e)
        {
            try
            {
                GorevVerileriniYukle(filtreliMi: true);
                ShowToast("Arama tamamlandı.", "info");
                LogInfo($"Personel görev araması yapıldı. Filtre: {ddlPersonel.SelectedValue}, {ddlIl.SelectedValue}");
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
                ddlPersonel.SelectedValue = "Hepsi";
                ddlIl.SelectedValue = "Hepsi";
                txtBaslangicTarihi.Text = string.Empty;
                txtBitisTarihi.Text = string.Empty;
                lblSonucBilgisi.Visible = false;

                GorevVerileriniYukle(filtreliMi: false);
                ShowToast("Tüm kayıtlar listelendi.", "info");
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
                ExportGridViewToExcel(GorevlerGrid, "PersonelGorevRapor_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                LogInfo("Personel görev raporu Excel'e aktarıldı.");
            }
            catch (Exception ex)
            {
                LogError("Excel export hatası", ex);
                ShowToast("Excel dosyası oluşturulurken hata oluştu.", "danger");
            }
        }

        #endregion

        #region Helper Methods

        private void KayitSayisiniGuncelle(int kayitSayisi)
        {
            lblKayitSayisi.Text = kayitSayisi > 0 ? $"{kayitSayisi} kayıt" : "Kayıt yok";
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
        }

        #endregion
    }
}