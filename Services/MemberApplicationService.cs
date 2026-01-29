using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace 水水水果API.Services
{

    public class MemberApplicationService : IMemberApplicationService
    {
        private readonly TWCRM_TESTContext _crmContext;
        private readonly TWAUTH_TESTContext _authContext;

        private readonly IAuthRepository _authRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly ILogger<MemberApplicationService> _logger;

        public MemberApplicationService(
            ICustomerRepository customerRepository,
            IMemberRepository memberRepository,
            IAuthRepository authRepository,
            TWCRM_TESTContext crmContext,
            TWAUTH_TESTContext authContext,
        ILogger<MemberApplicationService> logger)
        {
            _crmContext = crmContext;
            _customerRepository = customerRepository;
            _memberRepository = memberRepository;
            _authRepository = authRepository;
            _authContext = authContext;
            _logger = logger;
        }

        public void UpdateMember(MemberUpdate memberUpdate)
        {
            var strategy = _crmContext.Database.CreateExecutionStrategy();
            var useTransaction = _crmContext.Database.IsRelational() && _authContext.Database.IsRelational();

            strategy.Execute(() =>
            {
                try
                {
                    if (useTransaction)
                    {
                        _crmContext.Database.BeginTransaction();
                        _authContext.Database.BeginTransaction();
                    }
                    User updateUser = _authRepository.GetUserByEmail(memberUpdate.Email);
                    Member member = _memberRepository.GetMembersByUser(updateUser.Id);

                    _customerRepository.UpdateCustomer(new Customer()
                    {
                        Id = member.Customer.Id,
                        BirthDay = memberUpdate.Birthday,
                        BrandId = memberUpdate.BrandId,
                        Gender = memberUpdate.Gender,
                        FirstName = memberUpdate.FirstName,
                        LastName = memberUpdate.LastName,
                        Phone = memberUpdate.Phone,
                        Address = memberUpdate.Address
                    });

                    _memberRepository.UpdateMember(new Member()
                    {
                        Id = member.Id,
                        AvatarUrl = memberUpdate.AvatarUrl,
                        BrandId = memberUpdate.BrandId ,
                        CustomerId = memberUpdate.CustomerId ?? member.Customer.Id,
                        IsActive = memberUpdate.IsActive,
                        MemberTierId = memberUpdate.MemberTierId,
                        StoreId = memberUpdate.StoreId,
                        UserId = memberUpdate.UserId?? member.UserId,
                    });

                    if (useTransaction)
                    {
                        _crmContext.Database.CommitTransaction();
                        _authContext.Database.CommitTransaction();
                    }
                }
                catch (Exception ex)
                {
                    if (useTransaction)
                    {
                        _crmContext.Database.RollbackTransaction();
                        _authContext.Database.RollbackTransaction();
                    }
                    _logger.LogError(ex, "會員修改失敗， Rollback");
                    throw;
                }
            });
        }

        public MemberResponse RegisterMember(MemberCreate memberCreate)
        {
            int memberId = 0;
            var strategy = _crmContext.Database.CreateExecutionStrategy();
            var useTransaction = _crmContext.Database.IsRelational() && _authContext.Database.IsRelational();

            strategy.Execute(() =>
            {
                try
                {
                    if (useTransaction)
                    {
                        _crmContext.Database.BeginTransaction();
                        _authContext.Database.BeginTransaction();
                    }

                    int customerId = ResolveCustomerId(memberCreate);
                    int userId = ResolveUserId(memberCreate);

                    memberId = _memberRepository.CreateMember(new Member()
                    {
                        AvatarUrl = memberCreate.AvatarUrl,
                        BrandId = memberCreate.BrandId,
                        StoreId = memberCreate.StoreId,
                        CustomerId = customerId,
                        UserId = userId,
                        MemberTierId = memberCreate.MemberTierId,
                        IsActive = true,
                    });

                    if (useTransaction)
                    {
                        _crmContext.Database.CommitTransaction();
                        _authContext.Database.CommitTransaction();
                    }
                    _logger.LogInformation("會員註冊完成，MemberId: {MemberId}, CustomerId: {CustomerId}, UserId: {UserId}", memberId, customerId, userId);
                }
                catch (Exception)
                {
                    if (useTransaction)
                    {
                        _crmContext.Database.RollbackTransaction();
                        _authContext.Database.RollbackTransaction();
                    }
                    throw;
                }

            });
            return new MemberResponse()
            {
                Birthday = memberCreate.BirthDay.ToString("yyyy-MM-dd"),
                Email = memberCreate.Email,
                FirstName = memberCreate.FirstName,
                Gender = memberCreate.Gender,
                Phone = memberCreate.Phone,
                Address = memberCreate.Address,
            };
        }

        private int ResolveCustomerId(MemberCreate memberCreate)
        {
            if (memberCreate.CustomerId.HasValue)
            {
                var existingCustomer = _crmContext.Customers
                    .AsNoTracking()
                    .FirstOrDefault(c => c.Id == memberCreate.CustomerId.Value);

                if (existingCustomer == null)
                    throw new ArgumentException($"CustomerId {memberCreate.CustomerId.Value} is not exist");

                _logger.LogInformation("Use Exist Customer，CustomerId: {CustomerId}", memberCreate.CustomerId.Value);
                return memberCreate.CustomerId.Value;
            }

            var now = DateTime.Now;
            var newCustomer = new Customer
            {
                BrandId = memberCreate.BrandId,
                FirstName = memberCreate.FirstName,
                LastName = memberCreate.LastName,
                Gender = memberCreate.Gender,
                BirthDay = memberCreate.BirthDay,
                Phone = memberCreate.Phone,
                Address = memberCreate.Address
            };
            int newId = _customerRepository.CreateCustomer(newCustomer);

            _logger.LogInformation("Create Customer，CustomerId: {newId}", newId);
            return newId;
        }

        private int ResolveUserId(MemberCreate memberCreate)
        {
            if (memberCreate.UserId.HasValue)
            {
                var userExists = _authRepository.UserExists(memberCreate.UserId.Value);

                if (!userExists)
                {
                    throw new ArgumentException($"UserId {memberCreate.UserId.Value} is not exist");
                }

                _logger.LogInformation("Use Exist User，UserId: {UserId}", memberCreate.UserId.Value);
                return memberCreate.UserId.Value;
            }

            var userCreate = new User
            {
                Email = memberCreate.Email,
                PassWord = memberCreate.PassWord,
                IsActive = true
            };

            var newUserId = _authRepository.CreateUser(userCreate);
            _logger.LogInformation("Create User，UserId: {UserId}", newUserId);

            return newUserId;
        }
    }
}
