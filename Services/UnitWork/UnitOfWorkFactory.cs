using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Services.Interfaces.BusinessCard;
using Services.Interfaces.DIInjection;
using Services.Interfaces.UserManagement;
using Services.UnitWork;


namespace Services.UnitOfWork
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory , IScopedService
    {
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWorkFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IUnitOfWork Create()
        {
            var businessCard = _serviceProvider.GetRequiredService<IMasterBusinessCard<BusinessCard>>();
            var applicationUser = _serviceProvider.GetRequiredService<IMasterUser<ApplicationUser>>();

            return new UnitOfWork(businessCard, applicationUser);
        }
    }

}
