using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.ApplicationUserViewModel
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;

    public class ApplicationUserModelRegister
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserRole UserRole { get; set; } = UserRole.User;
    }
}
