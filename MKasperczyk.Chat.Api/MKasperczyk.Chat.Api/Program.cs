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
using MKasperczyk.Chat.Api.Repositories;
using MKasperczyk.Chat.Api.Services;
using MKasperczyk.Chat.Api.Validators;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

var corsAllowOrgins = "_chatCorsAllowOrgins";
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
services.AddScoped<IAuthService, AuthService>(); 
services.RegisterDatabase(configuration);
services.RegisterRequests();

services.AddSignalR();
services.AddChatAuth(configuration);

var app = builder.Build();

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

app.Run(configuration["RunningAddress"]);
