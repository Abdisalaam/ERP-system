using Accountingsystem.ViewModel;
using Accountingsystem.ViewModel.ReportsVM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace Accountingsystem.Models
{
    public class TotalDashBoard
    {
        public static List<Charts> GetAllChartColumn()
        {
            var db = new AccountingContainer();

            var monthellyRecordOfTotalAccountsBalances = db.Charts.ToList();

            //new Chart(width: 300, height: 200, theme: ChartTheme.Green)
            //    .AddTitle("المصاريف" + dateYear)
            //    .AddSeries("Default", chartType: "Column", xValue: xValue, yValues: yValue).Write("bmp");

            return monthellyRecordOfTotalAccountsBalances;


        }
        public static bool RecalculateDachboard(int? Recall)
        {
            var db = new AccountingContainer();
            var datechart = new ChartDate();
            datechart.Date = DateTime.Now;
            // var pro = GetCategorymostSale();
            // var Sup = GetMostSupplierAccount();
            // var Cus = GetMostCustomerAccount();
            //var sale= GetLasTSale();
            // var gl = GetLasTGL();
            // var pay = GetLastPayment();
            // var Recipt = GetLasTRecipt();
            if (Recall != 0 && Recall != null)
            {
                db.ChartDates.RemoveRange(db.ChartDates);
                db.ChartDates.Add(datechart);
                db.SaveChanges();
                return true;
            }
            var lastDate = db.ChartDates.FirstOrDefault();
            if (lastDate == null)
            {
                db.ChartDates.RemoveRange(db.ChartDates);
                db.ChartDates.Add(datechart);

                db.SaveChanges();
                return true;
            }
            var date = DateTime.Now;
            if (date.Day == lastDate.Date.Day && date.Month == lastDate.Date.Month && date.Year == lastDate.Date.Year)
            {
                return false;
            }
            db.ChartDates.RemoveRange(db.ChartDates);
            db.ChartDates.Add(datechart);
            db.SaveChanges();
            return true;


        }
        public static TotalDashBoardVM GetAllDashBoardDataNotRecalculate()
        {
            var db = new AccountingContainer();
            var dashBoard = new TotalDashBoardVM();



            dashBoard.revenu = db.ChartOfAccountings.Where(a => a.Account_Code == "04").FirstOrDefault().BalanceSheetValue.Value;
            dashBoard.CostOfSales = db.ChartOfAccountings.Where(a => a.Account_Code == "0501").FirstOrDefault().BalanceSheetValue.Value;
            dashBoard.Expences = db.ChartOfAccountings.Where(a => a.Account_Code == "05").FirstOrDefault().BalanceSheetValue.Value;
            dashBoard.ProfitAndLoss = dashBoard.revenu + (dashBoard.Expences /*+ dashBoard.CostOfSales*/);
            dashBoard.Customer = db.ChartOfAccountings.Where(a => a.Account_Code == "010109").FirstOrDefault().BalanceSheetValue.Value;
            dashBoard.Supplier = db.ChartOfAccountings.Where(a => a.Account_Code == "020101").FirstOrDefault().BalanceSheetValue.Value;
            dashBoard.Banks = db.ChartOfAccountings.Where(a => a.Account_Code == "010120002").FirstOrDefault().BalanceSheetValue.Value;
            dashBoard.Treasury = db.ChartOfAccountings.Where(a => a.Account_Code == "010120001").FirstOrDefault().BalanceSheetValue.Value;

            dashBoard.Purchase = db.ChartOfAccountings.Where(a => a.Account_Code == "010105").FirstOrDefault().BalanceSheetValue.Value;


            return dashBoard;


        }





        public static DashbordControlVM GetAllTreasuryBankAndStores()
        {
            var db = new AccountingContainer();
            var dashBoard = new DashbordControlVM();
            ReportMasterGeneralBudgetVM mastergeneral = new ReportMasterGeneralBudgetVM();
            // int start = 1995;
            DateTime start = new DateTime(1995, 12, 30);
            DateTime end = new DateTime(2200, 12, 30).AddHours(23).AddMinutes(59).AddSeconds(59);
            mastergeneral.StartDate = Convert.ToString(start);
            mastergeneral.EndDate = Convert.ToString(end);
            mastergeneral.WithOutOFFGLs = false;
            mastergeneral = Reports.ReportGeneralBadget(mastergeneral);



            dashBoard.Bank = db.ChartOfAccountings.Where(a => a.Account_Code == "010120002").FirstOrDefault().BalanceSheetValue.Value;
            dashBoard.Treasury = db.ChartOfAccountings.Where(a => a.Account_Code == "010120001").FirstOrDefault().BalanceSheetValue.Value;
            dashBoard.TreasuryAndBank = dashBoard.Treasury + dashBoard.Bank;
            dashBoard.Stores = db.ChartOfAccountings.Where(a => a.Account_Code == "010105").FirstOrDefault().BalanceSheetValue.Value;           

            return dashBoard;


        }


        /// <summary>
        /// make ahome total in home page by make general ledger in start date 1995 and end date 2200
        /// </summary>
        /// <returns>TotalDashBoardVM</returns>
        public static TotalDashBoardVM GetAllDashBoardData()
        {
            var db = new AccountingContainer();
            var dashBoard = new TotalDashBoardVM();
            ReportMasterGeneralBudgetVM mastergeneral = new ReportMasterGeneralBudgetVM();
            // int start = 1995;
            DateTime start = new DateTime(1995, 12, 30);
            DateTime end = new DateTime(2200, 12, 30).AddHours(23).AddMinutes(59).AddSeconds(59);
            mastergeneral.StartDate = Convert.ToString(start);
            mastergeneral.EndDate = Convert.ToString(end);
            mastergeneral.WithOutOFFGLs = true;
            mastergeneral = Reports.ReportGeneralBadget(mastergeneral);
            dashBoard.revenu = db.ChartOfAccountings.Where(a => a.Account_Code == "04").FirstOrDefault().BalanceSheetValue.Value;
            dashBoard.CostOfSales = db.ChartOfAccountings.Where(a => a.Account_Code == "0501").FirstOrDefault().BalanceSheetValue.Value;
            dashBoard.Expences = db.ChartOfAccountings.Where(a => a.Account_Code == "05").FirstOrDefault().BalanceSheetValue.Value /*- dashBoard.CostOfSales*/;
            dashBoard.ProfitAndLoss = dashBoard.revenu + (dashBoard.Expences /*+ dashBoard.CostOfSales*/);
            dashBoard.Customer = db.ChartOfAccountings.Where(a => a.Account_Code == "010109").FirstOrDefault().BalanceSheetValue.Value;
            dashBoard.Supplier = db.ChartOfAccountings.Where(a => a.Account_Code == "020101").FirstOrDefault().BalanceSheetValue.Value;
            dashBoard.Banks = db.ChartOfAccountings.Where(a => a.Account_Code == "010120002").FirstOrDefault().BalanceSheetValue.Value;
            dashBoard.Treasury = db.ChartOfAccountings.Where(a => a.Account_Code == "010120001").FirstOrDefault().BalanceSheetValue.Value;
            
            //total of all store
            dashBoard.Purchase = db.ChartOfAccountings.Where(a => a.Account_Code == "010105").FirstOrDefault().BalanceSheetValue.Value;


            return dashBoard;


        }
        /// <summary>
        /// Prepare data for chart in home page and make general ledger in each mounth of year
        /// </summary>
        /// <returns>List<DashboadDiagramVM> </returns>
        public static List<DashboadDiagramVM> makeDiagrame()
        {
            var db = new AccountingContainer();

            var TimeNow = DateTime.Now;
            var glAll = db.GLMasters.ToList();
            var getmounth = glAll.Where(a => a.GL_Date.Year == TimeNow.Year).ToList();
            var numberOfMounth = getmounth.Select(a => a.GL_Date.Month).Distinct();
            List<DashboadDiagramVM> ListDigrame = new List<DashboadDiagramVM> { };

            foreach (var that_Mount in numberOfMounth)
            {

                ReportMasterGeneralBudgetVM mastergeneral = new ReportMasterGeneralBudgetVM();
                DateTime start = new DateTime(TimeNow.Year, that_Mount, 1);
                var LastDayInMounth = DateTime.DaysInMonth(TimeNow.Year, that_Mount);
                DateTime end = new DateTime(TimeNow.Year, that_Mount, LastDayInMounth).AddHours(23).AddMinutes(59).AddSeconds(59);
                mastergeneral.StartDate = start.ToString();
                mastergeneral.EndDate = end.ToString();
                mastergeneral.WithOutOFFGLs = false;
                mastergeneral = Reports.ReportGeneralBadget(mastergeneral);

                var diagrame = makeFullbudget();
                diagrame.date = that_Mount;
                ListDigrame.Add(diagrame);



            }
            for (int a = 1; a <= 12; a++)
            {
                if (ListDigrame.Where(k => k.date == a).FirstOrDefault() == null)
                {
                    var Empty = MakeEmptyMounth(a);
                    ListDigrame.Add(Empty);
                }
            }
            var list = ListDigrame.OrderBy(a => a.date).ToList();

            var RmoveList = db.Charts.ToList();
            db.Charts.RemoveRange(RmoveList);
            db.SaveChanges();
            makeChartSavingInDataBase(list);
            return list;

        }
        /// <summary>
        /// put all Result of last method in entity 
        /// </summary>
        /// <param name="ListDashBord"></param>
        public static void makeChartSavingInDataBase(List<DashboadDiagramVM> ListDashBord)
        {
            var db = new AccountingContainer();
            var chartList = new List<Charts>();

            foreach (var item in ListDashBord)
            {
                var chart = new Charts();
                chart.Banks = item.Banks;
                chart.CostOfSales = item.CostOfSales;
                chart.Customer = item.Customer;
                chart.revenu = item.revenu * -1;
                chart.Purchase = item.Purchase;
                chart.Expences = item.Expences;
                chart.ProfitAndLoss = item.ProfitAndLoss * -1;
                chart.Supplier = item.Supplier * -1;
                chart.Treasury = item.Treasury;
                chart.date = item.date;
                chartList.Add(chart);

            }
            db.Charts.AddRange(chartList);
            db.SaveChanges();
            db.Dispose();


        }
        /// <summary>
        /// adding in Entity and save the value to make chart of that
        /// </summary>
        /// <returns>DashboadDiagramVM</returns>
        public static DashboadDiagramVM makeFullbudget()
        {
            var db = new AccountingContainer();
            var diagrame = new DashboadDiagramVM();
            diagrame.revenu = db.ChartOfAccountings.Where(a => a.Account_Code == "04").FirstOrDefault().BalanceSheetValue.Value;
            diagrame.CostOfSales = db.ChartOfAccountings.Where(a => a.Account_Code == "0501").FirstOrDefault().BalanceSheetValue.Value;
            diagrame.Expences = db.ChartOfAccountings.Where(a => a.Account_Code == "05").FirstOrDefault().BalanceSheetValue.Value /*- diagrame.CostOfSales*/;
            diagrame.ProfitAndLoss = diagrame.revenu + (diagrame.Expences /*+ diagrame.CostOfSales*/);
            diagrame.Customer = db.ChartOfAccountings.Where(a => a.Account_Code == "010109").FirstOrDefault().BalanceSheetValue.Value;
            diagrame.Supplier = db.ChartOfAccountings.Where(a => a.Account_Code == "020101").FirstOrDefault().BalanceSheetValue.Value;
            diagrame.Banks = db.ChartOfAccountings.Where(a => a.Account_Code == "010120002").FirstOrDefault().BalanceSheetValue.Value;
            diagrame.Treasury = db.ChartOfAccountings.Where(a => a.Account_Code == "010120001").FirstOrDefault().BalanceSheetValue.Value;
            //total of all store
            diagrame.Purchase = db.ChartOfAccountings.Where(a => a.Account_Code == "010105").FirstOrDefault().BalanceSheetValue.Value;
            db.SaveChanges();
            db.Dispose();
            return diagrame;


        }
        /// <summary>
        /// add in db Empty mounth if gneral ledger =0
        /// </summary>
        /// <param name="date"></param>
        /// <returns>DashboadDiagramVM</returns>
        public static DashboadDiagramVM MakeEmptyMounth(int date)
        {
            DashboadDiagramVM dashvm = new DashboadDiagramVM();
            dashvm.date = date;
            dashvm.revenu = 0;
            dashvm.CostOfSales = 0;
            dashvm.Expences = 0;
            dashvm.ProfitAndLoss = 0;
            dashvm.Customer = 0;
            dashvm.Supplier = 0;
            dashvm.Banks = 0;
            dashvm.Treasury = 0;
            return dashvm;

        }
        /// <summary>
        /// make chart of treasury that mounth =x and value of treasury =y
        /// </summary>

        public static void ChartColumnTreasury()
        {
            var db = new AccountingContainer();
            ArrayList xValue = new ArrayList();
            ArrayList yValue = new ArrayList();
            var dateYear = DateTime.Now.Year.ToString();
            var result = (from c in db.Charts select c);
            result.ToList().ForEach(rs => xValue.Add(rs.date));
            result.ToList().ForEach(rs => yValue.Add(rs.Treasury));

            new Chart(width: 300, height: 200, theme: ChartTheme.Green)
                .AddTitle("الصناديق" + dateYear)
                .AddSeries("Default", chartType: "Column", xValue: xValue, yValues: yValue).Write("bmp");


        }
        /// <summary>
        /// make chart of bank that mounth =x and value of bank =y
        /// </summary>

        public static void ChartColumnBank()
        {
            var db = new AccountingContainer();
            ArrayList xValue = new ArrayList();
            ArrayList yValue = new ArrayList();
            var dateYear = DateTime.Now.Year.ToString();
            var result = (from c in db.Charts select c);
            result.ToList().ForEach(rs => xValue.Add(rs.date));
            result.ToList().ForEach(rs => yValue.Add(rs.Banks));

            new Chart(width: 300, height: 200, theme: ChartTheme.Green)
                .AddTitle("البنوك" + dateYear)
                .AddSeries("Default", chartType: "Column", xValue: xValue, yValues: yValue).Write("bmp");


        }
        /// <summary>
        /// make chart of revenue that mounth =x and value of revenue =y
        /// </summary>

        public static void ChartColumnRevenu()
        {
            var db = new AccountingContainer();
            ArrayList xValue = new ArrayList();
            ArrayList yValue = new ArrayList();
            var dateYear = DateTime.Now.Year.ToString();
            var result = (from c in db.Charts select c);
            result.ToList().ForEach(rs => xValue.Add(rs.date));
            result.ToList().ForEach(rs => yValue.Add(rs.revenu));

            new Chart(width: 300, height: 200, theme: ChartTheme.Green)
                .AddTitle("الايرادات" + dateYear)
                .AddSeries("Default", chartType: "Column", xValue: xValue, yValues: yValue).Write("bmp");


        }
        /// <summary>
        /// make chart of Expences that mounth =x and value of Expenses =y
        /// </summary>

        public static void ChartColumnExpences()
        {
            var db = new AccountingContainer();
            ArrayList xValue = new ArrayList();
            ArrayList yValue = new ArrayList();
            var dateYear = DateTime.Now.Year.ToString();
            var result = (from c in db.Charts select c);
            result.ToList().ForEach(rs => xValue.Add(rs.date));
            result.ToList().ForEach(rs => yValue.Add(rs.Expences));

            new Chart(width: 300, height: 200, theme: ChartTheme.Green)
                .AddTitle("المصاريف" + dateYear)
                .AddSeries("Default", chartType: "Column", xValue: xValue, yValues: yValue).Write("bmp");


        }
        /// <summary>
        /// make chart of CostOfsale that mounth =x and value of CostOfsale =y
        /// </summary>

        public static void ChartColumnCostOfSales()
        {
            var db = new AccountingContainer();
            ArrayList xValue = new ArrayList();
            ArrayList yValue = new ArrayList();
            var dateYear = DateTime.Now.Year.ToString();
            var result = (from c in db.Charts select c);
            result.ToList().ForEach(rs => xValue.Add(rs.date));
            result.ToList().ForEach(rs => yValue.Add(rs.CostOfSales));

            new Chart(width: 300, height: 200, theme: ChartTheme.Green)
                .AddTitle("تكلفة مبيعات" + dateYear)
                .AddSeries("Default", chartType: "Column", xValue: xValue, yValues: yValue).Write("bmp");


        }
        /// <summary>
        /// make chart of Profit and Loss that mounth =x and value of Profit and loss =y
        /// </summary>

        public static void ChartColumnProfitAndLoss()
        {
            var db = new AccountingContainer();
            ArrayList xValue = new ArrayList();
            ArrayList yValue = new ArrayList();
            var dateYear = DateTime.Now.Year.ToString();
            var result = (from c in db.Charts select c);
            result.ToList().ForEach(rs => xValue.Add(rs.date));
            result.ToList().ForEach(rs => yValue.Add(rs.ProfitAndLoss));

            new Chart(width: 300, height: 200, theme: ChartTheme.Green)
                .AddTitle("الارباح والخسائر" + dateYear)
                .AddSeries("Default", chartType: "Column", xValue: xValue, yValues: yValue).Write("bmp");


        }
        public static void ChartColumnStore()
        {
            var db = new AccountingContainer();
            ArrayList xValue = new ArrayList();
            ArrayList yValue = new ArrayList();
            var dateYear = DateTime.Now.Year.ToString();
            var result = (from c in db.Charts select c);
            result.ToList().ForEach(rs => xValue.Add(rs.date));
            result.ToList().ForEach(rs => yValue.Add(rs.Purchase));

            new Chart(width: 300, height: 200, theme: ChartTheme.Green)
                .AddTitle("المخازن" + dateYear)
                .AddSeries("Default", chartType: "Column", xValue: xValue, yValues: yValue).Write("bmp");


        }
        /// <summary>
        /// make chart of Supplier that mounth =x and value of Supplier =y
        /// </summary>

        public static void ChartColumnSupplier()
        {
            var db = new AccountingContainer();
            ArrayList xValue = new ArrayList();
            ArrayList yValue = new ArrayList();
            var dateYear = DateTime.Now.Year.ToString();
            var result = (from c in db.Charts select c);
            result.ToList().ForEach(rs => xValue.Add(rs.date));
            result.ToList().ForEach(rs => yValue.Add(rs.Supplier));

            new Chart(width: 300, height: 200, theme: ChartTheme.Green)
                .AddTitle("الموردين" + dateYear)
                .AddSeries("Default", chartType: "Column", xValue: xValue, yValues: yValue).Write("bmp");


        }
        /// <summary>
        /// make chart of Customer that mounth =x and value of Customer =y
        /// </summary>

        public static void ChartColumnCustomer()
        {
            var db = new AccountingContainer();
            ArrayList xValue = new ArrayList();
            ArrayList yValue = new ArrayList();
            var dateYear = DateTime.Now.Year.ToString();
            var result = (from c in db.Charts select c);
            result.ToList().ForEach(rs => xValue.Add(rs.date));
            result.ToList().ForEach(rs => yValue.Add(rs.Customer));
            new Chart(width: 300, height: 200, theme: ChartTheme.Green)
                .AddTitle("العملاء" + dateYear)
                .AddSeries("Default", chartType: "Column", xValue: xValue, yValues: yValue).Write("bmp");


        }


      
        public static List<CategroyVM> GetCategorymostSale()
        {
            var db = new AccountingContainer();

            var MostProductSale = db.StoreCards.Where(z => z.IncreaseQuantity == false && (z.Document_Serial.StartsWith("SAL") || z.Document_Serial.StartsWith("POS"))).GroupBy(a => a.Product_Id).Select(g => new
            {

                p_Id = g.Key,

                totalquantity = g.Sum(a => a.Quantity),

                totalsalePrice = g.Sum(a => a.TotalAmount)


            }).OrderByDescending(x => x.totalquantity).ToList();

            var productList = new List<Product> { };
            foreach (var pro in MostProductSale)
            {
                var product = new Product();
                product.Prod_ID = pro.p_Id;
                product.Quantity = pro.totalquantity;
                product.Sale_Price = pro.totalsalePrice;
                product.Category_ID = db.Products.Find(pro.p_Id).Category_ID;
                productList.Add(product);

            }
            var MostCatSale = productList.GroupBy(a => a.Category_ID).Select(g => new
            {

                Cat_Id = g.Key,

                totalquantity = g.Sum(a => a.Quantity),

                totalsalePrice = g.Sum(a => a.Sale_Price)


            }).OrderByDescending(x => x.totalquantity).ToList();
            var First3 = MostCatSale.Take(3);
            var CatList = new List<CategroyVM> { };
            foreach (var Categ in First3.Where(a => a.Cat_Id != 0 && a.Cat_Id != null))
            {
                var Cat = new CategroyVM();

                var produc = db.Categories.Find(Categ.Cat_Id);
                Cat.Cat_Descrip = produc.Cat_Descrip;
                Cat.Cat_Code = produc.Cat_Code;
                Cat.CatQuantity = Categ.totalquantity;
                Cat.CatSales = Categ.totalsalePrice;
                CatList.Add(Cat);



            }
            return CatList;





        }
        public static List<Product> GetProductmostSale()
        {
            var db = new AccountingContainer();
            var MostProductSale = db.StoreCards.Where(z => z.IncreaseQuantity == false && (z.Document_Serial.StartsWith("SAL") || z.Document_Serial.StartsWith("POS"))).GroupBy(a => a.Product_Id).Select(g => new
            {

                p_Id = g.Key,

                totalquantity = g.Sum(a => a.Quantity),

                totalsalePrice = g.Sum(a => a.TotalAmount),


            }).OrderByDescending(x => x.totalquantity).ToList();

            var First7 = MostProductSale.Take(7);
            var productList = new List<Product> { };
            foreach (var pro in First7)
            {
                var prod = new Product();
                var produc = db.Products.Find(pro.p_Id);
                prod.Prod_Name = produc.Prod_Name;
                prod.Prod_Code = produc.Prod_Code;
                prod.Quantity = pro.totalquantity;
                prod.Sale_Price = pro.totalsalePrice;
                productList.Add(prod);



            }
            return productList;





        }

        public static List<Accounts> GetMostSupplierAccount()
        {
            var db = new AccountingContainer();
            var MostSupplierBallance = db.GLDetails.Where(z => z.Account_Id == 45 && z.Account_Child_ID != null).GroupBy(a => a.Account_Child_ID).Select(g => new
            {

                p_Id = g.Key,

                TotalBallance = (g.Sum(a => a.GLDetail_Debit) - g.Sum(a => a.GLDetail_Credit)) * -1




            }).OrderByDescending(x => x.TotalBallance).ToList();

            var First7 = MostSupplierBallance.Take(7);
            var SupList = new List<Accounts> { };
            foreach (var suplier in First7)
            {
                var Su = new Accounts();
                var supl = db.Accounts.Find(suplier.p_Id);
                Su.Account_Code = supl.Account_Code;
                Su.Account_Name = supl.Account_Name;
                Su.Account_Balance = suplier.TotalBallance;

                SupList.Add(Su);



            }
            return SupList;





        }
        public static List<Accounts> GetMostCustomerAccount()
        {
            var db = new AccountingContainer();
            var MostCustomerBalance = db.GLDetails.Where(z => z.Account_Id == 37 && z.Account_Child_ID != null).GroupBy(a => a.Account_Child_ID).Select(g => new
            {

                p_Id = g.Key,

                TotalBallance = g.Sum(a => a.GLDetail_Debit) - g.Sum(a => a.GLDetail_Credit)




            }).OrderByDescending(x => x.TotalBallance).ToList();

            var First7 = MostCustomerBalance.Take(7);
            var CustomerList = new List<Accounts> { };
            foreach (var customer in First7)
            {
                var cust = new Accounts();
                var custom = db.Accounts.Find(customer.p_Id);
                cust.Account_Code = custom.Account_Code;
                cust.Account_Name = custom.Account_Name;
                cust.Account_Balance = customer.TotalBallance;

                CustomerList.Add(cust);



            }
            return CustomerList;





        }
        public static List<GLMaster> GetLasTGL()
        {
            var db = new AccountingContainer();
            var MostCustomerBalance = db.GLMasters.ToList();
            MostCustomerBalance.Reverse();

            var First7 = MostCustomerBalance.Take(7).ToList();
            First7.Reverse();
            return First7;





        }
        public static List<InvoiceMaster> GetLasTSale()
        {
            var db = new AccountingContainer();

            var MostCustomerBalance = db.GLMasters.Where((a => a.GLCode.StartsWith("SAL") || a.GLCode.StartsWith("POS"))).Where(a => a.GLState != TransactionState.Void).ToList();
            MostCustomerBalance.Reverse();

            var First7 = MostCustomerBalance.Take(7).ToList();
            First7.Reverse();
            var invoicList = new List<InvoiceMaster> { };

            foreach (var invoice in First7)
            {
                var invoic = new InvoiceMaster();
                invoic.Serial_Number = invoice.GLCode;
                if (invoice.GLCode.StartsWith("SALRC"))
                {
                    var salesRecipt = db.SalesReceiptMasters.Where(a => a.Serial_Number == invoice.GLCode).FirstOrDefault();
                    invoic.TotaL_After_Fixed = salesRecipt.TotaL_After_Fixed;
                }
                if (invoice.GLCode.StartsWith("SAL-"))
                {
                    var invo = db.InvoiceMasters.Where(a => a.Serial_Number == invoice.GLCode).FirstOrDefault();
                    invoic.TotaL_After_Fixed = invo.TotaL_After_Fixed;
                }

                if (invoice.GLCode.StartsWith("POS-"))
                {
                    var invo = db.MasterPOSInvoices.Where(a => a.Serial_Number == invoice.GLCode).FirstOrDefault();
                    invoic.TotaL_After_Fixed = invo.Total_Amount;
                }
                invoicList.Add(invoic);


            }

            return invoicList;



        }
        public static List<Payment> GetLastPayment()
        {
            var db = new AccountingContainer();
            var allPayment = db.Payments.ToList();
            allPayment.Reverse();

            var First7 = allPayment.Take(7).ToList();
            First7.Reverse();
            return First7;





        }
        public static List<Receipt> GetLasTRecipt()
        {
            var db = new AccountingContainer();
            var allRecipt = db.Receipts.ToList();
            allRecipt.Reverse();

            var First7 = allRecipt.Take(7).ToList();
            First7.Reverse();
            return First7;

        }

        public static DailySalesVM getDailySales()
        {
            var db = new AccountingContainer();
            int dayesCountThisWeek = -6;
            int dayesCountLastWeek = -13;
            var startDateThisWeek = DateTime.Now.Date.AddDays(dayesCountThisWeek);
            var endDateThisWeek = DateTime.Now.Date;
            var startDateLastWeek = DateTime.Now.Date.AddDays(dayesCountLastWeek);    
            var salesInvoiceList = db.SalesReceiptMasters.Where(a => a.Invoice_Date >= startDateLastWeek).ToList();
            var invoiceList = db.InvoiceMasters.Where(a => a.Invoice_Date >= startDateLastWeek).ToList();
            var posList = db.MasterPOSInvoices.Where(a => a.Invoice_Date >= startDateLastWeek).ToList();           

            DailySalesVM Dayinvoice = new DailySalesVM();
            for (int i = 0; i < 7; i++)
            {
                foreach (var salesInvoice in salesInvoiceList)
                {
                    int resultSalesInvoiceThisWeek = DateTime.Compare(salesInvoice.Invoice_Date.Date, startDateThisWeek.Date);
                    int resultSalesInvoiceLastWeek = DateTime.Compare(salesInvoice.Invoice_Date.Date, startDateLastWeek.Date);
                    if (resultSalesInvoiceThisWeek == 0 )
                    {
                        Dayinvoice.FirstWeek[i] += 1;
                    }
                    if (resultSalesInvoiceLastWeek == 0)
                    {
                        Dayinvoice.LasttWeek[i] += 1;
                    }

                }
                foreach (var invoice in invoiceList)
                {
                    int resultSalesInvoiceThisWeek = DateTime.Compare(invoice.Invoice_Date.Date, startDateThisWeek.Date);
                    int resultSalesInvoiceLastWeek = DateTime.Compare(invoice.Invoice_Date.Date, startDateLastWeek.Date);
                    if (resultSalesInvoiceThisWeek == 0)
                    {
                        Dayinvoice.FirstWeek[i] += 1;
                    }
                    if (resultSalesInvoiceLastWeek == 0)
                    {
                        Dayinvoice.LasttWeek[i] += 1;
                    }

                }
                foreach (var pos in posList)
                {
                    int resultSalesInvoiceThisWeek = DateTime.Compare(pos.Invoice_Date.Value.Date, startDateThisWeek.Date);
                    int resultSalesInvoiceLastWeek = DateTime.Compare(pos.Invoice_Date.Value.Date, startDateLastWeek.Date);
                    if (resultSalesInvoiceThisWeek == 0)
                    {
                        Dayinvoice.FirstWeek[i] += 1;
                    }
                    if (resultSalesInvoiceLastWeek == 0)
                    {
                        Dayinvoice.LasttWeek[i] += 1;
                    }

                }
                Dayinvoice.DayesOffTheWeek[i] = startDateThisWeek.DayOfWeek.ToString();
                dayesCountThisWeek++;
                dayesCountLastWeek++;
                startDateThisWeek = DateTime.Now.Date.AddDays(dayesCountThisWeek);
                startDateLastWeek = DateTime.Now.Date.AddDays(dayesCountLastWeek);
              
            }
           
      

            //int sales = db.SalesReceiptMasters.Where(a => a.Invoice_Date.AddDays == fromDate).Count();
            //for (int i = 0; i < 7; i++)
            //{
            //    int sales = db.SalesReceiptMasters.Where(a => a.Invoice_Date.AddDays(dayesCount) > DateTime).Count();
            //    dayesCount--;
            //}

            //int invoice = db.InvoiceMasters.Where(a => a.Invoice_Date == fromDate).Count();
            //int pos = db.MasterPOSInvoices.Where(a => a.Invoice_Date == fromDate).Count();

            //var ToDate = DateTime.Now.ToString("MM/dd/yyyy");

            //var fromLasweekDate = DateTime.Now.AddDays(-14);
            //int day = 1;
            //while(fromDate != ToDate)
            //{
            //    DailySalesVM Dayinvoice = new DailySalesVM();
            //    Dayinvoice.day = day;

            //    Dayinvoice.NumberOfSalesInvoices = pos + invoice + sales;
            //    Dayinvoice.LastWeekNumberOfSalesInvoices = 0;
            //    Dayinvoice.date = fromDate;
            //    DayList.Add(Dayinvoice);
            //    day++;
            //    fromDate = fromDate.AddDays(1);
            //}
            //while (fromLasweekDate != fromDate)
            //{
            //    DailySalesVM Dayinvoice = new DailySalesVM();
            //    Dayinvoice.day = day;
            //    int sales = db.SalesReceiptMasters.Where(a => a.Invoice_Date == fromLasweekDate).Count();
            //    int invoice = db.InvoiceMasters.Where(a => a.Invoice_Date == fromLasweekDate).Count();
            //    int pos = db.MasterPOSInvoices.Where(a => a.Invoice_Date == fromLasweekDate).Count();
            //    Dayinvoice.NumberOfSalesInvoices = 0;
            //    Dayinvoice.LastWeekNumberOfSalesInvoices = pos + invoice + sales;
            //    Dayinvoice.date = fromLasweekDate;
            //    DayList.Add(Dayinvoice);
            //    day++;
            //    fromLasweekDate = fromLasweekDate.AddDays(1);
            //}
            return Dayinvoice;
        }


    }
}