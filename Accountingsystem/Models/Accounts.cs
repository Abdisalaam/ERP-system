using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.Models
{
    public partial class Accounts
    {
        /// <summary>
        /// 
        /// Get All Account
        /// </summary>
        /// <returns></returns>
        public static List<Accounts> getAllAccount()
        {
            var db = new AccountingContainer();
            var Account = db.Accounts.ToList();
          
            return Account;

        }
        /// <summary>
        /// get account by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Accounts findAccountbyID(int id)
        {
            var db = new AccountingContainer();
            var thisaccount = db.Accounts.Find(id);
            return thisaccount;
        }

        /// <summary>
        /// Get All Account To Make DrobDown List
        /// </summary>
        /// <returns></returns>
        public static List<Accounts> getAllAccountsFordrobDownList()
        {
            var db = new AccountingContainer();

            var lista = db.Accounts.ToList();

            return lista;


        }


        public static decimal RefreshTheBallanceOfCustomer(String code)
        {
            var db = new AccountingContainer();
            var Account = db.Accounts.Where(a => a.Account_Code == code).FirstOrDefault();
            var glList = db.GLDetails.Where(a => a.Account_Child_ID == Account.Accounts_Id).ToList();
            decimal TotalCredit = glList.Select(a => a.GLDetail_Credit).Sum();
            decimal TotalDepit = glList.Select(a => a.GLDetail_Debit).Sum();
            decimal ballance = TotalDepit - TotalCredit;
            Account.Account_Balance = ballance;
            db.SaveChanges();


            return ballance;


        }
        //public static bool DeleteSales()
        //{
        //    var db = new AccountingContainer();
        //    db.SalesReceiptDetails.RemoveRange(db.SalesReceiptDetails);
        //    db.SalesReceiptFiles.RemoveRange(db.SalesReceiptFiles);
        //    db.SalesReceiptTaxes.RemoveRange(db.SalesReceiptTaxes);
        //    db.ReverseSalesReceiptDetails.RemoveRange(db.ReverseSalesReceiptDetails);
        //    db.ReverseSalesReceiptFiles.RemoveRange(db.ReverseSalesReceiptFiles);
        //    db.ReverseSalesReceiptMasters.RemoveRange(db.ReverseSalesReceiptMasters);
        //    db.SalesReceiptMasters.RemoveRange(db.SalesReceiptMasters);

        //    db.InvoiceDetails.RemoveRange(db.SalesReceiptDetails);
        //    db.SalesFileDetails.RemoveRange(db.SalesReceiptFiles);
        //    db.InvoiceTaxes.RemoveRange(db.SalesReceiptTaxes);
        //    db.re.RemoveRange(db.ReverseSalesReceiptDetails);
        //    db.ReverseSalesReceiptFiles.RemoveRange(db.ReverseSalesReceiptFiles);
        //    db.ReverseSalesReceiptMasters.RemoveRange(db.ReverseSalesReceiptMasters);
        //    db.SalesReceiptMasters.RemoveRange(db.SalesReceiptMasters);


        //}

    }
}
