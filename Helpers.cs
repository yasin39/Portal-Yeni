using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI.WebControls;
using Portal.Base;

namespace Portal
{
    public class Helpers
    {
        #region Personnel & Organization

        /// <summary>
        /// Aktif personelleri DropDownList'e yükler
        /// </summary>
        /// <param name="ddl">Doldurulacak DropDownList</param>
        /// <param name="defaultText">Varsayılan metin (örn: "Personel Seçiniz")</param>
        /// <param name="addEmptyItem">Boş item eklensin mi?</param>
        public static void LoadActivePersonnel(DropDownList ddl, string defaultText = "Personel Seçiniz", bool addEmptyItem = true)
        {
            try
            {
                string query = @"SELECT Adi + ' ' + Soyad AS Kisi
                                FROM personel
                                WHERE Durum = 'Aktif'
                                AND CalismaDurumu != 'Geçici Görevde Pasif Çalışan'
                                AND Statu = 'Memur'
                                ORDER BY Adi ASC";

                DataTable dt = BasePage.ExecuteDataTable(query);

                ddl.Items.Clear();

                if (addEmptyItem)
                {
                    ddl.Items.Insert(0, new ListItem(defaultText, ""));
                }

                foreach (DataRow row in dt.Rows)
                {
                    ddl.Items.Add(new ListItem(row["Kisi"].ToString()));
                }
            }
            catch (Exception ex)
            {
                BasePage.LogError("LoadActivePersonnel hatası", ex);
                throw;
            }
        }

        /// <summary>
        /// İlleri DropDownList'e yükler (Bolge_Dahilimi = 1 olanlar)
        /// </summary>
        /// <param name="ddl">Doldurulacak DropDownList</param>
        /// <param name="defaultText">Varsayılan metin</param>
        public static void LoadProvinces(DropDownList ddl, string defaultText = "İl Seçiniz")
        {
            try
            {
                string query = @"SELECT sehir FROM sehirler
                                WHERE Bolge_Dahilimi = 1
                                ORDER BY sehir ASC";

                DataTable dt = BasePage.ExecuteDataTable(query);

                ddl.Items.Clear();
                ddl.Items.Insert(0, new ListItem(defaultText, ""));

                foreach (DataRow row in dt.Rows)
                {
                    ddl.Items.Add(new ListItem(row["sehir"].ToString()));
                }
            }
            catch (Exception ex)
            {
                BasePage.LogError("LoadProvinces hatası", ex);
                throw;
            }
        }

        /// <summary>
        /// Seçili ile göre ilçeleri DropDownList'e yükler
        /// </summary>
        /// <param name="ddl">Doldurulacak DropDownList</param>
        /// <param name="provinceId">İl adı</param>
        /// <param name="defaultText">Varsayılan metin</param>
        public static void LoadDistricts(DropDownList ddl, string provinceId, string defaultText = "İlçe Seçiniz")
        {
            try
            {
                string query = @"SELECT ilceadi FROM ililce
                                WHERE iladi = @IlAdi
                                ORDER BY ilceadi ASC";

                var parameters = BasePage.CreateParameters(("@IlAdi", provinceId));
                DataTable dt = BasePage.ExecuteDataTable(query, parameters);

                ddl.Items.Clear();
                ddl.Items.Insert(0, new ListItem(defaultText, ""));

                foreach (DataRow row in dt.Rows)
                {
                    ddl.Items.Add(new ListItem(row["ilceadi"].ToString()));
                }
            }
            catch (Exception ex)
            {
                BasePage.LogError("LoadDistricts hatası", ex);
                throw;
            }
        }

        /// <summary>
        /// Firmaları DropDownList'e yükler
        /// </summary>
        /// <param name="ddl">Doldurulacak DropDownList</param>
        /// <param name="tableName">Firma tablosu adı (varsayılan: cimer_firmalar)</param>
        public static void LoadCompanies(DropDownList ddl, string tableName = "cimer_firmalar")
        {
            try
            {
                string query = $"SELECT id, Firma_Unvan FROM {tableName} ORDER BY Firma_Unvan ASC";
                DataTable dt = BasePage.ExecuteDataTable(query);

                ddl.DataSource = dt;
                ddl.DataTextField = "Firma_Unvan";
                ddl.DataValueField = "id";
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("Firma Seçiniz", ""));
            }
            catch (Exception ex)
            {
                BasePage.LogError("LoadCompanies hatası", ex);
                throw;
            }
        }

        #endregion

        #region CIMER Specific

        /// <summary>
        /// Evrak geçmişini HTML tablosu olarak oluşturur
        /// </summary>
        /// <param name="documentId">Doküman ID (örn: Basvuru_No)</param>
        /// <param name="historyTableName">Geçmiş tablosu adı (varsayılan: cimer_basvuru_hareketleri)</param>
        /// <param name="documentIdColumn">Doküman ID kolonunun adı (varsayılan: Basvuru_id)</param>
        /// <returns>HTML tablo string'i</returns>
        public static string BuildDocumentHistoryHtml(string documentId,
                                                      string historyTableName = "cimer_basvuru_hareketleri",
                                                      string documentIdColumn = "Basvuru_id")
        {
            try
            {
                string query = $@"SELECT Sevk_Eden, Teslim_Alan, Tarih, Aciklama, islem_Aciklama
                                 FROM {historyTableName}
                                 WHERE {documentIdColumn} = @DocumentId
                                 ORDER BY id DESC";

                var parameters = BasePage.CreateParameters(("@DocumentId", documentId));
                DataTable dt = BasePage.ExecuteDataTable(query, parameters);

                if (dt.Rows.Count == 0)
                {
                    return "<div class='alert alert-info'>Henüz hareket kaydı bulunmamaktadır.</div>";
                }

                StringBuilder htmlTable = new StringBuilder();
                htmlTable.Append("<div class='table-responsive'>");
                htmlTable.Append("<table class='table table-bordered table-striped table-sm'>");
                htmlTable.Append("<thead class='table-dark'>");
                htmlTable.Append("<tr>");
                htmlTable.Append("<th>Sevk Eden</th>");
                htmlTable.Append("<th>Teslim Alan</th>");
                htmlTable.Append("<th>Tarih</th>");
                htmlTable.Append("<th>Açıklama</th>");
                htmlTable.Append("<th>İşlem Açıklama</th>");
                htmlTable.Append("</tr>");
                htmlTable.Append("</thead>");
                htmlTable.Append("<tbody>");

                foreach (DataRow row in dt.Rows)
                {
                    htmlTable.Append("<tr>");
                    htmlTable.Append($"<td>{row["Sevk_Eden"]}</td>");
                    htmlTable.Append($"<td>{row["Teslim_Alan"]}</td>");
                    htmlTable.Append($"<td>{Convert.ToDateTime(row["Tarih"]):dd.MM.yyyy HH:mm}</td>");
                    htmlTable.Append($"<td>{row["Aciklama"]}</td>");
                    htmlTable.Append($"<td>{row["islem_Aciklama"]}</td>");
                    htmlTable.Append("</tr>");
                }

                htmlTable.Append("</tbody>");
                htmlTable.Append("</table>");
                htmlTable.Append("</div>");

                return htmlTable.ToString();
            }
            catch (Exception ex)
            {
                BasePage.LogError("BuildDocumentHistoryHtml hatası", ex);
                return "<div class='alert alert-danger'>Geçmiş bilgisi yüklenirken hata oluştu.</div>";
            }
        }

        /// <summary>
        /// CİMER başvuru hareketine yeni kayıt ekler
        /// </summary>
        /// <param name="basvuruNo">Başvuru numarası</param>
        /// <param name="sevkEden">Sevk eden kullanıcı</param>
        /// <param name="teslimAlan">Teslim alan kullanıcı</param>
        /// <param name="aciklama">Açıklama</param>
        /// <param name="islemTuru">İşlem türü (1=HAVALE, 2=CEVAP, vb.)</param>
        /// <param name="islemAciklama">İşlem açıklama metni</param>
        public static void InsertCimerMovement(string basvuruNo, string sevkEden, string teslimAlan,
                                              string aciklama, int islemTuru, string islemAciklama)
        {
            try
            {
                string query = @"INSERT INTO cimer_basvuru_hareketleri
                                (Basvuru_id, Sevk_Eden, Teslim_Alan, Tarih, Aciklama, islem_turu, islem_Aciklama)
                                VALUES (@BasvuruId, @SevkEden, @TeslimAlan, @Tarih, @Aciklama, @IslemTuru, @IslemAciklama)";

                var parameters = BasePage.CreateParameters(
                    ("@BasvuruId", basvuruNo),
                    ("@SevkEden", sevkEden),
                    ("@TeslimAlan", teslimAlan),
                    ("@Tarih", DateTime.Now),
                    ("@Aciklama", aciklama ?? string.Empty),
                    ("@IslemTuru", islemTuru),
                    ("@IslemAciklama", islemAciklama)
                );

                BasePage.ExecuteNonQuery(query, parameters);
                BasePage.LogInfo($"CİMER hareket eklendi: Başvuru {basvuruNo}, İşlem: {islemAciklama}");
            }
            catch (Exception ex)
            {
                BasePage.LogError("InsertCimerMovement hatası", ex);
                throw;
            }
        }

        /// <summary>
        /// CİMER başvuru hareketine yeni kayıt ekler (Transaction destekli versiyon)
        /// </summary>
        /// <param name="connection">Açık SQL bağlantısı</param>
        /// <param name="transaction">Aktif transaction</param>
        /// <param name="basvuruNo">Başvuru numarası</param>
        /// <param name="sevkEden">Sevk eden kullanıcı</param>
        /// <param name="teslimAlan">Teslim alan kullanıcı</param>
        /// <param name="aciklama">Açıklama</param>
        /// <param name="islemTuru">İşlem türü (1=HAVALE, 2=CEVAP, vb.)</param>
        /// <param name="islemAciklama">İşlem açıklama metni</param>
        public static void InsertCimerMovement(SqlConnection connection, SqlTransaction transaction,
                                              string basvuruNo, string sevkEden, string teslimAlan,
                                              string aciklama, int islemTuru, string islemAciklama)
        {
            try
            {
                string query = @"INSERT INTO cimer_basvuru_hareketleri
                                (Basvuru_id, Sevk_Eden, Teslim_Alan, Tarih, Aciklama, islem_turu, islem_Aciklama)
                                VALUES (@BasvuruId, @SevkEden, @TeslimAlan, @Tarih, @Aciklama, @IslemTuru, @IslemAciklama)";

                var parameters = BasePage.CreateParameters(
                    ("@BasvuruId", basvuruNo),
                    ("@SevkEden", sevkEden),
                    ("@TeslimAlan", teslimAlan),
                    ("@Tarih", DateTime.Now),
                    ("@Aciklama", aciklama ?? string.Empty),
                    ("@IslemTuru", islemTuru),
                    ("@IslemAciklama", islemAciklama)
                );

                BasePage.ExecuteNonQueryWithTransaction(connection, transaction, query, parameters);
                BasePage.LogInfo($"CİMER hareket eklendi (Transaction): Başvuru {basvuruNo}, İşlem: {islemAciklama}");
            }
            catch (Exception ex)
            {
                BasePage.LogError("InsertCimerMovement (Transaction) hatası", ex);
                throw;
            }
        }

        /// <summary>
        /// CİMER başvuru kaydını günceller (dinamik alan güncelleme)
        /// </summary>
        /// <param name="basvuruNo">Başvuru numarası</param>
        /// <param name="fieldsToUpdate">Güncellenecek alanlar (kolon adı, değer)</param>
        public static void UpdateCimerApplication(string basvuruNo, Dictionary<string, object> fieldsToUpdate)
        {
            try
            {
                if (fieldsToUpdate == null || fieldsToUpdate.Count == 0)
                {
                    throw new ArgumentException("Güncellenecek alan belirtilmedi.");
                }

                // Dinamik UPDATE query oluştur
                StringBuilder queryBuilder = new StringBuilder("UPDATE cimer_basvurular SET ");
                List<SqlParameter> parameters = new List<SqlParameter>();

                int index = 0;
                foreach (var field in fieldsToUpdate)
                {
                    if (index > 0) queryBuilder.Append(", ");

                    string paramName = $"@Param{index}";
                    queryBuilder.Append($"{field.Key} = {paramName}");
                    parameters.Add(BasePage.CreateParameter(paramName, field.Value));

                    index++;
                }

                queryBuilder.Append(" WHERE Basvuru_No = @BasvuruNo");
                parameters.Add(BasePage.CreateParameter("@BasvuruNo", basvuruNo));

                BasePage.ExecuteNonQuery(queryBuilder.ToString(), parameters);
                BasePage.LogInfo($"CİMER başvuru güncellendi: {basvuruNo}");
            }
            catch (Exception ex)
            {
                BasePage.LogError("UpdateCimerApplication hatası", ex);
                throw;
            }
        }

        #endregion

        #region Audit Helpers

        /// <summary>
        /// INSERT işlemleri için audit alanlarını döndürür (KayitTarihi, KayitKullanici)
        /// </summary>
        /// <param name="kullaniciAdi">Kullanıcı adı (null ise Session'dan alınır)</param>
        /// <returns>Audit field'ları içeren Dictionary</returns>
        public static Dictionary<string, object> GetAuditFieldsForInsert(string kullaniciAdi = null)
        {
            string kullanici = kullaniciAdi ?? System.Web.HttpContext.Current?.Session["Ad"]?.ToString() ?? "Sistem";

            return new Dictionary<string, object>
            {
                { "Kayit_Tarihi", DateTime.Now },
                { "Kayit_Kullanici", kullanici }
            };
        }

        /// <summary>
        /// UPDATE işlemleri için audit alanlarını döndürür (GuncellemeTarihi, GuncellemeKullanici)
        /// </summary>
        /// <param name="kullaniciAdi">Kullanıcı adı (null ise Session'dan alınır)</param>
        /// <returns>Audit field'ları içeren Dictionary</returns>
        public static Dictionary<string, object> GetAuditFieldsForUpdate(string kullaniciAdi = null)
        {
            string kullanici = kullaniciAdi ?? System.Web.HttpContext.Current?.Session["Ad"]?.ToString() ?? "Sistem";

            return new Dictionary<string, object>
            {
                { "Guncelleme_Tarihi", DateTime.Now },
                { "Guncelleyen_Kullanici", kullanici }
            };
        }

        /// <summary>
        /// Mevcut SqlParameter listesine audit parametrelerini ekler
        /// </summary>
        /// <param name="existingParams">Mevcut parametre listesi</param>
        /// <param name="isUpdate">Update işlemi mi? (false ise Insert)</param>
        /// <param name="kullaniciAdi">Kullanıcı adı</param>
        /// <returns>Audit parametrelerini içeren yeni liste</returns>
        public static List<SqlParameter> AddAuditParameters(List<SqlParameter> existingParams,
                                                            bool isUpdate = false,
                                                            string kullaniciAdi = null)
        {
            if (existingParams == null)
                existingParams = new List<SqlParameter>();

            var auditFields = isUpdate ? GetAuditFieldsForUpdate(kullaniciAdi) : GetAuditFieldsForInsert(kullaniciAdi);

            foreach (var field in auditFields)
            {
                existingParams.Add(BasePage.CreateParameter($"@{field.Key}", field.Value));
            }

            return existingParams;
        }

        #endregion

        #region Security Helpers

        /// <summary>
        /// Parolayı SHA256 ile hashler
        /// </summary>
        /// <param name="password">Hashlenecek parola</param>
        /// <returns>Hashlenmiş parola (64 karakter hex)</returns>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            using (System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Girilen parolanın hash'i ile veritabanındaki hash'i karşılaştırır
        /// </summary>
        /// <param name="girilenParola">Kullanıcının girdiği parola</param>
        /// <param name="veritabaniHash">Veritabanındaki hash</param>
        /// <returns>Eşleşme durumu</returns>
        public static bool VerifyPassword(string girilenParola, string veritabaniHash)
        {
            if (string.IsNullOrEmpty(girilenParola) || string.IsNullOrEmpty(veritabaniHash))
                return false;

            string girilenHash = HashPassword(girilenParola);
            return girilenHash.Equals(veritabaniHash, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        /// <summary>
        /// Veritabanından sistem parametresi okur
        /// </summary>
        /// <param name="parametreAdi">Parametre adı</param>
        /// <param name="defaultDeger">Parametre bulunamazsa döndürülecek varsayılan değer</param>
        /// <returns>Parametre değeri</returns>
        public static string GetSistemParametresi(string parametreAdi, string defaultDeger = "")
        {
            try
            {
                string query = @"
            SELECT Parametre_Degeri 
            FROM sistem_parametreleri 
            WHERE Parametre_Adi = @ParametreAdi";

                var parameters = BasePage.CreateParameters(
                    ("@ParametreAdi", parametreAdi)
                );

                DataTable dt = BasePage.ExecuteDataTable(query, parameters);

                if (dt.Rows.Count > 0 && dt.Rows[0]["Parametre_Degeri"] != DBNull.Value)
                {
                    return dt.Rows[0]["Parametre_Degeri"].ToString();
                }

                BasePage.LogWarning($"Sistem parametresi bulunamadı: {parametreAdi}, varsayılan döndürülüyor.");
                return defaultDeger;
            }
            catch (Exception ex)
            {
                BasePage.LogError($"GetSistemParametresi hatası: {parametreAdi}", ex);
                return defaultDeger;
            }
        }
    }
}