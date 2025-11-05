# TEST DÖKÜMANI - MODULGOREV

**Proje:** Portal-Yeni
**Modül:** ModulGorev
**Test Tarihi:** 2025-11-05
**Test Sorumlusu:** QA Team
**Teknoloji:** ASP.NET Web Forms, C#, SQL Server

---

## İÇİNDEKİLER

1. [GorevRapor.aspx - Araç Görev Rapor](#1-gorevrapor)
2. [Gorevlendirme.aspx - Personel Görevlendirme](#2-gorevlendirme)
3. [PersonelRapor.aspx - Personel Görev Rapor](#3-personelrapor)
4. [TalepEkle.aspx - Görev Talep Ekle](#4-talepekle)
5. [TalepRapor.aspx - Görev Talep Rapor](#5-taleprapor)

---

## 1. GOREVRAPOR

**Dosya:** `ModulGorev/GorevRapor.aspx`
**Amaç:** Araç görevlerini listeleme, güncelleme, silme ve raporlama

### 1.1 Fonksiyonel Testler

| Test ID | Test Senaryosu | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|---------------|---------------|----------------|---------|
| GR-F-001 | Sayfa yüklenme testi | 1. GorevRapor.aspx sayfasını aç | Sayfa başarıyla yüklenir, istatistik kartları (Toplam Görev, Toplam KM, Toplam Yakıt) görüntülenir | Yüksek |
| GR-F-002 | Tüm görevleri listeleme | 1. "Tümü" butonuna tıkla | Tüm araç görevleri GridView'da listelenir, kayıt sayısı gösterilir | Yüksek |
| GR-F-003 | Plaka filtresi ile arama | 1. Araç Plakası dropdown'dan plaka seç<br>2. "Ara" butonuna tıkla | Seçilen plakaya ait görevler filtrelenir ve listelenir | Yüksek |
| GR-F-004 | Şube filtresi ile arama | 1. Görevlendiren Birim dropdown'dan şube seç<br>2. "Ara" butonuna tıkla | Seçilen şubeye ait görevler filtrelenir | Orta |
| GR-F-005 | Tarih aralığı filtresi | 1. Başlangıç tarihi seç<br>2. Bitiş tarihi seç<br>3. "Ara" butonuna tıkla | Belirlenen tarih aralığındaki görevler listelenir | Yüksek |
| GR-F-006 | Kombine filtreleme | 1. Plaka, şube ve tarih filtrelerini doldur<br>2. "Ara" butonuna tıkla | Tüm filtrelere uyan kayıtlar gösterilir | Orta |
| GR-F-007 | Görev seçimi | 1. GridView'dan bir görev seç<br>2. "Seç" butonuna tıkla | Güncelleme paneli açılır, seçilen görev bilgileri form alanlarına yüklenir | Yüksek |
| GR-F-008 | Görev güncelleme | 1. Bir görev seç<br>2. Dönüş KM değiştir<br>3. "Güncelle" butonuna tıkla | Görev başarıyla güncellenir, yapılan KM ve tüketilen yakıt otomatik hesaplanır | Yüksek |
| GR-F-009 | Otomatik yakıt hesaplama | 1. Bir görev seç<br>2. Çıkış KM: 1000, Dönüş KM: 1500 gir | Yapılan KM: 500 olarak hesaplanır, Ort. Yakıt Tük. ile tüketilen yakıt otomatik hesaplanır | Yüksek |
| GR-F-010 | Görev silme | 1. Bir görev seç<br>2. "Sil" butonuna tıkla<br>3. Onay dialogunda "OK" tıkla | Görev veritabanından silinir, liste güncellenir | Yüksek |
| GR-F-011 | Excel export | 1. Görevleri listele<br>2. "Excel'e Aktar" butonuna tıkla | Excel dosyası oluşturulur ve indirilir (AracGorevRapor_YYYYMMDD.xls formatında) | Orta |
| GR-F-012 | İstatistik güncelleme | 1. Filtreleme yap<br>2. İstatistik kartlarını kontrol et | Filtreye göre Toplam Görev, Toplam KM ve Toplam Yakıt değerleri güncellenir | Orta |
| GR-F-013 | Plaka değişikliğinde sürücü bilgisi | 1. Güncelleme panelinde plaka değiştir | Seçilen plakaya ait sürücü adı ve ort. yakıt tüketimi otomatik yüklenir | Orta |
| GR-F-014 | Vazgeç butonu | 1. Bir görev seç<br>2. Bilgileri değiştir<br>3. "Vazgeç" butonuna tıkla | Güncelleme paneli kapanır, form temizlenir | Düşük |

### 1.2 Validation Testleri

| Test ID | Test Senaryosu | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|---------------|---------------|----------------|---------|
| GR-V-001 | Boş tarih filtresi | 1. Tarih alanlarını boş bırak<br>2. "Ara" butonuna tıkla | Sistem tüm tarihleri getirir, hata vermez | Orta |
| GR-V-002 | Geçersiz tarih aralığı | 1. Bitiş tarihini başlangıç tarihinden önce seç<br>2. "Ara" butonuna tıkla | Uygun uyarı mesajı gösterilir veya sonuç bulunamaz | Orta |
| GR-V-003 | Negatif KM değeri | 1. Güncelleme panelinde Çıkış KM: 2000, Dönüş KM: 1500 gir | Yapılan KM negatif çıkar, uygun hata mesajı gösterilmeli | Yüksek |
| GR-V-004 | Alfanumerik KM girişi | 1. KM alanına harf gir | TextMode="Number" olduğu için harf girişi engellenir | Orta |
| GR-V-005 | Boş zorunlu alanlar | 1. Bir görev seç<br>2. Zorunlu alanları boşalt<br>3. "Güncelle" butonuna tıkla | Validation hatası gösterilir, güncelleme yapılmaz | Yüksek |
| GR-V-006 | Maksimum karakter uzunluğu | 1. Açıklama alanına 5000 karakterden fazla metin gir | Karakter limiti kontrolü yapılmalı | Düşük |
| GR-V-007 | SQL Injection testi | 1. Filtre alanlarına `' OR '1'='1` gir<br>2. Arama yap | Parametreli sorgu kullanıldığı için SQL injection engellenir | Kritik |
| GR-V-008 | XSS testi | 1. Açıklama alanına `<script>alert('XSS')</script>` gir<br>2. Kaydet ve listele | Script çalıştırılmaz, encode edilmiş halde gösterilir | Kritik |

### 1.3 RBAC (Role-Based Access Control) Testleri

| Test ID | Test Senaryosu | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|---------------|---------------|----------------|---------|
| GR-R-001 | Yetkisiz kullanıcı erişimi | 1. GOREV_TAKIP_SISTEMI yetkisi olmayan kullanıcı ile giriş yap<br>2. GorevRapor.aspx'e git | Erişim engellenir, yetki hatası gösterilir (kod satır 135-138'de yorum satırında) | Kritik |
| GR-R-002 | Yetkili kullanıcı erişimi | 1. GOREV_TAKIP_SISTEMI yetkisi olan kullanıcı ile giriş yap<br>2. GorevRapor.aspx'e git | Sayfa başarıyla yüklenir, tüm işlemler yapılabilir | Kritik |
| GR-R-003 | Kullanıcı log kaydı | 1. Herhangi bir CRUD işlemi yap | İşlem kullanıcı adı ile loglanır (CurrentUserName kullanılıyor) | Orta |
| GR-R-004 | Şube bazlı veri filtreleme | 1. Farklı şubelerden kullanıcılar ile giriş yap<br>2. Verileri listele | Her kullanıcı kendi yetkisine göre veri görmeli (şu an tüm veriler görünüyor, ihtiyaç varsa implement edilmeli) | Orta |

### 1.4 Negative Testler

| Test ID | Test Senaryosu | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|---------------|---------------|----------------|---------|
| GR-N-001 | Veritabanı bağlantı hatası | 1. Veritabanı bağlantısını kes<br>2. Sayfa yükle | Hata yakalanır, kullanıcıya anlaşılır hata mesajı gösterilir | Yüksek |
| GR-N-002 | Olmayan görev güncelleme | 1. Veritabanından bir görevi manuel sil<br>2. Aynı görevi arayüzden güncellemeye çalış | "Kayıt güncellenemedi" mesajı gösterilir | Orta |
| GR-N-003 | Aynı anda silinen kayıt | 1. İki kullanıcı aynı kaydı aynı anda seçsin<br>2. Biri silsin, diğeri güncellesin | Güncelleme başarısız olur, uygun mesaj gösterilir | Orta |
| GR-N-004 | Session timeout | 1. Sayfayı aç ve 30 dk bekle<br>2. İşlem yapmaya çalış | Session timeout hatası, login sayfasına yönlendirme | Orta |
| GR-N-005 | Boş GridView Excel export | 1. Hiç kayıt olmayan durumda Excel'e Aktar butonuna bas | "Export edilecek veri bulunamadı" uyarısı gösterilir | Düşük |
| GR-N-006 | Browser back button | 1. Görev güncelle<br>2. Browser back butonuna bas | Sayfa yeniden yüklenir, eski veriler gösterilmez | Düşük |
| GR-N-007 | Concurrent update | 1. Aynı kaydı iki farklı tarayıcıdan aç<br>2. Her ikisinden de farklı güncellemeler yap | Son yapılan güncelleme geçerli olur (optimistic concurrency kontrolü yok) | Orta |

---

## 2. GOREVLENDIRME

**Dosya:** `ModulGorev/Gorevlendirme.aspx`
**Amaç:** Personel görevlendirme işlemlerini yönetme (CRUD)

### 2.1 Fonksiyonel Testler

| Test ID | Test Senaryosu | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|---------------|---------------|----------------|---------|
| GV-F-001 | Sayfa yüklenme | 1. Gorevlendirme.aspx sayfasını aç | Sayfa yüklenir, istatistikler (Toplam, Aktif, Tamamlanan) gösterilir | Yüksek |
| GV-F-002 | Yeni görevlendirme ekleme | 1. Personel seç<br>2. İl seç<br>3. Tarih bilgilerini gir<br>4. Görev tanımı yaz<br>5. "Kaydet" butonuna tıkla | Görevlendirme kaydedilir, liste güncellenir, başarı mesajı gösterilir | Kritik |
| GV-F-003 | Zorunlu alan kontrolü | 1. Formu kısmen doldur<br>2. Zorunlu alanları boş bırak<br>3. "Kaydet" butonuna tıkla | RequiredFieldValidator devreye girer, kırmızı hata mesajları gösterilir | Yüksek |
| GV-F-004 | Süre validasyonu | 1. Görevlendirme Süresi alanına 400 gir<br>2. "Kaydet" butonuna tıkla | RangeValidator devreye girer: "Süre 1-365 gün arasında olmalıdır" | Yüksek |
| GV-F-005 | Görevlendirme listeleme | 1. Sayfa yükle | Tüm görevlendirmeler GridView'da listelenir, sayfalama çalışır (PageSize=15) | Yüksek |
| GV-F-006 | Görevlendirme güncelleme | 1. Listeden bir kayıt seç<br>2. Bilgileri değiştir<br>3. "Güncelle" butonuna tıkla | Kayıt güncellenir, GuncelleyenKullanici ve GuncellemeTarihi set edilir | Yüksek |
| GV-F-007 | Görevlendirme silme | 1. Listeden bir kayıt seç<br>2. "Sil" butonuna tıkla<br>3. Onay dialogunda "OK" | Kayıt silinir, liste güncellenir, istatistikler yeniden hesaplanır | Yüksek |
| GV-F-008 | Personel filtresi | 1. Filtre bölümünden personel seç<br>2. "Ara" butonuna tıkla | Seçilen personele ait görevlendirmeler listelenir | Orta |
| GV-F-009 | İl filtresi | 1. Filtre bölümünden il seç<br>2. "Ara" butonuna tıkla | Seçilen ile ait görevlendirmeler listelenir | Orta |
| GV-F-010 | Tarih aralığı filtresi | 1. Başlangıç ve bitiş tarihi seç<br>2. "Ara" butonuna tıkla | Tarih aralığındaki görevlendirmeler gösterilir | Orta |
| GV-F-011 | Filtreleri temizle | 1. Filtreleri uygula<br>2. "Temizle" butonuna tıkla | Tüm filtreler sıfırlanır, tüm kayıtlar listelenir | Düşük |
| GV-F-012 | Excel export | 1. Görevlendirmeleri listele<br>2. "Excel'e Aktar" butonuna tıkla | Excel dosyası indirilir (Gorevlendirmeler_YYYYMMDD.xls) | Orta |
| GV-F-013 | Sayfalama | 1. 15'ten fazla kayıt olduğundan emin ol<br>2. Sonraki sayfaya geç | Sayfalama çalışır, kayıtlar sayfa sayfa gösterilir | Orta |
| GV-F-014 | İstatistik hesaplama | 1. Yeni kayıt ekle<br>2. İstatistik kartlarını kontrol et | Toplam, aktif ve tamamlanan görev sayıları doğru hesaplanır | Orta |
| GV-F-015 | Aktif görev hesaplama | 1. Bitiş tarihi bugünden sonraki kayıtlar | Aktif görev sayısında sayılır | Orta |
| GV-F-016 | Vazgeç butonu | 1. Kayıt seç veya düzenlemeye başla<br>2. "Vazgeç" butonuna tıkla | Form temizlenir, ekleme moduna döner | Düşük |

### 2.2 Validation Testleri

| Test ID | Test Senaryosu | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|---------------|---------------|----------------|---------|
| GV-V-001 | ValidationSummary kontrolü | 1. Formu boş gönder | ValidationSummary tüm hata mesajlarını listeler | Orta |
| GV-V-002 | Personel seçimi | 1. Personel dropdown'dan "Seçiniz" seçili bırak<br>2. Kaydet | "Personel seçimi zorunludur" hatası gösterilir | Yüksek |
| GV-V-003 | İl seçimi | 1. İl dropdown'dan "Seçiniz" seçili bırak<br>2. Kaydet | "İl seçimi zorunludur" hatası gösterilir | Yüksek |
| GV-V-004 | Başlama tarihi | 1. Başlama tarihi boş bırak<br>2. Kaydet | "Başlama tarihi zorunludur" hatası | Yüksek |
| GV-V-005 | Süre aralığı - minimum | 1. Süre alanına 0 gir<br>2. Kaydet | "Süre 1-365 gün arasında olmalıdır" hatası | Orta |
| GV-V-006 | Süre aralığı - maksimum | 1. Süre alanına 366 gir<br>2. Kaydet | "Süre 1-365 gün arasında olmalıdır" hatası | Orta |
| GV-V-007 | Bitiş tarihi | 1. Bitiş tarihi boş bırak<br>2. Kaydet | "Bitiş tarihi zorunludur" hatası | Yüksek |
| GV-V-008 | Görev tanımı | 1. Görev tanımı boş bırak<br>2. Kaydet | "Görev tanımı zorunludur" hatası | Yüksek |
| GV-V-009 | Duplicate kayıt kontrolü | 1. Aynı personele aynı tarihte ikinci kayıt ekle | "Aynı tarihte aynı personele görevlendirme zaten eklenmiş" uyarısı | Yüksek |
| GV-V-010 | SQL Injection - Filtre | 1. Filtre alanlarına SQL injection denemesi yap | Parametreli sorgu koruması sayesinde engellenir | Kritik |
| GV-V-011 | XSS - Görev tanımı | 1. Görev tanımına `<script>alert('XSS')</script>` gir | Script encode edilir, çalıştırılmaz | Kritik |
| GV-V-012 | HTML Encoding | 1. Diğer İller alanına HTML tag'leri gir<br>2. Kaydet ve listele | HTML encode edilerek gösterilir | Orta |

### 2.3 RBAC Testleri

| Test ID | Test Senaryosu | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|---------------|---------------|----------------|---------|
| GV-R-001 | Yetki kontrolü | 1. GOREV_TAKIP_SISTEMI yetkisi olmayan kullanıcı<br>2. Gorevlendirme.aspx'e eriş | CheckPermission kontrolü ile erişim engellenir (satır 16-19) | Kritik |
| GV-R-002 | Kullanıcı adı kaydı | 1. Yeni görevlendirme ekle | KayitKullanici alanına CurrentUserName kaydedilir | Orta |
| GV-R-003 | Güncelleme kullanıcısı | 1. Bir kaydı güncelle | GuncelleyenKullanici ve GuncellemeTarihi set edilir | Orta |
| GV-R-004 | Log kaydı | 1. CRUD işlemleri yap | Her işlem için LogInfo/LogError ile log kaydı oluşturulur | Orta |

### 2.4 Negative Testler

| Test ID | Test Senaryosu | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|---------------|---------------|----------------|---------|
| GV-N-001 | Veritabanı hatası | 1. DB bağlantısını kes<br>2. Kayıt ekle | Try-catch ile hata yakalanır, kullanıcıya toast mesajı gösterilir | Yüksek |
| GV-N-002 | Olmayan kayıt güncelleme | 1. Kayıt seç, başka yerden sil<br>2. Güncelle | "Güncelleme sırasında bir hata oluştu" mesajı | Orta |
| GV-N-003 | Sayfalama hatası | 1. Son sayfadaki tüm kayıtları sil<br>2. Sayfaya gitmeye çalış | Hata yakalanır, uygun sayfa gösterilir | Düşük |
| GV-N-004 | Personel dropdown boş | 1. Personel tablosu boş<br>2. Sayfayı yükle | Dropdown yalnızca "Seçiniz" öğesini içerir, hata vermez | Orta |
| GV-N-005 | İl dropdown boş | 1. İl tablosu boş<br>2. Sayfayı yükle | Dropdown yalnızca "Seçiniz" öğesini içerir | Orta |
| GV-N-006 | Excel export boş liste | 1. Kayıt yokken Excel'e aktar | "Export edilecek veri bulunamadı" uyarısı | Düşük |
| GV-N-007 | Çok uzun metin | 1. Görev tanımına 10000 karakter gir | Veritabanı alan boyutu limiti kontrolü | Düşük |

---

## 3. PERSONELRAPOR

**Dosya:** `ModulGorev/PersonelRapor.aspx`
**Amaç:** Personel görevlendirme kayıtlarını raporlama ve görüntüleme

### 3.1 Fonksiyonel Testler

| Test ID | Test Senaryosu | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|---------------|---------------|----------------|---------|
| PR-F-001 | Sayfa yüklenme | 1. PersonelRapor.aspx sayfasını aç | Sayfa yüklenir, son 50 kayıt listelenir (TOP 50) | Yüksek |
| PR-F-002 | Personel filtresi | 1. Personel dropdown'dan bir personel seç<br>2. "Ara" butonuna tıkla | Seçilen personele ait görevlendirmeler listelenir | Yüksek |
| PR-F-003 | İl filtresi | 1. İl dropdown'dan bir il seç<br>2. "Ara" butonuna tıkla | Seçilen ile ait görevlendirmeler listelenir | Yüksek |
| PR-F-004 | Tarih aralığı filtresi | 1. Başlangıç tarihi seç<br>2. Bitiş tarihi seç<br>3. "Ara" butonuna tıkla | Belirlenen tarih aralığındaki kayıtlar gösterilir | Yüksek |
| PR-F-005 | Kombine filtre | 1. Personel, İl ve Tarih filtrelerini doldur<br>2. "Ara" butonuna tıkla | Tüm kriterlere uyan kayıtlar listelenir | Orta |
| PR-F-006 | Tümünü listeleme | 1. "Tümünü Listele" butonuna tıkla | Filtreler temizlenir, tüm kayıtlar (TOP 50) gösterilir | Orta |
| PR-F-007 | Kayıt sayısı badge | 1. Arama yap | lblKayitSayisi badge'inde sonuç sayısı gösterilir | Düşük |
| PR-F-008 | Sonuç bilgisi | 1. Filtreleme yap | lblSonucBilgisi "X kayıt bulundu" şeklinde gösterilir | Düşük |
| PR-F-009 | Excel export | 1. Kayıtları listele<br>2. "Excel'e Aktar" butonuna tıkla | PersonelGorevRapor_YYYYMMDD.xls dosyası indirilir | Orta |
| PR-F-010 | GridView footer toplam | 1. Kayıtları listele<br>2. Footer satırını kontrol et | Footer'da "Toplam" ve kayıt sayısı gösterilir | Düşük |
| PR-F-011 | Tarih formatı | 1. Kayıtları listele | Tarihler "dd.MM.yyyy" formatında gösterilir | Düşük |

### 3.2 Validation Testleri

| Test ID | Test Senaryosu | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|---------------|---------------|----------------|---------|
| PR-V-001 | Boş filtre arama | 1. Tüm filtreler boş<br>2. "Ara" butonuna tıkla | TOP 50 kayıt listelenir, hata vermez | Orta |
| PR-V-002 | Geçersiz tarih aralığı | 1. Bitiş tarihini başlangıçtan önce seç<br>2. Ara | Sonuç bulunamaz veya uyarı gösterilir | Orta |
| PR-V-003 | SQL Injection - Personel | 1. Personel alanına SQL injection gir (parametreli sorgu var) | SQL injection engellenir | Kritik |
| PR-V-004 | SQL Injection - İl | 1. İl alanına SQL injection gir | SQL injection engellenir | Kritik |
| PR-V-005 | Tarih formatı | 1. Geçersiz tarih formatı gir (elle) | Tarayıcı tarih input kontrolü engeller | Orta |

### 3.3 RBAC Testleri

| Test ID | Test Senaryosu | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|---------------|---------------|----------------|---------|
| PR-R-001 | Yetki kontrolü | 1. GOREV_TAKIP_SISTEMI yetkisi olmayan kullanıcı<br>2. PersonelRapor.aspx'e eriş | CheckPermission kontrolü ile erişim engellenir (satır 18) | Kritik |
| PR-R-002 | Log kaydı | 1. Filtreleme ve arama yap | LogInfo ile işlem loglanır | Orta |
| PR-R-003 | Sadece okuma yetkisi | 1. Rapor sayfasını kullan | Sadece görüntüleme var, CRUD işlemi yok | Bilgi |

### 3.4 Negative Testler

| Test ID | Test Senaryosu | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|---------------|---------------|----------------|---------|
| PR-N-001 | Veritabanı bağlantı hatası | 1. DB bağlantısını kes<br>2. Sayfayı yükle | Try-catch ile hata yakalanır, toast mesajı gösterilir | Yüksek |
| PR-N-002 | Boş sonuç kümesi | 1. Hiç eşleşmeyen filtre yap | "Kayıt bulunamadı" mesajı gösterilir | Orta |
| PR-N-003 | Excel export boş liste | 1. Sonuç yokken Excel'e Aktar butonuna tıkla | "Export edilecek veri bulunamadı" uyarısı gösterilir | Düşük |
| PR-N-004 | Personel dropdown yükleme hatası | 1. Personel tablosuna erişim engelli<br>2. Sayfayı yükle | Hata yakalanır, toast mesajı gösterilir | Orta |
| PR-N-005 | İl dropdown yükleme hatası | 1. İl tablosuna erişim engelli<br>2. Sayfayı yükle | Hata yakalanır, toast mesajı gösterilir | Orta |

---

## 4. TALEPEKLE

**Dosya:** `ModulGorev/TalepEkle.aspx`
**Amaç:** Görev talebi oluşturma, güncelleme ve silme

### 4.1 Fonksiyonel Testler

| Test ID | Test Senaryosu | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|---------------|---------------|----------------|---------|
| TE-F-001 | Sayfa yüklenme | 1. TalepEkle.aspx sayfasını aç | Sayfa yüklenir, form ve aktif talepler listesi gösterilir | Yüksek |
| TE-F-002 | Yeni talep ekleme | 1. Tüm zorunlu alanları doldur<br>2. "Talep Ekle" butonuna tıkla | Talep kaydedilir, Durum="Aktif" olarak set edilir, liste güncellenir | Kritik |
| TE-F-003 | Zorunlu alan kontrolü - Son Tarih | 1. Gidilmesi Gereken Son Tarih boş<br>2. Kaydet | "Tarih seçiniz" validation hatası gösterilir | Yüksek |
| TE-F-004 | Zorunlu alan kontrolü - Görev Türü | 1. Görev Türü seçilmemiş<br>2. Kaydet | "Görev türü seçiniz" validation hatası | Yüksek |
| TE-F-005 | Zorunlu alan kontrolü - Adres | 1. Adres boş bırak<br>2. Kaydet | "Adres giriniz" validation hatası | Yüksek |
| TE-F-006 | Validation summary | 1. Tüm zorunlu alanları boş bırak<br>2. Kaydet | ValidationSummary alert-danger ile tüm hataları listeler | Orta |
| TE-F-007 | İvedilik seçimi | 1. İvedilik dropdown'dan seçim yap | Normal (default), Günlü, Çok İvedi seçenekleri çalışır | Düşük |
| TE-F-008 | Şube müdürlüğü listesi | 1. Dropdown'ı kontrol et | "II. Bölge Müdürlüğü" + subeler tablosundan gelen şubeler listelenir | Orta |
| TE-F-009 | Aktif talepleri listeleme | 1. Kullanıcının aktif taleplerini görüntüle | Sadece giriş yapan kullanıcının ve Durum='Aktif' olanlar listelenir | Yüksek |
| TE-F-010 | Talep seçimi | 1. Listeden bir talep seç<br>2. "Seç" butonuna tıkla | Form alanları seçilen talep bilgileri ile doldurulur, butonlar güncelleme moduna geçer | Yüksek |
| TE-F-011 | Talep güncelleme | 1. Bir talep seç<br>2. Bilgileri değiştir<br>3. "Güncelle" butonuna tıkla | Talep güncellenir, Kayit_Tarihi güncellenir | Yüksek |
| TE-F-012 | Talep silme | 1. Bir talep seç<br>2. "Sil" butonuna tıkla<br>3. Confirm dialogunda OK | Talep veritabanından silinir, liste güncellenir | Yüksek |
| TE-F-013 | Vazgeç butonu | 1. Talep seç veya düzenle<br>2. "Vazgeç" butonuna tıkla | Form temizlenir, ekleme moduna döner | Düşük |
| TE-F-014 | Yazdır fonksiyonu | 1. Formu doldur<br>2. "Yazdır" butonuna tıkla | Yeni pencere açılır, form yazdırma önizlemesi gösterilir | Orta |
| TE-F-015 | Excel export | 1. Talepleri listele<br>2. "Excel'e Aktar" butonuna tıkla | GorevTalepleri_YYYYMMDD.xls dosyası indirilir | Orta |
| TE-F-016 | Buton durumu - Ekleme modu | 1. Sayfa yükle veya Vazgeç | "Talep Ekle" butonu visible, diğerleri hidden | Düşük |
| TE-F-017 | Buton durumu - Güncelleme modu | 1. Bir talep seç | "Güncelle", "Sil", "Vazgeç" butonları visible, "Talep Ekle" hidden | Düşük |
| TE-F-018 | Kayıt sayısı badge | 1. Talepleri listele | Aktif talep sayısı badge'de gösterilir | Düşük |

### 4.2 Validation Testleri

| Test ID | Test Senaryosu | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|---------------|---------------|----------------|---------|
| TE-V-001 | RequiredFieldValidator - Son Tarih | 1. Tarihi boş bırak<br>2. Kaydet | Kırmızı "*" işareti ve hata mesajı gösterilir | Yüksek |
| TE-V-002 | RequiredFieldValidator - Görev Türü | 1. Görev türü seçilmemiş<br>2. Kaydet | InitialValue="" kontrolü ile hata verilir | Yüksek |
| TE-V-003 | RequiredFieldValidator - Adres | 1. Adres boş<br>2. Kaydet | Validation hatası gösterilir | Yüksek |
| TE-V-004 | ValidationGroup kontrolü | 1. ValidationGroup="kayit" olan alanlar kontrol edilir | Sadece kayıt grubundaki validatorlar çalışır | Orta |
| TE-V-005 | Opsiyonel alanlar | 1. İlçe, Personel Sayısı, İş Süresi, Unvan, Açıklama boş bırak<br>2. Kaydet | Validation hatası vermez, NULL veya boş string kaydedilir | Orta |
| TE-V-006 | Personel sayısı negatif | 1. Personel Sayısı alanına -5 gir | TextMode="Number" kısıtlaması ve client-side validasyon | Orta |
| TE-V-007 | İş süresi negatif | 1. İş Süresi alanına -10 gir | TextMode="Number" kısıtlaması | Orta |
| TE-V-008 | Maksimum karakter limiti | 1. MultiLine alanlara çok uzun metin gir | Veritabanı alan boyutu limiti kontrolü | Düşük |
| TE-V-009 | SQL Injection - Form alanları | 1. Form alanlarına SQL injection denemesi | Parametreli sorgu koruması | Kritik |
| TE-V-010 | XSS - Açıklama alanı | 1. Açıklama alanına `<script>alert('XSS')</script>` | Script encode edilir, çalıştırılmaz | Kritik |
| TE-V-011 | HTML Injection | 1. Unvan veya Adres alanına HTML tag'leri gir | HTML encode edilir | Orta |

### 4.3 RBAC Testleri

| Test ID | Test Senaryosu | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|---------------|---------------|----------------|---------|
| TE-R-001 | Yetki kontrolü | 1. GOREV_TAKIP_SISTEMI yetkisi olmayan kullanıcı<br>2. TalepEkle.aspx'e eriş | CheckPermission kontrolü ile erişim engellenir (satır 17-20) | Kritik |
| TE-R-002 | Kullanıcıya özel liste | 1. Farklı kullanıcılarla giriş yap<br>2. Talepleri listele | Her kullanıcı sadece kendi taleplerini görür (WHERE Kullanici = @Kullanici) | Kritik |
| TE-R-003 | Kullanıcı adı kaydı | 1. Yeni talep ekle | Kullanici alanına CurrentUserName kaydedilir | Orta |
| TE-R-004 | Başka kullanıcının talebi | 1. Başka kullanıcının talebini manuel seçmeye çalış | Sadece kendi talepleri listede olduğu için erişilemez | Yüksek |
| TE-R-005 | Log kaydı | 1. CRUD işlemleri yap | LogInfo ile işlemler loglanır (kullanıcı adı ile) | Orta |

### 4.4 Negative Testler

| Test ID | Test Senaryosu | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|---------------|---------------|----------------|---------|
| TE-N-001 | Veritabanı hatası | 1. DB bağlantısını kes<br>2. Talep ekle | Try-catch ile hata yakalanır, toast mesajı gösterilir | Yüksek |
| TE-N-002 | CurrentUserName null | 1. Session expire olmuş durumda<br>2. Talep ekle | "Oturum bilgisi bulunamadı" uyarısı, işlem yapılmaz | Yüksek |
| TE-N-003 | Olmayan talep güncelleme | 1. Talep seç, başka yerden sil<br>2. Güncelle | "Talep güncellenemedi" mesajı | Orta |
| TE-N-004 | Şubeler tablosu boş | 1. Şubeler tablosunu boşalt<br>2. Sayfayı yükle | Sadece "II. Bölge Müdürlüğü" dropdown'da gösterilir, hata vermez | Orta |
| TE-N-005 | Excel export boş liste | 1. Hiç talep yokken Excel'e aktar | "Export edilecek veri bulunamadı" uyarısı | Düşük |
| TE-N-006 | JavaScript yazdırma hatası | 1. Popup blocker aktif<br>2. Yazdır butonuna tıkla | Popup engellenir, kullanıcı uyarılır | Düşük |
| TE-N-007 | Tarih geçmiş olma kontrolü | 1. Geçmiş bir "Son Tarih" gir<br>2. Kaydet | Client-side veya server-side kontrolü olmalı (şu an yok) | Orta |
| TE-N-008 | Eşzamanlı güncelleme | 1. Aynı talebi iki tarayıcıdan güncelle | Son güncelleme geçerli olur (concurrency yok) | Orta |

---

## 5. TALEPRAPOR

**Dosya:** `ModulGorev/TalepRapor.aspx`
**Amaç:** Görev taleplerini raporlama, güncelleme ve görselleştirme

### 5.1 Fonksiyonel Testler

| Test ID | Test Senaryosu | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|---------------|---------------|----------------|---------|
| TR-F-001 | Sayfa yüklenme | 1. TalepRapor.aspx sayfasını aç | Sayfa yüklenir, istatistikler ve grafik gösterilir | Yüksek |
| TR-F-002 | İstatistik kartları | 1. Sayfa yüklendiğinde kartları kontrol et | Toplam Görev, Aktif Görev, Tamamlanan Görev sayıları gösterilir | Yüksek |
| TR-F-003 | Toplam görev hesaplama | 1. DataTable'dan toplam kayıt sayısı | lblToplamGorev doğru değeri gösterir | Orta |
| TR-F-004 | Aktif görev hesaplama | 1. Durum='Aktif' kayıtlar sayılır | lblAktifGorev doğru değeri gösterir | Orta |
| TR-F-005 | Pasif görev hesaplama | 1. Durum='Pasif' kayıtlar sayılır | lblPasifGorev doğru değeri gösterir | Orta |
| TR-F-006 | İl filtresi | 1. İl dropdown'dan bir il seç<br>2. "Ara" butonuna tıkla | Seçilen ile ait talepler listelenir | Yüksek |
| TR-F-007 | Durum filtresi | 1. Durum dropdown'dan "Aktif" veya "Pasif" seç<br>2. "Ara" butonuna tıkla | Seçilen duruma göre talepler filtrelenir | Yüksek |
| TR-F-008 | Kombine filtre | 1. İl ve Durum seç<br>2. "Ara" butonuna tıkla | Her iki filtreye uyan kayıtlar gösterilir | Orta |
| TR-F-009 | Tümünü listeleme | 1. "Tümünü Listele" butonuna tıkla | Filtreler "Hepsi" olarak reset edilir, tüm kayıtlar listelenir | Orta |
| TR-F-010 | Görev seçimi | 1. GridView'dan bir görev seç<br>2. "Seç" butonuna tıkla | Güncelleme paneli (pnlGorevGuncelle) gösterilir, Talep ID ve Açıklama yüklenir | Yüksek |
| TR-F-011 | Görev güncelleme | 1. Görev seç<br>2. Güncelleme bilgilerini doldur<br>3. "Güncelle" butonuna tıkla | Durum "Pasif" olarak güncellenir, göreve çıkış tarihi ve diğer bilgiler kaydedilir | Kritik |
| TR-F-012 | Durum pasife çekme | 1. Aktif bir görevi güncelle | Durum otomatik olarak "Pasif" yapılır | Yüksek |
| TR-F-013 | Vazgeç butonu | 1. Güncelleme panelinde "Vazgeç" tıkla | Panel kapanır (Visible=false) | Düşük |
| TR-F-014 | Excel export | 1. Talepleri listele<br>2. "Excel'e Aktar" butonuna tıkla | GorevTalepRapor_YYYYMMDD.xls dosyası indirilir | Orta |
| TR-F-015 | Chart.js grafiği | 1. Sayfa yükle<br>2. Grafik bölümünü kontrol et | İllere göre görev dağılım bar grafiği gösterilir | Orta |
| TR-F-016 | Grafik veri kaynağı | 1. HiddenField hfGrafikVerisi'ni kontrol et | JSON formatında labels ve values dizileri içerir | Orta |
| TR-F-017 | Grafik güncelleme | 1. Yeni görev ekle veya güncelle<br>2. Grafik verilerini yenile | Grafik yeni verilerle güncellenir | Orta |
| TR-F-018 | İl dropdown dolumu | 1. Sayfa yükle | gorevkayit tablosundan DISTINCT Gorev_il çekilir ve dropdown doldurulur | Orta |

### 5.2 Validation Testleri

| Test ID | Test Senaryosu | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|---------------|---------------|----------------|---------|
| TR-V-001 | Boş göreve çıkış tarihi | 1. Görev seç<br>2. Göreve Çıkış Tarihi boş bırak<br>3. Güncelle | "Göreve çıkış tarihi giriniz" uyarısı, güncelleme yapılmaz | Yüksek |
| TR-V-002 | Boş giden personel | 1. Görev seç<br>2. Giden Personel boş bırak<br>3. Güncelle | "Giden personel bilgisi giriniz" uyarısı | Yüksek |
| TR-V-003 | Opsiyonel alanlar | 1. Gidiş Türü ve Görev Süresi boş bırak<br>2. Güncelle | Güncelleme başarılı olur (opsiyonel alanlar) | Orta |
| TR-V-004 | SQL Injection - Filtre | 1. Filtre alanlarına SQL injection gir<br>2. Ara | String concatenation kullanıldığı için DİKKAT! (satır 148, 153) SQL Injection riski var! | Kritik |
| TR-V-005 | Grafik boş veri | 1. Hiç görev kaydı yokken | hfGrafikVerisi = "{\\"labels\\": [], \\"values\\": []}" | Orta |
| TR-V-006 | XSS - Açıklama | 1. Açıklama alanında script tag'leri | HttpUtility.HtmlDecode kullanılıyor, encode kontrolü yapılmalı | Kritik |

### 5.3 RBAC Testleri

| Test ID | Test Senaryosu | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|---------------|---------------|----------------|---------|
| TR-R-001 | Yetki kontrolü | 1. GOREV_TAKIP_SISTEMI yetkisi olmayan kullanıcı<br>2. TalepRapor.aspx'e eriş | CheckPermission kontrolü ile erişim engellenir (satır 18-21) | Kritik |
| TR-R-002 | Tüm talepleri görme | 1. Yetkili kullanıcı ile giriş yap | Tüm kullanıcıların talepleri görüntülenebilir (kullanıcıya özel filtre yok) | Bilgi |
| TR-R-003 | Log kaydı | 1. Arama ve güncelleme yap | LogInfo ile işlemler loglanır | Orta |

### 5.4 Negative Testler

| Test ID | Test Senaryosu | Test Adımları | Beklenen Sonuç | Öncelik |
|---------|---------------|---------------|----------------|---------|
| TR-N-001 | Veritabanı hatası | 1. DB bağlantısını kes<br>2. Sayfayı yükle | Try-catch ile hata yakalanır, toast mesajı gösterilir | Yüksek |
| TR-N-002 | Grafik yükleme hatası | 1. Grafik verisi yüklenirken hata<br>2. Sayfa yükle | Boş grafik verisi set edilir, sayfa çökmez | Orta |
| TR-N-003 | Chart.js kütüphanesi yüklenemezse | 1. CDN erişimi engelli<br>2. Sayfayı yükle | Grafik gösterilmez ama sayfa çalışır | Düşük |
| TR-N-004 | Olmayan görev güncelleme | 1. Görev seç, başka yerden sil<br>2. Güncelle | Güncelleme başarısız olur, hata mesajı | Orta |
| TR-N-005 | Excel export boş liste | 1. Kayıt yokken Excel'e aktar | "Export edilecek veri bulunamadı" uyarısı | Düşük |
| TR-N-006 | İl dropdown boş | 1. Hiç görev kaydı yokken | Dropdown sadece "Hepsi" içerir | Düşük |
| TR-N-007 | JSON parse hatası | 1. hfGrafikVerisi geçersiz JSON içerirse | JavaScript console'da hata, grafik gösterilmez | Orta |
| TR-N-008 | GridView seçim hatası | 1. Geçersiz index ile görev seçimi | Try-catch ile hata yakalanır | Düşük |

---

## GÜVENLİK AÇIKLARI VE ÖNERİLER

### Kritik Güvenlik Sorunları

#### 1. SQL Injection Riski - TalepRapor.aspx
**Konum:** `TalepRapor.aspx.cs`, satır 148 ve 153
**Sorun:** String concatenation ile SQL sorgusu oluşturuluyor:
```csharp
filtre += " AND Gorev_il = '" + ddlIl.SelectedValue + "'";
filtre += " AND Durum = '" + ddlDurum.SelectedValue + "'";
```
**Risk Seviyesi:** Kritik
**Öneri:** Parametreli sorgu kullanılmalı:
```csharp
if (ddlIl.SelectedValue != "Hepsi")
{
    filtre += " AND Gorev_il = @Il";
    parameters.Add(("@Il", ddlIl.SelectedValue));
}
```

#### 2. XSS (Cross-Site Scripting) Riski
**Konum:** Tüm sayfalar, özellikle GridView ve form alanları
**Sorun:** Kullanıcı girdileri encode edilmeden gösteriliyor
**Risk Seviyesi:** Yüksek
**Öneri:**
- `HttpUtility.HtmlEncode()` kullanılmalı
- ASP.NET'in otomatik encode özelliği aktif olmalı
- `EnableEventValidation="false"` yerine `"true"` kullanılmalı

#### 3. Yetki Kontrolü - GorevRapor.aspx
**Konum:** `GorevRapor.aspx.cs`, satır 135-138
**Sorun:** Yetki kontrolü yorum satırında:
```csharp
//if (!CheckPermission(Sabitler.GOREV_TAKIP_SISTEMI))
//{
//    return;
//}
```
**Risk Seviyesi:** Kritik
**Öneri:** Yorum satırlarını kaldırıp yetki kontrolünü aktif et

### Genel Öneriler

1. **Input Validation:**
   - Client-side validation yanında mutlaka server-side validation yapılmalı
   - Tüm numerik alanlar için min/max değer kontrolü eklenmeli
   - Tarih alanları için geçmiş/gelecek kontrolü yapılmalı

2. **Error Handling:**
   - Hata mesajları kullanıcıya detaylı sistem bilgisi vermemeli
   - Tüm hatalar loglama sistemine kaydedilmeli
   - Generic hata mesajları kullanılmalı

3. **Concurrency Control:**
   - Optimistic concurrency kontrolü eklenmeli
   - Timestamp veya RowVersion kullanılmalı

4. **Session Management:**
   - Session timeout kontrolleri her sayfada yapılmalı
   - Kullanıcı oturumu süresi ayarlanmalı

5. **Logging:**
   - Tüm CRUD işlemleri detaylı loglanmalı
   - Başarısız giriş denemeleri kaydedilmeli
   - Audit trail oluşturulmalı

---

## TEST ÇALIŞTIRMA TALİMATLARI

### Ortam Hazırlığı

1. **Test Veritabanı:**
   - Ayrı bir test veritabanı oluşturun
   - Gerçek verilerle test yapmayın
   - Test verilerini script'lerle hazırlayın

2. **Test Kullanıcıları:**
   - Farklı yetki seviyelerinde test kullanıcıları oluşturun
   - GOREV_TAKIP_SISTEMI yetkisi olan ve olmayan kullanıcılar

3. **Tarayıcılar:**
   - Chrome, Firefox, Edge, Safari ile test yapın
   - Mobil responsive test yapın

### Test Sırası

1. **Fonksiyonel Testler:** İlk olarak tüm temel fonksiyonlar test edilmeli
2. **Validation Testler:** Form validasyonları ve veri doğrulamaları
3. **RBAC Testler:** Yetkilendirme ve erişim kontrolleri
4. **Negative Testler:** Hata senaryoları ve istisna durumlar
5. **Güvenlik Testler:** SQL Injection, XSS, CSRF testleri
6. **Performans Testler:** Yük ve stres testleri

### Test Raporlama

Her test için aşağıdaki bilgiler kaydedilmeli:
- Test ID
- Test Tarihi
- Test Eden Kişi
- Test Sonucu (Pass/Fail)
- Hata Ekran Görüntüsü (varsa)
- Hata Detayı
- Öncelik Seviyesi

---

## SONUÇ

Bu test dökümanı **ModulGorev** altındaki 5 sayfanın kapsamlı testini içermektedir:

- **GorevRapor.aspx:** 14 Fonksiyonel, 8 Validation, 4 RBAC, 7 Negative = **33 Test**
- **Gorevlendirme.aspx:** 16 Fonksiyonel, 12 Validation, 4 RBAC, 7 Negative = **39 Test**
- **PersonelRapor.aspx:** 11 Fonksiyonel, 5 Validation, 3 RBAC, 5 Negative = **24 Test**
- **TalepEkle.aspx:** 18 Fonksiyonel, 11 Validation, 5 RBAC, 8 Negative = **42 Test**
- **TalepRapor.aspx:** 18 Fonksiyonel, 6 Validation, 3 RBAC, 8 Negative = **35 Test**

**TOPLAM: 173 Test Senaryosu**

### Kritik Güvenlik Sorunları
- SQL Injection riski (TalepRapor.aspx)
- Yetki kontrolü devre dışı (GorevRapor.aspx)
- XSS riski (tüm sayfalar)

### Öncelikli Düzeltmeler
1. TalepRapor.aspx'te parametreli sorgu kullanımı
2. GorevRapor.aspx'te yetki kontrolünün aktifleştirilmesi
3. Tüm sayfalarda input encoding kontrolü
4. Concurrency kontrolü eklenmesi

---

**Test Dökümanı Hazırlayan:** Senior QA Engineer
**Son Güncelleme:** 2025-11-05
**Döküman Versiyonu:** 1.0
