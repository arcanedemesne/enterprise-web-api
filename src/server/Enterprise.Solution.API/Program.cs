using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Mvc.Versioning;
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

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/enterprise.solution.api.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

var host = builder.Host;
var services = builder.Services;
var configuration = builder.Configuration;

host.UseSerilog();

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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(setupAction =>
{
    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var commentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

    setupAction.IncludeXmlComments(commentsFullPath);

    //TODO: determine security type (jwt, bearer, openID, etc)
    setupAction.AddSecurityDefinition("EnterpriseSolutionBearerAuth", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
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
services.AddSingleton<FileExtensionContentTypeProvider>();

//#if DEBUG
//services.AddTransient<IMailService, LocalMailService>();
//#else
//services.AddTransient<IMailService, CloudMailService>();
//#endif

// Add Database context
services.AddDbContext<EnterpriseSolutionDbContext>(
    dbContextOptions =>
    dbContextOptions
        .UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql"), x => x.MigrationsAssembly("Enterprise.Solution.Data"))
        .EnableSensitiveDataLogging()
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

// Register Dependency Injection
services.AddStackExchangeRedisCache(cacheOptions =>
{
    cacheOptions.ConfigurationOptions = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("RedisCache"));
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

// Add AutoMapper
services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Configure Forwarded Headers Middleware to make it work under nginx load balancer
services.Configure<ForwardedHeadersOptions>(options => { options.ForwardedHeaders = ForwardedHeaders.All; });

// Configure HttpContextAccessor
services.AddHttpContextAccessor();

// Configure Cors (This policy is only for local dev)
services.AddCors(options =>
{
    options.AddPolicy("CorsLocalDevPolicy",
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

// Configure API versioning
services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("api-version")
        );
});
// Add Authentication
var key = builder.Configuration["AuthenticationOld:SecretForKey"] ?? string.Empty;
var issuer = builder.Configuration["AuthenticationIld:Issuer"];
var audience = builder.Configuration["AuthenticationOld:Audience"];
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(key))
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
