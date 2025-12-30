using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using 水水水果API.Interfaces;

namespace 水水水果API.Repositories
{
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

        public Member GetMemberById(int id)
        {
            return _crmConnection.Members.Where(x => x.Id == id).Include(x => x.Customer).AsNoTracking().SingleOrDefault();
        }

        public Member GetMembersByUser(int userId)
        {
            return GetMembersByUsers([userId]).SingleOrDefault();
        }

        public List<Member> GetMembersByUsers(List<int> userIds)
        {
            if (userIds == null || userIds.Count == 0)
                throw new ArgumentNullException(nameof(userIds), "At least one user ID must be provided.");
            return [.. _crmConnection.Members.Where(x => userIds.Contains(x.UserId)).Include(x => x.Customer).AsNoTracking()];
        }

        public IEnumerable<MemberResponse> GetMember(List<int> customers)
        {
            if (customers == null || customers.Count == 0)
                throw new ArgumentNullException(nameof(customers), "At least one Customer ID must be provided.");
            return _crmConnection.Members.Include(x => x.Customer).Where(x => customers.Contains(x.Id)).AsNoTracking().Select(m => new MemberResponse
            {
                FirstName = m.Customer.FirstName,
                LastName = m.Customer.LastName,
                Gender = m.Customer.Gender,
                Phone = m.Customer.Phone,
                Birthday = m.Customer.BirthDay.ToString("yyyy-MM-dd"),
            });
        }

        public IEnumerable<MemberResponse> GetMemberByPage(int page, int pageSize)
        {
            return [.. _crmConnection.Members.Skip((page - 1) * pageSize).Take(pageSize).AsNoTracking().Select(m => new MemberResponse
            {
                FirstName = m.Customer.FirstName,
                LastName = m.Customer.LastName,
                Gender = m.Customer.Gender,
                Phone = m.Customer.Phone,
                Birthday = m.Customer.BirthDay.ToString("yyyy-MM-dd"),
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
            ArgumentNullException.ThrowIfNull(member);
            var targetMember = _crmConnection.Members.Find(member.Id) ?? throw new ArgumentException($"Member with ID {member.Id} not found.");
            targetMember.IsActive = false;

            _crmConnection.SaveChanges();
        }
    }
}