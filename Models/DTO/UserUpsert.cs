namespace 水水水果API.Models.DTO
{
    public class UserUpsert
    {
        public int? MemberId { get; set; }

        public int? UserId { get; set; }

        public string Email { get; set; }

        public string PassWord { get; set; }

        public bool IsActive { get; set; }

        public string Provider { get; set; }

        public string ProviderId { get; set; }

        public string ProviderEmail { get; set; }

    }
}
