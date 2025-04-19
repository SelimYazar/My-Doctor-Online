using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using System.IO;
using Microsoft.AspNet.SignalR;


namespace _152120191023_WebBasedTechnologies_Hw4
{
    /// <summary>
    /// SendMessageHandler için özet açıklama
    /// </summary>
    public class SendMessageHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            int senderId = Convert.ToInt32(context.Request["senderId"]);
            int receiverId = Convert.ToInt32(context.Request["receiverId"]);
            string message = context.Request["message"];
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;

            byte[] fileData = null;
            string fileName = "";

            if (context.Request.Files.Count > 0)
            {
                HttpPostedFile file = context.Request.Files[0];
                fileName = Path.GetFileName(file.FileName);
                using (BinaryReader br = new BinaryReader(file.InputStream))
                {
                    fileData = br.ReadBytes(file.ContentLength);
                }
            }

            DateTime currentTime = DateTime.Now; // zamanı burada tutalım
            string formattedTime = currentTime.ToString("HH:mm");

            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                string query = @"INSERT INTO CHAT ([SenderID], [ReceiverID], [MessageText], [TimeSent], [IsRead], [FileData], [FileName]) 
                             VALUES (?,?,?,?,?,?,?)";
                OleDbCommand cmd = new OleDbCommand(query, conn);

                cmd.Parameters.Add("?", OleDbType.Integer).Value = senderId;
                cmd.Parameters.Add("?", OleDbType.Integer).Value = receiverId;
                cmd.Parameters.Add("?", OleDbType.LongVarWChar).Value = message;
                cmd.Parameters.Add("?", OleDbType.Date).Value = currentTime;
                cmd.Parameters.Add("?", OleDbType.Boolean).Value = false;

                if (fileData != null)
                {
                    cmd.Parameters.Add("?", OleDbType.LongVarBinary).Value = fileData;
                    cmd.Parameters.Add("?", OleDbType.VarWChar, 255).Value = fileName;
                }
                else
                {
                    cmd.Parameters.Add("?", OleDbType.LongVarBinary).Value = DBNull.Value;
                    cmd.Parameters.Add("?", OleDbType.VarWChar, 255).Value = DBNull.Value;
                }

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // SignalR ile tüm istemcilere mesajı anında ilet!
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            string senderName = GetSenderName(senderId, connStr);
            // SendMessageHandler.ashx içindeki çağrıyı şöyle güncelle:
            hubContext.Clients.All.receiveMessage(senderId, senderName, message, formattedTime, fileData != null, fileName, fileData != null ? Convert.ToBase64String(fileData) : "");

            context.Response.ContentType = "text/plain";
            context.Response.Write("Success");
        }

        // Sender'ın ismini alalım
        private string GetSenderName(int senderID, string connStr)
        {
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                string query = "SELECT Username FROM USERS WHERE UserID = ?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", senderID);
                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? result.ToString() : "Bilinmeyen";
            }
        }

        public bool IsReusable => false;
    }
}