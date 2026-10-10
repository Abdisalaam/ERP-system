using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Accountingsystem.ViewModel
{
    public class MatchingVM
    {

        public string Batch_Number { get; set; }
        [Remote("CheckCode", "Matching")]
        public string Matching_Code { get; set; }
        public int Match_Id { get; set; }

        [Required(ErrorMessage = "حاله Matching Pending ,Post,Void")]
        [Display(Name = "حاله Matching ")]
        public TransactionState Match_state { get; set; }
        [Required(ErrorMessage = "تاريخ Matching ")]
        [Display(Name = "التاريخ")]
        //[DataType(DataType.Date)]
        public string Date { get; set; }
        [Required(ErrorMessage = "تحديد العمله من القائمه")]
        [Display(Name = "تحديد العمله ")]
        public string Match_currency { get; set; }

        public SelectList myPayCurrency { get; set; }

        [Required(ErrorMessage = "تحديد الشركه من القئمه")]
        [Display(Name = "تحديد الشركه ")]
        public int Comp_ID { get; set; }
        public SelectList MyCompanyList { get; set; }

 
        [Display(Name = "تحديد النشاط ")]
        public int Match_BU { get; set; }
        public SelectList MyBusinessUnitList { get; set; }


        public decimal Tota_Amount { get; set; }

        [Required(ErrorMessage = "تحديد الفاتوره من القائمه")]
        [Display(Name = "تحديد الفاتوره ")]
        public int SelectedInvoiceMaster_Id { get; set; }
        public SelectList MyInvoiseMasterList { get; set; }
        [Required(ErrorMessage = "تحديد الايصال من القائمه")]
        [Display(Name = "تحديد الايصال ")]
        public int SelectedReceiptId { get; set; }
        public SelectList MyReceiptList { get; set; }
        [Required(ErrorMessage = "تحديد العميل من القائمه")]
        [Display(Name = "تحديد العميل ")]
        public int SelectedCustomerID { get; set; }
        public SelectList MyCustomerList { get; set; }


        // public  MatchingDetails Invo { get; set; }
        public TempDataDictionary TempData { get; set; }


    }
}
