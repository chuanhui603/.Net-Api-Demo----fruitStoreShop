using 水水水果API.Models.DTO;

namespace 水水水果API.Interfaces
{
    public interface IMemberRepository
    {
        IEnumerable<MemberResponse> GetMember(List<int> customers);
        IEnumerable<MemberResponse> GetMemberByPage(int page, int pageSize);
        Member GetMemberById(int id);
        int UpsertMember(MemberUpsert customer);
        void DeleteMember(Member customer);
    }
}