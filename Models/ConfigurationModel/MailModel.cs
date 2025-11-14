namespace 水水水果API.Models.ConfigurationModel
{
    public class MailModel
    {
        public const string MailSetting = "MailSetting";
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
    }
}

