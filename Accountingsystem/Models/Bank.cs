using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Office.Interop.Excel;
using System.Web.Mvc;

using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Data.Entity.Validation;
using System.Data.Entity;


namespace Accountingsystem.Models
{

    public partial class Bank
    {
        // public static readonly string StoreParentAccountCode = "010109";

        public static List<Bank> GetAllBank()
        //u will get all bank in  a list .
        {
            var db = new AccountingContainer();
            var Banks = db.Banks.Where(a => a.IsActive != false).ToList();

            return Banks;
        }
        public static List<Bank> GetAllNotActive()
        {
            var db = new AccountingContainer();
            var Banks = db.Banks.Where(a => a.IsActive == false).ToList();
            return Banks;
        }
        public static Bank GetBankById(int id)
        //u will find bank by ID 
        {
            var db = new AccountingContainer();
            var Bank = db.Banks.Find(id);
            return Bank;
        }
        /// <summary>
        /// Prepare VM too Create Bank
        /// </summary>
        /// <returns></returns>
        public static BankVM AddBankVM()
        {
            var db = new AccountingContainer();
            var bankVM = new BankVM();

            bankVM.MyChartOfAccountList = new SelectList(db.ChartOfAccountings.Where(accCode => accCode.Account_Code == "010120002"&&accCode.isActive!=false), "Account_Id", "Account_Name");
            bankVM.CurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");
            bankVM.EmpList = new SelectList(db.Employees, "Emp_Id", "Emp_Name");
            return bankVM;
        }
        /// <summary>
        /// Post the data come from user and save in data base and make gl if the customer add value 
        /// </summary>
        /// <param name="bvm"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static Bank AddNewBank(BankVM bvm, String UserId)

        {
            var db = new AccountingContainer();
            var newbank = new Bank();

            try
            {
                ChartOfAccountingVM accountVM = new ChartOfAccountingVM();
                //accountVM.ChartOfAccountingAccount_Id = 90;
                accountVM.ChartOfAccountingAccount_Id = bvm.SelectedChartOfAccountId;
                accountVM.Account_Name = bvm.Bank_Name;
                accountVM.Account_Curenncy = bvm.CurrencyID;
                accountVM.Account_Balance = bvm.balance;

                newbank.Bank_Code = ChartOfAccounting.CreateAccount(accountVM);
                newbank.Emp_Id = bvm.Emp_Id;
                newbank.Bank_Name = bvm.Bank_Name;
                newbank.Bank_Address = bvm.Bank_Address;
                var cur = db.Currencies.Find(bvm.CurrencyID);
                newbank.Bank_Currency = cur.Currency_Type;
                newbank.Bank_Eban_Number = bvm.IbanNumber;
                newbank.Bank_Number = bvm.BankNumber;
                newbank.Balance_Amount = bvm.balance;
                if (bvm.balance == 0)
                {
                }
                else
                {
                    var GlMAster = new GLMaster();
                    var GlDetail1 = new GLDetail();
                    var GlDetail = new GLDetail();
                    GlMAster.GL_Date = DateTime.Now;
                    GlMAster.GLType_Id = "قيد افتتاحى";
                    GlMAster.GL_Desc = bvm.Bank_Name + "رصيد افتتاحي";
                    GlMAster.Currency_Id = bvm.CurrencyID.ToString();
                    GlMAster.Company_Id = 1;
                    // GlMAster.GLCode = bvm.Bank_Code;
                    GlMAster.Auto = true;
                    ///////
                    GlMAster.GLState = TransactionState.Post;
                    var chart = db.ChartOfAccountings.Find(bvm.SelectedChartOfAccountId);
                    var ChartOfwsite = db.ChartOfAccountings.Where(s => s.Account_Code == "0601" && s.Account_Level == 2 && s.ChartOfAccountingAccount_Id == 130).ToList().LastOrDefault();
                    GlDetail.Account_Id = ChartOfwsite.Account_Id;
                    GlDetail.Account_Name = ChartOfwsite.Account_Name;
                    GlDetail.Bu_Id = 0;
                    GlDetail.GLDetail_Credit = (decimal)bvm.balance;//داءن

                    GlDetail.GLDetail_Debit = 0;//مدين
                    GlDetail.SubLedger_Id = 0;
                    GlDetail.Asset_Id = 0;
                    GlMAster.GLDetails.Add(GlDetail);
                    /////////
                    GlDetail1.Account_Id = bvm.SelectedChartOfAccountId;
                    GlDetail1.Account_Name = chart.Account_Name;
                    GlDetail1.Bu_Id = 0;
                    GlDetail1.GLDetail_Credit = 0;//داءن

                    GlDetail1.GLDetail_Debit = (decimal)bvm.balance;//مدين
                    GlDetail1.SubLedger_Id = 0;
                    GlDetail1.Asset_Id = 0;
                    GlMAster.GLDetails.Add(GlDetail1);
                    db.GLMasters.Add(GlMAster);
                    GLMaster.GetBalanceForAccount(GlMAster);
                    db.SaveChanges();
                }

                var traBatch = new TransactionBatch();

                traBatch.OperationName = "انشاء بنك";
                traBatch.Description = null;
                traBatch.Date = DateTime.Now;
                traBatch.UserId = UserId;
                traBatch.Operation_Code = newbank.Bank_Code;

                TransactionBatch.AddNewTransaction(traBatch);
                db.Banks.Add(newbank);


                db.SaveChanges();
                traBatch.OperationId = newbank.Bank_ID;
                db.Dispose();
                return newbank;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                var data = ex.Data;
                var erorr = msg;

            }
            return newbank;
        }
        public static BankVM FillBank(int bankid)
        //find bank by id to edit in it by returning the view of the screen fill of bank's data for user 
        {
            var db = new AccountingContainer();
            var MyBank = db.Banks.Find(bankid);
            BankVM bankVM = new BankVM();
            var lista = new List<ChartOfAccounting> { };
            foreach (var k in db.ChartOfAccountings)
            {
                var parent = db.ChartOfAccountings.Where(a => a.ChartOfAccountingAccount_Id == k.Account_Id);
                var o = parent.FirstOrDefault();
                if (o != null)
                {
                    lista.Add(db.ChartOfAccountings.Find(k.Account_Id));

                }


            }
            var thiscurrency = db.Currencies.Where(e => e.Currency_Type == MyBank.Bank_Currency).FirstOrDefault();
            bankVM.EmpList = new SelectList(db.Employees, "Emp_Id", "Emp_Name");
            bankVM.CurrencyID = db.Currencies.FirstOrDefault().Currency_Id;
            bankVM.MyChartOfAccountList = new SelectList(lista.Where(accCode => accCode.Account_Code.Contains("010120002")&&accCode.isActive!=false), "Account_Id", "Account_Name");
            bankVM.CurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");
            bankVM.Bank_ID = MyBank.Bank_ID;
            bankVM.Bank_Name = MyBank.Bank_Name;
            bankVM.Bank_Address = MyBank.Bank_Address;
            bankVM.Bank_Code = MyBank.Bank_Code;
            bankVM.IbanNumber = MyBank.Bank_Eban_Number;
            bankVM.BankNumber = MyBank.Bank_Number;
            bankVM.Emp_Id = MyBank.Emp_Id;

            return bankVM;

        }
        public static Bank editbank(BankVM vm)
        //we made that function to edit in the DB of the bank after editing
        //we will go to the DB . we will get the bank by id .if bank does not exist it will return by null .
        //else it will get to bank view model & then it will be the oppsite of Adding bank . 
        {
            var db = new AccountingContainer();
            //MyDbModelContainer function already exist so to the entity container is a logical grouping of entity sets, 
            //association sets, and function imports.
            Bank bank = db.Banks.Find(vm.Bank_ID);
            if (bank != null)
            {
                var resultaccount = db.ChartOfAccountings.Find(vm.SelectedChartOfAccountId);

                var cur = db.Currencies.FirstOrDefault();
                bank.Bank_Currency = cur.Currency_Type;
                bank.Bank_Eban_Number = vm.IbanNumber;
                bank.Bank_Number = vm.BankNumber;
                bank.Bank_Code = vm.Bank_Code;
                bank.Emp_Id = vm.Emp_Id;
                bank.Bank_Name = vm.Bank_Name;
                bank.Bank_Address = vm.Bank_Address;
                var account = db.ChartOfAccountings.Where(a => a.Account_Code == bank.Bank_Code).FirstOrDefault();
                var payment = db.PaymentMethods.Where(a => a.Bank_Account_Id == account.Account_Id).FirstOrDefault();

                payment.Payment_Type = vm.Bank_Name;
                account.Account_Name = vm.Bank_Name;
                var thiscurrency = db.Currencies.Find(vm.CurrencyID);
                account.Currency_Name = thiscurrency.Currency_Type;
                db.Entry(account).State = EntityState.Modified;
                db.Entry(bank).State = EntityState.Modified;

                db.SaveChanges();
                db.Dispose();
                return bank;
            }
            return null;
        }

        public static int DeleteBank(int perid)
        // u will find bank by id & if it exist it will be deleted .
        // the will save changes .
        {
            var db = new AccountingContainer();
            var MyBank = db.Banks.Find(perid);
            if (MyBank != null)
            {
                db.Banks.Remove(MyBank);
                db.SaveChanges();
                db.Dispose();
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// to make unique code for that bank
        /// </summary>
        /// <param name="Serial"></param>
        /// <returns></returns>
        public static bool containCodeOpen(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.Banks.ToList();
            foreach (var item in allinData)
            {
                if (item.Bank_Code == Serial || !Serial.StartsWith("OPB-"))
                {
                    return true;
                }
                else
                {

                }

            }
            return false;
        }

        public static BankOpenBalanceVM fillForCreateOpenBalance()
        {
            var db = new AccountingContainer();
            var bankOpenVM = new BankOpenBalanceVM();
            var lista = new List<ChartOfAccounting> { };
            foreach (var k in db.ChartOfAccountings)
            {
                var parent = db.ChartOfAccountings.Where(a => a.ChartOfAccountingAccount_Id == k.Account_Id);
                var o = parent.FirstOrDefault();
                if (o == null)
                {
                    lista.Add(db.ChartOfAccountings.Find(k.Account_Id));

                }


            }
            var lastCodebalance = db.Banks.OrderByDescending(c => c.Bank_ID).Where(e => e.Balance_Code != null).FirstOrDefault();
            if (lastCodebalance == null)
            {
                bankOpenVM.Balance_Code = "OPB-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                bankOpenVM.Balance_Code = "OPB-" + (Convert.ToInt32(lastCodebalance.Balance_Code.Substring(4, lastCodebalance.Balance_Code.Length - 4)) + 1).ToString("D3");
            }
            //  subvm.Account_Id = db.ChartOfAccountings.Where(myAccount => myAccount.Account_Name == subvm.Account_Id).FirstOrDefault().Account_Name;

            bankOpenVM.MyBankList = new SelectList(lista.Where(accCode => accCode.Account_Code.Contains("010120002")), "Account_Id", "Account_Name");

            return bankOpenVM;
        }

        public static void CreateOpenBalanceForBank(BankOpenBalanceVM bankOBvm)
        {
            var db = new AccountingContainer();
            var bank = new Bank();

            bank.Balance_Code = bankOBvm.Balance_Code;
            var resultBankAccount = db.ChartOfAccountings.Find(bankOBvm.SelectedBankId);
            if (resultBankAccount != null)
            {
                resultBankAccount.Account_Balance = bankOBvm.Balance_Amount;
            }




        }

        /// <summary>
        /// make creation the bank by importing file excel
        /// </summary>
        /// <param name="excelfile"></param>
        /// <returns></returns>
        public static String Import(HttpPostedFileBase excelfile)
        {
            var data = getdataFillter(excelfile);
            List<Bank> listbank = new List<Bank>();
            if (data.Item2.Count() == 0)
            {
                foreach (var item in data.Item1)
                {
                    var ban = AddNewBankFromFile(item);
                    listbank.Add(ban);

                }
                var listbankempty = listbank.Where(a => a.Balance_Amount != 0).ToList();

                if (listbankempty.Count == 0)
                {

                }
                else
                {
                    Utilites.OpenBalanceFile.MakeOpenBallanceForGroupOFBank(listbank);
                }


                return "تم حفظ الملف بنجاح";

            }
            string combindedString = string.Join(" ,", data.Item2.ToArray());
            return "يوجد اخطاء فى الاسطر التالية" + combindedString;

        }
        /// <summary>
        /// check the data in excel file validation
        /// </summary>
        /// <param name="excelfile"></param>
        /// <returns></returns>
        public static Tuple<List<BankVM>, List<String>> getdataFillter(HttpPostedFileBase excelfile)
        {
            var BankList = new List<BankVM> { };
            var messageList = new List<String> { };
            var db = new AccountingContainer();
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                return Tuple.Create(BankList, messageList);

            }

            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                   // string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + excelfile.FileName);

                    //if (System.IO.File.Exists(path))
                    //{


                    //    File.Delete(path);




                    //}
                    var nameOfFile = "Bank" + DateTime.Now.ToBinary() + ".xlsx";
                    string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + nameOfFile);

                    excelfile.SaveAs(path);

                    Application application = new Application();
                    Workbook workbook = application.Workbooks.Open(path);
                    Worksheet worksheet = workbook.ActiveSheet;
                    Range range = worksheet.UsedRange;
                    for (int row = 3; row <= range.Rows.Count; row++)
                    {
                        BankVM bank = new BankVM();
                        // cust.Customer_Id = int.Parse(((Range)range.Cells[row, 1]).Text);
                        bank.Bank_Name = ((Range)range.Cells[row, 1]).Text;
                        if (bank.Bank_Name == "")
                        {
                            messageList.Add(row.ToString());
                            continue;
                        }

                        try
                        {
                            var bankBal = ((Range)range.Cells[row, 5]).Text;
                            if (bankBal == "")
                            {
                                bank.balance = 0;
                            }
                            else
                            {
                                bank.balance = Convert.ToDecimal(((Range)range.Cells[row, 5]).Text);
                            }




                        }
                        catch
                        {

                            messageList.Add(row.ToString());
                            continue;


                        }
                        bank.IbanNumber = ((Range)range.Cells[row, 4]).Text;
                        bank.BankNumber = ((Range)range.Cells[row, 3]).Text;
                        if (bank.IbanNumber == "")
                        {
                            messageList.Add(row.ToString());
                            continue;
                        }

                        if (bank.BankNumber == "")
                        {
                            messageList.Add(row.ToString());
                            continue;
                        }
                        //try {
                        //    var str = Convert.ToDecimal(bank.BankNumber);
                        //}
                        //catch {

                        //    messageList.Add(row.ToString());
                        //    continue;

                        //}


                        bank.Bank_Address = ((Range)range.Cells[row, 2]).Value;

                        BankList.Add(bank);





                    }
                    workbook.Save();
                    workbook.Close(path);
                    //File.Delete(path);

                }
            }
            return Tuple.Create(BankList, messageList);
        }
        /// <summary>
        /// that create bank from excel file
        /// </summary>
        /// <param name="bvm"></param>
        /// <returns></returns>
        public static Bank AddNewBankFromFile(BankVM bvm)

        {
            var db = new AccountingContainer();
            var newbank = new Bank();

            try
            {
                ChartOfAccountingVM accountVM = new ChartOfAccountingVM();
                accountVM.ChartOfAccountingAccount_Id = 90;
                //accountVM.ChartOfAccountingAccount_Id = bvm.SelectedChartOfAccountId;
                accountVM.Account_Name = bvm.Bank_Name;
                accountVM.Account_Curenncy = db.Currencies.FirstOrDefault().Currency_Id;
                accountVM.Account_Balance = bvm.balance;

                newbank.Bank_Code = ChartOfAccounting.CreateAccount(accountVM);
                newbank.Bank_Name = bvm.Bank_Name;
                newbank.Bank_Address = bvm.Bank_Address;
                var cur = db.Currencies.FirstOrDefault();
                newbank.Bank_Currency = cur.Currency_Type;
                newbank.Bank_Eban_Number = bvm.IbanNumber;
                newbank.Bank_Number = bvm.BankNumber;
                newbank.Balance_Amount = bvm.balance;



                db.Banks.Add(newbank);


                db.SaveChanges();
                db.Dispose();
                return newbank;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                var data = ex.Data;
                var erorr = msg;

            }
            return newbank;
        }
        public static string DeleteTransfer(int id)
        {

            var db = new AccountingContainer();
            var deleteobject = db.Banks.Find(id);
            var Account = db.ChartOfAccountings.Where(a => a.Account_Code == deleteobject.Bank_Code).FirstOrDefault();
            if (deleteobject.IsActive != false)
            {
                deleteobject.IsActive = false;
                Account.isActive = false;

            }
            else
            {
                deleteobject.IsActive = true;
                Account.isActive = true;
            }
            var paymentmethod = db.PaymentMethods.Where(a => a.Bank_Account_Id == Account.Account_Id).ToList();
            if (paymentmethod.Count() > 0)
            {
                foreach(var pay in paymentmethod)
                {
                    pay.IsActive = deleteobject.IsActive;

                    db.Entry(pay).State = EntityState.Modified;

                }
            }
            db.Entry(deleteobject).State = EntityState.Modified;
            db.Entry(Account).State = EntityState.Modified;
            db.SaveChanges();


            return Resources.ProductDetails.TransferSucces;

        }
    }
}