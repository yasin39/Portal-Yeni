using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;

namespace Portal
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            ScriptManager.ScriptResourceMapping.AddDefinition("jquery",
                new ScriptResourceDefinition
                {                   
                    Path = "~/Scripts/jquery-3.7.1.min.js",                    
                    DebugPath = "~/Scripts/jquery-3.7.1.js",

                    // CDN (Content Delivery Network) yolu - opsiyonel
                    CdnPath = "https://ajax.aspnetcdn.com/ajax/jQuery/jquery-3.7.1.min.js",
                    CdnDebugPath = "https://ajax.aspnetcdn.com/ajax/jQuery/jquery-3.7.1.js",
                    LoadSuccessExpression = "window.jQuery" // Verifies jQuery loaded
                });
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}