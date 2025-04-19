using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;

namespace _152120191023_WebBasedTechnologies_Hw4
{
    public partial class Chat : Page
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
                LoadUserList();
            }
        }

        private void LoadUserList()
        {
            // Kullanıcının rolüne göre karşı roldeki kullanıcıları getir
            string currentRole = Session["Role"].ToString();  // "Doctor" veya "Patient"
            string targetRole = (currentRole == "Doctor") ? "Patient" : "Doctor";

            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                string query = "SELECT UserID, [FullName] FROM USERS WHERE [Role] = ?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", targetRole);
                conn.Open();
                using (OleDbDataReader dr = cmd.ExecuteReader())
                {
                    ddlUsers.DataSource = dr;
                    ddlUsers.DataTextField = "FullName";
                    ddlUsers.DataValueField = "UserID";
                    ddlUsers.DataBind();
                }
            }

            ddlUsers.Items.Insert(0, new ListItem("Kişi Seçiniz", "0"));  // Varsayılan
        }

        protected void ddlUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlUsers.SelectedValue != "0")
            {
                // Sohbet görüntülendiğinde, diğer kullanıcı tarafından gönderilmiş okunmamış mesajları güncelle
                MarkMessagesAsRead();
                // Seçilen kullanıcı ID'sini bir yerde tutabilir veya
                // sohbet geçmişini burada yükleyebilirsin.
                LoadConversation();
            }
        }

        private void LoadConversation()
        {
            int myId = Convert.ToInt32(Session["UserID"]);
            int otherId = Convert.ToInt32(ddlUsers.SelectedValue);

            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                string query = @"
            SELECT c.[SenderID],
                   c.[MessageText],
                   u.[Username] AS SenderName,
                   c.[TimeSent],
                   c.[IsRead],
                   c.[FileData],
                   c.[FileName] 
            FROM ([CHAT] AS c
            INNER JOIN [USERS] AS u ON c.[SenderID] = u.[UserID])
            WHERE 
                (c.[SenderID] = ? AND c.[ReceiverID] = ?)
                OR
                (c.[SenderID] = ? AND c.[ReceiverID] = ?)
            ORDER BY c.[TimeSent]";

                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", myId);
                cmd.Parameters.AddWithValue("?", otherId);
                cmd.Parameters.AddWithValue("?", otherId);
                cmd.Parameters.AddWithValue("?", myId);

                conn.Open();
                using (OleDbDataReader dr = cmd.ExecuteReader())
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();

                    while (dr.Read())
                    {
                        int senderId = Convert.ToInt32(dr["SenderID"]);
                        string senderName = dr["SenderName"].ToString();
                        string messageText = dr["MessageText"].ToString();
                        DateTime timeSent = Convert.ToDateTime(dr["TimeSent"]);
                        bool isRead = Convert.ToBoolean(dr["IsRead"]);

                        string formattedTime = timeSent.ToString("HH:mm");

                        // Belirleyelim: Gönderen (myId) veya Alıcı (diğer kişi)
                        // Profil resmi için URL; her iki durumda da ProfileImageHandler.ashx kullanıyoruz.
                        string profileImageUrl = "ProfileImageHandler.ashx?UserID=" + senderId;

                        // Okundu durumu için Font Awesome check ikonu
                        string checkIconClass = isRead ? "fa-solid fa-check checkIcon read" : "fa-solid fa-check checkIcon unread";

                        if (senderId == myId)
                        {
                            // Gönderen (Sender) mesaj: Sağ hizalı
                            sb.Append("<div class='message senderMessage'>");

                            // Mesaj içeriğini kapsayan container
                            sb.Append("  <div class='messageContent'>");
                            // Sender ismi, mesajın altında (sizin isteğinize göre aynı satırda da gösterebilirsiniz)
                            sb.Append("    <div class='senderName'>" + senderName + "</div>");
                            sb.Append("    <div class='textAndInfo'>");
                            byte[] fileData = dr["FileData"] != DBNull.Value ? (byte[])dr["FileData"] : null;
                            string fileName = dr["FileName"] != DBNull.Value ? dr["FileName"].ToString() : "";

                            if (fileData != null)
                            {
                                string base64File = Convert.ToBase64String(fileData);
                                string downloadLink = $"data:application/pdf;base64,{base64File}";

                                sb.Append("<div class='fileAttachment'>");
                                sb.Append($"<a download='{fileName}' href='{downloadLink}'><i class='fa-solid fa-file'></i> {fileName}</a>");
                                sb.Append("</div>");
                            }


                            sb.Append("      <span class='text'>" + messageText + "</span>");
                            sb.Append("      <span class='timeAndCheck'>");
                            // Sender mesajlarında; zaman bilgisinin solunda check icon
                            sb.Append("        <span class='timestamp'>" + formattedTime + "</span>");

                            sb.Append("        <i class='" + checkIconClass + "'></i>");
                            sb.Append("      </span>");
                            sb.Append("    </div>");
                            
                            sb.Append("  </div>");

                            // Profil resmi: Sender mesajlarında mesajın sağında yer alır
                            sb.Append("  <div class='profileImageContainer'>");
                            sb.Append("      <img src='" + profileImageUrl + "' class='profileImage' alt='Profil Resmi' />");
                            sb.Append("  </div>");
                            sb.Append("</div>");
                        }
                        else
                        {
                            // Receiver mesaj: Sol hizalı
                            sb.Append("<div class='message receiverMessage'>");

                            // Profil resmi: Receiver mesajlarında mesajın solunda yer alır
                            sb.Append("  <div class='profileImageContainer'>");
                            sb.Append("      <img src='" + profileImageUrl + "' class='profileImage ' alt='Profil Resmi' />");
                            sb.Append("  </div>");

                            // Mesaj içeriğini kapsayan container
                            sb.Append("  <div class='messageContent'>");
                            // Receiver ismi
                            sb.Append("    <div class='receiverName'>" + senderName + "</div>");
                            sb.Append("    <div class='textAndInfo'>");
                            byte[] fileData = dr["FileData"] != DBNull.Value ? (byte[])dr["FileData"] : null;
                            string fileName = dr["FileName"] != DBNull.Value ? dr["FileName"].ToString() : "";

                            if (fileData != null)
                            {
                                string base64File = Convert.ToBase64String(fileData);
                                string downloadLink = $"data:application/pdf;base64,{base64File}";

                                sb.Append("<div class='fileAttachment'>");
                                sb.Append($"<a download='{fileName}' href='{downloadLink}'><i class='fa-solid fa-file'></i> {fileName}</a>");
                                sb.Append("</div>");
                            }


                            // Receiver mesajlarında, isim ve zaman bilgileri mesaj metni ile aynı satırda; zamanın sağında check icon
                            sb.Append("      <span class='text'>" + messageText + "</span>");
                            sb.Append("      <span class='timeAndCheck'>");
                            sb.Append("        <span class='timestamp'>" + formattedTime + "</span>");
                            sb.Append("        <i class='" + checkIconClass + "'></i>");
                            sb.Append("      </span>");
                            sb.Append("    </div>");
                            sb.Append("  </div>");
                            sb.Append("</div>");
                        }
                    }

                    litMessages.Text = sb.ToString();

                }
            }
        }



        protected void btnSend_Click(object sender, EventArgs e)
        {
            // .NET tarafında bir OnClick ile veritabanına kaydetme
            // (SignalR ile client-side da gönderebilirsin, senin tercihin)

            if (ddlUsers.SelectedValue == "0")
            {
                // Lütfen kişi seçin uyarısı
                return;
            }
            int myId = Convert.ToInt32(Session["UserID"]);
            int otherId = Convert.ToInt32(ddlUsers.SelectedValue);
            string message = txtMessage.Text.Trim();

            // Mesajı veritabanına kaydet
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                string query = @"INSERT INTO CHAT ([SenderID], [ReceiverID], [MessageText], [TimeSent], [IsRead]) 
                     VALUES (?,?,?,?,?)";
                OleDbCommand cmd = new OleDbCommand(query, conn);

                // Parametreleri açık veri türü ile ekleyelim
                cmd.Parameters.Add("?", OleDbType.Integer).Value = myId;  // SenderID
                cmd.Parameters.Add("?", OleDbType.Integer).Value = otherId;  // ReceiverID
                                                                             // Eğer MessageText sütunu Memo tipinde ise LongVarWChar kullanmak uygundur:
                cmd.Parameters.Add("?", OleDbType.LongVarWChar).Value = message;  // MessageText
                cmd.Parameters.Add("?", OleDbType.Date).Value = DateTime.Now;  // TimeSent
                cmd.Parameters.Add("?", OleDbType.Boolean).Value = false;  // IsRead

                conn.Open();
                cmd.ExecuteNonQuery();
            }


            // Mesaj gönderildikten sonra textbox'ı temizleyebilir,
            // güncel sohbeti tekrar yükleyebilirsin
            txtMessage.Text = "";
            LoadConversation();
        }
        private void MarkMessagesAsRead()
        {
            int myId = Convert.ToInt32(Session["UserID"]);
            int otherId = Convert.ToInt32(ddlUsers.SelectedValue);

            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                // Receiver (benim) olan, diğer kullanıcı tarafından gönderilmiş ve henüz okunmamış mesajların güncellenmesi
                string queryUpdate = @"UPDATE CHAT 
                               SET IsRead = TRUE 
                               WHERE [ReceiverID] = ? AND [SenderID] = ? AND IsRead = FALSE";
                OleDbCommand cmd = new OleDbCommand(queryUpdate, conn);
                // Parametre sırasına dikkat edin: önce ReceiverID (benim), sonra mesajı gönderenin UserID
                cmd.Parameters.AddWithValue("?", myId);
                cmd.Parameters.AddWithValue("?", otherId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

    }
}