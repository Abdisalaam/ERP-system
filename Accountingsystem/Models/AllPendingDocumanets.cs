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
    public class AllPendingDocumanets
    {
        // this Method  is  to get  all pending Documents  From  wholeover  the  System  
        //we  start  by  intialize  a  List  and  then  get  list  of  each section ,getting  its  Documents which  is  pending  and  add  it 
        //to  this  list and  then  show  this  list

        public static List<AllSectionPendingVM> GetAllPendings()
        {
            var db = new AccountingContainer();
            List<AllSectionPendingVM> sectionList = new List<AllSectionPendingVM>();

            var purchoinv = db.PurInvoiceMasters.Where(e => e.Invoice_State == TransactionState.Pending).ToList();
            if (purchoinv != null)
            {
                foreach (var purchaseinv in purchoinv)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = purchaseinv.Master_Id;
                    thisdetaisl.Serial = purchaseinv.Serial_Number;
                    thisdetaisl.Description = purchaseinv.Description;
                    thisdetaisl.TotalAmount = purchaseinv.Total_Cost;
                    thisdetaisl.date = purchaseinv.Invoice_Date;
                    thisdetaisl.status = purchaseinv.Invoice_State;
                    sectionList.Add(thisdetaisl);
                }

            }
            var purchaseRecall = db.RecalPurchInvoiceMasters.Where(e => e.PI_State == TransactionState.Pending).ToList();
            if (purchaseRecall != null)
            {
                foreach (var recal in purchaseRecall)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = recal.Master_Id;
                    thisdetaisl.Serial = recal.Recall_MPurch_Serial;
                    thisdetaisl.Description = recal.Doc_Type;
                    thisdetaisl.TotalAmount = recal.Inv_TotalCost;
                    thisdetaisl.date = recal.Register_Date;
                    thisdetaisl.status = TransactionState.Pending;

                    sectionList.Add(thisdetaisl);
                }
            }
            var Payments = db.Payments.Where(e => e.Pay_State == TransactionState.Pending).ToList();
            if (Payments != null)
            {
                foreach (var pay in Payments)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = pay.Pay_ID;
                    thisdetaisl.Serial = pay.Pay_Number;
                    thisdetaisl.Description = pay.Rec_Description;
                    thisdetaisl.TotalAmount = pay.Pay_Amount;
                    thisdetaisl.date = pay.Pay_Date;
                    thisdetaisl.status = pay.Pay_State;
                    sectionList.Add(thisdetaisl);
                }
            }
            var TransToMaintentance = db.TransferToMaintenances.Where(e => e.Invoice_State == TransactionState.Pending).ToList();
            if (TransToMaintentance != null)
            {
                foreach (var trans in TransToMaintentance)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = trans.Master_Id;
                    thisdetaisl.Serial = trans.Trans_Code;
                    thisdetaisl.Description = trans.Describtion;
                    thisdetaisl.TotalAmount = trans.Total_Amount;
                    thisdetaisl.date = trans.Trans_Date;
                    thisdetaisl.status = trans.Invoice_State;
                    sectionList.Add(thisdetaisl);
                }
            }
            var ProjectInv = db.ProjectInvoiceMasters.Where(e => e.Project_State == TransactionState.Pending).ToList();
            if (ProjectInv != null)
            {
                foreach (var proj in ProjectInv)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = proj.PInvoiceMaster_ID;
                    thisdetaisl.Serial = proj.Serial_Number;
                    thisdetaisl.Number = proj.Number;
                    thisdetaisl.Description = proj.Project_Description;
                    thisdetaisl.TotalAmount = proj.ProjectInvoiceDetails.Select(a=>a.Total_Amount).Sum();
                    thisdetaisl.status = proj.Project_State;
                    thisdetaisl.date = proj.StartTime;

                    sectionList.Add(thisdetaisl);
                }
            }
            var projectPay = db.ProjectPayments.Where(e => e.ProjectPay_State == TransactionState.Pending).ToList();
            if (projectPay != null)
            {
                foreach (var projP in projectPay)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = projP.ProjectPaymentId;
                    thisdetaisl.Serial = projP.ProjectPay_Code;
                    thisdetaisl.Description = projP.Description;
                    thisdetaisl.TotalAmount = projP.Pay_Amount;
                    thisdetaisl.date = Convert.ToDateTime(projP.Date);
                    thisdetaisl.status = projP.ProjectPay_State;
                    sectionList.Add(thisdetaisl);
                }
            }
            var transtoProj = db.TransferToProjectMasters.Where(e => e.Invoice_State == TransactionState.Pending).ToList();
            if (transtoProj != null)
            {
                foreach (var transToPrj in transtoProj)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = transToPrj.Master_Id;
                    thisdetaisl.Serial = transToPrj.Trans_Code;
                    thisdetaisl.Description = transToPrj.Describtion;
                    thisdetaisl.TotalAmount = transToPrj.Total_Amount;
                    thisdetaisl.date = transToPrj.Trans_Date;
                    thisdetaisl.status = transToPrj.Invoice_State;
                    sectionList.Add(thisdetaisl);
                }
            }
            var invMast = db.InvoiceMasters.Where(e => e.Invoice_State == TransactionState.Pending||e.Invoice_State==TransactionState.Hold).ToList();
            if (invMast != null)
            {
                foreach (var sinv in invMast)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = sinv.Master_Id;
                    thisdetaisl.Serial = sinv.Serial_Number;
                    thisdetaisl.Number = sinv.Number;
                    thisdetaisl.Description = sinv.Description;
                    thisdetaisl.TotalAmount = sinv.Total_Amount;
                    thisdetaisl.date = sinv.Invoice_Date;
                    thisdetaisl.status = sinv.Invoice_State;
                    sectionList.Add(thisdetaisl);
                }
            }
            var SalesInvm = db.SalesReceiptMasters.Where(e => e.Invoice_State == TransactionState.Pending || e.Invoice_State == TransactionState.Hold).ToList();
            if (SalesInvm != null)
            {
                foreach (var Salesinv in SalesInvm)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = Salesinv.Master_Id;
                    thisdetaisl.Number = Salesinv.Number;
                    thisdetaisl.Serial = Salesinv.Serial_Number;
                    thisdetaisl.Description = Salesinv.Description;
                    thisdetaisl.TotalAmount = Salesinv.Total_Amount;
                    thisdetaisl.date = Salesinv.Invoice_Date;
                    thisdetaisl.status = Salesinv.Invoice_State;
                    sectionList.Add(thisdetaisl);
                }
            }
            var invtransHD = db.InventoryTransfer_Hd.Where(e => e.Tranfer_State == TransactionState.Pending).ToList();
            if (invtransHD != null)
            {
                foreach (var Saleinvtranssinv in invtransHD)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = Saleinvtranssinv.TransferHd_Id;
                    thisdetaisl.Serial = Saleinvtranssinv.Inv_Code;
                    thisdetaisl.Description = Saleinvtranssinv.InvTransfere_Notes;
                    thisdetaisl.TotalAmount = (decimal)Saleinvtranssinv.Total_Cost;
                    thisdetaisl.date = Saleinvtranssinv.Date;
                    thisdetaisl.status = Saleinvtranssinv.Tranfer_State;
                    sectionList.Add(thisdetaisl);
                }
            }
            var RevSalesInv = db.ReverseSalesInvoiceMasters.Where(e => e.Invoice_State == TransactionState.Pending).ToList();
            if (RevSalesInv != null)
            {
                foreach (var revSal in RevSalesInv)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = revSal.Master_Id;
                    thisdetaisl.Number = revSal.Number;
                    thisdetaisl.Serial = revSal.Serial_Number;
                    thisdetaisl.Description = revSal.Description;
                    thisdetaisl.TotalAmount = (decimal)revSal.Total_Amount;
                    thisdetaisl.date = revSal.Invoice_Date;
                    thisdetaisl.status = revSal.Invoice_State;
                    sectionList.Add(thisdetaisl);
                }
            }
            var RevSalesRec = db.ReverseSalesReceiptMasters.Where(e => e.Invoice_State == TransactionState.Pending).ToList();
            if (RevSalesRec != null)
            {
                foreach (var revSalR in RevSalesRec)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = revSalR.Master_Id;
                    thisdetaisl.Number = revSalR.Number;
                    thisdetaisl.Serial = revSalR.Serial_Number;
                    thisdetaisl.Description = revSalR.Description;
                    thisdetaisl.TotalAmount = (decimal)revSalR.Total_Amount;
                    thisdetaisl.date = revSalR.Invoice_Date;
                    thisdetaisl.status = revSalR.Invoice_State;
                    sectionList.Add(thisdetaisl);
                }
            }

            var Expense = db.Expenses.Where(e => e.Expense_State == TransactionState.Pending).ToList();
            if (Expense != null)
            {
                foreach (var exp in Expense)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = exp.Expense_Id;
                    thisdetaisl.Serial = exp.Expense_Code;
                    thisdetaisl.Description = exp.Expense_Description;
                    thisdetaisl.status = exp.Expense_State;

                    thisdetaisl.TotalAmount = exp.Expense_Value;
                    
                    thisdetaisl.date = exp.Expense_date;
                    sectionList.Add(thisdetaisl);
                }



            }
            var catchRecipt = db.CatchReceipts.Where(e => e.CatchReceipt_State == TransactionState.Pending).ToList();

            if (catchRecipt != null)
            {
                foreach (var catchre in catchRecipt)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = catchre.CatchReceipt_Id;
                    thisdetaisl.Serial = catchre.CatchReceipt_Code;
                    thisdetaisl.Description = catchre.CatchReceipt_Description;
                    thisdetaisl.TotalAmount = (decimal)catchre.CatchReceipt_Value;
                    thisdetaisl.date = catchre.CatchReceipt_date;
                    thisdetaisl.status = catchre.CatchReceipt_State;
                    sectionList.Add(thisdetaisl);
                }



            }
            var spentRecipt = db.SpentRecipts.Where(e => e.SpentReceipt_State == TransactionState.Pending).ToList();

            if (spentRecipt != null)
            {
                foreach (var spent in spentRecipt)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = spent.SpentReceipt_Id;
                    thisdetaisl.Serial = spent.SpentReceipt_Code;
                    thisdetaisl.Description = spent.SpentReceipt_Description;
                    thisdetaisl.TotalAmount = (decimal)spent.SpentReceipt_Value;
                    thisdetaisl.date = spent.CatchReceipt_date;
                    thisdetaisl.status = spent.SpentReceipt_State;
                    sectionList.Add(thisdetaisl);
                }



            }
            var Subscrip = db.Subscriptions.Where(e => e.Subscrip_State == TransactionState.Pending).ToList();

            if (Subscrip != null)
            {
                foreach (var sub in Subscrip)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = sub.Subscrip_Id;
                    thisdetaisl.Number = sub.Number;
                    thisdetaisl.Serial = sub.Subscrip_Code;
                    thisdetaisl.Description = sub.Subscrip_Description;
                    thisdetaisl.TotalAmount = (decimal)sub.AmountAfterTax;
                    thisdetaisl.date = sub.StartTime;
                    thisdetaisl.status = sub.Subscrip_State;
                    sectionList.Add(thisdetaisl);
                }



            }
            var recipt = db.Receipts.Where(e => e.Rec_state == TransactionState.Pending).ToList();

            if (recipt != null)
            {
                foreach (var rec in recipt)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = rec.Rec_ID;
                    thisdetaisl.Serial = rec.Rec_Number;
                    thisdetaisl.Description = rec.Rec_Description;
                    thisdetaisl.TotalAmount = (decimal)rec.Rec_Amount;
                    thisdetaisl.date = rec.Rec_Date;
                    thisdetaisl.status = rec.Rec_state;
                    sectionList.Add(thisdetaisl);
                }

            }
            var reciptPOS = db.POSReceipts.Where(e => e.Rec_state == TransactionState.Pending).ToList();

            if (reciptPOS != null)
            {
                foreach (var rec in reciptPOS)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = rec.Rec_ID;
                    thisdetaisl.Serial = rec.Rec_Number;
                    thisdetaisl.Description = rec.Rec_Description;
                    thisdetaisl.TotalAmount = (decimal)rec.Rec_Amount;
                    thisdetaisl.date = rec.Rec_Date;
                    thisdetaisl.status = rec.Rec_state;
                    sectionList.Add(thisdetaisl);
                }

            }

            var settlement = db.SettlementOfStores.Where(e => e.Settlement_State == TransactionState.Pending).ToList();

            if (settlement != null)
            {
                foreach (var rec in settlement)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = rec.SettlementId;
                    thisdetaisl.Serial = rec.Settlement_Code;
                    thisdetaisl.Description = "تسوية";
                    thisdetaisl.TotalAmount = rec.Total_Cost_Credit + rec.Total_Cost_Debit;
                    thisdetaisl.date = DateTime.Now;
                    thisdetaisl.status = rec.Settlement_State;
                    sectionList.Add(thisdetaisl);
                }

            }



            return sectionList;
        }
        // this  method  is takingList  of  AllSectionPendingVm and  Loop on  this  list  until  it  see a checked  item
        //it  take the  id  and  State  change  to pending  of  this item and  then  go  to  this item  Method which  make  the  item Posted
        public static string PostAllCHeckedDocuments(List<AllSectionPendingVM> AVM)
        {
            foreach (var item in AVM)
            {
                if (item.Serial.StartsWith("PUR") && item.Checked == true)
                {
                    var inv = PurInvoiceMaster.GetInvoiceDetailsById(item.MasterID);
                    PurMasterVM invVM = new PurMasterVM();
                    invVM.Master_Id = item.MasterID;
                    invVM.Invoice_State = TransactionState.Post;

                    if (inv.Store_ID != 0)
                    {
                        PurInvoiceMaster.UpdateInvoiceMaster(invVM);


                    }
                    else
                    {
                        PurInvoiceMaster.UpdateRegularInvoiceMaster(invVM);


                    }
                }
                else if (item.Serial.StartsWith("REVP") && item.Checked == true)
                {
                    RecalPurchInvoiceMasterVM recal = new RecalPurchInvoiceMasterVM();
                    recal.Master_Id = item.MasterID;
                    recal.PI_State = TransactionState.Post;
                    string result = RecalPurchInvoiceMaster.UpdateInvoiceMaster(recal);
                }
                else if (item.Serial.StartsWith("PAY") && item.Checked == true)
                {
                    PaymentVM ccVM = new PaymentVM();
                    ccVM.Pay_ID = item.MasterID;
                    ccVM.Pay_State = TransactionState.Post;
                    var payment = Payment.UpdatePayment(ccVM);
                }
                else if (item.Serial.StartsWith("TMain") && item.Checked == true)
                {
                    TransferToMaintenanceVM MaintenanceVM = new TransferToMaintenanceVM();
                    MaintenanceVM.Master_Id = item.MasterID;
                    MaintenanceVM.Invoice_State = TransactionState.Post;
                    TransferToMaintenance.UpdateInvoiceMaster(MaintenanceVM);
                }
                else if (item.Serial.StartsWith("PINV") && item.Checked == true)
                {
                    ProjectInvoiceMasterVM invoiceMasterVM = new ProjectInvoiceMasterVM();
                    invoiceMasterVM.PInvoiceMaster_ID = item.MasterID;
                    invoiceMasterVM.Project_State = TransactionState.Post;
                    Project.UpdateProjectInvoiceMaster(invoiceMasterVM);
                }
                else if (item.Serial.StartsWith("PROPAY") && item.Checked == true)
                {
                    ProjectPaymentVM projectPayVM = new ProjectPaymentVM();
                    projectPayVM.ProjectPaymentId = item.MasterID;
                    projectPayVM.ProjectPay_State = TransactionState.Post;
                    var payment = ProjectPayment.UpdateProjectPayment(projectPayVM);
                }
                else if (item.Serial.StartsWith("TTPRO") && item.Checked == true)
                {
                    TransferToProjectMasterVM salesinvoiceMasterVM = new TransferToProjectMasterVM();
                    salesinvoiceMasterVM.Master_Id = item.MasterID;
                    salesinvoiceMasterVM.Invoice_State = TransactionState.Post;
                    TransferToProjectMaster.UpdateInvoiceMaster(salesinvoiceMasterVM);
                }
                else if (item.Serial.StartsWith("TTSTO") && item.Checked == true)
                {
                    TransferToProjectMasterVM salesinvoiceMasterVM = new TransferToProjectMasterVM();
                    salesinvoiceMasterVM.Master_Id = item.MasterID;
                    salesinvoiceMasterVM.Invoice_State = TransactionState.Post;
                    TransferToProjectMaster.UpdateInvoiceMasterTrans(salesinvoiceMasterVM);
                }
                else if (item.Serial.StartsWith("SALRC") && item.Checked == true)
                {
                    SalesReceiptMasterVM salesinvoiceMasterVM = new SalesReceiptMasterVM();
                    salesinvoiceMasterVM.Master_Id = item.MasterID;
                    salesinvoiceMasterVM.Invoice_State = TransactionState.Post;
                    SalesReceiptMaster.UpdateInvoiceMaster(salesinvoiceMasterVM);

                }
                else if (item.Serial.StartsWith("SAL") && item.Checked == true)
                {
                    MasterVM invoiceMasterVM = new MasterVM();
                    invoiceMasterVM.Master_Id = item.MasterID;
                    invoiceMasterVM.Invoice_State = TransactionState.Post;
                    InvoiceMaster.UpdateInvoiceMaster(invoiceMasterVM);
                }

                else if (item.Serial.StartsWith("TRS") && item.Checked == true)
                {
                    InventoryTransfer_HdVM TransVM = new InventoryTransfer_HdVM();
                    TransVM.TransferHd_Id = item.MasterID;
                    TransVM.Tranfer_State = TransactionState.Post;
                    InventoryTransfer_Hd.UpdateInvoiceTransfer(TransVM);
                }
                else if (item.Serial.StartsWith("REVSa") && item.Checked == true)
                {
                    ReverseSalesReceiptMasterVM salesinvoiceMasterVM = new ReverseSalesReceiptMasterVM();
                    salesinvoiceMasterVM.Master_Id = item.MasterID;
                    salesinvoiceMasterVM.Invoice_State = TransactionState.Post;
                    ReverseSalesReceiptMaster.UpdateInvoiceMaster(salesinvoiceMasterVM);

                }
                else if (item.Serial.StartsWith("REVS") && item.Checked == true)
                {
                    ReverseSalesInvoiceVM invoiceMasterVM = new ReverseSalesInvoiceVM();
                    invoiceMasterVM.Master_Id = item.MasterID;
                    invoiceMasterVM.Invoice_State = TransactionState.Post;
                    ReverseSalesInvoiceMaster.UpdateInvoiceMaster(invoiceMasterVM);

                }
                else if (item.Serial.StartsWith("SPRE") && item.Checked == true)
                {
                    SpentReciptVM invoiceMasterVM = new SpentReciptVM();
                    invoiceMasterVM.SpentReceipt_Id = item.MasterID;
                    invoiceMasterVM.SpentReceipt_State = TransactionState.Post;
                    SpentRecipt.UpdatespentRecipt(invoiceMasterVM);

                }
                
                else if (item.Serial.StartsWith("EXP") && item.Checked == true)
                {
                    var exp = Expense.getDetail(item.MasterID);
                    if (exp.Expense_Value > 0)
                    {
                        ExpenseVM expenseVM = new ExpenseVM();
                        expenseVM.Expense_Id = item.MasterID;
                        expenseVM.Expense_State = TransactionState.Post;
                        Expense.UpdatePaymentExpences(expenseVM);
                    }
                    else
                    {
                  
                      
                        Expense.PostNewExpense(item.MasterID);
                    }
                }
              
                else if (item.Serial.StartsWith("REC") && item.Checked == true)
                {
                    ReceiptVM reciptNM = new ReceiptVM();
                    reciptNM.Rec_ID = item.MasterID;
                    reciptNM.Rec_state = TransactionState.Post;
                    Receipt.UpdateRecipt(reciptNM);

                }
                else if (item.Serial.StartsWith("RPS") && item.Checked == true)
                {
                    POSReceiptVM reciptoNM = new POSReceiptVM();
                    reciptoNM.Rec_ID = item.MasterID;
                    reciptoNM.Rec_state = TransactionState.Post;
                    POSReceipt.UpdateRecipt(reciptoNM);

                }
                else if (item.Serial.StartsWith("CARECI") && item.Checked == true)
                {
                    CatchReceiptVM catchVM = new CatchReceiptVM();
                    catchVM.CatchReceipt_Id = item.MasterID;
                    catchVM.CatchReceipt_State = TransactionState.Post;
                    CatchReceipt.UpdateInvoiceMaster(catchVM);

                }
                else if (item.Serial.StartsWith("SETSTO") && item.Checked == true)
                {
                    SettlementOfStoresVM settlementOfStoresVM = new SettlementOfStoresVM();
                    settlementOfStoresVM.SettlementId = item.MasterID;
                    settlementOfStoresVM.Settlement_State = TransactionState.Post;
                    SettlementOfStores.UpdateSettlementOfStores(settlementOfStoresVM);

                }
                else if (item.Serial.StartsWith("SUBS-") && item.Checked == true)
                {
                    SubscriptionVM sbuvm = new SubscriptionVM();
                    sbuvm.Subscrip_Id = item.MasterID;

                    Subscription.PostSubscription(sbuvm.Subscrip_Id);

                }

            }


            return "تم اعتماد المستندات المختاره";
        }
        /// <summary>
        /// that  make All Document that posted in Off Sitiuation
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>

        public static void TransferAllOperationToOff(DateTime starta, DateTime enda)
        {
            var db = new AccountingContainer();
            var start = Convert.ToDateTime(Convert.ToDateTime(starta).ToShortDateString());
            var end = Convert.ToDateTime(Convert.ToDateTime(enda).ToString("dd/MM/yyyy")).AddHours(23).AddMinutes(59).AddSeconds(59);
            var InvoiceMaster = db.InvoiceMasters.Where(e => e.Invoice_Date >= start && e.Invoice_Date <= end).ToList();
            var SalesRecipt = db.SalesReceiptMasters.Where(e => e.Invoice_Date >= start && e.Invoice_Date <= end).ToList();
            var projectsales = db.ProjectRecipts.Where(e => e.Recipt_Date >= start && e.Recipt_Date <= end).ToList();
            var ReversInvoiceMaster = db.ReverseSalesInvoiceMasters.Where(e => e.Invoice_Date >= start && e.Invoice_Date <= end).ToList();
            var ReversSalesRecipt = db.ReverseSalesReceiptMasters.Where(e => e.Invoice_Date >= start && e.Invoice_Date <= end).ToList();
            var ExpenList = db.Expenses.Where(e => e.Expense_date >= start && e.Expense_date <= end).ToList();
            var payList = db.Payments.Where(e => e.Pay_Date >= start && e.Pay_Date <= end).ToList();
            var RecList = db.Receipts.Where(e => e.Rec_Date >= start && e.Rec_Date <= end).ToList();
            var RecPOSList = db.POSReceipts.Where(e => e.Rec_Date >= start && e.Rec_Date <= end).ToList();
            var spentRecipt = db.SpentRecipts.Where(e => e.CatchReceipt_date >= start && e.CatchReceipt_date <= end).ToList();
            var catchRe = db.CatchReceipts.Where(e => e.CatchReceipt_date >= start && e.CatchReceipt_date <= end).ToList();
            var paymentProject = db.ProjectPayments.Where(e => e.Date >= start && e.Date <= end).ToList();
            var projectInvoice = db.ProjectInvoiceMasters.Where(e => e.ProjectInv_Date >= start && e.ProjectInv_Date <= end).ToList();
            var Treansfer = db.TransferToMaintenances.Where(e => e.Trans_Date >= start && e.Trans_Date <= end).ToList();
            var TreansferToStore = db.InventoryTransfer_Hd.Where(e => e.Date >= start && e.Date <= end).ToList();
            var TreansferToProject = db.TransferToProjectMasters.Where(e => e.Trans_Date >= start && e.Trans_Date <= end).ToList();
            var Purchase = db.PurInvoiceMasters.Where(e => e.Invoice_Date >= start && e.Invoice_Date <= end).ToList();
            var pOSInvoices = db.MasterPOSInvoices.Where(e => e.Invoice_Date >= start && e.Invoice_Date <= end).ToList();
            var recalPurch = db.RecalPurchInvoiceMasters.Where(e => e.Register_Date >= start && e.Register_Date <= end).ToList();
            var pOSInvoicesRevers = db.ReversePOSInvoices.Where(e => e.Invoice_Date >= start && e.Invoice_Date <= end).ToList();
            var settlememnt = db.SettlementOfStores.Where(e => e.Date >= start && e.Date <= end).ToList();
            var subscription = db.Subscriptions.Where(e => e.StartTime >= start && e.StartTime <= end).ToList();
            InvoiceMaster.ForEach(a => a.HasOff = true);
            SalesRecipt.ForEach(a => a.HasOff = true);
            projectsales.ForEach(a => a.HasOff = true);
            ReversInvoiceMaster.ForEach(a => a.HasOff = true);
            ReversSalesRecipt.ForEach(a => a.HasOff = true);
            ExpenList.ForEach(a => a.HasOff = true);
            payList.ForEach(a => a.HasOff = true);
            RecList.ForEach(a => a.HasOff = true);
            spentRecipt.ForEach(a => a.HasOff = true);
            catchRe.ForEach(a => a.HasOff = true);
            paymentProject.ForEach(a => a.HasOff = true);
            RecPOSList.ForEach(a => a.HasOff = true);
            projectInvoice.ForEach(a => a.HasOff = true);
            Treansfer.ForEach(a => a.HasOff = true);
            TreansferToStore.ForEach(a => a.HasOff = true);
            TreansferToProject.ForEach(a => a.HasOff = true);
            Purchase.ForEach(a => a.HasOff = true);
            pOSInvoices.ForEach(a => a.HasOff = true);
            recalPurch.ForEach(a => a.HasOff = true);
            pOSInvoicesRevers.ForEach(a => a.HasOff = true);
            settlememnt.ForEach(a => a.HasOff = true);
            subscription.ForEach(a => a.HasOff = true);
            db.SaveChanges();


            //foreach (var item in InvoiceMaster)
            //{
            //    item.HasOff = true;
            //    db.Entry(item).State = EntityState.Modified;
            //    db.SaveChanges();
            //}

            //foreach (var item in SalesRecipt)
            //{
            //    item.HasOff = true;
            //    db.Entry(item).State = EntityState.Modified;
            //    db.SaveChanges();
            //}

            //foreach (var item in projectsales)
            //{
            //    item.HasOff = true;
            //    db.Entry(item).State = EntityState.Modified;
            //    db.SaveChanges();
            //}

            //foreach (var item in ReversInvoiceMaster)
            //{
            //    item.HasOff = true;
            //    db.Entry(item).State = EntityState.Modified;
            //    db.SaveChanges();
            //}

            //foreach (var item in ReversSalesRecipt)
            //{
            //    item.HasOff = true;
            //    db.Entry(item).State = EntityState.Modified;
            //    db.SaveChanges();
            //}

            //foreach (var item in ExpenList)
            //{
            //    item.HasOff = true;
            //    db.Entry(item).State = EntityState.Modified;
            //    db.SaveChanges();
            //}

            //foreach (var item in payList)
            //{
            //    item.HasOff = true;
            //    db.Entry(item).State = EntityState.Modified;
            //    db.SaveChanges();
            //}

            //foreach (var item in RecList)
            //{
            //    item.HasOff = true;
            //    db.Entry(item).State = EntityState.Modified;
            //    db.SaveChanges();
            //}

            //foreach (var item in spentRecipt)
            //{
            //    item.HasOff = true;
            //    db.Entry(item).State = EntityState.Modified;
            //    db.SaveChanges();
            //}

            //foreach (var item in catchRe)
            //{
            //    item.HasOff = true;
            //    db.Entry(item).State = EntityState.Modified;
            //    db.SaveChanges();
            //}

            //foreach (var item in paymentProject)
            //{
            //    item.HasOff = true;
            //    db.Entry(item).State = EntityState.Modified;
            //    db.SaveChanges();
            //}

            //foreach (var item in projectInvoice)
            //{
            //    item.HasOff = true;
            //    db.Entry(item).State = EntityState.Modified;
            //    db.SaveChanges();
            //}

            //foreach (var item in Treansfer)
            //{
            //    item.HasOff = true;
            //    db.Entry(item).State = EntityState.Modified;
            //    db.SaveChanges();
            //}

            //foreach (var item in TreansferToStore)
            //{
            //    item.HasOff = true;
            //    db.Entry(item).State = EntityState.Modified;
            //    db.SaveChanges();
            //}

            //foreach (var item in TreansferToProject)
            //{
            //    item.HasOff = true;
            //    db.Entry(item).State = EntityState.Modified;
            //    db.SaveChanges();
            //}

            //foreach (var item in Purchase)
            //{
            //    item.HasOff = true;
            //    db.Entry(item).State = EntityState.Modified;
            //    db.SaveChanges();
            //}

            //foreach (var item in recalPurch)
            //{
            //    item.HasOff = true;
            //    db.Entry(item).State = EntityState.Modified;
            //    db.SaveChanges();
            //}

            //foreach (var item in pOSInvoices)
            //{
            //    item.HasOff = true;
            //    db.Entry(item).State = EntityState.Modified;
            //    db.SaveChanges();
            //}

            //foreach (var item in pOSInvoicesRevers)
            //{
            //    item.HasOff = true;
            //    db.Entry(item).State = EntityState.Modified;
            //    db.SaveChanges();
            //}

            //foreach (var item in settlememnt)
            //{
            //    item.HasOff = true;
            //    db.Entry(item).State = EntityState.Modified;
            //    db.SaveChanges();
            //}

            //foreach (var item in subscription)
            //{
            //    item.HasOff = true;
            //    db.Entry(item).State = EntityState.Modified;
            //    db.SaveChanges();
            //}
        }
        public static List<AllSectionPendingVM> GetAllPendingsForReport()
        {
            var db = new AccountingContainer();
            List<AllSectionPendingVM> sectionList = new List<AllSectionPendingVM>();

            var purchoinv = db.PurInvoiceMasters.Where(e => e.Invoice_State == TransactionState.Pending).ToList();
            if (purchoinv != null)
            {
                foreach (var purchaseinv in purchoinv)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = purchaseinv.Master_Id;
                    thisdetaisl.Serial = purchaseinv.Serial_Number;
                    thisdetaisl.Description = purchaseinv.Description;
                    thisdetaisl.TotalAmount = purchaseinv.Total_Cost;
                    thisdetaisl.date = purchaseinv.Invoice_Date;
                    if (purchaseinv.PaymentMethod_Id > 0)
                    {
                        thisdetaisl.TresuryOrBankId = purchaseinv.PaymentMethod_Id.Value;
                    }
                    else
                    {
                        thisdetaisl.SupplierId = purchaseinv.Supplier_Id;
                    }
                    thisdetaisl.CreditAccountId = 45;
                    thisdetaisl.TotalAmountForChild = -thisdetaisl.TotalAmount;
                    thisdetaisl.treasuryCredit = true;
                    sectionList.Add(thisdetaisl);
                }

            }
            var purchaseRecall = db.RecalPurchInvoiceMasters.Where(e => e.PI_State == TransactionState.Pending).ToList();
            if (purchaseRecall != null)
            {
                foreach (var recal in purchaseRecall)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = recal.Master_Id;
                    thisdetaisl.Serial = recal.Recall_MPurch_Serial;
                    thisdetaisl.Description = recal.Doc_Type;
                    thisdetaisl.TotalAmount = recal.Inv_TotalCost;
                    thisdetaisl.date = recal.Register_Date;
                    thisdetaisl.TotalAmountForChild = thisdetaisl.TotalAmount;
                    if (recal.PaymentMethod_Id > 0)
                    {
                        thisdetaisl.TresuryOrBankId = recal.PaymentMethod_Id.Value;
                    }
                    else
                    {
                        thisdetaisl.SupplierId = recal.SupplierSuppId;
                    }
                    thisdetaisl.CreditAccountId = 45;
                  
                    thisdetaisl.treasuryCredit = false;
                    sectionList.Add(thisdetaisl);
                }
            }
            var Payments = db.Payments.Where(e => e.Pay_State == TransactionState.Pending).ToList();
            if (Payments != null)
            {
                foreach (var pay in Payments)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = pay.Pay_ID;
                    thisdetaisl.Serial = pay.Pay_Number;
                    thisdetaisl.Description = pay.Rec_Description;
                    thisdetaisl.TotalAmount = pay.Pay_Amount;
                    thisdetaisl.date = pay.Pay_Date;
                    thisdetaisl.TresuryOrBankId = pay.PayBank_Account;
                    thisdetaisl.DebitAccountId = 45;
                    thisdetaisl.SupplierId = pay.Supplier_Id;
                    thisdetaisl.TotalAmountForChild = thisdetaisl.TotalAmount;
                    thisdetaisl.treasuryCredit = true;
                    sectionList.Add(thisdetaisl);
                }
            }
       
            var ProjectInv = db.ProjectInvoiceMasters.Where(e => e.Project_State == TransactionState.Pending).ToList();
            if (ProjectInv != null)
            {
                foreach (var proj in ProjectInv)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = proj.PInvoiceMaster_ID;
                    thisdetaisl.Serial = proj.Serial_Number;
                    thisdetaisl.Number = proj.Number;
                    thisdetaisl.Description = proj.Project_Description;
                    thisdetaisl.TotalAmount = proj.ProjectInvoiceDetails.Select(a => a.Total_Amount).Sum();
                    thisdetaisl.date = proj.StartTime;
                    thisdetaisl.CustomerId = proj.CustomerCustomer_Id;
                    //thisdetaisl.TresuryOrBankId = db.PaymentMethods.Find(proj.Payment_Method).Bank_Account_Id.Value;
                    thisdetaisl.treasuryCredit = false;
                    thisdetaisl.TotalAmountForChild = thisdetaisl.TotalAmount;
                    sectionList.Add(thisdetaisl);
                }
            }
            var projectPay = db.ProjectPayments.Where(e => e.ProjectPay_State == TransactionState.Pending).ToList();
            if (projectPay != null)
            {
                foreach (var projP in projectPay)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = projP.ProjectPaymentId;
                    thisdetaisl.Serial = projP.ProjectPay_Code;
                    thisdetaisl.Description = projP.Description;
                    thisdetaisl.TotalAmount = projP.Pay_Amount;
                    thisdetaisl.date = Convert.ToDateTime(projP.Date);
                    if (projP.Supplier_ID > 0)
                    {
                        thisdetaisl.SupplierId = projP.Supplier_ID.Value;
                    }
                    if (projP.Revenue_Id > 0)
                    {
                        thisdetaisl.TresuryOrBankId = projP.Revenue_Id;
                    }
                    thisdetaisl.TotalAmountForChild = -thisdetaisl.TotalAmount;
                    thisdetaisl.treasuryCredit = true;
                    sectionList.Add(thisdetaisl);
                }
            }
          
            var invMast = db.InvoiceMasters.Where(e => e.Invoice_State == TransactionState.Pending).ToList();
            if (invMast != null)
            {
                foreach (var sinv in invMast)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = sinv.Master_Id;
                    thisdetaisl.Serial = sinv.Serial_Number;
                    thisdetaisl.Number = sinv.Number;
                    thisdetaisl.Description = sinv.Description;
                    thisdetaisl.TotalAmount = sinv.Total_Amount;
                    thisdetaisl.date = sinv.Invoice_Date;
                    thisdetaisl.CustomerId = sinv.Customer_Id;
                    thisdetaisl.treasuryCredit = true;
                    thisdetaisl.TotalAmountForChild = thisdetaisl.TotalAmount;
                    sectionList.Add(thisdetaisl);
                }
            }
            var SalesInvm = db.SalesReceiptMasters.Where(e => e.Invoice_State == TransactionState.Pending).ToList();
            if (SalesInvm != null)
            {
                foreach (var Salesinv in SalesInvm)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = Salesinv.Master_Id;
                    thisdetaisl.Number = Salesinv.Number;
                    thisdetaisl.Serial = Salesinv.Serial_Number;
                    thisdetaisl.Description = Salesinv.Description;
                    thisdetaisl.TotalAmount = Salesinv.Total_Amount;
                    thisdetaisl.date = Salesinv.Invoice_Date;
                    thisdetaisl.treasuryCredit = false;
                    thisdetaisl.TresuryOrBankId = db.PaymentMethods.Find(Salesinv.PaymentMethod_Id).Bank_Account_Id.Value;
                    sectionList.Add(thisdetaisl);
                }
            }
         
            var RevSalesInv = db.ReverseSalesInvoiceMasters.Where(e => e.Invoice_State == TransactionState.Pending).ToList();
            if (RevSalesInv != null)
            {
                foreach (var revSal in RevSalesInv)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = revSal.Master_Id;
                    thisdetaisl.Number = revSal.Number;
                    thisdetaisl.Serial = revSal.Serial_Number;
                    thisdetaisl.Description = revSal.Description;
                    thisdetaisl.TotalAmount = (decimal)revSal.Total_Amount;
                    thisdetaisl.date = revSal.Invoice_Date;
                    thisdetaisl.CustomerId = revSal.Customer_Id;
                    thisdetaisl.treasuryCredit = true;
                    thisdetaisl.TotalAmountForChild = -thisdetaisl.TotalAmount;
                    sectionList.Add(thisdetaisl);
                }
            }
            var RevSalesRec = db.ReverseSalesReceiptMasters.Where(e => e.Invoice_State == TransactionState.Pending).ToList();
            if (RevSalesRec != null)
            {
                foreach (var revSalR in RevSalesRec)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = revSalR.Master_Id;
                    thisdetaisl.Number = revSalR.Number;
                    thisdetaisl.Serial = revSalR.Serial_Number;
                    thisdetaisl.Description = revSalR.Description;
                    thisdetaisl.TotalAmount = (decimal)revSalR.Total_Amount;
                    thisdetaisl.date = revSalR.Invoice_Date;
                    thisdetaisl.treasuryCredit = true;
                    thisdetaisl.TresuryOrBankId = db.PaymentMethods.Find(revSalR.PaymentMethod_Id).Bank_Account_Id.Value;
                    sectionList.Add(thisdetaisl);
                }
            }

            var Expense = db.Expenses.Where(e => e.Expense_State == TransactionState.Pending).ToList();
            if (Expense != null)
            {
                foreach (var exp in Expense)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = exp.Expense_Id;
                    thisdetaisl.Serial = exp.Expense_Code;
                    thisdetaisl.Description = exp.Expense_Description;
                    if (exp.Asset_Id > 0)
                    {
                        thisdetaisl.ChildAccountId = exp.Asset_Id.Value;
                    }
                    if (exp.AccountTreasure_Id > 0)
                    {
                        thisdetaisl.TresuryOrBankId = db.PaymentMethods.Find(exp.AccountTreasure_Id).Bank_Account_Id.Value;
                    }
                    thisdetaisl.TotalAmountForChild = -thisdetaisl.TotalAmount;
                    thisdetaisl.TotalAmount = exp.Expense_Value;
                    thisdetaisl.treasuryCredit = true;
                    thisdetaisl.date = exp.Expense_date;
                    sectionList.Add(thisdetaisl);
                }



            }
            var catchRecipt = db.CatchReceipts.Where(e => e.CatchReceipt_State == TransactionState.Pending).ToList();

            if (catchRecipt != null)
            {
                foreach (var catchre in catchRecipt)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = catchre.CatchReceipt_Id;
                    thisdetaisl.Serial = catchre.CatchReceipt_Code;
                    thisdetaisl.Description = catchre.CatchReceipt_Description;
                    thisdetaisl.TotalAmount = (decimal)catchre.CatchReceipt_Value;
                    thisdetaisl.date = catchre.CatchReceipt_date;
                    if (catchre.Child_AccountCredit_Id > 0)
                    {
                        thisdetaisl.ChildAccountId = catchre.Child_AccountCredit_Id.Value;
                    }
                    thisdetaisl.TotalAmountForChild = -thisdetaisl.TotalAmount;
                    thisdetaisl.TresuryOrBankId = catchre.AccountDebit_Id;
                    thisdetaisl.treasuryCredit = false;
                    sectionList.Add(thisdetaisl);
                }



            }
            var spentRecipt = db.SpentRecipts.Where(e => e.SpentReceipt_State == TransactionState.Pending).ToList();

            if (spentRecipt != null)
            {
                foreach (var spent in spentRecipt)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = spent.SpentReceipt_Id;
                    thisdetaisl.Serial = spent.SpentReceipt_Code;
                    thisdetaisl.Description = spent.SpentReceipt_Description;
                    thisdetaisl.TotalAmount = (decimal)spent.SpentReceipt_Value;
                    thisdetaisl.date = spent.CatchReceipt_date;
                    if (spent.Child_AccountDebit_Id > 0)
                    {
                        thisdetaisl.ChildAccountId = spent.Child_AccountDebit_Id.Value;
                    }
                    thisdetaisl.TotalAmountForChild = thisdetaisl.TotalAmount;
                    thisdetaisl.treasuryCredit = true;
                    thisdetaisl.TresuryOrBankId = spent.AccountCredit_Id;
                    sectionList.Add(thisdetaisl);
                }



            }
            var Subscrip = db.Subscriptions.Where(e => e.Subscrip_State == TransactionState.Pending).ToList();

            if (Subscrip != null)
            {
                foreach (var sub in Subscrip)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = sub.Subscrip_Id;
                    thisdetaisl.Number = sub.Number;
                    thisdetaisl.Serial = sub.Subscrip_Code;
                    thisdetaisl.Description = sub.Subscrip_Description;
                    thisdetaisl.TotalAmount = (decimal)sub.AmountAfterTax;
                    thisdetaisl.date = sub.StartTime;
                    thisdetaisl.CustomerId = sub.Customer_Id.Value;
                    thisdetaisl.treasuryCredit = true;
                    thisdetaisl.TotalAmountForChild = thisdetaisl.TotalAmount;
                    sectionList.Add(thisdetaisl);
                }



            }
            var recipt = db.Receipts.Where(e => e.Rec_state == TransactionState.Pending).ToList();

            if (recipt != null)
            {
                foreach (var rec in recipt)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = rec.Rec_ID;
                    thisdetaisl.Serial = rec.Rec_Number;
                    thisdetaisl.Description = rec.Rec_Description;
                    thisdetaisl.TotalAmount = (decimal)rec.Rec_Amount;
                    thisdetaisl.date = rec.Rec_Date;
                    thisdetaisl.CustomerId = rec.Customer_Id;
                    thisdetaisl.treasuryCredit = false;
                    thisdetaisl.TresuryOrBankId = db.PaymentMethods.Find(rec.RecBank_Account).Bank_Account_Id.Value;
                    thisdetaisl.TotalAmountForChild = -thisdetaisl.TotalAmount;
                    sectionList.Add(thisdetaisl);
                }

            }
            var reciptPOS = db.POSReceipts.Where(e => e.Rec_state == TransactionState.Pending).ToList();

            if (reciptPOS != null)
            {
                foreach (var rec in reciptPOS)
                {
                    AllSectionPendingVM thisdetaisl = new AllSectionPendingVM();
                    thisdetaisl.MasterID = rec.Rec_ID;
                    thisdetaisl.Serial = rec.Rec_Number;
                    thisdetaisl.Description = rec.Rec_Description;
                    thisdetaisl.TotalAmount = (decimal)rec.Rec_Amount;
                    thisdetaisl.date = rec.Rec_Date;
                    thisdetaisl.CustomerId = rec.Customer_Id;
                    thisdetaisl.treasuryCredit = false;
                    thisdetaisl.TotalAmountForChild = thisdetaisl.TotalAmount;
                    thisdetaisl.TresuryOrBankId = db.PaymentMethods.Find(rec.RecBank_Account).Bank_Account_Id.Value;
                    sectionList.Add(thisdetaisl);
                }

            }

       



            return sectionList;
        }
    }

}
