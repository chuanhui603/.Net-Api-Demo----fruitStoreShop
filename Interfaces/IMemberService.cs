﻿namespace 水水水果API.Interfaces
{
    public interface IMemberService
    {
        IEnumerable<MemberDTO> GetMembers();
        IEnumerable<MemberDTO> GetMembersByPage(int page, int pageSize);
        void CreateMember(MemberDTO member);
        void DeleteMember(Guid id);
        MemberDTO GetMemberById(Guid id);
        void UpdateMember(Guid id, MemberDTO member);
    }
}
