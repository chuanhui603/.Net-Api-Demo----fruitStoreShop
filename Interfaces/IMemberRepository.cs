using 水水水果API.Models.DTO;

namespace 水水水果API.Interfaces
{
    public interface IMemberRepository
    {
        IEnumerable<UserResponse> GetMember(List<int> customers);
        IEnumerable<UserResponse> GetMemberByPage(int page, int pageSize);
        Member GetMemberById(int id);
        int CreateMember(UserCreate customer);
        void UpdateMember(UserUpdate customer);
        void DeleteMember(Member customer);
    }
}