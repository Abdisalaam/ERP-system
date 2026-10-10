using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Accountingsystem.Models
{
    public partial class InvoiceWordPrintSetting
    {
        public static InvoiceWordPrintSettingVM PrepareSetting()
        {
            var db = new AccountingContainer();
            var vm = new InvoiceWordPrintSettingVM();
            var invoiceSetting = db.InvoiceWordPrintSettings.FirstOrDefault();
            vm.Discount = invoiceSetting.Discount;
            vm.Discount1 = invoiceSetting.Discount1;
            vm.DiscountInDetail = invoiceSetting.DiscountInDetail;
            vm.Id = invoiceSetting.Id;
            vm.ToatalAmountPaid = invoiceSetting.ToatalAmountPaid;
            vm.ToatalAmountPaid1 = invoiceSetting.ToatalAmountPaid1;
            vm.ToatalRamainingAmount = invoiceSetting.ToatalRamainingAmount;
            vm.ToatalRamainingAmount1 = invoiceSetting.ToatalRamainingAmount1;
            vm.ToatlAmountDue = invoiceSetting.ToatlAmountDue;
            vm.ToatlAmountDue1 = invoiceSetting.ToatlAmountDue1;
            vm.TotalExcludingVat = invoiceSetting.TotalExcludingVat;
            vm.TotalExcludingVat1 = invoiceSetting.TotalExcludingVat1;
            vm.TotalTaxableAmount = invoiceSetting.TotalTaxableAmount;
            vm.TotalTaxableAmount1 = invoiceSetting.TotalTaxableAmount1;
            vm.TotalVat = invoiceSetting.TotalVat;
            vm.TotalVat1 = invoiceSetting.TotalVat1;
            if (invoiceSetting.Time != null)
            {
                vm.Time = invoiceSetting.Time.Value;
            }
            if (invoiceSetting.BaptismNumber != null)
            {
                vm.BaptismNumber = invoiceSetting.BaptismNumber.Value;
            }
         
            return vm;

        }
        public static string EditSetting(InvoiceWordPrintSettingVM vm)
        {
            var db = new AccountingContainer();
       
            var invoiceSetting = db.InvoiceWordPrintSettings.FirstOrDefault();
            invoiceSetting.Discount = vm.Discount;
            invoiceSetting.Discount1 = vm.Discount1;
            invoiceSetting.DiscountInDetail = vm.DiscountInDetail;
          
            invoiceSetting.ToatalAmountPaid = vm.ToatalAmountPaid;
            invoiceSetting.ToatalAmountPaid1 = vm.ToatalAmountPaid1;
            invoiceSetting.ToatalRamainingAmount = vm.ToatalRamainingAmount;
            invoiceSetting.ToatalRamainingAmount1 = vm.ToatalRamainingAmount1;
            invoiceSetting.ToatlAmountDue = vm.ToatlAmountDue;
            invoiceSetting.ToatlAmountDue1 = vm.ToatlAmountDue1;
            invoiceSetting.TotalExcludingVat = vm.TotalExcludingVat;
            invoiceSetting.TotalExcludingVat1 = vm.TotalExcludingVat1;
            invoiceSetting.TotalTaxableAmount = vm.TotalTaxableAmount;
            invoiceSetting.TotalTaxableAmount1 = vm.TotalTaxableAmount1;
            invoiceSetting.TotalVat = vm.TotalVat;
            invoiceSetting.TotalVat1 = vm.TotalVat1;
            invoiceSetting.Time = vm.Time;
            invoiceSetting.BaptismNumber = vm.BaptismNumber;
            db.Entry(invoiceSetting).State = EntityState.Modified;
            db.SaveChanges();
            return "تم التعديل بمجاح";

        }

        public static InvoiceWordPrintSetting GetTheSetting()
        {
            var db = new AccountingContainer();

            var invoiceSetting = db.InvoiceWordPrintSettings.FirstOrDefault();
            return invoiceSetting;

        }
    }
}