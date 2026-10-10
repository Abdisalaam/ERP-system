using Accountingsystem.ViewModel;
using Accountingsystem.ViewModel.ReportsNewVM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class Tax
    {
        public static List<Tax> GetAlltaxes()
        //get akk tax in a list
        {
            var db = new AccountingContainer();
            var taxes = db.Taxes.ToList();
            return taxes;
        }
        public static Tax GetTaxesbyId(int id)
        //get tax by id
        //deatails function
        {
            var db = new AccountingContainer();
            var taxes = db.Taxes.Find(id);
            return taxes;
        }

        public static TaxesVM Create()
        {
            var db = new AccountingContainer();
            var taxVM = new TaxesVM();
            var chart = db.ChartOfAccountings.Where(a => a.isActive != false);
            taxVM.MyChartOfAccountList = new SelectList(chart.Where(accCode => accCode.Account_Code== "010108001" || accCode.Account_Code== "020104001"), "Account_Id", "Account_Name");
            return taxVM;
        }

        public static Tax AddNewTaxes(TaxesVM per)
        //we made it to add name , type , addeddate , percentage of the tax in the DB.
        {
            var db = new AccountingContainer();
            var newtaxes = new Tax();
            
            newtaxes.Tax_Name = per.Tax_Name;
            newtaxes.Tax_Type = per.Tax_Type;
            
            newtaxes.Tax_AddedDate = DateTime.Now.ToString();
            newtaxes.Tax_Percentage = per.Tax_Percentage;


            ChartOfAccountingVM accountVM = new ChartOfAccountingVM();
            accountVM.ChartOfAccountingAccount_Id = per.SelectedChartOfAccountId;
            accountVM.Account_Name = per.Tax_Name;
            accountVM.Account_Curenncy = 1;
            newtaxes.Tax_Code = ChartOfAccounting.CreateAccount(accountVM);

            db.Taxes.Add(newtaxes);
            db.SaveChanges();
            db.Dispose();
            if (per.IsProductTax == true)
            {
                if (newtaxes.Tax_Code.StartsWith("02"))
                {
                    ChangeProductTax(newtaxes.Tax_ID, true);
                }
                else
                {
                    ChangeProductTax(newtaxes.Tax_ID, false);
                }
              
            }
            return newtaxes;

        }
        public static TaxesVM FillTax(int id)
        // get for edit return the view of the screen fill of tax's data for user 
        {
            var db = new AccountingContainer();
            var Mytax = db.Taxes.Find(id);
            TaxesVM per = new TaxesVM();
           
            per.MyChartOfAccountList = new SelectList(db.ChartOfAccountings.Where(accCode => accCode.Account_Code == "010108001" || accCode.Account_Code == "020104001"), "Account_Id", "Account_Name");

            per.Tax_ID = Mytax.Tax_ID;
            per.Tax_Name = Mytax.Tax_Name;
            per.Tax_Type = Mytax.Tax_Type;
            per.Tax_Percentage = Mytax.Tax_Percentage;
            per.Tax_AddedDate = DateTime.Now.ToString(); 
            per.Tax_Code= Mytax.Tax_Code;
            return per;
        }

        public static Tax EditTaxes(TaxesVM per)
        //we made that function to edit in the DB of the tax after editing
        //we will go to the DB . we will get the tax by id .if tax does not exist it will return by null .
        //else it will get to tax view model
        {
            var db = new AccountingContainer();
            var MyTaxes = db.Taxes.Find(per.Tax_ID);
            if (MyTaxes != null)
            {

                //var resultaccount = db.ChartOfAccountings.Find(per.SelectedChartOfAccountId);
                MyTaxes.Tax_Name = per.Tax_Name;
                MyTaxes.Tax_Type = per.Tax_Type;
                MyTaxes.Tax_AddedDate = DateTime.Now.ToString();

                var account = db.ChartOfAccountings.Where(a => a.Account_Code == MyTaxes.Tax_Code).FirstOrDefault();
                account.Account_Name = per.Tax_Name ;
                db.Entry(account).State = EntityState.Modified;
                db.Entry(MyTaxes).State = EntityState.Modified;
                //   db.Taxes.Add(MyTaxes);
                db.SaveChanges();
                if (per.IsProductTax == true)
                {
                    if (MyTaxes.Tax_Code.StartsWith("02"))
                    {
                        ChangeProductTax(MyTaxes.Tax_ID, true);
                    }
                    else
                    {
                        ChangeProductTax(MyTaxes.Tax_ID, false);
                    }

                }
                db.Dispose();
                return MyTaxes;
            }
            return null;
        }

        public static Tax findForDelete(int id)
        //find tax by id & return bu id
        {
            var db = new AccountingContainer();
            var tax = db.Taxes.Find(id);

            return tax;
        }


        public static int DeleteTaxes(int id)
        //after finding tax by id . you will delete it & than save it .
        {
            var db = new AccountingContainer();
            var Mytax = db.Taxes.Find(id);
            if (Mytax != null)
            {
                if (Mytax.IsActive != false)
                {
                  
                    Mytax.IsActive = false;
                    Mytax.DateOfDisActive = DateTime.Now;

                }
                else
                {
                    Mytax.IsActive = true;
                    Mytax.DateOfDisActive = null;
                }
                db.Entry(Mytax).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                return 1;
            }
            return 0;
        }
        public static TaxReportVM FillForMakeTaxGLFRomReport()
        {

            var taxReportVM = new TaxReportVM();
            return taxReportVM;




        }
        public static string makeGlTaxFromReport(TaxReportVM vm)
        {

            var db = new AccountingContainer();
            var Result = ReportsNew.CalculateTax(vm);
            if (Result.DifferenceBetweenRevenueAndTax != 0)
            {
                var gl = new GLMaster();
                gl.GL_Date = Convert.ToDateTime(vm.Date);
                gl.GLType_Id = "قيد تسوية ضريبية";
                gl.GL_Desc= "قيد تسوية ضريبية";
                gl.GLState = TransactionState.Post;
                gl.Company_Id = 1;
                gl.Currency_Id = "1";
                var lastCode = db.GLMasters.OrderByDescending(c => c.GLMaster_Id).Where(a => a.GLCode.StartsWith("TAX-")).FirstOrDefault();
                if (lastCode == null)
                {
                    gl.GLCode = "TAX-001";
                }
                else
                {
                    //using string substring method to get the number of the last inserted
                    gl.GLCode = "TAX-" + (Convert.ToInt32(lastCode.GLCode.Substring(4, lastCode.GLCode.Length - 4)) + 1).ToString("D3");
                }
                foreach(var item in Result.taxReportDetail)
                {
                    var gldetail = new GLDetail();

                    gldetail.Account_Id = item.Tax_Id;
                    if (item.TaxBallance >0 )
                    {
                        gldetail.GLDetail_Credit = item.TaxBallance;
                    }
                    else
                    {
                        gldetail.GLDetail_Debit = item.TaxBallance*-1;
                    }
                    gldetail.Bu_Id = 0;
                    gldetail.Account_Name = item.Tax_Name;
                    gl.GLDetails.Add(gldetail);
                }
                var gldetail2=  new GLDetail();
                var CreditSum = gl.GLDetails.Select(a => a.GLDetail_Credit).Sum();
                var DepitSum = gl.GLDetails.Select(a => a.GLDetail_Debit).Sum();
                gldetail2.Account_Id =216;


                gldetail2.Bu_Id = 0;
                gldetail2.Account_Name =  "التسوية الضريبية";
                if (CreditSum> DepitSum)
                {
                    gldetail2.GLDetail_Debit = CreditSum - DepitSum;
                }
                else
                {
                    gldetail2.GLDetail_Credit = DepitSum - CreditSum;
                }
                gl.GLDetails.Add(gldetail2);
                db.GLMasters.Add(gl);
                GLMaster.GetBalanceForAccount(gl);
                GLMaster.GetBalanceForSubLedger(gl);
                db.SaveChanges();
                return "تم عمل القيد بنجاح";
            }
            return "لا توجد قيم للضرائب لتسويتها";
        }
        public static TaxPaymentVM PreparePaymentTax()
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

                }


            }
            var vm = new TaxPaymentVM();
            vm.BankList = new SelectList(lista.Where(accCode => accCode.Account_Code.StartsWith("0101200010") || accCode.Account_Code.StartsWith("010120002")), "Account_Id", "Account_Name");
            return vm;

        }
        public static string makePaymentTaxGL(TaxPaymentVM vm)
        {

            var db = new AccountingContainer();
            

            if (vm.Date == null)
            {
                vm.Date = Convert.ToString(new DateTime(2200, 12, 30));

            }
           
            var End = Convert.ToDateTime(Convert.ToDateTime(vm.Date).ToString("dd/MM/yyyy")).AddHours(23).AddMinutes(59).AddSeconds(59);
            var CreditStellement = db.GLDetails.Where(a => a.GLMaster.GL_Date <= End && a.Account_Id == 216).Select(a => a.GLDetail_Credit).Sum();
            var DepiteStellement = db.GLDetails.Where(a => a.GLMaster.GL_Date <= End && a.Account_Id == 216).Select(a => a.GLDetail_Debit).Sum();
            if (CreditStellement != DepiteStellement)
            {
                var gl = new GLMaster();
                try
                {
                    gl.GL_Date = Convert.ToDateTime(vm.GLDate);
                }
                catch
                {
                    gl.GL_Date = DateTime.Today;
                }
                gl.GLType_Id = "قيد سداد للتسوية الضريبية";
                gl.GL_Desc = "قيد سداد للتسوية الضريبية";
                gl.GLState = TransactionState.Post;
                gl.Company_Id = 1;
                gl.Currency_Id = "1";
                var lastCode = db.GLMasters.OrderByDescending(c => c.GLMaster_Id).Where(a => a.GLCode.StartsWith("TAXPAY-")).FirstOrDefault();
                if (lastCode == null)
                {
                    gl.GLCode = "TAXPAY-001";
                }
                else
                {
                    //using string substring method to get the number of the last inserted
                    gl.GLCode = "TAXPAY-" + (Convert.ToInt32(lastCode.GLCode.Substring(7, lastCode.GLCode.Length - 7)) + 1).ToString("D3");
                }
                
                var gldetail = new GLDetail();

                gldetail.Account_Id = vm.BankAccount_Id;
                if (CreditStellement> DepiteStellement)
                {
                    gldetail.GLDetail_Credit = CreditStellement- DepiteStellement;
                }
                else
                {
                    gldetail.GLDetail_Debit = DepiteStellement - CreditStellement;
                }
                gldetail.Bu_Id = 0;
                gldetail.Account_Name =db.ChartOfAccountings.Find(vm.BankAccount_Id).Account_Name;
                gl.GLDetails.Add(gldetail);
                var gldetail2 = new GLDetail();
                var CreditSum = gl.GLDetails.Select(a => a.GLDetail_Credit).Sum();
                var DepitSum = gl.GLDetails.Select(a => a.GLDetail_Debit).Sum();
                gldetail2.Account_Id = 216;
                
                gldetail2.Bu_Id = 0;
                gldetail2.Account_Name = "تسوية ضريبية";
                if (CreditSum > DepitSum)
                {
                    gldetail2.GLDetail_Debit = CreditSum - DepitSum;
                }
                else
                {
                    gldetail2.GLDetail_Credit = DepitSum - CreditSum;
                }
                gl.GLDetails.Add(gldetail2);
                db.GLMasters.Add(gl);
                GLMaster.GetBalanceForAccount(gl);
                GLMaster.GetBalanceForSubLedger(gl);
                db.SaveChanges();
                return "تم عمل القيد بنجاح";
            }
            return "حساب التسوية الضريبية يساوي صفر";
        }
        public static string ChangeProductTax(int Tax_Id , bool IsSaleTax)
        {

            var db = new AccountingContainer();
            string connstring = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(connstring);
            conn.Open();
            if (IsSaleTax == true)
            {
                //string sql2 = "  UPDATE Products SET Tax_Id = {0} WHERE  Tax_Id>0";
                string sql2 = "  UPDATE Products SET Tax_Id = {0} ";


                db.Database.ExecuteSqlCommand(sql2, Tax_Id);
                //SqlDataReader reader = myCommand2.ExecuteReader();
                //conn.Close();
            }
            else
            {
                //string sql2 = "UPDATE Products SET Tax_Id_Purch =  {0}  WHERE  Tax_Id_Purch>0";
                string sql2 = "UPDATE Products SET Tax_Id_Purch =  {0} ";
                var Tax_Id_Purch = Tax_Id;

                db.Database.ExecuteSqlCommand(sql2, Tax_Id_Purch);


                //SqlDataReader reader = myCommand2.ExecuteReader();
                //conn.Close();
            }
            return "done";
        }


        public static TaxSalesandPurchase MakeReportTaxSalesAndPurch(TaxSalesandPurchase TaxSale)
        {
           

            if (TaxSale.from == null)
            {
                TaxSale.from = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (TaxSale.to == null)
            {
                TaxSale.to = Convert.ToString(new DateTime(2200, 12, 30));

            }
 
            DateTime start = Convert.ToDateTime(Convert.ToDateTime(TaxSale.from).ToShortDateString());
            var End = Convert.ToDateTime(Convert.ToDateTime(TaxSale.to).ToString("dd/MM/yyyy")).AddHours(23).AddMinutes(59).AddSeconds(59);
            TaxSale.details = new List<TaxSalesandPurchaseDetails>();
            var db = new AccountingContainer();
            TaxSale.tax = db.Taxes.Find(TaxSale.tax_Id);
            TaxSale.BallanceBeforeDate = getTheBalanceOfDateBefore(TaxSale.tax.Tax_Code, start);
            var alldata = db.GLDetails.Where(e => e.GLMaster.GL_Date >= start && e.GLMaster.GL_Date <= End&&e.ChartOfAccounting.Account_Code == TaxSale.tax.Tax_Code).ToList();
            foreach(var gldetail in alldata)
            {
                var detail = new TaxSalesandPurchaseDetails();
                detail.Code = gldetail.GLMaster.GLCode;
                detail.GLMaster_Id = gldetail.GLMasterGLMaster_Id;
                var customerNAme = gldetail.GLMaster.GLDetails.Where(a => a.Account_Child_ID > 0).FirstOrDefault();
                if (customerNAme != null)
                {
                    var Account = db.Accounts.Find(customerNAme.Account_Child_ID).Account_Code;
                    if (Account.StartsWith("S"))
                    {
                        var supplier = db.Suppliers.Where(a => a.Supp_Code == Account).FirstOrDefault();
                        detail.CustomerName = supplier.Supp_Name;
                        detail.CustomerTaxNum = supplier.Tax_Number;
                    }
                    else if(Account.StartsWith("C"))
                    {
                        var Customer = db.Customers.Where(a => a.Customer_Code == Account).FirstOrDefault();
                        detail.CustomerName = Customer.Customer_Name;
                        detail.CustomerTaxNum = Customer.Customer_TaxNumber;
                    }
                    
                }
              
                detail.DocumentType = gldetail.GLMaster.GLType_Id;
                if (gldetail.GLMaster.GLCode.StartsWith("POS-"))
                {
                    detail.DocumentType = "مبيعات نقطة بيع";
                }
                detail.GLDate = gldetail.GLMaster.GL_Date;
                detail.Percentage = TaxSale.tax.Tax_Percentage;
                var quantity = db.StoreCards.Where(a => a.Document_Serial == gldetail.GLMaster.GLCode);
                if (quantity.Count() == 0)
                {
                    detail.QuantityOfSales = 1;


                }
                else
                {
                    detail.QuantityOfSales = quantity.Select(a=>a.Quantity).Sum();
                }
                detail.ValueOfTax = gldetail.GLDetail_Debit - gldetail.GLDetail_Credit;
                detail.salesPurchValue = detail.ValueOfTax / (TaxSale.tax.Tax_Percentage / 100);
                TaxSale.details.Add(detail);
            }
            TaxSale.company = db.Companies.FirstOrDefault();
            return TaxSale;
        }


        public static decimal getTheBalanceOfDateBefore(string AccountCode,DateTime date)
        {
            var db = new AccountingContainer();
            var glDetail = db.GLDetails.Where(a => a.ChartOfAccounting.Account_Code.StartsWith( AccountCode) && a.GLMaster.GL_Date < date).ToList();
            if (glDetail.Count() > 0)
            {

                return glDetail.Select(a => a.GLDetail_Debit).Sum() - glDetail.Select(a => a.GLDetail_Credit).Sum();

            }

            return 0;
        }
        public static decimal getTheBalanceOfDateBeforewithoutOFF(string AccountCode, DateTime date)
        {
            var db = new AccountingContainer();
            var glDetail = db.GLDetails.Where(a => a.ChartOfAccounting.Account_Code.StartsWith(AccountCode) && a.GLMaster.GL_Date < date&&!a.GLMaster.GLCode.StartsWith("GLOF-")).ToList();
          
            if (glDetail.Count() > 0)
            {

                return glDetail.Select(a => a.GLDetail_Debit).Sum() - glDetail.Select(a => a.GLDetail_Credit).Sum();

            }

            return 0;
        }

        public static decimal getTheBalanceChildOfDateBefore(int  ChildAccountId, DateTime date)
        {
            var db = new AccountingContainer();
            var glDetail = db.GLDetails.Where(a => a.Account_Child_ID== ChildAccountId && a.GLMaster.GL_Date < date).ToList();
            if (glDetail.Count() > 0)
            {

                return glDetail.Select(a => a.GLDetail_Debit).Sum() - glDetail.Select(a => a.GLDetail_Credit).Sum();

            }

            return 0;
        }
        public static decimal getTheBalanceChildOfDateBeforewithoutOFF(int ChildAccountId, DateTime date)
        {
            var db = new AccountingContainer();
            var glDetail = db.GLDetails.Where(a => a.Account_Child_ID == ChildAccountId && a.GLMaster.GL_Date < date && !a.GLMaster.GLCode.StartsWith("GLOF-")).ToList();
            if (glDetail.Count() > 0)
            {

                return glDetail.Select(a => a.GLDetail_Debit).Sum() - glDetail.Select(a => a.GLDetail_Credit).Sum();

            }

            return 0;
        }


        public static decimal getTheBalanceChildOfDateBeforewithoutOFF(int ChildAccountId, DateTime date,int AccountId)
        {
            var db = new AccountingContainer();
            var glDetail = db.GLDetails.Where(a => a.Account_Child_ID == ChildAccountId&&a.Account_Id== AccountId && a.GLMaster.GL_Date < date && !a.GLMaster.GLCode.StartsWith("GLOF-")).ToList();
            if (glDetail.Count() > 0)
            {

                return glDetail.Select(a => a.GLDetail_Debit).Sum() - glDetail.Select(a => a.GLDetail_Credit).Sum();

            }

            return 0;
        }
        public static decimal getTheBalanceChildOfDateBefore(int ChildAccountId, DateTime date, int AccountId)
        {
            var db = new AccountingContainer();
            var glDetail = db.GLDetails.Where(a => a.Account_Child_ID == ChildAccountId&&a.Account_Id== AccountId && a.GLMaster.GL_Date < date).ToList();
            if (glDetail.Count() > 0)
            {

                return glDetail.Select(a => a.GLDetail_Debit).Sum() - glDetail.Select(a => a.GLDetail_Credit).Sum();

            }

            return 0;
        }
        public static decimal getTheBalanceAssetOfDateBefore(int Asset, DateTime date)
        {
            var db = new AccountingContainer();
            var glDetail = db.GLDetails.Where(a => a.Asset_Id == Asset && a.GLMaster.GL_Date < date&&a.ChartOfAccounting.Account_Code.StartsWith("0102")).ToList();
            if (glDetail.Count() > 0)
            {

                return glDetail.Select(a => a.GLDetail_Debit).Sum() - glDetail.Select(a => a.GLDetail_Credit).Sum();

            }

            return 0;
        }
        public static decimal getTheBalanceAssetOfDateBeforewithoutOFF(int Asset, DateTime date)
        {
            var db = new AccountingContainer();
            var glDetail = db.GLDetails.Where(a => a.Account_Child_ID == Asset && a.GLMaster.GL_Date < date && !a.GLMaster.GLCode.StartsWith("GLOF-")&& a.ChartOfAccounting.Account_Code.StartsWith("0102")).ToList();
            if (glDetail.Count() > 0)
            {

                return glDetail.Select(a => a.GLDetail_Debit).Sum() - glDetail.Select(a => a.GLDetail_Credit).Sum();

            }

            return 0;
        }
    }
}