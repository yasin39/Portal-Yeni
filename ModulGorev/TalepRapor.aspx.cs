using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Portal.Base;

namespace Portal.ModulGorev
{
    public partial class TalepRapor : BasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(Sabitler.GOREV_TAKIP_SISTEMI))
                {
                    return;
                }

                IlleriYukle();
                GorevVerileriniYukle();
                GrafikVerileriniYukle();
            }
        }

        #region ViewState Yönetimi

        private string AktifFiltre
        {
            get { return ViewState["AktifFiltre"] as string ?? ""; }
            set { ViewState["AktifFiltre"] = value; }
        }

        #endregion

        #region Veri Yükleme Metodları

        private void IlleriYukle()
        {
            try
            {
                string query = "SELECT DISTINCT Gorev_il FROM gorevkayit WHERE Gorev_il IS NOT NULL ORDER BY Gorev_il";
                DataTable dt = ExecuteDataTable(query);

                ddlIl.Items.Clear();
                ddlIl.Items.Add(new ListItem("Hepsi", "Hepsi"));

                foreach (DataRow row in dt.Rows)
                {
                    ddlIl.Items.Add(new ListItem(row["Gorev_il"].ToString(), row["Gorev_il"].ToString()));
                }
            }
            catch (Exception ex)
            {
                LogError("İller yüklenirken hata", ex);
                ShowToast("İller yüklenirken hata oluştu.", "danger");
            }
        }

        private void GorevVerileriniYukle()
        {
            try
            {
                string baseQuery = "SELECT Talep_id, Vergi_Numarasi, Unvan, Adres, Gidilecen_Son_Tarih, Durum, Gorev_il FROM gorevkayit WHERE 1=1 ";
                string query = baseQuery + AktifFiltre + " ORDER BY Talep_id DESC";

                DataTable dt;

                if (string.IsNullOrEmpty(AktifFiltre))
                {
                    dt = ExecuteDataTable(query);
                }
                else
                {
                    var parametreler = CreateParameters(
                        ("@Il", ddlIl.SelectedValue),
                        ("@Durum", ddlDurum.SelectedValue)
                    );
                    dt = ExecuteDataTable(query, parametreler);
                }

                GorevlerGrid.DataSource = dt;
                GorevlerGrid.DataBind();

                IstatistikleriGuncelle();
            }
            catch (Exception ex)
            {
                LogError("Görev verileri yüklenirken hata", ex);
                ShowToast("Görev verileri yüklenirken hata oluştu.", "danger");
            }
        }

        private void IstatistikleriGuncelle()
        {
            try
            {
                string queryToplam = "SELECT COUNT(*) FROM gorevkayit";
                string queryAktif = "SELECT COUNT(*) FROM gorevkayit WHERE Durum = 'Aktif'";
                string queryPasif = "SELECT COUNT(*) FROM gorevkayit WHERE Durum = 'Pasif'";
                string queryGecenAy = @"SELECT COUNT(*) FROM gorevkayit 
                                       WHERE Durum = 'Pasif' 
                                       AND MONTH(Gidilecen_Son_Tarih) = MONTH(DATEADD(MONTH, -1, GETDATE())) 
                                       AND YEAR(Gidilecen_Son_Tarih) = YEAR(DATEADD(MONTH, -1, GETDATE()))";

                lblToplamGorev.Text = ExecuteScalar(queryToplam).ToString();
                lblAktifGorev.Text = ExecuteScalar(queryAktif).ToString();
                lblPasifGorev.Text = ExecuteScalar(queryPasif).ToString();
                lblGecenAyTamamlanan.Text = ExecuteScalar(queryGecenAy).ToString();
            }
            catch (Exception ex)
            {
                LogError("İstatistikler güncellenirken hata", ex);
            }
        }

        private void GrafikVerileriniYukle()
        {
            try
            {
                string query = @"SELECT Gorev_il, COUNT(*) As Gorev_Sayisi 
                                FROM gorevkayit 
                                WHERE Gorev_il IS NOT NULL 
                                AND Durum = 'Pasif'
                                AND MONTH(Gidilecen_Son_Tarih) = MONTH(DATEADD(MONTH, -1, GETDATE())) 
                                AND YEAR(Gidilecen_Son_Tarih) = YEAR(DATEADD(MONTH, -1, GETDATE()))
                                GROUP BY Gorev_il 
                                ORDER BY Gorev_il ASC";

                DataTable dt = ExecuteDataTable(query);

                if (dt.Rows.Count > 0)
                {
                    string[] labels = new string[dt.Rows.Count];
                    int[] values = new int[dt.Rows.Count];

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        labels[i] = dt.Rows[i]["Gorev_il"].ToString();
                        values[i] = Convert.ToInt32(dt.Rows[i]["Gorev_Sayisi"]);
                    }

                    string jsonData = $"{{\"labels\": {Newtonsoft.Json.JsonConvert.SerializeObject(labels)}, \"values\": {Newtonsoft.Json.JsonConvert.SerializeObject(values)}}}";
                    hfGrafikVerisi.Value = jsonData;
                }
                else
                {
                    hfGrafikVerisi.Value = "{\"labels\": [], \"values\": []}";
                }
            }
            catch (Exception ex)
            {
                LogError("Grafik verileri yüklenirken hata", ex);
                hfGrafikVerisi.Value = "{\"labels\": [], \"values\": []}";
            }
        }

        #endregion

        #region Button Event Metodları

        protected void btnAra_Click(object sender, EventArgs e)
        {
            try
            {
                string filtre = "";

                if (ddlIl.SelectedValue != "Hepsi")
                {
                    filtre += " AND Gorev_il = @Il";
                }

                if (ddlDurum.SelectedValue != "Hepsi")
                {
                    filtre += " AND Durum = @Durum";
                }

                AktifFiltre = filtre;
                GorevlerGrid.PageIndex = 0;
                GorevVerileriniYukle();

                ShowToast("Filtreleme tamamlandı.", "info");
                LogInfo($"Görev arama yapıldı. Filtre: {filtre}");
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
                ddlIl.SelectedValue = "Hepsi";
                ddlDurum.SelectedValue = "Hepsi";
                AktifFiltre = "";
                GorevlerGrid.PageIndex = 0;
                GorevVerileriniYukle();
                GrafikVerileriniYukle();
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
                GorevlerGrid.AllowPaging = false;
                GorevVerileriniYukle();
                ExportGridViewToExcel(GorevlerGrid, "GorevTalepRapor_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                LogInfo("Görev talep raporu Excel'e aktarıldı.");
                GorevlerGrid.AllowPaging = true;
                GorevVerileriniYukle();
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
                pnlGorevGuncelle.Visible = true;

                lblTalepId.Text = GorevlerGrid.SelectedDataKey.Value.ToString();
                lblMevcutDurum.Text = GorevlerGrid.SelectedRow.Cells[5].Text;

                ddlYeniDurum.ClearSelection();
                if (lblMevcutDurum.Text == "Aktif")
                {
                    ddlYeniDurum.SelectedValue = "Pasif";
                }
                else
                {
                    ddlYeniDurum.SelectedValue = "Aktif";
                }

                ShowToast("Görev seçildi. Yeni durumu seçip güncelleyebilirsiniz.", "info");
            }
            catch (Exception ex)
            {
                LogError("Görev seçimi sırasında hata", ex);
                ShowToast("Görev seçimi sırasında hata oluştu.", "danger");
            }
        }

        protected void GorevlerGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GorevlerGrid.PageIndex = e.NewPageIndex;
                GorevVerileriniYukle();
            }
            catch (Exception ex)
            {
                LogError("Sayfa değiştirme hatası", ex);
                ShowToast("Sayfa değiştirme sırasında hata oluştu.", "danger");
            }
        }

        #endregion

        #region Güncelleme Metodları

        protected void btnDurumDegistir_Click(object sender, EventArgs e)
        {
            try
            {
                string updateQuery = "UPDATE gorevkayit SET Durum = @YeniDurum WHERE Talep_id = @TalepId";

                var parametreler = CreateParameters(
                    ("@YeniDurum", ddlYeniDurum.SelectedValue),
                    ("@TalepId", lblTalepId.Text)
                );

                int etkilenenSatir = ExecuteNonQuery(updateQuery, parametreler);

                if (etkilenenSatir > 0)
                {
                    pnlGorevGuncelle.Visible = false;
                    GorevVerileriniYukle();
                    GrafikVerileriniYukle();

                    ShowToast($"Görev durumu '{ddlYeniDurum.SelectedValue}' olarak güncellendi.", "success");
                    LogInfo($"Görev durumu değiştirildi. Talep ID: {lblTalepId.Text}, Yeni Durum: {ddlYeniDurum.SelectedValue}");
                }
                else
                {
                    ShowToast("Güncelleme yapılamadı. Talep ID bulunamadı.", "warning");
                    LogWarning($"Durum güncellenemedi. Talep ID: {lblTalepId.Text}");
                }
            }
            catch (Exception ex)
            {
                LogError("Durum değiştirme hatası", ex);
                ShowToast("Durum değiştirme sırasında hata oluştu.", "danger");
            }
        }

        protected void btnVazgec_Click(object sender, EventArgs e)
        {
            pnlGorevGuncelle.Visible = false;
            ShowToast("İşlem iptal edildi.", "info");
        }

        #endregion

        #region Excel Export İçin Gerekli

        public override void VerifyRenderingInServerForm(Control control)
        {
        }

        #endregion
    }
}