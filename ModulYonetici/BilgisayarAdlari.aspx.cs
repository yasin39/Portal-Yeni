//using OfficeOpenXml;
using Portal;
using Portal.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace ModulYonetici
{
    public partial class BilgisayarAdlari : BasePage
    {
        private const int YETKI_NO = 900;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckPermission(YETKI_NO))
                    return;

                BilgisayarTipleriniYukle();
                KayitlariListele();
                SonrakiDomainNoAta();
            }
        }

        #region Bilgisayar Tipleri Yükleme

        private void BilgisayarTipleriniYukle()
        {
            ddlBilgisayarTipi.Items.Clear();
            ddlBilgisayarTipi.Items.Add(new ListItem("Seçiniz...", ""));

            foreach (var tip in Sabitler.BilgisayarTipleri)
            {
                ddlBilgisayarTipi.Items.Add(new ListItem(tip));
            }
        }

        #endregion

        #region Listeleme İşlemleri

        private void KayitlariListele(string domainNo = null, string kisiAdi = null)
        {
            try
            {
                string query = "SELECT * FROM bilgisayar_adlari WHERE 1=1";
                List<SqlParameter> parameters = new List<SqlParameter>();

                if (!string.IsNullOrEmpty(domainNo))
                {
                    query += " AND domain_no LIKE @DomainNo";
                    parameters.Add(CreateParameter("@DomainNo", "%" + domainNo + "%"));
                }

                if (!string.IsNullOrEmpty(kisiAdi))
                {
                    query += " AND kisi_adi LIKE @KisiAdi";
                    parameters.Add(CreateParameter("@KisiAdi", "%" + kisiAdi + "%"));
                }

                query += " ORDER BY domain_no ASC";

                DataTable dt = ExecuteDataTable(query, parameters);
                BilgisayarlarGrid.DataSource = dt;
                BilgisayarlarGrid.DataBind();

                litToplamKayit.Text = dt.Rows.Count + " kayıt";
            }
            catch (Exception ex)
            {
                LogError("KayitlariListele hatası", ex);
                ShowToast("Kayıtlar listelenirken hata oluştu!", "error");
            }
        }

        protected void btnTumunuListele_Click(object sender, EventArgs e)
        {
            txtAramaDomain.Text = string.Empty;
            txtAramaKisi.Text = string.Empty;
            KayitlariListele();
        }

        protected void btnAra_Click(object sender, EventArgs e)
        {
            string domainNo = txtAramaDomain.Text.Trim();
            string kisiAdi = txtAramaKisi.Text.Trim();

            if (string.IsNullOrEmpty(domainNo) && string.IsNullOrEmpty(kisiAdi))
            {
                ShowToast("Lütfen arama kriteri giriniz!", "warning");
                return;
            }

            KayitlariListele(domainNo, kisiAdi);
        }

        #endregion

        #region Domain No Otomatik Artış

        private void SonrakiDomainNoAta()
        {
            try
            {
                string query = @"SELECT TOP 1 domain_no 
                                FROM bilgisayar_adlari 
                                WHERE domain_no LIKE 'ANKB%' 
                                ORDER BY domain_no DESC";

                DataTable dt = ExecuteDataTable(query);

                if (dt.Rows.Count > 0)
                {
                    string sonDomain = dt.Rows[0]["domain_no"].ToString();
                    string sayisalKisim = sonDomain.Replace("ANKB", "");

                    if (int.TryParse(sayisalKisim, out int sonNumara))
                    {
                        int yeniNumara = sonNumara + 1;
                        txtDomainNo.Text = "ANKB" + yeniNumara.ToString("D3");
                    }
                }
                else
                {
                    txtDomainNo.Text = "ANKB001";
                }
            }
            catch (Exception ex)
            {
                LogError("SonrakiDomainNoAta hatası", ex);
                txtDomainNo.Text = "ANKB001";
            }
        }

        #endregion

        #region CRUD İşlemleri

        protected void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                string domainNo = txtDomainNo.Text.Trim().ToUpper();
                string kisiAdi = txtKisiAdi.Text.Trim();
                string bilgisayarTipi = ddlBilgisayarTipi.SelectedValue;
                string dahiliNo = txtDahiliNo.Text.Trim();

                if (string.IsNullOrEmpty(domainNo))
                {
                    ShowToast("Domain No alanı zorunludur!", "error");
                    return;
                }

                string checkQuery = "SELECT COUNT(*) FROM bilgisayar_adlari WHERE domain_no = @DomainNo";
                var checkParams = CreateParameters(("@DomainNo", domainNo));
                int kayitSayisi = Convert.ToInt32(ExecuteScalar(checkQuery, checkParams));

                if (kayitSayisi > 0)
                {
                    ShowToast("Bu Domain No zaten kayıtlı!", "error");
                    return;
                }

                string insertQuery = @"INSERT INTO bilgisayar_adlari 
                                      (domain_no, kisi_adi, bilgisayar_tipi, dahili_no, kayit_tarihi, kayit_kullanici)
                                      VALUES (@DomainNo, @KisiAdi, @BilgisayarTipi, @DahiliNo, @KayitTarihi, @KayitKullanici)";

                var parameters = CreateParameters(
                    ("@DomainNo", domainNo),
                    ("@KisiAdi", string.IsNullOrEmpty(kisiAdi) ? (object)DBNull.Value : kisiAdi),
                    ("@BilgisayarTipi", string.IsNullOrEmpty(bilgisayarTipi) ? (object)DBNull.Value : bilgisayarTipi),
                    ("@DahiliNo", string.IsNullOrEmpty(dahiliNo) ? (object)DBNull.Value : dahiliNo),
                    ("@KayitTarihi", DateTime.Now),
                    ("@KayitKullanici", CurrentUserName)
                );

                ExecuteNonQuery(insertQuery, parameters);

                ShowToast("Kayıt başarıyla eklendi!", "success");
                FormuTemizle();
                KayitlariListele();
                SonrakiDomainNoAta();
            }
            catch (Exception ex)
            {
                LogError("btnKaydet_Click hatası", ex);
                ShowToast("Kayıt eklenirken hata oluştu!", "error");
            }
        }

        protected void btnGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["SelectedId"] == null)
                {
                    ShowToast("Güncellenecek kayıt seçilmedi!", "error");
                    return;
                }

                int id = Convert.ToInt32(ViewState["SelectedId"]);
                string domainNo = txtDomainNo.Text.Trim().ToUpper();
                string kisiAdi = txtKisiAdi.Text.Trim();
                string bilgisayarTipi = ddlBilgisayarTipi.SelectedValue;
                string dahiliNo = txtDahiliNo.Text.Trim();

                string updateQuery = @"UPDATE bilgisayar_adlari 
                                      SET domain_no = @DomainNo,
                                          kisi_adi = @KisiAdi,
                                          bilgisayar_tipi = @BilgisayarTipi,
                                          dahili_no = @DahiliNo,
                                          guncelleme_tarihi = @GuncellemeTarihi,
                                          guncelleyen_kullanici = @GuncellemeKullanici
                                      WHERE id = @Id";

                var parameters = CreateParameters(
                    ("@Id", id),
                    ("@DomainNo", domainNo),
                    ("@KisiAdi", string.IsNullOrEmpty(kisiAdi) ? (object)DBNull.Value : kisiAdi),
                    ("@BilgisayarTipi", string.IsNullOrEmpty(bilgisayarTipi) ? (object)DBNull.Value : bilgisayarTipi),
                    ("@DahiliNo", string.IsNullOrEmpty(dahiliNo) ? (object)DBNull.Value : dahiliNo),
                    ("@GuncellemeTarihi", DateTime.Now),
                    ("@GuncellemeKullanici", CurrentUserName)
                );

                ExecuteNonQuery(updateQuery, parameters);

                ShowToast("Kayıt başarıyla güncellendi!", "success");
                FormuTemizle();
                KayitlariListele();
            }
            catch (Exception ex)
            {
                LogError("btnGuncelle_Click hatası", ex);
                ShowToast("Güncelleme sırasında hata oluştu!", "error");
            }
        }

        protected void btnSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["SelectedId"] == null)
                {
                    ShowToast("Silinecek kayıt seçilmedi!", "error");
                    return;
                }

                int id = Convert.ToInt32(ViewState["SelectedId"]);

                string deleteQuery = "DELETE FROM bilgisayar_adlari WHERE id = @Id";
                var parameters = CreateParameters(("@Id", id));

                ExecuteNonQuery(deleteQuery, parameters);

                ShowToast("Kayıt başarıyla silindi!", "success");
                FormuTemizle();
                KayitlariListele();
            }
            catch (Exception ex)
            {
                LogError("btnSil_Click hatası", ex);
                ShowToast("Silme işlemi sırasında hata oluştu!", "error");
            }
        }

        protected void btnVazgec_Click(object sender, EventArgs e)
        {
            FormuTemizle();
        }

        #endregion

        #region GridView İşlemleri

        protected void BilgisayarlarGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Sec")
            {
                try
                {
                    int id = Convert.ToInt32(e.CommandArgument);
                    KayitGetir(id);
                }
                catch (Exception ex)
                {
                    LogError("BilgisayarlarGrid_RowCommand hatası", ex);
                    ShowToast("Kayıt getirilirken hata oluştu!", "error");
                }
            }
        }

        private void KayitGetir(int id)
        {
            try
            {
                string query = "SELECT * FROM bilgisayar_adlari WHERE id = @Id";
                var parameters = CreateParameters(("@Id", id));
                DataTable dt = ExecuteDataTable(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    txtDomainNo.Text = row["domain_no"].ToString();
                    txtKisiAdi.Text = row["kisi_adi"].ToString();
                    ddlBilgisayarTipi.SelectedValue = row["bilgisayar_tipi"].ToString();
                    txtDahiliNo.Text = row["dahili_no"].ToString();

                    ViewState["SelectedId"] = id;
                    SetFormModeUpdate(btnKaydet, btnGuncelle, btnSil, btnVazgec);
                }
            }
            catch (Exception ex)
            {
                LogError("KayitGetir hatası", ex);
                ShowToast("Kayıt yüklenirken hata oluştu!", "error");
            }
        }

        #endregion

        #region Excel İşlemleri

        protected void btnExcelAktar_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT domain_no AS [Domain No], kisi_adi AS [Kişi Adı], bilgisayar_tipi AS [Bilgisayar Tipi], dahili_no AS [Dahili No] FROM bilgisayar_adlari ORDER BY domain_no";
                DataTable dt = ExecuteDataTable(query);

                if (dt.Rows.Count == 0)
                {
                    ShowToast("Aktarılacak kayıt bulunamadı!", "warning");
                    return;
                }

                //ExportToExcel(dt, "BilgisayarAdlari_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"));
                ExportGridViewToExcel(BilgisayarlarGrid, "BilgisayarAdlari.xls");
            }
            catch (Exception ex)
            {
                LogError("btnExcelAktar_Click hatası", ex);
                ShowToast("Excel aktarımında hata oluştu!", "error");
            }
        }


        #endregion

        #region Yardımcı Metodlar

        private void FormuTemizle()
        {
            ClearFormControls(txtKisiAdi, txtDahiliNo);
            ResetDropDownLists(ddlBilgisayarTipi);
            ViewState["SelectedId"] = null;
            SetFormModeInsert(btnKaydet, btnGuncelle, btnSil, btnVazgec);
            SonrakiDomainNoAta();
        }

        #endregion
    }
}