using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MKasperczyk.Chat.Api.DAL;
using MKasperczyk.Chat.Api.Extensions;
using MKasperczyk.Chat.Api.Features.Auth;
using MKasperczyk.Chat.Api.Features.Contacts;
using MKasperczyk.Chat.Api.Features.Messages;
using MKasperczyk.Chat.Api.Hubs;
using MKasperczyk.Chat.Api.Models;
using MKasperczyk.Chat.Api.Repositories;
using MKasperczyk.Chat.Api.Services;
using MKasperczyk.Chat.Api.Validators;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;

var corsAllowOrgins = "_chatCorsAllowOrgins";
var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddCors(options =>
{
    options.AddPolicy(name: corsAllowOrgins,
                      policy =>
                      {
                          // TODO: get correct origin from appsettings
                          //policy.AllowAnyOrigin();
                          policy.AllowAnyHeader();
                          policy.AllowAnyMethod();
                          policy.WithOrigins(builder.Configuration["Cors:Origins"]);
                          policy.AllowCredentials();
                      });
});

// SERVICE
services.AddSignalR();
services.AddTransient<IUserRepository, UserRepository>();
services.AddTransient<IUnitOfWork, UnitOfWork>();
services.AddScoped<IValidator<TokenRequest>, TokenRequestValidator>();
services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();
services.AddScoped<IValidator<SendMessageRequest>, SendMessageRequestValidator>(); 
services.AddScoped<IAuthService, AuthService>(); 
services.AddDbContextFactory<ChatContext>(options => options.UseNpgsql(configuration.GetConnectionString("ChatDatabase")));

//https://www.youtube.com/watch?v=oti4dU8Pv14
// AUTH
services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
    options.Events = new JwtBearerEvents
    {
        // https://learn.microsoft.com/en-us/aspnet/core/signalr/authn-and-authz?view=aspnetcore-6.0
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                (path.StartsWithSegments("/hubs/chat")))
            {
                // Read the token out of the query string
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});
services.AddAuthorization();

var app = builder.Build();
//app.MapSockets()


app.UseCors(corsAllowOrgins);
if (app.Environment.IsDevelopment())
{
    // DB Initializer
    var context = builder.Services.BuildServiceProvider()
        .GetRequiredService<ChatContext>();
    await ChatInitializer.Seed(context);
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/hubs/chat").RequireAuthorization();

app.MapGet("/", () => "Hello World!").AllowAnonymous();
app.MapGet("/contacts/{userId}", GetContactsHandler.Handle).RequireAuthorization();
app.MapPost("/avatar", AvatarHandler.Handle).RequireAuthorization();
app.MapGet("/messages", GetMessagesHandler.Handle).RequireAuthorization();
app.MapPost("/message", SendMessageHandler.Handle).RequireAuthorization();
app.MapPost("/auth/register", RegisterHandler.Handle);
app.MapPost("/auth/token", MKasperczyk.Chat.Api.Features.Auth.SecurityTokenHandler.Handle).AllowAnonymous();

app.Run();
