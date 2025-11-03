using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Base;

namespace Portal.ModulCimer
{
    public partial class Takip : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(Sabitler.CIMER_RAPORLAMA))
                {
                    return;
                }
                FirmalariYukle();
                LogInfo("CİMER Takip sayfası yüklendi.");
            }
            btnHareket.Visible = gvCimerBasvurular.SelectedIndex >= 0;
        }

        private void FirmalariYukle()
        {
            try
            {
                Helpers.LoadCompanies(ddlFirmalar);
                LogInfo("Firmalar dropdown'a yüklendi");
            }
            catch (Exception ex)
            {
                LogError("Firmalar yüklenirken hata", ex);
                ShowError("Firmalar yüklenirken bir hata oluştu.");
            }
        }

        protected void btnFiltrele_Click(object sender, EventArgs e)
        {
            FiltreleVeListele();
        }

        private void FiltreleVeListele()
        {
            try
            {
                string sqlSorgu = "SELECT * FROM cimer_basvurular WHERE 1=1";

                List<SqlParameter> parameters = new List<SqlParameter>();

                if (!string.IsNullOrEmpty(txtBasvuruNo.Text.Trim()))
                {
                    sqlSorgu += " AND Basvuru_No = @BasvuruNo";
                    parameters.Add(CreateParameter("@BasvuruNo", txtBasvuruNo.Text.Trim()));
                }

                if (!string.IsNullOrEmpty(txtAdiSoyadi.Text.Trim()))
                {
                    sqlSorgu += " AND Adi_Soyadi LIKE @AdiSoyadi";
                    parameters.Add(CreateParameter("@AdiSoyadi", "%" + txtAdiSoyadi.Text.Trim() + "%"));
                }

                if (ddlFirmalar.SelectedValue != "")
                {
                    sqlSorgu += " AND Sikayet_Edilen_Firma = (SELECT Firma_Unvan FROM cimer_firmalar WHERE id = @FirmaId)";
                    parameters.Add(CreateParameter("@FirmaId", ddlFirmalar.SelectedValue));
                }

                if (ddlDurum.SelectedValue != "")
                {
                    sqlSorgu += " AND Sonuc = @Durum";
                    parameters.Add(CreateParameter("@Durum", ddlDurum.SelectedValue));
                }

                DataTable dt = ExecuteDataTable(sqlSorgu, parameters);

                gvCimerBasvurular.DataSource = dt;
                gvCimerBasvurular.DataBind();

                if (dt.Rows.Count > 0)
                {
                    lblToplamKayit.Text = $"Toplam {dt.Rows.Count} kayıt bulundu.";
                    lblToplamKayit.Visible = true;
                }

                LogInfo($"CİMER filtreleme: {dt.Rows.Count} kayıt listelendi.");
            }
            catch (Exception ex)
            {
                LogError("Filtreleme hatası", ex);
                ShowError("Arama sırasında bir hata oluştu.");
            }
        }

        protected void gvCimerBasvurular_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnHareket.Visible = true;
        }

        protected void btnHareket_Click(object sender, EventArgs e)
        {
            if (gvCimerBasvurular.SelectedIndex < 0)
            {
                ShowError("Lütfen bir kayıt seçiniz.");
                return;
            }

            string basvuruNo = gvCimerBasvurular.SelectedRow.Cells[2].Text; // 
            EvrakGecmisiniGoster(basvuruNo);
        }

        private void EvrakGecmisiniGoster(string basvuruNo)
        {
            try
            {
                litGecmisTablo.Text = Helpers.BuildDocumentHistoryHtml(basvuruNo);
                pnlGecmis.Visible = true;
                LogInfo($"Evrak geçmişi gösterildi: Başvuru {basvuruNo}");
            }
            catch (Exception ex)
            {
                LogError("Evrak geçmişi hatası", ex);
                ShowError("Evrak geçmişi yüklenirken bir hata oluştu.");
            }
        }

        protected void btnGecmisKapat_Click(object sender, EventArgs e)
        {
            pnlGecmis.Visible = false;
        }

        protected void btnExcelAktar_Click(object sender, EventArgs e)
        {
            ExportGridViewToExcel(gvCimerBasvurular, "CimerBasvurular.xls");
            LogInfo("CİMER verileri Excel'e aktarıldı.");
        }

        protected void gvCimerBasvurular_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCimerBasvurular.PageIndex = e.NewPageIndex;
            FiltreleVeListele();
        }
    }
}