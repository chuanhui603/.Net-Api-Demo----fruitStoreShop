using 水水水果API.Models.DTO;

namespace 水水水果API.Interfaces
{
    public interface IMemberService
    {
        IEnumerable<MemberResponse> GetMembers(List<int> customers);
        IEnumerable<MemberResponse> GetMembersByPage(int page, int pageSize);
        void DeleteCustomer(int id);
        MemberResponse GetMemberById(int id);
        void UpdateMember(MemberUpsert member);
    }
}
