namespace 水水水果API.Services
{
    public class RecipientService : IRecipientService
    {
        private readonly IRecipientRepository _recipientRepository;

        public RecipientService(IRecipientRepository recipientRepository)
        {
            _recipientRepository = recipientRepository;
        }

        public IEnumerable<RecipientDTO> GetRecipients()
        {
            return _recipientRepository.GetRecipients().Select(r => new RecipientDTO
            {
                Id = r.Id,
                FirstName = r.FirstName,
                LastName = r.LastName,
                Phone = r.Phone,
                Address = r.Address,
                Gender = r.Gender,
                Email = r.Email,
            });
        }

        public void CreateRecipient(RecipientDTO recipient)
        {
            // Implementation for creating a recipient
            var newRecipient = new Recipient
            {
                Id = Guid.NewGuid(),
                FirstName = recipient.FirstName,
                LastName = recipient.LastName,
                Phone = recipient.Phone,
                Address = recipient.Address,
                Email = recipient.Email,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Gender = recipient.Gender,
            };
            recipient.Id = newRecipient.Id;
            _recipientRepository.CreateRecipient(newRecipient);
        }

        public void DeleteRecipient(Guid id)
        {
            _recipientRepository.DeleteRecipient(id);
        }

        public void UpdateRecipient(Guid id, RecipientDTO recipient)
        {
            // Implementation for updating a recipient
            _recipientRepository.UpdateRecipient(new Recipient
            {
                Id = id,
                FirstName = recipient.FirstName,
                LastName = recipient.LastName,
                Phone = recipient.Phone,
                Address = recipient.Address,
                Email = recipient.Email,
                UpdatedAt = DateTime.Now,
            });
        }


        public RecipientDTO GetRecipientById(Guid id)
        {
            // Implementation for getting a recipient by id
            var recipient = _recipientRepository.GetRecipientById(id);
            if (recipient == null) return new RecipientDTO();
            return new RecipientDTO
            {
                Id = recipient.Id,
                FirstName = recipient.FirstName,
                LastName = recipient.LastName,
                Phone = recipient.Phone,
                Address = recipient.Address
            };
        }
    }
}