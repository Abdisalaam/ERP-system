using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class StoreReportVM
    {
        public string Store_Name { get; set; }
        public string Store_Code { get; set; }
        public string Store_Address { get; set; }
        public string TodayDate { get; set; }
        public decimal Total_Batch_Cost { get; set; }
        public Company Company { get; set; }
        public List<StoreProductsVM> ListofProducts { get; set; }
        
    }
}