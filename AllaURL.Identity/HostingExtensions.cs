//using Serilog;

using AllaURL.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http; 
using Duende.IdentityServer;
using Duende.IdentityServer.EntityFramework;
using Duende.IdentityServer.EntityFramework.Storage;
using System.Reflection;

namespace AllaURL.Identity;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        var migrationsAssembly = typeof(HostingExtensions).GetTypeInfo().Assembly.GetName().Name;

        // uncomment if you want to add a UI
        builder.Services.AddRazorPages();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"), b => b.MigrationsAssembly(migrationsAssembly)));

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddIdentityServer(options =>
            {
                // https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/api_scopes#authorization-based-on-scopes
                options.EmitStaticAudienceClaim = true;
                options.ServerSideSessions.UserDisplayNameClaimType = "name";
            })
             .AddConfigurationStore(options =>
             {
                 options.ConfigureDbContext = db => db.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"), b => b.MigrationsAssembly(migrationsAssembly));

             })
             .AddOperationalStore(options =>
             {
                 options.ConfigureDbContext = db => db.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"), b => b.MigrationsAssembly(migrationsAssembly));
                 options.EnableTokenCleanup = true; // Enable token cleanup (expired tokens)
             })
            .AddAspNetIdentity<ApplicationUser>()
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients)
            
            .AddServerSideSessions()
            .AddTestUsers(TestUsers.Users); // Add test users TODO: Remove this in production


        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        //app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // uncomment if you want to add a UI
        app.UseStaticFiles();
        app.UseRouting();

        app.UseIdentityServer();

        // uncomment if you want to add a UI
        app.UseAuthorization();
        app.MapRazorPages().RequireAuthorization();

        return app;
    }
}
