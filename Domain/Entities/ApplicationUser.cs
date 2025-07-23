using Domain.Base;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ApplicationUser : BaseEntity
    {
        public string? ApplicationUserId { get; set; }
        public string? ApplicationUserUserName { get; set; }
        public string? ApplicationUserEmail { get; set; }
        public string? ApplicationUserPasswordHash { get; set; }
        public UserRole ApplicationUserRole { get; set; } = UserRole.User;
    }
}
