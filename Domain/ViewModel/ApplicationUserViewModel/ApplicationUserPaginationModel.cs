using Domain.ViewModel.BusinessCardViewModel;
using Domain.ViewModel.CommonViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.ApplicationUserViewModel
{
    
    public class GetApplicationUserPaginationModel 
    {
        [JsonProperty("data")]
        public List<ApplicationUserToViewFrontModel> ApplicationUserModel { get; set; }

        [JsonProperty(propertyName: "pagination")]
        public PaginationInfo Pagination { get; set; }

        [JsonProperty("countDeActiveAndDeleted")]
        public CountDeActiveAndDeleted CountDeActiveAndDeleted { get; set; }
    }
}
