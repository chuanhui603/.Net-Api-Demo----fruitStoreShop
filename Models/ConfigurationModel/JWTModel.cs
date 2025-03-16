namespace 水水水果API.Models.ConfigurationModel
{
    public class JWTModel
    {
        public const string JWTSetting = "JWTSetting";
        public string JWTIssuer { get; set; }
        public string JWTSignKey { get; set; }
    }
}
