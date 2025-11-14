using 水水水果API.Models.DTO.Mail;

namespace 水水水果API.Interfaces
{
    public interface IMailHelper
    {
        Task SendEmailiAsync(MailRequestDTO mailRequest);
    }
}
