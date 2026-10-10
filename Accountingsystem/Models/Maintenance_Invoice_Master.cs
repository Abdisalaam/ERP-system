using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Accountingsystem.Models
{
    public partial class Maintenance_Invoice_Master
    {
        /// <summary>
        /// make that invoice with unique code
        /// </summary>
        /// <param name="Serial"></param>
        /// <returns>bool</returns>
        public static bool containCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.Maintenance_Invoice_Master.ToList();
            foreach (var item in allinData)
            {

                if (item.Main_Invoice_Code == Serial || !Serial.StartsWith("MIV-"))
                {
                    return true;
                }

                else
                { }

            }
            return false;
        }
        /// <summary>
        /// get All maintanice invoice master 
        /// </summary>
        /// <returns>List<Maintenance_Invoice_Master></returns>
        public static List<Maintenance_Invoice_Master> GetAllInvoices()
        {
            var db = new AccountingContainer();
            var invoices = db.Maintenance_Invoice_Master.ToList();
            return invoices;
        }
        /// <summary>
        /// get the detail of Maintance invoice by using its id 
        /// </summary>
        /// <param name="detailId"></param>
        /// <returns>Maintenance_Invoice_Master</returns>
        public static Maintenance_Invoice_Master GetInvoiceDetailsById(int detailId)
        {
            var db = new AccountingContainer();
            var detail = db.Maintenance_Invoice_Master.Find(detailId);
            return detail;

        }
        /// <summary>
        /// get all invoice detail temp by using invoice master id
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns></returns>
        public static List<Maintenance_Invoice_Detail_Temp> GetAllTempInvoiceDetails(int masterVmId)
        {
            var db = new AccountingContainer();

            var InvoiceDetailsTemp = db.Maintenance_Invoice_Detail_Temp.Where(invoice => invoice.Maintenance_Invoice_Master_TempMain_Invoice_Id == masterVmId).ToList();
            return InvoiceDetailsTemp;
        }
        /// <summary>
        /// make fourm of in maintanice for crating buy using Subscription id
        /// </summary>
        /// <param name="Subscription_Id"></param>
        /// <returns>Maintenance_Invoice_MasterVM</returns>
        public static Maintenance_Invoice_MasterVM fillVMForCreation(int Subscription_Id)
        {
            var db = new AccountingContainer();
            var subscribtion = db.Subscriptions.Find(Subscription_Id);
            var invoice = new Maintenance_Invoice_MasterVM();
            invoice.Customer_Name = subscribtion.Customer_Name;
            invoice.Starting_Date =Convert.ToString( subscribtion.StartTime);
            invoice.Ending_Date = Convert.ToString(subscribtion.DeadLine);
            invoice.Subscription_Code = subscribtion.Subscrip_Code;
            invoice.Subscription_Id = subscribtion.Subscrip_Id;
            var lastSerial = db.Maintenance_Invoice_Master.OrderByDescending(c => c.Main_Invoice_Id).FirstOrDefault();
            if (lastSerial == null)
            {
                invoice.Main_Invoice_Code = "MIV-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                invoice.Main_Invoice_Code = "MIV-" + (Convert.ToInt32(lastSerial.Main_Invoice_Code.Substring(4, lastSerial.Main_Invoice_Code.Length - 4)) + 1).ToString("D3");
            }
            var MasterTemp = new Maintenance_Invoice_Master_Temp();
            var id = (from s in db.Maintenance_Invoice_Master_Temp
                      select s).ToList().LastOrDefault();
            if (id == null)
            { MasterTemp.Main_Invoice_Id = 1; }
            else
            {
                MasterTemp.Main_Invoice_Id = id.Main_Invoice_Id + 1;
            }
            db.Maintenance_Invoice_Master_Temp.Add(MasterTemp);
            db.SaveChanges();
            invoice.Main_Invoice_Id = MasterTemp.Main_Invoice_Id;
            return invoice;

        }

        //get all detail of sub scription to show it
        public static List<Subscription> getAllSubscription(int Subscription_Id) {
            var db = new AccountingContainer();
            var subscribtion = db.Subscriptions.Where(a=>a.Subscrip_Id== Subscription_Id).ToList();
            return subscribtion;

        }
        /// <summary>
        /// make form of detail of that invoice
        /// </summary>
        /// <returns></returns>
        public static Maintenance_Invoice_DetailVM prepareInvoiceDetail()
        {
            var MaintenanceDetailsVm = new Maintenance_Invoice_DetailVM();
            return MaintenanceDetailsVm;
        }
        /// <summary>
        /// add invoice detail to List
        /// </summary>
        /// <param name="detailvm"></param>
        /// <returns>string</returns>
        public static String addDetailToMaster(Maintenance_Invoice_DetailVM detailvm)
        {
            var db = new AccountingContainer();
            var InvoiceMasterTemp = db.Maintenance_Invoice_Master_Temp.Find(detailvm.Maintenance_Invoice_MasterMain_Invoice_Id);
            var InvoiceMasterDetailsTemp = new Maintenance_Invoice_Detail_Temp();
            InvoiceMasterDetailsTemp.Subscription_Cost = detailvm.Subscription_Cost;
            InvoiceMasterDetailsTemp.Subscription_Name = detailvm.Subscription_Name;
            InvoiceMasterDetailsTemp.Maintenance_Invoice_Master_TempMain_Invoice_Id = detailvm.Maintenance_Invoice_MasterMain_Invoice_Id;
            InvoiceMasterTemp.Maintenance_Invoice_Detail_Temp.Add(InvoiceMasterDetailsTemp);

            db.Entry(InvoiceMasterTemp).State = EntityState.Modified;
            db.SaveChanges();
            return "Details Has been added";
        }
        /// <summary>
        /// make new maintance invoice and save it in data base
        /// </summary>
        /// <param name="masterVM"></param>
        /// <returns></returns>
        public static string addNewInvoice(Maintenance_Invoice_MasterVM masterVM)
        {
            var db = new AccountingContainer();
            var master = new Maintenance_Invoice_Master();
            var invoiceTemp = db.Maintenance_Invoice_Master_Temp.Find(masterVM.Main_Invoice_Id);
            master.Customer_Name = masterVM.Customer_Name;
            master.Starting_Date = Convert.ToDateTime(masterVM.Starting_Date);
            master.Ending_Date =Convert.ToDateTime( masterVM.Ending_Date);
            master.Subscription_Code = masterVM.Subscription_Code;
            master.Describtion = masterVM.Describtion;
            master.Main_Invoice_Code = masterVM.Main_Invoice_Code;
            foreach (var item in invoiceTemp.Maintenance_Invoice_Detail_Temp.ToList())
            {
                var mainDetail = new Maintenance_Invoice_Detail();
                mainDetail.Subscription_Cost = item.Subscription_Cost;
                mainDetail.Subscription_Name = item.Subscription_Name;

                master.Maintenance_Invoice_Detail.Add(mainDetail);

                db.Maintenance_Invoice_Detail_Temp.Remove(item);
            }
            var subscrubtion = db.Subscriptions.Where(a => a.Subscrip_Code == masterVM.Subscription_Code).FirstOrDefault();
            var maindetail2 = new Maintenance_Invoice_Detail();
            maindetail2.Subscription_Cost = subscrubtion.Estimate_Amount;
            maindetail2.Subscription_Name = db.Subscription_Type.Find(subscrubtion.Sub_Type_ID).Subscrip_Name;
            master.Maintenance_Invoice_Detail.Add(maindetail2);
            db.Maintenance_Invoice_Master.Add(master);
            db.Maintenance_Invoice_Master_Temp.Remove(invoiceTemp);
            db.SaveChanges();
            db.Dispose();
            return "Success! Order is Complete";
        }
    }
}