namespace 水水水果API.Models.DTO
{
    public class UserUpdate
    {
        public int? MemberId { get; set; }

        public int? CustomerId { get; set; }

        public int? MemberTierId { get; set; }

        public int BrandId { get; set; }

        public string Email { get; set; }

        public string PassWord { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public DateTime BirthDay { get; set; }

        public string Phone { get; set; }

        public bool IsActive { get; set; }

        public DateTime? LastLoginAt { get; set; }
    }
}
