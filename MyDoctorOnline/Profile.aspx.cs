using System;
using System.Data.OleDb;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace _152120191023_WebBasedTechnologies_Hw4
{
    public partial class Profile : Page
    {
        string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            if (!IsPostBack)
            {
                LoadProfileImage();
                LoadUserData();  // Kullanıcının Username, Password, FullName, Email bilgilerini getir
            }
        }

        private void LoadProfileImage()
        {
            int userId = Convert.ToInt32(Session["UserID"]);
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                string query = "SELECT ProfilePicture FROM USERS WHERE UserID = ?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", userId);
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != DBNull.Value && result != null)
                {
                    byte[] imgData = (byte[])result;
                    string base64String = Convert.ToBase64String(imgData, 0, imgData.Length);
                    imgProfile.ImageUrl = "data:image/jpeg;base64," + base64String;
                }
                else
                {
                    imgProfile.ImageUrl = "~/Images/default_profile.png";
                }
            }
        }

        // Kullanıcı bilgilerini USERS tablosundan çek
        private void LoadUserData()
        {
            int userId = Convert.ToInt32(Session["UserID"]);
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                string query = "SELECT Username, [Password], FullName, Email FROM USERS WHERE UserID = ?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", userId);
                conn.Open();
                using (OleDbDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        txtUsername.Text = dr["Username"].ToString();
                        hfOldUsername.Value = dr["Username"].ToString();

                        txtPassword.Text = dr["Password"].ToString();
                        hfOldPassword.Value = dr["Password"].ToString();

                        txtFullName.Text = dr["FullName"].ToString();
                        hfOldFullName.Value = dr["FullName"].ToString();

                        txtEmail.Text = dr["Email"].ToString();
                        hfOldEmail.Value = dr["Email"].ToString();
                    }
                }
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (fuProfilePic.HasFile)
            {
                string ext = Path.GetExtension(fuProfilePic.FileName).ToLower();
                if (ext == ".png" || ext == ".jpg" || ext == ".jpeg")
                {
                    byte[] fileData = fuProfilePic.FileBytes;
                    int userId = Convert.ToInt32(Session["UserID"]);

                    using (OleDbConnection conn = new OleDbConnection(connStr))
                    {
                        string query = "UPDATE USERS SET ProfilePicture = ? WHERE UserID = ?";
                        OleDbCommand cmd = new OleDbCommand(query, conn);
                        cmd.Parameters.Add("?", System.Data.OleDb.OleDbType.Binary).Value = fileData;
                        cmd.Parameters.Add("?", System.Data.OleDb.OleDbType.Integer).Value = userId;
                        conn.Open();
                        try
                        {
                            cmd.ExecuteNonQuery();
                            lblMessage.Text = "Profil resmi güncellendi.";
                        }
                        catch (Exception ex)
                        {
                            lblMessage.Text = "Hata: " + ex.Message;
                        }
                    }
                    LoadProfileImage();
                }
                else
                {
                    lblMessage.Text = "Lütfen geçerli bir resim dosyası yükleyiniz (.png, .jpg, .jpeg).";
                }
            }
            else
            {
                lblMessage.Text = "Herhangi bir dosya seçilmedi.";
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Eski değerler (HiddenField)
            string oldUsername = hfOldUsername.Value;
            string oldPassword = hfOldPassword.Value;
            string oldFullName = hfOldFullName.Value;
            string oldEmail = hfOldEmail.Value;

            // Yeni değerler
            string newUsername = txtUsername.Text.Trim();
            string newPassword = txtPassword.Text.Trim();
            string newFullName = txtFullName.Text.Trim();
            string newEmail = txtEmail.Text.Trim();

            // Değişiklik kontrolü
            bool changed =
                (newUsername != oldUsername) ||
                (newPassword != oldPassword) ||
                (newFullName != oldFullName) ||
                (newEmail != oldEmail);

            if (!changed)
            {
                lblMessage.Text = "Hiçbir değişiklik yapılmadı.";
                return;
            }

            int userId = Convert.ToInt32(Session["UserID"]);
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                string query = @"UPDATE USERS 
                                 SET [Username] = ?, [Password] = ?, [FullName] = ?, [Email] = ?
                                 WHERE [UserID] = ?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", newUsername);
                cmd.Parameters.AddWithValue("?", newPassword);
                cmd.Parameters.AddWithValue("?", newFullName);
                cmd.Parameters.AddWithValue("?", newEmail);
                cmd.Parameters.AddWithValue("?", userId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            lblMessage.Text = "Profil bilgileriniz güncellendi.";
            // Yeniden yükleyelim, textbox'larda yeni bilgileri görelim
            LoadUserData();
        }
    }
}
