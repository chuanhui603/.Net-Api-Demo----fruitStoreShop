using System.Data.Common;
using 水水水果API.Models.DTO;

namespace 水水水果API.Interfaces
{
    public interface IMemberRepository
    {
        IEnumerable<MemberResponse> GetMember(List<int> customers);
        IEnumerable<MemberResponse> GetMemberByPage(int page, int pageSize);
        Member GetMemberById(int id);
        int CreateMember(Member member);
        int UpdateMember(Member member);
        void DeleteMember(Member member);
    }
}