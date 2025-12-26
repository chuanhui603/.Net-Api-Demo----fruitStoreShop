using System.Data.Common;
using 水水水果API.Interfaces;
using 水水水果API.Models.DTO;

namespace 水水水果API.Interfaces
{

    public interface IMemberApplicationService
    {
        MemberResponse RegisterMember(MemberCreate memberCreate);
        void UpdateMember(MemberUpdate memberUpdate);
    }
}
