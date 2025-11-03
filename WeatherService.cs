using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Collections.Generic;
using System.Configuration;

namespace Portal
{
    public class WeatherService
    {
        //   OpenWeatherMap API Ayarları
        private const string API_KEY = "f3ceab1e967e3d07d7c825827f9ec986";
        private const string API_URL = "https://api.openweathermap.org/data/2.5/forecast?id={0}&appid={1}&lang=tr&units=metric";
        private const int CACHE_MINUTES = 30; // 30 dakika cache
        private readonly string connectionString;

        //   Constructor
        public WeatherService()
        {
            connectionString = ConfigurationManager.ConnectionStrings["AnkaraPortalConnection"].ConnectionString;
        }

        //   Şehir listesi ve OpenWeatherMap City ID'leri
        public Dictionary<string, int> GetCityList()
        {
            return new Dictionary<string, int>
        {
            { "Ankara", 323786 },
            { "Eskişehir", 315202 },
            { "Kırıkkale", 308463 },
            { "Çankırı", 749748 },
            { "Nevşehir", 304922 },
            { "Kayseri", 311073 },
            { "Konya", 306571 },
            { "Aksaray", 325303 },
            { "Kırşehir", 308464 }
        };
        }

        //   Hava durumu verisini getir (Cache kontrolü ile)
        public string GetWeatherData(string cityName)
        {
            try
            {
                // 1. Cache'e bak
                string cachedData = GetCachedWeatherData(cityName);

                if (!string.IsNullOrEmpty(cachedData))
                {
                    return cachedData; // Cache'den döndür
                }

                // 2. Cache yoksa veya eskiyse API'den çek
                Dictionary<string, int> cities = GetCityList();

                if (!cities.ContainsKey(cityName))
                {
                    return "{}"; // Şehir bulunamadı
                }

                int cityId = cities[cityName];
                string apiUrl = string.Format(API_URL, cityId, API_KEY);

                using (WebClient client = new WebClient())
                {
                    client.Encoding = System.Text.Encoding.UTF8;
                    string jsonData = client.DownloadString(apiUrl);

                    // 3. Veritabanına kaydet
                    SaveWeatherDataToCache(cityName, cityId, jsonData);

                    return jsonData;
                }
            }
            catch (Exception ex)
            {
                // Log tutabilirsiniz
                System.Diagnostics.Debug.WriteLine("WeatherService Error: " + ex.Message);
                return "{}";
            }
        }

        //   Cache'den veri oku
        private string GetCachedWeatherData(string cityName)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT WeatherData, LastUpdated 
                            FROM WeatherCache 
                            WHERE CityName = @CityName";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CityName", cityName);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            DateTime lastUpdated = reader.GetDateTime(1);
                            TimeSpan diff = DateTime.Now - lastUpdated;

                            //   Cache süresi dolmadıysa döndür
                            if (diff.TotalMinutes < CACHE_MINUTES)
                            {
                                return reader.GetString(0);
                            }
                        }
                    }
                }
            }

            return null; // Cache yok veya eski
        }

        //   Veritabanına kaydet veya güncelle
        private void SaveWeatherDataToCache(string cityName, int cityId, string jsonData)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"IF EXISTS (SELECT 1 FROM WeatherCache WHERE CityName = @CityName)
                            BEGIN
                                UPDATE WeatherCache 
                                SET WeatherData = @WeatherData, 
                                    LastUpdated = GETDATE(),
                                    CityId = @CityId
                                WHERE CityName = @CityName
                            END
                            ELSE
                            BEGIN
                                INSERT INTO WeatherCache (CityName, CityId, WeatherData, LastUpdated)
                                VALUES (@CityName, @CityId, @WeatherData, GETDATE())
                            END";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CityName", cityName);
                    cmd.Parameters.AddWithValue("@CityId", cityId);
                    cmd.Parameters.AddWithValue("@WeatherData", jsonData);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        //   Cache'i temizle (opsiyonel - eski kayıtları silmek için)
        public void ClearOldCache(int daysOld = 7)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"DELETE FROM WeatherCache 
                            WHERE LastUpdated < DATEADD(DAY, -@DaysOld, GETDATE())";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DaysOld", daysOld);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}