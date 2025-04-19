# My-Doctor-Online Web Site

## Dosyaların Eklenmesi

1. **Yeni Proje Oluşturma**  
   - Visual Studio’yu açın ve **File → New → Project** yolunu izleyin.  
   - “ASP.NET Web Application (.NET Framework)” şablonunu seçin, .NET Framework 4.6.2’yi hedefleyin.  
   - Proje tipi olarak **Web Forms**’u işaretleyin ve projeyi oluşturun.

2. **Sizin Sağladığınız Dosyaların Eklenmesi**  
   - Çözüm Gezgini’nde proje adına sağ tıklayıp **Add → Existing Item** deyin.  
   - `.aspx`, `.aspx.cs`, `.ashx`, `Site.Master` ve `Web.config` gibi tüm dosyalarınızı seçip projeye dahil edin.  
   - Dosyalar `~/Pages`, `~/` veya istediğiniz klasöre eklenebilir; `Web.config` kök dizinde kalmalı.

## Gerekli NuGet Paketleri ve Kütüphaneler

1. **Access Veritabanı Desteği (System.Data.OleDb)**  
   - Proje `.NET Framework 4.6.2`’yi hedeflediği için `System.Data` namespace’i içinde gelen `System.Data.OleDb` assembly’sine doğrudan referansınız olmalıdır.  
   - Eğer referans eksikse, **Solution Explorer → References → Add Reference… → Assemblies → Framework** altında `System.Data`’yı işaretleyin.

2. **SignalR**  
   - **Solution Explorer** → projenize sağ tıklayın → **Manage NuGet Packages…**  
   - **Browse** sekmesinde `Microsoft.AspNet.SignalR` paketini aratıp **Install** edin.

3. **OWIN ve Host**  
   - Aynı ekrandan `Microsoft.Owin` ve `Microsoft.Owin.Host.SystemWeb` paketlerini yükleyin.  
   - Proje kökünde `Startup.cs` dosyası varsa bu paketler otomatik olarak OWIN pipeline’ını devreye alacaktır.

4. **Diğer Paketler (İsteğe Bağlı)**  
   - `Microsoft.Owin.Cors` (CORS desteği)  
   - `Microsoft.Azure.Storage.Blob` (Gelecekte bulut tabanlı dosya depolama için)

5. **jQuery 1.6.4**  
   - Projede eski bir Web Forms uygulaması olduğundan jQuery’yi NuGet’ten değil, projedeki **Scripts/jquery-1.6.4.min.js** dosyası ile kullanıyoruz.  
   - Eğer internetten güncel sürüm indirmek isterseniz, aynı ada sahip yeni bir `.js` dosyasını `Scripts` klasörüne atıp **Site.Master** ve **Chat.aspx** içinde referans yolunu güncelleyebilirsiniz.

> **Not:** NuGet ile yüklediğiniz paketler `packages.config` veya `*.csproj` dosyanıza otomatik eklenecektir. Script dosyalarını ve Assembly referanslarını kontrol etmeyi ihmal etmeyin.  


## Kurulum ve Çalıştırma

1. **Veritabanı Ayarları**  
   - Proje kökünde yer alan `App_Data\HW4_ASP_Chat.accdb` dosyasını kullanın veya kendi Access dosyanızı oluşturun.  
   - Access dosyanızda iki tablo olmalı:  
     - **USERS** (UserID, Username, Password, Role, FullName, Email, ProfilePicture OLE Object)  
     - **CHAT** (MessageID, SenderID, ReceiverID, MessageText, TimeSent, IsRead, FileData OLE Object, FileName)  
   - `Web.config` içindeki `DBConnection` connection string’ini aşağıdaki gibi güncelleyin:
     ```xml
     <connectionStrings>
       <add name="DBConnection" 
            connectionString="Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\HW4_ASP_Chat.accdb;Persist Security Info=False;" />
     </connectionStrings>
     ```

2. **Projeyi Çalıştırma**  
   1. Çözümü kaydedip **Restore NuGet Packages** komutunu çalıştırın.  
   2. **F5** ile debug modunda başlatın veya **Ctrl+F5** ile hata ayıklamadan çalıştırın.  
   3. Tarayıcı penceresinde açılan URL’e gidin (ör. `https://localhost:44300/`), önce **Login**, yoksa **Register** sayfasını kullanın.

---

## Ek Özellikler

- **Gerçek Zamanlı Mesajlaşma**  
  SignalR Hub ile metin ve dosya iletimi anlık gerçekleşir; gönderildiği anda her iki uç da güncellenir.

- **“User is typing…” Göstergesi**  
  Karşı taraf mesaj yazarken sağ alt köşede `<kullanıcı adı> is typing…` bildirimi belirir.

- **Mesaj Arama**  
  Sohbet içinde anahtar kelime veya dosya adı arayıp, bulunan mesajın üzerine kaydırma ve vurgulama yapar.

- **Dosya Paylaşımı**  
  PDF ve diğer dosyalar anında gönderilir; mesaj içindeki dosya simgesine tıklayınca indirme başlar.

- **Kullanıcı Profili**  
  Profil resmi yükleme, kullanıcı adı/şifre/ad‑soyad/email düzenleme imkânı.

- **Okundu/Beklemede Durum**  
  Mavi tik = okundu, gri tik = okunmadı durum bilgisini gösterir.

- **Responsive Tasarım**  
  Bootstrap 5 ile tüm cihazlarda uyumlu form kontrolleri, butonlar ve grid yapısı.

---

## Bilinen Sorunlar ve Kısıtlamalar

- **Büyük Dosya Boyutları**  
  Dosya boyutu sınırlaması yok; çok büyük yüklemeler performans sorununa yol açabilir.

- **Eski jQuery Sürümü**  
  jQuery 1.6.4 kullanılıyor; modern `.on()` metodu desteklenmez, bazı eklentiler çalışmayabilir.

- **Grup Sohbeti Yok**  
  Şu anda birden fazla kişiyi kapsayan grup sohbet desteği bulunmuyor.

- **Eşzamanlı Access Erişimi**  
  Birden fazla kullanıcı aynı anda yazma yapınca Access “database is locked” hatası verebilir.

- **Şifre Saklama**  
  Parolalar düz metin olarak saklanıyor; gerçek projede hash+salt mekanizması önerilir.

---
