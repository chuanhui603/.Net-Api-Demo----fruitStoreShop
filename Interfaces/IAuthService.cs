using 水水水果API.Models.DTO;
using 水水水果API.Models.DTO.Login;

namespace 水水水果API.Interfaces;
public interface IAuthService
{
    LoginResponseDTO Login(LoginDTO login);
    void Logout();
    LoginResponseDTO RefreshToken(string refreshToken);
    bool ValidMemberByEmail(string email);
    User GetUserById(int userId);

    /// <summary>
    /// 新增 User
    /// </summary>
    int CreateUser(UserCreate userCreate);

    /// <summary>
    /// 更新 User
    /// </summary>
    int UpdateUser(UserUpdate userUpdate);
}
