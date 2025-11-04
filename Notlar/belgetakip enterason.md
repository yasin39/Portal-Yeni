# BELGE TAKİP MODÜLÜ MANUEL TEST DOKÜMANI

## Test Ortamı Bilgileri
- **Proje**: Portal ASP.NET Web Forms
- **Framework**: .NET Framework 4.8.1
- **Veritabanı**: ankarabolge (SQL Server)
- **Tablo**: firma_belge_takip
- **Test Tarihi**: 04.11.2025

---

## Ön Koşullar

### Kullanıcı Yetkilendirme
1. Test kullanıcısının `yetki` tablosunda aşağıdaki yetkilere sahip olduğunu doğrulayın:
   - `Yetki_No = 700` (BELGE_TAKIP_FIRMALAR)
   - `Yetki_No = 701` (BELGE_TAKIP_ANALIZ)
   - `Yetki_No = 150` (BelgeTakip genel yetki)

### Test Verileri Hazırlığı
1. `firma_belge_takip` tablosunda en az 3 farklı firma kaydı bulunmalı
2. En az 1 firma için BelgeAlisTarihi bugünden 25 gün öncesi olmalı (takip için)
3. En az 1 firma için BelgeAlisTarihi bugünden 35 gün öncesi olmalı (süresi geçmiş)

---

## 1. TumFirmalar.aspx - Tüm Firma ve Belgeler Listesi

### Test Case 1.1: Sayfa Yükleme ve Yetki Kontrolü
**Amaç**: Sayfanın doğru yüklendiğini ve yetki kontrolünün çalıştığını doğrulamak

**Ön Koşul**: Geçerli oturum açılmış olmalı

**Adımlar**:
1. Ana menüden `Firma-Belge Takip` > `Tüm Firma ve Belgeler` seçeneğine tıklayın
2. Sayfanın yüklendiğini gözlemleyin
3. Breadcrumb'ın doğru gösterildiğini kontrol edin

**Beklenen Sonuç**:
- Sayfa başarıyla yüklenmeli
- Breadcrumb: "Ana Sayfa > Firma-Belge Takip > Tüm Firmalar" görünmeli
- GridView tüm firma kayıtlarını listelemeye başlamalı

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 1.2: Firma Listesi Görüntüleme
**Amaç**: GridView'da tüm firmaların doğru şekilde listelendiğini doğrulamak

**Adımlar**:
1. Sayfadaki GridView'ı inceleyin
2. Aşağıdaki kolonların görünür olduğunu kontrol edin:
   - Seç (Checkbox)
   - ÜNET
   - Firma Ünvanı
   - İl
   - İlçe
   - Belge Kodu
   - Belge Türü
   - Seri No
   - Belge Alış Tarihi
   - Tebliğ Tarihi
   - Sonraki Ceza Tarihi
   - Açıklama

**Beklenen Sonuç**:
- Tüm kolonlar görünür olmalı
- Veriler doğru formatta (tarih: dd/MM/yyyy) gösterilmeli
- Sayfalama kontrolü varsa çalışıyor olmalı

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 1.3: Arama ve Filtreleme
**Amaç**: Arama kriterleri ile filtreleme işlevini test etmek

**Adımlar**:
1. ÜNET numarası giriş alanına geçerli bir ÜNET numarası yazın
2. "Ara" butonuna tıklayın
3. Sonuçları gözlemleyin
4. "Temizle" butonuna tıklayın
5. Firma ünvanı ile arama yapın
6. İl/İlçe filtreleme yapın

**Beklenen Sonuç**:
- Arama kriterine göre sonuçlar filtrelenmeli
- "Temizle" butonu tüm filtreleri kaldırmalı
- Birden fazla kriter birlikte çalışmalı (AND mantığı)

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 1.4: Kayıt Seçimi ve Detay Görüntüleme
**Amaç**: Bir firma kaydını seçip detaylarını görüntülemek

**Adımlar**:
1. GridView'da herhangi bir satırı seçin (Seç butonuna tıklayın)
2. Seçilen satırın vurgulandığını kontrol edin
3. Detay panelinin açıldığını gözlemleyin
4. Detaylardaki bilgilerin doğru olduğunu kontrol edin

**Beklenen Sonuç**:
- Seçilen satır vurgulanmalı (farklı renk/arka plan)
- Detay paneli görünmeli
- Tüm bilgiler doğru gösterilmeli

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 1.5: Excel'e Aktarma
**Amaç**: Veri dışa aktarma özelliğini test etmek

**Adımlar**:
1. "Excel'e Aktar" butonuna tıklayın
2. İndirme işleminin başladığını kontrol edin
3. İndirilen dosyayı açın
4. Verilerin doğru aktarıldığını kontrol edin

**Beklenen Sonuç**:
- Excel dosyası indirilmeli (.xlsx)
- Tüm görünür kolonlar aktarılmalı
- Türkçe karakterler doğru görünmeli
- Tarih formatları korunmalı

**Gerçekleşen Sonuç**: [ ]

---

## 2. TakiptekiFirmalar.aspx - Takipteki Firmalar

### Test Case 2.1: Takip Listesi Yükleme
**Amaç**: 30 gün içinde belge alması gereken firmaların listelendiğini doğrulamak

**Ön Koşul**: 
- Veritabanında BelgeAlisTarihi bugünden 25-30 gün öncesi olan kayıtlar olmalı

**Adımlar**:
1. Ana menüden `Firma-Belge Takip` > `Takipteki Firmalar` seçeneğine tıklayın
2. Sayfanın yüklendiğini gözlemleyin
3. GridView'daki kayıtları inceleyin
4. Her kaydın "Kalan Gün" değerini kontrol edin

**Beklenen Sonuç**:
- Sadece BelgeAlisTarihi + 30 gün >= bugün olan kayıtlar listelenmeli
- "Kalan Gün" kolonu doğru hesaplanmalı
- Kalan gün 0 veya negatifse kırmızı vurgulanmalı
- İstatistik kartları doğru sayıları göstermeli

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 2.2: Öncelik Sıralaması
**Amaç**: Firmaların kalan gün sayısına göre sıralandığını doğrulamak

**Adımlar**:
1. GridView'daki kayıtları inceleyin
2. "Kalan Gün" kolonunu gözlemleyin
3. Sıralamayı kontrol edin (küçükten büyüğe)

**Beklenen Sonuç**:
- Kayıtlar "Kalan Gün" değerine göre ASC sıralanmalı
- En az kalan günü olan firma en üstte olmalı
- Negatif değerler (süresi geçmiş) en üstte görünmeli

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 2.3: Renk Kodlaması ve Uyarılar
**Amaç**: Kalan gün sayısına göre görsel uyarıların çalıştığını test etmek

**Adımlar**:
1. Kalan günü 0 veya negatif olan kayıtları bulun
2. Satır rengini kontrol edin
3. Kalan günü 1-7 arası olan kayıtları bulun
4. Satır rengini kontrol edin

**Beklenen Sonuç**:
- Kalan gün <= 0: Kırmızı arka plan veya kalın vurgu
- Kalan gün 1-7: Sarı/turuncu arka plan
- Kalan gün > 7: Normal arka plan

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 2.4: Filtre ve Arama - İl Bazlı
**Amaç**: İl/İlçe filtrelemesinin çalıştığını doğrulamak

**Adımlar**:
1. "İl" dropdown'ını açın
2. Belirli bir il seçin
3. "Listele" butonuna tıklayın
4. Sonuçları kontrol edin
5. "Tümü" seçeneğini seçin ve tekrar listeleyin

**Beklenen Sonuç**:
- Seçilen ile ait firmalar listelenmeli
- Diğer iller filtrelenmeli
- "Tümü" seçildiğinde tüm iller gösterilmeli

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 2.5: İstatistik Kartları
**Amaç**: Sayfa üstündeki istatistik kartlarının doğru bilgiyi gösterdiğini test etmek

**Adımlar**:
1. Sayfadaki istatistik kartlarını bulun
2. "Toplam Takipteki Firma" sayısını not edin
3. GridView'daki kayıt sayısını manuel olarak sayın
4. Sayıların eşleştiğini doğrulayın

**Beklenen Sonuç**:
- Toplam takipteki firma = GridView kayıt sayısı
- Süresi geçen firma sayısı doğru hesaplanmalı
- Kartlar bootstrap stat-card stili ile görünmeli

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 2.6: Firma Detayına Gitme
**Amaç**: Listeden firma seçerek işlem sayfalarına geçiş yapmak

**Adımlar**:
1. GridView'da bir firma satırını seçin
2. "Belge Kayıt Giriş" butonuna tıklayın
3. İlgili sayfanın açıldığını ve firma bilgilerinin dolu geldiğini kontrol edin
4. Geri dönün ve "Tebliğ Tarihi Giriş" butonunu test edin

**Beklenen Sonuç**:
- İlgili sayfa açılmalı
- ÜNET ve Ünvan alanları otomatik dolu gelmeli
- Session veya QueryString ile veri taşınmalı

**Gerçekleşen Sonuç**: [ ]

---

## 3. YeniKayit.aspx - İşlem: Yeni Kayıt

### Test Case 3.1: Boş Form Görüntüleme
**Amaç**: Sayfanın boş formla doğru yüklendiğini doğrulamak

**Adımlar**:
1. Ana menüden `Firma-Belge Takip` > `İşlem- Yeni Kayıt` seçeneğine tıklayın
2. Form alanlarını inceleyin
3. Required field işaretlerini kontrol edin

**Beklenen Sonuç**:
- Tüm alanlar boş olmalı
- Zorunlu alanlar (*) işareti ile belirtilmeli
- Tarih seçiciler çalışır durumda olmalı
- Dropdown'lar dolu olmalı (İl, Belge Türü vb.)

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 3.2: Validasyon - Zorunlu Alanlar
**Amaç**: Required field validasyonlarını test etmek

**Adımlar**:
1. Hiçbir alan doldurmadan "Kaydet" butonuna tıklayın
2. Hata mesajlarını gözlemleyin
3. Sadece ÜNET alanını doldurup tekrar kaydet deneyin
4. Tüm zorunlu alanları doldurun ve kaydet deneyin

**Beklenen Sonuç**:
- Eksik alan varsa kayıt yapılmamalı
- Her eksik alan için validation mesajı görünmeli
- Zorunlu alanlar: ÜNET, Ünvan, İl, İlçe, Belge Kodu, Belge Türü
- Tüm alanlar dolu olunca kayıt başarılı olmalı

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 3.3: ÜNET Numarası Validasyonu
**Amaç**: ÜNET numarası formatının kontrol edildiğini doğrulamak

**Adımlar**:
1. ÜNET alanına harf içeren değer girin
2. Kaydet butonuna tıklayın
3. Sayısal değer girin (örn: 1234567)
4. Kaydet butonuna tıklayın

**Beklenen Sonuç**:
- Harf içeren değer kabul edilmemeli
- Validation mesajı görünmeli: "ÜNET numarası sadece rakam olmalıdır"
- Geçerli sayısal değer kabul edilmeli

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 3.4: Duplicate Kayıt Kontrolü
**Amaç**: Aynı ÜNET ve Belge Kodu ile çift kayıt yapılmadığını test etmek

**Adımlar**:
1. Veritabanında mevcut bir ÜNET ve BelgeKodu kombinasyonunu kullanarak form doldurun
2. Kaydet butonuna tıklayın
3. Hata mesajını gözlemleyin

**Beklenen Sonuç**:
- Kayıt yapılmamalı
- Hata mesajı: "Bu firma için aynı belge kodu ile kayıt zaten mevcut"
- Kullanıcı forma geri dönmeli

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 3.5: Başarılı Kayıt
**Amaç**: Yeni firma kaydının başarıyla eklenmesini test etmek

**Adımlar**:
1. Tüm zorunlu alanları geçerli değerlerle doldurun:
   - ÜNET: 9999999 (benzersiz)
   - Ünvan: Test Firma A.Ş.
   - İl: Ankara
   - İlçe: Çankaya
   - Adres: Test Mahallesi Test Sokak No:1
   - Belge Kodu: TMFB
   - Belge Türü: Yetki Belgesi
   - Seri No: 06-ABC-123
   - Belge Alış Tarihi: Bugünün tarihi
2. Kaydet butonuna tıklayın
3. Başarı mesajını kontrol edin
4. Veritabanında kaydın oluştuğunu doğrulayın

**Beklenen Sonuç**:
- Başarı mesajı görünmeli: "Kayıt başarıyla eklendi"
- Toast/alert mesajı görünmeli
- Form temizlenmeli veya yeni kayıt moduna geçmeli
- Veritabanında yeni kayıt oluşmalı
- KayitKullanici ve KayitTarihi otomatik doldurulmalı

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 3.6: İl/İlçe Cascade İlişkisi
**Amaç**: İl seçildiğinde ilçelerin dinamik olarak yüklendiğini test etmek

**Adımlar**:
1. İl dropdown'ından "Ankara" seçin
2. İlçe dropdown'ının Ankara ilçeleri ile dolduğunu kontrol edin
3. İl dropdown'ından "İstanbul" seçin
4. İlçe dropdown'ının İstanbul ilçeleri ile güncellediğini kontrol edin

**Beklenen Sonuç**:
- İl değiştiğinde ilçe dropdown'ı otomatik güncellenmeli
- Sadece seçili ile ait ilçeler listelenmeli
- İlçe seçimi temizlenmeli (yeni il seçimi sonrası)

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 3.7: Tarih Seçici (DatePicker)
**Amaç**: Tarih seçici kontrolünün çalıştığını test etmek

**Adımlar**:
1. "Belge Alış Tarihi" alanındaki takvim ikonuna tıklayın
2. Takvim popup'ının açıldığını kontrol edin
3. Bugünün tarihini seçin
4. Tarihin doğru formatta (dd/MM/yyyy) göründüğünü kontrol edin

**Beklenen Sonuç**:
- Takvim popup'ı açılmalı
- Tarih seçimi yapılabilmeli
- Format: dd/MM/yyyy
- Geçersiz tarih girişi engellenmiş olmalı

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 3.8: Temizle Butonu
**Amaç**: Form temizleme fonksiyonunu test etmek

**Adımlar**:
1. Formu doldurun (tüm alanlar)
2. "Temizle" butonuna tıklayın
3. Tüm alanların temizlendiğini kontrol edin

**Beklenen Sonuç**:
- Tüm input alanları boşalmalı
- Dropdown'lar varsayılan değere dönmeli
- Validation mesajları kaybolmalı
- Sayfa yeniden yüklenmeden temizlenmeli (JavaScript)

**Gerçekleşen Sonuç**: [ ]

---

## 4. SonrakiCeza.aspx - İşlem: Sonraki Ceza Tarihi

### Test Case 4.1: Firma Arama
**Amaç**: ÜNET numarası ile firma aramanın çalıştığını doğrulamak

**Adımlar**:
1. Sayfayı açın
2. ÜNET arama alanına mevcut bir ÜNET numarası girin
3. "Ara" butonuna tıklayın
4. Firma bilgilerinin yüklendiğini kontrol edin

**Beklenen Sonuç**:
- Firma bilgileri otomatik doldurulmalı (Ünvan, İl, İlçe, vb.)
- Mevcut belge bilgileri görünmeli
- "Sonraki Ceza Tarihi" alanı düzenlenebilir olmalı

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 4.2: Olmayan Firma Arama
**Amaç**: Veritabanında bulunmayan ÜNET ile arama yapılmasını test etmek

**Adımlar**:
1. ÜNET alanına veritabanında olmayan bir numara girin (örn: 1111111)
2. "Ara" butonuna tıklayın
3. Hata mesajını gözlemleyin

**Beklenen Sonuç**:
- "Firma bulunamadı" mesajı görünmeli
- Form alanları boş kalmalı
- Kullanıcı yeni arama yapabilmeli

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 4.3: Sonraki Ceza Tarihi Güncelleme
**Amaç**: Mevcut kayda sonraki ceza tarihi eklenmesini test etmek

**Adımlar**:
1. Geçerli bir firma aratın
2. "Sonraki Ceza Tarihi" alanına gelecek bir tarih girin
3. "Güncelle" butonuna tıklayın
4. Başarı mesajını kontrol edin
5. Veritabanında güncellemenin yapıldığını doğrulayın

**Beklenen Sonuç**:
- Güncelleme başarılı olmalı
- "Sonraki ceza tarihi güncellendi" mesajı görünmeli
- Veritabanında SonrakiCezaTarihi alanı güncellenmiş olmalı
- GuncelleyenKullanici ve GuncellemeTarihi doldurulmuş olmalı

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 4.4: Geçmiş Tarih Validasyonu
**Amaç**: Geçmiş tarih girilmesinin engellendiğini test etmek

**Adımlar**:
1. Bir firma aratın ve bulun
2. "Sonraki Ceza Tarihi" alanına bugünden önceki bir tarih girin
3. "Güncelle" butonuna tıklayın

**Beklenen Sonuç**:
- Güncelleme yapılmamalı
- Hata mesajı: "Sonraki ceza tarihi bugünden ileri bir tarih olmalıdır"
- Kullanıcı forma geri dönmeli

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 4.5: Zorunlu Alan Kontrolü
**Amaç**: Sonraki ceza tarihi alanının zorunlu olduğunu test etmek

**Adımlar**:
1. Bir firma aratın
2. Sonraki Ceza Tarihi alanını boş bırakın
3. "Güncelle" butonuna tıklayın

**Beklenen Sonuç**:
- Güncelleme yapılmamalı
- Validation mesajı: "Sonraki ceza tarihi girilmelidir"

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 4.6: Çoklu Belge Kontrolü
**Amaç**: Bir firmaya ait birden fazla belge varsa doğru kaydın güncellendiğini test etmek

**Adımlar**:
1. Birden fazla belge kaydı olan bir firma ÜNET'i ile arama yapın
2. GridView'da belgelerin listelendiğini gözlemleyin
3. Bir belge seçin
4. Sonraki Ceza Tarihi girin ve güncelle
5. Sadece seçilen belgenin güncellendiğini doğrulayın

**Beklenen Sonuç**:
- Firma için tüm belgeler listelenmiş olmalı
- Sadece seçilen belge güncellenmiş olmalı
- Diğer belgeler değişmemiş olmalı

**Gerçekleşen Sonuç**: [ ]

---

## 5. BelgeKayit.aspx - İşlem: Belge Kayıt Giriş

### Test Case 5.1: Mevcut Firmaya Belge Ekleme
**Amaç**: Sistemde var olan bir firmaya yeni belge eklemek

**Adımlar**:
1. Sayfayı açın
2. ÜNET numarası ile firma aratın
3. Firma bilgilerinin yüklendiğini kontrol edin
4. Yeni belge bilgilerini girin:
   - Belge Kodu: TMFB
   - Belge Türü: Faaliyet Belgesi
   - Seri No: 06-XYZ-789
   - Belge Alış Tarihi: Bugün
5. "Kaydet" butonuna tıklayın

**Beklenen Sonuç**:
- Kayıt başarılı olmalı
- Aynı ÜNET'e ait yeni bir belge kaydı eklenmeli
- Başarı mesajı görünmeli
- ÜNET, Ünvan, İl, İlçe gibi firma bilgileri otomatik taşınmalı

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 5.2: Yeni Firma ve Belge Ekleme
**Amaç**: Sistemde olmayan yeni bir firma ve belgesi eklemek

**Adımlar**:
1. ÜNET alanına yeni bir numara girin
2. "Ara" butonuna tıklayın
3. "Firma bulunamadı" mesajını gözlemleyin
4. Tüm firma ve belge bilgilerini manuel girin
5. "Kaydet" butonuna tıklayın

**Beklenen Sonuç**:
- Yeni firma ve belge kaydı oluşmalı
- Başarı mesajı görünmeli
- Veritabanında yeni kayıt oluşmuş olmalı

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 5.3: Belge Alış Tarihi Otomatik Hesaplama
**Amaç**: Belge alış tarihi girildiğinde sonraki ceza tarihinin otomatik hesaplandığını test etmek

**Adımlar**:
1. Belge Alış Tarihi alanına bugünün tarihini girin
2. "Sonraki Ceza Tarihi" alanının otomatik doldurulduğunu kontrol edin
3. Hesaplanan tarihin Belge Alış Tarihi + 30 gün olduğunu doğrulayın

**Beklenen Sonuç**:
- Belge Alış Tarihi girildiğinde otomatik hesaplama yapılmalı
- Sonraki Ceza Tarihi = Belge Alış Tarihi + 30 gün
- JavaScript ile anlık hesaplama yapılmalı (postback olmadan)
- Hesaplanan değer düzenlenebilir olmalı

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 5.4: Duplicate Belge Kontrolü
**Amaç**: Aynı ÜNET ve Belge Kodu ile çift kayıt engellendiğini test etmek

**Adımlar**:
1. Mevcut bir ÜNET ve Belge Kodu kombinasyonu kullanın
2. Formu doldurun
3. "Kaydet" butonuna tıklayın

**Beklenen Sonuç**:
- Kayıt yapılmamalı
- Hata: "Bu firma için bu belge kodu ile kayıt zaten mevcut"

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 5.5: Belge Güncelleme
**Amaç**: Mevcut belge kaydını güncellemek

**Adımlar**:
1. ÜNET ile firma aratın
2. Belge listesinden bir belge seçin
3. Belge bilgilerini değiştirin (örn: Seri No)
4. "Güncelle" butonuna tıklayın
5. Başarı mesajını kontrol edin
6. Veritabanında güncellemenin yapıldığını doğrulayın

**Beklenen Sonuç**:
- Güncelleme başarılı olmalı
- Sadece seçilen belge güncellenmiş olmalı
- GuncelleyenKullanici ve GuncellemeTarihi doldurulmuş olmalı

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 5.6: Zorunlu Alan Kontrolü
**Amaç**: Belge formu zorunlu alanlarının kontrol edildiğini test etmek

**Adımlar**:
1. Formu kısmen doldurun (Belge Kodu eksik bırakın)
2. "Kaydet" butonuna tıklayın
3. Validation mesajlarını gözlemleyin

**Beklenen Sonuç**:
- Kayıt yapılmamalı
- Eksik alanlar için validation mesajı görünmeli
- Zorunlu alanlar: Belge Kodu, Belge Türü, Belge Alış Tarihi

**Gerçekleşen Sonuç**: [ ]

---

## 6. Teblig.aspx - İşlem: Tebliğ Tarihi Giriş

### Test Case 6.1: Firma Arama ve Belge Listesi
**Amaç**: ÜNET ile firma aratıp belgelerini listelemek

**Adımlar**:
1. Sayfayı açın
2. ÜNET alanına geçerli bir numara girin
3. "Ara" butonuna tıklayın
4. Firma bilgilerini kontrol edin
5. Belge listesini GridView'da gözlemleyin

**Beklenen Sonuç**:
- Firma bilgileri görünmeli
- GridView'da firmaya ait tüm belgeler listelenmiş olmalı
- Her belge için: Belge Kodu, Türü, Seri No, Alış Tarihi, Tebliğ Tarihi

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 6.2: Tebliğ Tarihi Ekleme
**Amaç**: Belgeye tebliğ tarihi eklemek

**Adımlar**:
1. Bir firma aratın ve belgelerini listeleyin
2. Tebliğ tarihi boş olan bir belge seçin
3. Tebliğ Tarihi alanına bugünün tarihini girin
4. "Kaydet" butonuna tıklayın
5. Başarı mesajını kontrol edin
6. Veritabanında güncellemeyi doğrulayın

**Beklenen Sonuç**:
- Tebliğ tarihi başarıyla kaydedilmeli
- Başarı mesajı: "Tebliğ tarihi kaydedildi"
- Veritabanında BelgeTebligTarihi alanı doldurulmuş olmalı

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 6.3: Tebliğ Tarihi Güncelleme
**Amaç**: Daha önce girilmiş tebliğ tarihini değiştirmek

**Adımlar**:
1. Tebliğ tarihi dolu bir belge bulun
2. Tebliğ Tarihi alanını değiştirin
3. "Güncelle" butonuna tıklayın
4. Onay mesajı görünüyorsa onayla
5. Veritabanında güncellemeyi kontrol edin

**Beklenen Sonuç**:
- Güncelleme yapılmalı
- Kullanıcıya onay sorusu gösterilebilir
- GuncelleyenKullanici ve GuncellemeTarihi güncellenmiş olmalı

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 6.4: Tebliğ Tarihi Validasyonu
**Amaç**: Tebliğ tarihinin belge alış tarihinden önce olamayacağını test etmek

**Adımlar**:
1. Bir belge seçin (Belge Alış Tarihi: 01/10/2025)
2. Tebliğ Tarihi alanına daha erken bir tarih girin (örn: 15/09/2025)
3. "Kaydet" butonuna tıklayın

**Beklenen Sonuç**:
- Kayıt yapılmamalı
- Hata: "Tebliğ tarihi, belge alış tarihinden önce olamaz"

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 6.5: Gelecek Tarih Validasyonu
**Amaç**: Tebliğ tarihinin gelecekte bir tarih olamayacağını test etmek

**Adımlar**:
1. Bir belge seçin
2. Tebliğ Tarihi alanına gelecek bir tarih girin
3. "Kaydet" butonuna tıklayın

**Beklenen Sonuç**:
- Kayıt yapılmamalı VEYA uyarı mesajı gösterilmeli
- "Gelecek tarih girildi, emin misiniz?" şeklinde onay istenebilir

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 6.6: Çoklu Belge İşlemi
**Amaç**: Birden fazla belgeye aynı anda tebliğ tarihi girilmesini test etmek

**Adımlar**:
1. Birden fazla belgesi olan bir firma bulun
2. GridView'da birden fazla belge seçin (checkbox)
3. Toplu Tebliğ Tarihi alanına tarih girin
4. "Toplu Kaydet" butonuna tıklayın
5. Tüm seçili belgelerin güncellendiğini kontrol edin

**Beklenen Sonuç**:
- Tüm seçili belgelere aynı tebliğ tarihi atanmalı
- Başarı mesajı: "3 belge için tebliğ tarihi güncellendi"
- Veritabanında tüm kayıtlar güncellenmiş olmalı

**Not**: Bu özellik yoksa test atlanabilir.

**Gerçekleşen Sonuç**: [ ]

---

## 7. Istatistik.aspx - İstatistikler

### Test Case 7.1: Sayfa Yükleme ve Genel Görünüm
**Amaç**: İstatistik sayfasının doğru yüklendiğini doğrulamak

**Adımlar**:
1. Ana menüden `Firma-Belge Takip` > `İstatistik` seçeneğine tıklayın
2. Sayfanın yüklendiğini gözlemleyin
3. Stat kartları (istatistik kartları) bulun
4. Grafikleri inceleyin

**Beklenen Sonuç**:
- Sayfa hızlıca yüklenmeli
- En az 4 stat kartı görünmeli (Toplam Firma, Aktif Belgeler, Süresi Geçenler, vb.)
- Grafikler render olmuş olmalı (Chart.js veya ASP.NET Chart)

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 7.2: İstatistik Kartları Doğrulama
**Amaç**: İstatistik kartlarındaki sayıların doğruluğunu test etmek

**Adımlar**:
1. "Toplam Firma Sayısı" kartındaki sayıyı not edin
2. TumFirmalar.aspx sayfasına gidin
3. GridView'daki toplam kayıt sayısını karşılaştırın
4. "Takipteki Firma" sayısını not edin
5. TakiptekiFirmalar.aspx sayfasındaki kayıt sayısı ile karşılaştırın

**Beklenen Sonuç**:
- Tüm sayılar gerçek verilerle eşleşmeli
- SQL COUNT(*) sorguları doğru çalışmalı
- Kartlar bootstrap stat-card stili ile tasarlanmış olmalı

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 7.3: İl Bazlı Dağılım Grafiği
**Amaç**: İllere göre firma dağılımı grafiğinin doğru çizildiğini test etmek

**Adımlar**:
1. "İllere Göre Firma Dağılımı" grafiğini bulun
2. Grafikte gösterilen illeri not edin
3. Her ilin firma sayısını kontrol edin
4. Grafik üzerinde hover yaparak tooltip'leri test edin

**Beklenen Sonuç**:
- Tüm iller listelenmiş olmalı
- Sayılar doğru olmalı (veritabanı ile eşleşmeli)
- Grafik: Pasta, bar veya column chart olabilir
- Tooltip'ler bilgi göstermeli

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 7.4: Belge Türü Dağılımı
**Amaç**: Belge türlerine göre dağılım grafiğini test etmek

**Adımlar**:
1. "Belge Türüne Göre Dağılım" grafiğini bulun
2. Her belge türünün sayısını kontrol edin
3. Toplam sayıların tutarlı olduğunu doğrulayın

**Beklenen Sonuç**:
- Tüm belge türleri (Yetki Belgesi, Faaliyet Belgesi, vb.) gösterilmeli
- Sayılar veritabanı ile eşleşmeli
- Pasta chart formatında görünebilir

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 7.5: Tarih Aralığı Filtreleme
**Amaç**: Tarih aralığına göre istatistik filtrelemeyi test etmek

**Adımlar**:
1. Başlangıç tarihi seçin (örn: 01/01/2024)
2. Bitiş tarihi seçin (örn: 31/12/2024)
3. "Filtrele" butonuna tıklayın
4. İstatistiklerin güncellendiğini gözlemleyin

**Beklenen Sonuç**:
- Seçili tarih aralığındaki veriler gösterilmeli
- Grafik ve kartlar güncellenmeli
- "Tümünü Göster" butonu ile filtre kaldırılabilmeli

**Not**: Bu özellik yoksa test atlanabilir.

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 7.6: Excel'e Aktarma
**Amaç**: İstatistik verilerini Excel'e aktarmayı test etmek

**Adımlar**:
1. "Excel'e Aktar" butonunu bulun
2. Butona tıklayın
3. İndirilen dosyayı açın
4. Verilerin doğru aktarıldığını kontrol edin

**Beklenen Sonuç**:
- Excel dosyası indirilmeli
- Toplam değerler ve dağılım tabloları yer almalı
- Formatlar korunmalı

**Not**: Bu özellik yoksa test atlanabilir.

**Gerçekleşen Sonuç**: [ ]

---

## 8. Analiz.aspx - Analiz Sayfası

### Test Case 8.1: Yetki Kontrolü
**Amaç**: Analiz sayfası için özel yetki kontrolünü test etmek

**Adımlar**:
1. Yetki_No = 701 (BELGE_TAKIP_ANALIZ) olmayan bir kullanıcı ile giriş yapın
2. Analiz sayfasına gitmeyi deneyin
3. Yetki mesajını gözlemleyin

**Beklenen Sonuç**:
- Sayfa açılmamalı
- Yetki hatası mesajı görünmeli
- Kullanıcı ana sayfaya yönlendirilmeli

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 8.2: Detaylı Analiz Raporları
**Amaç**: Sayfa üzerindeki detaylı analiz raporlarını test etmek

**Adımlar**:
1. Yetkili kullanıcı ile sayfayı açın
2. "Trend Analizi" bölümünü inceleyin
3. "Belge Gecikme Analizi" bölümünü inceleyin
4. "İl Bazlı Karşılaştırma" bölümünü inceleyin

**Beklenen Sonuç**:
- Tüm analiz bölümleri görünmeli
- Grafikler ve tablolar dolu olmalı
- Veriler güncel olmalı

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 8.3: Aylık Trend Grafiği
**Amaç**: Belge alış tarihlerine göre aylık trend grafiğini test etmek

**Adımlar**:
1. "Aylık Belge Alış Trendi" grafiğini bulun
2. Son 12 ayın verilerinin gösterildiğini kontrol edin
3. Her ay için kayıt sayısını gözlemleyin

**Beklenen Sonuç**:
- Son 12 ay gösterilmeli (Ara 2024 - Kas 2025)
- Her ay için belge alış sayısı doğru olmalı
- Line chart formatında olmalı

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 8.4: Gecikme Analizi
**Amaç**: Belge alış süresi geçmiş firmaların analizini test etmek

**Adımlar**:
1. "Gecikme Analizi" tablosunu bulun
2. Süresi geçmiş firmaların listelendiğini kontrol edin
3. "Gecikme Süresi (Gün)" kolonunu inceleyin
4. En yüksek gecikme süresine sahip firmayı bulun

**Beklenen Sonuç**:
- Sadece süresi geçmiş firmalar listelenmeli (DATEDIFF(day, BelgeAlisTarihi, GETDATE()) > 30)
- Gecikme süresi doğru hesaplanmış olmalı
- Firmalar gecikme süresine göre DESC sıralanmış olmalı

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 8.5: İl Karşılaştırma Analizi
**Amaç**: İller arası karşılaştırmalı analizi test etmek

**Adımlar**:
1. "İl Karşılaştırma" bölümünü bulun
2. Her il için aşağıdaki metrikleri kontrol edin:
   - Toplam firma sayısı
   - Aktif belge sayısı
   - Süresi geçmiş belge sayısı
   - Ortalama gecikme süresi

**Beklenen Sonuç**:
- Tüm iller listelenmiş olmalı
- Metrikler doğru hesaplanmış olmalı
- Karşılaştırma tablosu veya grafik net görünmeli

**Gerçekleşen Sonuç**: [ ]

---

### Test Case 8.6: Rapor Yazdırma
**Amaç**: Analiz raporunu yazdırma özelliğini test etmek

**Adımlar**:
1. "Yazdır" butonuna tıklayın
2. Print preview'ın açıldığını kontrol edin
3. Sayfa düzenini inceleyin

**Beklenen Sonuç**:
- Print dialog açılmalı
- Tüm grafikler ve tablolar yazdırma uygun formatta olmalı
- Başlık ve tarih bilgileri görünmeli

**Not**: Bu özellik yoksa test atlanabilir.

**Gerçekleşen Sonuç**: [ ]

---

## ENTEGRASYON TESTLERİ

### Entegrasyon Test 1: Tam Döngü - Yeni Firma Ekleme ve Takip
**Amaç**: Yeni bir firma ekleyip tüm süreçleri test etmek

**Adımlar**:
1. **YeniKayit.aspx**: Yeni bir firma ve belge kaydı oluşturun
2. **TumFirmalar.aspx**: Eklediğiniz firmayı listede görün
3. Belge Alış Tarihi'ni bugünden 28 gün öncesi olarak güncelleyin (SQL)
4. **TakiptekiFirmalar.aspx**: Firmanın takip listesinde göründüğünü doğrulayın
5. **Teblig.aspx**: Firmaya tebliğ tarihi girin
6. **SonrakiCeza.aspx**: Sonraki ceza tarihi ekleyin
7. **Istatistik.aspx**: İstatistiklerin güncellendiğini kontrol edin

**Beklenen Sonuç**:
- Tüm adımlar başarıyla tamamlanmalı
- Veriler tüm sayfalarda tutarlı görünmeli
- İstatistikler otomatik güncellenmiş olmalı

**Gerçekleşen Sonuç**: [ ]

---

### Entegrasyon Test 2: Belge Güncelleme ve Takip Akışı
**Amaç**: Mevcut bir belgeyi güncelleme ve takip sistemini test etmek

**Adımlar**:
1. **TakiptekiFirmalar.aspx**: Takipteki bir firma seçin
2. **BelgeKayit.aspx**: Firma belgesini güncelleyin (yeni Belge Alış Tarihi)
3. **TakiptekiFirmalar.aspx**: Yeni kalan gün hesaplamasını kontrol edin
4. **Teblig.aspx**: Tebliğ tarihini güncelleyin
5. **SonrakiCeza.aspx**: Sonraki ceza tarihini güncelleyin
6. **Analiz.aspx**: Trend grafiğinde değişikliği gözlemleyin

**Beklenen Sonuç**:
- Güncelleme tüm ilgili sayfalara yansımalı
- Tarih hesaplamaları otomatik yapılmalı
- Veri tutarlılığı korunmalı

**Gerçekleşen Sonuç**: [ ]

---

### Entegrasyon Test 3: Çoklu Kullanıcı ve Yetki Kontrolü
**Amaç**: Farklı yetki seviyelerindeki kullanıcıların modüle erişimini test etmek

**Adımlar**:
1. Admin kullanıcı ile giriş yapın - tüm sayfalara eriş
2. Normal kullanıcı ile giriş yapın (BELGE_TAKIP_FIRMALAR = 700)
3. Analiz sayfasına gitmeyi deneyin
4. Yetki hatası alın
5. İstatistik sayfasına gidin - erişim sağlayın

**Beklenen Sonuç**:
- Admin tüm sayfalara erişebilmeli
- Normal kullanıcı sadece yetkisi olan sayfalara erişebilmeli
- Yetki kontrolü her sayfada çalışmalı
- Yetkisiz erişim denemelerinde kullanıcı ana sayfaya yönlendirilmeli

**Gerçekleşen Sonuç**: [ ]

---

### Entegrasyon Test 4: Veri Tutarlılığı ve Cascade İşlemler
**Amaç**: Bir firmaya ait tüm belgelerin tutarlı şekilde yönetildiğini test etmek

**Adımlar**:
1. **YeniKayit.aspx**: Aynı ÜNET'e sahip 3 farklı belge ekleyin
2. **BelgeKayit.aspx**: 3 belgeyi de listeleyin
3. Her belgeye farklı tebliğ tarihleri girin
4. **TakiptekiFirmalar.aspx**: Firmanın en yakın tarihe göre listelenmesini kontrol edin
5. **Istatistik.aspx**: Toplam belge sayısının 3 olduğunu doğrulayın

**Beklenen Sonuç**:
- Aynı firmaya birden fazla belge eklenebilmeli
- Her belge bağımsız yönetilebilmeli
- İstatistikler doğru hesaplanmalı
- Takip listesinde en kritik belge baz alınmalı

**Gerçekleşen Sonuç**: [ ]

---

### Entegrasyon Test 5: Performans ve Pagination
**Amaç**: Büyük veri setleri ile sayfalama ve performans testi

**Ön Koşul**: Veritabanında en az 100 firma kaydı olmalı

**Adımlar**:
1. **TumFirmalar.aspx**: Sayfanın yüklenme süresini ölçün
2. Son sayfaya gidin (pagination)
3. Arama yapın - 50 kayıt dönsün
4. Excel'e aktarın - 100 kayıt
5. **Istatistik.aspx**: Grafiklerin render süresini gözlemleyin

**Beklenen Sonuç**:
- Sayfa yükleme süresi < 3 saniye
- Pagination sorunsuz çalışmalı
- Excel export < 5 saniye
- 100+ kayıt ile performans düşüşü olmamalı

**Gerçekleşen Sonuç**: [ ]

---

### Entegrasyon Test 6: Session ve State Yönetimi
**Amaç**: Sayfa geçişlerinde veri kaybı olmadığını test etmek

**Adımlar**:
1. **TakiptekiFirmalar.aspx**: Bir firma seçin
2. "Belge Kayıt Giriş" butonuna tıklayın
3. **BelgeKayit.aspx**: Seçilen firmanın bilgilerinin dolu geldiğini kontrol edin
4. Geri dön (tarayıcı back butonu)
5. **TakiptekiFirmalar.aspx**: Seçimin korunduğunu/korunmadığını test edin

**Beklenen Sonuç**:
- Session veya QueryString ile veri taşınmalı
- Hedef sayfada veriler otomatik doldurulmalı
- Geri dönüşte sayfa hala kullanılabilir olmalı

**Gerçekleşen Sonuç**: [ ]

---

## ÖZET

### Test Kapsamı
- **Toplam Test Case Sayısı**: 58
  - TumFirmalar.aspx: 5 test
  - TakiptekiFirmalar.aspx: 6 test
  - YeniKayit.aspx: 8 test
  - SonrakiCeza.aspx: 6 test
  - BelgeKayit.aspx: 6 test
  - Teblig.aspx: 6 test
  - Istatistik.aspx: 6 test
  - Analiz.aspx: 6 test
  - Entegrasyon Testleri: 6 test
  - Özel Testler: 3 test

### Kritik Test Noktaları
1. ✅ Yetki kontrolleri (her sayfada)
2. ✅ Veri validasyonları (required, format, duplicate)
3. ✅ Tarih hesaplamaları (30 günlük takip süresi)
4. ✅ İl/İlçe cascade ilişkisi
5. ✅ GridView pagination ve sıralama
6. ✅ Excel export fonksiyonları
7. ✅ Toast/Alert mesajları
8. ✅ Session/State yönetimi
9. ✅ Veritabanı CRUD işlemleri
10. ✅ İstatistik ve grafik hesaplamaları

### Önerilen Entegrasyon Testleri
1. **API Entegrasyonu**: Eğer harici bir API ile veri alışverişi yapılıyorsa (örn: ÜNET doğrulama)
2. **Email/SMS Bildirimi**: Süresi dolmak üzere olan belgeler için otomatik bildirim sistemi
3. **Toplu Veri İşleme**: Excel import ile toplu firma ve belge ekleme
4. **Raporlama Servisi**: Periyodik otomatik rapor oluşturma ve mail gönderimi
5. **Audit Log**: Tüm işlemlerin log kaydının tutulması ve raporlanması
6. **Dashboard Widget**: Ana sayfaya widget entegrasyonu (önemli uyarılar için)

### Test Sonuçları Özet Tablosu

| Test Kategorisi | Test Sayısı | Başarılı | Başarısız | Beklemede |
|-----------------|-------------|----------|-----------|-----------|
| TumFirmalar     | 5           | [ ]      | [ ]       | [ ]       |
| TakiptekiFirmalar | 6         | [ ]      | [ ]       | [ ]       |
| YeniKayit       | 8           | [ ]      | [ ]       | [ ]       |
| SonrakiCeza     | 6           | [ ]      | [ ]       | [ ]       |
| BelgeKayit      | 6           | [ ]      | [ ]       | [ ]       |
| Teblig          | 6           | [ ]      | [ ]       | [ ]       |
| Istatistik      | 6           | [ ]      | [ ]       | [ ]       |
| Analiz          | 6           | [ ]      | [ ]       | [ ]       |
| Entegrasyon     | 6           | [ ]      | [ ]       | [ ]       |
| Özel Testler    | 3           | [ ]      | [ ]       | [ ]       |
| **TOPLAM**      | **58**      | **[ ]**  | **[ ]**   | **[ ]**   |

---

## EK NOTLAR

### Bilinen Kısıtlamalar
- Interface kullanılmadığı için unit test yazılamaz
- Sadece manuel test mümkündür
- Test automation (Selenium vb.) düşünülebilir

### Test Ortamı Hazırlığı
1. Test veritabanı kopyası oluşturun
2. Test kullanıcıları oluşturun (farklı yetki seviyeleri ile)
3. Backup alın (testler sonrası geri yükleme için)
4. Log dosyalarını temizleyin

### Test Süreci Önerileri
1. Her test case sonrası durumu işaretleyin
2. Bulunan hataları ayrı bir dokümanda detaylandırın
3. Ekran görüntüsü alın (özellikle hatalar için)
4. Tarayıcı console loglarını kontrol edin
5. SQL Profiler ile veritabanı sorgularını izleyin

---

**Test Dokümanı Versiyonu**: 1.0  
**Son Güncelleme**: 04.11.2025  
**Hazırlayan**: Claude AI  
**Onaylayan**: [ ]

---

