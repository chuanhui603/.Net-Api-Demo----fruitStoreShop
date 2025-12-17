
namespace 水水水果API.ExtensionMethod;
public static class DependencyInjectionExtension
{
    public static IServiceCollection AddFruitStoreServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        // 註冊服務
        services.AddHttpContextAccessor();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IMemberService, MemberService>();
        services.AddScoped<ICouponService, CouponService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ILinePayService, LinePayService>();


        // 註冊儲存庫
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<ICouponRepository, CouponRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IAuthRepository, AuthRepository>();

        // 註冊 Redis 相關服務
        services.AddScoped<IRedisService, RedisService>(); 

        // 註冊輔助類別
        services.AddSingleton<JWTHelper>();
        services.AddSingleton<IMailHelper, MailHelper>();
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(configuration.GetValue<string>("RedisSetting:ConnectionString")));

        //註冊Filter
        services.AddScoped<LogoutActionFilter>();

        return services;

    }
}

