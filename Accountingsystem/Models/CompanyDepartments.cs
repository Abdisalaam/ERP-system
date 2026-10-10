using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace Accountingsystem.Models
{
    public partial class CompanyDepartments
    {


        public static List<CompanyDepartments> GetCompanyDepartment(int compid)
        //get all Company Departments in a list by id 
        {
            var db = new AccountingContainer();
            var companydepartment = db.CompanyDepartments.Where(cd => cd.CompanyComp_Id == compid&&cd.IsActive!=false).ToList();
         
            return companydepartment;
        }
        public static List<CompanyDepartments> GetAllNotActive(int compid)
        //get all Company Departments in a list by id 
        {
            var db = new AccountingContainer();
            var companydepartment = db.CompanyDepartments.Where(cd => cd.CompanyComp_Id == compid && cd.IsActive == false).ToList();
        
            return companydepartment;
        }

        public static CompanyDepartments GetCompanyDeptById(int id)
        // find companyby id
        {
            var db = new AccountingContainer();
            var companydept = db.CompanyDepartments.Find(id);
            return companydept;
        }


        public static List<CompanyDepartments> GetCompanyDeptByName(string compname)
        //get all Company Departments in a list by name
        {
            var db = new AccountingContainer();
            var compid = db.Companies.Where(c => c.Comp_Name == compname).FirstOrDefault();

            var companydept = db.CompanyDepartments.Where(cd => cd.CompanyComp_Id == compid.Comp_Id).ToList();

            return companydept;
        }



        public static CompDeptVM CreateVMForCompDept(int compid)
        // creat vm to user to create company department 
        //load page of ceration to create company department
        {
            var db = new AccountingContainer();
            var compdeptvm = new CompDeptVM();
            Company compmodel = new Company();
            var compdata = db.Companies.Find(compid);
            compdeptvm.Company_Name = compdata.Comp_Name;

            return compdeptvm;
        }


        public static CompanyDepartments CreateCompDept(CompDeptVM compdeptvm)
        //CompDeptVM  we made it to add code ,name , phone & Description of the CompanyDepartments in the DB.
        {

            var db = new AccountingContainer();
            var CompanyDepartmentsmodel = new CompanyDepartments();
            var SubVM = new SubLedgerVM();

            var compdata = db.Companies.Where(c => c.Comp_Name == compdeptvm.Company_Name);

            CompanyDepartmentsmodel.CompanyComp_Id = compdata.FirstOrDefault().Comp_Id;
            CompanyDepartmentsmodel.CompDept_Code = compdeptvm.CompDept_Code;
            CompanyDepartmentsmodel.CompDept_Name = compdeptvm.CompDept_Name;
            CompanyDepartmentsmodel.CompDept_Phone = compdeptvm.CompDept_Phone;
            CompanyDepartmentsmodel.CompDept_Description = compdeptvm.CompDept_Description;

            db.CompanyDepartments.Add(CompanyDepartmentsmodel);

           

            db.SaveChanges();
            SubVM.ParentSubledger_Id = db.SubLedgers.Where(a => a.SubLed_Name == CompanyDepartmentsmodel.Company.Comp_Name).FirstOrDefault().SubLed_Id;
            SubVM.SubLed_Name = compdeptvm.CompDept_Name;
            SubVM.SubLed_Date =Convert.ToString( DateTime.Now);
            SubVM.SubLed_Type = "قسم رئيسى لشركة";
            SubLedger.createSubLedger(SubVM);
            db.Dispose();
            return CompanyDepartmentsmodel;

        }




        public static CompDeptVM EditCompDeptFill(int compdeptid)
        //creat VM for user to edit in the Company Department  
        {
            var db = new AccountingContainer();
            var compdeptvm = new CompDeptVM();
            Company compmodel = new Company();

            //  var cdmodel=new CompanyDepartments();

            var compdeptdata = db.CompanyDepartments.Find(compdeptid);

            var compdata = db.Companies.Find(compdeptdata.CompanyComp_Id);



            compdeptvm.Id = compdeptid;

            compdeptvm.Company_Name = compdata.Comp_Name;

            compdeptvm.CompDept_Code = compdeptdata.CompDept_Code;
            compdeptvm.CompDept_Name = compdeptdata.CompDept_Name;

            compdeptvm.CompDept_Description = compdeptdata.CompDept_Description;
            compdeptvm.CompDept_Phone = compdeptdata.CompDept_Phone;


            return compdeptvm;
        }


        public static CompanyDepartments EditCompDeptPost(CompDeptVM compdeptvm)
        //we made that function to edit in the DB of the CompDept after editing
        //we will go to the DB . we will get the company by id .
        //else it will get to company view model & then it will be the oppsite of Adding CompDept .
        {

            var db = new AccountingContainer();
            var CompanyDepartmentsmodel = db.CompanyDepartments.Find(compdeptvm.Id);
            var sub = db.SubLedgers.Where(a => a.SubLed_Name == CompanyDepartmentsmodel.CompDept_Name&&a.SubLed_Type== "قسم رئيسى لشركة").FirstOrDefault();
            var compdata = db.Companies.FirstOrDefault();

            CompanyDepartmentsmodel.Id = compdeptvm.Id;

           
            CompanyDepartmentsmodel.CompDept_Name = compdeptvm.CompDept_Name;
            CompanyDepartmentsmodel.CompDept_Phone = compdeptvm.CompDept_Phone;
            CompanyDepartmentsmodel.CompDept_Description = compdeptvm.CompDept_Description;
       
            //entity state function to when you change the state to Modified 
            //all the properties of the entity will be marked as modified 
            //and all the property values will be sent to the database 
            //when SaveChanges is called
            sub.SubLed_Name= compdeptvm.CompDept_Name;
            db.Entry(sub).State = EntityState.Modified;
            db.Entry(CompanyDepartmentsmodel).State = EntityState.Modified;
            db.SaveChanges();
            db.Dispose();
            return CompanyDepartmentsmodel;

        }







        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public static CompDeptVM FillTheViewModel(int id)
        {
            var db = new AccountingContainer();

            var CompanyDeptModel = db.CompanyDepartments.Where(cd => cd.CompanyComp_Id == id).ToList();
            CompDeptVM companydeptvm = new CompDeptVM();
            return companydeptvm;

        }


        public static Company EditCompany(CompanyVM compvm)
        //u will edit company .
        //find first by compnay id .
        //if it doesnot exist 
        //fill the data & save it .
        {
            var db = new AccountingContainer();
            Company comp = db.Companies.Find(compvm.Comp_Id);

            
            if (comp != null)
            {
                comp.Comp_Name = compvm.Comp_Name;
                comp.Comp_Code = compvm.Comp_Code;
                comp.Comp_Address = compvm.Comp_Address;
                comp.Comp_Email = compvm.Comp_Email;
                comp.Comp_Fax = compvm.Comp_Fax;
                comp.Comp_Field = compvm.Comp_Field;
                comp.Comp_phone = compvm.Comp_phone;
                comp.Comp_PortalCode = compvm.Comp_PortalCode;

                var resultCountry = db.Countries.Find(compvm.SelectedCountryId);
                if (resultCountry != null)
                {
                    comp.Company_Country = resultCountry.Country_Name;
                }

                var resultProvinces = db.Provinces.Find(compvm.SelectedProvinceId);
                if (resultProvinces != null)
                {
                    comp.Comp_Province = resultProvinces.Province_Name;
                }

                var resultCities = db.Cities.Find(compvm.SelectedCityId);
                if (resultCities != null)
                {
                    comp.Comp_City = resultCities.City_Name;
                }

                db.Entry(comp).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                return comp;
            }


            return null;
        }


        public static string DeleteTransfer(int id)
        {

            var db = new AccountingContainer();
            var deleteobject = db.CompanyDepartments.Find(id);
            var branchsubledger = db.SubLedgers.Where(a => a.SubLed_Name == deleteobject.CompDept_Name && a.SubLed_Type == "قسم رئيسى لشركة").FirstOrDefault();

        
                SubLedger.DeleteTransfer(branchsubledger.SubLed_Id, 2);
            

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