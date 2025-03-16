using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using 水水水果API.DTO.Email;

namespace 水水水果API.Services
{
    public class MailService : IMailHelper
    {
        private readonly MailModel _mailSetting;
        public MailService(IOptions<MailModel> model)
        {
            _mailSetting = model.Value;
        }

        public async Task SendEmailiAsync(MailRequestDTO mailRequest)
        {
            // 寄/發送人的資訊
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSetting.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject; // 主題
            //發送內容
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null) // 事處理檔案的部分
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body; // 郵件訊息內容
            email.Body = builder.ToMessageBody();
            //=============================================================
            //smtp的寄送方式(使用appsetting.json的資訊)
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSetting.Host, int.Parse(_mailSetting.Port), SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSetting.Mail, _mailSetting.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
