using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class PrintRecalPurchInvoiceMasterVM
    {
        public int Master_Id { get; set; }
        public string Register_Date { get; set; }
        public string Due_Date { get; set; }
        public string Recall_MPurch_Serial { get; set; }
        public int SuppId { get; set; }
        public decimal Inv_TotalAmount { get; set; }
        public string PI_Currency { get; set; }
        public string Supplier_Name { get; set; }
        public decimal Inv_TotalTax { get; set; }
        public decimal Master_Discount { get; set; }
        public String invoice_Code{ get; set; }

        public List<PrintRecalPurchInvoiceDetailsVM>  MyDetailsRecall { get; set; }
        public decimal totalTax { get; set; }
        public decimal totalBeforeDiscount { get; set; }
        public decimal Masterdiscount { get; set; }
        public decimal totalAfterdiscount { get; set; }
        public Company company { get; set; }
        public string store_name { get; set; }

        public string supplierPhone { get; set; }
        public string supplierTel { get; set; }
        public string supplierfax { get; set; }
        public string supplierEmail { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeeMobile { get; set; }

    }
}