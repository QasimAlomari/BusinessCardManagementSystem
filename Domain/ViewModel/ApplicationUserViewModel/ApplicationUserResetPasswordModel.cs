using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.ApplicationUserViewModel
{
    public class ApplicationUserResetPasswordModel
    {
        public string? ApplicationUserId { get; set; }
        public string? OldPasswordHash { get; set; }
        public string? NewPasswordHash { get; set; }
        public string? ConfirmNewPasswordHash { get; set; }
    }
}
