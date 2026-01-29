using System.Security.Claims;
using FrameWork.Helper.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using 水水水果API.Interfaces;
using 水水水果API.Models.AUTH;
using 水水水果API.Models.ConfigurationModel;
using 水水水果API.Models.DTO;
using 水水水果API.Models.DTO.Login;
using 水水水果API.Services;

namespace WaterFruitApi.Tests
{
    public class AuthServiceTests
    {
        private static AuthService CreateService(
            Mock<IAuthRepository> authRepo,
            Mock<IMemberService> memberService,
            Mock<IRedisService> redisService,
            Mock<IHttpContextAccessor> httpContextAccessor,
            Mock<ILogger<AuthService>> logger,
            JWTModel options)
        {
            return new AuthService(
                logger.Object,
                new JWTHelper(),
                memberService.Object,
                authRepo.Object,
                httpContextAccessor.Object,
                redisService.Object,
                Options.Create(options));
        }

        [Fact]
        public void Login_RemovesLogoutEntryAndReturnsToken()
        {
            var authRepo = new Mock<IAuthRepository>();
            var memberService = new Mock<IMemberService>();
            var redisService = new Mock<IRedisService>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var logger = new Mock<ILogger<AuthService>>();

            var user = new User { Id = 1, Email = "user@test.com", PassWord = "pwd" };
            var member = new MemberResponse { Email = user.Email, FirstName = "Test" };
            var login = new LoginDTO { Email = user.Email, Password = user.PassWord };

            authRepo.Setup(r => r.GetUserByEmail(user.Email)).Returns(user);
            authRepo.Setup(r => r.ValidUserByPassword(login.Password)).Returns(true);
            memberService.Setup(m => m.GetMemberByUser(user)).Returns(member);
            redisService.Setup(r => r.IsUserLoggedOut(user.Email)).Returns(true);

            var service = CreateService(authRepo, memberService, redisService, httpContextAccessor, logger, new JWTModel
            {
                JWTIssuer = "issuer",
                JWTSignKey = "very-secret-sign-key-32bytes-long-0000"
            });

            var result = service.Login(login);

            redisService.Verify(r => r.RemoveUserFromLogoutList(user.Email), Times.Once);
            Assert.False(string.IsNullOrEmpty(result.AccessToken));
            Assert.Equal(member, result.User);
            Assert.InRange(result.Expiration, DateTime.Now.AddMinutes(59), DateTime.Now.AddMinutes(61));
        }

        [Fact]
        public void Login_InvalidPassword_Throws()
        {
            var authRepo = new Mock<IAuthRepository>();
            var memberService = new Mock<IMemberService>();
            var redisService = new Mock<IRedisService>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var logger = new Mock<ILogger<AuthService>>();

            var user = new User { Id = 1, Email = "user@test.com", PassWord = "pwd" };
            var login = new LoginDTO { Email = user.Email, Password = "wrong" };

            authRepo.Setup(r => r.GetUserByEmail(user.Email)).Returns(user);
            memberService.Setup(m => m.GetMemberByUser(user)).Returns(new MemberResponse());
            authRepo.Setup(r => r.ValidUserByPassword(login.Password)).Returns(false);

            var service = CreateService(authRepo, memberService, redisService, httpContextAccessor, logger, new JWTModel
            {
                JWTIssuer = "issuer",
                JWTSignKey = "very-secret-sign-key-32bytes-long-0000"
            });

            Assert.Throws<ArgumentException>(() => service.Login(login));
        }

        [Fact]
        public void Logout_WhenAlreadyLoggedOut_Throws()
        {
            var authRepo = new Mock<IAuthRepository>();
            var memberService = new Mock<IMemberService>();
            var redisService = new Mock<IRedisService>();
            var logger = new Mock<ILogger<AuthService>>();

            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim("Email", "user@test.com")
                }, "mock"))
            };
            httpContextAccessor.SetupGet(x => x.HttpContext).Returns(context);
            redisService.Setup(r => r.IsUserLoggedOut("user@test.com")).Returns(true);

            var service = CreateService(authRepo, memberService, redisService, httpContextAccessor, logger, new JWTModel());

            Assert.Throws<ArgumentException>(() => service.Logout());
            redisService.Verify(r => r.AddUserToLogoutList(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Logout_WhenNotLoggedOut_AddsToList()
        {
            var authRepo = new Mock<IAuthRepository>();
            var memberService = new Mock<IMemberService>();
            var redisService = new Mock<IRedisService>();
            var logger = new Mock<ILogger<AuthService>>();

            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim("Email", "user@test.com")
                }, "mock"))
            };
            httpContextAccessor.SetupGet(x => x.HttpContext).Returns(context);
            redisService.Setup(r => r.IsUserLoggedOut("user@test.com")).Returns(false);

            var service = CreateService(authRepo, memberService, redisService, httpContextAccessor, logger, new JWTModel());

            service.Logout();

            redisService.Verify(r => r.AddUserToLogoutList("user@test.com"), Times.Once);
        }

        [Fact]
        public void Logout_WhenMissingEmailClaim_SkipsLogout()
        {
            var authRepo = new Mock<IAuthRepository>();
            var memberService = new Mock<IMemberService>();
            var redisService = new Mock<IRedisService>();
            var logger = new Mock<ILogger<AuthService>>();

            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity())
            };
            httpContextAccessor.SetupGet(x => x.HttpContext).Returns(context);

            var service = CreateService(authRepo, memberService, redisService, httpContextAccessor, logger, new JWTModel());

            service.Logout();

            redisService.Verify(r => r.IsUserLoggedOut(It.IsAny<string>()), Times.Never);
            redisService.Verify(r => r.AddUserToLogoutList(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void ValidMemberByEmail_ReturnsTrueWhenUserExists()
        {
            var authRepo = new Mock<IAuthRepository>();
            var memberService = new Mock<IMemberService>();
            var redisService = new Mock<IRedisService>();
            var logger = new Mock<ILogger<AuthService>>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();

            authRepo.Setup(r => r.GetUserByEmail("user@test.com")).Returns(new User());

            var service = CreateService(authRepo, memberService, redisService, httpContextAccessor, logger, new JWTModel());

            Assert.True(service.ValidMemberByEmail("user@test.com"));
        }

        [Fact]
        public void ValidMemberByEmail_ReturnsFalseWhenMissing()
        {
            var authRepo = new Mock<IAuthRepository>();
            var memberService = new Mock<IMemberService>();
            var redisService = new Mock<IRedisService>();
            var logger = new Mock<ILogger<AuthService>>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();

            authRepo.Setup(r => r.GetUserByEmail("missing@test.com")).Returns((User)null);

            var service = CreateService(authRepo, memberService, redisService, httpContextAccessor, logger, new JWTModel());

            Assert.False(service.ValidMemberByEmail("missing@test.com"));
        }

        [Fact]
        public void CreateUser_MapsFieldsAndReturnsId()
        {
            var authRepo = new Mock<IAuthRepository>();
            var memberService = new Mock<IMemberService>();
            var redisService = new Mock<IRedisService>();
            var logger = new Mock<ILogger<AuthService>>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();

            var create = new UserCreate
            {
                Email = "user@test.com",
                PassWord = "pwd",
                Provider = "local",
                ProviderEmail = "user@test.com",
                ProviderId = "id",
                IsActive = true
            };

            authRepo.Setup(r => r.CreateUser(It.Is<User>(u => u.Email == create.Email && u.PassWord == create.PassWord))).Returns(42);

            var service = CreateService(authRepo, memberService, redisService, httpContextAccessor, logger, new JWTModel());

            Assert.Equal(42, service.CreateUser(create));
        }

        [Fact]
        public void UpdateUser_WhenMissing_Throws()
        {
            var authRepo = new Mock<IAuthRepository>();
            var memberService = new Mock<IMemberService>();
            var redisService = new Mock<IRedisService>();
            var logger = new Mock<ILogger<AuthService>>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();

            var update = new UserUpdate { UserId = 9, Email = "missing@test.com" };
            authRepo.Setup(r => r.GetUserById(update.UserId)).Returns((User)null);

            var service = CreateService(authRepo, memberService, redisService, httpContextAccessor, logger, new JWTModel());

            Assert.Throws<ArgumentException>(() => service.UpdateUser(update));
        }

        [Fact]
        public void UpdateUser_WhenExists_MapsAndCallsRepository()
        {
            var authRepo = new Mock<IAuthRepository>();
            var memberService = new Mock<IMemberService>();
            var redisService = new Mock<IRedisService>();
            var logger = new Mock<ILogger<AuthService>>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();

            var update = new UserUpdate { UserId = 3, Email = "user@test.com", PassWord = "pwd", Provider = "local", ProviderEmail = "mail", ProviderId = "pid", IsActive = true };
            authRepo.Setup(r => r.GetUserById(update.UserId)).Returns(new User { Id = update.UserId });
            authRepo.Setup(r => r.UpdateUser(It.Is<User>(u => u.Id == update.UserId && u.Email == update.Email))).Returns(1);

            var service = CreateService(authRepo, memberService, redisService, httpContextAccessor, logger, new JWTModel());

            Assert.Equal(1, service.UpdateUser(update));
        }

        [Fact]
        public void GetRefreshToken_ReturnsValueFromRepository()
        {
            var authRepo = new Mock<IAuthRepository>();
            var memberService = new Mock<IMemberService>();
            var redisService = new Mock<IRedisService>();
            var logger = new Mock<ILogger<AuthService>>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();

            var refreshToken = new RefreshToken { TokenJti = "token", UserId = 1 };
            authRepo.Setup(r => r.GetRefreshToken("token")).Returns(refreshToken);

            var service = CreateService(authRepo, memberService, redisService, httpContextAccessor, logger, new JWTModel());

            Assert.Equal(refreshToken, service.GetRefreshToken("token"));
        }

        [Fact]
        public void RefreshToken_ReturnsNull()
        {
            var authRepo = new Mock<IAuthRepository>();
            var memberService = new Mock<IMemberService>();
            var redisService = new Mock<IRedisService>();
            var logger = new Mock<ILogger<AuthService>>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();

            var service = CreateService(authRepo, memberService, redisService, httpContextAccessor, logger, new JWTModel());

            Assert.Null(service.RefreshToken("any"));
        }

        [Fact]
        public void GetUserByEmail_ReturnsRepositoryValue()
        {
            var authRepo = new Mock<IAuthRepository>();
            var memberService = new Mock<IMemberService>();
            var redisService = new Mock<IRedisService>();
            var logger = new Mock<ILogger<AuthService>>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();

            var user = new User { Id = 2, Email = "user@test.com" };
            authRepo.Setup(r => r.GetUserByEmail(user.Email)).Returns(user);

            var service = CreateService(authRepo, memberService, redisService, httpContextAccessor, logger, new JWTModel());

            Assert.Equal(user, service.GetUserByEmail(user.Email));
        }

        [Fact]
        public void GetUserById_ReturnsRepositoryValue()
        {
            var authRepo = new Mock<IAuthRepository>();
            var memberService = new Mock<IMemberService>();
            var redisService = new Mock<IRedisService>();
            var logger = new Mock<ILogger<AuthService>>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();

            var user = new User { Id = 4, Email = "user@test.com" };
            authRepo.Setup(r => r.GetUserById(4)).Returns(user);

            var service = CreateService(authRepo, memberService, redisService, httpContextAccessor, logger, new JWTModel());

            Assert.Equal(user, service.GetUserById(4));
        }
    }
}
