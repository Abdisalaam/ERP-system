using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Accountingsystem.Models
{
    public partial class MatchingPayment
    {
        public static bool containCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.MatchingPayments.ToList();
            if (!Serial.StartsWith("MATPAY-"))
            {
                return true;
            }
            else
            {
                foreach (var item in allinData)
                {
                    if (item.Match_Code == Serial || !Serial.StartsWith("MATPAY-"))
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
        // use this method to  Get List of MatchingPayment from Data base.
        public static List<MatchingPayment> GetAllMaching()
        {
            var db = new AccountingContainer();
            var Matchings = db.MatchingPayments.ToList();
            return Matchings;
        }

        // use this method to  Get one Element GetMatchingById from Data base. 
        public static MatchingPayment GetMatchingById(int id)
        {
            var db = new AccountingContainer();
            var Matching = db.MatchingPayments.Find(id);
            return Matching;
        }


        // spacific List of Payments and PurInvoiceMasters 
        // and Suppliers from other tabel 
        // spacific MatchingPayment Between PurInvoiceMasters and Payments
        public static MatchingPaymentVM CreatForCreation()

        {

            var db = new AccountingContainer();
            var matchiVM = new MatchingPaymentVM();

            matchiVM.MyCompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            matchiVM.MyBusinessUnitList = new SelectList(db.BussinesUnits, "BU_Id", "BU_Name");
            matchiVM.myPayCurrency = new SelectList(db.Currencies, "Currency_Id ", "Currency_Type");
            matchiVM.MyPaymentPay = new SelectList(db.Payments.Where(o => o.TotalAmountAfMat > 0), "Pay_ID", "Pay_Number");
            matchiVM.MyPurInvoiceMaster = new SelectList(db.PurInvoiceMasters.Where(o => o.TotalAmountAfMat > 0), "Master_Id", " Serial_Number");
            matchiVM.MySuppList = new SelectList(db.Suppliers, "Supp_Id", "Supp_Name");
            var lastSerial = db.MatchingPayments.Where(e=>e.Match_Code.StartsWith("MATPAY-")).ToList().LastOrDefault();
            if (lastSerial == null)
            {
                matchiVM.Match_Code = "MATPAY-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                matchiVM.Match_Code = "MATPAY-" + (Convert.ToInt32(lastSerial.Match_Code.Substring(7, lastSerial.Match_Code.Length - 7)) + 1).ToString("D3");
            }
            return matchiVM;
        }

        //  use this method to  Create New Matching and save it in DataBase.
        // after spacific data  another Tabel put this data in the model MatchingPayment 
        // and add data this tabel MatchingPayment put tabel MatchingPayment
        /// <summary>
        /// Used Method Create New Matching
        /// </summary>
        /// <param name="matchingVM"></param>
        /// <returns>bool</returns>
        public static bool CreateMatchingPayment(MatchingPaymentVM matchingVM)
        {
            var db = new AccountingContainer();
            var matchs = new MatchingPayment();


            matchs.Date = DateTime.Now;
            matchs.Match_state = TransactionState.Pending;
            var lastSerial = db.MatchingPayments.Where(e => e.Match_Code.StartsWith("MATPAY-")).ToList().LastOrDefault();
            if (lastSerial == null)
            {
                matchs.Match_Code = "MATPAY-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                matchs.Match_Code = "MATPAY-" + (Convert.ToInt32(lastSerial.Match_Code.Substring(7, lastSerial.Match_Code.Length - 7)) + 1).ToString("D3");
            }
            // create selectList from Database to entity Currency.
            var resultPayCurrency = db.Currencies.Find(matchingVM.Match_currency);
            if (resultPayCurrency != null)
            {
                matchs.Match_currency = resultPayCurrency.Currency_Id;
            }

            // create selectList from Database to entity Company.
            var resultPAyCampId = db.Companies.Find(matchingVM.Comp_ID);
            if (resultPAyCampId != null)
            {
                matchs.Comp_ID = resultPAyCampId.Comp_Id;
            }

            // create selectList from Database to entity PayBu .
            var resultPayBu = db.BussinesUnits.Find(matchingVM.Match_BU);
            if (resultPayBu != null)
            {
                matchs.Match_BU = resultPayBu.BU_Id;
            }
            // Used this Object invMaster Spscefic Parameter PurInvoiceMasters
            var invpayMaster = db.PurInvoiceMasters.Find(matchingVM.SelectedPurInvoiceMaster_Id);
            // Used this Object Receipts Spscefic Parameter Payments
            var Payments = db.Payments.Find(matchingVM.SelectedPaymentPay_ID);
            // If the invpayMaster is larger than the Payments
            if (invpayMaster.TotalAmountAfMat >= Payments.TotalAmountAfMat)
            {
                var result = Payments.TotalAmountAfMat;
                invpayMaster.TotalAmountAfMat = invpayMaster.TotalAmountAfMat - Payments.TotalAmountAfMat;
                Payments.TotalAmountAfMat = 0;

                matchs.Tota_Amount = result;
            }
            // If the Payments is larger than the invpayMaster
            else
            {
                var amount = invpayMaster.TotalAmountAfMat;
                Payments.TotalAmountAfMat = Payments.TotalAmountAfMat - invpayMaster.TotalAmountAfMat;
                invpayMaster.TotalAmountAfMat = 0;

                matchs.Tota_Amount = amount;
            }

            //try
            //{
            if (matchingVM != null)
            {
                // create selectList from Database to entity Payments.
                var resultPayment = db.Payments.Find(matchingVM.SelectedPaymentPay_ID);
                {
                    matchs.PaymentPay_ID = resultPayment.Pay_ID;
                }
                // create selectList from Database to entity PurInvoiceMasters.
                var resultInvoiseMaster = db.PurInvoiceMasters.Find(matchingVM.SelectedPurInvoiceMaster_Id);
                {

                    matchs.PurInvoiceMaster_Id = resultInvoiseMaster.Master_Id;
                }
                // create selectList from Database to entity Supplier.
                // var ressultSupplier = db.Payments.Find(matchingVM.SelectedSupp_Id);
                //{
                matchs.Supp_Id = matchingVM.SelectedSupp_Id;
                //}

                // this is Batch number any transaction matching.
                var traBatch = new TransactionBatch();

                traBatch.OperationName = "Matching Payments";
                traBatch.Description = null;
                traBatch.Date =Convert.ToDateTime( matchingVM.Date);
                traBatch.UserId = "";

                db.MatchingPayments.Add(matchs);

                db.SaveChanges();

                traBatch.OperationId = matchs.Match_Id;
                TransactionBatch.AddNewTransaction(traBatch);
                db.Dispose();
                return true;

            }
            //}

            //catch (Exception ex)
            //{
            //    var msg = ex.Message;
            //    var result = ex.Data;
            //    var tt = result;
            //    throw ;
            //}
            return false;
        }

        //when spacific Supplier  spacific Payments Regards this Supplier
        /// <summary>
        /// spacific Supplier  spacific Payments
        /// </summary>
        /// <param name="suupId"></param>
        /// <returns>Payment</returns>

        public static IEnumerable<SelectListItem> GetPayment(int suupId)
        {
            var db = new AccountingContainer();
            IEnumerable<SelectListItem> Payment = new SelectList(db.Payments.Where(p => p.Supplier_Id == suupId && p.TotalAmountAfMat > 0), "Pay_ID", "Pay_Number");
            return Payment;
        }


        //when spacific Supplier  spacific PurInvoiceMasters Regards this Supplier
        /// <summary>
        /// spacific Supplier  spacific PurInvoiceMasters
        /// </summary>
        /// <param name="suupId"></param>
        /// <returns>PayInvoice</returns>
        public static IEnumerable<SelectListItem> GetPayInvoiceMaster(int suupId)
        {
            var db = new AccountingContainer();
            IEnumerable<SelectListItem> PayInvoice = new SelectList(db.PurInvoiceMasters.Where(f => f.Supplier_Id == suupId && f.TotalAmountAfMat > 0), "Master_Id", "Serial_Number");
            return PayInvoice;
        }

        // Used this Method Partial Other model in this Model Matcing view data Details GetSupAllPurInvoiceDetailsVM
        /// <summary>
        ///  Method Partial Other model in this Model Matcing
        /// </summary>
        /// <param name="supId"></param>
        /// <returns>SupInvoiceDetailsVM</returns>
        public static List<SupInvoiceDetailsVM> GetSupAllPurInvoiceDetailsVM(int supId)
        {
            var db = new AccountingContainer();
            var InvoiceDetails = db.PurInvoiceMasters.Where(s => s.Supplier_Id == supId).ToList();

            List<SupInvoiceDetailsVM> mySupInvoList = new List<SupInvoiceDetailsVM>();

            foreach (var invoice in InvoiceDetails)
            {
                var _mySupInvoList = new SupInvoiceDetailsVM();
                _mySupInvoList.Id = invoice.Master_Id;
                _mySupInvoList.Serial_Number = invoice.Serial_Number;
                _mySupInvoList.Invoice_Date = invoice.Invoice_Date;
                _mySupInvoList.Total_Amount = invoice.Total_Amount;
                _mySupInvoList.TotalAmountAfMat = invoice.TotalAmountAfMat;
                mySupInvoList.Add(_mySupInvoList);
            }

            return mySupInvoList;
        }
        // Used this Method Partial Other model in this Model Matcing view data Details GetPurInvoiceDetailsVM
        /// <summary>
        ///  this Method Partial Other model in this Model
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns>PurInvoiceDetailVM</returns>
        public static MatchingPurInvoiceDetailVM GetPurInvoiceDetailsVM(int invoiceId)
        {
            var db = new AccountingContainer();
            var InvoiceDetails = db.PurInvoiceMasters.Find(invoiceId);
            MatchingPurInvoiceDetailVM invoiceDetailsVM = new MatchingPurInvoiceDetailVM();

            invoiceDetailsVM.Master_Id = InvoiceDetails.Master_Id;
            invoiceDetailsVM.Serial_Number = InvoiceDetails.Serial_Number;
            invoiceDetailsVM.Description = InvoiceDetails.Description;
            invoiceDetailsVM.Invoice_Date = InvoiceDetails.Invoice_Date;
            invoiceDetailsVM.Due_Date = Convert.ToDateTime(InvoiceDetails.Due_Date);
            invoiceDetailsVM.GL_Date = Convert.ToDateTime(InvoiceDetails.GL_Date);
            invoiceDetailsVM.Master_Discount = InvoiceDetails.Master_Discount;
            invoiceDetailsVM.Invoice_Currency = InvoiceDetails.Invoice_Currency;
            invoiceDetailsVM.Total_Amount = InvoiceDetails.Total_Amount;
            invoiceDetailsVM.Total_Tax = InvoiceDetails.Total_Tax;
            invoiceDetailsVM.Total_Cost = InvoiceDetails.Total_Cost;
            invoiceDetailsVM.Supplier_Id = InvoiceDetails.Supplier_Id;
            invoiceDetailsVM.Store_ID = Convert.ToInt32(InvoiceDetails.Store_ID);
            invoiceDetailsVM.Company_Id = InvoiceDetails.Company_Id;
            invoiceDetailsVM.TotalAmountAfMat = InvoiceDetails.TotalAmountAfMat;

            return invoiceDetailsVM;
        }
        // Used this Method Partial Other model in this Model Matcing view data Details GetPaymentsDetailsVM
        /// <summary>
        /// this Method Partial Other model in this Model Matcing
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns>PaymentVM</returns>
        public static PaymentVM GetPaymentsDetailsVM(int paymentId)
        {
            var db = new AccountingContainer();
            var payment = db.Payments.Find(paymentId);
            PaymentVM paymentDetailsVM = new PaymentVM();

            paymentDetailsVM.Pay_ID = payment.Pay_ID;
            paymentDetailsVM.PayComp_ID = payment.PayComp_ID;
            paymentDetailsVM.Pay_Amount = payment.Pay_Amount;
            //  paymentDetailsVM.Exchange_Rate = payment.Exchange_Rate;
            paymentDetailsVM.Pay_Number = payment.Pay_Number;
            paymentDetailsVM.PayBank_Account = payment.PayBank_Account;
            paymentDetailsVM.Rec_Description = payment.Rec_Description;
            paymentDetailsVM.PayGl_Date = payment.PayGl_Date.ToString();
            paymentDetailsVM.Pay_Date = payment.Pay_Date.ToString();
            paymentDetailsVM.Pay_State = payment.Pay_State;
            paymentDetailsVM.Pay_BU = payment.Pay_BU;
            paymentDetailsVM.TotalAmountAfMat = payment.TotalAmountAfMat;
            paymentDetailsVM.Supplier_Id = payment.Supplier_Id;

            return paymentDetailsVM;
        }
        // use this method For Edite Matching and save it DataBase.
        //Update MatchingPayment on the data from this tabel MatchingPayment 
        // and Update Gl Header and Upadte GLDetail PurInvoiceMasters and Payments
        /// <summary>
        ///  this method For Edite Matching 
        /// </summary>
        /// <param name="MatchVM"></param>
        /// <returns>bool</returns>
        public static bool UpdateMatching(MatchingPaymentVM MatchVM)
        {
            var db = new AccountingContainer();
            var match = db.MatchingPayments.Find(MatchVM.Match_Id);
            match.Match_state = MatchVM.Match_state;
            // Used this State Spacific State Gl start Pending 
            db.Entry(match).State = EntityState.Modified;

            // Used this GLHeader Create GL
            GLMaster glHeader = new GLMaster();

            glHeader.GL_Date = match.Date;
            glHeader.GL_Desc = "MatchingPayment";
            glHeader.Company_Id = match.Comp_ID;
            glHeader.GLType_Id = "MatchingPayment";
            glHeader.Currency_Id = (match.Match_currency).ToString();
            glHeader.GLState = TransactionState.Pending;
            glHeader.Auto = true;
            glHeader.GLCode = match.Match_Code;

            // this GL Details Start PrePayment_Vendors Secound AccountsPayable
            GLDetail PrePayment_Vendors = new GLDetail(),
                AccountsPayable = new GLDetail();

            // Enter the data  Supllier
            PrePayment_Vendors.Bu_Id =0;
            PrePayment_Vendors.Asset_Id = null;
            // Used ChartOfAccountRecive  Spacefic Account this PrePayment_Vendors
            var ChartOfAccounAddvance = db.ChartOfAccountings.Find(156);
            PrePayment_Vendors.Account_Id = ChartOfAccounAddvance.Account_Id;
            PrePayment_Vendors.Account_Name = ChartOfAccounAddvance.Account_Name;
            PrePayment_Vendors.GLMasterGLMaster_Id = glHeader.GLMaster_Id;
            PrePayment_Vendors.Description_Detail = "دفعات مقدمة موردين";

            // Enter the data AdvancedPayments
            AccountsPayable.Bu_Id = 0;
            AccountsPayable.Asset_Id = null;
            // Used ChartOfAccountRecive  Spacefic Account this AccountsPayable .
            var ChartOfAccounSupp = db.ChartOfAccountings.Find(45);
            AccountsPayable.Account_Id = ChartOfAccounSupp.Account_Id;
            AccountsPayable.Account_Name = ChartOfAccounSupp.Account_Name;
            AccountsPayable.GLMasterGLMaster_Id = glHeader.GLMaster_Id;
            AccountsPayable.Description_Detail = "حساب موردين";

            // This State GL Start Pending Secound Post thierd Voiod 
            switch (match.Match_state)
            {

                //Post GL
                case TransactionState.Post:
                    // From Account AccountsPayable to PrePayment_Vendors .
                    AccountsPayable.GLDetail_Debit = match.Tota_Amount;  //From
                    PrePayment_Vendors.GLDetail_Credit = match.Tota_Amount; //To

                    break;

                case TransactionState.Void:
                    // From Account PrePayment_Vendors to AccountsPayable .
                    PrePayment_Vendors.GLDetail_Debit = match.Tota_Amount;
                    AccountsPayable.GLDetail_Credit = match.Tota_Amount;

                    break;
                default:
                    break;
            }

            // add gl details in the gl header 
            // Supllier
            // AdvancedPayments
            db.GLMasters.Add(glHeader);
            glHeader.GLDetails.Add(PrePayment_Vendors);
            glHeader.GLDetails.Add(AccountsPayable);
            GLMaster.GetBalanceForAccount(glHeader);
            GLMaster.GetBalanceForSubLedger(glHeader);
            db.SaveChanges();
            db.Dispose();



            return true;

        }


    }

}