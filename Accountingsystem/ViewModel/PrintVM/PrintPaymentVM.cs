using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class PrintPaymentVM
    {
        public int Payment_Id { get; set; }
        public string Payment_Code { get; set; }
        public string Payment_Date { get; set; }
        public Supplier supp { get; set; }

        public string Payment_Currency { get; set; }
   
        public string Supplier_Name { get; set; }
        public decimal Amount { get; set; }

        public int accountId { get; set; }
        public Company company { get; set; }

        public string Description { get; set; }
        public int EmpId { get; set; }
    }
}