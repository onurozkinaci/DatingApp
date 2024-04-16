using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
   public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
   {
        //**=>Servicelerimizin bir kismini Program.cs'te tanimlamak yerine burada tanimladik ve bu extension metodunu Program.cs'te cagirmamiz yeterli olacagi gibi clean code yaklasimina uygun da hareket ediyoruz;
        //=>adding the dbcontext as a service by defining the configuration for connection string;
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlite(config.GetConnectionString("DefaultConnection")); 
        });

        services.AddCors(); //=>to provide CORS support btw client and BE apps (two different hosts). - Part 1

        //=>adding our own service in order to provide a JWT(Json Web Token) for the logged in user;
        services.AddScoped<ITokenService, TokenService>();
        //**own - burada servis olarak tanimlamasini yapmanla injectable olup controllerda inject edilmesi saglaniyor;
        services.AddScoped<IUserRepository,UserRepository>(); //=>In order to use "Repository Patterns".
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); //=>In order to use "AutoMapper", as injectable, etc.
        services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings")); //to use the defined Cloudinary configurations
        //=>Adding Cloudinary photoservices to be able to inject it into other classes()at their constructors);
        services.AddScoped<IPhotoService, PhotoService>();

        return services;
   }
}
