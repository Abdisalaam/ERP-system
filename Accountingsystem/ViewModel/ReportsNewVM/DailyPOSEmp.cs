using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class DailyPOSEmp
    {
        public List<MasterPOSInvoice> POSInvoiceList { get; set; }

        public List<Order> OrderList { get; set; }
        public List<Dailypayment> PaymentDailyList { get; set; }
        public List<Customer> CustomerList { get; set; }
        public string StartDate { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public string End_Date { get; set; }
        public Company company { get; set; }

        public int Employee_Id { get; set; }

        public SelectList EmployeeList { get; set; }
        public string Emp_Name { get; set; }
        public decimal totalUnpayedInvoice { get; set; }

    }
}