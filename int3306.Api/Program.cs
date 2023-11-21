using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using dotenv.net;
using int3306.Api;
using int3306.Api.Structures;
using int3306.Repository.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using AuthorizationMiddleware = int3306.Api.Middlewares.AuthorizationMiddleware;

DotEnv.Load();

if (int.TryParse(Environment.GetEnvironmentVariable("PORT") ?? "5000", out var port))
{
    port = 5000;
}
var jwtOptions = new JwtOptions
{
    Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "audience",
    Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "issuer",
    SecurityKey = new SymmetricSecurityKey(
        SHA1.HashData(
            Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY") ?? "key")        
        )
    )
};
var tokenValidationParameters = new TokenValidationParameters
{
    ValidIssuer = jwtOptions.Issuer,
    IssuerSigningKey = jwtOptions.SecurityKey
};

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(cors =>
{
    cors.AddDefaultPolicy(
        policyBuilder => policyBuilder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed(_ => true)
    );
});

builder.Services.AddAuthorization(o =>
{
    var defaultAuthorizationPolicyBuilder =
        new AuthorizationPolicyBuilder(
                JwtBearerDefaults.AuthenticationScheme
            )
            .RequireAuthenticatedUser();
    o.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
});
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter \"Bearer\" followed by the token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Name = "Bearer",
                In = ParameterLocation.Header,
                Reference = new OpenApiReference { Id = "Bearer", Type = ReferenceType.SecurityScheme }
            },
            new List<string>()
        }
    });

    options.EnableAnnotations();
    options.CustomSchemaIds(type => type.ToString());
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
    options.DocumentFilter<SwaggerPrefixDocumentFilter>("api");
}).AddSwaggerGenNewtonsoftSupport();
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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Map("/api", app =>
{
    app.UseAuthentication();
    app.UseRouting();
    app.UseCors();
    app.UseAuthorization();
    app.UseMiddleware<AuthorizationMiddleware>();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action=Index}/{id?}");
    });
});

app.Run($"http://0.0.0.0:{port}");