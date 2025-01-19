using Microsoft.EntityFrameworkCore;
using AllaURL.Data;
using Microsoft.Extensions.Configuration;  // Namespace where your ApplicationDbContext is located

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())  // Ensures the config is loaded from the correct directory
    .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)  // Loads the development settings
    .AddEnvironmentVariables();

var configuration = builder.Build();

// Get the connection string from appsettings.Development.json
var connectionString = configuration.GetConnectionString("PostgresConnection");

Console.WriteLine($"Connection string: {connectionString}");

// Set up the DbContext with the connection string
var optionsBuilder = new DbContextOptionsBuilder<AllaUrlDbContext>();
optionsBuilder.UseNpgsql(connectionString);

using var context = new AllaUrlDbContext(optionsBuilder.Options);

// Apply migrations
context.Database.Migrate();

Console.WriteLine("Database migration completed.");