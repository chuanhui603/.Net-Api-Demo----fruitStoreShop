using 水水水果API.DTO.Login;

namespace 水水水果API.Interfaces
{
    public interface IAuthService
    {
        LoginResponseDTO Login(LoginDTO login);
        void Logout();
        LoginResponseDTO RefreshToken(string token);
    }
}
