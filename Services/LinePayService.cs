using System.Security.Cryptography;
using 水水水果API.DTO.LinePay;

namespace 水水水果API.Services
{
    internal class LinePayService : ILinePayService
    {
        private readonly LinePayModel _options;
        private readonly ILogger<LinePayService> _logger;
        private  HttpClient client = new HttpClient();

        public LinePayService(IOptions<LinePayModel> options, ILogger<LinePayService> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public async Task<LinePayResponseDTO> SendPaymentRequest(LinePayRequestDTO dto)
        {
            _logger.LogInformation("Start SendPaymentRequest");
            var json = JsonConvert.SerializeObject(dto, Formatting.Indented);
            _logger.LogInformation("DTO: {json}", json);
       
            // 產生 GUID Nonce
            var nonce = Guid.NewGuid().ToString();
            // 要放入 signature 中的 requestUrl
            var requestUrl = "/v3/payments/request";

            //使用 channelSecretKey & requestUrl & jsonBody & nonce 做簽章
            var signature = HMACSHA256(_options.LineKey, _options.LineKey+ requestUrl + json + nonce);
            _logger.LogInformation("Nonce: {nonce}", nonce);
            _logger.LogInformation("Signature: {signature}", signature);
            var request = new HttpRequestMessage(HttpMethod.Post, _options.LineUrl+ requestUrl)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            // 帶入 Headers
            client.DefaultRequestHeaders.Add("X-LINE-ChannelId", _options.LineID);
            client.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", nonce);
            client.DefaultRequestHeaders.Add("X-LINE-Authorization", signature);
            var response = await client.SendAsync(request);
            _logger.LogInformation("Response: {linePayResponse}", await response.Content.ReadAsStringAsync());
            var linePayResponse = JsonConvert.DeserializeObject<LinePayResponseDTO>(await response.Content.ReadAsStringAsync());
            return linePayResponse;
        }

        // 取得 transactionId 後進行確認交易
        public async Task<LinePayConfirmResponseDTO> ConfirmPayment(string transactionId, string orderId, LinePayConfirmDTO dto) 
        {
            var json = JsonConvert.SerializeObject(dto, Formatting.Indented);
            var nonce = Guid.NewGuid().ToString();
            var requestUrl = string.Format("/v3/payments/{0}/confirm", transactionId);
            var signature = HMACSHA256(_options.LineKey, _options.LineKey + requestUrl + json + nonce);

            var request = new HttpRequestMessage(HttpMethod.Post, String.Format(_options.LineUrl + requestUrl, transactionId))
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            client.DefaultRequestHeaders.Add("X-LINE-ChannelId", _options.LineID);
            client.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", nonce);
            client.DefaultRequestHeaders.Add("X-LINE-Authorization", signature);

            var response = await client.SendAsync(request);
            _logger.LogInformation("Response: {linePayResponse}", await response.Content.ReadAsStringAsync());
            var responseDto = JsonConvert.DeserializeObject<LinePayConfirmResponseDTO>(await response.Content.ReadAsStringAsync());
            return responseDto;
        }
        // public async void TransactionCancel(string transactionId)
        // {
        //     //使用者取消交易則會到這裏。
        //     Console.WriteLine($"訂單 {transactionId} 已取消");
        // }
        private string HMACSHA256(string key, string message)
        {

            UTF8Encoding encoding = new ();

            //取的 key byte 值
            byte[] keyByte = encoding.GetBytes(key);

            // 取得 key 對應的 hmacsha256
            HMACSHA256 hmacsha256 = new (keyByte);

            // 取的 message byte 值
            byte[] messageBytes = encoding.GetBytes(message);

            // 將 message 使用 key 值對應的 hamcsha256 作 hash 簽章
            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);

            return Convert.ToBase64String(hashmessage);
        }
    }
}