namespace 水水水果API.Interfaces
{
    public interface IMemberRepository
    {
        IEnumerable<Member> GetMembers();
        Member GetMemberById(Guid id);
        Member GetMemberByLogin(MemberDTO member);
        void CreateMember(Member member);
        void UpdateMember(Member member);
        void DeleteMember(Guid id);
    }
}