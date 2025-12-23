using 水水水果API.Models.DTO;

namespace 水水水果API.Interfaces
{
    public interface IMemberRepository
    {
        IEnumerable<UserResponse> GetMember(List<int> customers);
        IEnumerable<UserResponse> GetMemberByPage(int page, int pageSize);
        Member GetMemberById(int id);
        int UpsertMember(UserUpdate customer);
        void DeleteMember(Member customer);
    }
}