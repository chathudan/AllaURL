using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllaURL.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AllaUrlDbContext>
    {
        public AllaUrlDbContext CreateDbContext(string[] args)
        {
            // Build configuration to read from appsettings.json (or other sources)
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())  // Path to the project
                .AddJsonFile("appsettings.Development.json", optional: false) // Specify settings file
                .Build();

            // Get connection string from configuration
            var connectionString = configuration.GetConnectionString("PostgresConnection");

            Console.WriteLine($"Connection string: {connectionString}");

            // Create DbContextOptions
            var optionsBuilder = new DbContextOptionsBuilder<AllaUrlDbContext>();
            optionsBuilder.UseNpgsql(connectionString);  // Use PostgreSQL provider

            // Return the DbContext with the options
            return new AllaUrlDbContext(optionsBuilder.Options);
        }
    }
}
