using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class MatchingReceiptDetailsVM
    {
        public int Rec_ID { get; set; }

        [DisplayName("رقم الشركه ")]
        [Required(ErrorMessage = "تحديد الشركه من القائمه ")]
        public int RecComp_ID { get; set; }

        [DisplayName("قيمة الايصال")]
        [Required(ErrorMessage = "قيمة الايصال")]
        public decimal Rec_Amount { get; set; }

        [DisplayName(" سعر الصرف")]
        [Required(ErrorMessage = " سعر الصرف")]
        public decimal? Exchange_Rate { get; set; }

        [DisplayName("رقم الايصال")]
        [Required(ErrorMessage = "رقم الايصال")]
        public string Rec_Number { get; set; }

        [DisplayName("رقم البنك ")]
        [Required(ErrorMessage = "رقم البنك في الشجره المحاسبيه")]
        public int RecBank_Account { get; set; }

        [DisplayName("وصف الايصال")]     
        public string Rec_Description { get; set; }

        [DisplayName("تاريخ القيد اليومي ")]
        [Required(ErrorMessage = "تاريخ القيد اليومي ")]
        [DataType(DataType.Date)]
        public DateTime RecGl_Date { get; set; }

        [DisplayName("تاريخ الايصال")]
        [Required(ErrorMessage = "تاريخ الايصال")]
        [DataType(DataType.Date)]
        public DateTime Rec_Date { get; set; }

        [DisplayName("حالة الايصال")]
        [Required(ErrorMessage = "حالة الايصال")]
        public TransactionState Rec_state { get; set; }

        [DisplayName(" نشاط الشركة")]
        [Required(ErrorMessage = "تحديد النشاط من القائمه")]
        public int Rec_BU { get; set; }

        [DisplayName("قيمة الايصال بعد مطابقه الفاتورة")]
        [Required(ErrorMessage = "قيمة الايصال بعد مطابقه لافاتورة")]
        public decimal TotalAmountAfMat { get; set; }

        [DisplayName("العملة")]
        [Required(ErrorMessage = "قائمة العملة")]
        public string RecCurrency_ID { get; set; }

        [DisplayName("العميل")]
        [Required(ErrorMessage = "اختيار العملاء")]
        public int Customer_Id { get; set; }
     
    }
}