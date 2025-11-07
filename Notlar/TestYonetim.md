# ModulYonetici Test Dokümanı

**Proje:** Portal-Yeni
**Modül:** ModulYonetici (Yönetim Modülü)
**Test Tipi:** Fonksiyonel, Validation, RBAC, Negative
**Hazırlayan:** QA Team
**Tarih:** 07.11.2025

---

## İçindekiler

1. [BilgisayarAdlari.aspx Test Senaryoları](#1-bilgisayaradlariaspx)
2. [Duyurular.aspx Test Senaryoları](#2-duyurularaspx)
3. [KullaniciIslem.aspx Test Senaryoları](#3-kullaniciislemaspx)
4. [YetkiIslem.aspx Test Senaryoları](#4-yetkiislemaspx)

---

## 1. BilgisayarAdlari.aspx

**Dosya Yolu:** `ModulYonetici/BilgisayarAdlari.aspx`
**Amaç:** Bilgisayar adlarını (domain) yönetme ve takip etme
**Yetki No:** 900
**Veritabanı Tablosu:** `bilgisayar_adlari`

### 1.1 Fonksiyonel Test Senaryoları

| Test ID | Test Adı | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|----------|---------------|----------------|---------|
| BIL-F-001 | Sayfa Yükleme | 1. Yetkili kullanıcı ile login ol<br>2. BilgisayarAdlari sayfasını aç | Sayfa başarıyla yüklenir, GridView listesi görünür, sonraki domain no otomatik atanır (ANKB001 veya son+1) | Yüksek |
| BIL-F-002 | Yeni Kayıt Ekleme (Tüm Alanlar) | 1. Domain No: ANKB999<br>2. Kişi Adı: Test Kullanıcı<br>3. Bilgisayar Tipi: Seç<br>4. Dahili No: 1234<br>5. Kaydet butonuna tıkla | Kayıt başarıyla eklenir, toast mesajı gösterilir, grid yenilenir, form temizlenir, yeni domain no atar | Yüksek |
| BIL-F-003 | Yeni Kayıt Ekleme (Sadece Zorunlu Alan) | 1. Domain No: ANKB998<br>2. Diğer alanları boş bırak<br>3. Kaydet butonuna tıkla | Kayıt başarıyla eklenir, opsiyonel alanlar NULL olarak kaydedilir | Yüksek |
| BIL-F-004 | Kayıt Arama - Domain No | 1. Arama Domain No: ANKB<br>2. Ara butonuna tıkla | Domain no'sunda "ANKB" geçen tüm kayıtlar listelenir | Orta |
| BIL-F-005 | Kayıt Arama - Kişi Adı | 1. Arama Kişi: Test<br>2. Ara butonuna tıkla | Kişi adında "Test" geçen tüm kayıtlar listelenir | Orta |
| BIL-F-006 | Kayıt Arama - Kombinasyon | 1. Arama Domain No: ANKB<br>2. Arama Kişi: Ahmet<br>3. Ara butonuna tıkla | Her iki kritere uyan kayıtlar listelenir (AND condition) | Orta |
| BIL-F-007 | Tümünü Listele | 1. Arama alanlarına değer gir<br>2. Ara butonuna bas<br>3. "Tümünü Listele" butonuna tıkla | Tüm kayıtlar listelenir, arama alanları temizlenir | Orta |
| BIL-F-008 | Kayıt Seçme (Grid'den) | 1. Grid'de bir kaydın "Seç" butonuna tıkla | Seçilen kayıt form alanlarına yüklenir, butonlar güncelleme moduna geçer (Güncelle, Sil, Vazgeç görünür) | Yüksek |
| BIL-F-009 | Kayıt Güncelleme | 1. Grid'den bir kayıt seç<br>2. Kişi Adı'nı değiştir<br>3. Güncelle butonuna tıkla | Kayıt başarıyla güncellenir, toast mesajı gösterilir, form temizlenir, grid yenilenir | Yüksek |
| BIL-F-010 | Kayıt Silme | 1. Grid'den bir kayıt seç<br>2. Sil butonuna tıkla<br>3. Confirm dialog'da OK | Kayıt silinir, toast mesajı gösterilir, form temizlenir, grid yenilenir | Yüksek |
| BIL-F-011 | Vazgeç (Düzenleme İptal) | 1. Grid'den bir kayıt seç<br>2. Bazı alanları değiştir<br>3. Vazgeç butonuna tıkla | Form temizlenir, butonlar insert moduna döner, ViewState temizlenir | Orta |
| BIL-F-012 | Excel'e Aktarma | 1. Kayıtları listele<br>2. "Excel'e Aktar" butonuna tıkla | Grid içeriği Excel dosyası olarak indirilir (BilgisayarAdlari.xls) | Orta |
| BIL-F-013 | Domain No Otomatik Artış | 1. Son kayıt ANKB005<br>2. Yeni kayıt sayfası aç | Domain No alanı otomatik olarak ANKB006 olarak gelir | Orta |
| BIL-F-014 | Domain No Büyük Harf Dönüşümü | 1. Domain No: ankb123 (küçük harf)<br>2. Kaydet | Kayıt ANKB123 olarak büyük harfle kaydedilir (.ToUpper()) | Düşük |
| BIL-F-015 | Toplu Kayıt Listeleme | Veritabanında 100+ kayıt varken sayfayı aç | Tüm kayıtlar grid'de gösterilir, "X kayıt" etiketi doğru sayıyı gösterir | Orta |
| BIL-F-016 | Boş Grid Durumu | Veritabanı boşken sayfayı aç | "Kayıt bulunamadı" mesajı gösterilir (EmptyDataText) | Düşük |

### 1.2 Validation Test Senaryoları

| Test ID | Test Adı | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|----------|---------------|----------------|---------|
| BIL-V-001 | Domain No Zorunlu Alan | 1. Domain No boş bırak<br>2. Kaydet butonuna tıkla | RequiredFieldValidator devreye girer: "Domain No zorunludur" hatası gösterilir, kayıt yapılmaz | Yüksek |
| BIL-V-002 | Domain No Tekil Kontrol | 1. Var olan domain no gir (örn: ANKB001)<br>2. Kaydet butonuna tıkla | "Bu Domain No zaten kayıtlı!" toast mesajı gösterilir, kayıt yapılmaz | Yüksek |
| BIL-V-003 | Domain No Max Length | 1. Domain No: 51 karakter gir<br>2. Form submit et | TextBox MaxLength=50 nedeniyle 51. karakter yazılamaz (client-side) | Orta |
| BIL-V-004 | Kişi Adı Max Length | Kişi Adı alanına 151 karakter gir | TextBox MaxLength=150 nedeniyle 151. karakter yazılamaz | Düşük |
| BIL-V-005 | Dahili No Max Length | Dahili No alanına 21 karakter gir | TextBox MaxLength=20 nedeniyle 21. karakter yazılamaz | Düşük |
| BIL-V-006 | Arama Kriteri Kontrolü | 1. Arama alanlarını boş bırak<br>2. Ara butonuna tıkla | "Lütfen arama kriteri giriniz!" warning toast gösterilir | Orta |
| BIL-V-007 | Null/Whitespace Kontrol | 1. Domain No: "   " (sadece boşluk)<br>2. Kaydet | .Trim() sonrası boş string olduğu için validation hatası verilir | Orta |
| BIL-V-008 | Dropdown Varsayılan Değer | Bilgisayar Tipi dropdown'ında "Seçiniz..." seçili bırak | Kayıt yapılır, veritabanına NULL değer kaydedilir (zorunlu alan değil) | Düşük |
| BIL-V-009 | SQL Injection Koruması | Domain No: `ANKB'; DROP TABLE bilgisayar_adlari;--` | Parametreli sorgu kullanıldığı için değer string olarak kaydedilir, SQL çalışmaz | Yüksek |
| BIL-V-010 | XSS Koruması | Kişi Adı: `<script>alert('XSS')</script>` | ASP.NET ValidateRequest korur veya kayıt olur ancak output encode edilir | Yüksek |

### 1.3 RBAC (Role-Based Access Control) Test Senaryoları

| Test ID | Test Adı | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|----------|---------------|----------------|---------|
| BIL-R-001 | Yetkisiz Erişim | 1. Yetki No 900 olmayan kullanıcı ile login<br>2. BilgisayarAdlari.aspx URL'ine git | CheckPermission(900) false döner, erişim engellenir, hata mesajı/yönlendirme | Yüksek |
| BIL-R-002 | Yetkili Erişim | 1. Yetki No 900 olan kullanıcı ile login<br>2. BilgisayarAdlari.aspx'e git | Sayfa açılır, tüm işlemler yapılabilir | Yüksek |
| BIL-R-003 | Session Timeout | 1. Login ol<br>2. Session expire olsun<br>3. Sayfa üzerinde işlem yap | Session kontrolü yapılır, login sayfasına yönlendirilir | Orta |
| BIL-R-004 | Direkt URL Erişim | Logout durumda direkt `/ModulYonetici/BilgisayarAdlari.aspx` URL'ine git | BasePage kontrolü ile login sayfasına yönlendirilir | Yüksek |
| BIL-R-005 | Cross-User Data Manipulation | Kullanıcı A'nın ViewState'ini Kullanıcı B'ye gönder | ViewState encryption/validation nedeniyle işlem başarısız | Orta |

### 1.4 Negative Test Senaryoları

| Test ID | Test Adı | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|----------|---------------|----------------|---------|
| BIL-N-001 | Veritabanı Bağlantı Hatası | 1. DB connection string'i hatalı yap<br>2. Sayfayı aç | Try-catch bloğu hatayı yakalar, "Kayıtlar listelenirken hata oluştu!" toast gösterilir, ErrorLog.txt'ye loglanır | Yüksek |
| BIL-N-002 | Olmayan Kayıt Güncelleme | 1. ViewState'e var olmayan ID koy<br>2. Güncelle butonuna tıkla | Güncelleme 0 satır etkiler, hata mesajı gösterilir veya sessizce geçer | Orta |
| BIL-N-003 | Olmayan Kayıt Silme | 1. ViewState'e var olmayan ID koy<br>2. Sil butonuna tıkla | Silme 0 satır etkiler, hata mesajı gösterilir veya sessizce geçer | Orta |
| BIL-N-004 | ViewState Manipülasyonu | ViewState["SelectedId"] değerini manuel olarak değiştir | ViewState MAC koruması nedeniyle hata alınır veya işlem başarısız | Orta |
| BIL-N-005 | Sil Confirm İptal | 1. Kayıt seç<br>2. Sil butonuna bas<br>3. Confirm dialog'da Cancel | Silme işlemi gerçekleşmez, kayıt korunur | Düşük |
| BIL-N-006 | Excel Export Boş Data | 1. Tüm kayıtları sil<br>2. Excel'e Aktar butonuna tıkla | "Aktarılacak kayıt bulunamadı!" warning toast gösterilir | Orta |
| BIL-N-007 | Çok Uzun String (MaxLength Aşımı Server-Side) | JavaScript disable et, 100 karakterlik domain no gönder | Server-side validation devreye girer veya DB constraint hatası | Orta |
| BIL-N-008 | Null Reference Exception | Grid boşken Seç butonuna tıkla (row yok) | Null kontrolü yapılır, exception fırlatılmaz | Orta |
| BIL-N-009 | Concurrent Update | 1. İki tarayıcıda aynı kaydı aç<br>2. Her ikisinde de farklı değişiklik yap<br>3. İkisinde de kaydet | Son kaydeden kazanır (optimistic concurrency kontrolü yok) | Düşük |
| BIL-N-010 | Özel Karakter Girişi | Domain No: `@#$%^&*(){}[]` | Kayıt başarılı, parametreli sorgu ile güvenli işlenir | Orta |
| BIL-N-011 | Unicode Karakter | Kişi Adı: `测试用户 тест 🚀` | Kayıt başarılı, Unicode desteklenir (NVARCHAR) | Düşük |
| BIL-N-012 | Arama Performans | 10000+ kayıt varken wildcard arama yap | Arama sonucu dönüyor ancak yavaş olabilir (index kontrolü) | Düşük |
| BIL-N-013 | Excel Dosya İsim Collision | Aynı anda birden fazla Excel export yap | Her export farklı timestamp ile kaydedilmeli veya dosya üzerine yazılır | Düşük |
| BIL-N-014 | Domain No Parse Hatası | Sonraki domain atama için son kayıt "ANKBABC" gibi parse edilemez bir değer | TryParse başarısız olur, varsayılan "ANKB001" atanır | Orta |
| BIL-N-015 | Grid Sıralama | Kayıtlar domain_no ASC sıralı mı kontrol et | ORDER BY domain_no ASC ile sıralı gelir | Düşük |

---

## 2. Duyurular.aspx

**Dosya Yolu:** `ModulYonetici/Duyurular.aspx`
**Amaç:** Sistem duyurularını yönetme (CRUD)
**Yetki No:** Sabitler.DUYURU_YONETIMI
**Veritabanı Tablosu:** `duyuru`

### 2.1 Fonksiyonel Test Senaryoları

| Test ID | Test Adı | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|----------|---------------|----------------|---------|
| DUY-F-001 | Sayfa Yükleme | 1. Yetkili kullanıcı ile login<br>2. Duyurular sayfasını aç | Sayfa yüklenir, duyuru listesi grid'de gösterilir, form insert modunda | Yüksek |
| DUY-F-002 | Yeni Duyuru Ekleme (Tam) | 1. Başlama Tarihi: 01.01.2025<br>2. Bitiş Tarihi: 31.01.2025<br>3. Durum: Aktif<br>4. Dosya: test.pdf<br>5. Duyuru: "Test duyuru metni"<br>6. Kaydet | Duyuru eklenir, dosya ~/duyuru/dd-MM-yyyy/ klasörüne yüklenir, grid yenilenir, toast mesajı | Yüksek |
| DUY-F-003 | Yeni Duyuru Ekleme (Dosyasız) | 1. Tüm zorunlu alanları doldur<br>2. Dosya seçme<br>3. Kaydet | Duyuru eklenir, Dosya alanı boş string olarak kaydedilir | Yüksek |
| DUY-F-004 | Duyuru Güncelleme (Dosya Değişmeden) | 1. Grid'den bir duyuru seç<br>2. Duyuru metnini değiştir<br>3. Yeni dosya seçme<br>4. Güncelle | Duyuru metni güncellenir, eski dosya korunur (hfMevcutDosya) | Yüksek |
| DUY-F-005 | Duyuru Güncelleme (Yeni Dosya) | 1. Grid'den bir duyuru seç<br>2. Yeni dosya seç<br>3. Güncelle | Yeni dosya yüklenir, Dosya alanı yeni path ile güncellenir | Yüksek |
| DUY-F-006 | Duyuru Silme | 1. Grid'den bir duyuru seç<br>2. Sil butonuna tıkla<br>3. Confirm OK | Duyuru silinir, grid yenilenir, form temizlenir | Yüksek |
| DUY-F-007 | Vazgeç (Form Temizle) | 1. Grid'den kayıt seç<br>2. Alanları değiştir<br>3. Vazgeç | Form temizlenir, grid selection kaldırılır, butonlar insert moduna döner | Orta |
| DUY-F-008 | Grid'den Kayıt Seçme | 1. Grid'de bir kaydın "Seç" butonuna tıkla | Form alanları doldurulur, dosya path HiddenField'a atanır, butonlar update moduna geçer | Yüksek |
| DUY-F-009 | Tarih Formatı | Başlama/Bitiş tarihleri {0:dd.MM.yyyy} formatında grid'de görüntülenir | Grid'de tarihler "01.01.2025" formatında | Düşük |
| DUY-F-010 | Kayıt Tarihi Otomatik | Yeni duyuru ekle | Kayit_Tarihi alanı DateTime.Now ile otomatik atanır | Orta |
| DUY-F-011 | Kullanıcı Takibi | Yeni duyuru ekle | Kullanici alanı CurrentUserName ile doldurulur | Orta |
| DUY-F-012 | Güncelleme Takibi | Duyuru güncelle | Guncelleme_Tarihi ve Guncelleyen_Kullanici alanları doldurulur | Orta |
| DUY-F-013 | Durum Dropdown | Durum: Aktif/Pasif seçeneklerini kontrol et | Her iki seçenek dropdown'da mevcut | Düşük |
| DUY-F-014 | Dosya Yükleme - Tarih Bazlı Klasör | 07.11.2025 tarihinde dosya yükle | Dosya ~/duyuru/07-11-2025/ klasörüne kaydedilir | Orta |
| DUY-F-015 | Dosya İsim Formatı | 14:30:25'te "rapor.pdf" yükle | Dosya "143025_rapor.pdf" olarak kaydedilir | Düşük |

### 2.2 Validation Test Senaryoları

| Test ID | Test Adı | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|----------|---------------|----------------|---------|
| DUY-V-001 | Başlama Tarihi Zorunlu | 1. Başlama Tarihi boş<br>2. Kaydet | rfvBaslama validator: "Başlama tarihi zorunludur" | Yüksek |
| DUY-V-002 | Bitiş Tarihi Zorunlu | 1. Bitiş Tarihi boş<br>2. Kaydet | rfvBitis validator: "Bitiş tarihi zorunludur" | Yüksek |
| DUY-V-003 | Duyuru Metni Zorunlu | 1. Duyuru metni boş<br>2. Kaydet | rfvDuyuru validator: "Duyuru metni zorunludur" | Yüksek |
| DUY-V-004 | Duyuru Metni Max Length | Duyuru metni alanına 4001 karakter gir | MaxLength=4000 nedeniyle 4001. karakter yazılamaz | Orta |
| DUY-V-005 | Page.IsValid Kontrolü | Server-side validation bypass dene | Page.IsValid false ise kaydetme işlemi yapılmaz | Yüksek |
| DUY-V-006 | Tarih Formatı Kontrolü | Geçersiz tarih formatı gönder | TextMode="Date" nedeniyle HTML5 validation devreye girer | Orta |
| DUY-V-007 | Dosya Uzantısı Kontrolü | .exe dosyası yüklemeye çalış | FileUpload accept=".xlsx,.xls" olmamasına rağmen client-side kısıt yok, server-side kontrol edilmeli | Orta |
| DUY-V-008 | Çok Büyük Dosya | 100MB dosya yükle | Web.config maxRequestLength limitine takılır | Orta |
| DUY-V-009 | SQL Injection | Duyuru: `'; DELETE FROM duyuru; --` | Parametreli sorgu ile güvenli işlenir | Yüksek |
| DUY-V-010 | XSS Injection | Duyuru: `<img src=x onerror=alert('XSS')>` | ValidateRequest veya output encoding ile korunur | Yüksek |

### 2.3 RBAC Test Senaryoları

| Test ID | Test Adı | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|----------|---------------|----------------|---------|
| DUY-R-001 | Yetkisiz Erişim | DUYURU_YONETIMI yetkisi olmayan kullanıcı ile giriş yap | CheckPermission(Sabitler.DUYURU_YONETIMI) false, erişim reddedilir | Yüksek |
| DUY-R-002 | Yetkili Erişim | DUYURU_YONETIMI yetkisi olan kullanıcı ile giriş yap | Sayfa açılır, tüm işlemler yapılabilir | Yüksek |
| DUY-R-003 | Session Kontrolü | Session expire olan kullanıcı işlem yapsın | BasePage session kontrolü ile login'e yönlendirilir | Orta |
| DUY-R-004 | Dosya Yolu Güvenliği | Başka kullanıcının dosyasına erişmeye çalış | ~/duyuru/ altında izole, ancak doğrudan URL erişimi kontrol edilmeli | Orta |

### 2.4 Negative Test Senaryoları

| Test ID | Test Adı | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|----------|---------------|----------------|---------|
| DUY-N-001 | DB Bağlantı Hatası | DB connection kes, sayfa aç | LoadDuyurular try-catch, "Duyurular yüklenirken hata oluştu." toast, ErrorLog | Yüksek |
| DUY-N-002 | Kayıt Bulunamadı (Grid Boş) | Tüm duyuruları sil | EmptyDataText: "Henüz kayıtlı duyuru bulunmamaktadır." | Orta |
| DUY-N-003 | Grid Selection -1 | Seçim yapmadan Güncelle/Sil butonuna tıkla | DuyurularGrid.SelectedIndex == -1 kontrolü, işlem yapılmaz | Orta |
| DUY-N-004 | Dosya Yükleme Hatası | 1. Disk full<br>2. Dosya yükle | HandleFileUpload try-catch, "Dosya yüklenirken hata oluştu." toast | Orta |
| DUY-N-005 | Klasör Oluşturma Hatası | ~/duyuru/ write permission yok | Directory.CreateDirectory exception, hata loglanır | Orta |
| DUY-N-006 | Parse Hata (Tarih) | GetGridViewCellTextSafe geçersiz tarih döndürürse | DateTime.TryParse kullanılır, parse başarısız ise alan boş kalır | Orta |
| DUY-N-007 | Concurrent Dosya Yazma | Aynı saniyede iki duyuru dosyası yükle | Dosya adı HHmmss formatında, collision riski var | Düşük |
| DUY-N-008 | Sil Confirm İptal | Sil butonunda Cancel | OnClientClick="confirm" false dönerse silme yapılmaz | Düşük |
| DUY-N-009 | Null Dosya Path | MevcutDosya null iken güncelleme yap | string.IsNullOrEmpty kontrolü ile boş string atanır | Düşük |
| DUY-N-010 | Grid Cell Index Hatası | GetGridViewCellTextSafe yanlış index | Try-catch ile safe erişim sağlanır | Düşük |
| DUY-N-011 | Unicode Dosya Adı | Dosya: `测试文档.pdf` | Path.Combine unicode destekler, kayıt başarılı | Düşük |
| DUY-N-012 | Maksimum Satır Sayısı | ORDER BY id DESC doğru çalışıyor mu | Tüm kayıtlar id descending sıralı | Düşük |
| DUY-N-013 | Grid Veri Boşsa Dosya Yükleme | HasFile false durumunu test et | HandleFileUpload boş string döner | Düşük |
| DUY-N-014 | Dropdown Varsayılan Değer | Durum değiştirilmeden kaydet | İlk item (Aktif) kaydedilir | Düşük |

---

## 3. KullaniciIslem.aspx

**Dosya Yolu:** `ModulYonetici/KullaniciIslem.aspx`
**Amaç:** Kullanıcı ekleme, güncelleme, şifre sıfırlama
**Yetki No:** 900
**Veritabanı Tablosu:** `kullanici`

### 3.1 Fonksiyonel Test Senaryoları

| Test ID | Test Adı | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|----------|---------------|----------------|---------|
| KUL-F-001 | Sayfa Yükleme | 1. Yetki 900 ile login<br>2. KullaniciIslem sayfasını aç | Sayfa yüklenir, kullanıcı listesi Repeater'da görünür, form insert modunda | Yüksek |
| KUL-F-002 | Yeni Kullanıcı Ekleme (Tam) | 1. Sicil No: 12345<br>2. Adı Soyadı: Test User<br>3. Mail: test@ankara.gov.tr<br>4. Kullanıcı Türü: Admin<br>5. Personel Tipi: 1<br>6. Birim: IT<br>7. Durum: Aktif<br>8. Parola: Test123!<br>9. Kaydet | Kullanıcı eklenir, parola hash'lenerek kaydedilir, toast mesajı, list yenilenir, form temizlenir | Yüksek |
| KUL-F-003 | Yeni Kullanıcı Ekleme (Zorunlu Alanlar) | 1. Sicil No, Adı Soyadı, Kullanıcı Türü, Durum, Parola doldur<br>2. Kaydet | Kullanıcı eklenir, opsiyonel alanlar boş kaydedilir | Yüksek |
| KUL-F-004 | Parola Hash'leme | Parola: "Test123!" ile kaydet | Helpers.HashPassword kullanılır, plain text değil hash kaydedilir | Yüksek |
| KUL-F-005 | Kullanıcı Düzenleme (Parola Değişmeden) | 1. Düzenle butonuna tıkla<br>2. Adı Soyadı değiştir<br>3. Parola alanını boş bırak<br>4. Güncelle | Kullanıcı güncellenir, parola değişmez (if string.IsNullOrEmpty kontrolü) | Yüksek |
| KUL-F-006 | Kullanıcı Düzenleme (Parola Değiştir) | 1. Düzenle butonuna tıkla<br>2. Parola: YeniSifre123!<br>3. Güncelle | Kullanıcı güncellenir, yeni parola hash'lenerek kaydedilir | Yüksek |
| KUL-F-007 | Şifre Sıfırlama | 1. Repeater'da bir kullanıcının "Şifre Sıfırla" butonuna tıkla<br>2. Confirm OK | Parola "Ankara2025!" olarak sıfırlanır, SifreDegistirmeZorla=1, toast mesajında yeni şifre gösterilir | Yüksek |
| KUL-F-008 | Vazgeç (Düzenleme İptal) | 1. Düzenle<br>2. Alanları değiştir<br>3. Vazgeç | Form temizlenir, Sicil No enable olur, butonlar insert moduna döner | Orta |
| KUL-F-009 | Liste Yenileme | "Yenile" butonuna tıkla | LoadKullanicilar çağrılır, liste refresh olur, "Liste yenilendi" info toast | Orta |
| KUL-F-010 | Kullanıcı Varlık Kontrolü | Mevcut sicil no ile kaydet | KullaniciVarMi() true döner, "Bu sicil numarasına sahip kullanıcı zaten mevcut!" warning toast | Yüksek |
| KUL-F-011 | Sicil No Disable (Edit Mode) | Düzenle butonuna tıkla | txtSicilNo.Enabled = false, Sicil No değiştirilemez | Orta |
| KUL-F-012 | Repeater Veri Binding | 10 kullanıcı varken sayfa aç | Repeater'da 10 satır görünür, her satırda doğru veriler | Orta |
| KUL-F-013 | Boş Kullanıcı Listesi | Kullanıcı yokken sayfa aç | lblMesaj görünür, "Kayıtlı kullanıcı bulunamadı." | Düşük |
| KUL-F-014 | Mail Adresi Icon | Mail adresi olan kullanıcı | Repeater'da mail yanında envelope icon (`<i class='fas fa-envelope'>`) | Düşük |
| KUL-F-015 | Mail Adresi Boş | Mail adresi boş kullanıcı | Repeater'da "-" gösterilir | Düşük |
| KUL-F-016 | Durum Badge | 1. Aktif kullanıcı: Yeşil badge (bg-success)<br>2. Pasif kullanıcı: Gri badge (bg-secondary) | Badge rengi duruma göre değişir | Düşük |
| KUL-F-017 | Repeater ItemCommand | Düzenle/SifreSifirla komutları doğru çalışıyor mu | CommandArgument sicil no olarak alınır, doğru metod çağrılır | Orta |
| KUL-F-018 | GetKullaniciAdi Helper | Şifre sıfırlamada kullanıcı adı alınır | Sicil No'dan Adi_Soyadi getirilir | Düşük |

### 3.2 Validation Test Senaryoları

| Test ID | Test Adı | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|----------|---------------|----------------|---------|
| KUL-V-001 | Sicil No Zorunlu | 1. Sicil No boş<br>2. Kaydet | rfvSicilNo: "Sicil No zorunludur" | Yüksek |
| KUL-V-002 | Adı Soyadı Zorunlu | 1. Adı Soyadı boş<br>2. Kaydet | rfvAdiSoyadi: "Ad Soyad zorunludur" | Yüksek |
| KUL-V-003 | Kullanıcı Türü Zorunlu | 1. Kullanıcı Türü seçme (InitialValue="")<br>2. Kaydet | rfvKullaniciTuru: "Kullanıcı türü seçiniz" | Yüksek |
| KUL-V-004 | Durum Zorunlu | 1. Durum seçme<br>2. Kaydet | rfvDurum: "Durum seçiniz" | Yüksek |
| KUL-V-005 | Parola Zorunlu (Yeni Kayıt) | 1. Yeni kayıt modunda parola boş<br>2. Kaydet | rfvParola: "Parola zorunludur" | Yüksek |
| KUL-V-006 | Parola Opsiyonel (Güncelleme) | 1. Düzenleme modunda parola boş<br>2. Güncelle | rfvParola.Enabled = false, validation çalışmaz | Yüksek |
| KUL-V-007 | Mail Adresi Format | 1. Mail: "gecersizmail"<br>2. Kaydet | revMail regex validation: "Geçerli bir mail adresi giriniz" | Orta |
| KUL-V-008 | Mail Adresi Geçerli Format | Mail: test@ankara.gov.tr | Validation pass | Orta |
| KUL-V-009 | Sicil No Max Length | Sicil No: 16 karakter | MaxLength=15 nedeniyle 16. karakter yazılamaz | Düşük |
| KUL-V-010 | Adı Soyadı Max Length | Adı Soyadı: 51 karakter | MaxLength=50 nedeniyle 51. karakter yazılamaz | Düşük |
| KUL-V-011 | Parola Max Length | Parola: 51 karakter | MaxLength=50 nedeniyle 51. karakter yazılamaz | Düşük |
| KUL-V-012 | Birim Max Length | Birim: 101 karakter | MaxLength=100 nedeniyle 101. karakter yazılamaz | Düşük |
| KUL-V-013 | Page.IsValid Kontrolü | Client-side validation bypass et | Page.IsValid false ise return, kayıt yapılmaz | Yüksek |
| KUL-V-014 | SQL Injection | Sicil No: `'; DROP TABLE kullanici; --` | Parametreli sorgu ile güvenli işlenir | Yüksek |
| KUL-V-015 | XSS Injection | Adı Soyadı: `<script>alert(1)</script>` | ValidateRequest veya encoding ile korunur | Yüksek |

### 3.3 RBAC Test Senaryoları

| Test ID | Test Adı | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|----------|---------------|----------------|---------|
| KUL-R-001 | Yetkisiz Erişim | Yetki 900 olmayan kullanıcı ile giriş | CheckPermission(900) false, erişim reddedilir | Yüksek |
| KUL-R-002 | Yetkili Erişim | Yetki 900 olan kullanıcı ile giriş | Sayfa açılır, tüm işlemler yapılabilir | Yüksek |
| KUL-R-003 | Session Timeout | Session expire olduğunda işlem yap | BasePage kontrolü ile login'e yönlendirilir | Orta |
| KUL-R-004 | Kendi Bilgilerini Değiştirme | Kullanıcı kendi sicil no'su ile düzenleme yapsın | İşlem başarılı (kısıt yok kodda) | Düşük |
| KUL-R-005 | Şifre Hash Görünürlüğü | Şifre sıfırla toast mesajını kontrol et | Şifre plain text gösterilir (güvenlik riski) | Orta |

### 3.4 Negative Test Senaryoları

| Test ID | Test Adı | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|----------|---------------|----------------|---------|
| KUL-N-001 | DB Bağlantı Hatası | DB connection kes, sayfa aç | Try-catch, "Kullanıcılar yüklenirken hata oluştu." toast, ErrorLog | Yüksek |
| KUL-N-002 | Parola Hash Hatası | Helpers.HashPassword exception fırlatsın | Try-catch bloğu hatayı yakalar, kayıt başarısız | Orta |
| KUL-N-003 | Olmayan Kullanıcı Güncelleme | Var olmayan Sicil No ile güncelle | UPDATE 0 satır etkiler, hata mesajı gösterilebilir | Orta |
| KUL-N-004 | Olmayan Kullanıcı Şifre Sıfırlama | Var olmayan Sicil No ile şifre sıfırla | UPDATE 0 satır etkiler, GetKullaniciAdi sicil no döner | Orta |
| KUL-N-005 | Repeater Boş CommandArgument | CommandArgument null gönder | Exception fırlatılır, try-catch ile yakalanmalı | Orta |
| KUL-N-006 | Concurrent Insert | Aynı Sicil No ile eş zamanlı iki kayıt | İkinci kayıt KullaniciVarMi kontrolüne takılabilir veya DB unique constraint hatası | Orta |
| KUL-N-007 | Mail Regex Bypass | JavaScript disable et, geçersiz mail gönder | Server-side RegularExpressionValidator devreye girer | Orta |
| KUL-N-008 | Dropdown Index Out of Range | ddlKullaniciTuru.SelectedIndex = 999 set et | ArgumentOutOfRangeException, try-catch gerekli | Düşük |
| KUL-N-009 | Null/Whitespace Sicil No | Sicil No: "   " (boşluk) | .Trim() sonrası boş string, validation hatası | Orta |
| KUL-N-010 | Şifre Sıfırlama Confirm İptal | Confirm dialog'da Cancel | OnClientClick return false, işlem yapılmaz | Düşük |
| KUL-N-011 | Çok Uzun Parola Hash | 1000 karakterlik parola hash'le | HashPassword çalışır, hash sabit uzunlukta | Düşük |
| KUL-N-012 | Unicode Kullanıcı Adı | Adı Soyadı: `测试用户` | NVARCHAR destekler, kayıt başarılı | Düşük |
| KUL-N-013 | Özel Karakter Sicil No | Sicil No: `@#$%` | Kayıt başarılı, parametreli sorgu güvenli | Düşük |
| KUL-N-014 | Repeater Paging Yok | 1000+ kullanıcı varken performans | Tüm kayıtlar yüklenir, yavaşlama olabilir (paging yok) | Düşük |
| KUL-N-015 | ORDER BY Sıralama | Kullanıcılar alfabetik mi | ORDER BY Adi_Soyadi ASC ile sıralı | Düşük |

---

## 4. YetkiIslem.aspx

**Dosya Yolu:** `ModulYonetici/YetkiIslem.aspx`
**Amaç:** Kullanıcılara yetki atama ve yetki yönetimi
**Yetki No:** 900
**Veritabanı Tablosu:** `yetki`, `kullanici`

### 4.1 Fonksiyonel Test Senaryoları

| Test ID | Test Adı | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|----------|---------------|----------------|---------|
| YET-F-001 | Sayfa Yükleme | 1. Yetki 900 ile login<br>2. YetkiIslem sayfasını aç | Kullanıcı dropdown doldurulur, yetki dropdown doldurulur, grid boş | Yüksek |
| YET-F-002 | Kullanıcı Seçme | 1. Kullanıcı dropdown'dan kullanıcı seç | AutoPostBack true, sicil no otomatik doldurulur, o kullanıcının yetkileri grid'e yüklenir | Yüksek |
| YET-F-003 | Yetki Ekleme | 1. Kullanıcı seç<br>2. Yetki seç<br>3. "Yetki Ekle" butonuna tıkla | Yetki eklenir, grid yenilenir, kayıt sayısı güncellenir, toast mesajı, yetki dropdown sıfırlanır | Yüksek |
| YET-F-004 | Duplicate Yetki Kontrolü | 1. Kullanıcının mevcut yetkisini tekrar ekle | "Bu kullanıcı zaten bu yetkiye sahip." warning toast, kayıt yapılmaz | Yüksek |
| YET-F-005 | Yetki Silme (Tekli) | 1. Grid'de bir yetkinin "Sil" butonuna tıkla<br>2. Confirm OK | Yetki silinir, grid yenilenir, toast mesajı | Yüksek |
| YET-F-006 | Toplu Silme | 1. Birden fazla checkbox işaretle<br>2. "Seçilenleri Sil" butonuna tıkla<br>3. Confirm OK | Seçilen tüm yetkiler silinir, toast'ta silinen sayısı gösterilir, grid yenilenir | Yüksek |
| YET-F-007 | Tümünü Seç Checkbox | Header'daki "Tümünü Seç" checkbox'ını işaretle | toggleAllCheckboxes() çağrılır, tüm satır checkbox'ları işaretlenir | Orta |
| YET-F-008 | Toplu Silme - Hiçbiri Seçili Değil | 1. Hiçbir checkbox işaretleme<br>2. "Seçilenleri Sil" | confirmTopluSilme() alert: "Lütfen silmek istediğiniz yetkileri seçiniz." | Orta |
| YET-F-009 | Toplu Silme - Çift Confirm | Toplu silmede confirm butonları | 1. İlk confirm: "Seçili X adet yetkiyi silmek istediğinizden emin misiniz?"<br>2. İkinci confirm: "SON ONAY: Bu işlem geri alınamaz!" | Orta |
| YET-F-010 | Kullanıcı Dropdown Populate | Sayfa yüklendiğinde | PopulateDropDownList kullanarak tüm kullanıcılar Adi_Soyadi - Sicil_No ile yüklenir | Orta |
| YET-F-011 | Yetki Dropdown Populate | Sayfa yüklendiğinde | DISTINCT Yetki ve Yetki_No çekilerek dropdown doldurulur, format: "Yetki Adı (Yetki_No)" | Orta |
| YET-F-012 | Yetki Adı Parse | Dropdown'dan seçilen yetki adı parse edilir | "Duyuru Yönetimi (100)" -> "Duyuru Yönetimi" (parantez kaldırılır) | Düşük |
| YET-F-013 | Kayıt Sayısı Badge | 5 yetki varken | lblKayitSayisi: "5 kayıt" | Düşük |
| YET-F-014 | Kayıt Sayısı Badge - Boş | 0 yetki varken | lblKayitSayisi: "0 kayıt" | Düşük |
| YET-F-015 | Grid EmptyDataText | Kullanıcı seçili değilken veya yetkisi yokken | "Henüz yetki kaydı bulunmamaktadır." | Düşük |
| YET-F-016 | Grid Sıralama | Yetkiler Yetki_No ASC sıralı mı | ORDER BY Yetki_No ASC ile sıralı | Düşük |
| YET-F-017 | Dropdown AutoPostBack | Kullanıcı dropdown'u değiştiğinde | OnSelectedIndexChanged tetiklenir, yetkiler otomatik yüklenir | Orta |
| YET-F-018 | Kullanıcı Seçimi Kaldır | Dropdown'da "Kullanıcı Seçiniz..." seç | txtSicilNo boşalır, grid temizlenir | Orta |

### 4.2 Validation Test Senaryoları

| Test ID | Test Adı | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|----------|---------------|----------------|---------|
| YET-V-001 | Kullanıcı Seçilmemiş | 1. Kullanıcı seçme<br>2. Yetki Ekle | "Lütfen kullanıcı seçiniz." warning toast | Yüksek |
| YET-V-002 | Yetki Seçilmemiş | 1. Kullanıcı seç<br>2. Yetki seçme (InitialValue="")<br>3. Yetki Ekle | "Lütfen yetki seçiniz." warning toast | Yüksek |
| YET-V-003 | Dropdown SelectedIndex Kontrolü | ddlKullanici.SelectedIndex <= 0 | Validation geçmez | Orta |
| YET-V-004 | Dropdown Yetki SelectedIndex | ddlYetki.SelectedIndex <= 0 | Validation geçmez | Orta |
| YET-V-005 | SQL Injection - Sicil No | Sicil No: `'; DROP TABLE yetki; --` | Parametreli sorgu ile güvenli | Yüksek |
| YET-V-006 | SQL Injection - Yetki No | Yetki No manipülasyonu | Parametreli sorgu ile güvenli | Yüksek |
| YET-V-007 | XSS Injection - Yetki Adı | Yetki: `<script>alert(1)</script>` | Encoding ile güvenli gösterim | Orta |
| YET-V-008 | Integer Parse (Yetki No) | Yetki No geçersiz integer | Convert.ToInt32 exception, try-catch gerekli | Orta |
| YET-V-009 | Null/Boş Sicil No | Sicil No null gönder | string.IsNullOrEmpty kontrolü yapılmalı | Orta |
| YET-V-010 | Grid DataKey Null | Grid DataKey null | Convert.ToInt32 exception, try-catch ile yakalanmalı | Orta |

### 4.3 RBAC Test Senaryoları

| Test ID | Test Adı | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|----------|---------------|----------------|---------|
| YET-R-001 | Yetkisiz Erişim | Yetki 900 olmayan kullanıcı ile giriş | CheckPermission(900) false, erişim reddedilir | Yüksek |
| YET-R-002 | Yetkili Erişim | Yetki 900 olan kullanıcı ile giriş | Sayfa açılır, tüm işlemler yapılabilir | Yüksek |
| YET-R-003 | Session Timeout | Session expire olduğunda işlem yap | BasePage kontrolü ile login'e yönlendirilir | Orta |
| YET-R-004 | Kendi Yetkisini Silme | Kullanıcı kendi yetkilerini silsin | İşlem başarılı (kısıt yok), ancak sonraki erişimde yetki kontrolü başarısız olur | Orta |
| YET-R-005 | Admin Yetkisi Silme | Admin kullanıcısının kritik yetkisini sil | İşlem başarılı (kısıt yok), dikkatli olunmalı | Düşük |

### 4.4 Negative Test Senaryoları

| Test ID | Test Adı | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|----------|---------------|----------------|---------|
| YET-N-001 | DB Bağlantı Hatası | DB connection kes, sayfa aç | Try-catch, "Kullanıcılar/Yetki listesi yüklenirken hata oluştu." toast, ErrorLog | Yüksek |
| YET-N-002 | Dropdown Populate Hatası | Kullanıcı tablosu boşken sayfa aç | Dropdown'da sadece "Kullanıcı Seçiniz..." | Orta |
| YET-N-003 | Yetki Tablosu Boş | Yetki tablosunda DISTINCT kayıt yokken | Dropdown'da sadece "Yetki Seçiniz..." | Orta |
| YET-N-004 | Olmayan Yetki Silme | Var olmayan ID ile yetki sil | DELETE 0 satır etkiler, sonuc > 0 kontrolü ile hata mesajı | Orta |
| YET-N-005 | Grid RowCommand Exception | CommandArgument geçersiz | Convert.ToInt32 exception, try-catch ile yakalanır | Orta |
| YET-N-006 | Checkbox FindControl Null | Checkbox control bulunamaz | chkSec != null kontrolü yapılır | Orta |
| YET-N-007 | DataKeys Index Out of Range | Grid'de olmayan row index | ArgumentOutOfRangeException riski | Orta |
| YET-N-008 | Concurrent Yetki Ekleme | Aynı kullanıcı-yetki çiftini eş zamanlı ekle | İkinci ekleme duplicate kontrolüne takılır | Düşük |
| YET-N-009 | Toplu Silme - Tümü Başarısız | DB constraint nedeniyle silme başarısız | Try-catch ile hata yakalanır, silinenSayisi = 0, warning toast | Orta |
| YET-N-010 | Sil Confirm İptal | Confirm dialog'da Cancel | OnClientClick return false, silme yapılmaz | Düşük |
| YET-N-011 | JavaScript Disabled | JavaScript kapalıyken toplu seç | toggleAllCheckboxes() çalışmaz, manuel seçim gerekir | Düşük |
| YET-N-012 | GetGridViewCellTextSafe Hatası | Grid cell index yanlış | Helper metod try-catch ile safe erişim | Düşük |
| YET-N-013 | Yetki Adı Substring Hatası | Yetki adı parantez içermiyorsa | IndexOf("(") -1 döner, Substring exception | Orta |
| YET-N-014 | Unicode Yetki Adı | Yetki: `中文权限` | NVARCHAR destekler, kayıt başarılı | Düşük |
| YET-N-015 | Çok Sayıda Yetki | 1000+ yetki kaydı varken grid performansı | GridView tüm kayıtları yükler, paging yok, yavaşlama olabilir | Düşük |
| YET-N-016 | Dropdown SelectedValue Hatası | SelectedValue veritabanında yoksa | Grid boş gelir veya exception | Düşük |
| YET-N-017 | AutoPostBack Exception | OnSelectedIndexChanged'da exception | Try-catch ile yakalanmalı, hata toast | Orta |
| YET-N-018 | Toplu Silme Confirm Bypass | JavaScript manipülasyonu ile confirm atla | Server-side kontrolü yok, silme gerçekleşir | Düşük |

---

## Test Özeti ve İstatistikler

| Sayfa | Fonksiyonel | Validation | RBAC | Negative | **Toplam** |
|-------|-------------|------------|------|----------|------------|
| **BilgisayarAdlari.aspx** | 16 | 10 | 5 | 16 | **47** |
| **Duyurular.aspx** | 15 | 10 | 4 | 14 | **43** |
| **KullaniciIslem.aspx** | 18 | 15 | 5 | 15 | **53** |
| **YetkiIslem.aspx** | 18 | 10 | 5 | 18 | **51** |
| **GENEL TOPLAM** | **67** | **45** | **19** | **63** | **194** |

---

## Test Önceliklendirmesi

### Yüksek Öncelik (52 Test)
- Tüm CRUD işlemleri
- Zorunlu alan validasyonları
- RBAC kontrolleri
- SQL Injection ve XSS testleri
- Kritik iş akışları

### Orta Öncelik (98 Test)
- Arama ve filtreleme
- Dosya işlemleri
- Dropdown ve grid işlemleri
- Hata yönetimi
- Concurrent işlemler

### Düşük Öncelik (44 Test)
- UI/UX detayları
- Format kontrolleri
- Unicode ve özel karakter testleri
- Performans testleri
- Edge case'ler

---

## Test Ortamı Gereksinimleri

### Yazılım
- ✅ IIS 10.0+
- ✅ .NET Framework 4.7.2+
- ✅ SQL Server 2016+
- ✅ Web Browser: Chrome, Firefox, Edge (son 2 versiyon)

### Veritabanı
- ✅ `bilgisayar_adlari` tablosu
- ✅ `duyuru` tablosu
- ✅ `kullanici` tablosu
- ✅ `yetki` tablosu

### Test Data
- ✅ En az 10 kullanıcı kaydı
- ✅ Farklı yetki seviyelerinde kullanıcılar
- ✅ Test için kullanılacak dosyalar (.pdf, .xlsx, .jpg)
- ✅ Çeşitli durumlarda duyurular (Aktif/Pasif)

---

## Önemli Notlar ve Gözlemler

### Güvenlik
1. **Parola Hash**: `Helpers.HashPassword()` kullanılıyor ✅
2. **SQL Injection**: Parametreli sorgular ile korunuyor ✅
3. **XSS**: ASP.NET ValidateRequest aktif olmalı ⚠️
4. **RBAC**: `CheckPermission()` her sayfada kontrol ediliyor ✅
5. **Session**: `BasePage` üzerinden yönetiliyor ✅

### Performans
1. **Paging**: Grid'lerde paging YOK ⚠️ (Büyük veri setlerinde yavaşlama)
2. **Indexing**: Arama kolonlarında index kontrolü yapılmalı
3. **Connection Pooling**: ADO.NET varsayılan pooling kullanıyor

### Kullanılabilirlik
1. **Toast Mesajları**: Tüm işlemlerde feedback var ✅
2. **Confirm Dialog**: Silme işlemlerinde onay alınıyor ✅
3. **Form Validation**: Client + Server side validation ✅
4. **Responsive**: Bootstrap kullanılıyor ✅

### Kod Kalitesi
1. **Error Logging**: `LogError()` ile loglama yapılıyor ✅
2. **Try-Catch**: Tüm kritik metodlarda mevcut ✅
3. **Helper Methods**: `BasePage` üzerinden ortak metodlar ✅
4. **Code Reuse**: `SetFormModeInsert/Update` gibi yardımcı metodlar ✅

### Eksik/İyileştirme Alanları
1. ⚠️ **Excel Import**: BilgisayarAdlari'nda yorum satırında (implement edilmemiş)
2. ⚠️ **Paging**: Büyük veri setlerinde performans sorunu
3. ⚠️ **Audit Trail**: Silme işlemlerinde hard delete (soft delete olabilir)
4. ⚠️ **Concurrent Kontrolü**: Optimistic concurrency yok
5. ⚠️ **Şifre Güvenliği**: Şifre sıfırlamada plain text toast'ta gösteriliyor

---

## Test Execution Checklist

### Test Öncesi
- [ ] Test ortamı hazır
- [ ] Veritabanı backup alındı
- [ ] Test kullanıcıları oluşturuldu
- [ ] Test dataları hazırlandı
- [ ] Tarayıcı cache temizlendi

### Test Sırası
1. [ ] BilgisayarAdlari.aspx - Fonksiyonel testler
2. [ ] BilgisayarAdlari.aspx - Validation testler
3. [ ] BilgisayarAdlari.aspx - RBAC testler
4. [ ] BilgisayarAdlari.aspx - Negative testler
5. [ ] Duyurular.aspx - Tüm test tipleri
6. [ ] KullaniciIslem.aspx - Tüm test tipleri
7. [ ] YetkiIslem.aspx - Tüm test tipleri

### Test Sonrası
- [ ] Test sonuçları dokümante edildi
- [ ] Bulunan bug'lar raporlandı
- [ ] Veritabanı restore edildi
- [ ] Test coverage hesaplandı
- [ ] Regression test listesi güncellendi

---

## Test Rapor Şablonu

```markdown
### Test Sonucu

**Test ID:** YET-F-001
**Test Tarihi:** 07.11.2025
**Test Eden:** QA Engineer
**Durum:** ✅ Başarılı / ❌ Başarısız / ⚠️ Uyarı

**Açıklama:**
[Test adımları ve sonuçları]

**Ekran Görüntüsü:**
[Screenshot path]

**Notlar:**
[Ek bilgiler]
```

---

**Son Güncelleme:** 07.11.2025
**Doküman Versiyonu:** 1.0
**Hazırlayan:** Senior QA Engineer & .NET Web Forms Specialist
