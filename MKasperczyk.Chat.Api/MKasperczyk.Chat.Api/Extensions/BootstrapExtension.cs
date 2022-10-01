
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MKasperczyk.Chat.Api.DAL;
using MKasperczyk.Chat.Api.Features.Auth;
using MKasperczyk.Chat.Api.Features.Messages;
using MKasperczyk.Chat.Api.Repositories;
using MKasperczyk.Chat.Api.Validators;
using System.Text;

namespace MKasperczyk.Chat.Api.Extensions
{
    public static class BootstrapExtension
    {
        public static IServiceCollection RegisterDatabase(this IServiceCollection services, ConfigurationManager? configuration)
        {
            services.AddDbContextFactory<ChatContext>(options => options.UseNpgsql(configuration.GetConnectionString("ChatDatabase")));
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            return services;
        }

        public static IServiceCollection RegisterRequests(this IServiceCollection services)
        {
            services.AddScoped<IValidator<SecurityTokenRequest>, TokenRequestValidator>();
            services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();
            services.AddScoped<IValidator<SendMessageRequest>, SendMessageRequestValidator>();

            return services;
        }

        public static IServiceCollection AddChatAuth(this IServiceCollection services, ConfigurationManager? configuration)
        {
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
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
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
            return services;
        }
    }
}
