
using 水水水果API.DTO.Login;

namespace 水水水果API.Services
{
    public class AuthService(ILogger<AuthService> logger,
        JWTHelper jwt, IMemberRepository memberRepository, IHttpContextAccessor httpcontext, IConnectionMultiplexer redis,IOptions<RedisSettingModel> options) : IAuthService
    {
        private readonly IMemberRepository _memberRepository = memberRepository;
        private readonly ILogger<AuthService> _logger = logger;
        private readonly JWTHelper _jwtHelper = jwt;
        private readonly IHttpContextAccessor _httpcontext = httpcontext;
        private readonly IConnectionMultiplexer _redis = redis;
        private readonly RedisSettingModel _redisSetting = options.Value;
        public LoginResponseDTO Login(LoginDTO login)
        {
            var user = GetUser(login);
            if (user.Email == null) return null;
            var db = _redis.GetDatabase();
            var key = "logout";
            var memberId = user.Id.ToString();
            if (db.ListRange(key).Any(id => id == memberId))
            {
                db.ListRemove(key, memberId);
            }
            var token = _jwtHelper.GenerateToken(user);
            return new LoginResponseDTO
            {
                Token = token,
                Expiration = DateTime.Now.AddMinutes(10)
            };
        }

        private Member GetUser(LoginDTO login)
        {
            return _memberRepository.GetMemberByLogin(new MemberDTO
            {
                Email = login.Email,
                Password = login.Password
            });
        }

        public void Logout()
        {
            _logger.LogInformation("Start Logout");
            var user = _httpcontext.HttpContext.User;
            var memberIdClaim = user.Claims.FirstOrDefault(c => c.Type == "MemberId");
            var db = _redis.GetDatabase();
            var key = _redisSetting.LogoutDefault;
            db.ListLeftPush(key, memberIdClaim.Value);
            _logger.LogInformation("MemberId {memberId} has been added to key {key}.", memberIdClaim.Value, key);
        }

        public LoginResponseDTO RefreshToken(string token)
        {
            return null;
        }
    }
}
