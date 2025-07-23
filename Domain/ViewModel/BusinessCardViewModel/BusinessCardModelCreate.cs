using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.BusinessCardViewModel
{
    public class BusinessCardModelCreate
    {
        [Required(ErrorMessage = "Business Card Name is required.")]
        [StringLength(100, ErrorMessage = "Business Card Name cannot be longer than 100 characters.")]
        public string BusinessCardName { get; set; }
        public string? BusinessCardTitle { get; set; }
        public string? BusinessCardPhone { get; set; }
        public string? BusinessCardEmail { get; set; }
        public string? BusinessCardCompany { get; set; }
        public string? BusinessCardWebsite { get; set; }
        public string? BusinessCardAddress { get; set; }
        public string? BusinessCardNotes { get; set; }
    }
}
