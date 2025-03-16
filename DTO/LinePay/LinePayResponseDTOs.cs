namespace 水水水果API.DTO.LinePay
{
    public class LinePayResponseDTO
    {
        public string ReturnCode { get; set; }
        public string ReturnMessage { get; set; }
        public ResponseInfoDTO Info { get; set; }
    }

    public class ResponseInfoDTO
    {
        public ResponsePaymentUrlDTO PaymentUrl { get; set; }
        public long TransactionId { get; set; }
        public string PaymentAccessToken { get; set; }
    }

    public class ResponsePaymentUrlDTO
    {
        public string Web { get; set; }
        public string App { get; set; }
    }
}
