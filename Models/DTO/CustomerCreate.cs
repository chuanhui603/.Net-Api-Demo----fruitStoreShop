namespace 水水水果API.Models.DTO
{
    /// <summary>
    /// 用於新增 Customer 的 DTO（Create Only）
    /// </summary>
    public class CustomerCreate
    {
        public int BrandId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDay { get; set; }
        public string Phone { get; set; }
    }
}
