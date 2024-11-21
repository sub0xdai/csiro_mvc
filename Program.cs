using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using csiro_mvc.Data;
using csiro_mvc.Models;
using csiro_mvc.Repositories;
using csiro_mvc.Services;
using csiro_mvc.Middleware;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Configure Kestrel with URLs
    builder.WebHost.UseUrls("http://localhost:5002");

    // Add Serilog
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
        .MinimumLevel.Override("System", LogEventLevel.Debug)
        .Enrich.FromLogContext()
        .WriteTo.Console(
            outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
        .WriteTo.File("logs/csiro_mvc-.txt", 
            rollingInterval: RollingInterval.Day,
            outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"));

    // Add services to the container.
    builder.Services.AddControllersWithViews();
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    // Add DbContext
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

    // Add Repository Pattern services
    builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IAdminSettingsRepository, AdminSettingsRepository>();

    // Add Application Services
    builder.Services.AddScoped<IApplicationService, ApplicationService>();
    builder.Services.AddScoped<IUniversityService, UniversityService>();
    builder.Services.AddScoped<IResearchProgramService, ResearchProgramService>();
    builder.Services.AddScoped<IApplicationSettingsService, ApplicationSettingsService>();
    builder.Services.AddScoped<IFileUploadService, FileUploadService>();
    builder.Services.AddScoped<IAdminService, AdminService>();
    builder.Services.AddScoped<INotificationService, NotificationService>();

    // Add Custom Cache Service (Singleton for cache consistency)
    builder.Services.AddSingleton<ICacheService, CustomCacheService>();

    // Add Identity
    builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 8;
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedPhoneNumber = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

    // Configure Identity Password Hasher
    builder.Services.Configure<PasswordHasherOptions>(options =>
    {
        options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3;
    });

    // Add Authorization Policies
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("RequireAdminRole", policy => 
            policy.RequireRole("Admin"));
        options.AddPolicy("RequireResearcherRole", policy => 
            policy.RequireRole("Researcher"));
        options.AddPolicy("RequireApplicantRole", policy => 
            policy.RequireRole("Applicant"));
    });

    // Configure Cookie Policy
    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;
    });

    var app = builder.Build();

    // Ensure the database is created and migrations are applied
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
            
            // Initialize database with roles and admin user
            await DbInitializer.Initialize(services, null);
            
            await SeedData.Initialize(services);
            Log.Information("Database seeded successfully");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while initializing the database");
            throw;
        }
    }

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
        app.UseHttpsRedirection();
    }

    // Add Serilog request logging
    app.UseSerilogRequestLogging();

    app.UseStaticFiles();
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();
    app.UseProfileCompletion();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
