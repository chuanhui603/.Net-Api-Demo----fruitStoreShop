using System.Text;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;
using Moq;
using 水水水果API.Models.ConfigurationModel;
using 水水水果API.Models.DTO.Mail;
using WaterMailService = 水水水果API.Services.MailService;

namespace WaterFruitApi.Tests
{
    public class MailServiceTests
    {
        [Fact]
        public async Task SendEmailAsync_SendsWithAttachments()
        {
            var smtp = new Mock<ISmtpClient>();
            var sentMessages = new List<MimeMessage>();
            smtp.Setup(s => s.SendAsync(It.IsAny<MimeMessage>(), It.IsAny<CancellationToken>(), null))
                .Returns<MimeMessage, CancellationToken, ITransferProgress>((msg, ct, progress) =>
                {
                    sentMessages.Add(msg);
                    return Task.FromResult(string.Empty);
                });

            var service = new WaterMailService(
                Options.Create(new MailModel
                {
                    Mail = "from@test.com",
                    Password = "pwd",
                    Host = "smtp.test",
                    Port = "25"
                }),
                () => smtp.Object);

            var attachmentContent = Encoding.UTF8.GetBytes("hello");
            var formFile = new FormFile(new MemoryStream(attachmentContent), 0, attachmentContent.Length, "file", "hello.txt")
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/plain"
            };

            var request = new MailRequestDTO
            {
                ToEmail = "to@test.com",
                Subject = "Subject",
                Body = "<b>Hi</b>",
                Attachments = new List<IFormFile> { formFile }
            };

            await service.SendEmailiAsync(request);

            smtp.Verify(s => s.Connect("smtp.test", 25, SecureSocketOptions.StartTls, It.IsAny<CancellationToken>()), Times.Once);
            smtp.Verify(s => s.Authenticate("from@test.com", "pwd", It.IsAny<CancellationToken>()), Times.Once);
            smtp.Verify(s => s.Disconnect(true, It.IsAny<CancellationToken>()), Times.Once);
            Assert.Single(sentMessages);
            Assert.Equal("Subject", sentMessages[0].Subject);
            Assert.Equal("to@test.com", sentMessages[0].To.Mailboxes.First().Address);
        }
    }
}
