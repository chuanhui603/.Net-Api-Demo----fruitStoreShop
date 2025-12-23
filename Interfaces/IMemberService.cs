using 水水水果API.Models.DTO;

namespace 水水水果API.Interfaces
{
    public interface IMemberService
    {
        IEnumerable<MemberResponse> GetMembers(List<int> customers);
        IEnumerable<MemberResponse> GetMembersByPage(int page, int pageSize);
        void DeleteMember(int id);
        MemberResponse GetMemberById(int id);
        int RegisterMember(MemberUpsert member);
        int UpdateMember(MemberUpsert member);
        int UpsertMember(MemberUpsert member);
    }
}
