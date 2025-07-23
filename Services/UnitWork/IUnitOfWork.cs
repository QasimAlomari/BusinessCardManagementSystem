using Domain.Entities;
using Services.Interfaces.BusinessCard;
using Services.Interfaces.DIInjection;
using Services.Interfaces.UserManagement;
using System.Data;


namespace Services.UnitWork
{
    public interface IUnitOfWork : ITransientService
    {
        public IMasterBusinessCard<BusinessCard> BusinessCard { get; }
        public IMasterUser<ApplicationUser> ApplicationUser { get; }
    }
}
