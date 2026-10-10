using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class PointOfSale
    {
        /// <summary>
        /// get all POint of Sales save in db
        /// </summary>
        /// <returns>List<PointOfSale></returns>
        public static List<PointOfSale> GetAllDataForPointOfSale()
        {
            var db = new AccountingContainer();
            var AllPOS = db.PointOfSales.ToList();
            return AllPOS;



        }
        /// <summary>
        /// get the point of sale by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>PointOfSale</returns>
        public static PointOfSale GetDateById(int id)
        {
            var db = new AccountingContainer();
            var POS = db.PointOfSales.Find(id);
            return POS;
        }
        /// <summary>
        /// delete Point of sale by its id and check is there invoice from that Point of sale  or not
        /// </summary>
        /// <param name="id">string</param>
        /// <returns></returns>
        public static String DeletePOS(int id)
        {
            var db = new AccountingContainer();
            var Invoice = db.MasterPOSInvoices.Where(a => a.PointOfSalePOS_Id == id).ToList();
            if (Invoice.FirstOrDefault()==null)
            {
                var POS = db.PointOfSales.Find(id);
                db.POSDetailPayments.RemoveRange(POS.POSDetailPayments);
                db.PointOfSales.Remove(POS);
                db.SaveChanges();
                db.Dispose();
                return "تم حذف نقطة البيع بنجاح";
            }
            else
            {
                return "يوجد فوتير بها نقطة البيع المراد حذفها";
            }

        }
        /// <summary>
        /// fill for create that point of sale 
        /// </summary>
        /// <returns>PointOfSaleVM</returns>
        public static PointOfSaleVM CreatePOS()
        {
            var db = new AccountingContainer();
            var POSVM = new PointOfSaleVM();
            POSVM.CompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            POSVM.BranchSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1&&a.isActive!=false && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
            POSVM.CurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");
            POSVM.DepartSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.isActive != false && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
            POSVM.BussinessList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.isActive != false && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            POSVM.SubledgerList = new SelectList(db.SubLedgers.Where(a => a.SubLed_Type == "مركز تكلفة يدوى" && a.isActive != false), "SubLed_Id", "SubLed_Name");
            var EmpList = db.Employees.Where(a => a.Reg_ID != null).ToList();
         
            var EmpForDrob = new List<Employee>();
            foreach(var emp in EmpList)
            {
                var EmpPOS = db.PointOfSales.Where(a => a.Employee_Id==emp.Emp_Id).FirstOrDefault();
                if (EmpPOS == null)
                {
                    var empo = db.Employees.Find(emp.Emp_Id);
                    EmpForDrob.Add(empo);
                }




            }
            POSVM.EmpList = new SelectList(EmpForDrob, "Emp_Id", "Emp_Name");
            POSVM.StoreList = new SelectList(db.Stores.Where(a=>a.isActive!=false), "Store_ID", "Store_Name");

            var POSTemp = new PointOfSaleTemp();
            var id = (from s in db.PointOfSaleTemps
                      select s).ToList().LastOrDefault();
            if (id == null)
            { POSTemp.POS_Id = 1; }
            else
            {
                POSTemp.POS_Id = id.POS_Id + 1;
            }
           

            db.PointOfSaleTemps.Add(POSTemp);
            db.SaveChanges();
            POSVM.POS_Id = POSTemp.POS_Id;
            return POSVM;
        }
        /// <summary>
        /// fill for create the detail for Point of sale
        /// </summary>
        /// <returns>POSDetailPaymentVM</returns>
        public static POSDetailPaymentVM CreatePOSMethod()
        {
            var db = new AccountingContainer();
            var POSpayment = new POSDetailPaymentVM();
            List<object> newList = new List<object>();
            var members = db.PaymentMethods.Where(a=>a.IsActive!=false).ToList();
            foreach (var member in members)
                newList.Add(new
                {
                    Id = member.PaymentMethod_Id,
                    Name = member.Payment_Type 
                });
            POSpayment.PaymentList = new SelectList(newList, "Id", "Name");
            return POSpayment;

        }
        /// <summary>
        /// get All List of payment method temp by the id of payment method
        /// </summary>
        /// <param name="TempMasterId"></param>
        /// <returns>List<POSDetailPaymentTemp></returns>
        public static List<POSDetailPaymentTemp> GetDataTempForCreation(int TempMasterId)
        {
            var db = new AccountingContainer();
            var POSPaymentList = db.POSDetailPaymentTemps.Where(a => a.PointOfSaleTempPOS_Id == TempMasterId).ToList();
            return POSPaymentList;
        }
        /// <summary>
        /// add to list payment List and check the validation
        /// </summary>
        /// <param name="PosPAymentVM"></param>
        /// <returns><string/returns>

        public static string AddToList(POSDetailPaymentVM PosPAymentVM)

        {
            var db = new AccountingContainer();

            if (PosPAymentVM.PaymentMethod_Id == null || PosPAymentVM.PaymentMethod_Id == 0)
            {
                return Accountingsystem.Resources.PointOfSaleCreate.PaymentMethodInformation;
            }
            var AllPayment = db.POSDetailPaymentTemps.Where(a => a.PointOfSaleTempPOS_Id == PosPAymentVM.POS_ID && a.PaymentMethod_Id == PosPAymentVM.PaymentMethod_Id).FirstOrDefault();
            if (AllPayment != null)
            {
                return Accountingsystem.Resources.PointOfSaleCreate.ThisPaymentIsEnter;
            }
            else
            {
                var POSPamentTemp = new POSDetailPaymentTemp();
                POSPamentTemp.PaymentMethod_Id = PosPAymentVM.PaymentMethod_Id;
                POSPamentTemp.PaymentMethod_Name = db.PaymentMethods.Find(PosPAymentVM.PaymentMethod_Id).Payment_Type;
                POSPamentTemp.PointOfSaleTempPOS_Id = PosPAymentVM.POS_ID;
                db.POSDetailPaymentTemps.Add(POSPamentTemp);
                db.SaveChanges();
                return Resources.MasterPOSInvoice.AddSuccessfully;
            }
        }

        /// <summary>
        /// Create POS and save it in db
        /// </summary>
        /// <param name="POSVM"></param>
        /// <returns>string</returns>
        public static string CreatePOS(PointOfSaleVM POSVM)
        {
            var db = new AccountingContainer();
            var POS = new PointOfSale();
            POS.Company_Id = POSVM.Company_Id;
            POS.Currency_Id = POSVM.Company_Id;
            POS.Employee_Id = POSVM.Employee_Id;
  
            POS.POS_Name = POSVM.POS_Name;
            if (POSVM.Store_Id==0|| POSVM.Store_Id == null)
            {
                POSVM.Store_Id = db.Stores.FirstOrDefault().Store_ID;
            }
                POS.Store_Id = POSVM.Store_Id;
            POS.Adress = POSVM.Adress;
            if (POSVM.Subledger_Id != null && POSVM.Subledger_Id != 0)
            {

                POS.Subledger_Id = POSVM.Subledger_Id.Value;

            }
            else if (POSVM.BussinesSubledger_Id != null && POSVM.BussinesSubledger_Id != 0)
            {

                POS.Subledger_Id = POSVM.BussinesSubledger_Id.Value;

            }
            else if (POSVM.DepartSubledger_Id != null && POSVM.DepartSubledger_Id != 0)
            {

                POS.Subledger_Id = POSVM.DepartSubledger_Id.Value;

            }
            else if (POSVM.BranchSubledger_Id != null && POSVM.BranchSubledger_Id != 0)
            {

                POS.Subledger_Id = POSVM.BranchSubledger_Id.Value;

            }
            else { POS.Subledger_Id = 1; }
            var PaymentMethodTemp = db.POSDetailPaymentTemps.Where(a => a.PointOfSaleTempPOS_Id == POSVM.POS_Id).ToList();
            if (PaymentMethodTemp.FirstOrDefault()==null)
            {
                return "ادخل طريقة دفع";
            }
            else
            {
                foreach (var item in PaymentMethodTemp)
                {
                    var POSPay = new POSDetailPayment(); ;
                    POSPay.PaymentMethod_Id = item.PaymentMethod_Id.Value;
                    POSPay.PaymentMethod_Name = db.PaymentMethods.Find(POSPay.PaymentMethod_Id).Payment_Type;
                    POS.POSDetailPayments.Add(POSPay);
                    db.POSDetailPaymentTemps.Remove(item);


                }
                POS.BranchSubledger_Id = POSVM.BranchSubledger_Id;
                POS.DepartSubledger_Id= POSVM.DepartSubledger_Id;
                POS.BussinesSubledger_Id = POSVM.BussinesSubledger_Id;
                db.PointOfSales.Add(POS);
                var Temp = db.PointOfSaleTemps.Find(POSVM.POS_Id);
                db.PointOfSaleTemps.Remove(Temp);
                db.SaveChanges();
                db.Dispose();
                return Resources.validate.CreateSucc;
            }

        }
        /// <summary>
        /// fill for Edit PointOfSale by POS_Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>PointOfSaleVM</returns>
        public static PointOfSaleVM EditVM(int id)
        {
            var db = new AccountingContainer();
            var POSVM = new PointOfSaleVM();
            POSVM.CompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            
            //POSVM.BranchSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
            POSVM.CurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");
            //POSVM.DepartSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
            //POSVM.BussinessList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            POSVM.SubledgerList = new SelectList(db.SubLedgers.Where(a => a.SubLed_Type == "مركز تكلفة يدوى" && a.isActive != false), "SubLed_Id", "SubLed_Name");
            var EmpList = db.Employees.Where(a => a.Reg_ID != null).ToList();

            var EmpForDrob = new List<Employee>();
            foreach (var emp in EmpList)
            {
                var EmpPOS = db.PointOfSales.Where(a => a.Employee_Id == emp.Emp_Id).FirstOrDefault();
                if (EmpPOS == null)
                {
                    var empo = db.Employees.Find(emp.Emp_Id);
                    EmpForDrob.Add(empo);
                }




            }
            var POS = db.PointOfSales.Find(id);
            POSVM.BranchSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.isActive != false && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
            if (POS.BranchSubledger_Id > 0)
            {
                POSVM.DepartSubList = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == POS.BranchSubledger_Id && p.isActive != false && p.Subled_Level == 2), "SubLed_Id", "SubLed_Name");
            }
            else
            {
                POSVM.DepartSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.isActive != false && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
                POSVM.BussinessList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.isActive != false && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            }
            if (POS.DepartSubledger_Id > 0)
            {
                POSVM.BussinessList = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == POS.DepartSubledger_Id && p.isActive != false), "SubLed_Id", "SubLed_Name");
            }
            else
            {
                POSVM.BussinessList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.isActive != false && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");

            }
            var EmpCurrent = db.Employees.Find(POS.Employee_Id);
            EmpForDrob.Add(EmpCurrent);
            POSVM.EmpList = new SelectList(EmpForDrob, "Emp_Id", "Emp_Name");
            POSVM.StoreList = new SelectList(db.Stores.Where(a=>a.isActive!=false), "Store_ID", "Store_Name");
           
            POSVM.POS_Name = POS.POS_Name;
            POSVM.Company_Id = POS.Company_Id;
            POSVM.Currency_Id = POS.Currency_Id;
            POSVM.Adress = POS.Adress;
            POSVM.Store_Id = POS.Store_Id;
            POSVM.Subledger_Id = POS.Subledger_Id;
            POSVM.Employee_Id = POS.Employee_Id;
            POSVM.DepartSubledger_Id = POS.DepartSubledger_Id;
            POSVM.BussinesSubledger_Id = POS.BussinesSubledger_Id;
            POSVM.BranchSubledger_Id = POS.BranchSubledger_Id;
            var POSTemp = new PointOfSaleTemp();
            var POSTEMP = db.PointOfSaleTemps.Where(a => a.POS_Id == id).FirstOrDefault();
            if (POSTEMP==null)
            {
                POSTemp.POS_Id = id;
                db.PointOfSaleTemps.Add(POSTemp);
                db.SaveChanges();
            }

            POSVM.POS_Id = id;


            return POSVM;



        }
        /// <summary>
        /// edit of POS and save change data in data base
        /// </summary>
        /// <param name="POSVM"></param>
        /// <returns>String</returns>
        public static String EditPost(PointOfSaleVM POSVM)
        {
            var db = new AccountingContainer();
            var POS = db.PointOfSales.Find(POSVM.POS_Id);
            POS.POS_Name = POSVM.POS_Name;
            POS.Company_Id = POSVM.Company_Id;
            POS.Currency_Id = POSVM.Currency_Id;
            POS.Adress = POSVM.Adress;
            if (POSVM.Store_Id == 0 || POSVM.Store_Id == null)
            {
                POSVM.Store_Id = db.Stores.FirstOrDefault().Store_ID;
            }
            POS.Store_Id = POSVM.Store_Id;
          
            POS.Employee_Id = POSVM.Employee_Id;
            POS.DepartSubledger_Id = POSVM.DepartSubledger_Id;
            POS.BussinesSubledger_Id = POSVM.BussinesSubledger_Id;
            POS.BranchSubledger_Id = POSVM.BranchSubledger_Id;
            if (POSVM.Subledger_Id != null && POSVM.Subledger_Id != 0)
            {

                POS.Subledger_Id = POSVM.Subledger_Id.Value;

            }
            else if (POSVM.BussinesSubledger_Id != null && POSVM.BussinesSubledger_Id != 0)
            {

                POS.Subledger_Id = POSVM.BussinesSubledger_Id.Value;

            }
            else if (POSVM.DepartSubledger_Id != null && POSVM.DepartSubledger_Id != 0)
            {

                POS.Subledger_Id = POSVM.DepartSubledger_Id.Value;

            }
            else if (POSVM.BranchSubledger_Id != null && POSVM.BranchSubledger_Id != 0)
            {

                POS.Subledger_Id = POSVM.BranchSubledger_Id.Value;

            }
            else { POS.Subledger_Id = 1; }
            var Temp = db.POSDetailPaymentTemps.Where(a => a.PointOfSaleTempPOS_Id == POSVM.POS_Id).ToList();
            if (Temp.Count==0)
            {
                return "لم تضف طرق دفع ";
            }
            List<POSDetailPaymentTemp> List = new List<POSDetailPaymentTemp> { };
            var Detail = db.POSDetailPayments.Where(a => a.PointOfSalePOS_Id == POSVM.POS_Id).ToList();
            db.POSDetailPayments.RemoveRange(Detail);
            db.SaveChanges();

            foreach (var item in Temp)
            {
                var payment = new POSDetailPayment();
                payment.PaymentMethod_Id = item.PaymentMethod_Id.Value;
                payment.PaymentMethod_Name = item.PaymentMethod_Name;
                List.Add(item);
                POS.POSDetailPayments.Add(payment);
            }
            db.POSDetailPaymentTemps.RemoveRange(List);
            var POSTemp = db.PointOfSaleTemps.Find(POSVM.POS_Id);
            db.PointOfSaleTemps.Remove(POSTemp);

            db.Entry(POSTemp).State = EntityState.Modified;
            
            db.SaveChanges();
            db.Dispose();
            return Resources.validate.Editsucc;
      





        }
        /// <summary>
        /// get List of that Point of sale details by used POS_Id and put it in detail temp of POS
        /// </summary>
        /// <param name="id"></param>
        /// <returns> List<POSDetailPaymentTemp></returns>
        public static List<POSDetailPaymentTemp> GetAllTempPointForEdit(int id)
        {

            var db = new AccountingContainer();
            var detailTemp = db.PointOfSaleTemps.Find(id);
            db.POSDetailPaymentTemps.RemoveRange(detailTemp.POSDetailPaymentTemps);
            db.SaveChanges();
            var Point = db.PointOfSales.Find(id);
            List<POSDetailPaymentTemp> Lista = new List<POSDetailPaymentTemp> { };
            foreach (var item in Point.POSDetailPayments)
            {
                var temp = new POSDetailPaymentTemp();
                temp.PaymentMethod_Id = item.PaymentMethod_Id;
                temp.PaymentMethod_Name = item.PaymentMethod_Name;
                temp.PointOfSaleTempPOS_Id = id;

               
                Lista.Add(temp);

            }
            db.POSDetailPaymentTemps.AddRange(Lista);

            db.SaveChanges();

            return detailTemp.POSDetailPaymentTemps.ToList();
        }
        /// <summary>
        /// delete the payment method temp from List by its id
        /// </summary>
        /// <param name="Detailid"></param>
        /// <returns>String</returns>
        public static String DeleteFromTemp(int Detailid)

        {
            var db = new AccountingContainer();
            var Detail = db.POSDetailPaymentTemps.Find(Detailid);
            db.POSDetailPaymentTemps.Remove(Detail);
            db.SaveChanges();
            return Resources.MasterPOSInvoice.DeleteSuccessfully;


        }
        public static bool checkValidateOfSaler(string id)
        {
            var db = new AccountingContainer();
            var Emp = db.Employees.Where(a => a.Reg_ID == id).FirstOrDefault();
            var EmpPOS = db.PointOfSales.Where(a => a.Employee_Id == Emp.Emp_Id).FirstOrDefault();
            if (EmpPOS == null)
            {
                return false;
            }
            return true;
        }

    }
}