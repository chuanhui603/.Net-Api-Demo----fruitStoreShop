namespace 水水水果API.Models.DTO
{
    /// <summary>
    /// 用於更新 Customer 的 DTO（Update Only）
    /// Id 必填
    /// </summary>
    public class CustomerUpdate
    {
        /// <summary>
        /// 必填：要更新的 Customer ID
        /// </summary>
        public int Id { get; set; }
        public int BrandId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDay { get; set; }
        public string Phone { get; set; }
    }
}
