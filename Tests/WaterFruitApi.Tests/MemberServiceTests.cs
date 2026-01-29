using Microsoft.Extensions.Logging;
using Moq;
using 水水水果API.Interfaces;
using 水水水果API.Models.CRM;
using 水水水果API.Models.DTO;
using 水水水果API.Models.AUTH;
using 水水水果API.Services;

namespace WaterFruitApi.Tests
{
    public class MemberServiceTests
    {
        [Fact]
        public void GetMembers_ReturnsRepositoryValue()
        {
            var repo = new Mock<IMemberRepository>();
            var logger = new Mock<ILogger<MemberService>>();
            var members = new List<MemberResponse> { new MemberResponse { Email = "a@test.com" } };
            repo.Setup(r => r.GetMember(new List<int> { 1 })).Returns(members);

            var service = new MemberService(logger.Object, repo.Object);

            Assert.Equal(members, service.GetMembers(new List<int> { 1 }));
        }

        [Fact]
        public void GetMemberByUser_WhenMissing_Throws()
        {
            var repo = new Mock<IMemberRepository>();
            var logger = new Mock<ILogger<MemberService>>();
            repo.Setup(r => r.GetMembersByUser(1)).Returns((Member)null);

            var service = new MemberService(logger.Object, repo.Object);

            Assert.Throws<ArgumentException>(() => service.GetMemberByUser(new User { Id = 1, Email = "none" }));
        }

        [Fact]
        public void GetMemberByUser_MapsToDto()
        {
            var repo = new Mock<IMemberRepository>();
            var logger = new Mock<ILogger<MemberService>>();

            var member = new Member
            {
                Id = 2,
                Customer = new Customer
                {
                    FirstName = "First",
                    LastName = "Last",
                    Phone = "123",
                    BirthDay = new DateTime(2000, 1, 1),
                    Gender = "M",
                    Address = "Somewhere"
                }
            };
            repo.Setup(r => r.GetMembersByUser(5)).Returns(member);

            var service = new MemberService(logger.Object, repo.Object);

            var result = service.GetMemberByUser(new User { Id = 5, Email = "user@test.com" });

            Assert.Equal("First", result.FirstName);
            Assert.Equal("user@test.com", result.Email);
            Assert.Equal("2000-01-01", result.Birthday);
        }

        [Fact]
        public void GetMemberById_MapsToDto()
        {
            var repo = new Mock<IMemberRepository>();
            var logger = new Mock<ILogger<MemberService>>();

            var member = new Member
            {
                Id = 4,
                Customer = new Customer
                {
                    FirstName = "A",
                    LastName = "B",
                    Phone = "111",
                    BirthDay = new DateTime(1990, 5, 5),
                    Gender = "F",
                    Address = "Addr"
                }
            };
            repo.Setup(r => r.GetMemberById(4)).Returns(member);

            var service = new MemberService(logger.Object, repo.Object);

            var result = service.GetMemberById(4);
            Assert.Equal("A", result.FirstName);
            Assert.Equal("1990-05-05", result.Birthday);
        }

        [Fact]
        public void DeleteMember_WhenMissing_Throws()
        {
            var repo = new Mock<IMemberRepository>();
            var logger = new Mock<ILogger<MemberService>>();
            repo.Setup(r => r.GetMembersByUser(3)).Returns((Member)null);

            var service = new MemberService(logger.Object, repo.Object);

            Assert.Throws<ArgumentException>(() => service.DeleteMember(3));
        }

        [Fact]
        public void DeleteMember_WhenExists_CallsRepository()
        {
            var repo = new Mock<IMemberRepository>();
            var logger = new Mock<ILogger<MemberService>>();
            var member = new Member { Id = 9 };
            repo.Setup(r => r.GetMembersByUser(9)).Returns(member);

            var service = new MemberService(logger.Object, repo.Object);
            service.DeleteMember(9);

            repo.Verify(r => r.DeleteMember(member), Times.Once);
        }
    }
}
