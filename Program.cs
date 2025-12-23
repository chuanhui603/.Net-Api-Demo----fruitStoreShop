using Framework.SqlCommon.SQLHelper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
               NameClaimType = JwtRegisteredClaimNames.Sub,
               RoleClaimType = ClaimTypes.Role,
               ValidateIssuer = true,
               ValidIssuer = builder.Configuration.GetValue<string>("JWTSetting:JWTIssuer"),
               ValidateAudience = false,
               //ValidAudience = "JwtAuthDemo", 
               // 一般都會驗證 Token 的有效期間
               ValidateLifetime = true,
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

    string connectStr = builder.Configuration.GetValue<string>("SqlConnection:DefaultConnection");
    builder.Services.AddSingleton(provider =>
    {
        return new SqlCommonContext(connectStr);
    });

    SqlCommonContext sqlContext =  new (connectStr);


    builder.Services.AddDbContext<TWRMS_TESTContext>(
        options => options.UseSqlServer(sqlContext.RMS.ConnectString)
     );

    builder.Services.AddDbContext<TWCRM_TESTContext>(
        options => options.UseSqlServer(sqlContext.CRM.ConnectString)
     );

    builder.Services.AddDbContext<TWERP_TESTContext>(
        options => options.UseSqlServer(sqlContext.ERP.ConnectString)
    );

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
{
        //同時啟動Vue測試
        var frontProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "FruitShopDemo_Fronted"); 
        if (Directory.Exists(frontProjectPath))
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c npm run dev",
                    WorkingDirectory = frontProjectPath,
                    UseShellExecute = true,
                    CreateNoWindow = false
                };
                Process.Start(psi);
            }
            catch
            {
            }
        }
    }

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
    app.UseSerilogRequestLogging(options =>
    {
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
            diagnosticContext.Set("UserID", httpContext.User.Identity?.Name);
        };
    });
    //if (app.Environment.IsDevelopment())
    //{
    //    app.UseSwagger();
    //    app.UseSwaggerUI();
    //}
    // app.UseHttpsRedirection();
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
