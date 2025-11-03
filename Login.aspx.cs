using System;
using System.Data;
using System.Web.UI;
using Portal.Base;

namespace Portal
{
    public partial class Login : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                OturumTemizle();
            }
        }

        private void OturumTemizle()
        {
            Session.Abandon();
            Session.Clear();
        }

        // Login.aspx.cs içinde - Parola kontrolü
        protected void btnGiris_Click(object sender, EventArgs e)
        {
            string sicilNo = txtSicilNo.Text.Trim();
            string parola = txtParola.Text;

            try
            {
                string query = @"SELECT Sicil_No, Adi_Soyadi, Kullanici_Turu, Personel_Tipi, 
                               Parola, SifreDegistirmeZorla 
                        FROM kullanici 
                        WHERE Sicil_No = @SicilNo AND Durum = 'Aktif'";

                var parameters = BasePage.CreateParameters(("@SicilNo", sicilNo));
                DataTable dt = BasePage.ExecuteDataTable(query, parameters);

                if (dt.Rows.Count == 0)
                {
                    ShowToast("Kullanıcı bulunamadı veya pasif durumda.", "danger");
                    return;
                }

                DataRow row = dt.Rows[0];
                string dbParolaHash = row["Parola"].ToString();

                // Parola kontrolü (Hash karşılaştırması)
                if (!Helpers.VerifyPassword(parola, dbParolaHash))
                {
                    ShowToast("Hatalı parola!", "danger");
                    BasePage.LogWarning($"Başarısız giriş denemesi: {sicilNo}");
                    return;
                }

                // Session'ları oluştur
                Session["Sicil"] = row["Sicil_No"].ToString();
                Session["Ad"] = row["Adi_Soyadi"].ToString();
                Session["Kturu"] = row["Kullanici_Turu"].ToString();
                Session["Ptipi"] = row["Personel_Tipi"].ToString();

                BasePage.LogInfo($"Başarılı giriş: {sicilNo}");

                // Şifre değiştirme zorunluluğu kontrolü
                bool sifreDegistirmeZorla = Convert.ToBoolean(row["SifreDegistirmeZorla"]);
                if (sifreDegistirmeZorla)
                {
                    Response.Redirect("~/SifreDegistir.aspx"); // Şifre değiştirme sayfasına yönlendir
                }
                else
                {
                    Response.Redirect("~/Anasayfa.aspx");
                }
            }
            catch (Exception ex)
            {
                BasePage.LogError("Login hatası", ex);
                ShowToast("Giriş sırasında hata oluştu.", "danger");
            }
        }

        protected void btnTemizle_Click(object sender, EventArgs e)
        {
            txtSicilNo.Text = string.Empty;
            txtParola.Text = string.Empty;
            pnlMessage.Visible = false;
            txtSicilNo.Focus();
        }
      
    }
}