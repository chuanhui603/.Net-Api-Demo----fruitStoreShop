using FrameWork.Helper.Models;
using 水水水果API.Interfaces;
using User = 水水水果API.Models.AUTH.User;


namespace 水水水果API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMemberService _memberService;
        private readonly IAuthRepository _authRepository;
        private readonly ILogger<AuthService> _logger;
        private readonly JWTModel _options;
        private readonly JWTHelper _jwtHelper;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly IRedisService _redisService;

        public AuthService(
            ILogger<AuthService> logger,
            JWTHelper jwt,
            IMemberService memberService,
            IAuthRepository authRepository,
            IHttpContextAccessor httpcontext,
            IRedisService redisService,
            IOptions<JWTModel> options)
        {
            _memberService = memberService;
            _authRepository = authRepository;
            _logger = logger;
            _jwtHelper = jwt;
            _httpcontext = httpcontext;
            _redisService = redisService;
            _options = options.Value;
        }

        public LoginResponseDTO Login(LoginDTO login)
        {
            _logger.LogInformation("開始登入程序");
            User user = _authRepository.GetUserByEmail(login.Email);
            MemberResponse member = _memberService.GetMemberByUser(user);
            if (!_authRepository.ValidUserByPassword(login.Password))
                throw new ArgumentException($"Member valid Fail. Email or Password not found.");

            _logger.LogInformation("{user}", user);
            var userEmail = user.Email;

            if (_redisService.IsUserLoggedOut(userEmail))
            {
                _redisService.RemoveUserFromLogoutList(userEmail);
            }

            var token = _jwtHelper.GenerateToken(new JwtInfo
            {
                Email = user.Email,
                JWTIssuer = _options.JWTIssuer,
                JWTSignKey = _options.JWTSignKey,
                Role = member.Role,
            });

            return new LoginResponseDTO
            {
                AccessToken = token,
                Expiration = DateTime.Now.AddMinutes(60),
                User = member
            };
        }

        public void Logout()
        {
            _logger.LogInformation("開始登出程序");
            var user = _httpcontext.HttpContext.User;
            var memberIdClaim = user.Claims.FirstOrDefault(c => c.Type == "Email");
            if (_redisService.IsUserLoggedOut(memberIdClaim.Value))
            {
                throw new ArgumentException("會員未登入");
            }
            if (memberIdClaim != null && !string.IsNullOrEmpty(memberIdClaim.Value))
            {
                _redisService.AddUserToLogoutList(memberIdClaim.Value);
            }
            else
            {
                _logger.LogWarning("找不到使用者 Email，無法完成登出程序");
            }
        }

        public LoginResponseDTO RefreshToken(string refreshToken)
        {
          
            return null;
        }

        public RefreshToken GetRefreshToken(string token)
        {
            return _authRepository.GetRefreshToken(token);
        }

        public bool ValidMemberByEmail(string email)
        {
            return _authRepository.GetUserByEmail(email) != null;
        }

        public User GetUserByEmail(string email)
        {
            return _authRepository.GetUserByEmail(email);
        }

        public User GetUserById(int userId)
        {
            try
            {
                return _authRepository.GetUserById(userId);
            }
            catch
            {
                return null;
            }
        }

        public int CreateUser(UserCreate userCreate)
        {
            _logger.LogInformation("Creating new user");
            return _authRepository.CreateUser(new User()
            {
                Email = userCreate.Email,
                PassWord = userCreate.PassWord,
                Provider = userCreate.Provider,
                ProviderEmail = userCreate.ProviderEmail,
                ProviderId = userCreate.ProviderId,
                IsActive = userCreate.IsActive,
            });
        }

        public int UpdateUser(UserUpdate userUpdate)
        {
            var existingUser = GetUserById(userUpdate.UserId);
            if (existingUser == null)
            {
                throw new ArgumentException($"User with ID {userUpdate.UserId} not found.");
            }

            _logger.LogInformation("Updating existing user with ID: {UserId}", userUpdate.UserId);
            return _authRepository.UpdateUser(new User()
            {
                Id = userUpdate.UserId,
                Email = userUpdate.Email,
                PassWord = userUpdate.PassWord,
                Provider = userUpdate.Provider,
                ProviderEmail = userUpdate.ProviderEmail,
                ProviderId = userUpdate.ProviderId,
                IsActive = userUpdate.IsActive,

            });
        }
    }
}
