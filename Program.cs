using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using StackExchange.Redis;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

try
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    //builder.Services.AddSwaggerGen();
    //註冊JWT認證
    builder.Services
       .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
       {
           // 當驗證失敗時，回應標頭會包含 WWW-Authenticate 標頭，這裡會顯示失敗的詳細錯誤原因
           options.IncludeErrorDetails = true; // 預設值為 true，有時會特別關閉

           options.TokenValidationParameters = new TokenValidationParameters
           {
               // 透過這項宣告，就可以從 "sub" 取值並設定給 User.Identity.Name

               NameClaimType = JwtRegisteredClaimNames.Sub, // 使用 "sub" 作為 NameClaimType
                                                            // 透過這項宣告，就可以從 "roles" 取值，並可讓 [Authorize] 判斷角色
               RoleClaimType = ClaimTypes.Role, // 使用 "roles" 作為 RoleClaimType

               // 一般都會驗證 Issuer
               ValidateIssuer = true,
               ValidIssuer = builder.Configuration.GetValue<string>("JWTSetting:JWTIssuer"),

               // 通常不太需要驗證 Audience
               ValidateAudience = false,
               //ValidAudience = "JwtAuthDemo", // 不驗證就不需要填寫

               // 一般都會驗證 Token 的有效期間
               ValidateLifetime = true,

               // 如果 Token 中包含 key 才需要驗證，一般都只有簽章
               ValidateIssuerSigningKey = false,

               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JWTSetting:JWTSignKey")))
           };
       });

    builder.Services.AddAuthorization();
    // 添加 CORS 設定
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins",
            builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
    });
    //加入依賴注入服務
    builder.Services.AddFruitStoreServices(builder.Configuration);
    //註冊設定檔
    builder.Services.AddConfigurationModelSetting(builder.Configuration);
    //註冊Logger
    builder.Services.AddSerilog();

    //註冊資料庫
    builder.Services.AddScoped<IDbConnection>(sp =>
        new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

    var app = builder.Build();
    app.UseSerilogRequestLogging(options =>
    {
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
            diagnosticContext.Set("UserID", httpContext.User.Identity?.Name);
        };
    });


    // 使用 CORS 設定
    app.UseCors("AllowAllOrigins");
    //if (app.Environment.IsDevelopment())
    //{
    //    app.UseSwagger();
    //    app.UseSwaggerUI();
    //}
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}
