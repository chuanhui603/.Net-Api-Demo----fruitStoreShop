using Framework.SqlCommon.SQLHelper;
using FrameWork.Helper.Transfer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using 水水水果API.Models.DTO;
using 水水水果API.Models.DTO.Login;

namespace 水水水果API.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ILogger<AuthRepository> _logger;
        private readonly TWAUTH_TESTContext _authConnection;
        private readonly DbConnection _authConnect;
        private readonly SqlCommonContext _commonContext;

        public AuthRepository(TWAUTH_TESTContext authConnection, SqlCommonContext commonContext, ILogger<AuthRepository> logger)
        {
            _logger = logger;
            _commonContext = commonContext;
            _authConnection = authConnection;
            _authConnect = authConnection.Database.GetDbConnection();
        }

        public List<User> GetUserByList(List<int> users)
        {
            return [.. _authConnection.Users.Where(x => users.Contains(x.Id))];
        }

        public User GetUserById(int user)
        {
            return GetUserByList([user]).SingleOrDefault();
        }

        public int CreateUser(User user)
        {
            DateTime dateNow = _commonContext.Auth.GetDbDate(_authConnect, _authConnection.Database.CurrentTransaction);

            var newUser = new User
            {
                Email = user.Email,
                PassWord = user.PassWord,
                IsActive = user.IsActive,
                Provider = user.Provider,
                ProviderId = user.ProviderId,
                ProviderEmail = user.ProviderEmail,
                CreateDate = dateNow,
                LastUpdateDate = dateNow
            };

            _authConnection.Users.Add(newUser);
            _authConnection.SaveChanges();

            return newUser.Id;
        }

        public int UpdateUser(User user)
        {
            DateTime dateNow = _commonContext.Auth.GetDbDate(_authConnect, _authConnection.Database.CurrentTransaction);
            var targetUser = _authConnection.Users.Find(user.Id)
                ?? throw new ArgumentException($"User with ID {user.Id} not found.");

            targetUser.Email = user.Email;
            targetUser.PassWord = user.PassWord;
            targetUser.IsActive = user.IsActive;
            targetUser.Provider = user.Provider;
            targetUser.ProviderId = user.ProviderId;
            targetUser.ProviderEmail = user.ProviderEmail;

            _authConnection.Entry(targetUser).Property(x => x.CreateDate).IsModified = false;
            targetUser.LastUpdateDate = dateNow;

            _authConnection.SaveChanges();

            return targetUser.Id;
        }

        public bool UserExists(int userId)
        {
            return _authConnection.Users.AsNoTracking().SingleOrDefault(x => x.Id == userId) != null;
        }

        public User GetUserByLogin(LoginDTO loginDTO)
        {
            return _authConnection.Users.AsNoTracking().SingleOrDefault(x => x.Email == loginDTO.Email && x.PassWord == loginDTO.Password);
        }

        public bool ValidUserByEmail(string email)
        {
            return _authConnection.Users.AsNoTracking().FirstOrDefault(x => x.Email == email) != null;
        }
    }
}
