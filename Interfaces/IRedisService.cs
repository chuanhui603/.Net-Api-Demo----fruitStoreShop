namespace 水水水果API.Interfaces
{
    public interface IRedisService
    {
        Task StoreAuthTokenAsync(string userId, string token, TimeSpan expiry);
        Task<bool> ValidateAuthTokenAsync(string userId, string token);
        Task InvalidateAuthTokenAsync(string userId);
        
        Task StoreUserSessionAsync(string sessionId, string userData, TimeSpan expiry);
        Task<string> GetUserSessionAsync(string sessionId);
        Task RemoveUserSessionAsync(string sessionId);
        
        // 快取管理
        Task<T> GetOrCreateCacheAsync<T>(string key, Func<Task<T>> factory, TimeSpan expiry) where T : class;
        Task RemoveCacheAsync(string key);
        Task<bool> KeyExistsAsync(string key);
        
        // 資料鎖定（用於平行處理）
        Task<bool> AcquireLockAsync(string lockKey, TimeSpan expiry);
        Task ReleaseLockAsync(string lockKey);
        
        // 使用者登入/登出管理
        bool IsUserLoggedOut(string userEmail);
        void RemoveUserFromLogoutList(string userEmail);
        void AddUserToLogoutList(string userEmail);
    }
}