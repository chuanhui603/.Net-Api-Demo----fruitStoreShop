using Framework.SqlCommon.SQLHelper;
using FrameWork.Helper.Transfer;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace 水水水果API.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly TWCRM_TESTContext _crmConnection;
        private readonly SqlCommonContext _commonContext;
        private readonly DbConnection _crmConnect;
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CustomerRepository> _logger;

        public MemberRepository(ILogger<CustomerRepository> logger,TWCRM_TESTContext crmConnection, SqlCommonContext commonContext, ICustomerRepository customerRepository)
        {
            _crmConnection = crmConnection;
            _commonContext = commonContext;
            _crmConnect = crmConnection.Database.GetDbConnection();
            _customerRepository = customerRepository;
            _logger = logger;
        }

        public IEnumerable<UserResponse> GetMember(List<int> customers)
        {
            if(!customers.Any()) throw new ArgumentException("Customer list cannot be empty", nameof(customers));
            return _crmConnection.Members.Include(x=>x.Customer).Where(x=> customers.Contains(x.Id)).Select(m=> new UserResponse
            {
                MemberId = m.Id,
                BrandId = m.BrandId,
                Email = m.Email,
                FirstName = m.Customer.FirstName,
                LastName = m.Customer.LastName,
                Gender = m.Customer.Gender,
                Phone = m.Customer.Phone,
                BirthDay = m.Customer.BirthDay,
                IsActive = m.IsActive,
                LastLoginAt = m.LastLoginAt,
            });
        }

        public IEnumerable<UserResponse> GetMemberByPage(int page, int pageSize)
        {
            return [.. _crmConnection.Members.Skip((page - 1) * pageSize).Take(pageSize).Select(m=>new UserResponse
            {
                MemberId = m.Id,
                BrandId = m.BrandId,
                Email = m.Email,
                FirstName = m.Customer.FirstName,
                LastName = m.Customer.LastName,
                Gender = m.Customer.Gender,
                Phone = m.Customer.Phone,
                BirthDay = m.Customer.BirthDay,
                IsActive = m.IsActive,
                LastLoginAt = m.LastLoginAt,
            })];
        }

        public Member GetMemberById(int id)
        {
            return _crmConnection.Members.Where(x => x.Id == id).Include(x => x.Customer).Single();
        }

        public int CreateMember(UserCreate mem)
        {

            DateTime dateNow = _commonContext.CRM.GetDbDate(_crmConnect, _crmConnection.Database.CurrentTransaction);

            Customer existCust = new();
            if (mem.CustomerId != null)
            {
                existCust = _customerRepository.GetCustomerById(mem.CustomerId.Value);
            }

            var createMem = MappingHelper.ModelMapping<Member>(mem);
            createMem.CreateDate = dateNow;
            createMem.LastUpdateDate = dateNow;
            createMem.LastLoginAt = dateNow;
            createMem.IsActive = true;

            _commonContext.CRM.ExecuteTransaction(_crmConnection, () =>
            {
                if (existCust == null || mem.CustomerId == null)
                {
                    Customer cust = MappingHelper.ModelMapping<Customer>(mem);
                    createMem.CustomerId = _customerRepository.CreateCustomer(cust);
                }

                if (createMem.Id == 0)
                {
                    createMem.Id = _crmConnection.Members.Max(x => x.Id + 1);
                }

                _crmConnection.Members.Add(createMem);
                _crmConnection.SaveChanges();
            });

            return createMem.Id;
        }

        public void UpdateMember(UserUpdate mem)
        {
            var targetMember = _crmConnection.Members.Find(mem.MemberId);
            DateTime currentDate = _commonContext.CRM.GetDbDate(_crmConnect, _crmConnection.Database.CurrentTransaction);
            Customer updateCust = MappingHelper.ModelMapping<Customer>(mem);
            if(targetMember == null)
                throw new ArgumentException($"Member with ID {mem.MemberId} not found.");

            _commonContext.CRM.ExecuteTransaction(_crmConnection, () =>
            {
                _customerRepository.UpdateCustomer(updateCust);
                targetMember.MemberTierId = mem.MemberTierId;
                targetMember.Email = mem.Email;
                targetMember.IsActive = mem.IsActive;
                targetMember.LastLoginAt = mem.LastLoginAt;
                targetMember.LastUpdateDate = currentDate;

                _crmConnection.SaveChanges();
            });
        }

        public void DeleteMember(Member mem)
        {
            _commonContext.CRM.ExecuteTransaction(_crmConnection, () =>
            {
                mem.IsActive = false;
                _crmConnection.Members.Update(mem);
                _crmConnection.SaveChanges();
            });
        }

        public Member GetMemberByEmail(string email)
        {
            return _crmConnection.Members.Where(x => x.Email == email)
               .Include(x => x.Customer)
               .SingleOrDefault();
        }

        public Member GetMemberByLogin(string email, string password)
        {
            return _crmConnection.Members.Where(x => x.Email == email && x.PassWord == password)
                .Include(x => x.Customer)
                .SingleOrDefault();
        }
    }
}