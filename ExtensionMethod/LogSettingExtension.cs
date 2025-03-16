namespace 水水水果API.ExtensionMethod
{
    internal static class LogSettingExtension
    {
        public static IServiceCollection AddLogServices(this ILoggingBuilder logger)
        {
            /**
             當前設定都會被SeriLog蓋掉
            **/

            return logger.Services;
        }
    }
}