using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using Serilog;
using StackExchange.Redis;
using System.Reflection;
using System.Text;

using Enterprise.Solution.Data.DbContexts;
using Enterprise.Solution.Repositories;
using Enterprise.Solution.Repository.Base;
using Enterprise.Solution.Service.Services;
using Enterprise.Solution.Service.Services.Cache;
using Enterprise.Solution.Shared;
using Enterprise.Solution.Email.Service;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/enterprise.solution.api.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();


var builder = WebApplication.CreateBuilder(args);

var host = builder.Host;
var services = builder.Services;

IConfiguration configuration = new ConfigurationBuilder()
    .AddConfiguration(builder.Configuration)
    .AddEnvironmentVariables("SolutionApi_")
    .Build();

host.UseSerilog();

// Register custom settings sections
var solutionSettingsConfigSection = configuration.GetSection(nameof(SolutionSettings));
services.Configure<SolutionSettings>(solutionSettingsConfigSection);
var currentSubPlatformSettings = new SolutionSettings();
solutionSettingsConfigSection.Bind(currentSubPlatformSettings);

// Add Controller options
services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
})
.AddNewtonsoftJson(x =>
{
    x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
})
.AddXmlDataContractSerializerFormatters();

services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddSwaggerGen(setupAction =>
{
    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var commentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

    setupAction.IncludeXmlComments(commentsFullPath);

    setupAction.AddSecurityDefinition("EnterpriseSolutionBearerAuth", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Input a valid token to access this API"
    });

    setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "EnterpriseSolutionBearerAuth"
                }
            }, new List<string>()
        }
    });
});

// Add Authentication
services
.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//.AddKeycloak(options =>
//{
//    //Use default signin scheme
//    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    //Keycloak server
//    options.Realm = currentSubPlatformSettings.Authentication.Schemes.Keycloak.ServerRealm;
//    //Keycloak client ID
//    options.ClientId = currentSubPlatformSettings.Authentication.Schemes.Keycloak.ClientId;
//    //Keycloak client secret
//    options.ClientSecret = currentSubPlatformSettings.Authentication.Schemes.Keycloak.ClientSecret;
//    //Keycloak .wellknown config origin to fetch config
//    options.UserInformationEndpoint = currentSubPlatformSettings.Authentication.Schemes.Keycloak.Metadata;
//    //Require keycloak to use SSL
//    options.Scope.Add("openid");
//    options.Scope.Add("profile");
//    //Save the token
//    options.SaveTokens = true;
//    //Token response type, will sometimes need to be changed to IdToken, depending on config.
//    options.AccessType = AspNet.Security.OAuth.Keycloak.KeycloakAuthenticationAccessType.Confidential;
//    //SameSite is needed for Chrome/Firefox, as they will give http error 500 back, if not set to unspecified.
//    options.CorrelationCookie.SameSite = SameSiteMode.Unspecified;

//    options.Version = new Version(20, 0);
//})
.AddJwtBearer(options =>
{
    // options.MetadataAddress = currentSubPlatformSettings.Authentication.Schemes.Keycloak.Metadata;
    // options.Authority = currentSubPlatformSettings.Authentication.Schemes.Keycloak.ServerRealm;
    // options.Audience = currentSubPlatformSettings.Authentication.Schemes.Keycloak.Audience;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = currentSubPlatformSettings.Authentication.Schemes.Swagger.ClaimsIssuer,
        ValidAudience = currentSubPlatformSettings.Authentication.Schemes.Swagger.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            currentSubPlatformSettings.Authentication.Schemes.Swagger.SecretForKey)),
    };
});

// Add Authorization
// TODO: This requirement is only for initial development purposes
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MustBeAnAllen", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("family_name", "Allen");
    });
});

services.AddSingleton<FileExtensionContentTypeProvider>();

// Add Database context
services.AddDbContext<EnterpriseSolutionDbContext>(
    dbContextOptions =>
    dbContextOptions
        .UseNpgsql(currentSubPlatformSettings.Database.GetConnectionString(), x => x.MigrationsAssembly("Enterprise.Solution.Data"))
        .EnableSensitiveDataLogging()
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

// Register Dependency Injection
services.AddStackExchangeRedisCache(cacheOptions =>
{
    cacheOptions.ConfigurationOptions = ConfigurationOptions.Parse(currentSubPlatformSettings.Cache.GetConnectionString());
    cacheOptions.ConfigurationOptions.AllowAdmin = true;
});

// Cache DI
services.AddScoped(typeof(ICacheService), typeof(CacheService));

// BaseRepository DI
services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

// Author DI
services.AddScoped(typeof(IAuthorRepository), typeof(AuthorRepository));
services.AddScoped<IAuthorService, AuthorService>();

// Book DI
services.AddScoped(typeof(IBookRepository), typeof(BookRepository));
services.AddScoped<IBookService, BookService>();

// Artist DI
services.AddScoped(typeof(IArtistRepository), typeof(ArtistRepository));
services.AddScoped<IArtistService, ArtistService>();

// EmailSubscription DI
services.AddScoped(typeof(IEmailSubscriptionRepository), typeof(EmailSubscriptionRepository));
services.AddScoped<IEmailSubscriptionService, EmailSubscriptionService>();

// Add AutoMapper
services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


//#if DEBUG
//services.AddTransient<ISomeService, SomeServiceImpl>();
//#else
//services.AddTransient<ISomeService, AnotherSomeServiceImpl>();
//#endif

// Add EmailService
services.AddTransient(typeof(IEmailService), typeof(MailKitEmailService));

// Configure Forwarded Headers Middleware to make it work under nginx load balancer
services.Configure<ForwardedHeadersOptions>(options => { options.ForwardedHeaders = ForwardedHeaders.All; });

// Configure HttpContextAccessor
services.AddHttpContextAccessor();

// Configure Cors (This policy is only for local dev)
services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.SetIsOriginAllowed(hostName => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

services.AddHttpClient();

// Configure mvc routes
services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.UseApiBehavior = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("apiVersion"),
        new HeaderApiVersionReader("apiVersion")
        );
});

// Build the app
var app = builder.Build();

app.UseForwardedHeaders();

// Get Database context and ensure that it has been created
var scope = app.Services.CreateScope();
EnterpriseSolutionDbContext dbcontext = scope.ServiceProvider.GetRequiredService<EnterpriseSolutionDbContext>();
dbcontext.Database.EnsureCreated();
scope.Dispose();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
