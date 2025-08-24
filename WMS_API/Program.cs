using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WEB_API.Models;
using WEB_API.Services;
using WMS_API.Data;
using DotNetEnv;

Env.Load();

// Read DB info from environment variables
var server = Environment.GetEnvironmentVariable("Db__Server") ?? "localhost";
var user = Environment.GetEnvironmentVariable("Db__User") ?? "sa";
var password = Environment.GetEnvironmentVariable("Db__Password") ?? "sa";
var database = Environment.GetEnvironmentVariable("Db__Database") ?? "test";

// Build connection string
var connectionString = $"Server={server};Database={database};User Id={user};Password={password};Encrypt=False;TrustServerCertificate=True;";
var connectionStringEnv = $"Server=localhost\\SQLEXPRESS;Database={database};Trusted_Connection=True;TrustServerCertificate=True; MultipleActiveResultSets=true;";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(connectionStringEnv));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

/*
This enables authentication services in your app using JWT Bearer tokens.
The scheme is set to JWT Bearer, meaning the app expects incoming tokens in the Authorization header with the prefix Bearer.
 */
var jwtKey = Environment.GetEnvironmentVariable("Jwt__Key");
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            
            ValidateIssuer = true, //Checks if the token’s issuer (who issued it) matches the expected issuer (ValidIssuer).
            ValidateAudience = true, //Checks if the token’s audience (who the token is meant for) matches the expected audience (ValidAudience)
            ValidateLifetime = true, //Checks if the token has expired or is still valid based on the token’s expiration (exp claim)
            ValidateIssuerSigningKey = true, //Ensures the token’s signature is valid — that it was signed using your secret key (IssuerSigningKey).
            ValidIssuer = jwtSettings["Issuer"], //The expected value of the issuer claim in the token.
            ValidAudience = jwtSettings["Audience"], //The expected value of the audience claim in the token.
            /*
             This creates the key that will be used to verify the token’s signature.
             -- The key is read as a UTF8 string from your configuration (jwtSettings["Key"]), then converted into a symmetric security key object.
             */
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

/*
This means all endpoints require authentication by default unless marked [AllowAnonymous].
If there is no global default, then only controllers/actions marked [Authorize] require tokens.
 */
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddScoped<IPasswordHasher<Accounts>, PasswordHasher<Accounts>>();
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<AuthServices>();
builder.Services.AddScoped<EmailServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
