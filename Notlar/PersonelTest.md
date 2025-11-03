# Personel Modülü Manuel Test Dokümanı

## Genel Bilgiler

**Test Edilen Modül:** Personel Yönetim Sistemi  
**Test Türü:** Manuel Fonksiyonel Test  
**Test Kapsamı:** Tüm personel modülü sayfaları  
**Gerekli Yetki:** Personel Modülü (Yetki No: 100)

---

## Test Öncesi Hazırlık

### Gerekli Veriler
1. **Test Kullanıcısı:** Mock session ile giriş yapılmış olmalı
2. **Test Veritabanı:** `personel`, `personel_izin`, `personel_ogrenim` tabloları dolu
3. **Referans Tablolar:** `personel_unvan`, `personel_sendika`, `personel_kurum`, `subeler` dolu olmalı

### Başlangıç Kontrolleri
- [ ] Web.Config bağlantı stringi doğru
- [ ] Session değerleri atanmış (Sicil, Ad, Kturu)
- [ ] Kullanıcının yetki kaydı var (yetki tablosunda Yetki_No = 100)

---

## 1. Kayit.aspx - Personel Kayıt/Güncelleme Sayfası

### Test Senaryosu 1.1: Yeni Personel Ekleme

**Ön Koşul:** Kayıt.aspx sayfası açık

**Adımlar:**
1. TC Kimlik No alanına 11 haneli geçerli TC girin (örn: 12345678901)
2. TC doğrulama mesajının yeşil olarak göründüğünü kontrol edin
3. Tüm zorunlu alanları doldurun:
   - Ad, Soyad
   - Doğum Yeri, Doğum Tarihi
   - Cinsiyet (dropdown)
   - Sicil No
   - Ünvan (dropdown)
   - Statü (dropdown)
   - Görev Yaptığı Birim (dropdown)
   - Çalışma Durumu (dropdown)
4. İsteğe bağlı alanları doldurun (telefon, mail, adres vb.)
5. **"Ekle"** butonuna tıklayın
6. **Beklenen Sonuç:**
   - Success toast mesajı görünmeli
   - Log dosyasına kayıt atılmalı
   - Veritabanında yeni kayıt oluşmalı

**Negatif Test:**
- Boş TC ile ekle butonuna tıklayın → Hata mesajı
- 10 haneli TC girin → Validation hatası
- Aynı TC ile 2. kez kayıt deneyin → SQL duplicate hatası beklenir

---

### Test Senaryosu 1.2: Mevcut Personel Güncelleme

**Adımlar:**
1. TC Kimlik No alanına veritabanında mevcut bir TC girin
2. **"Bilgileri Getir"** butonuna tıklayın
3. **Beklenen Sonuç:**
   - Tüm alanlar dolu gelecek
   - "Ekle" butonu gizlenecek
   - "Güncelle" butonu görünecek
4. Herhangi bir alanı değiştirin (örn: Cep Telefonu)
5. **"Güncelle"** butonuna tıklayın
6. **Beklenen Sonuç:**
   - Başarı mesajı + redirect
   - Veritabanında güncelleme yapılmalı
   - `SonGuncellemeTarihi` ve `SonGuncellemeKullanici` güncellenmiş olmalı

**Negatif Test:**
- Olmayan TC ile "Bilgileri Getir" → "Personel bulunamadı" uyarısı

---

### Test Senaryosu 1.3: Dropdown Veri Kontrolü

**Adımlar:**
1. Sayfayı yükleyin
2. Her dropdown'ı açıp veri kontrolü yapın:
   - `ddlUnvan` → personel_unvan tablosundan gelecek
   - `ddlSendika` → personel_sendika tablosundan gelecek
   - `ddlGorevYaptigiBirim` → subeler tablosundan gelecek
   - `ddlGeciciGelenKurum` ve `ddlGeciciGidenKurum` → personel_kurum
3. **Beklenen Sonuç:**
   - Tüm dropdown'lar "Seçiniz..." ile başlamalı
   - Alfabetik sıralı olmalı
   - Veritabanı verileri görünmeli

---

## 2. Liste.aspx - Personel Listeleme Sayfası

### Test Senaryosu 2.1: Varsayılan Listeleme

**Adımlar:**
1. Liste.aspx sayfasını açın
2. **Beklenen Sonuç:**
   - GridView'de aktif personeller listelenmeli
   - İstatistik kartları (Toplam, Aktif, Pasif) güncel olmalı
   - Kayıt sayısı badge'i görünmeli
   - Sayfalama çalışmalı (eğer >10 kayıt varsa)

---

### Test Senaryosu 2.2: Filtreleme

**Adımlar:**
1. Bir filtre seçin (örn: Ünvan = "Mühendis")
2. **"Ara"** butonuna tıklayın
3. **Beklenen Sonuç:**
   - Sadece seçilen ünvandaki personeller listelenmeli
   - Kayıt sayısı güncellenmiş olmalı
4. Birden fazla filtre kombinasyonu deneyin:
   - Ünvan + Birim
   - Cinsiyet + Medeni Hal
   - Çalışma Durumu + Sendika
5. **"Tümünü Listele"** butonuna tıklayın
6. **Beklenen Sonuç:**
   - Tüm filtreler sıfırlanmalı
   - Tüm aktif personeller görünmeli

**Negatif Test:**
- Hiç kayıt dönmeyen filtre kombinasyonu → "Kayıt yok" mesajı

---

### Test Senaryosu 2.3: Personel Detay Modal

**Adımlar:**
1. GridView'de herhangi bir satırın "Detay" linkine/butonuna tıklayın
2. **Beklenen Sonuç:**
   - Bootstrap modal açılmalı
   - Personelin tüm bilgileri görünmeli:
     - Kişisel Bilgiler (TC, Ad, Doğum vb.)
     - İş Bilgileri (Ünvan, Birim, Statü vb.)
     - İletişim Bilgileri
     - Acil Durum Kişisi
     - İzin Bilgileri
   - Resim varsa görünmeli, yoksa placeholder
3. Modalı kapatın ve başka kayıt deneyin

---

### Test Senaryosu 2.4: Excel Export

**Adımlar:**
1. Filtresiz listeyi yükleyin
2. **"Excel'e Aktar"** butonuna tıklayın
3. **Beklenen Sonuç:**
   - `PersonelListesi_YYYYMMDD.xls` dosyası inecek
   - Dosyayı açın → GridView verileri görünmeli
   - Türkçe karakterler düzgün olmalı
4. Boş liste ile Excel export deneyin → "Export edilecek veri yok" uyarısı

---

## 3. Ara.aspx - Personel Arama Sayfası

### Test Senaryosu 3.1: Sicil No ile Arama

**Adımlar:**
1. Ara.aspx sayfasını açın
2. **Beklenen Sonuç:**
   - Sayfa yüklendiğinde tüm aktif personel listelenmeli
   - Toplam aktif sayısı görünmeli
3. Sicil No alanına mevcut bir sicil girin
4. **"Ara"** butonuna tıklayın
5. **Beklenen Sonuç:**
   - Sadece o personel görünmeli
   - Bulunan sayısı = 1

---

### Test Senaryosu 3.2: Çoklu Filtre ile Arama

**Adımlar:**
1. Aşağıdaki kombinasyonları deneyin:
   - Ad + Soyad (LIKE arama)
   - Ünvan + Birim + Statü
   - Cinsiyet + Kan Grubu + Medeni Hal
   - Tarih aralığı (Kuruma Başlama Tarihi)
2. Her arama sonrası "Bulunan Sayı" kontrolü yapın

**Negatif Test:**
- Hiç olmayan ad/soyad → 0 kayıt

---

### Test Senaryosu 3.3: Temizle Fonksiyonu

**Adımlar:**
1. Tüm alanları doldurun ve arama yapın
2. **"Temizle"** butonuna tıklayın
3. **Beklenen Sonuç:**
   - Tüm textbox'lar boş
   - Tüm dropdown'lar "Hepsi" seçili
   - Grid ilk haline (tüm aktifler) dönmeli

---

### Test Senaryosu 3.4: Excel Export

**Adımlar:**
1. Filtreli bir arama yapın
2. **"Export"** butonuna tıklayın
3. **Beklenen Sonuç:**
   - `personel_ara_YYYYMMDD.xls` dosyası inmeli
   - Sadece arama sonuçları export edilmiş olmalı

---

## 4. IzinEkle.aspx - İzin Kayıt Sayfası

### Test Senaryosu 4.1: Personel Bilgisi Yükleme

**Adımlar:**
1. IzinEkle.aspx sayfasını açın
2. Sicil No alanına mevcut personel sicili girin
3. TextBox'tan çıkın (TextChanged event tetiklenir) VEYA "Bul" butonuna tıklayın
4. **Beklenen Sonuç:**
   - `pnlPersonelBilgi`, `pnlIzinDetay`, `pnlIzinGecmis` panelleri görünür olmalı
   - TC, Ad Soyad, Ünvan, Birim, Statü bilgileri dolu
   - Devreden, Cari, Toplam izin bilgileri görünmeli
   - Resim varsa görünmeli
   - GridView'de personelin izin geçmişi listelenmeli

**Negatif Test:**
- Olmayan sicil girin → "Personel bulunamadı" toast mesajı

---

### Test Senaryosu 4.2: Yıllık İzin Ekleme

**Adımlar:**
1. Personel bilgisini yükleyin
2. İzin türü: **"Yıllık İzin"** seçin
3. İzin süresi: 5 gün girin
4. İzne başlama tarihi: Bugünün tarihi (dd/MM/yyyy formatı)
5. **Beklenen Sonuç:**
   - `txtIzinSuresi` TextChanged event'i ile:
     - İzin bitiş tarihi otomatik hesaplanmalı
     - Göreve başlama tarihi hesaplanmalı
     - Memur ise: 5 gün sonra bitiş
     - İşçi ise: Pazarlar dahil edilip hesaplanmalı
6. **"Kaydet"** butonuna tıklayın
7. **Beklenen Sonuç:**
   - Toast success mesajı
   - `personel_izin` tablosuna kayıt atılmalı
   - `personel` tablosunda izin bakiyeleri güncellenmiş olmalı:
     - Devreden veya cari izinden düşülmeli
     - Toplam izin azalmalı
   - İzin geçmişi GridView'i yenilenmiş olmalı

**Negatif Test:**
- İzin süresi > Toplam İzin → "Toplam izinden fazla izin kullanılamaz" hatası
- Tarih çakışması olan kayıt → "Personelin seçilen tarihlerde zaten tanımlı izni bulunmaktadır" hatası

---

### Test Senaryosu 4.3: Rapor İzni Ekleme (PDF Oluşturma)

**Adımlar:**
1. İzin türü: **"Rapor"** seçin
2. Diğer alanları doldurun
3. **"Kaydet"** butonuna tıklayın
4. **Beklenen Sonuç:**
   - İzin kaydedilmeli
   - PDF dosyası otomatik indirilmeli (`Izin_Formu_[SicilNo]_[Tarih].pdf`)
   - PDF'i açın:
     - T.C. Ulaştırma ve Altyapı Bakanlığı başlığı
     - Personel bilgileri
     - İzin detayları
     - İmza alanları (Talep Eden, Birim Amiri, Uygundur)
     - Türkçe karakterler düzgün olmalı

---

### Test Senaryosu 4.4: İzin Güncelleme

**Adımlar:**
1. GridView'de bir izin kaydını seçin (SelectedIndexChanged)
2. **Beklenen Sonuç:**
   - Form alanları seçilen izin bilgileriyle dolu gelecek
   - "Güncelle" ve "Sil" butonları görünecek
   - "Kaydet" butonu gizlenecek
   - "Yeni Kayıt" butonu görünecek
3. İzin süresini değiştirin (örn: 5 → 7 gün)
4. **"Güncelle"** butonuna tıklayın
5. **Beklenen Sonuç:**
   - Toast success
   - Veritabanında güncelleme
   - GridView yenilenmiş

**Negatif Test:**
- Güncelleme sırasında tarih çakışması → Hata mesajı

---

### Test Senaryosu 4.5: İzin Silme

**Adımlar:**
1. GridView'den bir **Yıllık İzin** kaydı seçin
2. **"Sil"** butonuna tıklayın (Not: Butonu görebilmek için Yetki_No=199 gerekli)
3. **Beklenen Sonuç:**
   - Kayıt silinmeli
   - Eğer yıllık izin ise, personel tablosundaki izin bakiyesi geri yüklenmeli
   - GridView yenilenmiş olmalı

**Yetki Testi:**
- Normal kullanıcı ile Sil butonu görünmemeli
- Yetki_No=199 olan kullanıcı ile görünmeli

---

### Test Senaryosu 4.6: Excel Export

**Adımlar:**
1. İzin geçmişi dolu bir personel yükleyin
2. **"Excel Export"** butonuna tıklayın
3. **Beklenen Sonuç:**
   - `Personel_Izin_Gecmisi_[Sicil]_[Tarih].xlsx` dosyası inmeli
   - GridView verileri export edilmiş olmalı

---

## 5. IzinAra.aspx - İzin Arama ve Raporlama

### Test Senaryosu 5.1: Bugün İzinde Olanlar

**Adımlar:**
1. IzinAra.aspx sayfasını açın
2. **Beklenen Sonuç:**
   - İstatistik kartları dolu olmalı:
     - Toplam İzinli
     - Yıllık İzin
     - Raporlu
     - Diğer İzin
   - "Bugün İzinde Olanlar" GridView'i dolu olmalı
   - Grid footer'da toplam sayı görünmeli
   - Kayıtlar birime göre sıralı olmalı

---

### Test Senaryosu 5.2: Sicil No ile Personel Arama

**Adımlar:**
1. Sicil No textbox'ına bir sicil girin
2. TextBox'tan çıkın VEYA **"Ara"** butonuna tıklayın
3. **Beklenen Sonuç:**
   - `pnlPersonelBilgi` paneli görünür olmalı
   - Personel fotoğrafı, Ad Soyad, Statü görünmeli
   - Toplam izin istatistikleri (Yıllık, Rapor, Saatlik, Mazeret) görünmeli

**Negatif Test:**
- Olmayan sicil → "Personel bulunamadı" toast

---

### Test Senaryosu 5.3: Detaylı İzin Arama

**Adımlar:**
1. Arama kriterleri girin:
   - Ad Soyad (LIKE arama)
   - İzin Türü (dropdown)
   - Başlangıç - Bitiş Tarihi
2. **"Ara"** butonuna tıklayın
3. **Beklenen Sonuç:**
   - Arama Sonuçları GridView'i filtrelenmiş kayıtları göstermeli
   - Bulunan sayı güncellenmiş olmalı
   - Sayfalama çalışmalı

**Negatif Test:**
- Hiç sonuç dönmeyen kriterler → "Arama kriterlerine uygun kayıt bulunamadı" info toast

---

### Test Senaryosu 5.4: Temizle Fonksiyonu

**Adımlar:**
1. Tüm arama alanlarını doldurun
2. **"Temizle"** butonuna tıklayın
3. **Beklenen Sonuç:**
   - Tüm textbox'lar ve dropdown'lar sıfırlanmalı
   - Personel bilgi paneli gizlenmeli
   - Grid boşaltılmalı
   - "Arama kriterleri temizlendi" info toast

---

### Test Senaryosu 5.5: Excel Export

**Adımlar:**
1. Arama yapın (kayıt bulunmalı)
2. **"Excel'e Aktar"** butonuna tıklayın
3. **Beklenen Sonuç:**
   - `PersonelIzinArama_[Tarih].xls` dosyası inmeli
   - Arama sonuçları export edilmiş olmalı

**Negatif Test:**
- Boş grid ile export → "Export edilecek veri bulunamadı" warning toast

---

## 6. GenelIzin.aspx - Genel İzin Raporu

### Test Senaryosu 6.1: Varsayılan Listeleme

**Adımlar:**
1. GenelIzin.aspx sayfasını açın
2. **Beklenen Sonuç:**
   - Yıl dropdown'ı mevcut yıl seçili olmalı (Mevcut yıl -2 ile +1 arası)
   - Tüm aktif personellerin izin özeti listelenmeli:
     - Resim, Sicil No, Ad Soyad
     - Rapor, Saatlik, Mazeret, Hastane, Yıllık İzin toplamları
     - Genel Toplam
     - Devreden, Cari Yıl, Kalan İzin
   - Kayıt sayısı badge'i güncel

---

### Test Senaryosu 6.2: Yıl Filtresi

**Adımlar:**
1. Yıl dropdown'ından farklı bir yıl seçin
2. **Beklenen Sonuç:**
   - Grid o yıla ait izin verilerini göstermeli
   - İstatistikler seçilen yıla göre hesaplanmış olmalı

---

### Test Senaryosu 6.3: Arama ve Filtreleme

**Adımlar:**
1. Arama textbox'ına ad/soyad/sicil girin
2. **"Ara"** butonuna tıklayın
3. **Beklenen Sonuç:**
   - Eşleşen kayıtlar filtrelenmeli
4. İzin Türü dropdown'ından bir tür seçin (örn: "Yıllık İzin")
5. **"Ara"** butonuna tıklayın
6. **Beklenen Sonuç:**
   - Sadece seçilen izin türünde kayıt olanlar listelenmeli

**Negatif Test:**
- Sonuç dönmeyen arama → "Arama kriterlerine uygun kayıt bulunamadı" info toast

---

### Test Senaryosu 6.4: Tümünü Listele

**Adımlar:**
1. Filtreli arama yapın
2. **"Tümünü Listele"** butonuna tıklayın
3. **Beklenen Sonuç:**
   - Arama textbox ve dropdown sıfırlanmalı
   - Tüm kayıtlar yeniden listelenmeli
   - "Tüm kayıtlar listelendi" success toast

---

### Test Senaryosu 6.5: Excel Export

**Adımlar:**
1. Liste yüklü olsun
2. **"Excel'e Aktar"** butonuna tıklayın
3. **Beklenen Sonuç:**
   - `PersonelIzinRaporu_[Yıl]_[Tarih].xls` dosyası inmeli
   - Tüm kolonlar export edilmiş olmalı

**Negatif Test:**
- Boş liste → "Excel'e aktarılacak veri bulunamadı" warning toast

---

## 7. OgrenimEkle.aspx - Öğrenim Bilgisi Yönetimi

### Test Senaryosu 7.1: Personel Arama (TC/Sicil)

**Adımlar:**
1. OgrenimEkle.aspx sayfasını açın
2. TC No alanına mevcut bir TC girin
3. **"TC ile Ara"** butonuna tıklayın
4. **Beklenen Sonuç:**
   - Sicil, TC, Ad Soyad alanları dolu gelecek
   - Resim görünecek
   - Personelin mevcut öğrenim kayıtları GridView'de listelenecek
5. Aynı işlemi Sicil No ile tekrarlayın

**Negatif Test:**
- Olmayan TC/Sicil → "Personel bulunamadı" error alert

---

### Test Senaryosu 7.2: Öğrenim Kaydı Ekleme

**Adımlar:**
1. Personel bilgisi yüklenmiş olsun
2. Öğrenim Durumu: **"Lisans"** seçin
3. Okul: "İstanbul Üniversitesi" yazın
4. Bölüm: "Bilgisayar Mühendisliği" yazın
5. Mezuniyet Tarihi: Bir tarih seçin
6. **"Ekle"** butonuna tıklayın
7. **Beklenen Sonuç:**
   - "Öğrenim kaydı başarıyla eklendi" success toast
   - GridView yenilenmeli
   - Form alanları temizlenmeli
   - `personel_ogrenim` tablosuna kayıt atılmalı

**Negatif Test:**
- Boş okul alanı ile ekleme → "Öğrenim Durumu ve Okul bilgileri zorunludur" error

---

### Test Senaryosu 7.3: Öğrenim Kaydı Silme

**Adımlar:**
1. GridView'de bir satır seçin (SelectedIndexChanged)
2. **Beklenen Sonuç:**
   - Form alanları seçilen kayıtla dolu gelecek
   - "Sil" butonu görünür olacak
3. **"Sil"** butonuna tıklayın
4. **Beklenen Sonuç:**
   - "Öğrenim kaydı başarıyla silindi" success toast
   - GridView yenilenmeli
   - Form alanları temizlenmeli
   - Veritabanından kayıt silinmiş olmalı

**Negatif Test:**
- Hiçbir satır seçmeden Sil → "Silinecek kayıt seçilmedi" error

---

## 8. Raporlama.aspx - Personel Raporlama

### Test Senaryosu 8.1: Sayfa Yükleme ve İstatistikler

**Adımlar:**
1. Raporlama.aspx sayfasını açın
2. **Beklenen Sonuç:**
   - İstatistik kartları dolu olmalı:
     - Kadrolu Aktif
     - Geçici Aktif
     - Firma/TYP
     - Toplam Aktif
     - Kadrolu Geçici (Pasif)
     - Toplam Personel
   - SQL sorgularının doğru çalıştığını kontrol edin

---

### Test Senaryosu 8.2: Personel Dağılım GridView

**Adımlar:**
1. "Personel Dağılımı" GridView'ini inceleyin
2. **Beklenen Sonuç:**
   - Her ünvan için satır olmalı
   - Kolonlar:
     - Toplam Kadrolu
     - Kadrolu Giden
     - Kadrolu
     - Geçici Gelen
     - Firma
     - TYP
     - Toplam Aktif
   - Footer'da toplamlar doğru hesaplanmış olmalı
3. Footer'daki "Toplam X Adet Unvan" metnini kontrol edin

---

### Test Senaryosu 8.3: Birim Dağılım GridView

**Adımlar:**
1. "Birim Dağılımı" GridView'ini inceleyin
2. **Beklenen Sonuç:**
   - Her birim için personel sayısı görünmeli
   - Alfabetik sıralı olmalı
   - Sadece aktif ve pasif olmayan personeller sayılmalı

---

### Test Senaryosu 8.4: Grafikler (Chart.js)

**Adımlar:**
1. Sayfa yüklendikten sonra 3 grafik görmelisiniz:
   - Kadro Dağılımı (Pie/Doughnut chart)
   - Sendika Dağılımı
   - Engel Durumu Dağılımı
2. **Beklenen Sonuç:**
   - Grafikler doğru veri ile render olmalı
   - Hidden field'lar (`hdnKadroData`, `hdnSendikaData`, `hdnEngelData`) JSON içermeli
   - Grafiklerin üzerine gelindiğinde tooltip görünmeli
3. Browser console'da JavaScript hatası olmamalı

---

### Test Senaryosu 8.5: Excel Export

**Adımlar:**
1. **"Excel'e Aktar"** butonuna tıklayın
2. **Beklenen Sonuç:**
   - `PersonelDagilimRaporu.xls` dosyası inmeli
   - Personel Dağılım GridView export edilmiş olmalı
   - Footer toplamları da dahil olmalı

---

## 9. Cross-Page ve Entegrasyon Testleri

### Test Senaryosu 9.1: Yetki Kontrolleri

**Adımlar:**
1. Yetki olmayan bir kullanıcı ile giriş yapın (Yetki_No ≠ 100)
2. Herhangi bir personel sayfasını açmaya çalışın
3. **Beklenen Sonuç:**
   - "Bu işlem için yeterli yetkiniz bulunmamaktadır" hatası
   - Anasayfa.aspx'e redirect

---

### Test Senaryosu 9.2: Session Timeout

**Adımlar:**
1. Session'u manuel olarak temizleyin (`Session.Clear()` veya tarayıcı dev tools)
2. Herhangi bir personel sayfasını yenileyin
3. **Beklenen Sonuç:**
   - "Oturum süreniz dolmuş" hatası (geliştirme modunda mock session atanır)

---

### Test Senaryosu 9.3: Veri Tutarlılığı

**Adımlar:**
1. Kayit.aspx'de personel ekleyin
2. Liste.aspx'de yeni personelin göründüğünü doğrulayın
3. IzinEkle.aspx'de yeni personele izin ekleyin
4. GenelIzin.aspx'de izin toplamlarını kontrol edin
5. OgrenimEkle.aspx'de öğrenim ekleyin
6. Raporlama.aspx'de istatistiklerin güncel olduğunu doğrulayın

---

## 10. Performans ve UI/UX Testleri

### Test Senaryosu 10.1: Büyük Veri Testleri

**Adımlar:**
1. Veritabanına 500+ personel ekleyin
2. Liste.aspx, GenelIzin.aspx gibi listeleme sayfalarını açın
3. **Beklenen Sonuç:**
   - Sayfa yüklenme süresi < 3 saniye
   - Sayfalama çalışmalı
   - GridView donmamalı

---

### Test Senaryosu 10.2: Toast Notification Testleri

**Adımlar:**
1. Her sayfada success/error/warning/info toast'ları tetikleyin
2. **Beklenen Sonuç:**
   - Toast sağ üstte görünmeli
   - 3 saniye sonra otomatik kapanmalı
   - İkonlar doğru (Font Awesome)
   - Renkler doğru (Bootstrap 5 sınıfları)

---

### Test Senaryosu 10.3: Responsive Test

**Adımlar:**
1. Tarayıcı genişliğini küçültün (mobil görünüm)
2. Tüm sayfaları kontrol edin
3. **Beklenen Sonuç:**
   - GridView'ler responsive olmalı (Bootstrap table-responsive)
   - Formlar düzgün görünmeli
   - Butonlar tıklanabilir olmalı

---

## Test Sonuç Raporu Şablonu

```
TEST RAPORU
-----------
Tarih: [YYYY-MM-DD]
Test Eden: [İsim]
Modül: Personel Yönetim Sistemi

TOPLAM TEST SENARYOSU: 50+
BAŞARILI: __
BAŞARISIZ: __
ATLANAN: __

BULUNAN HATALAR:
1. [Hata Açıklaması]
   - Adımlar: ...
   - Beklenen: ...
   - Gerçekleşen: ...
   - Kritiklik: Yüksek/Orta/Düşük

NOTLAR:
- ...
```

---

## Test Tamamlama Kontrolü

- [ ] Tüm CRUD işlemleri test edildi
- [ ] Tüm dropdown'lar veri yüklüyor
- [ ] Tüm Excel export'lar çalışıyor
- [ ] Tüm toast mesajları görünüyor
- [ ] Yetki kontrolleri çalışıyor
- [ ] Negatif testler geçti
- [ ] Log dosyası düzenli yazılıyor
- [ ] Veritabanı tutarlılığı sağlandı
- [ ] UI/UX sorunsuz

---

Tüm senaryolardaki pozitif ve negatif test case'leri sayarak:
78 test case

**Test Doküman Sonu**

GEMİNİ:

Bu test planı, bir testçinin veya geliştiricinin "happy path" (beklenen/olumlu) senaryoları, "negative" (olumsuz/hata) senaryoları ve veri doğrulama adımlarını takip etmesi için tasarlanmıştır.

Testing Stratejisi
Testler, "Kara Kutu (Black Box)" ve "Gri Kutu (Gray Box)" metodolojilerinin bir karışımını kullanacaktır.

Kara Kutu: Kullanıcı arayüzünden (UI) girdiler verip, UI'daki çıktıları (mesajlar, grid güncellemeleri) gözlemleyeceğiz.

Gri Kutu: Kodun ve veritabanı şemasının bilgisiyle, UI'da yapılan bir işlemin (örn. "Ekle" butonu) personel veya personel_izin gibi ilgili tablolara doğru veriyi yazıp yazmadığını (veya güncelleyip güncellemediğini) kontrol edeceğiz.

1. Kayit.aspx.cs (Personel Kayıt Sayfası)
Bu sayfanın temel amacı yeni personel eklemek veya mevcut bir personeli TC Numarası ile bularak güncellemektir.

Senaryo 1.1: Sayfanın Başarılı Yüklenmesi

Kayit.aspx sayfasına gidin.

Beklenen Sonuç: Tüm form elemanları (Metin kutuları, dropdown'lar) boş veya "Seçiniz" durumunda yüklenir. btnGuncelle butonu görünmez (Visible = false), btnEkle butonu görünür olmalıdır.

Doğrulama: ddlUnvan, ddlSendika, ddlGorevYaptigiBirim gibi dropdown'ların personel_unvan, personel_sendika ve subeler tablolarından gelen verilerle dolu olduğunu kontrol edin.

Senaryo 1.2: TC No ile Mevcut Personeli Getirme (Güncelleme Modu)

Ön Koşul: Veritabanında TC'si bilinen bir personel (örn. TC: 11111111111) bulunsun.

txtTcKimlikNo alanına "11111111111" yazın.

btnBilgileriGetir butonuna tıklayın.

Beklenen Sonuç:

txtAdi, txtSoyad, txtSicilNo, ddlUnvan vb. tüm alanlar veritabanındaki personel bilgileriyle dolar.

"Personel bilgileri getirildi" şeklinde bir log veya ShowToast mesajı (eğer varsa) beklenir.

btnEkle butonu görünmez olur, btnGuncelle butonu görünür hale gelir.

ViewState["CurrentTcKimlikNo"] değerinin "11111111111" olarak ayarlanması beklenir (Gri Kutu).

Senaryo 1.3: Mevcut Personeli Güncelleme

Senaryo 1.2'yi uygulayın.

txtAdres alanındaki veriyi "Yeni Adres Bilgisi" olarak değiştirin.

btnGuncelle butonuna tıklayın.

Beklenen Sonuç: "Personel başarıyla güncellendi" mesajı alınır ve sayfa muhtemelen temizlenir veya yeniden yönlendirilir.

Veritabanı Doğrulaması: personel tablosuna gidip TC'si "11111111111" olan kaydın Adres kolonunun "Yeni Adres Bilgisi" olarak güncellendiğini ve SonGuncellemeTarihi kolonunun şu anki zamanı yansıttığını doğrulayın.

Senaryo 1.4: Yeni Personel Ekleme (Başarılı)

Ön Koşul: Veritabanında bulunmayan geçerli bir TC No (örn. "99999999999") kullanın.

txtTcKimlikNo alanına "99999999999" yazın.

Gerekli tüm alanları (Ad, Soyad, Sicil No, Birim, Statü vb.) doldurun.

btnEkle butonuna tıklayın.

Beklenen Sonuç: "Personel başarıyla eklendi" mesajı alınır.

Veritabanı Doğrulaması: personel tablosuna gidip TC'si "99999999999" olan yeni bir kaydın eklendiğini ve KayitKullanici alanının dolu olduğunu doğrulayın.

Senaryo 1.5: Hatalı Senaryolar (Negatif Testler)

Varolmayan Personeli Getirme: txtTcKimlikNo alanına "00000000000" (varolmayan) yazın ve btnBilgileriGetir'e tıklayın.

Beklenen Sonuç: "TC Kimlik No ile personel bulunamadı" uyarısı alınır.

Eksik TC No: txtTcKimlikNo alanına "123" yazın ve btnBilgileriGetir'e tıklayın.

Beklenen Sonuç: "Lütfen TC Kimlik No giriniz" (veya kodda varsa 11 haneli olmalı uyarısı) alınır.

Zorunlu Alan Eksikliği (Ekleme): txtAdi veya txtSoyad alanını boş bırakıp btnEkle'ye tıklayın.

Beklenen Sonuç: ASP.NET Validator'ların (eğer Page.IsValid kontrolü varsa) veya try-catch bloğunun hatayı yakalaması ve "Zorunlu alanları doldurun" benzeri bir uyarı vermesi beklenir.

2. Ara.aspx.cs (Detaylı Personel Arama Sayfası)
Bu sayfa, personelleri birçok farklı kritere göre filtrelemek için kullanılır.

Senaryo 2.1: Sayfa Yükleme (Varsayılan Liste)

Ara.aspx sayfasına gidin.

Beklenen Sonuç: gvPersonelAra grid'i, personel tablosundaki tüm aktif (Durum = 'Aktif') personellerle dolu olarak gelir. lblToplamSayi bu aktif personel sayısını gösterir. Tüm filtre dropdown'ları "Hepsi" seçili olarak gelir.

Senaryo 2.2: Tek Kriterli Arama (Sicil No)

txtSicilNo alanına veritabanında var olan bir personelin sicil numarasını yazın.

btnAra butonuna tıklayın.

Beklenen Sonuç: gvPersonelAra grid'i sadece o sicil numarasına sahip personeli gösterecek şekilde güncellenir. lblBulunanSayi "1" olarak güncellenir.

Senaryo 2.3: Çok Kriterli Arama (Birim ve Statü)

ddlBirim'den belirli bir birimi (örn. "Bilgi İşlem") seçin.

ddlStatu'den "Memur" seçin.

btnAra butonuna tıklayın.

Beklenen Sonuç: Grid, sadece "Bilgi İşlem" biriminde çalışan "Memur" statüsündeki personelleri listeler. lblBulunanSayi güncellenir.

Veritabanı Doğrulaması: SELECT COUNT(*) FROM personel WHERE GorevYaptigiBirim = 'Bilgi İşlem' AND Statu = 'Memur' sorgusunu çalıştırın ve sonucun lblBulunanSayi ile eşleştiğini doğrulayın.

Senaryo 2.4: Arama Sonucu Bulunamadı

txtAdi alanına "ASDFQWER" gibi anlamsız bir metin girin.

btnAra butonuna tıklayın.

Beklenen Sonuç: Grid boşalır. lblBulunanSayi "0" olur.

Senaryo 2.5: Temizle Butonu

Senaryo 2.3'ü uygulayarak filtreleri doldurun ve arama yapın.

btnTemizle butonuna tıklayın.

Beklenen Sonuç: Tüm filtre alanları (metin kutuları, dropdown'lar) varsayılan değerlerine ("Boş" veya "Hepsi") döner. Grid, Senaryo 2.1'deki gibi tüm aktif personellerle yeniden yüklenir.

Senaryo 2.6: Excel'e Aktarma

Senaryo 2.3'ü uygulayarak filtrelenmiş bir liste elde edin.

btnExport butonuna tıklayın.

Beklenen Sonuç: personel_ara_...xls adında bir Excel dosyası indirilir.

Doğrulama: İndirilen dosyayı açın ve içeriğinin, gvPersonelAra grid'indeki filtrelenmiş verilerle (sadece Bilgi İşlem / Memur) birebir aynı olduğunu doğrulayın.

3. Liste.aspx.cs (Personel Listesi ve Detay Modalı)
Bu sayfa, Ara.aspx'e benzer ancak ana liste görünümüdür ve detay göstermek için bir modal (popup) içerir.

Senaryo 3.1: Sayfa Yükleme (İstatistikler ve Liste)

Liste.aspx sayfasına gidin.

Beklenen Sonuç:

PersonellerGrid tüm aktif personelleri listeler. lblKayitSayisi bu sayıyı gösterir.

lblToplamPersonel, lblAktifPersonel, lblPasifPersonel istatistikleri doğru değerlerle dolar.

Veritabanı Doğrulaması: SELECT COUNT(*) FROM personel, SELECT COUNT(*) FROM personel WHERE Durum = 'Aktif' ve ... Durum = 'Pasif' sorgularını çalıştırıp istatistiklerin doğruluğunu kontrol edin.

Senaryo 3.2: Filtreleme (Durum: Pasif)

ddlDurum dropdown'ından "Pasif" seçeneğini seçin.

btnAra butonuna tıklayın.

Beklenen Sonuç: PersonellerGrid sadece Durum = 'Pasif' olan personelleri listeler. lblKayitSayisi güncellenir.

Senaryo 3.3: Tümünü Listele (Filtre Sıfırlama)

Senaryo 3.2'yi uygulayın.

btnTumunuListele butonuna tıklayın.

Beklenen Sonuç: ddlDurum "Aktif" olarak geri döner (kod FiltreleriTemizle'de bunu yapıyor) ve grid aktif personellerle yeniden yüklenir.

Senaryo 3.4: Personel Detay Modalını Açma

PersonellerGrid'de listelenen herhangi bir personelin satırındaki "Detay" (veya RowCommand'ı tetikleyen) butona tıklayın.

Beklenen Sonuç: modalPersonelDetay id'li Bootstrap modal'ı açılır.

Doğrulama: Modal içindeki lblDetayAdSoyad, lblDetaySicilNo, lblDetayBirim, lblDetayToplamIzin vb. tüm etiketlerin, seçilen personele ait doğru bilgilerle dolduğunu doğrulayın. Personel resmi varsa imgPersonelFoto'nun, yoksa divNoFoto'nun görünür olduğunu kontrol edin.

4. OgrenimEkle.aspx.cs (Personel Öğrenim Bilgisi)
Bu sayfa, mevcut bir personele (TC veya Sicil ile bulunan) eğitim/öğrenim kayıtları eklemek, silmek veya listelemek için kullanılır.

Senaryo 4.1: Personel Arama (TC ile)

Ön Koşul: personel tablosunda bir personel ve personel_ogrenim tablosunda bu personele ait (TC_No ile bağlı) en az bir kayıt bulunsun.

txtTc alanına personelin TC'sini girin.

btnTcAra butonuna tıklayın.

Beklenen Sonuç: lblAdSoyad ve txtSicil alanları dolar. GridViewOgrenim grid'i, bu personele ait mevcut öğrenim kayıtlarını (örn. "Lise", "Üniversite") listeler.

Senaryo 4.2: Personel Arama (Sicil ile)

txtSicil alanına personelin Sicil No'sunu girin.

btnSicilAra butonuna tıklayın.

Beklenen Sonuç: lblAdSoyad ve txtTc alanları dolar. GridViewOgrenim grid'i, bu personele ait mevcut öğrenim kayıtlarını listeler.

Senaryo 4.3: Yeni Öğrenim Bilgisi Ekleme

Senaryo 4.1 veya 4.2'yi uygulayın.

ddlOgrenimDurumu'nu "Yüksek Lisans" seçin.

txtOkul'a "Test Üniversitesi" yazın.

txtBolum'e "Test Bölümü" yazın.

EgitimEkle butonuna tıklayın.

Beklenen Sonuç: "Öğrenim kaydı başarıyla eklendi" mesajı alınır.

Doğrulama:

GridViewOgrenim grid'i güncellenir ve yeni "Yüksek Lisans" kaydını gösterir.

Form alanları (txtOkul, txtBolum vb.) temizlenir.

Veritabanı Doğrulaması: personel_ogrenim tablosunda, personelin TC_No'su ile eşleşen "Yüksek Lisans" kaydının eklendiğini doğrulayın.

Senaryo 4.4: Öğrenim Bilgisi Silme

Senaryo 4.3'ü uygulayarak silinecek bir kayıt olduğundan emin olun.

GridViewOgrenim grid'inde yeni eklenen "Yüksek Lisans" kaydının "Seç" butonuna/linkine tıklayın.

Beklenen Sonuç (Seçim): ddlOgrenimDurumu, txtOkul, txtBolum alanları seçilen kaydın bilgileriyle dolar. btnOgrenimSil butonu görünür hale gelir.

btnOgrenimSil butonuna tıklayın.

Beklenen Sonuç (Silme): "Öğrenim kaydı başarıyla silindi" mesajı alınır.

Doğrulama:

"Yüksek Lisans" kaydı GridViewOgrenim grid'inden kaybolur.

Form alanları temizlenir ve btnOgrenimSil tekrar görünmez olur.

Veritabanı Doğrulaması: personel_ogrenim tablosundan bu kaydın silindiğini doğrulayın.

5. IzinEkle.aspx.cs (Personel İzin İşlemleri)
Bu sayfa, personeli sicil no ile bularak izin geçmişini listeler ve yeni izin (yıllık, rapor vb.) ekler, günceller veya siler. İzin bakiyelerini de yönetir.

Senaryo 5.1: Personel Bilgisi ve İzin Yükleme

txtSicilNo alanına, personel tablosunda Devredenizin ve cariyilizni değerleri olan (örn. 10 ve 20) bir personelin sicil nosunu girin.

btnPersonelBul'a tıklayın (veya TextChanged'i tetikleyin).

Beklenen Sonuç:

pnlPersonelBilgi görünür olur.

lblPersonelAd, txtUnvan, txtBirim dolar.

lblDevredenIzin "10", lblCariIzin "20" ve lblToplamIzin "30" yazar.

gvIzinler grid'i, bu personelin personel_izin tablosundaki geçmiş izinlerini listeler.

İzin giriş formu "Ekleme" modunda hazır bekler (btnKaydet görünür).

Senaryo 5.2: Tarih Hesaplama (Memur vs İşçi)

Senaryo 5.1'i "Memur" statülü bir personel için yapın.

txtIzneBaslamaTarihi'ne (örn. Pazartesi, 10.11.2025) girin.

txtIzinSuresi'ne "5" girin ve alandan çıkın.

Beklenen Sonuç (Memur): txtIzinBitisTarihi "14.11.2025" (Cuma), txtGoreveBaslamaTarihi "15.11.2025" (Cumartesi) olarak hesaplanır.

Aynı işlemi "İşçi" statülü personel için tekrarlayın.

Beklenen Sonuç (İşçi): Kod CalculateSundayCount'ı çalıştırır. Pazar günleri (eğer araya giriyorsa) süreye eklenir. 10.11 (Pzt) + 5 gün = 14.11 (Cuma). Pazar girmedi. (Testi 7 gün ile yapın: 10.11 + 7 gün = 16.11 Pazar'ı içerir. Kod Pazar'ı ekler. Bitiş tarihi Pazar'ı atlayarak hesaplanmalıdır). Kodun analizine göre İşçi için Pazar günleri ekleniyor. 10.11(Pzt) + 7 gün (16.11 Pazar dahil) -> Kod 1 Pazar bulur. Bitiş: 10.11 + 7 + 1 = 18.11 -> Bitiş: 17.11 (Pzt). Başlama: 18.11 (Salı). Bu hesabı doğrulayın.

Senaryo 5.3: Yeni "Yıllık İzin" Ekleme (Bakiye Düşüşü)

Senaryo 5.1'i (Toplam İzin: 30) uygulayın.

ddlIzinTuru'nu "Yıllık İzin" seçin.

txtIzinSuresi'ne "5" girin ve tarihleri doldurun.

btnKaydet'e tıklayın.

Beklenen Sonuç: "İzin kaydı başarıyla eklendi" mesajı alınır.

Doğrulama:

Yeni izin gvIzinler'e eklenir.

Sayfadaki bakiye etiketleri güncellenir (örn. lblToplamIzin "25" olur).

Veritabanı Doğrulaması: personel_izin'e kayıt eklenir. personel tablosunda bu personelin toplamizin kolonu 25'e, cariyilizni (veya Devredenizin) 5 eksilmiş olarak güncellenir.

Senaryo 5.4: Yeni "Rapor" Ekleme (Bakiye Etkilenmez + PDF)

Senaryo 5.1'i (Toplam İzin: 30) uygulayın.

ddlIzinTuru'nu "Rapor" seçin.

Süre ve tarihleri girin.

btnKaydet'e tıklayın.

Beklenen Sonuç: "İzin kaydı başarıyla eklendi" mesajı alınır ve tarayıcı Izin_Formu_...pdf adında bir dosya indirmeyi başlatır.

Doğrulama:

Yeni izin gvIzinler'e eklenir.

Sayfadaki bakiye etiketleri (Toplam İzin: 30) değişmez.

Veritabanı Doğrulaması: personel_izin'e kayıt eklenir. personel tablosundaki bakiye değişmez.

Senaryo 5.5: Tarih Çakışması (Conflict Check)

Senaryo 5.3'ü (örn. 10.11-14.11 arası) uygulayın.

Yeni bir izin eklemeye çalışın (türü fark etmez).

txtIzneBaslamaTarihi'ne "12.11.2025" (çakışan bir tarih) girin.

btnKaydet'e tıklayın.

Beklenen Sonuç: lblMesaj "Personelin seçilen tarihlerde zaten tanımlı izni bulunmaktadır" uyarısını gösterir. Kayıt eklenmez.

Senaryo 5.6: "Yıllık İzin" Silme (Bakiye İadesi)

Senaryo 5.3'ü (5 günlük yıllık izin) uygulayın. Bakiye 25'e düştü.

gvIzinler'den bu 5 günlük "Yıllık İzin" kaydını "Seç"in.

Form dolar, btnGuncelle ve btnSil görünür olur.

btnSil'e tıklayın.

Beklenen Sonuç: "İzin kaydı başarıyla silindi" mesajı alınır.

Doğrulama:

Kayıt gvIzinler'den silinir.

Sayfadaki bakiye etiketleri güncellenir (örn. lblToplamIzin tekrar "30" olur).

Veritabanı Doğrulaması: personel_izin'den kayıt silinir. personel tablosunda cariyilizni ve toplamizin 5 artarak eski haline döner.

6. IzinAra.aspx.cs (İzin Arama ve İstatistik)
Bu sayfa, bugün izinlileri gösterir, sicil no ile personel bazlı toplamları verir ve detaylı izin araması yapar.

Senaryo 6.1: Sayfa Yükleme (Bugün İzinliler ve İstatistikler)

Ön Koşul: personel_izin tablosunda bugünün tarihini içeren (örn. izne_Baslama_Tarihi <= BUGÜN AND izin_Bitis_Tarihi >= BUGÜN) birkaç izin kaydı (Yıllık, Rapor vb.) oluşturun.

IzinAra.aspx sayfasına gidin.

Beklenen Sonuç:

Üstteki istatistik kartları (lblToplamIzinli, lblYillikIzin, lblRaporlu) bugünkü izinli sayılarını doğru gösterir.

BugunIzinlilerGrid grid'i, sadece bugün izinde olan personelleri listeler.

lblBugunSayisi bu grid'deki sayıyı gösterir.

Senaryo 6.2: Personel Bazlı Arama (Toplam Kullanım)

txtSicilNo alanına (Detaylı Arama veya Personel Arama bölümündeki) bir sicil no girin.

btnSicilAra'ya tıklayın.

Beklenen Sonuç: pnlPersonelBilgi paneli görünür olur.

Doğrulama: lblToplamYillik, lblToplamRapor vb. etiketler, o personelin tüm zamanlardaki toplam izin kullanımını (tüm izin_Suresi toplamları) gösterir.

Veritabanı Doğrulaması: SELECT SUM(CASE WHEN izin_turu = 'Yıllık İzin'...) FROM personel_izin WHERE Sicil_No = @SicilNo sorgusunu çalıştırıp etiketlerdeki değerlerle karşılaştırın.

Senaryo 6.3: Detaylı İzin Arama (Tarih Aralığı ve Tür)

"Detaylı Arama" bölümüne gidin.

ddlIzinTuru'nu "Rapor" seçin.

txtBaslangicTarihi ve txtBitisTarihi girerek belirli bir dönemi (örn. geçen ay) seçin.

btnAra'ya tıklayın.

Beklenen Sonuç: AramaSonuclariGrid grid'i, sadece "Rapor" türünde olan ve belirtilen tarih aralığında başlayan/biten tüm izin kayıtlarını listeler. lblAramaSayisi güncellenir.

7. GenelIzin.aspx.cs (Yıllık Toplam İzin Raporu)
Bu sayfa, seçilen bir yıl için tüm personelin izin kullanımlarını özetleyen bir pivot tablo/rapor sunar.

Senaryo 7.1: Sayfa Yükleme (Güncel Yıl Raporu)

GenelIzin.aspx sayfasına gidin.

Beklenen Sonuç: DdlYil (Yıl dropdown) güncel yılı seçili getirir (örn. 2025). PersonelIzinGrid yüklenir.

Doğrulama (Çok Önemli): Grid'den rastgele bir personel seçin (örn. Personel A).

Veritabanı Doğrulaması: SELECT SUM(izin_Suresi) FROM personel_izin WHERE Sicil_No = 'PersonelA_SicilNo' AND YEAR(izne_Baslama_Tarihi) = 2025 AND izin_turu = 'Yıllık İzin' sorgusunu çalıştırın. Çıkan sonucu, grid'deki "Personel A" satırındaki Toplam_Yillik sütunundaki değerle karşılaştırın. Aynı doğrulamayı Toplam_Rapor, Toplam_Saatlik vb. için de yapın.

Senaryo 7.2: Yıl Değiştirme

DdlYil'dan farklı bir yıl seçin (örn. 2024).

Beklenen Sonuç: Grid otomatik olarak güncellenir.

Doğrulama: Senaryo 7.1'deki Veritabanı Doğrulamasını 2024 yılı için tekrarlayın.

Senaryo 7.3: Filtreleme (İsim ve İzin Türü)

TxtArama alanına bir personel adı (örn. "Mehmet") girin.

DdlIzinTuru'ndan "Rapor" seçin.

BtnAra'ya tıklayın.

Beklenen Sonuç: Grid, sadece adında "Mehmet" geçen VE 2025 yılında 0'dan fazla "Rapor" izni kullanan personelleri listeler.

Doğrulama: BtnTumunuListele'ye tıklayın, arama kutuları temizlenmeli ve grid 2025 yılı için tam listeye dönmelidir.

Senaryo 7.4: Excel'e Aktarma (Filtreli)

Senaryo 7.3'ü uygulayın.

BtnExcelAktar'a tıklayın.

Beklenen Sonuç: İndirilen .xls dosyası, sadece filtrelenmiş veriyi (Mehmet / Raporlu) içerir.

8. Raporlama.aspx.cs (Personel Dashboard/İstatistik)
Bu sayfa, personel dağılımlarını (kadro, birim, sendika vb.) istatistik, grid ve grafiklerle gösteren bir dashboard'dur.

Senaryo 8.1: Sayfa Yükleme ve İstatistik Doğrulama

Raporlama.aspx sayfasına gidin.

Beklenen Sonuç: Sayfadaki tüm istatistik kartları (lblKadroluAktif, lblGeciciAktif, lblToplamAktif vb.) yüklenir.

Veritabanı Doğrulaması: LoadStatistics metodundaki her bir sorguyu (örn. SELECT COUNT(*) FROM personel WHERE CalismaDurumu='Kadrolu Aktif Çalışan' ...) manuel olarak veritabanında çalıştırın ve sonuçların etiketlerdeki değerlerle eşleştiğini doğrulayın.

Senaryo 8.2: Grid Doğrulaması (Personel Dağılım)

PersonelDagılımGrid'i (Unvanlara göre dağılım) inceleyin.

Beklenen Sonuç: Grid'in "Footer" (alt toplam) satırı, CalculateGridFooter tarafından hesaplanan toplamları gösterir.

Veritabanı Doğrulaması: LoadPersonelDagilimGrid'deki sorgu oldukça karmaşık. Daha basit bir doğrulama için: SELECT COUNT(*) FROM personel WHERE Durum='Aktif' AND CalismaDurumu='Kadrolu Aktif Çalışan' sorgusunun sonucu, grid'deki "Kadrolu" sütununun alt toplamıyla (FooterRow.Cells[3]) eşleşmelidir.

Senaryo 8.3: Grid Doğrulaması (Birim Dağılım)

BirimDagilimGrid'i (Birimlere göre dağılım) inceleyin.

Veritabanı Doğrulaması: SELECT GorevYaptigiBirim, COUNT(*) FROM personel WHERE Durum='Aktif' AND CalismaDurumu!='Geçici Görevde Pasif Çalışan' GROUP BY GorevYaptigiBirim sorgusunu çalıştırın ve sonuçların grid ile eşleştiğini doğrulayın.

Senaryo 8.4: Grafik Veri Doğrulaması (Gri Kutu)

Sayfayı yükleyin.

Tarayıcıda "Sayfa Kaynağını Görüntüle" (View Source) yapın.

Beklenen Sonuç: HTML içinde hdnKadroData, hdnSendikaData ve hdnEngelData adlı gizli (hidden) input'ları bulun.

Doğrulama: Bu input'ların value özniteliklerinin, LoadChartData metodundaki sorgularla (GROUP BY Unvan, GROUP BY Sendika vb.) eşleşen JSON verisi içerdiğini doğrulayın. (Grafiğin kendisinin çizilmesi JavaScript testidir, ancak verinin sunucudan doğru gönderildiğini böyle kontrol edebiliriz.)