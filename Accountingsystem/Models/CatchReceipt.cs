using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class CatchReceipt
    {
        /// <summary>
        /// make unique code for catchRecipt
        /// </summary>
        /// <param name="Serial"></param>
        /// <returns></returns>
        public static bool containCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.CatchReceipts.ToList();
            if (!Serial.StartsWith("CARECI-"))
            {
                return true;
            }
            else
            {
                foreach (var item in allinData)
                {
                    if (item.CatchReceipt_Code == Serial || !Serial.StartsWith("CARECI-"))
                    {
                        return true;
                    }
                    else
                    {

                    }

                }
            }
            return false;
        }/// <summary>
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
        /// get all Catch Recipt from db
        /// </summary>
        /// <returns></returns>
        public static List<CatchReceipt> GetAllCatchREceipt()
        {
            var db = new AccountingContainer();
            var Receipts = db.CatchReceipts.ToList();
            return Receipts;
        }
        /// <summary>
        /// get catch Recipt by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static CatchReceipt GetCatchReceiptById(int id)
        {
            var db = new AccountingContainer();
            var Receipt = db.CatchReceipts.Find(id);
            return Receipt;
        }
        /// <summary>
        /// get file with Catch From db
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public static CatchReceiptFileDetail getFileById(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.CatchReceiptFileDetails.Find(fileId);
            return file;
        }
        /// <summary>
        /// get All File From catch Recipt
        /// </summary>
        /// <param name="catReceId"></param>
        /// <returns></returns>
        public static List<CatchReceiptFileDetail> GetAllFiles(int catReceId)
        {
            var db = new AccountingContainer();
            var files = db.CatchReceiptFileDetails.Where(exp => exp.CatchReceipt_Id == catReceId).ToList();
            return files;
        }
        /// <summary>
        /// get VM tom Create Catch Recipt
        /// </summary>
        /// <returns></returns>
        public static CatchReceiptVM AddForCreation(int child ,string type)
        {
            var db = new AccountingContainer();
            var catchReceiptVM = new CatchReceiptVM(); 
            var lastSerial = db.CatchReceipts.OrderByDescending(c => c.CatchReceipt_Id).FirstOrDefault();
            if (lastSerial == null)
            {
                catchReceiptVM.CatchReceipt_Code = "CARECI-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                catchReceiptVM.CatchReceipt_Code = "CARECI-" + (Convert.ToInt32(lastSerial.CatchReceipt_Code.Substring(7, lastSerial.CatchReceipt_Code.Length - 7)) + 1).ToString("D3");
            }
            var lista = db.ChartOfAccountings.Where(a=>a.ChartOfAccountings.Count()==0&&a.isActive!=false);
            var newList = from pay in db.PaymentMethods.Where(a=>a.IsActive!=false)
                          select new
                          {

                              Id = pay.PaymentMethod_Id,
                              Name = pay.Payment_Type

                          };
            //var chartOfAccountings = db.ChartOfAccountings.ToList();
            //foreach (var chart in chartOfAccountings)
            //{
            //    var accounts = chartOfAccountings.Where(a => a.ChartOfAccountingAccount_Id == chart.Account_Id);

            //    if (accounts.Count() < 1)
            //    {
            //        lista.Add(chart);

            //    }


            //}
            catchReceiptVM.CatchReceipt_date = DateTime.Now.ToString("dd-MM-yyyy   HH:mm:ss");
            var listao = new List<ChartOfAccounting> { };

            catchReceiptVM.AccCreditList = new SelectList(lista, "Account_Id", "Account_Name");
            catchReceiptVM.SubChildList = new SelectList(listao, "Account_Id", "Account_Name");
            catchReceiptVM.ComList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            catchReceiptVM.BranchList = new SelectList(db.Branches.Where(a=>a.IsActive!=false), "Branch_Id", "Branch_Name");
            catchReceiptVM.CurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");
        
          
            
            catchReceiptVM.AccDebitList = new SelectList(newList, "Id", "Name");
            if (type!="")
            {
                if (type == "customer")
                {
                    catchReceiptVM.AccountCredit_Id = 37;
                    catchReceiptVM.SubChildList = new SelectList(db.Accounts.Where(p => p.Account_Id == catchReceiptVM.AccountCredit_Id.ToString()), "Accounts_Id", "Account_Name");
                    var cust = db.Customers.Find(child).Customer_Code;
                    catchReceiptVM.SubChild_AccountCredit_Id = db.Accounts.Where(a => a.Account_Code == cust).FirstOrDefault().Accounts_Id;
                }



            }
            return catchReceiptVM;

        }
        /// <summary>
        /// add new Catch Recipt in Db
        /// </summary>
        /// <param name="CVM"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static int AddCatchRecipt(CatchReceiptVM CVM, String UserId)
        {
            var db = new AccountingContainer();
            var newCatch = new CatchReceipt();

            var lastSerial = db.CatchReceipts.OrderByDescending(c => c.CatchReceipt_Id).FirstOrDefault();
            if (lastSerial == null)
            {
                newCatch.CatchReceipt_Code = "CARECI-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                newCatch.CatchReceipt_Code = "CARECI-" + (Convert.ToInt32(lastSerial.CatchReceipt_Code.Substring(7, lastSerial.CatchReceipt_Code.Length - 7)) + 1).ToString("D3");
            }

            newCatch.AccountDebit_Id = CVM.AccountDebit_Id;
            //if (CVM.AccountsubDebit_Id == 0)
            //{
            //    newCatch.AccountDebit_Id = CVM.AccountDebit_Id;
            //}
            //else
            //{ newCatch.AccountDebit_Id = CVM.AccountsubDebit_Id; }
            newCatch.CatchReceipt_Value = CVM.CatchReceipt_Value;
            newCatch.CatchReceipt_Description = CVM.CatchReceipt_Description;
            var comp = db.Companies.Find(CVM.Company_Id);
            if (comp != null)
            {
                var date = Convert.ToDateTime(CVM.CatchReceipt_date);
                if (date < comp.StartFinancialDate)
                {
                    newCatch.CatchReceipt_date = DateTime.Now;
                }
                else
                {
                    newCatch.CatchReceipt_date = Convert.ToDateTime(CVM.CatchReceipt_date);
                }
            }
            else
            {
                newCatch.CatchReceipt_date = DateTime.Now;
            }
          
            newCatch.Company_Id = CVM.Company_Id;
            newCatch.Currency_Id = CVM.Currency_Id;
            newCatch.CatchReceipt_State = TransactionState.Pending;
            newCatch.Reference = CVM.Reference;
            newCatch.BranchSubledger_Id = CVM.BranchSubledger_Id;
            //if (CVM.SubChild_AccountCredit_Id == 1&&CVM.Child_AccountCredit_Id!=1)
            //{
            //    newCatch.Child_AccountCredit_Id = CVM.Child_AccountCredit_Id;
            //    newCatch.AccountCredit_Id = CVM.AccountCredit_Id;
            //}
            //else if (CVM.Child_AccountCredit_Id == 1)
            //{
            //    newCatch.Child_AccountCredit_Id = 0;
            //    newCatch.AccountCredit_Id = (int)CVM.AccountCredit_Id;
            //}
            //else
            //{
            //    newCatch.Child_AccountCredit_Id = CVM.SubChild_AccountCredit_Id;
            //    newCatch.AccountCredit_Id = (int)CVM.Child_AccountCredit_Id;
            //}
            newCatch.AccountCredit_Id = CVM.AccountCredit_Id;

            if (CVM.SubChild_AccountCredit_Id != null)
            {
                newCatch.Child_AccountCredit_Id = CVM.SubChild_AccountCredit_Id;
              
            }     
         




            if (CVM.Files.FirstOrDefault() != null)
            {
                foreach (var file in CVM.Files)
                {
                    var filedetails = new CatchReceiptFileDetail();
                    var data = new byte[file.ContentLength];
                    filedetails.FileName = file.FileName;
                    filedetails.ContentLength = file.ContentLength;
                    file.InputStream.Read(data, 0, file.ContentLength);
                    filedetails.ProImg = data;
                    filedetails.File_Extention = file.ContentType;
                    if (data.Length < 1000000)
                    {
                        newCatch.CatchReceiptFileDetails.Add(filedetails);
                    }
                   
                }
            }
            db.CatchReceipts.Add(newCatch);
            db.SaveChanges();
            var traBatch = new TransactionBatch();

            traBatch.OperationName = "سند قبض";
            traBatch.Description = CVM.CatchReceipt_Description;

            traBatch.Date = Convert.ToDateTime(CVM.CatchReceipt_date);
            traBatch.UserId = UserId;
            traBatch.Description = CVM.CatchReceipt_Description;
            traBatch.Operation_Code = newCatch.CatchReceipt_Code;
            TransactionBatch.AddNewTransaction(traBatch);
            db.Dispose();
            return newCatch.CatchReceipt_Id;

        }
        /// <summary>
        /// make gl for Catch and change state
        /// </summary>
        /// <param name="CVM"></param>
        /// <returns></returns>
        public static string UpdateInvoiceMaster(CatchReceiptVM CVM)
        {
            var msg = "";
            var db = new AccountingContainer();

            CatchReceipt CatchR = db.CatchReceipts.Find(CVM.CatchReceipt_Id);
            OperationRun.Addnewoperation(CatchR.CatchReceipt_Code);
            var operation = db.OperationRuns.Where(a => a.Code == CatchR.CatchReceipt_Code).Count();
            if (operation != 1)
            {
                OperationRun.DeleteOperation(CatchR.CatchReceipt_Code);
                return Resources.CatchReceiptDetails.WaitAndTryAgainModelMessage;
            }
            if (CatchR.CatchReceipt_State == CVM.CatchReceipt_State)
            {
                OperationRun.DeleteOperation(CatchR.CatchReceipt_Code);
                return Resources.CatchReceiptDetails.ItsBeenComitted + " " + CatchR.CatchReceipt_State + " " + Resources.CatchReceiptDetails.Before;
            }
            else
            {
                CatchR.CatchReceipt_State = CVM.CatchReceipt_State;
            }
            db.Entry(CatchR).State = EntityState.Modified;


            #region Create GL
            // Add GLHeader 
            GLMaster glmaster = new GLMaster();
            glmaster.GLCode = CatchR.CatchReceipt_Code;
            glmaster.GL_Date = CatchR.CatchReceipt_date;
            glmaster.GL_Desc = CatchR.CatchReceipt_Description;
            glmaster.Company_Id = (int)CatchR.Company_Id;
            glmaster.GLType_Id = "قبض";
         
            glmaster.Currency_Id = "ريال سعودي";
            glmaster.Auto = true;
           
            //add GL Detail
            GLDetail ChildCredit = new GLDetail();
            GLDetail Dep = new GLDetail();
            GLDetail bankcomution = new GLDetail();
            

            var thischild = db.ChartOfAccountings.Find(CatchR.AccountCredit_Id);

            
            if (thischild.Account_Code == "010109" || thischild.Account_Code == "020101" || thischild.Account_Code == "050201001")
            {


                ChildCredit.Account_Id = thischild.Account_Id;
                ChildCredit.Account_Name = thischild.Account_Name;
                var thisaccount = db.Accounts.Where(t => t.Accounts_Id == CatchR.Child_AccountCredit_Id && t.Account_Id == CatchR.AccountCredit_Id.ToString()).ToList().FirstOrDefault();
                if (thisaccount == null)
                { }
                else
                {
                    ChildCredit.Account_Child_ID = thisaccount.Accounts_Id;
                    ChildCredit.Account_CHild_Name = thisaccount.Account_Name;
                }
            }

            else
            {
                var thisaccount = db.Accounts.Where(t => t.Accounts_Id == CatchR.Child_AccountCredit_Id && t.Account_Id == CatchR.AccountCredit_Id.ToString()).ToList().FirstOrDefault();
                if (thisaccount == null)
                {
                    var thischaraccount = db.ChartOfAccountings.Find(CatchR.Child_AccountCredit_Id);
                    if (thischaraccount == null)
                    {
                        ChildCredit.Account_Id = thischild.Account_Id;
                        ChildCredit.Account_Name = thischild.Account_Name;
                    }
                    else
                    {
                        ChildCredit.Account_Id = thischaraccount.Account_Id;
                        ChildCredit.Account_Name = thischaraccount.Account_Name;
                    }

                }
                else
                {
                    var thischaraccount = db.ChartOfAccountings.Find(CatchR.AccountCredit_Id);
                    ChildCredit.Account_Id = thischaraccount.Account_Id;
                    ChildCredit.Account_Name = thischaraccount.Account_Name;
                    ChildCredit.Account_Child_ID = thisaccount.Accounts_Id;
                    ChildCredit.Account_CHild_Name = thisaccount.Account_Name;
                }

            }

            ChildCredit.Bu_Id = 0;
            Dep.Bu_Id = 0;
            ChildCredit.Asset_Id = 0;
            ChildCredit.SubLedger_Id = 0;
            var paymentMethod = db.PaymentMethods.Find(CatchR.AccountDebit_Id);
            var AccountOfPayment = db.ChartOfAccountings.Find(paymentMethod.Bank_Account_Id.Value);
            Dep.Account_Id = AccountOfPayment.Account_Id;
            Dep.Account_Name = AccountOfPayment.Account_Name;
            Dep.SubLedger_Id = 0;
            Dep.Asset_Id = 0;
       
            Dep.Account_Child_ID = 0;
            Dep.Account_CHild_Name = "";
            var bank = db.ChartOfAccountings.Find(paymentMethod.Expense_Account_Id);

            if (bank != null)
            {
                bankcomution.Account_Id = bank.Account_Id;
                bankcomution.Account_Name = bank.Account_Name;
                var branch = db.Branches.Find(CatchR.BranchSubledger_Id);
                if (branch == null)
                {
                    bankcomution.SubLedger_Id = 1;
                }
                else
                {
                    bankcomution.SubLedger_Id = db.SubLedgers.Where(a => a.SubLed_Name == branch.Branch_Name).FirstOrDefault().SubLed_Id;
                }

            }
            switch (CatchR.CatchReceipt_State)
            {
                //Post Invoice
                case TransactionState.Post:
                    glmaster.GLState = TransactionState.Post;
                    var gl = db.GLMasters.Where(a => a.GLCode == glmaster.GLCode && a.GLState == glmaster.GLState).FirstOrDefault();
                    if (gl!=null) {
                        OperationRun.DeleteOperation(CatchR.CatchReceipt_Code);
                        return Resources.CatchReceiptDetails.SuccesFullyCreatedAConstraint;
                    }
                    ChildCredit.GLDetail_Debit = 0;
                    ChildCredit.GLDetail_Credit = CatchR.CatchReceipt_Value;
                    Dep.GLDetail_Debit = CatchR.CatchReceipt_Value - ((CatchR.CatchReceipt_Value * (paymentMethod.Percentage.Value / 100)) + paymentMethod.Operatin_Cost.Value);
                    bankcomution.GLDetail_Debit = (CatchR.CatchReceipt_Value * (paymentMethod.Percentage.Value / 100)) + paymentMethod.Operatin_Cost.Value;
                    Dep.GLDetail_Credit = 0;

                    break;
                // Void Invoice
                case TransactionState.Void:
                    glmaster.GLType_Id = "قبض عكسي";
                    glmaster.GLState = TransactionState.Void;
                    var glcheck = db.GLMasters.Where(a => a.GLCode == glmaster.GLCode && a.GLState == glmaster.GLState).FirstOrDefault();
                    if (glcheck != null)
                    {
                        OperationRun.DeleteOperation(CatchR.CatchReceipt_Code);
                        return Resources.CatchReceiptDetails.SuccesFullyCreatedAConstraint;
                    }
                    if (CatchR.HasOff == true)

                    {
                        msg = Resources.CatchReceiptDetails.ClosedConstraintAlreadyContains;
                        CatchR.CatchReceipt_State = TransactionState.Post;
                        db.SaveChanges();

                    }
                    else
                    {
                        ChildCredit.GLDetail_Debit = CatchR.CatchReceipt_Value;
                        Dep.GLDetail_Credit = CatchR.CatchReceipt_Value - ((CatchR.CatchReceipt_Value * (paymentMethod.Percentage.Value / 100)) + paymentMethod.Operatin_Cost.Value);
                        bankcomution.GLDetail_Credit = (CatchR.CatchReceipt_Value * (paymentMethod.Percentage.Value / 100)) + paymentMethod.Operatin_Cost.Value;
                        Dep.GLDetail_Debit = 0;
                    }
                    
                    break;
                default:
                    break;
            }
            if (msg == "")
            {
                if (bankcomution.GLDetail_Credit == 0 && bankcomution.GLDetail_Debit == 0)
                {

                }
                else
                {
                    glmaster.GLDetails.Add(bankcomution);

                }
                // Add all GL details to the master
                glmaster.GLDetails.Add(ChildCredit);
                glmaster.GLDetails.Add(Dep);
                #endregion

                db.GLMasters.Add(glmaster);

                GLMaster.GetBalanceForAccount(glmaster);
                GLMaster.GetBalanceForSubLedger(glmaster);
                db.SaveChanges();
                msg = Resources.CatchReceiptDetails.SuccesFullyCreatedAConstraint;

            }
            OperationRun.DeleteOperation(CatchR.CatchReceipt_Code);
            if (CatchR.CatchReceipt_State == TransactionState.Post)
            {
                msg = Resources.validate.GLDone;
            }
            else
            {
                msg = Resources.validate.GLReverse;
            }
            return msg;
        }
        /// <summary>
        /// fill VM with data that saved in db
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static CatchReceiptVM EditForCreation(int id)
        {
            var db = new AccountingContainer();
            var thisCatchr = db.CatchReceipts.Find(id);
            var catchVM = new CatchReceiptVM();
            catchVM.CatchReceipt_Id = thisCatchr.CatchReceipt_Id;
            catchVM.AccountCredit_Id = thisCatchr.AccountCredit_Id;

            catchVM.CatchReceipt_Value = thisCatchr.CatchReceipt_Value;
            catchVM.CatchReceipt_Description = thisCatchr.CatchReceipt_Description;
            catchVM.CatchReceipt_date = thisCatchr.CatchReceipt_date.Day + "-" + thisCatchr.CatchReceipt_date.Month + "-" + thisCatchr.CatchReceipt_date.Year;
            catchVM.Company_Id = thisCatchr.Company_Id;
            catchVM.Currency_Id = thisCatchr.Currency_Id;
            catchVM.Reference = thisCatchr.Reference;
            catchVM.CatchReceipt_Code = thisCatchr.CatchReceipt_Code;
            catchVM.BranchSubledger_Id = thisCatchr.BranchSubledger_Id;           
            catchVM.SubChild_AccountCredit_Id = thisCatchr.Child_AccountCredit_Id;
            catchVM.AccountDebit_Id = thisCatchr.AccountDebit_Id;
           
            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0&&a.isActive!=false);
            var newList = from pay in db.PaymentMethods.Where(a=>a.IsActive!=false)
                          select new
                          {

                              Id = pay.PaymentMethod_Id,
                              Name = pay.Payment_Type

                          };


            catchVM.AccCreditList = new SelectList(lista, "Account_Id", "Account_Name");
            if (catchVM.AccountCredit_Id == 45 || catchVM.AccountCredit_Id == 39 || catchVM.AccountCredit_Id == 37)
            {
                catchVM.SubChildList = new SelectList(db.Accounts.Where(p => p.Account_Id == catchVM.AccountCredit_Id.ToString()), "Accounts_Id", "Account_Name");

            }
            else
            {
                var List = new List<ChartOfAccounting>();
                catchVM.SubChildList = new SelectList(List, "Accounts_Id", "Account_Name");

            }
            //catchVM.SubChildList = new SelectList(db.ChartOfAccountings, "Account_Id", "Account_Name");
            //catchVM.AccCreditList = new SelectList(db.ChartOfAccountings.Where(t => t.Account_Level == 2), "Account_Id", "Account_Name");


            catchVM.ComList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            catchVM.BranchList = new SelectList(db.Branches.Where(a=>a.IsActive!=false), "Branch_Id", "Branch_Name");
            catchVM.CurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");
       
            catchVM.AccDebitList = new SelectList(newList, "Id", "Name");
            return catchVM;

        }/// <summary>
         /// save change that happen in database
         /// </summary>
         /// <param name="CVM"></param>
         /// <param name="UserId"></param>
         /// <returns></returns>
        public static string EditCatchRecipt(CatchReceiptVM CVM, String UserId)
        {
            var db = new AccountingContainer();
            var newCatch = db.CatchReceipts.Find(CVM.CatchReceipt_Id);
            //  newCatch.AccountCredit_Id = CVM.AccountCredit_Id;
            newCatch.AccountDebit_Id = CVM.AccountDebit_Id;
           
            newCatch.CatchReceipt_Value = CVM.CatchReceipt_Value;
            newCatch.CatchReceipt_Description = CVM.CatchReceipt_Description;
            newCatch.CatchReceipt_date = Convert.ToDateTime(CVM.CatchReceipt_date);            
            newCatch.Currency_Id = CVM.Currency_Id;
            newCatch.CatchReceipt_State = TransactionState.Pending;
            newCatch.Reference = CVM.Reference;
           
            newCatch.BranchSubledger_Id = CVM.BranchSubledger_Id;
            // newCatch.Child_AccountCredit_Id = CVM.Child_AccountCredit_Id;
            //newCatch.CatchReceipt_Code = CVM.CatchReceipt_Code;
            //if (CVM.SubChild_AccountCredit_Id == 1 && CVM.Child_AccountCredit_Id != 1)
            //{
            //    newCatch.Child_AccountCredit_Id = CVM.Child_AccountCredit_Id;
            //    newCatch.AccountCredit_Id = CVM.AccountCredit_Id;
            //}
            //else if (CVM.Child_AccountCredit_Id == 1)
            //{
            //    newCatch.Child_AccountCredit_Id = 0;
            //    newCatch.AccountCredit_Id = (int)CVM.AccountCredit_Id;
            //}
            //else
            //{
            //    newCatch.Child_AccountCredit_Id = CVM.SubChild_AccountCredit_Id;
            //    newCatch.AccountCredit_Id = (int)CVM.Child_AccountCredit_Id;
            //}
            if (CVM.Files.FirstOrDefault() != null)
            {
                foreach (var file in CVM.Files)
                {
                    var filedetails = new CatchReceiptFileDetail();
                    var data = new byte[file.ContentLength];
                    filedetails.FileName = file.FileName;
                    filedetails.ContentLength = file.ContentLength;
                    file.InputStream.Read(data, 0, file.ContentLength);
                    filedetails.ProImg = data;
                    filedetails.File_Extention = file.ContentType;
                   
                    if (data.Length < 1000000)
                    {
                        newCatch.CatchReceiptFileDetails.Add(filedetails);
                    }

                }
                db.SaveChanges();
            }
            newCatch.AccountCredit_Id = CVM.AccountCredit_Id;
            if (CVM.SubChild_AccountCredit_Id != null)
            {
                newCatch.Child_AccountCredit_Id = CVM.SubChild_AccountCredit_Id;
               
            }            
            else
            {
               
                newCatch.Child_AccountCredit_Id = null;
            }
            db.Entry(newCatch).State = EntityState.Modified;
            db.SaveChanges();
            var traBatch = new TransactionBatch();

            traBatch.OperationName = "تعديل سند القبض";
            traBatch.Description = newCatch.CatchReceipt_Description;
            traBatch.Operation_Code = newCatch.CatchReceipt_Code;
            traBatch.Date = Convert.ToDateTime(CVM.CatchReceipt_date);
            traBatch.UserId = UserId;
            

            traBatch.OperationId = newCatch.CatchReceipt_Id;
            TransactionBatch.AddNewTransaction(traBatch);
            db.Dispose();
            return Resources.validate.Editsucc;

        }
        /// <summary>
        /// make fillteration from drobdowList 
        /// </summary>
        /// <param name="CreditID"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> getCreditAccountCHild(int CreditID)
        {
            var db = new AccountingContainer();
            IEnumerable<SelectListItem> thisChart = new SelectList(db.ChartOfAccountings.Where(p => p.Account_Level == 3 && p.ChartOfAccountingAccount_Id == CreditID), "Account_Id", "Account_Name");
            return thisChart;
        }
        /// <summary>
        /// make fillteration from drobdowList 
        /// </summary>
        /// <param name="CreditID"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> getCreditAccountSubCHild(int ChildID)
        {
            var db = new AccountingContainer();
            var thischars = db.ChartOfAccountings.Find(ChildID);
            if (thischars.Account_Code == "010109" || thischars.Account_Code == "020101" || thischars.Account_Code == "010111")
            {

                IEnumerable<SelectListItem> AccountLedger = new SelectList(db.Accounts.Where(p => p.Account_Type == thischars.Account_Name && p.OpenBalance_Code != "1").ToList(), "Accounts_Id", "Account_Name");
                return AccountLedger;
            }
            else
            {
                var List = new List<ChartOfAccounting>();
            
                return new SelectList(List, "Accounts_Id", "Account_Name"); 
            }


        }
        /// <summary>
        /// delete Catch Recipt
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public static String DeleteCatch(int invoiceId)
        {
            var db = new AccountingContainer();
            CatchReceipt invoice = db.CatchReceipts.Find(invoiceId);

            if (invoice.CatchReceipt_State != TransactionState.Pending)
                return Resources.validate.Error;
            db.CatchReceiptFileDetails.RemoveRange(invoice.CatchReceiptFileDetails);

            db.CatchReceipts.Remove(invoice);

            db.SaveChanges();
            db.Dispose();
            return Resources.validate.DeleteSuccessful;
        }
        public static String deletionactiveGL(int id)
        {
            var db = new AccountingContainer();
            var doc = db.CatchReceipts.Find(id);
            if (doc.CatchReceipt_State != TransactionState.Pending && doc.HasOff != true)
            {
                var gl = db.GLMasters.Where(a => a.GLCode == doc.CatchReceipt_Code);
                foreach (var g in gl)
                {
                    db.GLDetails.RemoveRange(g.GLDetails);

                    db.GLMasters.Remove(g);

                }
                db.CatchReceiptFileDetails.RemoveRange(doc.CatchReceiptFileDetails);
                db.CatchReceipts.Remove(doc);
                db.SaveChanges();



                db.Dispose();
                return Resources.validate.DeleteSuccessful;
            }
            return Resources.validate.Error;
        }

    }
}