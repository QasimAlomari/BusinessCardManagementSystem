using Domain.ViewModel.ApplicationUserViewModel;
using Domain.ViewModel.CommonViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.BusinessCardViewModel
{
    
    public class GetBusinessCardWithListPagination
    {
        public List<BusinessCardModel> BusinessCardModel { get; set; }
        public PaginationInfo Pagination { get; set; }
    }
}
