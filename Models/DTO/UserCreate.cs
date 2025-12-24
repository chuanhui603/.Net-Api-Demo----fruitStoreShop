namespace 水水水果API.Models.DTO
{
    /// <summary>
    /// 用於新增 User 的 DTO（Create Only）
    /// </summary>
    public class UserCreate
    {
        public string Email { get; set; }
        public string PassWord { get; set; }
        public bool IsActive { get; set; } = true;
        public string Provider { get; set; }
        public string ProviderId { get; set; }
        public string ProviderEmail { get; set; }
    }
}
