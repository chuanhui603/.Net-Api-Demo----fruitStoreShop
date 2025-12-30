using 水水水果API.Models.DTO;

namespace 水水水果API.Interfaces
{
    public interface IMemberService
    {
        IEnumerable<MemberResponse> GetMembers(List<int> customers);
        IEnumerable<MemberResponse> GetMembersByPage(int page, int pageSize);
        MemberResponse GetMemberByUser(User user);
        void DeleteMember(int userId);
        MemberResponse GetMemberById(int id);
     
    }
}
