namespace 水水水果API.Interfaces
{
    public interface IRecipientRepository
    {
        IEnumerable<Recipient> GetRecipients();
        Recipient GetRecipientById(Guid id);
        void CreateRecipient(Recipient recipient);
        void UpdateRecipient(Recipient recipient);
        void DeleteRecipient(Guid id);
    }
}