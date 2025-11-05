using Portal.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Portal.ModulPersonel
{
    public partial class Liste : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(Sabitler.Personel)) return;
                DropDownlariDoldur();
                PersonelListesiniYukle();
                IstatistikleriGuncelle();
            }
        }

        #region DropDown Doldurma

        private void DropDownlariDoldur()
        {
            try
            {
                UnvanlariDoldur();
                MeslekiUnvanlariDoldur();
                SendikalariDoldur();
                BirimleriDoldur();
            }
            catch (Exception ex)
            {
                LogError("DropDown doldurma hatası", ex);
                ShowToast("Filtre seçenekleri yüklenirken hata oluştu.", "danger");
            }
        }

        private void UnvanlariDoldur()
        {
            string query = "SELECT Unvan FROM personel_unvan ORDER BY Unvan ASC";
            DataTable dt = ExecuteDataTable(query);

            AddSafeDropDownValue(ddlUnvan, "Hepsi", "Hepsi");
            foreach (DataRow row in dt.Rows)
            {
                AddSafeDropDownValue(ddlUnvan, row["Unvan"].ToString(), row["Unvan"].ToString());
            }
        }

        private void MeslekiUnvanlariDoldur()
        {
            string query = "SELECT DISTINCT MeslekiUnvan FROM personel WHERE MeslekiUnvan IS NOT NULL ORDER BY MeslekiUnvan ASC";
            DataTable dt = ExecuteDataTable(query);

            AddSafeDropDownValue(ddlMeslekiUnvan, "Hepsi", "Hepsi");
            foreach (DataRow row in dt.Rows)
            {
                AddSafeDropDownValue(ddlMeslekiUnvan, row["MeslekiUnvan"].ToString(), row["MeslekiUnvan"].ToString());
            }
        }

        private void SendikalariDoldur()
        {
            string query = "SELECT Sendika_Adi FROM personel_sendika ORDER BY Sendika_Adi ASC";
            DataTable dt = ExecuteDataTable(query);

            AddSafeDropDownValue(ddlSendika, "Hepsi", "Hepsi");
            foreach (DataRow row in dt.Rows)
            {
                AddSafeDropDownValue(ddlSendika, row["Sendika_Adi"].ToString(), row["Sendika_Adi"].ToString());
            }
        }

        private void BirimleriDoldur()
        {
            string query = "SELECT Sube_Adi FROM subeler ORDER BY Sube_Adi ASC";
            DataTable dt = ExecuteDataTable(query);

            AddSafeDropDownValue(ddlBirim, "Hepsi", "Hepsi");
            foreach (DataRow row in dt.Rows)
            {
                AddSafeDropDownValue(ddlBirim, row["Sube_Adi"].ToString(), row["Sube_Adi"].ToString());
            }
        }

        #endregion

        #region Personel Listesi Yükleme

        private DataTable PersonelListesiniGetir(bool filtreliMi = false)
        {
            string query = @"SELECT 
        Personelid, 
        TcKimlikNo, 
        SicilNo, 
        Adi, 
        Soyad, 
        Unvan, 
        MeslekiUnvan, 
        GorevYaptigiBirim, 
        CalismaDurumu, 
        Durum 
    FROM personel 
    WHERE 1=1";

            List<SqlParameter> parametreler = new List<SqlParameter>();

            if (filtreliMi)
            {
                if (ddlUnvan.SelectedValue != "Hepsi")
                {
                    query += " AND Unvan = @Unvan";
                    parametreler.Add(CreateParameter("@Unvan", ddlUnvan.SelectedValue));
                }

                if (ddlMeslekiUnvan.SelectedValue != "Hepsi")
                {
                    query += " AND MeslekiUnvan = @MeslekiUnvan";
                    parametreler.Add(CreateParameter("@MeslekiUnvan", ddlMeslekiUnvan.SelectedValue));
                }

                if (ddlStatu.SelectedValue != "Hepsi")
                {
                    query += " AND Statu = @Statu";
                    parametreler.Add(CreateParameter("@Statu", ddlStatu.SelectedValue));
                }

                if (ddlCalismaDurumu.SelectedValue != "Hepsi")
                {
                    query += " AND CalismaDurumu = @CalismaDurumu";
                    parametreler.Add(CreateParameter("@CalismaDurumu", ddlCalismaDurumu.SelectedValue));
                }

                if (ddlKanGrubu.SelectedValue != "Hepsi")
                {
                    query += " AND KanGrubu = @KanGrubu";
                    parametreler.Add(CreateParameter("@KanGrubu", ddlKanGrubu.SelectedValue));
                }

                if (ddlMedeniHal.SelectedValue != "Hepsi")
                {
                    query += " AND MedeniHali = @MedeniHali";
                    parametreler.Add(CreateParameter("@MedeniHali", ddlMedeniHal.SelectedValue));
                }

                if (ddlSendika.SelectedValue != "Hepsi")
                {
                    query += " AND Sendika = @Sendika";
                    parametreler.Add(CreateParameter("@Sendika", ddlSendika.SelectedValue));
                }

                if (ddlCinsiyet.SelectedValue != "Hepsi")
                {
                    query += " AND Cinsiyet = @Cinsiyet";
                    parametreler.Add(CreateParameter("@Cinsiyet", ddlCinsiyet.SelectedValue));
                }

                if (ddlBirim.SelectedValue != "Hepsi")
                {
                    query += " AND GorevYaptigiBirim = @GorevYaptigiBirim";
                    parametreler.Add(CreateParameter("@GorevYaptigiBirim", ddlBirim.SelectedValue));
                }

                if (ddlOgrenimDurumu.SelectedValue != "Hepsi")
                {
                    query += " AND Ogrenim_Durumu = @OgrenimDurumu";
                    parametreler.Add(CreateParameter("@OgrenimDurumu", ddlOgrenimDurumu.SelectedValue));
                }

                if (ddlDurum.SelectedValue != "Hepsi")
                {
                    query += " AND Durum = @Durum";
                    parametreler.Add(CreateParameter("@Durum", ddlDurum.SelectedValue));
                }
            }
            else
            {
                query += " AND Durum = 'Aktif'";
            }

            query += " ORDER BY Adi ASC, Soyad ASC";

            return ExecuteDataTable(query, parametreler);
        }

        private void PersonelListesiniYukle(bool filtreliMi = false)
        {
            try
            {
                DataTable dt = PersonelListesiniGetir(filtreliMi);
                PersonellerGrid.DataSource = dt;
                PersonellerGrid.DataBind();

                KayitSayisiniGuncelle(dt.Rows.Count);
            }
            catch (Exception ex)
            {
                LogError("Personel listesi yükleme hatası", ex);
                ShowToast("Personel listesi yüklenirken hata oluştu.", "danger");
            }
        }

        #endregion

        #region İstatistikler

        private void IstatistikleriGuncelle()
        {
            try
            {
                string query = @"
                    SELECT 
                        COUNT(*) as Toplam,
                        SUM(CASE WHEN Durum = 'Aktif' THEN 1 ELSE 0 END) as Aktif,
                        SUM(CASE WHEN Durum = 'Pasif' THEN 1 ELSE 0 END) as Pasif
                    FROM personel";

                DataTable dt = ExecuteDataTable(query);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    lblToplamPersonel.Text = row["Toplam"].ToString();
                    lblAktifPersonel.Text = row["Aktif"].ToString();
                    lblPasifPersonel.Text = row["Pasif"].ToString();
                }
            }
            catch (Exception ex)
            {
                LogError("İstatistik güncelleme hatası", ex);
            }
        }

        #endregion

        #region Button Events

        protected void btnAra_Click(object sender, EventArgs e)
        {
            try
            {
                PersonelListesiniYukle(filtreliMi: true);
                ShowToast("Filtreleme tamamlandı.", "success");
                LogInfo("Personel listesi filtrelendi.");
            }
            catch (Exception ex)
            {
                LogError("Arama hatası", ex);
                ShowToast("Arama sırasında hata oluştu.", "danger");
            }
        }

        protected void btnTumunuListele_Click(object sender, EventArgs e)
        {
            try
            {
                FiltreleriTemizle();
                PersonelListesiniYukle(filtreliMi: false);
                ShowToast("Tüm aktif personel listelendi.", "info");
            }
            catch (Exception ex)
            {
                LogError("Tümünü listele hatası", ex);
                ShowToast("Listeleme sırasında hata oluştu.", "danger");
            }
        }

        protected void btnExcelAktar_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtExcel = PersonelListesiniGetir(filtreliMi: true);

                if (dtExcel.Rows.Count == 0)
                {
                    ShowToast("Export edilecek veri bulunamadı.", "warning");
                    return;
                }

                GridView gridExport = new GridView();
                gridExport.DataSource = dtExcel;
                gridExport.DataBind();

                ExportGridViewToExcel(gridExport, "PersonelListesi_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                LogInfo($"Personel listesi Excel'e aktarıldı. Toplam: {dtExcel.Rows.Count} kayıt");
                ShowToast($"{dtExcel.Rows.Count} kayıt Excel'e aktarıldı.", "success");
            }
            catch (Exception ex)
            {
                LogError("Excel export hatası", ex);
                ShowToast("Excel dosyası oluşturulurken hata oluştu.", "danger");
            }
        }

        #endregion

        #region GridView Events

        protected void PersonellerGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                PersonellerGrid.PageIndex = e.NewPageIndex;
                PersonelListesiniYukle(filtreliMi: true);
            }
            catch (Exception ex)
            {
                LogError("Sayfa değiştirme hatası", ex);
                ShowToast("Sayfa değiştirme sırasında hata oluştu.", "danger");
            }
        }

        protected void PersonellerGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DetayGoster")
            {
                try
                {
                    int personelId = Convert.ToInt32(e.CommandArgument);
                    PersonelDetayGoster(personelId);
                }
                catch (Exception ex)
                {
                    LogError("Detay gösterme hatası", ex);
                    ShowToast("Personel detayı yüklenirken hata oluştu.", "danger");
                }
            }
        }

        #endregion

        #region Personel Detay

        private void PersonelDetayGoster(int personelId)
        {
            try
            {
                string query = @"
                    SELECT 
                        TcKimlikNo, SicilNo, Adi, Soyad, DogumYeri, 
                        CONVERT(VARCHAR, DogumTarihi, 103) as DogumTarihi,
                        Cinsiyet, Unvan, MeslekiUnvan, Statu, 
                        CONVERT(VARCHAR, ilkisegiristarihi, 103) as IlkIseGiris,
                        CONVERT(VARCHAR, KurumaBaslamaTarihi, 103) as KurumaBaslama,
                        GorevYaptigiBirim, CalismaDurumu, CepTelefonu, 
                        MailAdresi, EvTelefonu, Adres, AcilDurumdaAranacakKisi, 
                        AcilCep, KanGrubu, MedeniHali, Sendika, 
                        Devredenizin, cariyilizni, toplamizin, Durum, 
                        Dahili, Ogrenim_Durumu
                    FROM personel 
                    WHERE Personelid = @PersonelId";

                var parametreler = CreateParameters(("@PersonelId", personelId));
                DataTable dt = ExecuteDataTable(query, parametreler);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    lblDetayTcNo.Text = row["TcKimlikNo"].ToString();
                    lblDetaySicilNo.Text = row["SicilNo"].ToString();
                    lblDetayAdSoyad.Text = row["Adi"].ToString() + " " + row["Soyad"].ToString();
                    lblDetayDogumYeri.Text = row["DogumYeri"].ToString();
                    lblDetayDogumTarihi.Text = row["DogumTarihi"].ToString();
                    lblDetayCinsiyet.Text = row["Cinsiyet"].ToString();
                    lblDetayKanGrubu.Text = row["KanGrubu"].ToString();
                    lblDetayMedeniHal.Text = row["MedeniHali"].ToString();

                    lblDetayUnvan.Text = row["Unvan"].ToString();
                    lblDetayMeslekiUnvan.Text = row["MeslekiUnvan"].ToString();
                    lblDetayStatu.Text = row["Statu"].ToString();
                    lblDetayBirim.Text = row["GorevYaptigiBirim"].ToString();
                    lblDetayCalismaDurumu.Text = row["CalismaDurumu"].ToString();
                    lblDetayIlkIseGiris.Text = row["IlkIseGiris"].ToString();
                    lblDetayKurumaBaslama.Text = row["KurumaBaslama"].ToString();
                    lblDetaySendika.Text = row["Sendika"].ToString();
                    lblDetayDurum.Text = row["Durum"].ToString();

                    lblDetayCepTel.Text = row["CepTelefonu"].ToString();
                    lblDetayEvTel.Text = row["EvTelefonu"].ToString();
                    lblDetayDahili.Text = row["Dahili"].ToString();
                    lblDetayMail.Text = row["MailAdresi"].ToString();
                    lblDetayAdres.Text = row["Adres"].ToString();

                    lblDetayAcilKisi.Text = row["AcilDurumdaAranacakKisi"].ToString();
                    lblDetayAcilTel.Text = row["AcilCep"].ToString();

                    lblDetayDevredenIzin.Text = row["Devredenizin"].ToString() + " gün";
                    lblDetayCariYilIzin.Text = row["cariyilizni"].ToString() + " gün";
                    lblDetayToplamIzin.Text = row["toplamizin"].ToString() + " gün";

                    lblDetayOgrenim.Text = row["Ogrenim_Durumu"].ToString();                    

                    ScriptManager.RegisterStartupScript(this, GetType(), "showModal",
                        "var modal = new bootstrap.Modal(document.getElementById('modalPersonelDetay')); modal.show();", true);
                }
            }
            catch (Exception ex)
            {
                LogError("Personel detay yükleme hatası", ex);
                ShowToast("Personel detayı yüklenirken hata oluştu.", "danger");
            }
        }

        #endregion

        #region Helper Methods

        private void FiltreleriTemizle()
        {
            ResetDropDownLists(ddlUnvan, ddlMeslekiUnvan, ddlStatu, ddlCalismaDurumu,
                              ddlKanGrubu, ddlMedeniHal, ddlSendika, ddlCinsiyet,
                              ddlBirim, ddlOgrenimDurumu);
            ddlDurum.SelectedValue = "Aktif";
        }

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