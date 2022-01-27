using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace API.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddAplicationServices (this IServiceCollection services, IConfiguration config)
        {
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
             services.AddScoped<ITokenService, TokenService>();
             services.AddScoped<IPhotoService,PhotoService>();
             services.AddScoped<ILikesRepository, LikesRepository>();
             services.AddScoped<LogUserActivity>();
             services.AddScoped<IUserRepository, UsersRepository>();
             services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            services.AddDbContext<DataContext>(options =>
             {
                 options.UseSqlite(config.GetConnectionString("DefaultConnection"));

             }); // konekcijski string
             return services;

        }
    }
}