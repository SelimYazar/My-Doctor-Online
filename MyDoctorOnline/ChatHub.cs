using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Data.OleDb;
using System.Configuration;


namespace _152120191023_WebBasedTechnologies_Hw4
{
    public class ChatHub : Hub
    {
        string connStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;

        public void SendMessage(int senderID, int receiverID, string message, string formattedTime)
        {
            // Veritabanına kaydet
            SaveMessageToDatabase(senderID, receiverID, message);

            // Alıcıya ve gönderenin kendisine gönder
            // “User(...)” methodunu kullanabilmek için, kimlik doğrulama veya Connection/Username mapping gerekli.
            // Burada basitçe broadcast yapabilir veya Clients.All diyebilirsin
            string senderName = GetSenderName(senderID);
            Clients.All.receiveMessage(senderID, senderName, message, formattedTime);
        }
        private string GetSenderName(int senderID)
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

        public void MarkMessagesAsRead(int senderID, int receiverID)
        {
            // Veritabanında ilgili mesajları okundu olarak güncelleme işlemi
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                string query = @"UPDATE CHAT 
                         SET IsRead = TRUE 
                         WHERE [ReceiverID] = ? AND [SenderID] = ? AND IsRead = FALSE";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                // Parametre sırası: burada ReceiverID, yani mesajı alanın ID'si; ardından mesajı gönderenin UserID'si
                cmd.Parameters.AddWithValue("?", receiverID);
                cmd.Parameters.AddWithValue("?", senderID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // Bağlı tüm istemcilere (veya belirli istemciler hedeflenebilir)
            // okundu durumunun güncellendiğine dair bir mesaj gönderiyoruz
            Clients.All.updateReadStatus(senderID, receiverID);
        }

        private void SaveMessageToDatabase(int senderID, int receiverID, string message)
        {
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                string query = @"INSERT INTO CHAT ([SenderID], [ReceiverID], [MessageText], [TimeSent], [IsRead]) 
                 VALUES (?,?,?,?,?)";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.Add("?", OleDbType.Integer).Value = senderID;
                cmd.Parameters.Add("?", OleDbType.Integer).Value = receiverID;
                cmd.Parameters.Add("?", OleDbType.LongVarWChar).Value = message;
                cmd.Parameters.Add("?", OleDbType.Date).Value = DateTime.Now;
                cmd.Parameters.Add("?", OleDbType.Boolean).Value = false;
                conn.Open();
                cmd.ExecuteNonQuery();

            }
        }
        // Kullanıcı yazmaya başladı bilgisini diğer istemcilere bildirir.
        public void UserTyping(int senderId, int receiverId, string senderName)
        {
            Clients.All.userTyping(senderId, receiverId, senderName);
        }

        // Kullanıcı yazmayı durdurdu bilgisini diğer istemcilere bildirir.
        public void UserStoppedTyping(int senderId, int receiverId)
        {
            Clients.All.userStoppedTyping(senderId, receiverId);
        }
        public void SearchMessage(int userId, int otherUserId, string searchTerm)
        {
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                string query = @"
            SELECT TOP 1 [MessageText], [TimeSent] FROM CHAT 
            WHERE ((SenderID = ? AND ReceiverID = ?) OR (SenderID = ? AND ReceiverID = ?))
            AND [MessageText] LIKE ?
            ORDER BY [TimeSent] ASC"; // ilk eşleşen mesajı al

                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.Add("?", OleDbType.Integer).Value = userId;
                cmd.Parameters.Add("?", OleDbType.Integer).Value = otherUserId;
                cmd.Parameters.Add("?", OleDbType.Integer).Value = otherUserId;
                cmd.Parameters.Add("?", OleDbType.Integer).Value = userId;
                cmd.Parameters.Add("?", OleDbType.VarWChar).Value = "%" + searchTerm + "%";

                conn.Open();
                using (OleDbDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        string foundMessage = dr["MessageText"].ToString();
                        DateTime foundTime = Convert.ToDateTime(dr["TimeSent"]);
                        string formattedTime = foundTime.ToString("HH:mm");
                        Clients.Caller.messageFound(foundMessage, formattedTime);
                    }
                    else
                    {
                        Clients.Caller.messageNotFound();
                    }
                }
            }
        }


    }

}