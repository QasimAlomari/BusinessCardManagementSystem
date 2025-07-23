using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.ApplicationUserViewModel
{
    public class ApplicationUserUpdateInfoModel
    {
        public string? ApplicationUserId { get; set; }
        public string? ApplicationUserUserName { get; set; }
        public string? ApplicationUserEmail { get; set; }
    }
}
