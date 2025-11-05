# DENETIM MODÜLÜ TEST DOKÜMANI

**Proje:** Portal Yeni
**Modül:** ModulDenetim
**Test Tarihi:** 2025-11-05
**Hazırlayan:** Senior QA Engineer
**Test Ortamı:** .NET Web Forms Application

---

## İÇİNDEKİLER

1. [Test Kapsamı](#test-kapsami)
2. [İşletme Denetim Giriş Sayfası](#isletme-denetim-giris)
3. [Taşıt Denetim Giriş Sayfası](#tasit-denetim-giris)
4. [Uzaktan Denetim Kayıt Sayfası](#uzaktan-denetim-kayit)
5. [Uzak Görev Takibi Sayfası](#uzak-gorev-takibi)
6. [İşletme Denetim Raporlama](#isletme-rapor)
7. [Uzaktan Denetim Raporları](#uzak-rapor)
8. [Denetim İstatistikleri](#istatistik)
9. [Test Özeti](#test-ozeti)

---

## TEST KAPSAMI

### Test Edilen Sayfalar
- IsletmeGiris.aspx - İşletme Denetim Girişi
- TasitGiris.aspx - Taşıt Denetim Girişi
- UzakEkle.aspx - Uzaktan Denetim Kayıt
- UzakGorev.aspx - Uzak Görev Takibi
- IsletmeRapor.aspx - İşletme Denetim Raporlama
- UzakRapor.aspx - Uzaktan Denetim Raporları
- Istatistik.aspx - Denetim İstatistikleri
- TasitRapor.aspx - Taşıt Denetim Raporlama (Boş sayfa)

### Test Türleri
- **Fonksiyonel Testler:** Temel işlevsellik ve iş akışı testleri
- **Validation Testler:** Form validasyon ve veri doğrulama testleri
- **RBAC Testler:** Rol bazlı erişim kontrol testleri
- **Negative Testler:** Hata senaryoları ve sınır değer testleri

---

## İŞLETME DENETİM GİRİŞ

### Sayfa Bilgileri
- **Dosya:** IsletmeGiris.aspx / IsletmeGiris.aspx.cs
- **Yetki Sabiti:** DENETIM_ISLETME
- **Silme Yetkisi:** DENETIM_ISLETME_SILME
- **Veritabanı Tablosu:** denetimisletme

---

### TC-DEN-001: Yeni İşletme Denetim Kaydı Ekleme (Fonksiyonel)

**Önkoşul:**
- Kullanıcı sisteme giriş yapmış olmalı
- DENETIM_ISLETME yetkisi bulunmalı

**Test Adımları:**
1. IsletmeGiris.aspx sayfasına gidin
2. Vergi No alanına geçerli bir vergi numarası girin (örn: 1234567890)
3. Yetki Belgesi dropdown'dan bir belge seçin
4. Firma Unvanı alanına "Test Firma A.Ş." yazın
5. Firma Adresi alanına geçerli bir adres yazın
6. Denetim Türü olarak "Eşya Taşımacılığı" seçin
7. Denetim Tarihi seçin (örn: bugünün tarihi)
8. İl dropdown'dan bir il seçin
9. İlçe dropdown'dan bir ilçe seçin
10. Denetim Personeli 1 dropdown'dan bir personel seçin
11. Ceza Durumu olarak "Para Cezası" seçin
12. Açıklama alanına "Test denetim kaydı" yazın
13. "Kaydet" butonuna tıklayın

**Beklenen Sonuç:**
- Kayıt başarıyla veritabanına eklenmelidir
- "Denetim kaydı başarıyla eklendi." toast mesajı görünmelidir
- Form temizlenmelidir
- Log kaydı oluşturulmalıdır

**Dosya Referansı:** IsletmeGiris.aspx.cs:144-197

---

### TC-DEN-002: İşletme Kaydı Arama ve Güncelleme (Fonksiyonel)

**Önkoşul:**
- Sistemde en az bir işletme denetim kaydı bulunmalı
- Kullanıcı DENETIM_ISLETME yetkisine sahip olmalı

**Test Adımları:**
1. IsletmeGiris.aspx sayfasına gidin
2. Kayıt No alanına mevcut bir kayıt ID'si girin
3. "Bul" butonuna tıklayın
4. Firma Unvanı alanını "Güncellenmiş Firma A.Ş." olarak değiştirin
5. Ceza Durumu'nu "Uyarı Cezası" olarak değiştirin
6. "Güncelle" butonuna tıklayın

**Beklenen Sonuç:**
- Kayıt bulunmalı ve form alanları doldurulmalıdır
- "Kaydet" butonu gizlenmeli, "Güncelle" ve "Vazgeç" butonları görünmelidir
- Güncelleme başarılı olmalıdır
- "Denetim kaydı başarıyla güncellendi." mesajı görünmelidir
- Form temizlenmelidir
- GuncellemeTarihi ve GuncellemeKullanici alanları doldurulmalıdır

**Dosya Referansı:** IsletmeGiris.aspx.cs:264-328, 199-262

---

### TC-DEN-003: İl Seçimine Göre İlçe Yükleme (Fonksiyonel)

**Önkoşul:**
- Kullanıcı IsletmeGiris sayfasında olmalı

**Test Adımları:**
1. IsletmeGiris.aspx sayfasına gidin
2. İl dropdown'dan "ANKARA" seçin
3. İlçe dropdown'ını kontrol edin
4. İl dropdown'dan "İSTANBUL" seçin
5. İlçe dropdown'ını kontrol edin

**Beklenen Sonuç:**
- İl seçildiğinde ilgili ilçeler yüklenmelidir
- İlçe dropdown'u etkinleştirilmelidir
- Her il değişikliğinde ilçeler dinamik olarak yüklenmelidir
- İlçe dropdown'u AutoPostBack ile çalışmalıdır

**Dosya Referansı:** IsletmeGiris.aspx.cs:372-383

---

### TC-DEN-004: Zorunlu Alan Validasyonu (Validation)

**Önkoşul:**
- Kullanıcı IsletmeGiris sayfasında olmalı

**Test Adımları:**
1. IsletmeGiris.aspx sayfasına gidin
2. Hiçbir alan doldurmadan "Kaydet" butonuna tıklayın
3. ValidationSummary kontrolünü gözlemleyin

**Beklenen Sonuç:**
- Sayfa submit edilmemelidir
- Aşağıdaki zorunlu alan hataları görünmelidir:
  - "Vergi numarası zorunludur."
  - "Yetki belgesi seçiniz."
  - "Firma unvanı zorunludur."
  - "Firma adresi zorunludur."
  - "Denetim türü seçiniz."
  - "Denetim tarihi zorunludur."
  - "İl seçiniz."
  - "İlçe seçiniz."
  - "En az 1 personel seçmelisiniz."
  - "Ceza durumu seçiniz."
- RequiredFieldValidator'lar aktif olmalıdır
- ValidationSummary'de tüm hatalar listelenmelidir

**Dosya Referansı:** IsletmeGiris.aspx:68-183

---

### TC-DEN-005: İşletme Kaydı Silme Yetkisi (RBAC)

**Önkoşul:**
- Test için iki farklı kullanıcı gereklidir:
  - Kullanıcı A: DENETIM_ISLETME_SILME yetkisi olan
  - Kullanıcı B: DENETIM_ISLETME_SILME yetkisi olmayan

**Test Adımları (Kullanıcı A):**
1. DENETIM_ISLETME_SILME yetkisine sahip kullanıcı ile giriş yapın
2. IsletmeGiris.aspx sayfasına gidin
3. "Sil" butonunun görünürlüğünü kontrol edin

**Test Adımları (Kullanıcı B):**
4. DENETIM_ISLETME_SILME yetkisi olmayan kullanıcı ile giriş yapın
5. IsletmeGiris.aspx sayfasına gidin
6. "Sil" butonunun görünürlüğünü kontrol edin

**Beklenen Sonuç:**
- Kullanıcı A için "Sil" butonu görünür olmalıdır
- Kullanıcı B için "Sil" butonu görünmez olmalıdır
- Yetki kontrolü Page_Load'da yapılmalıdır
- SilmeYetkisiniKontrolEt() metodu doğru çalışmalıdır

**Dosya Referansı:** IsletmeGiris.aspx.cs:41-68

---

### TC-DEN-006: Sayfa Erişim Yetkisi Kontrolü (RBAC)

**Önkoşul:**
- DENETIM_ISLETME yetkisi olmayan bir kullanıcı

**Test Adımları:**
1. Yetkisiz kullanıcı ile sisteme giriş yapın
2. Browser'da manuel olarak IsletmeGiris.aspx URL'sini yazın
3. Sayfaya erişmeye çalışın

**Beklenen Sonuç:**
- Sayfa erişimi engellenmelidir
- CheckPermission(Sabitler.DENETIM_ISLETME) false dönmelidir
- Kullanıcı yetkisiz erişim mesajı almalı veya yönlendirilmelidir
- Sayfa içeriği yüklenmemelidir

**Dosya Referansı:** IsletmeGiris.aspx.cs:21-24

---

### TC-DEN-007: Olmayan Kayıt Arama (Negative)

**Önkoşul:**
- Kullanıcı IsletmeGiris sayfasında olmalı

**Test Adımları:**
1. IsletmeGiris.aspx sayfasına gidin
2. Kayıt No alanına veritabanında olmayan bir ID girin (örn: 999999)
3. "Bul" butonuna tıklayın

**Beklenen Sonuç:**
- "Kayıt bulunamadı." uyarı mesajı görünmelidir
- Form alanları boş kalmalıdır
- Güncelleme moduna geçilmemelidir
- Hata fırlatılmamalıdır

**Dosya Referansı:** IsletmeGiris.aspx.cs:318-321

---

### TC-DEN-008: Kayıt Silme İşlemi (Fonksiyonel)

**Önkoşul:**
- DENETIM_ISLETME_SILME yetkisi bulunmalı
- Silinebilir bir test kaydı bulunmalı

**Test Adımları:**
1. DENETIM_ISLETME_SILME yetkili kullanıcı ile giriş yapın
2. IsletmeGiris.aspx sayfasına gidin
3. Mevcut bir kayıt ID'si ile arama yapın
4. "Sil" butonuna tıklayın
5. JavaScript confirm dialog'unda "OK" butonuna tıklayın

**Beklenen Sonuç:**
- Confirm dialog gösterilmelidir: "Bu kaydı silmek istediğinizden emin misiniz?"
- Kayıt veritabanından silinmelidir
- "Denetim kaydı başarıyla silindi." mesajı görünmelidir
- Form temizlenmelidir
- Silme işlemi loglanmalıdır

**Dosya Referansı:** IsletmeGiris.aspx.cs:337-370, IsletmeGiris.aspx:220

---

### TC-DEN-009: Form Vazgeç İşlemi (Fonksiyonel)

**Önkoşul:**
- Güncelleme modunda bir kayıt açık olmalı

**Test Adımları:**
1. IsletmeGiris.aspx sayfasına gidin
2. Bir kayıt arayın ve yükleyin (güncelleme moduna geçin)
3. Bazı alanları değiştirin
4. "Vazgeç" butonuna tıklayın

**Beklenen Sonuç:**
- Form temizlenmelidir
- Güncelleme modundan çıkılmalıdır
- "Kaydet" butonu görünür olmalıdır
- "Güncelle" ve "Vazgeç" butonları gizlenmelidir
- "İşlem iptal edildi." mesajı görünmelidir
- Değişiklikler kaydedilmemelidir

**Dosya Referansı:** IsletmeGiris.aspx.cs:330-335

---

### TC-DEN-010: Geçersiz Tarih Formatı (Negative)

**Önkoşul:**
- Kullanıcı IsletmeGiris sayfasında olmalı

**Test Adımları:**
1. IsletmeGiris.aspx sayfasına gidin
2. Tüm zorunlu alanları doldurun
3. Denetim Tarihi alanına geçersiz bir format girin (browser kontrolü dışında)
4. "Kaydet" butonuna tıklayın

**Beklenen Sonuç:**
- DateTimeLocal input type validasyonu çalışmalıdır
- Geçersiz tarih kabul edilmemelidir
- Hata mesajı gösterilmelidir
- Kayıt oluşturulmamalıdır

**Dosya Referansı:** IsletmeGiris.aspx:127-131

---

## TAŞIT DENETİM GİRİŞ

### Sayfa Bilgileri
- **Dosya:** TasitGiris.aspx / TasitGiris.aspx.cs
- **Yetki Sabiti:** DENETIM_TASIT_GIRIS
- **Silme Yetkisi:** DENETIM_TASIT_SILME
- **Veritabanı Tablosu:** denetimtasit

---

### TC-DEN-011: Yeni Taşıt Denetim Kaydı Ekleme (Fonksiyonel)

**Önkoşul:**
- Kullanıcı DENETIM_TASIT_GIRIS yetkisine sahip olmalı

**Test Adımları:**
1. TasitGiris.aspx sayfasına gidin
2. Plaka No alanına "06UAB1989" yazın
3. Yarı Römork Plakası alanına "34TR2020" yazın (opsiyonel)
4. Taşıt/Yetki Belgesi Unvan alanına "ABC Taşımacılık Ltd." yazın
5. Denetim Yeri dropdown'dan bir yer seçin
6. Yetki Belgesi dropdown'dan bir belge seçin
7. Denetim Türü olarak "Tehlikeli Madde" seçin
8. Denetim Tarihi seçin
9. Denetim İl olarak bir il seçin
10. Denetim İlçe olarak bir ilçe seçin
11. Denetleyen Personel dropdown'dan bir personel seçin
12. Ceza Durumu olarak "Yok" seçin
13. Açıklama alanına "Taşıt denetim testi" yazın
14. "Kaydet" butonuna tıklayın

**Beklenen Sonuç:**
- Plaka otomatik uppercase'e çevrilmelidir
- Kayıt başarıyla eklenmeli
- "Denetim kaydı başarıyla eklendi." mesajı görünmelidir
- Form temizlenmelidir
- KayitTarihi ve KayitKullanici alanları otomatik doldurulmalıdır

**Dosya Referansı:** TasitGiris.aspx.cs:129-181

---

### TC-DEN-012: Duplicate Kayıt Kontrolü (Validation)

**Önkoşul:**
- Sistemde belirli plaka ve tarih ile bir kayıt bulunmalı

**Test Adımları:**
1. TasitGiris.aspx sayfasına gidin
2. Var olan bir kaydın plakasını girin
3. Aynı denetim tarihini girin
4. Diğer zorunlu alanları doldurun
5. "Kaydet" butonuna tıklayın

**Beklenen Sonuç:**
- "Aynı tarihte aynı plaka ile denetim kaydı zaten mevcut." mesajı görünmelidir
- Kayıt eklenmemelidir
- KayitVarMi() metodu duplicate kontrolü yapmalıdır

**Dosya Referansı:** TasitGiris.aspx.cs:135-140, 284-297

---

### TC-DEN-013: Plaka Uppercase Dönüşümü (Fonksiyonel)

**Önkoşul:**
- Kullanıcı TasitGiris sayfasında olmalı

**Test Adımları:**
1. TasitGiris.aspx sayfasına gidin
2. Plaka alanına küçük harflerle "06abc123" yazın
3. Plaka2 alanına "34tr456" yazın
4. Diğer zorunlu alanları doldurun
5. "Kaydet" butonuna tıklayın
6. Veritabanından kaydı kontrol edin

**Beklenen Sonuç:**
- Plakalar uppercase olarak kaydedilmelidir
- Veritabanında "06ABC123" ve "34TR456" olarak görünmelidir
- CSS class text-uppercase uygulanmalıdır

**Dosya Referansı:** TasitGiris.aspx.cs:153-154, TasitGiris.aspx:75-76

---

### TC-DEN-014: Taşıt Kaydı Güncelleme ile Duplicate Kontrolü (Validation)

**Önkoşul:**
- Sistemde en az 2 taşıt denetim kaydı bulunmalı

**Test Adımları:**
1. TasitGiris.aspx sayfasına gidin
2. Kayıt A'yı bulun ve yükleyin
3. Plaka alanını Kayıt B'nin plakası ile değiştirin
4. Denetim Tarihi'ni Kayıt B ile aynı yapın
5. "Güncelle" butonuna tıklayın

**Beklenen Sonuç:**
- "Aynı tarihte aynı plaka ile denetim kaydı zaten mevcut." mesajı görünmelidir
- Güncelleme yapılmamalıdır
- Kendi ID'si hariç duplicate kontrolü yapılmalıdır

**Dosya Referansı:** TasitGiris.aspx.cs:189-208

---

### TC-DEN-015: Taşıt Kaydı Silme (Fonksiyonel)

**Önkoşul:**
- DENETIM_TASIT_SILME yetkisi bulunmalı
- Silinecek bir test kaydı bulunmalı

**Test Adımları:**
1. DENETIM_TASIT_SILME yetkili kullanıcı ile giriş yapın
2. TasitGiris.aspx sayfasına gidin
3. Bir kayıt bulun
4. "Sil" butonuna tıklayın
5. Confirm dialog'unda onaylayın

**Beklenen Sonuç:**
- Confirm dialog gösterilmelidir
- Kayıt silinmelidir
- "Denetim kaydı başarıyla silindi." mesajı görünmelidir
- Silme işlemi loglanmalıdır

**Dosya Referansı:** TasitGiris.aspx.cs:259-282

---

### TC-DEN-016: Taşıt Zorunlu Alan Validasyonu (Validation)

**Önkoşul:**
- Kullanıcı TasitGiris sayfasında olmalı

**Test Adımları:**
1. TasitGiris.aspx sayfasına gidin
2. Sadece Plaka alanını doldurun
3. "Kaydet" butonuna tıklayın

**Beklenen Sonuç:**
- Aşağıdaki validation hataları görünmelidir:
  - "Unvan alanı zorunludur."
  - "Denetim yeri seçimi zorunludur."
  - "Denetim türü seçimi zorunludur."
  - "Denetim tarihi zorunludur."
  - "İl seçimi zorunludur."
  - "İlçe seçimi zorunludur."
  - "Personel seçimi zorunludur."
  - "Ceza durumu seçimi zorunludur."
- Form submit edilmemelidir

**Dosya Referansı:** TasitGiris.aspx:78-196

---

### TC-DEN-017: ReadOnly Alan Kontrolü (Fonksiyonel)

**Önkoşul:**
- Bir taşıt kaydı güncelleme modunda açık olmalı

**Test Adımları:**
1. TasitGiris.aspx sayfasına gidin
2. Bir kayıt bulun
3. Güncelleme moduna geçin
4. Kayıt No ve Unvan alanlarının ReadOnly durumunu kontrol edin

**Beklenen Sonuç:**
- Kayıt No alanı ReadOnly olmalıdır
- Unvan alanı ReadOnly olmalıdır
- Bu alanlar değiştirilememelidir

**Dosya Referansı:** TasitGiris.aspx.cs:115-116

---

### TC-DEN-018: Denetim Yeri Dropdown Yükleme (Fonksiyonel)

**Önkoşul:**
- denetimyerleri tablosunda kayıt bulunmalı

**Test Adımları:**
1. TasitGiris.aspx sayfasına gidin
2. Denetim Yeri dropdown'unu kontrol edin

**Beklenen Sonuç:**
- Dropdown denetimyerleri tablosundan yüklenmelidir
- "Seçiniz..." ilk item olarak görünmelidir
- Alfabetik sıralama yapılmalıdır

**Dosya Referansı:** TasitGiris.aspx.cs:58-60

---

## UZAKTAN DENETİM KAYIT

### Sayfa Bilgileri
- **Dosya:** UzakEkle.aspx / UzakEkle.aspx.cs
- **Yetki Sabiti:** DENETIM_UZAK_GIRIS
- **Veritabanı Tablosu:** denetimuzak

---

### TC-DEN-019: Yeni Uzak Denetim Kaydı Ekleme (Fonksiyonel)

**Önkoşul:**
- Kullanıcı DENETIM_UZAK_GIRIS yetkisine sahip olmalı

**Test Adımları:**
1. UzakEkle.aspx sayfasına gidin
2. Tarih alanına bugünün tarihini seçin
3. Araç Sayısı alanına "150" yazın
4. Atanan Personel dropdown'dan bir personel seçin
5. İşlem Durumu olarak "Açık" seçin
6. Açıklama alanına "Uzak denetim testi" yazın
7. "Kaydet" butonuna tıklayın

**Beklenen Sonuç:**
- Kayıt başarıyla eklenmeli
- "Kayıt başarıyla eklendi." mesajı görünmelidir
- Form temizlenmelidir
- KayitTarihi ve KayitKullanici otomatik doldurulmalıdır

**Dosya Referansı:** UzakEkle.aspx.cs:43-90

---

### TC-DEN-020: Uzak Denetim Kaydı Güncelleme (Fonksiyonel)

**Önkoşul:**
- Sistemde en az bir uzak denetim kaydı bulunmalı

**Test Adımları:**
1. UzakEkle.aspx sayfasına gidin
2. Kayıt ID alanına mevcut bir ID girin
3. "Ara" butonuna tıklayın
4. Araç Sayısı'nı "200" olarak değiştirin
5. İşlem Durumu'nu "Kapalı" yapın
6. "Güncelle" butonuna tıklayın

**Beklenen Sonuç:**
- Kayıt bulunmalı ve yüklenmeli
- "Kayıt bulundu." mesajı görünmelidir
- Uygunsuz Araç, Ceza Uygulanan Araç ve YB Kayıtlı Olmayan alanları görünür olmalıdır
- Güncelleme başarılı olmalıdır
- "Kayıt başarıyla güncellendi." mesajı görünmelidir
- Sayfa UzakEkle.aspx'e redirect edilmelidir

**Dosya Referansı:** UzakEkle.aspx.cs:92-149, 151-205

---

### TC-DEN-021: ReadOnly Alanlar Görünürlük Kontrolü (Fonksiyonel)

**Önkoşul:**
- Kullanıcı UzakEkle sayfasında olmalı

**Test Adımları:**
1. UzakEkle.aspx sayfasına yeni kayıt modunda gidin
2. divUygunsuzArac, divCezaliArac, divYBKayitliOlmayan div'lerinin görünürlüğünü kontrol edin
3. Bir kayıt arayın ve yükleyin
4. Bu div'lerin görünürlüğünü tekrar kontrol edin

**Beklenen Sonuç:**
- Yeni kayıt modunda bu alanlar görünmez olmalıdır (Visible=false)
- Güncelleme modunda bu alanlar görünür olmalıdır
- Alanlar ReadOnly ve BackColor="#f8f9fa" olmalıdır

**Dosya Referansı:** UzakEkle.aspx.cs:129-135, UzakEkle.aspx:107-131

---

### TC-DEN-022: Uzak Denetim Zorunlu Alan Validasyonu (Validation)

**Önkoşul:**
- Kullanıcı UzakEkle sayfasında olmalı

**Test Adımları:**
1. UzakEkle.aspx sayfasına gidin
2. Hiçbir alan doldurmadan "Kaydet" butonuna tıklayın

**Beklenen Sonuç:**
- Aşağıdaki validation hataları görünmelidir:
  - "Tarih zorunludur"
  - "Araç sayısı zorunludur"
  - "Personel seçimi zorunludur"
  - "İşlem durumu zorunludur"
- RequiredFieldValidator'lar çalışmalıdır
- Form submit edilmemelidir

**Dosya Referansı:** UzakEkle.aspx:72-147

---

### TC-DEN-023: Oturum Bilgisi Kontrolü (Security)

**Önkoşul:**
- Session'dan CurrentUserName bilgisi alınmalı

**Test Adımları:**
1. Session'dan kullanıcı bilgisini temizleyin (manual test için)
2. UzakEkle.aspx sayfasından kayıt eklemeye çalışın

**Beklenen Sonuç:**
- "Oturum bilgisi bulunamadı. Lütfen tekrar giriş yapınız." mesajı görünmelidir
- Kayıt eklenmemelidir
- CurrentUserName null kontrolü yapılmalıdır

**Dosya Referansı:** UzakEkle.aspx.cs:49-54, 157-162

---

### TC-DEN-024: Vazgeç İşlemi ile Sayfa Yenileme (Fonksiyonel)

**Önkoşul:**
- Güncelleme modunda bir kayıt açık olmalı

**Test Adımları:**
1. UzakEkle.aspx sayfasına gidin
2. Bir kayıt bulun ve güncelleme moduna geçin
3. "Vazgeç" butonuna tıklayın

**Beklenen Sonuç:**
- Sayfa yenilenmeli (Response.Redirect)
- UzakEkle.aspx'e yönlendirilmelidir
- Form temizlenmelidir

**Dosya Referansı:** UzakEkle.aspx.cs:207-210

---

## UZAK GÖREV TAKİBİ

### Sayfa Bilgileri
- **Dosya:** UzakGorev.aspx / UzakGorev.aspx.cs
- **Yetki Sabiti:** DENETIM_UZAK_GOREV
- **Veritabanı Tablosu:** denetimuzak

---

### TC-DEN-025: Açık Görevleri Listeleme (Fonksiyonel)

**Önkoşul:**
- Kullanıcı DENETIM_UZAK_GOREV yetkisine sahip olmalı
- Kullanıcıya atanmış Açık statüsünde görevler bulunmalı

**Test Adımları:**
1. UzakGorev.aspx sayfasına gidin
2. Sayfa yüklendiğinde GridView'i kontrol edin
3. "Açık Görevler" butonuna tıklayın

**Beklenen Sonuç:**
- Sadece kullanıcıya atanmış ve Durum="Açık" olan kayıtlar görünmelidir
- GridView dolulmalıdır
- Kayıt sayısı badge'de gösterilmelidir
- Tarih'e göre ASC sıralama yapılmalıdır

**Dosya Referansı:** UzakGorev.aspx.cs:27-74, 158-161

---

### TC-DEN-026: Tüm Görevleri Listeleme (Fonksiyonel)

**Önkoşul:**
- Kullanıcıya atanmış çeşitli durumlarda görevler bulunmalı

**Test Adımları:**
1. UzakGorev.aspx sayfasına gidin
2. "Tüm Görevler" butonuna tıklayın

**Beklened Sonuç:**
- Kullanıcıya atanmış tüm görevler görünmelidir (Açık ve Kapalı)
- Tarih'e göre DESC sıralama yapılmalıdır
- "Toplam X görev listelendi." mesajı görünmelidir
- Kayıt sayısı güncellenmelidir

**Dosya Referansı:** UzakGorev.aspx.cs:79-124, 166-169

---

### TC-DEN-027: Grid Satır Seçimi ve Detay Paneli (Fonksiyonel)

**Önkoşul:**
- GridView'de en az bir kayıt bulunmalı

**Test Adımları:**
1. UzakGorev.aspx sayfasına gidin
2. GridView'de bir satırın "Seç" butonuna tıklayın
3. Detay panelini kontrol edin

**Beklenen Sonuç:**
- PanelDetay görünür olmalıdır (Visible=true)
- Seçilen satırın verileri form alanlarına yüklenmelidir
- Tarih ve Araç Sayısı ReadOnly olmalıdır
- Uygunsuz Araç, YB Olmayan ve YB Kayıtlı Olmayan alanları doldurulabilir olmalıdır
- HtmlDecode ile veriler decode edilmelidir

**Dosya Referansı:** UzakGorev.aspx.cs:129-153

---

### TC-DEN-028: Görev Kapatma ve Raporlama (Fonksiyonel)

**Önkoşul:**
- Bir görev seçilmiş ve detay paneli açık olmalı

**Test Adımları:**
1. UzakGorev.aspx sayfasına gidin
2. GridView'den bir Açık görev seçin
3. Detay panelinde:
   - Uygunsuz Araç Sayısı: "25"
   - YB Olmayan Araç Sayısı: "10"
   - YB Kayıtlı Olmayan Araç Sayısı: "5"
   - Açıklama: "Görev tamamlandı"
4. "Kaydet ve Kapat" butonuna tıklayın

**Beklenen Sonuç:**
- Görev durumu "Kapalı" olarak güncellenmelidir
- DenetimKayitTarihi güncel tarih olarak kaydedilmelidir
- GuncelleyenKullanici ve GuncellemeTarihi doldurulmalıdır
- "Görev başarıyla kapatıldı ve kaydedildi." mesajı görünmelidir
- GridView yenilenmeli ve kapanan görev listeden çıkmalıdır (Açık görevler modundaysa)
- Detay paneli kapanmalıdır

**Dosya Referansı:** UzakGorev.aspx.cs:174-232

---

### TC-DEN-029: Görev Kapatma Validasyonu (Validation)

**Önkoşul:**
- Bir görev seçilmiş ve detay paneli açık olmalı

**Test Adımları:**
1. UzakGorev.aspx sayfasına gidin
2. Bir görev seçin
3. Detay panelinde zorunlu alanları boş bırakın
4. "Kaydet ve Kapat" butonuna tıklayın

**Beklenen Sonuç:**
- RequiredFieldValidator'lar çalışmalıdır
- "Zorunlu alan" hataları görünmelidir
- Form submit edilmemelidir
- Görev kapatılmamalıdır

**Dosya Referansı:** UzakGorev.aspx:131-157

---

### TC-DEN-030: Excel Export İşlemi (Fonksiyonel)

**Önkoşul:**
- GridView'de en az bir kayıt bulunmalı

**Test Adımları:**
1. UzakGorev.aspx sayfasına gidin
2. GridView'de kayıtlar yüklenmiş olsun
3. "Excel'e Aktar" butonuna tıklayın

**Beklenen Sonuç:**
- Excel dosyası indirilmelidir
- Dosya adı "UzakGorevler_YYYYMMDD.xls" formatında olmalıdır
- GridView içeriği Excel'e aktarılmalıdır
- ExportGridViewToExcel metodu çalışmalıdır
- İşlem loglanmalıdır

**Dosya Referansı:** UzakGorev.aspx.cs:246-264

---

### TC-DEN-031: Boş Grid Excel Export (Negative)

**Önkoşul:**
- GridView'de kayıt bulunmamalı

**Test Adımları:**
1. UzakGorev.aspx sayfasına gidin
2. Hiç görev olmayan bir kullanıcı ile giriş yapın (veya tüm görevleri silin)
3. "Excel'e Aktar" butonuna tıklayın

**Beklenen Sonuç:**
- "Export edilecek veri bulunamadı." uyarı mesajı görünmelidir
- Excel dosyası oluşturulmamalıdır
- İşlem yapılmamalıdır

**Dosya Referansı:** UzakGorev.aspx.cs:248-252

---

### TC-DEN-032: Kayıt Sayısı Badge Güncelleme (Fonksiyonel)

**Önkoşul:**
- Kullanıcı UzakGorev sayfasında olmalı

**Test Adımları:**
1. UzakGorev.aspx sayfasına gidin
2. Kayıt sayısı badge'ini kontrol edin
3. "Açık Görevler" butonuna tıklayın ve badge'i kontrol edin
4. "Tüm Görevler" butonuna tıklayın ve badge'i kontrol edin

**Beklenen Sonuç:**
- Badge dinamik olarak güncellenmelidir
- Kayıt varsa: "X kayıt" ve bg-primary class
- Kayıt yoksa: "Kayıt yok" ve bg-secondary class
- KayitSayisiniGuncelle metodu doğru çalışmalıdır

**Dosya Referansı:** UzakGorev.aspx.cs:276-288

---

### TC-DEN-033: Durum Badge Görünümü (UI/Functional)

**Önkoşul:**
- GridView'de hem Açık hem Kapalı statülü kayıtlar bulunmalı

**Test Adımları:**
1. UzakGorev.aspx sayfasına gidin
2. "Tüm Görevler" butonuna tıklayın
3. GridView'deki Durum sütununu kontrol edin

**Beklenen Sonuç:**
- Açık statülü kayıtlar için "durum-badge durum-acik" class uygulanmalıdır
- Kapalı statülü kayıtlar için "durum-badge durum-kapali" class uygulanmalıdır
- TemplateField doğru render edilmelidir

**Dosya Referansı:** UzakGorev.aspx:82-88

---

## İŞLETME DENETİM RAPORLAMA

### Sayfa Bilgileri
- **Dosya:** IsletmeRapor.aspx / IsletmeRapor.aspx.cs
- **Yetki Sabiti:** (BasePage kontrolü)
- **Veritabanı Tablosu:** denetimisletme

---

### TC-DEN-034: Çoklu Filtre ile Arama (Fonksiyonel)

**Önkoşul:**
- Veritabanında test kayıtları bulunmalı

**Test Adımları:**
1. IsletmeRapor.aspx sayfasına gidin
2. Vergi No alanına bir vergi numarası girin
3. Firma Unvanı alanına bir unvan yazın
4. Belge Türü dropdown'dan bir belge seçin
5. Denetim Türü olarak "Yolcu Taşımacılığı" seçin
6. Denetim İl olarak bir il seçin
7. Denetim İlçe olarak bir ilçe seçin
8. Denetleyen Personel seçin
9. Ceza Durumu olarak "Para Cezası" seçin
10. Başlangıç ve Bitiş Tarihi seçin
11. "Filtrele" butonuna tıklayın

**Beklenen Sonuç:**
- Tüm filtrelere uygun kayıtlar GridView'de listelenmelidir
- Toplam kayıt sayısı lblKayitSayisi'nde görünmelidir
- SQL query tüm filtreleri WHERE koşullarında kullanmalıdır
- EmptyDataTemplate gösterilmemeli (kayıt varsa)

**Dosya Referansı:** IsletmeRapor.aspx:102-193

---

### TC-DEN-035: İl-İlçe Cascade Dropdown (Fonksiyonel)

**Önkoşul:**
- Kullanıcı IsletmeRapor sayfasında olmalı

**Test Adımları:**
1. IsletmeRapor.aspx sayfasına gidin
2. İl dropdown'dan "ANTALYA" seçin
3. İlçe dropdown'unun içeriğini kontrol edin
4. İl dropdown'dan "İZMİR" seçin
5. İlçe dropdown'unun değiştiğini kontrol edin

**Beklenen Sonuç:**
- İl seçildiğinde AutoPostBack tetiklenmeli
- İlçe dropdown dinamik olarak yüklenmeli
- il_SelectedIndexChanged event handler çalışmalıdır
- Her il için doğru ilçeler yüklenmelidir

**Dosya Referansı:** IsletmeRapor.aspx:141, IsletmeRapor.aspx.cs (backend)

---

### TC-DEN-036: PDF Rapor Export (Fonksiyonel)

**Önkoşul:**
- GridView'de kayıtlar listelenmiş olmalı

**Test Adımları:**
1. IsletmeRapor.aspx sayfasına gidin
2. Filtreleme yaparak kayıtları listeleyin
3. "PDF Rapor Al" butonuna tıklayın

**Beklenen Sonuç:**
- PDF dosyası oluşturulmalıdır
- Dosya indirme dialog'u açılmalıdır
- GridView içeriği PDF formatında export edilmelidir
- btnPdfAktar_Click event handler çalışmalıdır

**Dosya Referansı:** IsletmeRapor.aspx:190

---

### TC-DEN-037: Excel Export (Fonksiyonel)

**Önkoşul:**
- GridView'de kayıtlar listelenmiş olmalı

**Test Adımları:**
1. IsletmeRapor.aspx sayfasına gidin
2. Filtreleme yaparak kayıtları listeleyin
3. "Excel'e Aktar" butonuna tıklayın

**Beklenen Sonuç:**
- Excel dosyası indirilmelidir
- GridView tüm kolonları ile export edilmelidir
- EnableEventValidation="false" çalışmalıdır
- exceleaktar_Click event handler tetiklenmelidir

**Dosya Referansı:** IsletmeRapor.aspx:191

---

### TC-DEN-038: Tarih Aralığı Filtresi (Validation)

**Önkoşul:**
- Kullanıcı IsletmeRapor sayfasında olmalı

**Test Adımları:**
1. IsletmeRapor.aspx sayfasına gidin
2. Başlangıç Tarihi olarak "01.01.2024" seçin
3. Bitiş Tarihi olarak "01.01.2023" seçin (geçmiş tarih)
4. "Filtrele" butonuna tıklayın

**Beklenen Sonuç:**
- Bitiş tarihi başlangıç tarihinden önce olamaz uyarısı gösterilmelidir (backend validation)
- Veya boş sonuç dönmelidir
- Tarih validasyonu yapılmalıdır

**Dosya Referansı:** IsletmeRapor.aspx:174-185

---

### TC-DEN-039: "Hepsi" Seçeneği ile Filtreleme (Fonksiyonel)

**Önkoşul:**
- Veritabanında farklı türlerde kayıtlar bulunmalı

**Test Adımları:**
1. IsletmeRapor.aspx sayfasına gidin
2. Denetim Türü'nü "Hepsi" olarak bırakın
3. Ceza Durumu'nu "Hepsi" olarak bırakın
4. Diğer filtreleri boş bırakın
5. "Filtrele" butonuna tıklayın

**Beklenen Sonuç:**
- Tüm kayıtlar listelenmelidir
- "Hepsi" seçeneği WHERE koşuluna eklenmemelidir
- Filtrelenmiş sonuç yerine genel liste dönmelidir

**Dosya Referansı:** IsletmeRapor.aspx:130-133, 164-170

---

### TC-DEN-040: Empty Data Template (UI)

**Önkoşul:**
- Hiçbir filtreyle sonuç dönmeyecek şekilde arama yapın

**Test Adımları:**
1. IsletmeRapor.aspx sayfasına gidin
2. Var olmayan bir vergi numarası girin
3. "Filtrele" butonuna tıklayın

**Beklenen Sonuç:**
- GridView boş olmalıdır
- EmptyDataTemplate render edilmelidir
- Şu mesaj görünmelidir: "Arama Sonucu Bulunamadı"
- Icon ve açıklama metni gösterilmelidir

**Dosya Referansı:** IsletmeRapor.aspx:239-245

---

### TC-DEN-041: GridView Footer ve DataBound Event (Fonksiyonel)

**Önkoşul:**
- GridView'de kayıtlar bulunmalı

**Test Adımları:**
1. IsletmeRapor.aspx sayfasına gidin
2. Filtreleme yapın
3. GridView footer'ını kontrol edin

**Beklenen Sonuç:**
- ShowFooter="True" ayarı çalışmalıdır
- GridView1_DataBound event handler tetiklenmelidir
- Footer'da özet bilgiler görünmelidir (varsa)

**Dosya Referansı:** IsletmeRapor.aspx:214, 216

---

## UZAKTAN DENETİM RAPORLARI

### Sayfa Bilgileri
- **Dosya:** UzakRapor.aspx / UzakRapor.aspx.cs
- **Veritabanı Tablosu:** denetimuzak

---

### TC-DEN-042: Personel ve Durum Filtresi ile Arama (Fonksiyonel)

**Önkoşul:**
- denetimuzak tablosunda kayıtlar bulunmalı

**Test Adımları:**
1. UzakRapor.aspx sayfasına gidin
2. Personel dropdown'dan bir personel seçin
3. Durum olarak "Açık" seçin
4. Başlangıç Tarihi seçin
5. Bitiş Tarihi seçin
6. "Ara" butonuna tıklayın

**Beklenen Sonuç:**
- Seçili personel ve durum filtrelerine uygun kayıtlar listelenmelidir
- Tarih aralığı filtresi uygulanmalıdır
- Kayıt sayısı lblKayitSayisi'nde güncellenmelidir
- btnAra_Click event handler çalışmalıdır

**Dosya Referansı:** UzakRapor.aspx:26-64

---

### TC-DEN-043: Tümünü Listeleme (Fonksiyonel)

**Önkoşul:**
- denetimuzak tablosunda kayıtlar bulunmalı

**Test Adımları:**
1. UzakRapor.aspx sayfasına gidin
2. Herhangi bir filtre uygulamayın
3. "Tümü" butonuna tıklayın

**Beklenen Sonuç:**
- Tüm uzak denetim kayıtları listelenmelidir
- Filtreler uygulanmamalıdır
- btnTumunuListele_Click event handler çalışmalıdır
- CausesValidation="false" ayarı çalışmalıdır

**Dosya Referansı:** UzakRapor.aspx:65-69

---

### TC-DEN-044: Durum Badge CSS (UI)

**Önkoşul:**
- GridView'de hem Açık hem Kapalı kayıtlar bulunmalı

**Test Adımları:**
1. UzakRapor.aspx sayfasına gidin
2. "Tümü" butonuna tıklayın
3. GridView'deki Durum sütununu inceleyin

**Beklenen Sonuç:**
- Açık kayıtlar için "durum-badge durum-acik" class uygulanmalıdır
- Kapalı kayıtlar için "durum-badge durum-kapali" class uygulanmalıdır
- TemplateField doğru render edilmelidir
- CSS dosyasında bu class'lar tanımlı olmalıdır

**Dosya Referansı:** UzakRapor.aspx:119-125

---

### TC-DEN-045: Excel Export (Fonksiyonel)

**Önkoşul:**
- GridView'de kayıtlar listelenmiş olmalı

**Test Adımları:**
1. UzakRapor.aspx sayfasına gidin
2. Kayıtları listeleyin
3. "Excel" butonuna tıklayın

**Beklenen Sonuç:**
- Excel dosyası indirilmelidir
- GridView içeriği export edilmelidir
- btnExcelAktar_Click event handler çalışmalıdır

**Dosya Referansı:** UzakRapor.aspx:91-94

---

## DENETİM İSTATİSTİKLERİ

### Sayfa Bilgileri
- **Dosya:** Istatistik.aspx / Istatistik.aspx.cs
- **Chart Kontrolleri:** ASP.NET Chart Controls kullanılıyor

---

### TC-DEN-046: Yıl Seçimi ve İstatistik Yükleme (Fonksiyonel)

**Önkoşul:**
- Farklı yıllarda denetim kayıtları bulunmalı

**Test Adımları:**
1. Istatistik.aspx sayfasına gidin
2. Raporlama Yılı dropdown'dan "2024" seçin
3. İstatistik widget'larını kontrol edin
4. Yıl dropdown'dan "2023" seçin
5. İstatistiklerin güncellendiğini kontrol edin

**Beklenen Sonuç:**
- AutoPostBack ile sayfa yenilenmelidir
- Seçilen yıla göre istatistikler hesaplanmalıdır
- Widget'lardaki sayılar güncellenmelidir
- Chart'lar yeniden render edilmelidir
- ddlYil_SelectedIndexChanged event tetiklenmelidir

**Dosya Referansı:** Istatistik.aspx:56-58

---

### TC-DEN-047: Denetim Türüne Göre İstatistikler (Fonksiyonel)

**Önkoşul:**
- Farklı türlerde denetim kayıtları bulunmalı

**Test Adımları:**
1. Istatistik.aspx sayfasına gidin
2. Yolcu Taşımacılığı widget'ını kontrol edin (İşletme ve Taşıt sayıları)
3. Eşya Taşımacılığı widget'ını kontrol edin
4. Tehlikeli Madde widget'ını kontrol edin
5. Uzak Denetim widget'ını kontrol edin

**Beklenen Sonuç:**
- Her denetim türü için doğru sayılar hesaplanmalıdır
- İşletme sayıları denetimisletme tablosundan gelmelidir
- Taşıt sayıları denetimtasit tablosundan gelmelidir
- Uzak Denetim sayıları denetimuzak tablosundan gelmelidir
- Widget'larda icon ve renkler doğru olmalıdır

**Dosya Referansı:** Istatistik.aspx:67-161

---

### TC-DEN-048: Aylık İşletme Denetim GridView (Fonksiyonel)

**Önkoşul:**
- Seçili yılda aylık işletme denetim kayıtları bulunmalı

**Test Adımları:**
1. Istatistik.aspx sayfasına gidin
2. "Aylık İşletme Denetimi" kartını bulun
3. GridView'i inceleyin

**Beklenen Sonuç:**
- Ay bazında denetim sayıları görünmelidir
- AutoGenerateColumns="true" çalışmalıdır
- GridView dinamik olarak oluşturulmalıdır
- 12 aylık veri gösterilmelidir

**Dosya Referansı:** Istatistik.aspx:188-206

---

### TC-DEN-049: Aylık Taşıt Denetim GridView (Fonksiyonel)

**Önkoşul:**
- Seçili yılda aylık taşıt denetim kayıtları bulunmalı

**Test Adımları:**
1. Istatistik.aspx sayfasına gidin
2. "Aylık Taşıt Denetimi" kartını bulun
3. GridView'i inceleyin

**Beklenen Sonuç:**
- Ay bazında taşıt denetim sayıları görünmelidir
- AutoGenerateColumns="true" çalışmalıdır
- GridView dinamik olarak oluşturulmalıdır

**Dosya Referansı:** Istatistik.aspx:208-227

---

### TC-DEN-050: İşletme Denetimi Excel Export (Fonksiyonel)

**Önkoşul:**
- İşletme GridView'de veri bulunmalı

**Test Adımları:**
1. Istatistik.aspx sayfasına gidin
2. "Aylık İşletme Denetimi" kartındaki "Excel'e Aktar" butonuna tıklayın

**Beklenen Sonuç:**
- Excel dosyası indirilmelidir
- GridView içeriği export edilmelidir
- btnExcelIsletme_Click event handler çalışmalıdır

**Dosya Referansı:** Istatistik.aspx:194-195

---

### TC-DEN-051: Taşıt Denetimi Excel Export (Fonksiyonel)

**Önkoşul:**
- Taşıt GridView'de veri bulunmalı

**Test Adımları:**
1. Istatistik.aspx sayfasına gidin
2. "Aylık Taşıt Denetimi" kartındaki "Excel'e Aktar" butonuna tıklayın

**Beklenen Sonuç:**
- Excel dosyası indirilmelidir
- GridView içeriği export edilmelidir
- btnExcelTasit_Click event handler çalışmalıdır

**Dosya Referansı:** Istatistik.aspx:215-216

---

### TC-DEN-052: İşletme Denetimi İllere Göre Pie Chart (Fonksiyonel)

**Önkoşul:**
- Farklı illerde işletme denetim kayıtları bulunmalı

**Test Adımları:**
1. Istatistik.aspx sayfasına gidin
2. "İşletme Denetimi İllere Göre Dağılımı" chart'ını inceleyin

**Beklenen Sonuç:**
- Pie chart render edilmelidir
- Her il için dilim gösterilmelidir
- Yüzde değerleri gösterilmelidir
- Label="#VALX - #VAL (#PERCENT{P})" formatı çalışmalıdır
- 3D görünüm aktif olmalıdır
- SmartLabelStyle enabled olmalıdır

**Dosya Referansı:** Istatistik.aspx:238-251

---

### TC-DEN-053: Araç Denetimi İllere Göre Pie Chart (Fonksiyonel)

**Önkoşul:**
- Farklı illerde araç denetim kayıtları bulunmalı

**Test Adımları:**
1. Istatistik.aspx sayfasına gidin
2. "Araç Denetimi İllere Göre Dağılımı" chart'ını inceleyin

**Beklenen Sonuç:**
- Pie chart render edilmelidir
- Her il için dilim gösterilmelidir
- Yüzde değerleri gösterilmelidir
- 3D görünüm aktif olmalıdır

**Dosya Referansı:** Istatistik.aspx:264-277

---

### TC-DEN-054: İşletme Denetim Türü Column Chart (Fonksiyonel)

**Önkoşul:**
- Farklı türlerde işletme denetim kayıtları bulunmalı

**Test Adımları:**
1. Istatistik.aspx sayfasına gidin
2. "İşletme Denetim Türü Dağılımı" chart'ını inceleyin

**Beklenen Sonuç:**
- Column chart (sütun grafik) render edilmelidir
- ChartType="Column" olmalıdır
- 3D cylinder görünüm aktif olmalıdır (DrawingStyle=Cylinder)
- Her denetim türü için sütun gösterilmelidir
- AxisX disabled olmalıdır
- Palette="SeaGreen" uygulanmalıdır

**Dosya Referansı:** Istatistik.aspx:292-306

---

### TC-DEN-055: Taşıt Denetim Türü Column Chart (Fonksiyonel)

**Önkoşul:**
- Farklı türlerde taşıt denetim kayıtları bulunmalı

**Test Adımları:**
1. Istatistik.aspx sayfasına gidin
2. "Taşıt Denetim Türü Dağılımı" chart'ını inceleyin

**Beklenen Sonuç:**
- Column chart render edilmelidir
- ChartType="Column" olmalıdır
- 3D cylinder görünüm aktif olmalıdır
- Her denetim türü için sütun gösterilmelidir

**Dosya Referansı:** Istatistik.aspx:319-333

---

### TC-DEN-056: Personele Göre İşletme Denetim Dağılımı (Fonksiyonel)

**Önkoşul:**
- Farklı personellerin yaptığı işletme denetimleri bulunmalı

**Test Adımları:**
1. Istatistik.aspx sayfasına gidin
2. "İşletme Denetimlerinin Personele Göre Dağılımı" chart'ını inceleyin

**Beklenen Sonuç:**
- Column chart render edilmelidir
- Her personel için sütun gösterilmelidir
- Personel adları ve denetim sayıları gösterilmelidir
- 3D görünüm aktif olmalıdır

**Dosya Referansı:** Istatistik.aspx:348-362

---

### TC-DEN-057: Personele Göre Taşıt Denetim Dağılımı (Fonksiyonel)

**Önkoşul:**
- Farklı personellerin yaptığı taşıt denetimleri bulunmalı

**Test Adımları:**
1. Istatistik.aspx sayfasına gidin
2. "Taşıt Denetimlerinin Personele Göre Dağılımı" chart'ını inceleyin

**Beklenen Sonuç:**
- Column chart render edilmelidir
- Her personel için sütun gösterilmelidir
- Personel adları ve denetim sayıları gösterilmelidir

**Dosya Referansı:** Istatistik.aspx:375-389

---

### TC-DEN-058: Toplam İstatistik Özet Kartları (Fonksiyonel)

**Önkoşul:**
- Denetim kayıtları bulunmalı

**Test Adımları:**
1. Istatistik.aspx sayfasına gidin
2. "Toplam İşletme Denetimi" kartını kontrol edin
3. "Toplam Araç Denetimi" kartını kontrol edin

**Beklenen Sonuç:**
- Toplam işletme denetim sayısı doğru hesaplanmalıdır
- Toplam araç denetim sayısı doğru hesaplanmalıdır
- lblToplamIsletme ve lblToplamArac label'ları güncellenmelidir
- Summary-box kartları görünmelidir

**Dosya Referansı:** Istatistik.aspx:164-185

---

## TAŞIT DENETİM RAPORLAMA (BOŞ SAYFA)

### TC-DEN-059: Boş Sayfa Kontrolü (Negative)

**Önkoşul:**
- Kullanıcı sisteme giriş yapmış olmalı

**Test Adımları:**
1. TasitRapor.aspx URL'sine manuel olarak gidin

**Beklenen Sonuç:**
- Sayfa render edilmelidir ancak içerik boş olmalıdır
- MainContent ContentPlaceHolder boş olmalıdır
- Hata fırlatılmamalıdır
- Master page yapısı korunmalıdır

**Dosya Referansı:** TasitRapor.aspx:1-8

---

## TEST ÖZETİ

### Test Kapsam İstatistikleri

| Test Kategorisi | Test Sayısı |
|----------------|------------|
| Fonksiyonel Testler | 37 |
| Validation Testleri | 10 |
| RBAC Testleri | 3 |
| Negative Testler | 8 |
| UI Testleri | 1 |
| **TOPLAM** | **59** |

### Sayfa Bazında Test Dağılımı

| Sayfa | Test Sayısı |
|-------|------------|
| IsletmeGiris.aspx | 10 |
| TasitGiris.aspx | 8 |
| UzakEkle.aspx | 6 |
| UzakGorev.aspx | 9 |
| IsletmeRapor.aspx | 8 |
| UzakRapor.aspx | 4 |
| Istatistik.aspx | 13 |
| TasitRapor.aspx | 1 |
| **TOPLAM** | **59** |

---

## ÖNEMLİ NOTLAR

### Yetki Kontrolleri
- Her sayfa için ilgili yetki sabitleri kullanılmalıdır
- RBAC testleri farklı yetki profillerine sahip kullanıcılarla yapılmalıdır
- Silme işlemleri için ayrı yetki kontrolü bulunmaktadır

### Validation Grupları
- Form validasyonları için "kayit" ValidationGroup kullanılmaktadır
- CausesValidation="false" butonlar validation'ı tetiklemez

### Veritabanı Tabloları
- **denetimisletme:** İşletme denetim kayıtları
- **denetimtasit:** Taşıt denetim kayıtları
- **denetimuzak:** Uzaktan denetim kayıtları
- **yetki:** Kullanıcı yetki bilgileri
- **personel:** Personel bilgileri
- **yetki_belgeleri:** Yetki belgesi tipleri
- **denetimyerleri:** Denetim yerleri

### Loglama
- Tüm CRUD işlemleri için loglama yapılmalıdır
- LogInfo() ve LogError() metodları kullanılmalıdır

### Toast Mesajları
- Başarılı işlemler için "success"
- Uyarılar için "warning"
- Hatalar için "danger"
- Bilgilendirme için "info"

---

**Test Dökümanı Sonu**

*Bu döküman ModulDenetim klasörü altındaki tüm sayfalar için hazırlanmıştır. Testler manuel veya otomasyon ile gerçekleştirilebilir.*
