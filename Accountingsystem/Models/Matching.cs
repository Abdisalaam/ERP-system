using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class Matching
    {
        // use this method to  Get List of Matching from Data base.
        public static List<Matching> GetAllMaching()
        {
            var db = new AccountingContainer();
            var Matchings = db.Matchings.ToList();
            return Matchings;
        }


        // Used this Method Partial Other model in this Model Matcing view data Details GetCusAllInvoiceDetailsVM
        /// <summary>
        /// this Method Partial Other model in this Model Matcing
        /// </summary>
        /// <param name="CusId"></param>
        /// <returns>CusInvoiceDetailsVM</returns>
        public static List<CusInvoiceDetailsVM> GetCusAllInvoiceDetailsVM(int CusId)
        {
            var db = new AccountingContainer();
            var InvoiceDetails = db.InvoiceMasters.Where(s => s.Customer_Id == CusId).ToList();

            List<CusInvoiceDetailsVM> mycustomerList = new List<CusInvoiceDetailsVM>();

            foreach (var invoice in InvoiceDetails)
            {
                var _myCustomerList = new CusInvoiceDetailsVM();
                _myCustomerList.Id = invoice.Master_Id;
                _myCustomerList.Serial_Number = invoice.Serial_Number;
                _myCustomerList.Invoice_Date = invoice.Invoice_Date;
                _myCustomerList.Total_Amount = invoice.Total_Amount;
                _myCustomerList.TotalAmountAfMat = invoice.TotalAmountAfMat;
                mycustomerList.Add(_myCustomerList);
            }

            return mycustomerList;
        }

        // Used this Method Partial Other model in this Model Matcing view data Details GetInvoiceDetailsVM
        /// <summary>
        /// this Method Partial Other model in this Model Matcing
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns>InvoiceDetailsVM</returns>
        public static MatchingInvoiceDetailsVM GetInvoiceDetailsVM(int invoiceId)
        {
            var db = new AccountingContainer();
            var InvoiceDetails = db.InvoiceMasters.Find(invoiceId);
            MatchingInvoiceDetailsVM invoiceDetailsVM = new MatchingInvoiceDetailsVM();

            invoiceDetailsVM.Company_Id = InvoiceDetails.Company_Id;
            invoiceDetailsVM.Customer_Id = InvoiceDetails.Customer_Id;
            invoiceDetailsVM.Description = InvoiceDetails.Description;
            invoiceDetailsVM.Due_Date = Convert.ToDateTime(InvoiceDetails.Due_Date);
            //invoiceDetailsVM.GL_Date = Convert.ToDateTime(InvoiceDetails.GL_Date);
            invoiceDetailsVM.Invoice_Currency = InvoiceDetails.Invoice_Currency;
            invoiceDetailsVM.Invoice_Date = InvoiceDetails.Invoice_Date;
            invoiceDetailsVM.Master_Discount = InvoiceDetails.Master_Discount;
            invoiceDetailsVM.Master_Id = InvoiceDetails.Master_Id;
            invoiceDetailsVM.Serial_Number = InvoiceDetails.Serial_Number;
            invoiceDetailsVM.Store_ID = InvoiceDetails.Store_ID;
            invoiceDetailsVM.TotalAmountAfMat = InvoiceDetails.TotalAmountAfMat;
            invoiceDetailsVM.Total_Amount = InvoiceDetails.Total_Amount;
            invoiceDetailsVM.Total_Cost = InvoiceDetails.Total_Cost;
            invoiceDetailsVM.Total_Tax = InvoiceDetails.Total_Tax;
            invoiceDetailsVM.Account_ID = InvoiceDetails.RevenueAccount_Id;

            return invoiceDetailsVM;
        }

        // Used this Method Partial Other model in this Model Matcing view data Details GetReceiptDetailsVM
        /// <summary>
        /// this Method Partial Other model in this Model Matcing
        /// </summary>
        /// <param name="receiptId"></param>
        /// <returns>ReceiptDetailsVM</returns>
        public static MatchingReceiptDetailsVM GetReceiptDetailsVM(int receiptId)
        {
            var db = new AccountingContainer();
            var receiptDetails = db.Receipts.Find(receiptId);
            MatchingReceiptDetailsVM receiptDetailsVM = new MatchingReceiptDetailsVM();

            receiptDetailsVM.RecComp_ID = receiptDetails.RecComp_ID.Value;
            receiptDetailsVM.Rec_ID = receiptDetails.Rec_ID;
            receiptDetailsVM.Rec_Amount = receiptDetails.Rec_Amount;
        
            receiptDetailsVM.Rec_Number = receiptDetails.Rec_Number;
            receiptDetailsVM.RecBank_Account = receiptDetails.RecBank_Account;
            receiptDetailsVM.Rec_Description = receiptDetails.Rec_Description;          
            receiptDetailsVM.Rec_Date = receiptDetails.Rec_Date;
            receiptDetailsVM.Rec_state = receiptDetails.Rec_state;
            receiptDetailsVM.Rec_BU = receiptDetails.Rec_BU;
            receiptDetailsVM.TotalAmountAfMat = receiptDetails.TotalAmountAfMat;
            receiptDetailsVM.RecCurrency_ID = receiptDetails.RecCurrency_ID;
            receiptDetailsVM.Customer_Id = receiptDetails.Customer_Id;

            return receiptDetailsVM;
        }

        // use this method to  Get one Element GetMatchingById from Data base. 
        /// <summary>
        /// GetMatchingById
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Matching</returns>
        public static Matching GetMatchingById(int id)
        {
            var db = new AccountingContainer();
            var Matching = db.Matchings.Find(id);
            return Matching;
        }


        // spacific List of receipt and InvoiceMasters 
        // and Custgomer from other tabel 
        // spacific matching Between Invoce Master and Receipt
        public static MatchingVM CreatForCreation()
        {
            var db = new AccountingContainer();
            var matchVM = new MatchingVM();



            matchVM.MyReceiptList = new SelectList(db.Receipts.Where(o => o.TotalAmountAfMat > 0), "Rec_ID", "Rec_Number");
            matchVM.MyInvoiseMasterList = new SelectList(db.InvoiceMasters.Where(o => o.TotalAmountAfMat > 0), "Master_Id", "Serial_Number");
            matchVM.MyCustomerList = new SelectList(db.Customers, "Customer_Id", "Customer_Name");
            matchVM.MyCompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            //matchVM.MyBusinessUnitList = new SelectList(db.BussinesUnits, "BU_Id", "BU_Name");
            matchVM.myPayCurrency = new SelectList(db.Currencies, "Currency_Id ", "Currency_Type");
            var lastCode = db.Matchings.OrderByDescending(c => c.Match_Id).FirstOrDefault();
            if (lastCode == null)
            {
                matchVM.Matching_Code = "MATCH-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                matchVM.Matching_Code = "MATCH-" + (Convert.ToInt32(lastCode.Matching_Code.Substring(6, lastCode.Matching_Code.Length - 6)) + 1).ToString("D3");
            }
            return matchVM;
        }

        //  use this method to  Create New Matching and save it in DataBase.
        // after spacific data  another Tabel put this data in the model Matching 
        // and add data this tabel Mathing put tabel Matching
        /// <summary>
        /// Used Method Create New Matching
        /// </summary>
        /// <param name="matchingVM"></param>
        /// <returns>bool</returns>
        public static bool containCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.Matchings.ToList();
            if (!Serial.StartsWith("MATCH-"))
            {
                return true;
            }
            else
            {
                foreach (var item in allinData)
                {

                    if (item.Matching_Code == Serial || !Serial.StartsWith("MATCH-"))
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
        public static bool CreateMatching(MatchingVM matchingVM)
        {
            var db = new AccountingContainer();
            var matchs = new Matching();

            matchs.Comp_ID = matchingVM.Comp_ID;
            matchs.Date = Convert.ToDateTime( matchingVM.Date);
            //matchs.Match_BU = matchingVM.Match_BU;
            matchs.Match_currency = matchingVM.Match_currency;
            matchs.Match_state = TransactionState.Pending;
            var lastCode = db.Matchings.OrderByDescending(c => c.Match_Id).FirstOrDefault();
            if (lastCode == null)
            {
                matchs.Matching_Code = "MATCH-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                matchs.Matching_Code = "MATCH-" + (Convert.ToInt32(lastCode.Matching_Code.Substring(6, lastCode.Matching_Code.Length - 6)) + 1).ToString("D3");
            }


            // Used this Object invMaster Spscefic Parameter InvoiceMasters
            var invMaster = db.InvoiceMasters.Find(matchingVM.SelectedInvoiceMaster_Id);
            // Used this Object Receipts Spscefic Parameter Receipts
            var reciept = db.Receipts.Find(matchingVM.SelectedReceiptId);
            // If the invoice is larger than the reciept

            if (invMaster.TotalAmountAfMat >= reciept.TotalAmountAfMat)
            {
                var result = reciept.TotalAmountAfMat;
                invMaster.TotalAmountAfMat = invMaster.TotalAmountAfMat - reciept.TotalAmountAfMat;
                reciept.TotalAmountAfMat = 0;
                matchs.Tota_Amount = result;
            }
            // If the receipt is larger than the invoice
            else
            {
                var amount = invMaster.TotalAmountAfMat;
                reciept.TotalAmountAfMat = reciept.TotalAmountAfMat - invMaster.TotalAmountAfMat;
                invMaster.TotalAmountAfMat = 0;
                matchs.Tota_Amount = amount;
            }


            try
            {
                if (matchingVM != null)
                {
                    // create selectList from Database to entity Receipts.
                    var resultReceipt = db.Receipts.Find(matchingVM.SelectedReceiptId);
                    {
                        matchs.ReceiptRec_ID = resultReceipt.Rec_ID;
                    }
                    // create selectList from Database to entity InvoiceMasters.
                    var resultInvoiseMaster = db.InvoiceMasters.Find(matchingVM.SelectedInvoiceMaster_Id);

                    {

                        matchs.InvoiceMasterMaster_Id = resultInvoiseMaster.Master_Id;

                    }

                    // create selectList from Database to entity Customers.
                    //var ressultCustomer = db.Customers.Find(matchingVM.SelectedCustomerID);
                    //{
                    matchs.Customer_Id = matchingVM.SelectedCustomerID;
                    // }


                    // this is Batch number any transaction matching. 

                    var traBatch = new TransactionBatch();

                    traBatch.OperationName = "Matching Receipt";
                    traBatch.Description = null;
                    traBatch.Date = Convert.ToDateTime(matchingVM.Date);
                    traBatch.UserId = "";


                    db.Matchings.Add(matchs);

                    db.SaveChanges();
                    traBatch.OperationId = matchs.Match_Id;
                    TransactionBatch.AddNewTransaction(traBatch);
                    db.Dispose();


                    return true;

                }
            }

            catch (Exception ex)
            {
                var msg = ex.Message;
                var result = ex.Data;
                var tt = result;
                throw;
            }
            return false;
        }



        //when spacific Customer  spacific InvoiceMasters Regards this customer
        /// <summary>
        ///  spacific Customer  spacific InvoiceMasters
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns>InvoiseMaster</returns>

        public static IEnumerable<SelectListItem> GetCusromerInvoise(int CustomerId)
        {
            var db = new AccountingContainer();
            IEnumerable<SelectListItem> InvoiseMaster = new SelectList(db.InvoiceMasters.Where(p => p.Customer_Id == CustomerId && p.TotalAmountAfMat > 0), "Master_Id", "Serial_Number");
            return InvoiseMaster;
        }

        //when spacific Customer  spacific Receipts Regards this customer
        /// <summary>
        ///  spacific Customer  spacific Receipts
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns>Recipt</returns>
        public static IEnumerable<SelectListItem> GetCustomerRecipt(int CustomerId)
        {
            var db = new AccountingContainer();
            IEnumerable<SelectListItem> Recipt = new SelectList(db.Receipts.Where(f => f.Customer_Id == CustomerId && f.TotalAmountAfMat > 0), "Rec_ID", "Rec_Number");
            return Recipt;
        }
        // use this method For Edite Matching and save it DataBase.
        //Update Matching on the data from this tabel Matching 
        // and Update Gl Header and Upadte GLDetail InvoiceMasters and Receipts
        /// <summary>
        ///  Edite Matching
        /// </summary>
        /// <param name="MatchVM"></param>
        /// <returns>bool</returns>
        public static bool UpdateMatching(MatchingVM MatchVM)
        {
            var db = new AccountingContainer();
            var match = db.Matchings.Find(MatchVM.Match_Id);
            match.Match_state = MatchVM.Match_state;

            // Used this State Spacific State Gl start Pending 
            db.Entry(match).State = EntityState.Modified;
            // Used this GLHeader Create GL
            GLMaster glHeader = new GLMaster();
            glHeader.GLCode = match.Matching_Code;
            glHeader.GL_Date = match.Date;
            glHeader.GL_Desc = "خصم ايصال";
            glHeader.Company_Id = match.Comp_ID;
            glHeader.GLType_Id = "خصم ايصال من فاتورة";
            glHeader.Currency_Id = match.Match_currency;
            glHeader.GLState = TransactionState.Pending;
            glHeader.Auto = true;
            // this GL Details Start CustomerAdvanced SecouE:\Alsalam Project\Alsalam V2\Final Alsalam 2-7-2018\Accountingsystem\Models\MeasuringUnit.csnd AccountReceivableGL
            GLDetail CustomerAdvanced = new GLDetail(),
                AccountReceivableGL = new GLDetail();

            // Enter the data CustomerAdvanced 
            CustomerAdvanced.Bu_Id = 0;
            CustomerAdvanced.Asset_Id = null;
            // Used ChartOfAccountRecive  Spacefic Account this CustomerAdvanced. 
            var ChartOfAccounAdvanced = db.ChartOfAccountings.Find(155);
            CustomerAdvanced.Account_Id = ChartOfAccounAdvanced.Account_Id;
            CustomerAdvanced.Account_Name = ChartOfAccounAdvanced.Account_Name;
            CustomerAdvanced.GLMasterGLMaster_Id = glHeader.GLMaster_Id;

            // Enter the data AccountReceivableGL
            AccountReceivableGL.Bu_Id = 0;
            AccountReceivableGL.Asset_Id = null;
            // Used ChartOfAccountRecive  Spacefic Account this AccountReceivableGL. 
            var ChartOfAccounRecive = db.ChartOfAccountings.Find(37);
            AccountReceivableGL.Account_Id = ChartOfAccounRecive.Account_Id;
            AccountReceivableGL.Account_Name = ChartOfAccounRecive.Account_Name;
            AccountReceivableGL.GLMasterGLMaster_Id = glHeader.GLMaster_Id;


            var cust = db.Customers.Find(match.Customer_Id);
            Accounts customerAccount = db.Accounts.Where(subled => subled.Account_Code == cust.Customer_Code).FirstOrDefault();
            AccountReceivableGL.Account_Child_ID = customerAccount.Accounts_Id;


            // This State GL Start Pending Secound Post thierd Voiod 
            // Update the state Gl frome depite to Bank
            // and  The opposite gl from cridet to AdvancedPayments 
            switch (match.Match_state)
            {
                //Post GL
                case TransactionState.Post:

                    // From Account CustomerAdvanced to AccountReceivableGL .

                    CustomerAdvanced.GLDetail_Debit = match.Tota_Amount;
                    AccountReceivableGL.GLDetail_Credit = match.Tota_Amount;//8////////

                    break;
                // Void Matching
                case TransactionState.Void:
                    // From Account AccountReceivableGL to CustomerAdvanced .

                    AccountReceivableGL.GLDetail_Debit = match.Tota_Amount;
                    CustomerAdvanced.GLDetail_Credit = match.Tota_Amount;

                    break;
                default:
                    break;
            }

            // add gl details in the gl header 
            // Bank
            // AdvancedPayments
            db.GLMasters.Add(glHeader);

            glHeader.GLDetails.Add(CustomerAdvanced);
            glHeader.GLDetails.Add(AccountReceivableGL);


            GLMaster.GetBalanceForAccount(glHeader);
            GLMaster.GetBalanceForSubLedger(glHeader);
            db.SaveChanges();
            db.Dispose();
            return true;

        }
    }
}