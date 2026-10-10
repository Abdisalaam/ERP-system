using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Accountingsystem.Models
{
    public partial class DecreasingDepreciation
    {

        ///*
        // * this  method  is  to  calculate  the  rate  of  depreciation for  all assets  and  for  all months
        // * it  calls  FixedAsset  model   and take  only  his  Age  and  then  start  its  equation 
        // */
        public static double CalculateDecDepRate(int id)
        {
            var db = new AccountingContainer();
            FixedAsset Fasset = db.FixedAssets.Find(id);
            double DepAge = Fasset.FAsset_AgeMonth;
            double deprate = (100 / DepAge) * 2;
            return deprate;
        }

      /// <summary>
      /// Calculate Depratation Of Asset
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
        public static List<double> CalculateDecDepForAllAssets(int id)
        {

            var db = new AccountingContainer();
            var fdep = db.FixedAssets.Find(id);
            var today = DateTime.Today.ToString("dd");
            var MyYear = DateTime.Now.Year.ToString();
            var Mymonth = DateTime.Now.Month.ToString();
            var MyLastDay = new DateTime(Convert.ToInt32(MyYear), Convert.ToInt32(Mymonth), DateTime.DaysInMonth(Convert.ToInt32(MyYear), Convert.ToInt32(Mymonth))).ToString("dd");
            var myyearint = DateTime.Now.Year;
            var startMonth = fdep.FAsset_WorkingDate.Month;
            var startday = fdep.FAsset_WorkingDate.Day;
            var monthNow = DateTime.Now.Month;
            var Datenow = DateTime.Now;
            DateTime assetWorkingDate = (DateTime)fdep.FAsset_WorkingDate;
            var todayworking = assetWorkingDate.Day;
            var MyYearworking = assetWorkingDate.Year.ToString();
            var Mymonthworking = assetWorkingDate.Month.ToString();
            var myDaysInWorkingDateLeft = new DateTime(Convert.ToInt32(MyYearworking), Convert.ToInt32(Mymonthworking), DateTime.DaysInMonth(Convert.ToInt32(MyYearworking), Convert.ToInt32(Mymonthworking))).ToString("dd");
            int workingdaysleft = Convert.ToInt32(myDaysInWorkingDateLeft) - Convert.ToInt32(todayworking);
            DateTime thisday = DateTime.Now;
            TimeSpan diffrenceDays = thisday.Subtract(assetWorkingDate);
            var dayesLeft = ((int)diffrenceDays.TotalDays)-workingdaysleft;
            var monthsofAsset = (int)(diffrenceDays.TotalDays) / 29;
            double DecBookValue = Math.Round((double)fdep.FAsset_PurchasePrice);
            double intialBookValue = 0;
            double instalment = 0;
            double Complex = 0;
            double finalBookValue = 0;
            double totalinstalment = 0;
            double totalComplex = 0;
            double totalcomplexforasset = 0;
            double totalinstalmentneeded = 0;

            double depratem = CalculateDecDepRate(fdep.Fasset_ID);
            double deprat = depratem / Convert.ToInt32(MyLastDay);
            var subledger = db.SubLedgers.Where(c => c.SubLed_Code == fdep.FAsset_Code).ToList().LastOrDefault();
            if (fdep.FAsset_Dispose.Value == false && fdep.FAsset_AgeMonth >= monthsofAsset && dayesLeft != 0)
            {
                if ((fdep.FAsset_Type == "lands") || (fdep.FAsset_Type == "land") || (fdep.FAsset_Type == "ارض") || (fdep.FAsset_Type == "أرض") || (fdep.FAsset_Type == "اراضي") || (fdep.FAsset_Type == "أراضي"))
                {
                }
                else
                {
                    for (int i = 0; i < dayesLeft; i++)
                    {
                        intialBookValue = Math.Round(DecBookValue - Complex);
                        instalment = Math.Round((intialBookValue) * (deprat / 100));
                        Complex = Math.Round(Complex + instalment);
                        finalBookValue = Math.Round(intialBookValue - instalment);

                        totalinstalment = Math.Round(totalinstalment + instalment);
                        totalComplex += Math.Round(totalComplex + Complex);
                        totalcomplexforasset = Math.Round(totalcomplexforasset + Complex);
                        totalinstalmentneeded += Math.Round(instalment);
                        fdep.Complex = totalcomplexforasset;
                        fdep.BookValue = finalBookValue;
                    }
                    db.Entry(fdep).State = EntityState.Modified;
                    // db.SaveChanges();
                    var saveassettask = new DeprTaskStatus();
                    saveassettask.Asset_id = fdep.Fasset_ID;
                    saveassettask.TaskDate = Datenow.Date;
                    saveassettask.TaskStatus = "اهلاك متناقص للاصل";
                    db.DeprTaskStatus.Add(saveassettask);

                    var GlMAster = new GLMaster();
                    GlMAster.GL_Date = Datenow;
                    GlMAster.GLType_Id = "3";
                    GlMAster.GL_Desc = "اهلاك متناقص للاصل";
                    GlMAster.Currency_Id = "1";
                    GlMAster.Company_Id = fdep.CompanyComp_Id;
                    GlMAster.GLState = TransactionState.Pending;
                    GlMAster.Auto = true;
                    GlMAster.GLCode = fdep.FAsset_Code;
                    var GlDetail = new GLDetail();
                    if (fdep.BussinesUnitBU_Id == null)
                    { GlDetail.Bu_Id = 0; }
                    else
                    { GlDetail.Bu_Id = (int)fdep.BussinesUnitBU_Id; }


                    var chartexactcompl = db.ChartOfAccountings.Where(c => c.Account_Name.Contains(fdep.FAsset_Type) && c.Account_Code.StartsWith("020113")).ToList().LastOrDefault();
                    GlDetail.Account_Id = chartexactcompl.Account_Id;
                    GlDetail.Account_Name = chartexactcompl.Account_Name;

                    GlDetail.GLDetail_Credit = (decimal)Complex;//داءن
                    GlDetail.GLMasterGLMaster_Id = GlMAster.GLMaster_Id;
                    GlDetail.GLDetail_Debit = 0;//مدين
                    GlDetail.SubLedger_Id = 0;
                    GlDetail.Asset_Id = fdep.Fasset_ID;
                    GlMAster.GLDetails.Add(GlDetail);
                    //db.GLMasters.Add(GlMAster);
                    //  db.SaveChanges();

                    var GlDetail1 = new GLDetail();
                    if (fdep.BussinesUnitBU_Id == null)
                    { GlDetail.Bu_Id = 0; }
                    else
                    { GlDetail.Bu_Id = (int)fdep.BussinesUnitBU_Id; }


                    var chartexactinstal = db.ChartOfAccountings.Where(c => c.Account_Name.Contains(fdep.FAsset_Type) && c.Account_Code.StartsWith("050204")).ToList().LastOrDefault();
                    GlDetail1.Account_Id = chartexactinstal.Account_Id;
                    GlDetail1.Account_Name = chartexactinstal.Account_Name;

                    GlDetail1.GLDetail_Credit = 0;//داءن
                    GlDetail1.GLDetail_Debit = (decimal)totalinstalment;//مدين
                    GlDetail1.GLMasterGLMaster_Id = GlMAster.GLMaster_Id;
                    GlDetail1.SubLedger_Id = fdep.SubLedger_ID;
                    GlDetail1.Asset_Id = fdep.Fasset_ID;
                    GlMAster.GLDetails.Add(GlDetail1);
                    db.GLMasters.Add(GlMAster);
                    // db.SaveChanges();
                    GLMaster.GetBalanceForAccount(GlMAster);
                    GLMaster.GetBalanceForSubLedger(GlMAster);
                    var transBatch = new TransactionBatch();
                    transBatch.Date = Datenow;
                    transBatch.OperationName = "إهلاك";
                    transBatch.OperationId = GlMAster.GLMaster_Id;
                    transBatch.Description = "إهلاك متناقص";
                    transBatch.UserId = "";
                    db.TransactionBatches.Add(transBatch);
                    db.SaveChanges();
                    db.Dispose();
                }
            }
            else
            {

            }
            List<double> myList = new List<double>();
            myList.Add(totalComplex);
            myList.Add(totalinstalmentneeded);
            return myList;
        }
        public static List<double> CalculateDecDepForAllAssets2(int id)
        {

            var db = new AccountingContainer();
            var fdep = db.FixedAssets.Find(id);
            var today = DateTime.Today.ToString("dd");
            var MyYear = DateTime.Now.Year.ToString();
            var Mymonth = DateTime.Now.Month.ToString();
            var MyLastDay = new DateTime(Convert.ToInt32(MyYear), Convert.ToInt32(Mymonth), DateTime.DaysInMonth(Convert.ToInt32(MyYear), Convert.ToInt32(Mymonth))).ToString("dd");
            var myyearint = DateTime.Now.Year;
            var startMonth = fdep.FAsset_WorkingDate.Month;
            var startday = fdep.FAsset_WorkingDate.Day;
            var monthNow = DateTime.Now.Month;
            var Datenow = DateTime.Now;
            var indexed = db.DeprTaskStatus.Where(r => r.Asset_id == fdep.Fasset_ID).ToList().LastOrDefault();
            DateTime assetWorkingDate = indexed.TaskDate;
            DateTime thisday = DateTime.Now;
            TimeSpan diffrenceDays = thisday.Subtract(assetWorkingDate);
            var dayesLeft = (int)diffrenceDays.TotalDays;
            var monthsofAsset = (int)(diffrenceDays.TotalDays) / 29;
            double DecBookValue = Math.Round((double)fdep.FAsset_PurchasePrice);
            double intialBookValue = 0;
            double instalment = 0;
            double Complex = 0;
            double finalBookValue = 0;
            double totalinstalment = 0;
            double totalComplex = 0;
            double totalcomplexforasset = 0;
            double totalinstalmentneeded = 0;

            double depratem = CalculateDecDepRate(fdep.Fasset_ID);
            double deprat = depratem / Convert.ToInt32(MyLastDay);
            var subledger = db.SubLedgers.Where(c => c.SubLed_Code == fdep.FAsset_Code).ToList().LastOrDefault();
            if (fdep.FAsset_Dispose.Value == false && fdep.FAsset_AgeMonth >= monthsofAsset && dayesLeft != 0)
            {
                if ((fdep.FAsset_Type == "lands") || (fdep.FAsset_Type == "land") || (fdep.FAsset_Type == "ارض") || (fdep.FAsset_Type == "أرض") || (fdep.FAsset_Type == "اراضي") || (fdep.FAsset_Type == "أراضي"))
                {
                }
                else
                {
                    for (int i = 0; i < dayesLeft; i++)
                    {
                        intialBookValue = Math.Round(DecBookValue - Complex);
                        instalment = Math.Round((intialBookValue) * (deprat / 100));
                        Complex = Math.Round(Complex + instalment);
                        finalBookValue = Math.Round(intialBookValue - instalment);

                        totalinstalment = Math.Round(totalinstalment + instalment);
                        totalComplex += Math.Round(totalComplex + Complex);
                        totalcomplexforasset = Math.Round(totalcomplexforasset + Complex);
                        totalinstalmentneeded += Math.Round(instalment);
                        fdep.Complex = totalcomplexforasset;
                        fdep.BookValue = finalBookValue;
                    }
                    db.Entry(fdep).State = EntityState.Modified;
                    // db.SaveChanges();
                    var saveassettask = new DeprTaskStatus();
                    saveassettask.Asset_id = fdep.Fasset_ID;
                    saveassettask.TaskDate = Datenow.Date;
                    saveassettask.TaskStatus = "اهلاك متناقص للاصل";
                    db.DeprTaskStatus.Add(saveassettask);

                    var GlMAster = new GLMaster();
                    GlMAster.GL_Date = Datenow;
                    GlMAster.GLType_Id = "3";
                    GlMAster.GL_Desc = "اهلاك متناقص للاصل";
                    GlMAster.Currency_Id = "1";
                    GlMAster.Company_Id = fdep.CompanyComp_Id;
                    GlMAster.GLState = TransactionState.Pending;
                    GlMAster.Auto = true;
                    GlMAster.GLCode = fdep.FAsset_Code;
                    var GlDetail = new GLDetail();
                    if (fdep.BussinesUnitBU_Id == null)
                    { GlDetail.Bu_Id = 0; }
                    else
                    { GlDetail.Bu_Id = (int)fdep.BussinesUnitBU_Id; }

                    var chartexactcompl = db.ChartOfAccountings.Where(c => c.Account_Name.Contains(fdep.FAsset_Type) && c.Account_Code.StartsWith("020113")).ToList().LastOrDefault();
                    GlDetail.Account_Id = chartexactcompl.Account_Id;
                    GlDetail.Account_Name = chartexactcompl.Account_Name;

                    GlDetail.GLDetail_Credit = (decimal)Complex;//داءن
                    GlDetail.GLMasterGLMaster_Id = GlMAster.GLMaster_Id;
                    GlDetail.GLDetail_Debit = 0;//مدين
                    GlDetail.SubLedger_Id = 0;
                    GlDetail.Asset_Id = fdep.Fasset_ID;
                    GlMAster.GLDetails.Add(GlDetail);
                    //db.GLMasters.Add(GlMAster);
                    //  db.SaveChanges();

                    var GlDetail1 = new GLDetail();
                    if (fdep.BussinesUnitBU_Id == null)
                    { GlDetail.Bu_Id = 0; }
                    else
                    { GlDetail.Bu_Id = (int)fdep.BussinesUnitBU_Id; }

                    var chartexactinstal = db.ChartOfAccountings.Where(c => c.Account_Name.Contains(fdep.FAsset_Type) && c.Account_Code.StartsWith("050204")).ToList().LastOrDefault();

                    GlDetail1.Account_Id = chartexactinstal.Account_Id;
                    GlDetail1.Account_Name = chartexactinstal.Account_Name;

                    GlDetail1.GLDetail_Credit = 0;//داءن
                    GlDetail1.GLDetail_Debit = (decimal)totalinstalment;//مدين
                    GlDetail1.GLMasterGLMaster_Id = GlMAster.GLMaster_Id;
                    GlDetail1.SubLedger_Id = fdep.SubLedger_ID;
                    GlDetail1.Asset_Id = fdep.Fasset_ID;
                    GlMAster.GLDetails.Add(GlDetail1);
                    db.GLMasters.Add(GlMAster);
                    // db.SaveChanges();
                    GLMaster.GetBalanceForAccount(GlMAster);
                    GLMaster.GetBalanceForSubLedger(GlMAster);
                    var transBatch = new TransactionBatch();
                    transBatch.Date = Datenow;
                    transBatch.OperationName = "إهلاك";
                    transBatch.OperationId = GlMAster.GLMaster_Id;
                    transBatch.Description = "إهلاك متناقص";
                    transBatch.UserId = "";
                    db.TransactionBatches.Add(transBatch);
                    db.SaveChanges();
                    db.Dispose();
                }
            }
            else
            {

            }
            List<double> myList = new List<double>();
            myList.Add(totalComplex);
            myList.Add(totalinstalmentneeded);
            return myList;
        }
    }
}