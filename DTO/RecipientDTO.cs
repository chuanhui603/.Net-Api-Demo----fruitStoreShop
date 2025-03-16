namespace 水水水果API.DTO
{
    public class RecipientDTO
    {
        public Guid Id { get; set; } // Changed from int to string
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
