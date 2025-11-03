using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Base;

namespace Portal.ModulGorev
{
    public partial class TalepEkle : BasePage
    {
        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(Sabitler.GOREV_TAKIP_SISTEMI))
                {
                    return;
                }

                SubeleriYukle();
                TalepleriYukle();
            }
        }

        #endregion

        #region Veri Yükleme Metodları

        private void SubeleriYukle()
        {
            try
            {
                string query = "SELECT Sube_Adi FROM subeler ORDER BY Sube_Adi ASC";
                DataTable dt = ExecuteDataTable(query);

                ddlSubeMudurlugu.Items.Clear();
                ddlSubeMudurlugu.Items.Add(new ListItem("II. Bölge Müdürlüğü", "II. Bölge Müdürlüğü"));

                foreach (DataRow row in dt.Rows)
                {
                    ddlSubeMudurlugu.Items.Add(new ListItem(row["Sube_Adi"].ToString()));
                }
            }
            catch (Exception ex)
            {
                LogError("Şubeler yüklenirken hata", ex);
                ShowToast("Şubeler yüklenirken hata oluştu.", "danger");
            }
        }

        private void TalepleriYukle()
        {
            try
            {
                string kullaniciAdi = CurrentUserName;

                if (string.IsNullOrEmpty(kullaniciAdi))
                {
                    ShowToast("Oturum bilgisi bulunamadı.", "warning");
                    return;
                }

                string query = @"SELECT * FROM gorevkayit 
                                WHERE Kullanici = @Kullanici AND Durum = 'Aktif' 
                                ORDER BY Kayit_Tarihi DESC";

                var parametreler = CreateParameters(("@Kullanici", kullaniciAdi));
                DataTable dt = ExecuteDataTable(query, parametreler);

                TaleplerGrid.DataSource = dt;
                TaleplerGrid.DataBind();

                lblKayitSayisi.Text = dt.Rows.Count > 0 ? $"{dt.Rows.Count} kayıt" : "Kayıt yok";
                lblKayitSayisi.CssClass = dt.Rows.Count > 0 ? "badge bg-primary ms-2" : "badge bg-secondary ms-2";
            }
            catch (Exception ex)
            {
                LogError("Talepler yüklenirken hata", ex);
                ShowToast("Talepler yüklenirken hata oluştu.", "danger");
            }
        }

        #endregion

        #region Button Events

        protected void btnEkle_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                string kullaniciAdi = CurrentUserName;

                if (string.IsNullOrEmpty(kullaniciAdi))
                {
                    ShowToast("Oturum bilgisi bulunamadı.", "warning");
                    return;
                }

                string query = @"INSERT INTO gorevkayit 
                    (Gorev_Turu, Gorev_il, Gorev_ilce, Gidilecen_Son_Tarih, sube_mudurlugu, 
                     Personel_Sayisi, sure, ivedilik, Unvan, Aciklama, Kayit_Tarihi, 
                     Kullanici, Durum, Adres) 
                    VALUES 
                    (@GorevTuru, @Il, @Ilce, @SonTarih, @SubeMudurlugu, 
                     @PersonelSayisi, @IsSuresi, @Ivedilik, @Unvan, @Aciklama, @KayitTarihi, 
                     @Kullanici, @Durum, @Adres)";

                var parametreler = CreateParameters(
                    ("@GorevTuru", ddlGorevTuru.SelectedValue),
                    ("@Il", ddlIl.SelectedValue),
                    ("@Ilce", txtIlce.Text.Trim()),
                    ("@SonTarih", txtSonTarih.Text),
                    ("@SubeMudurlugu", ddlSubeMudurlugu.SelectedValue),
                    ("@PersonelSayisi", string.IsNullOrEmpty(txtPersonelSayisi.Text) ? (object)DBNull.Value : txtPersonelSayisi.Text),
                    ("@IsSuresi", string.IsNullOrEmpty(txtIsSuresi.Text) ? (object)DBNull.Value : txtIsSuresi.Text),
                    ("@Ivedilik", ddlIvedilik.SelectedValue),
                    ("@Unvan", txtUnvan.Text.Trim()),
                    ("@Aciklama", txtAciklama.Text.Trim()),
                    ("@KayitTarihi", DateTime.Now),
                    ("@Kullanici", kullaniciAdi),
                    ("@Durum", "Aktif"),
                    ("@Adres", txtAdres.Text.Trim())
                );

                int sonuc = ExecuteNonQuery(query, parametreler);

                if (sonuc > 0)
                {
                    LogInfo($"Yeni görev talebi oluşturuldu: {kullaniciAdi}");
                    ShowToast("Talep başarıyla kaydedildi.", "success");
                    FormuTemizle();
                    TalepleriYukle();
                }
                else
                {
                    ShowToast("Talep kaydedilemedi.", "danger");
                }
            }
            catch (Exception ex)
            {
                LogError("Talep eklenirken hata", ex);
                ShowToast("Talep eklenirken hata oluştu.", "danger");
            }
        }

        protected void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                if (TaleplerGrid.SelectedIndex < 0)
                {
                    ShowToast("Lütfen güncellenecek talebi seçiniz.", "warning");
                    return;
                }

                int talepId = Convert.ToInt32(TaleplerGrid.SelectedRow.Cells[0].Text);

                string query = @"UPDATE gorevkayit SET 
                    Gorev_Turu = @GorevTuru, 
                    Gorev_il = @Il, 
                    Gorev_ilce = @Ilce, 
                    Gidilecen_Son_Tarih = @SonTarih, 
                    sube_mudurlugu = @SubeMudurlugu, 
                    Personel_Sayisi = @PersonelSayisi, 
                    sure = @IsSuresi, 
                    ivedilik = @Ivedilik, 
                    Unvan = @Unvan, 
                    Adres = @Adres, 
                    Aciklama = @Aciklama,
                    Kayit_Tarihi = @KayitTarihi
                    WHERE Talep_id = @TalepId";

                var parametreler = CreateParameters(
                    ("@GorevTuru", ddlGorevTuru.SelectedValue),
                    ("@Il", ddlIl.SelectedValue),
                    ("@Ilce", txtIlce.Text.Trim()),
                    ("@SonTarih", txtSonTarih.Text),
                    ("@SubeMudurlugu", ddlSubeMudurlugu.SelectedValue),
                    ("@PersonelSayisi", string.IsNullOrEmpty(txtPersonelSayisi.Text) ? (object)DBNull.Value : txtPersonelSayisi.Text),
                    ("@IsSuresi", string.IsNullOrEmpty(txtIsSuresi.Text) ? (object)DBNull.Value : txtIsSuresi.Text),
                    ("@Ivedilik", ddlIvedilik.SelectedValue),
                    ("@Unvan", txtUnvan.Text.Trim()),
                    ("@Adres", txtAdres.Text.Trim()),
                    ("@Aciklama", txtAciklama.Text.Trim()),
                    ("@KayitTarihi", DateTime.Now),
                    ("@TalepId", talepId)
                );

                int sonuc = ExecuteNonQuery(query, parametreler);

                if (sonuc > 0)
                {
                    LogInfo($"Görev talebi güncellendi: {talepId}");
                    ShowToast("Talep başarıyla güncellendi.", "success");
                    FormuTemizle();
                    TalepleriYukle();
                    ButonDurumunuAyarla(false);
                }
                else
                {
                    ShowToast("Talep güncellenemedi.", "danger");
                }
            }
            catch (Exception ex)
            {
                LogError("Talep güncellenirken hata", ex);
                ShowToast("Talep güncellenirken hata oluştu.", "danger");
            }
        }

        protected void btnSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (TaleplerGrid.SelectedIndex < 0)
                {
                    ShowToast("Lütfen silinecek talebi seçiniz.", "warning");
                    return;
                }

                int talepId = Convert.ToInt32(TaleplerGrid.SelectedRow.Cells[0].Text);

                string query = "DELETE FROM gorevkayit WHERE Talep_id = @TalepId";
                var parametreler = CreateParameters(("@TalepId", talepId));

                int sonuc = ExecuteNonQuery(query, parametreler);

                if (sonuc > 0)
                {
                    LogInfo($"Görev talebi silindi: {talepId}");
                    ShowToast("Talep başarıyla silindi.", "success");
                    FormuTemizle();
                    TalepleriYukle();
                    ButonDurumunuAyarla(false);
                }
                else
                {
                    ShowToast("Talep silinemedi.", "danger");
                }
            }
            catch (Exception ex)
            {
                LogError("Talep silinirken hata", ex);
                ShowToast("Talep silinirken hata oluştu.", "danger");
            }
        }

        protected void btnVazgec_Click(object sender, EventArgs e)
        {
            FormuTemizle();
            ButonDurumunuAyarla(false);
            TaleplerGrid.SelectedIndex = -1;
        }

        protected void btnExcelAktar_Click(object sender, EventArgs e)
        {
            if (TaleplerGrid.Rows.Count == 0)
            {
                ShowToast("Export edilecek veri bulunamadı.", "warning");
                return;
            }

            try
            {
                ExportGridViewToExcel(TaleplerGrid, "GorevTalepleri_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                LogInfo("Görev talepleri Excel'e aktarıldı.");
            }
            catch (Exception ex)
            {
                LogError("Excel export hatası", ex);
                ShowToast("Excel dosyası oluşturulurken hata oluştu.", "danger");
            }
        }

        #endregion

        #region GridView Events

        protected void TaleplerGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = TaleplerGrid.SelectedRow;

                SetSafeDropDownValue(ddlGorevTuru, GetGridViewCellTextSafe(row, 2));
                SetSafeDropDownValue(ddlIl, GetGridViewCellTextSafe(row, 3));
                txtIlce.Text = GetGridViewCellTextSafe(row, 4);
                txtSonTarih.Text = FormatDateTimeHtml(Convert.ToDateTime(GetGridViewCellTextSafe(row, 5)));
                SetSafeDropDownValue(ddlSubeMudurlugu, GetGridViewCellTextSafe(row, 6));
                txtPersonelSayisi.Text = GetGridViewCellTextSafe(row, 7);
                txtIsSuresi.Text = GetGridViewCellTextSafe(row, 8);
                SetSafeDropDownValue(ddlIvedilik, GetGridViewCellTextSafe(row, 9));
                txtUnvan.Text = GetGridViewCellTextSafe(row, 10);
                txtAdres.Text = GetGridViewCellTextSafe(row, 11);
                txtAciklama.Text = GetGridViewCellTextSafe(row, 12);

                ButonDurumunuAyarla(true);
            }
            catch (Exception ex)
            {
                LogError("Talep seçilirken hata", ex);
                ShowToast("Talep bilgileri yüklenirken hata oluştu.", "danger");
            }
        }

        #endregion

        #region Helper Methods

        private void FormuTemizle()
        {
            ClearFormControls(ddlSubeMudurlugu, ddlIl, ddlGorevTuru, ddlIvedilik,
                             txtIlce, txtSonTarih, txtPersonelSayisi, txtIsSuresi, txtUnvan, txtAdres, txtAciklama);
        }

        private void ButonDurumunuAyarla(bool guncellemeModu)
        {
            btnEkle.Visible = !guncellemeModu;
            btnGuncelle.Visible = guncellemeModu;
            btnSil.Visible = guncellemeModu;
            btnVazgec.Visible = guncellemeModu;
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            // Excel export için gerekli
        }

        #endregion
    }
}