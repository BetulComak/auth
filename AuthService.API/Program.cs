using System;
using AuthService.API.Data;
using AuthService.Business.Contracts;
using AuthService.Business.Services;
using Duende.IdentityServer.Configuration;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Database Bağlantısı
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Identity ve IdentityServer ekleniyor
builder.Services.AddIdentity<IdentityUser, IdentityRole>(o =>
{
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequiredUniqueChars = 0;
    o.Password.RequireUppercase = false;
    o.Password.RequireDigit = false;
    o.Password.RequiredLength = 6;
    o.Password.RequireLowercase = false;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityServer(options =>
{
    options.IssuerUri = "https://myauth-bebhf3f4czfdb9fe.canadacentral-01.azurewebsites.net";
    options.EmitStaticAudienceClaim = true;
})
   .AddAspNetIdentity<IdentityUser>()
   .AddInMemoryIdentityResources(new List<IdentityResource>
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile()
    })
   .AddInMemoryApiScopes(IdentityServerConfig.ApiScopes)
   .AddInMemoryApiResources(IdentityServerConfig.ApiResources)
   .AddInMemoryClients(IdentityServerConfig.Clients)
   .AddDeveloperSigningCredential();


builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITokenCreationService, DefaultTokenCreationService>();
builder.Services.AddTransient<IProfileService, CustomProfileService>();
builder.Services.AddScoped<AuthService.Business.Contracts.ITokenService, AuthService.Business.Services.TokenService>();


builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.DocumentFilter<DuendeTokenEndpointFilter>();
});



var app = builder.Build();

// 📌 Swagger UI ekleyelim
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthService API v1");
        options.RoutePrefix = string.Empty; // Ana sayfa olarak açılması için

    });
}

app.UseIdentityServer();
//app.UseAuthorization();
app.MapControllers();

app.Run();
