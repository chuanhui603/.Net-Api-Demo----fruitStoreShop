using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace 水水水果API.Models
{
    public class Member
    {
        [Column("id")]
        public Guid Id { get; set; } // char(36)

        [Column("first_name")]
        public string FirstName { get; set; } // varchar(20)

        [Column("last_name")]
        public string LastName { get; set; } // varchar(20)

        [Column("gender")]
        public string Gender { get; set; } // enum('M','F')

        [Column("birth_date")]
        public DateTime BirthDate { get; set; } // date

        [Column("postal_code")]
        public string PostalCode { get; set; } // varchar(6)

        [Column("city")]
        public string City { get; set; } // varchar(10)

        [Column("region")]
        public string Region { get; set; } // varchar(10)

        [Column("address")]
        public string Address { get; set; } // varchar(100)

        [Column("phone_number")]
        public string PhoneNumber { get; set; } // varchar(10)

        [Column("email")]
        public string Email { get; set; } // varchar(100)

        [Column("password")]
        public string Password { get; set; } // char(60)

        [Column("login_role")]
        public string LoginRole { get; set; } // enum('Admin','User')

        [Column("is_verified")]
        public bool IsVerified { get; set; } // tinyint(1)

        [Column("last_login_time")]
        public DateTime? LastLoginTime { get; set; } // timestamp

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } // timestamp

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } // timestamp
    }
}