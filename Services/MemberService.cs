using Serilog;
using System.Data.Common;

namespace 水水水果API.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;

        private readonly ILogger<MemberService> _logger;

        public MemberService(ILogger<MemberService> logger, IMemberRepository memberRepository)
        {
            _logger = logger;
            _memberRepository = memberRepository;
        }


        public IEnumerable<MemberResponse> GetMembers(List<int> customers)
        {
            return _memberRepository.GetMember(customers);
        }

        public IEnumerable<MemberResponse> GetMembersByPage(int page, int pageSize)
        {
            return _memberRepository.GetMemberByPage(page, pageSize);
        }

        public MemberResponse GetMemberByUser(User user)
        {
            Member m = _memberRepository.GetMembersByUser(user.Id) ?? throw new ArgumentException($"Member with UserID {user.Id} not found.");
            return new MemberResponse
            {
                
                FirstName = m.Customer?.FirstName,
                Email = user.Email,
                LastName = m.Customer?.LastName,
                Phone = m.Customer?.Phone,
                Birthday = m.Customer?.BirthDay.ToString("yyyy-MM-dd"),
                Gender = m.Customer?.Gender,
                Address = m.Customer?.Address,
            };
        }

        public MemberResponse GetMemberById(int id)
        {
            var m = _memberRepository.GetMemberById(id);

            return new MemberResponse
            {
                FirstName = m.Customer?.FirstName,
                LastName = m.Customer?.LastName,
                Phone = m.Customer?.Phone,
                Birthday = m.Customer?.BirthDay.ToString("yyyy-MM-dd"),
                Gender = m.Customer?.Gender,
                Address = m.Customer?.Address,
            };
        }

        public void DeleteMember(int userId)
        {
            Member member = _memberRepository.GetMembersByUser(userId) ?? throw new ArgumentException($"Member with UserID {userId} not found.");
            _memberRepository.DeleteMember(member);
        }
    }
}