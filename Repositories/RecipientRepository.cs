namespace 水水水果API.Repositories
{
    internal class RecipientRepository(ILogger<RecipientRepository> logger, IDbConnection dbConnection) : IRecipientRepository
    {
        private readonly IDbConnection _dbConnection = dbConnection;
        private readonly ILogger<RecipientRepository> _logger = logger;
        private readonly string _tableName = "recipient";

        public IEnumerable<Recipient> GetRecipients()
        {
            return _dbConnection.GetAll<Recipient>(_tableName);
        }
        public IEnumerable<Recipient> GetRecipientsByPage(int page, int pageSize)
        {
            return _dbConnection.GetByPage<Recipient>(_tableName, page, pageSize);
        }

        public Recipient GetRecipientById(Guid id)
        {
            return _dbConnection.GetById<Recipient>(_tableName, id);
        }

        public void CreateRecipient(Recipient recipient)
        {
            _dbConnection.Insert(_tableName, recipient);
        }

        public void UpdateRecipient(Recipient recipient)
        {
            _dbConnection.Update(_tableName, recipient);
        }

        public void DeleteRecipient(Guid id)
        {
            _dbConnection.Delete(_tableName, id);
        }


    }
}