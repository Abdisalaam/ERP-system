using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class SpentRecipt
    {/// <summary>
     /// check that code is valued or not
     /// </summary>
     /// <param name="Serial"></param>
     /// <returns>bool</returns>
        public static bool containCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.CatchReceipts.ToList();
            if (!Serial.StartsWith("SPRE-"))
            {
                return true;
            }
            else
            {
                foreach (var item in allinData)
                {
                    if (item.CatchReceipt_Code == Serial || !Serial.StartsWith("SPRE-"))
                    {
                        return true;
                    }
                    else
                    {

                    }

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
        /// get All spent Recipt fron db
        /// </summary>
        /// <returns> List<SpentRecipt> </returns>
        public static List<SpentRecipt> GetAllSpentREceipt()
        {
            var db = new AccountingContainer();
            var Receipts = db.SpentRecipts.ToList();
            return Receipts;
        }
        /// <summary>
        /// get spent Recipt data by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SpentRecipt GetSpentReceiptById(int id)
        {
            var db = new AccountingContainer();
            var Receipt = db.SpentRecipts.Find(id);
            return Receipt;
        }
        /// <summary>
        /// get file by its id
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns>SpentReciptFileDetail</returns>
        public static SpentReciptFileDetail getFileById(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.SpentReciptFileDetails.Find(fileId);
            return file;
        }
        /// <summary>
        /// get all File of SpentRecipt by its id
        /// </summary>
        /// <param name="spntReceId"></param>
        /// <returns></returns>
        public static List<SpentReciptFileDetail> GetAllFiles(int spntReceId)
        {
            var db = new AccountingContainer();
            var files = db.SpentReciptFileDetails.Where(exp => exp.CatchReceipt_Id == spntReceId).ToList();
            return files;
        }
        /// <summary>
        /// Fill For Creation of SpentRecipt
        /// </summary>
        /// <returns>SpentReciptVM</returns>
        public static SpentReciptVM AddForCreation(int? child, string type,int? spentOrderId)
        {
            var db = new AccountingContainer();
            var spentReceiptVM = new SpentReciptVM();

            var lastSerial = db.SpentRecipts.OrderByDescending(c => c.SpentReceipt_Id).FirstOrDefault();
            if (lastSerial == null)
            {
                spentReceiptVM.SpentReceipt_Code = "SPRE-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                spentReceiptVM.SpentReceipt_Code = "SPRE-" + (Convert.ToInt32(lastSerial.SpentReceipt_Code.Substring(5, lastSerial.SpentReceipt_Code.Length - 5)) + 1).ToString("D3");
            }
            //var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0 && a.isActive != false);
            var lista = db.ChartOfAccountings.Where(a => a.isActive != false);
            spentReceiptVM.SpentReceipt_date = DateTime.Now.ToString("dd-MM-yyyy   HH:mm:ss"); 
            spentReceiptVM.AccCreditList = new SelectList(db.ChartOfAccountings.Where(e => e.Account_Code.StartsWith("010120") && e.Account_Level == 4 && e.isActive != false), "Account_Id", "Account_Name");
            spentReceiptVM.AccDebitList = new SelectList(lista, "Account_Id", "Account_Name");
            spentReceiptVM.ParentList = new SelectList(db.ChartOfAccountings.Where(e => e.Account_Level == 2 && e.isActive != false), "Account_Id", "Account_Name");
            var ac = new List<Accounts> { }; 
            spentReceiptVM.ChildList = new SelectList(ac, "Accounts_Id", "Account_Name");
            spentReceiptVM.ComList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            spentReceiptVM.BranchList = new SelectList(db.Branches.Where(a=>a.IsActive!=false), "Branch_Id", "Branch_Name");
            spentReceiptVM.CurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");
            spentReceiptVM.AccsubCreditList = new SelectList(db.ChartOfAccountings.Where(e =>e.Account_Level == 5 && e.isActive != false), "Account_Id", "Account_Name");
            if (type != "")
            {
                if (type == "supplier")
                {
                    spentReceiptVM.AccountDebit_Id = 45;
                    spentReceiptVM.ChildList = new SelectList(db.Accounts.Where(p => p.Account_Id == spentReceiptVM.AccountDebit_Id.ToString()), "Accounts_Id", "Account_Name");
                    var supp = db.Suppliers.Find(child).Supp_Code;
                    spentReceiptVM.Child_AccountDepit_Id = db.Accounts.Where(a => a.Account_Code == supp).FirstOrDefault().Accounts_Id;
                }



            }
            if (spentOrderId > 0)
            {
                var spentOrder = db.SpentReciptOrders.Find(spentOrderId);
                spentReceiptVM.AccountDebit_Id = 39;
                spentReceiptVM.SpentReceipt_Value = spentOrder.Value;
                spentReceiptVM.ChildList = new SelectList(db.Accounts.Where(p => p.Account_Id == spentReceiptVM.AccountDebit_Id.ToString()), "Accounts_Id", "Account_Name");
                var emp = db.Employees.Find(child).Emp_Code;
                spentReceiptVM.Child_AccountDepit_Id = db.Accounts.Where(a => a.Account_Code == emp).FirstOrDefault().Accounts_Id;
                spentReceiptVM.SpentReceiptOrderId = spentOrderId.Value;
            }

            return spentReceiptVM;

        }
        /// <summary>
        /// Create new spentRecipt and save it in data base
        /// </summary>
        /// <param name="SVM"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static int AddSpentRecipt(SpentReciptVM SVM , String UserId)
        {
            var db = new AccountingContainer();
            var newSpent = new SpentRecipt();
            if (SVM.AccountsubCredit_Id == null || SVM.AccountsubCredit_Id == 0)
            {
                newSpent.AccountCredit_Id = SVM.AccountCredit_Id;
            }
            else
            { newSpent.AccountCredit_Id =(int) SVM.AccountsubCredit_Id; }
            


            //if (SVM.Parent_AccountDepit_Id != null && SVM.AccountDebit_Id == 0 && SVM.Child_AccountDepit_Id == 0)
            //{
            //    newSpent.AccountDebit_Id = SVM.Parent_AccountDepit_Id.Value;
            //}
            //else if (SVM.Parent_AccountDepit_Id != null && SVM.AccountDebit_Id != 0 && SVM.Child_AccountDepit_Id == null)
            //{
            //    newSpent.AccountDebit_Id = SVM.Parent_AccountDepit_Id.Value;
            //    newSpent.Child_AccountDebit_Id = SVM.AccountDebit_Id;
            //}
            //else if (SVM.Parent_AccountDepit_Id != null && SVM.AccountDebit_Id != 0 && SVM.Child_AccountDepit_Id != 0)
            //{
            //    newSpent.AccountDebit_Id = (int)SVM.AccountDebit_Id;
            //    newSpent.Child_AccountDebit_Id = SVM.Child_AccountDepit_Id;
            //}


            //if (SVM.AccountDebit_Id == null || SVM.AccountDebit_Id == 0)
            //{
            //    newSpent.AccountDebit_Id = (int)SVM.Parent_AccountDepit_Id;
            //    newSpent.Child_AccountDebit_Id = 0;
            //}
            //else if ((SVM.AccountDebit_Id != null && SVM.AccountDebit_Id != 0) && (SVM.Child_AccountDepit_Id == 0 || SVM.Child_AccountDepit_Id == null))
            //{
            //    newSpent.AccountDebit_Id = (int)SVM.Parent_AccountDepit_Id;
            //    newSpent.Child_AccountDebit_Id = SVM.AccountDebit_Id;
            //}
            //else if ((SVM.AccountDebit_Id != null && SVM.AccountDebit_Id != 0) && (SVM.Child_AccountDepit_Id != 0 && SVM.Child_AccountDepit_Id != null))
            //{

            //    newSpent.AccountDebit_Id = (int)SVM.AccountDebit_Id;
            //    newSpent.Child_AccountDebit_Id = SVM.Child_AccountDepit_Id;
            //}
            if (SVM.Child_AccountDepit_Id != null)
            {
                newSpent.Child_AccountDebit_Id = SVM.Child_AccountDepit_Id;
                newSpent.AccountDebit_Id = SVM.AccountDebit_Id.Value;
            }
            else if (SVM.AccountDebit_Id != null)
            {
                newSpent.AccountDebit_Id = SVM.AccountDebit_Id.Value;
                newSpent.Child_AccountDebit_Id = null;

            }
            else
            {
                newSpent.AccountDebit_Id = SVM.Parent_AccountDepit_Id.Value;
                newSpent.Child_AccountDebit_Id = null;
            }

            var lastSerial = db.SpentRecipts.OrderByDescending(c => c.SpentReceipt_Id).FirstOrDefault();
            if (lastSerial == null)
            {
                newSpent.SpentReceipt_Code = "SPRE-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                newSpent.SpentReceipt_Code = "SPRE-" + (Convert.ToInt32(lastSerial.SpentReceipt_Code.Substring(5, lastSerial.SpentReceipt_Code.Length - 5)) + 1).ToString("D3");
            }

            newSpent.SpentReceipt_Value = SVM.SpentReceipt_Value;
            newSpent.SpentReceipt_Description = SVM.SpentReceipt_Description;
            var comp = db.Companies.FirstOrDefault();
            if (comp != null)
            {
                var date = Convert.ToDateTime(SVM.SpentReceipt_date);
                if (date < comp.StartFinancialDate)
                {
                    newSpent.CatchReceipt_date = DateTime.Now;
                }
                else
                {

                    newSpent.CatchReceipt_date = Convert.ToDateTime(SVM.SpentReceipt_date);
                }
            }
            else
            {
                newSpent.CatchReceipt_date = DateTime.Now;
            }

            newSpent.Company_Id = SVM.Company_Id;
            newSpent.Currency_Id = SVM.Currency_Id;
            newSpent.SpentReceipt_State = TransactionState.Pending;
            newSpent.Reference = SVM.Reference;
            newSpent.BranchSubledger_Id = SVM.BranchSubledger_Id;
      
            if (SVM.Files.FirstOrDefault() != null)
            {
                foreach (var file in SVM.Files)
                {
                    var filedetails = new SpentReciptFileDetail();
                    var data = new byte[file.ContentLength];
                    filedetails.FileName = file.FileName;
                    filedetails.ContentLength = file.ContentLength;
                    file.InputStream.Read(data, 0, file.ContentLength);
                    filedetails.ProImg = data;
                    filedetails.File_Extention = file.ContentType;
                    if (data.Length < 1000000)
                    {
                        newSpent.SpentReciptFileDetails.Add(filedetails);
                    }
                    
                }
            }

            if (SVM.SpentReceiptOrderId > 0)
            {
                newSpent.SpentReciptOrderId = SVM.SpentReceiptOrderId;
                var spent = db.SpentReciptOrders.Find(newSpent.SpentReciptOrderId);
                spent.SpentReciptOrderTypeId = 4;
                db.Entry(spent).State = EntityState.Modified;

            }
            db.SpentRecipts.Add(newSpent);
            db.SaveChanges();
            var traBatch = new TransactionBatch();

            traBatch.OperationName = "سند صرف";
            traBatch.Description = SVM.SpentReceipt_Description;
            traBatch.Date = Convert.ToDateTime(SVM.SpentReceipt_date);
            traBatch.UserId = UserId;
            traBatch.OperationId = newSpent.SpentReceipt_Id;
            traBatch.Operation_Code = newSpent.SpentReceipt_Code;
            TransactionBatch.AddNewTransaction(traBatch);
            db.Dispose();
            return newSpent.SpentReceipt_Id;

        }
        /// <summary>
        /// check that status and make gl of spent recipt
        /// </summary>
        /// <param name="CVM"></param>
        /// <returns></returns>
        public static string UpdatespentRecipt(SpentReciptVM CVM)
        {
            var msg = "";
            var db = new AccountingContainer();

            SpentRecipt spentR = db.SpentRecipts.Find(CVM.SpentReceipt_Id);
            OperationRun.Addnewoperation(spentR.SpentReceipt_Code);
            var operation = db.OperationRuns.Where(a => a.Code == spentR.SpentReceipt_Code).Count();
            if (operation != 1)
            {
                OperationRun.DeleteOperation(spentR.SpentReceipt_Code);
                return Resources.InvoiceMasterDetails.WaitingMSGAndTryAgain;
            }
            if (spentR.SpentReceipt_State == CVM.SpentReceipt_State)
            {
                OperationRun.DeleteOperation(spentR.SpentReceipt_Code);
             
                return Resources.InvoiceMasterDetails.ItsDoneWorking + " " + spentR.SpentReceipt_State + " " + Resources.InvoiceMasterDetails.Before;
            }
            else
            {
                spentR.SpentReceipt_State = CVM.SpentReceipt_State;
            }

            db.Entry(spentR).State = EntityState.Modified;


            #region Create GL
            // Add GLHeader 
            GLMaster glmaster = new GLMaster();
            glmaster.GLCode = spentR.SpentReceipt_Code;
            glmaster.GL_Date = spentR.CatchReceipt_date;
            glmaster.GL_Desc = spentR.SpentReceipt_Description;
            glmaster.Company_Id = (int)spentR.Company_Id;
            glmaster.GLType_Id = "سند صرف";
            var thiscur = db.Currencies.Find(spentR.Currency_Id);
            glmaster.Currency_Id = thiscur.Currency_Type;
            glmaster.Auto = true;
            // glmaster.GL_State = TransactionState.Pending;
            //add GL Detail
            GLDetail ChildDebit = new GLDetail();
            GLDetail Dep = new GLDetail();


         
            var thischilds = db.Accounts.Find(spentR.Child_AccountDebit_Id);

            var thischild = db.ChartOfAccountings.Find(spentR.AccountDebit_Id);

         
            if (thischild.Account_Code == "010109" || thischild.Account_Code == "020101" || thischild.Account_Code == "050201001")
            {


                ChildDebit.Account_Id = thischild.Account_Id;
                ChildDebit.Account_Name = thischild.Account_Name;
               
                    var thisaccount = db.Accounts.Where(t => t.Accounts_Id == spentR.Child_AccountDebit_Id && t.Account_Id == spentR.AccountDebit_Id.ToString()).ToList().FirstOrDefault();
                if (thisaccount != null)
                {
                    ChildDebit.Account_Child_ID = thisaccount.Accounts_Id;
                    ChildDebit.Account_CHild_Name = thisaccount.Account_Name;
                }
            }
            else
            {
                var thisaccount = db.Accounts.Where(t => t.Accounts_Id == spentR.Child_AccountDebit_Id && t.Account_Id == spentR.AccountDebit_Id.ToString()).ToList().FirstOrDefault();
                if (thisaccount == null)
                {
                    var thischaraccount = db.ChartOfAccountings.Find(spentR.Child_AccountDebit_Id);
                    if (thischaraccount == null)
                    {
                        ChildDebit.Account_Id = thischild.Account_Id;
                        ChildDebit.Account_Name = thischild.Account_Name;
                    }
                    else
                    {
                        ChildDebit.Account_Id = thischaraccount.Account_Id;
                        ChildDebit.Account_Name = thischaraccount.Account_Name;
                    }

                }
                else
                {
                    var thischaraccount = db.ChartOfAccountings.Find(spentR.AccountDebit_Id);
                    ChildDebit.Account_Id = thischaraccount.Account_Id;
                    ChildDebit.Account_Name = thischaraccount.Account_Name;
                    ChildDebit.Account_Child_ID = thisaccount.Accounts_Id;
                    ChildDebit.Account_CHild_Name = thisaccount.Account_Name;
                }

            }





            var chartofdep = db.ChartOfAccountings.Find(spentR.AccountCredit_Id);
            Dep.Account_Id = chartofdep.Account_Id;
            Dep.Account_Name = chartofdep.Account_Name;
            Dep.SubLedger_Id = 0;
            Dep.Asset_Id = 0;
            //Dep.Description_Detail = "سند صرف";
            Dep.Account_Child_ID = 0;
            Dep.Account_CHild_Name = "";
            switch (spentR.SpentReceipt_State)
            {
                //Post Invoice
                case TransactionState.Post:
                    glmaster.GLState = TransactionState.Post;
                    var gl = db.GLMasters.Where(a => a.GLCode == glmaster.GLCode && a.GLState == glmaster.GLState).FirstOrDefault();
                    if (gl != null)
                    {
                        OperationRun.DeleteOperation(spentR.SpentReceipt_Code);
                        return Resources.InvoiceMasterDetails.RecordHasBeenSuccessfullyMade;
                    }

                    ChildDebit.GLDetail_Debit = spentR.SpentReceipt_Value;
                    ChildDebit.GLDetail_Credit = 0;
                    Dep.GLDetail_Debit = 0;
                    Dep.GLDetail_Credit = spentR.SpentReceipt_Value;

                    break;
                // Void Invoice
                case TransactionState.Void:
                    glmaster.GLType_Id = "عكس  سند صرف";
                    glmaster.GLState = TransactionState.Void;
                    var glChck = db.GLMasters.Where(a => a.GLCode == glmaster.GLCode && a.GLState == glmaster.GLState).FirstOrDefault();
                    if (glChck != null)
                    {
                        OperationRun.DeleteOperation(spentR.SpentReceipt_Code);
                        return Resources.InvoiceMasterDetails.RecordHasBeenSuccessfullyMade;
                    }
                    if (spentR.HasOff == true)

                    {

                        msg = Resources.InvoiceMasterDetails.InvoiceHasBeenIncluded + " ";
                        spentR.SpentReceipt_State = TransactionState.Post;
                        db.SaveChanges();
                        OperationRun.DeleteOperation(spentR.SpentReceipt_Code);
                        return msg;

                    }
                    else
                    {
                        ChildDebit.GLDetail_Debit = 0;
                        ChildDebit.GLDetail_Credit = spentR.SpentReceipt_Value;
                        Dep.GLDetail_Debit = spentR.SpentReceipt_Value;
                        Dep.GLDetail_Credit = 0;

                    }
                    break;
                default:
                    break;
            }
            if (msg == "")
            {
                // Add all GL details to the master
                glmaster.GLDetails.Add(ChildDebit);
                glmaster.GLDetails.Add(Dep);
                #endregion

                db.GLMasters.Add(glmaster);

                GLMaster.GetBalanceForAccount(glmaster);
                GLMaster.GetBalanceForSubLedger(glmaster);
                msg = Resources.InvoiceMasterDetails.RecordHasBeenSuccessfullyMade;

                db.SaveChanges();
            }
            OperationRun.DeleteOperation(spentR.SpentReceipt_Code);
            if (spentR.SpentReceipt_State == TransactionState.Post)
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
        /// prepare for Editing spent Recipt by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SpentReciptVM EditForCreation(int id)
        {
            var db = new AccountingContainer();
            var thisCatchr = db.SpentRecipts.Find(id);
            var catchVM = new SpentReciptVM();
            var AccountCredit = thisCatchr.AccountCredit_Id;
            var AccountDebit = thisCatchr.AccountDebit_Id;
            catchVM.SpentReceipt_Id = thisCatchr.SpentReceipt_Id;
            

            





            catchVM.SpentReceipt_Value = thisCatchr.SpentReceipt_Value;
            catchVM.SpentReceipt_Description = thisCatchr.SpentReceipt_Description;
            catchVM.SpentReceipt_date = thisCatchr.CatchReceipt_date.ToString("dd-MM-yyyy   HH:mm:ss"); ;
            catchVM.Company_Id = thisCatchr.Company_Id;
            catchVM.Currency_Id = thisCatchr.Currency_Id;
            catchVM.Reference = thisCatchr.Reference;
            catchVM.SpentReceipt_Code = thisCatchr.SpentReceipt_Code;
            catchVM.BranchSubledger_Id = thisCatchr.BranchSubledger_Id;
          //  catchVM.Child_AccountDepit_Id = thisCatchr.Child_AccountDebit_Id;
            
             if ( (thisCatchr.AccountDebit_Id == 39))
            {
                var thischart = db.ChartOfAccountings.Find(39);
                catchVM.Parent_AccountDepit_Id = (int)thischart.ChartOfAccountingAccount_Id;

                catchVM.AccountDebit_Id = thischart.Account_Id;
                catchVM.Child_AccountDepit_Id = thisCatchr.Child_AccountDebit_Id;
                catchVM.ChildList = new SelectList(db.Accounts.Where(a => a.Account_Code.StartsWith("EMP")), "Accounts_Id", "Account_Name");
                //  GLMaster.GetChildOfParent(thisCatchr.AccountDebit_Id);
                // GLMaster.GetAccountLedger(thischart.Account_Id);
            }
            else if ( (thisCatchr.AccountDebit_Id == 45))
            {
                var thischart = db.ChartOfAccountings.Find(45);
                catchVM.Parent_AccountDepit_Id = (int)thischart.ChartOfAccountingAccount_Id;
                catchVM.AccountDebit_Id = thischart.Account_Id;
                catchVM.Child_AccountDepit_Id = thisCatchr.Child_AccountDebit_Id;
                catchVM.ChildList = new SelectList(db.Accounts.Where(a => a.Account_Code.StartsWith("SUP")), "Accounts_Id", "Account_Name");
                // GLMaster.GetChildOfParent(thisCatchr.AccountDebit_Id);
                //  GLMaster.GetAccountLedger(thischart.Account_Id);
            }
            else if ( (thisCatchr.AccountDebit_Id == 111))
            {
                var thischart = db.ChartOfAccountings.Find(111);
                catchVM.Parent_AccountDepit_Id = (int)thischart.ChartOfAccountingAccount_Id;
                catchVM.AccountDebit_Id = thischart.Account_Id;
                catchVM.Child_AccountDepit_Id = thisCatchr.Child_AccountDebit_Id;
                catchVM.ChildList = new SelectList(db.Accounts.Where(a => a.Account_Code.StartsWith("EMP")), "Accounts_Id", "Account_Name");
                // GLMaster.GetChildOfParent(thisCatchr.AccountDebit_Id);
                //  GLMaster.GetAccountLedger(thischart.Account_Id);
            }
            else if ((thisCatchr.AccountDebit_Id == 37))
            {
                var thischart = db.ChartOfAccountings.Find(37);
                catchVM.Parent_AccountDepit_Id = (int)thischart.ChartOfAccountingAccount_Id;
                catchVM.AccountDebit_Id = thischart.Account_Id;
                catchVM.Child_AccountDepit_Id = thisCatchr.Child_AccountDebit_Id;
                catchVM.ChildList = new SelectList(db.Accounts.Where(a=>a.Account_Code.StartsWith("Cus")), "Accounts_Id", "Account_Name");
                // GLMaster.GetChildOfParent(thisCatchr.AccountDebit_Id);
                //  GLMaster.GetAccountLedger(thischart.Account_Id);
            }
            else if ((thisCatchr.AccountDebit_Id != 111 && thisCatchr.AccountDebit_Id != 45 && thisCatchr.AccountDebit_Id != 39 && thisCatchr.AccountDebit_Id != 37))
            {
                var thischart = db.ChartOfAccountings.Find(thisCatchr.AccountDebit_Id);
             
                catchVM.Parent_AccountDepit_Id = (int)thischart.ChartOfAccountingAccount_Id;

                catchVM.AccountDebit_Id = thischart.Account_Id;
                catchVM.Child_AccountDepit_Id = thisCatchr.Child_AccountDebit_Id;
                catchVM.ChildList = new SelectList(db.Accounts.Where(a=>a.Account_Name==""), "Accounts_Id", "Account_Name");



            }
            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0 && a.isActive != false);
            var Credit = db.ChartOfAccountings.Find(thisCatchr.AccountCredit_Id);
            if (Credit.Account_Level==4)
            {
                catchVM.AccountCredit_Id = thisCatchr.AccountCredit_Id;
                catchVM.AccsubCreditList = new SelectList(Credit.ChartOfAccountings.Where(a=>a.isActive!=false), "Account_Id", "Account_Name");

            }
            else
            {
                catchVM.AccountsubCredit_Id= thisCatchr.AccountCredit_Id;
                var Master = db.ChartOfAccountings.Find(catchVM.AccountsubCredit_Id).ChartOfAccounting1;
                catchVM.AccountCredit_Id = Master.Account_Id;
                catchVM.AccsubCreditList = new SelectList(Master.ChartOfAccountings.Where(a => a.isActive != false), "Account_Id", "Account_Name");
            }
            catchVM.AccCreditList = new SelectList(db.ChartOfAccountings.Where(e => e.Account_Code.StartsWith("010120") && e.Account_Level == 4 && e.isActive != false), "Account_Id", "Account_Name");
            catchVM.AccDebitList = new SelectList(lista, "Account_Id", "Account_Name");
            catchVM.ParentList = new SelectList(lista.Where(e => e.Account_Level == 2 && e.isActive != false), "Account_Id", "Account_Name");
            //catchVM.ChildList = new SelectList(db.Accounts, "Accounts_Id", "Account_Name");
            catchVM.ComList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            catchVM.BranchList = new SelectList(db.Branches.Where(a=>a.IsActive!=false), "Branch_Id", "Branch_Name");
            catchVM.CurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");
           
            return catchVM;

        }
        /// <summary>
        /// save change in db 
        /// </summary>
        /// <param name="CVM"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static int EditCatchRecipt(SpentReciptVM CVM , String UserId)
        {
            var db = new AccountingContainer();
            var newCatch = db.SpentRecipts.Find(CVM.SpentReceipt_Id);
            if (CVM.AccountsubCredit_Id == null || CVM.AccountsubCredit_Id == 0)
            {
                newCatch.AccountCredit_Id = CVM.AccountCredit_Id;
            }
            else
            { newCatch.AccountCredit_Id = (int)CVM.AccountsubCredit_Id; }
            // newCatch.AccountDebit_Id =(int) CVM.AccountDebit_Id;
            newCatch.SpentReceipt_Value = CVM.SpentReceipt_Value;
            newCatch.SpentReceipt_Description = CVM.SpentReceipt_Description;
         
            var comp = db.Companies.FirstOrDefault();
            if (comp != null)
            {
                var date = Convert.ToDateTime(CVM.SpentReceipt_date);
                if (date < comp.StartFinancialDate)
                {
                    newCatch.CatchReceipt_date = DateTime.Now;
                }
                else
                {

                    newCatch.CatchReceipt_date = Convert.ToDateTime(CVM.SpentReceipt_date);
                }
            }
            else
            {
                newCatch.CatchReceipt_date = DateTime.Now;
            }

            newCatch.Company_Id = CVM.Company_Id;
            newCatch.Currency_Id = CVM.Currency_Id;
            newCatch.SpentReceipt_State = TransactionState.Pending;
            newCatch.Reference = CVM.Reference;
            newCatch.SpentReceipt_Code = CVM.SpentReceipt_Code;
            newCatch.BranchSubledger_Id = CVM.BranchSubledger_Id;
          //  newCatch.Child_AccountDebit_Id = CVM.Child_AccountDepit_Id;
            newCatch.SpentReceipt_Code = CVM.SpentReceipt_Code;

            //if (CVM.AccountDebit_Id == null || CVM.AccountDebit_Id == 0)
            //{
            //    newCatch.AccountDebit_Id = (int)CVM.Parent_AccountDepit_Id;
            //    newCatch.Child_AccountDebit_Id = 0;
            //}
            //else if ((CVM.AccountDebit_Id != null && CVM.AccountDebit_Id != 0) && (CVM.Child_AccountDepit_Id == 0 || CVM.Child_AccountDepit_Id == null))
            //{
            //    newCatch.AccountDebit_Id = (int)CVM.Parent_AccountDepit_Id;
            //    newCatch.Child_AccountDebit_Id = CVM.AccountDebit_Id;
            //}
            //else if ((CVM.AccountDebit_Id != null && CVM.AccountDebit_Id != 0) && (CVM.Child_AccountDepit_Id != 0 && CVM.Child_AccountDepit_Id != null))
            //{

            //    newCatch.AccountDebit_Id = (int)CVM.AccountDebit_Id;
            //    newCatch.Child_AccountDebit_Id = CVM.Child_AccountDepit_Id;
            //}
            if (CVM.Child_AccountDepit_Id != null)
            {
                newCatch.Child_AccountDebit_Id = CVM.Child_AccountDepit_Id;
                newCatch.AccountDebit_Id = CVM.AccountDebit_Id.Value;
            }
            else if (CVM.AccountDebit_Id != null)
            {
                newCatch.AccountDebit_Id = CVM.AccountDebit_Id.Value;
                newCatch.Child_AccountDebit_Id = null;

            }
            else
            {
                newCatch.AccountDebit_Id = CVM.Parent_AccountDepit_Id.Value;
                newCatch.Child_AccountDebit_Id = null;
            }

            if (CVM.Files.FirstOrDefault() != null)
            {
                foreach (var file in CVM.Files)
                {
                    var filedetails = new SpentReciptFileDetail();
                    var data = new byte[file.ContentLength];
                    filedetails.FileName = file.FileName;
                    filedetails.ContentLength = file.ContentLength;
                    file.InputStream.Read(data, 0, file.ContentLength);
                    filedetails.ProImg = data;
                    filedetails.File_Extention = file.ContentType;
                    if (data.Length < 1000000)
                    {
                        newCatch.SpentReciptFileDetails.Add(filedetails);
                    }
                   
                }
                db.SaveChanges();
            }

            db.Entry(newCatch).State = EntityState.Modified;
            db.SaveChanges();
            var traBatch = new TransactionBatch();

            traBatch.OperationName = "تعديل سند صرف";
            traBatch.Description = CVM.SpentReceipt_Description;
            traBatch.Date = Convert.ToDateTime(CVM.SpentReceipt_date);
            traBatch.UserId = UserId;
            traBatch.Operation_Code = newCatch.SpentReceipt_Code;
            traBatch.OperationId = newCatch.SpentReceipt_Id;
            TransactionBatch.AddNewTransaction(traBatch);
            
            db.Dispose();
            return newCatch.SpentReceipt_Id;

        }
        /// <summary>
        /// delelte spent Recipt when its Pending
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns>String</returns>
        public static String DeleteSpent(int invoiceId)
        {
            var db = new AccountingContainer();
            SpentRecipt invoice = db.SpentRecipts.Find(invoiceId);

            if (invoice.SpentReceipt_State != TransactionState.Pending)
                return Resources.validate.Error;
            db.SpentReciptFileDetails.RemoveRange(invoice.SpentReciptFileDetails);

            db.SpentRecipts.Remove(invoice);

            db.SaveChanges();
            db.Dispose();
            return Resources.validate.DeleteSuccessful;
        }
        public static String deletionactiveGL(int id)
        {
            var db = new AccountingContainer();
            var doc = db.SpentRecipts.Find(id);
            if (doc.SpentReceipt_State != TransactionState.Pending && doc.HasOff != true)
            {
                var gl = db.GLMasters.Where(a => a.GLCode == doc.SpentReceipt_Code);
                foreach (var g in gl)
                {
                    db.GLDetails.RemoveRange(g.GLDetails);

                    db.GLMasters.Remove(g);

                }
                db.SpentReciptFileDetails.RemoveRange(doc.SpentReciptFileDetails);
                db.SpentRecipts.Remove(doc);
                db.SaveChanges();



                db.Dispose();
                return Resources.validate.DeleteSuccessful;
            }
            return Resources.validate.Error;
        }
    }
}