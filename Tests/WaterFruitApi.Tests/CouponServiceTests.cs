using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using 水水水果API.Interfaces;
using 水水水果API.Models.DTO;
using 水水水果API.Models.RMS;
using 水水水果API.Services;

namespace WaterFruitApi.Tests
{
    public class CouponServiceTests
    {
        [Fact]
        public void GetCoupons_ReturnsDtoList()
        {
            var repo = new Mock<ICouponRepository>();
            var logger = NullLogger<CouponService>.Instance;
            var httpAccessor = new Mock<IHttpContextAccessor>();

            repo.Setup(r => r.GetCoupons()).Returns(new List<Coupon>
            {
                new Coupon { Id = 1 },
                new Coupon { Id = 2 }
            });

            var service = new CouponService(logger, repo.Object, httpAccessor.Object);
            var result = service.GetCoupons();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetCouponByPage_ReturnsDtoList()
        {
            var repo = new Mock<ICouponRepository>();
            var logger = NullLogger<CouponService>.Instance;
            var httpAccessor = new Mock<IHttpContextAccessor>();

            repo.Setup(r => r.GetCouponsByPage(1, 10)).Returns(new List<Coupon> { new Coupon(), new Coupon() });

            var service = new CouponService(logger, repo.Object, httpAccessor.Object);
            var result = service.GetCouponByPage(1, 10);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetCouponByCode_ReturnsDto()
        {
            var repo = new Mock<ICouponRepository>();
            var logger = NullLogger<CouponService>.Instance;
            var httpAccessor = new Mock<IHttpContextAccessor>();

            repo.Setup(r => r.GetCouponByCode("ABC", 1)).Returns(new Coupon { Id = 1, Code = "ABC" });

            var service = new CouponService(logger, repo.Object, httpAccessor.Object);
            var result = service.GetCouponByCode("ABC", 1);

            Assert.NotNull(result);
        }

        [Fact]
        public void GetCouponById_ReturnsDto()
        {
            var repo = new Mock<ICouponRepository>();
            var logger = NullLogger<CouponService>.Instance;
            var httpAccessor = new Mock<IHttpContextAccessor>();

            repo.Setup(r => r.GetCouponById(5)).Returns(new Coupon { Id = 5 });

            var service = new CouponService(logger, repo.Object, httpAccessor.Object);
            var result = service.GetCouponById(5);

            Assert.NotNull(result);
        }

        [Fact]
        public void CreateCoupon_InvokesRepository()
        {
            var repo = new Mock<ICouponRepository>();
            var logger = NullLogger<CouponService>.Instance;
            var httpAccessor = new Mock<IHttpContextAccessor>();

            var service = new CouponService(logger, repo.Object, httpAccessor.Object);
            var dto = new CouponDTO { Id = 1, Code = "A" };

            service.CreateCoupon(dto);

            repo.Verify(r => r.CreateCoupon(It.IsAny<Coupon>()), Times.Once);
        }

        [Fact]
        public void RegisterMemberCode_WhenCouponMissing_DoesNothing()
        {
            var repo = new Mock<ICouponRepository>();
            var logger = NullLogger<CouponService>.Instance;
            var httpAccessor = new Mock<IHttpContextAccessor>();

            repo.Setup(r => r.GetCouponByCode("ABC", 1)).Returns((Coupon)null);

            var service = new CouponService(logger, repo.Object, httpAccessor.Object);
            service.RegisterMemberCode("ABC", 1);

            repo.Verify(r => r.RegiserCoupon(It.IsAny<CustomerCoupon>()), Times.Never);
        }

        [Fact]
        public void RegisterMemberCode_WhenCouponExists_RegistersCoupon()
        {
            var repo = new Mock<ICouponRepository>();
            var logger = NullLogger<CouponService>.Instance;
            var httpAccessor = new Mock<IHttpContextAccessor>();

            repo.Setup(r => r.GetCouponByCode("ABC", 1)).Returns(new Coupon { Id = 2 });

            var context = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim("Email", "user@test.com")
                }))
            };
            httpAccessor.SetupGet(x => x.HttpContext).Returns(context);

            var service = new CouponService(logger, repo.Object, httpAccessor.Object);
            service.RegisterMemberCode("ABC", 1);

            repo.Verify(r => r.RegiserCoupon(It.IsAny<CustomerCoupon>()), Times.Once);
        }

        [Fact]
        public void UpdateCoupon_InvokesRepository()
        {
            var repo = new Mock<ICouponRepository>();
            var logger = NullLogger<CouponService>.Instance;
            var httpAccessor = new Mock<IHttpContextAccessor>();

            var service = new CouponService(logger, repo.Object, httpAccessor.Object);
            service.UpdateCoupon(new CouponDTO { Id = 7 });

            repo.Verify(r => r.UpdateCoupon(It.IsAny<Coupon>()), Times.Once);
        }

        [Fact]
        public void DeleteCoupon_InvokesRepository()
        {
            var repo = new Mock<ICouponRepository>();
            var logger = NullLogger<CouponService>.Instance;
            var httpAccessor = new Mock<IHttpContextAccessor>();

            var service = new CouponService(logger, repo.Object, httpAccessor.Object);
            service.DeleteCoupon(9);

            repo.Verify(r => r.DeleteCoupon(9), Times.Once);
        }
    }
}
