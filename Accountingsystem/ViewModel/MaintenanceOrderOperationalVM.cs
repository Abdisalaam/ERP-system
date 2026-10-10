using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class MaintenanceOrderOperationalVM
    {
        public int Id { get; set; }
        public int Customer_Id { get; set; }
        public SelectList CustomerList { get; set; }
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public string Describtion { get; set; }
        public int Status { get; set; }
        public Nullable<int> guaranteeDuration { get; set; }
        public string Order_Code { get; set; }
        public string RequiredDescribtion { get; set; }
        public Nullable<decimal> Master_Discount { get; set; }
        public Nullable<decimal> Total_Amount { get; set; }
        public Nullable<decimal> Total_Tax { get; set; }
        public Nullable<decimal> Total_Cost { get; set; }
        public Nullable<decimal> AllowDiscount { get; set; }
        public System.DateTime Date { get; set; }
        public Nullable<System.DateTime> EndGuaranteeDate { get; set; }
        public Nullable<decimal> ExpectedAmount { get; set; }
        public int Emplyee_id { get; set; }
        public SelectList EmployeeList { get; set; }
        public bool Checked { get; set; }
    }
}