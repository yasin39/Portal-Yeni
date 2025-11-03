using System;
using System.Data;
using System.Web.UI.WebControls;
using Portal.Base;

namespace Portal.ModulYonetici
{
    public partial class YetkiIslem : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(900))
                {
                    return;
                }

                KullanicilariYukle();
                YetkiListesiniYukle();
            }
        }

        #region Veri Yükleme Metodları

        private void KullanicilariYukle()
        {
            try
            {
                string query = @"SELECT Adi_Soyadi, Sicil_No 
                                FROM kullanici 
                                ORDER BY Adi_Soyadi ASC";

                PopulateDropDownList(ddlKullanici, query, "Adi_Soyadi", "Sicil_No", true, null);

                if (ddlKullanici.Items.Count > 0)
                {
                    ddlKullanici.Items[0].Text = "Kullanıcı Seçiniz...";
                }

                LogInfo("Kullanıcılar yüklendi.");
            }
            catch (Exception ex)
            {
                LogError("Kullanıcılar yüklenirken hata", ex);
                ShowToast("Kullanıcılar yüklenirken hata oluştu.", "danger");
            }
        }

        private void YetkiListesiniYukle()
        {
            try
            {
                string query = @"SELECT DISTINCT Yetki, Yetki_No 
                                FROM yetki 
                                ORDER BY Yetki ASC";

                DataTable dt = ExecuteDataTable(query);

                ddlYetki.Items.Clear();
                ddlYetki.Items.Insert(0, new ListItem("Yetki Seçiniz...", ""));

                foreach (DataRow row in dt.Rows)
                {
                    string yetkiAdi = row["Yetki"].ToString();
                    string yetkiNo = row["Yetki_No"].ToString();
                    ddlYetki.Items.Add(new ListItem($"{yetkiAdi} ({yetkiNo})", yetkiNo));
                }

                LogInfo("Yetki listesi yüklendi.");
            }
            catch (Exception ex)
            {
                LogError("Yetki listesi yüklenirken hata", ex);
                ShowToast("Yetki listesi yüklenirken hata oluştu.", "danger");
            }
        }

        private void YetkileriYukle(string sicilNo)
        {
            try
            {
                if (string.IsNullOrEmpty(sicilNo))
                {
                    YetkilerGrid.DataSource = null;
                    YetkilerGrid.DataBind();
                    KayitSayisiniGuncelle(0);
                    return;
                }

                string query = @"SELECT id, Sicil_No, Yetki, Yetki_No 
                                FROM yetki 
                                WHERE Sicil_No = @SicilNo 
                                ORDER BY Yetki_No ASC";

                var parameters = CreateParameters(("@SicilNo", sicilNo));
                DataTable dt = ExecuteDataTable(query, parameters);

                YetkilerGrid.DataSource = dt;
                YetkilerGrid.DataBind();
                KayitSayisiniGuncelle(dt.Rows.Count);

                LogInfo($"Yetkiler yüklendi: {sicilNo} - {dt.Rows.Count} kayıt");
            }
            catch (Exception ex)
            {
                LogError("Yetkiler yüklenirken hata", ex);
                ShowToast("Yetkiler yüklenirken hata oluştu.", "danger");
            }
        }

        #endregion

        #region Dropdown ve Grid Event Metodları

        protected void ddlKullanici_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlKullanici.SelectedIndex > 0)
            {
                txtSicilNo.Text = ddlKullanici.SelectedValue;
                YetkileriYukle(ddlKullanici.SelectedValue);
            }
            else
            {
                txtSicilNo.Text = string.Empty;
                YetkileriYukle(null);
            }
        }

        protected void btnYetkiEkle_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlKullanici.SelectedIndex <= 0)
                {
                    ShowToast("Lütfen kullanıcı seçiniz.", "warning");
                    return;
                }

                if (ddlYetki.SelectedIndex <= 0)
                {
                    ShowToast("Lütfen yetki seçiniz.", "warning");
                    return;
                }

                string sicilNo = ddlKullanici.SelectedValue;
                string yetkiNo = ddlYetki.SelectedValue;
                string yetkiAdi = ddlYetki.SelectedItem.Text;

                if (yetkiAdi.Contains("("))
                {
                    yetkiAdi = yetkiAdi.Substring(0, yetkiAdi.IndexOf("(")).Trim();
                }

                string kontrolQuery = @"SELECT COUNT(*) 
                                       FROM yetki 
                                       WHERE Sicil_No = @SicilNo AND Yetki_No = @YetkiNo";

                var kontrolParams = CreateParameters(
                    ("@SicilNo", sicilNo),
                    ("@YetkiNo", yetkiNo)
                );

                DataTable dtKontrol = ExecuteDataTable(kontrolQuery, kontrolParams);
                int mevcutKayit = Convert.ToInt32(dtKontrol.Rows[0][0]);

                if (mevcutKayit > 0)
                {
                    ShowToast("Bu kullanıcı zaten bu yetkiye sahip.", "warning");
                    return;
                }

                string insertQuery = @"INSERT INTO yetki (Sicil_No, Yetki, Yetki_No) 
                                      VALUES (@SicilNo, @Yetki, @YetkiNo)";

                var insertParams = CreateParameters(
                    ("@SicilNo", sicilNo),
                    ("@Yetki", yetkiAdi),
                    ("@YetkiNo", yetkiNo)
                );

                int sonuc = ExecuteNonQuery(insertQuery, insertParams);

                if (sonuc > 0)
                {
                    LogInfo($"Yetki eklendi: {ddlKullanici.SelectedItem.Text} - {yetkiAdi} ({yetkiNo})");
                    ShowToast($"Yetki başarıyla eklendi: {yetkiAdi}", "success");

                    YetkileriYukle(sicilNo);
                    ddlYetki.SelectedIndex = 0;
                }
                else
                {
                    ShowToast("Yetki eklenirken bir hata oluştu.", "danger");
                }
            }
            catch (Exception ex)
            {
                LogError("Yetki eklenirken hata", ex);
                ShowToast("Yetki eklenirken hata oluştu.", "danger");
            }
        }

        protected void YetkilerGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int yetkiId = Convert.ToInt32(YetkilerGrid.SelectedDataKey.Value);
                string sicilNo = GetGridViewCellTextSafe(YetkilerGrid.SelectedRow, 2);
                string yetkiAdi = GetGridViewCellTextSafe(YetkilerGrid.SelectedRow, 3);

                string deleteQuery = "DELETE FROM yetki WHERE id = @Id";
                var parameters = CreateParameters(("@Id", yetkiId));

                int sonuc = ExecuteNonQuery(deleteQuery, parameters);

                if (sonuc > 0)
                {
                    LogInfo($"Yetki silindi: ID {yetkiId} - {yetkiAdi}");
                    ShowToast("Yetki başarıyla silindi.", "success");

                    YetkileriYukle(sicilNo);
                }
                else
                {
                    ShowToast("Yetki silinirken bir hata oluştu.", "danger");
                }
            }
            catch (Exception ex)
            {
                LogError("Yetki silinirken hata", ex);
                ShowToast("Yetki silinirken hata oluştu.", "danger");
            }
        }

        protected void btnTopluSil_Click(object sender, EventArgs e)
        {
            try
            {
                int silinenSayisi = 0;
                string sicilNo = string.Empty;

                foreach (GridViewRow row in YetkilerGrid.Rows)
                {
                    CheckBox chkSec = (CheckBox)row.FindControl("chkSec");

                    if (chkSec != null && chkSec.Checked)
                    {
                        int yetkiId = Convert.ToInt32(YetkilerGrid.DataKeys[row.RowIndex].Value);

                        if (string.IsNullOrEmpty(sicilNo))
                        {
                            sicilNo = GetGridViewCellTextSafe(row, 2);
                        }

                        string deleteQuery = "DELETE FROM yetki WHERE id = @Id";
                        var parameters = CreateParameters(("@Id", yetkiId));

                        int sonuc = ExecuteNonQuery(deleteQuery, parameters);

                        if (sonuc > 0)
                        {
                            silinenSayisi++;
                        }
                    }
                }

                if (silinenSayisi > 0)
                {
                    LogInfo($"Toplu silme tamamlandı: {silinenSayisi} yetki silindi");
                    ShowToast($"{silinenSayisi} adet yetki başarıyla silindi.", "success");

                    YetkileriYukle(sicilNo);
                }
                else
                {
                    ShowToast("Silinecek yetki bulunamadı.", "warning");
                }
            }
            catch (Exception ex)
            {
                LogError("Toplu silme hatası", ex);
                ShowToast("Toplu silme işlemi sırasında hata oluştu.", "danger");
            }
        }

        #endregion

        #region Yardımcı Metodlar

        private void KayitSayisiniGuncelle(int kayitSayisi)
        {
            if (kayitSayisi > 0)
            {
                lblKayitSayisi.Text = $"{kayitSayisi} kayıt";
                lblKayitSayisi.CssClass = "badge-count";
            }
            else
            {
                lblKayitSayisi.Text = "0 kayıt";
                lblKayitSayisi.CssClass = "badge-count";
            }
        }

        #endregion
    }
}