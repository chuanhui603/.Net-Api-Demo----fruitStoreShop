using StackExchange.Redis;

namespace 水水水果API.ExtensionMethod
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddFruitStoreServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            // 註冊服務
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICouponService, CouponService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRecipientService, RecipientService>();
            services.AddScoped<ILinePayService, LinePayService>();
            services.AddHttpContextAccessor();
            // 註冊儲存庫
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICouponRepository, CouponRepository>();
            services.AddScoped<IRecipientRepository, RecipientRepository>();
            // 註冊輔助類別
            services.AddSingleton<DownloadHelper>();
            services.AddSingleton<ImageHelper>();
            services.AddSingleton<JWTHelper>();
            services.AddSingleton<IMailHelper, MailHelper>();
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(configuration.GetValue<string>("ConnectionStrings:RedisConnectionStrings")));
            //註冊Filter
            services.AddScoped<LogoutActionFilter>();

            return services;

        }
    }
}

