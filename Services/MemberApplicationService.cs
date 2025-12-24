using Framework.SqlCommon.SQLHelper;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using 水水水果API.Interfaces;
using 水水水果API.Models.CRM;
using 水水水果API.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


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
            _crmContext.Database.BeginTransaction();
            var transaction = (DbTransaction)_crmContext.Database.CurrentTransaction;
            try
            {
                _customerRepository.UpdateCustomer(new Customer()
                {
                    Id = memberUpdate.CustomerId.Value,
                    BirthDay = memberUpdate.BirthDay,
                    BrandId = memberUpdate.BrandId,
                    Gender = memberUpdate.Gender,
                    FirstName = memberUpdate.FirstName,
                    LastName = memberUpdate.LastName,
                    Phone = memberUpdate.Phone,
                });

                _memberRepository.UpdateMember(new Member()
                {
                    Id = memberUpdate.MemberId,
                    AvatarUrl = memberUpdate.AvatarUrl,
                    BrandId = memberUpdate.BrandId,
                    CustomerId = memberUpdate.CustomerId.Value,
                    IsActive = memberUpdate.IsActive,
                    MemberTierId = memberUpdate.MemberTierId,
                    StoreId = memberUpdate.StoreId,
                    UserId = memberUpdate.UserId.Value,
                });

                transaction.Commit();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "會員修改失敗， Rollback");
                transaction.Rollback();
                throw;
            }
        }

        public int RegisterMember(MemberCreate memberCreate)
        {
            int memberId = 0;
            var strategy = _crmContext.Database.CreateExecutionStrategy();

            strategy.Execute(() =>
            {
                try
                {
                    _crmContext.Database.BeginTransaction();
                    _authContext.Database.BeginTransaction();

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

                    _crmContext.Database.CommitTransaction();
                    _authContext.Database.CommitTransaction();
                    _logger.LogInformation($"會員註冊完成，MemberId: {memberId}, CustomerId: {customerId}, UserId: {userId}");
                }
                catch (Exception ex)
                {
                    _crmContext.Database.RollbackTransaction();
                    _authContext.Database.RollbackTransaction();
                    throw;
                }
             
            });
            return memberId;

        }

        private int ResolveCustomerId(MemberCreate memberCreate)
        {
            if (memberCreate.CustomerId.HasValue)
            {
                var existingCustomer = _crmContext.Customers
                    .AsNoTracking()
                    .FirstOrDefault(c => c.Id == memberCreate.CustomerId.Value);

                if (existingCustomer == null)
                {
                    throw new ArgumentException($"CustomerId {memberCreate.CustomerId.Value} 不存在，無法連結");
                }

                _logger.LogInformation($"使用既有 Customer，CustomerId: {memberCreate.CustomerId.Value}");
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
            };
            int newId = _customerRepository.CreateCustomer(newCustomer);

            _logger.LogInformation($"新建 Customer，CustomerId: {newId}");
            return newId;
        }

        private int ResolveUserId(MemberCreate memberCreate)
        {
            if (memberCreate.UserId.HasValue)
            {
                var userExists = _authRepository.UserExists(memberCreate.UserId.Value);

                if (!userExists)
                {
                    throw new ArgumentException($"UserId {memberCreate.UserId.Value} 不存在，無法連結");
                }

                _logger.LogInformation("使用既有 User，UserId: {UserId}", memberCreate.UserId.Value);
                return memberCreate.UserId.Value;
            }

            var userCreate = new User
            {
                Email = memberCreate.Email,
                PassWord = memberCreate.PassWord,
                IsActive = true
            };

            var newUserId = _authRepository.CreateUser(userCreate);
            _logger.LogInformation("新建 User，UserId: {UserId}", newUserId);

            return newUserId;
        }
    }
}
