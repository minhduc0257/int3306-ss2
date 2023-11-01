using System.Security.Cryptography;
using System.Text;
using dotenv.net;
using int3306.Api;
using int3306.Api.Structures;
using int3306.Repository.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

DotEnv.Load();

var jwtOptions = new JwtOptions
{
    Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "audience",
    Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "issuer",
    SecurityKey = SHA1.HashData(
        Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY") ?? "key")
    )
};
var builder = WebApplication.CreateBuilder(args);

var tokenValidationParameters = new TokenValidationParameters
{
    ValidIssuer = jwtOptions.Issuer
};

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(jwtOptions);
builder.Services.AddRepository(opt =>
{
    var s = Environment.GetEnvironmentVariable("MARIADB_CONNECTION_STRING");
    opt.UseMySql(s, ServerVersion.AutoDetect(s));
});
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.Audience = jwtOptions.Audience;
    options.ClaimsIssuer = jwtOptions.Issuer;
    options.TokenValidationParameters = tokenValidationParameters;
});

var app = builder.Build();

app.UseAuthentication();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthorization();

app.MapControllers();

app.Run();