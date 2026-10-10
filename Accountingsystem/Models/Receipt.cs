using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using Accountingsystem.ViewModel;

namespace Accountingsystem.Models
{
    public partial class Receipt
    {
        /// <summary>
        /// make arecipt unique code for user
        /// </summary>
        /// <param name="Serial"></param>
        /// <returns>bool</returns>
        public static bool containCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.Receipts.ToList();
            foreach (var item in allinData)
            {
                if (item.Rec_Number == Serial || !Serial.StartsWith("REC-"))
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
        public static List<Receipt> GetAllReceipt(int ?all)
        {

            var db = new AccountingContainer();
            var receipts = db.Receipts.ToList();
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
        public static Receipt FindReceiptByID(int id)
        {
            var db = new AccountingContainer();
            var receipt = db.Receipts.Find(id);
            return receipt;
        }
        /// <summary>
        /// get file by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ReceiptFileDetail</returns>
        public static ReceiptFileDetail FindFileByID(int id)
        {
            var db = new AccountingContainer();
            var file = db.ReceiptFileDetails.Find(id);
            return file;
        }
        public static string deleteFile(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.ReceiptFileDetails.Find(fileId);
            db.ReceiptFileDetails.Remove(file);
            db.SaveChanges();
            return "تم حذف الملف ";
        }
        /// <summary>
        /// get all Files by the id of Recipts
        /// </summary>
        /// <param name="receId"></param>
        /// <returns>List<ReceiptFileDetail> </returns>
        public static List<ReceiptFileDetail> GetAllFiles(int receId)
        {
            var db = new AccountingContainer();
            var files = db.ReceiptFileDetails.Where(emp => emp.ReceiptRec_ID == receId).ToList();
            return files;
        }
        // used Method Edit to load details of data  for Receipt
        /// <summary>
        /// Edit Receipt used id 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>ReceiptVM</returns>
        public static ReceiptVM FillReseipt(int Id)
        {
            var db = new AccountingContainer();
            var MyResipt = db.Receipts.Find(Id);

            ReceiptVM receipt = new ReceiptVM();
            receipt.MyCurrencyList = new SelectList(db.Currencies, "Currency_ID ", "Currency_Type");
            receipt.MyCustomerList = new SelectList(db.Customers, "Rec_ID", "Rec_Number");


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
            var listofInvoices = db.InvoiceMasters.Where(e => e.Customer_Id == id).ToList();
            decimal totalamount = 0;
            if (listofInvoices.Count==0)
            {
                return totalamount =0;
            }

            else
            {
                foreach (var detail in listofInvoices)
                {
                    totalamount += detail.Total_Amount;
                }
                return totalamount;
            }
           
          
        }
        // Used Method for Creation new Receipt and get data from user
        public static ReceiptVM CreateVMForCreation(int? cust_Id, int?Invoice_Id)
        {
            var db = new AccountingContainer();
            ReceiptVM rec = new ReceiptVM();
           
            rec.MyCurrencyList = new SelectList(db.Currencies, "Currency_ID ", "Currency_Type");
            //List<object> newListo = new List<object>();
            //var members1 = db.Customers.Where(e => e.Customer_State == Customer_State.نشط).ToList();
            //foreach (var member in members1)
            //    newListo.Add(new
            //    {
            //        Id = member.Customer_Id,
            //        Name = member.Customer_Name + "        " + member.Customer_Phone
            //    });
            var newListo = (from cust in db.Customers.Where(a => a.Customer_State == Customer_State.نشط)
                           select new
                           {
                               Id = cust.Customer_Id,
                               Name = cust.Customer_Name + "        " + cust.Customer_Phone

                           }).ToList();
            rec.MyCustomerList = new SelectList(newListo, "Id", "Name");
            rec.MyCompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            rec.MySubLdgerList = new SelectList(db.BussinesUnits, "BU_Id", "BU_Name");
            rec.BranchList = new SelectList(db.Branches, "Branch_Id", "Branch_Name");
            if (cust_Id != null)
            {
                rec.SelectedCustomer = Convert.ToInt32(cust_Id);
            }
           
            var invoiceLists = db.InvoiceMasters.Where(o => o.TotalAmountAfMat > 0 && o.Invoice_State != TransactionState.Void);
            if (cust_Id != 0 && cust_Id != null)
            {
                if (Invoice_Id != 0 && Invoice_Id != null)
                {
                    invoiceLists = invoiceLists.Where(a => a.Master_Id == Invoice_Id);
                    List<object> newListInvoice = new List<object>();

                    foreach (var member in invoiceLists)
                        newListInvoice.Add(new
                        {
                            Id = member.Master_Id,
                            Name = member.Serial_Number + " " + member.Number
                        });
                    rec.InvoiceList = new SelectList(newListInvoice, "Id", "Name");
                    rec.Rec_Amount = invoiceLists.FirstOrDefault().TotalAmountAfMat;
                    rec.selectInvoiceID = invoiceLists.FirstOrDefault().Master_Id;
                }
                else
                {
                    invoiceLists = invoiceLists.Where(a => a.Customer_Id == cust_Id);
                    List<object> newListInvoice = new List<object>();

                    foreach (var member in invoiceLists)
                        newListInvoice.Add(new
                        {
                            Id = member.Master_Id,
                            Name = member.Serial_Number + " " + member.Number
                        });
                    rec.InvoiceList = new SelectList(newListInvoice, "Id", "Name");
                 
                }
            }
            else
            {
                var a = new List<string> { };
                rec.InvoiceList = new SelectList(a, "Master_Id", "Serial_Number");
            }
           
            var lastSerial = db.Receipts.OrderByDescending(c => c.Rec_ID).FirstOrDefault();
            if (lastSerial == null)
            {
                rec.Rec_Number = "REC-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                rec.Rec_Number = "REC-" + (Convert.ToInt32(lastSerial.Rec_Number.Substring(4, lastSerial.Rec_Number.Length - 4)) + 1).ToString("D3");
            }          
            rec.Rec_Date = DateTime.Now.ToString("dd-MM-yyyy   HH:mm:ss");

            var newList = (from pay in db.PaymentMethods.Where(a=>a.IsActive!=false)
                           select new
                           {
                               Id = pay.PaymentMethod_Id,
                               Name = pay.Payment_Type

                           });
            rec.MyChartOfAccountList= new SelectList(newList, "Id", "Name");
         

            return rec;
        }

        // Used this Method create New Receipt and save it in database
        // after spacific data  another Tabel put this data in the model 
        // and add data this tabel  put tabel Receipt
        /// <summary>
        /// Used Receipt Bvm Create in database
        /// </summary>
        /// <param name="bvm"></param>
        /// <returns>bool</returns>
        public static int AddNewReceipt(ReceiptVM bvm , String UserId)
        {
            var db = new AccountingContainer();
            var NeweReceipt = new Receipt();

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
                var invpayMaster = db.InvoiceMasters.Find(bvm.selectInvoiceID);
                // If the invpayMaster is larger than the Payments
                if (Math.Round( bvm.Rec_Amount,2,MidpointRounding.AwayFromZero) <= Math.Round(invpayMaster.TotalAmountAfMat,2,MidpointRounding.AwayFromZero))
                {

                    //  invpayMaster.TotalAmountAfMat = invpayMaster.TotalAmountAfMat - PaymentVM.Pay_Amount;
                    var newTotalAmount = invpayMaster.TotalAmountAfMat;
                }
                else
                {
                    return 0;
                }


                var lastSerial = db.Receipts.OrderByDescending(c => c.Rec_ID).FirstOrDefault();
                if (lastSerial == null)
                {
                    NeweReceipt.Rec_Number = "REC-001";
                }
                else
                {
                    //using string substring method to get the number of the last inserted
                    NeweReceipt.Rec_Number = "REC-" + (Convert.ToInt32(lastSerial.Rec_Number.Substring(4, lastSerial.Rec_Number.Length - 4)) + 1).ToString("D3");
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
                NeweReceipt.Invoice_ID = bvm.selectInvoiceID;
                // Used this Load File Database.
                if (bvm.Files.FirstOrDefault() != null)
                {
                    foreach (var file in bvm.Files)
                    {
                        var filedetails = new ReceiptFileDetail();
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
                traBatch.Date =Convert.ToDateTime(bvm.Rec_Date);
                traBatch.UserId = UserId;

                db.Receipts.Add(NeweReceipt);
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
            var receipt = db.Receipts.Find(id);
            var gl = db.GLMasters.Where(a => a.GLCode == receipt.Rec_Number).FirstOrDefault();
            if (receipt != null&& gl==null)
            {
                db.ReceiptFileDetails.RemoveRange(receipt.ReceiptFileDetails);
                db.Receipts.Remove(receipt);

                db.SaveChanges();
                db.Dispose();
                return "تم الحذف بنجاح";

            }
            return "لم يتم الحذف ";
        }
        /// <summary>
        /// fill for Editing Recipt 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ReceiptVM</returns>
        public static ReceiptVM FillEditTheReceipt(int id)
        {
            var db = new AccountingContainer();
            var receipt = db.Receipts.Find(id);
            var CurrencyId = receipt.RecCurrency_ID;
            var CompanyId = receipt.RecComp_ID;
            var CustomerName =db.Customers.Find( receipt.Customer_Id).Customer_Name;
            var BusId = receipt.Rec_BU;
            var ChartOfAcc = receipt.RecBank_Account;
            var BranchId = receipt.Branch_Id;
            var PaymentId = receipt.PaymentMethod_Id;


            if (receipt == null)
            {
                return null;
            }

            ReceiptVM receiptVM = new ReceiptVM();
            receiptVM.TotalAmountAfMat = receipt.TotalAmountAfMat;
            receiptVM.Rec_state = receipt.Rec_state;
            receiptVM.Rec_Number = receipt.Rec_Number;
            receiptVM.Rec_ID = receipt.Rec_ID;
            receiptVM.Rec_Description = receipt.Rec_Description;
            receiptVM.Rec_Date = (receipt.Rec_Date).ToString("dd-MM-yyyy   HH:mm:ss"); 
            receiptVM.Rec_Amount = receipt.Rec_Amount;
            receiptVM.selectInvoiceID = receipt.Invoice_ID;
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
            var newListo = (from cust in db.Customers.Where(a => a.Customer_State == Customer_State.نشط)
                           select new
                           {
                               Id = cust.Customer_Id,
                               Name = cust.Customer_Name + "        " + cust.Customer_Phone

                           }).ToList();
            receiptVM.MyCustomerList = new SelectList(newListo, "Id", "Name");
            receiptVM.MySubLdgerList = new SelectList(db.BussinesUnits, "BU_Id", "BU_Name");
            receiptVM.BranchList = new SelectList(db.Branches, "Branch_Id", "Branch_Name");
    


            List<object> newListInvoice = new List<object>();

            foreach (var member in db.InvoiceMasters.Where(e => e.Customer_Id == receiptVM.SelectedCustomer && e.TotalAmountAfMat > 0 && e.Invoice_State!=TransactionState.Void))
                newListInvoice.Add(new
                {
                    Id = member.Master_Id,
                    Name = member.Serial_Number + " " + member.Number
                });
            receiptVM.InvoiceList = new SelectList(newListInvoice, "Id", "Name");

            var newList = (from pay in db.PaymentMethods.Where(a => a.IsActive != false)
                           select new
                           {
                               Id = pay.PaymentMethod_Id,
                               Name = pay.Payment_Type

                           });
            receiptVM.MyChartOfAccountList = new SelectList(newList, "Id", "Name");

            return receiptVM;
        }
        /// <summary>
        /// Edit the Recipt and save change in db
        /// </summary>
        /// <param name="receVM"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static int EditReceipt(ReceiptVM receVM , String UserId)
        {
            var db = new AccountingContainer();
            var rece = db.Receipts.Find(receVM.Rec_ID);
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
                    rece.Invoice_ID = receVM.selectInvoiceID;
          

                if (receVM.Files.FirstOrDefault() != null)
                    {
                        foreach (var file in receVM.Files)
                        {
                            var filedetails = new ReceiptFileDetail();
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
        public static String UpdateRecipt(ReceiptVM vM)
        {
            var msg = "";
            var db = new AccountingContainer();
            var Rece = db.Receipts.Find(vM.Rec_ID);
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
            glHeader.GLType_Id = " قبض ميبعات";
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
            var thisinvoice = db.InvoiceMasters.Find(Rece.Invoice_ID);
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
                    BankGL.GLDetail_Debit = (Rece.Rec_Amount - (Rece.Rec_Amount * (paymentMethod.Percentage.Value/100))) - paymentMethod.Operatin_Cost.Value;
                    bankcomution.GLDetail_Debit=( Rece.Rec_Amount * (paymentMethod.Percentage.Value / 100))+paymentMethod.Operatin_Cost.Value;
                    thisinvoice.TotalAmountAfMat = Math.Round(thisinvoice.TotalAmountAfMat,2,MidpointRounding.AwayFromZero) - Math.Round(Rece.Rec_Amount,2,MidpointRounding.AwayFromZero);
                    break;
                // Void Invoice
                case TransactionState.Void:
                    glHeader.GLType_Id = "قبض مبيعات عكسي";
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


                        thisinvoice.TotalAmountAfMat = Math.Round(thisinvoice.TotalAmountAfMat,2,MidpointRounding.AwayFromZero) + Math.Round(Rece.Rec_Amount,2, MidpointRounding.AwayFromZero);


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
                var getAllReceipt = db.Receipts.Where(a => a.Invoice_ID == Rece.Invoice_ID && a.Rec_state != TransactionState.Void).Select(a => a.Rec_Amount).Sum();
                db.GLMasters.Add(glHeader);
                if (Math.Round(getAllReceipt,2,MidpointRounding.AwayFromZero)>Math.Round(thisinvoice.Total_Amount,2,MidpointRounding.AwayFromZero))
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
            var Myrecipt = db.Receipts.Find(perid);
            if (Myrecipt != null)
            {
                db.Receipts.Remove(Myrecipt);
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
            var list = db.InvoiceMasters.Where(f => f.Customer_Id == custID && f.TotalAmountAfMat > 0&&f.Invoice_State!=TransactionState.Void).ToList();
            List<InvoiceMaster> masterList = new List<InvoiceMaster> { };
            foreach (var invoice in list)
            {
                
                var reverse = db.ReverseSalesInvoiceMasters.Where(a => a.InvoiceMaster_Id == invoice.Master_Id&&a.Invoice_State!=TransactionState.Void).ToList();
                if (reverse.FirstOrDefault()==null)
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
                    if(TotalReferse < invoice.Total_Amount)
                    {
                        masterList.Add(invoice);

                    }
                }
                
            }

           
            //masterVM.purchInvList = new SelectList(invoices, "MasterPOSInvoiceId", "Serial_Number");

            List<object> newListInvoice = new List<object>();

            foreach (var member in masterList)
                newListInvoice.Add(new
                {
                    Id = member.Master_Id,
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
            var thismaster = db.ReverseSalesInvoiceMasters.Where(r => r.InvoiceMaster_Id == invoiceId).ToList();

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
                    foreach (var detail in recall.ReveInvoiceDetails)
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
            public static MatchingInvoiceDetailsVM GetInvoiceDetailsVM(int invoiceId)
            {
                var db = new AccountingContainer();
            MatchingInvoiceDetailsVM invoiceDetailsVM = new MatchingInvoiceDetailsVM();
            var InvoiceDetails = db.InvoiceMasters.Find(invoiceId);
            if (InvoiceDetails != null)
            {

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
                invoiceDetailsVM.Customer_Id = InvoiceDetails.Customer_Id;
                if (InvoiceDetails.Store_ID != 0 && InvoiceDetails.Store_ID != null)
                {
                    invoiceDetailsVM.Store_ID = InvoiceDetails.Store_ID;
                }
                else
                {
                    invoiceDetailsVM.Store_ID = 0;
                }
                
                invoiceDetailsVM.Company_Id = InvoiceDetails.Company_Id;
                invoiceDetailsVM.TotalAmountAfMat = InvoiceDetails.TotalAmountAfMat;
                invoiceDetailsVM.ReverseSales = new List<ReverseSalesInvoiceMaster>();
                var ReverseList = db.ReverseSalesInvoiceMasters.Where(a => a.InvoiceMaster_Id == invoiceId).ToList();
                if (ReverseList.Count() > 0)
                {
                    invoiceDetailsVM.TotalAmountAfMat = (InvoiceDetails.TotalAmountAfMat) - ReverseList.Select(a => a.Total_Amount).Sum();
                    invoiceDetailsVM.ReverseSales = ReverseList;
                }

                return invoiceDetailsVM;
            }
            return invoiceDetailsVM;
        }

        public static String deletionactiveGL(int id)
        {
            var db = new AccountingContainer();
            var doc = db.Receipts.Find(id);
           
            if (doc.Rec_state != TransactionState.Pending && doc.HasOff != true)
            {
                var vm = new ReceiptVM();
                vm.Rec_state = TransactionState.Void;
                vm.Rec_ID = doc.Rec_ID;
                UpdateRecipt(vm);


                var gl = db.GLMasters.Where(a => a.GLCode == doc.Rec_Number);
                foreach (var g in gl)
                {
                    db.GLDetails.RemoveRange(g.GLDetails);

                    db.GLMasters.Remove(g);

                }
                db.ReceiptFileDetails.RemoveRange(doc.ReceiptFileDetails);
                db.Receipts.Remove(doc);
                db.SaveChanges();



                db.Dispose();
                return Resources.validate.DeleteSuccessful;
            }
            return Resources.validate.Error;
        }
        public static decimal getAllPayForthatInvoicebyCode(string InvoiceCode)
        {
            var db = new AccountingContainer();
            var invoice = db.InvoiceMasters.Where(a => a.Serial_Number == InvoiceCode||a.Number== InvoiceCode).FirstOrDefault();
            var Recipt = db.Receipts.Where(a => a.Invoice_ID == invoice.Master_Id && a.Rec_state == TransactionState.Post).Select(a => a.Rec_Amount).ToList().Sum();
            return Recipt;
        }
    }
}