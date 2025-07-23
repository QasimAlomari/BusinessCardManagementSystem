using Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.ApplicationUserViewModel
{
    public class ApplicationUserToViewFrontModel : BaseEntityActivate
    {
        public string? ApplicationUserId { get; set; }
        public string? ApplicationUserUsername { get; set; }
        public string? ApplicationUserEmail { get; set; }
        public string? ApplicationUserRole { get; set; }
    }
}
