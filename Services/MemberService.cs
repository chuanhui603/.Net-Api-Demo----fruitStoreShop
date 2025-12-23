namespace 水水水果API.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly ILogger<MemberService> _logger;

        public MemberService(ILogger<MemberService> logger, IMemberRepository memberRepository, ICustomerRepository customerRepository)
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

        public void UpdateMember(MemberUpsert member)
        {
            _memberRepository.UpsertMember(member);
        }

        public MemberResponse GetMemberById(int id)
        {
            var m = _memberRepository.GetMemberById(id);
            return new MemberResponse
            {
                FirstName = m.Customer.FirstName,
                LastName = m.Customer.LastName,
                Phone = m.Customer.Phone,
                BirthDay = m.Customer.BirthDay,
                BrandId = m.BrandId,
                IsActive = m.IsActive,
                Gender = m.Customer.Gender,
            };
        }
        public void DeleteCustomer(int id)
        {
            Member member = _memberRepository.GetMemberById(id) ?? throw new ArgumentNullException($"Member is not exist. ID: {id}");
            _memberRepository.DeleteMember(member);
        }
    }
}