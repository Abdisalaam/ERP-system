using Accountingsystem.Utilites;
using Accountingsystem.ViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class ChartOfAccounting
    {
        //تقوم بارجاع جميع البيانات الخاصة بالحسابات على الشجرة كافة
        public static List<ChartOfAccounting> getAllData()
        {
            var db = new AccountingContainer();
            MakeLedger();
            var account = new PrintChartOfAccount();
            isThatAccountIsChildOrNot();
            var List = account.getAllBalanceSheetModel("0").Where(a => a.isActive != false).ToList();


            return List;

        }
        public static List<ChartOfAccounting> GetAllNotActive()
        {
            var db = new AccountingContainer();
            MakeLedger();
            var account = new PrintChartOfAccount();
            isThatAccountIsChildOrNot();
            var List = account.getAllBalanceSheetModel("0").Where(a=>a.isActive==false).ToList();
            return List;
        }
        //تقوم بارجاع جميع البيانات الخاصة بحساب معين
        public static ChartOfAccounting getDataForThatAccount(int id)
        {
            var db = new AccountingContainer();
            var account = db.ChartOfAccountings.Find(id);
            return account;

        }
        public static int GeTIsThatBankOrTreasury(int id)
        {
            var db = new AccountingContainer();
            var account = db.ChartOfAccountings.Find(id);
            
            if(account.Account_Code.StartsWith("0101200010") || account.Account_Code.StartsWith("0101200020"))
            {
                return 1;
            }

          
            return 0;

        }
        //send aview model to user to fill it
        public static ChartOfAccountingVM FillDataForCreate()
        {
            //var dash = TotalDashBoard.GetAllDashBoardData();

            var db = new AccountingContainer();
            var account = new ChartOfAccountingVM();

            //var acountOftreasury = db.ChartOfAccountings.Where(a => a.Account_Code.StartsWith("010120001") || a.Account_Code.StartsWith("010120002") ||
            //a.Account_Code.StartsWith("010105") || a.Account_Code.StartsWith("010108001") || a.Account_Code.StartsWith("020104001") || a.Account_Code == "0" ||a.Account_Code=="0302" || a.Account_Code == "010111" || a.Account_Code == "010109" || a.Account_Code == "020101");
            var ListForAll = db.ChartOfAccountings.ToList();
            var totalExcept = ListForAll.Where(a => a.Account_Code.StartsWith("010120001") || a.Account_Code.StartsWith("010120002") ||
            a.Account_Code.StartsWith("010105") || a.Account_Code.StartsWith("010108001") || a.Account_Code.StartsWith("020104001") || a.Account_Code == "0"|| a.Account_Code=="0302"||a.Account_Code=="010111"||a.Account_Code== "010109"  || a.Account_Code == "020101").ToList();
            var accounto = ListForAll;
            var accountfinal = accounto.Except(totalExcept).ToList();
            List<object> newList2 = new List<object>();

            foreach (var member in accountfinal)
                newList2.Add(new
                {
                    Id = member.Account_Id,
                    Name = member.Account_Name + "         " + member.Account_Code
                });

            account.chartList = new SelectList(newList2, "Id", "Name");

            account.CurrencyList = new SelectList(db.Currencies, "Currency_Id ", "Currency_Type");
            return account;
        }
        //يقوم بتسجيل وحفظ حساب جديد على الشجرة بكود معتمد على الحساب الرئيسى له
        public static string CreateAccount(ChartOfAccountingVM chvm)
        {
            var db = new AccountingContainer();
            var account = new ChartOfAccounting();
            account.Account_Name = chvm.Account_Name;
            if (chvm.Account_Balance > 0)
            {
                account.Account_Balance = chvm.Account_Balance;

            }
            else
            {
                account.Account_Balance = 0;

            }
            account.Currency_Name = db.Currencies.FirstOrDefault().Currency_Type;
            var resultAccount = db.ChartOfAccountings.Find(chvm.ChartOfAccountingAccount_Id);
            if (resultAccount != null)
            {
                account.ChartOfAccountingAccount_Id = resultAccount.Account_Id;
            }
            //create a level for new account that depend on father level
            var fatherLevel = db.ChartOfAccountings.Find(chvm.ChartOfAccountingAccount_Id).Account_Level;
            account.Account_Level = fatherLevel + 1;

            //all accounts that have the same father
            var accountFather = db.ChartOfAccountings.Where(a => a.ChartOfAccountingAccount_Id == chvm.ChartOfAccountingAccount_Id);
            var o = accountFather.FirstOrDefault();

            if (account.Account_Level < 4)
            {//if there is no account has the same father
                if (o != null)
                {
                    var lastAccount = accountFather.ToList().Last();
                    //transform last account code to long
                    long lastAccountlong = long.Parse(lastAccount.Account_Code);
                    //new code in long
                    var newCodelong = lastAccountlong + 1;

                    account.Account_Code = "0" + newCodelong.ToString();

                    //father code
                }
                else
                {//get the father account
                    var father = db.ChartOfAccountings.Find(chvm.ChartOfAccountingAccount_Id);
                    var fatherAccount = father.Account_Code;
                    account.Account_Code = fatherAccount + "01";
                }
            }
            else
            {//if there is no account has the same father
                if (o != null)
                {
                    var lastAccount = accountFather.ToList().Last();
                    //transform last account code to long
                    long lastAccountLong = long.Parse(lastAccount.Account_Code);
                    //new code in long
                    var newCodeLong = lastAccountLong + 1;

                    account.Account_Code = "0" + newCodeLong.ToString();

                    //father code
                }
                else
                {//get the father account
                    var father = db.ChartOfAccountings.Find(chvm.ChartOfAccountingAccount_Id);
                    var fatherAccount = father.Account_Code;
                    account.Account_Code = fatherAccount + "001";
                }
            }

            db.ChartOfAccountings.Add(account);
            db.SaveChanges();
            db.Dispose();
            if (account.Account_Code.StartsWith("0101200"))
            {
                PaymentMethod.CreatePaymentMethod(account);

            }
            return account.Account_Code;
        }

        public static int getChartbyCode(string code)
        {
            var db = new AccountingContainer();
            var id = db.ChartOfAccountings.Where(a => a.Account_Code == code).FirstOrDefault().Account_Id;
            return id;
        }
        //يقوم بارجاع بيانات الحساب الجديد للتعديل فيها
        public static ChartOfAccountingVM fillChartForediting(int id)
        {
            var db = new AccountingContainer();
            var account = ChartOfAccounting.getDataForThatAccount(id);

            if (account.Account_Code.StartsWith("010105") || account.Account_Code.StartsWith("010120"))
            {
                return null;
            }
                ChartOfAccountingVM accountVM = new ChartOfAccountingVM();
            accountVM.Account_Id = account.Account_Id;
            accountVM.Account_Code = account.Account_Code;
            accountVM.Account_Level = account.Account_Level;
            accountVM.Account_Name = account.Account_Name;
            var thiscurrency = db.Currencies.Where(e => e.Currency_Type.Contains(account.Currency_Name)).ToList().FirstOrDefault();
        
            accountVM.ChartOfAccountingAccount_Id = account.ChartOfAccountingAccount_Id;
            accountVM.CurrencyList = new SelectList(db.Currencies.ToList(), "Currency_Id ", "Currency_Type");
            return accountVM;

        }
        /// <summary>
        /// edit in chart of Accounting and change in db
        /// </summary>
        /// <param name="chatvm"></param>
        /// <returns></returns>
        public static ChartOfAccounting editForChart(ChartOfAccountingVM chatvm)
        {
            var db = new AccountingContainer();

            var thischart = db.ChartOfAccountings.Find(chatvm.Account_Id);

           

            thischart.Account_Name = chatvm.Account_Name;
     
            if (thischart.BalanceSheetValue > 0)
            {

            }
            else
            {
                thischart.BalanceSheetValue = 0;
            }
            if (thischart.Account_Code.StartsWith("0101200"))
            {
                var payment = db.PaymentMethods.Where(a => a.Payment_Type == chatvm.Account_Name).FirstOrDefault();
                payment.Payment_Type = chatvm.Account_Name;
                db.Entry(payment).State = EntityState.Modified;

            }

            db.Entry(thischart).State = EntityState.Modified;
            db.SaveChanges();
            db.Dispose();
            return thischart;


        }
        // Recursively traverse over the children of a node to return a json object of it and all of its children
        public static JObject traverseChildren(ChartOfAccounting parent)
        {
            // Create a Json Array of the tag
            JArray tag = new JArray();
            decimal ballance = Math.Round(parent.BalanceSheetValue.Value, 2, MidpointRounding.AwayFromZero);
            string ballancestr = "";
                if (ballance < 0)
            {
                 ballancestr = "(" + ballance*-1 + ")";
            }
            else
            {
                 ballancestr = ballance.ToString();
            }
            tag.Add(ballancestr);

            //  tag.Add(parent.Account_Code);
            var children = parent.ChartOfAccountings.Where(a=>a.isActive!=false).ToList();

            // IF the node has no children return a Json object of the name and the tag
            if (children.Count == 0)
                return JObject.FromObject(new
                {
                    text = parent.Account_Name +"  "+parent.Account_Code,

                    tags = tag
                });

            // If the node has children loop over its children and recursively traverse each of their children
            // then add the returned object to a Json Array 
            JArray jArr = new JArray();
            foreach (var child in children)
            {
                JObject childJson = traverseChildren(child);
                jArr.Add(childJson);
            }

            // return Json object of the name and the tag and its children Json Array
            return JObject.FromObject(new
            {
                text = parent.Account_Name + "  " + parent.Account_Code,
                nodes = jArr,
                tags = tag
            });
        }
        public static String deleteAccount(int id)
        {
            var db = new AccountingContainer();
            var account = db.ChartOfAccountings.Find(id);
            var gl = db.GLDetails.Where(a => a.Account_Id == id).FirstOrDefault();
            var child = db.ChartOfAccountings.Where(a => a.ChartOfAccountingAccount_Id == id).FirstOrDefault();
            var truseary = db.Treasuries.Where(t => t.Trea_Code == account.Account_Code).FirstOrDefault();
            

            var bankdata = db.Banks.Where(b => b.Bank_Code == account.Account_Code).FirstOrDefault();
            var StoreData = db.Stores.Where(a => a.Store_Code == account.Account_Code).FirstOrDefault();
            var partnerData = db.Partners.Where(a => a.Partner_Code == account.Account_Code).FirstOrDefault();
            var ListMSG = new List<string>();

            if (truseary != null)
            {
                return "  لا يمكنك حذف هذا الحساب هذا حساب صندوق اذا اردت حذفه توجهه الى قائمة الصناديق" ;
            }
            if (child != null)
            {
                return "  لا يمكنك حذف هذا الحساب يوجد حسابات اخري متفرعة من هذا الحساب";
            }
            if (bankdata != null)
            {
                return "لا يمكنك حذف هذا الحساب هذا حساب بنك اذا اردت حذفه توجهه الى قائمة البنوك";
            }
            if (StoreData != null)
            {
                return "لا يمكنك حذف هذا الحساب هذا حساب مخزن اذا اردت حذفه توجهه الى قائمة المخازن";
            }
            if (partnerData != null)
            {
                return "لا يمكنك حذف هذا الحساب هذا حساب شريك اذا اردت حذفه توجهه الى قائمة الشركاء";
            }
            if (gl != null)
            {
                return "  لا يمكنك حذف هذا الحساب يوجد رصيد علي هذا الحساب";
            }

            var TransToMaintentance = db.TransferToMaintenances.Where(e => e.Account_Id == id).ToList();
            if (TransToMaintentance.FirstOrDefault() != null)
            {
                
                return " لايمكنك حذف الحساب يوجد سندات تحويل لصيانة يوجد بها الحساب المراد حذفه  ";
            }
            var ProjectInv = db.ProjectInvoiceMasters.Where(e => e.Revenu_Id == id).FirstOrDefault();
            if (ProjectInv != null)
            {
                return " لايمكنك حذف الحساب يوجد فواتير مشاريع يوجد بها الحساب المراد حذفه  ";

            }
            var projectPay = db.ProjectPayments.Where(e => e.Revenue_Id == id).FirstOrDefault();
            if (projectPay != null)
            {
                return " لايمكنك حذف الحساب يوجد سندات قبض مشاريع يوجد بها الحساب المراد حذفه  ";

            }
            var transtoProj = db.TransferToProjectMasters.Where(e => e.Account_Id == id).FirstOrDefault();
            if (transtoProj != null)
            {
                return " لايمكنك حذف الحساب يوجد سندات تحويل لمشروع يوجد بها الحساب المراد حذفه  ";

            }
            var SalesInvm = db.SalesReceiptMasters.Where(e => e.Account_Id == id).FirstOrDefault();
            if (SalesInvm != null)
            {
                return " لايمكنك حذف الحساب يوجد فواتير نقدية يوجد بها الحساب المراد حذفه  ";

            }
            var Expense = db.ExpenseAccountTemps.Where(e => e.AccountExp_ID== id).FirstOrDefault();

            if (Expense != null)
            {
                return " لايمكنك حذف الحساب يوجد مصروفات يوجد بها الحساب المراد حذفه  ";

            }
            var catchRecipt = db.CatchReceipts.Where(e => e.AccountCredit_Id == id).FirstOrDefault();
            if (catchRecipt != null)
            {
                return " لايمكنك حذف الحساب يوجد سندات قبض  يوجد بها الحساب المراد حذفه  ";

            }
            var catchReciptDepit = db.CatchReceipts.Where(e => e.AccountDebit_Id == id).FirstOrDefault();
            if (catchReciptDepit != null)
            {
                return " لايمكنك حذف الحساب يوجد سندات قبض يوجد بها الحساب المراد حذفه  ";

            }
            var spentRecipt = db.SpentRecipts.Where(e => e.AccountCredit_Id== id).FirstOrDefault();

            if (spentRecipt != null)
            {
                return " لايمكنك حذف الحساب يوجد سندات صرف يوجد بها الحساب المراد حذفه  ";

            }
            var spentReciptDepit = db.SpentRecipts.Where(e => e.AccountDebit_Id == id).FirstOrDefault();

            if (spentReciptDepit != null)
            {
                return " لايمكنك حذف الحساب يوجد سندات صرف يوجد بها الحساب المراد حذفه  ";

            }





            db.ChartOfAccountings.Remove(account);
                db.SaveChanges();
                db.Dispose();
                return Resources.validate.DeleteSuccessful;
           


        }
        /// <summary>
        /// make drobdownList for chart of Account not contain Store and Treasery
        /// </summary>
        /// <returns></returns>
        public static List<ChartOfAccounting> getTrueData()
        {

            var db = new AccountingContainer();
            var acountOftreasury = db.ChartOfAccountings.Where(a => a.Account_Code.StartsWith("010120001") || a.Account_Code.StartsWith("010120002") ||
         a.Account_Code.StartsWith("010105") || a.Account_Code.StartsWith("010108001") || a.Account_Code.StartsWith("020104001") || a.Account_Code == "0");
            var accounto = db.ChartOfAccountings.Except(acountOftreasury).ToList();
            return accounto;

        }
        /// <summary>
        /// get all Child Chart of Accounting
        /// </summary>
        /// <returns></returns>
        public static IQueryable<object>  getAlldataForGL()
        {
            var db = new AccountingContainer();
            var lista = (from chart in db.ChartOfAccountings.Where(a=>a.ChartOfAccountings.Count()==0&& a.isActive != false).OrderBy(e => e.Account_Code)
                        select new {
                Id = chart.Account_Id,
                Name = chart.Account_Name + "  " + chart.Account_Code,
                Code = chart.Account_Code

            });
            //foreach (var k in db.ChartOfAccountings)
            //{
            //    var parent = db.ChartOfAccountings.Where(a => a.ChartOfAccountingAccount_Id == k.Account_Id);
            //    var o = parent.FirstOrDefault();
            //    if (o == null)
            //    {
            //        lista.Add(db.ChartOfAccountings.Find(k.Account_Id));

            //    }


            //}
            return lista;

        }
        /// <summary>
        /// make budget sheet fot chart of Accounting
        /// </summary>
        public static void MakeLedger()
        {
            var Total = TotalDashBoard.GetAllDashBoardData();
        }
        public static void  isThatAccountIsChildOrNot()
        {
            var db = new AccountingContainer();
            var lista = new List<ChartOfAccounting> { };
            foreach (var k in db.ChartOfAccountings)
            {
                var parent = db.ChartOfAccountings.Where(a => a.ChartOfAccountingAccount_Id == k.Account_Id);
                var o = parent.FirstOrDefault();
                if (o == null)
                {
                    lista.Add(db.ChartOfAccountings.Find(k.Account_Id));
                    k.ISFinalChild = true;
                    db.Entry(k).State = EntityState.Modified;
                  


                }
                else
                {
                    k.ISFinalChild = false;
                    db.Entry(k).State = EntityState.Modified;

                }

            }


            db.SaveChanges();

        }
        private static void saveChart(ChartOfAccounting account)
        {
            var db = new AccountingContainer();
            account.ISFinalChild = true;
            db.Entry(account).State = EntityState.Modified;
            db.SaveChanges();

        } 

        public static decimal GetBallanceOfAccount(string code)
        {
            var db = new AccountingContainer();
            //var Account = db.ChartOfAccountings.Where(a => a.Account_Code == code).FirstOrDefault();
            var glAcount = db.GLDetails.Where(a => a.ChartOfAccounting.Account_Code == code);
            if (glAcount.Count() == 0)
            {
                return 0;

            }
            else
            {
                return glAcount.Select(a => a.GLDetail_Debit).Sum() - glAcount.Select(a => a.GLDetail_Credit).Sum();
            }



        }




        public static string changeAccountValue()
        {

          
            var db = new AccountingContainer();
            string connstring = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(connstring);
            conn.Open();
            var Account = db.Accounts.Where(a => a.OpenBalance_Code == "1").FirstOrDefault();
            if (Account != null)
            {
                return "لا يوجد تعديلات";
            }
            var EmployeeNotActive = db.Employees.Where(a => a.Active == false).ToList();
            var SupplyNotActive = db.Suppliers.Where(a => a.Supplier_state == Customer_State.غير_نشط).ToList();
            var CustomerNotActive = db.Customers.Where(a => a.Customer_State == Customer_State.غير_نشط).ToList();
            foreach (var emp in EmployeeNotActive)
            {
              
                var account = db.Accounts.Where(a => a.Account_Code == emp.Emp_Code).FirstOrDefault();
                if (account != null)
                {
                    account.OpenBalance_Code = "1";
                    string sql = " UPDATE[Accounts] set OpenBalance_Code = '1' where Account_Id = {0}";
                    db.Database.ExecuteSqlCommand(sql, account.Accounts_Id);
                }
            }
            foreach (var Sup in SupplyNotActive)
            {
                var account = db.Accounts.Where(a => a.Account_Code == Sup.Supp_Code).FirstOrDefault();
                if (account != null)
                {
                    account.OpenBalance_Code = "1";
                    string sql = " UPDATE[Accounts] set OpenBalance_Code = '1' where Account_Id = {0}";
                    db.Database.ExecuteSqlCommand(sql, account.Accounts_Id);

                }
            }

            foreach (var cust in CustomerNotActive)
            {
            
                    var account = db.Accounts.Where(a => a.Account_Code == cust.Customer_Code).FirstOrDefault();
                if (account != null)
                {
                    account.OpenBalance_Code = "1";
                    string sql = " UPDATE[Accounts] set OpenBalance_Code = '1' where Account_Id = {0}";
                    db.Database.ExecuteSqlCommand(sql, account.Accounts_Id);
                }
            }
            db.SaveChanges();
            return "done";
        }
        public static string DeleteTransfer(int id)
        {

            var db = new AccountingContainer();
            var deleteobject = db.ChartOfAccountings.Find(id);
     
            if (deleteobject.isActive != false)
            {
                deleteobject.isActive = false;
            

            }
            else
            {
                deleteobject.isActive = true;
              
            }

            db.Entry(deleteobject).State = EntityState.Modified;
          
            db.SaveChanges();


            return Resources.ProductDetails.TransferSucces;

        }
    }
}