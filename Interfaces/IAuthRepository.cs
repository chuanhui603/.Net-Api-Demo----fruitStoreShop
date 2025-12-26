using System.Data.Common;
using 水水水果API.Models.DTO;

namespace 水水水果API.Interfaces
{
    public interface IAuthRepository
    {
        int CreateUser(User userCreate);

        int UpdateUser(User userUpdate);

        bool UserExists(int userId);

        List<User> GetUserByList(List<int> users);

        User GetUserById(int user);
        User GetUserByEmail(string email);
        bool ValidUserByPassword(string password);
    }
}
