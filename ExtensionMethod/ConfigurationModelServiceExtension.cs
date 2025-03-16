namespace 水水水果API.ExtensionMethod
{
    public static class ConfigurationModelServiceExtension
    {
        public static IServiceCollection AddConfigurationModelSetting(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.Configure<AppsetttingModel>(configuration.GetSection(AppsetttingModel.AppSetting));
            services.AddOptions<AppsetttingModel>()
                .Bind(configuration.GetSection(AppsetttingModel.AppSetting))
                .ValidateDataAnnotations()
                .ValidateOnStart();
            services.AddOptions<LinePayModel>()
                .Bind(configuration.GetSection(LinePayModel.LinePaySetting))
                .ValidateDataAnnotations()
                .ValidateOnStart();
            services.AddOptions<JWTModel>()
                .Bind(configuration.GetSection(JWTModel.JWTSetting))
                .ValidateDataAnnotations()
                .ValidateOnStart();
            services.AddOptions<MailModel>()
                .Bind(configuration.GetSection(MailModel.MailSetting))
                .ValidateDataAnnotations()
                .ValidateOnStart();
            services.AddOptions<RedisSettingModel>()
         .Bind(configuration.GetSection(RedisSettingModel.RedisSetting))
         .ValidateDataAnnotations()
         .ValidateOnStart();
            return services;
        }
    }
}
