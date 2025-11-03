using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Base;

namespace Portal.ModulGorev
{
    public partial class Gorevlendirme : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(Sabitler.GOREV_TAKIP_SISTEMI))
                {
                    return;
                }

                DropDownListeleriYukle();
                GorevlendirmeleriYukle();
                IstatistikleriGuncelle();
            }
        }

        #region Dropdown Yükleme Metodları

        private void DropDownListeleriYukle()
        {
            PersonelleriYukle();
            IlleriYukle();
        }

        private void PersonelleriYukle()
        {
            try
            {
                Helpers.LoadActivePersonnel(ddlPersonel, "Seçiniz");
                Helpers.LoadActivePersonnel(ddlFiltrePersonel, "Tümü");
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
                Helpers.LoadProvinces(ddlIl, "Seçiniz");
                Helpers.LoadProvinces(ddlFiltreIl, "Tümü");
            }
            catch (Exception ex)
            {
                LogError("İller yüklenirken hata", ex);
                ShowToast("İller yüklenirken hata oluştu.", "danger");
            }
        }

        #endregion

        #region Veri Yükleme Metodları

        private void GorevlendirmeleriYukle(string filtre = "")
        {
            try
            {
                string query = @"SELECT id, AdiSoyadi, BaslamaTarihi, GorevlendirmeSuresi, 
                               BitisTarihi, il, Digeriller, GorevTanimi, 
                               KayitTarihi, KayitKullanici, GuncellemeTarihi, GuncelleyenKullanici
                               FROM yolluk 
                               WHERE 1=1 " + filtre + " ORDER BY id DESC";

                DataTable dt = ExecuteDataTable(query);

                GorevlendirmeGrid.DataSource = dt;
                GorevlendirmeGrid.DataBind();

                KayitSayisiniGuncelle(dt.Rows.Count);
            }
            catch (Exception ex)
            {
                LogError("Görevlendirmeler yüklenirken hata", ex);
                ShowToast("Görevlendirmeler yüklenirken hata oluştu.", "danger");
            }
        }

        private void IstatistikleriGuncelle()
        {
            try
            {
                string queryToplam = "SELECT COUNT(*) FROM yolluk";
                DataTable dtToplam = ExecuteDataTable(queryToplam);
                int toplamGorev = dtToplam.Rows.Count > 0 ? Convert.ToInt32(dtToplam.Rows[0][0]) : 0;

                string queryAktif = "SELECT COUNT(*) FROM yolluk WHERE BitisTarihi >= @Bugun";
                var parametersAktif = CreateParameters(("@Bugun", DateTime.Now.Date));
                DataTable dtAktif = ExecuteDataTable(queryAktif, parametersAktif);
                int aktifGorev = dtAktif.Rows.Count > 0 ? Convert.ToInt32(dtAktif.Rows[0][0]) : 0;

                int tamamlananGorev = toplamGorev - aktifGorev;

                lblToplamGorev.Text = toplamGorev.ToString();
                lblAktifGorev.Text = aktifGorev.ToString();
                lblTamamlananGorev.Text = tamamlananGorev.ToString();
            }
            catch (Exception ex)
            {
                LogError("İstatistikler güncellenirken hata", ex);
            }
        }

        private void KayitSayisiniGuncelle(int kayitSayisi)
        {
            if (kayitSayisi > 0)
            {
                lblKayitSayisi.Text = $"{kayitSayisi} kayıt";
                lblKayitSayisi.CssClass = "badge bg-primary ms-2";
            }
            else
            {
                lblKayitSayisi.Text = "Kayıt yok";
                lblKayitSayisi.CssClass = "badge bg-secondary ms-2";
            }
        }

        #endregion

        #region Kayıt İşlemleri

        private bool KayitKontrol()
        {
            try
            {
                string query = @"SELECT COUNT(*) FROM yolluk 
                               WHERE AdiSoyadi = @AdiSoyadi 
                               AND BaslamaTarihi = @BaslamaTarihi";

                if (!string.IsNullOrEmpty(hfKayitID.Value) && hfKayitID.Value != "0")
                {
                    query += " AND id != @KayitID";
                }

                var parameters = CreateParameters(
                    ("@AdiSoyadi", ddlPersonel.SelectedValue),
                    ("@BaslamaTarihi", txtBaslamaTarihi.Text),
                    ("@KayitID", hfKayitID.Value)
                );

                DataTable dt = ExecuteDataTable(query, parameters);
                int kayitSayisi = dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0][0]) : 0;

                return kayitSayisi == 0;
            }
            catch (Exception ex)
            {
                LogError("Kayıt kontrolü sırasında hata", ex);
                return false;
            }
        }

        private void FormTemizle()
        {
            hfKayitID.Value = "0";
            ClearFormControls(ddlPersonel, ddlIl, txtDigerIller, txtBaslamaTarihi,
                             txtSure, txtBitisTarihi, txtGorevTanimi);

            lblFormBaslik.Text = "Yeni Görevlendirme Ekle";
            SetFormModeInsert(btnKaydet, btnGuncelle, btnSil, btnVazgec);
        }

        #endregion

        #region Button Event Metodları

        protected void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                if (!KayitKontrol())
                {
                    ShowToast("Aynı tarihte aynı personele görevlendirme zaten eklenmiş.", "warning");
                    return;
                }

                string query = @"INSERT INTO yolluk 
                               (AdiSoyadi, BaslamaTarihi, GorevlendirmeSuresi, BitisTarihi, 
                                il, Digeriller, GorevTanimi, KayitTarihi, KayitKullanici) 
                               VALUES 
                               (@AdiSoyadi, @BaslamaTarihi, @GorevlendirmeSuresi, @BitisTarihi, 
                                @il, @Digeriller, @GorevTanimi, @KayitTarihi, @KayitKullanici)";

                var parameters = CreateParameters(
                    ("@AdiSoyadi", ddlPersonel.SelectedValue),
                    ("@BaslamaTarihi", txtBaslamaTarihi.Text),
                    ("@GorevlendirmeSuresi", txtSure.Text),
                    ("@BitisTarihi", txtBitisTarihi.Text),
                    ("@il", ddlIl.SelectedValue),
                    ("@Digeriller", txtDigerIller.Text),
                    ("@GorevTanimi", txtGorevTanimi.Text),
                    ("@KayitTarihi", DateTime.Now),
                    ("@KayitKullanici", CurrentUserName ?? "Sistem")
                );

                int etkilenenSatir = ExecuteNonQuery(query, parameters);

                if (etkilenenSatir > 0)
                {
                    ShowToast("Görevlendirme başarıyla kaydedildi.", "success");
                    LogInfo($"Yeni görevlendirme eklendi: {ddlPersonel.SelectedValue}");
                    FormTemizle();
                    GorevlendirmeleriYukle();
                    IstatistikleriGuncelle();
                }
                else
                {
                    ShowToast("Kayıt sırasında bir hata oluştu.", "danger");
                }
            }
            catch (Exception ex)
            {
                LogError("Kayıt sırasında hata", ex);
                ShowToast("Kayıt sırasında bir hata oluştu.", "danger");
            }
        }

        protected void btnGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if (!KayitKontrol())
                {
                    ShowToast("Aynı tarihte aynı personele görevlendirme zaten eklenmiş.", "warning");
                    return;
                }

                string query = @"UPDATE yolluk SET 
                               AdiSoyadi = @AdiSoyadi,
                               BaslamaTarihi = @BaslamaTarihi,
                               GorevlendirmeSuresi = @GorevlendirmeSuresi,
                               BitisTarihi = @BitisTarihi,
                               il = @il,
                               Digeriller = @Digeriller,
                               GorevTanimi = @GorevTanimi,
                               GuncellemeTarihi = @GuncellemeTarihi,
                               GuncelleyenKullanici = @GuncelleyenKullanici
                               WHERE id = @KayitID";

                var parameters = CreateParameters(
                    ("@AdiSoyadi", ddlPersonel.SelectedValue),
                    ("@BaslamaTarihi", txtBaslamaTarihi.Text),
                    ("@GorevlendirmeSuresi", txtSure.Text),
                    ("@BitisTarihi", txtBitisTarihi.Text),
                    ("@il", ddlIl.SelectedValue),
                    ("@Digeriller", txtDigerIller.Text),
                    ("@GorevTanimi", txtGorevTanimi.Text),
                    ("@GuncellemeTarihi", DateTime.Now),
                    ("@GuncelleyenKullanici", CurrentUserName ?? "Sistem"),
                    ("@KayitID", hfKayitID.Value)
                );

                int etkilenenSatir = ExecuteNonQuery(query, parameters);

                if (etkilenenSatir > 0)
                {
                    ShowToast("Görevlendirme başarıyla güncellendi.", "success");
                    LogInfo($"Görevlendirme güncellendi: ID={hfKayitID.Value}");
                    FormTemizle();
                    GorevlendirmeleriYukle();
                    IstatistikleriGuncelle();
                }
                else
                {
                    ShowToast("Güncelleme sırasında bir hata oluştu.", "danger");
                }
            }
            catch (Exception ex)
            {
                LogError("Güncelleme sırasında hata", ex);
                ShowToast("Güncelleme sırasında bir hata oluştu.", "danger");
            }
        }

        protected void btnSil_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "DELETE FROM yolluk WHERE id = @KayitID";
                var parameters = CreateParameters(("@KayitID", hfKayitID.Value));

                int etkilenenSatir = ExecuteNonQuery(query, parameters);

                if (etkilenenSatir > 0)
                {
                    ShowToast("Görevlendirme başarıyla silindi.", "success");
                    LogInfo($"Görevlendirme silindi: ID={hfKayitID.Value}");
                    FormTemizle();
                    GorevlendirmeleriYukle();
                    IstatistikleriGuncelle();
                }
                else
                {
                    ShowToast("Silme sırasında bir hata oluştu.", "danger");
                }
            }
            catch (Exception ex)
            {
                LogError("Silme sırasında hata", ex);
                ShowToast("Silme sırasında bir hata oluştu.", "danger");
            }
        }

        protected void btnVazgec_Click(object sender, EventArgs e)
        {
            FormTemizle();
            ShowToast("İşlem iptal edildi.", "info");
        }

        protected void btnFiltrele_Click(object sender, EventArgs e)
        {
            try
            {
                string filtre = "";

                if (!string.IsNullOrEmpty(ddlFiltrePersonel.SelectedValue))
                {
                    filtre += $" AND AdiSoyadi = '{ddlFiltrePersonel.SelectedValue}'";
                }

                if (!string.IsNullOrEmpty(ddlFiltreIl.SelectedValue))
                {
                    filtre += $" AND il = '{ddlFiltreIl.SelectedValue}'";
                }

                if (!string.IsNullOrEmpty(txtFiltreBaslangic.Text))
                {
                    filtre += $" AND BaslamaTarihi >= '{txtFiltreBaslangic.Text}'";
                }

                if (!string.IsNullOrEmpty(txtFiltreBitis.Text))
                {
                    filtre += $" AND BitisTarihi <= '{txtFiltreBitis.Text}'";
                }

                GorevlendirmeleriYukle(filtre);
                ShowToast("Filtreleme tamamlandı.", "info");
            }
            catch (Exception ex)
            {
                LogError("Filtreleme sırasında hata", ex);
                ShowToast("Filtreleme sırasında bir hata oluştu.", "danger");
            }
        }

        protected void btnTemizle_Click(object sender, EventArgs e)
        {
            ddlFiltrePersonel.SelectedIndex = 0;
            ddlFiltreIl.SelectedIndex = 0;
            txtFiltreBaslangic.Text = string.Empty;
            txtFiltreBitis.Text = string.Empty;
            GorevlendirmeleriYukle();
            ShowToast("Filtreler temizlendi.", "info");
        }

        protected void btnExcelAktar_Click(object sender, EventArgs e)
        {
            if (GorevlendirmeGrid.Rows.Count == 0)
            {
                ShowToast("Export edilecek veri bulunamadı.", "warning");
                return;
            }

            try
            {
                ExportGridViewToExcel(GorevlendirmeGrid, "Gorevlendirmeler_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                LogInfo("Görevlendirmeler Excel'e aktarıldı.");
            }
            catch (Exception ex)
            {
                LogError("Excel export hatası", ex);
                ShowToast("Excel dosyası oluşturulurken hata oluştu.", "danger");
            }
        }

        #endregion

        #region GridView Event Metodları

        protected void GorevlendirmeGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GorevlendirmeGrid.PageIndex = e.NewPageIndex;
                GorevlendirmeleriYukle();
            }
            catch (Exception ex)
            {
                LogError("Sayfa değiştirme hatası", ex);
                ShowToast("Sayfa değiştirme sırasında hata oluştu.", "danger");
            }
        }

        protected void GorevlendirmeGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int selectedIndex = GorevlendirmeGrid.SelectedIndex;
                GridViewRow row = GorevlendirmeGrid.Rows[selectedIndex];

                hfKayitID.Value = row.Cells[1].Text;

                string query = "SELECT * FROM yolluk WHERE id = @KayitID";
                var parameters = CreateParameters(("@KayitID", hfKayitID.Value));
                DataTable dt = ExecuteDataTable(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    SetSafeDropDownValue(ddlPersonel, dr["AdiSoyadi"].ToString());
                    SetSafeDropDownValue(ddlIl, dr["il"].ToString());
                    txtDigerIller.Text = dr["Digeriller"].ToString();
                    txtBaslamaTarihi.Text = Convert.ToDateTime(dr["BaslamaTarihi"]).ToString("yyyy-MM-dd");
                    txtSure.Text = dr["GorevlendirmeSuresi"].ToString();
                    txtBitisTarihi.Text = Convert.ToDateTime(dr["BitisTarihi"]).ToString("yyyy-MM-dd");
                    txtGorevTanimi.Text = dr["GorevTanimi"].ToString();

                    lblFormBaslik.Text = "Görevlendirme Güncelle";
                    btnKaydet.Visible = false;
                    btnGuncelle.Visible = true;
                    btnSil.Visible = true;
                    btnVazgec.Visible = true;

                    ShowToast("Kayıt yüklendi. Güncelleyebilir veya silebilirsiniz.", "info");
                }
            }
            catch (Exception ex)
            {
                LogError("Kayıt seçimi sırasında hata", ex);
                ShowToast("Kayıt yüklenirken hata oluştu.", "danger");
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
        }

        #endregion
    }
}