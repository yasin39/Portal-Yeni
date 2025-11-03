using System;
using System.Data;
using System.Web.UI;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using Portal.Base;

namespace Portal.ModulBelgeTakip
{
    public partial class Istatistik : BasePage
    {
        private readonly JavaScriptSerializer JsonSerializer = new JavaScriptSerializer();

        #region SQL Sorguları
        
        private const string GetOzetQuery = @"
            SELECT 
                COUNT(*) as ToplamFirma,
                SUM(CASE WHEN BELGE_ALDIMI = 1 THEN 1 ELSE 0 END) as BelgeliFirma,
                SUM(CASE WHEN BELGE_ALDIMI = 0 THEN 1 ELSE 0 END) as BelgesizFirma,
                (SELECT COUNT(*) FROM DENETIMLER) as ToplamDenetim
            FROM FIRMALAR";

        private const string GetIlDagilimiQuery = @"
            SELECT 
                RTRIM(I.IL_AD) as Il, 
                COUNT(*) as Sayi 
            FROM FIRMALAR F
            INNER JOIN ILLER I ON F.IL = I.IL_ID
            GROUP BY I.IL_AD
            HAVING COUNT(*) > 0
            ORDER BY Sayi DESC";

        private const string GetBelgeDagilimiQuery = @"
            SELECT 
                RTRIM(B.BELGE_AD) as BelgeTuru,
                COUNT(*) as Toplam,
                SUM(CASE WHEN F.BELGE_ALDIMI = 1 THEN 1 ELSE 0 END) as Belgeli,
                SUM(CASE WHEN F.BELGE_ALDIMI = 0 THEN 1 ELSE 0 END) as Belgesiz
            FROM FIRMALAR F
            INNER JOIN BELGELER B ON F.BELGE_TIPI = B.ID
            WHERE B.IsActive = 1
            GROUP BY B.BELGE_AD
            HAVING COUNT(*) > 0
            ORDER BY Toplam DESC";

        private const string GetAylikDenetimlerQuery = @"
            SELECT 
                FORMAT(D.DENETIM_TARIHI, 'yyyy-MM') as Ay,
                COUNT(*) as ToplamDenetim,
                COUNT(DISTINCT D.FIRMA_ID) as TekilFirma,
                RTRIM(B.BELGE_AD) as BelgeTuru
            FROM DENETIMLER D
            INNER JOIN BELGELER B ON D.BELGE_TIPI = B.ID
            WHERE D.DENETIM_TARIHI IS NOT NULL AND B.IsActive = 1
            GROUP BY FORMAT(D.DENETIM_TARIHI, 'yyyy-MM'), B.BELGE_AD
            ORDER BY Ay DESC";

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (!CheckPermission(Sabitler.BELGE_TAKIP_ANALIZ))
                //{
                //    return;
                //}

                IstatistikleriYukle();
            }
        }

        private void IstatistikleriYukle()
        {
            try
            {
                OzetIstatistikleriYukle();
                IlDagilimiYukle();
                BelgeDagilimiYukle();
                AylikDenetimleriYukle();
            }
            catch (Exception ex)
            {
                LogError("İstatistikler yüklenirken hata", ex);
                ShowToast("İstatistikler yüklenirken bir hata oluştu.", "danger");
            }
        }

        private void OzetIstatistikleriYukle()
        {            
            DataTable dt = ExecuteDataTable(GetOzetQuery);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                lblToplamFirma.Text = row["ToplamFirma"].ToString();
                lblBelgeliFirma.Text = row["BelgeliFirma"].ToString();
                lblBelgesizFirma.Text = row["BelgesizFirma"].ToString();
                lblToplamDenetim.Text = row["ToplamDenetim"].ToString();
            }
        }

        private void IlDagilimiYukle()
        {            
            DataTable dt = ExecuteDataTable(GetIlDagilimiQuery);
            hdnIlData.Value = JsonSerializer.Serialize(DataTableToList(dt));
        }

        private void BelgeDagilimiYukle()
        {           
            DataTable dt = ExecuteDataTable(GetBelgeDagilimiQuery);
            hdnBelgeData.Value = JsonSerializer.Serialize(DataTableToList(dt));
        }

        private void AylikDenetimleriYukle()
        {            
            DataTable dt = ExecuteDataTable(GetAylikDenetimlerQuery);
            hdnAylikData.Value = JsonSerializer.Serialize(DataTableToList(dt));
        }

        private List<Dictionary<string, object>> DataTableToList(DataTable dt)
        {
            var Liste = new List<Dictionary<string, object>>();

            foreach (DataRow row in dt.Rows)
            {
                var Satir = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    Satir[col.ColumnName] = row[col];
                }
                Liste.Add(Satir);
            }

            return Liste;
        }
    }
}