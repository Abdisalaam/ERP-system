using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class ProjectReciptVM
    {
        public int ProjectRect_ID { get; set; }
        public string ProjectCode { get; set; }
    
        public string ReciptCode { get; set; }
 
        public string Recipt_Date { get; set; }
        public string Recipt_Description { get; set; }
        public string Project_start { get; set; }
        public string Project_End { get; set; }
        public decimal Project_Amount { get; set; }
     
        public decimal Pay_Amount { get; set; }
        public int Customer_ID { get; set; }
        public string Customer_Name { get; set; }
    
        public int Payment_Method { get; set; }
        public SelectList PaymentMethodList { get; set; }
        public int Proj_inv_ID { get; set; }
        public int Proj_ID { get; set; }
        public SelectList ProjecInvList { get; set; }
        public TransactionState transActionState { get; set; }
        public IEnumerable<HttpPostedFileBase> Files { get; set; }
    }
}