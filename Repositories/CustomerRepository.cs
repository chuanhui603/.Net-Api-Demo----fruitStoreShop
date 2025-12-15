
using Framework.SqlCommon.SQLHelper;
using FrameWork.Helper.Transfer;
using K4os.Compression.LZ4.Internal;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using 水水水果.Controllers;

namespace 水水水果API.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly TWCRM_TESTContext _crmConnection;

        private readonly SqlCommonContext _commonContext;
        private readonly DbConnection _crmConnect;
        private readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(ILogger<CustomerRepository> logger,TWCRM_TESTContext crmConnection, SqlCommonContext commonContext)
        {
            _crmConnection = crmConnection;
            _commonContext = commonContext;
            _crmConnect = crmConnection.Database.GetDbConnection();
            _logger = logger;

        }

        public int CreateCustomer(Customer cust)
        {
            DateTime dateNow = _commonContext.CRM.GetDbDate(_crmConnect, _crmConnection.Database.CurrentTransaction);

            var createCust = MappingHelper.ModelMapping<Customer>(cust);
            createCust.CreateDate = dateNow;
            createCust.LastUpdateDate = dateNow;

            _commonContext.CRM.ExecuteTransaction(_crmConnection, () =>
            {
                if (createCust.Id == 0)
                {

                    createCust.Id = _crmConnection.Customers.Max(x => x.Id) + 1;
                    if  (createCust.Id == 0)
                    {
                        createCust.Id = 1;
                    }
                }
                
                _crmConnection.Customers.Add(createCust);
                _crmConnection.SaveChanges();
            });
            return createCust.Id;
        }

        public void UpdateCustomer(Customer cust)
        {
            DateTime dateNow = _commonContext.CRM.GetDbDate(_crmConnect, _crmConnection.Database.CurrentTransaction);
           
            _commonContext.CRM.ExecuteTransaction(_crmConnection, () =>
            {
                var targetCust = _crmConnection.Customers.Find(cust.Id);
                targetCust.BirthDay = cust.BirthDay;
                targetCust.FirstName = cust.FirstName;
                targetCust.LastName = cust.LastName;
                targetCust.Gender = cust.Gender;
                targetCust.Phone = cust.Phone;
                targetCust.CreateDate = dateNow;
                targetCust.LastUpdateDate = dateNow;
                _crmConnection.SaveChanges();
            });
        }

        public Customer GetCustomerById(int custId)
        {



            return _crmConnection.Customers.Where(c => c.Id == custId).SingleOrDefault();
        }
    }
}
