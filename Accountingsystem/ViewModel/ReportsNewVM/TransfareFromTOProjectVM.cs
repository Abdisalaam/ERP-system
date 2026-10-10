using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class TransfareFromTOProjectVM
    {
        public string TransCOde { get; set; }
        public string projectName { get; set; }
        public string StoreName { get; set; }
        public decimal Cost { get; set; }
        public Company company { get; set; }
   

    }
}