using Microsoft.Extensions.Logging;
using Moq;
using 水水水果API.Interfaces;
using 水水水果API.Models.CRM;
using 水水水果API.Models.DTO;
using 水水水果API.Services;

namespace WaterFruitApi.Tests
{
    public class CustomerServiceTests
    {
        [Fact]
        public void GetCustomerById_ReturnsRepositoryValue()
        {
            var repo = new Mock<ICustomerRepository>();
            var logger = new Mock<ILogger<CustomerService>>();
            var customer = new Customer { Id = 1, FirstName = "A" };
            repo.Setup(r => r.GetCustomerById(1)).Returns(customer);

            var service = new CustomerService(logger.Object, repo.Object);

            Assert.Equal(customer, service.GetCustomerById(1));
        }

        [Fact]
        public void CreateCustomer_MapsFields()
        {
            var repo = new Mock<ICustomerRepository>();
            var logger = new Mock<ILogger<CustomerService>>();
            var dto = new CustomerCreate
            {
                BrandId = 5,
                FirstName = "First",
                LastName = "Last",
                BirthDay = new DateTime(2000, 1, 1),
                Gender = "M",
                Phone = "123"
            };

            repo.Setup(r => r.CreateCustomer(It.Is<Customer>(c => c.BrandId == dto.BrandId && c.FirstName == dto.FirstName && c.Phone == dto.Phone))).Returns(10);

            var service = new CustomerService(logger.Object, repo.Object);

            Assert.Equal(10, service.CreateCustomer(dto));
        }

        [Fact]
        public void UpdateCustomer_WhenMissing_Throws()
        {
            var repo = new Mock<ICustomerRepository>();
            var logger = new Mock<ILogger<CustomerService>>();
            var dto = new CustomerUpdate { Id = 3 };
            repo.Setup(r => r.GetCustomerById(dto.Id)).Returns((Customer)null);

            var service = new CustomerService(logger.Object, repo.Object);

            Assert.Throws<ArgumentException>(() => service.UpdateCustomer(dto));
        }

        [Fact]
        public void UpdateCustomer_WhenExists_MapsAndCallsRepository()
        {
            var repo = new Mock<ICustomerRepository>();
            var logger = new Mock<ILogger<CustomerService>>();
            var dto = new CustomerUpdate
            {
                Id = 3,
                BrandId = 7,
                FirstName = "First",
                LastName = "Last",
                BirthDay = new DateTime(1999, 2, 2),
                Gender = "F",
                Phone = "321"
            };

            repo.Setup(r => r.GetCustomerById(dto.Id)).Returns(new Customer { Id = dto.Id });
            repo.Setup(r => r.UpdateCustomer(It.Is<Customer>(c => c.Id == dto.Id && c.FirstName == dto.FirstName))).Returns(1);

            var service = new CustomerService(logger.Object, repo.Object);

            Assert.Equal(1, service.UpdateCustomer(dto));
        }
    }
}
