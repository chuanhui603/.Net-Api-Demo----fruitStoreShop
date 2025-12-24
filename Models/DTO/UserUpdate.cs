namespace 水水水果API.Models.DTO
{
    /// <summary>
    /// 用於更新 User 的 DTO（Update Only）
    /// UserId 必填
    /// </summary>
    public class UserUpdate
    {
        /// <summary>
        /// 必填：要更新的 User ID
        /// </summary>
        public int UserId { get; set; }

        public string Email { get; set; }

        public string PassWord { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public DateTime BirthDay { get; set; }

        public string Phone { get; set; }

        public bool IsActive { get; set; }

        public string Provider { get; set; }

        public string ProviderId { get; set; }

        public string ProviderEmail { get; set; }
    }
}
