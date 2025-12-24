using 水水水果API.Models.DTO;

namespace 水水水果API.Interfaces
{
    public interface IMemberService
    {
        IEnumerable<MemberResponse> GetMembers(List<int> customers);
        IEnumerable<MemberResponse> GetMembersByPage(int page, int pageSize);
        void DeleteMember(int id);
        MemberResponse GetMemberById(int id);

        /// <summary>
        /// 更新會員資料（僅限 Member 本身欄位，不涉及 Customer/User）
        /// </summary>
        int UpdateMember(MemberUpdate memberUpdate);
    }
}
