using 水水水果API.DTO.Email;

namespace 水水水果API.Interfaces
{
    public interface IMailHelper
    {
        Task SendEmailiAsync(MailRequestDTO mailRequest);
    }
}
