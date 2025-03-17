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
        public IEnumerable<MemberDTO> GetMembers()
        {
            return _memberRepository.GetMembers().Select(r => new MemberDTO
            {
                Id = r.Id,
                Email = r.Email,
                PhoneNumber = r.PhoneNumber,
                BirthDate = r.BirthDate,
                Address = r.Address,
                FirstName = r.FirstName,
                LastName = r.LastName,
                City = r.City,
                Gender = r.Gender,
                LoginRole = r.LoginRole,
                Password = r.Password,
                PostalCode = r.PostalCode,
                Region = r.Region,
                IsVerified = r.IsVerified,
                LastLoginTime = r.LastLoginTime,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            });

        }
        public IEnumerable<MemberDTO> GetMembersByPage(int page, int pageSize)
        {
            return _memberRepository.GetMembersByPage(page, pageSize).Select(r => new MemberDTO
            {
                Id = r.Id,
                Email = r.Email,
                PhoneNumber = r.PhoneNumber,
                BirthDate = r.BirthDate,
                Address = r.Address,
                FirstName = r.FirstName,
                LastName = r.LastName,
                City = r.City,
                Gender = r.Gender,
                LoginRole = r.LoginRole,
                Password = r.Password,
                PostalCode = r.PostalCode,
                Region = r.Region,
                IsVerified = r.IsVerified,
                LastLoginTime = r.LastLoginTime,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            });
        }

        public void CreateMember(MemberDTO member)
        {
            _memberRepository.CreateMember(new Member
            {
                Id = Guid.NewGuid(),
                Email = member.Email,
                PhoneNumber = member.PhoneNumber,
                BirthDate = member.BirthDate.Value,
                Address = member.Address,
                FirstName = member.FirstName,
                LastName = member.LastName,
                City = member.City,
                Gender = member.Gender,
                LoginRole = "User",
                Password = member.Password,
                PostalCode = member.PostalCode,
                Region = member.Region,
                IsVerified = false,
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
            });
        }
        public void UpdateMember(Guid id, MemberDTO member)
        {
            _memberRepository.UpdateMember(new Member
            {
                Id = member.Id,
                Email = member.Email,
                PhoneNumber = member.PhoneNumber,
                BirthDate = member.BirthDate.Value,
                Address = member.Address,
                FirstName = member.FirstName,
                LastName = member.LastName,
                City = member.City,
            });
        }

        public MemberDTO GetMemberById(Guid id)
        {
            var member = _memberRepository.GetMemberById(id);
            return new MemberDTO
            {
                Id = member.Id,
                BirthDate = member.BirthDate,
                Address = member.Address,
                FirstName = member.FirstName,
                LastName = member.LastName,
                City = member.City,
                Gender = member.Gender,
                LoginRole = member.LoginRole,
                Password = member.Password,
                PostalCode = member.PostalCode,
                Region = member.Region,
                IsVerified = false,
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
                Email = member.Email,
                LastLoginTime = member.LastLoginTime,
                PhoneNumber = member.PhoneNumber,
            };
        }
        public void DeleteMember(Guid id)
        {
            _memberRepository.DeleteMember(id);
        }


    }
}