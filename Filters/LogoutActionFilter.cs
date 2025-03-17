using Microsoft.AspNetCore.Mvc.Filters;

namespace 水水水果API.Filters
{
    public class LogoutActionFilter(IHttpContextAccessor httpcontext, IConnectionMultiplexer redis, IOptions<RedisSettingModel> options) : IAsyncActionFilter
    {
        private readonly IConnectionMultiplexer _redis = redis;
        private readonly IHttpContextAccessor _httpcontext = httpcontext;
        private readonly RedisSettingModel _redisSetting = options.Value;
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = _httpcontext.HttpContext.User;
            if (user != null)
            {
                var memberIdClaim = user.Claims.FirstOrDefault(c => c.Type == "Email");
                if (memberIdClaim != null)
                {
                    var db = _redis.GetDatabase();
                    // 取得 logout list 的所有值
                    var logoutIds = await db.ListRangeAsync(_redisSetting.LogoutDefault);
                    if (logoutIds.Any(x => x.ToString() == memberIdClaim.Value))
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }
                }
            }
            await next();
        }
    }
}