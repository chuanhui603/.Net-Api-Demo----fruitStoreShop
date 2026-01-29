using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using 水水水果API.Interfaces;
using 水水水果API.Models.AUTH;
using 水水水果API.Models.CRM;
using 水水水果API.Models.DTO;
using 水水水果API.Services;

namespace WaterFruitApi.Tests
{
    public class MemberApplicationServiceTests
    {
        [Fact]
        public void RegisterMember_WithExistingIds_UsesExistingRecords()
        {
            using var crmContext = TestDbContextFactory.CreateCrmContext(withCustomer: true);
            using var authContext = TestDbContextFactory.CreateAuthContext();

            var customerRepo = new Mock<ICustomerRepository>();
            var memberRepo = new Mock<IMemberRepository>();
            var authRepo = new Mock<IAuthRepository>();
            var logger = new Mock<ILogger<MemberApplicationService>>();

            authRepo.Setup(r => r.UserExists(5)).Returns(true);
            memberRepo.Setup(r => r.CreateMember(It.IsAny<Member>())).Returns(10);

            var service = new MemberApplicationService(customerRepo.Object, memberRepo.Object, authRepo.Object, crmContext, authContext, logger.Object);

            var request = new MemberCreate
            {
                CustomerId = 1,
                UserId = 5,
                BrandId = 2,
                StoreId = 3,
                MemberTierId = 4,
                Email = "user@test.com",
                PassWord = "pwd",
                AvatarUrl = "avatar",
                FirstName = "First",
                LastName = "Last",
                Gender = "M",
                Address = "addr",
                BirthDay = new DateTime(1990, 1, 1),
                Phone = "123"
            };

            var response = service.RegisterMember(request);

            customerRepo.Verify(r => r.CreateCustomer(It.IsAny<Customer>()), Times.Never);
            authRepo.Verify(r => r.CreateUser(It.IsAny<User>()), Times.Never);
            memberRepo.Verify(r => r.CreateMember(It.Is<Member>(m => m.CustomerId == 1 && m.UserId == 5 && m.BrandId == 2 && m.MemberTierId == 4)), Times.Once);

            Assert.Equal("1990-01-01", response.Birthday);
            Assert.Equal("user@test.com", response.Email);
            Assert.Equal("First", response.FirstName);
        }

        [Fact]
        public void RegisterMember_CreatesCustomerAndUser_WhenMissingIds()
        {
            using var crmContext = TestDbContextFactory.CreateCrmContext();
            using var authContext = TestDbContextFactory.CreateAuthContext();

            var customerRepo = new Mock<ICustomerRepository>();
            var memberRepo = new Mock<IMemberRepository>();
            var authRepo = new Mock<IAuthRepository>();
            var logger = new Mock<ILogger<MemberApplicationService>>();

            customerRepo.Setup(r => r.CreateCustomer(It.IsAny<Customer>())).Returns(11);
            authRepo.Setup(r => r.CreateUser(It.IsAny<User>())).Returns(22);
            memberRepo.Setup(r => r.CreateMember(It.IsAny<Member>())).Returns(33);

            var service = new MemberApplicationService(customerRepo.Object, memberRepo.Object, authRepo.Object, crmContext, authContext, logger.Object);

            var request = new MemberCreate
            {
                BrandId = 1,
                StoreId = 2,
                MemberTierId = 3,
                Email = "new@test.com",
                PassWord = "pwd",
                AvatarUrl = "avatar",
                FirstName = "First",
                LastName = "Last",
                Gender = "F",
                Address = "addr",
                BirthDay = new DateTime(2001, 2, 2),
                Phone = "999"
            };

            var response = service.RegisterMember(request);

            customerRepo.Verify(r => r.CreateCustomer(It.Is<Customer>(c => c.FirstName == request.FirstName && c.BrandId == request.BrandId)), Times.Once);
            authRepo.Verify(r => r.CreateUser(It.Is<User>(u => u.Email == request.Email && u.PassWord == request.PassWord)), Times.Once);
            memberRepo.Verify(r => r.CreateMember(It.Is<Member>(m => m.CustomerId == 11 && m.UserId == 22)), Times.Once);

            Assert.Equal("new@test.com", response.Email);
            Assert.Equal("First", response.FirstName);
        }

        [Fact]
        public void UpdateMember_UpdatesCustomerAndMember()
        {
            using var crmContext = TestDbContextFactory.CreateCrmContext(withCustomer: true);
            using var authContext = TestDbContextFactory.CreateAuthContext();

            var customerRepo = new Mock<ICustomerRepository>();
            var memberRepo = new Mock<IMemberRepository>();
            var authRepo = new Mock<IAuthRepository>();
            var logger = new Mock<ILogger<MemberApplicationService>>();

            var user = new User { Id = 8, Email = "user@test.com" };
            var member = new Member
            {
                Id = 9,
                UserId = user.Id,
                Customer = new Customer { Id = 1 }
            };

            authRepo.Setup(r => r.GetUserByEmail(user.Email)).Returns(user);
            memberRepo.Setup(r => r.GetMembersByUser(user.Id)).Returns(member);

            var service = new MemberApplicationService(customerRepo.Object, memberRepo.Object, authRepo.Object, crmContext, authContext, logger.Object);

            var update = new MemberUpdate
            {
                Email = user.Email,
                BrandId = 2,
                StoreId = 3,
                MemberTierId = 4,
                FirstName = "First",
                LastName = "Last",
                Gender = "M",
                Address = "addr",
                Birthday = new DateTime(1999, 9, 9),
                Phone = "555",
                AvatarUrl = "avatar",
                CustomerId = 1,
                UserId = user.Id,
                IsActive = true
            };

            service.UpdateMember(update);

            customerRepo.Verify(r => r.UpdateCustomer(It.Is<Customer>(c => c.Id == 1 && c.FirstName == update.FirstName && c.Phone == update.Phone)), Times.Once);
            memberRepo.Verify(r => r.UpdateMember(It.Is<Member>(m => m.Id == member.Id && m.CustomerId == 1 && m.MemberTierId == update.MemberTierId)), Times.Once);
        }

        [Fact]
        public void UpdateMember_WhenRepositoryFails_RollsBack()
        {
            using var crmConn = new SqliteConnection("Filename=:memory:");
            crmConn.Open();
            using var authConn = new SqliteConnection("Filename=:memory:");
            authConn.Open();

            var crmOptions = new DbContextOptionsBuilder<TWCRM_TESTContext>().UseSqlite(crmConn).Options;
            var authOptions = new DbContextOptionsBuilder<TWAUTH_TESTContext>().UseSqlite(authConn).Options;

            using var crmContext = new TWCRM_TESTContext(crmOptions);
            using var authContext = new TWAUTH_TESTContext(authOptions);

            var customerRepo = new Mock<ICustomerRepository>();
            var memberRepo = new Mock<IMemberRepository>();
            var authRepo = new Mock<IAuthRepository>();
            var logger = new Mock<ILogger<MemberApplicationService>>();

            var user = new User { Id = 1, Email = "user@test.com" };
            var member = new Member { Id = 2, UserId = 1, Customer = new Customer { Id = 3 } };

            authRepo.Setup(r => r.GetUserByEmail(user.Email)).Returns(user);
            memberRepo.Setup(r => r.GetMembersByUser(user.Id)).Returns(member);
            customerRepo.Setup(r => r.UpdateCustomer(It.IsAny<Customer>())).Throws(new InvalidOperationException("fail"));

            var service = new MemberApplicationService(customerRepo.Object, memberRepo.Object, authRepo.Object, crmContext, authContext, logger.Object);

            Assert.Throws<InvalidOperationException>(() => service.UpdateMember(new MemberUpdate
            {
                Email = user.Email,
                BrandId = 1,
                StoreId = 1,
                MemberTierId = 1,
                FirstName = "First",
                LastName = "Last",
                Gender = "M",
                Address = "addr",
                Birthday = new DateTime(2000, 1, 1),
                Phone = "123",
                CustomerId = 3,
                UserId = user.Id,
                IsActive = true
            }));
        }

        [Fact]
        public void RegisterMember_WithMissingCustomerId_Throws()
        {
            using var crmContext = TestDbContextFactory.CreateCrmContext(withCustomer: false);
            using var authContext = TestDbContextFactory.CreateAuthContext();

            var customerRepo = new Mock<ICustomerRepository>();
            var memberRepo = new Mock<IMemberRepository>();
            var authRepo = new Mock<IAuthRepository>();
            var logger = new Mock<ILogger<MemberApplicationService>>();

            var service = new MemberApplicationService(customerRepo.Object, memberRepo.Object, authRepo.Object, crmContext, authContext, logger.Object);

            var request = new MemberCreate
            {
                CustomerId = 999,
                UserId = null,
                BrandId = 1,
                StoreId = 1,
                MemberTierId = 1,
                Email = "missing@test.com",
                PassWord = "pwd",
                FirstName = "First",
                LastName = "Last",
                Gender = "M",
                Address = "addr",
                BirthDay = new DateTime(2000, 1, 1),
                Phone = "123"
            };

            Assert.Throws<ArgumentException>(() => service.RegisterMember(request));
        }

        [Fact]
        public void RegisterMember_WithMissingUserId_Throws()
        {
            using var crmContext = TestDbContextFactory.CreateCrmContext(withCustomer: true);
            using var authContext = TestDbContextFactory.CreateAuthContext();

            var customerRepo = new Mock<ICustomerRepository>();
            var memberRepo = new Mock<IMemberRepository>();
            var authRepo = new Mock<IAuthRepository>();
            var logger = new Mock<ILogger<MemberApplicationService>>();

            authRepo.Setup(r => r.UserExists(999)).Returns(false);

            var service = new MemberApplicationService(customerRepo.Object, memberRepo.Object, authRepo.Object, crmContext, authContext, logger.Object);

            var request = new MemberCreate
            {
                CustomerId = 1,
                UserId = 999,
                BrandId = 1,
                StoreId = 1,
                MemberTierId = 1,
                Email = "missing@test.com",
                PassWord = "pwd",
                FirstName = "First",
                LastName = "Last",
                Gender = "M",
                Address = "addr",
                BirthDay = new DateTime(2000, 1, 1),
                Phone = "123"
            };

            Assert.Throws<ArgumentException>(() => service.RegisterMember(request));
        }
    }

    internal static class TestDbContextFactory
    {
        public static TWCRM_TESTContext CreateCrmContext(bool withCustomer = false)
        {
            var options = new DbContextOptionsBuilder<TWCRM_TESTContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new TWCRM_TESTContext(options);

            if (withCustomer)
            {
                context.Customers.Add(new Customer
                {
                    Id = 1,
                    BrandId = 1,
                    FirstName = "F",
                    LastName = "L",
                    Address = "addr",
                    BirthDay = new DateTime(1990, 1, 1),
                    Gender = "M",
                    Phone = "123"
                });
                context.SaveChanges();
            }

            return context;
        }

        public static TWAUTH_TESTContext CreateAuthContext()
        {
            var options = new DbContextOptionsBuilder<TWAUTH_TESTContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new TWAUTH_TESTContext(options);
        }
    }
}
