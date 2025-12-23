using Framework.SqlCommon.SQLHelper;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using 水水水果API.Interfaces;

namespace 水水水果API.Repositories
{
    /// <summary>
    /// MemberRepository 只負責純資料存取操作，業務邏輯由 MemberService 處理
    /// </summary>
    public class MemberRepository : IMemberRepository
    {
        private readonly TWCRM_TESTContext _crmConnection;
        private readonly SqlCommonContext _commonContext;
        private readonly DbConnection _crmConnect;
        private readonly ILogger<MemberRepository> _logger;

        public MemberRepository(ILogger<MemberRepository> logger, TWCRM_TESTContext crmConnection, SqlCommonContext commonContext)
        {
            _crmConnection = crmConnection;
            _commonContext = commonContext;
            _crmConnect = crmConnection.Database.GetDbConnection();
            _logger = logger;
        }

        public Member GetMemberById(int id) => _crmConnection.Members.Where(x => x.Id == id).Include(x => x.Customer).AsNoTracking().Single();

        public Member GetMemberByIdTracking(int id) => _crmConnection.Members.Find(id);

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
            return [.. _crmConnection.Members.Skip((page - 1) * pageSize).Take(pageSize).AsNoTracking().Select(m => new MemberResponse
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

        public int CreateMember(Member member)
        {
            DateTime dateNow = _commonContext.CRM.GetDbDate(_crmConnect, _crmConnection.Database.CurrentTransaction);

            member.CreateDate = dateNow;
            member.LastUpdateDate = dateNow;
            member.IsActive = true;

            _crmConnection.Members.Add(member);
            _crmConnection.SaveChanges();

            return member.Id;
        }

        public int UpdateMember(Member member)
        {
            DateTime dateNow = _commonContext.CRM.GetDbDate(_crmConnect, _crmConnection.Database.CurrentTransaction);

            _crmConnection.Entry(member).Property(x => x.CreateDate).IsModified = false;
            member.LastUpdateDate = dateNow;

            _crmConnection.SaveChanges();

            return member.Id;
        }

        public void DeleteMember(Member member)
        {
            var targetMember = _crmConnection.Members.Find(member.Id) ?? throw new ArgumentException($"Member with ID {member.Id} not found.");
            targetMember.IsActive = false;

            _crmConnection.SaveChanges();
        }
    }
}