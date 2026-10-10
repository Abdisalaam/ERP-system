using Accountingsystem.ViewModel;
using Accountingsystem.ViewModel.PrintVM;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class CostDistribution
    { 
        /// <summary>
        /// that to make Code CostDistribution Unique
        /// </summary>
        /// <param name="Serial"></param>
        /// <returns></returns>
        public static bool containCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.CostDistributions.ToList();
            if (!Serial.StartsWith("COSDS-"))
            {
                return true;
            }
            else
            {
                foreach (var item in allinData)
                {
                    if (item.Cost_Distr_Code == Serial || !Serial.StartsWith("COSDS-"))
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
        /// <summary>
        /// getallCostDistribution in data Base
        /// </summary>
        /// <returns></returns>
        public static List<CostDistribution> GetAllCostDistr()
        {
            var db = new AccountingContainer();
            var costs = db.CostDistributions.ToList();
            return costs;
        }
        /// <summary>
        /// get that cost distributed by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static CostDistributionPrintVM GetCostDistrById(int id)
        {
            var db = new AccountingContainer();
            var cost = Reports.PrintCostDestribution(id);
            return cost;
        }

        /// <summary>
        /// fill For Create CostDistributed
        /// </summary>
        /// <returns></returns>
        
        public static CostDistributionVM FillCostDistributionVM()
        {
            var db = new AccountingContainer();
            List<PurInvoiceMaster> masterList = new List<PurInvoiceMaster> { };
            var list = db.PurInvoiceMasters.Where(f =>f.TotalAmountAfMat > 0 && f.Cost_distribute == false && f.Invoice_State == TransactionState.Post && f.Store_ID > 0).ToList();
            foreach (var invoice in list)
            {
                var reverse = db.RecalPurchInvoiceMasters.Where(a => a.PurInvoiceMasterMaster_Id == invoice.Master_Id && a.PI_State == TransactionState.Post).ToList();
                var TotalReferse = reverse.Select(a => a.Inv_TotalCost).Sum();
                
            
                if (TotalReferse < invoice.Total_Cost)
                {
                    masterList.Add(invoice);

                }
            }

            CostDistributionVM costDistributionVM = new CostDistributionVM();
            
            var newList = from invoice in masterList
                          select new
                          {
                              Id = invoice.Master_Id,
                              Name = invoice.Serial_Number

                          };
           
            costDistributionVM.MyInvoicesList = new SelectList(newList, "Id", "Name");
         var datebefore= new DateTime(2150, 12, 31);
            var subList = db.SubLedgers.Where(s => s.SubLed_Type == "مركز تكلفة يدوى" && s.isActive != false).ToList();
            var DrobList = new List<SubLedger>();
            foreach(var sub in subList)
            {
              if (SubLedger.getTheBalanceOfDateBefore(sub.SubLed_Id, datebefore) > 0)
                {
                    DrobList.Add(sub);
                }
            }


            var newList1 = from sub in DrobList
                           select new {

                Id = sub.SubLed_Id,
                Name = sub.SubLed_Name + "   " + sub.SubLed_Code

            };
            
            costDistributionVM.MySubledgersList = new SelectList(newList1, "Id", "Name");
       
            var lastSerial = db.CostDistributions.OrderByDescending(c => c.Id).FirstOrDefault();
            if (lastSerial == null)
            {
                costDistributionVM.Cost_Distr_Code = "COSDS-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                costDistributionVM.Cost_Distr_Code = "COSDS-" + (Convert.ToInt32(lastSerial.Cost_Distr_Code.Substring(6, lastSerial.Cost_Distr_Code.Length - 6)) + 1).ToString("D3");
            }

            return costDistributionVM;
        }
        /// <summary>
        /// make Create Cost Distributed And Save it in DataBase
        /// </summary>
        /// <param name="costDistVM"></param>
        /// <param name="UserId"></param>
        public static int CreateCostDistribution(CostDistributionVM costDistVM , String UserId)
        {
            var db = new AccountingContainer();

            CostDistribution costDist = new CostDistribution();
            var thisinv = db.PurInvoiceMasters.Find(Convert.ToInt32(costDistVM.Invoice_Serial));
            costDist.Invoice_Serial = thisinv.Serial_Number;
            costDist.Subledger_ID = costDistVM.Subledger_ID;

            var lastSerial = db.CostDistributions.OrderByDescending(c => c.Id).FirstOrDefault();
            if (lastSerial == null)
            {
                costDist.Cost_Distr_Code = "COSDS-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                costDist.Cost_Distr_Code = "COSDS-" + (Convert.ToInt32(lastSerial.Cost_Distr_Code.Substring(6, lastSerial.Cost_Distr_Code.Length - 6)) + 1).ToString("D3");
            }
            //List<GLDetail> glDetails = db.GLDetails.Where(gl => gl.SubLedger_Id == costDist.Subledger_ID&&gl.CostDistrbutionSubldger!=true).ToList();
           
            // Loop over all GLs and calculate the total additional cost
            //decimal totalAdditionalCost = glDetails.Sum(glDetail => glDetail.GLDetail_Debit);
            var date = new DateTime(2150, 12, 31);
            decimal totalAdditionalCost = SubLedger.getTheBalanceOfDateBefore(costDistVM.Subledger_ID,date);
            //decimal totalAdditionalCost = db.SubLedgers.Find(costDist.Subledger_ID).SubLedger_Balance.Value;
            if (totalAdditionalCost <= 0)
            {
                return 0;
            }
            List<GLDetail> glDetails = getAllGldetailsThatNotClosed(costDistVM.Subledger_ID, totalAdditionalCost);
            PurInvoiceMaster purchaseInvoice = db.PurInvoiceMasters.Where(invoice => invoice.Serial_Number == costDist.Invoice_Serial).First();
            List<ProductBatches> batches = db.ProductBatches.Where(storebatch =>storebatch.PurInvoice_ID == purchaseInvoice.Master_Id).ToList();
            var realDetail = PurInvoiceMaster.getMaterialOCompoundProduct(purchaseInvoice.InvoiceDetails.ToList(), purchaseInvoice.Store_ID.Value);
            var batch = realDetail.Select(a => a.Product_ID).Distinct().ToList();
            foreach (var detail in realDetail)
            {
                CostDistributionDetails costDistDetail = new CostDistributionDetails();

                costDistDetail.Product_ID = (int)detail.Product_ID;
                // Calculate the percentage of the cost of the product in the detail to the total cost of the Invoice Master
                decimal costPercentage = detail.Total_Amount / purchaseInvoice.Total_Amount;
                
                // Calculate the product additional cost by multiplying the total cost by the percentage
                costDistDetail.Product_AdditionalCost = totalAdditionalCost * costPercentage;
              
                // Add the cost distribution detail to the master
                costDist.CostDistributionDetails.Add(costDistDetail);

                var curBatch = db.ProductBatches.Where(batc => batc.Product_ID == detail.Product_ID && batc.Store_ID == purchaseInvoice.Store_ID).ToList().FirstOrDefault();

                if (curBatch.Product_Quantity == 0)
                {
                  curBatch.Product_Cost = 0;
                  curBatch.Batch_Cost += curBatch.Batch_Cost;
                }
                curBatch.Batch_Cost += costDistDetail.Product_AdditionalCost;
                curBatch.Product_Cost = curBatch.Batch_Cost / curBatch.Product_Quantity;
                db.Entry(curBatch).State = EntityState.Modified;
                var storecard = new StoreCard();
                storecard.CostPrice =0;
                storecard.Date = DateTime.Now;
                storecard.Store_Id = purchaseInvoice.Store_ID.Value;
                storecard.Quantity = 0;
                storecard.SalePrice = 0;
                storecard.Product_Id = detail.Product_ID.Value;
                storecard.IncreaseQuantity = true;
                storecard.TypeOfDocument = " توزيع التكلفة";
                storecard.Document_Serial = costDist.Cost_Distr_Code;
                storecard.TotalCost = costDistDetail.Product_AdditionalCost;
                storecard.TotalAmount = 0;
                storecard.Account_Id =0;
                storecard.Subledger_Id = purchaseInvoice.Subledger_Id;
                db.StoreCards.Add(storecard);
                
                db.SaveChanges();

            }

            db.CostDistributions.Add(costDist);
            db.SaveChanges();

            CreateGLsForCostDistribution(purchaseInvoice, costDist, glDetails);
            purchaseInvoice.Cost_distribute = true;
            db.Entry(purchaseInvoice).State = EntityState.Modified;
            db.SaveChanges();

            var traBatch = new TransactionBatch();

            traBatch.OperationName = "توزيع تكلفة";
            traBatch.Description = "توزيع تكلفة";
            traBatch.Date = DateTime.Now;
            traBatch.UserId = UserId;
            traBatch.OperationId = costDist.Id;
            traBatch.Operation_Code = costDist.Cost_Distr_Code;
            TransactionBatch.AddNewTransaction(traBatch);
            db.Dispose();
            return costDist.Id;
        }
        /// <summary>
        /// make GL For Cost Distributed
        /// </summary>
        /// <param name="purchaseInvoice"></param>
        /// <param name="costDist"></param>
        /// <param name="glDetails"></param>

        public static void CreateGLsForCostDistribution(PurInvoiceMaster purchaseInvoice, CostDistribution costDist, List<GLDetail> glDetails)
        {
            var db = new AccountingContainer();

            GLMaster glmaster = new GLMaster();

            glmaster.GL_Date = DateTime.Now;
            glmaster.GL_Desc = "توزيع تكلفة";
            glmaster.Company_Id = purchaseInvoice.Company_Id;
            glmaster.GLType_Id = "توزيع تكلفة";
            glmaster.Currency_Id = purchaseInvoice.Invoice_Currency;
            glmaster.GLCode = costDist.Cost_Distr_Code;
            glmaster.GLState = TransactionState.Post;
            glmaster.Auto = true;
            int touseID = 0;
            //add GL Detail
           

            foreach (var glDetail in glDetails)
            {
                GLDetail glDetailClone = new GLDetail();

                glDetailClone.Account_Id = glDetail.Account_Id;
                glDetailClone.Account_Name = glDetail.Account_Name;
                glDetailClone.Asset_Id = glDetail.Asset_Id;
                glDetailClone.Bu_Id = glDetail.Bu_Id;
                glDetailClone.SubLedger_Id = glDetail.SubLedger_Id;
                glDetailClone.CostDistrbutionSubldger = true;
                touseID = (int)glDetail.SubLedger_Id;
                if (glDetail.GLDetail_Debit == 0)
                {
                    
                  
                    glDetailClone.GLDetail_Debit = glDetail.GLDetail_Credit;
                   
                }
                else
                {
                 
                    glDetailClone.GLDetail_Credit = glDetail.GLDetail_Debit;
                    
                }
                glmaster.GLDetails.Add(glDetailClone);

                var data = db.GLDetails.Find(glDetail.GLDetail_Id);
                //updateGlDetail(glDetail);
                data.CostDistrbutionSubldger = true;
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
            }
            Store store = db.Stores.Find(purchaseInvoice.Store_ID);
            var storeAccount = db.ChartOfAccountings.Where(account => account.Account_Code == store.Store_Code).First();

            db.Products.Load();
         
                GLDetail inventoryGL = new GLDetail();

                inventoryGL.Account_Id = storeAccount.Account_Id;
                inventoryGL.Account_Name = storeAccount.Account_Name;
                inventoryGL.SubLedger_Id = 0;
                inventoryGL.GLDetail_Debit = glmaster.GLDetails.Select(g=>g.GLDetail_Credit-g.GLDetail_Debit).ToList().Sum();
                inventoryGL.Bu_Id = 1;
                glmaster.GLDetails.Add(inventoryGL);
            //while(glmaster.GLDetails.Select(a=>a.GLDetail_Credit).Sum()!= glmaster.GLDetails.Select(a => a.GLDetail_Debit).Sum())
            //{
            //    var gldetail = glmaster.GLDetails.FirstOrDefault();
            //    glmaster.GLDetails.Remove(gldetail);

            //}

            db.GLMasters.Add(glmaster);
            GLMaster.GetBalanceForAccount(glmaster);
            GLMaster.GetBalanceForSubLedger(glmaster);
            db.SaveChanges();
        }
       
        public static List<GLDetail>getAllGldetailsThatNotClosed(int Subledger,decimal SubBalance)
        {

            var db = new AccountingContainer();
            var glDetails = db.GLDetails.Where(a => a.SubLedger_Id == Subledger).OrderByDescending(z => z.GLMaster.GL_Date).ToList();
            List<GLDetail> realgldetail =new List<GLDetail>();
            decimal totalsub = 0;
          
                foreach (var detail in glDetails)
                {

                    totalsub += detail.GLDetail_Debit - detail.GLDetail_Credit;
                    realgldetail.Add(detail);
                    if(totalsub == SubBalance)
                    {
                        break;
                    }

                }


            return realgldetail;




        }

    }
}