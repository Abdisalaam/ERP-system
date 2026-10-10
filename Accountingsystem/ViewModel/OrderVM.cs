using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Accountingsystem.Models;
namespace Accountingsystem.ViewModel
{
    public class OrderVM
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int TempOrderId { get; set; }
        public string OrderNumberCount { get; set; }

        public int? StoreId { get; set; }
        public int QRNumber { get; set; }
        public string OrderNumber { get; set; }
        public string OrignalOrderNumber { get; set; }
        public decimal TotalProductsTaxAmount { get; set; }
        public decimal TotalExtraTaxAmount { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalTax { get; set; }
        public int EmployeeID { get; set; }
        public TransactionState Invoice_State { get; set; }
        public decimal TotalAfterDiscount { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotaBeforeDiscountAndTax { get; set; }
        public decimal TotalNet { get; set; }
        public decimal DicountAmount { get; set; }
        public decimal DicountPrecentage { get; set; }       
        public int SelectedPaymentMethodeID { get; set; }
        public SelectList MyPaymentMethodesList { get; set; }
        public int SelectedTaxID { get; set; }
        public SelectList MyTaxList { get; set; }
        public SelectList EmpListList { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm  dd-MM-yyyy }")]
        public DateTime OrderDate { get; set; }
        public int CustomeId { get; set; }
        public int SelectedCustomer_Id { get; set; }
        public SelectList MyCustomerList { get; set; }

        public int SelectedTypeOrderId { get; set; }
        public SelectList OrderTypeList { get; set; }
        public string OrderType { get; set; }

        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerEmail { get; set; }
        public string EmployeeName { get; set; }
        public string PaymentMethodeName { get; set; }
        public string Comment { get; set; }
        public string ExtraTaxName { get; set; }
        public string ProductTaxName { get; set; }

        public List<OrderDetailsVM> OrderDetailsVM { get; set; }
        public List<CategoeryProductVM> categoeryProductVMs { get; set; }

        public List<OrderProductVM> OrderProducts { get; set; }
        public List<ReverseOrder> ReverseOrders { get; set; }
        public List<Category> OrderCategoery { get; set; }
        public Customer Customer { get; set; }
        public string Number { get; set; }
    }
}