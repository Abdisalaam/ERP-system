using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

using Microsoft.Office.Interop.Excel;

using System.IO;
using System.Runtime.InteropServices;


namespace Accountingsystem.Models
{
    public partial class FixedAsset
    {
        // this  method  is  to  compare  Assets  Depends  on  Their Description  (Hash Code)
        internal class AssetComparer : EqualityComparer<FixedAsset>
        {
            public override bool Equals(FixedAsset x, FixedAsset y)
            {
                return String.Equals(x.FAsset_Description, y.FAsset_Description);
            }
            public override int GetHashCode(FixedAsset obj)
            {
                return obj == null ? 0 : obj.FAsset_Description.GetHashCode();
            }
        }
        //this method get  list of  non  disposedAssets and  compare  its Discription and use  distinct  Method  wich  see  the  ComparerMethod  we  do  and  leave  only  first of  all  similar
        public static List<dynamic> getAllNotDisposedAssets()
        {
            var db = new AccountingContainer();
            var assets = db.FixedAssets.Where(s => s.FAsset_Dispose == false).ToList();
            var distinctAssets = assets.Distinct(new AssetComparer());
            string a = "EEE", b = "EEE";
            bool sss = String.Equals(a, b);
            List<dynamic> assetsList = new List<dynamic>();
            foreach (var asset in distinctAssets)
            {
                var obj = new
                {
                    id = asset.Fasset_ID,
                    text = asset.FAsset_Description
                };
                assetsList.Add(obj);
            }
            return assetsList;
        }
        /*this method  Getts  All  assets  As  list  But  the  Value  of  Dispose  Should  be  False
  * if the  Search  Is null but  if  the  search is  not  null it  will  get only  the  searched  value  and   use  Sorting  for the  Values  that is  displayed
  */
  
        public static List<FixedAsset> getAllAssets(string search, string fixedSort)
        {
            var db = new AccountingContainer();
            //creating  object  from  DBContainer 
            if (search != null)
            {
                //using  switch case  from  every  sorttype
                switch (fixedSort)
                {
                    //make  sort by name  in  descending order  and  dont show  disposed  value  of  FixedAsset
                    case "Name":
                        var fixedAssByName = db.FixedAssets.Where(c => c.FAsset_Code.StartsWith(search) && c.FAsset_Dispose.Value == false);
                        return fixedAssByName.OrderByDescending(u => u.FAssetCat_name).ToList();

                    //make  sort by Price  in  descending order  and  dont show  disposed  value  of  FixedAsset
                    case "Price":
                        var fixedAssByPrice = db.FixedAssets.Where(c => c.FAsset_Code.StartsWith(search) && c.FAsset_Dispose.Value == false);
                        return fixedAssByPrice.OrderByDescending(u => u.FAsset_PurchasePrice).ToList();


                    //make  sort by Code  in  descending order  and  dont show  disposed  value  of  FixedAsset
                    case "Code":
                        var fixedAssByCode = db.FixedAssets.Where(c => c.FAsset_Code.StartsWith(search) && c.FAsset_Dispose.Value == false);
                        return fixedAssByCode.OrderByDescending(u => u.FAsset_Code).ToList();
                    //make  sort by categoryName  and  dont show  disposed  value  of  FixedAsset
                    default:
                        var fixedAssByNameByDefault = db.FixedAssets.Where(c => c.FAsset_Code.StartsWith(search) && c.FAsset_Dispose.Value == false);
                        return fixedAssByNameByDefault.OrderBy(u => u.FAssetCat_name).ToList();

                }

            }
            else
            {
                //show  the  whole  list of  assets  with no  sort and  no  dispose value
                var LAssets = db.FixedAssets.Where(c => c.FAsset_Dispose.Value == false).ToList();
                return LAssets;
            }
        }
        /*this method  Getts  All  assets  As  list  But  the  Value  of  Dispose  Should  be  true
  * if the  Search  Is null but  if  the  search is  not  null it  will  get only  the  searched  value  and   use  Sorting  for the  Values  that is  displayed
  */
        public static List<FixedAsset> getAllDisposeAssets(string search, string fixedSort)
        {
            var db = new AccountingContainer();
            //if (search != null)
            //{
            //    switch (fixedSort)
            //    {
            //        case "Name":
            //            var fixedAssByName = db.FixedAssets.Where(c => c.FAsset_Code.StartsWith(search) && c.FAsset_Dispose.Value == true);
            //            return fixedAssByName.OrderByDescending(u => u.FAssetCat_name).ToList();


            //        case "Price":
            //            var fixedAssByPrice = db.FixedAssets.Where(c => c.FAsset_Code.StartsWith(search) && c.FAsset_Dispose.Value == true);
            //            return fixedAssByPrice.OrderByDescending(u => u.FAsset_PurchasePrice).ToList();



            //        case "Code":
            //            var fixedAssByCode = db.FixedAssets.Where(c => c.FAsset_Code.StartsWith(search) && c.FAsset_Dispose.Value == true);
            //            return fixedAssByCode.OrderByDescending(u => u.FAsset_Code).ToList();

            //        default:
            //            var fixedAssByNameByDefault = db.FixedAssets.Where(c => c.FAsset_Code.StartsWith(search) && c.FAsset_Dispose.Value == true);
            //            return fixedAssByNameByDefault.OrderBy(u => u.FAssetCat_name).ToList();

            //    }

            //}
            //else
            //{
                var LAssets = db.FixedAssets.Where(c => c.FAsset_Dispose.Value == true).ToList();
                return LAssets;
            //}
        }
        // get  any asset  by its  ID and it  is  used  by  the system where  it  passes  the  ID  which  is  got  from  view  to controller  to here
        //and  search in DB  by this  Id  and  Get  its  Object  


        // Get all not disposed distinct assets to be converted to json

        // get  any asset  by its  ID and it  is  used  by  the system where  it  passes  the  ID  which  is  got  from  view  to controller  to here
        //and  search in DB  by this  Id  and  Get  its  Object  
        public static FixedAsset GetAssetByID(int ID)
        {
            var db = new AccountingContainer();
            var fixedasset = db.FixedAssets.Where(c => c.Fasset_ID == ID);
            var fAsset = new FixedAsset();
            /// here  we  was  going  to  add the disposed  Asset  but not now
            if (fixedasset != null)
            {
                fAsset = fixedasset.FirstOrDefault();
                return fAsset;
            }
            return null;
        }
        public static FixedAssetInformation GetAssetByIDfromNonAppearEntity(int ID)
        {
            var db = new AccountingContainer();
            var fixedasset = db.FixedAssetInformations.Where(c => c.Id == ID);
            var fAsset = new FixedAssetInformation();
            /// here  we  was  going  to  add the disposed  Asset  but not now
            if (fixedasset != null)
            {
                fAsset = fixedasset.FirstOrDefault();
                return fAsset;
            }
            return null;
        }
        //making  list  of  attributs  in (FixedAssetCategory,FixedAssetType,BussinesUnit,Employee,COmpany)
        //the  list  uses  the  ID  of  Object inside  but  show  Names only
        public static FixedAssetVM createVMForCreation()
        {
            var db = new AccountingContainer();
            var fixedasset = new FixedAssetVM();
            string startwithcodeTypeAsset = "0102";
            string startwithcodeCategoryAsset = "0102";
            fixedasset.MyCategoryList = new SelectList(db.ChartOfAccountings.Where(a => a.Account_Code.StartsWith(startwithcodeCategoryAsset) && a.Account_Level == 4), "Account_Code", "Account_Name");
            fixedasset.MyFAssetType = new SelectList(db.ChartOfAccountings.Where(a => a.Account_Code.StartsWith(startwithcodeTypeAsset) && a.Account_Level == 3), "Account_Code", "Account_Name");

            fixedasset.myBUnit = new SelectList(db.SubLedgers.Where(q=>q.isActive!=false), "SubLed_Id", "SubLed_Name"); new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            fixedasset.myEmployee = new SelectList(db.Employees, "Emp_Id", "Emp_Name");
            fixedasset.myCompaney = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            fixedasset.SubledgerList = new SelectList(db.SubLedgers.Where(e=>e.SubLed_Type== "مركز تكلفة يدوى" && e.isActive != false), "SubLed_Id", "SubLed_Name");
            fixedasset.DepListList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.isActive != false && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
            fixedasset.BranshList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.isActive != false && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
            var lastCode = db.FixedAssetInformations.OrderByDescending(c => c.Id).FirstOrDefault();
            if (lastCode == null)
            {
                fixedasset.FAsset_Codes = "AS-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                fixedasset.FAsset_Codes = "AS-" + (Convert.ToInt32(lastCode.FAsset_Code.Substring(3, lastCode.FAsset_Code.Length - 3)) + 1).ToString("D3");
            }
            return fixedasset;
        }
        public static bool containCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.FixedAssets.ToList();
            foreach (var item in allinData)
            {

                if (item.FAsset_Code == Serial || !Serial.StartsWith("AS-"))
                {
                    return true;
                }
                else
                {

                }

            }
            return false;
        }
        public static bool containCodeInfo(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.FixedAssetInformations.ToList();
            foreach (var item in allinData)
            {

                if (item.FAsset_Code == Serial || !Serial.StartsWith("AS-"))
                {
                    return true;
                }
                else
                {

                }

            }
            return false;
        }
        public static bool containCodeInfoOpen(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.FixedAssets.ToList();
            foreach (var item in allinData)
            {

                if (item.FAsset_Code == Serial || !Serial.StartsWith("AS-"))
                {
                    return true;
                }
                else
                {

                }

            }
            return false;
        }
        public static bool containCodeOpen(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.FixedAssets.ToList();
            foreach (var item in allinData)
            {
                if (item.FAsset_Code == Serial || !Serial.StartsWith("OPA-"))
                {
                    return true;
                }
                else
                {

                }

            }
            return false;
        }
        public static FixedAsset GetFixedAssetByCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.FixedAssets.Where(a=>a.FAsset_Code==Serial).FirstOrDefault();
            return allinData;
        }

        // here , we  are  adding  new Asset  Using The View Model which  we 
        //made  in the  Parameter  and  use  Object OF the container  To  call the  Database
        //making  object  of  this  class  to  set data from  VM  to it  and  then  add  to Database  
        public static FixedAssetInformation AddNewAsset(FixedAssetVM FVM)
        {
            var db = new AccountingContainer();
            var fixedAsset = new FixedAssetInformation();
            //this  object  holds  the  record  from  chart of  account  where  its  level  is  2  and  its  code  start  with 0102
            var chart = db.ChartOfAccountings.Where(c => c.Account_Level == 2 && c.Account_Code == "0102").ToList().FirstOrDefault();
            try
            {
                if (FVM != null)
                {
                    //this  object  holds  the  record  from  chart of  account  where  its  code  equals  type   that  is  choosed
                    var FATResult = db.ChartOfAccountings.Where(t => t.Account_Code == FVM.SelectedFixedAsset_TypeId).FirstOrDefault();
                    if (FATResult != null)
                    {
                        fixedAsset.FAsset_Type = FATResult.Account_Name;
                        //this variable contains  the  id  of  FixedaSSETS  THAT  will be  used  in  Last  variable +1 to make  the  code  of  the  Fixed Asset
                        int catCounter = (from count in db.FixedAssetInformations
                                          select count.Id).ToList().LastOrDefault();
                        var lastCode = db.FixedAssetInformations.OrderByDescending(c => c.Id).FirstOrDefault();
                        if (lastCode == null)
                        {
                            fixedAsset.FAsset_Code = "AS-001";
                        }
                        else
                        {
                            //using string substring method to get the number of the last inserted
                            fixedAsset.FAsset_Code = "AS-" + (Convert.ToInt32(lastCode.FAsset_Code.Substring(3, lastCode.FAsset_Code.Length - 3)) + 1).ToString("D3");
                        }
                        fixedAsset.FixedAssetTypeFAsset_TypeID = FATResult.Account_Id;



                    }
                    //this  object  holds  the  record  from  chart of  account  where  its  code  equals  category   that  is  choosed
                    var FASResult = db.ChartOfAccountings.Where(t => t.Account_Code == FVM.SelectedFixedAssetSecCategoryId).FirstOrDefault();
                    if (FASResult != null)
                    {
                        fixedAsset.FAssetCat_name = FASResult.Account_Name;
                        //this variable contains  the  id  of  FixedaSSETS  THAT  will be  used  in  Last  variable +1 to make  the  code  of  the  Fixed Asset
                        int catCounter = (from count in db.FixedAssetInformations
                                          select count.Id).ToList().LastOrDefault();
                        int last = catCounter + 1;
                        var lastCode = db.FixedAssetInformations.OrderByDescending(c => c.Id).FirstOrDefault();
                        if (lastCode == null)
                        {
                            fixedAsset.FAsset_Code = "AS-001";
                        }
                        else
                        {
                            //using string substring method to get the number of the last inserted
                            fixedAsset.FAsset_Code = "AS-" + (Convert.ToInt32(lastCode.FAsset_Code.Substring(3, lastCode.FAsset_Code.Length - 3)) + 1).ToString("D3");
                        }


                        fixedAsset.FixedAssetSecCategoryFAsset_CatID = FASResult.Account_Id;


                    }
                    var subResult = db.SubLedgers.Find(FVM.SubLedger_ID);
                    if (subResult != null)
                    {
                        fixedAsset.SubLedger_ID = subResult.SubLed_Id;
                    }
                    else
                    { if (FVM.FAsset_BUs != 0 && FVM.FAsset_BUs != null)
                        {
                            fixedAsset.SubLedger_ID = (int)FVM.FAsset_BUs;

                        }
                        else if ((FVM.FAsset_BUs == 0 || FVM.FAsset_BUs == null) && (FVM.DepartmentID != 0 && FVM.DepartmentID != null))
                        {
                            fixedAsset.SubLedger_ID = (int)FVM.DepartmentID;
                        }
                        else if ((FVM.FAsset_BUs == 0 || FVM.FAsset_BUs == null) && (FVM.DepartmentID == 0 || FVM.DepartmentID == null) && (FVM.BranchID != 0 && FVM.BranchID != null))
                        {
                            fixedAsset.SubLedger_ID = (int)FVM.BranchID;
                        }
                        else
                        {
                            var thissub = db.SubLedgers.Where(e => e.Subled_Level == 0).FirstOrDefault();
                            fixedAsset.SubLedger_ID = (int)thissub.SubLed_Id;
                        }
                    }
                    var FAEResult = db.Employees.Find(FVM.selectedFAsset_Employees_ID);
                    if (FAEResult != null)
                    {
                        fixedAsset.FAsset_Employee_ID = FAEResult.Emp_Id;
                    }
                    var FACResult = db.Companies.Find(FVM.selectedCompany_Id);
                    if (FACResult != null)
                    {
                        fixedAsset.CompanyComp_Id = (int)FVM.selectedCompany_Id; //FACResult.Comp_Id;
                    }


                    fixedAsset.FAsset_AgeMonth = FVM.FAsset_AgeMonths;
                    fixedAsset.FAsset_Description = FVM.FAsset_Descriptions;


                    fixedAsset.JunckValue = FVM.JunckValue;
                    fixedAsset.FAsset_WorkingDate = Convert.ToDateTime(FVM.FAsset_WorkingDates).Date;
                 
                    //subledger.SubLed_Name = fixedAsset.FAsset_Description;
                    //subledger.SubLed_Date = DateTime.Now;
                    //subledger.SubLed_Desc = chart.Account_Name;
                    db.FixedAssetInformations.Add(fixedAsset);
                    if (FVM.Files.FirstOrDefault() != null)
                    {
                        foreach (var file in FVM.Files)
                        {
                            var filedetails = new FixedAssetFileDetailInfo();
                            var data = new byte[file.ContentLength];
                            filedetails.FileName = file.FileName;
                            filedetails.File_Type = file.ContentType;
                            filedetails.ContentLength = file.ContentLength;
                            file.InputStream.Read(data, 0, file.ContentLength);
                            filedetails.ProImg = data;
                            filedetails.FixedAssetInformationId1 = fixedAsset.Id;
                            if (data.Length < 1000000)
                            {
                                fixedAsset.FixedAssetFileDetailInfoes.Add(filedetails);
                            }
                            
                        }
                    }
                    //db.SubLedgers.Add(subledger);
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
                    db.Dispose();
                    return fixedAsset;
                }
                return fixedAsset;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                throw;
            }
            // return null;
        }
        public static FixedAssetVM createVMForEditInfo(int id)
        {
            var db = new AccountingContainer();
            var thisfixed = db.FixedAssetInformations.Find(id);
            var fixedasset = new FixedAssetVM();
            string startwithcodeTypeAsset = "0102";
            string startwithcodeCategoryAsset = "0102";
            fixedasset.FAsset_Codes = thisfixed.FAsset_Code;
            fixedasset.selectedCompany_Id = thisfixed.CompanyComp_Id;
            fixedasset.FAsset_Descriptions = thisfixed.FAsset_Description;
            fixedasset.BranchID = thisfixed.SubLedger_ID;
         
            fixedasset.FAsset_WorkingDates = thisfixed.FAsset_WorkingDate.ToString("dd-MM-yyyy   HH:mm:ss");
           
            if (thisfixed.FAsset_Employee_ID == null)
            {

            }
            else
            {
                fixedasset.selectedFAsset_Employees_ID = (int)thisfixed.FAsset_Employee_ID;
            }
           
            fixedasset.JunckValue =(double) thisfixed.JunckValue;
            fixedasset.FAsset_AgeMonths = thisfixed.FAsset_AgeMonth;
            var thistypechart = db.ChartOfAccountings.Find(Convert.ToInt32(thisfixed.FixedAssetTypeFAsset_TypeID));
            fixedasset.SelectedFixedAsset_TypeId = thistypechart.Account_Code;
            fixedasset.MyCategoryList = new SelectList(db.ChartOfAccountings.Where(a => a.Account_Code.StartsWith(startwithcodeCategoryAsset) && a.Account_Level == 4), "Account_Code", "Account_Name");
            fixedasset.FAsset_Types = thisfixed.FAsset_Type;
            fixedasset.MyFAssetType = new SelectList(db.ChartOfAccountings.Where(a => a.Account_Code.StartsWith(startwithcodeTypeAsset) && a.Account_Level == 3), "Account_Code", "Account_Name");
            fixedasset.Fasset_IDs = thisfixed.Id;
            fixedasset.myBUnit = new SelectList(db.SubLedgers, "SubLed_Id", "SubLed_Name");
            fixedasset.myEmployee = new SelectList(db.Employees, "Emp_Id", "Emp_Name");
            fixedasset.myCompaney = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            fixedasset.SubledgerList = new SelectList(db.SubLedgers.Where(e => e.SubLed_Type == "يدوي"), "SubLed_Id", "SubLed_Name");
            fixedasset.DepListList = new SelectList(db.SubLedgers, "SubLed_Id", "SubLed_Name");
            fixedasset.BranshList = new SelectList(db.SubLedgers.Where(e => e.Subled_Level == 1), "SubLed_Id", "SubLed_Name");
      
            return fixedasset;
        }
        public static FixedAssetInformation EditFixedAssetInformation(FixedAssetVM FVM)
        {
            var db = new AccountingContainer();
            var fixedAsset = db.FixedAssetInformations.Find(FVM.Fasset_IDs);
            var chart = db.ChartOfAccountings.Where(c => c.Account_Level == 2 && c.Account_Code == "0102").ToList().FirstOrDefault();
          
            
            try
            {
                if (FVM != null)
                {
                    //this  object  holds  the  record  from  chart of  account  where  its  code  equals  type   that  is  choosed
                    var FATResult = db.ChartOfAccountings.Where(t => t.Account_Code == FVM.SelectedFixedAsset_TypeId).ToList().FirstOrDefault();
                    if (FATResult != null)
                    {

                       
                        fixedAsset.FAsset_Type = FATResult.Account_Name;
                        //this variable contains  the  id  of  FixedaSSETS  THAT  will be  used  in  Last  variable +1 to make  the  code  of  the  Fixed Asset
                        int catCounter = (from count in db.FixedAssetInformations
                                          select count.Id).ToList().LastOrDefault();
                    
                        fixedAsset.FixedAssetTypeFAsset_TypeID = FATResult.Account_Id;



                    }
                    //this  object  holds  the  record  from  chart of  account  where  its  code  equals  category   that  is  choosed
                    var FASResult = db.ChartOfAccountings.Where(t => t.Account_Code == FVM.SelectedFixedAssetSecCategoryId).FirstOrDefault();
                    if (FASResult != null)
                    {
                        fixedAsset.FAssetCat_name = "";
                        fixedAsset.FAssetCat_name = FASResult.Account_Name;
                        //this variable contains  the  id  of  FixedaSSETS  THAT  will be  used  in  Last  variable +1 to make  the  code  of  the  Fixed Asset
                       

                        fixedAsset.FixedAssetSecCategoryFAsset_CatID = FASResult.Account_Id;


                    }
                    var subResult = db.SubLedgers.Find(FVM.SubLedger_ID);
                    if (subResult != null)
                    {
                        fixedAsset.SubLedger_ID = subResult.SubLed_Id;
                    }
                    else
                    {
                        if (FVM.FAsset_BUs != 0 && FVM.FAsset_BUs != null)
                        {
                            fixedAsset.SubLedger_ID = (int)FVM.FAsset_BUs;

                        }
                        else if ((FVM.FAsset_BUs == 0 || FVM.FAsset_BUs == null) && (FVM.DepartmentID != 0 && FVM.DepartmentID != null))
                        {
                            fixedAsset.SubLedger_ID = (int)FVM.DepartmentID;
                        }
                        else if ((FVM.FAsset_BUs == 0 || FVM.FAsset_BUs == null) && (FVM.DepartmentID == 0 || FVM.DepartmentID == null) && (FVM.BranchID != 0 && FVM.BranchID != null))
                        {
                            fixedAsset.SubLedger_ID = (int)FVM.BranchID;
                        }
                        else
                        {
                            var thissub = db.SubLedgers.Where(e => e.Subled_Level == 0).FirstOrDefault();
                            fixedAsset.SubLedger_ID = (int)thissub.SubLed_Id;
                        }
                    }
                    var FAEResult = db.Employees.Find(FVM.selectedFAsset_Employees_ID);
                    if (FAEResult != null)
                    {
                        fixedAsset.FAsset_Employee_ID = FAEResult.Emp_Id;
                    }
                    var FACResult = db.Companies.Find(FVM.selectedCompany_Id);
                    if (FACResult != null)
                    {
                        fixedAsset.CompanyComp_Id = (int)FVM.selectedCompany_Id; //FACResult.Comp_Id;
                    }


                    fixedAsset.FAsset_AgeMonth = FVM.FAsset_AgeMonths;
                    fixedAsset.FAsset_Description = FVM.FAsset_Descriptions;


                    fixedAsset.JunckValue = FVM.JunckValue;
                    fixedAsset.FAsset_WorkingDate = Convert.ToDateTime(FVM.FAsset_WorkingDates).Date;
                    
                    //subledger.SubLed_Name = fixedAsset.FAsset_Description;
                    //subledger.SubLed_Date = DateTime.Now;
                    //subledger.SubLed_Desc = chart.Account_Name;
                    db.Entry(fixedAsset).State = EntityState.Modified;
                    if (FVM.Files.FirstOrDefault() != null)
                    {
                        foreach (var file in FVM.Files)
                        {
                            var filedetails = new FixedAssetFileDetailInfo();
                            var data = new byte[file.ContentLength];
                            filedetails.FileName = file.FileName;
                            filedetails.File_Type = file.ContentType;
                            filedetails.ContentLength = file.ContentLength;
                            file.InputStream.Read(data, 0, file.ContentLength);
                            filedetails.ProImg = data;
                            filedetails.FixedAssetInformationId1 = fixedAsset.Id;
                            if (data.Length < 1000000)
                            {
                                fixedAsset.FixedAssetFileDetailInfoes.Add(filedetails);
                            }
                           
                        }
                    }
                    //db.SubLedgers.Add(subledger);
                    db.SaveChanges(); 
                   
                    db.Dispose();
                    return fixedAsset;
                }
                return fixedAsset;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                throw;
            }


        }
        public static string deleteFixedassetInfo(int id)
        {
            var db = new AccountingContainer();
            var thisasset = db.FixedAssetInformations.Find(id);
            var thisFiles = db.FixedAssetFileDetailInfoes.Where(e => e.FixedAssetInformationId1 == id).ToList();
            if (thisFiles.Count != 0)
            {
                foreach (var file in thisFiles)
                {
                    db.FixedAssetFileDetailInfoes.Remove(file);
                }
                db.SaveChanges();
            }

            db.FixedAssetInformations.Remove(thisasset);
            db.SaveChanges();
            db.Dispose();
            return "asset deleted";
        }
        public static List<FixedAssetInformation> ListofAssetInfo()
        {
            var db =new AccountingContainer();
            var allassets = db.FixedAssetInformations.Where(a=>a.IsActive!=false).ToList();
            return allassets;
        }
        public static FixedAssetInformation getinfoByID(int id)
        {
            var db = new AccountingContainer();
            var thisinfo = db.FixedAssetInformations.Find(id);
            return thisinfo;
        }
        public static FixedAssetFileDetail getFileById(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.FixedAssetFileDetails.Find(fileId);
            return file;
        }
        public static List<FixedAssetFileDetail> GetAllFiles(int FassetId)
        {
            var db = new AccountingContainer();
            var files = db.FixedAssetFileDetails.Where(emp => emp.FixedAssetFasset_ID == FassetId).ToList();
            return files;
        }
        // this  method  is  to remove  with no  Delete  for  the  Asset   it  just make  the  dispose  Value  from Flase  to  true
        //then  get   the complex of   it  and  subtract  from it  the  price  of  Asset  and  Make  automatic GL 
        public static FixedAsset DisposeFixedAsset(int id , String UserId)
        {
            var db = new AccountingContainer();
            DateTime Datenow = DateTime.Now;
            FixedAsset MyAsset = db.FixedAssets.Find(id);
            //this  object  have   the  record  of  Subledger  where  its  code  equal  to  FixedAsset code  which  we  got  it  by the  ID
            var subledger = db.SubLedgers.Where(c => c.SubLed_Code == MyAsset.FAsset_Code).ToList().LastOrDefault();
            FixedDepreciation fdep = new FixedDepreciation();
            //this  variable  calculate  the  diffrence  between  the  pirce  of  the  asset  and  the  Complex  of  it  to  add  it  in The  GeneralLedger
            double diffrOFComplexAndPrice = Convert.ToDouble(MyAsset.FAsset_PurchasePrice - MyAsset.Complex);
            MyAsset.Complex = diffrOFComplexAndPrice;
            if (MyAsset.FAsset_Dispose.Value == false)
            {
                MyAsset.FAsset_Dispose = true;
                MyAsset.FAsset_SellDate = Datenow;
                db.Entry(MyAsset).State = EntityState.Modified;
                db.SaveChanges();
                //From here  we  start  make  the Gl  of  Dispose  1  master  and  2  detail  (from  and  to)
                var GlMAster = new GLMaster();
                GlMAster.GL_Date = Datenow;
                GlMAster.GLType_Id = "3";
                GlMAster.GL_Desc = "حساب وسيط";
                GlMAster.Currency_Id = "2";
                GlMAster.Company_Id = MyAsset.CompanyComp_Id;
                GlMAster.GLCode = MyAsset.FAsset_Code;
                GlMAster.GLState = TransactionState.Pending;
                GlMAster.Auto = true;

                var GlDetail = new GLDetail();
                GlDetail.SubLedger_Id = 0;
                GlDetail.Description_Detail = "حساب وسيط";
                var ChartOfAccounDepr = db.ChartOfAccountings.Find(120);
                GlDetail.Account_Id = ChartOfAccounDepr.Account_Id;
                GlDetail.Account_Name = ChartOfAccounDepr.Account_Name;

                GlDetail.GLDetail_Credit = (decimal)diffrOFComplexAndPrice;//داءن
                GlDetail.GLMasterGLMaster_Id = GlMAster.GLMaster_Id;
                GlDetail.GLDetail_Debit = 0;//مدين
                if (MyAsset.BussinesUnitBU_Id == null)
                {
                    GlDetail.Bu_Id = 0;
                }
                else
                {
                    GlDetail.Bu_Id = (int)MyAsset.BussinesUnitBU_Id;
                }

                GlDetail.Asset_Id = MyAsset.Fasset_ID;
                GlDetail.SubLedger_Id = MyAsset.SubLedger_ID;
                GlMAster.GLDetails.Add(GlDetail);


                var GlDetail1 = new GLDetail();
                GlDetail1.SubLedger_Id = 0;

                var ChartOfAccounComplex = db.ChartOfAccountings.Find(120);
                GlDetail1.Account_Id = ChartOfAccounComplex.Account_Id;
                GlDetail1.Account_Name = ChartOfAccounComplex.Account_Name;
                GlDetail1.Description_Detail = "حساب وسيط";
                GlDetail1.GLDetail_Credit = 0;//داءن
                GlDetail1.GLDetail_Debit = (decimal)diffrOFComplexAndPrice;//مدين
                if (MyAsset.BussinesUnitBU_Id == null)
                {
                    GlDetail1.Bu_Id = 0;
                }
                else
                {
                    GlDetail1.Bu_Id = (int)MyAsset.BussinesUnitBU_Id;
                }

                GlDetail1.GLMasterGLMaster_Id = GlMAster.GLMaster_Id;
                GlDetail1.Asset_Id = MyAsset.Fasset_ID;
                GlDetail1.SubLedger_Id = 0;
                GlMAster.GLDetails.Add(GlDetail1);

                db.GLMasters.Add(GlMAster);
                db.SaveChanges();
                MyAsset.BookValue = 0;
                db.Entry(MyAsset).State = EntityState.Modified;
                db.SaveChanges();
                //the   object  is  to  make  the  Batch number  from Dispose
                var transBatch = new TransactionBatch();
                transBatch.Date = Datenow;
                transBatch.OperationName = "إستبعاد";
                transBatch.OperationId = GlMAster.GLMaster_Id;
                transBatch.Description = "إستبعاد أصل";
                transBatch.UserId = UserId;
                transBatch.Operation_Code = MyAsset.FAsset_Code;
                db.TransactionBatches.Add(transBatch);
                db.SaveChanges();
                db.Dispose();
                return MyAsset;
            }
            else
            {
                return MyAsset;
            }
        }
        //this  MEthod  is  just  to  Edit  THe  Bessinus Unit of  an asset and  its  workingDate ,Subledger
        // it  rewrite  the  new  Value  on the  past value and  update  the  db
        public static FixedAsset EditFixedAsset(FixedAssetVM FVM)
        {
            try
            {
                var db = new AccountingContainer();
                FixedAsset MyAsset = db.FixedAssets.Find(FVM.Fasset_IDs);
                if (MyAsset != null)
                {
                  
              

                    MyAsset.FAsset_WorkingDate = Convert.ToDateTime(FVM.FAsset_WorkingDates).Date;
                    db.Entry(MyAsset).State = EntityState.Modified;
                    db.SaveChanges();
                    db.Dispose();
                    return MyAsset;
                }
                return null;
            }
            catch
            {
                var db = new AccountingContainer();
                FixedAsset MyAsset = db.FixedAssets.Find(FVM.Fasset_IDs);
                return MyAsset;
            }


        }
        //this  method  is  to fill the   ViewModel of the  Edit  by  the  Value  of (Bussines unit,workingdate,Employee)
        // of  the  asset   and  can  be  changaed
        public static FixedAssetVM FillTheViewModel(int ID)
        {
            var db = new AccountingContainer();
            var editModel = GetAssetByID(ID);
            FixedAssetVM FVM = new FixedAssetVM();
            FVM.Fasset_IDs = editModel.Fasset_ID;
            FVM.FAsset_WorkingDates = editModel.FAsset_WorkingDate.ToString();
            var BU = db.BussinesUnits.Where(d => d.BU_Id == editModel.BussinesUnitBU_Id);
            if (BU.Any())
            {
                FVM.FAsset_BUs = BU.FirstOrDefault().BU_Id;
            }
            FVM.myBUnit = new SelectList(db.BussinesUnits, "BU_Id", "BU_Name");
            var EMp = db.Employees.Where(d => d.Emp_Id == editModel.FAsset_Employee_ID);
            if (EMp.Any())
            {
                FVM.selectedFAsset_Employees_ID = EMp.FirstOrDefault().Emp_Id;
            }
            FVM.myEmployee = new SelectList(db.Employees, "Emp_Id", "Emp_Name");
            return FVM;

        }
        /*this  Method  is to Calculate  1 Asset Depreciation (Fixed  Or  Decreasing)
        //public static double startDep(FixedAssetVM FVM)
        //{
        //    double totalcomplex = 0;
        //    double totalInstallment = 0;
        //    // var oneAssetDep= DecreasingDepreciation.CalculateDecreasingDepfor1Asset(FVM.Fasset_IDs);
        //    var oneAssetDep = FixedDepreciation.CalculateFixedDep1Asset(FVM.Fasset_IDs);

        //    totalcomplex += oneAssetDep[0];
        //    totalInstallment += oneAssetDep[1];
        //    //the  second variable  will be  added  in  an object  to use totalcomplex
        //    return totalInstallment;
        //}*/
        //this  Method  is to Calculate All Asset Depreciation (Fixed  Or  Decreasing)
        /// <summary>
        ///  it  containes  the  2  method  of  Depreciation (Fixed  And  decreasing)  but i am commenting 
        ///  the  Decreasing until  the   Type  of  Depreciation work     at  the  intialization of  the system
        /// </summary>
        /// <returns></returns>
        public static decimal startAllDep(String UserId)
        {

            var db = new AccountingContainer();
            //this object  is   the  Hold  list  of  fixed  assets  where  their  dispose  value  equal  false  
            var result = db.FixedAssets.Where(c => c.FAsset_Dispose.Value == false).ToList().Where(a=>a.FAsset_AgeMonth>0&&a.FixedAssetTypeFAsset_TypeID!=11);
            DateTime Datenow = DateTime.Now;
            double TotalComplex = 0.0;
            double TotalInstallment = 0.0;
            var choosed = new CompanyConfigration();
            CompanyConfigration first = db.CompanyConfigrations.FirstOrDefault();
            if (first == null)
            {
                first=new CompanyConfigration() {ChosenBoolean=true,TypeOfConfigration="اهلاك ثابت" };
            }
            var today = DateTime.Today.ToString("dd");
            var MyYear = DateTime.Now.Year.ToString();
            var Mymonth = DateTime.Now.Month.ToString();
            var MyLastDay = new DateTime(Convert.ToInt32(MyYear), Convert.ToInt32(Mymonth), DateTime.DaysInMonth(Convert.ToInt32(MyYear), Convert.ToInt32(Mymonth))).ToString("dd");
            int countr = 0;
            decimal allaseeFixtDep = 0;
            //this  loop  for  the  list   of  Fixed assets  take  each one  and  make  depreciation on it
            //if (Datenow.Day == Convert.ToInt32(MyLastDay))
            //{
                foreach (var x in result)
                {

                    if (x.FAsset_WorkingDate == null || x.FAsset_PurchasePrice == null)
                    { }
                
                    else
                    {
                        //this  object  have  last  record  of  dep happen  where  its  id  equal  to  the  looped  on asset
                        var indexed = db.DeprTaskStatus.Where(r => r.Asset_id == x.Fasset_ID).ToList().LastOrDefault();
                        //making  if  condetion  on  indexed  object  if  it  is  null  it  make  the  depreciation
                        if (indexed == null)
                        {
                            if (first.ChosenBoolean == true)
                            {
                            /* var allaseeFixtDep = FixedDepreciation.CalculateFixedDepForAllAssets(x.Fasset_ID);*/
                             allaseeFixtDep = FixedDepreciation.startDepreciation(x.Fasset_ID , UserId);
                                 countr++;
                            }
                            else
                            {
                                //THIS IS  FOR  FIXED DEPRECIATION 
                                // allaseetDecDep = DecreasingDepreciation.CalculateDecDepForAllAssets(x.Fasset_ID);
                                // THIS  IS  FOR  DECREASING  DEPRECIATION >> remove  the  COmments   please
                                // var allaseetDecDep = DecreasingDepreciation.CalculateDecDepForAllAssets(x.Fasset_ID);
                                countr++;
                            }

                        }
                        //this  condetion  work  when  the   indexed  have  a  value  and  the  date in the  value  equals  the  date 
                        //of  today  with  day and  month and  it  will notdo  anything  so  no  depreciation happen in  the  same  asst
                        //in one  day
                        //else if (indexed.TaskDate.Year == DateTime.Now.Year && indexed.TaskDate.Month == DateTime.Now.Month)
                        //{

                        //}
                        else if (indexed.TaskDate==DateTime.Now)
                        {

                        }
                        //here  we  will make  depreciation  for  time  between  last  tiem  of  depreciation  and   today
                        else
                        {
                            if (first.ChosenBoolean == true)
                            {
                            //var allaseeFixtDep = FixedDepreciation.CalculateFixedDepForAllAssets2(x.Fasset_ID);
                             allaseeFixtDep = FixedDepreciation.startDepreciation(x.Fasset_ID , UserId);
                            countr++;
                            }
                            else
                            {
                                //THIS IS  FOR  FIXED DEPRECIATION 
                                //var allaseetDecDep = DecreasingDepreciation.CalculateDecDepForAllAssets2(x.Fasset_ID);
                                // THIS  IS  FOR  DECREASING  DEPRECIATION >> remove  the  COmments   please
                                // var allaseetDecDep = DecreasingDepreciation.CalculateDecDepForAllAssets(x.Fasset_ID);
                                countr++;
                            }
                        }
                    }
                }
            //}
            //else
            //{

            //    return 0;
            //}
            db.Dispose();
            return allaseeFixtDep;

        }
        //this  method  have  list  of child  of  the  choosed  type  of  lvl 4  and  it  show  lvl  3  under it 
        public static IEnumerable<SelectListItem> GetTypeCategory(string code)
        {
            var db = new AccountingContainer();
            IEnumerable<SelectListItem> provinces = new SelectList(db.ChartOfAccountings.Where(p => p.Account_Code.StartsWith(code) && p.Account_Level == 4), "Account_Code", "Account_Name");
            // = db.Provinces.Where(p => p.CountryId == countryId).ToList();
            return provinces;
        }
        /*
           * This is  For  FixedCategory making  list  of  Fixed Asset Types in the  ViewModel
           */
        //public static FixedAssetSecCategoryVM createVMForCreationCategory()
        //{
        //    var db = new AccountingContainer();
        //    var fixedassetsec = new FixedAssetSecCategoryVM();
        //    fixedassetsec.FixedAssetTypeLists = new SelectList(db.ChartOfAccountings, "Account_Id", "Account_Name");
        //    return fixedassetsec;
        //}
        //this  for  creating  neww  Category  For  assets while  it  have  taken 
        //its  data  From  Viewmodel and  Add  and  seve it in Database
        //public static FixedAssetSecCategory addFixedsecType(FixedAssetSecCategoryVM Fsectype)
        //{
        //    var db = new AccountingContainer();
        //    var fixedsectype = new FixedAssetSecCategory();

        //    try
        //    {
        //        if (Fsectype != null)
        //        {

        //            var fixedsecResult = db.FixedAssetTypes.Find(Fsectype.SelectedFixedAssetTypeIds);
        //            if (fixedsecResult != null)
        //            {
        //                fixedsectype.FixedAssetTypeId = fixedsecResult.FAsset_TypeID;
        //            }
        //            fixedsectype.FAssetCat_Name = Fsectype.FAssetCat_Names;
        //            fixedsectype.FixedAssCat_code = Fsectype.FixedAssCat_codes;
        //            db.FixedAssetSecCategories.Add(fixedsectype);
        //            db.SaveChanges();
        //            db.Dispose();

        //            return fixedsectype;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var msg = ex.Message;
        //        throw;
        //    }
        //    return null;
        //}
        ///*
        // * This is for  creating  new  Type of Assets  where  is  just  save  Name  and  code  in  Database
        // */
        //public static FixedAssetType addFixedType(FixedAssetTypeVM FasetVM)
        //{
        //    var db = new AccountingContainer();
        //    var fixedtype = new FixedAssetType();
        //    try
        //    {
        //        if (FasetVM != null)
        //        {
        //            fixedtype.FAsset_TypeName = FasetVM.FAsset_TypeNames;
        //            fixedtype.FixedAssetTyp_code = FasetVM.FixedAssetTyp_codes;
        //        }
        //        db.FixedAssetTypes.Add(fixedtype);
        //        db.SaveChanges();
        //        db.Dispose();
        //        return fixedtype;
        //    }
        //    catch (Exception ex)
        //    {
        //        var msg = ex.Message;
        //        throw;
        //    }

        //}
        public static FixedDisposeVM FillTheViewModelofDispose(int ID)
        {
            var db = new AccountingContainer();
            var editModel = GetAssetByID(ID);
            FixedDisposeVM FVM = new FixedDisposeVM();
            FVM.Fasset_IDs = editModel.Fasset_ID;
            FVM.FAsset_SellDates = DateTime.Now.ToString();
            if (editModel.BussinesUnitBU_Id == null)
            { }
            else
            { FVM.Bu_Id = Convert.ToInt32(editModel.BussinesUnitBU_Id); }
            var lista = new List<ChartOfAccounting> { };
            foreach (var k in db.ChartOfAccountings)
            {
                var parent = db.ChartOfAccountings.Where(a => a.ChartOfAccountingAccount_Id == k.Account_Id);
                var o = parent.FirstOrDefault();
                if (o == null)
                {
                    lista.Add(db.ChartOfAccountings.Find(k.Account_Id));

                }


            }
            FVM.sellingPrice = 0;
            FVM.MyAccountList = new SelectList(lista, "Account_Id", "Account_Name");
            List<object> newList2 = new List<object>();

         
            FVM.MyAccountBankList = new SelectList(db.ChartOfAccountings.Where(e => e.Account_Level == 5 && (e.ChartOfAccountingAccount_Id == 90 || e.ChartOfAccountingAccount_Id == 89)), "Account_Id", "Account_Name");


            FVM.currency_Name = editModel.Currency;
            var cur = db.Currencies.Where(e => e.Currency_Name == editModel.Currency).ToList().FirstOrDefault();
            if (editModel.Currency == null)
            { }
            else
            { FVM.Currency_Id = cur.Currency_Id; }
           
            var comp = db.Companies.FirstOrDefault();
            FVM.Company_name = comp.Comp_Name;
            FVM.Company_Id = editModel.CompanyComp_Id;
            FVM.FAsset_Descriptions = editModel.FAsset_Description;
            return FVM;

        }
        public static FixedDisposeVM FilltheViewofBankAccount(int ID)
        {
            var db = new AccountingContainer();
            var editModel = GetAssetByID(ID);
            FixedDisposeVM FVM = new FixedDisposeVM();
            FVM.Fasset_IDs = ID;
            FVM.MyAccountBankList = new SelectList(db.ChartOfAccountings.Where(e => e.Account_Level == 5 && (e.ChartOfAccountingAccount_Id == 90 || e.ChartOfAccountingAccount_Id == 89)), "Account_Id", "Account_Name");
            return FVM;
        }
        public static string DisposingAsset(FixedDisposeVM FDID , String UserId)
        {
            var db = new AccountingContainer();
            DateTime Datenow = DateTime.Now;
            FixedAsset MyAsset = db.FixedAssets.Find(FDID.Fasset_IDs);
            var glDeprastion = db.GLDetails.Where(a => a.GLMaster.GLType_Id == "اهلاك" && a.ChartOfAccounting.Account_Code.StartsWith("02") && a.Asset_Id == MyAsset.Fasset_ID).FirstOrDefault();
         

            if (glDeprastion == null)
            {
                //this  object  have   the  record  of  Subledger  where  its  code  equal  to  FixedAsset code  which  we  got  it  by the  ID
                // var subledger = db.SubLedgers.Where(c => c.SubLed_Code == MyAsset.FAsset_Code).ToList().LastOrDefault();
                FixedDepreciation fdep = new FixedDepreciation();
                //this  variable  calculate  the  diffrence  between  the  pirce  of  the  asset  and  the  Complex  of  it  to  add  it  in The  GeneralLedger
                double diffrOFComplexAndPrice = Convert.ToDouble(MyAsset.BookValue - MyAsset.Complex);
                // MyAsset.Complex = diffrOFComplexAndPrice;
                if (MyAsset.FAsset_Dispose.Value == false)
                {



                    var book = Convert.ToDecimal(MyAsset.BookValue.Value) - MyAsset.FixedAssetDetails.Select(g => g.DestructionValue).ToList().Sum();
                    //From here  we  start  make  the Gl  of  Dispose  1  master  and  2  detail  (from  and  to)
                    var GlMAster = new GLMaster();





                    GlMAster.GL_Date = Convert.ToDateTime(FDID.FAsset_SellDates);

                    var detail = new FixedAssetDetails();
                    detail.Date = Datenow;
                    if (MyAsset.FAsset_AgeDay == null)
                    {
                        MyAsset.FAsset_AgeDay = 0;
                    }
                    detail.DestructionDay = MyAsset.FAsset_AgeDay.Value - MyAsset.FixedAssetDetails.Select(a => a.DestructionDay).ToList().Sum();
                    if (detail.DestructionDay == 0)
                    {
                        detail.DestructionDay = 0;
                        detail.DestructionValue = 0;
                        MyAsset.PriceOfAssetDaily = 0;
                    }
                    detail.DestructionValue = detail.DestructionDay * MyAsset.PriceOfAssetDaily.Value;
                    detail.FixedAssetFasset_ID = MyAsset.Fasset_ID;
                    GlMAster.FixedAssetDetails.Add(detail);


                    GlMAster.GLType_Id = "بيع أصل";
                    GlMAster.GL_Desc = "بيع أصل";
                    GlMAster.Currency_Id = "الريال السعودى";
                    GlMAster.Company_Id = MyAsset.CompanyComp_Id;
                    GlMAster.GLCode = MyAsset.FAsset_Code;
                    GlMAster.GLState = TransactionState.Post;
                    GlMAster.Auto = true;
                    var GlDetail = new GLDetail();

                    GlDetail.Description_Detail = FDID.FAsset_Descriptions;
                    var chartexactcompl = db.ChartOfAccountings.Where(c => c.Account_Name == MyAsset.FAsset_Type).ToList().LastOrDefault();
                    GlDetail.Account_Id = chartexactcompl.Account_Id;
                    GlDetail.Account_Name = chartexactcompl.Account_Name;
                    GlDetail.SubLedger_Id = 0;
                    GlDetail.GLDetail_Credit = (decimal)book;//داءن
                    GlDetail.GLMasterGLMaster_Id = GlMAster.GLMaster_Id;
                    //مدين


                    GlDetail.Asset_Id = MyAsset.Fasset_ID;
                    GlMAster.GLDetails.Add(GlDetail);



                    var GlDetail2 = new GLDetail();
                    GlDetail2.SubLedger_Id = 0;
                    var ChartOfAccountbank = db.ChartOfAccountings.Where(e => e.Account_Id == FDID.SelectedBankAccountID).ToList().FirstOrDefault();
                    GlDetail2.Account_Id = ChartOfAccountbank.Account_Id;
                    GlDetail2.Account_Name = ChartOfAccountbank.Account_Name;
                    GlDetail2.Description_Detail = FDID.FAsset_Descriptions;
                    GlDetail2.GLDetail_Credit = 0;//داءن
                    GlDetail2.GLDetail_Debit = (decimal)FDID.sellingPrice;
                    GlMAster.GLDetails.Add(GlDetail2);
                    var GlDetail3 = new GLDetail();
                    GlDetail3.SubLedger_Id = 0;

                    var ChartOfAccountAssetRevenue = db.ChartOfAccountings.Find(196);
                    GlDetail3.Account_Id = ChartOfAccountAssetRevenue.Account_Id;
                    GlDetail3.Account_Name = ChartOfAccountAssetRevenue.Account_Name;
                    GlDetail3.Description_Detail = FDID.FAsset_Descriptions;
                    if (FDID.sellingPrice > book)
                    {
                        GlDetail3.GLDetail_Credit = FDID.sellingPrice.Value - book;
                    }
                    else
                    {
                        GlDetail3.GLDetail_Debit = book - FDID.sellingPrice.Value;
                    }
                    GlDetail3.SubLedger_Id = MyAsset.SubLedger_ID;

                    GlMAster.GLDetails.Add(GlDetail3);








                    db.GLMasters.Add(GlMAster);
                    GLMaster.GetBalanceForAccount(GlMAster);
                    GLMaster.GetBalanceForSubLedger(GlMAster);
                    db.SaveChanges();

                    MyAsset.FAsset_Dispose = true;
                    MyAsset.FAsset_SellDate = Datenow;
                    db.Entry(MyAsset).State = EntityState.Modified;

                    db.SaveChanges();
                    //the   object  is  to  make  the  Batch number  from Dispose
                    var transBatch = new TransactionBatch();
                    transBatch.Date = Datenow;
                    transBatch.OperationName = "إستبعاد";
                    transBatch.OperationId = GlMAster.GLMaster_Id;
                    transBatch.Description = "إستبعاد أصل";
                    transBatch.UserId = UserId;
                    transBatch.Operation_Code = MyAsset.FAsset_Code;
                    db.TransactionBatches.Add(transBatch);
                    db.SaveChanges();
                    db.Dispose();
                    return "تم بيع الاصل";









                }
                else
                {
                    return Resources.validate.Error;
                }
            }
            else
            {
                //this  object  have   the  record  of  Subledger  where  its  code  equal  to  FixedAsset code  which  we  got  it  by the  ID
                // var subledger = db.SubLedgers.Where(c => c.SubLed_Code == MyAsset.FAsset_Code).ToList().LastOrDefault();
                FixedDepreciation fdep = new FixedDepreciation();
                //this  variable  calculate  the  diffrence  between  the  pirce  of  the  asset  and  the  Complex  of  it  to  add  it  in The  GeneralLedger
                double diffrOFComplexAndPrice = Convert.ToDouble(MyAsset.BookValue - MyAsset.Complex);
                // MyAsset.Complex = diffrOFComplexAndPrice;
                if (MyAsset.FAsset_Dispose.Value == false)
                {



                    var book = Convert.ToDecimal(MyAsset.BookValue.Value) - MyAsset.FixedAssetDetails.Select(g => g.DestructionValue).ToList().Sum();
                    //From here  we  start  make  the Gl  of  Dispose  1  master  and  2  detail  (from  and  to)
                    var GlMAster = new GLMaster();





                    GlMAster.GL_Date = Convert.ToDateTime(FDID.FAsset_SellDates);

                    var detail = new FixedAssetDetails();
                    detail.Date = Datenow;
                    if (MyAsset.FAsset_AgeDay == null)
                    {
                        MyAsset.FAsset_AgeDay = 0;
                    }
                    detail.DestructionDay = MyAsset.FAsset_AgeDay.Value - MyAsset.FixedAssetDetails.Select(a => a.DestructionDay).ToList().Sum();
                    if (detail.DestructionDay == 0)
                    {
                        detail.DestructionDay = 0;
                        detail.DestructionValue = 0;
                        MyAsset.PriceOfAssetDaily = 0;
                    }
                    detail.DestructionValue = detail.DestructionDay * MyAsset.PriceOfAssetDaily.Value;
                    detail.FixedAssetFasset_ID = MyAsset.Fasset_ID;
                    GlMAster.FixedAssetDetails.Add(detail);


                    GlMAster.GLType_Id = "بيع أصل";
                    GlMAster.GL_Desc = "بيع أصل";
                    GlMAster.Currency_Id = "الريال السعودى";
                    GlMAster.Company_Id = MyAsset.CompanyComp_Id;
                    GlMAster.GLCode = MyAsset.FAsset_Code;
                    GlMAster.GLState = TransactionState.Post;
                    GlMAster.Auto = true;
                    var GlDetail = new GLDetail();

                    GlDetail.Description_Detail = FDID.FAsset_Descriptions;
                    var chartexactcompl = db.ChartOfAccountings.Where(c => c.Account_Name == MyAsset.FAsset_Type).ToList().LastOrDefault();
                    GlDetail.Account_Id = chartexactcompl.Account_Id;
                    GlDetail.Account_Name = chartexactcompl.Account_Name;
                    GlDetail.SubLedger_Id = 0;
                    GlDetail.GLDetail_Credit = (decimal)MyAsset.BookValue;//داءن
                    GlDetail.GLMasterGLMaster_Id = GlMAster.GLMaster_Id;
                    //مدين


                    GlDetail.Asset_Id = MyAsset.Fasset_ID;
                    GlMAster.GLDetails.Add(GlDetail);

                    var GlDetailLibritery = new GLDetail();

                    GlDetailLibritery.Description_Detail = FDID.FAsset_Descriptions;
                    var chartLibratery= db.ChartOfAccountings.Where(c => c.Account_Name.Contains( MyAsset.FAsset_Type)&&c.Account_Code.StartsWith("02020")).ToList().LastOrDefault();
                    GlDetailLibritery.Account_Id = chartLibratery.Account_Id;
                    GlDetailLibritery.Account_Name = chartLibratery.Account_Name;
                    GlDetailLibritery.SubLedger_Id = 0;
                    GlDetailLibritery.GLDetail_Debit = MyAsset.FixedAssetDetails.Select(g => g.DestructionValue).ToList().Sum();
                    GlDetailLibritery.GLMasterGLMaster_Id = GlMAster.GLMaster_Id;
                    //مدين


                    GlDetailLibritery.Asset_Id = MyAsset.Fasset_ID;
                    GlMAster.GLDetails.Add(GlDetailLibritery);







                    var GlDetail2 = new GLDetail();
                    GlDetail2.SubLedger_Id = 0;
                    var ChartOfAccountbank = db.ChartOfAccountings.Where(e => e.Account_Id == FDID.SelectedBankAccountID).ToList().FirstOrDefault();
                    GlDetail2.Account_Id = ChartOfAccountbank.Account_Id;
                    GlDetail2.Account_Name = ChartOfAccountbank.Account_Name;
                    GlDetail2.Description_Detail = FDID.FAsset_Descriptions;
                    GlDetail2.GLDetail_Credit = 0;//داءن
                    GlDetail2.GLDetail_Debit = (decimal)FDID.sellingPrice;
                    GlMAster.GLDetails.Add(GlDetail2);
                    var GlDetail3 = new GLDetail();
                    GlDetail3.SubLedger_Id = 0;

                    var ChartOfAccountAssetRevenue = db.ChartOfAccountings.Find(196);
                    GlDetail3.Account_Id = ChartOfAccountAssetRevenue.Account_Id;
                    GlDetail3.Account_Name = ChartOfAccountAssetRevenue.Account_Name;
                    GlDetail3.Description_Detail = FDID.FAsset_Descriptions;
                    if (FDID.sellingPrice > book)
                    {
                        GlDetail3.GLDetail_Credit = FDID.sellingPrice.Value - book;
                    }
                    else
                    {
                        GlDetail3.GLDetail_Debit = book - FDID.sellingPrice.Value;
                    }
                    GlDetail3.SubLedger_Id = MyAsset.SubLedger_ID;

                    GlMAster.GLDetails.Add(GlDetail3);








                    db.GLMasters.Add(GlMAster);
                    GLMaster.GetBalanceForAccount(GlMAster);
                    GLMaster.GetBalanceForSubLedger(GlMAster);
                    db.SaveChanges();

                    MyAsset.FAsset_Dispose = true;
                    MyAsset.FAsset_SellDate = Datenow;
                    db.Entry(MyAsset).State = EntityState.Modified;

                    db.SaveChanges();
                    //the   object  is  to  make  the  Batch number  from Dispose
                    var transBatch = new TransactionBatch();
                    transBatch.Date = Datenow;
                    transBatch.OperationName = "إستبعاد";
                    transBatch.OperationId = GlMAster.GLMaster_Id;
                    transBatch.Description = "إستبعاد أصل";
                    transBatch.UserId = UserId;
                    transBatch.Operation_Code = MyAsset.FAsset_Code;
                    db.TransactionBatches.Add(transBatch);
                    db.SaveChanges();
                    db.Dispose();
                    return "تم بيع الاصل";









                }
                else
                {
                    return Resources.validate.Error;
                }

            }
        }
        public static FixedAssetOpeningBalanceVM FillOpeingBalance()
        {
            var db = new AccountingContainer();
            var fixedasset = new FixedAssetOpeningBalanceVM();
            string startwithcodeTypeAsset = "0102";
            string startwithcodeCategoryAsset = "0102";
            fixedasset.MyCategoryList = new SelectList(db.ChartOfAccountings.Where(a => a.Account_Code.StartsWith(startwithcodeCategoryAsset) && a.Account_Level == 4), "Account_Code", "Account_Name");
            fixedasset.MyFAssetType = new SelectList(db.ChartOfAccountings.Where(a => a.Account_Code.StartsWith(startwithcodeTypeAsset) && a.Account_Level == 3), "Account_Code", "Account_Name");
            fixedasset.MyCurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");
           
            fixedasset.myEmployee = new SelectList(db.Employees, "Emp_Id", "Emp_Name");

            fixedasset.myCompaney = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            fixedasset.BranshList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
            fixedasset.DepListList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
            fixedasset.myBUnit = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            fixedasset.SubledgerList = new SelectList(db.SubLedgers.Where(a => a.SubLed_Type == "مركز تكلفة يدوى"), "SubLed_Id", "SubLed_Name");
            var lastCode = db.FixedAssets.OrderByDescending(c => c.Fasset_ID).FirstOrDefault();
            if (lastCode == null)
            {
                fixedasset.FAsset_Codes = "AS-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                fixedasset.FAsset_Codes = "AS-" + (Convert.ToInt32(lastCode.FAsset_Code.Substring(3, lastCode.FAsset_Code.Length - 3)) + 1).ToString("D3");
            }
            var lastCodeOpen = db.FixedAssets.OrderByDescending(c => c.Fasset_ID).Where(e=>e.OpenBalance_Code!=null).FirstOrDefault();
            if (lastCodeOpen == null)
            {
                fixedasset.OpenBalance_Codes = "OPA-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                fixedasset.OpenBalance_Codes = "OPA-" + (Convert.ToInt32(lastCodeOpen.OpenBalance_Code.Substring(4, lastCodeOpen.OpenBalance_Code.Length - 4)) + 1).ToString("D3");
            }
            return fixedasset;
        }
        public static FixedAsset AddOpeningBalanace(FixedAssetOpeningBalanceVM FVM , String UserId)
        {
            var db = new AccountingContainer();
            var fixedAsset = new FixedAsset();
            var Datenow = DateTime.Today;
            //this  object  holds  the  record  from  chart of  account  where  its  level  is  2  and  its  code  start  with 0102
            var chart = db.ChartOfAccountings.Where(c => c.Account_Level == 2 && c.Account_Code.StartsWith("0102")).ToList().FirstOrDefault();
            try
            {
                if (FVM != null)
                {
                    //this  object  holds  the  record  from  chart of  account  where  its  code  equals  type   that  is  choosed
                    var FATResult = db.ChartOfAccountings.Where(t => t.Account_Code == FVM.SelectedFixedAsset_TypeId).FirstOrDefault();
                    if (FATResult != null)
                    {
                        fixedAsset.FAsset_Type = FATResult.Account_Name;
                        //this variable contains  the  id  of  FixedaSSETS  THAT  will be  used  in  Last  variable +1 to make  the  code  of  the  Fixed Asset
                        int catCounter = (from count in db.FixedAssets
                                          select count.Fasset_ID).ToList().LastOrDefault();
                        int last = catCounter + 1;
                        var lastCode = db.FixedAssets.OrderByDescending(c => c.Fasset_ID).FirstOrDefault();
                        if (lastCode == null)
                        {
                            fixedAsset.FAsset_Code = "AS-001";
                        }
                        else
                        {
                            //using string substring method to get the number of the last inserted
                            fixedAsset.FAsset_Code = "AS-" + (Convert.ToInt32(lastCode.FAsset_Code.Substring(3, lastCode.FAsset_Code.Length - 3)) + 1).ToString("D3");
                        }
                        fixedAsset.FixedAssetTypeFAsset_TypeID = FATResult.Account_Id;

                    }
                    //this  object  holds  the  record  from  chart of  account  where  its  code  equals  category   that  is  choosed
                    var FASResult = db.ChartOfAccountings.Where(t => t.Account_Code == FVM.SelectedFixedAssetSecCategoryId).FirstOrDefault();
                    if (FASResult != null)
                    {
                        fixedAsset.FAssetCat_name = FASResult.Account_Name;
                        //this variable contains  the  id  of  FixedaSSETS  THAT  will be  used  in  Last  variable +1 to make  the  code  of  the  Fixed Asset
                        int catCounter = (from count in db.FixedAssets
                                          select count.Fasset_ID).ToList().LastOrDefault();
                        int last = catCounter + 1;
                        var lastCode = db.FixedAssets.OrderByDescending(c => c.Fasset_ID).FirstOrDefault();
                        if (lastCode == null)
                        {
                            fixedAsset.FAsset_Code = "AS-001";
                        }
                        else
                        {
                            //using string substring method to get the number of the last inserted
                            fixedAsset.FAsset_Code = "AS-" + (Convert.ToInt32(lastCode.FAsset_Code.Substring(3, lastCode.FAsset_Code.Length - 3)) + 1).ToString("D3");
                        }
                        fixedAsset.FixedAssetSecCategoryFAsset_CatID = FASResult.Account_Id;

                    }
                    var FAEResult = db.Employees.Find(FVM.selectedFAsset_Employees_ID);
                    if (FAEResult != null)
                    {
                        fixedAsset.FAsset_Employee_ID = FAEResult.Emp_Id;
                    }
                    var FACResult = db.Companies.Find(FVM.selectedCompany_Id);
                    if (FACResult != null)
                    {
                        fixedAsset.CompanyComp_Id = FVM.selectedCompany_Id; //FACResult.Comp_Id;
                    }
                    var FCur = db.Currencies.Find(FVM.SelectedCurrencyID);
                    if (FCur != null)
                    {
                        fixedAsset.Currency = FCur.Currency_Name; //FACResult.Comp_Id;
                    }
                    fixedAsset.FAsset_AgeMonth = FVM.FAsset_AgeMonths;
                    fixedAsset.FAsset_Description = FVM.FAsset_Descriptions;
                    fixedAsset.BussinesUnitBU_Id = FVM.FAsset_BUs;
                 
                    fixedAsset.FAsset_BuyDate = Convert.ToDateTime(FVM.FAsset_BuyDates).Date;
                    if (fixedAsset.FAsset_BuyDate.Value.Year > 1980)
                    {
                        fixedAsset.FAsset_BuyDate = Convert.ToDateTime(FVM.FAsset_BuyDates).Date;
                    }
                    else
                    {
                        fixedAsset.FAsset_BuyDate = null;
                    }
                    fixedAsset.FAsset_PurchasePrice = FVM.FAsset_PurchasePrices;
                    fixedAsset.BookValue = FVM.BookVlaue;
                    fixedAsset.Complex = FVM.Complex;
                    fixedAsset.FAsset_WorkingDate = Convert.ToDateTime(FVM.FAsset_WorkingDates).Date;
                    fixedAsset.JunckValue = FVM.JunckValue;
                    fixedAsset.OpenBalanceWorkDate = Convert.ToDateTime(FVM.OpenBalanceWorkDate).Date;
                    fixedAsset.Complex = FVM.Complex;
                    var subResult = db.SubLedgers.Find(FVM.SubLedger_ID);
                    if (subResult != null)
                    {
                        fixedAsset.SubLedger_ID = subResult.SubLed_Id;
                    }
                    else
                    {
                        if (FVM.FAsset_BUs != 0 && FVM.FAsset_BUs != null)
                        {
                            fixedAsset.SubLedger_ID = (int)FVM.FAsset_BUs;

                        }
                        else if ((FVM.FAsset_BUs == 0 || FVM.FAsset_BUs == null) && (FVM.DepartmentID != 0 && FVM.DepartmentID != null))
                        {
                            fixedAsset.SubLedger_ID = (int)FVM.DepartmentID;
                        }
                        else if ((FVM.FAsset_BUs == 0 || FVM.FAsset_BUs == null) && (FVM.DepartmentID == 0 || FVM.DepartmentID == null) && (FVM.BranchID != 0 && FVM.BranchID != null))
                        {
                            fixedAsset.SubLedger_ID = (int)FVM.BranchID;
                        }
                        else
                        {
                            var thissub = db.SubLedgers.Where(e => e.Subled_Level == 0).FirstOrDefault();
                            fixedAsset.SubLedger_ID = (int)thissub.SubLed_Id;
                        }
                    }




                    var lastCodeOpen = db.FixedAssets.OrderByDescending(c => c.Fasset_ID).Where(e => e.OpenBalance_Code != null).FirstOrDefault();
                    if (lastCodeOpen == null)
                    {
                        fixedAsset.OpenBalance_Code = "OPA-001";
                    }
                    else
                    {
                        //using string substring method to get the number of the last inserted
                        fixedAsset.OpenBalance_Code = "OPA-" + (Convert.ToInt32(lastCodeOpen.OpenBalance_Code.Substring(4, lastCodeOpen.OpenBalance_Code.Length - 4)) + 1).ToString("D3");
                    }
                   
                    if (FVM.Files.FirstOrDefault() != null)
                    {
                        foreach (var file in FVM.Files)
                        {
                            var filedetails = new FixedAssetFileDetail();
                            var data = new byte[file.ContentLength];
                            filedetails.FileName = file.FileName;
                            filedetails.File_Type = file.ContentType;
                            filedetails.ContentLength = file.ContentLength;
                            file.InputStream.Read(data, 0, file.ContentLength);
                            filedetails.ProImg = data;
                            filedetails.FixedAssetFasset_ID = fixedAsset.Fasset_ID;
                            fixedAsset.FixedAssetFileDetails.Add(filedetails);
                        }
                    }
                    if (fixedAsset.FAsset_AgeMonth > 0)
                    {
                        var endDate = fixedAsset.FAsset_WorkingDate.AddMonths(fixedAsset.FAsset_AgeMonth);
                        fixedAsset.FAsset_AgeDay = (endDate - fixedAsset.FAsset_WorkingDate).Days;
                        fixedAsset.PriceOfAssetDaily = Convert.ToDecimal( (fixedAsset.BookValue.Value-fixedAsset.JunckValue) / fixedAsset.FAsset_AgeDay.Value);
                    }
                    fixedAsset.CompanyComp_Id = db.Companies.FirstOrDefault().Comp_Id;
                    fixedAsset.OpenBalanceWorkDate = DateTime.Now;
                    if (fixedAsset.FAsset_PurchasePrice  ==null)
                    {
                        fixedAsset.FAsset_PurchasePrice = 0;
                    }
                    db.FixedAssets.Add(fixedAsset);
                    db.SaveChanges();
                    var thisDepTask = new DeprTaskStatus();
                    var thisAssetDepDate = new AssetDepDates();
                    thisAssetDepDate.Asset_ID = fixedAsset.Fasset_ID;
                    thisAssetDepDate.Date = fixedAsset.OpenBalanceWorkDate;
                    thisAssetDepDate.InstallMent_Amount =Convert.ToDecimal((double)fixedAsset.Complex);
                    if (fixedAsset.FAsset_PurchasePrice > 0)
                    {
                        thisAssetDepDate.Percentage = Math.Round((double)fixedAsset.Complex / (double)fixedAsset.FAsset_PurchasePrice * 100, 4);
                    }
                    db.AssetDepDates.Add(thisAssetDepDate);
                    
                    thisDepTask.Asset_id = fixedAsset.Fasset_ID;
                    thisDepTask.Count = 1;
                    thisDepTask.TaskDate = Convert.ToDateTime(fixedAsset.OpenBalanceWorkDate);
                    thisDepTask.TaskStatus = "اهلاك ثابت للاصل";
                    db.DeprTaskStatus.Add(thisDepTask);
                    db.SaveChanges();



                }


            var GlMAster = new GLMaster();
            var GlDetail1 = new GLDetail();
            var GlDetail = new GLDetail();
                var GlDetai2 = new GLDetail();
                var first = db.CompanyConfigrations.Find(1);
           
                GlMAster.GL_Date = Datenow;
                GlMAster.GLType_Id = "قيد افتتاحى";
                GlMAster.GL_Desc = "رصيد افتتاحي لاصل ثابت";
                GlMAster.Currency_Id = fixedAsset.Currency;
                GlMAster.Company_Id = fixedAsset.CompanyComp_Id;
                GlMAster.GLState = TransactionState.Post;
                    GlMAster.Auto = true;
                    GlMAster.GLCode = fixedAsset.OpenBalance_Code;
                //var chartexactcompl = db.ChartOfAccountings.Where(c => c.Account_Name.Contains(fixedAsset.FAsset_Type.Trim()) && c.Account_Code.StartsWith("020113")).ToList().LastOrDefault();
                //var ChartOfAccounDepr = db.ChartOfAccountings.Find(58);
                //GlDetail.Account_Id = chartexactcompl.Account_Id;
                //GlDetail.Account_Name = chartexactcompl.Account_Name;
                //if (fixedAsset.BussinesUnitBU_Id == null)
                //{ GlDetail.Bu_Id = 0; }
                //else
                //{ GlDetail.Bu_Id = (int)fixedAsset.BussinesUnitBU_Id; }

                //GlDetail.GLDetail_Credit =0 ;//داءن
                //GlDetail.GLMasterGLMaster_Id = GlMAster.GLMaster_Id;
                //GlDetail.GLDetail_Debit = (decimal)fixedAsset.BookValue;//مدين
                //GlDetail.SubLedger_Id = fixedAsset.SubLedger_ID;
                //GlDetail.Asset_Id = fixedAsset.Fasset_ID;
                //GlMAster.GLDetails.Add(GlDetail);
                //db.GLMasters.Add(GlMAster);
                // db.SaveChanges();



                // var ChartOfAccouninstalment = db.ChartOfAccountings.Find(57);
                var chartexactinstal = db.ChartOfAccountings.Where(t => t.Account_Code == FVM.SelectedFixedAsset_TypeId).FirstOrDefault();
                    GlDetail1.Account_Id = chartexactinstal.Account_Id;
                GlDetail1.Account_Name = chartexactinstal.Account_Name;
                if (fixedAsset.BussinesUnitBU_Id == null)
                { GlDetail1.Bu_Id = 0; }
                else
                { GlDetail1.Bu_Id = 0; }


                GlDetail1.GLDetail_Credit =0 ;//داءن
                GlDetail1.GLDetail_Debit = (decimal)fixedAsset.BookValue;//مدين
                GlDetail1.GLMasterGLMaster_Id = GlMAster.GLMaster_Id;
                GlDetail1.SubLedger_Id = 0;
                GlDetail1.Asset_Id = fixedAsset.Fasset_ID;
                GlMAster.GLDetails.Add(GlDetail1);
                db.GLMasters.Add(GlMAster);
                    //var charofWsite = db.ChartOfAccountings.Where(s => s.Account_Code == "0601" && s.Account_Level == 2 && s.ChartOfAccountingAccount_Id == 130).ToList().LastOrDefault();
                    var charofWsite = db.ChartOfAccountings.Where(e => e.Account_Code == "0601").ToList().FirstOrDefault();
                    GlDetai2.Account_Id = charofWsite.Account_Id;
                    GlDetai2.Account_Name = charofWsite.Account_Name;
                    if (fixedAsset.BussinesUnitBU_Id == null)
                    { GlDetai2.Bu_Id = 0; }
                    else
                    { GlDetai2.Bu_Id = 0; }


                    GlDetai2.GLDetail_Credit = (decimal)fixedAsset.BookValue;//داءن
                    GlDetai2.GLDetail_Debit = 0; //مدين
                    GlDetai2.GLMasterGLMaster_Id = GlMAster.GLMaster_Id;
                    GlDetai2.SubLedger_Id =0;
                    GlDetai2.Asset_Id = fixedAsset.Fasset_ID;

                    GlMAster.GLDetails.Add(GlDetai2);
                GlMAster.Company_Id = db.Companies.FirstOrDefault().Comp_Id;
                GlMAster.Currency_Id = db.Currencies.FirstOrDefault().Currency_Type;
                db.GLMasters.Add(GlMAster);

                    GLMaster.GetBalanceForAccount(GlMAster);
                    //db.SaveChanges();
                   
                    GLMaster.GetBalanceForSubLedger(GlMAster);

                    var transBatch = new TransactionBatch();

                transBatch.Date = Datenow;
                transBatch.OperationName = "إهلاك";
                transBatch.OperationId = GlMAster.GLMaster_Id;
                transBatch.Description = "رصيد افتتاحي للاصول";
                transBatch.UserId = UserId;
                transBatch.Operation_Code = fixedAsset.FAsset_Code;
                db.TransactionBatches.Add(transBatch);
                db.SaveChanges();
              
            
                db.Dispose();
                return fixedAsset;

          
            
        }
                catch (DbEntityValidationException ex)
            {
                var msg = ex.Message;
                throw;
            }

        }
        public static IEnumerable<SelectListItem> GetDepartments(int BranshID)
        {
            var db = new AccountingContainer();
            IEnumerable<SelectListItem> dep = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == BranshID && p.isActive != false && p.SubLed_Type == "فرع" || p.SubLed_Type == "قسم رئيسى لشركة"), "SubLed_Id", "SubLed_Name");
            // = db.Provinces.Where(p => p.CountryId == countryId).ToList();
            return dep;
        }
        public static IEnumerable<SelectListItem> GetBussinessUnits(int DepID)
        {
            var db = new AccountingContainer();
            IEnumerable<SelectListItem> dep = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == DepID && p.isActive != false && p.Subled_Level == 3&&p.SubLed_Type== "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            // = db.Provinces.Where(p => p.CountryId == countryId).ToList();
            return dep;
        }

        public static List<FixedAsset> GetAllFixedAssetForExport()
        {
            var db = new AccountingContainer();         
           
            //show  the  whole  list of  assets  with no  sort and  no  dispose value
            var LAssets = db.FixedAssets.ToList();
            return LAssets;
        
        }
        public static String Import(HttpPostedFileBase excelfile)
        {
            var data = getdataFillter(excelfile);
            List<FixedAsset> listFixedAsset = new List<FixedAsset>();
            if (data.Item2.Count() == 0)
            {
                foreach (var item in data.Item1)
                {
                    var tre = CreateAssetByFile(item);
                    listFixedAsset.Add(tre);

                }
               
                var listAssetempty = listFixedAsset.Where(a => a.BookValue != 0).ToList();

                if (listAssetempty.Count == 0)
                {

                }
                else
                {
                    Utilites.OpenBalanceFile.MakeOpenBallanceForGroupOFAsset(listFixedAsset);
                }
                return "تم حفظ الملف بنجاح";

            }
            string combindedString = string.Join(" ,", data.Item2.ToArray());
            return "يوجد اخطاء فى الاسطر التالية" + combindedString;


        }
        public static Tuple<List<FixedAssetForFileVM>, List<String>> getdataFillter(HttpPostedFileBase excelfile)
        {
            var FixedAssetList = new List<FixedAssetForFileVM> { };
            var messageList = new List<String> { };
            var db = new AccountingContainer();
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                return Tuple.Create(FixedAssetList, messageList);

            }

            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    //string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + excelfile.FileName);
                    var nameOfFile = "Asset" + DateTime.Now.ToBinary() + ".xlsx";
                    string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + nameOfFile);
                    //if (System.IO.File.Exists(path))
                    //{


                    //    File.Delete(path);




                    //}

                    excelfile.SaveAs(path);

                    Application application = new Application();
                    Workbook workbook = application.Workbooks.Open(path);
                    Worksheet worksheet = workbook.ActiveSheet;
                    Range range = worksheet.UsedRange;
                    for (int row = 3; row <= range.Rows.Count; row++)
                    {
                        FixedAssetForFileVM fixVM = new FixedAssetForFileVM();
                        // cust.Customer_Id = int.Parse(((Range)range.Cells[row, 1]).Text);
                        var FAsset_Descriptions = ((Range)range.Cells[row, 1]).Text;
                        var SelectedFixedAsset_TypeId = ((Range)range.Cells[row, 2]).Text;
                        var FAsset_PurchasePrices = ((Range)range.Cells[row, 3]).Text;
                        var Complex = ((Range)range.Cells[row, 4]).Text;
                        var BookVlaue = ((Range)range.Cells[row, 5]).Text;
                        var FAsset_WorkingDates = ((Range)range.Cells[row, 6]).Text;
                        var OpenBalanceWorkDate = ((Range)range.Cells[row, 7]).Text;
                        var FAsset_AgeMonths = ((Range)range.Cells[row, 8]).Text;
                        var JunckValue = ((Range)range.Cells[row, 9]).Text;


                        if (FAsset_Descriptions == "" || SelectedFixedAsset_TypeId == "" || FAsset_PurchasePrices == "" || Complex == "" || BookVlaue == "" || FAsset_WorkingDates == "" || OpenBalanceWorkDate == "" || FAsset_AgeMonths == "" || JunckValue == "")
                        {
                            messageList.Add(row.ToString());
                            continue;
                        }
                        var Chart = db.ChartOfAccountings.Where(a => a.ChartOfAccountingAccount_Id == 10).ToList();
                        try
                        {
                            fixVM.FAsset_Descriptions = FAsset_Descriptions;
                            fixVM.SelectedFixedAsset_TypeId = Convert.ToInt32(SelectedFixedAsset_TypeId);
                            var fixedType = Chart.Where(a => a.Account_Id == fixVM.SelectedFixedAsset_TypeId).FirstOrDefault().Account_Id;
                            fixVM.FAsset_PurchasePrices = Convert.ToDouble(FAsset_PurchasePrices);
                            fixVM.Complex = Convert.ToDouble(Complex);
                            fixVM.BookVlaue = Convert.ToDouble(BookVlaue);
                            fixVM.FAsset_WorkingDates = Convert.ToDateTime(FAsset_WorkingDates);
                            fixVM.OpenBalanceWorkDate = Convert.ToDateTime(OpenBalanceWorkDate);
                            fixVM.FAsset_AgeMonths = Convert.ToInt32(FAsset_AgeMonths);
                            fixVM.JunckValue = Convert.ToDouble(JunckValue);
                            if (fixVM.FAsset_PurchasePrices <= 0 || fixVM.Complex < 0 || fixVM.BookVlaue <= 0 || fixVM.FAsset_AgeMonths < 0 || fixVM.JunckValue <= 0)
                            {
                                messageList.Add(row.ToString());
                                continue;
                            }


                        }
                        catch
                        {

                            messageList.Add(row.ToString());
                            continue;


                        }


                        FixedAssetList.Add(fixVM);

                    }
                    workbook.Save();
                    workbook.Close(path);
                   // File.Delete(path);

                }
            }
            return Tuple.Create(FixedAssetList, messageList);
        }
        public static FixedAsset CreateAssetByFile(FixedAssetForFileVM AssetVM)
        {

            var db = new AccountingContainer();

            FixedAsset fixedAsset = new FixedAsset();
            DeprTaskStatus thisDepTask = new DeprTaskStatus();
            AssetDepDates thisAssetDepDate = new AssetDepDates();

            fixedAsset.FAsset_Description = AssetVM.FAsset_Descriptions;


            fixedAsset.FixedAssetTypeFAsset_TypeID = AssetVM.SelectedFixedAsset_TypeId;
            fixedAsset.FAsset_Type = db.ChartOfAccountings.Find(AssetVM.SelectedFixedAsset_TypeId).Account_Name;


            fixedAsset.FAsset_PurchasePrice = AssetVM.FAsset_PurchasePrices;


            fixedAsset.Complex = AssetVM.Complex;



            fixedAsset.BookValue = AssetVM.BookVlaue;


            fixedAsset.FAsset_WorkingDate = AssetVM.FAsset_WorkingDates;
            fixedAsset.OpenBalanceWorkDate = AssetVM.OpenBalanceWorkDate;
            fixedAsset.FAsset_AgeMonth = AssetVM.FAsset_AgeMonths;


            fixedAsset.JunckValue = AssetVM.JunckValue;


            var lastCodeOpen = db.FixedAssets.OrderByDescending(c => c.Fasset_ID).Where(e => e.OpenBalance_Code != null).FirstOrDefault();
            if (lastCodeOpen == null)
            {
                fixedAsset.OpenBalance_Code = "OPA-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                fixedAsset.OpenBalance_Code = "OPA-" + (Convert.ToInt32(lastCodeOpen.OpenBalance_Code.Substring(4, lastCodeOpen.OpenBalance_Code.Length - 4)) + 1).ToString("D3");
            }

            var lastCode = db.FixedAssets.OrderByDescending(c => c.Fasset_ID).FirstOrDefault();
            if (lastCode == null)
            {
                fixedAsset.FAsset_Code = "AS-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                fixedAsset.FAsset_Code = "AS-" + (Convert.ToInt32(lastCode.FAsset_Code.Substring(3, lastCode.FAsset_Code.Length - 3)) + 1).ToString("D3");
            }

            fixedAsset.CompanyComp_Id = db.Companies.FirstOrDefault().Comp_Id;
            fixedAsset.SubLedger_ID = db.SubLedgers.Where(a => a.SubLed_Name == db.Companies.FirstOrDefault().Comp_Name).FirstOrDefault().SubLed_Id;
            fixedAsset.FAsset_Dispose = false;
            fixedAsset.Currency = db.Currencies.FirstOrDefault().Currency_Name;

            db.FixedAssets.Add(fixedAsset);
            db.SaveChanges();


            thisAssetDepDate.Asset_ID = fixedAsset.Fasset_ID;
            thisAssetDepDate.Date = fixedAsset.OpenBalanceWorkDate;
            thisAssetDepDate.InstallMent_Amount = Convert.ToDecimal((double)fixedAsset.Complex);
            thisAssetDepDate.Percentage = Math.Round((double)fixedAsset.Complex / (double)fixedAsset.FAsset_PurchasePrice * 100, 4);
            db.AssetDepDates.Add(thisAssetDepDate);

            thisDepTask.Asset_id = fixedAsset.Fasset_ID;
            thisDepTask.Count = 1;
            thisDepTask.TaskDate = Convert.ToDateTime(fixedAsset.OpenBalanceWorkDate);
            thisDepTask.TaskStatus = "اهلاك ثابت للاصل";
            db.DeprTaskStatus.Add(thisDepTask);
            db.SaveChanges();
            return fixedAsset;

        }


        public static string StartDep(AssetDepDaysVM vm)
        {
            var db = new AccountingContainer();
            DateTime deprationDate = Convert.ToDateTime(vm.StartDate);
            bool isNew = true;
            var AssetDep = db.FixedAssets.Where(a => a.FAsset_Dispose != true && a.FAsset_AgeMonth > 0&&a.FAsset_WorkingDate< deprationDate &&a.FixedAssetTypeFAsset_TypeID!=11).ToList();
            var count = 0;
            var GLDeprestion = db.GLMasters.Where(a => a.GLType_Id == "اهلاك").FirstOrDefault();
            if (GLDeprestion == null)
            {
                isNew = true;
                CreateliabilitiesForAsset();

            }
            else
            {
                if (GLDeprestion.GLDetails.Where(a => a.ChartOfAccounting.Account_Code.StartsWith("01")).FirstOrDefault() == null)
                {
                    isNew = true;
                }
                else
                {
                    isNew = false;
                }
            }
            if (isNew == false)
            {
                foreach (var asset in AssetDep)
                {
                    var lastDateDeprat = new DateTime();
                    if (asset.FixedAssetDetails.Count() > 0)
                    {
                        lastDateDeprat = asset.FixedAssetDetails.OrderByDescending(a => a.Date).FirstOrDefault().Date;
                    }
                    else
                    {
                        lastDateDeprat = asset.FAsset_WorkingDate;
                    }
                    var xx = makeGLOfAsset(asset, deprationDate, lastDateDeprat);
                    if (xx == true)
                    {
                        count++;
                    }
                }
            }
            else
            {
                foreach (var asset in AssetDep)
                {
                    var lastDateDeprat = new DateTime();
                    if (asset.FixedAssetDetails.Count() > 0)
                    {
                        lastDateDeprat = asset.FixedAssetDetails.OrderByDescending(a => a.Date).FirstOrDefault().Date;
                    }
                    else
                    {
                        lastDateDeprat = asset.FAsset_WorkingDate;
                    }
                    var xx = makeGLOfAssetNew(asset, deprationDate, lastDateDeprat);
                    if (xx == true)
                    {
                        count++;
                    }
                }
            }

            return Resources.validate.Thedepreciationwassuccessful+" "+count;
        }

        public static bool CreateliabilitiesForAsset()
        {

            var db = new AccountingContainer();
            var AssetsLibility = db.ChartOfAccountings.Where(a => a.Account_Name.Contains("مخصص الاصول")).FirstOrDefault();
            if (AssetsLibility == null)
            {
                ChartOfAccountingVM accountVM = new ChartOfAccountingVM();

                accountVM.ChartOfAccountingAccount_Id = 66;
                accountVM.Account_Name = "مخصص الاصول";
                accountVM.Account_Curenncy = 1;
                accountVM.Account_Balance = 0;

                var Fathercode = ChartOfAccounting.CreateAccount(accountVM);
                var father = db.ChartOfAccountings.Where(a => a.Account_Code == Fathercode).FirstOrDefault();
                var AssetsType = db.ChartOfAccountings.Where(a => a.ChartOfAccounting1.Account_Id == 10 && a.Account_Code != "010201").ToList();
                foreach (var type in AssetsType)
                {
                    ChartOfAccountingVM accountVMchild = new ChartOfAccountingVM();

                    accountVMchild.ChartOfAccountingAccount_Id = father.Account_Id;
                    accountVMchild.Account_Name = "مخصص"+" "+ type.Account_Name;
                    accountVMchild.Account_Curenncy = 1;
                    accountVMchild.Account_Balance = 0;

                    var Child = ChartOfAccounting.CreateAccount(accountVMchild);
                }
            }
            return true;

        }

        public static bool makeGLOfAsset(FixedAsset asset, DateTime deprationDate,DateTime lastDateDeprat)
        {
            var db = new AccountingContainer();
            var GlMAster = new GLMaster();
            var GlDetail1 = new GLDetail();
            var GlDetail = new GLDetail();
           
         
                var detail = new FixedAssetDetails();
            detail.Date = deprationDate;
            detail.DestructionDay = (deprationDate- lastDateDeprat).Days;
            if (asset.PriceOfAssetDaily == null)
            {
                return false;
            }
            if (detail.DestructionDay < 0)
            {
                return false;
            }
            if (detail.DestructionDay+asset.FixedAssetDetails.Select(a=>a.DestructionDay).Sum()> asset.FAsset_AgeDay.Value)
            {
                detail.DestructionDay = asset.FAsset_AgeDay.Value - asset.FixedAssetDetails.Select(a => a.DestructionDay).Sum();
            }
            if (detail.DestructionDay == 0)
            {
                return false;
            }
            detail.DestructionValue = detail.DestructionDay * asset.PriceOfAssetDaily.Value;
            detail.FixedAssetFasset_ID = asset.Fasset_ID;
            GlMAster.FixedAssetDetails.Add(detail);
            GlMAster.GL_Date = deprationDate;
            GlMAster.GLType_Id = "اهلاك";
            GlMAster.GL_Desc = "اهلاك ثابت للاصل";
            GlMAster.Currency_Id = "ريال سعودي";
            GlMAster.Company_Id = db.Companies.FirstOrDefault().Comp_Id;
            GlMAster.GLState = TransactionState.Post;
            GlMAster.Auto = true;
            GlMAster.GLCode = asset.FAsset_Code;
            var ChartOfAccountExpense = db.ChartOfAccountings.Where(a => a.Account_Name == asset.FAsset_Type).FirstOrDefault();
            GlDetail.Account_Id = ChartOfAccountExpense.Account_Id;
            GlDetail.Account_Name = ChartOfAccountExpense.Account_Name;
            if (asset.BussinesUnitBU_Id == null)
            { GlDetail.Bu_Id = 0; }
            else
            { GlDetail.Bu_Id = (int)asset.BussinesUnitBU_Id; }
            GlDetail.GLDetail_Credit = detail.DestructionValue;//داءن
            GlDetail.GLMasterGLMaster_Id = GlMAster.GLMaster_Id;
            GlDetail.GLDetail_Debit = 0;//مدين
            GlDetail.SubLedger_Id = 0;
            GlDetail.Asset_Id = asset.Fasset_ID;
            GlDetail.Description_Detail = "اهلاك ثابت للاصل";
            GlMAster.GLDetails.Add(GlDetail);
            var account = db.ChartOfAccountings.Where(c => c.Account_Name.Contains(asset.FAsset_Type) && c.Account_Code.StartsWith("050204")).ToList();
            account.Reverse();
            var chartexactinstal = account.FirstOrDefault();
          
       


            GlDetail1.Account_Id = chartexactinstal.Account_Id;
            GlDetail1.Account_Name = chartexactinstal.Account_Name;
            if (asset.BussinesUnitBU_Id == null)
            { GlDetail.Bu_Id = 0; }
            else
            { GlDetail1.Bu_Id = (int)asset.BussinesUnitBU_Id; }
            GlDetail1.GLDetail_Credit = 0;//داءن
            GlDetail1.GLDetail_Debit = detail.DestructionValue;//مدين
            GlDetail1.GLMasterGLMaster_Id = GlMAster.GLMaster_Id;
            GlDetail1.SubLedger_Id = asset.SubLedger_ID;
            GlDetail1.Description_Detail = "اهلاك ثابت للاصل";
         
        
            GlMAster.GLDetails.Add(GlDetail1);
            db.GLMasters.Add(GlMAster);
            db.SaveChanges();
            return true;
        }
        public static bool makeGLOfAssetNew(FixedAsset asset, DateTime deprationDate, DateTime lastDateDeprat)
        {
            var db = new AccountingContainer();
            var GlMAster = new GLMaster();
            var GlDetail1 = new GLDetail();
            var GlDetail = new GLDetail();


            var detail = new FixedAssetDetails();
            detail.Date = deprationDate;
            detail.DestructionDay = (deprationDate - lastDateDeprat).Days;
            if (asset.PriceOfAssetDaily == null)
            {
                return false;
            }
            if (detail.DestructionDay < 0)
            {
                return false;
            }
            if (detail.DestructionDay + asset.FixedAssetDetails.Select(a => a.DestructionDay).Sum() > asset.FAsset_AgeDay.Value)
            {
                detail.DestructionDay = asset.FAsset_AgeDay.Value - asset.FixedAssetDetails.Select(a => a.DestructionDay).Sum();
            }
            if (detail.DestructionDay == 0)
            {
                return false;
            }
            detail.DestructionValue = detail.DestructionDay * asset.PriceOfAssetDaily.Value;
            detail.FixedAssetFasset_ID = asset.Fasset_ID;
            GlMAster.FixedAssetDetails.Add(detail);
            GlMAster.GL_Date = deprationDate;
            GlMAster.GLType_Id = "اهلاك";
            GlMAster.GL_Desc = "اهلاك ثابت للاصل";
            GlMAster.Currency_Id = "ريال سعودي";
            GlMAster.Company_Id = db.Companies.FirstOrDefault().Comp_Id;
            GlMAster.GLState = TransactionState.Post;
            GlMAster.Auto = true;
            GlMAster.GLCode = asset.FAsset_Code;
            var ChartOfAccountExpense = db.ChartOfAccountings.Where(c => c.Account_Name.Contains(asset.FAsset_Type) && c.Account_Code.StartsWith("0202")).FirstOrDefault();
            GlDetail.Account_Id = ChartOfAccountExpense.Account_Id;
            GlDetail.Account_Name = ChartOfAccountExpense.Account_Name;
            if (asset.BussinesUnitBU_Id == null)
            { GlDetail.Bu_Id = 0; }
            else
            { GlDetail.Bu_Id = (int)asset.BussinesUnitBU_Id; }
            GlDetail.GLDetail_Credit = detail.DestructionValue;//داءن
            GlDetail.GLMasterGLMaster_Id = GlMAster.GLMaster_Id;
            GlDetail.GLDetail_Debit = 0;//مدين
            GlDetail.SubLedger_Id = 0;
            GlDetail.Asset_Id = asset.Fasset_ID;
            GlDetail.Description_Detail = "اهلاك ثابت للاصل";
            GlMAster.GLDetails.Add(GlDetail);
            var asetType = asset.FixedAssetTypeFAsset_TypeID;
            var AssetTypeName = db.ChartOfAccountings.Find(asetType).Account_Name;
            var account = db.ChartOfAccountings.Where(c =>  c.Account_Code.StartsWith("050204")).ToList();
            account = db.ChartOfAccountings.Where(c => c.Account_Name.Contains(ChartOfAccountExpense.Account_Name)).ToList();
            var ll = asset.FAsset_Type;
             account = db.ChartOfAccountings.Where(c => c.Account_Name.Contains(AssetTypeName) && c.Account_Code.StartsWith("050204")).ToList();
            account.Reverse();
            var chartexactinstal = account.FirstOrDefault();




            GlDetail1.Account_Id = chartexactinstal.Account_Id;
            GlDetail1.Account_Name = chartexactinstal.Account_Name;
            if (asset.BussinesUnitBU_Id == null)
            { GlDetail.Bu_Id = 0; }
            else
            { GlDetail1.Bu_Id = (int)asset.BussinesUnitBU_Id; }
            GlDetail1.GLDetail_Credit = 0;//داءن
            GlDetail1.GLDetail_Debit = detail.DestructionValue;//مدين
            GlDetail1.GLMasterGLMaster_Id = GlMAster.GLMaster_Id;
            GlDetail1.SubLedger_Id = asset.SubLedger_ID;
            GlDetail1.Description_Detail = "اهلاك ثابت للاصل";


            GlMAster.GLDetails.Add(GlDetail1);
            db.GLMasters.Add(GlMAster);
            db.SaveChanges();
            return true;
        }


        public static void upgradeAllAsset()
        {
            
            var db = new AccountingContainer();
          
            var AssetList = db.FixedAssets.Where(a => a.FAsset_AgeDay == null && a.FAsset_AgeMonth > 0 && a.FixedAssetDetails.Count() == 0 && a.FixedAssetTypeFAsset_TypeID != 11).ToList();
            foreach (var asset in AssetList)
            {
                var endDate = asset.FAsset_WorkingDate.AddMonths(asset.FAsset_AgeMonth);

                asset.FAsset_AgeDay = (endDate - asset.FAsset_WorkingDate).Days;
                asset.PriceOfAssetDaily = Convert.ToDecimal((asset.FAsset_PurchasePrice.Value - asset.JunckValue) / asset.FAsset_AgeDay.Value);
                asset.BookValue = Convert.ToDouble(asset.FAsset_AgeDay * asset.PriceOfAssetDaily);
                db.Entry(asset).State = EntityState.Modified;

                var depration = db.DeprTaskStatus.Where(a => a.Asset_id == asset.Fasset_ID).OrderByDescending(a => a.TaskDate).FirstOrDefault();
                if (depration == null)
                {
                    db.SaveChanges();
                    continue;
                }
                var detail = new FixedAssetDetails();
                detail.Date = depration.TaskDate;
                detail.DestructionDay = (detail.Date - asset.FAsset_WorkingDate).Days;
                if (detail.DestructionDay < 0)
                {

                    continue;
                }
                if (detail.DestructionDay + asset.FixedAssetDetails.Select(a => a.DestructionDay).Sum() > asset.FAsset_AgeDay.Value)
                {
                    detail.DestructionDay = asset.FAsset_AgeDay.Value - asset.FixedAssetDetails.Select(a => a.DestructionDay).Sum();
                }
                if (detail.DestructionDay == 0)
                {
                    continue;
                }
                detail.DestructionValue = detail.DestructionDay * asset.PriceOfAssetDaily.Value;
                detail.FixedAssetFasset_ID = asset.Fasset_ID;
                detail.GLMasterGLMaster_Id = db.GLMasters.FirstOrDefault().GLMaster_Id;
                db.FixedAssetDetails.Add(detail);
                db.SaveChanges();
              

           
            }

         
            
        }
        public static bool updateAssetInfo()
        {
         
            using (var db = new AccountingContainer())
            {
                var purDetailList = db.PurchaseDetails.Where(a => a.Asset_Id > 0 && a.InvoiceMaster.Invoice_State == TransactionState.Post).ToList();
                var fixedAssetinfoList = db.FixedAssetInformations.Where(l=>l.IsActive!=null).ToList();
                foreach (var asset in fixedAssetinfoList)
                {
                    var a = fixedAssetinfoList.Where(d => d.Id == asset.Id).FirstOrDefault();
                    if (a!=null)
                    {
                        asset.IsActive = false;
                      
                        db.SaveChanges();
                    }
                }
                
            }
            return true;
        }
    }

}