using Domain.Entities;
using Services.Interfaces.BusinessCard;
using Services.Interfaces.UserManagement;
using Services.UnitWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IMasterBusinessCard<BusinessCard> BusinessCard { get; }
        public IMasterUser<ApplicationUser> ApplicationUser { get; }
        public UnitOfWork
            (
                IMasterBusinessCard<BusinessCard> _BusinessCard,
               IMasterUser<ApplicationUser> _ApplicationUser
            )
        {
            BusinessCard = _BusinessCard;
            ApplicationUser = _ApplicationUser;
        }
    }
}
