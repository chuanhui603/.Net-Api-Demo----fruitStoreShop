using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using 水水水果API.Models.ConfigurationModel;
using 水水水果API.Models.DTO.LinePay;
using 水水水果API.Services;

namespace WaterFruitApi.Tests
{
    public class LinePayServiceTests
    {
        [Fact]
        public async Task SendPaymentRequest_ReturnsParsedResponse()
        {
            var logger = NullLogger<LinePayService>.Instance;
            var handler = new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new LinePayResponseDTO
                {
                    ReturnCode = "0000",
                    ReturnMessage = "OK",
                    Info = new ResponseInfoDTO { TransactionId = 123 }
                }), Encoding.UTF8, "application/json")
            });

            var service = new LinePayService(
                Options.Create(new LinePayModel { LineID = "id", LineKey = "key", LineUrl = "https://example.com" }),
                logger,
                new HttpClient(handler));

            var dto = new LinePayRequestDTO
            {
                Amount = 100,
                Currency = "TWD",
                OrderId = "order-1",
                Packages = new List<PackageDTO>()
            };

            var response = await service.SendPaymentRequest(dto);

            Assert.Equal("0000", response.ReturnCode);
            Assert.Equal(123, response.Info.TransactionId);
        }

        [Fact]
        public async Task ConfirmPayment_ReturnsParsedResponse()
        {
            var logger = NullLogger<LinePayService>.Instance;
            var handler = new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new LinePayConfirmResponseDTO
                {
                    ReturnCode = "0000",
                    ReturnMessage = "OK",
                    Info = new ConfirmResponseInfoDTO { OrderId = "order-1" }
                }), Encoding.UTF8, "application/json")
            });

            var service = new LinePayService(
                Options.Create(new LinePayModel { LineID = "id", LineKey = "key", LineUrl = "https://example.com" }),
                logger,
                new HttpClient(handler));

            var response = await service.ConfirmPayment("tid", "order-1", new LinePayConfirmDTO { Amount = 100, Currency = "TWD" });

            Assert.Equal("0000", response.ReturnCode);
            Assert.Equal("order-1", response.Info.OrderId);
        }
    }

    internal class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _response;

        public FakeHttpMessageHandler(HttpResponseMessage response)
        {
            _response = response;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_response);
        }
    }
}
