using Accountingsystem.ViewModel;
using Accountingsystem.ViewModel.ReportsVM;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class SubLedger
    {
        /// <summary>
        /// check the subledger cod is unique
        /// </summary>
        /// <param name="Serial"></param>
        /// <returns>bool</returns>
        public static bool containCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.SubLedgers.ToList();
            foreach (var item in allinData)
            {
                if (item.SubLed_Code == Serial || !Serial.StartsWith("SUBLD-"))
                {
                    return true;
                }
                else
                {

                }

            }
            return false;
        }

        //get all data from subledger entity
        public static List<SubLedger> getAllData()
        {
            var db = new AccountingContainer();
            var mas = db.SubLedgers.Where(a => a.SubLed_Type == "مشروع");
            
            var query = mas.GroupBy(x => x.SubLed_Name)
           .Where(g => g.Count() > 1)
           .Select(y => y.Key)
           .ToList();
            var subLedger = db.SubLedgers.Where(s => s.SubLed_Type == "مركز تكلفة يدوى" && s.isActive != false).ToList();
            return subLedger;
        }
        public static List<SubLedger> GetAllNotActive()
        {
            var db = new AccountingContainer();
            var mas = db.SubLedgers.Where(a => a.SubLed_Type == "مشروع");

            var query = mas.GroupBy(x => x.SubLed_Name)
           .Where(g => g.Count() > 1)
           .Select(y => y.Key)
           .ToList();
            var subLedger = db.SubLedgers.Where(s => s.SubLed_Type == "مركز تكلفة يدوى"&&s.isActive==false).ToList();
            return subLedger;
        }
        /// <summary>
        /// 
        /// get the file of subledger by its id        /// </summary>
        /// <param name="fileId"></param>
        /// <returns>SubLedgerFileDetail1</returns>
        public static SubLedgerFileDetail1 getFileById(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.SubLedgerFileDetail1.Find(fileId);
            return file;
        }
        /// <summary>
        /// get all File that belong to that subledger by its id
        /// </summary>
        /// <param name="subLId"></param>
        /// <returns></returns>
        public static List<SubLedgerFileDetail1> GetAllFiles(int subLId)
        {
            var db = new AccountingContainer();
            var files = db.SubLedgerFileDetail1.Where(emp => emp.SubLedgerSubLed_Id == subLId).ToList();
            return files;
        }
        //get all data for that subledger by Id
        public static SubLedger getDetail(int id)
        {
            var db = new AccountingContainer();
            var subLedger = db.SubLedgers.Find(id);
            return subLedger;

        }
        /// <summary>
        /// get name of subledger by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string getName(int id)
        {
            var db = new AccountingContainer();

            var subLedger = db.SubLedgers.Find(id);
            var nameCode = subLedger.SubLed_Name + " " + subLedger.SubLed_Code;
            return nameCode;

        }
        //take vm that filled and save it in datanbase
        public static SubLedger createSubLedger(SubLedgerVM subVM)
        {
            var db = new AccountingContainer();
            var Sub = new SubLedger();
           
          
            Sub.SubLedger_Balance = 0;
            Sub.Subledger_Expense = 0;
            Sub.Subledger_Revenu = 0;

            Sub.SubLed_Name = subVM.SubLed_Name;
            Sub.SubLed_Date = DateTime.Now;
            Sub.SubLed_Type = subVM.SubLed_Type;
            Sub.SubLed_Desc = (string)subVM.SubLed_Desc;
            var fatherLevel = db.SubLedgers.Find(subVM.ParentSubledger_Id).Subled_Level;
            Sub.Subled_Level = fatherLevel + 1;
            Sub.SubLedgerSubLed_Id = subVM.ParentSubledger_Id;

            var FirstChild = db.SubLedgers.Where(a => a.Subled_Level == 1).FirstOrDefault();
            if (FirstChild == null && Sub.Subled_Level == 1)
            {
                Sub.SubLed_Code = "01";
            }
            else
            {
                //all accounts that have the same father
                var accountFather = db.SubLedgers.Where(a => a.SubLedgerSubLed_Id == subVM.ParentSubledger_Id);
                var o = accountFather.FirstOrDefault();

                if (Sub.Subled_Level < 4)
                {//if there is no account has the same father
                    if (o != null)
                    {
                        var lastAccount = accountFather.ToList().Last();
                        //transform last account code to long
                        long lastAccountlong = long.Parse(lastAccount.SubLed_Code);
                        //new code in long
                        var newCodelong = lastAccountlong + 1;

                        Sub.SubLed_Code = "0" + newCodelong.ToString();

                        //father code
                    }
                    else
                    {//get the father account
                        var father = db.SubLedgers.Find(subVM.ParentSubledger_Id);
                        var fatherAccount = father.SubLed_Code;
                        Sub.SubLed_Code = fatherAccount + "01";
                    }
                }
                else
                {//if there is no account has the same father
                    if (o != null)
                    {
                        var lastAccount = accountFather.ToList().Last();
                        //transform last account code to long
                        long lastAccountLong = long.Parse(lastAccount.SubLed_Code);
                        //new code in long
                        var newCodeLong = lastAccountLong + 1;

                        Sub.SubLed_Code = "0" + newCodeLong.ToString();

                        //father code
                    }
                    else
                    {//get the father account
                        var father = db.SubLedgers.Find(subVM.ParentSubledger_Id);
                        var fatherAccount = father.SubLed_Code;
                        Sub.SubLed_Code = fatherAccount + "001";
                    }



                }
            }
            //if (subVM.Files.FirstOrDefault() != null)
            //{
            //    foreach (var file in subVM.Files)
            //    {
            //        var filedetails = new SubLedgerFileDetail1();
            //        var data = new byte[file.ContentLength];
            //        filedetails.FileName = file.FileName;
            //        filedetails.File_Type = file.ContentType;
            //        filedetails.ContentLength = file.ContentLength;
            //        file.InputStream.Read(data, 0, file.ContentLength);
            //        filedetails.ProImg = data;
            //        filedetails.SubLedgerSubLed_Id = Sub.SubLed_Id;
            //        Sub.SubLedgerFileDetail1.Add(filedetails);
            //    }
            //}

            db.SubLedgers.Add(Sub);
            db.SaveChanges();
            db.Dispose();
            return Sub;

        }
        //that return vm that fill with data that it will be modifiy
        public static SubLedgerVM fillDataForEditing(int id)
        {
            var subLedger = getDetail(id);
            var db = new AccountingContainer();
            var subVM = new SubLedgerVM();

          
            
                subVM.SubLed_Id = subLedger.SubLed_Id;
                subVM.SubLed_Code = subLedger.SubLed_Code;
                subVM.SubLed_Name = subLedger.SubLed_Name;
                subVM.SubLed_Date = subLedger.SubLed_Date.ToString();


                subVM.SubLed_Type = subLedger.SubLed_Type;
                subVM.SubLed_Desc = subLedger.SubLed_Desc;
           

            return subVM;
        }
        //that post and modify old data and save it in database
        public static SubLedger editTheData(SubLedgerVM subvm)
        {
            try
            {
                var db = new AccountingContainer();
                var sub = db.SubLedgers.Find(subvm.SubLed_Id);
                sub.SubLed_Id = subvm.SubLed_Id;
                sub.SubLed_Code = subvm.SubLed_Code;
                sub.SubLed_Type = subvm.SubLed_Type;
                sub.SubLed_Date = DateTime.Now;
                sub.SubLed_Desc = subvm.SubLed_Desc;
                if (sub.SubLed_Type == "مركز تكلفة يدوى")
                {
                    
                    sub.SubLed_Name = subvm.SubLed_Name;             

                }
               

                db.Entry(sub).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                return sub;
            }
            catch (Exception exp)
            {
                var a = exp.Message;
                throw;
            }
          

        }
        /// <summary>
        /// prepare view for create new subledger
        /// </summary>
        /// <returns>SubLedgerVM</returns>
        public static SubLedgerVM fillForCreateSubLedger()
        {
            var db = new AccountingContainer();
            var subvm = new SubLedgerVM();
            //int lastCode = (from s in db.SubLedgers
            //                select s.SubLed_Id).ToList().LastOrDefault();
            //subvm.SubLed_Code = (lastCode + 1).ToString();

            List<object> result = new List<object>();
            foreach (var subledger in db.SubLedgers.Where(a=>a.isActive!=false))
            {
                var data = new
                {
                    // the value of each select item
                    id = subledger.SubLed_Id,
                    // showing both code and name in the drop-down list 
                    text = subledger.SubLed_Name + "\t" + subledger.SubLed_Code
                };

                result.Add(data);
            }

                subvm.SubledgerList = new SelectList(result, "id", "text");




                //var lastCodebalance = db.SubLedgers.OrderByDescending(c => c.SubLed_Id).Where(e => e.OpenBalance_Code != null).FirstOrDefault();
                //if (lastCodebalance == null)
                //{
                //    subvm.OpenBalance_Code = "OPS-001";
                //}
                //else
                //{
                //    //using string substring method to get the number of the last inserted
                //    subvm.OpenBalance_Code = "OPS-" + (Convert.ToInt32(lastCodebalance.OpenBalance_Code.Substring(4, lastCodebalance.OpenBalance_Code.Length - 4)) + 1).ToString("D3");
                //}
                ////  subvm.Account_Id = db.ChartOfAccountings.Where(myAccount => myAccount.Account_Name == subvm.Account_Id).FirstOrDefault().Account_Name;

                return subvm;

            
        }
        /// <summary>
        /// check opening balance is true or not
        /// </summary>
        /// <param name="Serial"></param>
        /// <returns></returns>
        public static bool containCodeOpen(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.SubLedgers.ToList();
            foreach (var item in allinData)
            {
                if (item.OpenBalance_Code == Serial || !Serial.StartsWith("OPS-"))
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
        /// make opening balance for Subledger 
        /// </summary>
        /// <param name="subVM"></param>
        /// <returns></returns>
        public static SubLedger createSubLedgerOpenBalance(SubLedgerVM subVM)
        {
            var db = new AccountingContainer();
            var Sub = new SubLedger();
         
            Sub.SubLed_Code = subVM.SubLed_Code;
            Sub.SubLed_Name = subVM.SubLed_Name;
            Sub.SubLed_Date = DateTime.Now;
            Sub.SubLed_Type = subVM.SubLed_Type;
            Sub.SubLed_Desc = subVM.SubLed_Desc;
            if (subVM.SubledgerBalanceCredit == null || subVM.SubledgerBalanceCredit == 0)
            { Sub.SubLedger_Balance = subVM.SubledgerBalanceDebit; }
            else
            { Sub.SubLedger_Balance = subVM.SubledgerBalanceCredit; }
           
            Sub.OpenBalance_Code = subVM.OpenBalance_Code;
            if (subVM.Files.FirstOrDefault() != null)
            {
                foreach (var file in subVM.Files)
                {
                    var filedetails = new SubLedgerFileDetail1();
                    var data = new byte[file.ContentLength];
                    filedetails.FileName = file.FileName;
                    filedetails.File_Type = file.ContentType;
                    filedetails.ContentLength = file.ContentLength;
                    file.InputStream.Read(data, 0, file.ContentLength);
                    filedetails.ProImg = data;
                    filedetails.SubLedgerSubLed_Id = Sub.SubLed_Id;
                    Sub.SubLedgerFileDetail1.Add(filedetails);
                }
            }

            db.SubLedgers.Add(Sub);
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
            if (subVM.SubledgerBalanceDebit == null && subVM.SubledgerBalanceCredit == null)
            { }
            else
            {
                var GlMAster = new GLMaster();
                var GlDetail1 = new GLDetail();
                var GlDetail = new GLDetail();
                GlMAster.GL_Date = Convert.ToDateTime(subVM.SubLed_Date);
                GlMAster.GLType_Id = "1";
                GlMAster.GL_Desc = "رصيد افتتاحي مركز تكلفة";
                GlMAster.Currency_Id = "1";
                GlMAster.Company_Id = 1;
                GlMAster.Auto = true;
                GlMAster.GLCode = subVM.OpenBalance_Code;
                GlMAster.Auto = true;
                ///////
                GlMAster.GLState = TransactionState.Post;
                if (subVM.SubledgerBalanceDebit == null)
                {
                    GlDetail.Account_Id = 0;
                    GlDetail.Account_Name = "";
                    GlDetail.Bu_Id = 0;
                    GlDetail.GLDetail_Credit = (decimal)subVM.SubledgerBalanceCredit;//داءن
                  
                    GlDetail.GLDetail_Debit = 0;//مدين
                    GlDetail.SubLedger_Id = Sub.SubLed_Id;
                    GlDetail.Asset_Id = 0;
                    GlDetail.Account_Name="";
                    GlMAster.GLDetails.Add(GlDetail);
                   
                    //GLMaster.GetBalanceForAccount(GlMAster);
                    GLMaster.GetBalanceForSubLedger(GlMAster);
                    var wsitchart = db.ChartOfAccountings.Where(e => e.Account_Code == "0601" && e.ChartOfAccountingAccount_Id ==130).ToList().LastOrDefault();
                    GlDetail1.Account_Id = wsitchart.Account_Id;
                    GlDetail1.Account_Name = wsitchart.Account_Name;
                    GlDetail1.Bu_Id = 0;
                    GlDetail1.GLDetail_Credit =0 ;//داءن

                    GlDetail1.GLDetail_Debit = (decimal)subVM.SubledgerBalanceCredit;//مدين
                    GlDetail1.SubLedger_Id = Sub.SubLed_Id;
                    GlDetail1.Asset_Id = 0;
                    GlDetail1.Account_Name = wsitchart.Account_Name ;
                    GlMAster.GLDetails.Add(GlDetail1);
                    db.GLMasters.Add(GlMAster);
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
                }
                else if (subVM.SubledgerBalanceCredit == null)
                {
                    GlDetail.Account_Id = 0;
                    GlDetail.Account_Name = "";
                    GlDetail.Bu_Id = 0;
                    GlDetail.GLDetail_Credit = 0;//داءن

                    GlDetail.GLDetail_Debit = (decimal)subVM.SubledgerBalanceDebit;//مدين
                    GlDetail.SubLedger_Id = Sub.SubLed_Id;
                    GlDetail.Asset_Id = 0;
                    GlDetail.Account_Name = "";
                    GlMAster.GLDetails.Add(GlDetail);
                  
                    //GLMaster.GetBalanceForAccount(GlMAster);
                    GLMaster.GetBalanceForSubLedger(GlMAster);
                    var wsitchart = db.ChartOfAccountings.Where(e => e.Account_Code == "0601" && e.ChartOfAccountingAccount_Id == 130).ToList().LastOrDefault();
                    GlDetail1.Account_Id = wsitchart.Account_Id;
                    GlDetail1.Account_Name = wsitchart.Account_Name;
                    GlDetail1.Bu_Id = 0;
                    GlDetail1.GLDetail_Credit = (decimal)subVM.SubledgerBalanceCredit;//داءن

                    GlDetail1.GLDetail_Debit = 0;//مدين
                    GlDetail1.SubLedger_Id = Sub.SubLed_Id;
                    GlDetail1.Asset_Id = 0;
                    GlDetail1.Account_Name = wsitchart.Account_Name;
                    GlMAster.GLDetails.Add(GlDetail1);
                    db.GLMasters.Add(GlMAster);
                    db.SaveChanges();
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
                }

            }

            db.SaveChanges();
            db.Dispose();
            return Sub;

        }

        /// <summary>
        /// make calculation of subledger vales like credit or depit
        /// </summary>
        public static void MakeCalculateOfSubLedgerForTree() {
            var db = new AccountingContainer();
            var subLedgers = db.SubLedgers.ToList();
            foreach (var item in subLedgers)
            {
                SubLedgerReportVM Sub = new SubLedgerReportVM();
                Sub.SubLedger_Id = item.SubLed_Id;
                var report = Reports.postDataToMakeReportOfSubLedger(Sub);
                item.SubLedger_Balance = report.Difference;
                db.Entry(item).State = EntityState.Modified;

            }

            db.SaveChanges();





        }
        /// <summary>
        /// make tree of subledger 
        /// </summary>
        /// <param name="parent"></param>
        /// <returns>JObject</returns>
        public static JObject traverseChildren(SubLedger parent)
        {
            // Create a Json Array of the tag
            JArray tag = new JArray();
            decimal ballance = Math.Round(parent.SubLedger_Balance.Value, 2, MidpointRounding.AwayFromZero);
            string ballancestr = "";
            if (ballance < 0)
            {
                ballancestr = "(" + ballance * -1 + ")";
            }
            else
            {
                ballancestr = ballance.ToString();
            }
            tag.Add(ballancestr);


         
            //tag.Add(Math.Round(parent.SubLedger_Balance.Value,2,MidpointRounding.AwayFromZero));
            
            var children = parent.SubLedgers;

            // IF the node has no children return a Json object of the name and the tag
            if (children.Count == 0)
                return JObject.FromObject(new
                {
                    text = parent.SubLed_Name+" "+parent.SubLed_Code,
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
                text = parent.SubLed_Name+" "+ parent.SubLed_Code,
                nodes = jArr,
                tags = tag
            });
        }
        /// <summary>
        /// create anew subledger and save it in db
        /// </summary>
        /// <param name="subVM"></param>
        /// <returns></returns>
        public static SubLedger createSubLedgerManual(SubLedgerVM subVM)
        {
            var db = new AccountingContainer();
            var Sub = new SubLedger();


            Sub.SubLedger_Balance = 0;
            Sub.Subledger_Expense = 0;
            Sub.Subledger_Revenu = 0;
            Sub.SubLed_Name = subVM.SubLed_Name;
            Sub.SubLed_Date = DateTime.Now;
            Sub.SubLed_Type = "مركز تكلفة يدوى";
            if (subVM.ParentSubledger_Id != null)
            {

                Sub.SubLedgerSubLed_Id = subVM.ParentSubledger_Id;

            }
          
            else { Sub.SubLedgerSubLed_Id = 1; }
            subVM.ParentSubledger_Id = Sub.SubLedgerSubLed_Id;
            Sub.SubLed_Desc = (string)subVM.SubLed_Desc;
            var fatherLevel = db.SubLedgers.Find(subVM.ParentSubledger_Id).Subled_Level;
            Sub.Subled_Level = fatherLevel + 1;
            Sub.SubLedgerSubLed_Id = subVM.ParentSubledger_Id;
            var FirstChild = db.SubLedgers.Where(a => a.Subled_Level == 1).FirstOrDefault();
            if (FirstChild == null && Sub.Subled_Level == 1)
            {
                Sub.SubLed_Code = "01";
            }
            else
            {
                //all accounts that have the same father
                var accountFather = db.SubLedgers.Where(a => a.SubLedgerSubLed_Id == subVM.ParentSubledger_Id);
                var o = accountFather.FirstOrDefault();

                if (Sub.Subled_Level < 4)
                {//if there is no account has the same father
                    if (o != null)
                    {
                        var lastAccount = accountFather.ToList().Last();
                        //transform last account code to long
                        long lastAccountlong = long.Parse(lastAccount.SubLed_Code);
                        //new code in long
                        var newCodelong = lastAccountlong + 1;

                        Sub.SubLed_Code = "0" + newCodelong.ToString();

                        //father code
                    }
                    else
                    {//get the father account
                        var father = db.SubLedgers.Find(subVM.ParentSubledger_Id);
                        var fatherAccount = father.SubLed_Code;
                        Sub.SubLed_Code = fatherAccount + "01";
                    }
                }
                else
                {//if there is no account has the same father
                    if (o != null)
                    {
                        var lastAccount = accountFather.ToList().Last();
                        //transform last account code to long
                        long lastAccountLong = long.Parse(lastAccount.SubLed_Code);
                        //new code in long
                        var newCodeLong = lastAccountLong + 1;

                        Sub.SubLed_Code = "0" + newCodeLong.ToString();

                        //father code
                    }
                    else
                    {//get the father account
                        var father = db.SubLedgers.Find(subVM.ParentSubledger_Id);
                        var fatherAccount = father.SubLed_Code;
                        Sub.SubLed_Code = fatherAccount + "001";
                    }



                }
            }
            //if (subVM.Files.FirstOrDefault() != null)
            //{
            //    foreach (var file in subVM.Files)
            //    {
            //        var filedetails = new SubLedgerFileDetail1();
            //        var data = new byte[file.ContentLength];
            //        filedetails.FileName = file.FileName;
            //        filedetails.File_Type = file.ContentType;
            //        filedetails.ContentLength = file.ContentLength;
            //        file.InputStream.Read(data, 0, file.ContentLength);
            //        filedetails.ProImg = data;
            //        filedetails.SubLedgerSubLed_Id = Sub.SubLed_Id;
            //        Sub.SubLedgerFileDetail1.Add(filedetails);
            //    }
            //}

            db.SubLedgers.Add(Sub);
            db.SaveChanges();
            db.Dispose();
            return Sub;


        }
        public static decimal getSubExpences(string name) {

            var db = new AccountingContainer();
            decimal Credit = 0;
            decimal depit = 0;
            var sub = db.SubLedgers.Where(a => a.SubLed_Name == name).FirstOrDefault();
            var Account = db.GLDetails.Where(a => a.SubLedger_Id == sub.SubLed_Id && a.ChartOfAccounting.Account_Code.StartsWith("05"));
            if (Account.Select(a => a.GLDetail_Credit).Count() > 0)
            {
                 Credit = Account.Select(a => a.GLDetail_Credit).Sum();
            }
            if (Account.Select(a => a.GLDetail_Debit).Count() > 0)
            {
                 depit = Account.Select(a => a.GLDetail_Debit).Sum();
            }
            return depit - Credit;

        }
        public static decimal getTheBalanceOfDateBefore(int subledger , DateTime date)
        {
            var db = new AccountingContainer();
            var glDetail = db.GLDetails.Where(a => a.SubLedger_Id == subledger && a.GLMaster.GL_Date < date).ToList();
            if (glDetail.Count() > 0)
            {

                return glDetail.Select(a => a.GLDetail_Debit).Sum() - glDetail.Select(a => a.GLDetail_Credit).Sum();

            }

            return 0;
        }
        public static string DeleteTransfer(int id,int ? level)
        {

            var db = new AccountingContainer();
            var deleteobject = db.SubLedgers.Find(id);
            var check = deleteobject.isActive;
            var subledgerList = db.SubLedgers.Where(a => a.SubLed_Code .StartsWith(deleteobject.SubLed_Code)).ToList();
            foreach(var sub in subledgerList)
            {
                if (check != false)
                {
                
                    sub.isActive = false;

                }
                else
                {

                    sub.isActive = true;
                }
                db.Entry(sub).State = EntityState.Modified;
          
            }
            db.SaveChanges();




            return Resources.ProductDetails.TransferSucces;

        }
    }
}