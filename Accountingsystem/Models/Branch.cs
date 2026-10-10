using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class Branch
    {
        /// <summary>
        /// check branch unique code
        /// </summary>
        /// <param name="Serial"></param>
        /// <returns></returns>
        public static bool containCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.Companies.ToList();
            foreach (var item in allinData)
            {
                if (item.Comp_Code == Serial || !Serial.StartsWith("BRA-"))
                {
                    return true;
                }
                else
                {

                }

            }
            return false;
        }

        public static List<Branch> GetBranches(int comp_id)
        //get all branches in a list by company id
        {
            var db = new AccountingContainer();
            var Branchies = db.Branches.Where(b => b.CompanyComp_Id == comp_id&&b.IsActive!=false).ToList();

            //  var Branchies = db.Branches.ToList();
            //db.Dispose();
            return Branchies;
        }
        public static List<Branch> GetAllNotActive(int id)
        {
            var db = new AccountingContainer();
            var Banks = db.Branches.Where(a => a.CompanyComp_Id == id && a.IsActive == false).ToList();
            return Banks;
        }

        public static Branch GetBranchById(int id)
        // u will find branch by id
        {
            var db = new AccountingContainer();
            var branch = db.Branches.Find(id);
            return branch;
        }

        public static List<Branch> GetBranchesByName(string compname)
        // u will get branch by name through compname 
        {
            var db = new AccountingContainer();
            // u will get the data base through "Filtering Operators - Where" it will get the branchname by filitering it from the DB .
            //& return it by first or default. & put it in a list .
            var compid = db.Companies.Where(c => c.Comp_Name == compname).FirstOrDefault();
            var companybranch = db.Branches.Where(cd => cd.CompanyComp_Id == compid.Comp_Id).ToList();
            return companybranch;
        }




        public static BranchVM CreateVMForCreation(int companyid)
        // for load page of ceration to create branch
        {

            var db = new AccountingContainer();
            var BranchVM = new BranchVM();

            Company compmodel = new Company();
            var compdata = db.Companies.Find(companyid);

            BranchVM.Company_ID = companyid;
            //BranchVM.MyCountryList = new SelectList(db.Countries, "Country_Id", "Country_Name");
            //BranchVM.MyProvinceList = new SelectList(db.Provinces, "Province_Id", "Province_Name");
            //BranchVM.MyCitiesList = new SelectList(db.Cities, "City_Id", "City_Name");
            //BranchVM.Branch_Code = "hamo";
            var lastCode = db.Branches.OrderByDescending(c => c.Branch_Id).FirstOrDefault();
            if (lastCode == null)
            {
                BranchVM.Branch_Code = "BRA-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                BranchVM.Branch_Code = "BRA-" + (Convert.ToInt32(lastCode.Branch_Code.Substring(4, lastCode.Branch_Code.Length - 4)) + 1).ToString("D3");
            }
            BranchVM.Company_Name = compdata.Comp_Name;
            return BranchVM;
        }

        public static Branch CreateBranch(BranchVM braVM)
        // method to create branch
        // if condition to check if it exist or not 
        // after adding the data i will be saved in DB . 
        {
            var db = new AccountingContainer();
            var Branch = new Branch();
            try
            {
                if (braVM != null)
                {
                    //var resultCountry = db.Countries.Find(braVM.SelectedCountryId);
                    //if (resultCountry != null)
                    //{
                    //    Branch.Branch_Country = resultCountry.Country_Name;
                    //}

                    //var resultProvinces = db.Provinces.Find(braVM.SelectedProvinceId);
                    //if (resultProvinces != null)
                    //{
                    //    Branch.Branch_Province = resultProvinces.Province_Name;
                    //}

                    //var resultCities = db.Cities.Find(braVM.SelectedCityId);
                    //if (resultCities != null)
                    //{
                    //    Branch.Branch_City = resultCities.City_Name;
                    //}

                    var data = db.Companies.FirstOrDefault();

                    Branch.CompanyComp_Id = data.Comp_Id;

                    Branch.Branch_Name = braVM.Branch_Name;
                    Branch.Branch_Address = braVM.Branch_Address;
                    Branch.Branch_Email = braVM.Branch_Email;
                    Branch.Branch_Fax = braVM.Branch_Fax;
                    Branch.Branch_Phone = braVM.Branch_Phone;
                    Branch.Branch_City = braVM.Branch_City;
                    Branch.Branch_Country = braVM.Branch_Country;
                    Branch.Branch_Province = braVM.Branch_Province;

                    var lastCode = db.Branches.OrderByDescending(c => c.Branch_Id).FirstOrDefault();
                    if (lastCode == null)
                    {
                        Branch.Branch_Code = "BRA-001";
                    }
                    else
                    {
                        //using string substring method to get the number of the last inserted
                        Branch.Branch_Code = "BRA-" + (Convert.ToInt32(lastCode.Branch_Code.Substring(4, lastCode.Branch_Code.Length - 4)) + 1).ToString("D3");
                    }
                    if (braVM.Photo.FirstOrDefault() != null)
                    {
                        foreach (var file in braVM.Photo)
                        {
                            var datam = new byte[file.ContentLength];
                            var length = file.ContentLength;
                            file.InputStream.Read(datam, 0, file.ContentLength);
                            Branch.BranchLogo = datam;

                        }
                    }
                    if (braVM.ImageHeader != null && braVM.ImageHeader.FirstOrDefault() != null)
                    {
                        foreach (var file in braVM.ImageHeader)
                        {
                            var datam = new byte[file.ContentLength];
                            var length = file.ContentLength;
                            file.InputStream.Read(datam, 0, file.ContentLength);
                            Branch.ImageHeader = datam;

                        }
                    }
                    if (braVM.ImageFooter != null && braVM.ImageFooter.FirstOrDefault() != null)
                    {
                        foreach (var file in braVM.ImageFooter)
                        {
                            var datam = new byte[file.ContentLength];
                            var length = file.ContentLength;
                            file.InputStream.Read(datam, 0, file.ContentLength);
                            Branch.ImageFooter = datam;

                        }
                    }
                    if (braVM.ImageWaterMark != null && braVM.ImageWaterMark.FirstOrDefault() != null)
                    {
                        foreach (var file in braVM.ImageWaterMark)
                        {
                            var datam = new byte[file.ContentLength];
                            var length = file.ContentLength;
                            file.InputStream.Read(datam, 0, file.ContentLength);
                            Branch.ImageWaterMark = datam;

                        }
                    }
                    db.Branches.Add(Branch);
                    db.SaveChanges();
                    makeSubLeger(Branch);
                    makeChildForThatBranch(Branch);
                    db.Dispose();
                    return Branch;
                }
            }
            //made catch in case if error has happended it will catch 
            catch (Exception ex)
            {
                var msg = ex.Message;
                var result = ex.Data;
                var tt = result;
                throw;
            }

            return Branch;
        }



        public static BranchVM EditCompDeptFill(int branchid)
        //get for edit return the view of the screen fill of branch's data for user 
        {
            var db = new AccountingContainer();
            var branchvm = new BranchVM();
            var branchmodel = new Branch();

            //  var cdmodel=new CompanyDepartments();

            var compdeptdata = db.Branches.Find(branchid);

            var compdata = db.Companies.Find(compdeptdata.CompanyComp_Id);

            branchvm.Branch_Id = compdeptdata.Branch_Id;
            branchvm.Branch_Name = compdeptdata.Branch_Name;

            branchvm.Company_Name = compdata.Comp_Name; //branchmodel.CompanyComp_Id;
            branchvm.Company_ID = (int)compdeptdata.CompanyComp_Id;
            branchvm.Branch_Address = compdeptdata.Branch_Address;
            branchvm.Branch_Email = compdeptdata.Branch_Email;
            branchvm.Branch_Fax = compdeptdata.Branch_Fax;
            branchvm.Branch_Phone = compdeptdata.Branch_Phone;
            branchvm.Branch_Code = compdeptdata.Branch_Code;

            branchvm.Branch_City = compdeptdata.Branch_City;
            branchvm.Branch_Country = compdeptdata.Branch_Country;
            branchvm.Branch_Province = compdeptdata.Branch_Province;

            //var countryName = compdeptdata.Branch_Country;
            //var provinceName = compdeptdata.Branch_Province;
            //var cityName = compdeptdata.Branch_City;

            //var Country = db.Countries.Where(c => c.Country_Name == countryName);
            //if (Country.Any())
            //{
            //    branchvm.SelectedCountryId = Country.First().Country_Id;
            //}
            //var province = db.Provinces.Where(c => c.Province_Name == provinceName);
            //if (province.Any())
            //{
            //    branchvm.SelectedProvinceId = province.First().Province_Id;
            //}
            //var city = db.Cities.Where(c => c.City_Name == cityName);
            //if (city.Any())
            //{
            //    branchvm.SelectedCityId = city.First().City_Id;
            //}


            //branchvm.MyCountryList = new SelectList(db.Countries, "Country_Id", "Country_Name");
            //branchvm.MyProvinceList = new SelectList(db.Provinces, "Province_Id", "Province_Name");
            //branchvm.MyCitiesList = new SelectList(db.Cities, "City_Id", "City_Name");



            return branchvm;
        }


        public static Branch EditBranchPost(BranchVM branchvm)
        //we made that function to edit in the DB of the branch after editing
        //we will go to the DB . we will get the company name .if branch does not exist it will return by null .
        //else it will get to branch view model & then it will be the oppsite of Adding branch . 
        {

            var db = new AccountingContainer();
            var Branchmodel = db.Branches.Find(branchvm.Branch_Id);

            var compdata = db.Companies.Where(c => c.Comp_Name == branchvm.Company_Name);


            var sub = db.SubLedgers.Where(a => a.SubLed_Name == Branchmodel.Branch_Name && a.SubLed_Type == "فرع").FirstOrDefault();

            //var resultCountry = db.Countries.Find(branchvm.SelectedCountryId);
            //if (resultCountry != null)
            //{
            //    Branchmodel.Branch_Country = resultCountry.Country_Name;
            //}

            //var resultProvinces = db.Provinces.Find(branchvm.SelectedProvinceId);
            //if (resultProvinces != null)
            //{
            //    Branchmodel.Branch_Province = resultProvinces.Province_Name;
            //}

            //var resultCities = db.Cities.Find(branchvm.SelectedCityId);
            //if (resultCities != null)
            //{
            //    Branchmodel.Branch_City = resultCities.City_Name;
            //}
            //entity state function to when you change the state to Modified 
            //all the properties of the entity will be marked as modified 
            //and all the property values will be sent to the database 
            //when SaveChanges is called
            Branchmodel.Branch_Id = branchvm.Branch_Id;
            Branchmodel.CompanyComp_Id = Branchmodel.CompanyComp_Id;
            Branchmodel.Branch_Name = branchvm.Branch_Name;
            Branchmodel.Branch_Address = branchvm.Branch_Address;
            Branchmodel.Branch_Email = branchvm.Branch_Email;
            Branchmodel.Branch_Fax = branchvm.Branch_Fax;
            Branchmodel.Branch_Phone = branchvm.Branch_Phone;
          


            Branchmodel.Branch_Country = branchvm.Branch_Country;
            Branchmodel.Branch_City = branchvm.Branch_City;
            Branchmodel.Branch_Province = branchvm.Branch_Province;

            sub.SubLed_Name = branchvm.Branch_Name;
            if (branchvm.Photo.FirstOrDefault() != null)
            {
                foreach (var file in branchvm.Photo)
                {
                    var data = new byte[file.ContentLength];
                    var length= file.ContentLength;
                    file.InputStream.Read(data, 0, file.ContentLength);
                    Branchmodel.BranchLogo = data;

                }
            }
            if (branchvm.ImageHeader != null && branchvm.ImageHeader.FirstOrDefault() != null)
            {
                foreach (var file in branchvm.ImageHeader)
                {
                    var datam = new byte[file.ContentLength];
                    var length = file.ContentLength;
                    file.InputStream.Read(datam, 0, file.ContentLength);
                    Branchmodel.ImageHeader = datam;

                }
            }
            if (branchvm.ImageFooter != null && branchvm.ImageFooter.FirstOrDefault() != null)
            {
                foreach (var file in branchvm.ImageFooter)
                {
                    var datam = new byte[file.ContentLength];
                    var length = file.ContentLength;
                    file.InputStream.Read(datam, 0, file.ContentLength);
                    Branchmodel.ImageFooter = datam;

                }
            }
            if (branchvm.ImageWaterMark != null && branchvm.ImageWaterMark.FirstOrDefault() != null)
            {
                foreach (var file in branchvm.ImageWaterMark)
                {
                    var datam = new byte[file.ContentLength];
                    var length = file.ContentLength;
                    file.InputStream.Read(datam, 0, file.ContentLength);
                    Branchmodel.ImageWaterMark = datam;

                }
            }
            db.Entry(Branchmodel).State = EntityState.Modified;
            db.Entry(sub).State = EntityState.Modified;
            db.SaveChanges();
            db.Dispose();
            return Branchmodel;

        }
        /// <summary>
        /// make subledger for branch
        /// </summary>
        /// <param name="branch"></param>
        public static void makeSubLeger(Branch branch)
        {
            var SubLedVM = new SubLedgerVM();
            var db = new AccountingContainer();
            var SubLedgera = db.SubLedgers.Where(a => a.SubLed_Type == "فرع").FirstOrDefault();
            SubLedVM.SubLed_Name = branch.Branch_Name;
            SubLedVM.SubLed_Type = "فرع";
            SubLedVM.SubLed_Date = Convert.ToString(DateTime.Now);

            if (SubLedgera != null)
            {
                SubLedVM.ParentSubledger_Id = SubLedgera.SubLedgerSubLed_Id.Value;
                var subBranch = SubLedger.createSubLedger(SubLedVM);
                var Department = SubLedgera.SubLedgers.Where(a => a.Subled_Level == 2 && a.SubLed_Type == "قسم لفرع");
                if (Department.FirstOrDefault() != null)
                {

                    foreach (var dep in Department)
                    {

                        var SubDepart = new SubLedgerVM();
                        SubDepart.SubLed_Name = dep.SubLed_Name;
                        SubDepart.SubLed_Type = "قسم لفرع";
                        SubDepart.SubLed_Date = Convert.ToString(DateTime.Now);
                        SubDepart.ParentSubledger_Id = subBranch.SubLed_Id;
                        var subDepart = SubLedger.createSubLedger(SubDepart);
                        var BussinessUnit = dep.SubLedgers.Where(a => a.Subled_Level == 3 && a.SubLed_Type == "نشاط لقسم");
                        if (BussinessUnit != null)
                        {
                            foreach (var bussines in BussinessUnit)
                            {
                                var Subbusines = new SubLedgerVM();
                                Subbusines.SubLed_Name = bussines.SubLed_Name;
                                Subbusines.SubLed_Type = "نشاط لقسم";
                                Subbusines.SubLed_Date = Convert.ToString(DateTime.Now);
                                Subbusines.ParentSubledger_Id = subDepart.SubLed_Id;
                                var subBussines = SubLedger.createSubLedger(Subbusines);



                            }




                        }


                    }


                }




            }
            else
            {
                SubLedVM.ParentSubledger_Id = db.SubLedgers.Where(a => a.Subled_Level == 0).FirstOrDefault().SubLed_Id;
                var subBranch = SubLedger.createSubLedger(SubLedVM);
            }

            db.Dispose();

        }
        /// <summary>
        /// take all child of branch and make that child for new branch 
        /// </summary>
        /// <param name="branch"></param>
        public static void makeChildForThatBranch(Branch branch)
        {
            var db = new AccountingContainer();
            var departmentmodel = new BranchDepartment();
            var Bussiness = new DepartmentBussinesUnit();
            var departmentOtherBranch = db.Branches.FirstOrDefault().Branch_Id;
            var department = db.BranchDepartments.Where(a => a.BranchBranch_Id == departmentOtherBranch).ToList();
            if (department.FirstOrDefault() != null)
            {
                foreach (var deb in department)
                {

                    departmentmodel.BranchBranch_Id = branch.Branch_Id;
                    departmentmodel.Dept_Name = deb.Dept_Name;
                    departmentmodel.Dept_Description = deb.Dept_Description;
                    BranchDepartment.MakeMultiDepartment(departmentmodel);
                    var bussines = db.DepartmentBussinesUnits.Where(a => a.BranchDepartmentDept_Id == deb.Dept_Id).ToList();
                    if (bussines.FirstOrDefault() != null)
                    {
                        foreach (var buss in bussines)
                        {
                            Bussiness.BussinesUnit_Name = buss.BussinesUnit_Name;
                            Bussiness.BussinesUnit_Description = buss.BussinesUnit_Description;
                            Bussiness.BranchDepartmentDept_Id = departmentmodel.Dept_Id;

                            DepartmentBussinesUnit.makeMultiBussiness(Bussiness);
                        }
                    }
                }



            }




        }


        public static Branch getAddressForInvoice(int? branchId)
        {
            var db = new AccountingContainer();
            var branch = new Branch();
            var company = db.Companies.FirstOrDefault();
            branch.Branch_Email = company.Comp_Email;
            branch.Branch_Phone = company.Comp_phone;
            branch.Branch_Address = company.Comp_Address;
           
            if (company.CompanyPhotoDetails.FirstOrDefault() != null)
            {
                branch.BranchLogo = company.CompanyPhotoDetails.FirstOrDefault().ProImg;

            }
            if (company.ImageHeader!= null)
            {
                branch.ImageHeader = company.ImageHeader;

            }
            if (company.ImageFooter!= null)
            {
                branch.ImageFooter = company.ImageFooter;

            }

            if (branchId > 0)
            {
                var branchsub= db.SubLedgers.Find(branchId.Value);
                var RealBranch = db.Branches.Where(a => a.Branch_Name == branchsub.SubLed_Name).FirstOrDefault();
                if (RealBranch != null) {
                    if (RealBranch.Branch_Email != null && RealBranch.Branch_Email != "")
                    {

                        branch.Branch_Email = RealBranch.Branch_Email;
                     }
                    if (RealBranch.Branch_Phone != null && RealBranch.Branch_Phone != "")
                    {

                        branch.Branch_Phone = RealBranch.Branch_Phone;
                    }
                    if (RealBranch.Branch_Address != null && RealBranch.Branch_Address != "")
                    {

                        branch.Branch_Address = RealBranch.Branch_Address;
                    }
                    if (RealBranch.BranchLogo != null )
                    {

                        branch.BranchLogo = RealBranch.BranchLogo;
                    }
                    if (RealBranch.ImageHeader != null)
                    {

                        branch.ImageHeader = RealBranch.ImageHeader;
                    }
                    if (RealBranch.ImageFooter != null)
                    {

                        branch.ImageFooter = RealBranch.ImageFooter;
                    }
                    branch.Branch_Name = branchsub.SubLed_Name;
                }


            }


            return branch;



        }
        public static string DeleteTransfer(int id)
        {

            var db = new AccountingContainer();
            var deleteobject = db.Branches.Find(id);
            var branchsubledger = db.SubLedgers.Where(a => a.SubLed_Name == deleteobject.Branch_Name && a.SubLed_Type == "فرع").FirstOrDefault();
     
            foreach (var branchDepartment in deleteobject.BranchDepartments)
            {
               
                BranchDepartment.DeleteTransfer(branchDepartment.Dept_Id,1);

            }
            SubLedger.DeleteTransfer(branchsubledger.SubLed_Id,1);
            if (deleteobject.IsActive != false)
            {
                deleteobject.IsActive = false;
            

            }
            else
            {
                deleteobject.IsActive = true;
          
            }
            db.Entry(deleteobject).State = EntityState.Modified;
            db.SaveChanges();

            return Resources.ProductDetails.TransferSucces;

        }




    }




}
