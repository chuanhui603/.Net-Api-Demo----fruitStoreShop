using Framework.SqlCommon.SQLHelper;
using FrameWork.Helper.Transfer;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace 水水水果API.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ILogger<CustomerRepository> _logger;
        private readonly TWAUTH_TESTContext _authConnection;
        private readonly DbConnection _authConnect;
        private readonly SqlCommonContext _commonContext;

        public AuthRepository(TWAUTH_TESTContext authConnection, SqlCommonContext commonContext)
        {
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
            return GetUserByList([user]).Single();
        }

        public int UpsertUser(UserUpsert user)
        {
            DateTime dateNow = _commonContext.CRM.GetDbDate(_authConnect, _authConnection.Database.CurrentTransaction);
            User createMem = MappingHelper.ModelMapping<User>(user);
            User targetUser = _authConnection.Users.Find(user.UserId);
            int newId = 0;

            _commonContext.CRM.ExecuteTransaction(_authConnection, () =>
            {
                if (targetUser == null)
                {
                    createMem.CreateDate = dateNow;
                    createMem.LastUpdateDate = dateNow;
                    createMem.IsActive = true;
                    _authConnection.Users.Add(createMem);
                    _authConnection.SaveChanges();
                    newId = createMem.Id;
                }
                else
                {
                    _authConnection.Entry(targetUser).CurrentValues.SetValues(user);
                    _authConnection.Entry(targetUser).Property(x => x.CreateDate).IsModified = false;
                    targetUser.LastUpdateDate = dateNow;
                    _authConnection.SaveChanges();
                    newId = targetUser.Id;
                }
            });
            return newId;
        }
    }
}
