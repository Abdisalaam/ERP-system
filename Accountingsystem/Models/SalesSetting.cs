using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public class SalesSetting
    {
        public static List<SubLedger> getAllNotesSubledger()
        {
            var db = new AccountingContainer();
            var sub = db.SubLedgers.Where(a => a.InvoiceDesc != null || a.InvoiceNotes != null || a.OfferDesc != null || a.OfferNotes != null).ToList();
            return sub;


        }
        public static SubLedger getSubledgerById(int id)
        {
            var db = new AccountingContainer();
            var sub = db.SubLedgers.Find(id);
            return sub;


        }
        public static SubLedger getSubledgerByIdForJson(int id)
        {
            var db = new AccountingContainer();
            var sub = db.SubLedgers.Find(id);
        if(sub.InvoiceDesc!=null|| sub.InvoiceNotes != null)
            {
                return sub;
            }
            else
            {
                if (sub.SubLedgerSubLed_Id > 0)
                {
                    sub= getSubledgerByIdForJson(sub.SubLedgerSubLed_Id.Value);
                }
                else
                {
                    return sub;
                }
            }
            return sub;


        }
        public static SubLedger getSubledgerByIdForJsonforOffer(int id)
        {
            var db = new AccountingContainer();
            var sub = db.SubLedgers.Find(id);
            if (sub.OfferDesc != null || sub.OfferNotes != null)
            {
                return sub;
            }
            else
            {
                if (sub.SubLedgerSubLed_Id > 0)
                {
                    sub = getSubledgerByIdForJsonforOffer(sub.SubLedgerSubLed_Id.Value);
                }
                else
                {
                    return sub;
                }
            }
            return sub;


        }
        public static SubLedgerVM prepareSubledger()
        {
            var db = new AccountingContainer();
            SubLedgerVM vm = new SubLedgerVM();
            var j= db.SubLedgers.Where(a => a.Subled_Level == 2 && a.isActive != false && a.SubLed_Type == "قسم لفرع").ToList();
            vm.DepartSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.isActive != false && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
            vm.BranchSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.isActive != false && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
            return vm;


        }
        public static SubLedgerVM CreateNotesForSubledger(SubLedgerVM vm)
        {
            var db = new AccountingContainer();
            if (vm.SubLed_Id > 0)
            {
                
            }
            else if (vm.DepartSubledger_Id > 0)
            {
                vm.SubLed_Id = vm.DepartSubledger_Id;
            }
            else if (vm.BranchSubledger_Id > 0)
            {
                vm.SubLed_Id = vm.BranchSubledger_Id;
            }
            else
            {
                vm.SubLed_Id = 1;
            }
            var sub = db.SubLedgers.Find(vm.SubLed_Id);
            sub.InvoiceNotes = vm.InvoiceNotes;
            sub.InvoiceDesc = vm.InvoiceDescribtion;
            sub.OfferDesc = vm.OfferDesc;
            sub.OfferNotes = vm.OfferNotes;
            db.Entry(sub).State = EntityState.Modified;

            db.SaveChanges();
            return vm;


        }
        public static SubLedgerVM prepareSubledgerForEdit(int id)
        {
            var db = new AccountingContainer();
 
            var sub = db.SubLedgers.Find(id);
            SubLedgerVM vm = new SubLedgerVM();
            vm.InvoiceDescribtion = sub.InvoiceDesc;
            vm.InvoiceNotes = sub.InvoiceNotes;
            vm.SubLed_Name = sub.SubLed_Name;
            vm.InvoiceNotes = sub.InvoiceNotes;
            vm.OfferDesc = sub.OfferDesc;
            vm.OfferNotes = sub.OfferNotes;
            vm.SubLed_Name = sub.SubLed_Name;
            vm.SubLed_Id = sub.SubLed_Id;
            return vm;


        }
        public static SubLedgerVM EditNotesForSubledger(SubLedgerVM vm)
        {
            var db = new AccountingContainer();
       
                vm.SubLed_Id = vm.SubLed_Id;
         
            var sub = db.SubLedgers.Find(vm.SubLed_Id);
            sub.InvoiceNotes = vm.InvoiceNotes;
            sub.InvoiceDesc = vm.InvoiceDescribtion;
            sub.OfferDesc = vm.OfferDesc;
            sub.OfferNotes = vm.OfferNotes;
            db.Entry(sub).State = EntityState.Modified;

            db.SaveChanges();
            return vm;


        }
    }
}