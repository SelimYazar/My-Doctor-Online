using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _152120191023_WebBasedTechnologies_Hw4
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string page = System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToLower();

                if (page == "dashboard.aspx")
                {
                    // HtmlAnchor olduğu için CssClass yerine Attributes["class"]
                    lnkDashboard.Attributes["class"] += " active";
                }
                else if (page == "profile.aspx")
                {
                    lnkProfile.Attributes["class"] += " active";
                }
            }
        }
    }
}