using user_ms.Src.Data;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using user_ms.Src.Repositories;
using user_ms.Src.Repositories.Interfaces;
using user_ms.Src.Services.Interfaces;
using user_ms.Src.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using user_ms.Src.Exceptions;

namespace user_ms.Src.Extensions
{
    public static class AppServiceExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            InitEnvironmentVariables();
            AddAutoMapper(services);
            AddServices(services);
            AddSwaggerGen(services);
            AddDbContext(services, config);
            AddUnitOfWork(services);
            AddAuthentication(services, config);
        }

        private static void InitEnvironmentVariables()
        {
            Env.Load();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IMapperService, MapperService>();
            services.AddScoped<IAuthService, AuthService>();
        }

        private static void AddSwaggerGen(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "User Microservice API", Version = "v1" })
            );

        }

        private static void AddDbContext(IServiceCollection services, IConfiguration config)
        {
            var ConnectionString = config.GetConnectionString("Postgres");

            services.AddDbContext<DataContext>(opt => {
                opt.UseNpgsql(ConnectionString);
            });
        }

        private static void AddUnitOfWork(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        private static void AddAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
        }

        private static IServiceCollection AddAuthentication(IServiceCollection services, IConfiguration config)
        {
            var jwtSecret = Env.GetString("JWT_SECRET") ??
                throw new InvalidJwtException("JWT_SECRET not present in .ENV");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            return services;
        }

    }
}