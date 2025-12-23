using 水水水果API.Models.DTO;

namespace 水水水果API.Interfaces
{
    public interface IMemberRepository
    {
        IEnumerable<MemberResponse> GetMember(List<int> customers);
        IEnumerable<MemberResponse> GetMemberByPage(int page, int pageSize);
        Member GetMemberById(int id);
        Member GetMemberByIdTracking(int id);
        int CreateMember(Member member);
        int UpdateMember(Member member);
        void DeleteMember(Member member);
    }
}