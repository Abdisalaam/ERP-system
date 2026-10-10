using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class ProjectReciptPrintVM
    {
        public int ProjectRect_ID { get; set; }
        public int? branchId { get; set; }
        public string ProjectCode { get; set; }
        public string ReciptCode { get; set; }
        public string Recipt_Date { get; set; }
        public string Recipt_Description { get; set; }
        public string Project_start { get; set; }
        public string projectName { get; set; }
        public string Project_End { get; set; }
        public decimal Project_Amount { get; set; }
        public decimal Pay_Amount { get; set; }
        public string Customer_Name { get; set; }
        public Company company { get; set; }
        public string Payment_Method { get; set; }
        public string Customer_Code { get; set; }
     
       
     
    }
}