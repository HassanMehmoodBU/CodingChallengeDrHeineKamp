using FileServerServiceLogic.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileServerServiceLogic.Extensions.ServiceExt
{
    public static class ServiceCollection
    {
        //public static void ConfigureCors(this IServiceCollection services) =>
        //    services.AddCors(options =>
        //    {
        //        options.AddPolicy("CorsPolicy", builder =>
        //            builder.AllowAnyOrigin()
        //            .AllowAnyMethod()
        //            .AllowAnyHeader());
        //    });

        public static void ConfigureSqlServerDbContext(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<FileServerDataContext>(opts =>
                opts.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));



    }
}
