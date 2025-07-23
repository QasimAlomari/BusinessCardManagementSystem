using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository.Constants;
using Repository.Repositories;
using Repository.Respositroies;
using Services.Interfaces.BusinessCard;
using Services.Interfaces.DIInjection;
using Services.Interfaces.UserManagement;
using Services.Services.BusinessCardServices;
using Services.Services.UserManagementServices;
using Services.UnitWork;


namespace Services
{
    public static class ServiceRegistration
    {
        public static void AddInfraStructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Scan(scan => scan
                .FromAssemblyOf<IMasterBusinessCard<BusinessCard>>()
                    .AddClasses(classes => classes.AssignableTo<IScopedService>())
                        .AsImplementedInterfaces()
                        .WithScopedLifetime()
                    .AddClasses(classes => classes.AssignableTo<ITransientService>())
                        .AsImplementedInterfaces()
                        .WithTransientLifetime()
                    //.AddClasses(classes => classes.AssignableTo<ISingletonService>())
                    //    .AsImplementedInterfaces()
                    //    .WithSingletonLifetime()
            );
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            CommonConstants.ConnectionString = configuration.GetConnectionString("SqlConn");
        }
    }
}
