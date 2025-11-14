
namespace 水水水果API.Services
{
    public class RedisService : IRedisService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<RedisService> _logger;
        private readonly RedisSettingModel _redisSetting;

        public RedisService(ILogger<RedisService> logger, IOptions<RedisSettingModel> options,IConnectionMultiplexer redis)
        {
            _redis = redis;
            _logger = logger;
            _redisSetting = options.Value;
        }

       

        #region 認證相關

        public async Task StoreAuthTokenAsync(string userId, string token, TimeSpan expiry)
        {
            var db = _redis.GetDatabase();
            string key = $"auth:{userId}";
            await db.StringSetAsync(key, token, expiry);
            _logger.LogInformation("已儲存用戶 {userId} 的認證權杖，有效期限: {expiry}", userId, expiry);
        }

        public async Task<bool> ValidateAuthTokenAsync(string userId, string token)
        {
            var db = _redis.GetDatabase();
            string key = $"auth:{userId}";
            string storedToken = await db.StringGetAsync(key);

            bool isValid = storedToken == token;
            if (!isValid)
            {
                _logger.LogWarning("用戶 {userId} 的認證權杖驗證失敗", userId);
            }

            return isValid;
        }

        public async Task InvalidateAuthTokenAsync(string userId)
        {
            var db = _redis.GetDatabase();
            string key = $"auth:{userId}";
            await db.KeyDeleteAsync(key);
            // 從字串插值修改為結構化日誌
            _logger.LogInformation("用戶 {userId} 的認證權杖已失效", userId);
        }

        #endregion

        #region 使用者工作階段管理

        public async Task StoreUserSessionAsync(string sessionId, string userData, TimeSpan expiry)
        {
            var db = _redis.GetDatabase();
            string key = $"session:{sessionId}";
            await db.StringSetAsync(key, userData, expiry);
        }

        public async Task<string> GetUserSessionAsync(string sessionId)
        {
            var db = _redis.GetDatabase();
            string key = $"session:{sessionId}";
            return await db.StringGetAsync(key);
        }

        public async Task RemoveUserSessionAsync(string sessionId)
        {
            var db = _redis.GetDatabase();
            string key = $"session:{sessionId}";
            await db.KeyDeleteAsync(key);
        }

        #endregion

        #region 快取管理

        public async Task<T> GetOrCreateCacheAsync<T>(string key, Func<Task<T>> factory, TimeSpan expiry) where T : class
        {
            var db = _redis.GetDatabase();
            string cacheKey = $"cache:{key}";

            // 嘗試從快取取得
            var cachedValue = await db.StringGetAsync(cacheKey);
            if (cachedValue.HasValue)
            {
                try
                {
                    return JsonConvert.DeserializeObject<T>(cachedValue);
                }
                catch (Exception ex)
                {
                    // 修正為結構化日誌
                    _logger.LogError(ex, "解析快取資料時發生錯誤: {key}", key);
                }
            }

            // 建立新值
            var value = await factory();
            if (value != null)
            {
                try
                {
                    string serialized = JsonConvert.SerializeObject(value);
                    await db.StringSetAsync(cacheKey, serialized, expiry);
                }
                catch (Exception ex)
                {
                    // 修正為結構化日誌
                    _logger.LogError(ex, "序列化快取資料時發生錯誤: {key}", key);
                }
            }

            return value;
        }

        public async Task RemoveCacheAsync(string key)
        {
            var db = _redis.GetDatabase();
            string cacheKey = $"cache:{key}";
            await db.KeyDeleteAsync(cacheKey);
        }

        public async Task<bool> KeyExistsAsync(string key)
        {
            var db = _redis.GetDatabase();
            return await db.KeyExistsAsync(key);
        }

        #endregion

        #region 資料鎖定

        public async Task<bool> AcquireLockAsync(string lockKey, TimeSpan expiry)
        {
            var db = _redis.GetDatabase();
            string key = $"lock:{lockKey}";
            return await db.StringSetAsync(key, "1", expiry, When.NotExists);
        }

        public async Task ReleaseLockAsync(string lockKey)
        {
            var db = _redis.GetDatabase();
            string key = $"lock:{lockKey}";
            await db.KeyDeleteAsync(key);
        }

        #endregion

        #region 使用者登入/登出管理

        /// <summary>
        /// 檢查使用者是否在登出清單中
        /// </summary>
        public bool IsUserLoggedOut(string userEmail)
        {
            var db = _redis.GetDatabase();
            var key = _redisSetting.LogoutDefault;
            return db.ListRange(key).Any(id => id == userEmail);
        }

        /// <summary>
        /// 從登出清單中移除使用者
        /// </summary>
        public void RemoveUserFromLogoutList(string userEmail)
        {
            var db = _redis.GetDatabase();
            var key = _redisSetting.LogoutDefault;
            if (IsUserLoggedOut(userEmail))
            {
                db.ListRemove(key, userEmail);
                _logger.LogInformation("使用者 {email} 已從登出清單移除", userEmail);
            }
        }

        /// <summary>
        /// 將使用者加入登出清單
        /// </summary>
        public void AddUserToLogoutList(string userEmail)
        {
            var db = _redis.GetDatabase();
            var key = _redisSetting.LogoutDefault;
            db.ListLeftPush(key, userEmail);
            _logger.LogInformation("使用者 {email} 已加入登出清單 {key}", userEmail, key);
        }

        #endregion
    }
}