namespace 水水水果API.Models.DTO.Login
{
    public class LoginResponseDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public MemberResponse User { get; set; }
        public DateTime Expiration { get; set; }
    }
}
