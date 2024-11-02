using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Blazor;
using SyncfusionHelpDesk.Components;
using SyncfusionHelpDesk.Components.Account;
using SyncfusionHelpDesk.Data;
using SyncfusionHelpDesk.Models;

namespace SyncfusionHelpDesk
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<IdentityUserAccessor>();
            builder.Services.AddScoped<IdentityRedirectManager>();
            builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

            // Add Identity with roles support
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

            // Syncfusion Support
            builder.Services.AddSyncfusionBlazor();

            // Get SYNCFUSION_APIKEY from appsettings.json
            var SyncfusionApiKey = builder.Configuration["SYNCFUSION_APIKEY"];

            if (SyncfusionApiKey != "{{ Enter your Syncfusion License from Syncfusion.com }}")
            {
                // Register Syncfusion license
                Syncfusion.Licensing.SyncfusionLicenseProvider
                    .RegisterLicense(SyncfusionApiKey);
            }

            // To access HelpDesk tables
            builder.Services.AddDbContextFactory<SyncfusionHelpDeskContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddScoped<SyncfusionHelpDeskService>();
            builder.Services.AddScoped<EmailSender>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // Authentication and Authorization Middleware
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseAntiforgery(); // Must be placed after UseAuthentication and UseAuthorization

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.MapAdditionalIdentityEndpoints();

            // Ensure the Administrator role is created at startup
            await CreateRoles(app.Services);

            await app.RunAsync();
        }

        // Role creation logic
        private static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Check if the Administrator role exists, if not, create it
            if (!await roleManager.RoleExistsAsync("Administrators"))
            {
                var adminRole = new IdentityRole("Administrators");
                await roleManager.CreateAsync(adminRole);
            }
        }
    }
}
