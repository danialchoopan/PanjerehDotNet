using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PanjerehDotNet.Application.Interfaces;
using PanjerehDotNet.Application.Services;
using PanjerehDotNet.Domain.Interfaces;
using PanjerehDotNet.Infrastructure.Persistence;
using PanjerehDotNet.Infrastructure.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

// Database
var dbProvider = builder.Configuration["DatabaseProvider"] ?? "SQLite";
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    if (dbProvider == "SQLite")
        options.UseSqlite(builder.Configuration.GetConnectionString("SQLiteConnection"));
    else
        options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"));
});

// Authentication - Support both JWT (for API/Mobile) and Cookies (for Web UI)
var keyString = builder.Configuration["Jwt:Key"] ?? "SecretKeyForPanjerehDotNetProject2024";
var key = Encoding.ASCII.GetBytes(keyString);

builder.Services.AddAuthentication(options => {
    options.DefaultScheme = "SmartAuth";
    options.DefaultChallengeScheme = "SmartAuth";
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => {
    options.LoginPath = "/Login";
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => {
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
})
.AddPolicyScheme("SmartAuth", "SmartAuth", options => {
    options.ForwardDefaultSelector = context => {
        string auth = context.Request.Headers["Authorization"];
        if (!string.IsNullOrEmpty(auth) && auth.StartsWith("Bearer "))
            return JwtBearerDefaults.AuthenticationScheme;
        return CookieAuthenticationDefaults.AuthenticationScheme;
    };
});

// Dependency Injection
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAdService, AdService>();
builder.Services.AddScoped<IChatService, ChatService>();

var app = builder.Build();

// Seed Database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    if (dbProvider == "SQLite")
    {
        context.Database.EnsureCreated();
    }
    else
    {
        context.Database.Migrate();
    }
    await PanjerehDotNet.Infrastructure.Data.DataSeeder.SeedAsync(context);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();
app.MapHub<PanjerehDotNet.Infrastructure.Hubs.ChatHub>("/chatHub");

app.Run();
