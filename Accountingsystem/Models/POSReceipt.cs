using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Models
{

    public partial class POSReceipt
    {
        /// <summary>
        /// make arecipt unique code for user
        /// </summary>
        /// <param name="Serial"></param>
        /// <returns>bool</returns>
        public static bool containCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.POSReceipts.ToList();
            foreach (var item in allinData)
            {
                if (item.Rec_Number == Serial || !Serial.StartsWith("RPS-"))
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
        // Used this Method to Get List of Receipt in this model from data base.
        public static List<POSReceipt> GetAllReceipt(int? all,string userId)
        {

            var db = new AccountingContainer();
            var receipts = new List<POSReceipt>();
            try
            {
                var emp = db.Employees.Where(a => a.Reg_ID == userId).FirstOrDefault();
                if (emp != null && emp.SystemRoleId != 6)
                {
                    var pos = db.PointOfSales.Where(a => a.Employee_Id == emp.Emp_Id).FirstOrDefault();
                    receipts = pos.MasterPOSInvoices.SelectMany(a => a.POSReceipts).ToList();
                }
                else
                {
                    receipts = db.POSReceipts.ToList();
                }
               
            }
            catch
            {

                receipts = db.POSReceipts.ToList();

            }
            if (all == null)
            {
                receipts.Reverse();
                receipts = receipts.Take(100).ToList();
            }
            return receipts;
        }
        // used this Method to Get one Element ReceiptById in this model ferom data base.
        /// <summary>
        /// Used Method Get ReceiptById
        /// </summary>
        /// <param name="id"></param>
        /// <returns>receipt</returns>
        public static POSReceipt FindReceiptByID(int id)
        {
            var db = new AccountingContainer();
            var receipt = db.POSReceipts.Find(id);
            return receipt;
        }
        /// <summary>
        /// get file by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ReceiptFileDetail</returns>
        public static ReceiptPOSFileDetail FindFileByID(int id)
        {
            var db = new AccountingContainer();
            var file = db.ReceiptPOSFileDetails.Find(id);
            return file;
        }
        public static string deleteFile(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.ReceiptPOSFileDetails.Find(fileId);
            db.ReceiptPOSFileDetails.Remove(file);
            db.SaveChanges();
            return "تم حذف الملف ";
        }
        /// <summary>
        /// get all Files by the id of Recipts
        /// </summary>
        /// <param name="receId"></param>
        /// <returns>List<ReceiptFileDetail> </returns>
        public static List<ReceiptPOSFileDetail> GetAllFiles(int receId)
        {
            var db = new AccountingContainer();
            var files = db.ReceiptPOSFileDetails.Where(emp => emp.ReceiptRec_ID == receId).ToList();
            return files;
        }
        // used Method Edit to load details of data  for Receipt
        /// <summary>
        /// Edit Receipt used id 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>ReceiptVM</returns>
        public static POSReceiptVM FillReseipt(int Id)
        {
            var db = new AccountingContainer();
            var MyResipt = db.Receipts.Find(Id);

            POSReceiptVM receipt = new POSReceiptVM();
            receipt.MyCurrencyList = new SelectList(db.Currencies, "Currency_ID ", "Currency_Type");
            receipt.MyCustomerList = new SelectList(db.Customers.Where(a=>a.Customer_State!=Customer_State.غير_نشط), "Rec_ID", "Rec_Number");


            return receipt;
        }
        /// <summary>
        /// get customer balnace by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>decimal</returns>
        public static decimal BalanceOFCustomer(int id)
        {
            var db = new AccountingContainer();
            var listofInvoices = db.MasterPOSInvoices.Where(e => e.Customer_Id == id).ToList();
            decimal totalamount = 0;
            if (listofInvoices.Count == 0)
            {
                return totalamount = 0;
            }

            else
            {
                foreach (var detail in listofInvoices)
                {
                    totalamount += detail.Total_Amount.Value;
                }
                return totalamount;
            }


        }
        // Used Method for Creation new Receipt and get data from user
        public static POSReceiptVM CreateVMForCreation(int? cust_Id, int? Invoice_Id, string userid)
        {
            try
            {
                var db = new AccountingContainer();
                POSReceiptVM rec = new POSReceiptVM();

                var emp = db.Employees.Where(a => a.Reg_ID == userid).FirstOrDefault();
                var pos = db.PointOfSales.Where(a => a.Employee_Id == emp.Emp_Id).FirstOrDefault();
                if (emp == null || pos == null)
                {
                    return null;
                }
                if (emp.ViewAllCustomer != true)
                {

                    var newListo = (from cust in db.Customers.Where(a => a.Customer_State == Customer_State.نشط && (a.Employee_Id == emp.Emp_Id || a.Employee_Id == null))
                                    select new
                                    {
                                        Id = cust.Customer_Id,
                                        Name = cust.Customer_Name + "        " + cust.Customer_Phone

                                    }).ToList();
                    rec.MyCustomerList = new SelectList(newListo, "Id", "Name");
                }
                else
                {
                    var newListo = (from cust in db.Customers.Where(a => a.Customer_State == Customer_State.نشط)
                                    select new
                                    {
                                        Id = cust.Customer_Id,
                                        Name = cust.Customer_Name + "        " + cust.Customer_Phone

                                    }).ToList();
                    rec.MyCustomerList = new SelectList(newListo, "Id", "Name");
                }



                if (cust_Id != null)
                {
                    rec.SelectedCustomer = Convert.ToInt32(cust_Id);
                }


                var invoiceLists = db.MasterPOSInvoices.Where(o => o.Total_Amount > 0
                    && o.Invoice_State == TransactionState.Post
                    && o.PosPaymentMethods.Count() == 0
                    && o.POSReceipts.Select(a => a.Rec_Amount).Sum() < o.Total_Amount);
                if (cust_Id != 0 && cust_Id != null)
                {
                    if (Invoice_Id != 0 && Invoice_Id != null)
                    {
                        invoiceLists = invoiceLists.Where(a => a.MasterPOSInvoiceId == Invoice_Id);
                        rec.InvoiceList = new SelectList(invoiceLists, "MasterPOSInvoiceId", "Serial_Number");
                        rec.Rec_Amount = (invoiceLists.FirstOrDefault().Total_Amount.Value + invoiceLists.FirstOrDefault().TotalNewTaxs.Value) - invoiceLists.FirstOrDefault().POSReceipts.Select(z => z.Rec_Amount).Sum();
                        rec.selectInvoiceID = invoiceLists.FirstOrDefault().MasterPOSInvoiceId;
                    }
                    else
                    {
                        invoiceLists = invoiceLists.Where(a => a.Customer_Id == cust_Id);
                        rec.InvoiceList = new SelectList(invoiceLists, "MasterPOSInvoiceId", "Serial_Number");
                    }
                }
                else
                {
                    var a = new List<string> { };
                    rec.InvoiceList = new SelectList(a, "MasterPOSInvoiceId", "Serial_Number");
                }

                var lastSerial = db.POSReceipts.OrderByDescending(c => c.Rec_ID).FirstOrDefault();
                if (lastSerial == null)
                {
                    rec.Rec_Number = "RPS-001";
                }
                else
                {
                    //using string substring method to get the number of the last inserted
                    rec.Rec_Number = "RPS-" + (Convert.ToInt32(lastSerial.Rec_Number.Substring(4, lastSerial.Rec_Number.Length - 4)) + 1).ToString("D3");
                }
                rec.Rec_Date = DateTime.Now.ToString("dd-MM-yyyy   HH:mm:ss");

                var newList = (from pay in pos.POSDetailPayments
                               select new
                               {
                                   Id = db.PaymentMethods.Find(pay.PaymentMethod_Id).PaymentMethod_Id,
                                   Name = db.PaymentMethods.Find(pay.PaymentMethod_Id).Payment_Type

                               });
                rec.MyChartOfAccountList = new SelectList(newList, "Id", "Name");


                return rec;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // Used this Method create New Receipt and save it in database
        // after spacific data  another Tabel put this data in the model 
        // and add data this tabel  put tabel Receipt
        /// <summary>
        /// Used Receipt Bvm Create in database
        /// </summary>
        /// <param name="bvm"></param>
        /// <returns>bool</returns>
        public static int AddNewReceipt(POSReceiptVM bvm, String UserId)
        {
            var db = new AccountingContainer();
            var NeweReceipt = new POSReceipt();

            if (bvm != null)

            {
                // create selectList from Database to entity Currency.
                var resultCurrency = db.Currencies.Find(bvm.SelectedCurrencyId);
                if (resultCurrency != null)
                {
                    NeweReceipt.RecCurrency_ID = resultCurrency.Currency_Type;
                }
                // create selectList from Database to entity Customer.
                var resultCustomer = db.Customers.Find(bvm.SelectedCustomer);
                if (resultCustomer != null)
                {
                    NeweReceipt.Customer_Id = resultCustomer.Customer_Id;
                }
                // create selectList from Database to entity Company.
                var resultCompany = db.Companies.Find(bvm.RecComp_ID);
                if (resultCompany != null)
                {
                    NeweReceipt.RecComp_ID = resultCompany.Comp_Id;
                }


                NeweReceipt.Rec_BU = 0;

                // create selectList from Database to entity ChartOfAccounting.
                var resultChartAccount = db.PaymentMethods.Find(bvm.SelectedChartOfAccountId);
                if (resultChartAccount != null)
                {
                    NeweReceipt.RecBank_Account = resultChartAccount.PaymentMethod_Id;
                }

                var resultBreanch = db.Branches.Find(bvm.SelectedBranch_Id);
                if (resultBreanch != null)
                {
                    NeweReceipt.Branch_Id = resultBreanch.Branch_Id;
                }
                var invpayMaster = db.MasterPOSInvoices.Find(bvm.selectInvoiceID);
                // If the invpayMaster is larger than the Payments
                //if (Math.Round(bvm.Rec_Amount+ invpayMaster.POSReceipts.Select(a=>a.Rec_Amount).Sum(), 2, MidpointRounding.AwayFromZero) <= invpayMaster.Total_Amount))
                //{

                //    //  invpayMaster.TotalAmountAfMat = invpayMaster.TotalAmountAfMat - PaymentVM.Pay_Amount;
                   
                //}
                //else
                //{
                //    return 0;
                //}


                var lastSerial = db.POSReceipts.OrderByDescending(c => c.Rec_ID).FirstOrDefault();
                if (lastSerial == null)
                {
                    NeweReceipt.Rec_Number = "RPS-001";
                }
                else
                {
                    //using string substring method to get the number of the last inserted
                    NeweReceipt.Rec_Number = "RPS-" + (Convert.ToInt32(lastSerial.Rec_Number.Substring(4, lastSerial.Rec_Number.Length - 4)) + 1).ToString("D3");
                }
                var empId = db.Employees.Where(a => a.Reg_ID == UserId).FirstOrDefault();
                if (empId != null)
                {
                    NeweReceipt.Emp_Id = empId.Emp_Id;
                }
                else
                {
                    NeweReceipt.Emp_Id = 1;
                }
                NeweReceipt.Emp_Id = 1;
                NeweReceipt.Rec_Amount = bvm.Rec_Amount;
                NeweReceipt.Rec_state = TransactionState.Pending;


                var comp = db.Companies.FirstOrDefault();
                if (comp != null)
                {
                    var date = Convert.ToDateTime(bvm.Rec_Date);
                    if (date < comp.StartFinancialDate)
                    {
                        NeweReceipt.Rec_Date = DateTime.Now;
                    }
                    else
                    {
                        NeweReceipt.Rec_Date = Convert.ToDateTime(bvm.Rec_Date);
                    }
                }
                else
                {
                    NeweReceipt.Rec_Date = DateTime.Now;
                }
                NeweReceipt.Rec_Description = bvm.Rec_Description;

                NeweReceipt.TotalAmountAfMat = bvm.Rec_Amount;
                NeweReceipt.MasterPOSInvoiceMasterPOSInvoiceId = bvm.selectInvoiceID;
                // Used this Load File Database.
                if (bvm.Files.FirstOrDefault() != null)
                {
                    foreach (var file in bvm.Files)
                    {
                        var filedetails = new ReceiptPOSFileDetail();
                        var data = new byte[file.ContentLength];
                        filedetails.FileName = file.FileName;
                        filedetails.ContentLength = file.ContentLength;
                        file.InputStream.Read(data, 0, file.ContentLength);
                        filedetails.ProImg = data;
                        filedetails.ReceiptRec_ID = NeweReceipt.Rec_ID;
                        if (data.Length < 1000000)
                        {
                            NeweReceipt.ReceiptFileDetails.Add(filedetails);
                        }

                    }
                }
                var traBatch = new TransactionBatch();

                traBatch.OperationName = "سند قبض عميل";
                traBatch.Description = bvm.Rec_Description;
                traBatch.Date = Convert.ToDateTime(bvm.Rec_Date);
                traBatch.UserId = UserId;

                db.POSReceipts.Add(NeweReceipt);
                db.SaveChanges();
                traBatch.OperationId = NeweReceipt.Rec_ID;
                traBatch.Operation_Code = NeweReceipt.Rec_Number;
                TransactionBatch.AddNewTransaction(traBatch);
                db.Dispose();
                return NeweReceipt.Rec_ID;
            }


            return 0;
        }


        // use this method to Delete receipt and save DataBase.
        public static string DeleteReceipt(int id)
        {
            var db = new AccountingContainer();
            var receipt = db.POSReceipts.Find(id);
            if (receipt.Rec_state == TransactionState.Pending)
            {
                if (receipt != null)
                {
                    db.ReceiptPOSFileDetails.RemoveRange(receipt.ReceiptFileDetails);
                    db.POSReceipts.Remove(receipt);

                    db.SaveChanges();
                    db.Dispose();
                    return "تم الحذف بنجاح";

                }
            }
            else
            {
                return "لم يتم الحذف ";
            }
            return "لم يتم الحذف ";
        }
        /// <summary>
        /// fill for Editing Recipt 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ReceiptVM</returns>
        public static POSReceiptVM FillEditTheReceipt(int id,string userId)
        {
            try
            {
                var db = new AccountingContainer();
                var emp = db.Employees.Where(a => a.Reg_ID == userId).FirstOrDefault();
                var pos = db.PointOfSales.Where(a => a.Employee_Id == emp.Emp_Id).FirstOrDefault();
                var receipt = db.POSReceipts.Find(id);
                var CurrencyId = receipt.RecCurrency_ID;
                var CompanyId = receipt.RecComp_ID;
                var CustomerName = db.Customers.Find(receipt.Customer_Id).Customer_Name;
                var BusId = receipt.Rec_BU;
                var ChartOfAcc = receipt.RecBank_Account;
                var BranchId = receipt.Branch_Id;
                var PaymentId = receipt.PaymentMethod_Id;


                if (receipt == null)
                {
                    return null;
                }

                POSReceiptVM receiptVM = new POSReceiptVM();
                receiptVM.TotalAmountAfMat = receipt.TotalAmountAfMat;

                receiptVM.Rec_Number = receipt.Rec_Number;
                receiptVM.Rec_ID = receipt.Rec_ID;
                receiptVM.Rec_Description = receipt.Rec_Description;
                receiptVM.Rec_Date = (receipt.Rec_Date).ToString("dd-MM-yyyy   HH:mm:ss");
                receiptVM.Rec_Amount = receipt.Rec_Amount;
                receiptVM.selectInvoiceID = receipt.MasterPOSInvoiceMasterPOSInvoiceId;
                //receiptVM.RecBank_Account = receipt.RecBank_Account;


                var resultCurrency = db.Currencies.Where(c => c.Currency_Type == CurrencyId);
                if (resultCurrency.Any())
                {
                    receiptVM.SelectedCurrencyId = resultCurrency.First().Currency_Id;
                }

                // create selectList from Database to entity Customer.
                var resultCustomer = db.Customers.Where(c => c.Customer_Name == CustomerName);
                if (resultCustomer.Any())
                {
                    receiptVM.SelectedCustomer = resultCustomer.First().Customer_Id;
                }
                // create selectList from Database to entity Company.
                var company = db.Companies.Where(c => c.Comp_Id == CompanyId);
                if (company.Any())
                {
                    receiptVM.RecComp_ID = company.First().Comp_Id;
                }
                // create selectList from Database to entity PayBu .
                var BusinessUN = db.BussinesUnits.Where(c => c.BU_Id == BusId);
                if (BusinessUN.Any())
                {
                    receiptVM.Rec_BU = BusinessUN.First().BU_Id;
                }
                var Branch = db.Branches.Where(b => b.Branch_Id == BranchId);
                if (Branch.Any())
                {
                    receiptVM.SelectedBranch_Id = Branch.First().Branch_Id;
                }
                var PaymentMethod = db.PaymentMethods.Where(p => p.PaymentMethod_Id == PaymentId);
                if (PaymentMethod.Any())
                {
                    receiptVM.SelectPayment_Method = PaymentMethod.First().PaymentMethod_Id;
                }


                receiptVM.SelectedChartOfAccountId = receipt.RecBank_Account;


                receiptVM.MyCompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
                receiptVM.MyCurrencyList = new SelectList(db.Currencies, "Currency_Id ", "Currency_Type");
                if (emp.ViewAllCustomer != true)
                {

                    var newListo = (from cust in db.Customers.Where(a => a.Customer_State == Customer_State.نشط && (a.Employee_Id == emp.Emp_Id || a.Employee_Id == null))
                                    select new
                                    {
                                        Id = cust.Customer_Id,
                                        Name = cust.Customer_Name + "        " + cust.Customer_Phone

                                    }).ToList();
                    receiptVM.MyCustomerList = new SelectList(newListo, "Id", "Name");
                }
                else
                {
                    var newListo = (from cust in db.Customers.Where(a => a.Customer_State == Customer_State.نشط)
                                    select new
                                    {
                                        Id = cust.Customer_Id,
                                        Name = cust.Customer_Name + "        " + cust.Customer_Phone

                                    }).ToList();
                    receiptVM.MyCustomerList = new SelectList(newListo, "Id", "Name");
                }
                receiptVM.MySubLdgerList = new SelectList(db.BussinesUnits, "BU_Id", "BU_Name");
                receiptVM.BranchList = new SelectList(db.Branches, "Branch_Id", "Branch_Name");
                receiptVM.InvoiceList = new SelectList(db.MasterPOSInvoices.Where(f => f.Customer_Id == receiptVM.SelectedCustomer && f.Invoice_State == TransactionState.Post
               && (f.POSReceipts.Count() == 0 || f.Total_Amount > f.POSReceipts.Where(a => a.Rec_state == TransactionState.Post).Select(a => a.Rec_Amount).ToList().Sum())).ToList(), "MasterPOSInvoiceId", "Serial_Number");


                var newList = (from pay in pos.POSDetailPayments
                               select new
                               {
                                   Id = db.PaymentMethods.Find(pay.PaymentMethod_Id).PaymentMethod_Id,
                                   Name = db.PaymentMethods.Find(pay.PaymentMethod_Id).Payment_Type

                               });
                receiptVM.MyChartOfAccountList = new SelectList(newList, "Id", "Name");
                return receiptVM;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        /// <summary>
        /// Edit the Recipt and save change in db
        /// </summary>
        /// <param name="receVM"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static int EditReceipt(POSReceiptVM receVM, String UserId)
        {
            var db = new AccountingContainer();
            var rece = db.POSReceipts.Find(receVM.Rec_ID);
            var CurrencyId = rece.RecCurrency_ID;
            var CompanyId = rece.RecComp_ID;

            try
            {

                if (receVM != null)
                {

                    var resultCurrency = db.Currencies.Find(receVM.SelectedCurrencyId);
                    if (resultCurrency != null)
                    {
                        rece.RecCurrency_ID = resultCurrency.Currency_Type;
                    }
                    // create selectList from Database to entity Customer.
                    var resultCustomer = db.Customers.Find(receVM.SelectedCustomer);
                    if (resultCustomer != null)
                    {
                        rece.Customer_Id = resultCustomer.Customer_Id;
                    }
                    // create selectList from Database to entity Company.
                    var resultCompany = db.Companies.Find(receVM.RecComp_ID);
                    if (resultCompany != null)
                    {
                        rece.RecComp_ID = resultCompany.Comp_Id;
                    }
                    // create selectList from Database to entity PayBu .
                    var resultSub = db.BussinesUnits.Find(receVM.Rec_BU);
                    if (resultSub != null)
                    {

                        rece.Rec_BU = resultSub.BU_Id;

                    }
                    // create selectList from Database to entity ChartOfAccounting.
                    var resultChartAccount = db.ChartOfAccountings.Find(receVM.SelectedChartOfAccountId);
                    if (resultChartAccount != null)

                    {
                        rece.RecBank_Account = resultChartAccount.Account_Id;
                    }

                    var resultBreanch = db.Branches.Find(receVM.SelectedBranch_Id);
                    if (resultBreanch != null)
                    {
                        rece.Branch_Id = resultBreanch.Branch_Id;
                    }

                    var resultPaymentMethod = db.PaymentMethods.Find(receVM.SelectPayment_Method);
                    if (resultPaymentMethod != null)
                    {
                        rece.PaymentMethod_Id = resultPaymentMethod.PaymentMethod_Id;
                    }


                    rece.Rec_state = TransactionState.Pending; ;

                    rece.Rec_ID = receVM.Rec_ID;
                    rece.Rec_Description = receVM.Rec_Description;


                    var comp = db.Companies.FirstOrDefault();
                    if (comp != null)
                    {
                        var date = Convert.ToDateTime(receVM.Rec_Date);
                        if (date < comp.StartFinancialDate)
                        {
                            rece.Rec_Date = DateTime.Now;
                        }
                        else
                        {
                            rece.Rec_Date = Convert.ToDateTime(receVM.Rec_Date);
                        }
                    }
                    else
                    {
                        rece.Rec_Date = DateTime.Now;
                    }
                    rece.Rec_BU = receVM.Rec_BU;
                    rece.Rec_Amount = receVM.Rec_Amount;
                    rece.TotalAmountAfMat = receVM.Rec_Amount;
                    rece.RecComp_ID = receVM.RecComp_ID;
                    rece.MasterPOSInvoiceMasterPOSInvoiceId = receVM.selectInvoiceID;


                    if (receVM.Files.FirstOrDefault() != null)
                    {
                        foreach (var file in receVM.Files)
                        {
                            var filedetails = new ReceiptPOSFileDetail();
                            var data = new byte[file.ContentLength];
                            filedetails.FileName = file.FileName;
                            filedetails.ContentLength = file.ContentLength;
                            file.InputStream.Read(data, 0, file.ContentLength);
                            filedetails.ProImg = data;
                            filedetails.ReceiptRec_ID = rece.Rec_ID;
                            if (data.Length < 1000000)
                            {
                                rece.ReceiptFileDetails.Add(filedetails);
                            }

                        }
                    }
                    var traBatch = new TransactionBatch();

                    traBatch.OperationName = "تعديل سند قبض عميل";
                    traBatch.Description = null;
                    traBatch.Date = Convert.ToDateTime(receVM.Rec_Date);
                    traBatch.UserId = UserId;
                    rece.Rec_Number = receVM.Rec_Number;
                    db.Entry(rece).State = EntityState.Modified;
                    db.SaveChanges();
                    traBatch.OperationId = rece.Rec_ID;

                    //traBatch.Operation_Code = rece.Rec_Number;
                    //TransactionBatch.AddNewTransaction(traBatch);
                    db.Dispose();
                    return rece.Rec_ID;
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
        // use this method For Edite Receipt and save it DataBase.
        /// <summary>
        /// Edit Receipt Used receipt VM
        /// </summary>
        /// <param name="vM"></param>
        /// <returns>bool</returns>
        public static String UpdateRecipt(POSReceiptVM vM)
        {
            var msg = "";
            var db = new AccountingContainer();
            var Rece = db.POSReceipts.Find(vM.Rec_ID);
            OperationRun.Addnewoperation(Rece.Rec_Number);
            var operation = db.OperationRuns.Where(a => a.Code == Rece.Rec_Number).Count();
            if (operation != 1)
            {
                OperationRun.DeleteOperation(Rece.Rec_Number);
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.PleaseWait;
            }

            // Used this State Spacific State Gl start Pending 


            if (Rece.Rec_state == vM.Rec_state)
            {
                OperationRun.DeleteOperation(Rece.Rec_Number);
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.hasbeendoneby + " " + Rece.Rec_state + " " + Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.previously;
            }
            else
            {
                Rece.Rec_state = vM.Rec_state;
            }
            db.Entry(Rece).State = EntityState.Modified;
            GLMaster glHeader = new GLMaster();


            // Used this GLHeader Create GL
            glHeader.GLCode = Rece.Rec_Number;
            glHeader.GL_Date = Rece.Rec_Date;
            glHeader.GL_Desc = Rece.Rec_Description;
            glHeader.Company_Id = Rece.RecComp_ID.Value;
            glHeader.GLType_Id = " قبض نقطة بيع";
            glHeader.Currency_Id = Rece.RecCurrency_ID;
            glHeader.GLState = TransactionState.Pending;
            glHeader.Auto = true;


            // this GL Details Start Bank Secound advansedrec
            GLDetail BankGL = new GLDetail(),
                advansedrec = new GLDetail(),
            bankcomution = new GLDetail();
            // var BusId = db.BussinesUnits.FirstOrDefault().BU_Id;
            BankGL.Bu_Id = vM.Rec_BU;
            BankGL.Asset_Id = null;

            // Used ChartOfAccountRecive  Spacefic Account this Bank or treasury or tax. 
            var paymentMethod = db.PaymentMethods.Find(Rece.RecBank_Account);
            var ChartOfAccounRecive = db.ChartOfAccountings.Find(paymentMethod.Bank_Account_Id);
            BankGL.Account_Id = ChartOfAccounRecive.Account_Id;
            BankGL.Account_Name = ChartOfAccounRecive.Account_Name;


            var bank = db.ChartOfAccountings.Find(paymentMethod.Expense_Account_Id);
            if (bank != null)
            {
                bankcomution.Account_Id = bank.Account_Id;
                bankcomution.Account_Name = bank.Account_Name;
                var branch = db.Branches.Find(Rece.Branch_Id);
                if (branch != null)
                {
                    bankcomution.SubLedger_Id = db.SubLedgers.Where(a => a.SubLed_Name == branch.Branch_Name).FirstOrDefault().SubLed_Id;
                }
                else
                {
                    var compName = db.Companies.FirstOrDefault().Comp_Name;
                    bankcomution.SubLedger_Id = db.SubLedgers.Where(a => a.SubLed_Name == compName).FirstOrDefault().SubLed_Id;
                }

            }

            //var ChartOfAccounRecive = db.ChartOfAccountings.Find(6);
            //BankGL.Account_Id = ChartOfAccounRecive.Account_Id;
            //BankGL.Account_Name = ChartOfAccounRecive.Account_Name;
            BankGL.GLMasterGLMaster_Id = glHeader.GLMaster_Id;

            var ChartOfAccounadvansedrec = db.ChartOfAccountings.Find(37);
            advansedrec.Account_Id = ChartOfAccounadvansedrec.Account_Id;
            advansedrec.Account_Name = ChartOfAccounadvansedrec.Account_Name;
            advansedrec.Bu_Id = vM.Rec_BU;
            advansedrec.Asset_Id = null;
            advansedrec.GLMasterGLMaster_Id = glHeader.GLMaster_Id;

            //var cust = db.Customers.Find(Rece.Customer_Id);
            //SubLedger customerSubledger = db.SubLedgers.Where(subled => subled.SubLed_Code == cust.Customer_Code).FirstOrDefault();
            //BankGL.SubLedger_Id = 0;
            //advansedrec.SubLedger_Id = customerSubledger.SubLed_Id;


            var Customerr = db.Customers.Find(Rece.Customer_Id);
            Accounts Cust = db.Accounts.Where(C => C.Account_Code == Customerr.Customer_Code).FirstOrDefault();
            advansedrec.Account_Child_ID = Cust.Accounts_Id;
            advansedrec.Account_CHild_Name = Cust.Account_Name;
            BankGL.Account_Child_ID = 0;
            var thisinvoice = db.MasterPOSInvoices.Find(Rece.MasterPOSInvoiceMasterPOSInvoiceId);
            // This State GL Start Pending Secound Post thierd Voiod 
            switch (Rece.Rec_state)
            {
                //Post GL
                case TransactionState.Post:
                    glHeader.GLState = TransactionState.Post;
                    var gl = db.GLMasters.Where(a => a.GLCode == glHeader.GLCode && a.GLState == glHeader.GLState).FirstOrDefault();
                    if (gl != null)
                    {
                        OperationRun.DeleteOperation(Rece.Rec_Number);
                        return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                    }

                    // From Account advansedrec to BankGL .
                    advansedrec.GLDetail_Credit = Rece.Rec_Amount;
                    BankGL.GLDetail_Debit = (Rece.Rec_Amount - (Rece.Rec_Amount * (paymentMethod.Percentage.Value / 100))) - paymentMethod.Operatin_Cost.Value;
                    bankcomution.GLDetail_Debit = (Rece.Rec_Amount * (paymentMethod.Percentage.Value / 100)) + paymentMethod.Operatin_Cost.Value;

                    break;
                // Void Invoice
                case TransactionState.Void:
                    glHeader.GLType_Id = "قبض نقطة بيع عكسي";
                    glHeader.GLState = TransactionState.Void;
                    var glcheck = db.GLMasters.Where(a => a.GLCode == glHeader.GLCode && a.GLState == glHeader.GLState).FirstOrDefault();
                    if (glcheck != null)
                    {
                        OperationRun.DeleteOperation(Rece.Rec_Number);
                        return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                    }

                    if (Rece.HasOff == true)

                    {
                        msg = Resources.ReceiptCreate.UnderCloseing;
                        Rece.Rec_state = TransactionState.Post;
                        db.SaveChanges();
                        return msg;

                    }
                    else
                    {

                        advansedrec.GLDetail_Debit = Rece.Rec_Amount;
                        BankGL.GLDetail_Credit = (Rece.Rec_Amount - (Rece.Rec_Amount * (paymentMethod.Percentage.Value / 100))) - paymentMethod.Operatin_Cost.Value;
                        bankcomution.GLDetail_Credit = (Rece.Rec_Amount * (paymentMethod.Percentage.Value / 100)) + paymentMethod.Operatin_Cost.Value;





                    }
                    break;
                default:
                    break;
            }

            glHeader.Exchange_Rate = Rece.Exchange_Rate;
            if (msg == "")
            {
                glHeader.GLDetails.Add(BankGL);
                glHeader.GLDetails.Add(advansedrec);
                if (bankcomution.GLDetail_Credit == 0 && bankcomution.GLDetail_Debit == 0)
                {

                }
                else
                {
                    glHeader.GLDetails.Add(bankcomution);

                }

                db.GLMasters.Add(glHeader);
                if (Math.Round(thisinvoice.Total_Amount.Value, 2, MidpointRounding.AwayFromZero) - thisinvoice.POSReceipts.Where(a => a.Rec_state == TransactionState.Post).Select(a => a.Rec_Amount).Sum() < 0)
                {
                    OperationRun.DeleteOperation(Rece.Rec_Number);
                    return Resources.ReceiptCreate.TotalPayGreaterthanInvoiceValue;
                }
                else
                {
                    db.Entry(thisinvoice).State = EntityState.Modified;
                    GLMaster.GetBalanceForAccount(glHeader);
                    GLMaster.GetBalanceForSubLedger(glHeader);
                    msg = Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                    db.SaveChanges();
                    db.Dispose();
                }

            }
            OperationRun.DeleteOperation(Rece.Rec_Number);
            if (Rece.Rec_state == TransactionState.Post)
            {
                msg = Resources.validate.GLDone;
            }
            else
            {
                msg = Resources.validate.GLReverse;
            }

            return msg;
        }

        // Used this Methoud Delete receipt.

        public static int DeleteRecepit(int perid)
        {
            var db = new AccountingContainer();
            var Myrecipt = db.POSReceipts.Find(perid);
            if (Myrecipt != null)
            {
                db.POSReceipts.Remove(Myrecipt);
                db.SaveChanges();
                db.Dispose();
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// get list of customer for drobdownlist
        /// </summary>
        /// <returns>ReceiptVM</returns>
        public static ReceiptVM GetCustomerList()
        {
            var db = new AccountingContainer();
            ReceiptVM vm = new ReceiptVM();
            List<object> newList = new List<object>();
            var members1 = db.Customers.Where(e => e.Customer_State == Customer_State.نشط).ToList();
            foreach (var member in members1)
                newList.Add(new
                {
                    Id = member.Customer_Id,
                    Name = member.Customer_Name + "        " + member.Customer_Phone
                });
            vm.MyCustomerList = new SelectList(newList, "Id", "Name");

            return vm;
        }
        /// <summary>
        /// get all invoice master fot that customer by id and calculate the reverse for that invoices
        /// </summary>
        /// <param name="custID"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetPayInvoiceMaster(int custID)
        {
            var db = new AccountingContainer();
            var list = db.MasterPOSInvoices.Where(f => f.Customer_Id == custID && f.Invoice_State == TransactionState.Post
            && (f.POSReceipts.Count()==0 || f.Total_Amount > f.POSReceipts.Where(a => a.Rec_state == TransactionState.Post).Select(a => a.Rec_Amount).ToList().Sum())).ToList();
        
                List <MasterPOSInvoice> masterList = new List<MasterPOSInvoice> { };
                foreach (var invoice in list)
                {

                    var reverse = db.ReversePOSInvoices.Where(a => a.MasterPOSInvoiceMasterPOSInvoiceId == invoice.MasterPOSInvoiceId && a.Invoice_State == TransactionState.Post).ToList();
                    if (reverse.FirstOrDefault() == null)
                    {
                        masterList.Add(invoice);
                    }
                    else
                    {
                        decimal TotalReferse = 0;
                        foreach (var reverseInvoice in reverse)
                        {
                            TotalReferse += reverseInvoice.Total_Amount;

                        }
                        if (TotalReferse < invoice.Total_Amount)
                        {
                            masterList.Add(invoice);

                        }
                 
                    }

                }

            List<object> newListInvoice = new List<object>();

            foreach (var member in masterList)
                newListInvoice.Add(new
                {
                    Id = member.MasterPOSInvoiceId,
                    Name = member.Serial_Number + " " + member.Number
                });
            IEnumerable<SelectListItem> PayInvoice = new SelectList(newListInvoice, "Id", "Name");
      
                return PayInvoice;
            }
            /// <summary>
            /// get aReverse of invoice masterr by its id
            /// </summary>
            /// <param name="invoiceId"></param>
            /// <returns></returns>
            public static ReceiptInvoiceSalesRecallVM getAllRecallOfInvoiceSales(int invoiceId)
            {
                var db = new AccountingContainer();
                var thismaster = db.ReversePOSInvoices.Where(r => r.Master_Id == invoiceId).ToList();

                if (thismaster.Count != 0)
                {
                    ReceiptInvoiceSalesRecallVM PPRV = new ReceiptInvoiceSalesRecallVM();
                    ReveInvoiceDetailsVM RVM = new ReveInvoiceDetailsVM();
                    List<decimal> Quantity = new List<decimal>();
                    List<string> code = new List<string>();
                    List<decimal> totalcost = new List<decimal>();

                    foreach (var recall in thismaster)
                    {
                        decimal quant = 0;
                        foreach (var detail in recall.ReversePOSDetails)
                        {

                            quant += detail.Product_Quantity;
                        }
                        Quantity.Add(quant);
                        code.Add(recall.Serial_Number);
                        totalcost.Add(recall.Total_Amount);
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
            /// <summary>
            /// get information for invoices by its id 
            /// </summary>
            /// <param name="invoiceId"></param>
            /// <returns></returns>
            

            public static String deletionactiveGL(int id)
            {
                var db = new AccountingContainer();
                var doc = db.POSReceipts.Find(id);

                if (doc.Rec_state != TransactionState.Pending && doc.HasOff != true)
                {
                    var vm = new POSReceiptVM();
                    vm.Rec_state = TransactionState.Void;
                    vm.Rec_ID = doc.Rec_ID;
                    UpdateRecipt(vm);


                    var gl = db.GLMasters.Where(a => a.GLCode == doc.Rec_Number);
                    foreach (var g in gl)
                    {
                        db.GLDetails.RemoveRange(g.GLDetails);

                        db.GLMasters.Remove(g);

                    }
                    db.ReceiptPOSFileDetails.RemoveRange(doc.ReceiptFileDetails);
                    db.POSReceipts.Remove(doc);
                    db.SaveChanges();



                    db.Dispose();
                    return Resources.validate.DeleteSuccessful;
                }
                return Resources.validate.Error;
            }
            public static decimal getAllPayForthatInvoicebyCode(string InvoiceCode)
            {
                var db = new AccountingContainer();
                var invoice = db.MasterPOSInvoices.Where(a => a.Serial_Number == InvoiceCode).FirstOrDefault();
                var Recipt = db.POSReceipts.Where(a => a.MasterPOSInvoiceMasterPOSInvoiceId == invoice.MasterPOSInvoiceId && a.Rec_state == TransactionState.Post).Select(a => a.Rec_Amount).ToList().Sum();
                return Recipt;
            }
        public static MasterPOSInvoice getInvoiceById(int invoiceId)
        {
            var db = new AccountingContainer();
            var invoice = db.MasterPOSInvoices.Find(invoiceId);
          
            return invoice;
        }
    }
    }