using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;

namespace _152120191023_WebBasedTechnologies_Hw4
{
    /// <summary>
    /// ProfileImageHandler için özet açıklama
    /// </summary>
    public class ProfileImageHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            int userId = 0;
            if (int.TryParse(context.Request.QueryString["UserID"], out userId))
            {
                string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
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
                        context.Response.ContentType = "image/jpeg";
                        context.Response.BinaryWrite(imgData);
                    }
                    else
                    {
                        string defaultImagePath = context.Server.MapPath("~/Images/default_profile.png");
                        context.Response.ContentType = "image/png";
                        context.Response.WriteFile(defaultImagePath);
                    }
                }
            }
        }

        public bool IsReusable => false;
    }
}