namespace 水水水果API.DTO.LinePay
{
    public class LinePayConfirmDTO
    {
        [JsonProperty("amount")]
        public int Amount { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}
