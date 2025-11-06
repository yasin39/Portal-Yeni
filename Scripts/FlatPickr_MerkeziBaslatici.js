/**
 * ===================================================================
 * Flatpickr için Merkezi Başlatıcı (Central Initializer)
 * ===================================================================
 * Bu script, sayfadaki tüm flatpickr örneklerini CSS sınıflarına
 * göre otomatik olarak bulur ve başlatır.
 *
 * Kullanılabilen Sınıflar:
 * .fp-date       : Standart, düzenlenebilir tarih seçici.
 * .fp-date-readonly: Tıklanmayan, sadece görsel salt okunur tarih.
 * .fp-datetime     : Düzenlenebilir tarih ve saat seçici.
 */

// 1. Farklı ihtiyaçlar için ayar profilleri oluşturun.
const flatpickrConfigs = {
    // Standart, düzenlenebilir tarih
    "date": {
        dateFormat: "d/m/Y",
        locale: "tr",
        allowInput: true,
        disableMobile: true
    },
    // Salt okunur, tıklanmayan tarih
    "date-readonly": {
        dateFormat: "d/m/Y",
        locale: "tr",
        allowInput: false,
        clickOpens: false, // Tıklayınca takvim açılmasın
        disableMobile: true
    },
    // YENİ EKLENDİ: Tarih ve Saat Seçici
    "datetime": {
        dateFormat: "d/m/Y H:i", // Saat ve dakikayı ekle
        locale: "tr",
        enableTime: true,        // Saati etkinleştir
        time_24hr: true,         // 24 saat formatı
        allowInput: true,
        disableMobile: true
    }
};

// 2. Tüm flatpickr'ları başlatan ana fonksiyon
function initializeAllFlatpickrs() {

    // ".fp-date" sınıfına sahip tüm elemanları bul ve başlat
    document.querySelectorAll('.fp-date').forEach(function (element) {
        // Eğer zaten başlatılmadıysa (postback'te tekrarı önler)
        if (!element._flatpickr) {
            flatpickr(element, flatpickrConfigs.date);
        }
    });

    // ".fp-date-readonly" sınıfına sahip tüm elemanları bul ve başlat
    document.querySelectorAll('.fp-date-readonly').forEach(function (element) {
        if (!element._flatpickr) {
            flatpickr(element, flatpickrConfigs["date-readonly"]);
        }
    });

    // YENİ EKLENDİ: ".fp-datetime" sınıfına sahip tüm elemanları bul ve başlat
    document.querySelectorAll('.fp-datetime').forEach(function (element) {
        if (!element._flatpickr) {
            flatpickr(element, flatpickrConfigs.datetime);
        }
    });
}

// 3. ASP.NET Yaşam Döngüsüne Entegrasyon

// A. Sayfa ilk yüklendiğinde (normal load)
document.addEventListener('DOMContentLoaded', initializeAllFlatpickrs);

// B. ASP.NET AJAX PostBack sonrası (UpdatePanel için kritik)
// Sys.WebForms.PageRequestManager yüklendiyse...
if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(initializeAllFlatpickrs);
}