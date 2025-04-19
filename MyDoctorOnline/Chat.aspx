<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Chat.aspx.cs" Inherits="_152120191023_WebBasedTechnologies_Hw4.Chat" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="mb-3 text-warning fw-50">Chat Arayüzü</h2>
    
    <!-- 1) Kullanıcı Seçimi ve Arama Aynı Satırda -->
    <div class="d-flex justify-content-between align-items-center mb-3 w-100">
        <!-- Solda: Kullanıcı Listesi -->
        <asp:DropDownList ID="ddlUsers" runat="server"
            CssClass="form-control w-auto"
            AutoPostBack="true"
            OnSelectedIndexChanged="ddlUsers_SelectedIndexChanged">
        </asp:DropDownList>

        <!-- Sağda: Arama ve Uyarı -->
        <div class="d-flex align-items-center">
            <div id="searchWarning" class="text-warning small me-2"></div>
            <input type="text" id="txtSearch" placeholder="Mesaj ara..."
                   class="form-control form-control-sm me-2 text-primary"
                   style="width:200px;" />
            <button id="btnSearch" type="button"
                    class="btn btn-lg btn-primary align-self-stretch">
                Ara
            </button>
        </div>
    </div>
    
    <!-- 2) Chat Paneli: Yüksekliği 50vh, scroll bar -->
    <div id="chatContainer" class="w-100 border p-2"
         style="height:50vh; overflow-y:auto;">
        <div id="chatPanelContainer">
            <asp:Literal ID="litMessages" runat="server"></asp:Literal>
        </div>
        <div id="typingIndicator" class="text-muted large text-end mt-1 mb-2"></div>
    </div>
    
    <!-- 3) Mesaj Giriş ve Gönderme Aynı Satırda -->
    <div class="d-flex align-items-center mt-2 w-100 mb-3">
        <div class="position-relative flex-grow-1 me-2">
            <asp:TextBox ID="txtMessage" runat="server"
                CssClass="form-control ps-5 text-primary"
                placeholder="Mesajınızı yazın..."></asp:TextBox>
            <label for="fileUpload"
                   class="position-absolute top-50 start-0 translate-middle-y ps-2 text-secondary"
                   style="cursor:pointer;">
                <i class="fa-solid fa-paperclip fa-2x text-primary"></i>
            </label>
            <input type="file" id="fileUpload"
                   style="display:none;" accept=".pdf" />
        </div>
        <asp:Button ID="btnSend" runat="server"
            Text="Gönder"
            OnClientClick="return sendMessage();"
            UseSubmitBehavior="false"
            CssClass="btn btn-lg btn-primary align-self-stretch" />
    </div>
    
    <!-- Gerekli JavaScript / SignalR kütüphaneleri -->
 <!-- jQuery ve SignalR Script’leri -->
<script src="Scripts/jquery-1.6.4.min.js"></script>
<script src="Scripts/jquery.signalR-2.4.3.min.js"></script>
<script src="signalr/hubs"></script>

<script type="text/javascript">
    // 1. Hub proxy nesnesini tanımlayın

    var chatHub = $.connection.chatHub;
    $(document).ready(function () {
        // Sayfa açıldığında / postback sonrası en alta kaydır
        var $c = $("#chatContainer");
        $c.scrollTop($c[0].scrollHeight);
    });


    // 2. Hub bağlantısını sayfa yüklendiğinde başlatın (globalde yalnızca bir kez çalışması gerekiyor)
    $.connection.hub.start().done(function () {
        console.log("SignalR bağlantısı başarıyla başlatıldı.");
    }).fail(function (err) {
        console.log("SignalR bağlantısı başlatılamadı: ", err);
    });

    // 3. Sunucudan gelen mesajları yakalayın:
    chatHub.client.receiveMessage = function (senderId, senderName, message, formattedTime, hasFile, fileName, fileBase64) {
        var currentUserId = '<%= Session["UserID"] %>';
        var messageHtml = "";

        if (senderId == currentUserId) {
            // Gönderen (sağ hizalı) mesaj HTML’i
            messageHtml += "<div class='message senderMessage'>";
            messageHtml += "  <div class='messageContent'><div class='senderName'>" + senderName + "</div>";
            messageHtml += "    <div class='textAndInfo'><span class='text'>" + message + "</span>";
            messageHtml += "      <span class='timeAndCheck'><span class='timestamp'>" + formattedTime +
                "</span><i class='fa-solid fa-check checkIcon read'></i></span>";
            messageHtml += "    </div>";

            // Dosya varsa ekle
            if (hasFile) {
                var downloadLink = "data:application/pdf;base64," + fileBase64;
                messageHtml += "<div class='fileAttachment'>";
                messageHtml += "<a download='" + fileName + "' href='" + downloadLink + "'><i class='fa-solid fa-file'></i> " + fileName + "</a>";
                messageHtml += "</div>";
            }

            messageHtml += "  </div>";
            messageHtml += "  <div class='profileImageContainer'><img src='ProfileImageHandler.ashx?UserID=" +
                senderId + "' class='profileImage' alt='Profil Resmi' /></div>";
            messageHtml += "</div>";

        } else {
            // Alıcı (sol hizalı) mesaj HTML’i
            messageHtml += "<div class='message receiverMessage'>";
            messageHtml += "  <div class='profileImageContainer'><img src='ProfileImageHandler.ashx?UserID=" +
                senderId + "' class='profileImage' alt='Profil Resmi' /></div>";
            messageHtml += "  <div class='messageContent'><div class='receiverName'>" + senderName + "</div>";
            messageHtml += "    <div class='textAndInfo'><span class='text'>" + message + "</span>";
            messageHtml += "      <span class='timeAndCheck'><span class='timestamp'>" + formattedTime +
                "</span><i class='fa-solid fa-check checkIcon read'></i></span>";
            messageHtml += "    </div>";

            // Dosya varsa ekle
            if (hasFile) {
                var downloadLink = "data:application/pdf;base64," + fileBase64;
                messageHtml += "<div class='fileAttachment'>";
                messageHtml += "<a download='" + fileName + "' href='" + downloadLink + "'><i class='fa-solid fa-file'></i> " + fileName + "</a>";
                messageHtml += "</div>";
            }

            messageHtml += "  </div>";
            messageHtml += "</div>";
        }

        $("#chatPanelContainer").append(messageHtml);
        var $c = $("#chatContainer");
        $c.scrollTop($c[0].scrollHeight);
    };


    // 4. Mesaj gönderme fonksiyonu, bağlantı zaten başlatıldıktan sonra çalışır:
    window.sendMessage = function () {
        var message = $("#<%= txtMessage.ClientID %>").val();
        var receiverId = $("#<%= ddlUsers.ClientID %>").val();

    if (receiverId == "0" || (message.trim() === "" && $("#fileUpload")[0].files.length === 0)) {
        alert("Lütfen mesaj yazın veya dosya ekleyin!");
        return false;
    }

    var formData = new FormData();
    formData.append("senderId", "<%= Session["UserID"] %>");
    formData.append("receiverId", receiverId);
    formData.append("message", message);

    if ($("#fileUpload")[0].files.length > 0) {
        formData.append("file", $("#fileUpload")[0].files[0]);
    }

    $.ajax({
        url: "SendMessageHandler.ashx",
        type: "POST",
        data: formData,
        contentType: false,
        processData: false,
        success: function () {
            $("#<%= txtMessage.ClientID %>").val("");
            $("#fileUpload").val("");

            var $c = $("#chatContainer");
            $c.scrollTop($c[0].scrollHeight);
        },
        error: function () {
            alert("Mesaj veya dosya gönderilemedi!");
        }
    });

        return false;
    };

    //typing
    $(document).ready(function () {

        var typingTimer;
        var typingInterval = 1000; // 1 saniye durunca yazıyor yazısını kaldır

        // txtMessage focus olduğunda (tıklandığında ve yazıldığında)
        $("#<%=txtMessage.ClientID%>").bind("focus keyup", function () {
            var receiverId = $("#<%=ddlUsers.ClientID%>").val();
        if (receiverId == "0") return;

        chatHub.server.userTyping(
            parseInt("<%= Session["UserID"] %>"),
            parseInt(receiverId),
            "<%= Session["Username"] %>"
        );

        clearTimeout(typingTimer);
        typingTimer = setTimeout(function () {
            chatHub.server.userStoppedTyping(
                parseInt("<%= Session["UserID"] %>"),
                parseInt(receiverId)
            );
        }, typingInterval);
    });

    // Yazma bittiğinde (odaktan çıktığında)
    $("#<%=txtMessage.ClientID%>").bind("blur", function () {
        var receiverId = $("#<%=ddlUsers.ClientID%>").val();
        if(receiverId == "0") return;

        chatHub.server.userStoppedTyping(
            parseInt("<%= Session["UserID"] %>"),
            parseInt(receiverId)
        );
    });

    // Sunucudan gelen yazma durumunu karşılayan fonksiyonlar
    chatHub.client.userTyping = function (senderId, receiverId, senderName) {
        var myId = parseInt("<%= Session["UserID"] %>");
        if (receiverId === myId) {
            $("#typingIndicator").text(senderName + " is typing...");
            var $c = $("#chatContainer");
            $c.scrollTop($c[0].scrollHeight);

        }
    };

    chatHub.client.userStoppedTyping = function (senderId, receiverId) {
            var myId = parseInt("<%= Session["UserID"] %>");
            if (receiverId === myId) {
                $("#typingIndicator").text("");
            }
        };

    });
    // Search state
    var searchMatches = [];
    var searchIndex = 0;

    $(document).ready(function () {
        // .unbind yerine .unbind('click'), .bind yerine .bind('click', fn)
        $("#btnSearch").unbind('click').bind('click', function () {
            var term = $("#txtSearch").val().trim().toLowerCase();
            var $warning = $("#searchWarning");
            var $container = $("#chatContainer");

            if (!term) {
                $warning.text("Lütfen arama yapmak için geçerli bir metin girin!");
                return;
            }
            $warning.text("");

            var $msgs = $("#chatPanelContainer .message");
            searchMatches = $msgs.filter(function () {
                var txt = $(this).find(".text").text().toLowerCase();
                var file = $(this).find(".fileAttachment a").text().toLowerCase();
                return txt.indexOf(term) !== -1 || file.indexOf(term) !== -1;
            });

            if (searchMatches.length === 0) {
                $warning.text("Eşleşen mesaj bulunamadı.");
                return;
            }

            // Mod alma sayesinde tekrar başa döner
            var idx = searchIndex % searchMatches.length;
            var $el = $(searchMatches[idx]);
            searchIndex++;

            // Önceki vurguları temizle
            $msgs.removeClass("search-highlight");

            // scrollTop: container içinde pozisyon
            var containerH = $container.height(),
                containerOffset = $container.offset().top,
                elOffset = $el.offset().top,
                elH = $el.outerHeight(),
                currentScroll = $container.scrollTop();

            // Mesajı tam ortalamak için:
            // elemOffset - containerOffset + currentScroll  => elem'in container içindeki tam topoğrafyası
            var scrollTo = (elOffset - containerOffset + currentScroll)
                - (containerH / 2 - elH / 2);

            $container.animate({ scrollTop: scrollTo }, 400);


            // Vurgula ve 2 saniye sonra kaldır
            $el.addClass("search-highlight");
            setTimeout(function () {
                $el.removeClass("search-highlight");
            }, 2000);
        });
    });


</script>




</asp:Content>
