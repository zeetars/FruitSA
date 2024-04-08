using FruitSA.API.Auth.Services;
using FruitSA.API.Middleware;
using FruitSA.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FruitSA.API.Mappers;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("FruitSAConnection")
    ));
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 5;
}).AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

builder.Services.AddAuthentication(auth =>
{
auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
            {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUploadRepository, UploadRepository>();
builder.Services.AddScoped<IAuthService, UserService>();

builder.Services.AddAutoMapper(typeof(Mapping));
builder.Services.AddControllers();
builder.Services.AddRazorPages();

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

//Middleware and Auth

app.UseAuthentication();
app.UseAuthorization();

app.Use(next => context => {
    context.Request.EnableBuffering();
    return next(context);
});
app.UseMiddleware<AuditLogMiddleware>();

//app.UseWhen(context => !context.Request.Method.Equals("GET", StringComparison.InvariantCultureIgnoreCase), appBuilder =>
//{
//    appBuilder.UseMiddleware<AuditLogMiddleware>();
//});
//Conditional Middleware only for api
//app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), appBuilder =>
//{
//    appBuilder.UseMiddleware<AuditLogMiddleware>();
//});
app.MapControllers();
app.Run();
