using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class ExtrnalSalesEmployeeVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid Mobile Number.")]
        public string MobileNumber { get; set; }
        public string City { get; set; }
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string IBAN { get; set; }
        public string PromoCode { get; set; }
        public DateTime RegistedDate { get; set; }
        public decimal TotalChildsSalesAmountBalance { get; set; }
        public decimal TotalChildsSalesCount { get; set; }
        public decimal TotalAmountBalance { get; set; }
        public decimal TotalReceivedAmount { get; set; }
        public decimal TotalCommissionsAmount { get; set; }
        public Nullable<decimal> TotalCommissionsAmountFromChild { get; set; }
        public bool EmailConfirmed { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string ParantName { get; set; }

        public string ParentPromoCode { get; set; }
        public int TotalCustommerCount { get; set; }
        public string Comment { get; set; }
        public int SelectedFoundAboutUs { get; set; }
        public SelectList FoundAboutUs { get; set; }
        public int SelectedParentID { get; set; }
        public SelectList ParentList { get; set; }
        public List<EmployeeSalesDetailsVM> EmployeeSalesDetails { get; set; }
        public List<ExternalEmployeePaymentsVM> ExternalEmployeePayments { get; set; }


    }
}