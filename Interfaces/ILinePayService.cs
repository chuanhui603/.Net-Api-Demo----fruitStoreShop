using 水水水果API.DTO.LinePay;

namespace 水水水果API.Interfaces
{
    public interface ILinePayService
    {
        Task<LinePayResponseDTO> SendPaymentRequest(LinePayRequestDTO dto);
        Task<LinePayConfirmResponseDTO> ConfirmPayment(string transactionId, string orderId, LinePayConfirmDTO dto);
        // void TransactionCancel(string transactionId);
    }
}