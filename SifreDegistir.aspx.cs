using System;
using System.Data;
using Portal;
using Portal.Base;

namespace Portal
{
    public partial class SifreDegistir : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Sicil"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
        }

        // SifreDegistir.aspx.cs - Hash özelliği kaldırıldı

        protected void btnDegistir_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string sicilNo = Session["Sicil"].ToString();
            string mevcutSifre = txtMevcutSifre.Text;
            string yeniSifre = txtYeniSifre.Text;

            if (yeniSifre.Length < 6)
            {
                ShowToast("Yeni şifre en az 6 karakter olmalıdır.", "warning");
                return;
            }

            try
            {
                string query = "SELECT Parola FROM kullanici WHERE Sicil_No = @SicilNo";
                var parameters = CreateParameters(("@SicilNo", sicilNo));
                DataTable dt = ExecuteDataTable(query, parameters);

                if (dt.Rows.Count == 0)
                {
                    ShowToast("Kullanıcı bulunamadı!", "danger");
                    return;
                }

                // DEĞİŞİKLİK 1: Veritabanından gelen parola artık düz metin
                string dbParola = dt.Rows[0]["Parola"].ToString(); // dbParolaHash -> dbParola

                // DEĞİŞİKLİK 2: Hash kontrolü yerine düz metin kontrolü
                if (mevcutSifre != dbParola) // Helpers.VerifyPassword kaldırıldı
                {
                    ShowToast("Mevcut şifre hatalı!", "danger");
                    return;
                }

                // DEĞİŞİKLİK 3: Yeni şifreyi hash'leme satırı kaldırıldı
                // string yeniParolaHash = Helpers.HashPassword(yeniSifre); // BU SATIR SİLİNDİ

                string updateQuery = @"UPDATE kullanici SET 
                              Parola = @Parola,
                              SifreDegistirmeZorla = 0,
                              SonSifreDegistirmeTarihi = @Tarih
                              WHERE Sicil_No = @SicilNo";

                var updateParams = CreateParameters(
                    // DEĞİŞİKLİK 4: Veritabanına hash yerine düz metin 'yeniSifre' gönderiliyor
                    ("@Parola", yeniSifre), // yeniParolaHash -> yeniSifre
                    ("@Tarih", DateTime.Now),
                    ("@SicilNo", sicilNo)
                );

                ExecuteNonQuery(updateQuery, updateParams);

                ShowToast("Şifreniz başarıyla değiştirildi!", "success");
                LogInfo($"Şifre değiştirildi: {sicilNo}");

                System.Threading.Thread.Sleep(1500);
                Response.Redirect("~/Anasayfa.aspx");
            }
            catch (Exception ex)
            {
                LogError("Şifre değiştirme hatası", ex);
                ShowToast("Şifre değiştirilirken hata oluştu.", "danger");
            }
        }
    }
}
