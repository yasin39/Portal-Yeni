# ARAÇLAR MODÜLÜ MANUEL TEST DOKÜMANI

**Proje:** Portal - ASP.NET Web Forms  
**Modül:** Araçlar (ModulAraclar)  
**Test Tarihi:** ____________________  
**Test Eden:** ____________________

---

## TEST KAPSAMI

Bu dokümanda aşağıdaki sayfalar test edilecektir:
- Personel İzin Formu (PersonelIzinFormu.aspx)
- Dosya Birleştir (DosyaBirlestir.aspx)
- PDF Birleştirici (PdfBirlestir.aspx)
- Resim Küçültme (ResimKucult.aspx)
- Telefon Rehberi (TelefonRehberi.aspx)

**Not:** BasePage.cs, Sabitler.cs, Helpers.cs ve AnaV2.Master.cs test kapsamı dışındadır.

---

## 1. PERSONEL İZİN FORMU (PersonelIzinFormu.aspx)

### 1.1. Sayfa Erişim Testi

**Adımlar:**
1. Tarayıcıda sayfa URL'sini aç: `/ModulAraclar/PersonelIzinFormu.aspx`
2. Sayfanın yüklendiğini doğrula

**Beklenen Sonuçlar:**
- ✅ Sayfa başarıyla yüklenir
- ✅ Breadcrumb: Ana Sayfa > Çeşitli Araçlar > Personel İzin Formu
- ✅ Başlık: "Personel İzin Formu (Saatlik / Hastane)"
- ✅ Yetki kontrolü (1001) geçerse sayfa açılır
- ✅ Personel bilgileri ve geçmiş izinler panelleri gizli durumda

---

### 1.2. Sicil No ile Personel Bulma - Başarılı Senaryo

**Ön Koşullar:**
- Veritabanında geçerli bir personel kaydı mevcut olmalı

**Adımlar:**
1. "Sicil No ile Personel Bul" alanına geçerli bir sicil no gir (örn: 12345)
2. "Bul" butonuna tıkla VEYA Enter'a bas (AutoPostBack aktif)

**Beklenen Sonuçlar:**
- ✅ Personel bilgileri paneli (pnlPersonelDetay) görünür hale gelir
- ✅ Aşağıdaki bilgiler doldurulur:
  - Ad-Soyad
  - TC Kimlik No
  - Ünvan
  - Çalıştığı Birim
  - Statü
- ✅ Tüm bilgi alanları readonly olarak görüntülenir
- ✅ Eğer personelin geçmiş izinleri varsa "Geçmiş İzinler" paneli görünür
- ✅ GridView'de izin kayıtları listelenir (İzin Türü, Açıklama, Tarihler, Saatler, Kayıt Bilgileri)

---

### 1.3. Sicil No ile Personel Bulma - Hatalı Senaryo

**Adımlar:**
1. "Sicil No ile Personel Bul" alanına mevcut olmayan bir sicil no gir (örn: 99999)
2. "Bul" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ "Personel bulunamadı." uyarısı gösterilir (warning toast)
- ✅ Personel bilgileri paneli gizli kalır
- ✅ Geçmiş izinler paneli gizli kalır

---

### 1.4. Sicil No ile Personel Bulma - Boş Alan

**Adımlar:**
1. Sicil No alanını boş bırak
2. "Kaydet ve PDF Oluştur" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ "Sicil No Boş Olamaz." validation hatası gösterilir (kırmızı)
- ✅ Form submit edilmez

---

### 1.5. İzin Kaydı Oluşturma - Başarılı Senaryo

**Ön Koşullar:**
- Geçerli bir personel bulunmuş olmalı

**Adımlar:**
1. "İzin Türü" dropdown'ından bir seçenek seç (Saatlik izin veya Hastane İzni)
2. "İzne Başlama Tarihi ve Saati" alanına tıkla, flatpickr takviminden tarih-saat seç (örn: 01.12.2024 09:00)
3. "İzin Bitiş Tarihi ve Saati" alanına tıkla, takvimden tarih-saat seç (örn: 01.12.2024 17:00)
4. "Açıklama / İzin Sebebi" alanına açıklama yaz (örn: "Doktor randevusu")
5. "Kaydet ve PDF Oluştur" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ İzin kaydı veritabanına başarıyla eklenir
- ✅ PDF otomatik olarak indirilir (PersonelIzin_{SicilNo}_{IzinId}.pdf)
- ✅ PDF içeriği kontrol edilir:
  - Başlık: "T.C. ULAŞTIRMA VE ALTYAPI BAKANLIĞI II. Bölge Müdürlüğü Saatlik İzin Formu"
  - Personel bilgileri (Ad-Soyad, Sicil, Statü, Unvan, Birim)
  - İzin bilgileri (İzin Türü, Sebep, Başlama-Bitiş Tarihi/Saati)
  - İmza bölümleri (Talep Eden, Birim Amiri)
  - Uygunluk bölümü
  - Oluşturma tarihi
- ✅ Başarı mesajı gösterilir (success toast)
- ✅ Geçmiş izinler listesi güncellenir, yeni kayıt listenin başında görünür

---

### 1.6. İzin Kaydı Oluşturma - Zorunlu Alan Kontrolü

**Adımlar:**
1. Personel bul
2. Zorunlu alanlardan birini veya birkaçını boş bırak:
   - İzin Türü
   - Başlama Tarihi
   - Bitiş Tarihi
   - Açıklama
3. "Kaydet ve PDF Oluştur" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ Her boş alan için ilgili validation hatası gösterilir:
  - "İzin Türü Seçiniz."
  - "Başlama Tarihi Seçiniz."
  - "Bitiş Tarihi Seçiniz."
  - "Açıklama zorunludur."
- ✅ Form submit edilmez

---

### 1.7. İzin Kaydı Oluşturma - Geçersiz Tarih Formatı

**Adımlar:**
1. Personel bul
2. Tarih alanlarına manuel olarak geçersiz format gir (örn: "abc" veya "32.13.2024")
3. "Kaydet ve PDF Oluştur" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ "Tarih formatı geçersiz. (Örn: 01.01.2025 10:30)" uyarısı gösterilir
- ✅ Kayıt oluşturulmaz

---

### 1.8. İzin Kaydı Oluşturma - Bitiş Tarihi Kontrolü

**Adımlar:**
1. Personel bul
2. İzin Türü seç
3. Başlama Tarihi: 02.12.2024 14:00
4. Bitiş Tarihi: 01.12.2024 10:00 (başlama tarihinden önce)
5. Açıklama yaz
6. "Kaydet ve PDF Oluştur" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ "Bitiş tarihi, başlama tarihinden sonra olmalıdır." uyarısı gösterilir
- ✅ Kayıt oluşturulmaz

---

### 1.9. İzin Kaydı Oluşturma - İzin Çakışması Kontrolü

**Ön Koşullar:**
- Personelin mevcut bir izin kaydı olmalı (örn: 01.12.2024 09:00 - 01.12.2024 17:00)

**Adımlar:**
1. Aynı personeli bul
2. Çakışan tarihler gir:
   - Başlama: 01.12.2024 10:00
   - Bitiş: 01.12.2024 12:00
3. Diğer alanları doldur ve "Kaydet ve PDF Oluştur" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ "Seçilen tarihlerde personelin zaten tanımlı bir izni bulunmaktadır." hatası gösterilir (danger toast)
- ✅ Kayıt oluşturulmaz

---

### 1.10. Flatpickr Tarih Seçici Testi

**Adımlar:**
1. "İzne Başlama Tarihi ve Saati" input alanına tıkla
2. Flatpickr popup'ının açıldığını doğrula
3. Takvimden bir tarih seç
4. Saat seçici ile saat seç
5. Seçilen değerin input'a yazıldığını doğrula

**Beklenen Sonuçlar:**
- ✅ Flatpickr popup açılır
- ✅ Türkçe dil dosyası yüklüdür (Ay ve gün isimleri Türkçe)
- ✅ Tarih formatı: d.m.Y H:i (örn: 01.12.2024 14:30)
- ✅ 24 saat formatı kullanılır
- ✅ Manuel giriş de yapılabilir (allowInput: true)
- ✅ Z-index sorunu yok (1056), diğer elementlerin üstünde görünür

---

### 1.11. Formu Temizle Butonu

**Adımlar:**
1. Form alanlarını doldur
2. "Formu Temizle" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ Sayfa yeniden yüklenir (Response.Redirect)
- ✅ Tüm alanlar temizlenir
- ✅ Paneller gizli duruma döner

---

### 1.12. Geçmiş İzinler GridView Testi

**Ön Koşullar:**
- Personelin en az 2-3 izin kaydı olmalı

**Adımlar:**
1. Geçerli bir personel bul
2. "Geçmiş İzinler" panelini kontrol et

**Beklenen Sonuçlar:**
- ✅ GridView görüntülenir
- ✅ Kolonlar doğru sırada: İzin Türü, Açıklama, Baş. Tarihi, Baş. Saati, Bitiş Tarihi, Bitiş Saati, Kayıt Eden, Kayıt Tarihi
- ✅ Tarih formatı: dd.MM.yyyy
- ✅ Saat formatı: hh:mm
- ✅ Kayıtlar en yeni en üstte olacak şekilde sıralanmış (ORDER BY id DESC)
- ✅ Bootstrap tablo stilleri uygulanmış (table-striped, table-hover, table-bordered)

---

## 2. DOSYA BİRLEŞTİR (DosyaBirlestir.aspx)

### 2.1. Sayfa Erişim Testi

**Adımlar:**
1. Tarayıcıda sayfa URL'sini aç: `/ModulAraclar/DosyaBirlestir.aspx`
2. Sayfanın yüklendiğini doğrula

**Beklenen Sonuçlar:**
- ✅ Sayfa başarıyla yüklenir
- ✅ Breadcrumb: Ana Sayfa > Çeşitli Araçlar > Dosya Birleştir
- ✅ Başlık: "Arşiv Dosya Birleştir"
- ✅ UNET Numarası input alanı görünür
- ✅ Belge Türü dropdown görünür ve dolu
- ✅ "Ara" butonu aktif
- ✅ "Birleştir" butonu pasif (Enabled=false)
- ✅ Bilgi mesajı gösterilir
- ✅ GridView boş veri mesajı gösterir: "Kayıt bulunamadı."

---

### 2.2. Belge Türü Dropdown Testi

**Adımlar:**
1. Sayfa yüklendiğinde "Belge Türü" dropdown'ını kontrol et
2. Dropdown'ı aç ve seçenekleri incele

**Beklenen Sonuçlar:**
- ✅ Dropdown dolu gelir (Page_Load'da doldurulur)
- ✅ Belge türleri veritabanından çekilir
- ✅ Seçenekler düzgün listelenir

---

### 2.3. UNET Numarası ile Arama - Başarılı

**Ön Koşullar:**
- Veritabanında geçerli UNET numarası ile kayıtlar mevcut

**Adımlar:**
1. "UNET Numarası" alanına geçerli bir numara gir (örn: 12345)
2. "Ara" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ GridView'de ilgili kayıtlar listelenir
- ✅ Kolonlar: ID, UNET, Ünvan, Belge Türü, Sayfa Sayısı
- ✅ Footer'da toplam sayfa sayısı gösterilir
- ✅ "Birleştir" butonu aktif hale gelir (Enabled=true)

---

### 2.4. Belge Türü ile Arama - Başarılı

**Adımlar:**
1. "Belge Türü" dropdown'ından bir seçenek seç
2. "Ara" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ Seçilen belge türüne ait kayıtlar listelenir
- ✅ GridView doldurulur
- ✅ "Birleştir" butonu aktif olur

---

### 2.5. UNET ve Belge Türü ile Kombine Arama

**Adımlar:**
1. "UNET Numarası" alanına numara gir
2. "Belge Türü" dropdown'ından seçim yap
3. "Ara" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ Her iki kritere uyan kayıtlar listelenir
- ✅ Filtre doğru çalışır

---

### 2.6. Arama - Sonuç Bulunamadı

**Adımlar:**
1. Mevcut olmayan UNET numarası gir (örn: 99999999)
2. "Ara" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ GridView boş görünür
- ✅ "Kayıt bulunamadı." mesajı gösterilir
- ✅ "Birleştir" butonu pasif kalır

---

### 2.7. Dosya Birleştirme - Başarılı

**Ön Koşullar:**
- En az 2 dosya listelenmiş olmalı

**Adımlar:**
1. UNET veya belge türü ile ara
2. Kayıtların listelendiğini doğrula
3. "Birleştir" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ Listelenen tüm PDF dosyaları birleştirilir
- ✅ Birleştirilmiş PDF indirilir
- ✅ PDF içeriği:
  - Tüm sayfalar sırayla birleştirilmiş
  - Sayfa numaraları düzgün
  - İçerik okunabilir
- ✅ Başarı mesajı gösterilir

---

### 2.8. Dosya Birleştirme - Kayıt Yokken

**Adımlar:**
1. Herhangi bir arama yapmadan direkt "Birleştir" butonuna tıklamaya çalış

**Beklenen Sonuçlar:**
- ✅ Buton zaten pasif olduğu için tıklanmaz
- ✅ Alternatif: Eğer aktif olsaydı, "Birleştirilecek dosya bulunamadı" gibi hata mesajı gösterilmeli

---

### 2.9. GridView Footer Toplam Testi

**Ön Koşullar:**
- Birden fazla dosya listelenmiş olmalı

**Adımlar:**
1. Arama yap ve sonuçları listele
2. GridView footer'ına bak

**Beklenen Sonuçlar:**
- ✅ Footer'da "Toplam Sayfa Sayısı: X" gibi özet bilgi gösterilir
- ✅ Footer bold ve koyu renkte (table-dark)

---

## 3. PDF BİRLEŞTİRİCİ (PdfBirlestir.aspx)

### 3.1. Sayfa Erişim Testi

**Adımlar:**
1. Tarayıcıda sayfa URL'sini aç: `/ModulAraclar/PdfBirlestir.aspx`
2. Sayfanın yüklendiğini doğrula

**Beklenen Sonuçlar:**
- ✅ Sayfa başarıyla yüklenir
- ✅ Breadcrumb: Ana Sayfa > Çeşitli Araçlar > PDF Birleştirici
- ✅ Başlık: "PDF Birleştirme Aracı"
- ✅ "Nasıl Kullanılır?" bilgi kutusu görünür
- ✅ Bilgi: "En az 2, en fazla 3 dosya seçmelisiniz"
- ✅ FileUpload kontrolü görünür
- ✅ "Seçili PDF'leri Birleştir" butonu görünür

---

### 3.2. Dosya Seçimi - Çoklu Seçim

**Adımlar:**
1. "Birleştirilecek PDF Dosyaları" alanına tıkla
2. Ctrl tuşuna basılı tutarak 2-3 PDF dosyası seç
3. Dosya seçimini onayla

**Beklenen Sonuçlar:**
- ✅ Birden fazla dosya seçilebilir (AllowMultiple="true")
- ✅ Seçilen dosya sayısı input'ta veya altında gösterilir
- ✅ Accept="application/pdf" olduğu için sadece PDF dosyaları filtrelenir

---

### 3.3. PDF Birleştirme - Başarılı (2 Dosya)

**Ön Koşullar:**
- 2 adet PDF dosyası hazır olmalı

**Adımlar:**
1. 2 PDF dosyası seç
2. "Seçili PDF'leri Birleştir" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ Dosyalar başarıyla birleştirilir
- ✅ Birleştirilmiş PDF indirilir (BirlesmisDoc.pdf)
- ✅ PDF içeriği:
  - İlk dosyanın tüm sayfaları
  - İkinci dosyanın tüm sayfaları
  - Seçim sırasına göre birleştirilmiş
- ✅ Başarı mesajı gösterilir

---

### 3.4. PDF Birleştirme - Başarılı (3 Dosya)

**Adımlar:**
1. 3 PDF dosyası seç
2. "Seçili PDF'leri Birleştir" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ 3 dosya başarıyla birleştirilir
- ✅ Birleştirilmiş PDF indirilir
- ✅ Tüm sayfalar sırayla mevcut

---

### 3.5. PDF Birleştirme - Hatalı Dosya Sayısı (1 Dosya)

**Adımlar:**
1. Sadece 1 PDF dosyası seç
2. "Seçili PDF'leri Birleştir" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ Hata mesajı gösterilir: "Lütfen en az 2 dosya seçiniz."
- ✅ İşlem gerçekleştirilmez

---

### 3.6. PDF Birleştirme - Hatalı Dosya Sayısı (4+ Dosya)

**Adımlar:**
1. 4 veya daha fazla PDF dosyası seç
2. "Seçili PDF'leri Birleştir" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ Hata mesajı gösterilir: "En fazla 3 dosya seçebilirsiniz."
- ✅ İşlem gerçekleştirilmez

---

### 3.7. PDF Birleştirme - Dosya Seçilmeden

**Adımlar:**
1. Hiç dosya seçmeden "Seçili PDF'leri Birleştir" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ Hata mesajı: "Lütfen dosya seçiniz."
- ✅ İşlem gerçekleştirilmez

---

### 3.8. PDF Birleştirme - Geçersiz Dosya Formatı

**Adımlar:**
1. PDF olmayan dosyalar seç (örn: .docx, .jpg)
2. "Seçili PDF'leri Birleştir" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ Accept="application/pdf" sayesinde seçim sırasında filtrelenir
- ✅ Eğer seçilebilirse: "Sadece PDF dosyaları seçiniz." hatası gösterilir

---

### 3.9. PDF Birleştirme - Büyük Dosyalar

**Adımlar:**
1. Büyük boyutlu (>10MB) PDF dosyaları seç
2. "Seçili PDF'leri Birleştir" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ İşlem başarıyla tamamlanır veya timeout/boyut hatası gösterilir
- ✅ Hata durumunda kullanıcıya bilgi verilir

---

## 4. RESİM KÜÇÜLTME (ResimKucult.aspx)

### 4.1. Sayfa Erişim Testi

**Adımlar:**
1. Tarayıcıda sayfa URL'sini aç: `/ModulAraclar/ResimKucult.aspx`
2. Sayfanın yüklendiğini doğrula

**Beklenen Sonuçlar:**
- ✅ Sayfa başarıyla yüklenir
- ✅ Breadcrumb: Ana Sayfa > Çeşitli Araçlar > Resim Boyutu Küçültme
- ✅ Başlık: "Resim Boyutu Küçültme Aracı"
- ✅ "Nasıl Kullanılır?" bilgi kutusu görünür
- ✅ FileUpload kontrolü görünür (accept="image/*")
- ✅ "Küçültme Oranı" dropdown görünür ve dolu
- ✅ "Resmi Küçült" butonu görünür

---

### 4.2. Küçültme Oranı Dropdown Testi

**Adımlar:**
1. Sayfa yüklendiğinde "Küçültme Oranı" dropdown'ını kontrol et
2. Dropdown'ı aç ve seçenekleri incele

**Beklenen Sonuçlar:**
- ✅ Dropdown dolu gelir (Page_Load'da doldurulur)
- ✅ Örnek seçenekler:
  - 90%
  - 80%
  - 70%
  - 60%
  - 50%
  - 40%
  - 30%
  - 20%
- ✅ Tüm seçenekler düzgün görünür

---

### 4.3. Resim Küçültme - Başarılı (JPG)

**Ön Koşullar:**
- Bir JPG resim dosyası hazır olmalı

**Adımlar:**
1. JPG resim dosyası seç
2. Küçültme oranı seç (örn: 50%)
3. "Resmi Küçült" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ Resim başarıyla küçültülür
- ✅ Küçültülmüş resim indirilir
- ✅ Dosya adı: orijinal_oran.jpg (örn: resim_50.jpg)
- ✅ Dosya boyutu orijinalden daha küçük
- ✅ Görsel kalite kabul edilebilir seviyede
- ✅ Başarı mesajı gösterilir

---

### 4.4. Resim Küçültme - Başarılı (PNG)

**Adımlar:**
1. PNG resim dosyası seç
2. Küçültme oranı seç (örn: 70%)
3. "Resmi Küçült" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ PNG resim başarıyla küçültülür
- ✅ Küçültülmüş PNG indirilir
- ✅ Şeffaflık korunur (eğer varsa)
- ✅ Dosya boyutu küçülür

---

### 4.5. Resim Küçültme - Diğer Formatlar (GIF, BMP)

**Adımlar:**
1. GIF veya BMP dosyası seç
2. Küçültme oranı seç
3. "Resmi Küçült" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ İşlem başarıyla tamamlanır
- ✅ İlgili formatta küçültülmüş dosya indirilir

---

### 4.6. Resim Küçültme - Dosya Seçilmeden

**Adımlar:**
1. Hiç dosya seçmeden "Resmi Küçült" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ Hata mesajı: "Lütfen bir resim dosyası seçiniz."
- ✅ İşlem gerçekleştirilmez

---

### 4.7. Resim Küçültme - Oran Seçilmeden

**Adımlar:**
1. Resim dosyası seç
2. Küçültme oranı seçmeden (veya boş) "Resmi Küçült" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ Hata mesajı: "Lütfen küçültme oranı seçiniz."
- ✅ İşlem gerçekleştirilmez

---

### 4.8. Resim Küçültme - Geçersiz Dosya Formatı

**Adımlar:**
1. Resim olmayan bir dosya seçmeye çalış (örn: .pdf, .docx)
2. "Resmi Küçült" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ Accept="image/*" sayesinde seçim sırasında filtrelenir
- ✅ Eğer seçilebilirse: "Geçersiz dosya formatı. Sadece resim dosyaları (.jpg, .png, .gif, .bmp) desteklenir." hatası gösterilir

---

### 4.9. Resim Küçültme - Farklı Oranlarla Test

**Adımlar:**
1. Aynı resmi %90, %50 ve %20 oranlarıyla ayrı ayrı küçült
2. Dosya boyutlarını karşılaştır

**Beklenen Sonuçlar:**
- ✅ %90 oranında minimal boyut azalması
- ✅ %50 oranında orta seviye boyut azalması
- ✅ %20 oranında maksimum boyut azalması
- ✅ Tüm dosyalar görüntülenebilir kalitede

---

### 4.10. Resim Küçültme - Büyük Dosya

**Adımlar:**
1. Büyük boyutlu bir resim seç (örn: >10MB)
2. Küçültme oranı seç
3. "Resmi Küçült" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ İşlem başarıyla tamamlanır veya timeout hatası gösterilir
- ✅ Hata durumunda bilgilendirme yapılır

---

## 5. TELEFON REHBERİ (TelefonRehberi.aspx)

### 5.1. Sayfa Erişim Testi

**Adımlar:**
1. Tarayıcıda sayfa URL'sini aç: `/ModulAraclar/TelefonRehberi.aspx`
2. Sayfanın yüklendiğini doğrula

**Beklenen Sonuçlar:**
- ✅ Sayfa başarıyla yüklenir
- ✅ Breadcrumb: Ana Sayfa > Çeşitli Araçlar > Telefon Rehberi
- ✅ Başlık: "Telefon Rehberi Sorgulama"
- ✅ "Adı", "Soyadı" ve "Birimi" alanları görünür
- ✅ "Ara" butonu görünür
- ✅ Birim dropdown dolu gelir (Page_Load'da doldurulur)
- ✅ Alt bilgi kutusu görünür: "Dış Nizamiye Güvenlik : 9200 | Çay Ocağı : 9214 ..."

---

### 5.2. Birim Dropdown Testi

**Adımlar:**
1. Sayfa yüklendiğinde "Birimi" dropdown'ını kontrol et
2. Dropdown'ı aç ve seçenekleri incele

**Beklenen Sonuçlar:**
- ✅ Dropdown dolu gelir (veritabanından)
- ✅ Tüm birimler listelenmiş
- ✅ İlk seçenek boş veya "Tümü" gibi genel seçenek

---

### 5.3. Ada Göre Arama - Başarılı

**Ön Koşullar:**
- Veritabanında kayıtlar mevcut

**Adımlar:**
1. "Adı" alanına bir isim gir (örn: "Ahmet")
2. "Ara" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ Adı "Ahmet" ile başlayan veya içeren kayıtlar listelenir
- ✅ Sonuçlar tablo (lblTable) içinde HTML tablo olarak gösterilir
- ✅ Kolonlar: Sicil No, Ad-Soyad, Unvan, Birim, Telefon (dahili)
- ✅ Tablo responsive ve Bootstrap 5 stilleri uygulanmış

---

### 5.4. Soyada Göre Arama - Başarılı

**Adımlar:**
1. "Soyadı" alanına bir soyisim gir (örn: "Yılmaz")
2. "Ara" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ Soyadı "Yılmaz" ile başlayan veya içeren kayıtlar listelenir
- ✅ Sonuçlar düzgün gösterilir

---

### 5.5. Birime Göre Arama - Başarılı

**Adımlar:**
1. "Birimi" dropdown'ından bir birim seç (örn: "İdari İşler")
2. "Ara" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ Seçilen birime ait tüm personeller listelenir
- ✅ Sonuçlar doğru

---

### 5.6. Kombine Arama (Ad + Soyad + Birim)

**Adımlar:**
1. "Adı" alanına isim gir
2. "Soyadı" alanına soyisim gir
3. "Birimi" dropdown'ından birim seç
4. "Ara" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ Tüm kriterlere uyan kayıtlar listelenir
- ✅ Filtre kombinasyonu doğru çalışır

---

### 5.7. Kısmi Arama Testi

**Adımlar:**
1. "Adı" alanına kısmi bir kelime gir (örn: "Ah" veya "met")
2. "Ara" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ LIKE sorgusu kullanıldığı için kısmi eşleşmeler de listelenir
- ✅ "Ahmet", "Mehmet" gibi kayıtlar gösterilir

---

### 5.8. Arama - Sonuç Bulunamadı

**Adımlar:**
1. Mevcut olmayan bir isim gir (örn: "XYZ12345")
2. "Ara" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ "Kayıt bulunamadı." mesajı gösterilir
- ✅ lblTable boş veya uyarı mesajı içerir

---

### 5.9. Boş Arama Testi

**Adımlar:**
1. Tüm alanları boş bırak
2. "Ara" butonuna tıkla

**Beklenen Sonuçlar:**
- ✅ Tüm kayıtlar listelenir VEYA
- ✅ "En az bir arama kriteri giriniz." uyarısı gösterilir
(Backend implementasyonuna bağlı)

---

### 5.10. Tablo Görünümü ve Responsive Test

**Adımlar:**
1. Herhangi bir arama yap
2. Sonuç tablosunu incele
3. Tarayıcı penceresini küçült (mobil görünüm)

**Beklenen Sonuçlar:**
- ✅ Tablo Bootstrap 5 stilleri ile oluşturulmuş
- ✅ table-responsive div içinde sarılı
- ✅ Mobil görünümde yatay scroll yapılabilir
- ✅ Tüm kolonlar okunabilir

---

### 5.11. Alt Bilgi (Footer) Testi

**Adımlar:**
1. Sayfanın alt kısmındaki bilgi kutusunu kontrol et

**Beklenen Sonuçlar:**
- ✅ Footer görünür
- ✅ Bilgi metni: "Dış Nizamiye Güvenlik : 9200 | Çay Ocağı : 9214 | Bölge Müdürlüğü Faks : 3974086 | Bakanlık Santral : 1000"
- ✅ alert-info stili uygulanmış
- ✅ Okunabilir ve düzgün gösterilir

---

## TEST SONUÇLARI ÖZET TABLOSU

| # | Sayfa | Toplam Test | Başarılı | Hatalı | Sonuç |
|---|-------|-------------|----------|--------|-------|
| 1 | Personel İzin Formu | 12 | | | |
| 2 | Dosya Birleştir | 9 | | | |
| 3 | PDF Birleştirici | 9 | | | |
| 4 | Resim Küçültme | 10 | | | |
| 5 | Telefon Rehberi | 11 | | | |
| **TOPLAM** | **51** | | | |

---

## BULUNAN HATALAR VE ÖNERİLER

### Kritik Hatalar
_Test sırasında bulunan kritik hatalar buraya eklenecektir._

---

### Orta Seviye Hatalar
_Test sırasında bulunan orta seviye hatalar buraya eklenecektir._

---

### Küçük Hatalar / İyileştirme Önerileri
_Test sırasında bulunan küçük hatalar ve öneriler buraya eklenecektir._

---

## NOTLAR

1. Test sırasında veritabanı bağlantısının aktif olduğundan emin olun.
2. Test verilerini test ortamında hazırlayın.
3. Her test senaryosu için ekran görüntüsü alınması önerilir.
4. Hata durumlarında browser console'u kontrol edin.
5. Toast mesajlarının düzgün göründüğünden emin olun.
6. PDF ve resim dosyalarının indirme işlemlerinde dosya boyutlarını kontrol edin.

---

**Test Tamamlanma Tarihi:** ____________________  
**Test Eden İmza:** ____________________
