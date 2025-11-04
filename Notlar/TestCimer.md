# CİMER MODÜLÜ MANUEL TEST DOKÜMANI

## 1. KAYIT.ASPX - Başvuru Kayıt ve Güncelleme Sayfası

### 1.1 Sayfa Yükleme Testi
**Test Case ID:** TC_KAYIT_001  
**Açıklama:** Sayfa yetki kontrolü ve başlangıç durumu kontrolü

**Test Adımları:**
1. Yetkili kullanıcı ile sisteme giriş yapın
2. Kayit.aspx sayfasına gidin
3. Sayfanın başarıyla yüklendiğini doğrulayın
4. Dropdown'ların dolu olduğunu kontrol edin (Şikayet Konusu, Firma, Atanan Kullanıcı)
5. Form alanlarının boş olduğunu kontrol edin
6. Kaydet butonunun görünür, Güncelle ve İptal butonlarının gizli olduğunu kontrol edin

**Beklenen Sonuç:** Sayfa başarıyla yüklenir, tüm dropdown'lar dolu, form boş

### 1.2 Yeni Başvuru Kayıt Testi
**Test Case ID:** TC_KAYIT_002  
**Açıklama:** Yeni başvuru kaydı oluşturma

**Test Adımları:**
1. Başvuru No alanına benzersiz bir numara girin (örn: 2025001)
2. Başvuru Tarihi alanına geçerli tarih girin (gg.aa.yyyy)
3. TC No alanına 11 haneli geçerli TC kimlik numarası girin
4. Ad Soyad alanına isim girin
5. Telefon numarası girin
6. Email adresi girin (geçerli format)
7. Adres bilgisi girin
8. Başvuru Metni alanına şikayet metnini girin
9. Şikayet Konusu dropdown'ından bir seçim yapın
10. Şikayet Edilen Firma dropdown'ından bir firma seçin
11. Atanan Kullanıcı dropdown'ından bir kullanıcı seçin
12. Durum dropdown'ından bir durum seçin
13. Onay Durumu dropdown'ından bir durum seçin
14. Kaydet butonuna tıklayın
15. Başarı mesajı göründüğünü kontrol edin
16. Form alanlarının temizlendiğini doğrulayın

**Beklenen Sonuç:** Başvuru başarıyla kaydedilir, "Başvuru başarıyla kaydedildi" mesajı görünür, form temizlenir

### 1.3 Dosya Yükleme Testi
**Test Case ID:** TC_KAYIT_003  
**Açıklama:** Ek dosya yükleme fonksiyonu

**Test Adımları:**
1. Tüm zorunlu alanları doldurun
2. "Başvuru Ek" için dosya seçin butonuna tıklayın
3. 5MB boyutunda .pdf dosyası seçin ve ekleyin
4. Kaydet butonuna tıklayın
5. Dosyanın başarıyla yüklendiğini kontrol edin

**Beklenen Sonuç:** Dosya başarıyla yüklenir, başvuru kaydedilir

### 1.4 Dosya Boyut Limiti Testi
**Test Case ID:** TC_KAYIT_004  
**Açıklama:** 10MB üzeri dosya yükleme engelleme

**Test Adımları:**
1. Tüm zorunlu alanları doldurun
2. 11MB boyutunda bir dosya seçin
3. Kaydet butonuna tıklayın

**Beklenen Sonuç:** "Dosya boyutu 10MB'ı aşamaz" hata mesajı görünür, kayıt yapılmaz

### 1.5 Geçersiz Dosya Tipi Testi
**Test Case ID:** TC_KAYIT_005  
**Açıklama:** İzin verilmeyen dosya tipi yükleme engelleme

**Test Adımları:**
1. Tüm zorunlu alanları doldurun
2. .exe veya .zip uzantılı dosya seçin
3. Kaydet butonuna tıklayın

**Beklenen Sonuç:** "Desteklenmeyen dosya türü" hata mesajı görünür, kayıt yapılmaz

### 1.6 Başvuru Arama Testi
**Test Case ID:** TC_KAYIT_006  
**Açıklama:** Mevcut başvuru arama ve yükleme

**Test Adımları:**
1. Başvuru No alanına daha önce kaydedilmiş bir başvuru numarası girin
2. Ara butonuna tıklayın
3. Form alanlarının ilgili verilerle dolduğunu kontrol edin
4. Güncelle ve İptal butonlarının görünür olduğunu kontrol edin
5. Kaydet butonunun gizli olduğunu kontrol edin

**Beklenen Sonuç:** Başvuru bulunur, form doldurulur, "Başvuru bilgileri yüklendi" mesajı görünür

### 1.7 Var Olmayan Başvuru Arama Testi
**Test Case ID:** TC_KAYIT_007  
**Açıklama:** Sistemde olmayan başvuru arama

**Test Adımları:**
1. Başvuru No alanına sistemde olmayan bir numara girin (örn: 9999999)
2. Ara butonuna tıklayın

**Beklenen Sonuç:** "Başvuru bulunamadı" bilgi mesajı görünür

### 1.8 Başvuru Güncelleme Testi
**Test Case ID:** TC_KAYIT_008  
**Açıklama:** Mevcut başvuru güncelleme

**Test Adımları:**
1. Mevcut bir başvuruyu arayın (TC_KAYIT_006)
2. Başvuru Metni alanını değiştirin
3. Durum dropdown'ından farklı bir değer seçin
4. Güncelle butonuna tıklayın
5. Başarı mesajı göründüğünü kontrol edin
6. Form alanlarının temizlendiğini kontrol edin

**Beklenen Sonuç:** Başvuru güncellenir, "Başvuru başarıyla güncellendi" mesajı görünür

### 1.9 Güncelleme İptal Testi
**Test Case ID:** TC_KAYIT_009  
**Açıklama:** Güncelleme işlemini iptal etme

**Test Adımları:**
1. Mevcut bir başvuruyu arayın
2. Herhangi bir alanda değişiklik yapın
3. İptal butonuna tıklayın
4. Form alanlarının temizlendiğini kontrol edin
5. Güncelle ve İptal butonlarının gizlendiğini kontrol edin
6. Kaydet butonunun göründüğünü kontrol edin

**Beklenen Sonuç:** İşlem iptal edilir, "İşlem iptal edildi" mesajı görünür, form temizlenir

### 1.10 Zorunlu Alan Validasyonu
**Test Case ID:** TC_KAYIT_010  
**Açıklama:** Zorunlu alanlar boş bırakıldığında hata kontrolü

**Test Adımları:**
1. Sadece Başvuru No alanını doldurun
2. Diğer zorunlu alanları boş bırakın
3. Kaydet butonuna tıklayın
4. Validation hata mesajlarının göründüğünü kontrol edin

**Beklenen Sonuç:** Her zorunlu alan için validation hatası görünür, kayıt yapılmaz

---

## 2. ONAY.ASPX - Başvuru Onaylama Sayfası

### 2.1 Sayfa Yükleme ve Liste Görüntüleme
**Test Case ID:** TC_ONAY_001  
**Açıklama:** Onay bekleyen başvuruları listeleme

**Test Adımları:**
1. Personel yetkisine sahip kullanıcı ile giriş yapın
2. Onay.aspx sayfasına gidin
3. GridView'de sadece ilgili kullanıcıya atanan ve onay bekleyen başvuruların göründüğünü kontrol edin
4. GridView sütunlarının doğru başlıkları içerdiğini kontrol edin

**Beklenen Sonuç:** Sayfa yüklenir, onay bekleyen başvurular listelenir

### 2.2 Başvuru Seçme ve Detay Görüntüleme
**Test Case ID:** TC_ONAY_002  
**Açıklama:** GridView'den başvuru seçme

**Test Adımları:**
1. GridView'deki bir başvuruyu seçin (Seç butonuna tıklayın)
2. Onay panelinin göründüğünü kontrol edin
3. Başvuru No, Başvuru Metni, Yapılan İşlem alanlarının dolu olduğunu kontrol edin
4. Açıklama textbox'ının boş olduğunu kontrol edin
5. Kullanıcı dropdown'ının dolu olduğunu kontrol edin (yetki varsa)

**Beklenen Sonuç:** Başvuru detayları gösterilir, onay paneli açılır

### 2.3 Normal Personel Onay İşlemi
**Test Case ID:** TC_ONAY_003  
**Açıklama:** Normal personel tarafından başvuru onaylama

**Test Adımları:**
1. Normal personel (Ptipi=0) kullanıcısı ile giriş yapın
2. Bir başvuru seçin
3. Açıklama alanına metin girin
4. Kullanıcı dropdown'ının görünmediğini kontrol edin
5. Onayla butonuna tıklayın
6. Başarı mesajı göründüğünü kontrol edin
7. Başvurunun listeden kaybolduğunu kontrol edin

**Beklenen Sonuç:** Başvuru onaylanır, kayıt kullanıcısına geri döner, durum "Bitti" olur

### 2.4 Üst Yetkili Onay İşlemi
**Test Case ID:** TC_ONAY_004  
**Açıklama:** Üst yetkili tarafından başka kullanıcıya atama

**Test Adımları:**
1. Üst yetkili (Ptipi=1) kullanıcısı ile giriş yapın
2. Bir başvuru seçin
3. Kullanıcı dropdown'ının göründüğünü kontrol edin
4. Dropdown'dan bir kullanıcı seçin
5. Açıklama alanına metin girin
6. Onayla butonuna tıklayın
7. Başarı mesajı göründüğünü kontrol edin

**Beklenen Sonuç:** Başvuru seçilen kullanıcıya atanır, onay durumu "Onay Bekliyor" olarak kalır

### 2.5 Başvuru İade İşlemi
**Test Case ID:** TC_ONAY_005  
**Açıklama:** Başvuruyu iade etme

**Test Adımları:**
1. Bir başvuru seçin
2. Açıklama alanına iade sebebi girin
3. İade Et butonuna tıklayın
4. Başarı mesajı göründüğünü kontrol edin
5. Başvurunun listeden kaybolduğunu kontrol edin

**Beklenen Sonuç:** Başvuru iade edilir, durum "Havale" olur, ilgili kullanıcıya geri döner

### 2.6 Evrak Geçmişi Görüntüleme
**Test Case ID:** TC_ONAY_006  
**Açıklama:** Başvuru hareket geçmişini görüntüleme

**Test Adımları:**
1. Bir başvuru seçin
2. Evrak Geçmişi butonuna tıklayın
3. Geçmiş panelinin göründüğünü kontrol edin
4. Hareket tablosunda verilerin göründüğünü kontrol edin
5. Sevk Eden, Teslim Alan, Tarih, Açıklama sütunlarının dolu olduğunu kontrol edin
6. Kapat butonuna tıklayın
7. Onay paneline geri döndüğünü kontrol edin

**Beklenen Sonuç:** Evrak geçmişi tablo formatında gösterilir

### 2.7 Excel Export İşlemi
**Test Case ID:** TC_ONAY_007  
**Açıklama:** GridView'i Excel'e aktarma

**Test Adımları:**
1. GridView'de veri olduğunu kontrol edin
2. Excel'e Aktar butonuna tıklayın
3. Excel dosyasının indirilmeye başladığını kontrol edin
4. İndirilen dosyayı açın
5. Verilerin doğru şekilde aktarıldığını kontrol edin

**Beklenen Sonuç:** "CimerOnaylar.xls" dosyası indirilir, veriler doğru formatta

### 2.8 Boş Liste Excel Export
**Test Case ID:** TC_ONAY_008  
**Açıklama:** Veri olmadan export engelleme

**Test Adımları:**
1. GridView boş olduğunda (onay bekleyen başvuru yok)
2. Excel'e Aktar butonuna tıklayın

**Beklenen Sonuç:** "Export edilecek veri bulunamadı" hata mesajı görünür

### 2.9 Açıklama Zorunlu Alan Testi
**Test Case ID:** TC_ONAY_009  
**Açıklama:** Açıklama alanı boş bırakıldığında validation

**Test Adımları:**
1. Bir başvuru seçin
2. Açıklama alanını boş bırakın
3. Onayla veya İade Et butonuna tıklayın

**Beklenen Sonuç:** Required field validation hatası görünür, işlem yapılmaz

---

## 3. BASVURUYAZ.ASPX - Başvuru Rapor Yazdırma Sayfası

### 3.1 Sayfa Yükleme Testi
**Test Case ID:** TC_RAPOR_001  
**Açıklama:** Sayfa açılış ve yetki kontrolü

**Test Adımları:**
1. Raporlama yetkisi olan kullanıcı ile giriş yapın
2. BasvuruYaz.aspx sayfasına gidin
3. Başvuru No textbox'ının boş olduğunu kontrol edin
4. Getir butonunun aktif olduğunu kontrol edin
5. Rapor panelinin gizli olduğunu kontrol edin

**Beklenen Sonuç:** Sayfa başarıyla yüklenir, form boş durumda

### 3.2 Başvuru Raporu Getirme
**Test Case ID:** TC_RAPOR_002  
**Açıklama:** Başvuru numarasına göre rapor görüntüleme

**Test Adımları:**
1. Başvuru No alanına geçerli bir başvuru numarası girin
2. Getir butonuna tıklayın
3. Rapor panelinin göründüğünü kontrol edin
4. Başvuru No, Tarih, İlgili Firma alanlarının dolu olduğunu kontrol edin
5. Şikayet metninin doğru formatta (HTML break'lerle) göründüğünü kontrol edin
6. Rapor formatının yazdırma için uygun olduğunu kontrol edin

**Beklenen Sonuç:** "Rapor yüklendi" başarı mesajı görünür, rapor gösterilir

### 3.3 Var Olmayan Başvuru Raporu
**Test Case ID:** TC_RAPOR_003  
**Açıklama:** Sistemde olmayan başvuru için rapor talep etme

**Test Adımları:**
1. Başvuru No alanına sistemde olmayan bir numara girin
2. Getir butonuna tıklayın
3. Rapor panelinin gizli kaldığını kontrol edin

**Beklenen Sonuç:** "Başvuru bulunamadı" hata mesajı görünür

### 3.4 Boş Başvuru No ile Getir
**Test Case ID:** TC_RAPOR_004  
**Açıklama:** Başvuru numarası girilmeden getir butonuna tıklama

**Test Adımları:**
1. Başvuru No alanını boş bırakın
2. Getir butonuna tıklayın

**Beklenen Sonuç:** "Lütfen başvuru numarası giriniz" uyarı mesajı görünür

### 3.5 Yazdırma İşlemi
**Test Case ID:** TC_RAPOR_005  
**Açıklama:** Raporu yazdırma

**Test Adımları:**
1. Geçerli bir başvuru raporu getirin
2. Tarayıcının yazdırma fonksiyonunu kullanın (Ctrl+P)
3. Yazdırma önizlemesinde raporun düzgün göründüğünü kontrol edin
4. Başlık ve içeriğin yazdırma için uygun formatta olduğunu kontrol edin

**Beklenen Sonuç:** Rapor yazdırılabilir formatta önizlenir

---

## 4. BEKLEMEDE.ASPX - Beklemede Olan Başvurular Sayfası

### 4.1 Sayfa Yükleme ve Listeleme
**Test Case ID:** TC_BEKLE_001  
**Açıklama:** Beklemede olan başvuruları listeleme

**Test Adımları:**
1. Devam eden başvuru yetkisi olan kullanıcı ile giriş yapın
2. Beklemede.aspx sayfasına gidin
3. GridView'de sadece "Bekleme_Durumu = 'Evet'" olan kayıtların listelendiğini kontrol edin
4. Footer'da toplam kayıt sayısının göründüğünü kontrol edin
5. Tüm sütunların düzgün başlıklarla göründüğünü kontrol edin

**Beklenen Sonuç:** Beklemede olan başvurular listelenir, toplam sayı gösterilir

### 4.2 Başvuru Seçme ve Geçmiş Görüntüleme
**Test Case ID:** TC_BEKLE_002  
**Açıklama:** Başvuru geçmişini görüntüleme

**Test Adımları:**
1. GridView'den bir başvuru seçin
2. Geçmiş bölümünün göründüğünü kontrol edin
3. Hareket tablosunda verilerin göründüğünü kontrol edin
4. Tabloda Sevk Eden, Teslim Alan, Tarih, Açıklama sütunlarının olduğunu kontrol edin

**Beklenen Sonuç:** Seçilen başvurunun hareket geçmişi gösterilir

### 4.3 Excel Export İşlemi
**Test Case ID:** TC_BEKLE_003  
**Açıklama:** Beklemede olan başvuruları Excel'e aktarma

**Test Adımları:**
1. GridView'de veri olduğunu kontrol edin
2. Excel'e Aktar butonuna tıklayın
3. Dosyanın "BeklemedeBasvurular_YYYYMMDD.xls" formatında indirildiğini kontrol edin
4. İndirilen Excel dosyasını açın
5. Verilerin doğru şekilde aktarıldığını kontrol edin

**Beklenen Sonuç:** Excel dosyası başarıyla indirilir

### 4.4 Boş Liste Durumu
**Test Case ID:** TC_BEKLE_004  
**Açıklama:** Beklemede başvuru olmadığında sayfa durumu

**Test Adımları:**
1. Sistemde beklemede başvuru olmadığında sayfayı açın
2. GridView'in boş olduğunu kontrol edin
3. "Veri bulunamadı" mesajının göründüğünü kontrol edin

**Beklenen Sonuç:** Boş liste mesajı gösterilir

---

## 5. INCELENEN.ASPX - İncelenen Başvurular Sayfası

### 5.1 Sayfa Yükleme ve Listeleme
**Test Case ID:** TC_INCEL_001  
**Açıklama:** Kullanıcının incelediği başvuruları listeleme

**Test Adımları:**
1. Personel yetkisi olan kullanıcı ile giriş yapın
2. Incelenen.aspx sayfasına gidin
3. GridView'de sadece o kullanıcının güncellediği ve beklemede olan başvuruların göründüğünü kontrol edin
4. Firma ve Onay Kullanıcısı dropdown'larının dolu olduğunu kontrol edin
5. Detay panelinin başlangıçta gizli olduğunu kontrol edin

**Beklenen Sonuç:** İlgili başvurular listelenir, dropdown'lar dolu

### 5.2 Başvuru Seçme ve Detay Görüntüleme
**Test Case ID:** TC_INCEL_002  
**Açıklama:** Başvuru detaylarını görüntüleme

**Test Adımları:**
1. GridView'den bir başvuru seçin
2. Detay panelinin göründüğünü kontrol edin
3. Başvuru No, Başvuru Metni, Yapılan İşlem, Son Yapılan İşlem alanlarının dolu olduğunu kontrol edin
4. Firma dropdown'ının ilgili firma ile dolu olduğunu kontrol edin
5. Durum dropdown'ının seçilebilir olduğunu kontrol edin

**Beklenen Sonuç:** Başvuru detayları panelde gösterilir

### 5.3 Başvuru Güncelleme ve Onaya Gönderme
**Test Case ID:** TC_INCEL_003  
**Açıklama:** İncelenen başvuruyu güncelleme

**Test Adımları:**
1. Bir başvuru seçin
2. Son Yapılan İşlem alanına yeni metin girin
3. Durum dropdown'ından bir değer seçin
4. Açıklama alanına metin girin
5. Onay Kullanıcısı dropdown'ından bir kullanıcı seçin
6. Kaydet butonuna tıklayın
7. Alert mesajının göründüğünü kontrol edin
8. Sayfa yenilendiğinde başvurunun listeden kalktığını kontrol edin

**Beklenen Sonuç:** "İşlem kaydedildi ve onaya gönderildi" mesajı görünür, başvuru güncellenir

### 5.4 Tarihçe Görüntüleme
**Test Case ID:** TC_INCEL_004  
**Açıklama:** Başvuru hareketlerini görüntüleme

**Test Adımları:**
1. Bir başvuru seçin
2. Tarihçe butonuna tıklayın
3. Tarihçe panelinin göründüğünü kontrol edin
4. Detay panelinin gizlendiğini kontrol edin
5. Hareket tablosunda verilerin göründüğünü kontrol edin
6. Kapat butonuna tıklayın
7. Detay paneline geri döndüğünü kontrol edin

**Beklenen Sonuç:** Tarihçe tablo formatında gösterilir

### 5.5 Panel Kapatma
**Test Case ID:** TC_INCEL_005  
**Açıklama:** Detay panelini kapatma

**Test Adımları:**
1. Bir başvuru seçin
2. Detay panelinde Kapat butonuna tıklayın
3. Panelin gizlendiğini kontrol edin
4. GridView'in görünür kaldığını kontrol edin

**Beklenen Sonuç:** Panel kapanır, liste görünür kalır

### 5.6 Excel Export İşlemi
**Test Case ID:** TC_INCEL_006  
**Açıklama:** İncelenen başvuruları Excel'e aktarma

**Test Adımları:**
1. Excel butonuna tıklayın
2. Gizli sütunların Excel için gösterildiğini kontrol edin
3. "CimerTakipEdilen.xls" dosyasının indirildiğini kontrol edin
4. Dosyayı açın ve verileri kontrol edin

**Beklenen Sonuç:** Excel dosyası tüm sütunlar ile indirilir

### 5.7 Zorunlu Alan Validasyonu
**Test Case ID:** TC_INCEL_007  
**Açıklama:** Gerekli alanlar doldurulmadan kaydetme

**Test Adımları:**
1. Bir başvuru seçin
2. Açıklama veya Onay Kullanıcısı alanlarını boş bırakın
3. Kaydet butonuna tıklayın

**Beklenen Sonuç:** Validation hataları görünür, kayıt yapılmaz

---

## 6. DEVAMEDEN.ASPX - Devam Eden Başvurular Sayfası

### 6.1 Sayfa Yükleme ve Listeleme
**Test Case ID:** TC_DEVAM_001  
**Açıklama:** Devam eden başvuruları listeleme

**Test Adımları:**
1. Devam eden başvuru yetkisi (606) olan kullanıcı ile giriş yapın
2. DevamEden.aspx sayfasına gidin
3. GridView'de "Onay_Durumu != '3'" olan tüm başvuruların listelendiğini kontrol edin
4. "Süre" sütununun hesaplanarak göründüğünü kontrol edin
5. Footer'da toplam kayıt sayısının göründüğünü kontrol edin

**Beklenen Sonuç:** Devam eden tüm başvurular listelenir, süre bilgisi gösterilir

### 6.2 Süre Renklendirme Kontrolü
**Test Case ID:** TC_DEVAM_002  
**Açıklama:** Başvuru süresine göre renk kodlaması

**Test Adımları:**
1. GridView'deki kayıtları inceleyin
2. 0-5 gün arası sürelerin beyaz arka planlı olduğunu kontrol edin
3. 6-10 gün arası sürelerin sarı arka planlı olduğunu kontrol edin
4. 10+ gün sürelerin kırmızı arka planlı olduğunu kontrol edin

**Beklenen Sonuç:** Süreler doğru renk kodlaması ile gösterilir

### 6.3 Başvuru Geçmişi Görüntüleme
**Test Case ID:** TC_DEVAM_003  
**Açıklama:** Seçilen başvurunun hareket geçmişi

**Test Adımları:**
1. GridView'den bir başvuru seçin
2. Geçmiş panelinin göründüğünü kontrol edin
3. Hareket tablosunda tüm hareketlerin göründüğünü kontrol edin
4. Sevk Eden, Teslim Alan, Tarih, Açıklama, İşlem Açıklaması sütunlarının dolu olduğunu kontrol edin

**Beklenen Sonuç:** Başvuru geçmişi detaylı olarak gösterilir

### 6.4 Excel Export İşlemi
**Test Case ID:** TC_DEVAM_004  
**Açıklama:** Devam eden başvuruları Excel'e aktarma

**Test Adımları:**
1. Excel'e Aktar butonuna tıklayın
2. "CimerDevamEdenBasvurular.xls" dosyasının indirildiğini kontrol edin
3. Dosyayı açın
4. Tüm sütunların ve verilerin doğru aktarıldığını kontrol edin
5. Renklendirme bilgisinin Excel'de olmadığını not edin

**Beklenen Sonuç:** Excel dosyası başarıyla indirilir

### 6.5 Boş Geçmiş Durumu
**Test Case ID:** TC_DEVAM_005  
**Açıklama:** Henüz hareketi olmayan başvuru

**Test Adımları:**
1. Yeni kaydedilmiş hareket geçmişi olmayan bir başvuru seçin
2. Geçmiş panelini açın

**Beklenen Sonuç:** "Bu başvuru için henüz hareket kaydı bulunmamaktadır" mesajı görünür

---

## 7. CEVAPLADIKLARIM.ASPX - Cevapladığım Başvurular Sayfası

### 7.1 Sayfa Yükleme ve Listeleme
**Test Case ID:** TC_CEVAP_001  
**Açıklama:** Kullanıcının cevapladığı başvuruları listeleme

**Test Adımları:**
1. Personel yetkisi olan kullanıcı ile giriş yapın
2. Cevapladiklarim.aspx sayfasına gidin
3. GridView'de sadece o kullanıcının güncellediği tüm başvuruların göründüğünü kontrol edin
4. Kayıtların id'ye göre azalan sırada olduğunu kontrol edin

**Beklenen Sonuç:** Kullanıcının cevapladığı başvurular listelenir

### 7.2 Evrak Geçmişi Görüntüleme
**Test Case ID:** TC_CEVAP_002  
**Açıklama:** Başvuru hareket geçmişini görüntüleme

**Test Adımları:**
1. GridView'den bir başvuru seçin
2. Geçmiş bölümünün göründüğünü kontrol edin
3. Hareket tablosunda verilerin göründüğünü kontrol edin
4. Tablo formatının düzgün olduğunu kontrol edin

**Beklenen Sonuç:** Evrak geçmişi HTML tablo formatında gösterilir

### 7.3 Geçmiş Kapatma
**Test Case ID:** TC_CEVAP_003  
**Açıklama:** Geçmiş panelini kapatma

**Test Adımları:**
1. Bir başvuru seçip geçmişi açın
2. Kapat butonuna tıklayın
3. Geçmiş bölümünün gizlendiğini kontrol edin

**Beklenen Sonuç:** Panel kapanır

### 7.4 Excel Export İşlemi
**Test Case ID:** TC_CEVAP_004  
**Açıklama:** Cevapladığım başvuruları Excel'e aktarma

**Test Adımları:**
1. Excel'e Aktar butonuna tıklayın
2. "CimerCevapladiklarim.xls" dosyasının indirildiğini kontrol edin
3. Dosyayı açın ve verileri kontrol edin

**Beklenen Sonuç:** Excel dosyası başarıyla indirilir

### 7.5 Boş Liste Durumu
**Test Case ID:** TC_CEVAP_005  
**Açıklama:** Hiç cevap verilmemiş durumda sayfa

**Test Adımları:**
1. Hiç başvuru cevaplamamış bir kullanıcı ile giriş yapın
2. Sayfayı açın
3. GridView'in boş olduğunu kontrol edin

**Beklenen Sonuç:** Boş liste mesajı gösterilir

---

## 8. BITEN.ASPX - Biten Başvurular Sayfası

### 8.1 Sayfa Yükleme ve Listeleme
**Test Case ID:** TC_BITEN_001  
**Açıklama:** Biten başvuruları listeleme

**Test Adımları:**
1. Biten başvuru yetkisi (608) olan kullanıcı ile giriş yapın
2. Biten.aspx sayfasına gidin
3. GridView'de "Onay_Durumu = '2'" olan başvuruların listelendiğini kontrol edin
4. Süreç Durum dropdown'ının "Evet/Hayır" seçenekleri ile dolu olduğunu kontrol edin

**Beklenen Sonuç:** Biten başvurular listelenir

### 8.2 Başvuru Seçme ve Detay Görüntüleme
**Test Case ID:** TC_BITEN_002  
**Açıklama:** Biten başvuru detaylarını görüntüleme

**Test Adımları:**
1. GridView'den bir başvuru seçin
2. Havale bölümünün göründüğünü kontrol edin
3. Başvuru No, Ad Soyad, Firma, Mevcut Bekleme alanlarının dolu olduğunu kontrol edin
4. Açıklama, Durum, Süreç Durum alanlarının boş olduğunu kontrol edin

**Beklenen Sonuç:** Seçilen başvuru bilgileri panelde gösterilir

### 8.3 Kaydı Kapatma İşlemi
**Test Case ID:** TC_BITEN_003  
**Açıklama:** Biten başvuruyu sonlandırma

**Test Adımları:**
1. Bir başvuru seçin
2. Açıklama alanına kapanış notu girin
3. Durum dropdown'ından bir değer seçin
4. Süreç Durum dropdown'ından "Evet" veya "Hayır" seçin
5. Kaydı Kapat butonuna tıklayın
6. Başarı mesajının göründüğünü kontrol edin
7. Sayfa yenilendiğinde başvurunun durumunun "3" olduğunu kontrol edin

**Beklenen Sonuç:** "Kayıt başarıyla kapatıldı" mesajı görünür, başvuru kapatılır

### 8.4 Evrak Geçmişi Görüntüleme
**Test Case ID:** TC_BITEN_004  
**Açıklama:** Biten başvurunun geçmişini görüntüleme

**Test Adımları:**
1. Bir başvuru seçin
2. Evrak Geçmişi butonuna tıklayın
3. Geçmiş bölümünün göründüğünü kontrol edin
4. Hareket tablosunun doğru veriler ile dolu olduğunu kontrol edin
5. Kapat butonuna tıklayın

**Beklenen Sonuç:** Evrak geçmişi gösterilir

### 8.5 Excel Export İşlemi
**Test Case ID:** TC_BITEN_005  
**Açıklama:** Biten başvuruları Excel'e aktarma

**Test Adımları:**
1. GridView'de veri olduğunu kontrol edin
2. Excel'e Aktar butonuna tıklayın
3. "CimerBitenBasvurular.xls" dosyasının indirildiğini kontrol edin

**Beklenen Sonuç:** Excel dosyası başarıyla indirilir

### 8.6 Boş Başvuru No ile Kapatma
**Test Case ID:** TC_BITEN_006  
**Açıklama:** Başvuru seçilmeden kapatma işlemi

**Test Adımları:**
1. Hiç başvuru seçmeden Kaydı Kapat butonuna erişmeye çalışın

**Beklenen Sonuç:** Buton disabled durumda veya "Başvuru No bulunamadı" hatası görünür

### 8.7 Zorunlu Alan Validasyonu
**Test Case ID:** TC_BITEN_007  
**Açıklama:** Gerekli alanlar boş bırakıldığında validation

**Test Adımları:**
1. Bir başvuru seçin
2. Açıklama, Durum veya Süreç Durum alanlarını boş bırakın
3. Kaydı Kapat butonuna tıklayın

**Beklenen Sonuç:** Required field validation hataları görünür

---

## 9. HAVALE.ASPX - Havale Edilen Başvurular Sayfası

### 9.1 Sayfa Yükleme ve Listeleme
**Test Case ID:** TC_HAVALE_001  
**Açıklama:** Kullanıcıya havale edilen başvuruları listeleme

**Test Adımları:**
1. Personel yetkisi olan kullanıcı ile giriş yapın
2. Havale.aspx sayfasına gidin
3. GridView'de o kullanıcıya atanan ve "Onay_Durumu = '0'" olan başvuruların listelendiğini kontrol edin
4. Kullanıcı, Onaylayıcı ve Firma dropdown'larının dolu olduğunu kontrol edin

**Beklenen Sonuç:** Havale edilen başvurular listelenir, dropdown'lar doldurulur

### 9.2 Başvuru Seçme ve Detay Yükleme
**Test Case ID:** TC_HAVALE_002  
**Açıklama:** Başvuru detaylarını görüntüleme

**Test Adımları:**
1. GridView'den bir başvuru seçin
2. Havale panelinin göründüğünü kontrol edin
3. Başvuru No alanının dolu olduğunu kontrol edin
4. Yapılan İşlem, Son İşlem, Firma, Durum, Bekleme alanlarının mevcut veriler ile dolu olduğunu kontrol edin

**Beklenen Sonuç:** Başvuru detayları panelde gösterilir

### 9.3 Başka Kullanıcıya Havale Etme
**Test Case ID:** TC_HAVALE_003  
**Açıklama:** Başvuruyu başka kullanıcıya havale etme

**Test Adımları:**
1. Bir başvuru seçin
2. Kullanıcı dropdown'ından bir kullanıcı seçin
3. Açıklama alanına havale sebebi girin
4. Havale Et butonuna tıklayın
5. Sayfa yenilendiğinde başvurunun listeden kalktığını kontrol edin

**Beklenen Sonuç:** Başvuru seçilen kullanıcıya havale edilir, sayfa yenilenir

### 9.4 Başvuruya Cevap Yazma
**Test Case ID:** TC_HAVALE_004  
**Açıklama:** Başvuruya cevap verme işlemi

**Test Adımları:**
1. Bir başvuru seçin
2. Cevap Yaz butonuna tıklayın
3. Cevap panelinin göründüğünü kontrol edin
4. Bekleme Durumu dropdown'ından seçim yapın
5. Son İşlem alanına metin girin
6. Yapılan İşlem alanına metin girin
7. Durum dropdown'ından seçim yapın
8. Firma dropdown'ından seçim yapın
9. Onaylayıcı dropdown'ından bir kullanıcı seçin
10. Kaydet butonuna tıklayın
11. Sayfa yenilendiğinde başvurunun listeden kalktığını kontrol edin

**Beklenen Sonuç:** Cevap kaydedilir, başvuru onaya gönderilir (Onay_Durumu='1')

### 9.5 Başvuru İade Etme
**Test Case ID:** TC_HAVALE_005  
**Açıklama:** Başvuruyu iade/sevk etme

**Test Adımları:**
1. Bir başvuru seçin
2. Açıklama alanına iade sebebi girin
3. İade Et butonuna tıklayın
4. Sayfa yenilendiğini kontrol edin

**Beklenen Sonuç:** Başvuru iade edilir, durum "İade Edildi" olur, Onay_Durumu='3' olur

### 9.6 Geçmiş Görüntüleme
**Test Case ID:** TC_HAVALE_006  
**Açıklama:** Başvuru hareketlerini görüntüleme

**Test Adımları:**
1. Bir başvuru seçin
2. Geçmiş butonuna tıklayın
3. Geçmiş panelinin göründüğünü kontrol edin
4. GridView'de hareket kayıtlarının göründüğünü kontrol edin
5. Kapat butonuna tıklayın
6. Havale paneline geri döndüğünü kontrol edin

**Beklenen Sonuç:** Hareket geçmişi GridView formatında gösterilir

### 9.7 Tümünü Listele Butonu
**Test Case ID:** TC_HAVALE_007  
**Açıklama:** Tüm başvuruları listeleme butonu

**Test Adımları:**
1. Tümünü Listele butonuna tıklayın
2. GridView'in yenilendiğini kontrol edin

**Beklenen Sonuç:** Liste yenilenir (aynı query çalışır)

### 9.8 Dropdown Yetki Kontrolü
**Test Case ID:** TC_HAVALE_008  
**Açıklama:** Personel tipine göre dropdown içeriği

**Test Adımları:**
1. Normal personel (Ptipi=0) ile giriş yapın ve dropdown'ları kontrol edin
2. Sadece Ptipi='1' olan kullanıcıların listelendiğini kontrol edin
3. Üst yetkili (Ptipi=1) ile giriş yapın
4. Tüm aktif kullanıcıların listelendiğini kontrol edin

**Beklenen Sonuç:** Dropdown içeriği yetki seviyesine göre değişir

### 9.9 Zorunlu Alan Validasyonu
**Test Case ID:** TC_HAVALE_009  
**Açıklama:** Gerekli alanlar boş bırakıldığında hata

**Test Adımları:**
1. Bir başvuru seçin
2. Havale Et için Kullanıcı seçmeden butona tıklayın
3. Cevap Yaz panelinde zorunlu alanları boş bırakın

**Beklenen Sonuç:** Validation hataları görünür, işlem yapılmaz

---

## 10. TAKIP.ASPX - Başvuru Takip Sayfası

### 10.1 Sayfa Yükleme Testi
**Test Case ID:** TC_TAKIP_001  
**Açıklama:** Sayfa açılış ve başlangıç durumu

**Test Adımları:**
1. Raporlama yetkisi olan kullanıcı ile giriş yapın
2. Takip.aspx sayfasına gidin
3. Firma dropdown'ının dolu olduğunu kontrol edin
4. Durum dropdown'ının dolu olduğunu kontrol edin
5. GridView'in boş olduğunu kontrol edin
6. Filtre butonunun aktif olduğunu kontrol edin

**Beklenen Sonuç:** Sayfa yüklenir, dropdown'lar dolu, grid boş

### 10.2 Başvuru No ile Filtreleme
**Test Case ID:** TC_TAKIP_002  
**Açıklama:** Başvuru numarasına göre arama

**Test Adımları:**
1. Başvuru No alanına geçerli bir numara girin
2. Filtrele butonuna tıklayın
3. GridView'de ilgili başvurunun göründüğünü kontrol edin
4. Toplam kayıt sayısının göründüğünü kontrol edin

**Beklenen Sonuç:** İlgili başvuru listelenir, "Toplam 1 kayıt bulundu" mesajı görünür

### 10.3 Ad Soyad ile Filtreleme
**Test Case ID:** TC_TAKIP_003  
**Açıklama:** Başvuru sahibi adına göre arama

**Test Adımları:**
1. Ad Soyad alanına bir isim veya ismin bir kısmı girin (LIKE sorgusu)
2. Filtrele butonuna tıklayın
3. GridView'de eşleşen tüm başvuruların göründüğünü kontrol edin

**Beklenen Sonuç:** İsim içeren tüm başvurular listelenir

### 10.4 Firma ile Filtreleme
**Test Case ID:** TC_TAKIP_004  
**Açıklama:** Şikayet edilen firmaya göre filtreleme

**Test Adımları:**
1. Firma dropdown'ından bir firma seçin
2. Filtrele butonuna tıklayın
3. GridView'de sadece o firmaya ait başvuruların göründüğünü kontrol edin

**Beklenen Sonuç:** Seçili firmaya ait başvurular listelenir

### 10.5 Durum ile Filtreleme
**Test Case ID:** TC_TAKIP_005  
**Açıklama:** Başvuru durumuna göre filtreleme

**Test Adımları:**
1. Durum dropdown'ından bir durum seçin (örn: "Devam Ediyor")
2. Filtrele butonuna tıklayın
3. GridView'de sadece o durumda olan başvuruların göründüğünü kontrol edin

**Beklenen Sonuç:** Seçili durumdaki başvurular listelenir

### 10.6 Çoklu Filtre Kombinasyonu
**Test Case ID:** TC_TAKIP_006  
**Açıklama:** Birden fazla filtreyi birlikte kullanma

**Test Adımları:**
1. Başvuru No alanını boş bırakın
2. Ad Soyad alanına kısmi isim girin
3. Firma dropdown'ından bir firma seçin
4. Durum dropdown'ından bir durum seçin
5. Filtrele butonuna tıklayın
6. Tüm kriterlere uyan başvuruların listelendiğini kontrol edin

**Beklenen Sonuç:** Tüm filtrelere uyan kayıtlar listelenir

### 10.7 Başvuru Seçme ve Hareket Butonu
**Test Case ID:** TC_TAKIP_007  
**Açıklama:** Başvuru seçildiğinde hareket butonu aktivasyonu

**Test Adımları:**
1. Filtre uygulayın ve başvuru listeleyin
2. Hareket butonunun gizli/disabled olduğunu kontrol edin
3. GridView'den bir başvuru seçin
4. Hareket butonunun görünür olduğunu kontrol edin

**Beklenen Sonuç:** Başvuru seçildiğinde hareket butonu aktif olur

### 10.8 Evrak Geçmişini Gösterme
**Test Case ID:** TC_TAKIP_008  
**Açıklama:** Seçilen başvurunun hareket geçmişini görüntüleme

**Test Adımları:**
1. Filtre uygulayın ve başvuru listeleyin
2. Bir başvuru seçin
3. Hareket butonuna tıklayın
4. Geçmiş panelinin göründüğünü kontrol edin
5. Hareket tablosunda verilerin göründüğünü kontrol edin
6. Kapat butonuna tıklayın
7. Panelin kapandığını kontrol edin

**Beklenen Sonuç:** Evrak geçmişi HTML tablo formatında gösterilir

### 10.9 Sayfalama Testi
**Test Case ID:** TC_TAKIP_009  
**Açıklama:** GridView sayfalama işlevi

**Test Adımları:**
1. 10'dan fazla kayıt döndürecek filtre uygulayın
2. GridView'in sayfalandığını kontrol edin
3. Sayfa numaralarına tıklayın
4. Sayfa değişimlerinin çalıştığını kontrol edin
5. Filtrenin korunduğunu kontrol edin

**Beklenen Sonuç:** Sayfalama düzgün çalışır, filtre korunur

### 10.10 Excel Export İşlemi
**Test Case ID:** TC_TAKIP_010  
**Açıklama:** Filtrelenmiş sonuçları Excel'e aktarma

**Test Adımları:**
1. Filtre uygulayın ve sonuç alın
2. Excel'e Aktar butonuna tıklayın
3. "CimerBasvurular.xls" dosyasının indirildiğini kontrol edin
4. Dosyayı açın ve verileri kontrol edin

**Beklenen Sonuç:** Filtrelenmiş veriler Excel dosyasına aktarılır

### 10.11 Boş Filtre Sonucu
**Test Case ID:** TC_TAKIP_011  
**Açıklama:** Hiç sonuç döndürmeyen filtre

**Test Adımları:**
1. Var olmayan bir başvuru numarası girin
2. Filtrele butonuna tıklayın
3. GridView'in boş olduğunu kontrol edin
4. Toplam kayıt label'ının "Toplam 0 kayıt bulundu" dediğini kontrol edin

**Beklenen Sonuç:** Boş liste gösterilir, uygun mesaj görünür

### 10.12 Filtre Temizleme
**Test Case ID:** TC_TAKIP_012  
**Açıklama:** Filtreleri sıfırlama

**Test Adımları:**
1. Tüm filtre alanlarını doldurun
2. Filtrele butonuna tıklayın
3. Sayfa yenilendiğinde filtrelerin temizlenip temizlenmediğini kontrol edin

**Beklenen Sonuç:** Filtreler korunur (PostBack ile), manuel temizleme gerekir

---

## 11. ISTATISTIK.ASPX - İstatistik Sayfası

### 11.1 Sayfa Yükleme ve Varsayılan Yıl
**Test Case ID:** TC_ISTAT_001  
**Açıklama:** Sayfa açılış ve mevcut yıl istatistikleri

**Test Adımları:**
1. Personel yetkisi olan kullanıcı ile giriş yapın
2. Istatistik.aspx sayfasına gidin
3. Yıl dropdown'ının mevcut yıl ile dolu olduğunu kontrol edin
4. Chart'ın yüklendiğini kontrol edin
5. Devam Eden, Cevaplanan, Toplam, İkinci Kez sayılarının göründüğünü kontrol edin

**Beklenen Sonuç:** Sayfa yüklenir, mevcut yıl istatistikleri gösterilir

### 11.2 Yıl Dropdown İçeriği
**Test Case ID:** TC_ISTAT_002  
**Açıklama:** Yıl dropdown'ının doğru içeriği

**Test Adımları:**
1. Yıl dropdown'ını açın
2. 2017'den başlayıp mevcut yıl + 5 yıla kadar olan yılların listelendiğini kontrol edin
3. Yılların artan sırada olduğunu kontrol edin

**Beklenen Sonuç:** Dropdown 2017'den itibaren gelecek 5 yıla kadar tüm yılları içerir

### 11.3 Yıl Değiştirme Testi
**Test Case ID:** TC_ISTAT_003  
**Açıklama:** Farklı yıl seçerek istatistik görüntüleme

**Test Adımları:**
1. Yıl dropdown'ından önceki bir yıl seçin (örn: 2023)
2. Chart'ın yenilendiğini kontrol edin
3. İstatistik sayılarının güncellendiğini kontrol edin
4. Değerlerin seçilen yıla ait olduğunu doğrulayın

**Beklenen Sonuç:** Seçilen yıla ait istatistikler gösterilir

### 11.4 Şirket Dağılım Chart Kontrolü
**Test Case ID:** TC_ISTAT_004  
**Açıklama:** Firma bazlı şikayet dağılımı chart'ı

**Test Adımları:**
1. Chart'ın göründüğünü kontrol edin
2. X eksende firma isimlerinin göründüğünü kontrol edin
3. Y eksende sayı değerlerinin göründüğünü kontrol edin
4. Her firmanın kaç başvuru aldığının görülebildiğini kontrol edin
5. Chart üzerine mouse ile gelindiğinde detayların göründüğünü kontrol edin

**Beklenen Sonuç:** Firma dağılımı bar chart olarak görselleştirilir

### 11.5 Devam Eden Başvuru Sayısı
**Test Case ID:** TC_ISTAT_005  
**Açıklama:** Devam eden başvuru istatistiği

**Test Adımları:**
1. "Devam Eden" label'ının sayısal değer gösterdiğini kontrol edin
2. Bu sayının "Onay_Durumu != '3'" olan kayıt sayısına eşit olduğunu doğrulayın

**Beklenen Sonuç:** Devam eden başvuru sayısı doğru gösterilir

### 11.6 Cevaplanan Başvuru Sayısı
**Test Case ID:** TC_ISTAT_006  
**Açıklama:** Cevaplanan başvuru istatistiği

**Test Adımları:**
1. "Cevaplanan" label'ının sayısal değer gösterdiğini kontrol edin
2. Bu sayının "Onay_Durumu = '3'" olan kayıt sayısına eşit olduğunu doğrulayın

**Beklenen Sonuç:** Cevaplanan başvuru sayısı doğru gösterilir

### 11.7 Toplam Başvuru Sayısı
**Test Case ID:** TC_ISTAT_007  
**Açıklama:** Toplam başvuru istatistiği

**Test Adımları:**
1. "Toplam" label'ının sayısal değer gösterdiğini kontrol edin
2. Bu sayının Devam Eden + Cevaplanan toplamına eşit olduğunu doğrulayın

**Beklenen Sonuç:** Toplam doğru hesaplanır ve gösterilir

### 11.8 İkinci Kez Cevaplanan Sayısı
**Test Case ID:** TC_ISTAT_008  
**Açıklama:** İkinci kez cevap verilen başvuru istatistiği

**Test Adımları:**
1. "İkinci Kez" label'ının sayısal değer gösterdiğini kontrol edin
2. Bu sayının "Son_Yapilan_islem IS NOT NULL" olan kayıt sayısına eşit olduğunu doğrulayın

**Beklenen Sonuç:** İkinci kez cevaplanan sayısı doğru gösterilir

### 11.9 Veri Olmayan Yıl Testi
**Test Case ID:** TC_ISTAT_009  
**Açıklama:** Hiç başvuru olmayan yıl seçme

**Test Adımları:**
1. Gelecek yıllardan birini seçin (henüz veri yok)
2. Chart'ın boş olduğunu veya "Veri yok" mesajı gösterdiğini kontrol edin
3. Tüm sayıların 0 olduğunu kontrol edin

**Beklenen Sonuç:** Boş chart ve sıfır değerler gösterilir

### 11.10 Chart Etkileşim Testi
**Test Case ID:** TC_ISTAT_010  
**Açıklama:** Chart üzerinde etkileşim

**Test Adımları:**
1. Chart'taki barlardan birine mouse ile tıklayın veya üzerine gelin
2. Tooltip veya detay bilgisinin göründüğünü kontrol edin
3. Firma adı ve sayı bilgisinin net göründüğünü kontrol edin

**Beklenen Sonuç:** Chart etkileşimli şekilde çalışır

---

## GENEL TEST SENARYOLARI

### GEN.1 Yetki Kontrolü Testleri
**Test Case ID:** TC_GEN_001  
**Açıklama:** Her sayfa için yetki kontrolü

**Test Adımları:**
1. Yetkisiz kullanıcı ile sisteme giriş yapın
2. Sırayla her Cimer modülü sayfasına erişmeye çalışın
3. Her sayfa için yetki hatası veya yönlendirme olduğunu kontrol edin

**Beklenen Sonuç:** Yetkisiz kullanıcılar sayfalara erişemez, uygun hata mesajı görür

### GEN.2 Session Timeout Testi
**Test Case ID:** TC_GEN_002  
**Açıklama:** Oturum süresi dolduğunda davranış

**Test Adımları:**
1. Sisteme giriş yapın
2. Herhangi bir Cimer sayfasını açın
3. 20 dakika bekleyin (session timeout)
4. Sayfada herhangi bir işlem yapmaya çalışın

**Beklenen Sonuç:** Login sayfasına yönlendirme yapılır

### GEN.3 Eşzamanlı İşlem Testi
**Test Case ID:** TC_GEN_003  
**Açıklama:** Aynı başvuruyu birden fazla kullanıcının işlemesi

**Test Adımları:**
1. İki farklı tarayıcıda iki farklı kullanıcı ile giriş yapın
2. Aynı başvuruyu her iki kullanıcı ile açın
3. Birinci kullanıcı ile başvuruyu güncelleyin
4. İkinci kullanıcı ile de aynı başvuruyu güncellemeye çalışın

**Beklenen Sonuç:** Son güncelleyen kullanıcının değişiklikleri kaydedilir

### GEN.4 SQL Injection Testi
**Test Case ID:** TC_GEN_004  
**Açıklama:** SQL injection saldırısı önleme

**Test Adımları:**
1. Herhangi bir arama/filtre alanına SQL injection deneyin:
   - `'; DROP TABLE cimer_basvurular; --`
   - `1' OR '1'='1`
2. Aramanın hata vermediğini kontrol edin
3. Veritabanının etkilenmediğini kontrol edin

**Beklenen Sonuç:** Parametreli sorgular sayesinde SQL injection engellenir

### GEN.5 XSS Saldırısı Testi
**Test Case ID:** TC_GEN_005  
**Açıklama:** Cross-site scripting saldırısı önleme

**Test Adımları:**
1. Metin alanlarına script kodu girin:
   - `<script>alert('XSS')</script>`
   - `<img src=x onerror=alert('XSS')>`
2. Kaydedin ve sayfayı yenileyin
3. Script'in çalışmadığını, text olarak göründüğünü kontrol edin

**Beklenen Sonuç:** XSS saldırıları engellenir, HTML encode edilir

### GEN.6 Responsive Tasarım Testi
**Test Case ID:** TC_GEN_006  
**Açıklama:** Mobil ve tablet cihazlarda görünüm

**Test Adımları:**
1. Tarayıcı geliştirici araçlarını açın
2. Mobil görünüme (375px) geçin
3. Her sayfayı açın ve kontrol edin
4. Tablet görünüme (768px) geçin
5. Her sayfayı açın ve kontrol edin

**Beklenen Sonuç:** Bootstrap responsive özellikleri ile her ekranda düzgün görünüm

### GEN.7 Tarayıcı Uyumluluğu
**Test Case ID:** TC_GEN_007  
**Açıklama:** Farklı tarayıcılarda çalışma

**Test Adımları:**
1. Chrome'da tüm sayfaları test edin
2. Firefox'ta tüm sayfaları test edin
3. Edge'de tüm sayfaları test edin
4. Safari'de (varsa) tüm sayfaları test edin

**Beklenen Sonuç:** Tüm modern tarayıcılarda düzgün çalışır

### GEN.8 Hata Mesajları Tutarlılığı
**Test Case ID:** TC_GEN_008  
**Açıklama:** Hata mesajlarının tutarlı ve anlaşılır olması

**Test Adımları:**
1. Her sayfada kasıtlı hatalar oluşturun
2. Hata mesajlarının Türkçe ve anlaşılır olduğunu kontrol edin
3. Toast mesajlarının doğru renkte göründüğünü kontrol edin:
   - Başarı: yeşil
   - Uyarı: sarı
   - Hata: kırmızı
   - Bilgi: mavi

**Beklenen Sonuç:** Tüm mesajlar tutarlı ve kullanıcı dostu

### GEN.9 Veritabanı Transaction Testi
**Test Case ID:** TC_GEN_009  
**Açıklama:** Transaction rollback mekanizması

**Test Adımları:**
1. Kayıt işleminde veritabanı hatası simüle edin
2. Ana tablonun ve hareketler tablosunun tutarlı kaldığını kontrol edin
3. Partial commit olmadığını doğrulayın

**Beklenen Sonuç:** Transaction rollback çalışır, veri tutarlılığı korunur

### GEN.10 Performans Testi
**Test Case ID:** TC_GEN_010  
**Açıklama:** Büyük veri setlerinde performans

**Test Adımları:**
1. Veritabanına 10000+ başvuru kaydı ekleyin
2. DevamEden.aspx gibi tüm kayıtları listeleyen sayfaları açın
3. Sayfa yükleme süresini ölçün
4. GridView'de sayfalama çalıştığını kontrol edin

**Beklenen Sonuç:** Sayfa makul sürede yüklenir, sayfalama ile performans korunur

---

## TEST SONUÇ ÖZETİ

### Test İstatistikleri
- **Toplam Test Case Sayısı:** 133
- **Kayit.aspx:** 10 test case
- **Onay.aspx:** 9 test case
- **BasvuruYaz.aspx:** 5 test case
- **Beklemede.aspx:** 4 test case
- **Incelenen.aspx:** 7 test case
- **DevamEden.aspx:** 5 test case
- **Cevapladiklarim.aspx:** 5 test case
- **Biten.aspx:** 7 test case
- **Havale.aspx:** 9 test case
- **Takip.aspx:** 12 test case
- **Istatistik.aspx:** 10 test case
- **Genel Testler:** 10 test case

### Test Öncelikleri
- **Yüksek Öncelik:** Yetki kontrolleri, veri kaydetme/güncelleme, transaction işlemleri
- **Orta Öncelik:** Filtreleme, arama, Excel export, validation
- **Düşük Öncelik:** UI/UX, responsive, tarayıcı uyumluluğu

### Test Ortamı Gereksinimleri
- ASP.NET Framework 4.8.1
- SQL Server Database
- Bootstrap 5.3.0
- Font Awesome
- Modern web tarayıcıları (Chrome, Firefox, Edge)
- Test veritabanı (production'dan bağımsız)

### Test Verisi Gereksinimleri
- En az 5 aktif kullanıcı (farklı yetki seviyelerinde)
- En az 10 firma kaydı
- En az 50 başvuru kaydı (farklı durumlarda)
- En az 100 hareket kaydı

**Toplam Test Case Sayısı: 133**
