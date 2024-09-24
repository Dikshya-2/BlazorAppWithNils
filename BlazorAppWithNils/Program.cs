using BlazorAppWithNils.Components;
using BlazorAppWithNils.Components.Account;
using BlazorAppWithNils.Data;
using BlazorAppWithNils.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;

namespace BlazorAppWithNils
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<IdentityUserAccessor>();
            builder.Services.AddScoped<IdentityRedirectManager>();
            builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddIdentityCookies();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString));

            //For todoList

            var TodoConnection = builder.Configuration.GetConnectionString("TodoConnection") ?? throw new InvalidOperationException("Connection string 'TodoConnection' not found.");
            builder.Services.AddDbContext<TodoListContext>(options =>
                options.UseSqlite(TodoConnection));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
                builder.Services.AddAuthorization(options =>
                {
                    options.AddPolicy("AuthenticatedUser", policy =>
                    {
                        policy.RequireAuthenticatedUser();
                    });
                    options.AddPolicy("RequireAdministratorRole", policy =>
                    {
                        policy.RequireRole("Admin");
                    });
                });
            builder.WebHost.UseKestrel((contex, serverOptions) =>
            {
                serverOptions.Configure(contex.Configuration.GetSection("kestrePassword"))
                .Endpoint("HTTPS", listenOptions => { listenOptions.HttpsOptions.SslProtocols = SslProtocols.Tls12; });
            });
            string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            userFolder = Path.Combine(userFolder, "aspnet");
            userFolder = Path.Combine(userFolder, "https");
            userFolder = Path.Combine(userFolder, "Dikshya.pfx");
            userFolder = builder.Configuration.GetSection("Kestrel:EndPoints:Https:Certificate:Path").Value= userFolder;

            string kestrelPassword = builder.Configuration.GetValue<string>("DikshyaPassword");
            builder.Configuration.GetSection("Kestrel:Endpoints:Https:Certificate:Password").Value = kestrelPassword;

            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;
            });

            var app = builder.Build();
         




            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            // Add additional endpoints required by the Identity /Account Razor components.
            app.MapAdditionalIdentityEndpoints();

            app.Run();
        }
    }
}
