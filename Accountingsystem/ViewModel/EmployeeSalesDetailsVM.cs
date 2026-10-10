using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class EmployeeSalesDetailsVM
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public decimal CommissionAmount { get; set; }
        public decimal ParentCommissionAmount { get; set; }
        public DateTime SalesDate { get; set; }
        public int SelectedExtrnalEmployeeID { get; set; }
        public SelectList ExtrnalEmployeeList { get; set; }
        public int SelectedCustomerID { get; set; }
        public SelectList CustomerList { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerMobileNumber { get; set; }       
        public string Comment { get; set; }
        public int ExternalSalesEmployeeID { get; set; }
        public ExtrnalSalesEmployee ExtrnalSalesEmployee { get; set; }

    }
}