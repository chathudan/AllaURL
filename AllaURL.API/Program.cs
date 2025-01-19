using AllaURL.API.Extensions;
using AllaURL.Data;
using AllaURL.Data.Entities; 
using AllaURL.Domain.Interfaces;
using AllaURL.Domain.JsonConverters;
using AllaURL.Domain.Repositories;
using AllaURL.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()  // Allow all origins (you can restrict this to specific origins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.Converters.Add(new TokenDataConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();

builder.Services.AddCors(o => o.AddPolicy("local", x =>
    x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

// Register the in-memory database
//builder.Services.AddDbContext<AllaUrlDbContext>(options =>
//    options.UseInMemoryDatabase("NFCDatabase"));

// Register the AllaUrlDbContext with PostgreSQL provider
builder.Services.AddDbContext<AllaUrlDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));


// Add Repositories
builder.Services.AddScoped<ITokenRepository, TokenRepository>(); 

// Add TokenService
builder.Services.AddScoped<ITokenService, TokenService>();


var redisConn = builder.Configuration["ConnectionStrings:RedisConnection"];
bool isRedisUse = bool.Parse(builder.Configuration["UseRedis"]);

if (isRedisUse && !string.IsNullOrWhiteSpace(redisConn))
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = redisConn;
        options.InstanceName = "cache-nfc";
    });
}
else
{
    builder.Services.AddDistributedMemoryCache();
}

// Add Swagger with enum as string configuration
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

    // Add extension method to handle enums as strings
    c.AddEnumStringRepresentation();

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{ 
    app.UseCors("local");
    var config = builder.Configuration;
  //  var connectionStringName = config.GetValue<string>("ActiveDbConnection");
  //  var connectionString = config.GetConnectionString(connectionStringName);

    /*switch (connectionStringName.ToLower())
    {
        case "mssql":
            builder.Services.AddDbContext<AllaUrlDbContext>(options =>
                options.UseSqlServer(connectionString));
            break;
        case "mysql":
            builder.Services.AddDbContext<AllaUrlDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            break;
        case "pgsql":
            builder.Services.AddDbContext<AllaUrlDbContext>(options =>
                options.UseNpgsql(connectionString));
            break;
        default:
            builder.Services.AddDbContext<AllaUrlDbContext>(options =>
                options.UseInMemoryDatabase("NFCDatabase"));
            break;
    }*/
    
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

// Register the custom exception handling middleware
app.UseMiddleware<AllaURL.API.Middleware.ExceptionHandlingMiddleware>();

app.UseRouting();
app.MapControllers();
app.UseHttpsRedirection();

app.MapHealthChecks("/status");

// Initialize data when application starts
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AllaUrlDbContext>();
  
    // SeedData(dbContext);  // Seed data
}

app.Run();



void SeedData(AllaUrlDbContext context)
{
    context.TokenEntity.AddRange(new List<TokenEntity>
    {
        new TokenEntity
        {
            Id = 1,
           // Vcard = "https://washia.com.au",
            Identifier = "washia",
            CreatedAt = DateTime.UtcNow,
           // RedirectUrl = "https://washia.com.au" 
        },
        new TokenEntity
        {
            Id = 2,
            //Vcard = "https://wildnestvilla.com",
            Identifier = "wildnestvilla",
            CreatedAt = DateTime.UtcNow,
          //  RedirectUrl = "https://wildnestvilla.com" 
        }
    });

    context.SaveChanges();
}