using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;


namespace _152120191023_WebBasedTechnologies_Hw4
{

    public partial class Login : Page
    {
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim(); // Gerçek projede şifre hash'i kullanılmalıdır.
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                string query = "SELECT * FROM USERS WHERE Username = ? AND Password = ?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", username);
                cmd.Parameters.AddWithValue("?", password);
                conn.Open();
                OleDbDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Session["UserID"] = reader["UserID"];
                    Session["Username"] = reader["Username"];
                    Session["Role"] = reader["Role"];
                    Response.Redirect("Dashboard.aspx");
                }
                else
                {
                    lblMessage.Text = "Geçersiz kullanıcı adı veya şifre.";
                }
            }
        }
    }

}