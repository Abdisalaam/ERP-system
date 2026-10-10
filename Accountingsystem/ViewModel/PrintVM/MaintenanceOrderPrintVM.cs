using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class MaintenanceOrderPrintVM
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public string Describtion { get; set; }
        public DateTime Date { get; set; }
        public MaintenanceStatus Status { get; set; }
        public Nullable<int> guaranteeDuration { get; set; }
        public string Order_Code { get; set; }
        public string RequiredDescribtion { get; set; }
        public Company company { get; set; }
        public Customer customer { get; set; }
        public Employee employee { get; set; }
        public decimal totalTax { get; set; }
        public decimal totalBeforeDiscount { get; set; }
        public decimal Masterdiscount { get; set; }
        public decimal totalAfterdiscount { get; set; }
        public decimal fixed_Discount { get; set; }
        public decimal total_After_Fixed { get; set; }
        public List<MaintenanceOrderDetailPrintVM> MyDetails { get; set; }
    }
}