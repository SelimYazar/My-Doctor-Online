using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _152120191023_WebBasedTechnologies_Hw4
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string page = Path.GetFileName(Request.Url.AbsolutePath).ToLower();

                // Mevcut class’ı alıp, active sınıfını ekliyoruz
                if (page == "login.aspx")
                    lnkLogin.Attributes["class"] += " active";
                else if (page == "register.aspx")
                    lnkRegister.Attributes["class"] += " active";
            }
        }
    }
}