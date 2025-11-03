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

                string dbParolaHash = dt.Rows[0]["Parola"].ToString();

                if (!Helpers.VerifyPassword(mevcutSifre, dbParolaHash))
                {
                    ShowToast("Mevcut şifre hatalı!", "danger");
                    return;
                }

                string yeniParolaHash = Helpers.HashPassword(yeniSifre);

                string updateQuery = @"UPDATE kullanici SET 
                                      Parola = @Parola,
                                      SifreDegistirmeZorla = 0,
                                      SonSifreDegistirmeTarihi = @Tarih
                                      WHERE Sicil_No = @SicilNo";

                var updateParams = CreateParameters(
                    ("@Parola", yeniParolaHash),
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
