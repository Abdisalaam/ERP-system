using Accountingsystem.ViewModel;
using Accountingsystem.ViewModel.ReportsVM;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Accountingsystem.Utilites;

namespace Accountingsystem.Models
{
    public partial class GLMaster
    {
        //تقوم بارجاع جميع القيود اليومية المخزنة 
        public static List<GLMaster> getALLGLMaster()
        {
            var db = new AccountingContainer();
            var glmas = db.GLMasters.Where(e => e.Auto == false && e.GLType_Id != "قيد اقفال").ToList();
            return glmas;


        }
        public static List<GLMaster> getALLGLMasterAll()
        {
            var db = new AccountingContainer();
            var glmas = db.GLMasters.ToList();
            return glmas;


        }
        /// <summary>
        /// chek that date is suitable
        /// </summary>
        /// <returns></returns>
        public static string Checkdate(string date)
        {
            var datetime = Convert.ToDateTime(date);
            var db = new AccountingContainer();
            var Comstartdate = db.Companies.FirstOrDefault().StartFinancialDate;
            if (Comstartdate != null)
            {
                if (datetime < Comstartdate.Value)
                {
                    return Resources.GLMasterFillGlForCreate.DateShouldBeInThisFisicalYear;
                }
            }

            return null;


        }
        //تقوم بارجاع جميع القيود الالية المخزنة 
        public static List<GLMaster> getALLGLMasterAuto()
        {
            var db = new AccountingContainer();
            var glmas = db.GLMasters.Where(e => e.Auto == true && (!e.GL_Desc.Contains("افتتاحي") || e.GL_Desc == "" || e.GL_Desc == null) && !e.GLCode.Contains("GLST-")).ToList();
            return glmas;


        }
        //تقوم بارجاع جميع القيود الافتتاحية المخزنة 
        public static List<GLMaster> getALLGLMasterAutoOpen()
        {
            var db = new AccountingContainer();
            var glmas = db.GLMasters.Where(e => e.Auto == true && e.GLCode.StartsWith("GLST") || e.GL_Desc.Contains("افتتاحي")).ToList();
            return glmas;


        }

        public static List<GLMaster> getALLTAXGL()
        {
            var db = new AccountingContainer();
            var glmas = db.GLMasters.Where(e => e.GLCode.StartsWith("TAX")).ToList();
            return glmas;


        }
        public static List<GLMaster> getALLEMPGL()
        {
            var db = new AccountingContainer();
            var glmas = db.GLMasters.Where(e => e.GLCode.StartsWith("EMPT-")).ToList();
            return glmas;


        }
        //تقوم بارجاع القيد اليومى المراد
        public static GLMaster getDetailById(int id)
        {
            var db = new AccountingContainer();

            var glmas = db.GLMasters.Find(id);

            foreach (var child in glmas.GLDetails)
            {
                if (child.Account_Child_ID != 0 || child.Account_Child_ID != null)
                {
                    var thisaccount = db.Accounts.Find(child.Account_Child_ID);
                    if (thisaccount != null)
                    {
                        child.Account_CHild_Name = thisaccount.Account_Name;

                    }
                }

                else
                { return glmas; }
            }
            db.Entry(glmas).State = EntityState.Modified;
            db.SaveChanges();
            return glmas;

        }
        //send vm to user to fill it 
        public static GLMasterVM fillToCreate()
        {

            var db = new AccountingContainer();
            var mastervm = new GLMasterVM();
            var newList2 = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0 && a.isActive != false).OrderBy(e => e.Account_Code).ToList();
            var notActiveTaxes = db.Taxes.Where(a => a.IsActive == false).ToList();
            foreach (var notActiveTax in notActiveTaxes)
            {
                foreach (var account in newList2.ToList())
                {
                    if (account.Account_Code == notActiveTax.Tax_Code)
                    {
                        newList2.Remove(account);
                    }
                }

            }
            mastervm.accountList = new SelectList(newList2, "Account_Id", "Account_Name");
            var newList = from subled in db.SubLedgers.Where(a => a.isActive != false)
                          select new
                          {

                              Id = subled.SubLed_Id,
                              Name = subled.SubLed_Name + "        " + subled.SubLed_Code


                          };
            mastervm.SubLedgerList = new SelectList(newList, "Id", "Name");
            mastervm.CompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            mastervm.GLTypeList = new SelectList(db.GL_Type, "GL_Type_Id", "GL_Type_Name");
            mastervm.CurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");


            mastervm.AssetList = new SelectList(db.FixedAssets, "Fasset_ID", "FAsset_Type");
            mastervm.childList = new SelectList(db.SubLedgers.Where(a => a.isActive != false), "SubLed_Id", "SubLed_Name");

            var lastCode = db.GLMasters.Where(a => a.GLState == TransactionState.Post).OrderByDescending(c => c.GLMaster_Id).Where(a => a.GLCode.StartsWith("GL-")).FirstOrDefault();
            if (lastCode == null)
            {
                mastervm.GLCode = "GL-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                mastervm.GLCode = "GL-" + (Convert.ToInt32(lastCode.GLCode.Substring(3, lastCode.GLCode.Length - 3)) + 1).ToString("D3");
            }

            return mastervm;
        }

        public static GLMasterVM fillToEdit(int id)
        {

            var db = new AccountingContainer();
            var gl = db.GLMasters.Find(id);
            var com = Company.GetCompanies().FirstOrDefault();
            if ((gl.Auto == false || gl.GLCode.StartsWith("GLST-")) && gl.GL_Date >= com.StartFinancialDate)
            {
                var mastervm = new GLMasterVM();
                mastervm.GL_Desc = gl.GL_Desc;
                mastervm.GL_Date = gl.GL_Date.ToString("dd-MM-yyyy   HH:mm:ss");
                var newList2 = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0 && a.isActive != false).OrderBy(e => e.Account_Code).ToList();
                var notActiveTaxes = db.Taxes.Where(a => a.IsActive == false).ToList();
                foreach (var notActiveTax in notActiveTaxes)
                {
                    foreach (var account in newList2.ToList())
                    {
                        if (account.Account_Code == notActiveTax.Tax_Code)
                        {
                            newList2.Remove(account);
                        }
                    }

                }
                mastervm.accountList = new SelectList(newList2, "Account_Id", "Account_Name");
                var newList = from subled in db.SubLedgers.Where(a => a.isActive != false)
                              select new
                              {

                                  Id = subled.SubLed_Id,
                                  Name = subled.SubLed_Name + "        " + subled.SubLed_Code


                              };
                mastervm.SubLedgerList = new SelectList(newList, "Id", "Name");

                mastervm.GLTypeList = new SelectList(db.GL_Type, "GL_Type_Id", "GL_Type_Name");
                mastervm.CurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");



                mastervm.childList = new SelectList(db.SubLedgers.Where(a => a.isActive != false), "SubLed_Id", "SubLed_Name");
                mastervm.GLCode = gl.GLCode;
                mastervm.detailForThatMaster = new List<GLDetailVM>();
                foreach (var detailvm in gl.GLDetails)
                {
                    var vm = new GLDetailVM();
                    vm.GLDetail_Debit = detailvm.GLDetail_Debit;
                    vm.GLDetail_Credit = detailvm.GLDetail_Credit;
                    vm.Account_Name = detailvm.Account_Name;
                    vm.Account_Id = detailvm.Account_Id;
                    if (detailvm.SubLedger_Id > 0)
                    {
                        var sub = db.SubLedgers.Find(detailvm.SubLedger_Id);
                        vm.Subledger_Name = sub.SubLed_Name;
                        vm.SubLedger_Id = sub.SubLed_Id;
                    }
                    if (detailvm.Account_Child_ID > 0)
                    {
                        var sub = db.Accounts.Find(detailvm.Account_Child_ID);
                        vm.Account_Child_ID = sub.Accounts_Id;
                        vm.Account_Child_Name = sub.Account_Name;
                    }
                    vm.Description_Detail = detailvm.Description_Detail;
                    vm.GLDetail_Debit = detailvm.GLDetail_Debit;
                    vm.GLDetail_Debit = detailvm.GLDetail_Debit;

                    mastervm.detailForThatMaster.Add(vm);
                }
                mastervm.GLMaster_Id = gl.GLMaster_Id;

                return mastervm;
            }
            else
            {
                return null;
            }
        }
        public static int EditPost(GLMasterVM glmasvm)
        {
            var db = new AccountingContainer();
            OperationRun.Addnewoperation(glmasvm.GLCode);
            var operation = db.OperationRuns.Where(a => a.Code == glmasvm.GLCode).Count();
            if (operation != 1)
            {

                return db.GLMasters.Where(a => a.GLState == TransactionState.Post).Where(a => a.GLState == TransactionState.Post).OrderByDescending(c => c.GLMaster_Id).Where(a => a.GLCode.StartsWith("GL-")).FirstOrDefault().GLMaster_Id;
            }
            var glmas = db.GLMasters.Find(glmasvm.GLMaster_Id);
            glmas.GLMaster_Id = glmasvm.GLMaster_Id;
            glmas.GL_Date = Convert.ToDateTime(glmasvm.GL_Date);
            glmas.GL_Desc = glmasvm.GL_Desc;




            glmas.Account_Child_ID = glmasvm.Account_Child_ID;

            var CompanyResult = db.Companies.FirstOrDefault();
            var CurrencyResult = db.Currencies.FirstOrDefault();

            if (CompanyResult != null)
            {

                glmas.Company_Id = CompanyResult.Comp_Id;

            }
            if (CurrencyResult != null)
            {

                glmas.Currency_Id = CurrencyResult.Currency_Id.ToString();

            }
            List<GLDetail> oldList = glmas.GLDetails.ToList();

            foreach (var item in glmasvm.detailForThatMaster)
            {
                var detail = new GLDetail();
                detail.GLDetail_Id = item.GLDetail_Id;
                detail.GLDetail_Debit = item.GLDetail_Debit;
                detail.GLDetail_Credit = item.GLDetail_Credit;
                detail.Account_Id = item.Account_Id;
                detail.CostDistrbutionSubldger = false;
                var name = db.ChartOfAccountings.Find(detail.Account_Id);
                detail.Account_Name = name.Account_Name;
                detail.Bu_Id = 0;
                detail.Asset_Id = item.Asset_Id;
                detail.Description_Detail = item.Description_Detail;
                detail.SubLedger_Id = item.SubLedger_Id;
                var AccountCode = db.ChartOfAccountings.Find(item.Account_Id).Account_Code;
                if ((AccountCode.StartsWith("05") || AccountCode.StartsWith("04")) && (item.SubLedger_Id == 0 || item.SubLedger_Id == null))
                {
                    try
                    {
                        detail.SubLedger_Id = db.SubLedgers.FirstOrDefault().SubLed_Id;
                    }
                    catch
                    {
                        detail.SubLedger_Id = 0;
                    }

                }
                detail.Account_Child_ID = item.Account_Child_ID;
                if (detail.Account_Child_ID != 0)
                {

                    detail.Account_CHild_Name = db.Accounts.Find(detail.Account_Child_ID).Account_Name;
                }
                detail.GLMasterGLMaster_Id = glmasvm.GLMaster_Id;
                glmas.GLDetails.Add(detail);
            }
            db.GLDetails.RemoveRange(oldList);

            glmas.GLState = TransactionState.Post;
            db.Entry(glmas).State = EntityState.Modified;

            db.SaveChanges();
            GetBalanceForAccount(glmas);
            GetBalanceForSubLedger(glmas);
            //make abatch number for GL

            OperationRun.DeleteOperation(glmasvm.GLCode);
            db.Dispose();
            return glmas.GLMaster_Id;
        }

        public static GLMasterVM fillToCreateForSettlement(string code)
        {

            var db = new AccountingContainer();
            var mastervm = new GLMasterVM();
            var newList2 = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0 && a.isActive != false).OrderBy(e => e.Account_Code).ToList();
            var notActiveTaxes = db.Taxes.Where(a => a.IsActive == false).ToList();
            foreach (var notActiveTax in notActiveTaxes)
            {
                foreach (var account in newList2.ToList())
                {
                    if (account.Account_Code == notActiveTax.Tax_Code)
                    {
                        newList2.Remove(account);
                    }

                }
            }
                mastervm.accountList = new SelectList(newList2, "Account_Id", "Account_Name");
                mastervm.AccountSettlementList = new SelectList(newList2.Where(a => a.Account_Code.StartsWith("04") || a.Account_Code.StartsWith("05") || a.Account_Code.StartsWith("06")), "Account_Id", "Account_Name");

                var newList = from subled in db.SubLedgers.Where(a => a.isActive != false)
                              select new
                              {

                                  Id = subled.SubLed_Id,
                                  Name = subled.SubLed_Name + "        " + subled.SubLed_Code


                              };
                mastervm.SubLedgerList = new SelectList(newList, "Id", "Name");

                mastervm.childList = new SelectList(db.SubLedgers.Where(a => a.isActive != false), "SubLed_Id", "SubLed_Name");

                var lastCode = db.GLMasters.Where(a => a.GLState == TransactionState.Post).OrderByDescending(c => c.GLMaster_Id).Where(a => a.GLCode.StartsWith("GL-")).FirstOrDefault();
                if (lastCode == null)
                {
                    mastervm.GLCode = "GL-001";
                }
                else
                {
                    //using string substring method to get the number of the last inserted
                    mastervm.GLCode = "GL-" + (Convert.ToInt32(lastCode.GLCode.Substring(3, lastCode.GLCode.Length - 3)) + 1).ToString("D3");
                }
            if (code !=null)
            {
                if (code.StartsWith("SUP-"))
                {
                    var supplier = db.Accounts.Where(a => a.Account_Code == code).FirstOrDefault();
                    mastervm.selectedAccount = 45;
                    mastervm.childList = new SelectList(db.Accounts.Where(p => p.Account_Id == mastervm.selectedAccount.ToString()), "Accounts_Id", "Account_Name");

                    mastervm.Account_Child_ID = supplier.Accounts_Id;
                }

                else if (code.StartsWith("CUST-"))
                {
                    var cust = db.Accounts.Where(a => a.Account_Code == code).FirstOrDefault();
                    mastervm.selectedAccount = 37;
                    mastervm.childList = new SelectList(db.Accounts.Where(p => p.Account_Code.StartsWith("CUST-") ), "Accounts_Id", "Account_Name");

                    mastervm.Account_Child_ID = cust.Accounts_Id;
             
                }
                else if (code.StartsWith("EMP-"))
                {
                    var cust = db.Accounts.Where(a => a.Account_Code == code).FirstOrDefault();
                    mastervm.selectedAccount = 39;
                    mastervm.childList = new SelectList(db.Accounts.Where(p => p.Account_Code.StartsWith("EMP-")), "Accounts_Id", "Account_Name");

                    mastervm.Account_Child_ID = cust.Accounts_Id;

                }
                mastervm.RealBallance = getbalanceChildAccount(mastervm.Account_Child_ID);
            }
         


            return mastervm;
        }

        public static int createGLStellement(GLMasterVM glmasvm, String UserId)
        {
            var db = new AccountingContainer();
            
            var glmas = new GLMaster();
            glmas.GLMaster_Id = glmasvm.GLMaster_Id;
            glmas.GL_Date = Convert.ToDateTime(glmasvm.GL_Date);
            glmas.GL_Desc = glmasvm.GL_Desc;
         
                if (db.GLMasters.Where(a => a.GLCode.StartsWith("GL-")).FirstOrDefault() == null)
                {
                    glmas.GLCode = "GL-001";

                }
                else
                {
                    var lastCode = db.GLMasters.Where(a => a.GLState == TransactionState.Post).Where(a => a.GLState == TransactionState.Post).OrderByDescending(c => c.GLMaster_Id).Where(a => a.GLCode.StartsWith("GL-")).FirstOrDefault();
                    glmas.GLCode = "GL-" + (Convert.ToInt32(lastCode.GLCode.Substring(3, lastCode.GLCode.Length - 3)) + 1).ToString("D3");

                }
           
           



            glmas.Account_Child_ID = glmasvm.Account_Child_ID;

            var CompanyResult = db.Companies.FirstOrDefault();
            var CurrencyResult = db.Currencies.FirstOrDefault();

            if (CompanyResult != null)
            {

                glmas.Company_Id = CompanyResult.Comp_Id;

            }
            if (CurrencyResult != null)
            {

                glmas.Currency_Id = CurrencyResult.Currency_Id.ToString();

            }
         
            decimal oldbalance = 0;
            var chart = db.ChartOfAccountings.Find(glmasvm.selectedAccount);
            var detail = new GLDetail();
            if (glmasvm.Account_Child_ID > 0)
            {
                var Account = db.Accounts.Find(glmasvm.Account_Child_ID);
              oldbalance=Accounts.RefreshTheBallanceOfCustomer(Account.Account_Code);
                detail.Account_Child_ID = Account.Accounts_Id;
                detail.Account_CHild_Name = Account.Account_Name;
            }
            else
            {
              
                 oldbalance = ChartOfAccounting.GetBallanceOfAccount(chart.Account_Code);
            }
            
             detail.Account_Id = chart.Account_Id;
            detail.Account_Name = chart.Account_Name;

            decimal balanceWant = glmasvm.debit - glmasvm.Credit;
            decimal value = balanceWant - oldbalance;
            var AccountCode = db.ChartOfAccountings.Find(detail.Account_Id).Account_Code;
            if ((AccountCode.StartsWith("05") || AccountCode.StartsWith("04")) && (glmasvm.SubLed_Id == 0 || glmasvm.SubLed_Id == null))
            {
                try
                {
                    detail.SubLedger_Id = db.SubLedgers.FirstOrDefault().SubLed_Id;
                }
                catch
                {
                    detail.SubLedger_Id = 0;
                }

            }

            var detail2 = new GLDetail();
             
             
                detail2.Account_Id = glmasvm.AccountSettlement;
                detail2.CostDistrbutionSubldger = false;
                var charrt = db.ChartOfAccountings.Find(detail2.Account_Id);
                detail2.Account_Name = charrt.Account_Name;
              
                detail2.SubLedger_Id = glmasvm.SubLed_Id;
                var AccountCode1 = db.ChartOfAccountings.Find(charrt.Account_Id).Account_Code;
                if ((AccountCode1.StartsWith("05") || AccountCode1.StartsWith("04")) && (glmasvm.SubLed_Id == 0 || glmasvm.SubLed_Id == null))
                {
                    try
                    {
                        detail2.SubLedger_Id = db.SubLedgers.FirstOrDefault().SubLed_Id;
                    }
                    catch
                    {
                        detail2.SubLedger_Id = 0;
                    }

                }
              
            if (value > 0)
            {
                detail.GLDetail_Debit = value;
                detail2.GLDetail_Credit = value;
            }
            else
            {
                detail.GLDetail_Credit = value * -1;
                detail2.GLDetail_Debit = value*-1;
            }
            glmas.GLDetails.Add(detail);
            glmas.GLDetails.Add(detail2);



            glmas.GLType_Id = "تسوية حسابية";


            glmas.GLState = TransactionState.Post;
            db.GLMasters.Add(glmas);

            db.SaveChanges();
            GetBalanceForAccount(glmas);
            GetBalanceForSubLedger(glmas);
            //make abatch number for GL
            var trans = new TransactionBatch();
            trans.UserId = UserId;
            trans.OperationName = "قيد يومى ";
            trans.Date = glmas.GL_Date;
            trans.OperationId = glmas.GLMaster_Id;
            trans.Description = glmas.GL_Desc;
            trans.Operation_Code = glmas.GLCode;
            var transactionExp = TransactionBatch.AddNewTransaction(trans);
            OperationRun.DeleteOperation(glmasvm.GLCode);
            db.Dispose();
            return glmas.GLMaster_Id;
        }
        /// <summary>
        /// 
        /// make filteration in sub Account only When Choose cutomer and Supplier and Employee 
        /// </summary>
        /// <param name="acc_ID"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetAccountLedger(int acc_ID)
        {
            var db = new AccountingContainer();
            var thischar = db.ChartOfAccountings.Find(acc_ID);
            var chartid = thischar.Account_Id.ToString();
            if (thischar.Account_Id == 50)
            {
                chartid = "39";
            }
         
            IEnumerable<SelectListItem> accountled;

            if (thischar.Account_Code == "010109" || thischar.Account_Code == "020101" || thischar.Account_Code == "010111"||thischar.Account_Code== "020105")
            {
                IEnumerable<SelectListItem> AccountLedger = new SelectList(db.Accounts.Where(p => p.Account_Id == chartid && p.OpenBalance_Code != "1").ToList(), "Accounts_Id", "Account_Name");
                accountled = AccountLedger;
            }
            else
            {
                IEnumerable<SelectListItem> AccountLedger = new SelectList(db.ChartOfAccountings.Where(p => p.ChartOfAccountingAccount_Id == acc_ID).ToList(), "Account_Id", "Account_Name");
                accountled = AccountLedger;
            }


            // = db.Provinces.Where(p => p.CountryId == countryId).ToList();
            return accountled;
        }

        /// <summary>
        /// make filteration in sub Account only When Choose cutomer and Supplier and Employee 
        /// </summary>
        /// <param name="acc_ID"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetChildOfParent(int acc_ID)
        {
            var db = new AccountingContainer();
            var thischar = db.ChartOfAccountings.Find(acc_ID);
            var listchild = db.ChartOfAccountings.Where(p => p.ChartOfAccountingAccount_Id == acc_ID).ToList();
            IEnumerable<SelectListItem> AccountLedger = new SelectList(listchild.Where(a => a.isActive != false), "Account_Id", "Account_Name");
            return AccountLedger;
        }
        public static IEnumerable<SelectListItem> GetsubCredit(int acc_ID)
        {
            var db = new AccountingContainer();
            var thischar = db.ChartOfAccountings.Find(acc_ID);
            var listchild = db.ChartOfAccountings.Where(p => p.ChartOfAccountingAccount_Id == acc_ID).ToList();
            IEnumerable<SelectListItem> AccountLedger = new SelectList(listchild.Where(a => a.isActive != false), "Account_Id", "Account_Name");
            return AccountLedger;
        }
        //تقوم بأنشاء قيد يومى بتفاصيلة المختلفة و حفظه فى قاعدة البيانات
        public static int createGL(GLMasterVM glmasvm, String UserId)
        {
            var db = new AccountingContainer();
            OperationRun.Addnewoperation(glmasvm.GLCode);
            var operation = db.OperationRuns.Where(a => a.Code == glmasvm.GLCode).Count();
            if (operation != 1)
            {

                return db.GLMasters.Where(a => a.GLState == TransactionState.Post).Where(a => a.GLState == TransactionState.Post).OrderByDescending(c => c.GLMaster_Id).Where(a => a.GLCode.StartsWith("GL-")).FirstOrDefault().GLMaster_Id;
            }
            var glmas = new GLMaster();
            glmas.GLMaster_Id = glmasvm.GLMaster_Id;
            glmas.GL_Date = Convert.ToDateTime(glmasvm.GL_Date);
            glmas.GL_Desc = glmasvm.GL_Desc;
            if (glmasvm.GLCode.StartsWith("GL-"))
            {
                if (db.GLMasters.Where(a => a.GLCode.StartsWith("GL-")).FirstOrDefault() == null)
                {
                    glmas.GLCode = "GL-001";

                }
                else
                {
                    var lastCode = db.GLMasters.Where(a => a.GLState == TransactionState.Post).Where(a => a.GLState == TransactionState.Post).OrderByDescending(c => c.GLMaster_Id).Where(a => a.GLCode.StartsWith("GL-")).FirstOrDefault();
                    glmas.GLCode = "GL-" + (Convert.ToInt32(lastCode.GLCode.Substring(3, lastCode.GLCode.Length - 3)) + 1).ToString("D3");

                }
            }
            else
            {
                if (db.GLMasters.Where(a => a.GLState == TransactionState.Post).Where(a => a.GLCode.StartsWith("GLST-")).FirstOrDefault() == null)
                {
                    glmas.GLCode = "GLST-001";

                }
                else
                {
                    var lastCode = db.GLMasters.Where(a => a.GLState == TransactionState.Post).OrderByDescending(c => c.GLMaster_Id).Where(a => a.GLCode.StartsWith("GLST")).FirstOrDefault();
                    glmas.GLCode = "GLST-" + (Convert.ToInt32(lastCode.GLCode.Substring(5, lastCode.GLCode.Length - 5)) + 1).ToString("D3");
                }
            }



            glmas.Account_Child_ID = glmasvm.Account_Child_ID;

            var CompanyResult = db.Companies.Find(glmasvm.Company_Id);
            var CurrencyResult = db.Currencies.Find(glmasvm.Currency_Id);

            if (CompanyResult != null)
            {

                glmas.Company_Id = CompanyResult.Comp_Id;

            }
            if (CurrencyResult != null)
            {

                glmas.Currency_Id = CurrencyResult.Currency_Id.ToString();

            }

            foreach (var item in glmasvm.detailForThatMaster)
            {
                var detail = new GLDetail();
                detail.GLDetail_Id = item.GLDetail_Id;
                detail.GLDetail_Debit = item.GLDetail_Debit;
                detail.GLDetail_Credit = item.GLDetail_Credit;
                detail.Account_Id = item.Account_Id;
                detail.CostDistrbutionSubldger = false;
                var name = db.ChartOfAccountings.Find(detail.Account_Id);
                detail.Account_Name = name.Account_Name;
                detail.Bu_Id = 0;
                detail.Asset_Id = item.Asset_Id;
                detail.Description_Detail = item.Description_Detail;
                detail.SubLedger_Id = item.SubLedger_Id;
                var AccountCode = db.ChartOfAccountings.Find(item.Account_Id).Account_Code;
                if ((AccountCode.StartsWith("05") || AccountCode.StartsWith("04")) && (item.SubLedger_Id == 0 || item.SubLedger_Id == null))
                {
                    try
                    {
                        detail.SubLedger_Id = db.SubLedgers.FirstOrDefault().SubLed_Id;
                    }
                    catch
                    {
                        detail.SubLedger_Id = 0;
                    }

                }
                detail.Account_Child_ID = item.Account_Child_ID;
                if (detail.Account_Child_ID != 0)
                {

                    detail.Account_CHild_Name = db.Accounts.Find(detail.Account_Child_ID).Account_Name;
                }
                detail.GLMasterGLMaster_Id = glmasvm.GLMaster_Id;
                glmas.GLDetails.Add(detail);
            }
            glmas.GLType_Id = "قيد يومية";

            if (glmas.GLCode.Contains("GLST"))
            {
                glmas.Auto = true;
                glmas.GLType_Id = "قيد افتتاحى";
            }
            else { glmas.Auto = false; }
            glmas.GLState = TransactionState.Post;
            db.GLMasters.Add(glmas);

            db.SaveChanges();
            GetBalanceForAccount(glmas);
            GetBalanceForSubLedger(glmas);
            //make abatch number for GL
            var trans = new TransactionBatch();
            trans.UserId = UserId;
            trans.OperationName = "قيد يومى ";
            trans.Date = glmas.GL_Date;
            trans.OperationId = glmas.GLMaster_Id;
            trans.Description = glmas.GL_Desc;
            trans.Operation_Code = glmas.GLCode;
            var transactionExp = TransactionBatch.AddNewTransaction(trans);
            OperationRun.DeleteOperation(glmasvm.GLCode);
            db.Dispose();
            return glmas.GLMaster_Id;
        }
        //حذف القيد اليومى عن طريق Id
        public static int deleteGl(int glMas)
        {
            var db = new AccountingContainer();
            var Glmas = db.GLMasters.Find(glMas);


            var detail = Glmas.GLDetails;

            db.GLDetails.RemoveRange(detail);



            db.GLMasters.Remove(Glmas);
            db.SaveChanges();
            db.Dispose();
            return 1;
        }

        /// <summary>
        /// change Account ballance and all masteres of that Account
        /// </summary>
        /// <param name="GL"></param>
        public static void GetBalanceForAccount(GLMaster GL)
        {
            var db = new AccountingContainer();
            foreach (var detail in GL.GLDetails)
            {

                var minusCredit = detail.GLDetail_Credit * -1;
                var balance = minusCredit + detail.GLDetail_Debit;
                var account = db.ChartOfAccountings.Find(detail.Account_Id);

                account.Account_Balance = account.Account_Balance + balance;
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                var parent = account.ChartOfAccountingAccount_Id;
                while (parent != null)
                {
                    var parentDetail = db.ChartOfAccountings.Find(parent);
                    parentDetail.Account_Balance = parentDetail.Account_Balance + balance;
                    db.Entry(parentDetail).State = EntityState.Modified;
                    db.SaveChanges();
                    parent = parentDetail.ChartOfAccountingAccount_Id;
                }



            }
            db.SaveChanges();
            db.Dispose();
        }
        /// <summary>
        /// change Account ballance and all masteres of that Account
        /// </summary>
        /// <param name="GL"></param>
        public static void GetBalanceForSubLedger(GLMaster GL)
        {
            var db = new AccountingContainer();

            foreach (var detail in GL.GLDetails)
            {
                if (detail.Account_Child_ID != 0 && detail.Account_Child_ID != null)
                {
                    var minusCrdit = detail.GLDetail_Credit * -1;
                    var balance = minusCrdit + detail.GLDetail_Debit;
                    var acc = db.Accounts.Find(detail.Account_Child_ID);

                    acc.Account_Balance = acc.Account_Balance.Value + balance;
                    db.Entry(acc).State = EntityState.Modified;
                    db.SaveChanges();
                }

                if (detail.SubLedger_Id != 0 && detail.SubLedger_Id != null)
                {

                    var minusCredit = detail.GLDetail_Credit * -1;
                    var balance = minusCredit + detail.GLDetail_Debit;
                    var subledger = db.SubLedgers.Find(detail.SubLedger_Id);

                    subledger.SubLedger_Balance = subledger.SubLedger_Balance + balance;
                    db.Entry(subledger).State = EntityState.Modified;
                    db.SaveChanges();
                    var parent = subledger.SubLedgerSubLed_Id;
                    while (parent != null)
                    {
                        var parentDetail = db.SubLedgers.Find(parent);
                        parentDetail.SubLedger_Balance = parentDetail.SubLedger_Balance.Value + balance;
                        db.Entry(parentDetail).State = EntityState.Modified;
                        db.SaveChanges();
                        parent = parentDetail.SubLedgerSubLed_Id;




                    }





                }

                db.SaveChanges();
            }


        }
        /// <summary>
        /// fill for Create start gl that crate unique  code for Each GL Star
        /// </summary>
        /// <returns></returns>
        public static GLMasterVM fillToCreateStartingGL()
        {
            var db = new AccountingContainer();
            var mastervm = new GLMasterVM();
            IQueryable<object> newList2 = ChartOfAccounting.getAlldataForGL();



            mastervm.accountList = new SelectList(newList2, "Id", "Name");

            var newList = from subled in db.SubLedgers.Where(a => a.isActive != false)
                          select new {

                              Id = subled.SubLed_Id,
                              Name = subled.SubLed_Name + "        " + subled.SubLed_Code


                          };

            mastervm.SubLedgerList = new SelectList(newList, "Id", "Name");
            mastervm.CompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            mastervm.GLTypeList = new SelectList(db.GL_Type, "GL_Type_Id", "GL_Type_Name");
            mastervm.CurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");


            //mastervm.BUList = new SelectList(db.BussinesUnits, "BU_Id", "BU_Name");
            mastervm.AssetList = new SelectList(db.FixedAssets, "Fasset_ID", "FAsset_Type");
            mastervm.childList = new SelectList(db.SubLedgers.Where(a => a.isActive != false), "SubLed_Id", "SubLed_Name");

            mastervm.GLType_Id = 2;
            var lastCode = db.GLMasters.Where(a => a.GLState == TransactionState.Post).OrderByDescending(c => c.GLMaster_Id).Where(a => a.GLCode.StartsWith("GLST")).FirstOrDefault();
            if (lastCode == null)
            {
                mastervm.GLCode = "GLST-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                mastervm.GLCode = "GLST-" + (Convert.ToInt32(lastCode.GLCode.Substring(5, lastCode.GLCode.Length - 5)) + 1).ToString("D3");
            }

            return mastervm;
        }

        /// <summary>
        /// تقوم بجلب المستند صاحب القيد لمعرفة تفاصيلة
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static GLDetailHelper SendToDetailView(String code)
        {
            var db = new AccountingContainer();
            var GLHelper = new GLDetailHelper();
            if (code.StartsWith("GL-"))
            {
                GLHelper.id = db.GLMasters.Where(a => a.GLCode == code).FirstOrDefault().GLMaster_Id;
                GLHelper.Name = "GL";


            }
            else if (code.StartsWith("SAL-"))
            {
                GLHelper.id = db.InvoiceMasters.Where(a => a.Serial_Number == code).FirstOrDefault().Master_Id;
                GLHelper.Name = "SAL";


            }
            else if(code.StartsWith("SALRC-"))
            {
                GLHelper.id = db.SalesReceiptMasters.Where(a => a.Serial_Number == code).FirstOrDefault().Master_Id;
                GLHelper.Name = "SALRC";


            }
            else if (code.StartsWith("PUR-"))
            {
                GLHelper.id = db.PurInvoiceMasters.Where(a => a.Serial_Number == code).FirstOrDefault().Master_Id;
                GLHelper.Name = "PUR";


            }
            else if (code.StartsWith("PRCT-"))
            {
                GLHelper.id = db.ProjectRecipts.Where(a => a.ReciptCode == code).FirstOrDefault().ProjectRect_ID;
                GLHelper.Name = "PRCT";


            }
            else if (code.StartsWith("PAY-"))
            {
                GLHelper.id = db.Payments.Where(a => a.Pay_Number == code).FirstOrDefault().Pay_ID;
                GLHelper.Name = "PAY";

                return GLHelper;
            }
            else if (code.StartsWith("REC-"))
            {
                GLHelper.id = db.Receipts.Where(a => a.Rec_Number == code).FirstOrDefault().Rec_ID;
                GLHelper.Name = "REC";


            }
            else if (code.StartsWith("REVP-"))
            {
                GLHelper.id = db.Receipts.Where(a => a.Rec_Number == code).FirstOrDefault().Rec_ID;
                GLHelper.Name = "REVP";


            }
            else if (code.StartsWith("REVS-"))
            {
                GLHelper.id = db.ReverseSalesInvoiceMasters.Where(a => a.Serial_Number == code).FirstOrDefault().Master_Id;
                GLHelper.Name = "REVS";


            }
            else if (code.StartsWith("EXP-"))
            {
                GLHelper.id = db.Expenses.Where(a => a.Expense_Code == code).FirstOrDefault().Expense_Id;
                GLHelper.Name = "EXP";


            }
            else if (code.StartsWith("REVSa-"))
            {
                GLHelper.id = db.ReverseSalesReceiptMasters.Where(a => a.Serial_Number == code).FirstOrDefault().Master_Id;
                GLHelper.Name = "REVSa";


            }
            else if (code.StartsWith("SUBS-"))
            {
                GLHelper.id = db.Subscriptions.Where(a => a.Subscrip_Code == code).FirstOrDefault().Subscrip_Id;
                GLHelper.Name = "SUBS";


            }
            else if (code.StartsWith("SUBINV-"))
            {
                GLHelper.id = db.SubscriptionInvoices.Where(a => a.Serial_Number == code).FirstOrDefault().Inv_ID;
                GLHelper.Name = "SUBINV";


            }
            else if (code.StartsWith("PROPAY-"))
            {
                GLHelper.id = db.ProjectPayments.Where(a => a.ProjectPay_Code == code).FirstOrDefault().ProjectPaymentId;
                GLHelper.Name = "PROPAY";


            }
            else if (code.StartsWith("PINV-"))
            {
                GLHelper.id = db.ProjectInvoiceMasters.Where(a => a.Serial_Number == code).FirstOrDefault().PInvoiceMaster_ID;
                GLHelper.Name = "PINV";


            }
            else if (code.StartsWith("COSDS-"))
            {
                GLHelper.id = db.CostDistributions.Where(a => a.Cost_Distr_Code == code).FirstOrDefault().Id;
                GLHelper.Name = "COSDS";


            }
            else if(code.StartsWith("SPRE-"))
            {
                GLHelper.id = db.SpentRecipts.Where(a => a.SpentReceipt_Code == code).FirstOrDefault().SpentReceipt_Id;
                GLHelper.Name = "SPRE";


            }

            else if(code.StartsWith("MATPAY-"))
            {
                GLHelper.id = db.MatchingPayments.Where(a => a.Match_Code == code).FirstOrDefault().Match_Id;
                GLHelper.Name = "MATPAY";
            }
            else if(code.StartsWith("CARECI-"))
            {
                GLHelper.id = db.CatchReceipts.Where(a => a.CatchReceipt_Code == code).FirstOrDefault().CatchReceipt_Id;
                GLHelper.Name = "CARECI";
            }

            else if(code.StartsWith("TMain-"))
            {
                GLHelper.id = db.TransferToMaintenances.Where(a => a.Trans_Code == code).FirstOrDefault().Master_Id;
                GLHelper.Name = "TMain";
            }
            else if(code.StartsWith("TTPRO-"))
            {
                GLHelper.id = db.TransferToProjectMasters.Where(a => a.Trans_Code == code).FirstOrDefault().Master_Id;
                GLHelper.Name = "TTPRO";
            }
            else if (code.StartsWith("AS-"))
            {
                GLHelper.id = db.FixedAssets.Where(a => a.FAsset_Code == code).FirstOrDefault().Fasset_ID;
                GLHelper.Name = "AS";
            }
            else if (code.StartsWith("PRO-"))
            {
                GLHelper.id = db.Products.Where(a => a.Prod_Code == code).FirstOrDefault().Prod_ID;
                GLHelper.Name = "PRO";
            }
            else if (code.StartsWith("TRS-"))
            {
                GLHelper.id = db.InventoryTransfer_Hd.Where(a => a.Inv_Code == code).FirstOrDefault().TransferHd_Id;
                GLHelper.Name = "TRS";
            }
            else if (code.StartsWith("SETSTO-"))
            {
                GLHelper.id = db.SettlementOfStores.Where(a => a.Settlement_Code == code).FirstOrDefault().SettlementId;
                GLHelper.Name = "SETSTO";
            }
            else if (code.StartsWith("POS-"))
            {
                GLHelper.id = db.MasterPOSInvoices.Where(a => a.Serial_Number == code).FirstOrDefault().MasterPOSInvoiceId;
                GLHelper.Name = "POS";
            }
            else if (code.StartsWith("REVPOS-"))
            {
                GLHelper.id = db.ReversePOSInvoices.Where(a => a.Serial_Number == code).FirstOrDefault().Master_Id;
                GLHelper.Name = "REVPOS";
            }
            else if (code.StartsWith("ORD-"))
            {
                GLHelper.id = db.Orders.Where(a => a.OrderNumber == code).FirstOrDefault().Id;
                GLHelper.Name = "ORD";
            }
            else if (code.StartsWith("RORD-"))
            {
                GLHelper.id = db.ReverseOrders.Where(a => a.OrderNumber == code).FirstOrDefault().Id;
                GLHelper.Name = "RORD";
            }
            else 
            {
                GLHelper.id = db.GLMasters.Where(a => a.GLCode == code).FirstOrDefault().GLMaster_Id;
                GLHelper.Name = "";
            }
            return GLHelper;
        }
        /// <summary>
        /// تقوم بجلب المستند صاحب القيد لمعرفة تفاصيلة عن طريق كوده
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static GLDetailHelper SendToDetailViewOpenBalance(String code)
        {
            var db = new AccountingContainer();
            var GLHelper = new GLDetailHelper();
            if (code.StartsWith("GLST-"))
            {
                GLHelper.id = db.GLMasters.Where(a => a.GLCode == code).FirstOrDefault().GLMaster_Id;
                GLHelper.Name = "GLST";
            }
            if (code.StartsWith("OPA-"))
            {
                GLHelper.id = db.FixedAssets.Where(a => a.OpenBalance_Code == code).FirstOrDefault().Fasset_ID;
                GLHelper.Name = "OPA";
            }
            if (code.StartsWith("PRO-"))
            {
                GLHelper.id = db.Products.Where(a => a.OpenBalance_Code == code).FirstOrDefault().Prod_ID;
                GLHelper.Name = "PRO";
            }
            if (code.StartsWith("OPB-"))
            {
                GLHelper.id = db.Banks.Where(a => a.Balance_Code == code).FirstOrDefault().Bank_ID;
                GLHelper.Name = "OPB";
            }

            return GLHelper;
        }


        /// <summary>
        /// fill Creation off gl and chosse the Account
        /// </summary>
        /// <returns></returns>
        public static OffGlMasterVM MakingOffGlGet() {

            var offgl = new OffGlMasterVM();
            String StartCodeOfOwner = "030";
            var db = new AccountingContainer();

            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0).ToList();

            var lastCode = db.GLMasters.OrderByDescending(c => c.GLMaster_Id).Where(a => a.GLCode.StartsWith("GLOF-")).FirstOrDefault();
            if (lastCode == null)
            {
                offgl.GlCode = "GLOF-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                offgl.GlCode = "GLOF-" + (Convert.ToInt32(lastCode.GLCode.Substring(5, lastCode.GLCode.Length - 5)) + 1).ToString("D3");
            }

            offgl.OwnerList = new SelectList(lista.Where(a => a.Account_Code.StartsWith(StartCodeOfOwner) && a.isActive != false), "Account_Id", "Account_Name");

            return offgl;
        }


        /// <summary>
        /// Create Off gl and close any gl Posted and make start Gl in second day
        /// يجب ان يكون تاريخ اقفال الشركة اقل من تاريخ اليوم لكي يتم القيد
        /// </summary>
        /// <returns></returns>
        public static String makingOffglPost(OffGlMasterVM offGl)
        {

            var db = new AccountingContainer();
            TrialBalncePostVM trialba = new TrialBalncePostVM();
            TrialBalncePostVM trialba2 = new TrialBalncePostVM();
            var time = isThatTimeisSuitable();
            trialba.Start_Date = time.Item1.Startdate;
            trialba.End_Date = time.Item1.Enddate;
            offGl.Startdate = time.Item1.Startdate;
            offGl.Enddate = time.Item1.Enddate;
            decimal totalCredit = 0;
            decimal totalDepit = 0;
            decimal dif = 0;
            if (time.Item2 == false)
            {
                return "لا تستطيع الاقفال الان استنادا لتاريخ الاقفال المسجل عند انشاء الشركة";
            }
            trialba.WithOutOFFGLs = true;
            trialba2 = Reports.makeTrialOthermethod(trialba);
            var Start = Convert.ToDateTime(trialba.Start_Date);
            var End = Convert.ToDateTime(trialba.End_Date).AddHours(23).AddMinutes(59).AddSeconds(59);
            var wasset = db.GLDetails.Where(a=>a.Account_Id==131&&a.GLMaster.GL_Date<= End).ToList();
      
            try { dif = wasset.Select(a => a.GLDetail_Credit).Sum() - wasset.Select(a => a.GLDetail_Debit).Sum(); }
            catch
            {
                dif = 0;
            }

            if (Math.Round(dif, 2, MidpointRounding.AwayFromZero) != 0)
            {
                return "برجاء تسوية حساب الوسيط حتى يتم قيد الاقفال";
            }
       

            var ListOfPending = AllPendingDocumanets.GetAllPendings().Where(e => e.date >= Start && e.date <= End).Count();
            if (ListOfPending != 0)
            {

                return "بعض الفواتير لم يتم تفعيلها فى الحسابات ";

            }



            else
            {
                AllPendingDocumanets.TransferAllOperationToOff(Start, End);
                var gl = new GLMaster();
                gl.GL_Date = Convert.ToDateTime(trialba.End_Date);
                gl.GLType_Id = "قيد اقفال";
                gl.Company_Id = 1;
                gl.Currency_Id = "1";
                var lastCode = db.GLMasters.OrderByDescending(c => c.GLMaster_Id).Where(a => a.GLCode.StartsWith("GLOF-")).FirstOrDefault();
                if (lastCode == null)
                {
                    gl.GLCode = "GLOF-001";
                }
                else
                {
                    //using string substring method to get the number of the last inserted
                    gl.GLCode = "GLOF-" + (Convert.ToInt32(lastCode.GLCode.Substring(5, lastCode.GLCode.Length - 5)) + 1).ToString("D3");
                }
                gl.GLState = TransactionState.Post;
                gl.GL_Desc = "من" + Start.ToString() + "الى" + End.ToString();
                var account_of_TrilBallance = trialba2.ballanceAttrribute.Where(a => a.Account_Code.StartsWith("05") || a.Account_Code.StartsWith("04"));
                foreach (var account in account_of_TrilBallance)
                {
                    var gldetail = new GLDetail();
                    gldetail.Account_Name = db.ChartOfAccountings.Find(account.Account_Id).Account_Name;
                    gldetail.Account_Id = account.Account_Id;

                    if (account.pureFinalCredit == 0)
                    {

                        gldetail.GLDetail_Credit = account.pureFinaldepit;

                    }
                    else { gldetail.GLDetail_Debit = account.pureFinalCredit; }
                    totalCredit += gldetail.GLDetail_Credit;
                    totalDepit += gldetail.GLDetail_Debit;
                    if (gldetail.GLDetail_Credit == 0 && gldetail.GLDetail_Debit == 0)
                    {

                    }
                    else
                    {
                        gl.GLDetails.Add(gldetail);
                    }
                }
                var gldetail2 = new GLDetail();
                gldetail2.Account_Id = offGl.Owner;
                gldetail2.Account_Name = db.ChartOfAccountings.Find(offGl.Owner).Account_Name;
                if (totalCredit > totalDepit)
                {
                    gldetail2.GLDetail_Debit = totalCredit - totalDepit;

                }
                else
                {
                    gldetail2.GLDetail_Credit = totalDepit - totalCredit;
                }
                if (gldetail2.GLDetail_Debit == 0 && gldetail2.GLDetail_Credit == 0)
                {

                }
                else
                {
                    gl.GLDetails.Add(gldetail2);
                }
                db.GLMasters.Add(gl);

                db.SaveChanges();
                db.Dispose();
                makeingOffglForAssetandOther(offGl);
                return "تم عمل قيد الاقفال بنجاح";
            }
        }
        /// <summary>
        /// عمل قيد افتتاحي للاصول التى تم اقفالها 
        /// </summary>
        /// <param name="offGl"></param>
        public static void makeingOffglForAssetandOther(OffGlMasterVM offGl)
        {
            var db = new AccountingContainer();
            TrialBalncePostVM trialba = new TrialBalncePostVM();
            TrialBalncePostVM trialba2 = new TrialBalncePostVM();

            trialba.Start_Date = offGl.Startdate;
            trialba.End_Date = offGl.Enddate;
            decimal totalCredit = 0;
            decimal totalDepit = 0;
            trialba.WithOutOFFGLs = true;
            trialba2 = Reports.makeTrialOthermethod(trialba);
            var gl = new GLMaster();
            gl.GL_Date = Convert.ToDateTime(trialba.End_Date);
            gl.GLType_Id = "قيد اقفال";
            gl.Company_Id = 1;
            gl.Currency_Id = "1";
            var lastCode = db.GLMasters.OrderByDescending(c => c.GLMaster_Id).Where(a => a.GLCode.StartsWith("GLOF-")).FirstOrDefault();
            if (lastCode == null)
            {
                gl.GLCode = "GLOF-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                gl.GLCode = "GLOF-" + (Convert.ToInt32(lastCode.GLCode.Substring(5, lastCode.GLCode.Length - 5)) + 1).ToString("D3");
            }
            gl.GLState = TransactionState.Post;
            gl.GL_Desc = "من" + trialba.Start_Date.ToString() + "الى" + trialba.End_Date.ToString();
            var account_of_TrilBallance = trialba2.ballanceAttrribute.Where(a => a.Account_Code.StartsWith("01") || a.Account_Code.StartsWith("02") || a.Account_Code.StartsWith("03"));
            foreach (var account in account_of_TrilBallance)
            {
                var gldetail = new GLDetail();
                gldetail.Account_Id = account.Account_Id;
                gldetail.Account_Name = db.ChartOfAccountings.Where(a => a.Account_Id == account.Account_Id).FirstOrDefault().Account_Name;
                if (account.pureFinalCredit == 0)
                {

                    gldetail.GLDetail_Credit = account.pureFinaldepit;

                }
                else { gldetail.GLDetail_Debit = account.pureFinalCredit; }
                totalCredit += gldetail.GLDetail_Credit;
                totalDepit += gldetail.GLDetail_Debit;
                if (gldetail.GLDetail_Credit == 0 && gldetail.GLDetail_Debit == 0)
                {

                }
                else
                {
                    gl.GLDetails.Add(gldetail);
                }
            }
            var gldetail2 = new GLDetail();
            gldetail2.Account_Id = 131;
            gldetail2.Account_Name = db.ChartOfAccountings.Find(offGl.Owner).Account_Name;
            if (totalCredit > totalDepit)
            {
                gldetail2.GLDetail_Debit = totalCredit - totalDepit;

            }
            else
            {
                gldetail2.GLDetail_Credit = totalDepit - totalCredit;
            }
            if (gldetail2.GLDetail_Debit == 0 && gldetail2.GLDetail_Credit == 0)
            {

            }
            else
            {
                gl.GLDetails.Add(gldetail2);
            }
            db.GLMasters.Add(gl);

            db.SaveChanges();
            db.Dispose();

            MakeVoidOfPostingGLOffCloseGL(gl.GLMaster_Id, gl.GL_Date.AddDays(1));


        }

        /// <summary>
        /// that metod check date is suitable to make off gl or not
        /// </summary>
        /// <returns></returns>
        public static Tuple<OffGlMasterVM, Boolean> isThatTimeisSuitable()
        {
            var db = new AccountingContainer();

            var company = db.Companies.FirstOrDefault();
            var star = company.StartFinancialDate.Value;

            var end = company.EndFinancialDate.Value.AddDays(1);
            var gl = new GLMaster();

            try { gl = db.GLMasters.Where(a => a.GLCode.StartsWith("GLOF-")).ToList().Last(); }
            catch
            {
                gl = null;
            }

            OffGlMasterVM timeOfOffGL = new OffGlMasterVM();

            if (gl == null && end <= DateTime.Now)
            {

                timeOfOffGL.Startdate = company.StartFinancialDate.ToString();
                timeOfOffGL.Enddate = company.EndFinancialDate.ToString();
                return Tuple.Create(timeOfOffGL, true);


            }
            else
            {

                var DiferenceTime = company.EndFinancialDate - company.StartFinancialDate;
                var expectTime = new DateTime();
                var start_date = new DateTime();
                if (gl == null) {

                    start_date = company.StartFinancialDate.Value;


                }
                else
                {
                    if (db.GLMasters.Where(a => a.GLCode == gl.GLCode).Count() > 1)
                    {
                        start_date = gl.GL_Date;
                    }
                    else {
                        start_date = gl.GL_Date.AddDays(1);
                    }

                }

                expectTime = start_date + DiferenceTime.Value;







                if (expectTime.AddDays(1) <= DateTime.Now)
                {
                    timeOfOffGL.Startdate = start_date.ToString();
                    timeOfOffGL.Enddate = expectTime.ToString();
                    return Tuple.Create(timeOfOffGL, true);
                }
                return Tuple.Create(timeOfOffGL, false);

            }






        }

        /// <summary>
        /// get all off gl in data base
        /// </summary>
        /// <returns>List<GLMaster> </returns>
        public static List<GLMaster> getallOffGL()
        {
            var db = new AccountingContainer();
            var glOff = db.GLMasters.Where(a => a.GLCode.StartsWith("GLOF")).ToList();
            return glOff;


        }
        /// <summary>
        /// تقوم بعمل تقييم مخزني اي تساوي بين قيم المخزن الموجودة فى قائمة المخازن وبين رصيدها فى ميزان الراجعة
        /// فاذا كان الفرق بينهم يساوي صفر لن يتم عمل التعديل
        /// </summary>
        /// <param name="tril"></param>
        /// <returns></returns>
        public static bool makeEvaluationOfStore() {
            var Trialbalance = new TrialBalncePostVM();

            Trialbalance.Start_Date = Convert.ToString(new DateTime(1995, 1, 1));





            Trialbalance.End_Date = Convert.ToString(new DateTime(2200, 12, 30));


            var tril = Reports.makeTrialOthermethod(Trialbalance);
            var db = new AccountingContainer();
            decimal StoreDifference = 0;
            decimal StoreballanceDependBatch = 0;
            var accountgl = tril.ballanceAttrribute.Where(a => a.Account_Code.StartsWith("010105")).ToList();
            var store = db.ChartOfAccountings.Where(p => p.Account_Code.StartsWith("0101050")).ToList();
            foreach (var storegl in accountgl)
            {
                var thatstore = store.Where(a => a.Account_Code == storegl.Account_Code).FirstOrDefault();
                var storeinStores = db.Stores.Where(a => a.Store_Code == thatstore.Account_Code).FirstOrDefault();
                //foreach (var batches in storeinStores.ProductBatches)
                //{
                //    StoreballanceDependBatch += batches.Batch_Cost;

                //}
                StoreballanceDependBatch = db.ProductBatches.Where(a => a.Store_ID == storeinStores.Store_ID).Sum(a => a.Product_Quantity * a.Product_Cost);
                StoreDifference = StoreballanceDependBatch - storegl.pureFinaldepit;
                StoreEvaluation bankev = new StoreEvaluation();
                bankev.difference = StoreDifference;
                bankev.StoreCode = storegl.Account_Code;


                if (bankev.difference != 0)
                {
                    makeGLEvaluation(bankev);
                }



            }
            return true;



        }

        /// <summary>
        /// عمل قيد التقييم المخزني فى حالة عدم تساوي الرصيدين
        /// </summary>
        /// <param name="store"></param>
        public static void makeGLEvaluation(StoreEvaluation store)
        {
            GLMaster glHeader = new GLMaster();
            var db = new AccountingContainer();
            // Used this GLHeader Create GL
            glHeader.GL_Date = DateTime.Now;
            glHeader.GL_Desc = "قيد تقييم مخزنى";
            glHeader.Company_Id = 1;
            glHeader.GLType_Id = "قيد تقييم مخزنى";
            glHeader.Currency_Id = "الريال السعودى";
            var lastCode = db.GLMasters.OrderByDescending(c => c.GLMaster_Id).Where(a => a.GLCode.StartsWith("GLEV-")).FirstOrDefault();
            if (lastCode == null)
            {
                glHeader.GLCode = "GLEV-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                glHeader.GLCode = "GLEV-" + (Convert.ToInt32(lastCode.GLCode.Substring(5, lastCode.GLCode.Length - 5)) + 1).ToString("D3");
            }
            glHeader.Auto = true;
            glHeader.GLState = TransactionState.Post;
            GLDetail storegl = new GLDetail(),
              evaluategl = new GLDetail();
            storegl.Account_Id = db.ChartOfAccountings.Where(a => a.Account_Code == store.StoreCode).FirstOrDefault().Account_Id;
            storegl.Account_Name = db.ChartOfAccountings.Where(a => a.Account_Code == store.StoreCode).FirstOrDefault().Account_Name;
            evaluategl.Account_Id = 151;
            evaluategl.Account_Name = db.ChartOfAccountings.Find(151).Account_Name;
            evaluategl.SubLedger_Id = 1;

            if (store.difference > 0)
            {
                storegl.GLDetail_Debit = store.difference;
                evaluategl.GLDetail_Credit = store.difference;
            }
            else
            {
                var diff = store.difference * -1;
                storegl.GLDetail_Credit = diff;
                evaluategl.GLDetail_Debit = diff;
            }
            glHeader.GLDetails.Add(storegl);
            glHeader.GLDetails.Add(evaluategl);
            db.GLMasters.Add(glHeader);
            db.SaveChanges();
            db.Dispose();

        }
        /// <summary>
        /// عمل عكس للقيد المشار اليه بواسطة رقمه
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static string MakeVoidOfPostingGL(int Id)
        {
            var db = new AccountingContainer();
            var Gl = db.GLMasters.Find(Id);
            OperationRun.Addnewoperation(Gl.GLCode);
            var operation = db.OperationRuns.Where(a => a.Code == Gl.GLCode).Count();
            if (operation != 1)
            {
                OperationRun.DeleteOperation(Gl.GLCode);
                return Resources.validate.GLReverse;
            }
            if (db.GLMasters.Where(a => a.GLCode == Gl.GLCode).Count() > 1)
            {
                OperationRun.DeleteOperation(Gl.GLCode);
                return Resources.validate.GLReverse;
            }
            var GLVoid = new GLMaster();
            GLVoid.Account_Child_ID = Gl.Account_Child_ID;
            GLVoid.Auto = Gl.Auto;
            GLVoid.Company_Id = Gl.Company_Id;
            GLVoid.Currency_Id = Gl.Currency_Id;
            GLVoid.Exchange_Rate = Gl.Exchange_Rate;
            GLVoid.GLCode = Gl.GLCode;
            GLVoid.GLType_Id = " عكس " + Gl.GLType_Id;
            GLVoid.GLState = TransactionState.Void;
            GLVoid.GL_Date = Gl.GL_Date;
            GLVoid.GL_Desc = "قيد عكسي";
            foreach (var item in Gl.GLDetails)
            {
                var glDetail = new GLDetail();
                glDetail.Account_Child_ID = item.Account_Child_ID;
                glDetail.Account_CHild_Name = item.Account_CHild_Name;
                glDetail.Account_Id = item.Account_Id;
                glDetail.Account_Name = item.Account_Name;
                glDetail.Asset_Id = item.Asset_Id;
                glDetail.Bu_Id = item.Bu_Id;
                glDetail.CostDistrbutionSubldger = item.CostDistrbutionSubldger;
                glDetail.Description_Detail = item.Description_Detail;
                glDetail.GLDetail_Credit = item.GLDetail_Debit;
                glDetail.GLDetail_Debit = item.GLDetail_Credit;
                glDetail.SubLedger_Id = item.SubLedger_Id;
                GLVoid.GLDetails.Add(glDetail);


            }
            db.GLMasters.Add(GLVoid);
            //Gl.GLState = TransactionState.Void;
            //db.Entry(Gl).State = EntityState.Modified;
            db.SaveChanges();
            GetBalanceForAccount(GLVoid);
            GetBalanceForSubLedger(GLVoid);
            OperationRun.DeleteOperation(Gl.GLCode);
            db.Dispose();

            return Resources.validate.GLReverse;

        }
        /// <summary>
        /// عكس القيد فى حالة عكس قيد الاستحقاق حيث فى هذه الحالة لايؤثر في رصيدهم
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static string MakeVoidOfPostingGLForHR(int Id)
        {
            var db = new AccountingContainer();
            var Gl = db.GLMasters.Find(Id);
            var GLVoid = new GLMaster();
            GLVoid.Account_Child_ID = Gl.Account_Child_ID;
            GLVoid.Auto = Gl.Auto;
            GLVoid.Company_Id = Gl.Company_Id;
            GLVoid.Currency_Id = Gl.Currency_Id;
            GLVoid.Exchange_Rate = Gl.Exchange_Rate;
            GLVoid.GLCode = Gl.GLCode;
            GLVoid.GLType_Id = Gl.GLType_Id + "قيد عكسي";
            GLVoid.GLState = TransactionState.Void;
            GLVoid.GL_Date = DateTime.Now;
            GLVoid.GL_Desc = "قيد عكسي";
            foreach (var item in Gl.GLDetails)
            {
                var glDetail = new GLDetail();
                glDetail.Account_Child_ID = item.Account_Child_ID;
                glDetail.Account_CHild_Name = item.Account_CHild_Name;
                glDetail.Account_Id = item.Account_Id;
                glDetail.Account_Name = item.Account_Name;
                glDetail.Asset_Id = item.Asset_Id;
                glDetail.Bu_Id = item.Bu_Id;
                glDetail.CostDistrbutionSubldger = item.CostDistrbutionSubldger;
                glDetail.Description_Detail = item.Description_Detail;
                glDetail.GLDetail_Credit = item.GLDetail_Debit;
                glDetail.GLDetail_Debit = item.GLDetail_Credit;
                glDetail.SubLedger_Id = item.SubLedger_Id;
                GLVoid.GLDetails.Add(glDetail);


            }
            db.GLMasters.Add(GLVoid);
            Gl.GLState = TransactionState.Void;
            db.Entry(Gl).State = EntityState.Modified;
            db.SaveChanges();
            var gldetilLoan = GLVoid.GLDetails.Where(a => a.Account_Id != 50).ToList();
            var glmasterWithoutLibality = new GLMaster();
            glmasterWithoutLibality.GLDetails = gldetilLoan;
            GetBalanceForAccount(glmasterWithoutLibality);
            GetBalanceForSubLedger(glmasterWithoutLibality);
            db.Dispose();
            return "تم عكس القيد بنجاح";

        }
        /// <summary>
        /// عمل قيد عكسي فى حالة قيد الاقفال وجعل القيد مثل رصيد افتتاحي للفترة الجديدة
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string MakeVoidOfPostingGLOffCloseGL(int Id, DateTime date)
        {
            var db = new AccountingContainer();
            var Gl = db.GLMasters.Find(Id);
            var GLVoid = new GLMaster();
            GLVoid.Account_Child_ID = Gl.Account_Child_ID;
            GLVoid.Auto = Gl.Auto;
            GLVoid.Company_Id = Gl.Company_Id;
            GLVoid.Currency_Id = Gl.Currency_Id;
            GLVoid.Exchange_Rate = Gl.Exchange_Rate;
            GLVoid.GLCode = Gl.GLCode;
            GLVoid.GLType_Id = "قيد افتتاحى";
            GLVoid.GLState = TransactionState.Void;
            GLVoid.GL_Date = date;
            GLVoid.GL_Desc = "قيد افتتاحي للفترة الجديدة ";
            foreach (var item in Gl.GLDetails)
            {
                var glDetail = new GLDetail();
                glDetail.Account_Child_ID = item.Account_Child_ID;
                glDetail.Account_CHild_Name = item.Account_CHild_Name;
                glDetail.Account_Id = item.Account_Id;
                glDetail.Account_Name = item.Account_Name;
                glDetail.Asset_Id = item.Asset_Id;
                glDetail.Bu_Id = item.Bu_Id;
                glDetail.CostDistrbutionSubldger = item.CostDistrbutionSubldger;
                glDetail.Description_Detail = item.Description_Detail;
                glDetail.GLDetail_Credit = item.GLDetail_Debit;
                glDetail.GLDetail_Debit = item.GLDetail_Credit;

                glDetail.SubLedger_Id = item.SubLedger_Id;

                GLVoid.GLDetails.Add(glDetail);


            }
            db.GLMasters.Add(GLVoid);
            Gl.GLState = TransactionState.Void;
            db.Entry(Gl).State = EntityState.Modified;
            db.SaveChanges();
            GetBalanceForAccount(GLVoid);
            GetBalanceForSubLedger(GLVoid);
            db.Dispose();
            return "تم عكس القيد بنجاح";

        }
        /// <summary>
        /// get diference from crdit and Depit
        /// </summary>
        public static List<GLMaster> getdifferenceGl()
        {
            var db = new AccountingContainer();
            var glList = new List<GLMaster> { };
            foreach (var gl in db.GLMasters)
            {
                var glo = new GLMaster();
                var credit = gl.GLDetails.Sum(a => a.GLDetail_Credit);
                var depit = gl.GLDetails.Sum(a => a.GLDetail_Debit);
                if (Math.Abs(credit - depit) > 1)
                {

                    continue;

                }
                if (Math.Round(credit, 5, MidpointRounding.AwayFromZero) != Math.Round(depit, 5, MidpointRounding.AwayFromZero))
                {
                    {
                        if (credit > depit)
                        {
                            var q = gl.GLDetails.FirstOrDefault().GLDetail_Debit;
                            gl.GLDetails.FirstOrDefault().GLDetail_Debit = q + (credit - depit);
                        }
                        {
                            var p = gl.GLDetails.FirstOrDefault().GLDetail_Credit;
                            gl.GLDetails.FirstOrDefault().GLDetail_Credit = p + (depit - credit);


                        }

                        glList.Add(gl);
                        db.Entry(gl).State = EntityState.Modified;
                    }



                }


            }
            // db.SaveChanges();
            return glList;
        }
        public static int addGlDetail()
        {
            var db = new AccountingContainer();
            var glmaster1 = db.GLMasters.Where(a => a.GLCode == "PUR-186").FirstOrDefault();
            var Credit = glmaster1.GLDetails.Sum(a => a.GLDetail_Credit);
            var depit = glmaster1.GLDetails.Sum(a => a.GLDetail_Debit);
            if (Credit != depit)
            {
                var gldetail = new GLDetail();
                gldetail.Account_Child_ID = 354;
                gldetail.Account_CHild_Name = "شركة السحيلي للتجارة والإنماء";
                gldetail.Account_Id = 45;
                gldetail.Account_Name = "الموردين";
                gldetail.GLDetail_Debit = 0;
                gldetail.GLDetail_Credit = 4162;
                glmaster1.GLDetails.Add(gldetail);


            }
            var glmaster2 = db.GLMasters.Where(a => a.GLCode == "PUR-194").FirstOrDefault();
            var Credit2 = glmaster2.GLDetails.Sum(a => a.GLDetail_Credit);
            var depit2 = glmaster2.GLDetails.Sum(a => a.GLDetail_Debit);
            if (Credit2 != depit2)
            {
                var gldetail = new GLDetail();
                gldetail.Account_Child_ID = 392;
                gldetail.Account_CHild_Name = "مؤسسة الخضراء / علاء";
                gldetail.Account_Id = 45;
                gldetail.Account_Name = "الموردين";
                gldetail.GLDetail_Debit = 0;
                gldetail.GLDetail_Credit = Convert.ToDecimal(1733.602500000000);
                glmaster2.GLDetails.Add(gldetail);


            }
            db.SaveChanges();


            return 0;

        }
        public static int GetGlByCode(string code)
        {
            var db = new AccountingContainer();
            var gl = db.GLMasters.Where(a => a.GLCode == code).FirstOrDefault();
            return gl.GLMaster_Id;


        }
        public static String deletionactiveGL(int id)
        {
            var db = new AccountingContainer();
            var doc = db.GLMasters.Find(id);
            if (doc.GL_Date >= db.Companies.FirstOrDefault().StartFinancialDate && (doc.Auto != true||doc.GLCode.StartsWith("EmpT-")|| doc.GLCode.StartsWith("GLST-")))
            {
                if (doc.GLCode.StartsWith("EmpT-"))
                {
                    var SalMounth = db.EmployeeSalaryMonths.Where(a => a.Date_Take_Sal != null).ToList();
                    SalMounth = SalMounth.Where(a => a.Date_Take_Sal.Value.Day == doc.GL_Date.Day && a.Date_Take_Sal.Value.Month == doc.GL_Date.Month && a.Date_Take_Sal.Value.Year == doc.GL_Date.Year).ToList();

                    foreach (var sal in SalMounth)
                    {
                        var account = db.Accounts.Where(a => a.Account_Code == sal.Emp_Code).FirstOrDefault();
                        if (doc.GLDetails.Where(a => a.Account_Child_ID == account.Accounts_Id).FirstOrDefault() != null)
                        {
                            sal.Date_Take_Sal = null;
                            sal.AcountSal = null;
                            sal.Status = صرف_المرتبات_الشهرى.لم_يصرف;
                            db.Entry(sal).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                    }


                }

                var gl = db.GLMasters.Where(a => a.GLCode == doc.GLCode);
                foreach (var g in gl)
                {
                    db.GLDetails.RemoveRange(g.GLDetails);

                    db.GLMasters.Remove(g);

                }
            
                
                db.SaveChanges();



                db.Dispose();
                return Resources.validate.DeleteSuccessful;
            }
            return Resources.validate.Error;
        }
        public static decimal getbalanceAccount(int AccountId)
        {
            var db = new AccountingContainer();

            var thischar = db.ChartOfAccountings.Find(AccountId);
            var chartid = thischar.Account_Id.ToString();
            if (thischar.Account_Id == 50)
            {
                chartid = "39";
            }
            if (thischar.Account_Code == "010109" || thischar.Account_Code == "020101" || thischar.Account_Code == "010111" || thischar.Account_Code == "020105")
            {
                var AccountLedger = db.Accounts.Where(p => p.Account_Id == chartid && p.OpenBalance_Code != "1").FirstOrDefault();

                return getbalanceChildAccount(AccountLedger.Accounts_Id);
            }
            var balance = db.GLDetails.Where(a => a.Account_Id == AccountId).ToList();
          
            return Math.Round( balance.Select(a => a.GLDetail_Debit).Sum() - balance.Select(a => a.GLDetail_Credit).Sum(),2,MidpointRounding.AwayFromZero);
        }
        public static decimal getbalanceChildAccount(int childAccount)
        {
            var db = new AccountingContainer();
            var balance = db.GLDetails.Where(a => a.Account_Child_ID == childAccount).ToList();
            return Math.Round(balance.Select(a => a.GLDetail_Debit).Sum() - balance.Select(a => a.GLDetail_Credit).Sum(), 2, MidpointRounding.AwayFromZero);
        }
    }
}