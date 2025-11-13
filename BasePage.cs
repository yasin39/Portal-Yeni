using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Linq;
using ListItem = System.Web.UI.WebControls.ListItem;

namespace Portal.Base
{
    /// <summary>
    /// Tüm sayfaların türeyeceği base page sınıfı
    /// </summary>
    public class BasePage : Page
    {
        public BasePage()
        {

        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            // Performans izlemeyi başlat
            PerformanceHelper.StartMonitoring(Request.Url.AbsolutePath);

            // Geliştirme modu kontrolü,canlıya alırken web.config'de false yapacağız
            bool isDevMode = ConfigurationManager.AppSettings["DevMode"] == "true";

            if (isDevMode && (Session["Sicil"] == null || string.IsNullOrEmpty(Session["Sicil"].ToString())))
            {
                //Geliştirme için mock session değerleri ata(test kullanıcı bilgileri)
                Session["Sicil"] = "admin";
                Session["Ad"] = "II.Bölge Müdürlüğü";
                Session["Kturu"] = "Admin";
                Session["Ptipi"] = "1"; // Örnek personel tipi ( NORMAL_PERSONEL gibi sabitlerden alın)

                LogInfo("Geliştirme modu: Mock session değerleri atandı.");

                //Özellikle CİMER modülü testinde bu ek kullanıcılara ihtiyaç olacak
                //Ptipi=0 olan Cimer kullanıcısı örneği
                //Session["Sicil"] = "UB9547"; 
                //Session["Ad"] = "Banu YILMAZ BAZOĞLU"; 
                //Session["Kturu"] = "Personel"; 
                //Session["Ptipi"] = "0";

                //Ptipi=1 olan Cimer kullanıcısı örneği
                //Session["Sicil"] = "UB9141"; 
                //Session["Ad"] = "Yasin ÇINAR"; 
                //Session["Kturu"] = "Personel"; 
                //Session["Ptipi"] = "1";

                //Ptipi=2 olan Cimer kullanıcısı örneği
                //Session["Sicil"] = "UB8062";
                //Session["Ad"] = "Serkan CEBECİ";
                //Session["Kturu"] = "Personel";
                //Session["Ptipi"] = "2";

            }
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);

            // Performans izlemeyi bitir
            string sicil = Session["Sicil"]?.ToString();
            string ad = Session["Ad"]?.ToString();
            PerformanceHelper.EndMonitoring(sicil, ad);
        }

        #region Yetki Kontrolü
        /// <summary>
        /// Kullanıcının belirtilen yetki koduna sahip olup olmadığını kontrol eder.
        /// Sahip değilse hata gösterir ve ana sayfaya yönlendirir.
        /// </summary>
        protected bool CheckPermission(int yetkiNo)
        {
            // Session kontrolü: Kullanıcı giriş yapmış mı?
            string sicilNo = Session["Sicil"]?.ToString();

            if (string.IsNullOrEmpty(sicilNo))
            {
                ShowErrorAndRedirect("Oturum süreniz dolmuş veya giriş yapmamışsınız. Lütfen tekrar giriş yapınız.", "~/Login.aspx");
                return false;
            }

            // SQL ile yetki kontrolü
            try
            {
                string query = @"
                    SELECT COUNT(*) AS YetkiSayisi 
                    FROM yetki 
                    WHERE Sicil_No = @SicilNo AND Yetki_No = @YetkiNo";

                var parameters = CreateParameters(
                    ("@SicilNo", sicilNo),
                    ("@YetkiNo", yetkiNo)
                );

                DataTable dt = ExecuteDataTable(query, parameters);

                int yetkiSayisi = Convert.ToInt32(dt.Rows[0]["YetkiSayisi"]);

                if (yetkiSayisi == 0)
                {
                    ShowErrorAndRedirect($"Bu işlem için yeterli yetkiniz bulunmamaktadır. ({yetkiNo})", "~/Anasayfa.aspx");
                    LogWarning($"Yetkisiz erişim denemesi: {sicilNo} - {yetkiNo}");
                    return false;
                }

                LogInfo($"Yetki kontrolü başarılı: {sicilNo} - {yetkiNo}");
                return true;
            }
            catch (Exception ex)
            {
                LogError("Yetki kontrolü hatası", ex);
                ShowErrorAndRedirect("Yetki kontrolü sırasında bir hata oluştu. Lütfen yöneticiyle iletişime geçiniz.", "~/Anasayfa.aspx");
                return false;
            }
        }

        #endregion

        #region Dosya Tabanlı Logging 
        /// <summary>
        /// Basit dosya tabanlı loglama 
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// Bilgi mesajı logla
        /// </summary>
        public static void LogInfo(string message)
        {
            WriteLog("INFO", message);
        }

        /// <summary>
        /// Hata mesajı logla
        /// </summary>
        public static void LogError(string message)
        {
            WriteLog("ERROR", message);
        }

        /// <summary>
        /// Mesaj ve Exception'ı birlikte logla
        /// </summary>
        public static void LogError(string message, Exception ex)
        {
            //  Son satırı al
            string lastLine = GetLastStackTraceLine(ex.StackTrace);
            string fullMessage = $"{message}\n{ex.Message}\nStackTrace (Last): {lastLine}";
            if (ex.InnerException != null)
            {
                string innerLastLine = GetLastStackTraceLine(ex.InnerException.StackTrace);
                fullMessage += $"\nInner Exception: {ex.InnerException.Message}\nInner StackTrace (Last): {innerLastLine}";
            }
            fullMessage += "\n=============\n";
            WriteLog("ERROR", fullMessage);
        }

        /// <summary>
        /// Uyarı mesajı logla
        /// </summary>
        public static void LogWarning(string message)
        {
            WriteLog("WARNING", message);
        }

        /// <summary>
        /// StackTrace'in son satırını (en spesifik hata konumunu) döndürür
        /// </summary>
        private static string GetLastStackTraceLine(string stackTrace)
        {
            if (string.IsNullOrEmpty(stackTrace))
                return "N/A";

            var lines = stackTrace.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            // Son satırı al ve trim'le
            return lines.Length > 0 ? lines[lines.Length - 1].Trim() : "N/A";
        }


        /// <summary>
        /// Log dosyasına yaz (private) - ErrorLog.txt proje köküne, günlük sıfırlama ile
        /// </summary>
        private static void WriteLog(string level, string message)
        {
            try
            {
                lock (_lock)
                {
                    // 1. ErrorLog.txt dosyasını proje kök dizininde tut
                    string fileName = "ErrorLog.txt";
                    string projectRoot = AppDomain.CurrentDomain.BaseDirectory; // wwwroot veya bin klasörüne değil, proje köküne gitmek için düzeltme
                    string filePath = Path.Combine(projectRoot, fileName);

                    // 2. Dosya yoksa oluştur, varsa tarih kontrolü yap
                    bool shouldClearFile = false;

                    if (File.Exists(filePath))
                    {
                        var lastModified = File.GetLastWriteTime(filePath);
                        // Eğer dosya bugünden eskiyse (yani bir önceki gün yazılmışsa), içeriği temizle
                        if (lastModified.Date < DateTime.Today)
                        {
                            shouldClearFile = true;
                        }
                    }

                    // 3. Dosyayı temizle (gerekirse) ve yeni logu ekle
                    using (StreamWriter writer = new StreamWriter(filePath, !shouldClearFile, System.Text.Encoding.UTF8))
                    {
                        if (shouldClearFile)
                        {
                            writer.Write(string.Empty); // Dosyayı sıfırla
                            writer.Flush();
                        }

                        string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
                        writer.WriteLine(logMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log yazma hatası olursa, hata mesajını System.Diagnostics'e fallback et (sessiz hata)
                System.Diagnostics.Debug.WriteLine("LOG YAZMA HATASI: " + ex.Message);
            }
        }
        #endregion

        #region Hata,Warning mesajları ve Redirect
        /// <summary>
        /// Hata mesajı göster ve yönlendir
        /// </summary>
        protected void ShowErrorAndRedirect(string message, string redirectUrl)
        {
            string script = $"alert('{message}'); window.location='{ResolveUrl(redirectUrl)}';";
            ScriptManager.RegisterStartupScript(this, GetType(), "redirect", script, true);
        }

        /// <summary>
        /// Başarı mesajı göster ve yönlendir
        /// </summary>
        protected void ShowSuccessAndRedirect(string message, string redirectUrl)
        {
            string script = $"alert('{HttpUtility.JavaScriptStringEncode(message)}'); window.location='{ResolveUrl(redirectUrl)}';";
            ScriptManager.RegisterStartupScript(this, GetType(), "redirect", script, true);
        }


        /// <summary>
        /// Modern toast notification göster (Sağ üstte, 3 saniye)
        /// </summary>
        /// <param name="message">Gösterilecek mesaj</param>
        /// <param name="type">Toast tipi: success, danger, warning, info</param>
        protected void ShowToast(string message, string type = "success")
        {
            //  Icon mapping dictionary
            var iconMap = new Dictionary<string, string>
    {
        { "success", "fa-check-circle" },
        { "danger", "fa-exclamation-circle" },
        { "warning", "fa-exclamation-triangle" },
        { "info", "fa-info-circle" }
    };

            string icon = iconMap.ContainsKey(type) ? iconMap[type] : "fa-bell";

            //  Warning için özel stil (koyu turuncu background, beyaz text)
            string bgClass = type == "warning" ? "bg-dark-warning" : $"bg-{type}";
            string customStyle = type == "warning" ? "style='background-color: #d97706 !important;'" : "";

            string script = $@"
        var toastHtml = `
            <div class='toast align-items-center text-white {bgClass} border-0 shadow-lg' {customStyle} role='alert' aria-live='assertive' aria-atomic='true'>
                <div class='d-flex'>
                    <div class='toast-body'>
                        <i class='fas {icon} me-2'></i>{message}
                    </div>
                    <button type='button' class='btn-close btn-close-white me-2 m-auto' data-bs-dismiss='toast'></button>
                </div>
            </div>
        `;
        
        var toastContainer = document.getElementById('toastContainer');
        if (!toastContainer) {{
            toastContainer = document.createElement('div');
            toastContainer.id = 'toastContainer';
            toastContainer.className = 'toast-container position-fixed top-0 end-0 p-3';
            toastContainer.style.zIndex = '9999';
            document.body.appendChild(toastContainer);
        }}
        
        toastContainer.insertAdjacentHTML('beforeend', toastHtml);
        var toastEl = toastContainer.lastElementChild;
        var toast = new bootstrap.Toast(toastEl, {{ 
            delay: 3000,
            animation: true 
        }});
        toast.show();
        
        toastEl.addEventListener('hidden.bs.toast', function() {{
            toastEl.remove();
        }});
    ";

            ScriptManager.RegisterStartupScript(this, GetType(), "toast" + Guid.NewGuid(), script, true);
        }



        protected void ShowError(string message)
        {
            string script = $"alert(' Hata ! - {message}');";
            ScriptManager.RegisterStartupScript(this, GetType(), "message", script, true);
        }

        #endregion

        #region SQL Komutları
        /// <summary>
        /// SELECT sorgusu çalıştırır ve DataTable döner
        /// </summary>
        public static DataTable ExecuteDataTable(string query, List<SqlParameter> parameters = null)
        {
            // Sayaç artır
            PerformanceHelper.IncrementQueryCount();
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.CommandType = CommandType.Text;

                        //  Parametreleri ekle
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters.ToArray());
                        }

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            conn.Open();
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"ExecuteDataTable hatası: {ex.Message}\nQuery: {query}");
                throw;
            }

            return dt;
        }

        /// <summary>
        /// Stored Procedure çalıştırır ve DataTable döner
        /// </summary>
        public static DataTable ExecuteStoredProcedure(string procedureName, List<SqlParameter> parameters = null)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(procedureName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        //  Parametreleri ekle
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters.ToArray());
                        }

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            conn.Open();
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"ExecuteStoredProcedure hatası: {ex.Message}\nProcedure: {procedureName}");
                throw;
            }

            return dt;
        }


        /// <summary>
        /// INSERT/UPDATE/DELETE sorgusu çalıştırır, etkilenen satır sayısını döner
        /// </summary>
        public static int ExecuteNonQuery(string query, List<SqlParameter> parameters = null)
        {
            // Sayaç artır
            PerformanceHelper.IncrementQueryCount();

            int affectedRows = 0;

            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.CommandType = CommandType.Text;

                        //  Parametreleri ekle
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters.ToArray());
                        }

                        conn.Open();
                        affectedRows = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"ExecuteNonQuery hatası: {ex.Message}\nQuery: {query}");
                throw;
            }

            return affectedRows;
        }

        /// <summary>
        /// Transaction içinde INSERT/UPDATE/DELETE çalıştırır
        /// </summary>
        public static int ExecuteNonQueryWithTransaction(SqlConnection conn, SqlTransaction transaction,
            string query, List<SqlParameter> parameters = null)
        {
            // Sayaç artır
            PerformanceHelper.IncrementQueryCount();

            int affectedRows = 0;

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                {
                    cmd.CommandType = CommandType.Text;

                    //  Parametreleri ekle
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }

                    affectedRows = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                LogError($"ExecuteNonQueryWithTransaction hatası: {ex.Message}\nQuery: {query}");
                throw;
            }

            return affectedRows;
        }

        /// <summary>
        /// Tek bir değer dönen sorgu çalıştırır (COUNT, MAX, vb.)
        /// </summary>
        public static object ExecuteScalar(string query, List<SqlParameter> parameters = null)
        {
            // Sayaç artır
            PerformanceHelper.IncrementQueryCount();

            object result = null;

            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.CommandType = CommandType.Text;

                        //  Parametreleri ekle
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters.ToArray());
                        }

                        conn.Open();
                        result = cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"ExecuteScalar hatası: {ex.Message}\nQuery: {query}");
                throw;
            }

            return result;
        }

        /// <summary>
        /// INSERT yapıp SCOPE_IDENTITY() döner (yeni eklenen kaydın ID'si)
        /// </summary>
        public static int ExecuteInsertWithIdentity(string query, List<SqlParameter> parameters = null)
        {
            int newId = 0;

            try
            {
                //  Sorgunun sonuna SCOPE_IDENTITY ekle
                string queryWithIdentity = query + "; SELECT CAST(SCOPE_IDENTITY() AS INT)";

                using (SqlConnection conn = GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(queryWithIdentity, conn))
                    {
                        cmd.CommandType = CommandType.Text;

                        //  Parametreleri ekle
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters.ToArray());
                        }

                        conn.Open();
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            newId = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"ExecuteInsertWithIdentity hatası: {ex.Message}\nQuery: {query}");
                throw;
            }

            return newId;
        }

        /// <summary>
        /// SqlParameter oluşturmayı kolaylaştırır
        /// </summary>
        public static SqlParameter CreateParameter(string name, object value)
        {
            return new SqlParameter(name, value ?? DBNull.Value);
        }

        /// <summary>
        /// Birden fazla parametre oluştur
        /// </summary>
        public static List<SqlParameter> CreateParameters(params (string name, object value)[] parameters)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();

            foreach (var param in parameters)
            {
                sqlParams.Add(CreateParameter(param.name, param.value));
            }

            return sqlParams;
        }
        #endregion

        #region Connection Öğeleri

        private static readonly string _connectionString;

        static BasePage()
        {
            try
            {
                _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AnkaraPortalConnection"].ConnectionString;

                if (string.IsNullOrEmpty(_connectionString))
                {
                    throw new Exception("Connection string bulunamadı!");
                }
            }
            catch (Exception ex)
            {
                LogError($"Connection string hatası: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Yeni SQL bağlantısı oluşturur 
        /// </summary>
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        /// <summary>
        /// Açık ve hazır bir bağlantı döner 
        /// </summary>
        public static SqlConnection GetOpenConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        #endregion

        #region Çeşitli fonksiyonlar

        #region PDF Export

        /// <summary>
        /// GridView'i PDF'e export eder (iTextSharp ile - Türkçe karakter destekli)
        /// </summary>
        /// <param name="gv">Export edilecek GridView</param>
        /// <param name="dosyaAdi">PDF dosya adı</param>
        /// <param name="raporBaslik">Rapor başlığı</param>
        protected void ExportGridViewToPdf(GridView gv, string dosyaAdi = "rapor.pdf", string raporBaslik = "Rapor")
        {
            if (gv == null || gv.Rows.Count == 0)
            {
                ShowToast("Export edilecek veri bulunamadı.", "warning");
                return;
            }

            try
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", $"attachment;filename={dosyaAdi}");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentEncoding = Encoding.UTF8;
                Response.Charset = "UTF-8";

                var document = new Document(PageSize.A4.Rotate(), 20, 20, 20, 20);
                var writer = PdfWriter.GetInstance(document, Response.OutputStream);

                document.Open();

                string arialPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                string arialBoldPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arialbd.ttf");

                BaseFont bfTitle = BaseFont.CreateFont(arialBoldPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                BaseFont bfHeader = BaseFont.CreateFont(arialBoldPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                BaseFont bfCell = BaseFont.CreateFont(arialPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                var title = new Paragraph(raporBaslik, new Font(bfTitle, 16));
                title.Alignment = Element.ALIGN_CENTER;
                title.SpacingAfter = 10f;
                document.Add(title);

                var dateInfo = new Paragraph($"Rapor Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm}", new Font(bfCell, 9));
                dateInfo.Alignment = Element.ALIGN_RIGHT;
                dateInfo.SpacingAfter = 15f;
                document.Add(dateInfo);

                int kolonSayisi = gv.HeaderRow.Cells.Count;
                var table = new PdfPTable(kolonSayisi);
                table.WidthPercentage = 100;

                foreach (TableCell headerCell in gv.HeaderRow.Cells)
                {
                    var cell = new PdfPCell(new Phrase(headerCell.Text, new Font(bfHeader, 10)));
                    cell.BackgroundColor = new BaseColor(75, 123, 236);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Padding = 5;
                    table.AddCell(cell);
                }

                foreach (GridViewRow row in gv.Rows)
                {
                    foreach (TableCell dataCell in row.Cells)
                    {
                        string cellText = dataCell.Text
                            .Replace("&nbsp;", " ")
                            .Replace("&#304;", "İ")
                            .Replace("&#305;", "ı");

                        var cell = new PdfPCell(new Phrase(cellText, new Font(bfCell, 9)));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Padding = 4;
                        table.AddCell(cell);
                    }
                }

                document.Add(table);

                var footer = new Paragraph($"Toplam Kayıt: {gv.Rows.Count}", new Font(bfCell, 9));
                footer.Alignment = Element.ALIGN_RIGHT;
                footer.SpacingBefore = 10f;
                document.Add(footer);

                document.Close();
                Response.End();
            }
            catch (Exception ex)
            {
                LogError("PDF export hatası", ex);
                ShowToast("PDF oluşturulurken hata oluştu.", "danger");
            }
        }

        #endregion

        /// <summary>
        /// GridView'i Excel'e export eder (common pattern from multiple pages).
        /// </summary>
        /// <param name="gv">Export edilecek GridView</param>
        /// <param name="filename">Dosya adı (varsayılan: "export.xls")</param>
        /// <param name="sleepMs">Opsiyonel gecikme (thread sleep için)</param>
        protected void ExportGridViewToExcel(GridView gv, string filename = "export.xls", int sleepMs = 300)
        {
            if (sleepMs > 0) System.Threading.Thread.Sleep(sleepMs);

            // First column'u gizle (common pattern)
            if (gv.Columns.Count > 0) gv.Columns[0].Visible = false;

            Response.ClearContent();
            Response.AppendHeader("content-disposition", $"attachment;filename={filename}");
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter ht = new HtmlTextWriter(sw);

            // Header styling (common)
            gv.HeaderRow.Style.Add("background-color", "white");
            foreach (TableCell cell in gv.HeaderRow.Cells)
            {
                cell.Style["background-color"] = "blue"; // Tablo başlık rengi
            }

            // Row styling (common)
            foreach (GridViewRow row in gv.Rows)
            {
                row.BackColor = System.Drawing.Color.White;
                foreach (TableCell cell in row.Cells)
                {
                    cell.Style["background-color"] = "white";
                }
            }

            gv.RenderControl(ht);

            // Charset ve DOCTYPE (common for Turkish chars)
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1254");
            Response.Charset = "windows-1254";
            string doctype = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\n<head>\n<title></title>\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=windows-1254\" />\n<style>\n</style>\n</head>\n<body>\n";
            Response.Write(doctype + sw.ToString());
            Response.End();
        }

        /// <summary>
        /// DropDownList'e güvenli değer atar. Değer listede yoksa ilk öğeyi seçer.
        /// </summary>
        public void SetSafeDropDownValue(DropDownList ddl, string value)
        {
            if (ddl == null) throw new ArgumentNullException(nameof(ddl));

            if (string.IsNullOrEmpty(value))
            {
                ddl.SelectedIndex = 0;
                return;
            }

            ListItem item = ddl.Items.FindByValue(value);
            if (item != null)
            {
                ddl.SelectedValue = value;
            }
            else
            {
                // Değer listede yoksa Text ile dene
                item = ddl.Items.FindByText(value);
                if (item != null)
                {
                    ddl.SelectedValue = item.Value;
                }
                else
                {
                    // Hiçbiri yoksa ilk öğeyi seç
                    ddl.SelectedIndex = 0;
                    LogWarning($"DropDownList '{ddl.ID}' için değer bulunamadı: {value}");
                    ShowToast($"Veri uyumsuzluğu: '{value}' değeri listede yok. Varsayılan seçildi.", "warning");
                }
            }
        }


        /// <summary>
        /// Güvenli DropDownList değeri atar. Değer yoksa belirtilen varsayılanı seçer.
        /// </summary>
        /// <param name="ddl">DropDownList</param>
        /// <param name="value">Veritabanından gelen değer</param>
        /// <param name="defaultValue">Değer yoksa seçilecek değer (opsiyonel)</param>
        protected void SetSafeDropDownValue(DropDownList ddl, string value, string defaultValue = null)
        {
            if (ddl == null) throw new ArgumentNullException(nameof(ddl));

            value = value?.Trim() ?? string.Empty;

            // 1. Value ile ara
            var item = ddl.Items.FindByValue(value);
            if (item != null)
            {
                ddl.SelectedValue = value;
                return;
            }

            // 2. Text ile ara
            item = ddl.Items.FindByText(value);
            if (item != null)
            {
                ddl.SelectedValue = item.Value;
                return;
            }

            // 3. Varsayılan değer varsa onu dene
            if (!string.IsNullOrEmpty(defaultValue))
            {
                item = ddl.Items.FindByValue(defaultValue) ?? ddl.Items.FindByText(defaultValue);
                if (item != null)
                {
                    ddl.SelectedValue = item.Value;
                    return;
                }
            }

            // 4. Hiçbiri yoksa ilk öğeyi seç
            if (ddl.Items.Count > 0)
            {
                ddl.SelectedIndex = 0;
            }

            LogWarning($"DropDownList '{ddl.ID}' için değer bulunamadı: '{value}' (Varsayılan: '{defaultValue}')");
            ShowToast("warning", $"Uyumsuz veri: '{value}'. Varsayılan seçildi.");
        }



        /// <summary>
        /// DropDownList'e güvenli bir şekilde değer ekler (duplicate kontrolü ile)
        /// </summary>
        protected void AddSafeDropDownValue(DropDownList ddl, string text, string value)
        {
            if (ddl == null)
            {
                LogWarning("AddSafeDropDownValue: DropDownList null");
                return;
            }

            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(value))
            {
                return;
            }

            ListItem existingItem = ddl.Items.FindByValue(value);
            if (existingItem == null)
            {
                ddl.Items.Add(new ListItem(text, value));
            }
        }


        /// <summary>
        /// DropDownList'i veritabanından doldurur (common doldur() pattern).
        /// </summary>
        /// <param name="ddl">Doldurulacak DropDownList</param>
        /// <param name="query">SQL sorgusu (e.g., "SELECT * FROM table ORDER BY Name ASC")</param>
        /// <param name="textField">DataTextField</param>
        /// <param name="valueField">DataValueField</param>
        /// <param name="addDefault">Default item ekle (e.g., "Hepsi")</param>
        protected void PopulateDropDownList(DropDownList ddl, string query, string textField, string valueField, bool addDefault = true)
        {
            DataTable dt = ExecuteDataTable(query);
            ddl.DataSource = dt;
            ddl.DataTextField = textField;
            ddl.DataValueField = valueField;
            ddl.DataBind();

            if (addDefault)
            {
                ddl.Items.Insert(0, new ListItem("Seçiniz...", ""));
            }
        }

        /// <summary>
        /// DropDownList'i veritabanından doldurur (parametreli sorgu versiyonu).
        /// </summary>
        /// <param name="ddl">Doldurulacak DropDownList</param>
        /// <param name="query">SQL sorgusu (parametreli)</param>
        /// <param name="textField">DataTextField</param>
        /// <param name="valueField">DataValueField</param>
        /// <param name="addDefault">Default item ekle (e.g., "Seçiniz...")</param>
        /// <param name="parameters">SQL parametreleri</param>
        protected void PopulateDropDownList(DropDownList ddl, string query, string textField, string valueField, bool addDefault, List<SqlParameter> parameters)
        {
            DataTable dt = ExecuteDataTable(query, parameters);
            ddl.DataSource = dt;
            ddl.DataTextField = textField;
            ddl.DataValueField = valueField;
            ddl.DataBind();

            if (addDefault)
            {
                ddl.Items.Insert(0, new ListItem("Seçiniz...", ""));
            }
        }

        #endregion

        #region Form Management

        /// <summary>
        /// Form kontrollerini temizler (TextBox, DropDownList, CheckBox, RadioButton)
        /// </summary>
        /// <param name="controls">Temizlenecek kontroller</param>
        protected void ClearFormControls(params Control[] controls)
        {
            if (controls == null || controls.Length == 0)
                return;

            foreach (var control in controls)
            {
                if (control is TextBox textBox)
                {
                    textBox.Text = string.Empty;
                }
                else if (control is DropDownList dropDownList)
                {
                    if (dropDownList.Items.Count > 0)
                        dropDownList.SelectedIndex = 0;
                }
                else if (control is CheckBox checkBox)
                {
                    checkBox.Checked = false;
                }
                else if (control is RadioButton radioButton)
                {
                    radioButton.Checked = false;
                }
            }
        }

        /// <summary>
        /// Birden fazla DropDownList'i aynı anda sıfırlar (SelectedIndex = 0)
        /// </summary>
        /// <param name="dropdowns">Sıfırlanacak DropDownList'ler</param>
        protected void ResetDropDownLists(params DropDownList[] dropdowns)
        {
            foreach (var ddl in dropdowns)
            {
                if (ddl != null) ddl.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Formu INSERT (yeni kayıt) moduna getirir
        /// </summary>
        /// <param name="btnSave">Kaydet butonu</param>
        /// <param name="btnUpdate">Güncelle butonu</param>
        /// <param name="btnDelete">Sil butonu</param>
        /// <param name="btnCancel">Vazgeç butonu</param>
        protected void SetFormModeInsert(Button btnSave, Button btnUpdate, Button btnDelete, Button btnCancel)
        {
            if (btnSave != null) btnSave.Visible = true;
            if (btnUpdate != null) btnUpdate.Visible = false;
            if (btnDelete != null) btnDelete.Visible = false;
            if (btnCancel != null) btnCancel.Visible = false;
        }

        /// <summary>
        /// Formu UPDATE (güncelleme) moduna getirir
        /// </summary>
        /// <param name="btnSave">Kaydet butonu</param>
        /// <param name="btnUpdate">Güncelle butonu</param>
        /// <param name="btnDelete">Sil butonu</param>
        /// <param name="btnCancel">Vazgeç butonu</param>
        protected void SetFormModeUpdate(Button btnSave, Button btnUpdate, Button btnDelete, Button btnCancel)
        {
            if (btnSave != null) btnSave.Visible = false;
            if (btnUpdate != null) btnUpdate.Visible = true;
            if (btnDelete != null) btnDelete.Visible = true;
            if (btnCancel != null) btnCancel.Visible = true;
        }

        #endregion

        #region GridView Helpers

        /// <summary>
        /// GridView cell'den güvenli bir şekilde text alır (null check ve HTML decode)
        /// </summary>
        /// <param name="row">GridViewRow</param>
        /// <param name="cellIndex">Cell index'i</param>
        /// <param name="htmlDecode">HTML decode yapılsın mı?</param>
        /// <returns>Cell text değeri</returns>
        protected string GetGridViewCellTextSafe(GridViewRow row, int cellIndex, bool htmlDecode = true)
        {
            if (row == null || cellIndex < 0 || cellIndex >= row.Cells.Count)
                return string.Empty;

            string text = row.Cells[cellIndex].Text;

            if (string.IsNullOrEmpty(text) || text == "&nbsp;")
                return string.Empty;

            return htmlDecode ? Server.HtmlDecode(text) : text;
        }

        #endregion

        #region Date Formatting

        /// <summary>
        /// Tarihi Türkçe formatında string'e çevirir (dd.MM.yyyy)
        /// </summary>
        /// <param name="date">Tarih</param>
        /// <returns>Formatlanmış tarih string'i</returns>
        protected string FormatDateTurkish(DateTime? date)
        {
            if (date == null || date == DateTime.MinValue)
                return string.Empty;

            return date.Value.ToString("dd.MM.yyyy");
        }

        /// <summary>
        /// Tarih ve saati Türkçe formatında string'e çevirir (dd.MM.yyyy HH:mm)
        /// </summary>
        /// <param name="dateTime">Tarih saat</param>
        /// <returns>Formatlanmış tarih saat string'i</returns>
        protected string FormatDateTimeTurkish(DateTime? dateTime)
        {
            if (dateTime == null || dateTime == DateTime.MinValue)
                return string.Empty;

            return dateTime.Value.ToString("dd.MM.yyyy HH:mm");
        }

        /// <summary>
        /// Tarihi HTML5 input[type=date] formatında string'e çevirir (yyyy-MM-dd)
        /// </summary>
        /// <param name="date">Tarih</param>
        /// <returns>HTML formatında tarih string'i</returns>
        protected string FormatDateTimeHtml(DateTime? date)
        {
            if (date == null || date == DateTime.MinValue)
                return string.Empty;

            return date.Value.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Tarih ve saati HTML5 input[type=datetime-local] formatında string'e çevirir (yyyy-MM-ddTHH:mm)
        /// </summary>
        /// <param name="dateTime">Tarih saat</param>
        /// <returns>HTML formatında tarih saat string'i</returns>
        protected string FormatDateTimeHtmlLocal(DateTime? dateTime)
        {
            if (dateTime == null || dateTime == DateTime.MinValue)
                return string.Empty;

            return dateTime.Value.ToString("yyyy-MM-ddTHH:mm");
        }

        #endregion

        #region Session Properties

        /// <summary>
        /// Oturum açmış kullanıcının adını döndürür
        /// </summary>
        protected string CurrentUserName
        {
            get { return Session["Ad"]?.ToString() ?? "Bilinmiyor"; }
        }

        /// <summary>
        /// Oturum açmış kullanıcının sicil numarasını döndürür
        /// </summary>
        protected string CurrentUserSicil
        {
            get { return Session["Sicil"]?.ToString(); }
        }

        /// <summary>
        /// Oturum açmış kullanıcının tipini döndürür (Ptipi)
        /// </summary>
        protected int CurrentUserType
        {
            get { return Convert.ToInt32(Session["Ptipi"] ?? 0); }
        }

        #endregion

        #region Log Management

        /// <summary>
        /// Log entry model sınıfı
        /// </summary>
        public class LogEntry
        {
            public DateTime Timestamp { get; set; }
            public string Level { get; set; }
            public string Message { get; set; }
            public string LevelClass { get; set; }
            public string LevelIcon { get; set; }

            public LogEntry()
            {
                SetLevelProperties();
            }

            public void SetLevelProperties()
            {
                switch (Level)
                {
                    case "ERROR":
                        LevelClass = "danger";
                        LevelIcon = "fa-exclamation-circle";
                        break;
                    case "WARNING":
                        LevelClass = "warning";
                        LevelIcon = "fa-exclamation-triangle";
                        break;
                    case "INFO":
                        LevelClass = "info";
                        LevelIcon = "fa-info-circle";
                        break;
                    default:
                        LevelClass = "secondary";
                        LevelIcon = "fa-question-circle";
                        break;
                }
            }
        }

        /// <summary>
        /// Log istatistikleri model sınıfı
        /// </summary>
        public class LogStatistics
        {
            public int TotalCount { get; set; }
            public int ErrorCount { get; set; }
            public int WarningCount { get; set; }
            public int InfoCount { get; set; }
        }

        /// <summary>
        /// ErrorLog.txt dosyasını okur ve log kayıtlarını liste olarak döner
        /// </summary>
        public static List<LogEntry> GetLogEntries(string filterLevel = "", DateTime? filterDate = null, string searchText = "")
        {
            List<LogEntry> logEntries = new List<LogEntry>();

            try
            {
                string projectRoot = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = Path.Combine(projectRoot, "ErrorLog.txt");

                if (!File.Exists(filePath))
                {
                    LogWarning("ErrorLog.txt dosyası bulunamadı.");
                    return logEntries;
                }

                string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);

                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    LogEntry entry = ParseLogLine(line);

                    if (entry == null) continue;

                    if (!string.IsNullOrEmpty(filterLevel) && entry.Level != filterLevel)
                        continue;

                    if (filterDate.HasValue && entry.Timestamp.Date != filterDate.Value.Date)
                        continue;

                    if (!string.IsNullOrEmpty(searchText) &&
                    entry.Message.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) == -1)
                        continue;

                    logEntries.Add(entry);
                }

                logEntries = logEntries.OrderByDescending(x => x.Timestamp).ToList();
            }
            catch (Exception ex)
            {
                LogError("GetLogEntries hatası", ex);
            }

            return logEntries;
        }

        /// <summary>
        /// Log satırını parse eder ve LogEntry objesi döner
        /// </summary>
        private static LogEntry ParseLogLine(string line)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(line)) return null;

                int firstBracketEnd = line.IndexOf(']');
                if (firstBracketEnd == -1) return null;

                string dateStr = line.Substring(1, firstBracketEnd - 1).Trim();

                int secondBracketStart = line.IndexOf('[', firstBracketEnd);
                int secondBracketEnd = line.IndexOf(']', secondBracketStart);

                if (secondBracketStart == -1 || secondBracketEnd == -1) return null;

                string level = line.Substring(secondBracketStart + 1, secondBracketEnd - secondBracketStart - 1).Trim();
                string message = line.Substring(secondBracketEnd + 1).Trim();

                LogEntry entry = new LogEntry
                {
                    Timestamp = DateTime.Parse(dateStr),
                    Level = level,
                    Message = message
                };

                entry.SetLevelProperties();

                return entry;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Log istatistiklerini hesaplar
        /// </summary>
        public static LogStatistics GetLogStatistics()
        {
            LogStatistics stats = new LogStatistics();

            try
            {
                List<LogEntry> allLogs = GetLogEntries();

                stats.TotalCount = allLogs.Count;
                stats.ErrorCount = allLogs.Count(x => x.Level == "ERROR");
                stats.WarningCount = allLogs.Count(x => x.Level == "WARNING");
                stats.InfoCount = allLogs.Count(x => x.Level == "INFO");
            }
            catch (Exception ex)
            {
                LogError("GetLogStatistics hatası", ex);
            }

            return stats;
        }

        /// <summary>
        /// Log dosyasını tamamen temizler
        /// </summary>
        public static void ClearLogFile()
        {
            try
            {
                string projectRoot = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = Path.Combine(projectRoot, "ErrorLog.txt");

                if (File.Exists(filePath))
                {
                    File.WriteAllText(filePath, string.Empty);
                    LogInfo("Log dosyası temizlendi.");
                }
            }
            catch (Exception ex)
            {
                LogError("ClearLogFile hatası", ex);
            }
        }

        #endregion

    }
}