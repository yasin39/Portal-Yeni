using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Portal.ModulDenetim
{

    public partial class IsletmeRapor : Portal.Base.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                CheckPermission(200); // 200: Denetim Modülü Yetkisi

                DoldurDropdownlar();

                // 3. Sayfayı ilk açılışta son 50 kayıtla doldur
                BindGrid(true);
            }
        }

        /// <summary>
        /// Filtreleme için kullanılan DropDownList kontrollerini veritabanından doldurur.
        /// BasePage.PopulateDropDownList() kullanır.
        /// </summary>
        private void DoldurDropdownlar()
        {
            try
            {
                // Personel Listesi
                string personelQuery = "select Adi+' '+Soyad as Kisi from personel where Durum='Aktif' and CalismaDurumu!='Geçici Görevde Pasif Çalışan' and Statu='Memur' order by Adi asc";
                PopulateDropDownList(personel, personelQuery, "Kisi", "Kisi", false); // addDefault=false, "Hepsi" manuel eklenecek
                personel.Items.Insert(0, new ListItem("Hepsi", "Hepsi"));

                // İl Listesi
                string sehirQuery = "select sehir from sehirler where Bolge_Dahilimi=1 order by sehir asc";
                PopulateDropDownList(il, sehirQuery, "sehir", "sehir", false);
                il.Items.Insert(0, new ListItem("Hepsi", "Hepsi"));

                // Yetki Belgesi Listesi
                string ybQuery = "select Belge_Adi from yetki_belgeleri order by Belge_Adi asc";
                PopulateDropDownList(yetkibelgesi, ybQuery, "Belge_Adi", "Belge_Adi", false);
                yetkibelgesi.Items.Insert(0, new ListItem("Hepsi", "Hepsi"));
            }
            catch (Exception ex)
            {
                LogError("IsletmeRapor.DoldurDropdownlar hatası", ex);
                ShowToast("Dropdown listeleri yüklenirken bir hata oluştu.", "danger");
            }
        }

        /// <summary>
        /// Seçilen İle göre İlçe DropDownList'ini doldurur.
        /// BasePage.PopulateDropDownList() (parametreli) kullanır.
        /// </summary>
        protected void il_SelectedIndexChanged(object sender, EventArgs e)
        {
            ilce.Items.Clear();
            if (il.SelectedValue == "Hepsi" || string.IsNullOrEmpty(il.SelectedValue))
            {
                ilce.Items.Add(new ListItem("Hepsi", "Hepsi"));
                return;
            }

            try
            {
                // BasePage.CreateParameters ile parametreli sorgu
                string ilceQuery = "select ilceadi from [ililce] where iladi=@il order by ilceadi asc";
                var parameters = CreateParameters(("@il", il.SelectedValue));

                PopulateDropDownList(ilce, ilceQuery, "ilceadi", "ilceadi", false, parameters);
                ilce.Items.Insert(0, new ListItem("Hepsi", "Hepsi"));
            }
            catch (Exception ex)
            {
                LogError($"IsletmeRapor.il_SelectedIndexChanged hatası (Seçilen İl: {il.SelectedValue})", ex);
                ShowToast("İlçe listesi yüklenirken bir hata oluştu.", "danger");
            }
        }

        /// <summary>
        /// Filtre kriterlerine göre GridView'i doldurur.
        /// Güvenli, parametreli sorgu yapısı kullanır.
        /// </summary>
        /// <param name="ilkYukleme">True ise, son 50 kaydı getirir (eski listele() davranışı).</param>
        private void BindGrid(bool ilkYukleme = false)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                StringBuilder sqlSorgu = new StringBuilder();

                if (ilkYukleme)
                {
                    // Orijinal 'listele' metodundaki 'top 50' sorgusu
                    sqlSorgu.Append("SELECT TOP 50 * FROM denetimisletme WHERE 1=1 ");
                }
                else
                {
                    sqlSorgu.Append("SELECT * FROM denetimisletme WHERE 1=1 ");

                    if (!string.IsNullOrEmpty(vergino.Text))
                    {
                        sqlSorgu.Append("AND VergiNo=@VergiNo ");
                        parameters.Add(CreateParameter("@VergiNo", vergino.Text));
                    }
                    if (!string.IsNullOrEmpty(unvan.Text))
                    {
                        sqlSorgu.Append("AND Unvan LIKE @Unvan ");
                        parameters.Add(CreateParameter("@Unvan", "%" + unvan.Text.Trim() + "%"));
                    }
                    if (yetkibelgesi.SelectedValue != "Hepsi")
                    {
                        sqlSorgu.Append("AND YetkiBelgesi=@YetkiBelgesi ");
                        parameters.Add(CreateParameter("@YetkiBelgesi", yetkibelgesi.SelectedValue));
                    }
                    if (denetimturu.SelectedValue != "Hepsi")
                    {
                        sqlSorgu.Append("AND DenetimTuru=@DenetimTuru ");
                        parameters.Add(CreateParameter("@DenetimTuru", denetimturu.SelectedValue));
                    }
                    if (il.SelectedValue != "Hepsi")
                    {
                        sqlSorgu.Append("AND il=@il ");
                        parameters.Add(CreateParameter("@il", il.SelectedValue));
                    }
                    if (ilce.SelectedValue != "Hepsi" && !string.IsNullOrEmpty(ilce.SelectedValue))
                    {
                        sqlSorgu.Append("AND ilce=@ilce ");
                        parameters.Add(CreateParameter("@ilce", ilce.SelectedValue));
                    }
                    if (personel.SelectedValue != "Hepsi")
                    {
                        sqlSorgu.Append("AND (Personel1=@Personel OR Personel2=@Personel) ");
                        parameters.Add(CreateParameter("@Personel", personel.SelectedValue));
                    }
                    if (cezadurumu.SelectedValue != "Hepsi")
                    {
                        sqlSorgu.Append("AND CezaDurumu=@CezaDurumu ");
                        parameters.Add(CreateParameter("@CezaDurumu", cezadurumu.SelectedValue));
                    }

                    // Tarih filtreleri (CAST kullanarak)
                    if (!string.IsNullOrEmpty(ilktarih.Text))
                    {
                        sqlSorgu.Append("AND CAST(DenetimTarihi AS DATE) >= @ilktarih ");
                        parameters.Add(CreateParameter("@ilktarih", ilktarih.Text));
                    }
                    if (!string.IsNullOrEmpty(sontarih.Text))
                    {
                        sqlSorgu.Append("AND CAST(DenetimTarihi AS DATE) <= @sontarih ");
                        parameters.Add(CreateParameter("@sontarih", sontarih.Text));
                    }
                }

                sqlSorgu.Append(" ORDER BY id DESC");

                // Veriyi BasePage.ExecuteDataTable ile çek
                DataTable dt = ExecuteDataTable(sqlSorgu.ToString(), parameters);

                // GridView'i bağla
                GridView1.DataSource = dt;
                GridView1.DataBind();

                // Raporlama (PDF/Excel) butonlarının kullanabilmesi için sonucu Session'a kaydet
                Session["IsletmeRaporData"] = dt;

                // ASPX'teki kayıt sayısı etiketini güncelle
                lblKayitSayisi.Text = dt.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                LogError("IsletmeRapor.BindGrid hatası", ex);
                ShowToast("Veriler yüklenirken bir hata oluştu. Lütfen filtreleri kontrol edin.", "danger");
                Session["IsletmeRaporData"] = null; // Hata durumunda Session'ı temizle
            }
        }

        /// <summary>
        /// GridView veri bağlaması tamamlandığında Footer'ı (alt bilgi) ayarlar.
        /// </summary>
        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            if (GridView1.Rows.Count > 0 && GridView1.FooterRow != null)
            {
                DataTable dt = Session["IsletmeRaporData"] as DataTable;
                int toplamKayit = dt != null ? dt.Rows.Count : 0;

                GridView1.FooterRow.Cells[0].Text = "Toplam Kayıt:";
                GridView1.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                GridView1.FooterRow.Cells[1].Text = toplamKayit.ToString();
            }
        }

        protected void bulbuton_Click(object sender, EventArgs e)
        {
            // Filtreli modda (ilkYukleme=false) Grid'i doldurur
            BindGrid(false);
        }

        protected void exceleaktar_Click(object sender, EventArgs e)
        {
            // Verinin güncel olduğundan emin olmak için Session'ı kontrol et
            DataTable dt = Session["IsletmeRaporData"] as DataTable;
            if (dt == null)
            {
                // Eğer Session boşsa (örneğin, Filtrele'ye basılmadıysa),
                // önce veriyi çek, sonra tekrar session'dan al.
                BindGrid(false);
                dt = Session["IsletmeRaporData"] as DataTable;

                if (dt == null || dt.Rows.Count == 0)
                {
                    ShowToast("Excel'e aktarılacak veri bulunamadı.", "warning");
                    return;
                }
            }

            GridView1.DataSource = dt;
            GridView1.DataBind();

            ExportGridViewToExcel(GridView1, "IsletmeDenetimRaporu.xls");
        }

        protected void btnPdfAktar_Click(object sender, EventArgs e)
        {
            DataTable dt = Session["IsletmeRaporData"] as DataTable;
            if (dt == null)
            {
                BindGrid(false);
                dt = Session["IsletmeRaporData"] as DataTable;

                if (dt == null || dt.Rows.Count == 0)
                {
                    ShowToast("PDF olarak dışa aktarılacak veri bulunamadı.", "warning");
                    return;
                }
            }

            GridView1.DataSource = dt;
            GridView1.DataBind();

            if (GridView1.HeaderRow != null)
            {
                foreach (TableCell headerCell in GridView1.HeaderRow.Cells)
                {
                    headerCell.Text = HttpUtility.HtmlDecode(headerCell.Text);
                }
            }


            // ==> Değişiklik: PDF'e göndermeden önce GridView'daki Türkçe karakterleri düzelt
            foreach (GridViewRow row in GridView1.Rows)
            {
                foreach (TableCell cell in row.Cells)
                {
                    cell.Text = HttpUtility.HtmlDecode(cell.Text);
                }
            }

            // BasePage'deki hazır PDF fonksiyonunu çağır            
            string raporBaslik = "II. Bölge Müdürlüğü İşletme Denetim Raporu";
            ExportGridViewToPdf(GridView1, "IsletmeDenetimRaporu.pdf", raporBaslik);
        }

        /// <summary>
        /// GridView'i bir form içinde render etmek için gereklidir.
        /// BasePage'deki ExportGridViewToExcel ve ExportGridViewToPdf fonksiyonları
        /// 'RenderControl' kullandığı için bu metodun 'override' edilmesi zorunludur.
        /// </summary>
        public override void VerifyRenderingInServerForm(Control control)
        {
            // Bu metodun içeriğinin boş olması, hatayı engellemek için yeterlidir.
        }
    }
}