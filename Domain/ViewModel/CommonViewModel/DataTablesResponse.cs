﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.CommonViewModel
{
    public class DataTablesRequest
    {
        [JsonProperty("draw")]
        public int Draw { get; set; }
        public int Start { get; set; }     
        public int Length { get; set; }     
        public SearchInfo Search { get; set; }
        public List<OrderInfo> Order { get; set; }
        public List<ColumnInfo> Columns { get; set; }
    }

    public class SearchInfo
    {
        public string Value { get; set; }
        public bool Regex { get; set; }
    }

    public class OrderInfo
    {
        public int Column { get; set; }
        public string Dir { get; set; }  
    }

    public class ColumnInfo
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public SearchInfo Search { get; set; }
    }
}
