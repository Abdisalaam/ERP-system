using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class MatchingPaymentVM
    {
        public int Match_Id { get; set; }
        [Required(ErrorMessage = "تاريخ Matching ")]
        [Display(Name = "التاريخ")]
        public string Date { get; set; }
        [Required(ErrorMessage = "تحديد العمله من القائمه")]
        [Display(Name = "تحديد العمله ")]
        public int Match_currency { get; set; }
        public SelectList myPayCurrency { get; set; }
        [Required(ErrorMessage = "تحديد الشركه من القئمه")]
        [Display(Name = "تحديد الشركه ")]
        public int Comp_ID { get; set; }
        public SelectList MyCompanyList { get; set; }
        [Required(ErrorMessage = "تحديد النشاط من القائمه")]
        [Display(Name = "تحديد النشاط ")]
        public Nullable<int> Match_BU { get; set; }
        public SelectList MyBusinessUnitList { get; set; }

        [Required(ErrorMessage = "ادخل القيمه")]
        [Display(Name = "القيمه ")]
        public decimal Tota_Amount { get; set; }
        [Required(ErrorMessage = "تحديد المورد من القائمه")]
        [Display(Name = "تحديد المورد ")]
        public int SelectedPaymentPay_ID { get; set; }
        public SelectList MyPaymentPay { get; set; }

        [Required(ErrorMessage = "تحديد الفاتوره من القائمه")]
        [Display(Name = "تحديد الفاتوره ")]
        public int SelectedPurInvoiceMaster_Id { get; set; }
        public SelectList MyPurInvoiceMaster { get; set; }
        public string Batch_Number { get; set; }
        [Required(ErrorMessage = "حاله Matching Pending ,Post,Void")]
        [Display(Name = "حاله Matching ")]
        public TransactionState Match_state { get; set; }
        public int SelectedSupp_Id { get; set; }
        public SelectList MySuppList { get; set; }
        public decimal TotalAmountAfMat { get; set; }
        [Remote("CheckCode", "MatchingPayment")]
        public string Match_Code { get; set; }
        

    }
}