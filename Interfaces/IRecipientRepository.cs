namespace 水水水果API.Interfaces
{
    public interface IRecipientRepository
    {
        IEnumerable<Recipient> GetRecipients();
        IEnumerable<Recipient> GetRecipientsByPage(int page, int pageSize);
        Recipient GetRecipientById(Guid id);
        void CreateRecipient(Recipient recipient);
        void UpdateRecipient(Recipient recipient);
        void DeleteRecipient(Guid id);
    }
}