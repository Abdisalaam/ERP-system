using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Accountingsystem.ViewModel;

namespace Accountingsystem.Models
{
    public partial class FixedDepreciation
    {
        /*Calculate Intial Complex And  FinalBook Vlaue 
             Connect  With  DBContext

            get  the  Number  of  Rows  that  FixedAsset = id of  parameter
            check IF the  count2 variable  is  0  to  make my  initial  row
             get  an object  of  type  FixedAsset  that  have  ID  Equal  to  Parameter ID 
            Adding  The  Purchaseprice  value of  Fixed Asset  in  the  IntialBookValue  OF  FixedDepreciation
            Adding  the FixedAssetID   Value  to  FixedAssetID  Attribute  in FixedDepreciation
             Adding  the  Calculated  Value  OF  expense  and  Adding it  in  Variable CalculatedExpense
            Adding   The  Variable  CalculatedExpense Value  in  FixedDepreciation  Instalment  
            as  its  the  first  row  to be  dont  to  the  FixedAsset
            then  the  Complex  will be  equal the  Installlment
            Calculating  the  Final  Book  Value  From  the  Intial Book Value  - Complex of  the  Fixed Asset
            Adding  Object  of  Fixed  Depreciation  to  the  Db  object  of  COntext
            Saving  Infromation  to  Database
            Closing   The  COnnnection  With Database*/
       
        /* Calculate Depreciation expense =(مصروف الاهلاك)
        Connect  With  DBContext
        MAking  Object From  FixedAsset Where  ID   equal  parameter id 
        getting  pruchase price of  asset  and  add it  in  variable  called  Pcost
        getting  junckValue of  asset  and  add it  in  variable  named  PJunk
        getting  the   Age of  the  asset  and  add it in  Variable  Named  PAge
        calculate  the  Expense of  that  Asset where (سعر الاصل-قيمة الخردة)/عمر الاصل)*/
        public static decimal CalculateFixedDepExpense(int id)
        {
            var db = new AccountingContainer();
            FixedAsset Fasset = db.FixedAssets.Find(id);
            decimal Pcost = Convert.ToDecimal(Fasset.FAsset_PurchasePrice);
            decimal PAge = Fasset.FAsset_AgeMonth;
            decimal PJunck = Convert.ToDecimal(Fasset.JunckValue);
            decimal DepExp = (Pcost - PJunck) / PAge;
            return DepExp;
        }
        /// <summary>
        /// calculate different between to date time in mounth
        /// </summary>
        /// <param name="lValue"></param>
        /// <param name="rValue"></param>
        /// <returns></returns>
        
        public static int MonthDifferences(DateTime lValue, DateTime rValue)
        {
            return Math.Abs((lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year));
        }


        /// <summary>
        /// makeing start depratiation for one or more asset and make its gl and change value of asset  
        /// </summary>
        /// <param name="id"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static decimal startDepreciation(int id , String UserId)
        {
            var db = new AccountingContainer(); 
            var thistask = db.DeprTaskStatus.Where(e => e.Asset_id == id).ToList();
            if (thistask.Count > 0)
            {
                return CalculateFixedDepWithListTask(id , UserId);
            }
            else
            {
                return CalculateFixedDepWithList(id , UserId);
            }
        }
        /// <summary>
        /// calulate the depration on dayes for first time in Depratiation and make gl depend on his type and final book Value 
        /// that calculate number of dayes that have of that aset and the cost of Depration on one day in every mounth and 
        /// calculate the total Depratiation and make gl For that
        /// </summary>
        /// <param name="id"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static decimal CalculateFixedDepWithList(int id , String UserId)
        {
            var db = new AccountingContainer();
            var fdep = db.FixedAssets.Find(id);
            if ((fdep.FixedAssetTypeFAsset_TypeID ==11|| fdep.FAsset_AgeMonth==0))
            {
                return 0;
            }
     
            else
            {
                decimal depratem = CalculateFixedDepExpense(id);
                List<AssetDepDaysVM> allDepDays = new List<AssetDepDaysVM>();
                AssetDepDaysVM firstIndex = new AssetDepDaysVM();
                firstIndex.FirstDate = fdep.FAsset_WorkingDate.Date;
                firstIndex.LastDate = fdep.FAsset_WorkingDate.AddMonths(1).AddDays(-1).Date;
                TimeSpan dayscount = firstIndex.LastDate.Date.Subtract(firstIndex.FirstDate.Date);
                firstIndex.NumberofDays = (int)dayscount.TotalDays + 1;
                firstIndex.AmountPerDay = (depratem / firstIndex.NumberofDays);
                firstIndex.Total = (firstIndex.AmountPerDay * firstIndex.NumberofDays);
                allDepDays.Add(firstIndex);
                if (fdep.FAsset_AgeMonth > 1)
                {
                    for (int i = 0; i < fdep.FAsset_AgeMonth - 1; i++)
                    {
                        AssetDepDaysVM anotherIndex = new AssetDepDaysVM();
                        anotherIndex.FirstDate = allDepDays.LastOrDefault().LastDate.AddDays(1);
                        anotherIndex.LastDate = anotherIndex.FirstDate.AddMonths(1).AddDays(-1);
                        dayscount = anotherIndex.LastDate.Date.Subtract(anotherIndex.FirstDate.Date);
                        anotherIndex.NumberofDays = (int)dayscount.TotalDays + 1;
                        anotherIndex.AmountPerDay = (depratem / anotherIndex.NumberofDays);
                        anotherIndex.Total = (anotherIndex.AmountPerDay * anotherIndex.NumberofDays);
                        allDepDays.Add(anotherIndex);
                    }
                }
                decimal complex = Convert.ToDecimal(fdep.Complex);
                decimal FinalBookValue = Convert.ToDecimal(fdep.BookValue);
                foreach (var item in allDepDays)
                {
                    var now = DateTime.Now;
                    if (now.Date >= item.FirstDate.Date && now.Date <= item.LastDate.Date)
                    {
                        var numofdayreset = ((now.Date.Subtract(item.FirstDate.Date)).TotalDays) + 1;
                        decimal totalmoney = (item.AmountPerDay * (int)numofdayreset);
                        complex += totalmoney;
                        FinalBookValue -= totalmoney;
                        break;
                    }
                    complex += item.Total;
                    FinalBookValue -= item.Total;
                }
                fdep.Complex = Convert.ToDouble(Math.Round(complex, 2));
                fdep.BookValue = Convert.ToDouble(Math.Round(FinalBookValue, 2));
                db.Entry(fdep).State = EntityState.Modified;
                var GlMAster = new GLMaster();
                var GlDetail1 = new GLDetail();
                var GlDetail = new GLDetail();
                var task = db.DeprTaskStatus.Where(e => e.Asset_id == fdep.Fasset_ID).ToList().FirstOrDefault();
                if (task == null || task.Count == 0)
                {
                    var saveassettask = new DeprTaskStatus();
                    saveassettask.Asset_id = fdep.Fasset_ID;
                    saveassettask.TaskDate = DateTime.Now.Date;
                    saveassettask.TaskStatus = "اهلاك ثابت للاصل";
                    saveassettask.Count = 1;
                    db.DeprTaskStatus.Add(saveassettask);
                }
                else
                {
                    task.Count += 1;
                    task.TaskDate = DateTime.Today;
                    task.TaskStatus = "اهلاك ثابت للاصل";
                    db.Entry(task).State = EntityState.Modified;
                }
                GlMAster.GL_Date = DateTime.Now.Date;
                GlMAster.GLType_Id = "اهلاك";
                GlMAster.GL_Desc = "اهلاك ثابت للاصل";
                GlMAster.Currency_Id = "ريال سعودي";
                GlMAster.Company_Id = db.Companies.FirstOrDefault().Comp_Id ;
                GlMAster.GLState = TransactionState.Pending;
                GlMAster.Auto = true;
                GlMAster.GLCode = fdep.FAsset_Code;
                var ChartOfAccountExpense = db.ChartOfAccountings.Where(a => a.Account_Name == fdep.FAsset_Type).FirstOrDefault();
                GlDetail.Account_Id = ChartOfAccountExpense.Account_Id;
                GlDetail.Account_Name = ChartOfAccountExpense.Account_Name;
                if (fdep.BussinesUnitBU_Id == null)
                { GlDetail.Bu_Id = 0; }
                else
                { GlDetail.Bu_Id = (int)fdep.BussinesUnitBU_Id; }
                GlDetail.GLDetail_Credit = Math.Round((decimal)complex, 2);//داءن
                GlDetail.GLMasterGLMaster_Id = GlMAster.GLMaster_Id;
                GlDetail.GLDetail_Debit = 0;//مدين
                GlDetail.SubLedger_Id = 0;
                GlDetail.Asset_Id = fdep.Fasset_ID;
                GlDetail.Description_Detail = "اهلاك ثابت للاصل";
                GlMAster.GLDetails.Add(GlDetail);
                var chartexactinstal = db.ChartOfAccountings.Where(c => c.Account_Name.Contains(fdep.FAsset_Type) && c.Account_Code.StartsWith("050204")).ToList().LastOrDefault();
                if (chartexactinstal == null)
                {
                    chartexactinstal = db.ChartOfAccountings.Where(c => c.Account_Name.Contains(fdep.FAsset_Type) && c.Account_Code.StartsWith("050204")).ToList().LastOrDefault();
                }

                GlDetail1.Account_Id = chartexactinstal.Account_Id;
                GlDetail1.Account_Name = chartexactinstal.Account_Name;
                if (fdep.BussinesUnitBU_Id == null)
                { GlDetail.Bu_Id = 0; }
                else
                { GlDetail1.Bu_Id = (int)fdep.BussinesUnitBU_Id; }
                GlDetail1.GLDetail_Credit = 0;//داءن
                GlDetail1.GLDetail_Debit = Math.Round((decimal)complex, 2);//مدين
                GlDetail1.GLMasterGLMaster_Id = GlMAster.GLMaster_Id;
                GlDetail1.SubLedger_Id = fdep.SubLedger_ID;
                GlDetail1.Description_Detail = "اهلاك ثابت للاصل";
                GlDetail1.Asset_Id = fdep.Fasset_ID;
                GlMAster.GLDetails.Add(GlDetail1);
               
                if(GlMAster.GLDetails.Select(a=>a.GLDetail_Credit).Sum()!=0&& GlMAster.GLDetails.Select(a => a.GLDetail_Debit).Sum() != 0)
                {
                    GLMaster.GetBalanceForAccount(GlMAster);
                    GLMaster.GetBalanceForSubLedger(GlMAster);
                    GlMAster.Currency_Id = "الريال السعودي";
                    db.GLMasters.Add(GlMAster);
                    var transBatch = new TransactionBatch();
                    transBatch.Date = DateTime.Now.Date;
                    transBatch.OperationName = "إهلاك";
                    transBatch.OperationId = GlMAster.GLMaster_Id;
                    transBatch.Description = "إهلاك ثابت";
                    transBatch.UserId = UserId;
                    transBatch.Operation_Code = fdep.FAsset_Code;
                    db.TransactionBatches.Add(transBatch);
                    var AssetDateSave = new AssetDepDates();
                    AssetDateSave.Asset_ID = fdep.Fasset_ID;
                    AssetDateSave.Date = DateTime.Today;
                    AssetDateSave.InstallMent_Amount = Math.Round((decimal)complex, 2);
                    AssetDateSave.Percentage = Math.Round((double)complex / (double)fdep.FAsset_PurchasePrice * 100, 4);
                    db.AssetDepDates.Add(AssetDateSave);
                    db.SaveChanges();
                    db.Dispose();
                }
                //db.SaveChanges();
                
                
                return complex;
            }
        
              
           
           
        }


        /// <summary>
        ///  calulate the depration on dayes for After first time in Depratiation and make gl depend on his type and final book Value 
        /// that calculate number of dayes that have of that aset and the cost of Depration on one day in every mounth and 
        /// calculate the total Depratiation and make gl For that
        /// </summary>
        /// <param name="id"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>

        public static decimal CalculateFixedDepWithListTask(int id , String UserId)
        {
            var db = new AccountingContainer();
            var fdep = db.FixedAssets.Find(id);
            if ((fdep.FixedAssetTypeFAsset_TypeID == 11||fdep.FAsset_AgeMonth==0 ) )
            {
                return 0;
            }
         
            else
            {
               var tasks = db.DeprTaskStatus.Where(e => e.Asset_id == id).ToList().OrderByDescending(a => a.TaskDate).FirstOrDefault();
               // var tasks = db.DeprTaskStatus.Where(e => e.Asset_id == id).ToList().LastOrDefault();
                if (tasks.TaskDate < DateTime.Now.Date)
                {
                    decimal depratem = CalculateFixedDepExpense(id);
                    List<AssetDepDaysVM> allDepDays = new List<AssetDepDaysVM>();
                    AssetDepDaysVM firstIndex = new AssetDepDaysVM();
                    firstIndex.FirstDate = fdep.FAsset_WorkingDate.Date;
                    firstIndex.LastDate = fdep.FAsset_WorkingDate.AddMonths(1).AddDays(-1).Date;
                    TimeSpan dayscount = firstIndex.LastDate.Date.Subtract(firstIndex.FirstDate.Date);
                    firstIndex.NumberofDays = (int)dayscount.TotalDays + 1;
                    firstIndex.AmountPerDay = (depratem / firstIndex.NumberofDays);
                    firstIndex.Total = (firstIndex.AmountPerDay * firstIndex.NumberofDays);
                    allDepDays.Add(firstIndex);
                    if (fdep.FAsset_AgeMonth > 1)
                    {
                        for (int i = 0; i < fdep.FAsset_AgeMonth - 1; i++)
                        {
                            AssetDepDaysVM anotherIndex = new AssetDepDaysVM();
                            anotherIndex.FirstDate = allDepDays.LastOrDefault().LastDate.AddDays(1);
                            anotherIndex.LastDate = anotherIndex.FirstDate.AddMonths(1).AddDays(-1);
                            dayscount = anotherIndex.LastDate.Date.Subtract(anotherIndex.FirstDate.Date);
                            anotherIndex.NumberofDays = (int)dayscount.TotalDays + 1;
                            anotherIndex.AmountPerDay = (depratem / anotherIndex.NumberofDays);
                            anotherIndex.Total = (anotherIndex.AmountPerDay * anotherIndex.NumberofDays);
                            allDepDays.Add(anotherIndex);
                        }
                    }
                    decimal complex = Convert.ToDecimal(fdep.Complex);
                    decimal value = 0;
                    decimal FinalBookValue = Convert.ToDecimal(fdep.BookValue);

                    
                    var allDepTest = allDepDays.Where(e => e.LastDate >= tasks.TaskDate.AddDays(1)).ToList();

                    var lastDep = tasks.TaskDate.AddDays(1);


                    foreach (var item in allDepTest)
                    {
                        var now = DateTime.Now;

                        if (now.Date >= lastDep.Date && now.Date <= item.LastDate.Date)
                        {
                            var numofdayreset = ((now.Date.Subtract(lastDep.Date)).TotalDays) + 1;


                           
                            decimal totalmoney = (item.AmountPerDay * (int)numofdayreset);
                            value += totalmoney;
                            complex += totalmoney;
                            FinalBookValue -= totalmoney;
                            lastDep = item.LastDate.AddDays(1);
                            break;

                        }
                        else 
                        {

                            var numofdayreset = ((item.LastDate.Date.Subtract(lastDep.Date)).TotalDays) + 1;


                            
                            decimal totalmoney = (item.AmountPerDay * (int)numofdayreset);
                            value += totalmoney;
                            complex += totalmoney;
                            FinalBookValue -= totalmoney;

                            lastDep = item.LastDate.AddDays(1);



                        }
                    }
                    fdep.Complex = Convert.ToDouble(Math.Round(complex, 2));
                    fdep.BookValue = Convert.ToDouble(Math.Round(FinalBookValue, 2));
                    db.Entry(fdep).State = EntityState.Modified;
                    var GlMAster = new GLMaster();
                    var GlDetail1 = new GLDetail();
                    var GlDetail = new GLDetail();
                    var task = db.DeprTaskStatus.Where(e => e.Asset_id == fdep.Fasset_ID).ToList().FirstOrDefault();
                    if (task == null || task.Count == 0)
                    {
                        var saveassettask = new DeprTaskStatus();
                        saveassettask.Asset_id = fdep.Fasset_ID;
                        saveassettask.TaskDate = DateTime.Now.Date;
                        saveassettask.TaskStatus = "اهلاك ثابت للاصل";
                        saveassettask.Count = 1;
                        db.DeprTaskStatus.Add(saveassettask);
                    }
                    else
                    {
                        task.Count += 1;
                        task.TaskDate = DateTime.Today;
                        task.TaskStatus = "اهلاك ثابت للاصل";
                        db.Entry(task).State = EntityState.Modified;
                    }
                    GlMAster.GL_Date = DateTime.Now.Date;
                    GlMAster.GLType_Id = "اهلاك";
                    GlMAster.GL_Desc = "اهلاك ثابت للاصل";
                    GlMAster.Currency_Id = fdep.Currency;
                    GlMAster.Company_Id = fdep.CompanyComp_Id;
                    GlMAster.GLState = TransactionState.Pending;
                    GlMAster.Auto = true;
                    GlMAster.GLCode = fdep.FAsset_Code;
                    var ChartOfAccountExpense = db.ChartOfAccountings.Where(a => a.Account_Name == fdep.FAsset_Type).FirstOrDefault();
                    GlDetail.Account_Id = ChartOfAccountExpense.Account_Id;
                    GlDetail.Account_Name = ChartOfAccountExpense.Account_Name;
                    if (fdep.BussinesUnitBU_Id == null)
                    { GlDetail.Bu_Id = 0; }
                    else
                    { GlDetail.Bu_Id = (int)fdep.BussinesUnitBU_Id; }
                    GlDetail.GLDetail_Credit = Math.Round((decimal)value, 2);//داءن
                    GlDetail.GLMasterGLMaster_Id = GlMAster.GLMaster_Id;
                    GlDetail.GLDetail_Debit = 0;//مدين
                    GlDetail.SubLedger_Id = 0;
                    GlDetail.Asset_Id = fdep.Fasset_ID;
                    GlDetail.Description_Detail = "اهلاك ثابت للاصل";
                    GlMAster.GLDetails.Add(GlDetail);
                    var account= db.ChartOfAccountings.Where(c => c.Account_Name.Contains(fdep.FAsset_Type) && c.Account_Code.StartsWith("050204")).ToList();
                    account.Reverse();
                    var chartexactinstal = account.FirstOrDefault();
                    var code = "";
                    //if (chartexactinstal == null)
                    //{
                    //    ChartOfAccountingVM accountVM = new ChartOfAccountingVM();
                    //    accountVM.ChartOfAccountingAccount_Id = 110;
                    //    accountVM.Account_Name = fdep.FAsset_Type+" "+"اهلاك";
                    //    accountVM.Account_Curenncy = 1;
                    //    code = ChartOfAccounting.CreateAccount(accountVM);
                    //    chartexactinstal = db.ChartOfAccountings.Where(a => a.Account_Code == code).FirstOrDefault();
                    //}
                  
                
                    GlDetail1.Account_Id = chartexactinstal.Account_Id;
                    GlDetail1.Account_Name = chartexactinstal.Account_Name;
                    if (fdep.BussinesUnitBU_Id == null)
                    { GlDetail.Bu_Id = 0; }
                    else
                    { GlDetail1.Bu_Id = (int)fdep.BussinesUnitBU_Id; }
                    GlDetail1.GLDetail_Credit = 0;//داءن
                    GlDetail1.GLDetail_Debit = Math.Round((decimal)value, 2);//مدين
                    GlDetail1.GLMasterGLMaster_Id = GlMAster.GLMaster_Id;
                    GlDetail1.SubLedger_Id = fdep.SubLedger_ID;
                    GlDetail1.Description_Detail = "اهلاك ثابت للاصل";
                    GlDetail1.Asset_Id = fdep.Fasset_ID;
                    GlMAster.Company_Id = db.Companies.FirstOrDefault().Comp_Id;
                    GlMAster.Currency_Id = db.Currencies.FirstOrDefault().Currency_Type;
                    GlMAster.GLDetails.Add(GlDetail1);
                    db.GLMasters.Add(GlMAster);
              
                    GLMaster.GetBalanceForAccount(GlMAster);
                    GLMaster.GetBalanceForSubLedger(GlMAster);
                    var transBatch = new TransactionBatch();
                    transBatch.Date = DateTime.Now.Date;
                    transBatch.OperationName = "إهلاك";
                    transBatch.OperationId = GlMAster.GLMaster_Id;
                    transBatch.Description = "إهلاك ثابت";
                    transBatch.UserId = UserId;
                    transBatch.Operation_Code = fdep.FAsset_Code;
                    db.TransactionBatches.Add(transBatch);
                    var AssetDateSave = new AssetDepDates();
                    AssetDateSave.Asset_ID = fdep.Fasset_ID;
                    AssetDateSave.Date = DateTime.Today;
                    AssetDateSave.InstallMent_Amount = Math.Round((decimal)complex, 2);
                    AssetDateSave.Percentage = Math.Round((double)complex / (double)fdep.FAsset_PurchasePrice * 100, 4);
                    db.AssetDepDates.Add(AssetDateSave);
                    db.SaveChanges();
                    db.Dispose();
                    return complex;
                }
                else
                {
                    return 0;
                }
            }
           

        }

    }
 
}