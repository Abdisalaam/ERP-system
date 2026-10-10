using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class ProjectPayment
    {
        /// <summary>
        /// make that payment unique code
        /// </summary>
        /// <param name="Serial"></param>
        /// <returns>bool</returns>
        public static bool containCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.ProjectPayments.ToList();
            foreach (var item in allinData)
            {

                if (item.ProjectPay_Code == Serial || !Serial.StartsWith("PROPAY-"))
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

        /// <summary>
        /// get All project payment from database
        /// </summary>
        /// <returns>List<ProjectPayment></returns>
        public static List<ProjectPayment> GetAllProjectPayment()
        {
            var db = new AccountingContainer();
            var Pays = db.ProjectPayments.ToList();
            return Pays;
        }/// <summary>
         /// get detail of project payment using by its id
         /// </summary>
         /// <param name="id"></param>
         /// <returns>ProjectPayment</returns>
        public static ProjectPayment GetProjectPaymentById(int id)
        {
            var db = new AccountingContainer();
            var projectPayment = db.ProjectPayments.Find(id);
            return projectPayment;
        }
        /// <summary>
        /// get file for payment project by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ProjectPaymentFile FindFileByID(int id)
        {
            var db = new AccountingContainer();
            var file = db.ProjectPaymentFiles.Find(id);
            return file;
        }
        /// <summary>
        /// get all file that belonge to that Project payement by its id
        /// </summary>
        /// <param name="proId"></param>
        /// <returns>List<ProjectPaymentFile> </returns>
        public static List<ProjectPaymentFile> GetAllFiles(int proId)
        {
            var db = new AccountingContainer();
            var files = db.ProjectPaymentFiles.Where(p => p.ProjectPaymentId == proId).ToList();
            return files;
        }
        /// <summary>
        /// prepare project payment for Creation
        /// </summary>
        /// <returns>ProjectPaymentVM</returns>
        public static ProjectPaymentVM AddProjectPaymentVM()
        {
            var db = new AccountingContainer();
            var PaymentVM = new ProjectPaymentVM();

            //sort the Invoices and get the last insert Payment.
            var lastSerial = db.ProjectPayments.OrderByDescending(c => c.ProjectPaymentId).FirstOrDefault();
            if (lastSerial == null)
            {
                PaymentVM.ProjectPay_Code = "PROPAY-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                PaymentVM.ProjectPay_Code = "PROPAY-" + (Convert.ToInt32(lastSerial.ProjectPay_Code.Substring(7, lastSerial.ProjectPay_Code.Length - 7)) + 1).ToString("D3");
            }


            PaymentVM.Date = DateTime.Now.ToString("dd-MM-yyyy   HH:mm:ss");
            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() ==0&&a.isActive!=false);
            var subLedgerList = from sub in db.SubLedgers.Where(a => a.SubLed_Type == "مشروع"&&a.isActive!=false)
                                select new
                                {
                                    Id = sub.SubLed_Id,
                                    Name = sub.SubLed_Name + "   " + db.Projects.Where(a => a.Project_Name == sub.SubLed_Name).FirstOrDefault().Project_Code,
                                    status= db.Projects.Where(a => a.Project_Name == sub.SubLed_Name).FirstOrDefault().Project_State

                                };

            var supList = from Supp in db.Suppliers.Where(a => a.Supplier_state == Customer_State.نشط)
                          select new
                          {
                              Id = Supp.Supp_Id,
                              Name = Supp.Supp_Name + " " + Supp.Supp_Phone

                          };
            PaymentVM.ProjectSubSubList = new SelectList(Enumerable.Empty<SelectListItem>());
            PaymentVM.ProjectSubList = new SelectList(Enumerable.Empty<SelectListItem>());
            PaymentVM.ProjectClauseList = new SelectList(Enumerable.Empty<SelectListItem>());
            PaymentVM.SupplierList = new SelectList(supList, "Id", "Name");

            //PaymentVM.BranchSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
            //PaymentVM.DepartSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
            //PaymentVM.BussinessList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            //PaymentVM.SubledgerList = new SelectList(db.SubLedgers.Where(a => a.SubLed_Type == "مركز تكلفة يدوى"), "SubLed_Id", "SubLed_Name");
            var thistaxinchart = db.ChartOfAccountings.Find(35);
            PaymentVM.taxList = new SelectList(db.Taxes.Where(e=>e.Tax_Code.Contains(thistaxinchart.Account_Code)).Where(a => a.IsActive != false).ToList(), "Tax_ID", "Tax_Name");
            PaymentVM.MyExpenseList = new SelectList(lista.Where(accCode => accCode.Account_Code.StartsWith("05")), "Account_Id", "Account_Name");
            PaymentVM.MyRevenueList = new SelectList(lista.Where(accCode => accCode.Account_Code.StartsWith("0101200010") || accCode.Account_Code.StartsWith("0101200020")), "Account_Id", "Account_Name");
            PaymentVM.MyProjectList = new SelectList(subLedgerList.Where(a=>a.status!=ProjectState.Finished), "Id", "Name");
          
            PaymentVM.ProjectPay_State = TransactionState.Pending;
            return PaymentVM;
        }
        /// <summary>
        /// add new Project payment and save it in data base and check the user
        /// </summary>
        /// <param name="PaymentVM"></param>
        /// <param name="UserId"></param>
        /// <returns>bool</returns>
        public static int AddNewProjectPayment(ProjectPaymentVM PaymentVM , String UserId)
        {
            var db = new AccountingContainer();
            var projectPayment = new ProjectPayment();
            if((PaymentVM.Revenue_Id==0&& PaymentVM.SupplierID == 0)||(PaymentVM.Revenue_Id == null && PaymentVM.SupplierID == null))
            {
                return 0;
            }


            projectPayment.ProjectPay_State = TransactionState.Pending;
            projectPayment.Description = PaymentVM.Description;
            var lastSerial = db.ProjectPayments.OrderByDescending(c => c.ProjectPaymentId).FirstOrDefault();
            if (lastSerial == null)
            {
                projectPayment.ProjectPay_Code = "PROPAY-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                projectPayment.ProjectPay_Code = "PROPAY-" + (Convert.ToInt32(lastSerial.ProjectPay_Code.Substring(7, lastSerial.ProjectPay_Code.Length - 7)) + 1).ToString("D3");
            }
            projectPayment.Pay_Amount = PaymentVM.Pay_Amount;

            var comp = db.Companies.FirstOrDefault();
            if (comp != null)
            {
                var date = Convert.ToDateTime(PaymentVM.Date);
                if (date < comp.StartFinancialDate)
                {
                    projectPayment.Date = DateTime.Now;
                }
                else
                {
                    projectPayment.Date = Convert.ToDateTime(PaymentVM.Date);
                }
            }
            else
            {
                projectPayment.Date = DateTime.Now;
            }           

            var resultAccount = db.ChartOfAccountings.Find(PaymentVM.Expense_Id);
            if (resultAccount != null)
            {
                projectPayment.Expense_Id = resultAccount.Account_Id;
                projectPayment.Expense_Name = resultAccount.Account_Name;
            }
            if (PaymentVM.Revenue_Id > 0)
            {
                var resultAccountRevenu = db.ChartOfAccountings.Find(PaymentVM.Revenue_Id);
                if (resultAccountRevenu != null)
                {
                    projectPayment.Revenue_Id = resultAccountRevenu.Account_Id;
                    projectPayment.Revenue_Name = resultAccountRevenu.Account_Name;
                }
            }
            else
            {
                PaymentVM.Revenue_Id = 0;
            }
            if (PaymentVM.SupplierID > 0)
            {
             
                    projectPayment.Supplier_ID = PaymentVM.SupplierID;
                var thissupplier = db.Suppliers.Find(PaymentVM.SupplierID);
                projectPayment.Revenue_Name = thissupplier.Supp_Name;
            }
            else
            {
                projectPayment.Supplier_ID = 0;

            }
            if(PaymentVM.ProjectSubSub_Id != 0&& PaymentVM.ProjectSubSub_Id != null)
            {
                projectPayment.SubLedger_Id = PaymentVM.ProjectSubSub_Id;
            }
            else if (PaymentVM.ProjectSub_Id != 0 && PaymentVM.ProjectSub_Id != null)
            {
                projectPayment.SubLedger_Id = PaymentVM.ProjectSub_Id;
            }
            else
            {
                projectPayment.SubLedger_Id = PaymentVM.Project_Id;
            }
            projectPayment.ProjectSubSub_Id = PaymentVM.ProjectSubSub_Id;
            projectPayment.ProjectSub_Id = PaymentVM.ProjectSub_Id;
            projectPayment.ProjectClause_Id = PaymentVM.ProjectClause_Id;
            projectPayment.Project_Id = PaymentVM.Project_Id;

            if (PaymentVM.Selected_Tax_Id > 0 && PaymentVM.Selected_Tax_Id != null)
            {
                var tax = db.Taxes.Find(PaymentVM.Selected_Tax_Id);
                if (tax != null)
                {
                    projectPayment.Tax_Id = tax.Tax_ID;
                    projectPayment.Total_Tax = (PaymentVM.Pay_Amount / (1 + (tax.Tax_Percentage / 100)))* (tax.Tax_Percentage / 100);
                }

            }
            else
            {
                projectPayment.Total_Tax = 0;
            }

            // Used this Load File Database.
            if (PaymentVM.Files.FirstOrDefault() != null)
            {
                foreach (var file in PaymentVM.Files)
                {
                    var filedetails = new ProjectPaymentFile();
                    var data = new byte[file.ContentLength];
                    filedetails.FileName = file.FileName;
                    filedetails.ContentLength = file.ContentLength;
                    file.InputStream.Read(data, 0, file.ContentLength);
                    filedetails.ProImg = data;
                    filedetails.ProjectPaymentId = projectPayment.ProjectPaymentId;
                    if (data.Length < 1000000)
                    {
                        projectPayment.ProjectPaymentFiles.Add(filedetails);
                    }

                    
                }
            }

           

            var traBatch = new TransactionBatch();

            traBatch.OperationName =  "سند صرف مشروع";
            traBatch.Description = null;
            traBatch.Date = Convert.ToDateTime(PaymentVM.Date);
            traBatch.UserId = UserId;

            db.ProjectPayments.Add(projectPayment);
            db.SaveChanges();
            traBatch.OperationId = projectPayment.ProjectPaymentId;
            traBatch.Operation_Code = projectPayment.ProjectPay_Code;
            TransactionBatch.AddNewTransaction(traBatch);
            db.Dispose();
            return projectPayment.ProjectPaymentId;
        }
        /// <summary>
        /// prepare form for Edit ProjectPayment 
        /// </summary>
        /// <param name="payId"></param>
        /// <returns>ProjectPaymentVM</returns>
        public static ProjectPaymentVM FillForEditPayment(int payId)
        {
            var db = new AccountingContainer();
            var PaymentVM = new ProjectPaymentVM();
            var payment = db.ProjectPayments.Find(payId);
           
            var RevenueId = payment.Revenue_Id;
            var ExpenseId = payment.Expense_Id;
            var projectId = payment.Project_Id;
            PaymentVM.Project_Id = payment.Project_Id;
            if (payment.Revenue_Id == 0)
            {
                PaymentVM.SupplierID = payment.Supplier_ID;
            }
            else
            {
                var ChartOfAccountReven = db.ChartOfAccountings.Where(c => c.Account_Id == RevenueId);
                if (ChartOfAccountReven.Any())
                {
                    PaymentVM.Revenue_Id = ChartOfAccountReven.First().Account_Id;
                    PaymentVM.Revenue_Name = ChartOfAccountReven.First().Account_Name;
                }
            }
         

            var ChartOfAccount = db.ChartOfAccountings.Where(c => c.Account_Id == ExpenseId);
            if (ChartOfAccount.Any())
            {
                PaymentVM.Expense_Id = ChartOfAccount.First().Account_Id;
                PaymentVM.Expense_Name = ChartOfAccount.First().Account_Name;
            }

            var projects = db.SubLedgers.Where(c => c.SubLed_Id == projectId).FirstOrDefault();
            var pro = db.Projects.Where(z => z.Project_Name == projects.SubLed_Name).FirstOrDefault();
            
            if (payment.Tax_Id > 0 && payment.Tax_Id != null)
            {
                var taxId = payment.Tax_Id;
                var tax = db.Taxes.Where(c => c.Tax_ID == taxId);
                if (tax.Any())
                {
                    PaymentVM.Selected_Tax_Id = tax.First().Tax_ID;
                }
            }
         

            PaymentVM.ProjectPaymentId = payment.ProjectPaymentId;
            PaymentVM.Date = payment.Date.ToString("dd-MM-yyyy   HH:mm:ss");
            PaymentVM.ProjectPay_Code = payment.ProjectPay_Code;
            PaymentVM.Description = payment.Description;
            PaymentVM.Pay_Amount = payment.Pay_Amount;
            PaymentVM.SupplierID = payment.Supplier_ID;



            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0 && a.isActive != false);
            var subLedgerList = from sub in db.SubLedgers.Where(a => a.SubLed_Type == "مشروع"&& a.isActive != false)
                                select new
                                {
                                    Id = sub.SubLed_Id,
                                    Name = sub.SubLed_Name + "   " + db.Projects.Where(a => a.Project_Name == sub.SubLed_Name).FirstOrDefault().Project_Code,
                                    status= db.Projects.Where(a => a.Project_Name == sub.SubLed_Name).FirstOrDefault().Project_State
                                };

            var supList = from Supp in db.Suppliers.Where(a => a.Supplier_state == Customer_State.نشط)
                          select new
                          {
                              Id = Supp.Supp_Id,
                              Name = Supp.Supp_Name + " " + Supp.Supp_Phone

                          };
            var thistaxinchart = db.ChartOfAccountings.Find(35);
            PaymentVM.taxList = new SelectList(db.Taxes.Where(e => e.Tax_Code.Contains(thistaxinchart.Account_Code)).ToList().Where(a => a.IsActive != false), "Tax_ID", "Tax_Name");
            if (db.SubLedgers.Where(a => a.SubLedgerSubLed_Id == payment.Project_Id).Count() > 0)
            {
                PaymentVM.ProjectSubList = new SelectList(db.SubLedgers.Where(a => a.SubLedgerSubLed_Id == payment.Project_Id&& a.isActive != false), "SubLed_Id", "SubLed_Name");
                PaymentVM.ProjectSub_Id = payment.ProjectSub_Id;
            }
            else
            {
                PaymentVM.ProjectSubList = new SelectList(Enumerable.Empty<SelectListItem>());
            }
            if (payment.ProjectSub_Id > 0)
            {
                PaymentVM.ProjectSubSubList = new SelectList(db.SubLedgers.Where(a => a.SubLedgerSubLed_Id== payment.ProjectSub_Id && a.isActive != false), "SubLed_Id", "SubLed_Name");
               
                    PaymentVM.ProjectSubSub_Id = payment.ProjectSubSub_Id;
                
                
            }
            else
            {
                PaymentVM.ProjectSubSubList = new SelectList(Enumerable.Empty<SelectListItem>());
             
             
            }
            if (pro.ProjectSubs.Count() > 0)
            {
               
                PaymentVM.ProjectClauseList = new SelectList(db.ProjectClauses.Where(p => p.Project_Id == pro.Project_Id), "Id", "Name");
                PaymentVM.ProjectClause_Id = payment.ProjectClause_Id;
            }
            else
            {
              

                PaymentVM.ProjectClauseList = new SelectList(Enumerable.Empty<SelectListItem>());
            }
            //PaymentVM.DepartSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");


            PaymentVM.MyRevenueList = new SelectList(lista.Where(accCode => accCode.Account_Code.StartsWith("0101200010") || accCode.Account_Code.StartsWith("0101200020")), "Account_Id", "Account_Name");
            PaymentVM.MyProjectList = new SelectList(subLedgerList.Where(a => a.status != ProjectState.Finished), "Id", "Name");
            PaymentVM.MyExpenseList = new SelectList(lista.Where(accCode => accCode.Account_Code.StartsWith("05")), "Account_Id", "Account_Name");
            PaymentVM.SupplierList = new SelectList(supList, "Id", "Name");
            PaymentVM.ProjectPay_State = TransactionState.Pending;
            return PaymentVM;
        }
        /// <summary>
        /// save the editing data in data base
        /// </summary>
        /// <param name="PaymentVM"></param>
        /// <param name="UserId"></param>
        /// <returns>bool</returns>
        public static int EditProjectPayment(ProjectPaymentVM PaymentVM , String UserId)
        {

            var db = new AccountingContainer();
            var projectPayment = db.ProjectPayments.Find(PaymentVM.ProjectPaymentId);
            try
            {
                if (PaymentVM != null)
                {
                    if ((PaymentVM.Revenue_Id == 0 && PaymentVM.SupplierID == 0) || (PaymentVM.Revenue_Id == null && PaymentVM.SupplierID == null))
                    {
                        return 0;
                    }
                    projectPayment.ProjectPay_State = TransactionState.Pending;
                    projectPayment.Description = PaymentVM.Description;
                    projectPayment.ProjectPay_Code = PaymentVM.ProjectPay_Code;
                    projectPayment.Pay_Amount = PaymentVM.Pay_Amount;
                    projectPayment.Supplier_ID = PaymentVM.SupplierID;
                    if (PaymentVM.Revenue_Id == null)
                    {
                        projectPayment.Revenue_Id = 0;
                    }
               else
                    {
                        projectPayment.Revenue_Id = PaymentVM.Revenue_Id.Value;
                    }
                    //projectPayment.Date = Convert.ToDateTime( PaymentVM.Date);

                    var comp = db.Companies.FirstOrDefault();
                    if (comp != null)
                    {
                        var date = Convert.ToDateTime(PaymentVM.Date);
                        if (date < comp.StartFinancialDate)
                        {
                            projectPayment.Date = DateTime.Now;
                        }
                        else
                        {
                            projectPayment.Date = Convert.ToDateTime(PaymentVM.Date);
                        }
                    }
                    else
                    {
                        projectPayment.Date = DateTime.Now;
                    }

                    var resultAccount = db.ChartOfAccountings.Find(PaymentVM.Expense_Id);
                    if (resultAccount != null)
                    {
                        projectPayment.Expense_Id = resultAccount.Account_Id;
                        projectPayment.Expense_Name = resultAccount.Account_Name;
                    }
                    if (PaymentVM.Revenue_Id > 0)
                    {
                        var resultAccountRevenu = db.ChartOfAccountings.Find(PaymentVM.Revenue_Id);
                        if (resultAccountRevenu != null)
                        {
                            projectPayment.Revenue_Id = resultAccountRevenu.Account_Id;
                            projectPayment.Revenue_Name = resultAccountRevenu.Account_Name;
                        }
                     
                        
                    }
                    else
                    {
                        PaymentVM.Revenue_Id = 0;
                    }
                    if (PaymentVM.SupplierID > 0)
                    {

                        projectPayment.Supplier_ID = PaymentVM.SupplierID;
                        PaymentVM.Revenue_Id = 0;
                      
                        var thissupplier = db.Suppliers.Find(PaymentVM.SupplierID);
                        projectPayment.Revenue_Name = thissupplier.Supp_Name;
                    }

                    //var resultAccountRevenu = db.ChartOfAccountings.Find(PaymentVM.Revenue_Id);
                    //if (resultAccountRevenu != null)
                    //{
                    //    projectPayment.Revenue_Id = resultAccountRevenu.Account_Id;
                    //    projectPayment.Revenue_Name = resultAccountRevenu.Account_Name;
                    //}
                    if (PaymentVM.ProjectSubSub_Id != 0 && PaymentVM.ProjectSubSub_Id != null)
                    {
                        projectPayment.SubLedger_Id = PaymentVM.ProjectSubSub_Id;
                    }
                    else if (PaymentVM.ProjectSub_Id != 0 && PaymentVM.ProjectSub_Id != null)
                    {
                        projectPayment.SubLedger_Id = PaymentVM.ProjectSub_Id;
                    }
                    else
                    {
                        projectPayment.SubLedger_Id = PaymentVM.Project_Id;
                    }
                    projectPayment.ProjectSubSub_Id = PaymentVM.ProjectSubSub_Id;
                    projectPayment.ProjectSub_Id = PaymentVM.ProjectSub_Id;
                    projectPayment.ProjectClause_Id = PaymentVM.ProjectClause_Id;
                    projectPayment.Project_Id = PaymentVM.Project_Id;
                    if (PaymentVM.Selected_Tax_Id > 0 && PaymentVM.Selected_Tax_Id != null)
                    {
                        var tax = db.Taxes.Find(PaymentVM.Selected_Tax_Id);
                        if (tax != null)
                        {
                            projectPayment.Tax_Id = tax.Tax_ID;
                           
                            projectPayment.Total_Tax = (PaymentVM.Pay_Amount / (1 + (tax.Tax_Percentage / 100))) * (tax.Tax_Percentage / 100);
                        }
                    }
                    else
                    {
                        if (projectPayment.Tax_Id == 0||projectPayment==null)
                        {
                            projectPayment.Tax_Id = 0;
                            projectPayment.Total_Tax = 0;
                        }
                    }

                    // Used this Load File Database.
                    if (PaymentVM.Files.FirstOrDefault() != null)
                    {
                        foreach (var file in PaymentVM.Files)
                        {
                            var filedetails = new ProjectPaymentFile();
                            var data = new byte[file.ContentLength];
                            filedetails.FileName = file.FileName;
                            filedetails.ContentLength = file.ContentLength;
                            file.InputStream.Read(data, 0, file.ContentLength);
                            filedetails.ProImg = data;
                            filedetails.ProjectPaymentId = projectPayment.ProjectPaymentId;
                            if (data.Length < 1000000)
                            {
                                projectPayment.ProjectPaymentFiles.Add(filedetails);
                            }
                        }
                    }

                    var traBatch = new TransactionBatch();

                    traBatch.OperationName = " تعديل سند صرف مشروع";
                    traBatch.Description = null;
                    traBatch.Date = Convert.ToDateTime(PaymentVM.Date);
                    traBatch.UserId = UserId;

                    db.Entry(projectPayment).State = EntityState.Modified;                
                    db.SaveChanges();
                    traBatch.OperationId = projectPayment.ProjectPaymentId;
                    traBatch.Operation_Code = projectPayment.ProjectPay_Code;
                    TransactionBatch.AddNewTransaction(traBatch);
                    db.Dispose();
                    return projectPayment.ProjectPaymentId;
                }
            }
            catch (Exception e)
            {
                var masg = e.Message;
                var result = e.Data;
            }

            return 0;
        }
        /// <summary>
        /// make gl for that payment Project and check is valid or not
        /// </summary>
        /// <param name="payVM"></param>
        /// <returns></returns>
        public static String UpdateProjectPayment(ProjectPaymentVM payVM)
        {
            var msg = "";
            var db = new AccountingContainer();
            var payment = db.ProjectPayments.Find(payVM.ProjectPaymentId);
            OperationRun.Addnewoperation(payment.ProjectPay_Code);
            var operation = db.OperationRuns.Where(a => a.Code == payment.ProjectPay_Code).Count();
            if (operation != 1)
            {
                OperationRun.DeleteOperation(payment.ProjectPay_Code);
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.PleaseWait;
            }
            if (payment.ProjectPay_State == payVM.ProjectPay_State)
            {
                OperationRun.DeleteOperation(payment.ProjectPay_Code);
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.hasbeendoneby + " " + payment.ProjectPay_State + " " + Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.previously;
            }
            else
            {
                payment.ProjectPay_State = payVM.ProjectPay_State;
                db.Entry(payment).State = EntityState.Modified;
                GLMaster glHeader = new GLMaster();
                // Used this GLHeader Create GL
                glHeader.GL_Date = Convert.ToDateTime(payment.Date);
                glHeader.GL_Desc = payment.Description;
                glHeader.Company_Id = db.Companies.ToList().FirstOrDefault().Comp_Id;
                glHeader.GLType_Id = " صرف مشروع";
                glHeader.Currency_Id = "الريال السعودى";
                glHeader.GLCode = payment.ProjectPay_Code;
                glHeader.Auto = true;
                glHeader.GLState = TransactionState.Pending;

                // this GL Details Start advansedrec Secound BankGL
                GLDetail ExpenseAccount = new GLDetail(),
                    TaxAccount = new GLDetail(),
                    RevenueAccount = new GLDetail(),
                    supplierCredit=new GLDetail(),
                    supplierDepit=new GLDetail();

                

                
                RevenueAccount.Bu_Id = 0;
                RevenueAccount.Asset_Id = null;



                // Used ChartOfAccountRecive  Spacefic Account this Bank or treasury or tax. 
                if (payment.Revenue_Id > 0)
                {
                    var ChartOfAccounPay = db.ChartOfAccountings.Find(payment.Revenue_Id);
                    RevenueAccount.Account_Id = ChartOfAccounPay.Account_Id;
                    RevenueAccount.Account_Name = ChartOfAccounPay.Account_Name;
                    RevenueAccount.GLMasterGLMaster_Id = glHeader.GLMaster_Id;
                }
                if (payment.Revenue_Id > 0 && payment.Supplier_ID > 0)
                {
                    var sup = db.Suppliers.Find(payment.Supplier_ID);
                    var ChartOfAccounPay = db.ChartOfAccountings.Find(45);
                    supplierCredit.Account_Id = ChartOfAccounPay.Account_Id;
                    supplierCredit.Account_Name = ChartOfAccounPay.Account_Name;
                    Accounts supplSubledger = db.Accounts.Where(e => e.Account_Code == sup.Supp_Code).ToList().FirstOrDefault();
                    supplierCredit.Account_Child_ID = supplSubledger.Accounts_Id;
                    supplierCredit.Account_CHild_Name = supplSubledger.Account_Name;
                    supplierCredit.GLMasterGLMaster_Id = glHeader.GLMaster_Id;


                    supplierDepit.Account_Id = ChartOfAccounPay.Account_Id;
                    supplierDepit.Account_Name = ChartOfAccounPay.Account_Name;
                    
                    supplierDepit.Account_Child_ID = supplSubledger.Accounts_Id;
                    supplierDepit.Account_CHild_Name = supplSubledger.Account_Name;
                    supplierDepit.GLMasterGLMaster_Id = glHeader.GLMaster_Id;
                }

                if(payment.Supplier_ID>0&& payment.Revenue_Id==0)
                {
                    var sup = db.Suppliers.Find(payment.Supplier_ID);
                    var ChartOfAccounPay = db.ChartOfAccountings.Find(45);
                    RevenueAccount.Account_Id = ChartOfAccounPay.Account_Id;
                    RevenueAccount.Account_Name = ChartOfAccounPay.Account_Name;
                    Accounts supplSubledger = db.Accounts.Where(e => e.Account_Code == sup.Supp_Code).ToList().FirstOrDefault();
                    RevenueAccount.Account_Child_ID = supplSubledger.Accounts_Id;
                    RevenueAccount.Account_CHild_Name = supplSubledger.Account_Name;
                    RevenueAccount.GLMasterGLMaster_Id = glHeader.GLMaster_Id;
                }




                if (payment.Tax_Id != null && payment.Tax_Id > 0)
                {
                    var thistax = db.Taxes.Find(payment.Tax_Id).Tax_Code;
                    if (thistax != null)
                    {
                        var taxChart = db.ChartOfAccountings.Where(a => a.Account_Code == thistax).FirstOrDefault();
                        TaxAccount.Account_Id = taxChart.Account_Id;
                        TaxAccount.Account_Name = taxChart.Account_Name;
                        TaxAccount.GLMasterGLMaster_Id = glHeader.GLMaster_Id;

                        
                        TaxAccount.Bu_Id = 0;
                        TaxAccount.Asset_Id = null;
                    }
                }



                
                ExpenseAccount.Bu_Id = 1;
                ExpenseAccount.Asset_Id = null;

                // Used ChartOfAccountRecive  Spacefic Account this advansedPayment . 
                var ChartOfAccounadvansedrec = db.ChartOfAccountings.Find(payment.Expense_Id);
                ExpenseAccount.Account_Id = ChartOfAccounadvansedrec.Account_Id;
                ExpenseAccount.Account_Name = ChartOfAccounadvansedrec.Account_Name;
                ExpenseAccount.GLMasterGLMaster_Id = glHeader.GLMaster_Id;

                //var sup = db.Projects.Find(project.Project_Id);
                //SubLedger supplSubledger = db.SubLedgers.Where(subled => subled.SubLed_Code == sup.Project_Code).FirstOrDefault();
                //ExpenseAccount.SubLedger_Id = supplSubledger.SubLed_Id;

                //var sup = db.SubLedgers.Find(project.Project_Id);
                ExpenseAccount.SubLedger_Id = payment.SubLedger_Id;
                ExpenseAccount.ProjectClause_Id = payment.ProjectClause_Id;
                RevenueAccount.SubLedger_Id = 0;

                // This State GL Start Pending Secound Post thierd Voiod 
                switch (payment.ProjectPay_State)
                {
                    //Post GL
                    case TransactionState.Post:
                        // From Account ExpenseAccount to RevenueAccounts.
                        //ExpenseAccount.GLDetail_Debit = payment.Pay_Amount;
                        glHeader.GLState = TransactionState.Post;
                        var gl = db.GLMasters.Where(a => a.GLCode == glHeader.GLCode && a.GLState == glHeader.GLState).FirstOrDefault();
                        if (gl != null)
                        {
                            OperationRun.DeleteOperation(payment.ProjectPay_Code);
                            return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                        }

                        RevenueAccount.GLDetail_Credit = payment.Pay_Amount;
                        if (payment.Total_Tax != null && payment.Total_Tax > 0)
                        {
                            TaxAccount.GLDetail_Debit = (decimal)payment.Total_Tax;
                            ExpenseAccount.GLDetail_Debit = payment.Pay_Amount-payment.Total_Tax.Value;

                        }
                        else
                        {
                            ExpenseAccount.GLDetail_Debit = payment.Pay_Amount;
                        }
                        if (payment.Revenue_Id > 0 && payment.Supplier_ID > 0)
                        {
                            supplierCredit.GLDetail_Credit= payment.Pay_Amount;
                            supplierDepit.GLDetail_Debit = payment.Pay_Amount;

                        }


                            break;
                    // Void Invoice
                    case TransactionState.Void:
                        glHeader.GLType_Id = " صرف مشروع عكسي";
                        glHeader.GLState = TransactionState.Void;
                        var glcheck = db.GLMasters.Where(a => a.GLCode == glHeader.GLCode && a.GLState == glHeader.GLState).FirstOrDefault();
                        if (glcheck != null)
                        {
                            OperationRun.DeleteOperation(payment.ProjectPay_Code);
                            return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                        }

                        if (payment.HasOff == true)

                        {
                            msg = Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.UnderCloseing;
                            payment.ProjectPay_State = TransactionState.Post;
                            db.SaveChanges();
                            OperationRun.DeleteOperation(payment.ProjectPay_Code);
                            return msg;

                        }
                        else
                        {
                            // From Account RevenueAccounts  to ExpenseAccount                    

                            RevenueAccount.GLDetail_Debit = payment.Pay_Amount;
                            if (payment.Total_Tax != null && payment.Total_Tax > 0)
                            {
                                TaxAccount.GLDetail_Credit = (decimal)payment.Total_Tax;

                                ExpenseAccount.GLDetail_Credit = payment.Pay_Amount-payment.Total_Tax.Value ;

                            }
                            else
                            {
                                ExpenseAccount.GLDetail_Credit = payment.Pay_Amount;
                            }
                        }
                        if (payment.Revenue_Id > 0 && payment.Supplier_ID > 0)
                        {
                            supplierCredit.GLDetail_Debit = payment.Pay_Amount;
                            supplierDepit.GLDetail_Credit = payment.Pay_Amount;

                        }
                        break;
                    default:
                        break;
                }
                if (msg == "")
                {
                    glHeader.GLDetails.Add(RevenueAccount);
                    if (payment.Total_Tax > 0)
                    {
                        glHeader.GLDetails.Add(TaxAccount);
                    }
                    glHeader.GLDetails.Add(ExpenseAccount);
                    //if (TaxAccount.GLDetail_Credit == 0 && TaxAccount.GLDetail_Credit == 0)
                    //{

                    //}
                    //else { glHeader.GLDetails.Add(TaxAccount); }
                    if (payment.Revenue_Id > 0 && payment.Supplier_ID > 0)
                    {
                        glHeader.GLDetails.Add(supplierCredit);
                        glHeader.GLDetails.Add(supplierDepit);
                    }

                        db.GLMasters.Add(glHeader);
                    GLMaster.GetBalanceForAccount(glHeader);
                    GLMaster.GetBalanceForSubLedger(glHeader);

                    db.SaveChanges();
                    msg = "تم عمل قيد الايصال بنجاح";
                    db.Dispose();
                }
                OperationRun.DeleteOperation(payment.ProjectPay_Code);
                if (payment.ProjectPay_State == TransactionState.Post)
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
        /// filter that department for that branch by using branch id
        /// </summary>
        /// <param name="Branch_Id"></param>
        /// <returns>IEnumerable<SelectListItem></returns>
        public static IEnumerable<SelectListItem> GetDepartment(int Branch_Id)
        {
            var db = new AccountingContainer();
            IEnumerable<SelectListItem> Department = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == Branch_Id && p.Subled_Level == 2), "SubLed_Id", "SubLed_Name");

            return Department;
        }
        /// <summary>
        /// filter bussinesunit belong to that department by using department id
        /// </summary>
        /// <param name="Department_Id"></param>
        /// <returns>IEnumerable<SelectListItem> </returns>
        public static IEnumerable<SelectListItem> GetBussines(int Department_Id)
        {
            var db = new AccountingContainer();
            IEnumerable<SelectListItem> Bussines = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == Department_Id), "SubLed_Id", "SubLed_Name");

            return Bussines;
        }

/// <summary>
/// delete project payment from database if status is pending and delete all file belongs to by using it id
/// </summary>
/// <param name="id"></param>
/// <returns></returns>
        public static string DeleteProjectPayment(int id)
        {
            var db = new AccountingContainer();
            ProjectPayment projectPayment = db.ProjectPayments.Find(id);

            if (projectPayment.ProjectPay_State != TransactionState.Pending)
                return "لم يتم الحذف";
            var thisfiles = db.ProjectPaymentFiles.Where(e => e.ProjectPaymentId == projectPayment.ProjectPaymentId).ToList();
            List<ProjectPaymentFile> Porjectfiles = new List<ProjectPaymentFile>();
            if (thisfiles.Count > 0)
            {
                foreach (var file in thisfiles)
                {
                    Porjectfiles.Add(file);
                }
            }
            if (Porjectfiles.Count > 0)
            {
                db.ProjectPaymentFiles.RemoveRange(Porjectfiles);
                db.SaveChanges();
            }
            db.ProjectPayments.Remove(projectPayment);

            db.SaveChanges();
            db.Dispose();
            return "تم الحذف بنجاح";
        }
        public static String deletionactiveGL(int id)
        {
            var db = new AccountingContainer();
            var doc = db.ProjectPayments.Find(id);

            if (doc.ProjectPay_State != TransactionState.Pending && doc.HasOff != true)
            {
                var vm = new ProjectPaymentVM();
                vm.ProjectPay_State = TransactionState.Void;
                vm.ProjectPaymentId = doc.ProjectPaymentId;
               UpdateProjectPayment(vm);


                var gl = db.GLMasters.Where(a => a.GLCode == doc.ProjectPay_Code);
                foreach (var g in gl)
                {
                    db.GLDetails.RemoveRange(g.GLDetails);

                    db.GLMasters.Remove(g);

                }
                db.ProjectPaymentFiles.RemoveRange(doc.ProjectPaymentFiles);
                db.ProjectPayments.Remove(doc);
                db.SaveChanges();



                db.Dispose();
                return Resources.validate.DeleteSuccessful;
            }
            return Resources.validate.Error;
        }

        public static IEnumerable<SelectListItem> GetSubledger(int Project_Id)
        {
            var db = new AccountingContainer();
            IEnumerable<SelectListItem> subledgerList = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == Project_Id), "SubLed_Id", "SubLed_Name");

            return subledgerList;
        }
        public static IEnumerable<SelectListItem> GetClause(int Project_Id)
        {
            var db = new AccountingContainer();
            var projsub = db.SubLedgers.Find(Project_Id);
            var project = db.Projects.Where(a => a.Project_Name == projsub.SubLed_Name).FirstOrDefault();
            IEnumerable<SelectListItem> ClauseList = new SelectList(db.ProjectClauses.Where(p => p.Project_Id == project.Project_Id), "Id", "Name");

            return ClauseList;
        }
        public static IEnumerable<SelectListItem> GetSubsubledger(int subledgerId)
        {
            var db = new AccountingContainer();
            if (subledgerId == 0)
            {
                return new SelectList(Enumerable.Empty<SelectListItem>());
            }
            IEnumerable<SelectListItem> SubsubledgerList = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == subledgerId), "SubLed_Id", "SubLed_Name");

            return SubsubledgerList;
        }
    }
}