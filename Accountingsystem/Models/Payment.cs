using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class Payment
    {
        // use this method to  Get List of Payment from Data base. 
        public static List<Payment> GetAllPayment(int ?all)
        {
            var db = new AccountingContainer();
            var Pay = db.Payments.ToList();
            if (all == null)
            {
                Pay.Reverse();
                Pay = Pay.Take(100).ToList();
            }
            return Pay;
        }
        /// <summary>
        /// chek that date is suitable
        /// </summary>
        /// <returns></returns>
        public static bool Checkdate(string date)
        {
            var datetime = Convert.ToDateTime(date);
            var db = new AccountingContainer();
            var Comstartdate = db.Companies.FirstOrDefault().StartFinancialDate;
            if (Comstartdate != null)
            {
                if (datetime < Comstartdate.Value)
                {
                    return false;
                }


            }

            return true;


        }
        // use this method to  Get one Element PaymentById from Data base. 
        /// <summary>
        /// Used Method Get GetPaymentById.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>paym</returns>
        public static Payment GetPaymentById(int id)
        {
            var db = new AccountingContainer();
            var paym = db.Payments.Find(id);
            return paym;
        }
        /// <summary>
        /// get the file of payment by its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static PaymentFileDetail FindFileByID(int id)
        {
            var db = new AccountingContainer();
            var file = db.PaymentFileDetails.Find(id);
            return file;
        }
        public static string deleteFile(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.PaymentFileDetails.Find(fileId);
            db.PaymentFileDetails.Remove(file);
            db.SaveChanges();
            return Resources.validate.DeleteSuccessful;
        }
        /// <summary>
        /// get All Payment file for Payment that chosen
        /// </summary>
        /// <param name="PayId"></param>
        /// <returns></returns>
        public static List<PaymentFileDetail> GetAllFiles(int PayId)
        {
            var db = new AccountingContainer();
            var files = db.PaymentFileDetails.Where(emp => emp.PaymentPay_ID == PayId).ToList();
            return files;
        }
        // use this method For Creation new  Payment and get Data from user.
        public static PaymentVM AddPaymentVM(int? suppId)
        {
            var db = new AccountingContainer();
            var PaymentVM = new PaymentVM();

           

            //sort the Invoices and get the last insert Payment.
            var lastSerial = db.Payments.OrderByDescending(c => c.Pay_ID).FirstOrDefault();
            if (lastSerial == null)
            {
                PaymentVM.Pay_Number = "PAY-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                PaymentVM.Pay_Number = "PAY-" + (Convert.ToInt32(lastSerial.Pay_Number.Substring(4, lastSerial.Pay_Number.Length - 4)) + 1).ToString("D3");
            }


            PaymentVM.myPayCurrency = new SelectList(db.Currencies, "Currency_Id ", "Currency_Type");
            PaymentVM.MyCompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            PaymentVM.myPayBu = new SelectList(db.BussinesUnits, "BU_Id", "BU_Name");
            PaymentVM.mySupplierId = new SelectList(db.Suppliers.Where(a=>a.Supplier_state!=Customer_State.غير_نشط), "Supp_Id", "Supp_Name");
            if (suppId != null)
            {
                PaymentVM.Supplier_Id = Convert.ToInt32(suppId);
            }
           
            PaymentVM.MyPurInvoiceMaster = new SelectList(db.PurInvoiceMasters.Where(o => o.TotalAmountAfMat > 0 && o.Invoice_State == TransactionState.Post&&o.Supplier_Id==suppId), "Master_Id", " Serial_Number");
            PaymentVM.Pay_Date = DateTime.Now.ToString("dd-MM-yyyy   HH:mm:ss");
            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0&& a.isActive != false).ToList();
            
            PaymentVM.MyChartOfAccountList = new SelectList(lista.Where(accCode => accCode.Account_Code.StartsWith("0101200010")|| accCode.Account_Code.StartsWith("010120002")), "Account_Id", "Account_Name");
         
            return PaymentVM;
        }
        // use this method to  Create New Payment and save it in DataBase.
        // after spacific data  another Tabel put this data in the model 
        // and add data this tabel  put tabel Receipt
        /// <summary>
        /// Used Payment PaymentVM Create in database
        /// </summary>
        /// <param name="PaymentVM"></param>
        /// <returns>bool</returns>
        public static Tuple<String,int> AddNewPayment(PaymentVM PaymentVM , string UserId)
        {
            var db = new AccountingContainer();
            var NewPayment = new Payment();

            try
            {
                if (PaymentVM != null)
                {
                    // create selectList from Database to entity Currency.
                    var resultPayCurrency = db.Currencies.FirstOrDefault();
                    if (resultPayCurrency != null)
                    {
                        NewPayment.Pay_Currency_ID = resultPayCurrency.Currency_Type;
                    }
                    // create selectList from Database to entity Company.
                    var resultPAyCampId = db.Companies.FirstOrDefault();
                    if (resultPAyCampId != null)
                    {
                        NewPayment.PayComp_ID = resultPAyCampId.Comp_Id;
                    }
                    // create selectList from Database to entity PayBu .
                    var resultPayBu = db.BussinesUnits.Find(PaymentVM.Pay_BU);
                    if (resultPayBu != null)
                    {
                        NewPayment.Pay_BU = resultPayBu.BU_Id;
                    }
                    // create selectList from Database to entity Suppliers .
                    var resultSupplier = db.Suppliers.Find(PaymentVM.Supplier_Id);
                    if (resultSupplier != null)
                    {
                        NewPayment.Supplier_Id = resultSupplier.Supp_Id;
                    }
                    // create selectList from Database to entity ChartOfAccounting.
                    var resultChartAccount = db.ChartOfAccountings.Find(PaymentVM.SelectedChartOfAccountId);
                    if (resultChartAccount != null)
                    {
                        NewPayment.PayBank_Account = resultChartAccount.Account_Id;
                    }

                    var invpayMaster = db.PurInvoiceMasters.Find(PaymentVM.SelectedPurInvoiceMaster_Id);                  
                    // If the invpayMaster is larger than the Payments
                    if (Math.Round(PaymentVM.Pay_Amount,2,MidpointRounding.AwayFromZero) <= Math.Round(invpayMaster.TotalAmountAfMat,2,MidpointRounding.AwayFromZero) )
                    {
                       
                      //  invpayMaster.TotalAmountAfMat = invpayMaster.TotalAmountAfMat - PaymentVM.Pay_Amount;
                        var newTotalAmount = invpayMaster.TotalAmountAfMat;
                    }
                    else
                    {
                        return Tuple.Create( "قيمة الايصال اكبر من قيمة الفاتورة ",0);
                    }

                    var lastSerial = db.Payments.OrderByDescending(c => c.Pay_ID).FirstOrDefault();
                    if (lastSerial == null)
                    {
                        NewPayment.Pay_Number = "PAY-001";
                    }
                    else
                    {
                        //using string substring method to get the number of the last inserted
                        NewPayment.Pay_Number = "PAY-" + (Convert.ToInt32(lastSerial.Pay_Number.Substring(4, lastSerial.Pay_Number.Length - 4)) + 1).ToString("D3");
                    }
                    var empId = db.Employees.Where(a => a.Reg_ID == UserId).FirstOrDefault();
                    if (empId != null)
                    {
                        NewPayment.Emp_Id = empId.Emp_Id;
                    }
                    else
                    {
                        NewPayment.Emp_Id = 1;
                    }
                    NewPayment.Pay_Amount = PaymentVM.Pay_Amount;
                    NewPayment.Exchange_Rate = PaymentVM.Exchange_Rate;
                    NewPayment.Rec_Description = PaymentVM.Rec_Description;

                    var comp = db.Companies.Find(PaymentVM.PayComp_ID);
                    if (comp != null)
                    {
                        var date = Convert.ToDateTime(PaymentVM.Pay_Date);
                        if (date < comp.StartFinancialDate)
                        {
                            NewPayment.Pay_Date = DateTime.Now;
                        }
                        else
                        {
                            NewPayment.Pay_Date = Convert.ToDateTime(PaymentVM.Pay_Date);
                        }
                    }
                    else
                    {
                        NewPayment.Pay_Date = DateTime.Now;
                    }

            
                    NewPayment.Pay_State = TransactionState.Pending;
                    NewPayment.TotalAmountAfMat = PaymentVM.Pay_Amount;
                    NewPayment.RecCheck_Number = PaymentVM.RecCheck_Number;
                    NewPayment.PurchaseInv_ID = PaymentVM.SelectedPurInvoiceMaster_Id;
                    // Used this Load File Database.
                    if (PaymentVM.Files.FirstOrDefault() != null)
                    {
                        foreach (var file in PaymentVM.Files)
                        {
                            var filedetails = new PaymentFileDetail();
                            var data = new byte[file.ContentLength];
                            filedetails.FileName = file.FileName;
                            filedetails.ContentLength = file.ContentLength;
                            file.InputStream.Read(data, 0, file.ContentLength);
                            filedetails.ProImg = data;
                            filedetails.PaymentPay_ID = NewPayment.Pay_ID;
                           
                            if (data.Length < 1000000)
                            {
                                NewPayment.PaymentFileDetails.Add(filedetails);
                            }
                        }
                    }
                    var traBatch = new TransactionBatch();

                    traBatch.OperationName = "سند صرف مورد";
                    traBatch.Description = PaymentVM.Rec_Description;
                    traBatch.Date = Convert.ToDateTime(PaymentVM.Pay_Date);
                    traBatch.UserId = UserId;
                   
                    db.Payments.Add(NewPayment);
                    db.SaveChanges();
                    traBatch.OperationId = NewPayment.Pay_ID;
                    traBatch.Operation_Code = NewPayment.Pay_Number;
                    TransactionBatch.AddNewTransaction(traBatch);
                    db.Dispose();
                    return Tuple.Create( Resources.validate.CreateSucc,NewPayment.Pay_ID);
                }
            }
            catch (Exception e)
            {
                var masg = e.Message;
                var result = e.Data;

                throw;
            }

            return Tuple.Create(Resources.validate.Error, 0);
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
            var list = db.PurInvoiceMasters.Where(f => f.Supplier_Id == suupId && f.TotalAmountAfMat > 0 && f.Invoice_State == TransactionState.Post).ToList();
            List<PurInvoiceMaster> masterList = new List<PurInvoiceMaster> { };

            foreach (var invoice in list)
            {

                var reverse = db.RecalPurchInvoiceMasters.Where(a => a.PurInvoiceMasterMaster_Id== invoice.Master_Id && a.PI_State == TransactionState.Post).ToList();
                if (reverse.FirstOrDefault() == null)
                {
                    masterList.Add(invoice);
                }
                else
                {
                    decimal TotalReferse = 0;
                    foreach (var reverseInvoice in reverse)
                    {
                        TotalReferse += reverseInvoice.Inv_TotalCost;

                    }
                    if (TotalReferse < invoice.Total_Cost)
                    {
                        masterList.Add(invoice);

                    }

                }

            }

            IEnumerable <SelectListItem> PayInvoice = new SelectList(masterList, "Master_Id", "Serial_Number");
            return PayInvoice;
        }
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
            if(InvoiceDetails.TotaL_After_Fixed!=0&& InvoiceDetails.TotaL_After_Fixed != null)
            {
                invoiceDetailsVM.Total_Cost = InvoiceDetails.TotaL_After_Fixed.Value;
            }
            else
            {
                invoiceDetailsVM.Total_Cost = InvoiceDetails.Total_Cost;
            }
            
            invoiceDetailsVM.Supplier_Id = InvoiceDetails.Supplier_Id;
            if (InvoiceDetails.Store_ID != 0 && InvoiceDetails.Store_ID != null)
            {
                invoiceDetailsVM.Store_ID = InvoiceDetails.Store_ID.Value;
            }
            else
            {
                invoiceDetailsVM.Store_ID = 0;
            }
          
            invoiceDetailsVM.Company_Id = InvoiceDetails.Company_Id;
            invoiceDetailsVM.TotalAmountAfMat = InvoiceDetails.TotalAmountAfMat;
            invoiceDetailsVM.RecallList = new List<RecalPurchInvoiceMaster>();
            var ReverseList = db.RecalPurchInvoiceMasters.Where(a => a.PurInvoiceMasterMaster_Id == invoiceId).ToList();
            if (ReverseList.Count() > 0)
            {
                invoiceDetailsVM.TotalAmountAfMat = (InvoiceDetails.TotalAmountAfMat) - ReverseList.Select(a => a.Inv_TotalCost).Sum();
                invoiceDetailsVM.RecallList = ReverseList;
            }

            return invoiceDetailsVM;
        }
        /// <summary>
        /// get All Recall of chosen invoice by its id
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns>PaymentPurchaseRecallVM</returns>
        public static PaymentPurchaseRecallVM getAllRecalls(int invoiceId)
        {
            var db = new AccountingContainer();
            var thismaster = db.RecalPurchInvoiceMasters.Where(r => r.PurInvoiceMasterMaster_Id == invoiceId).ToList();
            if (thismaster.Count != 0)
            {
                PaymentPurchaseRecallVM PPRV = new PaymentPurchaseRecallVM();
                RecalPurchInvoiceDetailsVM RVM = new RecalPurchInvoiceDetailsVM();
                List<decimal> Quantity = new List<decimal>();
                List<string> code = new List<string>();
                List<decimal> totalcost = new List<decimal>();
                foreach (var recall in thismaster)
                {
                    decimal quant = 0;
                    foreach (var detail in recall.RecalPurchInvoiceDetails)
                    {
                        quant += detail.Product_Quantity;
                    }
                    Quantity.Add(quant);
                    code.Add(recall.Recall_MPurch_Serial);
                    totalcost.Add(recall.Inv_TotalCost);
                }
                PPRV.RecallQuantity = Quantity;
                PPRV.Recall_Code = code;
                PPRV.RecallTotalCost = totalcost;
                return PPRV;
            }
            else
            {
                return null;
            }
        }
        // use this method to load Details of Data for Payment.
        public static PaymentVM FillViewModelForPayment(int payId)
        {
            var db = new AccountingContainer();
            var PaymentVM = new PaymentVM();
            var payment = db.Payments.Find(payId);

            PaymentVM.myPayCurrency = new SelectList(db.Currencies, "Currency_Id ", "Currency_Type");
            PaymentVM.MyCompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            PaymentVM.myPayBu = new SelectList(db.BussinesUnits, "BU_Id", "BU_Name");
            PaymentVM.mySupplierId = new SelectList(db.Suppliers.Where(a=>a.Supplier_state!=Customer_State.غير_نشط), "Supp_Id", "Supp_Name");
            PaymentVM.Pay_Date = DateTime.Now.ToString("dd-MM-yyyy   HH:mm:ss");
            PaymentVM.MyPurInvoiceMaster = new SelectList(db.PurInvoiceMasters.Where(o => o.TotalAmountAfMat > 0 &&o.Supplier_Id==payment.Supplier_Id), "Master_Id", " Serial_Number");
            return PaymentVM;
        }
        // use this method For Edite Payment and save it DataBase.
        /// <summary>
        /// Edit Receipt Used payment PaymentVM.
        /// </summary>
        /// <param name="payVM"></param>
        /// <returns>bool</returns>
        public static String UpdatePayment(PaymentVM payVM)
        {
            var msg = "";
            var db = new AccountingContainer();
            var payment = db.Payments.Find(payVM.Pay_ID);
            OperationRun.Addnewoperation(payment.Pay_Number);
            var operation = db.OperationRuns.Where(a => a.Code == payment.Pay_Number).Count();
            if (operation != 1)
            {
                OperationRun.DeleteOperation(payment.Pay_Number);
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.PleaseWait;
            }
            if (payment.Pay_State == payVM.Pay_State)
            {
                OperationRun.DeleteOperation(payment.Pay_Number);
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.hasbeendoneby + " " + payment.Pay_State + " " + Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.previously;
            }
            else
            {
                // Used this State Spacific State Gl start Pending 
                payment.Pay_State = payVM.Pay_State;
                db.Entry(payment).State = EntityState.Modified;
                GLMaster glHeader = new GLMaster();
                // Used this GLHeader Create GL
                glHeader.GL_Date = Convert.ToDateTime(payment.Pay_Date);
                if (payment.Rec_Description == null)
                {
                    glHeader.GL_Desc = "سند صرف مورد";
                }
                else
                {
                    glHeader.GL_Desc = payment.Rec_Description;
                }

                glHeader.Company_Id = payment.PayComp_ID;
                glHeader.GLType_Id = " صرف مورد";
                glHeader.Currency_Id = payment.Pay_Currency_ID;
                glHeader.GLCode = payment.Pay_Number;
                glHeader.Auto = true;
                glHeader.GLState = TransactionState.Pending;



                // this GL Details Start advansedrec Secound BankGL
                GLDetail BankGL = new GLDetail(),
                    advansedPayment = new GLDetail();


                BankGL.Description_Detail = "القيمة الدائنة";
                BankGL.Bu_Id = 0;
                BankGL.Asset_Id = null;

                // Used ChartOfAccountRecive  Spacefic Account this Bank or treasury or tax. 
                var ChartOfAccounPay = db.ChartOfAccountings.Find(payment.PayBank_Account);
                BankGL.Account_Id = ChartOfAccounPay.Account_Id;
                BankGL.Account_Name = ChartOfAccounPay.Account_Name;
                BankGL.GLMasterGLMaster_Id = glHeader.GLMaster_Id;


                advansedPayment.Description_Detail = "القيمة المدينة";

                advansedPayment.Bu_Id = 0;
                advansedPayment.Asset_Id = null;

                // Used ChartOfAccountRecive  Spacefic Account this advansedPayment . 
                var ChartOfAccounadvansedrec = db.ChartOfAccountings.Find(45);
                advansedPayment.Account_Id = ChartOfAccounadvansedrec.Account_Id;
                advansedPayment.Account_Name = ChartOfAccounadvansedrec.Account_Name;
                var supCode = db.Suppliers.Find(payment.Supplier_Id).Supp_Code;
                advansedPayment.Account_Child_ID = db.Accounts.Where(a => a.Account_Code == supCode).FirstOrDefault().Accounts_Id;
                advansedPayment.Account_CHild_Name = db.Accounts.Where(a => a.Account_Code == supCode).FirstOrDefault().Account_Name;
                advansedPayment.GLMasterGLMaster_Id = glHeader.GLMaster_Id;

                //var sup = db.Suppliers.Find(payment.Supplier_Id);
                //SubLedger supplSubledger = db.SubLedgers.Where(subled => subled.SubLed_Code == sup.Supp_Code).FirstOrDefault();
                //advansedPayment.SubLedger_Id = supplSubledger.SubLed_Id;
                //BankGL.SubLedger_Id = 0;

                //var supplierr = db.Suppliers.Find(payment.Supplier_Id);
                //Accounts Supp = db.Accounts.Where(S => S.Account_Code == supplierr.Supp_Code).FirstOrDefault();
                //advansedPayment.Account_Child_ID = Supp.Accounts_Id;
                //advansedPayment.Account_CHild_Name = Supp.Account_Name;
                //BankGL.Account_Child_ID = 0;

                // This State GL Start Pending Secound Post thierd Voiod 
                var thisPurchase = db.PurInvoiceMasters.Find(payment.PurchaseInv_ID);
                if (thisPurchase != null)
                {
                    if (thisPurchase.Payed_Amount == null)
                    {
                        thisPurchase.Payed_Amount = 0;
                    }
                }
                switch (payment.Pay_State)
                {
                    //Post GL
                    case TransactionState.Post:

                        // From Account advansedPayment to BankGL .
                        BankGL.GLDetail_Credit = payment.Pay_Amount;
                        advansedPayment.GLDetail_Debit = payment.Pay_Amount;
                        thisPurchase.Payed_Amount = (decimal)thisPurchase.Payed_Amount + payment.Pay_Amount;
                        thisPurchase.TotalAmountAfMat = thisPurchase.TotalAmountAfMat - payment.Pay_Amount;




                        break;
                    // Void Invoice
                    case TransactionState.Void:
                        glHeader.GLType_Id = " صرف مورد عكسي";
                        if (payment.HasOff == true)

                        {
                            msg = Resources.ReceiptCreate.UnderCloseing;
                            payment.Pay_State = TransactionState.Post;
                            db.SaveChanges();

                        }
                        else
                        {
                            // From Account BankGL to advansedrec.
                            advansedPayment.GLDetail_Credit = payment.Pay_Amount;
                            BankGL.GLDetail_Debit = payment.Pay_Amount;
                            thisPurchase.Payed_Amount = (decimal)thisPurchase.Payed_Amount - payment.Pay_Amount;
                            thisPurchase.TotalAmountAfMat = Math.Round(thisPurchase.TotalAmountAfMat, 2, MidpointRounding.AwayFromZero) + Math.Round(payment.Pay_Amount, 2, MidpointRounding.AwayFromZero);


                        }
                        break;
                    default:
                        break;
                }

                if (msg == "")
                {
                    glHeader.GLDetails.Add(BankGL);
                    glHeader.GLDetails.Add(advansedPayment);

                    if (Math.Round(thisPurchase.Payed_Amount.Value, 2, MidpointRounding.AwayFromZero) > Math.Round(thisPurchase.Total_Cost, 2, MidpointRounding.AwayFromZero))
                    {
                        OperationRun.DeleteOperation(payment.Pay_Number);
                        return Resources.ReceiptCreate.TotalPayGreaterthanInvoiceValue;
                    }
                    else
                    {
                        db.Entry(thisPurchase).State = EntityState.Modified;
                        db.GLMasters.Add(glHeader);
                        GLMaster.GetBalanceForAccount(glHeader);
                        GLMaster.GetBalanceForSubLedger(glHeader);

                        msg = Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                        db.SaveChanges();
                        db.Dispose();
                    }

                }
                OperationRun.DeleteOperation(payment.Pay_Number);
                if (payment.Pay_State == TransactionState.Post)
                {
                    msg = Resources.validate.GLDone;
                }
                else
                {
                    msg = Resources.validate.GLReverse;
                }
                return msg;
            }
        }
        /// <summary>
        /// fill for making Edit 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static PaymentVM FilltheEditPayment(int id)
        {
            var db = new AccountingContainer();
            var payment = db.Payments.Find(id);
            var CurrencyId = payment.Pay_Currency_ID;
            var CompanyId = payment.PayComp_ID;
            var SuppId = payment.Supplier_Id;
            var BusId = payment.Pay_BU;
            var ChartOfAcc = payment.PayBank_Account;

            //var chartId = payment.Pay

            if (payment == null)
            {
                return null;
            }

            PaymentVM paymVM = new PaymentVM();
            paymVM.SelectedPurInvoiceMaster_Id = payment.PurchaseInv_ID;
            paymVM.Pay_ID = payment.Pay_ID;
            paymVM.Pay_Number = payment.Pay_Number;
            paymVM.Pay_State = payment.Pay_State;          
            paymVM.Pay_Date = (payment.Pay_Date).ToString("dd-MM-yyyy   HH:mm:ss");    
            paymVM.Pay_Amount = payment.Pay_Amount;
            paymVM.Pay_BU = payment.Pay_BU;          
            paymVM.PayGl_Date = payment.PayGl_Date.ToString();
            paymVM.PayComp_ID = payment.PayComp_ID;
            paymVM.Exchange_Rate = payment.Exchange_Rate;
            paymVM.Rec_Description = payment.Rec_Description;
            paymVM.RecCheck_Number = payment.RecCheck_Number;
            paymVM.SelectedChartOfAccountId = payment.PayBank_Account;
            var Currency = db.Currencies.Where(c => c.Currency_Type == CurrencyId);
            if (Currency.Any())
            {
                paymVM.Pay_Currency_ID = Currency.First().Currency_Id;
            }

            var company = db.Companies.Where(c => c.Comp_Id == CompanyId);
            if (company.Any())
            {
                paymVM.PayComp_ID = company.First().Comp_Id;
            }

            var suppId = db.Suppliers.Where(c => c.Supp_Id == SuppId);
            if (suppId.Any())
            {
                paymVM.Supplier_Id = suppId.First().Supp_Id;
            }


            var ChartOfAccount = db.ChartOfAccountings.Where(c => c.Account_Id == ChartOfAcc);
            if (ChartOfAccount.Any())
            {
                paymVM.PayBank_Account = ChartOfAccount.First().Account_Id;
            }


            var BusinessUN = db.BussinesUnits.Where(c => c.BU_Id == BusId);
            if (BusinessUN.Any())
            {
                paymVM.Supplier_Id = BusinessUN.First().BU_Id;
            }

            paymVM.MyPurInvoiceMaster = new SelectList(db.PurInvoiceMasters.Where(o => o.TotalAmountAfMat > 0 && o.Supplier_Id == payment.Supplier_Id), "Master_Id", " Serial_Number");
            paymVM.MyCompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            paymVM.myPayCurrency = new SelectList(db.Currencies, "Currency_Id ", "Currency_Type");
            paymVM.mySupplierId = new SelectList(db.Suppliers, "Supp_Id", "Supp_Name");
            paymVM.myPayBu = new SelectList(db.BussinesUnits, "BU_Id", "BU_Name");
            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0).ToList();
            paymVM.MyChartOfAccountList = new SelectList(lista.Where(accCode => accCode.Account_Code.StartsWith("0101200010") || accCode.Account_Code.StartsWith("010120002")), "Account_Id", "Account_Name");

            return paymVM;
        }
        public static int EditPayment(PaymentVM paymVM , String UserId)
        {

            var db = new AccountingContainer();
            var payment = db.Payments.Find(paymVM.Pay_ID);
            try
            {
                if (paymVM != null)
                {
                    var resultPayCurrency = db.Currencies.Find(paymVM.Pay_Currency_ID);
                    if (resultPayCurrency != null)
                    {
                        payment.Pay_Currency_ID = resultPayCurrency.Currency_Type;
                    }
                    // create selectList from Database to entity Company.
                    var resultPAyCampId = db.Companies.Find(paymVM.PayComp_ID);
                    if (resultPAyCampId != null)
                    {
                        payment.PayComp_ID = resultPAyCampId.Comp_Id;
                    }
                    // create selectList from Database to entity PayBu .
                    var resultPayBu = db.BussinesUnits.Find(paymVM.Pay_BU);
                    if (resultPayBu != null)
                    {
                        payment.Pay_BU = resultPayBu.BU_Id;
                    }
                    // create selectList from Database to entity Suppliers .
                    var resultSupplier = db.Suppliers.Find(paymVM.Supplier_Id);
                    if (resultSupplier != null)
                    {
                        payment.Supplier_Id = resultSupplier.Supp_Id;
                    }
                    // create selectList from Database to entity ChartOfAccounting.
                    var resultChartAccount = db.ChartOfAccountings.Find(paymVM.SelectedChartOfAccountId);
                    if (resultChartAccount != null)
                    {
                        payment.PayBank_Account = resultChartAccount.Account_Id;
                    }
                    payment.PurchaseInv_ID = paymVM.SelectedPurInvoiceMaster_Id;
                    payment.Pay_Number = paymVM.Pay_Number;
                    payment.Pay_Amount = paymVM.Pay_Amount;
                    payment.Exchange_Rate = paymVM.Exchange_Rate;
                    payment.Rec_Description = paymVM.Rec_Description;
                    payment.PayGl_Date = DateTime.Now;
                    payment.Pay_Date = Convert.ToDateTime(paymVM.Pay_Date);
                    payment.Pay_State = TransactionState.Pending;
                    payment.TotalAmountAfMat = paymVM.Pay_Amount;
                    payment.RecCheck_Number = paymVM.RecCheck_Number;

                    if (paymVM.Files.FirstOrDefault() != null)
                    {
                        foreach (var file in paymVM.Files)
                        {
                            var filedetails = new PaymentFileDetail();
                            var data = new byte[file.ContentLength];
                            filedetails.FileName = file.FileName;
                            filedetails.ContentLength = file.ContentLength;
                            file.InputStream.Read(data, 0, file.ContentLength);
                            filedetails.ProImg = data;
                            filedetails.PaymentPay_ID = payment.Pay_ID;
                            if (data.Length < 1000000)
                            {
                                payment.PaymentFileDetails.Add(filedetails);
                            }
                           
                        }
                    }

                    // this is Batch number any transaction Payment. 
                    var traBatch = new TransactionBatch();

                    traBatch.OperationName = "تعديل سند صرف مورد";
                    traBatch.Description = null;
                    traBatch.Date = Convert.ToDateTime(paymVM.Pay_Date);
                    traBatch.UserId = UserId;
                    traBatch.Operation_Code = paymVM.Pay_Number;
                    db.Entry(payment).State = EntityState.Modified;
                 
                    traBatch.OperationId = payment.Pay_ID;
                    TransactionBatch.AddNewTransaction(traBatch);
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
                    db.Dispose();
                    return payment.Pay_ID;
                }
            }
            catch (Exception e)
            {
                var masg = e.Message;
                var result = e.Data;

                throw;
            }

            return 0;
        }
        // use this method to Delete payment and save DataBase.
        public static string Deletepayment(int pmid)
        {
            var db = new AccountingContainer();
            var MyPay = db.Payments.Find(pmid);
            if (MyPay != null)
            {
                db.PaymentFileDetails.RemoveRange(MyPay.PaymentFileDetails);
                db.Payments.Remove(MyPay);
               
                db.SaveChanges();
                db.Dispose();
                return Resources.validate.DeleteSuccessful;

            }
            return Resources.validate.Error;
        }
        /// <summary>
        /// make List For dropdownList for Supplier
        /// </summary>
        /// <returns>PaymentVM</returns>
        public static PaymentVM GetSupplierList()
        {
            var db = new AccountingContainer();
            PaymentVM vm = new PaymentVM();

            vm.mySupplierId = new SelectList(db.Suppliers, "Supp_Id", "Supp_Name");

            return vm;
        }
        /// <summary>
        /// get All Invoices for that supplier
        /// </summary>
        /// <param name="supid"></param>
        /// <returns>IEnumerable<SelectListItem></returns>
        public static IEnumerable<SelectListItem> GetPurchaseInvoice(int supid)
        {
            var db = new AccountingContainer();
            var thischar = db.Suppliers.Find(supid);
            var listchild = db.PurInvoiceMasters.Where(p => p.Supplier_Id == thischar.Supp_Id).ToList();
            IEnumerable<SelectListItem> AccountLedger = new SelectList(listchild, "Master_Id", "Serial_Number");
            return AccountLedger;
        }
        public static String deletionactiveGL(int id)
        {
            var db = new AccountingContainer();
            var doc = db.Payments.Find(id);

            if (doc.Pay_State != TransactionState.Pending && doc.HasOff != true)
            {
                var vm = new PaymentVM();
                vm.Pay_State = TransactionState.Void;
                vm.Pay_ID = doc.Pay_ID;
                UpdatePayment(vm);


                var gl = db.GLMasters.Where(a => a.GLCode == doc.Pay_Number);
                foreach (var g in gl)
                {
                    db.GLDetails.RemoveRange(g.GLDetails);

                    db.GLMasters.Remove(g);

                }
                db.PaymentFileDetails.RemoveRange(doc.PaymentFileDetails);
                db.Payments.Remove(doc);
                db.SaveChanges();



                db.Dispose();
                return Resources.validate.DeleteSuccessful;
            }
            return Resources.validate.Error;
        }
    }
}