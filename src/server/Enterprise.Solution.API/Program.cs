using Enterprise.Solution.API.Helpers;
using Enterprise.Solution.Data.DbContexts;
using Enterprise.Solution.Repositories;
using Enterprise.Solution.Repository.Base;
using Enterprise.Solution.Service.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/enterprise.solution.api.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

// Add Mvc options to filter and handle cancellationTokens
builder.Services.AddMvc(options =>
{
    options.Filters.Add<OperationCancelledExceptionFilter>();
});

// Add Controller options
builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
})
.AddNewtonsoftJson()
.AddXmlDataContractSerializerFormatters();

// Add Cors options
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
        .AllowAnyOrigin() //TODO: determine FE uri
        .AllowAnyHeader() //TODO: determine allowed headers
        .AllowAnyMethod() //TODO: determine if a method should be disallowed
        //.WithMethods("GET", "PUT", "DELETE", "POST", "PATCH") //not really necessary when AllowAnyMethods is used.
        );
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setupAction =>
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
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

//#if DEBUG
//builder.Services.AddTransient<IMailService, LocalMailService>();
//#else
//builder.Services.AddTransient<IMailService, CloudMailService>();
//#endif



// Add Database context
builder.Services.AddDbContext<EnterpriseSolutionDbContext>(
    dbContextOptions =>
    dbContextOptions
        .UseNpgsql(builder.Configuration["ConnectionStrings:PostgreSql"], x => x.MigrationsAssembly("Enterprise.Solution.Data"))
        .EnableSensitiveDataLogging()
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

// Register Dependency Injection
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

// Author DI
builder.Services.AddScoped(typeof(IAuthorRepository), typeof(AuthorRepository));
builder.Services.AddScoped<IAuthorService, AuthorService>();

// Item DI
builder.Services.AddScoped(typeof(IItemRepository), typeof(ItemRepository));
builder.Services.AddScoped<IItemService, ItemService>();

// Add AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add Authentication
var key = builder.Configuration["Authentication:SecretForKey"] ?? string.Empty;
var issuer = builder.Configuration["Authentication:Issuer"];
var audience = builder.Configuration["Authentication:Audience"];
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
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MustBeAnAllen", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("family_name", "Allen");
    });
});

// Add Versioning
builder.Services.AddApiVersioning(setupActions =>
{
    setupActions.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    setupActions.AssumeDefaultVersionWhenUnspecified = true;
    setupActions.ReportApiVersions = true;
});

// Build the app
var app = builder.Build();

// Get Database context and ensure that it has been created
var scope = app.Services.CreateScope();
EnterpriseSolutionDbContext dbcontext = scope.ServiceProvider.GetRequiredService<EnterpriseSolutionDbContext>();
dbcontext.Database.EnsureCreated();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseRouting();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
