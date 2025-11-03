using System;

namespace Portal
{
    public partial class AnaV2 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //  Session Kontrolü
            if (Session["Kturu"] == null)
            {
               // Response.Redirect("Login.aspx");
            }
            else
            {
                //  Kullanıcı Adını Göster
                if (!string.IsNullOrEmpty(Session["Ad"].ToString()))
                {
                    lblKullaniciAdi.Text = Session["Ad"].ToString();
                }

                //  Sicil bilgisi (ihtiyaç varsa)
                if (Session["Sicil"] != null)
                {
                    string sicil = Session["Sicil"].ToString().Substring(2);
                    // Gerekirse başka işlemler yapılabilir
                }

                //  İlk yüklemede menü ayarları
                if (!IsPostBack)
                {
                    // Menü yetkilerini ayarla (ihtiyaç varsa)
                    SetMenuPermissions();
                }
            }
        }

        /// <summary>
        /// Kullanıcının yetkisine göre menü öğelerini gösterir/gizler
        /// </summary>
        private void SetMenuPermissions()
        {
            //  Kullanıcı türüne göre menü kontrolü
            string kullaniciTuru = Session["Kturu"]?.ToString();

            if (!string.IsNullOrEmpty(kullaniciTuru))
            {
                // Örnek: Yönetici değilse Yönetici Panelini gizle
                //if (kullaniciTuru != "Admin")
                //{
                //    menuYonetici.Visible = false;
                //}

                // Diğer yetki kontrolleri buraya eklenebilir
            }
        }
    }
}