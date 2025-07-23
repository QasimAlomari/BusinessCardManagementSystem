using Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BusinessCard : BaseEntity
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

