using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Accountingsystem.ViewModel;

namespace Accountingsystem.Models
{
    public partial class ProjectRecipt
    {
        /// <summary>
        /// get all project Recipt in data base
        /// </summary>
        /// <returns></returns>
        public static List<ProjectRecipt> GetListOFRecipt()
        {
            var db = new AccountingContainer();
            var allList = db.ProjectRecipts.ToList();
            return allList;
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
        /// get detail of project Recipt by using its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ProjectRecipt getdetailofRecipt(int  id)
        {
            var db = new AccountingContainer();
            var thisrecipt = db.ProjectRecipts.Find(id);
            return thisrecipt;
        }
        /// <summary>
        /// get the file of project Recipt by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ProjectReceiptFile</returns>

        public static ProjectReceiptFile FindFileByID(int id)
        {
            var db = new AccountingContainer();
            var file = db.ProjectReceiptFiles.Find(id);
            return file;
        }
        /// <summary>
        /// get All File for Project Recipt by its id
        /// </summary>
        /// <param name="proId"></param>
        /// <returns>List<ProjectReceiptFile></returns>
        /// 
        public static List<ProjectReceiptFile> GetAllFiles(int proId)
        {
            var db = new AccountingContainer();
            var files = db.ProjectReceiptFiles.Where(p => p.ProjectRecipt_ID == proId).ToList();
            return files;
        }
        /// <summary>
        /// make Reipt for project by its id 
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns>ProjectReciptVM</returns>
        public static ProjectReciptVM projectReciptForCreation(int  ProjectID)
        {
            ProjectReciptVM PRVM = new ProjectReciptVM();
            var db = new AccountingContainer();
            var thisporject = db.Projects.Find(ProjectID);
            PRVM.Customer_ID = thisporject.CustomerCustomer_Id;
            PRVM.Proj_ID = ProjectID;
            var thiscustomer = db.Customers.Find(thisporject.CustomerCustomer_Id);
            PRVM.Customer_Name = thiscustomer.Customer_Name;
            List<object> newList = new List<object>();
            var members = db.PaymentMethods.Where(a=>a.IsActive!=false).ToList();
            foreach (var member in members)
                newList.Add(new
                {
                    Id = member.PaymentMethod_Id,
                    Name = member.Payment_Type + "   " + Math.Round(member.Percentage.Value,2,MidpointRounding.AwayFromZero) + "%" + " " + member.Bank_Account_Name
                });
            PRVM.PaymentMethodList = new SelectList(newList, "Id", "Name");

            PRVM.ProjectCode = thisporject.Project_Code;
            if (thisporject.DeadLine.HasValue)
            {
                PRVM.Project_End = Convert.ToDateTime(thisporject.DeadLine).Day + "-" + Convert.ToDateTime(thisporject.DeadLine).Month + "-" + Convert.ToDateTime(thisporject.DeadLine).Year;
            }
            PRVM.Project_start = Convert.ToDateTime(thisporject.StartTime).Day + "-" + Convert.ToDateTime(thisporject.StartTime).Month + "-" + Convert.ToDateTime(thisporject.StartTime).Year;
            var lastSerial = db.ProjectRecipts.OrderByDescending(c => c.ProjectRect_ID).FirstOrDefault();
            if (lastSerial == null)
            {
                PRVM.ReciptCode = "PRCT-001";
            }
            else
            {
                PRVM.ReciptCode = "PRCT-" + (Convert.ToInt32(lastSerial.ReciptCode.Substring(5, lastSerial.ReciptCode.Length - 5)) + 1).ToString("D3");
            }
            PRVM.Recipt_Date = DateTime.Now.Day+"-"+DateTime.Now.Month+"-"+DateTime.Now.Year;
            PRVM.Project_Amount = thisporject.Estimate_Amount;
            return PRVM;
        }
        /// <summary>
        /// save Project Recipt in data base and make gl for it in status post
        /// </summary>
        /// <param name="PRVM"></param>
        /// <returns></returns>
        public static int SaveRecipte(ProjectReciptVM PRVM)
        {
            var db = new AccountingContainer();
            try
            {
                ProjectRecipt proR = new ProjectRecipt();
                var poject = db.Projects.Find(PRVM.Proj_ID);
                proR.Customer_ID = poject.CustomerCustomer_Id;
                proR.Customer_Name = db.Customers.Find(proR.Customer_ID).Customer_Name;
                proR.HasOff = false;
                proR.Payment_Method = PRVM.Payment_Method;
                proR.Proj_inv_ID = PRVM.Proj_inv_ID;
                proR.Pay_Amount = PRVM.Pay_Amount;
                proR.ProjectCode = PRVM.ProjectCode;
                var lastSerial = db.ProjectRecipts.OrderByDescending(c => c.ProjectRect_ID).FirstOrDefault();
                if (lastSerial == null)
                {
                    proR.ReciptCode = "PRCT-001";
                }
                else
                {
                    proR.ReciptCode = "PRCT-" + (Convert.ToInt32(lastSerial.ReciptCode.Substring(5, lastSerial.ReciptCode.Length - 5)) + 1).ToString("D3");
                }
                proR.Project_Amount = PRVM.Project_Amount;
              
                 proR.Project_End = poject.DeadLine; 
               
                proR.Project_start = poject.StartTime;
                proR.ProjectCode = poject.Project_Code;

                proR.Recipt_Date = Convert.ToDateTime(PRVM.Recipt_Date).Date;
                if (PRVM.Recipt_Description == null)
                {
                    proR.Recipt_Description = " ";
                }
                else
                {
                    proR.Recipt_Description = PRVM.Recipt_Description;
                }

                // Used this Load File Database.
                if (PRVM.Files.FirstOrDefault() != null)
                {
                    foreach (var file in PRVM.Files)
                    {
                        var filedetails = new ProjectReceiptFile();
                        var data = new byte[file.ContentLength];
                        filedetails.FileName = file.FileName;
                        filedetails.ContentLength = file.ContentLength;
                        file.InputStream.Read(data, 0, file.ContentLength);
                        filedetails.ProImg = data;
                        filedetails.ProjectRecipt_ID = proR.ProjectRect_ID;
                        if (data.Length < 1000000)
                        {
                            proR.ProjectReceiptFiles.Add(filedetails);
                        }
                       
                    }
                }

                proR.transActionState = TransactionState.Post;
                db.ProjectRecipts.Add(proR);
                db.SaveChanges();
                var glHeader = new GLMaster();
                var GlDetail1 = new GLDetail();
                var GlDetail = new GLDetail();
                var PaymentGL = new GLDetail();
        

                // subNeeded.Estimate_Amount = subNeeded.Estimate_Amount + taxAmount;
                var paymentmethod = db.PaymentMethods.Find(proR.Payment_Method);
                glHeader.GL_Date = (DateTime)proR.Recipt_Date.Date;
                glHeader.GL_Desc = proR.Recipt_Description;
                glHeader.Company_Id = db.Companies.FirstOrDefault().Comp_Id;
                glHeader.GLType_Id = "قبض مشاريع";
                glHeader.Currency_Id = "الريال السعودي";
                glHeader.Auto = true;
                // Initialize GLState to Pending when first created
               
                glHeader.GLCode = proR.ReciptCode;
                GlDetail.Account_Id = (int)paymentmethod.Bank_Account_Id;
                GlDetail.Account_Name = db.ChartOfAccountings.Find(GlDetail.Account_Id).Account_Name;
                glHeader.GLState = TransactionState.Post;
                //var gl = db.GLMasters.Where(a => a.GLCode == glHeader.GLCode && a.GLState == glHeader.GLState).FirstOrDefault();
                //if (gl != null)
                //{
                //    return 0;
                //}


                GlDetail.Bu_Id =0;
                GlDetail.GLDetail_Credit = 0;//داءن

                var payment = db.PaymentMethods.Find(proR.Payment_Method);




                if (payment.Percentage == 0 && payment.Operatin_Cost == 0)
                {
                    GlDetail.GLDetail_Debit = proR.Pay_Amount;
                }
                else
                {


                    PaymentGL.Account_Id = payment.Expense_Account_Id.Value;
                    PaymentGL.Account_Name = db.ChartOfAccountings.Find(PaymentGL.Account_Id).Account_Name;
                    PaymentGL.GLDetail_Debit = (payment.Percentage.Value * (proR.Pay_Amount)) / 100 + payment.Operatin_Cost.Value;
                    PaymentGL.SubLedger_Id = 0;
                    GlDetail.GLDetail_Debit = proR.Pay_Amount - PaymentGL.GLDetail_Debit;


                }
                GlDetail.SubLedger_Id = 0;
                GlDetail.Asset_Id = 0;
                glHeader.GLDetails.Add(GlDetail);

                var thisaccount = db.ChartOfAccountings.Find(37);
                GlDetail1.Account_Id = (int)thisaccount.Account_Id;
                GlDetail1.Account_Name = thisaccount.Account_Name;
                GlDetail1.Bu_Id =0;
                GlDetail1.GLDetail_Credit = proR.Pay_Amount;//داءن
                GlDetail1.GLDetail_Debit = 0;//مدين
                var Customerr = db.Customers.Find(proR.Customer_ID);
                Accounts Cust = db.Accounts.Where(C => C.Account_Code == Customerr.Customer_Code).FirstOrDefault();
                GlDetail1.Account_Child_ID = Cust.Accounts_Id;
                GlDetail1.Account_CHild_Name = Cust.Account_Name;
                GlDetail1.SubLedger_Id = 0;
                GlDetail1.Asset_Id = 0;
                // Create five GlDetail
                glHeader.GLDetails.Add(GlDetail1);
                if (payment.Percentage != 0 || payment.Operatin_Cost != 0)
                {
                    glHeader.GLDetails.Add(PaymentGL);
                }
                db.GLMasters.Add(glHeader);
                GLMaster.GetBalanceForAccount(glHeader);
                GLMaster.GetBalanceForSubLedger(glHeader);
                db.SaveChanges();
                db.Dispose();
                return proR.ProjectRect_ID;
            }
            catch (Exception ex)
            {
                return 0;
            }
           
        }
        /// <summary>
        /// make gl for that Recipt and chnge status to Post
        /// </summary>
        /// <param name="ReciptID"></param>
        /// <returns>string</returns>
        public static string voidPorjectRecipt(int ReciptID)
        {
            var db = new AccountingContainer();
            var thisprojRecipt = db.ProjectRecipts.Find(ReciptID);
            OperationRun.Addnewoperation(thisprojRecipt.ReciptCode);
            var operation = db.OperationRuns.Where(a => a.Code == thisprojRecipt.ReciptCode).Count();
            if (operation != 1)
            {
                OperationRun.DeleteOperation(thisprojRecipt.ReciptCode);
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.PleaseWait;
            }
            if (thisprojRecipt.transActionState == TransactionState.Void)
            {
                OperationRun.DeleteOperation(thisprojRecipt.ReciptCode);

                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.hasbeendoneby + " " + thisprojRecipt.transActionState + " " + Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.previously;
            }
            if (thisprojRecipt.HasOff == true)

            {
               
                thisprojRecipt.transActionState = TransactionState.Post;
                OperationRun.DeleteOperation(thisprojRecipt.ReciptCode);
                db.SaveChanges();
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.UnderCloseing;

            }
            else
            {
                var glHeader = new GLMaster();
                var GlDetail1 = new GLDetail();
                var GlDetail = new GLDetail();
                var PaymentGL = new GLDetail();


                // subNeeded.Estimate_Amount = subNeeded.Estimate_Amount + taxAmount;
                var paymentmethod = db.PaymentMethods.Find(thisprojRecipt.Payment_Method);
                glHeader.GL_Date = (DateTime)thisprojRecipt.Recipt_Date.Date;
                glHeader.GL_Desc = "سند قبض مشاريع";
                glHeader.Company_Id = db.Companies.FirstOrDefault().Comp_Id;
                glHeader.GLType_Id = "سند قبض";
                glHeader.Currency_Id = "الريال السعودي";
                glHeader.Auto = true;
                // Initialize GLState to Pending when first created
                glHeader.GLState = TransactionState.Post;
                glHeader.GLCode = thisprojRecipt.ReciptCode;
                GlDetail.Account_Id = (int)paymentmethod.Bank_Account_Id;
                GlDetail.Account_Name = db.ChartOfAccountings.Find(GlDetail.Account_Id).Account_Name;
                GlDetail.Bu_Id = 0;
                GlDetail.GLDetail_Debit = 0;//داءن
                var payment = db.PaymentMethods.Find(thisprojRecipt.Payment_Method);
                glHeader.GLState = TransactionState.Void;
                var glcheck = db.GLMasters.Where(a => a.GLCode == glHeader.GLCode && a.GLState == glHeader.GLState).FirstOrDefault();
                if (glcheck != null)
                {
                    OperationRun.DeleteOperation(thisprojRecipt.ReciptCode);
                    return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                }




                if (payment.Percentage == 0 && payment.Operatin_Cost == 0)
                {
                    GlDetail.GLDetail_Credit = thisprojRecipt.Pay_Amount;
                }
                else
                {


                    PaymentGL.Account_Id = payment.Expense_Account_Id.Value;
                    PaymentGL.Account_Name = db.ChartOfAccountings.Find(PaymentGL.Account_Id).Account_Name;
                    PaymentGL.GLDetail_Credit = (payment.Percentage.Value * (thisprojRecipt.Pay_Amount)) / 100 + payment.Operatin_Cost.Value;
                    PaymentGL.SubLedger_Id = 0;
                    GlDetail.GLDetail_Credit = thisprojRecipt.Pay_Amount - PaymentGL.GLDetail_Credit;


                }
                GlDetail.SubLedger_Id = 0;
                GlDetail.Asset_Id = 0;
                glHeader.GLDetails.Add(GlDetail);

                var thisaccount = db.ChartOfAccountings.Find(37);
                GlDetail1.Account_Id = (int)thisaccount.Account_Id;
                GlDetail1.Account_Name = thisaccount.Account_Name;
                GlDetail1.Bu_Id = 0;
                GlDetail1.GLDetail_Debit = thisprojRecipt.Pay_Amount;//داءن
                GlDetail1.GLDetail_Credit = 0;//مدين
                var Customerr = db.Customers.Find(thisprojRecipt.Customer_ID);
                Accounts Cust = db.Accounts.Where(C => C.Account_Code == Customerr.Customer_Code).FirstOrDefault();
                GlDetail1.Account_Child_ID = Cust.Accounts_Id;
                GlDetail1.Account_CHild_Name = Cust.Account_Name;
                GlDetail1.SubLedger_Id = 0;
                GlDetail1.Asset_Id = 0;
                // Create five GlDetail
                glHeader.GLDetails.Add(GlDetail1);
                if (payment.Percentage != 0 || payment.Operatin_Cost != 0)
                {
                    glHeader.GLDetails.Add(PaymentGL);
                }
                db.GLMasters.Add(glHeader);
                GLMaster.GetBalanceForAccount(glHeader);
                GLMaster.GetBalanceForSubLedger(glHeader);
                db.SaveChanges();
                thisprojRecipt.transActionState = TransactionState.Void;
                db.Entry(thisprojRecipt).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                OperationRun.DeleteOperation(thisprojRecipt.ReciptCode);
                db.Dispose();
                return Resources.validate.GLReverse;
            }
            

        }
        /// <summary>
        /// fill For Edit in Recipt by its id
        /// </summary>
        /// <param name="PReciptID"></param>
        /// <returns>ProjectReciptVM</returns>
        public static ProjectReciptVM EditforCreation(int PReciptID)
        {
            var db = new AccountingContainer();
            var thisRecipt = db.ProjectRecipts.Find(PReciptID);
            ProjectReciptVM PRVM = new ProjectReciptVM();
            PRVM.Customer_ID = thisRecipt.Customer_ID;
            PRVM.Customer_Name = thisRecipt.Customer_Name;
            PRVM.Payment_Method = thisRecipt.Payment_Method;
            PRVM.Pay_Amount = thisRecipt.Pay_Amount;
            PRVM.ProjectCode = thisRecipt.ProjectCode;
            PRVM.ProjectRect_ID = thisRecipt.ProjectRect_ID;
            PRVM.Project_Amount = thisRecipt.Project_Amount;
            if (thisRecipt.Project_End != null)
            {
                PRVM.Project_End = Convert.ToDateTime(thisRecipt.Project_End).Day + "-" + Convert.ToDateTime(thisRecipt.Project_End).Month + "-" + Convert.ToDateTime(thisRecipt.Project_End).Year;
            }
           
            PRVM.Project_start = thisRecipt.Project_start.Day + "-" + thisRecipt.Project_start.Month + "-" + thisRecipt.Project_start.Year;
            PRVM.Proj_inv_ID = 0;
            PRVM.ReciptCode = thisRecipt.ReciptCode;
            PRVM.Recipt_Date = thisRecipt.Recipt_Date.Day + "-" + thisRecipt.Recipt_Date.Month + "-" + thisRecipt.Recipt_Date.Year;
            PRVM.Recipt_Description = thisRecipt.Recipt_Description;
            PRVM.PaymentMethodList = new SelectList(db.PaymentMethods.Where(a=>a.IsActive!=false), "PaymentMethod_Id", "Payment_Type");
           
            return PRVM;
        }
        /// <summary>
        /// post the Edit for that Recipt in data base
        /// </summary>
        /// <param name="PRVM"></param>
        /// <returns></returns>
        public static string EditProjectRecipt(ProjectReciptVM PRVM)
        {
            var db = new AccountingContainer();
            try
            {
                var thisproRecipt = db.ProjectRecipts.Find(PRVM.ProjectRect_ID);
                thisproRecipt.Customer_ID = PRVM.Customer_ID;
                thisproRecipt.Customer_Name = PRVM.Customer_Name;
                thisproRecipt.Payment_Method = PRVM.Payment_Method;
                thisproRecipt.Pay_Amount = PRVM.Pay_Amount;
                thisproRecipt.Project_Amount = PRVM.Project_Amount;
                thisproRecipt.Project_End = Convert.ToDateTime(PRVM.Project_End).Date;
                thisproRecipt.Project_start = Convert.ToDateTime(PRVM.Project_start).Date;
                thisproRecipt.Proj_inv_ID =0;
                thisproRecipt.Recipt_Date = Convert.ToDateTime(PRVM.Recipt_Date).Date;
                thisproRecipt.Recipt_Description = PRVM.Recipt_Description;
                // Used this Load File Database.
                if (PRVM.Files.FirstOrDefault() != null)
                {
                    foreach (var file in PRVM.Files)
                    {
                        var filedetails = new ProjectReceiptFile();
                        var data = new byte[file.ContentLength];
                        filedetails.FileName = file.FileName;
                        filedetails.ContentLength = file.ContentLength;
                        file.InputStream.Read(data, 0, file.ContentLength);
                        filedetails.ProImg = data;
                        filedetails.ProjectRecipt_ID = PRVM.ProjectRect_ID;
                        if (data.Length < 1000000)
                        {
                            thisproRecipt.ProjectReceiptFiles.Add(filedetails);
                        }

                      
                    }
                }

                db.Entry(thisproRecipt).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                return "تم تعديل سند قبض المشروع";
            }
            catch (Exception ex)
            {
                return "لم يتم تعديل سند قبض المشروع";
            }
        }
        /// <summary>
        /// get Project invoice by its id
        /// </summary>
        /// <param name="porjectinvoiceID"></param>
        /// <returns></returns>
        public static ProjectInvoiceMaster getprojectInvoice(int porjectinvoiceID)
        {
            var db = new AccountingContainer();
            var thisprojectinv = db.ProjectInvoiceMasters.Find(porjectinvoiceID);
            return thisprojectinv;
        }
        /// <summary>
        /// delete the Recipt by its id
        /// </summary>
        /// <param name="recID"></param>
        /// <returns>string</returns>
        public static string Deletethisrec(int  recID)
        {
            var db = new AccountingContainer();
            var thisrec = db.ProjectRecipts.Find(recID);
            db.ProjectReceiptFiles.RemoveRange(thisrec.ProjectReceiptFiles);
            db.ProjectRecipts.Remove(thisrec);
            db.SaveChanges();
            db.Dispose();
            return "تم حذف سند قبض المشروع";
        }
        public static String deletionactiveGL(int id)
        {
            var db = new AccountingContainer();
            var doc = db.ProjectRecipts.Find(id);

            if (doc.transActionState != TransactionState.Pending && doc.HasOff != true)
            {

                voidPorjectRecipt(doc.ProjectRect_ID);


                   var gl = db.GLMasters.Where(a => a.GLCode == doc.ReciptCode);
                foreach (var g in gl)
                {
                    db.GLDetails.RemoveRange(g.GLDetails);

                    db.GLMasters.Remove(g);

                }
                db.ProjectReceiptFiles.RemoveRange(doc.ProjectReceiptFiles);
                db.ProjectRecipts.Remove(doc);
                db.SaveChanges();



                db.Dispose();
                return Resources.validate.DeleteSuccessful;
            }
            return Resources.validate.Error;
        }
        public static decimal getAllPayForthatInvoicebyCode(string InvoiceCode)
        {
            var db = new AccountingContainer();
            var invoice = db.ProjectInvoiceMasters.Where(a => a.Serial_Number == InvoiceCode).FirstOrDefault();
            var Recipt = db.ProjectRecipts.Where(a => a.Proj_inv_ID == invoice.PInvoiceMaster_ID && a.transActionState == TransactionState.Post).Select(a => a.Pay_Amount).ToList().Sum();
            return Recipt;
        }

    }
}