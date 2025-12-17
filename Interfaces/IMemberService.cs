using 水水水果API.Models.DTO;

namespace 水水水果API.Interfaces
{
    public interface IMemberService
    {
        IEnumerable<UserResponse> GetMembers(List<int> customers);
        IEnumerable<UserResponse> GetMembersByPage(int page, int pageSize);
        void CreateMember(UserCreate member);
        void DeleteCustomer(int id);
        UserResponse GetMemberById(int id);
        void UpdateMember(UserUpdate member);
    }
}
