using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Base;

namespace Portal.ModulTehlikeliMadde
{
    public partial class Tanimlamalar : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(899)) return;

                FaaliyetleriYukle();
            }
        }

        #region Veri Yükleme

        private void FaaliyetleriYukle()
        {
            try
            {
                string Sorgu = "SELECT id, FaaliyetAdi, Aciklama FROM tmfaaliyetalanlari ORDER BY FaaliyetAdi ASC";
                DataTable DtFaaliyetler = ExecuteDataTable(Sorgu);

                FaaliyetlerGrid.DataSource = DtFaaliyetler;
                FaaliyetlerGrid.DataBind();

                lblKayitSayisi.Text = DtFaaliyetler.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                LogError("Faaliyetler yüklenirken hata", ex);
                ShowToast("Veriler yüklenirken hata oluştu.", "danger");
            }
        }

        #endregion

        #region CRUD İşlemleri

        protected void btnKaydet_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                string FaaliyetAdi = txtFaaliyetAdi.Text.Trim();
                string Aciklama = txtAciklama.Text.Trim();

                if (string.IsNullOrWhiteSpace(FaaliyetAdi))
                {
                    ShowToast("Faaliyet adı boş bırakılamaz.", "warning");
                    return;
                }

                string Sorgu = @"INSERT INTO tmfaaliyetalanlari (FaaliyetAdi, Aciklama) 
                                VALUES (@FaaliyetAdi, @Aciklama)";

                var Parametreler = CreateParameters(
                    ("@FaaliyetAdi", FaaliyetAdi),
                    ("@Aciklama", Aciklama)
                );

                ExecuteNonQuery(Sorgu, Parametreler);

                ShowToast("Faaliyet alanı başarıyla kaydedildi.", "success");
                LogInfo($"Yeni faaliyet eklendi: {FaaliyetAdi}");

                ClearFormControls(txtFaaliyetAdi, txtAciklama);
                FaaliyetleriYukle();
            }
            catch (Exception ex)
            {
                LogError("Faaliyet kaydetme hatası", ex);
                ShowToast("Kayıt sırasında hata oluştu.", "danger");
            }
        }

        protected void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                if (FaaliyetlerGrid.SelectedRow == null)
                {
                    ShowToast("Lütfen güncellenecek kaydı seçiniz.", "warning");
                    return;
                }

                string FaaliyetId = FaaliyetlerGrid.SelectedDataKey.Value.ToString();
                string FaaliyetAdi = txtFaaliyetAdi.Text.Trim();
                string Aciklama = txtAciklama.Text.Trim();

                if (string.IsNullOrWhiteSpace(FaaliyetAdi))
                {
                    ShowToast("Faaliyet adı boş bırakılamaz.", "warning");
                    return;
                }

                string Sorgu = @"UPDATE tmfaaliyetalanlari 
                                SET FaaliyetAdi = @FaaliyetAdi, Aciklama = @Aciklama 
                                WHERE id = @Id";

                var Parametreler = CreateParameters(
                    ("@FaaliyetAdi", FaaliyetAdi),
                    ("@Aciklama", Aciklama),
                    ("@Id", FaaliyetId)
                );

                ExecuteNonQuery(Sorgu, Parametreler);

                ShowToast("Faaliyet alanı başarıyla güncellendi.", "success");
                LogInfo($"Faaliyet güncellendi: {FaaliyetAdi} (ID: {FaaliyetId})");

                SetFormModeInsert(btnKaydet, btnGuncelle, null, btnVazgec);
                ClearFormControls(txtFaaliyetAdi, txtAciklama);
                FaaliyetleriYukle();
            }
            catch (Exception ex)
            {
                LogError("Faaliyet güncelleme hatası", ex);
                ShowToast("Güncelleme sırasında hata oluştu.", "danger");
            }
        }

        protected void btnVazgec_Click(object sender, EventArgs e)
        {
            SetFormModeInsert(btnKaydet, btnGuncelle, null, btnVazgec);
            ClearFormControls(txtFaaliyetAdi, txtAciklama);
            FaaliyetlerGrid.SelectedIndex = -1;
        }

        #endregion

        #region GridView Events

        protected void FaaliyetlerGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow SecilenSatir = FaaliyetlerGrid.SelectedRow;

                if (SecilenSatir != null)
                {
                    txtFaaliyetAdi.Text = GetGridViewCellTextSafe(SecilenSatir, 1);
                    txtAciklama.Text = GetGridViewCellTextSafe(SecilenSatir, 2);

                    SetFormModeUpdate(btnKaydet, btnGuncelle, null, btnVazgec);
                }
            }
            catch (Exception ex)
            {
                LogError("GridView seçim hatası", ex);
                ShowToast("Kayıt seçilirken hata oluştu.", "danger");
            }
        }

        protected void FaaliyetlerGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            FaaliyetlerGrid.PageIndex = e.NewPageIndex;
            FaaliyetleriYukle();
        }

        #endregion
    }
}