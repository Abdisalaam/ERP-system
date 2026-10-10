using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class SubscriptionTempPrintVM
    {
        public int SubID { get; set; }
        public string SubCOde { get; set; }
        public string startTime { get; set; }
        public string DayEnd { get; set; }
        public string Description { get; set; }
        public decimal Master_Discount { get; set; }
        public decimal totalDiscounts { get; set; }
        //public int customer_Id { get; set; }
        public decimal EstimatedAMount { get; set; }
        public Company company { get; set; }
        public decimal totalTax { get; set; }
        public decimal totalafterAddingItems { get; set; }
        public decimal totalAfterdiscount { get; set; }
        public decimal fixed_Discount { get; set; }
        public decimal total_After_Fixed { get; set; }
        public decimal totalAftertax { get; set; }
        public List<SubscriptionTempItemsPrintVM> MyDetails { get; set; }
        ///new attibutes
        public string Customer_Name { get; set; }
        public string customerPhone { get; set; }
        public string customerTel { get; set; }
        public string customerfax { get; set; }
        public string customerEmail { get; set; }
        public string TaxName { get; set; }


    }
}