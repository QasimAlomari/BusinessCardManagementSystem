using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.BusinessCardViewModel
{
    using System.ComponentModel.DataAnnotations;

    public class BusinessCardExcelModelCreate
    {
        [Required(ErrorMessage = "Business Card Name is required.")]
        [StringLength(100, ErrorMessage = "Business Card Name cannot be longer than 100 characters.")]
        [Display(Name = "Card Name")]
        public string BusinessCardName { get; set; }

        [Display(Name = "Card Title")]
        public string? BusinessCardTitle { get; set; }

        [Display(Name = "Card Phone")]
        public string? BusinessCardPhone { get; set; }

        [Display(Name = "Card Email")]
        public string? BusinessCardEmail { get; set; }

        [Display(Name = "Card Company")]
        public string? BusinessCardCompany { get; set; }

        [Display(Name = "Card Website")]
        public string? BusinessCardWebsite { get; set; }

        [Display(Name = "Card Address")]
        public string? BusinessCardAddress { get; set; }

        [Display(Name = "Card Notes")]
        public string? BusinessCardNotes { get; set; }

        public string CreateId { get; set; } = string.Empty;
    }

}
