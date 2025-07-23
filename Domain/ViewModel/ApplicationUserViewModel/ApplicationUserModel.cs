using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.ViewModel.ApplicationUserViewModel
{
    public class ApplicationUserModel 
    {
        public string? ApplicationUserId { get; set; }
        public string? ApplicationUserUsername { get; set; }
        public string? ApplicationUserEmail { get; set; }
        public string? ApplicationUserRole { get; set; }
        public string? ApplicationUserToken { get; set; }
    }
}
