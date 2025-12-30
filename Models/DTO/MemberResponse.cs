using System.ComponentModel.DataAnnotations;

namespace 水水水果API.Models.DTO
{
    public record MemberResponse
    {
        //尚未實作 預設Member
        public string Role { get; set; } = "Member";

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public string Birthday { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}