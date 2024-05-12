using EducationPortalApp.Business.CustomDescriber;
using EducationPortalApp.Business.Services.Concrete;
using EducationPortalApp.Business.Services.Interfaces;
using EducationPortalApp.Business.ValidationRules.FluentValidation.AppUserValidations;
using EducationPortalApp.DataAccess.Contexts.EntityFramework;
using EducationPortalApp.DataAccess.UnitOfWork;
using EducationPortalApp.Dtos.AppUserDtos;
using EducationPortalApp.Entities.UserEntities;
using EducationPortalApp.Shared.Services;
using EducationPortalApp.Shared.Utilities.Security;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace EducationPortalApp.Business.DependencyResolvers.Microsoft
{
    public static class DependencyExtension
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            //Identity
            services.AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.Password.RequireDigit = true;
                opt.Password.RequiredLength = 1;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Lockout.MaxFailedAccessAttempts = 5;
            }).AddErrorDescriber<CustomErrorDescriber>().AddEntityFrameworkStores<AppDbContext>();

            //Context
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("SqlServerConnection"));
            });

            //Scopes,Singletons,Transients
            services.AddScoped<IUow, Uow>();
            services.AddScoped<ISharedIdentityService, SharedIdentityService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAppUserService, AppUserService>();

            //FluentValidations
            services.AddScoped<IValidator<AppUserLoginDto>, AppUserLoginDtoValidator>();
            services.AddScoped<IValidator<AppUserRegisterDto>, AppUserRegisterDtoValidator>();
            services.AddScoped<IValidator<AppUserChangePasswordDto>, AppUserChangePasswordDtoValidator>();
            services.AddScoped<IValidator<RoleAssingSendDto>, RoleAssingSendDtoValidator>();

            //AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            //JWT register (Auto şema)
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.RequireHttpsMetadata = false;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = JwtTokenDefaults.ValidIssuer,
                    ValidAudience = JwtTokenDefaults.ValidAudience,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtTokenDefaults.Key)),
                };
            });
        }
    }
}
