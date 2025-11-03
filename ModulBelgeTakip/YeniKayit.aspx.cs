using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Base;

namespace Portal.ModulBelgeTakip
{
    public partial class YeniKayit : BasePage
    {
        private const string SqlInsertFirma = @"INSERT INTO FIRMALAR (IL, ILCE, FIRMA_TIPI, BELGE_TIPI, VERGI_NUMARASI, FIRMA_ADI, FIRMA_ADRESI, KATEGORI_TIPI, SONCEZA_TEBLIG_TARIHI, BELGE_ALDIMI) VALUES (@IL, @ILCE, @FIRMA_TIPI, @BELGE_TIPI, @VERGI_NUMARASI, @FIRMA_ADI, @FIRMA_ADRESI, @KATEGORI, @TODAY, 0); SELECT SCOPE_IDENTITY();";
        private const string SqlInsertDenetim = @"INSERT INTO DENETIMLER (FIRMA_ID, D_Y_PERSONEL, PERSONEL1, PERSONEL2, BELGE_TIPI, MAKBUZ_NO, DENETIM_TARIHI, DENETIM_TIPI) VALUES (@FirmaId, @Personel, @Personel1, @Personel2, @BelgeTipi, @MakbuzNo, @DenetimTarihi, @DenetimTuru)";
        private const string SqlGetIller = @"SELECT IL_ID, IL_AD FROM ILLER ORDER BY IL_AD";
        private const string SqlGetIlceler = @"SELECT ILCE_ID, ILCE_AD FROM ILCELER WHERE IL_ID = @IL_ID ORDER BY ILCE_AD";
        private const string SqlGetPersoneller = @"SELECT ID, ADSOYAD FROM DenetimPersonel WHERE IsActive = 1 ORDER BY ADSOYAD";
        private const string SqlGetBelgeTurleri = @"SELECT ID, BELGE_AD FROM BELGELER WHERE IsActive = 1 ORDER BY BELGE_AD";
        private const string SqlGetFirmaTipleri = @"SELECT FIRMA_TIP_ID, FIRMA_TIP_AD FROM FIRMA_TIP WHERE IsActive = 1 ORDER BY FIRMA_TIP_AD";
        private const string SqlGetKategoriler = @"SELECT ID, KATEGORI_AD FROM KATEGORILER WHERE IsActive = 1 ORDER BY KATEGORI_AD";

        private const string CEZA_KATEGORI_ID = "1";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //if (!CheckPermission(Sabitler.BELGE_TAKIP_FIRMALAR))
                    //{
                    //    return;
                    //}

                    SayfayiHazirla();
                }
            }
            catch (Exception ex)
            {
                LogError("Sayfa yüklenirken hata", ex);
                ShowToast("Sayfa yüklenirken bir hata oluştu.", "danger");
            }
        }

        private void SayfayiHazirla()
        {
            VerileriYukle();
            VarsayilanDurumlariAyarla();
        }

        private void VerileriYukle()
        {
            IlleriYukle();
            PersonelleriYukle();
            BelgeTurleriniYukle();
            FirmaTipleriniYukle();
            KategorileriYukle();
        }

        private void VarsayilanDurumlariAyarla()
        {
            ddlIlce.Enabled = false;
            txtCezaMakbuzNo.Enabled = false;
            lblCezaMakbuzNo.Enabled = false;
            rfvMakbuz.Enabled = false;
        }

        private void IlleriYukle()
        {
            try
            {
                PopulateDropDownList(ddlIl, SqlGetIller, "IL_AD", "IL_ID", addDefault: true);
                ddlIl.Items[0].Text = "İl Seçiniz";
                ddlIl.Items[0].Value = "-1";
            }
            catch (Exception ex)
            {
                LogError("İller yüklenirken hata", ex);
                ShowToast("İller yüklenirken hata oluştu.", "danger");
            }
        }

        private void IlceleriYukle(int ilId)
        {
            ddlIlce.Items.Clear();
            ddlIlce.Items.Add(new ListItem("İlçe Seçiniz", "-1"));

            if (ilId <= 0)
            {
                ddlIlce.Enabled = false;
                return;
            }

            try
            {
                var parameters = CreateParameters(("@IL_ID", ilId));
                PopulateDropDownList(ddlIlce, SqlGetIlceler, "ILCE_AD", "ILCE_ID", addDefault: true, parameters);
                ddlIlce.Items[0].Text = "İlçe Seçiniz";
                ddlIlce.Items[0].Value = "-1";
                ddlIlce.Enabled = true;
            }
            catch (Exception ex)
            {
                LogError("İlçeler yüklenirken hata", ex);
                ShowToast("İlçeler yüklenirken hata oluştu.", "danger");
            }
        }

        private void PersonelleriYukle()
        {
            try
            {
                PopulateDropDownList(ddlPersonel1, SqlGetPersoneller, "ADSOYAD", "ID", addDefault: true);
                ddlPersonel1.Items[0].Text = "Personel Seçiniz";
                ddlPersonel1.Items[0].Value = "-1";

                PopulateDropDownList(ddlPersonel2, SqlGetPersoneller, "ADSOYAD", "ID", addDefault: true);
                ddlPersonel2.Items[0].Text = "Personel Seçiniz";
                ddlPersonel2.Items[0].Value = "-1";
            }
            catch (Exception ex)
            {
                LogError("Personeller yüklenirken hata", ex);
                ShowToast("Personeller yüklenirken hata oluştu.", "danger");
            }
        }

        private void BelgeTurleriniYukle()
        {
            try
            {
                PopulateDropDownList(ddlBelgeTuru, SqlGetBelgeTurleri, "BELGE_AD", "ID", addDefault: true);
                ddlBelgeTuru.Items[0].Text = "Belge Türü Seçiniz";
                ddlBelgeTuru.Items[0].Value = "-1";
            }
            catch (Exception ex)
            {
                LogError("Belge türleri yüklenirken hata", ex);
                ShowToast("Belge türleri yüklenirken hata oluştu.", "danger");
            }
        }

        private void FirmaTipleriniYukle()
        {
            try
            {
                PopulateDropDownList(ddlFirmaTipi, SqlGetFirmaTipleri, "FIRMA_TIP_AD", "FIRMA_TIP_ID", addDefault: true);
                ddlFirmaTipi.Items[0].Text = "Firma Tipi Seçiniz";
                ddlFirmaTipi.Items[0].Value = "-1";
            }
            catch (Exception ex)
            {
                LogError("Firma tipleri yüklenirken hata", ex);
                ShowToast("Firma tipleri yüklenirken hata oluştu.", "danger");
            }
        }

        private void KategorileriYukle()
        {
            try
            {
                PopulateDropDownList(ddlKategori, SqlGetKategoriler, "KATEGORI_AD", "ID", addDefault: true);
                ddlKategori.Items[0].Text = "Kategori Seçiniz";
                ddlKategori.Items[0].Value = "-1";
            }
            catch (Exception ex)
            {
                LogError("Kategoriler yüklenirken hata", ex);
                ShowToast("Kategoriler yüklenirken hata oluştu.", "danger");
            }
        }

        protected void ddlIl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int ilId = Convert.ToInt32(ddlIl.SelectedValue);
                IlceleriYukle(ilId);
            }
            catch (Exception ex)
            {
                LogError("İl değişikliği sırasında hata", ex);
                ShowToast("İl değişikliği sırasında hata oluştu.", "danger");
            }
        }

        protected void ddlKategori_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                bool cezaKategorisiMi = ddlKategori.SelectedValue == CEZA_KATEGORI_ID;

                txtCezaMakbuzNo.Enabled = cezaKategorisiMi;
                lblCezaMakbuzNo.Enabled = cezaKategorisiMi;
                rfvMakbuz.Enabled = cezaKategorisiMi;

                if (!cezaKategorisiMi)
                {
                    txtCezaMakbuzNo.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                LogError("Kategori değişikliği sırasında hata", ex);
                ShowToast("Kategori değişikliği sırasında hata oluştu.", "danger");
            }
        }

        protected void ddlPersonel1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPersonel1.SelectedValue == ddlPersonel2.SelectedValue &&
                ddlPersonel1.SelectedValue != "-1")
            {
                ShowToast("Birinci personel ikinci personel ile aynı olamaz!", "warning");
                SetSafeDropDownValue(ddlPersonel1, "-1");
            }
        }

        protected void ddlPersonel2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPersonel2.SelectedValue == ddlPersonel1.SelectedValue &&
                ddlPersonel2.SelectedValue != "-1")
            {
                ShowToast("İkinci personel birinci personel ile aynı olamaz!", "warning");
                SetSafeDropDownValue(ddlPersonel2, "-1");
            }
        }

        protected void btnTemizle_Click(object sender, EventArgs e)
        {
            FormuTemizle();
            ShowToast("Form temizlendi.", "info");
        }

        protected void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                Page.Validate();

                if (!Page.IsValid)
                {
                    ShowToast("Lütfen zorunlu alanları doldurunuz.", "warning");
                    return;
                }

                int sonuc = KayitIslemiGerceklestir();

                if (sonuc >= 2)
                {
                    ShowToast("Kayıt başarıyla oluşturuldu.", "success");
                    LogInfo($"Yeni firma kaydı oluşturuldu - Vergi No: {txtVergiNo.Text.Trim()}");
                    FormuTemizle();
                }
                else
                {
                    ShowToast("Kayıt işlemi başarısız oldu.", "danger");
                }
            }
            catch (Exception ex)
            {
                LogError("Kayıt işlemi sırasında hata", ex);
                ShowToast("Kayıt işlemi sırasında hata oluştu: " + ex.Message, "danger");
            }
        }

        protected void cvVergiNo_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string vergiNo = args.Value.Trim();

            if (string.IsNullOrEmpty(vergiNo) ||
                !System.Text.RegularExpressions.Regex.IsMatch(vergiNo, @"^\d{10,11}$"))
            {
                args.IsValid = false;
                ((CustomValidator)source).ErrorMessage = "Geçerli bir vergi numarası giriniz (10 veya 11 haneli sayı).";
            }
            else
            {
                args.IsValid = true;
            }
        }

        protected void cvDenetimTarihi_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string tarihStr = hdnDenetimTarihi.Value;
            CustomValidator validator = (CustomValidator)source;

            if (string.IsNullOrEmpty(tarihStr))
            {
                validator.ErrorMessage = "Denetim tarihi zorunludur.";
                args.IsValid = false;
                return;
            }

            try
            {
                var parts = tarihStr.Split('.');
                if (parts.Length != 3)
                {
                    validator.ErrorMessage = "Denetim tarihi geçerli bir formatta değil (gg.aa.yyyy).";
                    args.IsValid = false;
                    return;
                }

                var secilenTarih = new DateTime(int.Parse(parts[2]), int.Parse(parts[1]), int.Parse(parts[0]));
                DateTime bugun = DateTime.Today;

                if (secilenTarih > bugun)
                {
                    validator.ErrorMessage = "Denetim tarihi gelecekte olamaz. Lütfen geçerli bir tarih seçin.";
                    args.IsValid = false;
                }
                else
                {
                    args.IsValid = true;
                }
            }
            catch (Exception)
            {
                validator.ErrorMessage = "Denetim tarihi geçerli bir formatta değil (gg.aa.yyyy).";
                args.IsValid = false;
            }
        }

        private int KayitIslemiGerceklestir()
        {
            int toplamEtkilenenSatir = 0;

            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int firmaId = FirmaKaydiOlustur(connection, transaction);

                        if (firmaId > 0)
                        {
                            toplamEtkilenenSatir++;

                            int denetimSonuc = DenetimKaydiOlustur(connection, transaction, firmaId);
                            toplamEtkilenenSatir += denetimSonuc;
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return toplamEtkilenenSatir;
        }

        private int FirmaKaydiOlustur(SqlConnection connection, SqlTransaction transaction)
        {
            using (SqlCommand cmd = new SqlCommand(SqlInsertFirma, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@IL", ddlIl.SelectedValue);
                cmd.Parameters.AddWithValue("@ILCE", ddlIlce.SelectedValue);
                cmd.Parameters.AddWithValue("@FIRMA_TIPI", ddlFirmaTipi.SelectedValue);
                cmd.Parameters.AddWithValue("@BELGE_TIPI", ddlBelgeTuru.SelectedValue);
                cmd.Parameters.AddWithValue("@VERGI_NUMARASI", txtVergiNo.Text.Trim());
                cmd.Parameters.AddWithValue("@FIRMA_ADI", txtFirmaAdi.Text.Trim());
                cmd.Parameters.AddWithValue("@FIRMA_ADRESI", txtAdres.Text.Trim());
                cmd.Parameters.AddWithValue("@KATEGORI", ddlKategori.SelectedValue);
                cmd.Parameters.AddWithValue("@TODAY", DateTime.Today);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private int DenetimKaydiOlustur(SqlConnection connection, SqlTransaction transaction, int firmaId)
        {
            using (SqlCommand cmd = new SqlCommand(SqlInsertDenetim, connection, transaction))
            {
                string personelBirlesmis = $"{ddlPersonel1.SelectedItem.Text.Trim()}, {ddlPersonel2.SelectedItem.Text.Trim()}";

                bool cezaKategorisiMi = ddlKategori.SelectedValue == CEZA_KATEGORI_ID;

                var parts = hdnDenetimTarihi.Value.Split('.');
                if (parts.Length != 3)
                    throw new FormatException("Denetim tarihi geçerli bir formatta değil.");

                var denetimTarihi = new DateTime(int.Parse(parts[2]), int.Parse(parts[1]), int.Parse(parts[0]));

                cmd.Parameters.AddWithValue("@FirmaId", firmaId);
                cmd.Parameters.AddWithValue("@Personel", personelBirlesmis);
                cmd.Parameters.AddWithValue("@Personel1", ddlPersonel1.SelectedValue);
                cmd.Parameters.AddWithValue("@Personel2", ddlPersonel2.SelectedValue);
                cmd.Parameters.AddWithValue("@BelgeTipi", ddlBelgeTuru.SelectedValue);
                cmd.Parameters.AddWithValue("@MakbuzNo", cezaKategorisiMi ? txtCezaMakbuzNo.Text.Trim() : string.Empty);
                cmd.Parameters.AddWithValue("@DenetimTarihi", denetimTarihi);
                cmd.Parameters.AddWithValue("@DenetimTuru", "İlkDenetim");

                return cmd.ExecuteNonQuery();
            }
        }

        private void FormuTemizle()
        {
            ClearFormControls(txtVergiNo, txtFirmaAdi, txtAdres, txtCezaMakbuzNo, txtDenetimTarihi,
                             ddlIl, ddlFirmaTipi, ddlBelgeTuru, ddlKategori, ddlPersonel1, ddlPersonel2);
            hdnDenetimTarihi.Value = string.Empty;

            ddlIlce.Items.Clear();
            ddlIlce.Items.Insert(0, new ListItem("İlçe Seçiniz", "-1"));
            ddlIlce.SelectedIndex = 0;

            VarsayilanDurumlariAyarla();
        }
    }
}