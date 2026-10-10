using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class DepartmentBussinesUnit
    {
        /// <summary>
        /// that method get All Bussinesunit Depend on Department
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public static List<DepartmentBussinesUnit> GetAllBussiness(int deptId)

        {
            var db = new AccountingContainer();
            var business = db.DepartmentBussinesUnits.Where(b => b.BranchDepartmentDept_Id == deptId && b.IsActive != false).ToList();
       
            return business;
        }
        public static List<DepartmentBussinesUnit> GetAllNotActive(int id)
        {
            var db = new AccountingContainer();
            var Banks = db.DepartmentBussinesUnits.Where(a => a.BranchDepartmentDept_Id == id && a.IsActive == false).ToList();
            return Banks;
        }
        /// <summary>
        /// get that bussines unit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DepartmentBussinesUnit GetBussinesById(int id)

        {
            var db = new AccountingContainer();
            var business = db.DepartmentBussinesUnits.Find(id);
            return business;
        }
        public static DepartmentBussinesUnitVM CreateVM(int deptid)
        {
            var db = new AccountingContainer();
            var bussines = new DepartmentBussinesUnitVM();
            bussines.BranchDepartmentDept_Id = deptid;
           
            return bussines;
        }
        public static string addNewBusinessUnit(DepartmentBussinesUnitVM BussinesVm)
        {
            var db = new AccountingContainer();
            var bussines = new DepartmentBussinesUnit();
            var SubLedgera = db.SubLedgers.Where(a=>a.Subled_Level==2&&a.SubLed_Type== "قسم لفرع" && a.isActive != false).ToList();
            var DepartmentName = db.BranchDepartments.Find(BussinesVm.BranchDepartmentDept_Id).Dept_Name;
            var Departmenta= db.BranchDepartments.Where(a=>a.Dept_Name== DepartmentName).Count();
         
            if (Departmenta >1)
            {
                foreach (var Department in db.BranchDepartments.Where(a => a.Dept_Name == DepartmentName))
                {
                    bussines.BranchDepartmentDept_Id = Department.Dept_Id;
                    bussines.BussinesUnit_Name = BussinesVm.BussinesUnit_Name;
                    bussines.BussinesUnit_Description = BussinesVm.BussinesUnit_Description;

                    makeMultiBussiness(bussines);


                }
               
                foreach (var sub in SubLedgera.Where(a=>a.SubLed_Name== DepartmentName))
                {
                    var suVM = new SubLedgerVM();
                    suVM.SubLed_Name = BussinesVm.BussinesUnit_Name;
                    suVM.ParentSubledger_Id = sub.SubLed_Id;
                    suVM.SubLed_Date = Convert.ToString(DateTime.Now);
                    suVM.SubLed_Type = "نشاط لقسم";
                    SubLedger.createSubLedger(suVM);
                }
            }
            else
            {
                bussines.BranchDepartmentDept_Id = BussinesVm.BranchDepartmentDept_Id;
                bussines.BussinesUnit_Name = BussinesVm.BussinesUnit_Name;
                bussines.BussinesUnit_Description = BussinesVm.BussinesUnit_Description;
                db.DepartmentBussinesUnits.Add(bussines);
                db.SaveChanges();
                var suVM = new SubLedgerVM();
                suVM.SubLed_Name = BussinesVm.BussinesUnit_Name;
                var FatherName = db.BranchDepartments.Find(BussinesVm.BranchDepartmentDept_Id).Dept_Name;
                suVM.ParentSubledger_Id = db.SubLedgers.Where(a => a.SubLed_Name == FatherName).FirstOrDefault().SubLed_Id;
                suVM.SubLed_Date = Convert.ToString(DateTime.Now);
                suVM.SubLed_Type = "نشاط لقسم";
                SubLedger.createSubLedger(suVM);
            }
          

          
            db.Dispose();
            return "Successful ";

        }
        public static void makeMultiBussiness(DepartmentBussinesUnit dep)
        {
            var db = new AccountingContainer();
            db.DepartmentBussinesUnits.Add(dep);
            db.SaveChanges();


        }
        public static DepartmentBussinesUnitVM getEditBussines(int id)
        {
            var VM = new DepartmentBussinesUnitVM();
            var db = new AccountingContainer();
            var Account = db.DepartmentBussinesUnits.Find(id);
            VM.BranchDepartmentDept_Id = Account.BranchDepartmentDept_Id;
            VM.BussinesUnit_Description = Account.BussinesUnit_Description;
            VM.BussinesUnit_Name = Account.BussinesUnit_Name;
            VM.BussinessUnit_Id = Account.BussinessUnit_Id;
            VM.departList = new SelectList(db.BranchDepartments.Where(a=>a.IsActive != false), "Dept_Id", "Dept_Name");
            return VM;

        }
        public static void PostEditBussines(DepartmentBussinesUnitVM VM)
        {
            
            var db = new AccountingContainer();
            var buss = db.DepartmentBussinesUnits.Find(VM.BussinessUnit_Id);
            var Bussiness = db.DepartmentBussinesUnits.Where(a => a.BussinesUnit_Name == buss.BussinesUnit_Name&&a.BranchDepartment.Dept_Name==buss.BranchDepartment.Dept_Name).ToList();
            var Subs = db.SubLedgers.Where(a => a.SubLed_Name == buss.BussinesUnit_Name&&a.SubLed_Type== "نشاط لقسم" && a.isActive != false && a.SubLedger1.SubLed_Name== buss.BranchDepartment.Dept_Name).ToList();
            var k = Subs.FirstOrDefault().SubLedger1.SubLed_Name;
            buss.BranchDepartmentDept_Id = VM.BranchDepartmentDept_Id;
            buss.BussinesUnit_Description = VM.BussinesUnit_Description;
            buss.BussinesUnit_Name = VM.BussinesUnit_Name;
            db.Entry(buss).State = EntityState.Modified;
            foreach (var dept in Bussiness)
            {
                dept.BussinesUnit_Name = VM.BussinesUnit_Name;
                db.Entry(dept).State = EntityState.Modified;
            }
            foreach (var sub in Subs)
            {
                sub.SubLed_Name = VM.BussinesUnit_Name;
                db.Entry(sub).State = EntityState.Modified;
            }



            db.SaveChanges();
            db.Dispose();


        }
        public static string DeleteTransfer(int id,int? level)
        {

            var db = new AccountingContainer();
            var deleteobject = db.DepartmentBussinesUnits.Find(id);
            var branchsubledger = db.SubLedgers.Where(a => a.SubLed_Name == deleteobject.BussinesUnit_Name&&a.SubLedger1.SubLed_Name==deleteobject.BranchDepartment.Dept_Name&&a.SubLedger1.SubLedger1.SubLed_Name==deleteobject.BranchDepartment.Branch.Branch_Name && a.SubLed_Type == "نشاط لقسم").FirstOrDefault();

            if (level > 0) {

            }
            else
            {
                SubLedger.DeleteTransfer(branchsubledger.SubLed_Id,3);
            }
            
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