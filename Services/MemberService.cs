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

        public MemberResponse GetMemberById(int id)
        {
            var m = _memberRepository.GetMemberById(id);
            if (m == null) return null;

            return new MemberResponse
            {
                MemberId = m.Id,
                FirstName = m.Customer?.FirstName,
                LastName = m.Customer?.LastName,
                Phone = m.Customer?.Phone,
                BirthDay = m.Customer?.BirthDay ?? default,
                BrandId = m.BrandId,
                IsActive = m.IsActive,
                Gender = m.Customer?.Gender,
            };
        }  

        public int UpdateMember(MemberUpdate memberUpdate)
        {
            var existingMember = _memberRepository.GetMemberById(memberUpdate.MemberId)
                ?? throw new ArgumentException($"Member with ID {memberUpdate.MemberId} not found.");

            existingMember.BrandId = memberUpdate.BrandId;
            existingMember.MemberTierId = memberUpdate.MemberTierId;
            existingMember.IsActive = memberUpdate.IsActive;

            int memberId = _memberRepository.UpdateMember(existingMember);
            _logger.LogInformation("Updated Member with ID: {MemberId}", memberId);

            return memberId;
        }

        public void DeleteMember(int id)
        {
            Member member = _memberRepository.GetMemberById(id) 
                ?? throw new ArgumentNullException($"Member does not exist. ID: {id}");
            _memberRepository.DeleteMember(member);
        }
    }
}