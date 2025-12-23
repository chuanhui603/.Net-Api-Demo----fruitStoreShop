using Framework.SqlCommon.SQLHelper;
using FrameWork.Helper.Transfer;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using 水水水果API.Interfaces;

namespace 水水水果API.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly TWCRM_TESTContext _crmConnection;
        private readonly TWAUTH_TESTContext _authConnection;
        private readonly SqlCommonContext _commonContext;
        private readonly DbConnection _crmConnect;
        private readonly ICustomerRepository _customerRepository;
        private readonly IAuthRepository _authRepository;
        private readonly ILogger<CustomerRepository> _logger;

        public MemberRepository(ILogger<CustomerRepository> logger, TWCRM_TESTContext crmConnection, SqlCommonContext commonContext, ICustomerRepository customerRepository, IAuthRepository authRepository, TWAUTH_TESTContext authConnection)
        {
            _authRepository = authRepository;
            _crmConnection = crmConnection;
            _commonContext = commonContext;
            _authConnection = authConnection;
            _crmConnect = crmConnection.Database.GetDbConnection();
            _customerRepository = customerRepository;
            _logger = logger;
        }

        public Member GetMemberById(int id) => _crmConnection.Members.Where(x => x.Id == id).Include(x => x.Customer).AsNoTracking().Single();

        public IEnumerable<MemberResponse> GetMember(List<int> customers)
        {
            if (customers.Count == 0) throw new ArgumentException("Customer list cannot be empty", nameof(customers));
            return _crmConnection.Members.Include(x => x.Customer).Where(x => customers.Contains(x.Id)).AsNoTracking().Select(m => new MemberResponse
            {
                MemberId = m.Id,
                BrandId = m.BrandId,
                FirstName = m.Customer.FirstName,
                LastName = m.Customer.LastName,
                Gender = m.Customer.Gender,
                Phone = m.Customer.Phone,
                BirthDay = m.Customer.BirthDay,
                IsActive = m.IsActive,
            });
        }

        public IEnumerable<MemberResponse> GetMemberByPage(int page, int pageSize)
        {
            return [.. _crmConnection.Members.Skip((page - 1) * pageSize).Take(pageSize).AsNoTracking().Select(m=>new MemberResponse
            {
                MemberId = m.Id,
                BrandId = m.BrandId,
                FirstName = m.Customer.FirstName,
                LastName = m.Customer.LastName,
                Gender = m.Customer.Gender,
                Phone = m.Customer.Phone,
                BirthDay = m.Customer.BirthDay,
                IsActive = m.IsActive,
            })];
        }


        public int UpsertMember(MemberUpsert mem)
        {
            DateTime dateNow = _commonContext.CRM.GetDbDate(_crmConnect, _crmConnection.Database.CurrentTransaction);
            var createMem = MappingHelper.ModelMapping<Member>(mem);

            int memId = 0;
            _commonContext.CRM.ExecuteTransaction(_crmConnection, () =>
            {
                var targetMember = _crmConnection.Members.Find(mem.MemberId);

                if (targetMember == null)
                {
                    if (mem.CustomerId.HasValue)
                    {
                        var existCust = _customerRepository.GetCustomerById(mem.CustomerId.Value);
                        if (existCust == null)
                        {
                            CustomerUpsert cust = MappingHelper.ModelMapping<CustomerUpsert>(mem);
                            createMem.CustomerId = _customerRepository.UpsertCustomer(cust);
                        }
                        else
                            createMem.CustomerId = existCust.Id;
                    }
                    else
                    {
                        CustomerUpsert cust = MappingHelper.ModelMapping<CustomerUpsert>(mem);
                        createMem.CustomerId = _customerRepository.UpsertCustomer(cust);
                    }

                    if (mem.UserId.HasValue)
                    {
                        var existUser = _authRepository.GetUserById(mem.UserId.Value);
                        if (existUser == null)
                        {
                            UserUpsert user = MappingHelper.ModelMapping<UserUpsert>(mem);
                            createMem.UserId = _authRepository.UpsertUser(user);
                        }
                        else
                            createMem.UserId = existUser.Id;
                    }
                    else
                    {
                        UserUpsert user = MappingHelper.ModelMapping<UserUpsert>(mem);
                        createMem.UserId = _authRepository.UpsertUser(user);
                    }

                    createMem.CreateDate = dateNow;
                    createMem.LastUpdateDate = dateNow;
                    createMem.IsActive = true;

                    _crmConnection.Members.Add(createMem);
                    _crmConnection.SaveChanges();
                    memId = createMem.Id;
                }
                else
                {
                    _crmConnection.Entry(targetMember).CurrentValues.SetValues(mem);
                    _crmConnection.Entry(targetMember).Property(x => x.CreateDate).IsModified = false;
                    targetMember.LastUpdateDate = dateNow;

                    _crmConnection.SaveChanges();
                    memId = targetMember.Id;
                }
            });
            return memId;
        }

        public void DeleteMember(Member mem)
        {
            _commonContext.CRM.ExecuteTransaction(_crmConnection, () =>
            {
                var targetMember = _crmConnection.Members.Find(mem.Id) ?? throw new ArgumentException($"Member with ID {mem.Id} not found.");
                targetMember.IsActive = false;

                _crmConnection.SaveChanges();
            });
        }
    }
}