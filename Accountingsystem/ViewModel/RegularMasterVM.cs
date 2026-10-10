using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class RegularMasterVM
    {
        public int Master_Id { get; set; }
        [Required(ErrorMessage = "رقم الفاتوره")]
        [Display(Name = "سريال الفاتورة")]
       
        public string Serial_Number { get; set; }
        [Required(ErrorMessage = "رقم الحركه")]
        [Display(Name = "الحركه")]
        public string Patch_Number { get; set; }
        [Required(ErrorMessage = "ادخل الوصف")]
        [Display(Name = "وصف")]
        public string Description { get; set; }
        [Required(ErrorMessage = "اختر التاريخ")]
        [Display(Name = "تاريخ الفاتورة")]
        //[DataType(DataType.Date)]
        public string Invoice_Date { get; set; }
        [Required(ErrorMessage = "اختر التاريخ")]
        [Display(Name = "تاريخ الاستحقاق")]
        //[DataType(DataType.Date)]
        public string Due_Date { get; set; }
        [Required(ErrorMessage = "اختر التاريخ")]
        [Display(Name = "تاريخ القيد")]
        //[DataType(DataType.Date)]
        public string GL_Date { get; set; }
        [Required(ErrorMessage = "قيمه الخصم")]
        [Display(Name = "الخصم")]
        public decimal Master_Discount { get; set; }
        [Required(ErrorMessage = "اختر العمله")]
        [Display(Name = "العملة")]
        public string Invoice_Currency { get; set; }
        [Required(ErrorMessage = "اختر العميل")]

        [Display(Name = "رقم العميل")]
        public int Customer_Id { get; set; }
        [Display(Name = "القيمه")]
        public decimal Total_Amount { get; set; }
        [Display(Name = "رقم الشركة")]
        public int Company_Id { get; set; }
        public decimal Total_Tax { get; set; }
        public Document_Type Document_Type { get; set; }
        public TransactionState Invoice_State { get; set; } //Enum
        [Display(Name = " الشركة")]
        [Required(ErrorMessage = "اختر الشركة من القائمه")]
        public int SelectedCompany_Id { get; set; }
        public SelectList MyCompanyList { get; set; }

        [Display(Name = " العمله")]
        [Required(ErrorMessage = "اختر العمله من القائمه")]
        public int SelectedCurrencyId { get; set; }
        public SelectList MyCurrencyList { get; set; }
        [Display(Name = " مركز التكلفه ")]
        [Required(ErrorMessage = "اختر مركز التكلفه من القائمه")]
        public int SelectedBussinesUnitId { get; set; }
        public SelectList MyBussinesUnitList { get; set; }
        [Display(Name = " العميل ")]
        [Required(ErrorMessage = "اختر العميل من القائمه")]
        public int SelectedCustomerId { get; set; }
        [Display(Name = "الحساب")]
        [Required(ErrorMessage = "اختر الحساب من القائمه")]
        public int Subledger_Id { get; set; }
        public SelectList sublist { get; set; }
        [Display(Name = "المبلغ")]
        public List<RegularDetailsVM> myDetailsVM { get; set; }

        public Nullable<decimal> Exchange_Rate { get; set; }
        public SelectList CustomerList { get; set; }
        public IEnumerable<HttpPostedFileBase> Files { get; set; }


        /////

    }
}