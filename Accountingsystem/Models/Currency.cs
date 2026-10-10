using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Accountingsystem.Models
{
    public partial class Currency
    {
        public static List<Currency> GetAllcurrency()
        // i want here to get all Currency in a list 
        {
            var db = new AccountingContainer();
            var currency = db.Currencies.ToList();
            return currency;
        }
        public static Currency GetCurrencyById(int id)
        //Person will get Currency by iD to find it 
        // detail function
        {
            var db = new AccountingContainer();
            var currency = db.Currencies.Find(id);
        
            return currency;
        }
        /// <summary>
        /// get Currency VM for add in it
        /// </summary>
        /// <returns></returns>
        public static CurrencyVM CreateCurrencyVM()
        {
            var newCur = new CurrencyVM();
            return newCur;
        }
        public static Currency CreateCurrency(CurrencyVM currVM)
        //CurrencyVM we made it to add name of the Currency ( type & country & ISO ) in the DB.
        {
            var db = new AccountingContainer();
            var currency = new Currency();
            try
            {
                if (currVM != null)
                {

                    currency.Currency_Type = currVM.Currency_Type;
                    currency.Currency_Name = currVM.Currency_Name;

                    currency.Currency_ISO = currVM.Currency_ISO;
                    currency.Currency_Iso2 = currVM.Currency_Iso2;

                    currency.Break_Coin = currVM.Break_Coin;
                    currency.Break_Coin2 = currVM.Break_Coin2;
                    currency.Current_Price = currVM.Current_Price;
                    currency.Currency_Country = currVM.Currency_Country;


                    db.Currencies.Add(currency);
                    db.SaveChanges();
                    db.Dispose();
                    return currency;
                }


            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                var result = ex.Data;
                var tt = result;
                throw;
            }
            return null;
        }
        public static Currency EidtCurrncy(CurrencyVM curr)
        //we made that function to edit in the DB of the Currency after editing
        //we will go to the DB . we will get the Currency by id .if Currency does not exist it will return by null .
        //else it will get to Currency view model 
        {
            var db = new AccountingContainer();
            var mycurrncy = db.Currencies.Find(curr.Currency_Id);
            if (mycurrncy != null)
            {
                mycurrncy.Currency_Type = curr.Currency_Type;
                mycurrncy.Currency_ISO = curr.Currency_ISO;
                mycurrncy.Currency_Country = curr.Currency_Country;


                mycurrncy.Currency_Name = curr.Currency_Name;
                mycurrncy.Currency_Iso2 = curr.Currency_Iso2;
                mycurrncy.Break_Coin = curr.Break_Coin;
                mycurrncy.Break_Coin2 = curr.Break_Coin2;
                mycurrncy.Current_Price = curr.Current_Price;


                db.Entry(mycurrncy).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                return mycurrncy;
            }
            return null;
        }
        public static CurrencyVM FillCurrncy(int id)
        // get for edit return the view of the screen fill of person's data for user
        {
            var db = new AccountingContainer();
            var Fa = db.Currencies.Find(id);
            CurrencyVM data = new CurrencyVM();

            data.Currency_Id = id;
            data.Currency_Type = Fa.Currency_Type;
            data.Currency_ISO = Fa.Currency_ISO;
            data.Currency_Country = Fa.Currency_Country;

            data.Currency_Name = Fa.Currency_Name;
            data.Currency_Iso2 = Fa.Currency_Iso2;
            data.Break_Coin = Fa.Break_Coin;
            data.Break_Coin2 = Fa.Break_Coin2;
            data.Current_Price = Fa.Current_Price.Value;
            return data;
        }

        public static Currency FindCurrencyById(int id)
        // find Currency by id
        {
            var db = new AccountingContainer();
            var del = db.Currencies.Find(id);
            return del;
        }
        public static int DelCurrency(int currId)
        //delete the currency by id 
        {
            var db = new AccountingContainer();
            var myCurrency = db.Currencies.Find(currId);
            if (CurrencyExist(currId))
            {
                return 0;
            }
            else
            {

                db.Currencies.Remove(myCurrency);
                db.SaveChanges();
                db.Dispose();
                return 1;

            }


        }


        //this  MEthod  is  to Check  if  the  Currency  i need  to Delete is  in Use with  any  Process or  any  Data
        public static bool CurrencyExist(int CurID) 
        {
            var db = new AccountingContainer();
            var thiscurrecny = db.Currencies.Find(CurID);
            var curInbank = db.Banks.Where(e => e.Bank_Currency == thiscurrecny.Currency_Type || e.Bank_Currency == thiscurrecny.Currency_Name).ToList();
            if (curInbank.Count > 0)
            {
                return true;
            }
            var curInChartofAccount = db.ChartOfAccountings.Where(e => e.Currency_Name == thiscurrecny.Currency_Type || e.Currency_Name == thiscurrecny.Currency_Name).ToList();
            if (curInChartofAccount.Count > 0)
            {
                return true;
            }
            var curIncatchreciepts = db.CatchReceipts.Where(e => e.Currency_Id == thiscurrecny.Currency_Id).ToList();
            if (curIncatchreciepts.Count > 0)
            {
                return true;
            }
            var curInCompany = db.Companies.Where(e => e.Comp_Currency_Id == thiscurrecny.Currency_Id).ToList();
            if (curInCompany.Count > 0)
            {
                return true;
            }
            var curInExpense = db.Expenses.Where(e => e.Currency_Id == thiscurrecny.Currency_Id).ToList();
            if (curInExpense.Count > 0)
            {
                return true;
            }
            var curInGLMaster = db.GLMasters.Where(e => e.Currency_Id == thiscurrecny.Currency_Id.ToString()||e.Currency_Id == thiscurrecny.Currency_Type || e.Currency_Id == thiscurrecny.Currency_Name).ToList();
            if (curInGLMaster.Count > 0)
            {
                return true;
            }
            var curInFixedAssets = db.FixedAssets.Where(e => e.Currency == thiscurrecny.Currency_Type || e.Currency == thiscurrecny.Currency_Name).ToList();
            if (curInFixedAssets.Count > 0)
            {
                return true;
            }
            var curInInvoiceMastertemp = db.InvoiceMasterTemps.Where(e => e.Invoice_Currency == thiscurrecny.Currency_Type || e.Invoice_Currency == thiscurrecny.Currency_Name).ToList();
            if (curInInvoiceMastertemp.Count > 0)
            {
                return true;
            }
            var curInInvoiceMaster = db.InvoiceMasters.Where(e => e.Invoice_Currency == thiscurrecny.Currency_Type || e.Invoice_Currency == thiscurrecny.Currency_Name).ToList();
            if (curInInvoiceMaster.Count > 0)
            {
                return true;
            }
            var curInInventoryTansTemp = db.InventoryTransfer_HdTemp.Where(e => e.Inv_Currency == thiscurrecny.Currency_Type || e.Inv_Currency == thiscurrecny.Currency_Name).ToList();
            if (curInInventoryTansTemp.Count > 0)
            {
                return true;
            }
            var curIninventoryTransfersHD = db.InventoryTransfer_Hd.Where(e => e.Inv_Currency == thiscurrecny.Currency_Type || e.Inv_Currency == thiscurrecny.Currency_Name).ToList();
            if (curIninventoryTransfersHD.Count > 0)
            {
                return true;
            }
            var curInOfferPriceMaster = db.OfferPriceMasters.Where(e => e.Currency_Id == thiscurrecny.Currency_Id).ToList();
            if (curInOfferPriceMaster.Count > 0)
            {
                return true;
            }
            var curInMatching = db.Matchings.Where(e => e.Match_currency == thiscurrecny.Currency_Type||e.Match_currency==thiscurrecny.Currency_Name).ToList();
            if (curInMatching.Count > 0)
            {
                return true;
            }
            var curInMatchingPayment = db.MatchingPayments.Where(e => e.Match_currency == thiscurrecny.Currency_Id).ToList();
            if (curInMatchingPayment.Count > 0)
            {
                return true;
            }
            var curInPurchinvMasterTemp = db.PurInvoiceMasterTemps.Where(e => e.Invoice_Currency == thiscurrecny.Currency_Type|| e.Invoice_Currency == thiscurrecny.Currency_Name).ToList();
            if (curInPurchinvMasterTemp.Count > 0)
            {
                return true;
            }
            var curInPurchinvMaster = db.PurInvoiceMasters.Where(e => e.Invoice_Currency == thiscurrecny.Currency_Type || e.Invoice_Currency == thiscurrecny.Currency_Name).ToList();
            if (curInPurchinvMaster.Count > 0)
            {
                return true;
            }
            var curInPurchOrderMaster = db.PurchaseOrderMasters.Where(e => e.Invoice_Currency == thiscurrecny.Currency_Type || e.Invoice_Currency == thiscurrecny.Currency_Name).ToList();
            if (curInPurchOrderMaster.Count > 0)
            {
                return true;
            }
            var curInPayments = db.Payments.Where(e => e.Pay_Currency_ID == thiscurrecny.Currency_Type || e.Pay_Currency_ID == thiscurrecny.Currency_Name).ToList();
            if (curInPayments.Count > 0)
            {
                return true;
            }
            var curInReverseSalesReciptMasterTemp1 = db.ReverseSalesReceiptMasterTemp1.Where(e => e.Invoice_Currency == thiscurrecny.Currency_Type || e.Invoice_Currency == thiscurrecny.Currency_Name||e.Invoice_Currency==thiscurrecny.Currency_Id.ToString()).ToList();
            if (curInReverseSalesReciptMasterTemp1.Count > 0)
            {
                return true;
            }
            var curInReverseSalesReciptMaster = db.ReverseSalesReceiptMasters.Where(e => e.Invoice_Currency == thiscurrecny.Currency_Type || e.Invoice_Currency == thiscurrecny.Currency_Name || e.Invoice_Currency == thiscurrecny.Currency_Id.ToString()).ToList();
            if (curInReverseSalesReciptMaster.Count > 0)
            {
                return true;
            }
            var curInReverseSalesinvoicetMaster = db.ReverseSalesInvoiceMasters.Where(e => e.Invoice_Currency == thiscurrecny.Currency_Type || e.Invoice_Currency == thiscurrecny.Currency_Name || e.Invoice_Currency == thiscurrecny.Currency_Id.ToString()).ToList();
            if (curInReverseSalesinvoicetMaster.Count > 0)
            {
                return true;
            }
            var curInReverseSalesinvoicetMastertemp = db.ReverseSalesInvoiceMasterTemps.Where(e => e.Invoice_Currency == thiscurrecny.Currency_Type || e.Invoice_Currency == thiscurrecny.Currency_Name || e.Invoice_Currency == thiscurrecny.Currency_Id.ToString()).ToList();
            if (curInReverseSalesinvoicetMastertemp.Count > 0)
            {
                return true;
            }
            var curInReciepts = db.Receipts.Where(e => e.RecCurrency_ID == thiscurrecny.Currency_Type || e.RecCurrency_ID == thiscurrecny.Currency_Name ).ToList();
            if (curInReciepts.Count > 0)
            {
                return true;
            }
            var curInRecalPurchaseMaster = db.RecalPurchInvoiceMasters.Where(e => e.PI_Currency == thiscurrecny.Currency_Type || e.PI_Currency == thiscurrecny.Currency_Name).ToList();
            if (curInRecalPurchaseMaster.Count > 0)
            {
                return true;
            }
            var curInRecalPurchaseMastertemp = db.RecallPurchInvoicMasterTemps.Where(e => e.PI_Currency == thiscurrecny.Currency_Type || e.PI_Currency == thiscurrecny.Currency_Name).ToList();
            if (curInRecalPurchaseMastertemp.Count > 0)
            {
                return true;
            }
            var curInSpentRecipt = db.SpentRecipts.Where(e => e.Currency_Id == thiscurrecny.Currency_Id ).ToList();
            if (curInSpentRecipt.Count > 0)
            {
                return true;
            }
            var curInsalesReciptsMaster = db.SalesReceiptMasters.Where(e => e.Invoice_Currency == thiscurrecny.Currency_Type||e.Invoice_Currency==thiscurrecny.Currency_Name).ToList();
            if (curInsalesReciptsMaster.Count > 0)
            {
                return true;
            }
            var curInsalesReciptsMastertemp = db.SalesReceiptMasterTemps.Where(e => e.Invoice_Currency == thiscurrecny.Currency_Type || e.Invoice_Currency == thiscurrecny.Currency_Name).ToList();
            if (curInsalesReciptsMastertemp.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}