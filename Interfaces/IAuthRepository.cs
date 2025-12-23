namespace 水水水果API.Interfaces
{
    public interface IAuthRepository
    {
        public int UpsertUser(UserUpsert user);
        List<User> GetUserByList(List<int> users);
        User GetUserById(int user);
    }
}
