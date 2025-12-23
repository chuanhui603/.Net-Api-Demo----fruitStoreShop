using FrameWork.Helper.Transfer;
using System.Transactions;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace 水水水果API.Services
{
    /// <summary>
    /// MemberService 負責處理 Member 相關的業務邏輯
    /// 透過 CustomerService 和 AuthService 處理關聯實體
    /// </summary>
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly ICustomerService _customerService;
        private readonly IAuthService _authService;
        private readonly ILogger<MemberService> _logger;

        public MemberService(ILogger<MemberService> logger, IMemberRepository memberRepository, ICustomerService customerService, IAuthService authService)
        {
            _logger = logger;
            _memberRepository = memberRepository;
            _customerService = customerService;
            _authService = authService;
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
            return new MemberResponse
            {
                MemberId = m.Id,
                FirstName = m.Customer.FirstName,
                LastName = m.Customer.LastName,
                Phone = m.Customer.Phone,
                BirthDay = m.Customer.BirthDay,
                BrandId = m.BrandId,
                IsActive = m.IsActive,
                Gender = m.Customer.Gender,
            };
        }

        public int RegisterMember(MemberUpsert memberDto)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted });

            int customerId;
            if (memberDto.CustomerId.HasValue)
            {
                var existingCustomer = _customerService.GetCustomerById(memberDto.CustomerId.Value);
                if (existingCustomer != null)
                {
                    customerId = existingCustomer.Id;
                }
                else
                {
                    customerId = _customerService.UpsertCustomer(new CustomerUpsert
                    {
                        Id = 0,
                        BrandId = memberDto.BrandId,
                        FirstName = memberDto.FirstName,
                        LastName = memberDto.LastName,
                        Gender = memberDto.Gender,
                        BirthDay = memberDto.BirthDay,
                        Phone = memberDto.Phone
                    });
                }
            }
            else
            {
                customerId = _customerService.UpsertCustomer(new CustomerUpsert
                {
                    Id = 0,
                    BrandId = memberDto.BrandId,
                    FirstName = memberDto.FirstName,
                    LastName = memberDto.LastName,
                    Gender = memberDto.Gender,
                    BirthDay = memberDto.BirthDay,
                    Phone = memberDto.Phone
                });
            }

            int userId;
            if (memberDto.UserId.HasValue)
            {
                var existingUser = _authService.GetUserById(memberDto.UserId.Value);
                if (existingUser != null)
                {
                    userId = existingUser.Id;
                }
                else
                {
                    userId = _authService.UpsertUser(new UserUpsert
                    {
                        UserId = null,
                        MemberId = memberDto.MemberId,
                        Email = memberDto.Email,
                        PassWord = memberDto.PassWord,
                        IsActive = memberDto.IsActive
                    });
                }
            }
            else
            {
                userId = _authService.UpsertUser(new UserUpsert
                {
                    UserId = null,
                    MemberId = memberDto.MemberId,
                    Email = memberDto.Email,
                    PassWord = memberDto.PassWord,
                    IsActive = memberDto.IsActive
                });
            }

            var member = new Member
            {
                CustomerId = customerId,
                UserId = userId,
                BrandId = memberDto.BrandId,
                MemberTierId = memberDto.MemberTierId,
                IsActive = true
            };

            int memberId = _memberRepository.CreateMember(member);
            scope.Complete();

            return memberId;
        }

        public int UpdateMember(MemberUpsert memberDto)
        {
            if (!memberDto.MemberId.HasValue)
                throw new ArgumentException("MemberId is required for update operation.");

            using var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted });

            var existingMember = _memberRepository.GetMemberByIdTracking(memberDto.MemberId.Value)
                ?? throw new ArgumentException($"Member with ID {memberDto.MemberId} not found.");

            existingMember.BrandId = memberDto.BrandId;
            existingMember.MemberTierId = memberDto.MemberTierId;

            int memberId = _memberRepository.UpdateMember(existingMember);
            scope.Complete();

            return memberId;
        }

        public int UpsertMember(MemberUpsert memberDto)
        {
            if (memberDto.MemberId.HasValue)
            {
                var existingMember = _memberRepository.GetMemberByIdTracking(memberDto.MemberId.Value);
                if (existingMember != null)
                {
                    return UpdateMember(memberDto);
                }
            }

            return RegisterMember(memberDto);
        }

        public void DeleteMember(int id)
        {
            Member member = _memberRepository.GetMemberById(id) ?? throw new ArgumentNullException($"Member does not exist. ID: {id}");
            _memberRepository.DeleteMember(member);
        }
    }
}