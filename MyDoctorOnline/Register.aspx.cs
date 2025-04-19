using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;


namespace _152120191023_WebBasedTechnologies_Hw4
{
    public partial class Register : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Doctor
                rblRole.Items[0].Attributes.Add("class", "doctorChoice");
                // Patient
                rblRole.Items[1].Attributes.Add("class", "patientChoice");
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim(); // Gerçek projede şifre güvenliği için hash kullanılmalı.
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string role = rblRole.SelectedValue;

            if (string.IsNullOrEmpty(role))
            {
                lblMessage.Text = "Lütfen bir rol seçiniz.";
                return;
            }

            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                string query = "INSERT INTO USERS ([Username], [Password], [Role], [FullName], [Email]) VALUES (?,?,?,?,?)";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", username);
                cmd.Parameters.AddWithValue("?", password);
                cmd.Parameters.AddWithValue("?", role);
                cmd.Parameters.AddWithValue("?", fullName);
                cmd.Parameters.AddWithValue("?", email);
                conn.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    // Kayıt sonrası otomatik olarak Login sayfasına yönlendirme:
                    Response.Redirect("Login.aspx");
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Hata: " + ex.Message;
                }
            }
        }
    }
}