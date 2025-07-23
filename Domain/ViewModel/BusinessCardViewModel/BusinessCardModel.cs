using Domain.Base;
using Domain.Entities;
using Domain.ViewModel.ApplicationUserViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.BusinessCardViewModel
{
    public class BusinessCardModel : BaseEntityActivate
    {
        public int BusinessCardId { get; set; }
        public string? BusinessCardName { get; set; }
        public string? BusinessCardTitle { get; set; }
        public string? BusinessCardPhone { get; set; }
        public string? BusinessCardEmail { get; set; }
        public string? BusinessCardCompany { get; set; }
        public string? BusinessCardWebsite { get; set; }
        public string? BusinessCardAddress { get; set; }
        public string? BusinessCardNotes { get; set; }
    }
}
