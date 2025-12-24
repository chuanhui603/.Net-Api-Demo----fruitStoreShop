
using Framework.SqlCommon.SQLHelper;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;


namespace 水水水果API.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly TWCRM_TESTContext _crmConnection;
        private readonly SqlCommonContext _commonContext;
        private readonly DbConnection _crmConnect;
        private readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(ILogger<CustomerRepository> logger, TWCRM_TESTContext crmConnection, SqlCommonContext commonContext)
        {
            _crmConnection = crmConnection;
            _commonContext = commonContext;
            _crmConnect = crmConnection.Database.GetDbConnection();
            _logger = logger;

        }

        public int CreateCustomer(Customer cust)
        {
            DateTime dateNow = _commonContext.CRM.GetDbDate(_crmConnect, _crmConnection.Database.CurrentTransaction);
            cust.CreateDate = dateNow;
            cust.LastUpdateDate = dateNow;

            _crmConnection.Customers.Add(cust);
            _crmConnection.SaveChanges();

            return cust.Id;
        }

        public int UpdateCustomer(Customer cust)
        {
            DateTime dateNow = _commonContext.CRM.GetDbDate(_crmConnect, _crmConnection.Database.CurrentTransaction);
            var targetCustomer = _crmConnection.Customers.Find(cust.Id)
                ?? throw new ArgumentException($"Customer with ID {cust.Id} not found.");

            targetCustomer.BrandId = cust.BrandId;
            targetCustomer.FirstName = cust.FirstName;
            targetCustomer.LastName = cust.LastName;
            targetCustomer.Gender = cust.Gender;
            targetCustomer.BirthDay = cust.BirthDay;
            targetCustomer.Phone = cust.Phone;

            _crmConnection.Entry(targetCustomer).Property(x => x.CreateDate).IsModified = false;
            targetCustomer.LastUpdateDate = dateNow;

            _crmConnection.SaveChanges();

            return targetCustomer.Id;
        }

        public Customer GetCustomerById(int custId)
        {
            return _crmConnection.Customers.AsNoTracking().SingleOrDefault(c => c.Id == custId);
        }
    }
}
