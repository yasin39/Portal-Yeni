// weather-widget.js

//  Global değişken (HiddenField ClientID için)
var weatherHiddenFieldId = '';

//  Initialization fonksiyonu (aspx'ten çağrılacak)
function initWeatherWidget(hiddenFieldClientId) {
    weatherHiddenFieldId = hiddenFieldClientId;
    loadWeatherData();
}

//  Hava durumu verisini yükle ve göster
function loadWeatherData() {
    //  Hidden field'dan JSON verisini al
    var weatherDataField = document.getElementById(weatherHiddenFieldId);

    if (!weatherDataField || !weatherDataField.value) {
        console.error('Hava durumu verisi bulunamadı');
        return;
    }

    try {
        var weatherData = JSON.parse(weatherDataField.value);

        if (!weatherData || !weatherData.list || weatherData.list.length === 0) {
            showError('Hava durumu verisi alınamadı');
            return;
        }

        //  Güncel hava durumunu göster (ilk veri)
        displayCurrentWeather(weatherData);

        //  5 günlük tahmini göster
        displayForecast(weatherData);

    } catch (e) {
        console.error('JSON parse hatası:', e);
        showError('Veri işlenirken hata oluştu');
    }
}

//  Güncel hava durumunu göster
function displayCurrentWeather(data) {
    var current = data.list[0];

    //  Sıcaklık
    document.getElementById('temperature').textContent = Math.round(current.main.temp) + '°C';

    //  Açıklama (İlk harf büyük)
    var description = current.weather[0].description;
    document.getElementById('description').textContent = description.charAt(0).toUpperCase() + description.slice(1);

    //  Icon
    var iconCode = current.weather[0].icon;
    document.getElementById('weatherIcon').src = 'https://openweathermap.org/img/wn/' + iconCode + '@2x.png';

    //  Nem
    document.getElementById('humidity').textContent = current.main.humidity + '%';

    //  Rüzgar (m/s'den km/s'ye çevir)
    var windSpeed = (current.wind.speed * 3.6).toFixed(1);
    document.getElementById('windSpeed').textContent = windSpeed + ' km/s';

    //  Basınç
    document.getElementById('pressure').textContent = current.main.pressure + ' hPa';

    //  Görüş mesafesi (metre'den km'ye)
    var visibility = (current.visibility / 1000).toFixed(1);
    document.getElementById('visibility').textContent = visibility + ' km';
}

//  5 günlük tahmini göster
function displayForecast(data) {
    var forecastDiv = document.getElementById('forecast');
    forecastDiv.innerHTML = ''; // Önceki verileri temizle

    //  Her gün için öğlen saatindeki veriyi al (12:00)
    var dailyForecasts = getDailyForecasts(data.list);

    //  İlk 5 günü göster
    for (var i = 0; i < Math.min(5, dailyForecasts.length); i++) {
        var forecast = dailyForecasts[i];
        var forecastHTML = createForecastCard(forecast);
        forecastDiv.innerHTML += forecastHTML;
    }
}

//  Her gün için öğlen saatindeki tahmini al
function getDailyForecasts(list) {
    var dailyData = [];
    var processedDates = [];

    for (var i = 0; i < list.length; i++) {
        var item = list[i];
        var date = new Date(item.dt * 1000);
        var dateString = date.toISOString().split('T')[0]; // YYYY-MM-DD

        //  Bu tarih daha önce eklenmemişse ve saat 12:00 civarındaysa
        if (processedDates.indexOf(dateString) === -1) {
            var hour = date.getHours();

            //  Öğlen saatine yakın veriyi al (9-15 arası)
            if (hour >= 9 && hour <= 15) {
                dailyData.push(item);
                processedDates.push(dateString);
            }
        }
    }

    return dailyData;
}

//  Tahmin kartı oluştur
function createForecastCard(forecast) {
    var date = new Date(forecast.dt * 1000);
    var dayName = getDayName(date);
    var dateStr = date.getDate() + '/' + (date.getMonth() + 1);

    var temp = Math.round(forecast.main.temp);
    var description = forecast.weather[0].description;
    var iconCode = forecast.weather[0].icon;
    var iconUrl = 'https://openweathermap.org/img/wn/' + iconCode + '.png';

    var html = '<div class="col">' +
        '    <div class="forecast-date mb-1">' + dayName + '<br>' + dateStr + '</div>' +
        '    <img src="' + iconUrl + '" alt="' + description + '">' +
        '    <div class="forecast-temp">' + temp + '°C</div>' +
        '    <div class="forecast-desc">' + capitalizeFirst(description) + '</div>' +
        '</div>';

    return html;
}

//  Gün adını Türkçe olarak al
function getDayName(date) {
    var days = ['Pazar', 'Pazartesi', 'Salı', 'Çarşamba', 'Perşembe', 'Cuma', 'Cumartesi'];
    return days[date.getDay()];
}

//  İlk harfi büyük yap
function capitalizeFirst(text) {
    return text.charAt(0).toUpperCase() + text.slice(1);
}

//  Hata mesajı göster
function showError(message) {
    var currentWeather = document.getElementById('currentWeather');
    if (currentWeather) {
        currentWeather.innerHTML =
            '<div class="alert alert-danger" role="alert">' +
            '<i class="fas fa-exclamation-triangle me-2"></i>' + message +
            '</div>';
    }
}

//  UpdatePanel desteği (eğer kullanılıyorsa)
if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(function () {
        loadWeatherData(); //  Yeni veriyi yükle
    });
}