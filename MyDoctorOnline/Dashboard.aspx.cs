using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _152120191023_WebBasedTechnologies_Hw4
{
    public partial class Dashboard : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            string username = Session["Username"].ToString();
            string role = Session["Role"].ToString();
            lblWelcome.Text = "Welcome " +  role + username  ;
            // Rol bazlı özel içerikler eklenebilir.
        }
    }
}