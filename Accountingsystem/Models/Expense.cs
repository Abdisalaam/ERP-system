using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using System.Web.Mvc;

using System.Data.Entity.Validation;

namespace Accountingsystem.Models
{
    public partial class Expense
    {
        /// <summary>
        /// make Expenses Unique Code 
        /// </summary>
        /// <param name="Serial"></param>
        /// <returns></returns>
        public static bool containCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.Expenses.ToList();
            foreach (var item in allinData)
            {
                if (item.Expense_Code == Serial || !Serial.StartsWith("EXP-"))
                {
                    return true;
                }

                else
                {

                }

            }
            return false;
        }

        /// <summary>
        /// chek that date is suitable
        /// </summary>
        /// <returns></returns>
        public static bool Checkdate(string date)
        {
            var datetime = Convert.ToDateTime(date);
            var db = new AccountingContainer();
            var Comstartdate = db.Companies.FirstOrDefault().StartFinancialDate;
            if (Comstartdate != null)
            {
                if (datetime < Comstartdate.Value)
                {
                    return false;
                }


            }

            return true;


        }

        /// <summary>
        /// //get all expenses that saved in data base
        /// </summary>
        /// <returns>List<Expense></returns>
        public static List<Expense> getAllData(int ?all)
        {
            var db = new AccountingContainer();
            var exp = db.Expenses.ToList();
            if (all == null)
            {
                exp.Reverse();
                exp = exp.Take(100).ToList();
            }
            return exp;


        }/// <summary>
        /// get that File by his Id
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
          public static ExpenseFileDetail getFileById(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.ExpenseFileDetails.Find(fileId);
            return file;
        }
        /// <summary>
        /// get that File by his Id
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public static ExpenseFileDetail DeleteFileById(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.ExpenseFileDetails.Find(fileId);
            db.ExpenseFileDetails.Remove(file);
            db.SaveChanges();
            return file;
        }
        /// <summary>
        /// get All Files for that Expence
        /// </summary>
        /// <param name="expId"></param>
        /// <returns></returns>
        public static List<ExpenseFileDetail> GetAllFiles(int expId)
        {
            var db = new AccountingContainer();
            var files = db.ExpenseFileDetails.Where(exp => exp.ExpenseExpense_Id == expId).ToList();
            return files;
        }
        /// <summary>
        /// that method send vm to user to fill it 
        /// </summary>
        /// <returns>ExpenseVM</returns>
        public static ExpenseVM prepareForCrate()
        {
            var db = new AccountingContainer();
            var Expvm = new ExpenseVM();
            String StartCodeOfExpenses = "050";
          

            var lista = db.ChartOfAccountings.Where(a=>a.ChartOfAccountings.FirstOrDefault()==null);
            

            var lastSerial = db.Expenses.OrderByDescending(c => c.Expense_Id).FirstOrDefault();
            if (lastSerial == null)
            {
                Expvm.Expense_Code = "EXP-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                Expvm.Expense_Code = "EXP-" + (Convert.ToInt32(lastSerial.Expense_Code.Substring(4, lastSerial.Expense_Code.Length - 4)) + 1).ToString("D3");
            }

            Expvm.Account_List = new SelectList(lista.Where(a => a.Account_Code.StartsWith(StartCodeOfExpenses)), "Account_Id", "Account_Name");
            Expvm.CompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            Expvm.CurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");
           

            Expvm.BranchSubList = new SelectList(db.SubLedgers.Where(a=>a.Subled_Level==1&&(a.SubLed_Type== "فرع"||a.SubLed_Type== "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
            Expvm.DepartSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2&&a.SubLed_Type== "قسم لفرع"), "SubLed_Id", "SubLed_Name");
            Expvm.BussinessList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3&&a.SubLed_Type== "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            Expvm.SubledgerList = new SelectList(db.SubLedgers.Where(a => a.SubLed_Type== "مركز تكلفة يدوى"), "SubLed_Id", "SubLed_Name");
            List<object> newList = new List<object>();
            var members = db.PaymentMethods.Where(a=>a.Percentage==0&&a.IsActive!=false).ToList();
            foreach (var member in members)
                newList.Add(new
                {
                    Id = member.PaymentMethod_Id,
                    Name = member.Payment_Type + "   " + member.Percentage + "%" + "   " + member.Bank_Account_Name
                });
            Expvm.Treasure_List = new SelectList(newList, "Id", "Name");
          Expvm.ChildAcountList= new SelectList(db.Accounts.Where(a=>a.Account_Code.StartsWith("SUP")).ToList(), "Accounts_Id", "Account_Name");
            Expvm.AssetList = new SelectList(db.FixedAssets, "Fasset_ID", "FAsset_Type");
            Expvm.Expense_date = DateTime.Now.ToString("dd-MM-yyyy   HH:mm:ss");
            return Expvm;

        }
        /// <summary>
        /// تقوم بحفظ المصروف الجديد فى قاعدة البيانات
        /// </summary>
        /// <param name="expvm"></param>
        /// <returns>Expense</returns>
        public static Expense createExpense(ExpenseVM expvm , String UserId)
        {
            var db = new AccountingContainer();
            var exp = new Expense();


           
          
            if (expvm.AccountTreasure_Id >0)
            {

                exp.AccountTreasure_Id = expvm.AccountTreasure_Id.Value;

            }
            var resultAccount = db.ChartOfAccountings.Find(expvm.AccountExpense_Id);
            if (resultAccount != null)
            {

                exp.AccountExpense_Id = resultAccount.Account_Id;

            }
          

              

            
            var resultCompany = db.Companies.Find(expvm.Company_Id);
            if (resultCompany != null)
            {

                exp.Company_Id = resultCompany.Comp_Id;

            }
            var resultCurrency = db.Currencies.FirstOrDefault();
            if (resultCurrency != null)
            {

                exp.Currency_Id = resultCurrency.Currency_Id;

            }
            var Tax = db.Taxes.Find(expvm.selecteTax_ID);
            if (Tax != null)
            {

                exp.Tax_ID = Tax.Tax_ID;

            }
            if (expvm.Subledger_Id != null&& expvm.Subledger_Id!=0)
            {

                exp.Subledger_Id = expvm.Subledger_Id;

            }
            else if (expvm.BussinesSubledger_Id != null &&expvm.BussinesSubledger_Id != 0)
            {

                exp.Subledger_Id = expvm.BussinesSubledger_Id;

            }
            else if (expvm.DepartSubledger_Id != null && expvm.DepartSubledger_Id != 0)
            {

                exp.Subledger_Id = expvm.DepartSubledger_Id;

            }
            else if (expvm.BranchSubledger_Id != null&& expvm.BranchSubledger_Id!=0)
            {

                exp.Subledger_Id = expvm.BranchSubledger_Id;

            }
            else { exp.Subledger_Id = 1; }

            exp.AccountExpense_Id = (int)expvm.AccountExpense_Id;
            exp.Asset_Id = expvm.ChildAccount;
            exp.BU_Id = expvm.BU_Id;
            exp.Expense_Description = expvm.Expense_Description;
            exp.Expense_Value = expvm.Expense_Value;
            
            exp.Currency_Id = expvm.Currency_Id;
           

            var comp = db.Companies.Find(expvm.Company_Id);
            if (comp != null)
            {
                var date = Convert.ToDateTime(expvm.Expense_date);
                if (date < comp.StartFinancialDate)
                {
                    exp.Expense_date = DateTime.Now;
                }
                else
                {
                    exp.Expense_date = Convert.ToDateTime(expvm.Expense_date);
                }
            }
            else
            {
                exp.Expense_date = DateTime.Now;
            }


           
            exp.Company_Id = expvm.Company_Id;
            exp.Expense_State = TransactionState.Pending;
            exp.Reference = expvm.Refference;
            var lastSerial = db.Expenses.OrderByDescending(c => c.Expense_Id).FirstOrDefault();
            if (lastSerial == null)
            {
                exp.Expense_Code = "EXP-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                exp.Expense_Code = "EXP-" + (Convert.ToInt32(lastSerial.Expense_Code.Substring(4, lastSerial.Expense_Code.Length - 4)) + 1).ToString("D3");
            }

            exp.BranchSubledger_Id = expvm.BranchSubledger_Id;
            exp.DepartSubledger_Id = expvm.DepartSubledger_Id;
            exp.BussinesSubledger_Id = expvm.BussinesSubledger_Id;
            if (expvm.Files.FirstOrDefault() != null)

            {
                foreach (var file in expvm.Files)
                {
                    var filedetails = new ExpenseFileDetail();
                    var data = new byte[file.ContentLength];
                    filedetails.FileName = file.FileName;
                    filedetails.ContentLength = file.ContentLength;
                    file.InputStream.Read(data, 0, file.ContentLength);
                    filedetails.ProImg = data;
                    filedetails.File_Extention = file.ContentType;
                    filedetails.ExpenseExpense_Id = exp.Expense_Id;
                    if (data.Length < 1000000)
                    {
                        exp.ExpenseFileDetails.Add(filedetails);
                    }
                    
                }

            }
            db.Expenses.Add(exp);
            db.SaveChanges();

            //create abatch number for that expenses
            var trans = new TransactionBatch();
            trans.UserId = UserId;
            trans.OperationName = "تسجيل مصروف";
            trans.Date = exp.Expense_date;
            trans.OperationId = exp.Expense_Id;
            trans.Operation_Code = exp.Expense_Code;
            trans.Description = exp.Expense_Description;
            var transactionExp = TransactionBatch.AddNewTransaction(trans);
            db.Dispose();
            return exp;

            
        }
        /// <summary>
        /// تقوم بأرجاع بيانات المصروف المحدد بواسطة رقمه
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Expense</returns>
        public static Expense getDetail(int id)
        {
            var db = new AccountingContainer();
            var exp = db.Expenses.Find(id);
            return exp;

        }
        /// <summary>
        ///  // لاجراء عملية التعديل vmتقوم بارجاع بيانات المصروف المراد فى
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ExpenseVM</returns>
        public static ExpenseVM fillDataForEditing(int id)
        {
            var db = new AccountingContainer();
            var exp = getDetail(id);
            var Expvm = new ExpenseVM();
            Expvm.Expense_Id = exp.Expense_Id;
            var account = exp.AccountExpense_Id;
            var currency = exp.Currency_Id;
            var company = exp.Company_Id;
            var treasure = exp.AccountTreasure_Id;
            var Asset = exp.Asset_Id;
           
            var bussiness = exp.BU_Id;


            //تقوم بارجاع القيمة السابقة من(dropdownlist) 
            var accou = db.ChartOfAccountings.Where(c => c.Account_Id == account);
            if (accou.Any())
            {
                Expvm.AccountExpense_Id = accou.FirstOrDefault().Account_Id;
            }

        

            var treas = db.PaymentMethods.Where(c => c.Bank_Account_Id == treasure);
            if (treas.Any())
            {
                Expvm.AccountTreasure_Id = treas.FirstOrDefault().PaymentMethod_Id;
            }
            var curr = db.Currencies.Where(c => c.Currency_Id == currency);
            if (curr.Any())
            {
                Expvm.Currency_Id = curr.FirstOrDefault().Currency_Id;
            }
            var com = db.Companies.Where(c => c.Comp_Id == company);
            if (com.Any())
            {
                Expvm.Company_Id = com.FirstOrDefault().Comp_Id;
            }
            var asset = db.FixedAssets.Where(c => c.Fasset_ID == Asset);
            if (asset.Any())
            {
                Expvm.Asset_Id = asset.FirstOrDefault().Fasset_ID;
            }
            var bussinresult = db.BussinesUnits.Where(c => c.BU_Id == bussiness);
            if (bussinresult.Any())
            {
                Expvm.BU_Id = bussinresult.FirstOrDefault().BU_Id;
            }

            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0 && a.isActive != false);
            
            String StartCodeOfExpenses = "050";

          
            var newList = from pay in  db.PaymentMethods.Where(a => a.Percentage == 0 && a.IsActive != false) select new {

                Id = pay.PaymentMethod_Id,
                Name = pay.Payment_Type 


            };
            
            Expvm.Treasure_List = new SelectList(newList, "Id", "Name");
            Expvm.Account_List = new SelectList(lista.Where(a => a.Account_Code.StartsWith(StartCodeOfExpenses)), "Account_Id", "Account_Name");
        
         // Expvm.SubList = new SelectList(db.SubLedgers, "SubLed_Id", "SubLed_Name");
            Expvm.CompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            Expvm.CurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");
            Expvm.AssetList = new SelectList(db.FixedAssets, "Fasset_ID", "FAsset_Type");
            Expvm.BranchSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.isActive != false && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
            if (exp.BranchSubledger_Id > 0)
            {
                Expvm.DepartSubList = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == exp.BranchSubledger_Id && p.isActive != false && p.Subled_Level == 2), "SubLed_Id", "SubLed_Name");
            }
            else
            {
                Expvm.DepartSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.isActive != false && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
                Expvm.BussinessList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.isActive != false && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");

            }
            if (exp.DepartSubledger_Id > 0)
            {
                Expvm.BussinessList = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == exp.DepartSubledger_Id && p.isActive != false && p.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            }
            else
            {
                Expvm.BussinessList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.isActive != false && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");

            }
            Expvm.SubledgerList = new SelectList(db.SubLedgers.Where(a => a.SubLed_Type == "مركز تكلفة يدوى" && a.isActive != false), "SubLed_Id", "SubLed_Name");
            Expvm.ChildAcountList = new SelectList(db.Accounts.Where(a => a.Account_Code.StartsWith("SUP")).ToList(), "Accounts_Id", "Account_Name");
            Expvm.Expense_Value = exp.Expense_Value;
            Expvm.Expense_Description = exp.Expense_Description;
            Expvm.Expense_date =exp.Expense_date.Day+"-"+exp.Expense_date.Month+"-"+exp.Expense_date.Year;


            Expvm.Expense_State = TransactionState.Pending;
            Expvm.Expense_Code = exp.Expense_Code;
            Expvm.Refference = exp.Reference;
            Expvm.BranchSubledger_Id = exp.BranchSubledger_Id;
            Expvm.DepartSubledger_Id = exp.DepartSubledger_Id;
            Expvm.BussinesSubledger_Id = exp.BussinesSubledger_Id;
            var thistaxinchart = db.ChartOfAccountings.Find(35);
       
            var newListtax = from tax in db.Taxes.Where(e => e.Tax_Code.Contains(thistaxinchart.Account_Code)).Where(a => a.IsActive != false) select new {

                Id = tax.Tax_ID,
                Name = tax.Tax_Name + "   " + tax.Tax_Percentage + "%"

            } ;
            
            Expvm.taxList = new SelectList(newListtax, "Id", "Name");

            Expvm.Expense_State = exp.Expense_State;
            Expvm.RandomValue = exp.RandomValue;



            return Expvm;

        }

        /// <summary>
        /// يتم فيها اخذ البيانات المعدلة و حفظها فى قاعدة البيانات 
        /// </summary>
        /// <param name="expvm"></param>
        /// <returns>Expense </returns>
        public static String editTheData(ExpenseVM expvm)
        {
            try
            {
                var msg = "";
                var db = new AccountingContainer();
                var exp = db.Expenses.Find(expvm.Expense_Id);
                var thisexptemps = db.ExpenseAccountTemps.Where(e => e.RandoumVAlue == exp.RandomValue).ToList();
                if (thisexptemps.Count() == 0)
                {
                    return Resources.Expenses.ExpensesIsNotBalancedModelMessage;
                }
                else
                {
                    var credit = thisexptemps.Sum(a => a.AccountExp_Debit);
                    var depit = thisexptemps.Sum(a => a.AccountTreat_Credit);
                    if (Math.Round(credit, 2, MidpointRounding.AwayFromZero) != Math.Round(depit, 2, MidpointRounding.AwayFromZero))
                    {
                        return Resources.Expenses.ExpensesIsNotBalancedModelMessage;
                    }

                }
              

                exp.Expense_State = expvm.Expense_State;
                //  exp.Exchange_Rate=
                //عمل القيد للمصروف على حسب حالته
                switch (exp.Expense_State)
                {
                    case TransactionState.Post:
                        var Glmaster1 = Expense.makeAutoLedgerPost(exp);
                        db.GLMasters.Add(Glmaster1);
                        GLMaster.GetBalanceForAccount(Glmaster1);
                        GLMaster.GetBalanceForSubLedger(Glmaster1);
                        break;
                    case TransactionState.Void:
                        if (exp.HasOff == true)
                        {
                            msg = Resources.Expenses.ExpensesIsAlradyRecorded;
                            exp.Expense_State = TransactionState.Post;
                            db.SaveChanges();

                        }
                        else
                        {
                            var Glmaster2 = Expense.makeAutoLedgerVoid(exp);
                            db.GLMasters.Add(Glmaster2);
                            GLMaster.GetBalanceForAccount(Glmaster2);
                            GLMaster.GetBalanceForSubLedger(Glmaster2);
                        }
                        break;

                }
                db.Entry(exp).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                if (msg == "")
                {
                    msg = Resources.Expenses.ExpensesIsSuccesfullyRecorded;

                }
                if (exp.Expense_State == TransactionState.Post)
                {
                    msg = Resources.validate.GLDone;
                }
                else
                {
                    msg = Resources.validate.GLReverse;
                }
                return msg;

            }
            catch (Exception ex)
            {

                var msg = ex.Message;
                var data = ex.Data;
            }
            return null;


        }

        public static ExpenseVM fillDataForEditingNew(int id)
        {
            var db = new AccountingContainer();
            var exp = getDetail(id);
            var Expvm = new ExpenseVM();
            Expvm.Expense_Id = exp.Expense_Id;
            var account = exp.AccountExpense_Id;
            var currency = exp.Currency_Id;
            var company = exp.Company_Id;
            var treasure = exp.AccountTreasure_Id;
            var Asset = exp.Asset_Id;

            var bussiness = exp.BU_Id;


            //تقوم بارجاع القيمة السابقة من(dropdownlist) 
            var accou = db.ChartOfAccountings.Where(c => c.Account_Id == account);
            if (accou.Any())
            {
                Expvm.AccountExpense_Id = accou.FirstOrDefault().Account_Id;
            }




            if (exp.AccountTreasure_Id>0)
            {
                Expvm.AccountTreasure_Id = exp.AccountTreasure_Id;
            }
            var curr = db.Currencies.FirstOrDefault();
            if (curr!=null)
            {
                Expvm.Currency_Id = curr.Currency_Id;
            }
            var com = db.Companies.FirstOrDefault();
            if (com!=null)
            {
                Expvm.Company_Id = com.Comp_Id;
            }
            var asset = db.Accounts.Where(c => c.Accounts_Id == Asset);
            if (asset.Any())
            {
                Expvm.Asset_Id = asset.FirstOrDefault().Accounts_Id;
            }
            var bussinresult = db.BussinesUnits.Where(c => c.BU_Id == bussiness);
            if (bussinresult.Any())
            {
                Expvm.BU_Id = bussinresult.FirstOrDefault().BU_Id;
            }

            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0 && a.isActive != false);

            String StartCodeOfExpenses = "050";


            var newList = from pay in db.PaymentMethods.Where(a => a.Percentage == 0 && a.IsActive != false)
                          select new
                          {

                              Id = pay.PaymentMethod_Id,
                              Name = pay.Payment_Type


                          };

            Expvm.Treasure_List = new SelectList(newList, "Id", "Name");
            Expvm.Account_List = new SelectList(lista.Where(a => a.Account_Code.StartsWith(StartCodeOfExpenses)), "Account_Id", "Account_Name");

            // Expvm.SubList = new SelectList(db.SubLedgers, "SubLed_Id", "SubLed_Name");
            Expvm.CompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            Expvm.CurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");
            Expvm.AssetList = new SelectList(db.FixedAssets, "Fasset_ID", "FAsset_Type");
            Expvm.BranchSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.isActive != false && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
            if (exp.BranchSubledger_Id > 0)
            {
                Expvm.DepartSubList = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == exp.BranchSubledger_Id && p.isActive != false && p.Subled_Level == 2), "SubLed_Id", "SubLed_Name");
            }
            else
            {
                Expvm.DepartSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.isActive != false && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
                Expvm.BussinessList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.isActive != false && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");

            }
            if (exp.DepartSubledger_Id > 0)
            {
                Expvm.BussinessList = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == exp.DepartSubledger_Id && p.isActive != false && p.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            }
            else
            {
                Expvm.BussinessList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.isActive != false && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");

            }
            Expvm.SubledgerList = new SelectList(db.SubLedgers.Where(a => a.SubLed_Type == "مركز تكلفة يدوى" && a.isActive != false), "SubLed_Id", "SubLed_Name");
            Expvm.ChildAcountList = new SelectList(db.Accounts.Where(a => a.Account_Code.StartsWith("SUP")).ToList(), "Accounts_Id", "Account_Name");
            Expvm.Expense_Value = exp.Expense_Value;
            Expvm.Expense_Description = exp.Expense_Description;
            Expvm.Expense_date = exp.Expense_date.Day + "-" + exp.Expense_date.Month + "-" + exp.Expense_date.Year;


            Expvm.Expense_State = TransactionState.Pending;
            Expvm.Expense_Code = exp.Expense_Code;
            Expvm.Refference = exp.Reference;
            Expvm.BranchSubledger_Id = exp.BranchSubledger_Id;
            Expvm.DepartSubledger_Id = exp.DepartSubledger_Id;
            Expvm.BussinesSubledger_Id = exp.BussinesSubledger_Id;

            Expvm.selecteTax_ID = exp.Tax_ID;
            Expvm.AccountTreasure_Id = exp.AccountTreasure_Id;
            Expvm.AccountExpense_Id = exp.AccountExpense_Id;
            Expvm.Expense_Value = exp.Expense_Value;
            if (exp.Asset_Id > 0)
            {
                Expvm.ChildAccount = exp.Asset_Id.Value;
            }
            var thistaxinchart = db.ChartOfAccountings.Find(35);

            var newListtax = from tax in db.Taxes.Where(e => e.Tax_Code.Contains(thistaxinchart.Account_Code)).Where(a => a.IsActive != false)
                             select new
                             {

                                 Id = tax.Tax_ID,
                                 Name = tax.Tax_Name + "   " + tax.Tax_Percentage + "%"

                             };

            Expvm.taxList = new SelectList(newListtax, "Id", "Name");

            Expvm.Expense_State = exp.Expense_State;
            Expvm.RandomValue = exp.RandomValue;



            return Expvm;

        }

        /// <summary>
        /// يتم فيها اخذ البيانات المعدلة و حفظها فى قاعدة البيانات 
        /// </summary>
        /// <param name="expvm"></param>
        /// <returns>Expense </returns>
   


        /// <summary>
        ///  //نقوم فيها برؤية المصروف المراد حزفه من قاعدة البيانات
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Expense</returns>
        public static Expense showForDelete(int id)
        {
            var exp = Expense.getDetail(id);

            return exp;
        }
        /// <summary>
        ///   نقوم بحذف المصروف المراد
        /// </summary>
        /// <param name="id"></param>
        /// <returns> bool</returns>
        public static String deletion(int id)
        {
            var db = new AccountingContainer();
            var expo = db.Expenses.Find(id);
            if (expo.Expense_State == TransactionState.Pending)
            {
                db.ExpenseFileDetails.RemoveRange(expo.ExpenseFileDetails);
                db.Expenses.Remove(expo);

                db.SaveChanges();

                db.Dispose();
                return Resources.validate.DeleteSuccessful;
            }
            return Resources.validate.Error;
        }
        public static String deletionactiveGL(int id)
        {
            var db = new AccountingContainer();
            var expo = db.Expenses.Find(id);
            if (expo.Expense_State != TransactionState.Pending&&expo.HasOff!=true)
            {
                var gl = db.GLMasters.Where(a => a.GLCode == expo.Expense_Code);
                foreach (var g in gl)
                {
                    db.GLDetails.RemoveRange(g.GLDetails);
                   
                    db.GLMasters.Remove(g);
                   
                }
                db.ExpenseFileDetails.RemoveRange(expo.ExpenseFileDetails);
                db.Expenses.Remove(expo);
                db.SaveChanges();



                db.Dispose();
                return Resources.validate.DeleteSuccessful;
            }
            return Resources.validate.Error;
        }

        /// <summary>
        ///   //create gl for posting operation
        //تقوم بعمل قيد للمصروف فى حالة(post)
        /// </summary>
        /// <param name="exvm"></param>
        /// <returns>GLMaster</returns>
        public static GLMaster makeAutoLedgerPost(Expense exvm)
        {
            var db = new AccountingContainer();
            var Glmaster = new GLMaster();
            //fill gl master
            Glmaster.GLCode = exvm.Expense_Code;
            Glmaster.GL_Date = exvm.Expense_date;
        
            Glmaster.Company_Id = exvm.Company_Id;
            Glmaster.GLType_Id = "سند صرف";
            Glmaster.Currency_Id = exvm.Currency_Id.ToString();
            Glmaster.GL_Desc = exvm.Expense_Description;
            Glmaster.Auto = true;
            Glmaster.GLState = TransactionState.Pending;

            //fill gl detil1

            var glDetail1 = new GLDetail();
            glDetail1.GLDetail_Credit = exvm.Expense_Value;
            glDetail1.GLDetail_Debit = 0;
            glDetail1.Account_Id = exvm.AccountTreasure_Id;
            var ChartAccountGLdetail1 = db.ChartOfAccountings.Find(glDetail1.Account_Id);
            glDetail1.Account_Name = ChartAccountGLdetail1.Account_Name;
            glDetail1.Bu_Id = 0;
            // glDetail1.Asset_Id = 0;
       
            glDetail1.GLMasterGLMaster_Id = Glmaster.GLMaster_Id;
            Glmaster.GLDetails.Add(glDetail1);
            //fill gl detil2

            var glDetail2 = new GLDetail();
            glDetail2.GLDetail_Credit = 0;
            glDetail2.GLDetail_Debit = exvm.Expense_Value;
            glDetail2.Account_Id = exvm.AccountExpense_Id;
            glDetail2.SubLedger_Id = exvm.Subledger_Id;
          var ChartAccountGLdetail2 = db.ChartOfAccountings.Find(glDetail2.Account_Id);
            glDetail2.Account_Name = ChartAccountGLdetail2.Account_Name;
            glDetail2.Bu_Id = 0;
      
            glDetail2.GLMasterGLMaster_Id = Glmaster.GLMaster_Id;
            glDetail2.Asset_Id = exvm.Asset_Id;
            Glmaster.GLDetails.Add(glDetail2);




            return Glmaster;



        }


        /// <summary>
        ///create gl for void operation
        /// تقوم بعمل قيد للمصروف فى حالة(void)
        /// </summary>
        /// <param name="exvm"></param>
        /// <returns>GLMaster</returns>
        public static GLMaster makeAutoLedgerVoid(Expense exvm)
        {
            var db = new AccountingContainer();
            var Glmaster = new GLMaster();
            //fill gl master
            Glmaster.GLCode = exvm.Expense_Code;
            Glmaster.GL_Date = exvm.Expense_date;
            Glmaster.Company_Id = exvm.Company_Id;
            Glmaster.GLType_Id = "سند صرف";
            Glmaster.Currency_Id = exvm.Currency_Id.ToString();
            Glmaster.GL_Desc = "سند صرف";
            Glmaster.GLState = TransactionState.Pending;
            Glmaster.Auto = true;
            //fill gl detil1
            var glDetail1 = new GLDetail();
            glDetail1.GLDetail_Credit = 0;
            glDetail1.GLDetail_Debit = exvm.Expense_Value;
            glDetail1.Account_Id = exvm.AccountTreasure_Id;
            var ChartAccountGLdetail1 = db.ChartOfAccountings.Find(glDetail1.Account_Id);
            glDetail1.Account_Name = ChartAccountGLdetail1.Account_Name;
            glDetail1.Bu_Id = 0;
            glDetail1.GLMasterGLMaster_Id = Glmaster.GLMaster_Id;
            Glmaster.GLDetails.Add(glDetail1);
            //fill gl detil2

            var glDetail2 = new GLDetail();
            glDetail2.GLDetail_Credit = exvm.Expense_Value;
            glDetail2.GLDetail_Debit = 0;
            glDetail2.Account_Id = exvm.AccountExpense_Id;
            var ChartAccountGLdetail2 = db.ChartOfAccountings.Find(glDetail2.Account_Id);
            glDetail2.Account_Name = ChartAccountGLdetail2.Account_Name;
            glDetail2.Bu_Id =0;
            glDetail2.SubLedger_Id = exvm.Subledger_Id;
            glDetail2.GLMasterGLMaster_Id = Glmaster.GLMaster_Id;
            glDetail2.Asset_Id = exvm.Asset_Id;
            Glmaster.GLDetails.Add(glDetail2);


            return Glmaster;



        }

        /// <summary>
        /// edit data in expenses and save it in data base
        /// </summary>
        /// <param name="expvm"></param>
        /// <returns>Expense</returns>
        public static Expense DataForEditing(ExpenseVM expvm)
        {
            var db = new AccountingContainer();
            var exp = db.Expenses.Find(expvm.Expense_Id);
            if (exp != null)
            {//نقوم بتعديل 

                var resultBU = db.BussinesUnits.Find(expvm.BU_Id);
                if (resultBU != null)
                {

                    exp.BU_Id = resultBU.BU_Id;

                }
          
                    exp.AccountTreasure_Id = 0;

                    exp.AccountExpense_Id = 0;

                var resultCompany = db.Companies.Find(expvm.Company_Id);
                if (resultCompany != null)
                {

                    exp.Company_Id = resultCompany.Comp_Id;

                }
                var resultCurrency = db.Currencies.Find(expvm.Currency_Id);
                if (resultCurrency != null)
                {

                    exp.Currency_Id = resultCurrency.Currency_Id;

                }
                var resultAsset = db.FixedAssets.Find(expvm.Asset_Id);
                if (resultAsset != null)
                {

                    exp.Asset_Id = resultAsset.Fasset_ID;

                }
                if (expvm.Subledger_Id != null && expvm.Subledger_Id != 0)
                {

                    exp.Subledger_Id = expvm.Subledger_Id;

                }
                else if (expvm.BussinesSubledger_Id != null && expvm.BussinesSubledger_Id != 0)
                {

                    exp.Subledger_Id = expvm.BussinesSubledger_Id;

                }
                else if (expvm.DepartSubledger_Id != null && expvm.DepartSubledger_Id != 0)
                {

                    exp.Subledger_Id = expvm.DepartSubledger_Id;

                }
                else if (expvm.BranchSubledger_Id != null && expvm.BranchSubledger_Id != 0)
                {

                    exp.Subledger_Id = expvm.BranchSubledger_Id;

                }
           
                else { exp.Subledger_Id = 1; }
                exp.BussinesSubledger_Id = expvm.BussinesSubledger_Id;
                exp.DepartSubledger_Id = expvm.DepartSubledger_Id;
                exp.BranchSubledger_Id = expvm.BranchSubledger_Id;
                var thisexptemps = db.ExpenseAccountTemps.Where(e => e.RandoumVAlue == exp.RandomValue).ToList();
                if (thisexptemps.Count() == 0)
                {
                    return null;
                }
                else
                {
                    var credit = thisexptemps.Sum(a => a.AccountExp_Debit);
                    var depit = thisexptemps.Sum(a => a.AccountTreat_Credit);
                    if (Math.Round(credit, 2, MidpointRounding.AwayFromZero) != Math.Round(depit, 2, MidpointRounding.AwayFromZero))
                    {
                        return null;
                    }

                }
                if (expvm.Files.FirstOrDefault() != null)

                {
                    foreach (var file in expvm.Files)
                    {
                        var filedetails = new ExpenseFileDetail();
                        var data = new byte[file.ContentLength];
                        filedetails.FileName = file.FileName;
                        filedetails.ContentLength = file.ContentLength;
                        file.InputStream.Read(data, 0, file.ContentLength);
                        filedetails.ProImg = data;
                        filedetails.File_Extention = file.ContentType;
                        filedetails.ExpenseExpense_Id = exp.Expense_Id;
                        if (data.Length < 1000000)
                        {
                            exp.ExpenseFileDetails.Add(filedetails);
                        }
                        
                    }

                }
                
                exp.Expense_Id = expvm.Expense_Id;
                exp.RandomValue = expvm.RandomValue;
                exp.Tax_ID = expvm.selecteTax_ID;

                exp.Expense_Value = expvm.Expense_Value;
                exp.Expense_Description = expvm.Expense_Description;

                if (InvoiceMaster.Checkdate(expvm.Expense_date) == true)
                {
                    exp.Expense_date = Convert.ToDateTime(expvm.Expense_date);
                }
                else
                {
                    exp.Expense_date = DateTime.Now;
                }
              
                exp.Reference = expvm.Refference;
                exp.Expense_Code = expvm.Expense_Code;
                exp.BranchSubledger_Id = expvm.BranchSubledger_Id;
                exp.DepartSubledger_Id = expvm.DepartSubledger_Id;
                exp.BussinesSubledger_Id = expvm.BussinesSubledger_Id;
                //var expdetail = db.ExpenseAccountTemps.Where(a => a.Expense_ID == exp.Expense_Id&&a.AccountExp_ID!=0&&a.AccountExp_ID!=null).ToList();
                //foreach(var detail in expdetail)
                //{
                //    var account = db.ChartOfAccountings.Find(detail.AccountExp_ID).Account_Code;

                //    if (account.StartsWith("05"))
                //    {
                //        detail.SubLedger_ID = exp.Subledger_Id;
                //    }
                //    else
                //    {
                //        detail.SubLedger_ID = 0;
                //    }
                //}

                db.Entry(exp).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                return exp;
            }
            return null;
        }


        public static Expense DataForEditingNew(ExpenseVM expvm)
        {
            var db = new AccountingContainer();
            var exp = db.Expenses.Find(expvm.Expense_Id);
            if (exp != null)
            {//نقوم بتعديل 

                var resultBU = db.BussinesUnits.Find(expvm.BU_Id);
                if (resultBU != null)
                {

                    exp.BU_Id = resultBU.BU_Id;

                }

                exp.AccountTreasure_Id = 0;

                exp.AccountExpense_Id = 0;

                var resultCompany = db.Companies.Find(expvm.Company_Id);
                if (resultCompany != null)
                {

                    exp.Company_Id = resultCompany.Comp_Id;

                }
                var resultCurrency = db.Currencies.Find(expvm.Currency_Id);
                if (resultCurrency != null)
                {

                    exp.Currency_Id = resultCurrency.Currency_Id;

                }
            
                if (expvm.Subledger_Id != null && expvm.Subledger_Id != 0)
                {

                    exp.Subledger_Id = expvm.Subledger_Id;

                }
                else if (expvm.BussinesSubledger_Id != null && expvm.BussinesSubledger_Id != 0)
                {

                    exp.Subledger_Id = expvm.BussinesSubledger_Id;

                }
                else if (expvm.DepartSubledger_Id != null && expvm.DepartSubledger_Id != 0)
                {

                    exp.Subledger_Id = expvm.DepartSubledger_Id;

                }
                else if (expvm.BranchSubledger_Id != null && expvm.BranchSubledger_Id != 0)
                {

                    exp.Subledger_Id = expvm.BranchSubledger_Id;

                }

                else { exp.Subledger_Id = 1; }
                exp.BussinesSubledger_Id = expvm.BussinesSubledger_Id;
                exp.DepartSubledger_Id = expvm.DepartSubledger_Id;
                exp.BranchSubledger_Id = expvm.BranchSubledger_Id;
                
                if (expvm.Files.FirstOrDefault() != null)

                {
                    foreach (var file in expvm.Files)
                    {
                        var filedetails = new ExpenseFileDetail();
                        var data = new byte[file.ContentLength];
                        filedetails.FileName = file.FileName;
                        filedetails.ContentLength = file.ContentLength;
                        file.InputStream.Read(data, 0, file.ContentLength);
                        filedetails.ProImg = data;
                        filedetails.File_Extention = file.ContentType;
                        filedetails.ExpenseExpense_Id = exp.Expense_Id;
                        if (data.Length < 1000000)
                        {
                            exp.ExpenseFileDetails.Add(filedetails);
                        }

                    }

                }

                exp.Expense_Id = expvm.Expense_Id;
                exp.RandomValue = expvm.RandomValue;
                exp.Tax_ID = expvm.selecteTax_ID;
                if (expvm.AccountTreasure_Id > 0)
                {
                    exp.AccountTreasure_Id = expvm.AccountTreasure_Id.Value;
                }
                exp.Asset_Id = expvm.ChildAccount;
                exp.Tax_ID = expvm.selecteTax_ID;
                exp.AccountExpense_Id = expvm.AccountExpense_Id.Value;

                exp.Expense_Value = expvm.Expense_Value;
                exp.Expense_Description = expvm.Expense_Description;

                if (InvoiceMaster.Checkdate(expvm.Expense_date) == true)
                {
                    exp.Expense_date = Convert.ToDateTime(expvm.Expense_date);
                }
                else
                {
                    exp.Expense_date = DateTime.Now;
                }

                exp.Reference = expvm.Refference;
       
                exp.BranchSubledger_Id = expvm.BranchSubledger_Id;
                exp.DepartSubledger_Id = expvm.DepartSubledger_Id;
                exp.BussinesSubledger_Id = expvm.BussinesSubledger_Id;
          
                db.Entry(exp).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                return exp;
            }
            return null;
        }
        /// <summary>
        /// get All Department for that branch by using branch id
        /// </summary>
        /// <param name="Branch_Id"></param>
        /// <returns></returns>
        /// 
        public static IEnumerable<SelectListItem> GetDepartment(int Branch_Id)
        {
            var db = new AccountingContainer();
            IEnumerable<SelectListItem> Department = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == Branch_Id&&p.isActive!=false&& p.Subled_Level== 2 && p.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
            // = db.Provinces.Where(p => p.CountryId == countryId).ToList();
            return Department;
        }

        // use this method to make ListItem from Provinces Cities and get Data from DataBase.
        public static IEnumerable<SelectListItem> GetBussines(int Department_Id)
        {
            var db = new AccountingContainer();
            
            IEnumerable<SelectListItem> Bussines = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == Department_Id && p.isActive != false && p.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            // = db.Provinces.Where(p => p.CountryId == countryId).ToList();
            return Bussines;
        }
        /// <summary>
        /// Fill For Creation in Expences in new Style
        /// </summary>
        /// <returns>ExpenseVM</returns>
        public static ExpenseVM prepareForCrateNew()
        {
            var db = new AccountingContainer();
            var Expvm = new ExpenseVM();
            String StartCodeOfExpenses = "050";


            var lista = db.ChartOfAccountings.Where(a=>a.ChartOfAccountings.Count()==0&&a.isActive!=false).ToList();
            

            var lastSerial = db.Expenses.OrderByDescending(c => c.Expense_Id).FirstOrDefault();
            if (lastSerial == null)
            {
                Expvm.Expense_Code = "EXP-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                Expvm.Expense_Code = "EXP-" + (Convert.ToInt32(lastSerial.Expense_Code.Substring(4, lastSerial.Expense_Code.Length - 4)) + 1).ToString("D3");
            }

            Expvm.Account_List = new SelectList(lista.Where(a => a.Account_Code.StartsWith(StartCodeOfExpenses)), "Account_Id", "Account_Name");
            Expvm.CompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            Expvm.CurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");


            Expvm.BranchSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.isActive != false && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
            Expvm.DepartSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.isActive != false && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
            Expvm.BussinessList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.isActive != false && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            Expvm.SubledgerList = new SelectList(db.SubLedgers.Where(a => a.SubLed_Type == "مركز تكلفة يدوى" && a.isActive != false), "SubLed_Id", "SubLed_Name");
            Expvm.ChildAcountList = new SelectList(db.Accounts.Where(a => a.Account_Code.StartsWith("SUP")).ToList(), "Accounts_Id", "Account_Name");
           
            var newList = from pay in  db.PaymentMethods.Where(a => a.Percentage == 0 && a.IsActive != false)
                          select new {

                Id = pay.PaymentMethod_Id,
                Name = pay.Payment_Type

            };
            
            Expvm.Treasure_List = new SelectList(newList, "Id", "Name");

            Expvm.AssetList = new SelectList(db.FixedAssets, "Fasset_ID", "FAsset_Type");
            Expvm.Expense_date = DateTime.Now.ToString("dd-MM-yyyy   HH:mm:ss");
            var thistaxinchart = db.ChartOfAccountings.Find(35);
         
            var newListtax = from tax in db.Taxes.Where(e => e.Tax_Code.Contains(thistaxinchart.Account_Code)).Where(a=> a.IsActive != false) select new {

                Id = tax.Tax_ID,
                Name = tax.Tax_Name + "   " + tax.Tax_Percentage + "%"

            };
            
            Expvm.taxList = new SelectList(newListtax, "Id", "Name");
            Random getrandom = new Random();
            int random = getrandom.Next(1, 10000000);
            var allexp = db.Expenses.Where(e=>e.RandomValue==random).ToList();
            if (allexp.Count > 0)
            {
                random = getrandom.Next(1, 10000000);
                int i= 0;
                for (i=0; i <= random; i++)
                {
                   int  random1 = getrandom.Next(1, 10000000);
                    var thisexps = db.Expenses.Where(e => e.RandomValue == random1).ToList();
                    if (thisexps.Count > 0)
                    {
                        random1 = getrandom.Next(1, 10000000);

                    }
                    else
                    {
                        Expvm.RandomValue = random1;
                        break;
                    }
                }
            }
            else
            {
                Expvm.RandomValue = random;
            }
            return Expvm;

        }
        /// <summary>
        /// Create Expenses and save it in data base and check if depit =Credit  crease Else not Create
        /// </summary>
        /// <param name="expvm"></param>
        /// <param name="UserId"></param>
        /// <returns>Expense</returns>
        public static Expense createExpenseNew(ExpenseVM expvm , String UserId)
        {
            var db = new AccountingContainer();
            var exp = new Expense();
                exp.AccountTreasure_Id = 0;  

                exp.AccountExpense_Id = 0;
  
            var resultCompany = db.Companies.Find(expvm.Company_Id);
            if (resultCompany != null)
            {

                exp.Company_Id = resultCompany.Comp_Id;

            }
            var resultCurrency = db.Currencies.Find(expvm.Currency_Id);
            if (resultCurrency != null)
            {

                exp.Currency_Id = resultCurrency.Currency_Id;

            }
            var resultAsset = db.FixedAssets.Find(expvm.Asset_Id);
            if (resultAsset != null)
            {

                exp.Asset_Id = resultAsset.Fasset_ID;

            }
            if (expvm.Subledger_Id != null && expvm.Subledger_Id != 0)
            {

                exp.Subledger_Id = expvm.Subledger_Id;

            }
            else if (expvm.BussinesSubledger_Id != null && expvm.BussinesSubledger_Id != 0)
            {

                exp.Subledger_Id = expvm.BussinesSubledger_Id;

            }
            else if (expvm.DepartSubledger_Id != null && expvm.DepartSubledger_Id != 0)
            {

                exp.Subledger_Id = expvm.DepartSubledger_Id;

            }
            else if (expvm.BranchSubledger_Id != null && expvm.BranchSubledger_Id != 0)
            {

                exp.Subledger_Id = expvm.BranchSubledger_Id;

            }
            else { exp.Subledger_Id = 1; }
            if (expvm.selecteTax_ID > 0 && expvm.selecteTax_ID != null)
            {
                exp.Tax_ID = expvm.selecteTax_ID;
            }
            //exp.AccountExpense_Id = expvm.AccountExpense_Id;

            exp.BU_Id = expvm.BU_Id;
            exp.Expense_Description = expvm.Expense_Description;
            exp.Expense_Value = expvm.Expense_Value;

            exp.Currency_Id = expvm.Currency_Id;


            if (InvoiceMaster.Checkdate(expvm.Expense_date) == true)
            {
                exp.Expense_date = Convert.ToDateTime(expvm.Expense_date);
            }
            else
            {
                exp.Expense_date = DateTime.Now;
            }



            exp.Company_Id = expvm.Company_Id;
            exp.Expense_State = TransactionState.Pending;
            exp.Reference = expvm.Refference;
            var lastSerial = db.Expenses.OrderByDescending(c => c.Expense_Id).FirstOrDefault();
            if (lastSerial == null)
            {
                exp.Expense_Code = "EXP-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                exp.Expense_Code = "EXP-" + (Convert.ToInt32(lastSerial.Expense_Code.Substring(4, lastSerial.Expense_Code.Length - 4)) + 1).ToString("D3");
            }

            exp.BranchSubledger_Id = expvm.BranchSubledger_Id;
            exp.DepartSubledger_Id = expvm.DepartSubledger_Id;
            exp.BussinesSubledger_Id = expvm.BussinesSubledger_Id;
            exp.RandomValue = expvm.RandomValue;
            if (expvm.Files.FirstOrDefault() != null)

            {
                foreach (var file in expvm.Files)
                {
                    var filedetails = new ExpenseFileDetail();
                    var data = new byte[file.ContentLength];
                    filedetails.FileName = file.FileName;
                    filedetails.ContentLength = file.ContentLength;
                    file.InputStream.Read(data, 0, file.ContentLength);
                    filedetails.ProImg = data;
                    filedetails.File_Extention = file.ContentType;
                    filedetails.ExpenseExpense_Id = exp.Expense_Id;
                    exp.ExpenseFileDetails.Add(filedetails);
                }

            }
            var thisexptemps = db.ExpenseAccountTemps.Where(e => e.RandoumVAlue == exp.RandomValue).ToList();
            if (thisexptemps.Count() == 0)
            {
                return null;
            }
            else
            {
                var credit = thisexptemps.Sum(a => a.AccountExp_Debit);
                var depit = thisexptemps.Sum(a => a.AccountTreat_Credit);
                if (Math.Round(credit,2,MidpointRounding.AwayFromZero) !=Math.Round( depit,2,MidpointRounding.AwayFromZero))
                {
                    return null;
                }

            }
            db.Expenses.Add(exp);
            db.SaveChanges();
            
            foreach (var rand in thisexptemps)
            {
                rand.Expense_ID = exp.Expense_Id;
                db.Entry(rand).State = EntityState.Modified;
                
            }

            //create abatch number for that expenses
            var trans = new TransactionBatch();
            trans.UserId = UserId;
            trans.OperationName = "تسجيل مصروف";
            trans.Date = exp.Expense_date;
            trans.OperationId = exp.Expense_Id;
            trans.Operation_Code = exp.Expense_Code;
            trans.Description = exp.Expense_Description;
            var transactionExp = TransactionBatch.AddNewTransaction(trans);
            db.SaveChanges();
            db.Dispose();
            return exp;


        }
        /// <summary>
        /// get All Expences that have the same Randome 
        /// </summary>
        /// <param name="random"></param>
        /// <returns></returns>
        public static List<ExpenseAccountTemp> getlistofExpenseAccounts(int  random)
        {
            var db = new AccountingContainer();
            var thisexp = db.ExpenseAccountTemps.Where(e => e.RandoumVAlue == random).ToList();
            return thisexp;
        }
        /// <summary>
        /// add part of expences in Expences Temp and calculate the totel Expence and Check if Account Expenses or Account _Id
        /// </summary>
        /// <param name="EATVM"></param>
        /// <returns></returns>
        public static string addExpenseTemp(ExpenseAccountTempVM EATVM)
        {
            var db = new AccountingContainer();
        
  
            ExpenseAccountTemp extp = new ExpenseAccountTemp();
            if (EATVM.AccountExp_ID < 1 && EATVM.AccountTreasury_ID < 1)
            {
                return "يجب اختيار حساب";
            }


            if (Math.Round(EATVM.Amount,2,MidpointRounding.AwayFromZero)==0)
            {
                return "ادخل القيمة";
            }
            if (EATVM.taxid > 0 && EATVM.AccountExp_ID > 0)
            {
               
                ExpenseAccountTemp extpaccount = new ExpenseAccountTemp();
              
                    if (EATVM.SubLedger_ID != null && EATVM.SubLedger_ID != 0)
                    {

                        extpaccount.SubLedger_ID = EATVM.SubLedger_ID;

                    }
                    else if (EATVM.BussinesSubledger_Id != 0)
                    {

                        extpaccount.SubLedger_ID = EATVM.BussinesSubledger_Id;

                    }
                    else if (EATVM.DepartSubledger_Id != 0)
                    {

                        extpaccount.SubLedger_ID = EATVM.DepartSubledger_Id;

                    }
                    else if (EATVM.BranchSubledger_Id != 0)
                    {

                        extpaccount.SubLedger_ID = EATVM.BranchSubledger_Id;

                    }
                    else { extpaccount.SubLedger_ID = 1; }
                    extpaccount.AccountTreasury_ID = 0;
                    extpaccount.AccountTreat_Credit = 0;
                    var thistaxs = db.Taxes.Find(EATVM.taxid);
                decimal debnegAmount = (EATVM.Amount / (1 + (thistaxs.Tax_Percentage / 100))) * (thistaxs.Tax_Percentage / 100);

              
                    extpaccount.AccountExp_Debit = EATVM.Amount-debnegAmount;
                    var resultAccount = db.ChartOfAccountings.Find(EATVM.AccountExp_ID);
                    if (resultAccount != null)
                    {

                        extpaccount.AccountExp_ID = resultAccount.Account_Id;

                    }
                   
                    extpaccount.Tax_Bool = false;

                    if (String.IsNullOrEmpty(EATVM.ExpDesc))
                    {
                        extpaccount.ExpDesc = " ";
                    }
                    else
                    {
                        extpaccount.ExpDesc = EATVM.ExpDesc;
                    }

                    extpaccount.RandoumVAlue = EATVM.RandoumVAlue;
                    extpaccount.Expense_ID = 0;
                    db.ExpenseAccountTemps.Add(extpaccount);
                    db.SaveChanges();
               
                extp.Tax_Bool = true;
                extp.AccountExp_ID = EATVM.taxid;
                var thistax = db.Taxes.Find(EATVM.taxid);
                extp.AccountExp_Debit = (EATVM.Amount / (1 + (thistaxs.Tax_Percentage / 100))) * (thistaxs.Tax_Percentage / 100);
                //if (EATVM.SubLedger_ID != null && EATVM.SubLedger_ID != 0)
                //{

                //    extp.SubLedger_ID = EATVM.SubLedger_ID;

                //}
                //else if (EATVM.BussinesSubledger_Id != 0)
                //{

                //    extp.SubLedger_ID = EATVM.BussinesSubledger_Id;

                //}
                //else if (EATVM.DepartSubledger_Id != 0)
                //{

                //    extp.SubLedger_ID = EATVM.DepartSubledger_Id;

                //}
                //else if (EATVM.BranchSubledger_Id != 0)
                //{

                //    extp.SubLedger_ID = EATVM.BranchSubledger_Id;

                //}
                //else { extp.SubLedger_ID = 1; }
                extp.SubLedger_ID = null;
                extp.ExpDesc = EATVM.ExpDesc;

                extp.RandoumVAlue = EATVM.RandoumVAlue;
                extp.AccountTreasury_ID = 0;
                extp.AccountTreat_Credit = 0;
                extp.Expense_ID = 0;
                extp.ExpDesc = EATVM.ExpDesc;
                if (String.IsNullOrEmpty(extp.ExpDesc))
                {
                    extp.ExpDesc = " ";
                }
                else
                {
                    extp.ExpDesc = EATVM.ExpDesc;
                }

                db.ExpenseAccountTemps.Add(extp);
                try
                { db.SaveChanges(); }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            var x = validationError.PropertyName;
                            var y = validationError.ErrorMessage;
                        }
                    }
                }
                db.Dispose();
                return "تم اضافة الحساب";
            }
            else if (EATVM.taxid == 0 && EATVM.AccountExp_ID > 1)
            {

                ExpenseAccountTemp extpaccount = new ExpenseAccountTemp();
           
                    if (EATVM.SubLedger_ID != null && EATVM.SubLedger_ID != 0)
                    {

                        extpaccount.SubLedger_ID = EATVM.SubLedger_ID;

                    }
                    else if (EATVM.BussinesSubledger_Id != 0)
                    {

                        extpaccount.SubLedger_ID = EATVM.BussinesSubledger_Id;

                    }
                    else if (EATVM.DepartSubledger_Id != 0)
                    {

                        extpaccount.SubLedger_ID = EATVM.DepartSubledger_Id;

                    }
                    else if (EATVM.BranchSubledger_Id != 0)
                    {

                        extpaccount.SubLedger_ID = EATVM.BranchSubledger_Id;

                    }
                    else { extpaccount.SubLedger_ID = 1; }
                    extpaccount.AccountTreasury_ID = 0;
                    extpaccount.AccountTreat_Credit = 0;
                    
                
                    extpaccount.AccountExp_Debit = EATVM.Amount ;
                    var resultAccount = db.ChartOfAccountings.Find(EATVM.AccountExp_ID);
                    if (resultAccount != null)
                    {

                        extpaccount.AccountExp_ID = resultAccount.Account_Id;

                    }
                    
                    extpaccount.Tax_Bool = false;

                    if (String.IsNullOrEmpty(EATVM.ExpDesc))
                    {
                        extpaccount.ExpDesc = " ";
                    }
                    else
                    {
                        extpaccount.ExpDesc = EATVM.ExpDesc;
                    }

                    extpaccount.RandoumVAlue = EATVM.RandoumVAlue;
                    extpaccount.Expense_ID = 0;
                    db.ExpenseAccountTemps.Add(extpaccount);
                    db.SaveChanges();
                //}
                //////////////////////////
                
                db.Dispose();
                return "تم اضافة الحساب";
            }
            else if (EATVM.AccountTreasury_ID >= 1)
            {
                if (String.IsNullOrEmpty(EATVM.ExpDesc))
                {
                    extp.ExpDesc = " ";
                }
                else
                {
                    extp.ExpDesc = EATVM.ExpDesc;
                }

                extp.RandoumVAlue = EATVM.RandoumVAlue;
                extp.Expense_ID = 0;
                


                //var thisexoAccount = db.ExpenseAccountTemps.Where(e => e.AccountTreasury_ID == EATVM.AccountTreasury_ID && e.RandoumVAlue == EATVM.RandoumVAlue).ToList();
                //if (thisexoAccount.Count > 0)
                //{
                //    return " يوجد هذا الحساب  من قبل";
                //}
                //else
                //{
                    var resultTrea = db.PaymentMethods.Find(EATVM.AccountTreasury_ID);
                    if (resultTrea != null)
                    {

                        extp.AccountTreasury_ID = resultTrea.Bank_Account_Id.Value;

                    }
                     
                    extp.AccountTreat_Credit = EATVM.Amount;
                    extp.AccountExp_Debit = 0;
                    extp.AccountExp_ID = 0;
                    extp.SubLedger_ID = 0;
                    extp.Tax_Bool = false;
                //}


                db.ExpenseAccountTemps.Add(extp);
                db.SaveChanges();
                db.Dispose();
                return "تم اضافة الحساب";
            }
            else if (EATVM.childAccount >= 1)
            {
                if (String.IsNullOrEmpty(EATVM.ExpDesc))
                {
                    extp.ExpDesc = " ";
                }
                else
                {
                    extp.ExpDesc = EATVM.ExpDesc;
                }

                extp.RandoumVAlue = EATVM.RandoumVAlue;
                extp.Expense_ID = 0;



                //var thisexoAccount = db.ExpenseAccountTemps.Where(e => e.AccountTreasury_ID == EATVM.AccountTreasury_ID && e.RandoumVAlue == EATVM.RandoumVAlue).ToList();
                //if (thisexoAccount.Count > 0)
                //{
                //    return " يوجد هذا الحساب  من قبل";
                //}
                //else
                //{
                var resultTrea = db.Accounts.Find(EATVM.childAccount);
                if (resultTrea != null)
                {

                    extp.AccountTreasury_ID = 45;

                }

                extp.AccountTreat_Credit = EATVM.Amount;
                extp.AccountExp_Debit = 0;
                extp.AccountExp_ID = 0;
                extp.SubLedger_ID = 0;
                extp.ChildAccount_Id = resultTrea.Accounts_Id;
                extp.Tax_Bool = false;
                //}


                db.ExpenseAccountTemps.Add(extp);
                db.SaveChanges();
                db.Dispose();
                return "تم اضافة الحساب";
            }
            return "يوجد خطء بالحساب القيد";
        }
        /// <summary>
        /// delete expense Temp From Db 
        /// </summary>
        /// <param name="expID"></param>
        /// <returns></returns>
        public static string DeleteExpenseTempItem(int expID)
        {
            var db = new AccountingContainer();
            var thistemp = db.ExpenseAccountTemps.Find(expID);
            db.ExpenseAccountTemps.Remove(thistemp);
            db.SaveChanges();
            db.Dispose();
            return "تم حذف الحساب من قائمة حسابات المصروف";
        }
        /// <summary>
        /// check Total odf Ceadit is Equal Total of Depit
        /// </summary>
        /// <param name="randNum"></param>
        /// <returns>int</returns>
        public static int checkTotals(int randNum)
        {
            var db = new AccountingContainer();
            decimal debits = 0;
            decimal credits = 0;

            var thistemps = db.ExpenseAccountTemps.Where(e => e.RandoumVAlue == randNum).ToList();
            if (thistemps.Count > 0)
            {
                foreach (var item in thistemps)
                {
                    debits = debits + item.AccountExp_Debit;
                    credits = credits + item.AccountTreat_Credit;
                }
                if (Math.Round(debits,2,MidpointRounding.AwayFromZero) ==Math.Round( credits,2,MidpointRounding.AwayFromZero))
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }
            
        }
        /// <summary>
        /// make post of Expenses and check if has tax or not        /// </summary>
        /// <param name="expID"></param>
        /// <returns>string</returns>
        public static string PostNewExpense(int expID)
        {
            var db = new AccountingContainer();
            var Glmaster = new GLMaster();
            var thisexp = db.Expenses.Find(expID);
            OperationRun.Addnewoperation(thisexp.Expense_Code);
            var operation = db.OperationRuns.Where(a => a.Code == thisexp.Expense_Code).Count();
            if (operation != 1)
            {
                OperationRun.DeleteOperation(thisexp.Expense_Code);
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.PleaseWait;
            }
            if (thisexp.Expense_State == TransactionState.Post)
            {
                OperationRun.DeleteOperation(thisexp.Expense_Code);
               
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.hasbeendoneby + " " + thisexp.Expense_State + " " + Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.previously;
            }
            else
            {
                //fill gl master
                Glmaster.GLCode = thisexp.Expense_Code;
                Glmaster.GL_Date = thisexp.Expense_date;

                Glmaster.Company_Id = thisexp.Company_Id;
                Glmaster.GLType_Id = "سند مصروف";
                Glmaster.Currency_Id = "الريال السعودي";
                Glmaster.GL_Desc = thisexp.Reference;
                Glmaster.Auto = true;
              
                var thisexpTemp = db.ExpenseAccountTemps.Where(e => e.RandoumVAlue == thisexp.RandomValue).ToList();
                if (thisexpTemp.Count()==0)
                {
                    return "لايمكنك اعتماد المصروف";
                }
                Glmaster.GLState = TransactionState.Post;
                var gl = db.GLMasters.Where(a => a.GLCode == Glmaster.GLCode && a.GLState == Glmaster.GLState).FirstOrDefault();
                if (gl != null)
                {
                    OperationRun.DeleteOperation(thisexp.Expense_Code);
                    return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                }
                if (thisexpTemp.Count > 0)
                {
                    foreach (var tempitem in thisexpTemp)
                    {
                        var glDetail1 = new GLDetail();
                        if (tempitem.AccountTreat_Credit > 0)
                        {

                            glDetail1.GLDetail_Credit = tempitem.AccountTreat_Credit;
                            glDetail1.GLDetail_Debit = 0;
                            glDetail1.Account_Id = (int)tempitem.AccountTreasury_ID;
                            var ChartAccountGLdetail1 = db.ChartOfAccountings.Find(glDetail1.Account_Id);
                            glDetail1.Account_Name = ChartAccountGLdetail1.Account_Name;
                            glDetail1.Description_Detail = tempitem.ExpDesc;
                            glDetail1.Bu_Id = 0;
                            // glDetail1.Asset_Id = 0;
                            glDetail1.Account_Child_ID = tempitem.ChildAccount_Id;
                            glDetail1.GLMasterGLMaster_Id = Glmaster.GLMaster_Id;
                            Glmaster.GLDetails.Add(glDetail1);
                        }
                        else
                        {
                            if (tempitem.Tax_Bool == true)
                            {
                                glDetail1.GLDetail_Credit = 0;
                                glDetail1.GLDetail_Debit = tempitem.AccountExp_Debit;
                                glDetail1.Account_Id = (int)tempitem.AccountExp_ID;
                                var taxName = db.Taxes.Find(glDetail1.Account_Id);
                                var chartTax = db.ChartOfAccountings.Where(a => a.Account_Code == taxName.Tax_Code).FirstOrDefault();
                                glDetail1.Account_Name = chartTax.Account_Name;
                                glDetail1.Account_Id = (int)chartTax.Account_Id;
                                glDetail1.Bu_Id = 0;
                                glDetail1.SubLedger_Id = 0;
                                glDetail1.Account_Child_ID = tempitem.ChildAccount_Id;
                                glDetail1.GLMasterGLMaster_Id = Glmaster.GLMaster_Id;
                                Glmaster.GLDetails.Add(glDetail1);
                            }
                            else
                            {
                                glDetail1.GLDetail_Credit = 0;
                                glDetail1.GLDetail_Debit = tempitem.AccountExp_Debit;
                                glDetail1.Account_Id = (int)tempitem.AccountExp_ID;
                                var expaccount = db.ChartOfAccountings.Find(glDetail1.Account_Id);
                                glDetail1.Account_Name = expaccount.Account_Name;
                                glDetail1.Bu_Id = 0;
                                glDetail1.SubLedger_Id = tempitem.SubLedger_ID;
                                glDetail1.Description_Detail = tempitem.ExpDesc;
                                glDetail1.Account_Child_ID = tempitem.ChildAccount_Id;
                                glDetail1.GLMasterGLMaster_Id = Glmaster.GLMaster_Id;
                                Glmaster.GLDetails.Add(glDetail1);
                            }
                        }
                    }
                }
                db.GLMasters.Add(Glmaster);
                db.SaveChanges();
                GLMaster.GetBalanceForAccount(Glmaster);
                GLMaster.GetBalanceForSubLedger(Glmaster);
                thisexp.Expense_State = TransactionState.Post;
                db.Entry(thisexp).State = EntityState.Modified;
                db.SaveChanges();
                OperationRun.DeleteOperation(thisexp.Expense_Code);
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
            }
           
        }
        /// <summary>
        /// make Void of Expenses and check if has tax or not        /// </summary>
        /// <param name="expID"></param>
        /// <returns>string</returns>
        public static string VoidNewExpense(int expID)
        {
            var db = new AccountingContainer();
            var Glmaster = new GLMaster();
            var thisexp = db.Expenses.Find(expID);
            OperationRun.Addnewoperation(thisexp.Expense_Code);
            var operation = db.OperationRuns.Where(a => a.Code == thisexp.Expense_Code).Count();
            if (operation != 1)
            {
                OperationRun.DeleteOperation(thisexp.Expense_Code);
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.PleaseWait;
            }
            if (thisexp.Expense_State == TransactionState.Void)
            {
                OperationRun.DeleteOperation(thisexp.Expense_Code);
                //return "لقد تم عمل" + " " + thisexp.Expense_State + " " + "من قبل";
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.hasbeendoneby + " " + thisexp.Expense_State + " " + Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.previously;
            }
            else
            {
                if (thisexp.HasOff == true)
                {
                    OperationRun.DeleteOperation(thisexp.Expense_Code);
                    return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.UnderCloseing;
                }

                //fill gl master
                Glmaster.GLCode = thisexp.Expense_Code;
                Glmaster.GL_Date = thisexp.Expense_date;

                Glmaster.Company_Id = thisexp.Company_Id;
                Glmaster.GLType_Id = "عكس سند مصروف ";
                Glmaster.Currency_Id = "الريال السعودي";
                Glmaster.GL_Desc = thisexp.Reference;
                Glmaster.Auto = true;
                Glmaster.GLState = TransactionState.Void;
                var glcheck = db.GLMasters.Where(a => a.GLCode == Glmaster.GLCode && a.GLState == Glmaster.GLState).FirstOrDefault();
                if (glcheck != null)
                {
                    OperationRun.DeleteOperation(thisexp.Expense_Code);
                    return Resources.validate.GLReverse;
                }
                var thisexpTemp = db.ExpenseAccountTemps.Where(e => e.RandoumVAlue == thisexp.RandomValue).ToList();
                if (thisexpTemp.Count() == 0)
                {
                    return "لايمكنك اعتماد المصروف";
                }
                if (thisexpTemp.Count > 0)
                {
                    foreach (var tempitem in thisexpTemp)
                    {
                        var glDetail1 = new GLDetail();
                        if (tempitem.AccountTreat_Credit > 0)
                        {

                            glDetail1.GLDetail_Debit = tempitem.AccountTreat_Credit;
                            glDetail1.GLDetail_Credit = 0;
                            glDetail1.Account_Id = (int)tempitem.AccountTreasury_ID;
                            var ChartAccountGLdetail1 = db.ChartOfAccountings.Find(glDetail1.Account_Id);
                            glDetail1.Account_Name = ChartAccountGLdetail1.Account_Name;
                            glDetail1.Bu_Id = 0;
                            glDetail1.Account_Child_ID = tempitem.ChildAccount_Id;
                            // glDetail1.Asset_Id = 0;

                            glDetail1.GLMasterGLMaster_Id = Glmaster.GLMaster_Id;
                            Glmaster.GLDetails.Add(glDetail1);
                        }
                        else
                        {
                            if (tempitem.Tax_Bool == true)
                            {
                                glDetail1.GLDetail_Debit = 0;
                                glDetail1.GLDetail_Credit = tempitem.AccountExp_Debit;
                                glDetail1.Account_Id = (int)tempitem.AccountExp_ID;
                                var taxName = db.Taxes.Find(glDetail1.Account_Id);

                                var chartTax = db.ChartOfAccountings.Where(a => a.Account_Code == taxName.Tax_Code).FirstOrDefault();
                                glDetail1.Account_Name = chartTax.Account_Name;
                                glDetail1.Account_Id = (int)chartTax.Account_Id;
                                glDetail1.Account_Name = taxName.Tax_Name;
                                glDetail1.Bu_Id = 0;
                                glDetail1.SubLedger_Id = 0;
                                glDetail1.Account_Child_ID = tempitem.ChildAccount_Id;
                                glDetail1.GLMasterGLMaster_Id = Glmaster.GLMaster_Id;
                                Glmaster.GLDetails.Add(glDetail1);
                            }
                            else
                            {
                                glDetail1.GLDetail_Debit = 0;
                                glDetail1.GLDetail_Credit = tempitem.AccountExp_Debit;
                                glDetail1.Account_Id = (int)tempitem.AccountExp_ID;
                                var expaccount = db.ChartOfAccountings.Find(glDetail1.Account_Id);
                                glDetail1.Account_Name = expaccount.Account_Name;
                                glDetail1.Description_Detail = tempitem.ExpDesc;
                                glDetail1.Bu_Id = 0;
                                glDetail1.SubLedger_Id = tempitem.SubLedger_ID;
                                glDetail1.GLMasterGLMaster_Id = Glmaster.GLMaster_Id;
                                glDetail1.Account_Child_ID = tempitem.ChildAccount_Id;
                                Glmaster.GLDetails.Add(glDetail1);
                            }
                        }
                    }
                }
                db.GLMasters.Add(Glmaster);
                db.SaveChanges();
                GLMaster.GetBalanceForAccount(Glmaster);
                GLMaster.GetBalanceForSubLedger(Glmaster);
                thisexp.Expense_State = TransactionState.Void;
                db.Entry(thisexp).State = EntityState.Modified;
                db.SaveChanges();
                OperationRun.DeleteOperation(thisexp.Expense_Code);
                return Resources.validate.GLReverse;
            }
            
        }

        public static String UpdatePaymentExpences(ExpenseVM payVM)
        {
            var msg = "";
            var db = new AccountingContainer();
            var payment = db.Expenses.Find(payVM.Expense_Id);
            OperationRun.Addnewoperation(payment.Expense_Code);
            var operation = db.OperationRuns.Where(a => a.Code == payment.Expense_Code).Count();
            if (operation != 1)
            {
                OperationRun.DeleteOperation(payment.Expense_Code);
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.PleaseWait;
            }
            if (payment.Expense_State == payVM.Expense_State)
            {
                OperationRun.DeleteOperation(payment.Expense_Code);
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.hasbeendoneby + " " + payment.Expense_State + " " + Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.previously;
            }
            else
            {
                payment.Expense_State = payVM.Expense_State;
                db.Entry(payment).State = EntityState.Modified;
                GLMaster glHeader = new GLMaster();
                // Used this GLHeader Create GL
                glHeader.GL_Date = Convert.ToDateTime(payment.Expense_date);
                glHeader.GL_Desc = payment.Expense_Description;
                glHeader.Company_Id = db.Companies.ToList().FirstOrDefault().Comp_Id;
                glHeader.GLType_Id = "مصروف";
                glHeader.Currency_Id = "الريال السعودى";
                glHeader.GLCode = payment.Expense_Code;
                glHeader.Auto = true;
                glHeader.GLState = TransactionState.Post;

                // this GL Details Start advansedrec Secound BankGL
                GLDetail ExpenseAccount = new GLDetail(),
                    TaxAccount = new GLDetail(),
                    RevenueAccount = new GLDetail(),
                    supplierCredit = new GLDetail(),
                    supplierDepit = new GLDetail();




                RevenueAccount.Bu_Id = 0;
                RevenueAccount.Asset_Id = null;



                // Used ChartOfAccountRecive  Spacefic Account this Bank or treasury or tax. 
              
                if (payment.Asset_Id > 0)
                {
                   
                    var ChartOfAccounPay = db.ChartOfAccountings.Find(45);
                    supplierCredit.Account_Id = ChartOfAccounPay.Account_Id;
                    supplierCredit.Account_Name = ChartOfAccounPay.Account_Name;
                    Accounts supplSubledger = db.Accounts.Find(payment.Asset_Id);
                    supplierCredit.Account_Child_ID = supplSubledger.Accounts_Id;
                    supplierCredit.Account_CHild_Name = supplSubledger.Account_Name;
                    supplierCredit.GLMasterGLMaster_Id = glHeader.GLMaster_Id;


                    supplierDepit.Account_Id = ChartOfAccounPay.Account_Id;
                    supplierDepit.Account_Name = ChartOfAccounPay.Account_Name;

                    supplierDepit.Account_Child_ID = supplSubledger.Accounts_Id;
                    supplierDepit.Account_CHild_Name = supplSubledger.Account_Name;
                    supplierDepit.GLMasterGLMaster_Id = glHeader.GLMaster_Id;
                }

                if (payment.AccountTreasure_Id > 0 )
                {
                    var paymento = db.PaymentMethods.Find(payment.AccountTreasure_Id);
                    var ChartOfAccounPay = db.ChartOfAccountings.Find(paymento.Bank_Account_Id);
                    RevenueAccount.Account_Id = ChartOfAccounPay.Account_Id;
                    RevenueAccount.Account_Name = ChartOfAccounPay.Account_Name;
                   
                    RevenueAccount.GLMasterGLMaster_Id = glHeader.GLMaster_Id;
                }




                if (payment.Tax_ID != null && payment.Tax_ID > 0)
                {
                    var thistax = db.Taxes.Find(payment.Tax_ID).Tax_Code;
                    if (thistax != null)
                    {
                        var taxChart = db.ChartOfAccountings.Where(a => a.Account_Code == thistax).FirstOrDefault();
                        TaxAccount.Account_Id = taxChart.Account_Id;
                        TaxAccount.Account_Name = taxChart.Account_Name;
                        TaxAccount.GLMasterGLMaster_Id = glHeader.GLMaster_Id;


                        TaxAccount.Bu_Id = 0;
                        TaxAccount.Asset_Id = null;
                    }
                }




                ExpenseAccount.Bu_Id = 1;
                ExpenseAccount.Asset_Id = null;

                // Used ChartOfAccountRecive  Spacefic Account this advansedPayment . 
                var ChartOfAccounadvansedrec = db.ChartOfAccountings.Find(payment.AccountExpense_Id);
                ExpenseAccount.Account_Id = ChartOfAccounadvansedrec.Account_Id;
                ExpenseAccount.Account_Name = ChartOfAccounadvansedrec.Account_Name;
                ExpenseAccount.GLMasterGLMaster_Id = glHeader.GLMaster_Id;

                //var sup = db.Projects.Find(project.Project_Id);
                //SubLedger supplSubledger = db.SubLedgers.Where(subled => subled.SubLed_Code == sup.Project_Code).FirstOrDefault();
                //ExpenseAccount.SubLedger_Id = supplSubledger.SubLed_Id;

                //var sup = db.SubLedgers.Find(project.Project_Id);
                ExpenseAccount.SubLedger_Id = payment.Subledger_Id;
             
                RevenueAccount.SubLedger_Id = 0;

                // This State GL Start Pending Secound Post thierd Voiod 
                switch (payment.Expense_State)
                {
                    //Post GL
                    case TransactionState.Post:
                        // From Account ExpenseAccount to RevenueAccounts.
                        //ExpenseAccount.GLDetail_Debit = payment.Pay_Amount;
                        glHeader.GLState = TransactionState.Post;
                        var gl = db.GLMasters.Where(a => a.GLCode == glHeader.GLCode && a.GLState == glHeader.GLState).FirstOrDefault();
                        if (gl != null)
                        {
                            OperationRun.DeleteOperation(payment.Expense_Code);
                            return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                        }

                        RevenueAccount.GLDetail_Credit = payment.Expense_Value;
                        if (payment.Tax_ID > 0)
                        {
                            var thistaxs = db.Taxes.Find(payment.Tax_ID);
                            var TaxAmount= (payment.Expense_Value / (1 + (thistaxs.Tax_Percentage / 100))) * (thistaxs.Tax_Percentage / 100);
                            TaxAccount.GLDetail_Debit = TaxAmount;
                            ExpenseAccount.GLDetail_Debit = payment.Expense_Value - TaxAmount;

                        }
                        else
                        {
                            ExpenseAccount.GLDetail_Debit = payment.Expense_Value;
                        }
                        if (payment.AccountTreasure_Id > 0 && payment.Asset_Id > 0)
                        {
                            supplierCredit.GLDetail_Credit = payment.Expense_Value;
                            supplierDepit.GLDetail_Debit = payment.Expense_Value;

                        }
                        else if (payment.Asset_Id > 0)
                        {
                            supplierCredit.GLDetail_Credit = payment.Expense_Value;
                        }

                        break;
                    // Void Invoice
                    case TransactionState.Void:
                        glHeader.GLType_Id = " مصروف عكسي";
                        glHeader.GLState = TransactionState.Void;
                        var glcheck = db.GLMasters.Where(a => a.GLCode == glHeader.GLCode && a.GLState == glHeader.GLState).FirstOrDefault();
                        if (glcheck != null)
                        {
                            OperationRun.DeleteOperation(payment.Expense_Code);
                            return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                        }

                        if (payment.HasOff == true)

                        {
                            msg = Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.UnderCloseing;
                            payment.Expense_State = TransactionState.Post;
                            db.SaveChanges();
                            OperationRun.DeleteOperation(payment.Expense_Code);
                            return msg;

                        }
                        else
                        {
                            // From Account RevenueAccounts  to ExpenseAccount                    

                            RevenueAccount.GLDetail_Debit = payment.Expense_Value;
                            if ( payment.Tax_ID > 0)
                            {
                                var thistaxs = db.Taxes.Find(payment.Tax_ID);
                                var TaxAmount = (payment.Expense_Value / (1 + (thistaxs.Tax_Percentage / 100))) * (thistaxs.Tax_Percentage / 100);
                                TaxAccount.GLDetail_Credit = TaxAmount;
                                ExpenseAccount.GLDetail_Credit = payment.Expense_Value - TaxAmount;

                              

                            }
                            else
                            {
                                ExpenseAccount.GLDetail_Credit = payment.Expense_Value;
                            }
                        }
                        if (payment.AccountTreasure_Id > 0 && payment.Asset_Id > 0)
                        {
                            supplierCredit.GLDetail_Debit = payment.Expense_Value;
                            supplierDepit.GLDetail_Credit = payment.Expense_Value;

                        }
                        else if (payment.Asset_Id > 0)
                        {
                            supplierCredit.GLDetail_Debit = payment.Expense_Value;
                        }
                        break;
                    default:
                        break;
                }
                if (msg == "")
                {
                 
                    if (payment.Tax_ID > 0)
                    {
                        glHeader.GLDetails.Add(TaxAccount);
                    }
                    glHeader.GLDetails.Add(ExpenseAccount);
                    //if (TaxAccount.GLDetail_Credit == 0 && TaxAccount.GLDetail_Credit == 0)
                    //{

                    //}
                    //else { glHeader.GLDetails.Add(TaxAccount); }
                    if (payment.AccountTreasure_Id > 0 && payment.Asset_Id > 0)
                    {
                        glHeader.GLDetails.Add(supplierCredit);
                        glHeader.GLDetails.Add(supplierDepit);
                    }
                    else if (payment.AccountTreasure_Id == 0 && payment.Asset_Id > 0)
                    {
                        glHeader.GLDetails.Add(supplierCredit);
                    }
                        if (RevenueAccount.Account_Id > 0)
                    {
                        glHeader.GLDetails.Add(RevenueAccount);
                    }

                    db.GLMasters.Add(glHeader);
                    GLMaster.GetBalanceForAccount(glHeader);
                    GLMaster.GetBalanceForSubLedger(glHeader);

                    db.SaveChanges();
                    msg = "تم عمل قيد الايصال بنجاح";
                    db.Dispose();
                }
                OperationRun.DeleteOperation(payment.Expense_Code);
                if (payment.Expense_State == TransactionState.Post)
                {
                    msg = Resources.validate.GLDone;
                }
                else
                {
                    msg = Resources.validate.GLReverse;
                }
                return msg;
            }

        }
    }
}