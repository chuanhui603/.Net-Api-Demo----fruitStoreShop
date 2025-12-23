
using Framework.SqlCommon.SQLHelper;
using FrameWork.Helper.Transfer;
using K4os.Compression.LZ4.Internal;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using 水水水果.Controllers;
using 水水水果API.Interfaces;

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

        public int UpsertCustomer(CustomerUpsert cust)
        {
            DateTime dateNow = _commonContext.CRM.GetDbDate(_crmConnect, _crmConnection.Database.CurrentTransaction);
            var targetCustomer = _crmConnection.Customers.Find(cust.Id);

            int custId = 0;
            _commonContext.CRM.ExecuteTransaction(_crmConnection, () =>
            {
                if (targetCustomer == null)
                {
                    var newCustomer = new Customer();

                    _crmConnection.Entry(newCustomer).CurrentValues.SetValues(cust);

                    newCustomer.CreateDate = dateNow;
                    newCustomer.LastUpdateDate = dateNow;

                    _crmConnection.Customers.Add(newCustomer);
                    _crmConnection.SaveChanges();
                    custId = newCustomer.Id;
                }
                else
                {
                    _crmConnection.Entry(targetCustomer).CurrentValues.SetValues(cust);

                    _crmConnection.Entry(targetCustomer).Property(x => x.CreateDate).IsModified = false;
                    targetCustomer.LastUpdateDate = dateNow;

                    _crmConnection.SaveChanges();
                    custId = targetCustomer.Id;
                }
            });

            return custId;
        }

        public Customer GetCustomerById(int custId)
        {
            return _crmConnection.Customers.Where(c => c.Id == custId).AsNoTracking().SingleOrDefault();
        }
    }
}
