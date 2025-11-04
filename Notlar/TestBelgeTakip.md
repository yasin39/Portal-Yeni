# BelgeTakip Modülü Manuel Test Dokümanı

## Test Ortamı Hazırlığı
- Tarayıcı: Chrome, Firefox veya Edge (son sürüm)
- Test veritabanı: ankarabolge
- Test kullanıcısı ile giriş yapılmış olmalı
- Test verileri hazırlanmış olmalı

---

## 1. YeniKayit.aspx - Yeni Firma ve Denetim Kaydı

### Test Case 1.1: Sayfa Yükleme Testi
**Amaç:** Sayfanın doğru yüklendiğini kontrol etmek

**Adımlar:**
1. Ana menüden "Yeni Kayıt" sayfasına gidin
2. Sayfanın tam olarak yüklendiğini kontrol edin

**Beklenen Sonuç:**
- Sayfa başlığı "Yeni Firma Kaydı" olarak görünmeli
- "Firma Bilgileri" ve "Denetim Bilgileri" bölümleri görünür olmalı
- Tüm alanlar boş olmalı
- İl dropdown'ı dolu olmalı
- İlçe dropdown'ı devre dışı olmalı
- Kaydet ve Temizle butonları görünür olmalı

### Test Case 1.2: Zorunlu Alan Validasyonu
**Amaç:** Zorunlu alanların kontrolünü test etmek

**Adımlar:**
1. Hiçbir alan doldurmadan "Kaydet" butonuna tıklayın
2. Her bir zorunlu alan için hata mesajlarını kontrol edin

**Beklenen Sonuç:**
- Vergi numarası için hata mesajı: "Vergi numarası zorunludur."
- Firma adı için hata mesajı: "Firma adı zorunludur."
- İl için hata mesajı: "İl seçimi zorunludur."
- İlçe için hata mesajı: "İlçe seçimi zorunludur."
- Firma tipi için hata mesajı: "Firma tipi seçimi zorunludur."
- Adres için hata mesajı: "Firma adresi zorunludur."
- Personel 1 için hata mesajı: "Personel seçimi zorunludur."
- Personel 2 için hata mesajı: "Personel seçimi zorunludur."
- Belge türü için hata mesajı: "Belge türü seçimi zorunludur."
- Kategori için hata mesajı: "Kategori seçimi zorunludur."
- Denetim tarihi için hata mesajı görünmeli

### Test Case 1.3: Vergi Numarası Validasyonu
**Amaç:** Vergi numarası formatını test etmek

**Adımlar:**
1. Vergi numarası alanına "123" girin
2. "Kaydet" butonuna tıklayın
3. Vergi numarası alanına "12345678901234" girin (12 hane)
4. "Kaydet" butonuna tıklayın
5. Vergi numarası alanına "1234567890" girin (10 hane)
6. Diğer tüm alanları doldurun
7. "Kaydet" butonuna tıklayın

**Beklenen Sonuç:**
- İlk iki durumda vergi numarası format hatası görünmeli
- Son durumda kayıt başarılı olmalı

### Test Case 1.4: İl-İlçe Bağımlılığı
**Amaç:** İl seçildiğinde ilçe dropdown'ının aktif olmasını test etmek

**Adımlar:**
1. İl dropdown'ından bir il seçin
2. İlçe dropdown'ının durumunu kontrol edin
3. İlçe dropdown'ından bir ilçe seçin

**Beklenen Sonuç:**
- İl seçilmeden önce İlçe dropdown'ı devre dışı olmalı
- İl seçildikten sonra İlçe dropdown'ı aktif olmalı
- İlçe listesi seçilen ile göre doldurulmalı

### Test Case 1.5: Personel Seçimi
**Amaç:** Aynı personelin iki kez seçilememesini test etmek

**Adımlar:**
1. Denetim Personeli 1 dropdown'ından bir personel seçin
2. Denetim Personeli 2 dropdown'ından aynı personeli seçmeye çalışın
3. "Kaydet" butonuna tıklayın

**Beklenen Sonuç:**
- Sistem aynı personelin iki kez seçilemeyeceği uyarısı vermeli

### Test Case 1.6: Kategori Seçimi ve Ceza Makbuz Alanı
**Amaç:** Kategori seçildiğinde ceza makbuz alanının aktif olmasını test etmek

**Adımlar:**
1. Kategori dropdown'ından "Ceza" seçin
2. Ceza Makbuz No alanının durumunu kontrol edin
3. Kategori dropdown'ından "Uyarı" seçin
4. Ceza Makbuz No alanının durumunu kontrol edin

**Beklenen Sonuç:**
- "Ceza" seçildiğinde Ceza Makbuz No alanı aktif olmalı ve zorunlu hale gelmeli
- Diğer kategorilerde Ceza Makbuz No alanı devre dışı olmalı

### Test Case 1.7: Denetim Tarihi Validasyonu
**Amaç:** Gelecek tarih seçilemeyeceğini test etmek

**Adımlar:**
1. Denetim Tarihi alanına tıklayın
2. Tarih seçiciden bugünden sonraki bir tarih seçmeye çalışın
3. Manuel olarak gelecek bir tarih yazmayı deneyin

**Beklenen Sonuç:**
- Takvimde gelecek tarihler seçilemez olmalı (maxDate: "today")
- Manuel giriş engellenmeli (allowInput: false)

### Test Case 1.8: Başarılı Kayıt
**Amaç:** Tüm alanlar doğru doldurulduğunda kaydın başarılı olduğunu test etmek

**Adımlar:**
1. Vergi Numarası: "1234567890"
2. Firma Adı: "Test Firması A.Ş."
3. İl: "Ankara" seçin
4. İlçe: "Çankaya" seçin
5. Firma Tipi: Bir tür seçin
6. Adres: "Test Mahallesi Test Sokak No:1"
7. Denetim Personeli 1: Bir personel seçin
8. Denetim Personeli 2: Farklı bir personel seçin
9. Belge Türü: Bir tür seçin
10. Kategori: "Uyarı" seçin
11. Denetim Tarihi: Bugünün tarihini seçin
12. "Kaydet" butonuna tıklayın

**Beklenen Sonuç:**
- Başarı mesajı görünmeli
- Form temizlenmeli
- Veritabanında kayıt oluşturulmalı

### Test Case 1.9: Temizle Butonu
**Amaç:** Temizle butonunun çalıştığını test etmek

**Adımlar:**
1. Formdaki bazı alanları doldurun
2. "Temizle" butonuna tıklayın

**Beklenen Sonuç:**
- Tüm alanlar temizlenmeli
- Dropdown'lar varsayılan değerlere dönmeli
- İlçe dropdown'ı devre dışı olmalı

### Test Case 1.10: Tekrar Kayıt Kontrolü
**Amaç:** Aynı vergi numarasının iki kez kaydedilemeyeceğini test etmek

**Adımlar:**
1. Daha önce kaydedilmiş bir vergi numarası girin
2. Diğer tüm alanları doldurun
3. "Kaydet" butonuna tıklayın

**Beklenen Sonuç:**
- "Bu vergi numarası sistemde kayıtlıdır" hata mesajı görünmeli
- Kayıt yapılmamalı

---

## 2. BelgeKayit.aspx - Firma Belge Kayıt İşlemleri

### Test Case 2.1: Sayfa Yükleme Testi
**Amaç:** Sayfanın doğru yüklendiğini kontrol etmek

**Adımlar:**
1. Ana menüden "Belge Kayıt" sayfasına gidin
2. Sayfanın tam olarak yüklendiğini kontrol edin

**Beklenen Sonuç:**
- "Firma Arama" bölümü görünür olmalı
- Belge kayıt formu (PanelBelge) gizli olmalı
- Vergi numarası alanı boş ve aktif olmalı
- "Firma Ara" butonu görünür olmalı

### Test Case 2.2: Vergi Numarası Validasyonu (Arama)
**Amaç:** Arama için vergi numarası validasyonunu test etmek

**Adımlar:**
1. Vergi numarası alanına "abc" yazın
2. "Firma Ara" butonuna tıklayın
3. Vergi numarası alanını boş bırakın
4. "Firma Ara" butonuna tıklayın
5. Vergi numarası alanına "123" yazın
6. "Firma Ara" butonuna tıklayın

**Beklenen Sonuç:**
- "abc" girişinde: "Vergi numarası 10 veya 11 haneli sayı olmalıdır" hatası
- Boş bırakıldığında: "Vergi numarası zorunludur" hatası
- "123" girişinde: "Vergi numarası 10 veya 11 haneli sayı olmalıdır" hatası

### Test Case 2.3: Firma Arama - Kayıt Bulunamadı
**Amaç:** Sistemde olmayan vergi numarasıyla arama yapıldığında sonuç kontrolü

**Adımlar:**
1. Vergi numarası alanına sistemde olmayan bir numara girin (örn: "9999999999")
2. "Firma Ara" butonuna tıklayın

**Beklenen Sonuç:**
- GridView boş görünmeli
- "Vergi numarası ile eşleşen firma bulunamadı." mesajı görünmeli

### Test Case 2.4: Firma Arama - Kayıt Bulundu
**Amaç:** Sistemde olan vergi numarasıyla arama yapıldığında sonuç kontrolü

**Adımlar:**
1. Vergi numarası alanına sistemde kayıtlı bir numara girin
2. "Firma Ara" butonuna tıklayın
3. GridView'deki sonuçları kontrol edin

**Beklenen Sonuç:**
- GridView'de firma bilgileri görünmeli
- Firma adı, vergi no, adres, belge türü ve belge durumu kolonları dolu olmalı
- Belge durumu: "✓ Belgeli" veya "✗ Belgesiz" şeklinde görünmeli
- Her satırda "Seç" butonu olmalı

### Test Case 2.5: Firma Seçimi
**Amaç:** GridView'den firma seçildiğinde belge kayıt formunun açılmasını test etmek

**Adımlar:**
1. Vergi numarasıyla firma arayın
2. Sonuçlardan belgesiz bir firmayı seçin ("Seç" butonuna tıklayın)
3. Belge kayıt formunun görünürlüğünü kontrol edin

**Beklenen Sonuç:**
- PanelBelge görünür hale gelmeli
- Firma bilgileri (Firma Adı, Vergi No, Adres, Belge Türü) görüntülenmeli
- Belge Tarihi ve Belge Numarası alanları boş ve aktif olmalı
- "Belge Kaydını Tamamla" butonu görünür olmalı

### Test Case 2.6: Belgeli Firma Seçimi
**Amaç:** Zaten belgesi olan bir firma seçildiğinde uyarı kontrolü

**Adımlar:**
1. Vergi numarasıyla firma arayın
2. Sonuçlardan belgeli bir firmayı seçin
3. Uyarı mesajını kontrol edin

**Beklenen Sonuç:**
- "Bu firma zaten belge almıştır" uyarısı görünmeli
- Belge kayıt formu açılmamalı

### Test Case 2.7: Belge Tarihi Validasyonu
**Amaç:** Belge tarihi validasyonunu test etmek

**Adımlar:**
1. Belgesiz bir firma seçin
2. Belge Tarihi alanını boş bırakın
3. Belge numaralarını doldurun
4. "Belge Kaydını Tamamla" butonuna tıklayın
5. Belge Tarihi alanına gelecek bir tarih yazmayı deneyin

**Beklenen Sonuç:**
- Boş bırakıldığında: "Belge tarihi zorunludur" hatası
- Gelecek tarih seçimi engellenmeli
- "Belge tarihi gelecek bir tarih olamaz" hatası görünmeli

### Test Case 2.8: Belge Numarası Validasyonu
**Amaç:** Belge numarası format kontrolünü test etmek

**Adımlar:**
1. Belgesiz bir firma seçin
2. İlk kısma (TxtBelge1) "1" yazın (2 hane olmalı)
3. İkinci kısma (TxtBelge2) "123" yazın (6 hane olmalı)
4. "Belge Kaydını Tamamla" butonuna tıklayın
5. İlk kısma "12" yazın
6. İkinci kısma "123456" yazın
7. "Belge Kaydını Tamamla" butonuna tıklayın

**Beklenen Sonuç:**
- İlk durumda: "İlk kısım 2 haneli sayı olmalıdır" ve "İkinci kısım 6 haneli sayı olmalıdır" hatası
- İkinci durumda: Validasyon hatası olmamalı, kayıt başarılı olmalı

### Test Case 2.9: Belge Numarası Otomatik Odaklanma
**Amaç:** İlk kısım 2 hane doldurulduğunda otomatik olarak ikinci kısma geçişi test etmek

**Adımlar:**
1. Belgesiz bir firma seçin
2. İlk kısma "12" yazın
3. Odak noktasını kontrol edin

**Beklenen Sonuç:**
- İlk kısma 2 hane girildiğinde otomatik olarak ikinci kısma odak geçmeli

### Test Case 2.10: Başarılı Belge Kaydı
**Amaç:** Tüm alanlar doğru doldurulduğunda belge kaydının başarılı olduğunu test etmek

**Adımlar:**
1. Belgesiz bir firma arayın ve seçin
2. Belge Tarihi: Bugünün tarihini seçin
3. Belge Numarası İlk Kısım: "12"
4. Belge Numarası İkinci Kısım: "345678"
5. "Belge Kaydını Tamamla" butonuna tıklayın

**Beklenen Sonuç:**
- Başarı mesajı görünmeli
- Form temizlenmeli
- Veritabanında belge kaydı güncellenmiş olmalı
- Firmayı tekrar aradığınızda "Belgeli" olarak görünmeli

### Test Case 2.11: Sayfa Yenileme (Pagination)
**Amaç:** GridView'de sayfalama çalıştığını test etmek

**Adımlar:**
1. 10'dan fazla kayıt dönen bir arama yapın
2. GridView'in altındaki sayfa numaralarını kontrol edin
3. İkinci sayfaya tıklayın

**Beklenen Sonuç:**
- Sayfalama kontrolleri görünür olmalı
- İkinci sayfaya tıklandığında ilgili kayıtlar görünmeli

### Test Case 2.12: Sadece Rakam Girişi Kontrolü
**Amaç:** Vergi no ve belge numarası alanlarına sadece rakam girilebileceğini test etmek

**Adımlar:**
1. Vergi numarası alanına "abc123" yazmayı deneyin
2. İlk belge numarası alanına "ab" yazmayı deneyin
3. İkinci belge numarası alanına "xyz" yazmayı deneyin

**Beklenen Sonuç:**
- Tüm alanlarda sadece rakamlar görünmeli
- Harf ve özel karakterler yazıldığında otomatik temizlenmeli

---

## 3. TakiptekiFirmalar.aspx - Belge Almayan Firmaların Listesi

### Test Case 3.1: Sayfa Yükleme Testi
**Amaç:** Sayfanın doğru yüklendiğini ve filtre kontrollerini test etmek

**Adımlar:**
1. Ana menüden "Takipteki Firmalar" sayfasına gidin
2. Sayfanın yüklenmesini bekleyin

**Beklenen Sonuç:**
- Sayfa başlığı "Takipteki Firmalar - Belge Takip Sistemi" olmalı
- Kayıt sayısı badge'i görünür olmalı
- Filtre bölümü (Belge Türü, İl, İlçe dropdown'ları) görünür olmalı
- GridView tüm takipteki firmaları göstermeli
- "Yenile" butonu görünür olmalı
- "Excel'e Aktar" butonu görünür olmalı

### Test Case 3.2: İlk Veri Yükleme
**Amaç:** Sayfa ilk açıldığında verilerin doğru yüklendiğini test etmek

**Adımlar:**
1. Sayfayı yeniden yükleyin
2. GridView'deki verileri kontrol edin

**Beklenen Sonuç:**
- Belge almamış (BELGE_ALDIMI = false) firmalar listelenmeli
- Belge Türü, İl, İlçe, Adres, Firma Adı, Vergi No, Denetim Tarihi kolonları dolu olmalı
- Tebliğ Tarihi, Muafiyet Durumu, Ceza Sayısı, Belge Durumu kolonları görünür olmalı
- Belge Durumu "Belge Almadı" şeklinde kırmızı renkte görünmeli

### Test Case 3.3: Belge Türü Filtresi
**Amaç:** Belge türüne göre filtrelemenin çalıştığını test etmek

**Adımlar:**
1. Belge Türü dropdown'ından "Tümü" seçin
2. Sonuçları kontrol edin
3. Belge Türü dropdown'ından belirli bir tür seçin (örn: "A2")
4. Sonuçları kontrol edin

**Beklenen Sonuç:**
- "Tümü" seçildiğinde tüm belge türleri görünmeli
- Belirli bir tür seçildiğinde sadece o türdeki firmalar görünmeli
- Kayıt sayısı badge'i güncellenmeli

### Test Case 3.4: İl Filtresi
**Amaç:** İl seçildiğinde filtreleme ve ilçe dropdown'ının güncellenmesini test etmek

**Adımlar:**
1. İl dropdown'ından "Tümü" seçin
2. İlçe dropdown'ının durumunu kontrol edin
3. İl dropdown'ından "Ankara" seçin
4. İlçe dropdown'ının durumunu ve içeriğini kontrol edin
5. GridView sonuçlarını kontrol edin

**Beklenen Sonuç:**
- "Tümü" seçildiğinde tüm iller görünmeli, ilçe dropdown'ı "Tümü" göstermeli
- "Ankara" seçildiğinde ilçe dropdown'ı Ankara'nın ilçeleriyle dolmalı
- GridView sadece Ankara'daki firmaları göstermeli
- Kayıt sayısı güncellenmeli

### Test Case 3.5: İlçe Filtresi
**Amaç:** İlçe seçildiğinde filtrelemenin çalıştığını test etmek

**Adımlar:**
1. İl dropdown'ından bir il seçin
2. İlçe dropdown'ından bir ilçe seçin
3. GridView sonuçlarını kontrol edin

**Beklenen Sonuç:**
- Sadece seçilen il ve ilçedeki firmalar görünmeli
- Kayıt sayısı güncellenmeli

### Test Case 3.6: Kombine Filtreler
**Amaç:** Birden fazla filtrenin birlikte çalıştığını test etmek

**Adımlar:**
1. Belge Türü: "A2" seçin
2. İl: "Ankara" seçin
3. İlçe: "Çankaya" seçin
4. Sonuçları kontrol edin

**Beklenen Sonuç:**
- Sadece A2 belge türünde, Ankara-Çankaya'daki firmalar görünmeli
- Kayıt sayısı doğru hesaplanmalı

### Test Case 3.7: Yenile Butonu
**Amaç:** Yenile butonunun tüm filtreleri sıfırlayıp listeyi yenilemesini test etmek

**Adımlar:**
1. Filtrelerden bazılarını uygulayın
2. "Yenile" butonuna tıklayın

**Beklenen Sonuç:**
- Tüm filtreler "Tümü" değerine dönmeli
- GridView tüm takipteki firmaları göstermeli
- Kayıt sayısı güncellenmeli

### Test Case 3.8: Sıralama (Sorting)
**Amaç:** Kolon başlıklarına tıklandığında sıralamanın çalıştığını test etmek

**Adımlar:**
1. "Firma Adı" kolon başlığına tıklayın
2. Sıralamayı kontrol edin
3. Tekrar aynı başlığa tıklayın (azalan sıralama için)
4. "Denetim Tarihi" kolon başlığına tıklayın

**Beklenen Sonuç:**
- İlk tıklamada A-Z sıralaması yapılmalı
- İkinci tıklamada Z-A sıralaması yapılmalı
- Tarih sıralaması doğru çalışmalı (en eski/en yeni)

### Test Case 3.9: Tebliğ Tarihi Gösterimi
**Amaç:** Tebliğ tarihinin doğru hesaplanıp gösterildiğini test etmek

**Adımlar:**
1. GridView'deki kayıtları inceleyin
2. Tebliğ Tarihi kolonunu kontrol edin

**Beklenen Sonuç:**
- Ceza verilen firmalar için tebliğ tarihi görünmeli
- Hesaplama: Denetim Tarihi + 60 gün
- Tarih formatı: gg.aa.yyyy

### Test Case 3.10: Muafiyet Durumu Gösterimi
**Amaç:** Muafiyet durumunun doğru hesaplanıp gösterildiğini test etmek

**Adımlar:**
1. GridView'deki kayıtları inceleyin
2. Muafiyet Durumu kolonunu kontrol edin

**Beklenen Sonuç:**
- Tebliğ tarihinden 60 gün geçmişse "Muafiyet Süresi Dolmuş" (kırmızı)
- Henüz geçmemişse "Muafiyet Süresi Var" (yeşil)
- Ceza verilmemişse boş veya "-" gösterilmeli

### Test Case 3.11: Ceza Sayısı Gösterimi
**Amaç:** Firma için verilen toplam ceza sayısının doğru gösterildiğini test etmek

**Adımlar:**
1. GridView'deki kayıtları inceleyin
2. Ceza Sayısı kolonunu kontrol edin

**Beklenen Sonuç:**
- Her firma için toplam ceza sayısı kırmızı badge içinde görünmeli
- Sayı doğru hesaplanmalı (kategori="Ceza" olan kayıtların toplamı)

### Test Case 3.12: Belge Durumu Gösterimi
**Amaç:** Belge durumunun doğru gösterildiğini test etmek

**Adımlar:**
1. GridView'deki kayıtları inceleyin
2. Belge Durumu kolonunu kontrol edin

**Beklenen Sonuç:**
- Tüm kayıtlar "Belge Almadı" olarak gösterilmeli (kırmızı, X icon)
- Çünkü bu sayfa sadece belge almayan firmaları gösteriyor

### Test Case 3.13: Excel'e Aktarma
**Amaç:** Excel export işlevinin çalıştığını test etmek

**Adımlar:**
1. Filtre uygulamadan "Excel'e Aktar" butonuna tıklayın
2. İndirilen dosyayı açın
3. Filtre uygulayın ve tekrar "Excel'e Aktar" butonuna tıklayın

**Beklenen Sonuç:**
- Excel dosyası indirilmeli
- Dosya adı: "TakiptekiFirmalar_GGAAYYYY_SSDD.xlsx" formatında olmalı
- İçerikte GridView'deki tüm kolonlar ve veriler olmalı
- Filtre uygulandıysa sadece filtrelenmiş veriler export edilmeli

### Test Case 3.14: Veri Olmadığında Görünüm
**Amaç:** Hiç takipteki firma olmadığında gösterimin doğru olduğunu test etmek

**Adımlar:**
1. Tüm firmaların belgelerini tamamlayın (test ortamında)
2. Sayfayı yenileyin

**Beklenen Sonuç:**
- GridView boş olmalı
- "Görüntülenecek kayıt bulunamadı." mesajı görünmeli
- Excel'e Aktar butonu görünür olmalı (boş export yapabilir)

### Test Case 3.15: RowDataBound Olayı
**Amaç:** Her satırın doğru şekilde formatlandığını test etmek

**Adımlar:**
1. Farklı durumları içeren kayıtları inceleyin
2. Renklendirme ve formatlama kontrolü yapın

**Beklenen Sonuç:**
- Muafiyet süresi dolmuş kayıtlar farklı renkte olabilir
- Tarihler doğru formatlanmalı
- Badge'ler doğru renklerde olmalı

---

## 4. TumFirmalar.aspx - Tüm Firmaların Listesi

### Test Case 4.1: Sayfa Yükleme Testi
**Amaç:** Sayfanın doğru yüklendiğini test etmek

**Adımlar:**
1. Ana menüden "Tüm Firmalar" sayfasına gidin
2. Sayfa içeriğini kontrol edin

**Beklenen Sonuç:**
- Sayfa başlığı "Firma Listesi" olmalı
- Kayıt sayısı badge'i görünür olmalı
- Filtre bölümü (Vergi No, Belge Türü, İl, İlçe) görünür olmalı
- "Ara" ve "Tümünü Listele" butonları görünür olmalı
- GridView tüm firmaları göstermeli
- "Excel'e Aktar" butonu görünür olmalı

### Test Case 4.2: İlk Veri Yükleme
**Amaç:** Sayfa ilk açıldığında tüm firmaların listelendiğini test etmek

**Adımlar:**
1. Sayfayı yeniden yükleyin
2. GridView'deki verileri kontrol edin

**Beklenen Sonuç:**
- Hem belgeli hem belgesiz tüm firmalar listelenmeli
- Vergi No, Firma Adı, Belge Türü, İl, İlçe, Adres kolonları dolu olmalı
- Denetim Tarihi formatı: gg.aa.yyyy
- Belge Durumu: "Belgeli" (yeşil) veya "Belgesiz" (kırmızı)
- "Detay" butonu her satırda görünür olmalı

### Test Case 4.3: Vergi Numarası ile Arama
**Amaç:** Vergi numarası alanıyla arama yapıldığını test etmek

**Adımlar:**
1. Vergi No alanına tam bir vergi numarası girin
2. "Ara" butonuna tıklayın
3. Vergi No alanına kısmi bir numara girin (örn: "123")
4. "Ara" butonuna tıklayın

**Beklenen Sonuç:**
- Tam numara ile arama yapıldığında o firma görünmeli
- Kısmi numara ile arama yapıldığında içinde o rakamları geçen tüm firmalar görünmeli
- Kayıt sayısı güncellenmeli

### Test Case 4.4: Vergi Numarası Validasyonu
**Amaç:** Vergi numarası alanına sadece sayı girilebileceğini test etmek

**Adımlar:**
1. Vergi No alanına "abc" yazmayı deneyin
2. Vergi No alanına "123abc456" yazmayı deneyin

**Beklenen Sonuç:**
- "Sadece sayısal değer giriniz" hata mesajı görünmeli
- Arama yapılmamalı

### Test Case 4.5: Belge Türü Filtresi
**Amaç:** Belge türü filtresinin çalıştığını test etmek

**Adımlar:**
1. Belge Türü dropdown'ından "Tümü" seçin
2. "Ara" butonuna tıklayın
3. Belge Türü dropdown'ından "A2" seçin
4. "Ara" butonuna tıklayın

**Beklenen Sonuç:**
- "Tümü" ile tüm belge türleri görünmeli
- "A2" ile sadece A2 belge türündeki firmalar görünmeli

### Test Case 4.6: İl-İlçe Filtresi
**Amaç:** İl seçildiğinde ilçe dropdown'ının güncellenmesini ve filtrelemenin çalışmasını test etmek

**Adımlar:**
1. İl dropdown'ından "Ankara" seçin
2. İlçe dropdown'ını kontrol edin
3. İlçe dropdown'ından "Çankaya" seçin
4. "Ara" butonuna tıklayın

**Beklenen Sonuç:**
- İl seçildiğinde ilçe dropdown'ı o ilin ilçeleriyle dolmalı
- Sadece Ankara-Çankaya'daki firmalar görünmeli
- Kayıt sayısı güncellenmeli

### Test Case 4.7: Kombine Filtreler
**Amaç:** Birden fazla filtrenin birlikte çalıştığını test etmek

**Adımlar:**
1. Vergi No: "12" girin
2. Belge Türü: "A2" seçin
3. İl: "Ankara" seçin
4. İlçe: "Çankaya" seçin
5. "Ara" butonuna tıklayın

**Beklenen Sonuç:**
- Tüm kriterleri sağlayan firmalar görünmeli
- Kayıt sayısı doğru olmalı

### Test Case 4.8: Tümünü Listele Butonu
**Amaç:** Tümünü Listele butonunun tüm filtreleri temizleyip listeyi sıfırlamasını test etmek

**Adımlar:**
1. Çeşitli filtreler uygulayın
2. "Tümünü Listele" butonuna tıklayın

**Beklenen Sonuç:**
- Tüm filtre alanları temizlenmeli
- Dropdown'lar "Tümü" değerine dönmeli
- GridView tüm firmaları göstermeli

### Test Case 4.9: Pagination (Sayfalama)
**Amaç:** Sayfalama kontrollerinin çalıştığını test etmek

**Adımlar:**
1. Tüm firmaları listeleyin (10'dan fazla kayıt olmalı)
2. GridView altındaki sayfa numaralarını kontrol edin
3. Sayfa 2'ye tıklayın
4. Sayfa 1'e geri dönün

**Beklenen Sonuç:**
- Her sayfada 10 kayıt görünmeli
- Sayfa geçişleri düzgün çalışmalı
- Filtreler uygulandıktan sonra da pagination çalışmalı

### Test Case 4.10: Sorting (Sıralama)
**Amaç:** Kolon başlıklarına tıklandığında sıralamanın çalıştığını test etmek

**Adımlar:**
1. "Firma Adı" başlığına tıklayın
2. Tekrar tıklayın
3. "Denetim Tarihi" başlığına tıklayın
4. "Belge Durumu" başlığına tıklayın

**Beklenen Sonuç:**
- İlk tıklamada artan sıralama
- İkinci tıklamada azalan sıralama
- Tarih sıralaması doğru çalışmalı
- Belge durumu sıralaması (belgeli/belgesiz) çalışmalı

### Test Case 4.11: Detay Butonu
**Amaç:** Detay butonunun FirmaDetay sayfasına yönlendirdiğini test etmek

**Adımlar:**
1. Bir firmanın satırındaki "Detay" butonuna tıklayın
2. Açılan sayfayı kontrol edin

**Beklenen Sonuç:**
- FirmaDetay.aspx sayfası açılmalı
- Firma ID parametresi URL'de görünmeli (örn: FirmaDetay.aspx?id=123)
- Firma bilgileri doğru şekilde yüklenmiş olmalı

### Test Case 4.12: Excel'e Aktarma
**Amaç:** Excel export işlevinin çalıştığını test etmek

**Adımlar:**
1. Filtre uygulamadan "Excel'e Aktar" butonuna tıklayın
2. İndirilen dosyayı açın
3. Filtre uygulayın ve tekrar export edin

**Beklenen Sonuç:**
- Excel dosyası indirilmeli
- Dosya adı: "TumFirmalar_GGAAYYYY_SSDD.xlsx" formatında
- Tüm kolonlar ve veriler export edilmiş olmalı
- Filtre uygulandıysa sadece filtrelenmiş veriler export edilmeli

### Test Case 4.13: Kayıt Sayısı Badge
**Amaç:** Kayıt sayısının doğru hesaplanıp gösterildiğini test etmek

**Adımlar:**
1. Tüm firmaları listeleyin
2. Kayıt sayısı badge'ini kontrol edin
3. Filtre uygulayın
4. Kayıt sayısını tekrar kontrol edin

**Beklenen Sonuç:**
- Toplam kayıt sayısı badge'de görünmeli
- Filtre uygulandığında kayıt sayısı güncellenmeli

### Test Case 4.14: Boş Sonuç
**Amaç:** Hiç sonuç döndürmeyen bir arama yapıldığında görünüm kontrolü

**Adımlar:**
1. Vergi No alanına sistemde olmayan bir numara girin
2. "Ara" butonuna tıklayın

**Beklenen Sonuç:**
- GridView boş olmalı
- "Görüntülenecek kayıt bulunamadı." mesajı görünmeli
- Kayıt sayısı: 0

### Test Case 4.15: Belge Durumu İkonları
**Amaç:** Belge durumu gösteriminin doğru olduğunu test etmek

**Adımlar:**
1. GridView'deki Belge Durumu kolonunu inceleyin
2. Hem belgeli hem belgesiz firmaları kontrol edin

**Beklenen Sonuç:**
- Belgeli firmalar: Yeşil renkle, "✓" ikonu ve "Belgeli" yazısı
- Belgesiz firmalar: Kırmızı renkle, "✗" ikonu ve "Belgesiz" yazısı

---

## 5. FirmaDetay.aspx - Firma Detay Görüntüleme ve Düzenleme

### Test Case 5.1: Sayfa Yükleme - Geçerli ID ile
**Amaç:** Geçerli bir firma ID'si ile sayfanın doğru yüklendiğini test etmek

**Adımlar:**
1. TumFirmalar sayfasından bir firmayı seçip Detay'a tıklayın
2. veya URL'e doğrudan FirmaDetay.aspx?id=X yazın (X: geçerli bir firma ID)
3. Sayfa içeriğini kontrol edin

**Beklenen Sonuç:**
- Sayfa başlığı "Firma Detay Bilgileri" olmalı
- "Geri Dön" butonu görünür olmalı
- Firma bilgileri tablosu dolu olmalı (Firma Adı, Vergi No, İl, İlçe, Adres, Belge Türü, Belge Durumu)
- Belge Durumu: "Belgeli" (yeşil) veya "Belgesiz" (kırmızı) şeklinde
- Denetimler bölümü görünür olmalı
- "Yeni Denetim Ekle" butonu görünür olmalı

### Test Case 5.2: Sayfa Yükleme - Geçersiz ID ile
**Amaç:** Geçersiz veya eksik ID ile sayfaya erişildiğinde hata kontrolü

**Adımlar:**
1. URL'e FirmaDetay.aspx (id parametresi olmadan) yazın
2. FirmaDetay.aspx?id=abc yazın (sayısal olmayan değer)
3. FirmaDetay.aspx?id=99999 yazın (sistemde olmayan id)

**Beklenen Sonuç:**
- Hata mesajı görünmeli: "Geçersiz firma bilgisi" veya benzeri
- Firma detay bilgileri görüntülenmemeli
- Kullanıcı liste sayfasına yönlendirilmeli veya hata paneli görüntülenmeli

### Test Case 5.3: Firma Bilgilerinin Görüntülenmesi
**Amaç:** Firma bilgilerinin doğru ve tam olarak görüntülendiğini test etmek

**Adımlar:**
1. Geçerli bir firma detayına girin
2. Firma bilgileri tablosunu inceleyin

**Beklenen Sonuç:**
- Firma Adı: Dolu ve doğru
- Vergi Numarası: Dolu ve doğru
- İl: Dolu ve doğru
- İlçe: Dolu ve doğru
- Adres: Dolu ve doğru
- Belge Türü: Dolu ve doğru
- Belge Durumu: "Belgeli" veya "Belgesiz" (stil ile birlikte)
- Tüm bilgiler veritabanı ile eşleşmeli

### Test Case 5.4: Denetim Geçmişi GridView
**Amaç:** Firma için yapılmış denetimlerin doğru listelendiğini test etmek

**Adımlar:**
1. Denetimleri olan bir firma detayına girin
2. Denetimler GridView'ini kontrol edin

**Beklenen Sonuç:**
- Tüm denetimler kronolojik sırada listelenmeli
- Denetim Tarihi, Personel 1, Personel 2, Kategori, Ceza Makbuz No (varsa) kolonları dolu olmalı
- Tarih formatı: gg.aa.yyyy
- Her satırda "Düzenle" ve "Sil" butonları görünür olmalı

### Test Case 5.5: Denetim Olmayan Firma
**Amaç:** Henüz denetimi olmayan firma için görünüm kontrolü

**Adımlar:**
1. Yeni eklenmiş ve denetimi olmayan bir firma detayına girin
2. Denetimler bölümünü kontrol edin

**Beklenen Sonuç:**
- GridView boş olmalı
- "Bu firma için henüz denetim kaydı bulunmamaktadır." mesajı görünmeli
- "Yeni Denetim Ekle" butonu yine görünür olmalı

### Test Case 5.6: Yeni Denetim Ekle Butonu
**Amaç:** Yeni denetim ekleme formunun açılmasını test etmek

**Adımlar:**
1. Firma detay sayfasında "Yeni Denetim Ekle" butonuna tıklayın
2. Açılan panel/form'u kontrol edin

**Beklenen Sonuç:**
- Yeni denetim formu görünür hale gelmeli
- Personel 1, Personel 2 dropdown'ları dolu olmalı
- Kategori dropdown'ı dolu olmalı
- Denetim Tarihi alanı boş olmalı
- Ceza Makbuz No alanı devre dışı olmalı
- "Denetim Kaydet" butonu görünür olmalı
- "İptal" butonu görünür olmalı

### Test Case 5.7: Yeni Denetim Ekleme - Validasyon
**Amaç:** Yeni denetim eklerken zorunlu alan kontrollerini test etmek

**Adımlar:**
1. "Yeni Denetim Ekle" butonuna tıklayın
2. Hiçbir alan doldurmadan "Denetim Kaydet" butonuna tıklayın

**Beklenen Sonuç:**
- Personel 1 seçimi zorunlu hatası
- Personel 2 seçimi zorunlu hatası
- Kategori seçimi zorunlu hatası
- Denetim tarihi zorunlu hatası
- Form submit edilmemeli

### Test Case 5.8: Yeni Denetim Ekleme - Aynı Personel
**Amaç:** Aynı personelin iki kez seçilememesini test etmek

**Adımlar:**
1. "Yeni Denetim Ekle" butonuna tıklayın
2. Personel 1'i seçin
3. Personel 2'yi aynı personel olarak seçin
4. Diğer alanları doldurun
5. "Denetim Kaydet" butonuna tıklayın

**Beklenen Sonuç:**
- "Aynı personel iki kez seçilemez" hata mesajı görünmeli
- Kayıt yapılmamalı

### Test Case 5.9: Yeni Denetim Ekleme - Ceza Kategorisi
**Amaç:** Ceza kategorisi seçildiğinde makbuz no alanının aktif olmasını test etmek

**Adımlar:**
1. "Yeni Denetim Ekle" butonuna tıklayın
2. Kategori olarak "Ceza" seçin
3. Ceza Makbuz No alanının durumunu kontrol edin
4. Kategori olarak "Uyarı" seçin
5. Ceza Makbuz No alanının durumunu kontrol edin

**Beklenen Sonuç:**
- "Ceza" seçildiğinde Ceza Makbuz No alanı aktif ve zorunlu olmalı
- Diğer kategorilerde Ceza Makbuz No alanı devre dışı olmalı

### Test Case 5.10: Yeni Denetim Ekleme - Başarılı Kayıt
**Amaç:** Tüm alanlar doğru doldurulduğunda denetim kaydının başarılı olduğunu test etmek

**Adımlar:**
1. "Yeni Denetim Ekle" butonuna tıklayın
2. Personel 1: Bir personel seçin
3. Personel 2: Farklı bir personel seçin
4. Kategori: "Uyarı" seçin
5. Denetim Tarihi: Bugünü seçin
6. "Denetim Kaydet" butonuna tıklayın

**Beklenen Sonuç:**
- Başarı mesajı görünmeli
- Denetim formu kapanmalı
- GridView yenilenip yeni denetim görünmeli
- Kayıt veritabanına eklenmiş olmalı

### Test Case 5.11: Denetim Düzenleme
**Amaç:** Mevcut denetim kaydının düzenlenebilmesini test etmek

**Adımlar:**
1. Denetimler GridView'inde bir denetimin "Düzenle" butonuna tıklayın
2. Açılan düzenleme formunu kontrol edin
3. Alanları değiştirin
4. "Güncelle" butonuna tıklayın

**Beklenen Sonuç:**
- Düzenleme formu mevcut değerlerle dolu olmalı
- Değişiklikler yapılabilmeli
- Güncelleme sonrası başarı mesajı görünmeli
- GridView güncellenmiş değerleri göstermeli

### Test Case 5.12: Denetim Silme
**Amaç:** Denetim kaydının silinebilmesini test etmek

**Adımlar:**
1. Denetimler GridView'inde bir denetimin "Sil" butonuna tıklayın
2. Onay mesajını kontrol edin (varsa)
3. Silme işlemini onaylayın

**Beklenen Sonuç:**
- JavaScript onay kutusu görünmeli: "Bu denetim kaydını silmek istediğinizden emin misiniz?"
- Onaylandığında kayıt silinmeli
- GridView güncellenmeli
- Başarı mesajı görünmeli

### Test Case 5.13: Denetim Düzenleme - Validasyon
**Amaç:** Düzenleme sırasında validasyonların çalıştığını test etmek

**Adımlar:**
1. Bir denetimi düzenlemeye başlayın
2. Zorunlu alanları boş bırakın
3. "Güncelle" butonuna tıklayın

**Beklenen Sonuç:**
- Validasyon hataları görünmeli
- Güncelleme yapılmamalı

### Test Case 5.14: Geri Dön Butonu
**Amaç:** Geri Dön butonunun çalıştığını test etmek

**Adımlar:**
1. Firma detay sayfasında "Geri Dön" butonuna tıklayın

**Beklenen Sonuç:**
- TumFirmalar.aspx sayfasına yönlendirilmeli
- Önceki filtreler (varsa) korunmalı

### Test Case 5.15: Belge Bilgileri Güncelleme
**Amaç:** Firma için belge kaydı yapıldığında detay sayfasının güncellenmesini test etmek

**Adımlar:**
1. Belgesiz bir firmanın detay sayfasına gidin
2. Belge Durumu'nu kontrol edin
3. BelgeKayit sayfasından bu firmaya belge kaydedin
4. Detay sayfasını yenileyin

**Beklenen Sonuç:**
- İlk durumda "Belgesiz" (kırmızı) görünmeli
- Belge kaydından sonra "Belgeli" (yeşil) görünmeli

---

## 6. Teblig.aspx - Tebliğ Tarihi Kayıt İşlemleri

### Test Case 6.1: Sayfa Yükleme Testi
**Amaç:** Sayfanın doğru yüklendiğini test etmek

**Adımlar:**
1. Ana menüden "Tebliğ" sayfasına gidin
2. Sayfa içeriğini kontrol edin

**Beklenen Sonuç:**
- Sayfa başlığı "Tebliğ Tarihi Kayıt" olmalı
- Firma arama bölümü (Vergi No alanı) görünür olmalı
- "Firma Getir" butonu görünür olmalı
- Tebliğ kayıt formu gizli olmalı (firma seçilene kadar)

### Test Case 6.2: Vergi Numarası Validasyonu
**Amaç:** Vergi numarası girişi için validasyon kontrolü

**Adımlar:**
1. Vergi No alanını boş bırakın ve "Firma Getir" butonuna tıklayın
2. Vergi No alanına "abc" yazın ve "Firma Getir" butonuna tıklayın
3. Vergi No alanına "123" yazın (10 haneden az) ve "Firma Getir" butonuna tıklayın

**Beklenen Sonuç:**
- Boş bırakıldığında: "Vergi numarası zorunludur" hatası
- Harf girildiğinde: "Vergi numarası sadece rakamlardan oluşmalıdır" hatası
- Eksik hane: "Vergi numarası 10 veya 11 haneli olmalıdır" hatası

### Test Case 6.3: Firma Getirme - Kayıt Bulunamadı
**Amaç:** Sistemde olmayan vergi numarasıyla arama yapıldığında kontrol

**Adımlar:**
1. Vergi No alanına sistemde olmayan bir numara girin (örn: "9999999999")
2. "Firma Getir" butonuna tıklayın

**Beklenen Sonuç:**
- Hata mesajı: "Bu vergi numarasına ait firma bulunamadı" veya benzeri
- Tebliğ kayıt formu açılmamalı

### Test Case 6.4: Firma Getirme - Başarılı
**Amaç:** Sistemde olan firma için bilgilerin yüklenmesini test etmek

**Adımlar:**
1. Vergi No alanına sistemde kayıtlı ceza almış bir firmanın numarasını girin
2. "Firma Getir" butonuna tıklayın
3. Görüntülenen bilgileri kontrol edin

**Beklenen Sonuç:**
- Firma bilgileri (Firma Adı, Vergi No, Adres) info card içinde görünmeli
- Denetim bilgileri (Denetim Tarihi, Personeller, Kategori) görünmeli
- Eğer kategori "Ceza" ise Ceza Makbuz No görünmeli
- Tebliğ tarihi kayıt formu görünür hale gelmeli
- Tebliğ Tarihi alanı boş olmalı
- "Tebliğ Tarihini Kaydet" butonu görünür olmalı

### Test Case 6.5: Ceza Almamış Firma Kontrolü
**Amaç:** Ceza almamış firmaya tebliğ tarihi kaydedilemeyeceğini test etmek

**Adımlar:**
1. Kategori'si "Uyarı" veya "Bilgilendirme" olan bir firmanın vergi numarasını girin
2. "Firma Getir" butonuna tıklayın

**Beklenen Sonuç:**
- Uyarı mesajı: "Bu firma için ceza kaydı bulunmamaktadır, tebliğ tarihi kaydedilemez" veya benzeri
- Tebliğ kayıt formu açılmamalı

### Test Case 6.6: Tebliğ Tarihi Validasyonu
**Amaç:** Tebliğ tarihi alanı için validasyon kontrolü

**Adımlar:**
1. Ceza almış bir firma getirin
2. Tebliğ Tarihi alanını boş bırakın
3. "Tebliğ Tarihini Kaydet" butonuna tıklayın

**Beklenen Sonuç:**
- "Tebliğ tarihi zorunludur" hata mesajı görünmeli
- Kayıt yapılmamalı

### Test Case 6.7: Geçersiz Tarih Kontrolü
**Amaç:** Denetim tarihinden önce bir tebliğ tarihi girilemeyeceğini test etmek

**Adımlar:**
1. Ceza almış bir firma getirin
2. Denetim tarihini kontrol edin
3. Tebliğ Tarihi olarak denetim tarihinden önceki bir tarih seçin
4. "Tebliğ Tarihini Kaydet" butonuna tıklayın

**Beklenen Sonuç:**
- "Tebliğ tarihi, denetim tarihinden önce olamaz" hata mesajı görünmeli
- Kayıt yapılmamalı

### Test Case 6.8: Gelecek Tarih Kontrolü
**Amaç:** Gelecek tarih seçilemeyeceğini test etmek

**Adımlar:**
1. Ceza almış bir firma getirin
2. Tebliğ Tarihi alanına tıklayın
3. Gelecek bir tarih seçmeye çalışın

**Beklenen Sonuç:**
- Takvimde gelecek tarihler seçilemez olmalı (maxDate: "today")
- Manuel giriş engellenmeli

### Test Case 6.9: Başarılı Tebliğ Tarihi Kaydı
**Amaç:** Doğru tarih girildiğinde kaydın başarılı olduğunu test etmek

**Adımlar:**
1. Ceza almış bir firma getirin
2. Denetim tarihini kontrol edin (örn: 01.01.2025)
3. Tebliğ Tarihi olarak denetim tarihinden sonraki bir tarih seçin (örn: 15.01.2025)
4. "Tebliğ Tarihini Kaydet" butonuna tıklayın

**Beklenen Sonuç:**
- Başarı mesajı: "Tebliğ tarihi başarıyla kaydedildi" veya benzeri
- Form temizlenmeli
- Veritabanında SONCEZA_TEBLIG_TARIHI alanı güncellenmiş olmalı

### Test Case 6.10: Mevcut Tebliğ Tarihi Güncellemesi
**Amaç:** Daha önce tebliğ tarihi kaydedilmiş firmaya yeni tarih kaydı

**Adımlar:**
1. Tebliğ tarihi daha önce kaydedilmiş bir firma getirin
2. Mevcut tebliğ tarihini kontrol edin (görünür mü?)
3. Yeni bir tebliğ tarihi girin
4. "Tebliğ Tarihini Kaydet" butonuna tıklayın

**Beklenen Sonuç:**
- Mevcut tebliğ tarihi varsa gösterilmeli
- "Bu firma için daha önce tebliğ tarihi kaydedilmiştir, güncellemek istiyor musunuz?" onay mesajı görünmeli (isteğe bağlı)
- Onaylandığında tarih güncellenmiş olmalı

### Test Case 6.11: Tarih Formatı Kontrolü
**Amaç:** Tebliğ tarihinin doğru formatta kaydedildiğini test etmek

**Adımlar:**
1. Ceza almış bir firma getirin
2. Flatpickr ile tarih seçin
3. Seçilen tarihin formatını kontrol edin
4. Kaydedin

**Beklenen Sonuç:**
- Tarih formatı: gg.aa.yyyy
- Veritabanında doğru datetime formatında kaydedilmeli

### Test Case 6.12: Firma Değiştirme
**Amaç:** Farklı firmalar için ardışık işlem yapılabildiğini test etmek

**Adımlar:**
1. İlk firmayı getirin ve tebliğ tarihini kaydedin
2. Vergi No alanına farklı bir firma numarası girin
3. "Firma Getir" butonuna tıklayın
4. İkinci firma için tebliğ tarihini kaydedin

**Beklenen Sonuç:**
- Her firma için form temiz bir şekilde yüklenmeli
- Önceki firma bilgileri temizlenmiş olmalı
- Her kayıt bağımsız olarak başarılı olmalı

### Test Case 6.13: Flatpickr Türkçe Lokalizasyon
**Amaç:** Tarih seçicinin Türkçe olduğunu test etmek

**Adımlar:**
1. Tebliğ Tarihi alanına tıklayın
2. Açılan takvimi inceleyin

**Beklenen Sonuç:**
- Ay isimleri Türkçe olmalı (Ocak, Şubat, vb.)
- Gün isimleri Türkçe olmalı (Pzt, Sal, vb.)
- Tarih formatı: gg.aa.yyyy

### Test Case 6.14: Form Temizleme
**Amaç:** İptal veya temizleme işlevinin çalıştığını test etmek (varsa)

**Adımlar:**
1. Bir firma getirin
2. "İptal" veya "Temizle" butonuna tıklayın (varsa)

**Beklenen Sonuç:**
- Form temizlenmeli
- Firma bilgileri gizlenmeli
- Vergi No alanı boşaltılmalı

### Test Case 6.15: Çoklu Denetim Kontrolü
**Amaç:** Firmada birden fazla denetim varsa en son ceza kaydı için işlem yapılması

**Adımlar:**
1. Birden fazla denetimi olan ve son denetimi "Ceza" olan bir firma getirin
2. Görüntülenen denetim bilgilerini kontrol edin

**Beklenen Sonuç:**
- En son denetim bilgileri görünmeli
- Son denetim ceza ise tebliğ formu açılmalı
- Son denetim ceza değilse uyarı verilmeli

---

## 7. SonrakiCeza.aspx - Sonraki Ceza Kayıt İşlemleri

### Test Case 7.1: Sayfa Yükleme Testi
**Amaç:** Sayfanın doğru yüklendiğini test etmek

**Adımlar:**
1. Ana menüden "Sonraki Ceza" sayfasına gidin
2. Sayfa içeriğini kontrol edin

**Beklenen Sonuç:**
- Sayfa başlığı "Sonraki Ceza Kaydı" olmalı
- Firma arama bölümü (Vergi No alanı) görünür olmalı
- "Firma Ara" butonu görünür olmalı
- Sonraki ceza kayıt formu gizli olmalı

### Test Case 7.2: Vergi Numarası ile Firma Arama
**Amaç:** Vergi numarasıyla firma arama işlevini test etmek

**Adımlar:**
1. Vergi No alanına geçerli bir vergi numarası girin
2. "Firma Ara" butonuna tıklayın

**Beklenen Sonuç:**
- Firma bilgileri görüntülenmeli
- GridView firma için yapılmış önceki denetimler listelenmiş olmalı
- Sonraki ceza kayıt formu görünür hale gelmeli

### Test Case 7.3: Önceki Denetimler GridView
**Amaç:** Firma için yapılmış denetimlerin doğru listelendiğini test etmek

**Adımlar:**
1. Birden fazla denetimi olan bir firma arayın
2. GridView'i inceleyin

**Beklenen Sonuç:**
- Tüm önceki denetimler kronolojik sırada listelenmeli
- Denetim Tarihi, Personeller, Kategori, Ceza Makbuz No kolonları dolu olmalı
- Tarih formatı: gg.aa.yyyy

### Test Case 7.4: İlk Denetimi Olmayan Firma
**Amaç:** Hiç denetimi olmayan firmaya sonraki ceza kaydedilemeyeceğini test etmek

**Adımlar:**
1. Henüz denetimi yapılmamış bir firmanın vergi numarasını girin
2. "Firma Ara" butonuna tıklayın

**Beklenen Sonuç:**
- Uyarı mesajı: "Bu firma için önceki denetim kaydı bulunmamaktadır" veya benzeri
- Sonraki ceza formu açılmamalı
- "Önce ilk denetimi YeniKayit sayfasından yapın" önerisi verilmeli

### Test Case 7.5: Yeni Ceza Kayıt Formu Açılması
**Amaç:** Firma seçildiğinde form alanlarının doğru göründüğünü test etmek

**Adımlar:**
1. Denetimi olan bir firma arayın
2. Açılan formu inceleyin

**Beklenen Sonuç:**
- Firma bilgileri (Firma Adı, Vergi No, Adres) görüntülenmeli
- Personel 1 ve Personel 2 dropdown'ları dolu olmalı
- Kategori dropdown'ı dolu olmalı
- Denetim Tarihi alanı boş olmalı
- Ceza Makbuz No alanı devre dışı olmalı
- "Sonraki Ceza Kaydet" butonu görünür olmalı

### Test Case 7.6: Zorunlu Alan Validasyonu
**Amaç:** Zorunlu alanların kontrolünü test etmek

**Adımlar:**
1. Bir firma arayın ve formu açın
2. Hiçbir alan doldurmadan "Sonraki Ceza Kaydet" butonuna tıklayın

**Beklenen Sonuç:**
- Personel 1 zorunlu hatası
- Personel 2 zorunlu hatası
- Kategori zorunlu hatası
- Denetim tarihi zorunlu hatası
- Kayıt yapılmamalı

### Test Case 7.7: Aynı Personel Kontrolü
**Amaç:** Aynı personelin iki kez seçilememesini test etmek

**Adımlar:**
1. Forma geçerli değerler girin
2. Personel 1 ve Personel 2'yi aynı seçin
3. "Sonraki Ceza Kaydet" butonuna tıklayın

**Beklenen Sonuç:**
- "Aynı personel iki kez seçilemez" hata mesajı görünmeli
- Kayıt yapılmamalı

### Test Case 7.8: Kategori ve Ceza Makbuz Kontrolü
**Amaç:** Kategori "Ceza" seçildiğinde makbuz no alanının aktif olmasını test etmek

**Adımlar:**
1. Kategori dropdown'ından "Ceza" seçin
2. Ceza Makbuz No alanının durumunu kontrol edin
3. Kategori dropdown'ından "Uyarı" seçin
4. Ceza Makbuz No alanının durumunu kontrol edin

**Beklenen Sonuç:**
- "Ceza" seçildiğinde Ceza Makbuz No alanı aktif ve zorunlu olmalı
- Diğer kategorilerde devre dışı olmalı

### Test Case 7.9: Denetim Tarihi Kontrolü
**Amaç:** Denetim tarihinin önceki denetim tarihlerinden sonra olması gerektiğini test etmek

**Adımlar:**
1. Firma arayın ve önceki denetim tarihlerini not edin
2. En son denetim tarihinden önceki bir tarih seçin
3. "Sonraki Ceza Kaydet" butonuna tıklayın

**Beklenen Sonuç:**
- "Denetim tarihi, önceki denetim tarihinden sonra olmalıdır" hata mesajı görünmeli
- Kayıt yapılmamalı

### Test Case 7.10: Gelecek Tarih Kontrolü
**Amaç:** Gelecek tarih seçilemeyeceğini test etmek

**Adımlar:**
1. Denetim Tarihi alanına tıklayın
2. Gelecek bir tarih seçmeye çalışın

**Beklenen Sonuç:**
- Takvimde gelecek tarihler seçilemez olmalı (maxDate: "today")

### Test Case 7.11: Başarılı Sonraki Ceza Kaydı
**Amaç:** Tüm alanlar doğru doldurulduğunda kaydın başarılı olduğunu test etmek

**Adımlar:**
1. Denetimi olan bir firma arayın
2. Personel 1: Bir personel seçin
3. Personel 2: Farklı bir personel seçin
4. Kategori: "Ceza" seçin
5. Ceza Makbuz No: "12345678" girin
6. Denetim Tarihi: Önceki denetimden sonraki bir tarih seçin
7. "Sonraki Ceza Kaydet" butonuna tıklayın

**Beklenen Sonuç:**
- Başarı mesajı görünmeli
- Form temizlenmeli
- Veritabanına yeni denetim kaydı eklenmiş olmalı
- Firmayı tekrar aradığınızda yeni kayıt GridView'de görünmeli

### Test Case 7.12: Ceza Makbuz No Validasyonu
**Amaç:** Ceza makbuz numarası format kontrolünü test etmek

**Adımlar:**
1. Kategori olarak "Ceza" seçin
2. Ceza Makbuz No alanına "abc" yazın
3. "Sonraki Ceza Kaydet" butonuna tıklayın

**Beklenen Sonuç:**
- "Ceza makbuz numarası sadece rakamlardan oluşmalıdır" hata mesajı (varsa)
- Kayıt yapılmamalı

### Test Case 7.13: Önceki Denetimler GridView Sıralaması
**Amaç:** Önceki denetimlerin tarih sırasına göre listelendiğini test etmek

**Adımlar:**
1. Birden fazla denetimi olan bir firma arayın
2. GridView'deki tarihleri kontrol edin

**Beklenen Sonuç:**
- Denetimler tarih sırasına göre (en yeniden en eskiye veya tersine) listelenmeli
- Kolonlara tıklanarak sıralama değiştirilebilmeli (varsa)

### Test Case 7.14: Form Temizleme
**Amaç:** Yeni firma arandığında formun temizlendiğini test etmek

**Adımlar:**
1. Bir firma arayın ve formu doldurun
2. Farklı bir vergi numarası girin
3. "Firma Ara" butonuna tıklayın

**Beklenen Sonuç:**
- Eski firma bilgileri temizlenmiş olmalı
- Form alanları boşalmış olmalı
- Yeni firma bilgileri yüklenmiş olmalı

### Test Case 7.15: Birden Fazla Sonraki Ceza Kaydı
**Amaç:** Aynı firmaya birden fazla sonraki ceza kaydedilebileceğini test etmek

**Adımlar:**
1. Firma arayın ve bir sonraki ceza kaydedin
2. Aynı firmayı tekrar arayın
3. Bir sonraki ceza daha kaydedin

**Beklenen Sonuç:**
- Her iki kayıt da başarılı olmalı
- GridView'de her iki kayıt da görünmeli
- Her kayıt için tarih kontrolü yapılmalı (her yeni kayıt öncekinden sonra olmalı)

---

## 8. Istatistik.aspx - İstatistikler Sayfası

### Test Case 8.1: Sayfa Yükleme Testi
**Amaç:** Sayfanın doğru yüklendiğini ve istatistiklerin gösterildiğini test etmek

**Adımlar:**
1. Ana menüden "İstatistikler" sayfasına gidin
2. Sayfanın tam olarak yüklenmesini bekleyin

**Beklenen Sonuç:**
- Sayfa başlığı "Belge Takip İstatistikleri" olmalı
- 4 adet istatistik kartı görünür olmalı:
  - Toplam Firma
  - Belgeli Firma
  - Belgesiz Firma
  - Toplam Denetim
- Her kartta sayı değeri görünmeli
- Grafikler yüklenmiş olmalı

### Test Case 8.2: Toplam Firma Kartı
**Amaç:** Toplam firma sayısının doğru hesaplandığını test etmek

**Adımlar:**
1. Toplam Firma kartını kontrol edin
2. Veritabanından manuel sorgu ile toplam firma sayısını kontrol edin

**Beklenen Sonuç:**
- Kart başlığı: "Toplam Firma"
- Gösterilen sayı veritabanındaki DISTINCT firma sayısı ile eşleşmeli
- Kart rengi: Mavi gradient
- İkon: Bina ikonu

### Test Case 8.3: Belgeli Firma Kartı
**Amaç:** Belgeli firma sayısının doğru hesaplandığını test etmek

**Adımlar:**
1. Belgeli Firma kartını kontrol edin
2. Veritabanından BELGE_ALDIMI=1 olan firma sayısını kontrol edin

**Beklenen Sonuç:**
- Kart başlığı: "Belgeli Firma"
- Gösterilen sayı veritabanındaki belgeli firma sayısı ile eşleşmeli
- Kart rengi: Yeşil gradient
- İkon: Onay ikonu

### Test Case 8.4: Belgesiz Firma Kartı
**Amaç:** Belgesiz firma sayısının doğru hesaplandığını test etmek

**Adımlar:**
1. Belgesiz Firma kartını kontrol edin
2. Veritabanından BELGE_ALDIMI=0 veya NULL olan firma sayısını kontrol edin

**Beklenen Sonuç:**
- Kart başlığı: "Belgesiz Firma"
- Gösterilen sayı veritabanındaki belgesiz firma sayısı ile eşleşmeli
- Kart rengi: Kırmızı gradient
- İkon: Uyarı ikonu

### Test Case 8.5: Toplam Denetim Kartı
**Amaç:** Toplam denetim sayısının doğru hesaplandığını test etmek

**Adımlar:**
1. Toplam Denetim kartını kontrol edin
2. Veritabanından toplam denetim kayıt sayısını kontrol edin

**Beklenen Sonuç:**
- Kart başlığı: "Toplam Denetim"
- Gösterilen sayı veritabanındaki tüm denetim kayıtlarının toplamı ile eşleşmeli
- Kart rengi: Mor gradient
- İkon: Clipboard ikonu

### Test Case 8.6: Belge Türüne Göre Dağılım Grafiği
**Amaç:** Belge türüne göre firma dağılımı grafiğinin doğru çizildiğini test etmek

**Adımlar:**
1. "Belge Türüne Göre Dağılım" grafiğini kontrol edin
2. Grafikteki değerleri veritabanı ile karşılaştırın

**Beklenen Sonuç:**
- Grafik türü: Bar chart (çubuk grafik) veya Pie chart (pasta grafik)
- Her belge türü için firma sayısı gösterilmeli
- Renkler ayırt edici olmalı
- Toplam değerler kartlardaki değerlerle tutarlı olmalı

### Test Case 8.7: İl Bazında Dağılım Grafiği
**Amaç:** İllere göre firma dağılımı grafiğinin doğru çizildiğini test etmek

**Adımlar:**
1. "İl Bazında Dağılım" grafiğini kontrol edin
2. Grafikteki değerleri veritabanı ile karşılaştırın

**Beklenen Sonuç:**
- Grafik türü: Bar chart
- Her il için firma sayısı gösterilmeli
- İller alfabetik veya değer sırasına göre listelenmiş olmalı
- Ankara en çok firma olan il ise en üstte veya en büyük değerde görünmeli

### Test Case 8.8: Aylık Denetim Grafiği
**Amaç:** Aylara göre yapılan denetim sayısı grafiğinin doğru çizildiğini test etmek

**Adımlar:**
1. "Aylık Denetim İstatistiği" grafiğini kontrol edin
2. Son 12 ayın verilerini kontrol edin

**Beklenen Sonuç:**
- Grafik türü: Line chart (çizgi grafik) veya Bar chart
- Son 12 ay görünmeli (eski aydan yeni aya doğru)
- Her ay için o ayda yapılan denetim sayısı gösterilmeli
- Değerler veritabanı ile eşleşmeli

### Test Case 8.9: Kategori Bazında Denetim Grafiği
**Amaç:** Denetim kategorilerine göre (Ceza, Uyarı, vb.) dağılım grafiğini test etmek

**Adımlar:**
1. "Kategori Bazında Denetim" grafiğini kontrol edin
2. Grafikteki değerleri veritabanı ile karşılaştırın

**Beklenen Sonuç:**
- Grafik türü: Pie chart veya Doughnut chart
- Her kategori için denetim sayısı gösterilmeli
- Renkler: Ceza (kırmızı), Uyarı (sarı), Bilgilendirme (mavi)
- Yüzde değerleri doğru hesaplanmış olmalı

### Test Case 8.10: Grafik Etkileşimi
**Amaç:** Grafiklerin interaktif özelliklerini test etmek

**Adımlar:**
1. Her grafiğin üzerine fare ile gelin (hover)
2. Grafik elemanlarına tıklayın (varsa)
3. Legend (açıklama) alanını kontrol edin

**Beklenen Sonuç:**
- Hover ile tooltip görünmeli (değer ve etiket)
- Legend tıklanabilir ise kategoriyi göster/gizle yapabilmeli
- Grafikler responsive olmalı (tarayıcı boyutu değiştirildiğinde ayarlanmalı)

### Test Case 8.11: Veri Güncelliği
**Amaç:** Gösterilen istatistiklerin güncel verilere dayandığını test etmek

**Adımlar:**
1. İstatistik sayfasını açın ve değerleri not edin
2. Yeni bir firma veya denetim kaydı ekleyin
3. İstatistik sayfasını yenileyin

**Beklenen Sonuç:**
- Sayfa yenilendiğinde değerler güncellenmeli
- Yeni eklenen kayıtlar istatistiklere yansımış olmalı

### Test Case 8.12: Boş Veri Durumu
**Amaç:** Hiç veri olmadığında istatistiklerin nasıl göründüğünü test etmek

**Adımlar:**
1. Test veritabanını boşaltın (veya boş bir veritabanı kullanın)
2. İstatistik sayfasını açın

**Beklenen Sonuç:**
- Tüm kartlarda "0" değeri görünmeli
- Grafikler boş veya "Veri bulunmamaktadır" mesajı göstermeli
- Sayfa hata vermemeli

### Test Case 8.13: Grafik Renk Şeması
**Amaç:** Grafiklerin renk uyumunu ve okunabilirliğini test etmek

**Adımlar:**
1. Tüm grafikleri görsel olarak inceleyin
2. Renk kontrastlarını kontrol edin

**Beklenen Sonuç:**
- Renkler ayırt edici ve profesyonel olmalı
- Yeşil: Pozitif/başarılı durumlar (belgeli)
- Kırmızı: Negatif/eksik durumlar (belgesiz)
- Mavi/mor: Nötr istatistikler
- Renkler genel tema ile uyumlu olmalı

### Test Case 8.14: Chart.js Kütüphanesi
**Amaç:** Chart.js kütüphanesinin doğru yüklendiğini test etmek

**Adımlar:**
1. Tarayıcı konsolunu açın
2. Sayfa yüklenirken hataları kontrol edin
3. Chart.js CDN bağlantısını kontrol edin

**Beklenen Sonuç:**
- Console'da Chart.js ile ilgili hata olmamalı
- Grafik objesi (window.Chart) tanımlı olmalı
- CDN'den script yüklenmiş olmalı

### Test Case 8.15: Performans Testi
**Amaç:** Büyük veri setlerinde sayfanın hızlı yüklendiğini test etmek

**Adımlar:**
1. Veritabanına 1000+ firma ve denetim kaydı ekleyin
2. İstatistik sayfasını açın
3. Yüklenme süresini ölçün

**Beklenen Sonuç:**
- Sayfa 3 saniye içinde yüklenmeli
- Grafikler düzgün render edilmeli
- Tarayıcı donmamalı

---

## 9. Analiz.aspx - Analiz ve Raporlama Sayfası

### Test Case 9.1: Sayfa Yükleme Testi
**Amaç:** Sayfanın doğru yüklendiğini test etmek

**Adımlar:**
1. Ana menüden "Analiz" sayfasına gidin
2. Sayfa içeriğini kontrol edin

**Beklenen Sonuç:**
- Sayfa başlığı "Analiz ve İstatistik" olmalı
- Özet istatistik kartları görünür olmalı
- Filtre bölümü (tarih aralığı, il, belge türü) görünür olmalı
- "Analiz Et" butonu görünür olmalı
- Sonuç tablosu veya grafikler bölümü hazır olmalı

### Test Case 9.2: Özet İstatistik Kartları
**Amaç:** Özet kartların doğru bilgileri gösterdiğini test etmek

**Adımlar:**
1. Sayfayı yükleyin
2. Özet kartları inceleyin

**Beklenen Sonuç:**
- 4-6 adet özet kartı görünür olmalı:
  - Toplam Firma Sayısı
  - Belgeli Firma Oranı (%)
  - Ortalama Denetim Süresi (gün)
  - En Çok Denetlenen İl
  - Toplam Ceza Sayısı
  - Muafiyet Süresi Dolmuş Firma
- Her kartta ilgili ikon ve değer görünmeli

### Test Case 9.3: Tarih Aralığı Filtresi
**Amaç:** Tarih aralığı seçilerek analiz yapılabildiğini test etmek

**Adımlar:**
1. Başlangıç Tarihi: 01.01.2024 seçin
2. Bitiş Tarihi: 31.12.2024 seçin
3. "Analiz Et" butonuna tıklayın

**Beklenen Sonuç:**
- Sadece seçilen tarih aralığındaki veriler analiz edilmeli
- Sonuç tablosu/grafikler güncellenmeli
- Özet kartlar güncellenmeli

### Test Case 9.4: Başlangıç-Bitiş Tarihi Validasyonu
**Amaç:** Bitiş tarihinin başlangıç tarihinden önce olamayacağını test etmek

**Adımlar:**
1. Başlangıç Tarihi: 31.12.2024 seçin
2. Bitiş Tarihi: 01.01.2024 seçin
3. "Analiz Et" butonuna tıklayın

**Beklenen Sonuç:**
- Hata mesajı: "Bitiş tarihi, başlangıç tarihinden önce olamaz"
- Analiz yapılmamalı

### Test Case 9.5: İl Filtresi
**Amaç:** İl bazında analiz yapılabildiğini test etmek

**Adımlar:**
1. İl dropdown'ından "Ankara" seçin
2. "Analiz Et" butonuna tıklayın
3. İl dropdown'ından "Tümü" seçin
4. "Analiz Et" butonuna tıklayın

**Beklenen Sonuç:**
- "Ankara" seçildiğinde sadece Ankara verileri analiz edilmeli
- "Tümü" seçildiğinde tüm iller dahil edilmeli

### Test Case 9.6: Belge Türü Filtresi
**Amaç:** Belge türüne göre analiz yapılabildiğini test etmek

**Adımlar:**
1. Belge Türü dropdown'ından "A2" seçin
2. "Analiz Et" butonuna tıklayın

**Beklenen Sonuç:**
- Sadece A2 belge türündeki firmalar analiz edilmeli
- Sonuçlar buna göre güncellenmeli

### Test Case 9.7: Kombine Filtreler
**Amaç:** Birden fazla filtrenin birlikte çalıştığını test etmek

**Adımlar:**
1. Tarih Aralığı: 01.01.2024 - 30.06.2024
2. İl: "Ankara"
3. Belge Türü: "A2"
4. "Analiz Et" butonuna tıklayın

**Beklenen Sonuç:**
- Tüm filtreler uygulanarak analiz yapılmalı
- Sonuç: 2024 1. yarısında, Ankara'da, A2 belge türündeki firmalar

### Test Case 9.8: Belge Alım Oranı Grafiği
**Amaç:** Belge alım oranı grafiğinin doğru çizildiğini test etmek

**Adımlar:**
1. Filtre uygulayın ve analiz edin
2. Belge alım oranı grafiğini kontrol edin

**Beklenen Sonuç:**
- Grafik türü: Doughnut veya Pie chart
- İki segment: "Belgeli" (yeşil) ve "Belgesiz" (kırmızı)
- Yüzde değerleri doğru hesaplanmış olmalı
- Toplam %100 olmalı

### Test Case 9.9: Aylık Trend Grafiği
**Amaç:** Seçilen dönemdeki aylık trend grafiğini test etmek

**Adımlar:**
1. 12 aylık bir dönem seçin (örn: 01.01.2024 - 31.12.2024)
2. "Analiz Et" butonuna tıklayın
3. Aylık trend grafiğini inceleyin

**Beklenen Sonuç:**
- Grafik türü: Line chart
- X ekseninde aylar (Ocak-Aralık)
- Y ekseninde firma veya denetim sayıları
- Her ay için veri noktası görünmeli

### Test Case 9.10: İl Bazında Karşılaştırma Grafiği
**Amaç:** İllerin karşılaştırmalı grafiğini test etmek

**Adımlar:**
1. "Tümü" seçerek tüm illeri dahil edin
2. "Analiz Et" butonuna tıklayın
3. İl karşılaştırma grafiğini inceleyin

**Beklenen Sonuç:**
- Grafik türü: Bar chart (yatay veya dikey)
- Her il için firma/denetim sayısı gösterilmeli
- İller değer sırasına göre veya alfabetik sıralanmış olmalı

### Test Case 9.11: Kategori Dağılımı Grafiği
**Amaç:** Denetim kategorilerinin dağılım grafiğini test etmek

**Adımlar:**
1. Filtre uygulayın ve analiz edin
2. Kategori dağılımı grafiğini kontrol edin

**Beklenen Sonuç:**
- Her kategori için sayı ve yüzde görünmeli
- Ceza, Uyarı, Bilgilendirme kategorileri ayrı renklerde
- Toplam denetim sayısı ile tutarlı olmalı

### Test Case 9.12: Detaylı Tablo Görünümü
**Amaç:** Analiz sonuçlarının tablo formatında görüntülenmesini test etmek

**Adımlar:**
1. Analiz yapın
2. Detaylı tablo bölümünü inceleyin

**Beklenen Sonuç:**
- Tablo kolonları: Firma Adı, Vergi No, İl, Belge Durumu, Denetim Sayısı, Son Denetim Tarihi
- Sayfalama (pagination) çalışmalı
- Sıralama (sorting) yapılabilmeli
- Export butonu (Excel/PDF) olmalı

### Test Case 9.13: Excel'e Aktarma
**Amaç:** Analiz sonuçlarının Excel'e aktarılabildiğini test etmek

**Adımlar:**
1. Filtre uygulayıp analiz yapın
2. "Excel'e Aktar" butonuna tıklayın
3. İndirilen dosyayı açın

**Beklenen Sonuç:**
- Excel dosyası indirilmeli
- Dosya adı: "Analiz_GGAAYYYY_SSDD.xlsx"
- İçerikte filtrelenmiş veriler ve özet istatistikler olmalı
- Grafikler görsel olarak da export edilmiş olmalı (isteğe bağlı)

### Test Case 9.14: PDF'e Aktarma
**Amaç:** Analiz raporunun PDF olarak oluşturulabildiğini test etmek

**Adımlar:**
1. Filtre uygulayıp analiz yapın
2. "PDF Rapor Oluştur" butonuna tıklayın (varsa)
3. İndirilen dosyayı açın

**Beklenen Sonuç:**
- PDF dosyası indirilmeli
- Dosya adı: "Analiz_Raporu_GGAAYYYY.pdf"
- İçerikte özet kartlar, grafikler ve detaylı tablo olmalı
- Formatı düzgün ve okunabilir olmalı

### Test Case 9.15: Karşılaştırmalı Analiz
**Amaç:** İki dönem arasında karşılaştırma yapılabildiğini test etmek (varsa)

**Adımlar:**
1. Dönem 1: 01.01.2024 - 30.06.2024 seçin ve analiz edin
2. Sonuçları kaydedin veya not edin
3. Dönem 2: 01.07.2024 - 31.12.2024 seçin ve analiz edin
4. Karşılaştırma özelliğini kullanın (varsa)

**Beklenen Sonuç:**
- İki dönemin verileri yan yana gösterilmeli
- Artış/azalış yüzdeleri hesaplanmış olmalı
- Trend yönü ok işaretleri ile gösterilmeli (↑↓)

---

## Test Sonuç Özeti

**Toplam Test Case Sayısı: 185**

### Sayfa Bazında Dağılım:
1. **YeniKayit.aspx**: 10 test case
2. **BelgeKayit.aspx**: 12 test case
3. **TakiptekiFirmalar.aspx**: 15 test case
4. **TumFirmalar.aspx**: 15 test case
5. **FirmaDetay.aspx**: 15 test case
6. **Teblig.aspx**: 15 test case
7. **SonrakiCeza.aspx**: 15 test case
8. **Istatistik.aspx**: 15 test case
9. **Analiz.aspx**: 15 test case

### Test Kapsamı:
- ✅ Sayfa yükleme testleri
- ✅ Validasyon kontrolleri
- ✅ Form işlemleri (ekleme, düzenleme, silme)
- ✅ Filtreleme ve arama işlemleri
- ✅ Sıralama (sorting) testleri
- ✅ Sayfalama (pagination) testleri
- ✅ Export işlemleri (Excel, PDF)
- ✅ Grafik ve görselleştirme testleri
- ✅ İş kuralları validasyonları
- ✅ Tarih kontrolleri
- ✅ Dropdown bağımlılıkları (İl-İlçe)
- ✅ GridView işlemleri
- ✅ Hata durumları ve boş veri senaryoları
- ✅ Kullanıcı deneyimi testleri

### Önemli Notlar:
- Tüm testler manuel olarak yapılacaktır
- Test veritabanında yeterli test verisi bulunmalıdır
- Her test sonucunda ekran görüntüsü alınması önerilir
- Bulunan hatalar ayrı bir doküman veya issue tracker'da tutulmalıdır
- Testler farklı tarayıcılarda (Chrome, Firefox, Edge) tekrarlanmalıdır
- Responsive tasarım testleri için farklı ekran boyutları denenmelidir
