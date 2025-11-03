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

        private void GorevVerileriniYukle(string filtre = "")
        {
            try
            {
                string query = "SELECT * FROM gorevkayit WHERE 1=1 ";

                if (!string.IsNullOrEmpty(filtre))
                {
                    query += filtre;
                }

                query += " ORDER BY Talep_id DESC";

                DataTable dt = ExecuteDataTable(query);

                GorevlerGrid.DataSource = dt;
                GorevlerGrid.DataBind();

                IstatistikleriGuncelle(dt);
            }
            catch (Exception ex)
            {
                LogError("Görev verileri yüklenirken hata", ex);
                ShowToast("Görev verileri yüklenirken hata oluştu.", "danger");
            }
        }

        private void IstatistikleriGuncelle(DataTable dt)
        {
            try
            {
                int toplamGorev = dt.Rows.Count;
                int aktifGorev = dt.Select("Durum = 'Aktif'").Length;
                int pasifGorev = dt.Select("Durum = 'Pasif'").Length;

                lblToplamGorev.Text = toplamGorev.ToString();
                lblAktifGorev.Text = aktifGorev.ToString();
                lblPasifGorev.Text = pasifGorev.ToString();
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
                    filtre += " AND Gorev_il = '" + ddlIl.SelectedValue + "'";
                }

                if (ddlDurum.SelectedValue != "Hepsi")
                {
                    filtre += " AND Durum = '" + ddlDurum.SelectedValue + "'";
                }

                GorevVerileriniYukle(filtre);
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
                ExportGridViewToExcel(GorevlerGrid, "GorevTalepRapor_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                LogInfo("Görev talep raporu Excel'e aktarıldı.");
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

                lblTalepId.Text = GorevlerGrid.SelectedRow.Cells[1].Text;
                lblAciklama.Text = System.Web.HttpUtility.HtmlDecode(GorevlerGrid.SelectedRow.Cells[9].Text);

                txtGoreveCikisTarihi.Text = "";
                txtGidenPersonel.Text = "";
                ddlGidisTuru.SelectedIndex = 0;
                txtGorevSuresi.Text = "";

                ShowToast("Görev seçildi. Bilgileri doldurup güncelleyebilirsiniz.", "info");
            }
            catch (Exception ex)
            {
                LogError("Görev seçimi sırasında hata", ex);
                ShowToast("Görev seçimi sırasında hata oluştu.", "danger");
            }
        }

        #endregion

        #region Güncelleme Metodları

        protected void btnGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtGoreveCikisTarihi.Text))
                {
                    ShowToast("Göreve çıkış tarihi giriniz.", "warning");
                    return;
                }

                if (string.IsNullOrEmpty(txtGidenPersonel.Text))
                {
                    ShowToast("Giden personel bilgisi giriniz.", "warning");
                    return;
                }

                string updateQuery = @"UPDATE gorevkayit 
                                     SET Durum = 'Pasif', 
                                         Goreve_Cikis_Tarihi = @GoreveCikisTarihi,
                                         Goreve_Giden_Personel = @GidenPersonel,
                                         Goreve_Gidis_Turu = @GidisTuru,
                                         Gorev_Suresi = @GorevSuresi
                                     WHERE Talep_id = @TalepId";

                var parametreler = CreateParameters(
                    ("@GoreveCikisTarihi", txtGoreveCikisTarihi.Text),
                    ("@GidenPersonel", txtGidenPersonel.Text),
                    ("@GidisTuru", ddlGidisTuru.SelectedValue),
                    ("@GorevSuresi", txtGorevSuresi.Text),
                    ("@TalepId", lblTalepId.Text)
                );

                ExecuteNonQuery(updateQuery, parametreler);

                pnlGorevGuncelle.Visible = false;
                GorevVerileriniYukle();
                GrafikVerileriniYukle();

                ShowToast("Görev başarıyla güncellendi ve pasife alındı.", "success");
                LogInfo($"Görev güncellendi. Talep ID: {lblTalepId.Text}");
            }
            catch (Exception ex)
            {
                LogError("Görev güncelleme hatası", ex);
                ShowToast("Güncelleme sırasında hata oluştu.", "danger");
            }
        }

        protected void btnVazgec_Click(object sender, EventArgs e)
        {
            pnlGorevGuncelle.Visible = false;
            ShowToast("Güncelleme iptal edildi.", "info");
        }

        #endregion

        #region Excel Export İçin Gerekli

        public override void VerifyRenderingInServerForm(Control control)
        {
        }

        #endregion
    }
}