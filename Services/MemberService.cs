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
        public IEnumerable<UserResponse> GetMembers(List<int> customers)
        {
            return _memberRepository.GetMember(customers);
        }

        public IEnumerable<UserResponse> GetMembersByPage(int page, int pageSize)
        {
            return _memberRepository.GetMemberByPage(page, pageSize);
        }

        public void CreateMember(UserCreate member)
        {
            //預計實作
            Member existmem = null;

            if (existmem != null) throw new Exception("建立客戶失敗，帳號已重複");

            _memberRepository.CreateMember(member);
        }

        public void UpdateMember(UserUpdate member)
        {
            _memberRepository.UpsertMember(member);
        }

        public UserResponse GetMemberById(int id)
        {
            var m = _memberRepository.GetMemberById(id);
            return new UserResponse
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