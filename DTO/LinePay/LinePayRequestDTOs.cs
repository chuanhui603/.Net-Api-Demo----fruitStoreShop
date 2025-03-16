namespace 水水水果API.DTO
{
#nullable enable
    public class LinePayRequestDTO
    {
        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("currency")]
        public string? Currency { get; set; }

        [JsonProperty("orderId")]
        public string? OrderId { get; set; }

        [JsonProperty("packages")]
        public List<PackageDTO>? Packages { get; set; }

        [JsonProperty("redirectUrls")]
        public RedirectUrlsDTO? RedirectUrls { get; set; }

        [JsonProperty("options")]
        public RequestOptionDTO? Options { get; set; }
    }

    public class PackageDTO
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("products")]
        public List<LinePayProductDTO>? Products { get; set; }

        [JsonProperty("userFee")]
        public int? UserFee { get; set; }
    }

    public class LinePayProductDTO
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("imageUrl")]
        public string? ImageUrl { get; set; }

        [JsonProperty("originalPrice")]
        public int? OriginalPrice { get; set; }
    }

    public class RedirectUrlsDTO
    {
        [JsonProperty("confirmUrl")]
        public string? ConfirmUrl { get; set; }

        [JsonProperty("cancelUrl")]
        public string? CancelUrl { get; set; }

        [JsonProperty("appPackageName")]
        public string? AppPackageName { get; set; }

        [JsonProperty("confirmUrlType")]
        public string? ConfirmUrlType { get; set; }
    }

    public class RequestOptionDTO
    {
        [JsonProperty("payment")]
        public PaymentOptionDTO? Payment { get; set; }

        [JsonProperty("displpy")]
        public DisplpyOptionDTO? Displpy { get; set; }

        [JsonProperty("shipping")]
        public ShippingOptionDTO? Shipping { get; set; }

        [JsonProperty("extra")]
        public ExtraOptionsDTO? Extra { get; set; }
    }

    public class PaymentOptionDTO
    {
        [JsonProperty("capture")]
        public bool? Capture { get; set; }

        [JsonProperty("payType")]
        public string? PayType { get; set; }
    }

    public class DisplpyOptionDTO
    {
        [JsonProperty("local")]
        public string? Local { get; set; }

        [JsonProperty("checkConfirmUrlBrowser")]
        public bool? CheckConfirmUrlBrowser { get; set; }
    }

    public class ShippingOptionDTO
    {
        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("feeAmount")]
        public int FeeAmount { get; set; }

        [JsonProperty("feeInquiryUrl")]
        public string? FeeInquiryUrl { get; set; }

        [JsonProperty("feeInquiryType")]
        public string? FeeInquiryType { get; set; }

        [JsonProperty("address")]
        public ShippingAddressDTO? Address { get; set; }
    }

    public class ShippingAddressDTO
    {
        [JsonProperty("country")]
        public string? Country { get; set; }

        [JsonProperty("postalCode")]
        public string? PostalCode { get; set; }

        [JsonProperty("state")]
        public string? State { get; set; }

        [JsonProperty("city")]
        public string? City { get; set; }

        [JsonProperty("detail")]
        public string? Detail { get; set; }

        [JsonProperty("optional")]
        public string? Optional { get; set; }

        [JsonProperty("recipient")]
        public ShippingAddressRecipientDTO? Recipient { get; set; }
    }

    public class ShippingAddressRecipientDTO
    {
        [JsonProperty("firstName")]
        public string? FirstName { get; set; }

        [JsonProperty("lastName")]
        public string? LastName { get; set; }

        [JsonProperty("firstNameOptional")]
        public string? FirstNameOptional { get; set; }

        [JsonProperty("lastNameOptional")]
        public string? LastNameOptional { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("phoneNo")]
        public string? PhoneNo { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }
    }

    public class ExtraOptionsDTO
    {
        [JsonProperty("branchName")]
        public string? BranchName { get; set; }

        [JsonProperty("branchId")]
        public string? BranchId { get; set; }
    }
#nullable disable
}


