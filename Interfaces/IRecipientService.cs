namespace 水水水果API.Interfaces
{
    public interface IRecipientService
    {
        IEnumerable<RecipientDTO> GetRecipients();
        void CreateRecipient(RecipientDTO recipient);
        void DeleteRecipient(Guid id);
        void UpdateRecipient(Guid id, RecipientDTO recipient);
        RecipientDTO GetRecipientById(Guid id);
    }
}
