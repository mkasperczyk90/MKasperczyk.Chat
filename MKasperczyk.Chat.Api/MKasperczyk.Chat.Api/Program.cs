using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MKasperczyk.Chat.Api.DAL;
using MKasperczyk.Chat.Api.Extensions;
using MKasperczyk.Chat.Api.Features.Auth;
using MKasperczyk.Chat.Api.Features.Contacts;
using MKasperczyk.Chat.Api.Features.Messages;
using MKasperczyk.Chat.Api.Hubs;
using MKasperczyk.Chat.Api.Repositories;
using MKasperczyk.Chat.Api.Services;

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
                          policy.WithOrigins(configuration["Cors:Origins"]);
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
app.MapGet("/contacts/{userId}", (IUnitOfWork unitOfWork, int userId) 
    => GetContactsHandler.Handle(unitOfWork, userId)).RequireAuthorization();

app.MapPost("/avatar", (IUnitOfWork unitOfWork, HttpRequest request) 
    => AvatarHandler.Handle(unitOfWork, request)).RequireAuthorization();

app.MapGet("/messages", (IDbContextFactory<ChatContext> dbContextFactory, int senderId, int receiverId) 
    => GetMessagesHandler.Handle(dbContextFactory, senderId, receiverId)).RequireAuthorization();

app.MapPost("/message", (IUnitOfWork unitOfWork, IDbContextFactory<ChatContext> dbContextFactory, IValidator<SendMessageRequest> validator, SendMessageRequest model) 
    => SendMessageHandler.Handle(unitOfWork, dbContextFactory, validator, model)).RequireAuthorization();

app.MapPost("/auth/register", (IUnitOfWork unitOfWork, IValidator<RegisterRequest> validator, IAuthService authService, RegisterRequest registerInfo) 
    => RegisterHandler.Handle(unitOfWork, validator, authService, registerInfo));

app.MapPost("/auth/token", (IDbContextFactory<ChatContext> dbContextFactory, IAuthService authService, IValidator<SecurityTokenRequest> validator, SecurityTokenRequest tokenRequest) 
    => SecurityTokenHandler.Handle(dbContextFactory, authService, validator, tokenRequest)).AllowAnonymous();

app.Run(configuration["RunningAddress"]);
