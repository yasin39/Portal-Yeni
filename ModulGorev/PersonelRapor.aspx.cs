using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Base;

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
                string query = "SELECT TOP 50 * FROM yolluk WHERE 1=1 ";

                if (filtreliMi)
                {
                    if (ddlIl.SelectedValue != "Hepsi")
                        query += " AND il = @Il";

                    if (ddlPersonel.SelectedValue != "Hepsi")
                        query += " AND AdiSoyadi = @Personel";

                    if (!string.IsNullOrEmpty(txtBaslangicTarihi.Text))
                        query += " AND CONVERT(DATE, BaslamaTarihi, 23) >= @BaslangicTarihi";

                    if (!string.IsNullOrEmpty(txtBitisTarihi.Text))
                        query += " AND CONVERT(DATE, BitisTarihi, 23) <= @BitisTarihi";
                }

                query += " ORDER BY id DESC";

                var parameters = CreateParameters(
                    ("@Il", ddlIl.SelectedValue),
                    ("@Personel", ddlPersonel.SelectedValue),
                    ("@BaslangicTarihi", txtBaslangicTarihi.Text),
                    ("@BitisTarihi", txtBitisTarihi.Text)
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