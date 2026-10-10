using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Accountingsystem.ViewModel;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Configuration;
using System.Data.SqlClient;

namespace Accountingsystem.Models
{
    public partial class Subscription
    {
        /// <summary>
        /// Check if the code is exsit before or not
        /// </summary>
        /// <param name="Serial"></param>
        /// <returns></returns>
        public static bool containCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.Subscriptions.ToList();
            foreach (var item in allinData)
            {
                if (item.Subscrip_Code == Serial || !Serial.StartsWith("SUBS-"))
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
        /// Check if the code is exsit before or not
        /// </summary>
        /// <param name="Serial"></param>
        /// <returns></returns>
        public static bool containCodes(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.SubscriptionInvoices.ToList();
            foreach (var item in allinData)
            {
                if (item.Serial_Number == Serial || !Serial.StartsWith("SUBINV-"))
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
        /// Get all subscription in state pending
        /// </summary>
        /// <returns></returns>
        public static List<Subscription> GetAllSubscriptionPending()
        {
            var db = new AccountingContainer();
            var supscrip = db.Subscriptions.Where(s => s.Subscrip_Stop == false && s.Subscrip_State == TransactionState.Pending && s.IsCanceled == false).ToList();
            return supscrip;
        }
        /// <summary>
        /// Get all subscription in state void
        /// </summary>
        /// <returns></returns>
        public static List<Subscription> GetAllSubscriptionVoid()
        {
            var db = new AccountingContainer();
            var supscrip = db.Subscriptions.Where(s => s.Subscrip_Stop == false && s.Subscrip_State == TransactionState.Void && s.IsCanceled == false).ToList();
            return supscrip;
        }

        /// <summary>
        /// Get all subscription in state post
        /// </summary>
        /// <returns></returns>
        public static List<Subscription> GetAllSubscription()
        {
            var db = new AccountingContainer();
            var supscrip = db.Subscriptions.Where(s => s.Subscrip_Stop == false &&s.Subscrip_State==TransactionState.Post && s.SubType_State != SubType_State.منتهى && s.IsCanceled == false).ToList();
            return supscrip;
        }

        public static string GetEndDateSub(string date,int typeSub)
        {
            var db = new AccountingContainer();
            var type = db.Subscription_Type.Find(typeSub);
            var start = Convert.ToDateTime(date);
            var end = start.AddMonths(type.Durationsubscription);
            return end.ToString("dd-MM-yyyy   HH:mm:ss");
        }
        /// <summary>
        /// Get all subscription in state finished
        /// </summary>
        /// <returns></returns>
        public static List<Subscription> GetAllSubscriptionFinshed()
        {
            var db = new AccountingContainer();
            var supscrip = db.Subscriptions.Where(s => s.Subscrip_Stop == false && s.Subscrip_State == TransactionState.Post && s.SubType_State == SubType_State.منتهى && s.IsCanceled == false).ToList();
            return supscrip;
        }
        /// <summary>
        /// Cancel subscription by subscribe id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string MakeSubscriptionIsCanceled(int id)
        {
            var db = new AccountingContainer();
            var subscription = db.Subscriptions.Find(id);
            var subscriptionTemp = db.SubscriptionTemps.Where(st => st.Subscrip_Id == id).FirstOrDefault();
            subscription.IsCanceled = true;
            if (subscriptionTemp != null)
            {
                subscriptionTemp.IsCanceled = true;
                db.Entry(subscriptionTemp).State = EntityState.Modified;
            }
           
            db.Entry(subscription).State = EntityState.Modified;
         
            db.SaveChanges();
            db.Dispose();
            return Resources.validate.Subcancel;
        }
        /// <summary>
        /// Get all subscription in state canceled
        /// </summary>
        /// <returns></returns>
        public static List<Subscription> GetSubscriptionsIsCanceled()
        {
            var db = new AccountingContainer();
            var supscrip = db.Subscriptions.Where(s => s.IsCanceled == true).ToList();
            return supscrip;
        }
        /// <summary>
        /// Get subscription details by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Subscription GetAllSubscriptionById(int id)
        {
            var db = new AccountingContainer();
            var subscription = db.Subscriptions.Find(id);
            return subscription;
        }
        public static Subscription_Type GetAllSubscriptionBySerial(string serial,int qr)
        {
            var db = new AccountingContainer();
            var subscription = db.Subscriptions.Where(a=>a.Subscrip_Code==serial||a.QRNumber== qr).FirstOrDefault();

            return GetAllTypeInfo(subscription.Sub_Type_ID);
        }
        public static Subscription GetAllInvoiceQR( int qr)
        {
            var db = new AccountingContainer();
            var subscription = db.Subscriptions.Where(a =>  a.QRNumber == qr).FirstOrDefault();

            return subscription;
        }
        /// <summary>
        /// Get all types of subscription
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Subscription_Type GetAllTypeInfo(int id)
        {
            var db = new AccountingContainer();
            var thistype = db.Subscription_Type.Find(id);
            return thistype;
        }

        /// <summary>
        /// Get for creation subscription
        /// </summary>
        /// <returns></returns>
        public static SubscriptionVM CreateVMFoCreation(string userId )
        {
            var db = new AccountingContainer();
            var supVM = new SubscriptionVM();

            supVM.MyCusromerList = new SelectList(db.Customers.Where(e => e.Customer_State == Customer_State.نشط).ToList(), "Customer_Id", "Customer_Name");
            var thischartaxParent = db.ChartOfAccountings.Find(48);
            supVM.tax_List = new SelectList(db.Taxes.Where(e => e.Tax_Code.Contains(thischartaxParent.Account_Code)).Where(a => a.IsActive != false).ToList(), "Tax_ID", "Tax_Name");
            // supVM.StartTime = DateTime.Now.ToString("dd-MM-yyyy   HH:mm:ss");
            supVM.SubType_State = SubType_State.مشترك;
            supVM.MyEmpTeam = new SelectList(db.EmployeeTeams, "Team_ID", "Team_Name");
            supVM.MySubTypeList = new SelectList(db.Subscription_Type, "Subscription_Type_ID", "Subscrip_Name");
            supVM.MyPayMethod = new SelectList(db.PaymentMethods.Where(a=>a.IsActive!=false), "PaymentMethod_Id", "Payment_Type");

            supVM.EmpList = new SelectList(db.Employees.Where(e => e.Reg_ID != null), "Emp_Id", "Emp_Name");

            supVM.MyCompList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");

            supVM.MySubscripPranch = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.isActive != false && (a.SubLed_Type == "فرع " || a.SubLed_Type == "قسم رئيسي للشركه")), "SubLed_Id", "SubLed_Name");
            supVM.MyDepartmentList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.isActive != false && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
            supVM.MyBuList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.isActive != false && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            
            supVM.MySubList = new SelectList(db.SubLedgers.Where(a => a.SubLed_Type == "مركز تكلفة يدوى"&& a.isActive != false), "SubLed_Id", "SubLed_Name");
            //var supcs = new SubscriptionVM();




            var lastSerial = db.Subscriptions.ToList().LastOrDefault();
            if (lastSerial == null)
            {
                supVM.Subscrip_Code = "SUBS-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                supVM.Subscrip_Code = "SUBS-" + (Convert.ToInt32(lastSerial.Subscrip_Code.Substring(5, lastSerial.Subscrip_Code.Length - 5)) + 1).ToString("D3");
            }
            var emp = db.Employees.Where(a => a.Reg_ID == userId).FirstOrDefault();
            if (emp != null)
            {
                supVM.Emp_Id = emp.Emp_Id;
            }
            return supVM;
        }
        
        /// <summary>
        /// Create a new subsceiption 
        /// </summary>
        /// <param name="SupscripeVM"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static Subscription createNewSubscription(SubscriptionVM SupscripeVM , String UserId)
        {
            var db = new AccountingContainer();
            var thissubscription = new Subscription();
            if (SupscripeVM.Subscrip_Id > 0)
            {
                thissubscription = db.Subscriptions.Find(SupscripeVM.Subscrip_Id);
                
            }
            var NewSubscription = new Subscription();
            try
            {
                if (SupscripeVM != null)
                {

                    Tax resulttaxe = null;



                    if (SupscripeVM.Taxe_ID != null && SupscripeVM.Taxe_ID != 0)
                    {
                        NewSubscription.Tax_ID = SupscripeVM.Taxe_ID.Value;
                        resulttaxe = db.Taxes.Find(SupscripeVM.Taxe_ID);
                    }
                    else
                    {
                        NewSubscription.Tax_ID = 0;
                    }
                    var resultCompany = db.Companies.Find(SupscripeVM.Comp_Id);
                    if (resultCompany != null)
                    {
                        NewSubscription.Subscrip_Comp = resultCompany.Comp_Id;
                    }

                    var newDate = Convert.ToDateTime(SupscripeVM.Sub_Date);

                    if (SupscripeVM.Sub_Date == null)
                    {
                        NewSubscription.Sub_Date = DateTime.Now;
                    }
                    else
                    {
                        if (newDate < resultCompany.StartFinancialDate)
                        {
                            NewSubscription.Sub_Date = DateTime.Now;
                        }
                        else
                        {
                            NewSubscription.Sub_Date = Convert.ToDateTime(SupscripeVM.Sub_Date);
                        }
                    }

                    var resulCustomer = db.Customers.Find(SupscripeVM.Customer_Id);
                    if (resulCustomer != null)
                    {

                        NewSubscription.Customer_Id = resulCustomer.Customer_Id;
                        NewSubscription.Customer_Name = resulCustomer.Customer_Name;
                    }


                    var resulEmp_team = db.EmployeeTeams.Find(SupscripeVM.SelectEmp_Team);
                    if (resulEmp_team != null)
                    {
                        NewSubscription.Emp_Team = resulEmp_team.Team_ID;
                    }
                    var resultType = db.Subscription_Type.Find(SupscripeVM.SelectedSubscrip_Type);
                    if (resultType != null)
                    {
                        NewSubscription.Subscrip_Type = resultType.Durationsubscription;
                        NewSubscription.Subscrip_Description = resultType.Subscrip_Name;
                        NewSubscription.Subscrip_Comp = resultType.Subscrip_Company;
                        NewSubscription.Sub_Type_ID = resultType.Subscription_Type_ID;

                        if (resultType.Subscrip_BU == null || resultType.Subscrip_BU == 0)
                        {
                            NewSubscription.Subscrip_BU = 0;
                        }
                        else
                        {
                            NewSubscription.Subscrip_BU = (int)resultType.Subscrip_BU;
                        }
                    }
                    var resultMethod = db.PaymentMethods.Find(SupscripeVM.SelecltPayment_Method);
                    if (resultMethod != null)
                    {
                        NewSubscription.Payment_Method = resultMethod.PaymentMethod_Id;
                    }
                    var resulEmployee = db.Employees.Find(SupscripeVM.Emp_Id);
                    if (resulEmployee != null)
                    {
                        NewSubscription.Emp_Id = resulEmployee.Emp_Id;                   
                    }
                    var subResult = db.SubLedgers.Find(SupscripeVM.Subledger_ID);
                    if (subResult != null)
                    {
                        NewSubscription.Subledger_ID = subResult.SubLed_Id;
                    }
                    else
                    {
                        if (SupscripeVM.Subscrip_BU != null && SupscripeVM.Subscrip_BU != 0)
                        {
                            NewSubscription.Subledger_ID = (int)SupscripeVM.Subscrip_BU;
                            NewSubscription.Subscrip_BU = SupscripeVM.Subscrip_BU;
                        }
                        else if ((SupscripeVM.Subscrip_BU == null || SupscripeVM.Subscrip_BU == 0) && (SupscripeVM.Department_Id != null && SupscripeVM.Department_Id != 0))
                        {
                            NewSubscription.Subledger_ID = (int)SupscripeVM.Department_Id;
                            NewSubscription.Department_Id = SupscripeVM.Department_Id;
                        }
                        else if ((SupscripeVM.Subscrip_BU == null || SupscripeVM.Subscrip_BU == 0) && (SupscripeVM.Department_Id == null || SupscripeVM.Department_Id == 0) && (SupscripeVM.Subscrip_Pranch != null && SupscripeVM.Subscrip_Pranch != 0))
                        {
                            NewSubscription.Subledger_ID = (int)SupscripeVM.Subscrip_Pranch;
                            NewSubscription.Subscrip_Pranch = SupscripeVM.Subscrip_Pranch;
                        }
                        else
                        {
                            var thissub = db.SubLedgers.Where(e => e.Subled_Level == 0).FirstOrDefault();
                            NewSubscription.Subledger_ID = (int)thissub.SubLed_Id;
                        }
                    }
                    NewSubscription.Subscrip_BU = SupscripeVM.Subscrip_BU;
                    NewSubscription.Department_Id = SupscripeVM.Department_Id;
                    NewSubscription.Subscrip_Pranch = SupscripeVM.Subscrip_Pranch;
               
         
                    NewSubscription.IsCanceled = false;
                    NewSubscription.Subscrip_State = TransactionState.Pending;
                    // NewSubscription.Subscrip_Type = SupscripeVM.Subscrip_Type;
                  
                    NewSubscription.tel_customer = SupscripeVM.tel_customer;
                    NewSubscription.Check_Number = SupscripeVM.Check_Number;
                    NewSubscription.Discount = SupscripeVM.Discount;
                    NewSubscription.DiscountAllow = SupscripeVM.AllowDiscount;
                    var lastSerial = db.Subscriptions.OrderByDescending(c => c.Subscrip_Id).FirstOrDefault();
                    if (lastSerial == null)
                    {
                        NewSubscription.Subscrip_Code = "SUBS-001";
                    }
                    else
                    {
                        //using string substring method to get the number of the last inserted
                        NewSubscription.Subscrip_Code = "SUBS-" + (Convert.ToInt32(lastSerial.Subscrip_Code.Substring(5, lastSerial.Subscrip_Code.Length - 5)) + 1).ToString("D3");
                        thissubscription.NewSubCode = NewSubscription.Subscrip_Code;
                    }
                    NewSubscription.StartTime = Convert.ToDateTime(SupscripeVM.StartTime);
                    NewSubscription.Estimate_Amount = SupscripeVM.Estimate_Amount;
                    NewSubscription.DeadLine = Convert.ToDateTime(SupscripeVM.DeadLine);
                    
                    NewSubscription.SubType_State = SubType_State.مشترك;
                    NewSubscription.Sub_Pay_State = Models.Sub_Pay_State.لم_يتم_الدفع;
                    var TotalDiscount = (NewSubscription.Estimate_Amount * (NewSubscription.Discount / 100));
                    var TotalAfterDiscount = NewSubscription.Estimate_Amount - TotalDiscount;
                    if (NewSubscription.Tax_ID == 0)
                    {
                        if (NewSubscription.DiscountAllow != null && NewSubscription.DiscountAllow > 0)
                        {
                            NewSubscription.AmountAfterTax = TotalAfterDiscount.Value - (NewSubscription.DiscountAllow.Value);
                        }
                        else
                        {
                            NewSubscription.AmountAfterTax = TotalAfterDiscount.Value;
                        }
                         }
                    else
                    { NewSubscription.AmountAfterTax = (TotalAfterDiscount.Value + (TotalAfterDiscount.Value * (resulttaxe.Tax_Percentage / 100)) - (NewSubscription.DiscountAllow.Value)); }

                    //var mm = db.Subscription_Type.Find(SupscripeVM.SelectedSubscrip_Type);

                    NewSubscription.DeadLine = Convert.ToDateTime(SupscripeVM.DeadLine);
                    NewSubscription.Payed_Amount = 0;
                    NewSubscription.Subscrip_Description = SupscripeVM.Subscrip_Description;
                    /// qr Invoice
                    // /
                     var qrMaster = new MasterQRInvoice();
                    qrMaster.QrGuid = Guid.NewGuid();
                    qrMaster.AllowDiscount = NewSubscription.DiscountAllow.Value;
                    qrMaster.ApprovalCode = "";
                    qrMaster.Company_Id = db.Companies.ToList().FirstOrDefault().Comp_Id;
                    qrMaster.Customer_Id = NewSubscription.Customer_Id;
                    qrMaster.Description = NewSubscription.Subscrip_Description;
                    if (NewSubscription.Emp_Id != null)
                    {
                        qrMaster.Employee_Id = NewSubscription.Emp_Id.Value;
                    }
                    else
                    {
                        qrMaster.Employee_Id = db.Employees.Where(a => a.Reg_ID != null).FirstOrDefault().Emp_Id;
                    }
                    if ( NewSubscription.DiscountAllow== null)
                    {
                        NewSubscription.DiscountAllow = 0;
                    }
                    if (NewSubscription.Discount == null)
                    {
                        NewSubscription.Discount = 0;
                    }
                    qrMaster.Invoice_Date = NewSubscription.Sub_Date;
                    qrMaster.Invoice_State = NewSubscription.Subscrip_State;
                    qrMaster.InvoiceType_Id = 5;
                    qrMaster.Master_Discount = NewSubscription.Discount.Value;
                    qrMaster.TotalBeforeDiscount = NewSubscription.Estimate_Amount;
                    qrMaster.Reference = "";
                    qrMaster.Serial_Number = NewSubscription.Subscrip_Code;

                    qrMaster.Subledger_Id = NewSubscription.Subledger_ID;
                    qrMaster.Notes = "";
                    if (resulttaxe != null)
                    {
                        qrMaster.TotalNewTaxs = (TotalAfterDiscount.Value * (resulttaxe.Tax_Percentage / 100));
                    }

                    qrMaster.Total_Amount = NewSubscription.AmountAfterTax+NewSubscription.DiscountAllow.Value;
                    qrMaster.InvoiceType = "فاتورة مبيعات اشتراك";
                    qrMaster.Total_Payment_Method_Cost = 0;
                    qrMaster.Total_Tax = 0;
                    qrMaster.Branch_Id = NewSubscription.Subscrip_Pranch;
                    qrMaster.Department_ID = NewSubscription.Department_Id;
                    qrMaster.BussinesUnit_ID = NewSubscription.Subscrip_BU;
                    qrMaster.StartDate = NewSubscription.StartTime;
                    qrMaster.EndDate = NewSubscription.DeadLine;

                    qrMaster.Total_Cost = 0;
                    qrMaster.Notes = "";
                    qrMaster.SubScribtionType_Id = NewSubscription.Sub_Type_ID;
                    qrMaster.TaxQRInvoices = new List<TaxQRInvoice>();
                    if (NewSubscription.Tax_ID > 0)
                    {
                        var taxQr = new TaxQRInvoice();
                        taxQr.Tax_Id = resulttaxe.Tax_ID.ToString();
                        taxQr.Amount = (TotalAfterDiscount.Value * (resulttaxe.Tax_Percentage / 100));
                        qrMaster.TaxQRInvoices.Add(taxQr);
                        qrMaster.TotalNewTaxs = (TotalAfterDiscount.Value * (resulttaxe.Tax_Percentage / 100));
                    }
                    qrMaster.Number = InvoiceSerial.generateSerialForInvoice();
                    qrMaster.DateOfCreate = DateTime.Now;
                    NewSubscription.Number = qrMaster.Number;
                    db.MasterQRInvoices.Add(qrMaster);
                    db.SaveChanges();
                    NewSubscription.QRNumber = qrMaster.Id;
                    db.Subscriptions.Add(NewSubscription);
                   
                    if (thissubscription.Subscrip_Code!=null) {

                        db.Entry(thissubscription).State = EntityState.Modified;
                    }
                       
                    
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

                    var traBatch = new TransactionBatch();

                    traBatch.OperationName = "انشاء اشتراك";
                    traBatch.Description = SupscripeVM.Subscrip_Description;
                    traBatch.Date = Convert.ToDateTime(SupscripeVM.StartTime);
                    traBatch.UserId = UserId;
                    traBatch.OperationId = NewSubscription.Subscrip_Id;
                    traBatch.Operation_Code = NewSubscription.Subscrip_Code;
                    TransactionBatch.AddNewTransaction(traBatch);

                    return NewSubscription;
                }



            }
            catch (Exception s)
            {
                var Msg = s.Message;
                var msg = s.Message;
                var result = s.Data;
                var tt = result;
                throw;
            }
            return NewSubscription;

        }


        public static Subscription EditPenddingSubscription(SubscriptionVM SupscripeVM, String UserId)
        {
            var db = new AccountingContainer();
       
          
               var thissubscription = db.Subscriptions.Find(SupscripeVM.Subscrip_Id);

            
        
            try
            {
                if (SupscripeVM != null)
                {

                    Tax resulttaxe = null;

                    thissubscription.Emp_Id = SupscripeVM.Emp_Id;
                    thissubscription.Emp_Team = SupscripeVM.SelectEmp_Team;
                    if (SupscripeVM.Taxe_ID != null && SupscripeVM.Taxe_ID != 0)
                    {
                        thissubscription.Tax_ID = SupscripeVM.Taxe_ID.Value;
                        resulttaxe = db.Taxes.Find(SupscripeVM.Taxe_ID);
                    }
                    else
                    {
                        thissubscription.Tax_ID = 0;
                    }
                    var resultCompany = db.Companies.Find(SupscripeVM.Comp_Id);
                    if (resultCompany != null)
                    {
                        thissubscription.Subscrip_Comp = resultCompany.Comp_Id;
                    }

                    var newDate = Convert.ToDateTime(SupscripeVM.Sub_Date);

                    if (SupscripeVM.Sub_Date == null)
                    {
                        thissubscription.Sub_Date = DateTime.Now;
                    }
                    else
                    {
                        if (newDate < resultCompany.StartFinancialDate)
                        {
                            thissubscription.Sub_Date = DateTime.Now;
                        }
                        else
                        {
                            thissubscription.Sub_Date = Convert.ToDateTime(SupscripeVM.Sub_Date);
                        }
                    }

                    var resulCustomer = db.Customers.Find(SupscripeVM.Customer_Id);
                    if (resulCustomer != null)
                    {

                        thissubscription.Customer_Id = resulCustomer.Customer_Id;
                        thissubscription.Customer_Name = resulCustomer.Customer_Name;
                    }


                    var resulEmp_team = db.EmployeeTeams.Find(SupscripeVM.SelectEmp_Team);
                    if (resulEmp_team != null)
                    {
                        thissubscription.Emp_Team = resulEmp_team.Team_ID;
                    }
                    var resultType = db.Subscription_Type.Find(SupscripeVM.SelectedSubscrip_Type);
                    if (resultType != null)
                    {
                        thissubscription.Subscrip_Type = resultType.Durationsubscription;
                        thissubscription.Subscrip_Description = resultType.Subscrip_Name;
                        thissubscription.Subscrip_Comp = resultType.Subscrip_Company;
                        thissubscription.Sub_Type_ID = resultType.Subscription_Type_ID;

                        if (resultType.Subscrip_BU == null || resultType.Subscrip_BU == 0)
                        {
                            thissubscription.Subscrip_BU = 0;
                        }
                        else
                        {
                            thissubscription.Subscrip_BU = (int)resultType.Subscrip_BU;
                        }
                    }
                    var resultMethod = db.PaymentMethods.Find(SupscripeVM.SelecltPayment_Method);
                    if (resultMethod != null)
                    {
                        thissubscription.Payment_Method = resultMethod.PaymentMethod_Id;
                    }
                    var resulEmployee = db.Employees.Find(SupscripeVM.Emp_Id);
                    if (resulEmployee != null)
                    {
                        thissubscription.Emp_Id = resulEmployee.Emp_Id;
                    }
                    var subResult = db.SubLedgers.Find(SupscripeVM.Subledger_ID);
                    if (subResult != null)
                    {
                        thissubscription.Subledger_ID = subResult.SubLed_Id;
                    }
                    else
                    {
                        if (SupscripeVM.Subscrip_BU != null && SupscripeVM.Subscrip_BU != 0)
                        {
                            thissubscription.Subledger_ID = (int)SupscripeVM.Subscrip_BU;
                            thissubscription.Subscrip_BU = SupscripeVM.Subscrip_BU;
                        }
                        else if ((SupscripeVM.Subscrip_BU == null || SupscripeVM.Subscrip_BU == 0) && (SupscripeVM.Department_Id != null && SupscripeVM.Department_Id != 0))
                        {
                            thissubscription.Subledger_ID = (int)SupscripeVM.Department_Id;
                            thissubscription.Department_Id = SupscripeVM.Department_Id;
                        }
                        else if ((SupscripeVM.Subscrip_BU == null || SupscripeVM.Subscrip_BU == 0) && (SupscripeVM.Department_Id == null || SupscripeVM.Department_Id == 0) && (SupscripeVM.Subscrip_Pranch != null && SupscripeVM.Subscrip_Pranch != 0))
                        {
                            thissubscription.Subledger_ID = (int)SupscripeVM.Subscrip_Pranch;
                            thissubscription.Subscrip_Pranch = SupscripeVM.Subscrip_Pranch;
                        }
                        else
                        {
                            var thissub = db.SubLedgers.Where(e => e.Subled_Level == 0).FirstOrDefault();
                            thissubscription.Subledger_ID = (int)thissub.SubLed_Id;
                        }
                    }
                    thissubscription.Subscrip_BU = SupscripeVM.Subscrip_BU;
                    thissubscription.Department_Id = SupscripeVM.Department_Id;
                    thissubscription.Subscrip_Pranch = SupscripeVM.Subscrip_Pranch;


                    thissubscription.IsCanceled = false;
                    thissubscription.Subscrip_State = TransactionState.Pending;
                    // NewSubscription.Subscrip_Type = SupscripeVM.Subscrip_Type;

                    thissubscription.tel_customer = SupscripeVM.tel_customer;
                    thissubscription.Check_Number = SupscripeVM.Check_Number;
                    thissubscription.Discount = SupscripeVM.Discount;
                    thissubscription.DiscountAllow = SupscripeVM.AllowDiscount;

                    thissubscription.StartTime = Convert.ToDateTime(SupscripeVM.StartTime);
                    thissubscription.Estimate_Amount = SupscripeVM.Estimate_Amount;
                    thissubscription.DeadLine = Convert.ToDateTime(SupscripeVM.DeadLine);

                    thissubscription.SubType_State = SubType_State.مشترك;
                    thissubscription.Sub_Pay_State = Models.Sub_Pay_State.لم_يتم_الدفع;
                    var TotalDiscount = (thissubscription.Estimate_Amount * (thissubscription.Discount / 100));
                    var TotalAfterDiscount = thissubscription.Estimate_Amount - TotalDiscount;
                    if (thissubscription.Tax_ID == 0)
                    {
                        if (thissubscription.DiscountAllow != null && thissubscription.DiscountAllow > 0)
                        {
                            thissubscription.AmountAfterTax = TotalAfterDiscount.Value - (thissubscription.DiscountAllow.Value);
                        }
                        else
                        {
                            thissubscription.AmountAfterTax = TotalAfterDiscount.Value;
                        }
                    }
                    else
                    { thissubscription.AmountAfterTax = (TotalAfterDiscount.Value + (TotalAfterDiscount.Value * (resulttaxe.Tax_Percentage / 100)) - (thissubscription.DiscountAllow.Value)); }

                    //var mm = db.Subscription_Type.Find(SupscripeVM.SelectedSubscrip_Type);

                    thissubscription.DeadLine = Convert.ToDateTime(SupscripeVM.DeadLine);
                    thissubscription.Payed_Amount = 0;
                    thissubscription.Subscrip_Description = SupscripeVM.Subscrip_Description;
                    //NewSubscription.DeadLine = DateTime.Now.AddMonths(SupscripeVM.SelectedSubscrip_Type);
                    //Subscript.DeadLine = DateTime.Now.AddMonths(subscVM.Durationsubscription);
                    db.Subscriptions.Add(thissubscription);
                  

                        db.Entry(thissubscription).State = EntityState.Modified;
                    


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

                    var traBatch = new TransactionBatch();

                    traBatch.OperationName = "تعديل اشتراك";
                    traBatch.Description = SupscripeVM.Subscrip_Description;
                    traBatch.Date = Convert.ToDateTime(SupscripeVM.StartTime);
                    traBatch.UserId = UserId;
                    traBatch.OperationId = thissubscription.Subscrip_Id;
                    traBatch.Operation_Code = thissubscription.Subscrip_Code;
                    TransactionBatch.AddNewTransaction(traBatch);

                    return thissubscription;
                }



            }
            catch (Exception s)
            {
                var Msg = s.Message;
                var msg = s.Message;
                var result = s.Data;
                var tt = result;
                throw;
            }
            return thissubscription;

        }

        /// <summary>
        /// Create a gl for subscription by id
        /// </summary>
        /// <param name="SubID"></param>
        /// <returns></returns>
        public static string PostSubscription(int SubID)
        {
            var db = new AccountingContainer();
            var NewSubscription = db.Subscriptions.Find(SubID);
            OperationRun.Addnewoperation(NewSubscription.Subscrip_Code);
            var operation = db.OperationRuns.Where(a => a.Code == NewSubscription.Subscrip_Code).Count();
            if (operation != 1)
            {
                OperationRun.DeleteOperation(NewSubscription.Subscrip_Code);
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.PleaseWait;
            }
            if (NewSubscription.Subscrip_State == TransactionState.Post)
            {
                OperationRun.DeleteOperation(NewSubscription.Subscrip_Code);

                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.hasbeendoneby + " " + NewSubscription.Subscrip_State + " " + Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.previously;
            }
            else
            {
                NewSubscription.Subscrip_State = TransactionState.Post;
                var TotalDiscount = (NewSubscription.Estimate_Amount * (NewSubscription.Discount / 100));
                Tax resulttaxe = null;

                if (NewSubscription.Tax_ID != 0)
                {
                    NewSubscription.Tax_ID = NewSubscription.Tax_ID;
                    resulttaxe = db.Taxes.Find(NewSubscription.Tax_ID);
                }
                var TotalAfterDiscount = NewSubscription.Estimate_Amount - TotalDiscount;
                var glHeader = new GLMaster();
                var RevenueGL = new GLDetail();
                var CusGL = new GLDetail();
                var GlDetaitax = new GLDetail();
                var Discount = new GLDetail();
                var AllowDiscount = new GLDetail();

                var paymentmethod = db.PaymentMethods.Find(NewSubscription.Payment_Method);
                glHeader.GL_Date = NewSubscription.Sub_Date.Value;
                glHeader.GL_Desc = db.Subscription_Type.Find( NewSubscription.Sub_Type_ID).Subscrip_Name;
                glHeader.Company_Id = Convert.ToInt32(NewSubscription.Subscrip_Comp);
                glHeader.GLType_Id = "مبيعات اشتراك";
                glHeader.Currency_Id =  "الريال السعودي";
                glHeader.Auto = true;

                var thiscust = db.Customers.Find(NewSubscription.Customer_Id);
                var subled = db.Accounts.Where(e => e.Account_Code == thiscust.Customer_Code).ToList().FirstOrDefault();


                // Initialize GLState to Pending when first created
               

                glHeader.GLCode = NewSubscription.Subscrip_Code;
                glHeader.GLState = TransactionState.Post;
                var gl = db.GLMasters.Where(a => a.GLCode == glHeader.GLCode && a.GLState == glHeader.GLState).FirstOrDefault();
                if (gl != null)
                {
                    OperationRun.DeleteOperation(NewSubscription.Subscrip_Code);
                    return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                }
                var thisaccount = db.ChartOfAccountings.Find(37);
                CusGL.Account_Id = thisaccount.Account_Id;
                CusGL.Account_Name = thisaccount.Account_Name;


                CusGL.Bu_Id = Convert.ToInt32(NewSubscription.Subscrip_BU);
                CusGL.GLDetail_Credit = 0; //داءن

                CusGL.GLDetail_Debit = NewSubscription.AmountAfterTax;


                CusGL.SubLedger_Id = 0;
                CusGL.Account_Child_ID = subled.Accounts_Id;
                CusGL.Account_CHild_Name = db.Accounts.Find(CusGL.Account_Child_ID).Account_Name;
                CusGL.Asset_Id = 0;
                glHeader.GLDetails.Add(CusGL);

                var DiscountCost = db.ChartOfAccountings.Find(86);
                Discount.Account_Id = DiscountCost.Account_Id;
                Discount.Account_Name = DiscountCost.Account_Name;
                Discount.SubLedger_Id = NewSubscription.Subledger_ID;
                Discount.GLDetail_Debit = TotalDiscount.Value;
                Discount.GLDetail_Credit = 0;





                var chartFixedDiscount = db.ChartOfAccountings.Where(a=>a.Account_Code== "040103004").FirstOrDefault();
                AllowDiscount.Account_Id = chartFixedDiscount.Account_Id;
                AllowDiscount.Account_Name = chartFixedDiscount.Account_Name;
                AllowDiscount.SubLedger_Id = NewSubscription.Subledger_ID;
                AllowDiscount.GLDetail_Debit = NewSubscription.DiscountAllow.Value;
                AllowDiscount.GLDetail_Credit = 0;


                if (resulttaxe != null)
                {
                    var thistaxCHart = db.ChartOfAccountings.Where(e => e.Account_Code == resulttaxe.Tax_Code).ToList().FirstOrDefault();
                    GlDetaitax.Account_Id = thistaxCHart.Account_Id;
                    GlDetaitax.Account_Name = thistaxCHart.Account_Name;



                    GlDetaitax.Bu_Id = Convert.ToInt32(NewSubscription.Subscrip_BU);

                    GlDetaitax.GLDetail_Credit = TotalAfterDiscount.Value * (resulttaxe.Tax_Percentage / 100); //داءن
                    GlDetaitax.GLDetail_Debit = 0;//مدين
                    GlDetaitax.SubLedger_Id = 0;
                    GlDetaitax.Asset_Id = 0;
                    glHeader.GLDetails.Add(GlDetaitax);
                }



                var subtype = db.Subscription_Type.Find(NewSubscription.Sub_Type_ID);
                var thischar = db.ChartOfAccountings.Find(subtype.Rvenuo_ID);
                RevenueGL.Account_Id = (int)subtype.Rvenuo_ID;
                RevenueGL.Account_Name = thischar.Account_Name;
                RevenueGL.Bu_Id = Convert.ToInt32(NewSubscription.Subscrip_BU);

                RevenueGL.GLDetail_Credit = NewSubscription.Estimate_Amount;

                RevenueGL.GLDetail_Debit = 0;//مدين
                RevenueGL.SubLedger_Id = NewSubscription.Subledger_ID;
                RevenueGL.Asset_Id = 0;


                if (AllowDiscount.GLDetail_Credit == 0 && AllowDiscount.GLDetail_Debit == 0)
                {

                }
                else
                {
                    glHeader.GLDetails.Add(AllowDiscount);

                }
                if (Discount.GLDetail_Credit != 0 || Discount.GLDetail_Debit != 0)
                {
                    glHeader.GLDetails.Add(Discount);
                }






                // Create five GlDetail
                glHeader.GLDetails.Add(RevenueGL);
                db.GLMasters.Add(glHeader);
                GLMaster.GetBalanceForAccount(glHeader);
                GLMaster.GetBalanceForSubLedger(glHeader);
                db.Entry(NewSubscription).State = EntityState.Modified;
              
                var oldSub = db.Subscriptions.Where(a => a.NewSubCode == NewSubscription.Subscrip_Code).ToList().FirstOrDefault();
                if (oldSub != null)
                {
                    oldSub.IsCanceled = true;
                    db.Entry(oldSub).State = EntityState.Modified;
                }
                
                db.SaveChanges();
                OperationRun.DeleteOperation(NewSubscription.Subscrip_Code);
                db.Dispose();

                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
            }
           
        }

        /// <summary>
        /// Reverse a gl that created for subscription
        /// </summary>
        /// <param name="SubID"></param>
        /// <returns></returns>
        public static string VoidSubscription(int SubID)
        {
            var db = new AccountingContainer();
            var NewSubscription = db.Subscriptions.Find(SubID);
            OperationRun.Addnewoperation(NewSubscription.Subscrip_Code);
            var operation = db.OperationRuns.Where(a => a.Code == NewSubscription.Subscrip_Code).Count();
            if (operation != 1)
            {
                OperationRun.DeleteOperation(NewSubscription.Subscrip_Code);
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.PleaseWait;
            }
            if (NewSubscription.Subscrip_State == TransactionState.Void)
            {
                OperationRun.DeleteOperation(NewSubscription.Subscrip_Code);

                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.hasbeendoneby + " " + NewSubscription.Subscrip_State + " " + Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.previously;
            }
            else
            {
                var msg = "";
                if (NewSubscription.HasOff == true)
                {
                    msg = Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.UnderCloseing;
                    NewSubscription.Subscrip_State = TransactionState.Post;
                    db.SaveChanges();

                }
                else
                {


                    NewSubscription.Subscrip_State = TransactionState.Void;
                    NewSubscription.IsCanceled = true;
                    var TotalDiscount = (NewSubscription.Estimate_Amount * (NewSubscription.Discount / 100));
                    Tax resulttaxe = null;



                    if (NewSubscription.Tax_ID != 0)
                    {
                        NewSubscription.Tax_ID = NewSubscription.Tax_ID;
                        resulttaxe = db.Taxes.Find(NewSubscription.Tax_ID);
                    }
                    var TotalAfterDiscount = NewSubscription.Estimate_Amount - TotalDiscount;
                    var glHeader = new GLMaster();
                    var RevenueGL = new GLDetail();
                    var CusGL = new GLDetail();
                    var GlDetaitax = new GLDetail();
                    var Discount = new GLDetail();
                    var AllowDiscount = new GLDetail();

                    var paymentmethod = db.PaymentMethods.Find(NewSubscription.Payment_Method);
                    glHeader.GL_Date = DateTime.Now;
                    glHeader.GL_Desc = db.Subscription_Type.Find(NewSubscription.Subscrip_Type).Subscrip_Name;
                    glHeader.Company_Id = Convert.ToInt32(NewSubscription.Subscrip_Comp);
                    glHeader.GLType_Id = "مبيعات اشتراك عكسي";
                    glHeader.Currency_Id = "الريال السعودي";
                    glHeader.Auto = true;

                    var thiscust = db.Customers.Find(NewSubscription.Customer_Id);
                    var subled = db.Accounts.Where(e => e.Account_Code == thiscust.Customer_Code).ToList().FirstOrDefault();


                    // Initialize GLState to Pending when first created
                    
                    glHeader.GLCode = NewSubscription.Subscrip_Code;
                    glHeader.GLState = TransactionState.Void;
                    var glcheck = db.GLMasters.Where(a => a.GLCode == glHeader.GLCode && a.GLState == glHeader.GLState).FirstOrDefault();
                    if (glcheck != null)
                    {
                        OperationRun.DeleteOperation(NewSubscription.Subscrip_Code);
                        return " تم عمل قيد الفاتورة بنجاح";
                    }

                    var thisaccount = db.ChartOfAccountings.Find(37);
                    CusGL.Account_Id = thisaccount.Account_Id;
                    CusGL.Account_Name = thisaccount.Account_Name;


                    CusGL.Bu_Id = Convert.ToInt32(NewSubscription.Subscrip_BU);
                    CusGL.GLDetail_Debit = 0; //داءن

                    CusGL.GLDetail_Credit = NewSubscription.AmountAfterTax;


                    CusGL.SubLedger_Id = 0;
                    CusGL.Account_Child_ID = subled.Accounts_Id;
                    CusGL.Account_CHild_Name = db.Accounts.Find(CusGL.Account_Child_ID).Account_Name;
                    CusGL.Asset_Id = 0;
                    glHeader.GLDetails.Add(CusGL);

                    var DiscountCost = db.ChartOfAccountings.Find(86);
                    Discount.Account_Id = DiscountCost.Account_Id;
                    Discount.Account_Name = DiscountCost.Account_Name;
                    Discount.SubLedger_Id = NewSubscription.Subledger_ID;
                    Discount.GLDetail_Credit = TotalDiscount.Value;
                    Discount.GLDetail_Debit = 0;





                    var chartFixedDiscount = db.ChartOfAccountings.Where(a=>a.Account_Code== "040103004").FirstOrDefault();
                    AllowDiscount.Account_Id = chartFixedDiscount.Account_Id;
                    AllowDiscount.Account_Name = chartFixedDiscount.Account_Name;
                    AllowDiscount.SubLedger_Id = NewSubscription.Subledger_ID;
                    AllowDiscount.GLDetail_Credit = NewSubscription.DiscountAllow.Value;
                    AllowDiscount.GLDetail_Debit = 0;


                    if (resulttaxe != null)
                    {
                        var thistaxCHart = db.ChartOfAccountings.Where(e => e.Account_Code == resulttaxe.Tax_Code).ToList().FirstOrDefault();
                        GlDetaitax.Account_Id = thistaxCHart.Account_Id;
                        GlDetaitax.Account_Name = thistaxCHart.Account_Name;



                        GlDetaitax.Bu_Id = Convert.ToInt32(NewSubscription.Subscrip_BU);

                        GlDetaitax.GLDetail_Debit = TotalAfterDiscount.Value * (resulttaxe.Tax_Percentage / 100); //داءن
                        GlDetaitax.GLDetail_Credit = 0;//مدين
                        GlDetaitax.SubLedger_Id = 0;
                        GlDetaitax.Asset_Id = 0;
                        glHeader.GLDetails.Add(GlDetaitax);
                    }



                    var subtype = db.Subscription_Type.Find(NewSubscription.Sub_Type_ID);
                    var thischar = db.ChartOfAccountings.Find(subtype.Rvenuo_ID);
                    RevenueGL.Account_Id = (int)subtype.Rvenuo_ID;
                    RevenueGL.Account_Name = thischar.Account_Name;
                    RevenueGL.Bu_Id = Convert.ToInt32(NewSubscription.Subscrip_BU);

                    RevenueGL.GLDetail_Debit = NewSubscription.Estimate_Amount;

                    RevenueGL.GLDetail_Credit = 0;//مدين
                    RevenueGL.SubLedger_Id = NewSubscription.Subledger_ID;
                    RevenueGL.Asset_Id = 0;


                    if (AllowDiscount.GLDetail_Debit == 0 && AllowDiscount.GLDetail_Credit == 0)
                    {

                    }
                    else
                    {
                        glHeader.GLDetails.Add(AllowDiscount);

                    }
                    if (Discount.GLDetail_Debit != 0 || Discount.GLDetail_Credit != 0)
                    {
                        glHeader.GLDetails.Add(Discount);
                    }

                    // Create five GlDetail
                    glHeader.GLDetails.Add(RevenueGL);
                    db.GLMasters.Add(glHeader);
                    GLMaster.GetBalanceForAccount(glHeader);
                    GLMaster.GetBalanceForSubLedger(glHeader);
                    db.Entry(NewSubscription).State = EntityState.Modified;
                    db.SaveChanges();
                    db.Dispose();
                    OperationRun.DeleteOperation(NewSubscription.Subscrip_Code);
                    return Resources.validate.GLReverse;
                }
                OperationRun.DeleteOperation(NewSubscription.Subscrip_Code);
                return Resources.validate.Error;
            }
           
        }

        /// <summary>
        /// Calculate the amount of subscription by tax amount , amount , discount and allowDiscount 
        /// </summary>
        /// <param name="taxID"></param>
        /// <param name="amount"></param>
        /// <param name="discount"></param>
        /// <param name="AllowDiscount"></param>
        /// <param name="subTempId"></param>
        /// <returns></returns>
        public static decimal CalculateAmount(int? taxID, decimal amount, decimal discount, decimal? AllowDiscount, int? subTempId)
        {
            var db = new AccountingContainer();
            decimal totalofitems = 0;
            decimal totalAftertax = 0;
            var totalDiscount = amount * (discount / 100);
            var TotalAfterDiscount = amount - totalDiscount;
            if (AllowDiscount == null)
            {
                AllowDiscount = 0;
            }
            if (taxID != null || taxID > 0)
            {
                var resulttaxe = db.Taxes.Find(taxID);
                totalAftertax = ((TotalAfterDiscount) + (TotalAfterDiscount * (resulttaxe.Tax_Percentage / 100))) - AllowDiscount.Value;
            }
            else
            {
                totalAftertax = TotalAfterDiscount - AllowDiscount.Value;
            }
            if (subTempId > 0)
            {
                var thislistoftemp = db.SubscriptionItemsTemps.Where(e => e.SubTempID == subTempId).ToList();
                if (thislistoftemp.Count > 0)
                {
                    foreach (var item in thislistoftemp)
                    {
                        totalofitems = totalofitems + (item.Item_Quantity * item.Item_price);
                    }
                    totalAftertax = totalAftertax + totalofitems;
                }
                else
                {
                    
                }
            }
            return Math.Round( totalAftertax,2,MidpointRounding.AwayFromZero);
        }
        /// <summary>
        /// fill edit by old data by subscription id
        /// </summary>
        /// <param name="supId"></param>
        /// <returns></returns>
        public static SubscriptionVM FillSubscription(int supId)
        {
            var db = new AccountingContainer();
            var mySubs = db.Subscriptions.Find(supId);
            //var supscVM = new SubscriptionVM();
            var BuId = mySubs.Subscrip_BU;
            var company = mySubs.Subscrip_Comp;
            var customer = mySubs.Customer_Id;
            var team = mySubs.Emp_Team;
            var branch = mySubs.Subscrip_Pranch;
            var Type = mySubs.Subscrip_Type;
            var Methoud = mySubs.Payment_Method;



            var Supscript = new SubscriptionVM();
           
          
                Supscript.Subscrip_State = mySubs.Subscrip_State;
                Supscript.Estimate_Amount = mySubs.Estimate_Amount;
            
            //var supcs = new SubscriptionVM();
            //var lastSerial = db.Subscriptions.OrderByDescending(c => c.Subscrip_Id).FirstOrDefault();
            //if (lastSerial == null)
            //{
            //    Supscript.Subscrip_Code = "SUBS-001";
            //}
            //else
            //{
            //    //using string substring method to get the number of the last inserted
            //    Supscript.Subscrip_Code = "SUBS-" + (Convert.ToInt32(lastSerial.Subscrip_Code.Substring(5, lastSerial.Subscrip_Code.Length - 5)) + 1).ToString("D3");
            //}
            Supscript.Subscrip_Code = mySubs.Subscrip_Code;
            var resultBussinesUnit = db.BussinesUnits.Where(B => B.BU_Id == BuId);
            if (resultBussinesUnit.Any())
            {
                Supscript.Subscrip_BU = resultBussinesUnit.FirstOrDefault().BU_Id;

            }
            if (mySubs.Sub_Date!=null)
            {
                Supscript.Sub_Date = mySubs.Sub_Date.Value.ToString();
            }
           

            var resultCompany = db.Companies.Where(C => C.Comp_Id == company);
            if (resultCompany.Any())
            {
                Supscript.Comp_Id = resultCompany.FirstOrDefault().Comp_Id;
            }

            var resultCustomer = db.Customers.Where(c => c.Customer_Id == customer);
            if (resultCustomer.Any())
            {
                Supscript.Customer_Id = resultCustomer.FirstOrDefault().Customer_Id;
            }


            var resultTeam = db.EmployeeTeams.Where(c => c.Team_ID == team);
            if (resultTeam.Any())
            {
                Supscript.SelectEmp_Team = resultTeam.FirstOrDefault().Team_ID;
            }

            //var resultBranchs = db.SubLedgers.Where(c => c.SubLed_Id == branch);
            //if (resultTeam != null)
            //{
            //    Supscript.Subscrip_Pranch = resultBranchs.First().SubLed_Id;
            //}

            var resultType = db.Subscription_Type.Where(c => c.Subscription_Type_ID == Type);
            if (resultType.Any())
            {
                Supscript.SelectedSubscrip_Type = resultType.FirstOrDefault().Subscription_Type_ID;
            }

            var resultPay = db.PaymentMethods.Where(c => c.PaymentMethod_Id == Methoud);
            if (resultPay.Any())
            {
                Supscript.SelecltPayment_Method = resultPay.FirstOrDefault().PaymentMethod_Id;
            }
            Supscript.Subscrip_Id = mySubs.Subscrip_Id;



            Supscript.Subledger_ID = mySubs.Subledger_ID;
            Supscript.Subscrip_BU = mySubs.Subscrip_BU;
            Supscript.Department_Id = mySubs.Department_Id;
            Supscript.Subscrip_Pranch = mySubs.Subscrip_Pranch;


            //Supscript.Subscrip_Type = mySubscription.Subscrip_Type;
            Supscript.Subscrip_Description = mySubs.Subscrip_Description;

            Supscript.Estimate_Amount = mySubs.Estimate_Amount;
            Supscript.tel_customer = mySubs.tel_customer;
            Supscript.Subscrip_DateEnd = mySubs.Subscrip_DateEnd;

            Supscript.Customer_Id = (int)mySubs.Customer_Id;
            Supscript.SubType_State = SubType_State.مشترك;
            Supscript.tel_customer = mySubs.tel_customer;
            //var type = db.Subscription_Type.Where(e => e.Durationsubscription == mySubs.Subscrip_Type && e.Subscrip_Name == mySubs.Subscrip_Description).ToList().FirstOrDefault();
            //Supscript.SelectedSubscrip_Type = type.Subscription_Type_ID;
            Supscript.StartTime = mySubs.StartTime.Day + "-" + mySubs.StartTime.Month + "-" + mySubs.StartTime.Year;
          
            Supscript.DeadLine = mySubs.DeadLine.Day + "-" + mySubs.DeadLine.Month + "-" + mySubs.DeadLine.Year;
            Supscript.Subscrip_State = TransactionState.Pending;
            Supscript.Customer_Name = mySubs.Customer_Name;
            Supscript.Check_Number = mySubs.Check_Number;
            Supscript.Taxe_ID = mySubs.Tax_ID;
            //Supscript.Durationsubscription = mySubscription.Durationsubscription;
            Supscript.EmpList = new SelectList(db.Employees.Where(e => e.Reg_ID != null), "Emp_Id", "Emp_Name");
            Supscript.MyCompList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            Supscript.MyCusromerList = new SelectList(db.Customers.Where(e => e.Customer_State == Customer_State.نشط).ToList(), "Customer_Id", "Customer_Name");
            Supscript.MyEmpTeam = new SelectList(db.EmployeeTeams, "Team_ID", "Team_Name");
            Supscript.MySubTypeList = new SelectList(db.Subscription_Type, "Subscription_Type_ID", "Subscrip_Name");
            Supscript.MyPayMethod = new SelectList(db.PaymentMethods.Where(a=>a.IsActive!=false), "PaymentMethod_Id", "Payment_Type");
            Supscript.MySubscripPranch = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 &&a.isActive!=false&& (a.SubLed_Type == "فرع " || a.SubLed_Type == "قسم رئيسي للشركه")), "SubLed_Id", "SubLed_Name");
            Supscript.MyDepartmentList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.isActive != false && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
            Supscript.MyBuList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.isActive != false && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            Supscript.MySubList = new SelectList(db.SubLedgers.Where(a => a.SubLed_Type == "مركز تكلفه يدوي" && a.isActive != false), "SubLed_Id", "SubLed_Name");
            var thischartaxParent = db.ChartOfAccountings.Find(48);
            Supscript.tax_List = new SelectList(db.Taxes.Where(e => e.Tax_Code.Contains(thischartaxParent.Account_Code)).Where(a => a.IsActive != false).ToList(), "Tax_ID", "Tax_Name");
            if (mySubs.Discount != null && mySubs.Discount > 0)
            {
                Supscript.Discount = (decimal)mySubs.Discount;
            }
            if (mySubs.DiscountAllow != null && mySubs.DiscountAllow > 0)
            {
                Supscript.AllowDiscount = (decimal)mySubs.DiscountAllow;
            }


            return Supscript;
        }
        /// <summary>
        /// Edit data of subscription 
        /// </summary>
        /// <param name="SupscripeVM"></param>
        /// <returns></returns>
        public static Subscription EditSubscription(SubscriptionVM SupscripeVM)
        {
            var db = new AccountingContainer();
     
            var NewSubscription = db.Subscriptions.Find(SupscripeVM.Subscrip_Id);

            if (SupscripeVM != null)
            {








                var resulEmp_team = db.EmployeeTeams.Find(SupscripeVM.SelectEmp_Team);
                if (resulEmp_team != null)
                {
                    NewSubscription.Emp_Team = resulEmp_team.Team_ID;
                }




                NewSubscription.Subscrip_Description = SupscripeVM.Subscrip_Description;


                NewSubscription.DeadLine = Convert.ToDateTime(SupscripeVM.DeadLine);
                NewSubscription.StartTime = Convert.ToDateTime(SupscripeVM.StartTime);
                if (NewSubscription.DeadLine >= DateTime.Now)
                {

                    //NewSubscription.Subscrip_State = TransactionState.Post;
                    NewSubscription.SubType_State = SubType_State.مشترك;
                }
                else
                {
                    NewSubscription.SubType_State = SubType_State.منتهى;
                }


                db.Entry(NewSubscription).State = EntityState.Modified;
                if (NewSubscription.QRNumber != 0 && NewSubscription.QRNumber != null) { 
                var QRSub = db.MasterQRInvoices.Find(NewSubscription.QRNumber);
                    QRSub.StartDate = NewSubscription.StartTime;
                    QRSub.EndDate = NewSubscription.DeadLine;
                    db.Entry(QRSub).State = EntityState.Modified;

                }
                    
                        db.SaveChanges();
                    
                    


                    return NewSubscription;
                }

            return null;
           
            
            
         }           

        /// <summary>
        /// Delete subscription by id
        /// </summary>
        /// <param name="SibbId"></param>
        /// <returns></returns>
        public static int DeleteSubscription(int SibbId)
        {
            var db = new AccountingContainer();
            var Supp = db.Subscriptions.Find(SibbId);
            if (Supp != null&&Supp.Subscrip_State==TransactionState.Pending)
            {
                db.Subscriptions.Remove(Supp);
                db.SaveChanges();
                db.Dispose();
                return 1;
            }
            return 0;
        }
        /// <summary>
        /// Create a GL(General Ledger) for subscription 
        /// </summary>
        /// <param name="SupVM"></param>
        /// <returns></returns>
        public static bool UpdateSubscription(SubscriptionVM SupVM)
        {
            var db = new AccountingContainer();
            var Sup = db.Subscriptions.Find(SupVM.Subscrip_Id);
            OperationRun.Addnewoperation(Sup.Subscrip_Code);
            var operation = db.OperationRuns.Where(a => a.Code == Sup.Subscrip_Code).Count();
            if (operation != 1)
            {
                OperationRun.DeleteOperation(Sup.Subscrip_Code);
                return false;
            }
            Sup.Subscrip_State = SupVM.Subscrip_State;
            db.Entry(Sup).State = EntityState.Modified;
            GLMaster glHeader = new GLMaster();


            glHeader.GL_Date = Sup.StartTime;
            glHeader.GL_Desc = Sup.Subscrip_Description;
            glHeader.Company_Id = Convert.ToInt32(Sup.Subscrip_Comp);
            glHeader.GLType_Id = "اشتراك";
            glHeader.Currency_Id = "";
            glHeader.GLState = TransactionState.Pending;
            glHeader.Auto = true;
            GLDetail Clientaccount = new GLDetail(),
            Maintenanceincome = new GLDetail();



            Clientaccount.Bu_Id = Convert.ToInt32(Sup.Subscrip_BU);
            Clientaccount.Asset_Id = null;

            var ChartOfAccounCustomer = db.ChartOfAccountings.Find(37);
            Clientaccount.Account_Id = ChartOfAccounCustomer.Account_Id;
            Clientaccount.Account_Name = ChartOfAccounCustomer.Account_Name;
            Clientaccount.GLMasterGLMaster_Id = glHeader.GLMaster_Id;

            Maintenanceincome.Bu_Id = Convert.ToInt32(Sup.Subscrip_BU);
            Maintenanceincome.Asset_Id = null;

            var ChartOfAccountMaintenance = db.ChartOfAccountings.Find(5);
            Maintenanceincome.Account_Id = ChartOfAccountMaintenance.Account_Id;
            Maintenanceincome.Account_Name = ChartOfAccountMaintenance.Account_Name;
            Maintenanceincome.GLMasterGLMaster_Id = glHeader.GLMaster_Id;



            var sub = db.Subscriptions.Find(Sup.Subledger_ID);
            SubLedger sasa = db.SubLedgers.Where(a => a.SubLed_Code == sub.Subscrip_Code).FirstOrDefault();
            Maintenanceincome.SubLedger_Id = sasa.SubLed_Id;
            Clientaccount.SubLedger_Id = 0;

            //var Char = db.ChartOfAccountings.Find(Sup.);
            //var ChartOfAccounInventory = db.ChartOfAccountings.Where(account => account.Account_Code == store.Store_Code).First();
            //inventoryGL.Account_Id = ChartOfAccounInventory.Account_Id;
            //inventoryGL.Account_Name = ChartOfAccounInventory.Account_Name;



            switch (Sup.Subscrip_State)
            {
                case TransactionState.Post:
                    glHeader.GLState = TransactionState.Post;
                    var gl = db.GLMasters.Where(a => a.GLCode == glHeader.GLCode && a.GLState == glHeader.GLState).FirstOrDefault();
                    if (gl != null)
                    {
                        OperationRun.DeleteOperation(Sup.Subscrip_Code);
                        return true;
                    }

                    Clientaccount.GLDetail_Debit = Sup.Estimate_Amount;
                    Maintenanceincome.GLDetail_Credit = Sup.Estimate_Amount;
                    break;
                case TransactionState.Void:
                    glHeader.GLState = TransactionState.Void;
                    var glcheck = db.GLMasters.Where(a => a.GLCode == glHeader.GLCode && a.GLState == glHeader.GLState).FirstOrDefault();
                    if (glcheck != null)
                    {
                        OperationRun.DeleteOperation(Sup.Subscrip_Code);
                        return true;
                    }

                    Maintenanceincome.GLDetail_Debit = Sup.Estimate_Amount;
                    Clientaccount.GLDetail_Credit = Sup.Estimate_Amount;
                    break;
                default:
                    break;
            }
            
            db.GLMasters.Add(glHeader);
            glHeader.GLDetails.Add(Clientaccount);
            glHeader.GLDetails.Add(Maintenanceincome);
            GLMaster.GetBalanceForAccount(glHeader);
            GLMaster.GetBalanceForSubLedger(glHeader);
            db.SaveChanges();
            OperationRun.DeleteOperation(Sup.Subscrip_Code);
            db.Dispose();
            return true;
        }
        public static IEnumerable<SelectListItem> GetSubTypy(int typeId)
        {

            var db = new AccountingContainer();
            IEnumerable<SelectListItem> DeadLine = new SelectList(db.Subscription_Type.Where(p => p.Subscription_Type_ID == typeId), "Subscription_Type_ID", "Subscrip_Name");
            return DeadLine;
        }

        /// <summary>
        /// For fill dropdown list by data customers
        /// </summary>
        /// <returns></returns>
        public static SubscriptionVM GetCustomerList()
        {
            var db = new AccountingContainer();
            SubscriptionVM vm = new SubscriptionVM();
            var List = from cust in db.Customers.Where(e => e.Customer_State == Customer_State.نشط)
                       select new
                       {
                           Id = cust.Customer_Id,
                           Name = cust.Customer_Name + " " + cust.Customer_Phone

                       };
            vm.MyCusromerList = new SelectList(List, "Id", "Name");
            return vm;
        }

        /// <summary>
        /// Get end date for subscription by start date and Duration subscription
        /// </summary>
        /// <param name="typeid"></param>
        /// <param name="starttime"></param>
        /// <returns></returns>
        public static string getdate(int typeid, string starttime)
        {
            var db = new AccountingContainer();

            var typemodel = db.Subscription_Type.Find(typeid);
            DateTime time = Convert.ToDateTime(starttime);
            var vm = new SubscriptionVM();
            //NewSubscription.DeadLine = DateTime.Now.AddMonths(mm.Durationsubscription);
            vm.DeadLine = time.AddMonths(typemodel.Durationsubscription).ToString();
            //   var deadline = Convert.ToDateTime( startdate.AddMonths(typemodel.Durationsubscription).ToString("dd-MM-yyyy   HH:mm:ss")); 
            return vm.DeadLine;
        }
        /// <summary>
        /// Get end date for subscription by start date and Duration subscription
        /// </summary>
        /// <param name="Date"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public static string getdateofsub(string Date, int types)
        {
            var db = new AccountingContainer();

            // var typemodel = db.Subscriptions.Find(typeid);
            DateTime date = Convert.ToDateTime(Date);
            var type = db.Subscription_Type.Find(types);
            var vm = new SubscriptionVM();
            //NewSubscription.DeadLine = DateTime.Now.AddMonths(mm.Durationsubscription);
            vm.DeadLine = date.AddMonths(type.Durationsubscription).ToString();
            //   var deadline = Convert.ToDateTime( startdate.AddMonths(typemodel.Durationsubscription).ToString("dd-MM-yyyy   HH:mm:ss")); 
            return vm.DeadLine;
        }
        /// <summary>
        /// Get customer phone by customer id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static string getCustPhone(int ID)
        {
            var db = new AccountingContainer();

            var typemodel = db.Customers.Find(ID);
            var vm = new SubscriptionVM();
            //NewSubscription.DeadLine = DateTime.Now.AddMonths(mm.Durationsubscription);
            vm.tel_customer = typemodel.Customer_Phone;
            //   var deadline = Convert.ToDateTime( startdate.AddMonths(typemodel.Durationsubscription).ToString("dd-MM-yyyy   HH:mm:ss")); 
            return vm.tel_customer;
        }
   
        /// <summary>
        /// Get old date for subscription by id and fill the view of edit by it
        /// </summary>
        /// <param name="supId"></param>
        /// <returns></returns>
        public static SubscriptionVM FillSubscrip(int supId)
        {
            var db = new AccountingContainer();
            var mySubs = db.Subscriptions.Find(supId);
            //var supscVM = new SubscriptionVM();
            var BuId = mySubs.Subscrip_BU;
            var company = mySubs.Subscrip_Comp;
            var customer = mySubs.Customer_Id;
            var team = mySubs.Emp_Team;
            var branch = mySubs.Subscrip_Pranch;
            var Type = mySubs.Subscrip_Type;
            var Methoud = mySubs.Payment_Method;



            var Supscript = new SubscriptionVM();
            //var supcs = new SubscriptionVM();
            int LastCode = (from s in db.Subscriptions
                            select s.Subscrip_Id).ToList().LastOrDefault();
            Supscript.Subscrip_Code = mySubs.Subscrip_Code;

            var resultBussinesUnit = db.BussinesUnits.Where(B => B.BU_Id == BuId);
            if (resultBussinesUnit.Any())
            {
                Supscript.Subscrip_BU = resultBussinesUnit.First().BU_Id;
            }
            if (mySubs.Sub_Date != null)
            {
                Supscript.Sub_Date = mySubs.Sub_Date.Value.ToString();
            }


            var resultCompany = db.Companies.Where(C => C.Comp_Id == company);
            if (resultCompany.Any())
            {
                Supscript.Comp_Id = resultCompany.First().Comp_Id;
            }

            var resultCustomer = db.Customers.Where(c => c.Customer_Id == customer);
            if (resultCustomer.Any())
            {
                Supscript.Customer_Id = resultCustomer.First().Customer_Id;
            }


            var resultTeam = db.EmployeeTeams.Where(c => c.Team_ID == team);
            if (resultTeam.Any())
            {
                Supscript.SelectEmp_Team = resultTeam.First().Team_ID;
            }

            var resultBranchs = db.Branches.Where(c => c.Branch_Id == branch);
            if (resultTeam.Any())
            {
                Supscript.Subscrip_Pranch = resultBranchs.First().Branch_Id;
            }

            var resultType = db.Subscription_Type.Where(c => c.Subscription_Type_ID == Type);
            if (resultType.Any())
            {
                Supscript.SelectedSubscrip_Type = resultType.First().Subscription_Type_ID;
            }

            var resultPay = db.PaymentMethods.Where(c => c.PaymentMethod_Id == Methoud);
            if (resultPay.Any())
            {
                Supscript.SelecltPayment_Method = resultPay.First().PaymentMethod_Id;
            }
            Supscript.Subscrip_Id = mySubs.Subscrip_Id;

            //Supscript.Subscrip_Type = mySubscription.Subscrip_Type;
            Supscript.Subscrip_Description = mySubs.Subscrip_Description;
            Supscript.Subscrip_Code = mySubs.Subscrip_Code;
            Supscript.Estimate_Amount = mySubs.Estimate_Amount;
            Supscript.tel_customer = mySubs.tel_customer;
            Supscript.Subscrip_DateEnd = mySubs.Subscrip_DateEnd;

            Supscript.SubType_State = SubType_State.مشترك;
            Supscript.DeadLine = mySubs.DeadLine.ToString();
            Supscript.StartTime = mySubs.StartTime.ToString();
            Supscript.Subscrip_State = TransactionState.Pending;

            //Supscript.Durationsubscription = mySubscription.Durationsubscription;
            Supscript.MyBuList = new SelectList(db.BussinesUnits, "BU_Id", "BU_Name");
            Supscript.MyCompList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            Supscript.MyCusromerList = new SelectList(db.Customers.Where(e => e.Customer_State == Customer_State.نشط).ToList(), "Customer_Id", "Customer_Name");
            Supscript.MyEmpTeam = new SelectList(db.EmployeeTeams, "Team_ID", "Team_Name");
            Supscript.MySubscripPranch = new SelectList(db.Branches, "Branch_Id", "Branch_Name");
            Supscript.MySubTypeList = new SelectList(db.Subscription_Type, "Subscription_Type_ID", "Subscrip_Name");
            Supscript.MyPayMethod = new SelectList(db.PaymentMethods, "PaymentMethod_Id", "Payment_Type");
            Supscript.EmpList = new SelectList(db.Employees.Where(e => e.Reg_ID != null), "Emp_Id", "Emp_Name");


            Supscript.DeadLine = mySubs.DeadLine.ToString("dd-MM-yyyy   HH:mm:ss");
            return Supscript;
        }
        /// <summary>
        /// Edit a subscription 
        /// </summary>
        /// <param name="subVM"></param>
        /// <returns></returns>
        public static Subscription EditSubscrip(SubscriptionVM subVM)
        {
            var db = new AccountingContainer();

            //var NewSubscr = new Subscription();

            //  var mm = db.Subscription_Type.Find(subVM.SelectedSubscrip_Type);
            var NewSub = db.Subscriptions.Find(subVM.Subscrip_Id);

            TimeSpan diffrenceDays = Convert.ToDateTime(NewSub.Subscrip_DateEnd).Subtract(NewSub.StartTime);
            int s = diffrenceDays.Days;
            NewSub.Subscrip_Stop = false;
            DateTime endDate = new DateTime();

            endDate = Convert.ToDateTime(subVM.DeadLine);
            //  TimeSpan days = NewSub.StartTime.Subtract(Convert.ToDateTime(NewSub.Subscrip_DateEnd));
            NewSub.DeadLine = NewSub.DeadLine.AddDays(s);
            //NewSub.Subscrip_DateEnd = Convert.ToDateTime((NewSub.StartTime - DateTime.Now));

            //NewSub.StartTime -= subVM.Subscrip_DateEnd.Value.AddDays(DeadLine);
            //endDate = endDate.AddDays(mm.Subscrip_EndDuration);
            //NewSub.DeadLine = endDate;

            //DateTime dt1 = DateTime.Parse("05/05/2005");
            ////second date
            //DateTime dt2 = DateTime.Now;
            ////Subtact dates
            //TimeSpan ts = dt2.Subtract(dt1);

            db.Entry(NewSub).State = EntityState.Modified;

            db.SaveChanges();
            db.Dispose();
            return NewSub;
        }
        /// <summary>
        /// Stop subscription by id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Subscription StopSub(int id)
        {
            var db = new AccountingContainer();
            var Subbb = db.Subscriptions.Find(id);
            Subbb.Subscrip_Stop = true;


            db.Entry(Subbb).State = EntityState.Modified;

            //db.Entry<thisub>.sate=entitystate.modify;
            db.SaveChanges();
            db.Dispose();
            return Subbb;
        }
        /// <summary>
        /// Get list of stoped subscription 
        /// </summary>
        /// <returns></returns>
        public static List<Subscription> stoppedsub()
        {
            var db = new AccountingContainer();
            var listofstop = db.Subscriptions.Where(s => s.Subscrip_Stop == true).ToList();
            return listofstop;

        }
 
        /// <summary>
        /// Get data to fill subscription invoice by subscription id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static SubscriptionInvoiceVM FillinvForcreation(int ID)
        {
            var db = new AccountingContainer();
            var thisSub = db.Subscriptions.Find(ID);
            SubscriptionInvoiceVM SUVM = new SubscriptionInvoiceVM();
            SUVM.Customer_Id = thisSub.Customer_Id;
            SUVM.Customer_Name = thisSub.Customer_Name;
            SUVM.SubscribtionId = ID;
            SUVM.Subscrip_Code = thisSub.Subscrip_Code;
            SUVM.Subscrip_Description = thisSub.Subscrip_Description;
            SUVM.DeadLine = thisSub.DeadLine.Day + "-" + thisSub.DeadLine.Month + "-" + thisSub.DeadLine.Year;
            SUVM.StartTime = thisSub.StartTime.Day + "-" + thisSub.StartTime.Month + "-" + thisSub.StartTime.Year;
            SUVM.Estimate_Amount = thisSub.AmountAfterTax;
            SUVM.Subscrip_Pranch = thisSub.Subscrip_Pranch;
            SUVM.Sub_Type_ID = thisSub.Sub_Type_ID;

            SUVM.Subscrip_BU = Convert.ToInt32(thisSub.Subscrip_BU);
            SUVM.Pay_Amount = 0;
            var lastSerial = db.SubscriptionInvoices.OrderByDescending(c => c.Inv_ID).FirstOrDefault();
            if (lastSerial == null)
            {
                SUVM.Serial_Number = "SUBINV-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                SUVM.Serial_Number = "SUBINV-" + (Convert.ToInt32(lastSerial.Serial_Number.Substring(7, lastSerial.Serial_Number.Length - 7)) + 1).ToString("D3");
            }

            SUVM.CustomerList = new SelectList(db.Customers.Where(e => e.Customer_State == Customer_State.نشط).ToList(), "Customer_Id", "Customer_Name");
            List<object> newList = new List<object>();
            var members = db.PaymentMethods.Where(a=>a.IsActive!=false).ToList();
            foreach (var member in members)
                newList.Add(new
                {
                    Id = member.PaymentMethod_Id,
                    Name = member.Payment_Type 
                });
            SUVM.paymentMethodList = new SelectList(newList, "Id", "Name");



            //SUVM.BranshList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسي للشركه"), "SubLed_Id", "SubLed_Name");
            //SUVM.DepartmentList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
            //SUVM.BUList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            //SUVM.SubledgerList = new SelectList(db.SubLedgers.Where(a => a.SubLed_Type == "مركز تكلفه يدوي"), "SubLed_Id", "SubLed_Name");

            return SUVM;
        }
        /// <summary>
        /// Find file of subscription invoice 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SubscriptionInvoiceFile FindFileByID(int id)
        {
            var db = new AccountingContainer();
            var file = db.SubscriptionInvoiceFiles.Find(id);
            return file;
        }
        /// <summary>
        /// Get list of files for  subscription invoice 
        /// </summary>
        /// <param name="subId"></param>
        /// <returns></returns>
        public static List<SubscriptionInvoiceFile> GetAllFiles(int subId)
        {
            var db = new AccountingContainer();
            var files = db.SubscriptionInvoiceFiles.Where(s => s.SubscriptionInvoice_ID == subId).ToList();
            return files;
        }
        /// <summary>
        /// Create new invoice for subscription for customer
        /// </summary>
        /// <param name="SUVM"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static SubscriptionInvoice CreateInvoice(SubscriptionInvoiceVM SUVM , String UserId)
        {
            var db = new AccountingContainer();
            SubscriptionInvoice SUBI = new SubscriptionInvoice();
            SUBI.Customer_Id = SUVM.Customer_Id;
            var thiscust = db.Customers.Find(SUVM.Customer_Id);
            var sub = db.Subscriptions.Where(a => a.Subscrip_Id == SUVM.SubscribtionId).FirstOrDefault();
            SUBI.Customer_Name = thiscust.Customer_Name;
            SUBI.Subscrip_Code = SUVM.Subscrip_Code;

            SUBI.Subscrip_Description = SUVM.Note;
            SUBI.DeadLine = Convert.ToDateTime(SUVM.DeadLine);
            SUBI.StartTime =sub.StartTime;
            if (sub.DeadLine == null)
            {
                SUBI.DeadLine = DateTime.Today;
            }
            else
            {
                SUBI.DeadLine = sub.DeadLine;
            }
            SUBI.Estimate_Amount = SUVM.Estimate_Amount;
            SUBI.Subscrip_Pranch = SUVM.Subscrip_Pranch;
            var lastSerial = db.SubscriptionInvoices.OrderByDescending(c => c.Inv_ID).FirstOrDefault();
            if (lastSerial == null)
            {
                SUBI.Serial_Number = "SUBINV-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                SUBI.Serial_Number = "SUBINV-" + (Convert.ToInt32(lastSerial.Serial_Number.Substring(7, lastSerial.Serial_Number.Length - 7)) + 1).ToString("D3");
            }
            SUBI.InvoiceDate = Convert.ToDateTime(SUVM.InvoiceDate);
            SUBI.Subscrip_Pranch = SUVM.Subscrip_Pranch;
            SUBI.Payment_Method = SUVM.Payment_Method;
            SUBI.Pay_Amount = SUVM.Pay_Amount;
            SUBI.Transaction_State = TransactionState.Post;
            SUBI.Note = SUVM.Note;
            db.SubscriptionInvoices.Add(SUBI);
            db.SaveChanges();
            var subNeeded = db.Subscriptions.Where(e => e.Subscrip_Code == SUBI.Subscrip_Code).ToList().FirstOrDefault();
            var subtype = db.Subscription_Type.Find(SUVM.Sub_Type_ID);
            var glHeader = new GLMaster();
            var GlDetail1 = new GLDetail();
            var GlDetail = new GLDetail();
            var PaymentGL = new GLDetail();
            var tax = db.Taxes.Find(subNeeded.Tax_ID);
            subNeeded.Payed_Amount = subNeeded.Payed_Amount + SUVM.Pay_Amount;
            if (Math.Round(subNeeded.Payed_Amount.Value , 2) >= Math.Round(subNeeded.AmountAfterTax,2))
            {
                subNeeded.Sub_Pay_State = Models.Sub_Pay_State.تم_الدفع;
            }
            else
            {
                subNeeded.Sub_Pay_State = Models.Sub_Pay_State.دفع_جزئي;
            }

            // Add file to payment a subscription 
            if (SUVM.Files.FirstOrDefault() != null)
            {
                foreach (var file in SUVM.Files)
                {
                    var filedetails = new SubscriptionInvoiceFile();
                    var data = new byte[file.ContentLength];
                    filedetails.FileName = file.FileName;
                    filedetails.ContentLength = file.ContentLength;
                    file.InputStream.Read(data, 0, file.ContentLength);
                    filedetails.ProImg = data;
                    filedetails.SubscriptionInvoice_ID = SUBI.Inv_ID;
                    if (data.Length < 1000000)
                    {
                        SUBI.SubscriptionInvoiceFiles.Add(filedetails);
                    }
                    
                }
            }
            //   subNeeded.Payed_Amount = subNeeded.Payed_Amount + SUVM.Pay_Amount;
            subNeeded.Check_Number = SUVM.checkNumber;
            subNeeded.Payment_Method = SUVM.Payment_Method;
            db.Entry(subNeeded).State = EntityState.Modified;
            decimal taxAmount = 0;
            if (tax == null)
            {
                taxAmount = 0;
            }
            else
            {
                taxAmount = subNeeded.Estimate_Amount * (tax.Tax_Percentage / 100);
            }

            // subNeeded.Estimate_Amount = subNeeded.Estimate_Amount + taxAmount;
            var paymentmethod = db.PaymentMethods.Find(SUBI.Payment_Method);
            glHeader.GL_Date = (DateTime)SUBI.InvoiceDate;
            glHeader.GL_Desc = SUVM.Note;
            glHeader.Company_Id = Convert.ToInt32(subNeeded.Subscrip_Comp);
            glHeader.GLType_Id = " قبض اشتراك";
            glHeader.Currency_Id = "الريال السعودي";
            glHeader.Auto = true;
            // Initialize GLState to Pending when first created
           
            glHeader.GLCode = SUBI.Serial_Number;
            glHeader.GLState = TransactionState.Post;
            var gl = db.GLMasters.Where(a => a.GLCode == glHeader.GLCode && a.GLState == glHeader.GLState).FirstOrDefault();
            if (gl != null)
            {
                return SUBI;
            }

            GlDetail.Account_Id = (int)paymentmethod.Bank_Account_Id;
            GlDetail.Account_Name = db.ChartOfAccountings.Find(GlDetail.Account_Id).Account_Name;
            GlDetail.Bu_Id = Convert.ToInt32(SUBI.Subscrip_BU);
            GlDetail.GLDetail_Credit = 0;//داءن
            var payment = db.PaymentMethods.Find(SUBI.Payment_Method);

            if (payment.Percentage == 0 && payment.Operatin_Cost == 0)
            {
                GlDetail.GLDetail_Debit = SUBI.Pay_Amount;
            }
            else {


                PaymentGL.Account_Id = payment.Expense_Account_Id.Value;
                PaymentGL.Account_Name = db.ChartOfAccountings.Find(PaymentGL.Account_Id).Account_Name;
                PaymentGL.GLDetail_Debit = (payment.Percentage.Value * (SUBI.Pay_Amount)) / 100 + payment.Operatin_Cost.Value;
                PaymentGL.SubLedger_Id = SUBI.Subledger_ID;
                GlDetail.GLDetail_Debit = SUBI.Pay_Amount - PaymentGL.GLDetail_Debit;


            }
            GlDetail.SubLedger_Id = 0;
            GlDetail.Asset_Id = 0;
            glHeader.GLDetails.Add(GlDetail);

            var thisaccount = db.ChartOfAccountings.Find(37);
            GlDetail1.Account_Id = (int)thisaccount.Account_Id;
            GlDetail1.Account_Name = thisaccount.Account_Name;
            GlDetail1.Bu_Id = Convert.ToInt32(SUBI.Subscrip_BU);
            GlDetail1.GLDetail_Credit = SUBI.Pay_Amount;//داءن
            GlDetail1.GLDetail_Debit = 0;//مدين
            var Customerr = db.Customers.Find(SUVM.Customer_Id);
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

            var traBatch = new TransactionBatch();

            traBatch.OperationName = "فاتورة اشتراك";
            traBatch.Description = SUVM.Subscrip_Description;
            traBatch.Date = Convert.ToDateTime(SUVM.InvoiceDate);
            traBatch.UserId = UserId;
            traBatch.OperationId = SUBI.Inv_ID;
            traBatch.Operation_Code = SUBI.Serial_Number;
            TransactionBatch.AddNewTransaction(traBatch);

            db.Dispose();
            return SUBI;
        }
        /// <summary>
        /// Create a reverse GL(Gerenal Ledger) for subscription invoice
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string VoidInvoice(int id)
        {
            var db = new AccountingContainer();
            var SUVM = db.SubscriptionInvoices.Find(id);
            OperationRun.Addnewoperation(SUVM.Serial_Number);
            var operation = db.OperationRuns.Where(a => a.Code == SUVM.Serial_Number).Count();
            if (operation != 1)
            {
                OperationRun.DeleteOperation(SUVM.Serial_Number);
                return "يرجي الانتظار ومحاولة الاعتماد مرة اخرى ";
            }
            if (SUVM.Transaction_State == TransactionState.Void)
            {
                OperationRun.DeleteOperation(SUVM.Serial_Number);
                return "لقد تم عمل" + " " + SUVM.Transaction_State + " " + "من قبل";
            }
            else
            {
                var thiscust = db.Customers.Find(SUVM.Customer_Id);
                // var subled = db.SubLedgers.Where(e => e.SubLed_Code == thiscust.Customer_Code).ToList().FirstOrDefault();

                var subNeeded = db.Subscriptions.Where(e => e.Subscrip_Code == SUVM.Subscrip_Code).ToList().FirstOrDefault();
                //var subtype = db.Subscription_Type.Find(SUVM.Sub_Type_ID);
                var glHeader = new GLMaster();
                glHeader.GLType_Id = "قبض اشتراك عكسي";
                var GlDetail1 = new GLDetail();
                var GlDetail = new GLDetail();
                var PaymentGL = new GLDetail();
                var tax = db.Taxes.Find(subNeeded.Tax_ID);
                var paymentmethod = db.PaymentMethods.Find(subNeeded.Payment_Method);
                if (subNeeded.Payed_Amount == SUVM.Pay_Amount)
                {
                    subNeeded.Sub_Pay_State = Models.Sub_Pay_State.لم_يتم_الدفع;
                    subNeeded.Payed_Amount = 0;
                }
                else if (SUVM.Pay_Amount < subNeeded.Payed_Amount)
                {
                    subNeeded.Sub_Pay_State = Models.Sub_Pay_State.دفع_جزئي;
                    subNeeded.Payed_Amount = subNeeded.Payed_Amount - SUVM.Pay_Amount;
                }


                subNeeded.Payment_Method = 0;
                db.Entry(subNeeded).State = EntityState.Modified;
                decimal taxAmount = 0;
                if (tax == null)
                {
                    taxAmount = 0;
                }
                else
                {
                    taxAmount = subNeeded.Estimate_Amount * (tax.Tax_Percentage / 100);
                }

                // subNeeded.Estimate_Amount = subNeeded.Estimate_Amount + taxAmount;

                glHeader.GL_Date = DateTime.Now.Date;
                glHeader.GL_Desc = subNeeded.Subscrip_Description;
                glHeader.Company_Id = Convert.ToInt32(subNeeded.Subscrip_Comp);
         
                glHeader.Currency_Id = "الريال السعودي";
                glHeader.Auto = true;
                // Initialize GLState to Pending when first created
           
                glHeader.GLCode = SUVM.Serial_Number;
                glHeader.GLState = TransactionState.Void;
                var glcheck = db.GLMasters.Where(a => a.GLCode == glHeader.GLCode && a.GLState == glHeader.GLState).FirstOrDefault();
                if (glcheck != null)
                {
                    OperationRun.DeleteOperation(SUVM.Serial_Number);
                    return " تم عمل قيد الفاتورة بنجاح";
                }

                GlDetail.Account_Id = (int)paymentmethod.Bank_Account_Id;
                GlDetail.Account_Name = db.ChartOfAccountings.Find(GlDetail.Account_Id).Account_Name;
                GlDetail.Bu_Id = Convert.ToInt32(SUVM.Subscrip_BU);
                GlDetail.GLDetail_Debit = 0;//داءن
                var payment = db.PaymentMethods.Find(SUVM.Payment_Method);




                if (payment.Percentage == 0 && payment.Operatin_Cost == 0)
                {
                    GlDetail.GLDetail_Credit = SUVM.Pay_Amount;
                }
                else
                {


                    PaymentGL.Account_Id = payment.Expense_Account_Id.Value;
                    PaymentGL.Account_Name = db.ChartOfAccountings.Find(PaymentGL.Account_Id).Account_Name;
                    PaymentGL.GLDetail_Credit = (payment.Percentage.Value * (SUVM.Pay_Amount)) / 100 + payment.Operatin_Cost.Value;
                    PaymentGL.SubLedger_Id = SUVM.Subledger_ID;
                   
                    GlDetail.GLDetail_Credit = SUVM.Pay_Amount - PaymentGL.GLDetail_Credit;

                }
                GlDetail.SubLedger_Id = 0;
                GlDetail.Asset_Id = 0;
                glHeader.GLDetails.Add(GlDetail);

                var thisaccount = db.ChartOfAccountings.Find(37);
                GlDetail1.Account_Id = (int)thisaccount.Account_Id;
                GlDetail1.Account_Name = thisaccount.Account_Name;
                GlDetail1.Bu_Id = Convert.ToInt32(SUVM.Subscrip_BU);
                GlDetail1.GLDetail_Credit = 0;//داءن
                GlDetail1.GLDetail_Debit = SUVM.Pay_Amount;//مدين
                var Customerr = db.Customers.Find(SUVM.Customer_Id);
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
                SUVM.Transaction_State = TransactionState.Void;
                db.Entry(SUVM).State = EntityState.Modified;
                db.SaveChanges();
                db.SaveChanges();
                OperationRun.DeleteOperation(SUVM.Serial_Number);
                db.Dispose();
                return "تم تسجيل قيد عكس لسند القبض";
            }
           
        }
        /// <summary>
        /// Get list of all subscription invoices
        /// </summary>
        /// <returns></returns>
        public static List<SubscriptionInvoice> getallInvoices()
        {
            var db = new AccountingContainer();
            var allInv = db.SubscriptionInvoices.ToList();
            return allInv;
        } 
        /// <summary>
        /// Get details of subscription invoice by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SubscriptionInvoice GetThisINvoiceDetail(int id)
        {
            var db = new AccountingContainer();
            var thisInv = db.SubscriptionInvoices.Find(id);
            return thisInv;
        }

        /// <summary>
        /// Fill view of edit subscription by old data to edit it
        /// </summary>
        /// <param name="supId"></param>
        /// <returns></returns>
        public static SubscriptionVM FillSubscripForNew(int supId,bool?edit)
        {
            var db = new AccountingContainer();
            var mySubs = db.Subscriptions.Find(supId);
            //var supscVM = new SubscriptionVM();
            var BuId = mySubs.Subscrip_BU;
            var company = mySubs.Subscrip_Comp;
            var customer = mySubs.Customer_Id;
            var team = mySubs.Emp_Team;
            var branch = mySubs.Subscrip_Pranch;
            var Type = mySubs.Subscrip_Type;
            var Methoud = mySubs.Payment_Method;



            var Supscript = new SubscriptionVM();
            Supscript.Emp_Id = mySubs.Emp_Id;
            //var supcs = new SubscriptionVM();
            var lastSerial = db.Subscriptions.ToList().LastOrDefault();
            if (lastSerial == null)
            {
                Supscript.Subscrip_Code = "SUBS-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                Supscript.Subscrip_Code = "SUBS-" + (Convert.ToInt32(lastSerial.Subscrip_Code.Substring(5, lastSerial.Subscrip_Code.Length - 5)) + 1).ToString("D3");
            }

            var resultBussinesUnit = db.BussinesUnits.Where(B => B.BU_Id == BuId);
            if (resultBussinesUnit.Any())
            {
                Supscript.Subscrip_BU = resultBussinesUnit.FirstOrDefault().BU_Id;

            }

            if (mySubs.Sub_Date != null)
            {
                Supscript.Sub_Date = mySubs.Sub_Date.Value.ToString();
            }

            var resultCompany = db.Companies.Where(C => C.Comp_Id == company);
            if (resultCompany.Any())
            {
                Supscript.Comp_Id = resultCompany.FirstOrDefault().Comp_Id;
            }

            var resultCustomer = db.Customers.Where(c => c.Customer_Id == customer);
            if (resultCustomer.Any())
            {
                Supscript.Customer_Id = resultCustomer.FirstOrDefault().Customer_Id;
            }


            var resultTeam = db.EmployeeTeams.Where(c => c.Team_ID == team);
            if (resultTeam.Any())
            {
                Supscript.SelectEmp_Team = resultTeam.FirstOrDefault().Team_ID;
            }

            //var resultBranchs = db.SubLedgers.Where(c => c.SubLed_Id == branch);
            //if (resultTeam != null)
            //{
            //    Supscript.Subscrip_Pranch = resultBranchs.First().SubLed_Id;
            //}

            var resultType = db.Subscription_Type.Where(c => c.Subscription_Type_ID == Type);
            if (resultType.Any())
            {
                Supscript.SelectedSubscrip_Type = resultType.FirstOrDefault().Subscription_Type_ID;
            }

            var resultPay = db.PaymentMethods.Where(c => c.PaymentMethod_Id == Methoud);
            if (resultPay.Any())
            {
                Supscript.SelecltPayment_Method = resultPay.FirstOrDefault().PaymentMethod_Id;
            }
            Supscript.Subscrip_Id = mySubs.Subscrip_Id;



            Supscript.Subledger_ID = mySubs.Subledger_ID;
            Supscript.Subscrip_BU = mySubs.Subscrip_BU;
            Supscript.Department_Id = mySubs.Department_Id;
            Supscript.Subscrip_Pranch = mySubs.Subscrip_Pranch;
            if (mySubs.DiscountAllow!=null){
                Supscript.totalinPageonly = mySubs.AmountAfterTax - mySubs.DiscountAllow.Value;
            }
            else
            {
                Supscript.totalinPageonly = mySubs.AmountAfterTax;
            }
            //Supscript.Subscrip_Type = mySubscription.Subscrip_Type;
            Supscript.Subscrip_Description = mySubs.Subscrip_Description;

            Supscript.Estimate_Amount = mySubs.Estimate_Amount;
            Supscript.tel_customer = mySubs.tel_customer;
            Supscript.Subscrip_DateEnd = mySubs.Subscrip_DateEnd;
            Supscript.Customer_Id = (int)mySubs.Customer_Id;
            Supscript.SubType_State = SubType_State.مشترك;
            Supscript.tel_customer = mySubs.tel_customer;
            var type = db.Subscription_Type.Find(mySubs.Sub_Type_ID);
            Supscript.SelectedSubscrip_Type = type.Subscription_Type_ID;
         
            Supscript.Subscrip_State = TransactionState.Pending;
            Supscript.Customer_Name = mySubs.Customer_Name;
            Supscript.Check_Number = mySubs.Check_Number;
            Supscript.Taxe_ID = mySubs.Tax_ID;
            //Supscript.Durationsubscription = mySubscription.Durationsubscription;
            Supscript.EmpList = new SelectList(db.Employees.Where(e => e.Reg_ID != null), "Emp_Id", "Emp_Name");
            Supscript.MyCompList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            Supscript.MyCusromerList = new SelectList(db.Customers.Where(e => e.Customer_State == Customer_State.نشط).ToList(), "Customer_Id", "Customer_Name");
            Supscript.MyEmpTeam = new SelectList(db.EmployeeTeams, "Team_ID", "Team_Name");
            Supscript.MySubTypeList = new SelectList(db.Subscription_Type, "Subscription_Type_ID", "Subscrip_Name");
            Supscript.MyPayMethod = new SelectList(db.PaymentMethods.Where(a=>a.IsActive!=false), "PaymentMethod_Id", "Payment_Type");
            Supscript.MySubscripPranch = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.isActive != false && (a.SubLed_Type == "فرع " || a.SubLed_Type == "قسم رئيسي للشركه")), "SubLed_Id", "SubLed_Name");
            Supscript.MyDepartmentList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.isActive != false && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
            Supscript.MyBuList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.isActive != false && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            Supscript.MySubList = new SelectList(db.SubLedgers.Where(a => a.SubLed_Type == "مركز تكلفه يدوي"&& a.isActive != false), "SubLed_Id", "SubLed_Name");
            var thischartaxParent = db.ChartOfAccountings.Find(48);
            Supscript.tax_List = new SelectList(db.Taxes.Where(e => e.Tax_Code.Contains(thischartaxParent.Account_Code)).Where(a => a.IsActive != false).ToList(), "Tax_ID", "Tax_Name");
            if (mySubs.Discount != null && mySubs.Discount > 0)
            {
                Supscript.Discount = (decimal)mySubs.Discount;
            }
            if (mySubs.DiscountAllow != null && mySubs.DiscountAllow > 0)
            {
                Supscript.AllowDiscount = (decimal)mySubs.DiscountAllow;
            }
            if (edit == true)
            {
                Supscript.StartTime = mySubs.StartTime.ToString("dd-MM-yyyy   HH:mm:ss");
                Supscript.DeadLine = mySubs.DeadLine.ToString("dd-MM-yyyy   HH:mm:ss");
            }
            else
            {
                Supscript.StartTime = mySubs.DeadLine.ToString("dd-MM-yyyy   HH:mm:ss");
                var lastDates = mySubs.DeadLine.AddMonths(type.Durationsubscription);
                Supscript.DeadLine = lastDates.ToString("dd-MM-yyyy   HH:mm:ss");
            }
            return Supscript;
        }

        /// <summary>
        /// Temprary stop for subscription by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string StopSubscription(int id)
        {
            var db = new AccountingContainer();
            var thissub = db.Subscriptions.Find(id);
            thissub.Subscrip_Stop = true;
            thissub.Subscrip_DateEnd = DateTime.Now;
            thissub.SubType_State = SubType_State.ايقاف_مؤقت;
            db.Entry(thissub).State = EntityState.Modified;
            db.SaveChanges();
            db.Dispose();
            return Resources.validate.SubStop;
        }
        /// <summary>
        /// Start subscription again by id and added the missing days to end day of subscription.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string StartSubscription(int id)
        {
            var db = new AccountingContainer();
            var thissub = db.Subscriptions.Find(id);
            thissub.Subscrip_Stop = false;
            DateTime today = DateTime.Now;
            TimeSpan diffrenceDays = Convert.ToDateTime(today).Subtract((DateTime)thissub.Subscrip_DateEnd);
            //var totaldays = diffrenceDays.TotalDays;
            int total = diffrenceDays.Days;
            var x= thissub.DeadLine.AddDays(total);
            thissub.DeadLine = x;
            thissub.SubType_State = SubType_State.مشترك;
            db.Entry(thissub).State = EntityState.Modified;
            db.SaveChanges();
            db.Dispose();
            return Resources.validate.SubActive;
        }
        /// <summary>
        /// Get a list of stopped subscription
        /// </summary>
        /// <returns></returns>
        public static List<Subscription> GetAllSubscriptionstoped()
        {
            var db = new AccountingContainer();
            var supscrip = db.Subscriptions.Where(s => s.Subscrip_Stop == true && s.IsCanceled == false).ToList();
            return supscrip;
        }
        public static void changestate()
        {
            var db = new AccountingContainer();
            var allsubs = db.Subscriptions.Where(s=>s.IsCanceled == false).ToList();
            string connstring = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(connstring);
            conn.Open();

            string sql2 = "  UPDATE Subscriptions SET SubType_State = 3 WHERE    DeadLine < GETDATE() and IsCanceled=0 "


   + " UPDATE Subscriptions SET SubType_State = 2 WHERE DeadLine > GETDATE()and DeadLine<GETDATE()+14 and IsCanceled = 0 ";



            db.Database.ExecuteSqlCommand(sql2);
            //SqlDataReader reader = myCommand2.ExecuteReader();
            //conn.Close();

            //foreach (var sub in allsubs)
            //{
            //    if ((sub.DeadLine - DateTime.Now).TotalDays <= 0)
            //    {
            //        if (sub.SubType_State == SubType_State.على_وشك_الانتهاء || sub.SubType_State == SubType_State.مشترك || sub.SubType_State == SubType_State.ايقاف_مؤقت)
            //        {
            //            sub.SubType_State = SubType_State.منتهى;
            //            db.Entry(sub).State = EntityState.Modified;
            //            db.SaveChanges();
            //            //db.Dispose();

            //        }
            //    }
            //    else if ((sub.DeadLine - DateTime.Now).TotalDays <= 14)
            //    {
            //        sub.SubType_State = SubType_State.على_وشك_الانتهاء;
            //        db.Entry(sub).State = EntityState.Modified;
            //        db.SaveChanges();
            //        //db.Dispose();
            //    }

            //}
        }
        //public static string VoidSubscription(string code)
        //{
        //    var db = new AccountingContainer();

        //    var glId = db.GLMasters.Where(a => a.GLCode == code).FirstOrDefault();
        //    var message =GLMaster.MakeVoidOfPostingGL(glId.GLMaster_Id);
        //    var sub = db.Subscriptions.Where(a => a.Subscrip_Code == code).FirstOrDefault();
        //    sub.SubType_State = SubType_State.منتهى;
        //    db.Entry(sub).State = EntityState.Modified;
        //    db.SaveChanges();

        //    return message;

        //}
        public static string EditSub(SubscriptionVM supscripeVM)
        {
            var db = new AccountingContainer();
            var thissub = db.Subscriptions.Find(supscripeVM.Subscrip_Id);
            thissub.Subscrip_Code = supscripeVM.Subscrip_Code;
            thissub.Subscrip_Description = supscripeVM.Subscrip_Description;
            thissub.DeadLine = Convert.ToDateTime(supscripeVM.DeadLine);
            thissub.StartTime = Convert.ToDateTime(supscripeVM.StartTime);
            thissub.Estimate_Amount = supscripeVM.Estimate_Amount;
            thissub.Customer_Id = supscripeVM.Customer_Id;
            var thiscust = db.Customers.Find(thissub.Customer_Id);
            if (thiscust != null)
            {
                thissub.Customer_Name = thiscust.Customer_Name;
            }
            var resultEmplo = db.Employees.Find(thissub.Emp_Id);
            if (resultEmplo != null)
            {
                thissub.Emp_Id = resultEmplo.Emp_Id;
            }
            thissub.Subscrip_Comp = supscripeVM.Comp_Id;
            thissub.Subscrip_BU = supscripeVM.Subscrip_BU;
            thissub.SubType_State = supscripeVM.SubType_State;
            thissub.Subscrip_Pranch = supscripeVM.Subscrip_Pranch;
            thissub.Emp_Team = supscripeVM.SelectEmp_Team;
            thissub.Payment_Method = supscripeVM.SelecltPayment_Method;
            thissub.tel_customer = supscripeVM.tel_customer;
            thissub.Subscrip_DateEnd = supscripeVM.Subscrip_DateEnd;
            thissub.Sub_Type_ID = supscripeVM.Sub_Type_ID;
            thissub.Check_Number = supscripeVM.Check_Number;
            thissub.Tax_ID = (int)supscripeVM.Taxe_ID;
            if (supscripeVM.Sub_Date!=null)
            {
                thissub.Sub_Date = Convert.ToDateTime(supscripeVM.Sub_Date);
            }
            

            db.Entry(thissub).State = EntityState.Modified;
            db.SaveChanges();
            db.Dispose();
            return Resources.validate.Editsucc;
                   

            
        }
        public static IEnumerable<SelectListItem> GetDepartment(int Branch)
        {
            var db = new AccountingContainer();
            IEnumerable<SelectListItem> Department = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == Branch && p.Subled_Level == 2), "SubLed_Id", "SubLed_Name");
            // = db.Provinces.Where(p => p.CountryId == countryId).ToList();
            return Department;
        }
        public static IEnumerable<SelectListItem> GetBesniessUnet(int BuId)
        {
            var db = new AccountingContainer();
            IEnumerable<SelectListItem> Besniss = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == BuId && p.Subled_Level == 3), "SubLed_Id", "SubLed_Name");
            // = db.Provinces.Where(p => p.CountryId == countryId).ToList();
            return Besniss;
        }
        public static decimal GetsubByCode(string code)
        {
            var db = new AccountingContainer();

            var PayedAmount = db.Subscriptions.Where(e => e.Subscrip_Code == code||e.Number==code).ToList().FirstOrDefault();
            if (PayedAmount==null)
            {
                return 0;
            }
            return PayedAmount.Payed_Amount.Value;
        }
        public static Subscription GetsubByCode1(string code)
        {
            var db = new AccountingContainer();

            var sub = db.Subscriptions.Where(e => e.Subscrip_Code == code).ToList().FirstOrDefault();
            return sub;
        }

        /// from  here  we  start  the  Subscriptions  that  needs no Process  with  Money
        public static List<SubscriptionTemp> GetsubsofTemp()
        {
            var db = new AccountingContainer();
            var thisSUbs = db.Subscriptions.Where(e => (e.SubType_State == SubType_State.على_وشك_الانتهاء || e.SubType_State == SubType_State.منتهى) &&e.Subscrip_State==TransactionState.Post).ToList();
            var notneededSubs = db.Subscriptions.Where(e =>e.SubType_State == SubType_State.مشترك || e.SubType_State == SubType_State.ايقاف_مؤقت).ToList();
            var pendingsubs = db.Subscriptions.Where(e => e.Subscrip_State == TransactionState.Pending).ToList();
            foreach (var sub in thisSUbs)
            {
                var thistemp = db.SubscriptionTemps.Where(e => e.Subscrip_Id == sub.Subscrip_Id).ToList();
                if (thistemp.Count > 0)
                {

                }
                else
                {
                    var subtemp = new SubscriptionTemp();
                    subtemp.Subscrip_Id = sub.Subscrip_Id;
                    //if (sub.Discount == 100 && sub.Discount != null)
                    //{
                    //    var TotalDiscount = (sub.Estimate_Amount * (sub.Discount / 100));
                    //    var TotalAfterDiscount = sub.Estimate_Amount - TotalDiscount;
                    //    if (sub.Tax_ID > 0)
                    //    {
                    //        var taxresult = db.Taxes.Find(sub.Tax_ID);
                    //        var totalsoftax = sub.Estimate_Amount * (taxresult.Tax_Percentage / 100);
                    //        subtemp.AmountAfterTax = (decimal)TotalAfterDiscount + totalsoftax;
                    //    }
                    //    else
                    //    {
                    //        subtemp.AmountAfterTax = (decimal)TotalAfterDiscount;
                    //    }
                    //}
                    //else
                    //{
                    //    subtemp.AmountAfterTax = sub.AmountAfterTax;
                    //}
                    if (sub.Discount == 100)
                    {
                        subtemp.Discount = 0;
                        if (sub.Tax_ID > 0)
                        {
                            var taxresult = db.Taxes.Find(sub.Tax_ID);
                            var totalsoftax = sub.Estimate_Amount * (taxresult.Tax_Percentage / 100);
                            subtemp.AmountAfterTax = (decimal)sub.Estimate_Amount + totalsoftax;
                        }
                        else
                        {
                            subtemp.AmountAfterTax = sub.Estimate_Amount;
                        }
                    }
                    else
                    {
                        subtemp.Discount = sub.Discount;
                        subtemp.AmountAfterTax = sub.AmountAfterTax;
                    }
                    subtemp.Check_Number = sub.Check_Number;
                    subtemp.Customer_Id = sub.Customer_Id;
                    subtemp.Customer_Name = sub.Customer_Name;
                    subtemp.DeadLine = sub.DeadLine;
                    subtemp.Department_Id = sub.Department_Id;
            
                    subtemp.DiscountAllow = sub.DiscountAllow;
                    subtemp.Emp_Team = sub.Emp_Team;
                    subtemp.Estimate_Amount = sub.Estimate_Amount;
                    subtemp.Payed_Amount = sub.Payed_Amount;
                    subtemp.Payment_Method = sub.Payment_Method;
                    subtemp.StartTime = sub.StartTime;
                    subtemp.Subledger_ID = sub.Subledger_ID;
                    subtemp.Subscrip_BU = sub.Subscrip_BU;
                    subtemp.IsCanceled = false;
                    var lastSerial = db.SubscriptionTemps.OrderByDescending(c => c.Id).FirstOrDefault();
                    if (lastSerial == null)
                    {
                        subtemp.Subscrip_Code = "PSUBS-001";
                    }
                    else
                    {
                        //using string substring method to get the number of the last inserted
                        subtemp.Subscrip_Code = "PSUBS-" + (Convert.ToInt32(lastSerial.Subscrip_Code.Substring(6, lastSerial.Subscrip_Code.Length - 6)) + 1).ToString("D3");
                    }
                    subtemp.Subscrip_Comp = sub.Subscrip_Comp;
                    subtemp.Subscrip_DateEnd = sub.Subscrip_DateEnd;
                    subtemp.Subscrip_Description = sub.Subscrip_Description;
                    subtemp.Subscrip_Pranch = sub.Subscrip_Pranch;
                    subtemp.Subscrip_State = sub.Subscrip_State;
                    subtemp.Subscrip_Stop = sub.Subscrip_Stop;
                    subtemp.Subscrip_Type = sub.Subscrip_Type;
                    subtemp.SubType_State = sub.SubType_State;
                    subtemp.Sub_Pay_State = sub.Sub_Pay_State;
                    subtemp.Sub_Type_ID = sub.Sub_Type_ID;
                    subtemp.Sub_Date = sub.Sub_Date;
                    subtemp.Tax_ID = sub.Tax_ID;
                    subtemp.tel_customer = sub.tel_customer;
                    db.SubscriptionTemps.Add(subtemp);
                    db.SaveChanges();
                }
               
                
            }
            foreach (var nonsub in notneededSubs)
            {
                var thistemp = db.SubscriptionTemps.Where(e => e.Subscrip_Id == nonsub.Subscrip_Id).ToList();
                if (thistemp.Count > 0)
                {
                    foreach (var reSub in thistemp)
                    {
                        db.SubscriptionTemps.Remove(reSub);
                        var thistempitems = db.SubscriptionItemsTemps.Where(e => e.SubTempID == reSub.Id).ToList();
                        if (thistempitems.Count > 0)
                        {
                            foreach (var item in thistempitems)
                            {
                                db.SubscriptionItemsTemps.Remove(item);
                            }
                        }
                        db.SaveChanges();
                    }
                }
                else
                {
                }
             }
            foreach (var nonsubs in pendingsubs)
            {
                var thistemp = db.SubscriptionTemps.Where(e => e.Subscrip_Id == nonsubs.Subscrip_Id).ToList();
                if (thistemp.Count > 0)
                {
                    foreach (var reSubs in thistemp)
                    {
                        db.SubscriptionTemps.Remove(reSubs);
                        var thistempitems = db.SubscriptionItemsTemps.Where(e => e.SubTempID == reSubs.Id).ToList();
                        if (thistempitems.Count > 0)
                        {
                            foreach (var item in thistempitems)
                            {
                                db.SubscriptionItemsTemps.Remove(item);
                            }
                        }
                        db.SaveChanges();
                    }
                }
                else
                {
                }
            }
            var tempsubs = db.SubscriptionTemps.ToList();
            foreach (var temp in tempsubs)
            {
                var thissub = db.Subscriptions.Find(temp.Subscrip_Id);
                if (thissub.NewSubCode!=null)
                {
                    db.SubscriptionTemps.Remove(temp);
                    var thistempitems = db.SubscriptionItemsTemps.Where(e => e.SubTempID == temp.Id).ToList();
                    if (thistempitems.Count > 0)
                    {
                        foreach (var item in thistempitems)
                        {
                            db.SubscriptionItemsTemps.Remove(item);
                        }
                    }
                    db.SaveChanges();
                }
                else
                {

                }
            }
           
            var tempsub = db.SubscriptionTemps.Where(s=>s.IsCanceled == false && (s.SubType_State == SubType_State.على_وشك_الانتهاء || s.SubType_State == SubType_State.منتهى )).ToList();
            return tempsub;
        }
        public static SubscriptionTempVM EditSubTempForCreation(int SubtempID)
        {
            var db = new AccountingContainer();
            var mySubs = db.SubscriptionTemps.Find(SubtempID);
            var BuId = mySubs.Subscrip_BU;
            var company = mySubs.Subscrip_Comp;
            var customer = mySubs.Customer_Id;
            var team = mySubs.Emp_Team;
            var branch = mySubs.Subscrip_Pranch;
            var Type = mySubs.Subscrip_Type;
            var Methoud = mySubs.Payment_Method;
            var Supscript = new SubscriptionTempVM();
            var resultBussinesUnit = db.BussinesUnits.Where(B => B.BU_Id == BuId);
            if (resultBussinesUnit.Any())
            {
                Supscript.Subscrip_BU = resultBussinesUnit.FirstOrDefault().BU_Id;
            }
            var resultCompany = db.Companies.Where(C => C.Comp_Id == company);
            if (resultCompany.Any())
            {
                Supscript.Comp_Id = resultCompany.FirstOrDefault().Comp_Id;
            }
            var resultCustomer = db.Customers.Where(c => c.Customer_Id == customer);
            if (resultCustomer.Any())
            {
                Supscript.Customer_Id = resultCustomer.FirstOrDefault().Customer_Id;
            }
            var resultTeam = db.EmployeeTeams.Where(c => c.Team_ID == team);
            if (resultTeam.Any())
            {
                Supscript.SelectEmp_Team = resultTeam.FirstOrDefault().Team_ID;
            }
            var resultType = db.Subscription_Type.Where(c => c.Subscription_Type_ID == Type);
            if (resultType.Any())
            {
                Supscript.SelectedSubscrip_Type = resultType.FirstOrDefault().Subscription_Type_ID;
            }

            var resultPay = db.PaymentMethods.Where(c => c.PaymentMethod_Id == Methoud);
            if (resultPay.Any())
            {
                Supscript.SelecltPayment_Method = resultPay.FirstOrDefault().PaymentMethod_Id;
            }
            Supscript.Subscrip_Id = mySubs.Subscrip_Id;
            Supscript.Subledger_ID = mySubs.Subledger_ID;
            Supscript.Subscrip_BU = mySubs.Subscrip_BU;
            Supscript.Department_Id = mySubs.Department_Id;
            Supscript.Subscrip_Pranch = mySubs.Subscrip_Pranch;
            Supscript.Subscrip_Description = mySubs.Subscrip_Description;
            Supscript.Estimate_Amount = mySubs.Estimate_Amount;
            Supscript.tel_customer = mySubs.tel_customer;
            Supscript.Subscrip_DateEnd = mySubs.Subscrip_DateEnd;
            Supscript.Customer_Id = (int)mySubs.Customer_Id;
            Supscript.SubType_State = SubType_State.مشترك;
            Supscript.tel_customer = mySubs.tel_customer;
            var type = db.Subscription_Type.Where(e => e.Durationsubscription == mySubs.Subscrip_Type && e.Subscrip_Name == mySubs.Subscrip_Description).ToList().FirstOrDefault();
            Supscript.SelectedSubscrip_Type = type.Subscription_Type_ID;
            Supscript.StartTime = mySubs.StartTime.ToString("dd-MM-yyyy   HH:mm:ss");
            Supscript.DeadLine = mySubs.DeadLine.ToString("dd-MM-yyyy   HH:mm:ss");
            Supscript.Subscrip_State = TransactionState.Pending;
            Supscript.Customer_Name = mySubs.Customer_Name;
            Supscript.Check_Number = mySubs.Check_Number;
            Supscript.Taxe_ID = mySubs.Tax_ID;
            Supscript.Subscrip_Id = mySubs.Subscrip_Id;
            Supscript.Id = mySubs.Id;
            Supscript.Subscrip_Code = mySubs.Subscrip_Code;
            //Supscript.Durationsubscription = mySubscription.Durationsubscription;
            Supscript.EmpList = new SelectList(db.Employees.Where(e => e.Reg_ID != null), "Emp_Id", "Emp_Name");
            Supscript.MyCompList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            Supscript.MyCusromerList = new SelectList(db.Customers.Where(e => e.Customer_State == Customer_State.نشط).ToList(), "Customer_Id", "Customer_Name");
            Supscript.MyEmpTeam = new SelectList(db.EmployeeTeams, "Team_ID", "Team_Name");
            Supscript.MySubTypeList = new SelectList(db.Subscription_Type, "Subscription_Type_ID", "Subscrip_Name");
            Supscript.MyPayMethod = new SelectList(db.PaymentMethods, "PaymentMethod_Id", "Payment_Type");
            Supscript.MySubscripPranch = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && (a.SubLed_Type == "فرع " || a.SubLed_Type == "قسم رئيسي للشركه")), "SubLed_Id", "SubLed_Name");
            Supscript.MyDepartmentList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
            Supscript.MyBuList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            Supscript.MySubList = new SelectList(db.SubLedgers.Where(a => a.SubLed_Type == "مركز تكلفه يدوي"), "SubLed_Id", "SubLed_Name");
            var thischartaxParent = db.ChartOfAccountings.Find(48);
            Supscript.tax_List = new SelectList(db.Taxes.Where(e => e.Tax_Code.Contains(thischartaxParent.Account_Code)).Where(a => a.IsActive != false).ToList(), "Tax_ID", "Tax_Name");
            if (mySubs.Discount != null && mySubs.Discount > 0)
            {
                if ((decimal)mySubs.Discount == 100)
                {
                    Supscript.Discount = 0;
                }
                else
                {
                    Supscript.Discount = (decimal)mySubs.Discount;
                }
               
            }
            if (mySubs.DiscountAllow != null && mySubs.DiscountAllow > 0)
            {
                if (mySubs.Discount != null && mySubs.Discount > 0)
                {
                    if ((decimal)mySubs.Discount == 100)
                    {
                        Supscript.AllowDiscount = 0;
                    }
                }
                else
                {
                    Supscript.AllowDiscount = (decimal)mySubs.DiscountAllow;
                }
               
            }
            if (mySubs.DiscountAllow != null)
            {
                Supscript.totalinPageonly = mySubs.Estimate_Amount - mySubs.DiscountAllow.Value;
            
            }
            else
            {
                Supscript.totalinPageonly = mySubs.Estimate_Amount;
            }
          
            return Supscript;
        }
        public static string EditSubscriptionTemp(SubscriptionTempVM SupscripeVM)
        {
            var db = new AccountingContainer();

            var NewSubscription = db.SubscriptionTemps.Find(SupscripeVM.Id);
         
                if (SupscripeVM != null)
                {

                    Tax resulttaxe = null;



                    if (SupscripeVM.Taxe_ID != null && SupscripeVM.Taxe_ID != 0)
                    {
                        NewSubscription.Tax_ID = SupscripeVM.Taxe_ID.Value;
                        resulttaxe = db.Taxes.Find(SupscripeVM.Taxe_ID);
                    }
                    else
                    {
                        NewSubscription.Tax_ID = 0;
                    }
                    var resultCompany = db.Companies.Find(SupscripeVM.Comp_Id);
                    if (resultCompany != null)
                    {
                        NewSubscription.Subscrip_Comp = resultCompany.Comp_Id;

                    }
                    var resulCustomer = db.Customers.Find(SupscripeVM.Customer_Id);
                    if (resulCustomer != null)
                    {

                        NewSubscription.Customer_Id = resulCustomer.Customer_Id;
                        NewSubscription.Customer_Name = resulCustomer.Customer_Name;
                    }


                    var resulEmp_team = db.EmployeeTeams.Find(SupscripeVM.SelectEmp_Team);
                    if (resulEmp_team != null)
                    {
                        NewSubscription.Emp_Team = resulEmp_team.Team_ID;
                    }
                    var resultType = db.Subscription_Type.Find(SupscripeVM.SelectedSubscrip_Type);
                    if (resultType != null)
                    {
                        NewSubscription.Subscrip_Type = resultType.Durationsubscription;
                        NewSubscription.Subscrip_Description = resultType.Subscrip_Name;
                        NewSubscription.Subscrip_Comp = resultType.Subscrip_Company;
                        NewSubscription.Sub_Type_ID = resultType.Subscription_Type_ID;

                        if (resultType.Subscrip_BU == null || resultType.Subscrip_BU == 0)
                        {
                            NewSubscription.Subscrip_BU = 0;
                        }
                        else
                        {
                            NewSubscription.Subscrip_BU = (int)resultType.Subscrip_BU;
                        }
                    }
                    var resultMethod = db.PaymentMethods.Find(SupscripeVM.SelecltPayment_Method);
                    if (resultMethod != null)
                    {
                        NewSubscription.Payment_Method = resultMethod.PaymentMethod_Id;
                    }
                    var resultEmployee = db.Employees.Find(SupscripeVM.Emp_Id);
                    if (resultEmployee != null)
                    {
                        NewSubscription.Emp_Id = resultEmployee.Emp_Id;
                    }
                    var subResult = db.SubLedgers.Find(SupscripeVM.Subledger_ID);
                    if (subResult != null)
                    {
                        NewSubscription.Subledger_ID = subResult.SubLed_Id;
                    }
                    else
                    {
                        if (SupscripeVM.Subscrip_BU != null && SupscripeVM.Subscrip_BU != 0)
                        {
                            NewSubscription.Subledger_ID = (int)SupscripeVM.Subscrip_BU;
                            NewSubscription.Subscrip_BU = SupscripeVM.Subscrip_BU;
                        }
                        else if ((SupscripeVM.Subscrip_BU == null || SupscripeVM.Subscrip_BU == 0) && (SupscripeVM.Department_Id != null && SupscripeVM.Department_Id != 0))
                        {
                            NewSubscription.Subledger_ID = (int)SupscripeVM.Department_Id;
                            NewSubscription.Department_Id = SupscripeVM.Department_Id;
                        }
                        else if ((SupscripeVM.Subscrip_BU == null || SupscripeVM.Subscrip_BU == 0) && (SupscripeVM.Department_Id == null || SupscripeVM.Department_Id == 0) && (SupscripeVM.Subscrip_Pranch != null && SupscripeVM.Subscrip_Pranch != 0))
                        {
                            NewSubscription.Subledger_ID = (int)SupscripeVM.Subscrip_Pranch;
                            NewSubscription.Subscrip_Pranch = SupscripeVM.Subscrip_Pranch;
                        }
                        else
                        {
                            var thissub = db.SubLedgers.Where(e => e.Subled_Level == 0).FirstOrDefault();
                            NewSubscription.Subledger_ID = (int)thissub.SubLed_Id;
                        }
                    }
                    NewSubscription.Subscrip_BU = SupscripeVM.Subscrip_BU;
                    NewSubscription.Department_Id = SupscripeVM.Department_Id;
                    NewSubscription.Subscrip_Pranch = SupscripeVM.Subscrip_Pranch;
                    NewSubscription.Sub_Date =Convert.ToDateTime( SupscripeVM.Sub_Date);
                    NewSubscription.Subscrip_State = TransactionState.Pending;
                    // NewSubscription.Subscrip_Type = SupscripeVM.Subscrip_Type;
                    NewSubscription.SubType_State = SubType_State.مشترك;
                    NewSubscription.tel_customer = SupscripeVM.tel_customer;
             
                    NewSubscription.Discount = SupscripeVM.Discount;
                    NewSubscription.DiscountAllow = SupscripeVM.AllowDiscount;
                  
                    NewSubscription.StartTime = Convert.ToDateTime(SupscripeVM.StartTime);
                    NewSubscription.Estimate_Amount = SupscripeVM.Estimate_Amount;
                    NewSubscription.DeadLine = Convert.ToDateTime(SupscripeVM.DeadLine);
                    NewSubscription.Sub_Date= Convert.ToDateTime(SupscripeVM.Sub_Date);
                    var TotalDiscount = (NewSubscription.Estimate_Amount * (NewSubscription.Discount / 100));
                    var TotalAfterDiscount = NewSubscription.Estimate_Amount - TotalDiscount;
                if (NewSubscription.Tax_ID == 0)
                {
                    if (NewSubscription.DiscountAllow != null && NewSubscription.DiscountAllow > 0)
                    {
                        NewSubscription.AmountAfterTax = TotalAfterDiscount.Value - (NewSubscription.DiscountAllow.Value);
                    }
                    else
                    {
                        NewSubscription.AmountAfterTax = TotalAfterDiscount.Value;
                    }
                }
                else
                {
                    NewSubscription.AmountAfterTax = (TotalAfterDiscount.Value + (TotalAfterDiscount.Value * (resulttaxe.Tax_Percentage / 100)) - (NewSubscription.DiscountAllow.Value));
                }
              
                    //var mm = db.Subscription_Type.Find(SupscripeVM.SelectedSubscrip_Type);

                    NewSubscription.DeadLine = Convert.ToDateTime(SupscripeVM.DeadLine);
                    NewSubscription.Payed_Amount = 0;
                    if (SupscripeVM.Emp_Id == null)
                    {
                        NewSubscription.Emp_Id = 1;
                    }
                //NewSubscription.DeadLine = DateTime.Now.AddMonths(SupscripeVM.SelectedSubscrip_Type);
                //Subscript.DeadLine = DateTime.Now.AddMonths(subscVM.Durationsubscription);
                NewSubscription.Check_Number = "3434";
                
                db.Entry(NewSubscription).State = EntityState.Modified;
                  


                        db.SaveChanges();
                
                    


                    return Resources.validate.Editsucc;
                }

            
            
            return null;

        }
        public static SubscriptionItemsTempVM prepareitemsofSubtemp()
        {
            var newitemtemp = new SubscriptionItemsTempVM();
            return newitemtemp;
        }
        public static string addingItemsinTemp(SubscriptionItemsTempVM SubtVM)
        {
            var db = new AccountingContainer();
            try
            {
                var newtemp = new SubscriptionItemsTemp();
                newtemp.Item_Name = SubtVM.Item_Name;
                newtemp.Item_price = (decimal)SubtVM.Item_price;
                newtemp.Item_Quantity = (int)SubtVM.Item_Quantity;
                newtemp.SubTempID = SubtVM.SubTempID;
                db.SubscriptionItemsTemps.Add(newtemp);
                db.SaveChanges();
                db.Dispose();
                return "تم اضافة الصنف";
            }
            catch (Exception ex)
            {
                return "يوجد خطء في البيانات";
            }
       
        }
        public static List<SubscriptionItemsTemp> getListofTemp(int tempID)
        {
            var db = new AccountingContainer();
            var thisitemList = db.SubscriptionItemsTemps.Where(e => e.SubTempID == tempID).ToList();
            return thisitemList;
        }
        public static string deletefromListofTemp(int id)
        {
            var db = new AccountingContainer();
            var thisitemList = db.SubscriptionItemsTemps.Find(id);
            db.SubscriptionItemsTemps.Remove(thisitemList);
            db.SaveChanges();
            db.Dispose();
            return Resources.validate.DeleteSuccessful;
        }
        public static SubscriptionTemp getDetailsofSubTemp(int tempID)
        {
            var db = new AccountingContainer();
            var thistemp = db.SubscriptionTemps.Find(tempID);
            return thistemp;
        }
        public static List<SubscriptionItemsTemp> getItemTemps(int TempID)
        {
            var db = new AccountingContainer();
            var subItems = db.SubscriptionItemsTemps.Where(e => e.SubTempID == TempID).ToList();
            List<SubscriptionItemsTemp> productsJArr = new List<SubscriptionItemsTemp>();
            int counter = 0;
            foreach (var item in subItems)
            {
                productsJArr.Add(item);
            }
            return productsJArr.ToList();
        }
        public static string DeleteSupInvoiceWithGL(int id )
        {
            var db = new AccountingContainer();
            var invoicesub = db.SubscriptionInvoices.Find(id);
            var gl = db.GLMasters.Where(a => a.GLCode == invoicesub.Serial_Number);
            if (gl.Count() == 1)
            {
                VoidInvoice(id);
            }
            var gl2 = db.GLMasters.Where(a => a.GLCode == invoicesub.Serial_Number);
            foreach(var gldelete in gl2.ToList())
            {
                db.GLDetails.RemoveRange(gldelete.GLDetails);
                db.SaveChanges();
                db.GLMasters.Remove(gldelete);
                db.SaveChanges();
            }
            db.SubscriptionInvoices.Remove(invoicesub);
            db.SaveChanges();
            return Resources.validate.DeleteSuccessful;
        }
        public static decimal getAllPayForthatInvoicebyCode( string subCode)
        {
            var db = new AccountingContainer();
            var sub = db.Subscriptions.Where(a=>a.Subscrip_Code== subCode||a.Number== subCode).FirstOrDefault();
            var Reciptlist = db.SubscriptionInvoices.Where(a => a.Subscrip_Code == sub.Subscrip_Code && a.Transaction_State == TransactionState.Post).ToList();
            var Recipt = db.SubscriptionInvoices.Where(a => a.Subscrip_Code == sub.Subscrip_Code && a.Transaction_State == TransactionState.Post).Select(a => a.Pay_Amount).ToList().Sum();
            return Recipt;
        }

    }
}
