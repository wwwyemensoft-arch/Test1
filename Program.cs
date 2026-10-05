using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UltimateArchiveAPI.Controllers;
using UltimateArchiveAPI;
using UltimateArchiveAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// No Calims Code
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContextService, UserContextService>();


// Database Connections
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

//builder.Services.AddDbContext<UltimateArchiveLangContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("UltimateArchiveLangConnection")));

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// DI
builder.Services.AddScoped<ConnectionStringService>();

// Controllers & Roles
builder.Services.AddScoped<DboptionController>();
builder.Services.AddScoped<UserController>();
builder.Services.AddScoped<AccessCodeInfoUController>();
builder.Services.AddScoped<ImgAccessCodeInfoUController>();

// Session
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(3000);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});


var app = builder.Build();

// 2. Routing
//app.UseRouting();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseSwaggerUI(c =>
    //{
    //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "External System V1");
    //    c.InjectStylesheet("/swagger/custom-swagger.css");
    //});
}
// Use React CORS policy (supports credentials/JWT)
app.UseCors("Cors");
app.MapControllers();
// 6. Session (Must be before Controllers)
app.UseSession();

app.UseHttpsRedirection();

// 5. Identity (Order is critical: Auth -> Authorization)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
