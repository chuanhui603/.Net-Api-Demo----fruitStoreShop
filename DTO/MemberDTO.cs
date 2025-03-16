namespace 水水水果API.DTO
{
    public record MemberDTO
    {
        public Guid Id { get; set; } // Changed to Guid
        public string FirstName { get; set; } // varchar(20)
        public string LastName { get; set; } // varchar(20)
        public string Gender { get; set; } // enum('M','F')
        public DateTime? BirthDate { get; set; } // date
        public string PostalCode { get; set; } // varchar(6)
        public string City { get; set; } // varchar(10)
        public string Region { get; set; } // varchar(10)
        public string Address { get; set; } // varchar(100)
        public string PhoneNumber { get; set; } // varchar(10)
        public string Email { get; set; } // varchar(100)
        public string Password { get; set; } // char(60)
        public string LoginRole { get; set; } // enum('Admin','User')
        public bool? IsVerified { get; set; } // tinyint(1)
        public DateTime? LastLoginTime { get; set; } // timestamp
        public DateTime? UpdatedAt { get; set; } // timestamp
        public DateTime? CreatedAt { get; set; } // timestamp
    }
}