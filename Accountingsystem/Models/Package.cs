using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Accountingsystem.Models
{
    public partial class Package
    {
        public static List<Package> getAllPackage()
        {
            var db = new AccountingContainer();
            var package = db.Packages.ToList();
            return package;

        }
        public static Package getThatPackageById(int id)
        {
            var db = new AccountingContainer();
            var package = db.Packages.Find(id);
            return package;

        }
        public static PackageVM createVM()
        {

            var vm = new PackageVM();
            return vm;

        }
        public static string CreatePackagePost(PackageVM vm)
        {
            var db = new AccountingContainer();
            var pack = new Package();
            pack.Asset = vm.Asset;
            pack.Commission = vm.Commission;
            pack.Number_Branch = vm.Number_Branch;
            pack.Number_POS = vm.Number_POS;
            pack.CompoundProduct = vm.ProductCompound;
            pack.Number_User = vm.Number_User;
            pack.HR = vm.HR;
            pack.ExtraTaxes = vm.ExtraTaxes;
            pack.Expenses= vm.Expences;
            pack.Package_duration = vm.Package_duration;
            pack.Package_Name = vm.Package_Name;
            pack.Project = vm.Project;
            pack.Subscribtion = vm.Subscribtion;
            pack.OffGL = vm.OffGL;
            pack.Number_store = vm.Number_store;
            pack.Number_subLedgger = vm.Number_subLedgger;
            pack.Number_Employee = vm.Number_Employee;

            pack.Accounting = vm.Accounting;
            pack.ChartOfAccount = vm.ChartOfAccount;
            pack.Maintenance = vm.Maintenance;
            pack.ReportsAndPending = vm.ReportsAndPending;
            pack.Suppliers = vm.Suppliers;
            pack.Purchase = vm.Purchase;
            pack.Casher = vm.Casher;
            pack.Sales = vm.Sales;
            pack.StartOfInReverse = vm.StartOfInReverse;
            pack.StartOfInvoice = vm.StartOfInvoice;
            pack.NumberOfInvoice = vm.NumberOfInvoice;
            pack.NumberOfReverse = vm.NumberOfReverse;
            pack.Attendingandleaving = vm.Attendingandleaving;
            pack.PrintSpecial = vm.PrintSpecial;
            pack.Sewing = vm.Sewing;
            pack.SewingArab = vm.SewingArab;
            pack.SewingEng = vm.SewingEng;
            db.Packages.Add(pack);
            db.SaveChanges();
            return "تم اضافة باقة جديدة";



        }
        public static PackageVM fillForEditing(int id)
        {
            var db = new AccountingContainer();
            var pack = db.Packages.Find(id);
            var vm = new PackageVM();
            vm.Asset = pack.Asset;
            vm.Number_Branch = pack.Number_Branch;
            vm.Number_POS = pack.Number_POS;
            vm.Number_User = pack.Number_User;
            if (pack.CompoundProduct == null)
            {
                vm.ProductCompound = false;
            }
            else
            {
                vm.ProductCompound = pack.CompoundProduct.Value;
            }
            if (pack.Commission == null)
            {
                vm.Commission = false;
            }
            else
            {
                vm.Commission = pack.Commission.Value;
            }
            vm.Package_duration = pack.Package_duration;
            vm.Package_Name = pack.Package_Name;
            vm.HR = pack.HR;
            if (pack.Expenses == null)
            {
                vm.Expences = true;
            }
            else
            {
                vm.Expences = pack.Expenses.Value;
            }
            if (pack.ExtraTaxes == null)
            {
                vm.ExtraTaxes = false;
            }
            else
            {
                vm.ExtraTaxes = pack.ExtraTaxes.Value;
            }

            if (pack.Accounting == null)
            {
                vm.Accounting = true;
            }
            else
            {
                vm.Accounting = pack.Accounting.Value;
            }

            if (pack.Suppliers == null)
            {
                vm.Suppliers = true;
            }
            else
            {
                vm.Suppliers = pack.Suppliers.Value;
            }

            if (pack.ReportsAndPending == null)
            {
                vm.ReportsAndPending = true;
            }
            else
            {
                vm.ReportsAndPending = pack.ReportsAndPending.Value;
            }


            if (pack.ChartOfAccount == null)
            {
                vm.ChartOfAccount = true;
            }
            else
            {
                vm.ChartOfAccount = pack.ChartOfAccount.Value;
            }
            if (pack.Maintenance == null)
            {
                vm.Maintenance = false;
            }
            else
            {
                vm.Maintenance = pack.Maintenance.Value;
            }
            if (pack.Purchase == null)
            {
                vm.Purchase = true;
            }
            else
            {
                vm.Purchase = pack.Purchase.Value;
            }
            if (pack.Casher == null)
            {
                vm.Casher = false;
            }
            else
            {
                vm.Casher = pack.Casher.Value;
            }
            if (pack.Sales == null)
            {
                vm.Sales = true;
            }
            else
            {
                vm.Sales = pack.Sales.Value;
            }
            if (pack.Attendingandleaving == null)
            {
                vm.Attendingandleaving = false;
            }
            else
            {
                vm.Attendingandleaving = pack.Attendingandleaving.Value;
            }
            if (pack.PrintSpecial == null)
            {
                vm.PrintSpecial = false;
            }
            else
            {
                vm.PrintSpecial = pack.PrintSpecial.Value;
            }
            if (pack.Sewing == null)
            {
                vm.Sewing = false;
            }
            else
            {
                vm.Sewing = pack.Sewing.Value;
            }
            vm.SewingArab = pack.SewingArab;
            vm.SewingEng = pack.SewingEng;
            vm.StartOfInReverse = pack.StartOfInReverse;
            vm.StartOfInvoice = pack.StartOfInvoice;
            vm.Project = pack.Project;
            vm.Subscribtion = pack.Subscribtion;
            vm.OffGL = pack.OffGL;
            vm.Number_store = pack.Number_store;
            vm.Number_subLedgger = pack.Number_subLedgger;
            vm.Id = pack.Id;
            vm.Number_Employee = pack.Number_Employee;
            return vm;



        }
        public static string EditPost(PackageVM vm)
        {
            var db = new AccountingContainer();
            var pack = db.Packages.Find(vm.Id);
            pack.Asset = vm.Asset;
            pack.Number_Branch = vm.Number_Branch;
            pack.Number_POS = vm.Number_POS;
            pack.Number_User = vm.Number_User;
            pack.Package_duration = vm.Package_duration;
            pack.Package_Name = vm.Package_Name;
            pack.Project = vm.Project;
            pack.Subscribtion = vm.Subscribtion;
            pack.OffGL = vm.OffGL;
            pack.Number_store = vm.Number_store;
            pack.HR = vm.HR;
            pack.ExtraTaxes = vm.ExtraTaxes;
            pack.CompoundProduct = vm.ProductCompound;
            pack.Number_Employee = vm.Number_Employee;
            pack.Number_subLedgger = vm.Number_subLedgger;
            pack.Expenses = vm.Expences;
            pack.Casher = vm.Casher;
            pack.Sales = vm.Sales;
            pack.Accounting = vm.Accounting;
            pack.ChartOfAccount = vm.ChartOfAccount;
            pack.Suppliers = vm.Suppliers;
            pack.ReportsAndPending = vm.ReportsAndPending;
            pack.Maintenance = vm.Maintenance;
            pack.Purchase = vm.Purchase;
            pack.Commission = vm.Commission;
            pack.Attendingandleaving = vm.Attendingandleaving;
            pack.PrintSpecial = vm.PrintSpecial;
            pack.StartOfInReverse = vm.StartOfInReverse;
            pack.StartOfInvoice = vm.StartOfInvoice;

            pack.Sewing = vm.Sewing;
            pack.SewingArab = vm.SewingArab;
            pack.SewingEng = vm.SewingEng;
            db.Entry(pack).State = EntityState.Modified;
            db.SaveChanges();
            return "تم تعديل الباقة بنجاح";


        }
        public static string deletePackage(int id)
        {
            var db = new AccountingContainer();
            var pack = db.Packages.Find(id);
            if (pack.Clients.Count() != 0)
            {
                return "يوجد عملاء علي هذه الباقة";

            }
            else
            {

                db.Packages.Remove(pack);
                db.SaveChanges();
                return "تم الحذف بنجاح";
            }


        }
    }
}