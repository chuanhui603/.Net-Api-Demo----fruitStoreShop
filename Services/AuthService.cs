using FrameWork.Helper.Models;
using FrameWork.Helper.Transfer;
using 水水水果API.Models.DTO;
using 水水水果API.Models.DTO.Login;


namespace 水水水果API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly ILogger<AuthService> _logger;
        private readonly JWTModel _options;
        private readonly JWTHelper _jwtHelper;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly IRedisService _redisService;

        public AuthService(
            ILogger<AuthService> logger,
            JWTHelper jwt,
            IMemberRepository memberRepository,
            IHttpContextAccessor httpcontext,
            IRedisService redisService,
            IOptions<JWTModel> options)
        {
            _memberRepository = memberRepository;
            _logger = logger;
            _jwtHelper = jwt;
            _httpcontext = httpcontext;
            _redisService = redisService;
            _options = options.Value;
        }

        public LoginResponseDTO Login(LoginDTO login)
        {
            _logger.LogInformation("開始登入程序");
            var user = GetUser(login);
            if (user == null) throw new ArgumentNullException("使用者不存在，請檢察Email和密碼");

            _logger.LogInformation("{user}", user);
            var userEmail = user.Email.ToString();

            if (_redisService.IsUserLoggedOut(userEmail))
            {
                _redisService.RemoveUserFromLogoutList(userEmail);
            }

            var token = _jwtHelper.GenerateToken(new JwtInfo{
                    Email = user.Email,
                    MemberId = user.Id,
                    JWTIssuer = _options.JWTIssuer,
                    JWTSignKey = _options.JWTSignKey
            });

            return new LoginResponseDTO
            {
                AccessToken = token,
                Expiration = DateTime.Now.AddMinutes(60)
            };
        }

        private User GetUser(LoginDTO login)
        {
            //預計實作
            return null;
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
                // 使用 RedisService 將使用者加入登出清單
                _redisService.AddUserToLogoutList(memberIdClaim.Value);
            }
            else
            {
                _logger.LogWarning("找不到使用者 Email，無法完成登出程序");
            }
        }

        public LoginResponseDTO RefreshToken(string refreshToken)
        {
            // 將來實作權杖重新整理功能
            return null;
        }

        public bool ValidMemberByEmail(string email)
        {
            throw new NotImplementedException();
        }
    }
}
