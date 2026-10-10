using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Accountingsystem.Models
{
    public partial class BranchDepartment
    {
        public static List<BranchDepartment> GetBranchDepartment(int Branchid)
        //get all Branch Department in list by id
        {
            var db = new AccountingContainer();
            var branchdepartment = db.BranchDepartments.Where(cd => cd.BranchBranch_Id == Branchid && cd.IsActive != false).ToList();

            return branchdepartment;
        }
        public static List<BranchDepartment> GetAllNotActive(int id)
        {
            var db = new AccountingContainer();
            var Banks = db.BranchDepartments.Where(a => a.BranchBranch_Id == id && a.IsActive == false).ToList();
            return Banks;
        }

        public static BranchDepartment GetBranchDepartmentByID(int ID)
        //get all Branch Department in list by id
        {
            var db = new AccountingContainer();
            var branchdepartment = db.BranchDepartments.Find(ID);

            return branchdepartment;
        }
        public static BranchDeptVM CreateVMForbranchDept(int branchid)
        // creat vm to user to create branch department 
        //load page of ceration to create branch department
        {
            var db = new AccountingContainer();
            var branchdeptvm = new BranchDeptVM();
            Branch compmodel = new Branch();
            var bradata = db.Branches.Find(branchid);
            branchdeptvm.BranchBranch_Id = bradata.Branch_Id;

            return branchdeptvm;
        }


        public static BranchDepartment CreateBranchDept(BranchDeptVM branchdeptvm)
        //PBranchDeptVM we made it to add name ,Description , Phone & id of the Branch in the DB.
        {

            var db = new AccountingContainer();
            var BranchDepartmentsmodel = new BranchDepartment();
            var SubLedgera = db.SubLedgers.Where(a => a.Subled_Level == 1 && a.SubLed_Type == "فرع" && a.isActive != false).ToList();
            var bussines = db.DepartmentBussinesUnits.FirstOrDefault();
            var branch = db.Branches.ToList().Count();

            if (branch > 1)
            {
                foreach (var Branch in db.Branches)
                {
                    BranchDepartmentsmodel.BranchBranch_Id = Branch.Branch_Id;
                    BranchDepartmentsmodel.Dept_Name = branchdeptvm.Dept_Name;
                    BranchDepartmentsmodel.Dept_Description = branchdeptvm.Dept_Description;
                    BranchDepartmentsmodel.Dept_Phone = branchdeptvm.Dept_Phone;

                    MakeMultiDepartment(BranchDepartmentsmodel);

                }


                foreach (var sub in SubLedgera)
                {
                    var suVM = new SubLedgerVM();
                    suVM.SubLed_Name = branchdeptvm.Dept_Name;
                    suVM.ParentSubledger_Id = sub.SubLed_Id;
                    suVM.SubLed_Date = Convert.ToString(DateTime.Now);
                    suVM.SubLed_Type = "قسم لفرع";
                    SubLedger.createSubLedger(suVM);
                    //         MakeMultiDepartmentSubLedger(BranchDepartmentsmodel);
                }
            }
            else
            {
                BranchDepartmentsmodel.BranchBranch_Id = branchdeptvm.BranchBranch_Id;
                BranchDepartmentsmodel.Dept_Name = branchdeptvm.Dept_Name;
                BranchDepartmentsmodel.Dept_Description = branchdeptvm.Dept_Description;
                BranchDepartmentsmodel.Dept_Phone = branchdeptvm.Dept_Phone;
                MakeMultiDepartment(BranchDepartmentsmodel);

                var suVM = new SubLedgerVM();
                suVM.SubLed_Name = branchdeptvm.Dept_Name;
                var FatherName = db.Branches.Find(branchdeptvm.BranchBranch_Id).Branch_Name;
                suVM.ParentSubledger_Id = db.SubLedgers.Where(a => a.SubLed_Name == FatherName).FirstOrDefault().SubLed_Id;
                suVM.SubLed_Date = Convert.ToString(DateTime.Now);
                suVM.SubLed_Type = "قسم لفرع";
                SubLedger.createSubLedger(suVM);
                //      MakeMultiDepartmentSubLedger(BranchDepartmentsmodel);

            }

            db.Dispose();
            //if (bussines != null)
            //{
            //    makeAllBussinesi(BranchDepartmentsmodel);
            //}

            return BranchDepartmentsmodel;

        }


        public static BranchDeptVM EditCompDeptFill(int branchdeptid)
        //get for edit return the view of the screen fill of branchDep's data for user 
        {
            var db = new AccountingContainer();
            var branchvm = new BranchDeptVM();

            //  var cdmodel=new CompanyDepartments();

            var compdeptdata = db.BranchDepartments.Find(branchdeptid);

            branchvm.BranchBranch_Id = compdeptdata.BranchBranch_Id;
            branchvm.Dept_Name = compdeptdata.Dept_Name;

            branchvm.Dept_Phone = compdeptdata.Dept_Phone; //branchmodel.CompanyComp_Id;
            branchvm.Dept_Description = compdeptdata.Dept_Description;
            branchvm.Dept_Id = compdeptdata.Dept_Id;

            return branchvm;
        }


        public static BranchDepartment EditBranchPost(BranchDeptVM branchdeptvm)
        //we made that function to edit in the DB of the branch department after editing
        //we will go to the DB . we will get the Dep ID .if department does not exist it will return by null .
        //else it will get to branch view model & then it will be the oppsite of Adding branch . 
        {

            var db = new AccountingContainer();
            var branchdeptmodel = db.BranchDepartments.Find(branchdeptvm.Dept_Id);

            var Departs = db.BranchDepartments.Where(a => a.Dept_Name == branchdeptmodel.Dept_Name).ToList();
            var Subs = db.SubLedgers.Where(a => a.SubLed_Name == branchdeptmodel.Dept_Name && a.SubLed_Type == "قسم لفرع" && a.isActive != false).ToList();

            branchdeptmodel.BranchBranch_Id = branchdeptvm.BranchBranch_Id;
            branchdeptmodel.Dept_Id = branchdeptvm.Dept_Id;
            branchdeptmodel.Dept_Name = branchdeptvm.Dept_Name;
            branchdeptmodel.Dept_Phone = branchdeptvm.Dept_Phone;
            branchdeptmodel.Dept_Description = branchdeptvm.Dept_Description;

            db.Entry(branchdeptmodel).State = EntityState.Modified;

            foreach (var dept in Departs)
            {
                dept.Dept_Name = branchdeptvm.Dept_Name;
                db.Entry(dept).State = EntityState.Modified;
            }
            foreach (var sub in Subs)
            {
                sub.SubLed_Name = branchdeptvm.Dept_Name;
                db.Entry(sub).State = EntityState.Modified;
            }


            db.SaveChanges();
            db.Dispose();
            return branchdeptmodel;

        }
        /// <summary>
        /// add branch deparment
        /// </summary>
        /// <param name="Dep"></param>

        public static void MakeMultiDepartment(BranchDepartment Dep)
        {
            var db = new AccountingContainer();
            db.BranchDepartments.Add(Dep);
            db.SaveChanges();

        }




        /// <summary>
        /// put all bussinesunit and put it in new Department
        /// </summary>
        /// <param name="dep"></param>
        public static void makeAllBussines(BranchDepartment dep)
        {
            var db = new AccountingContainer();
            var BuDep = new DepartmentBussinesUnit();
            var SubVM = new SubLedgerVM();
            var BranchId = db.DepartmentBussinesUnits.FirstOrDefault().BranchDepartmentDept_Id;
            var bussines = db.DepartmentBussinesUnits.Where(a => a.BranchDepartmentDept_Id == BranchId).ToList();
            foreach (var bu in bussines)
            {
                BuDep.BranchDepartmentDept_Id = dep.Dept_Id;
                BuDep.BussinesUnit_Description = bu.BussinesUnit_Description;
                BuDep.BussinesUnit_Name = bu.BussinesUnit_Name;
                DepartmentBussinesUnit.makeMultiBussiness(BuDep);
                SubVM.SubLed_Name = BuDep.BussinesUnit_Name;
                var FatherName = db.BranchDepartments.Find(dep.Dept_Id).Dept_Name;
                SubVM.ParentSubledger_Id = db.SubLedgers.Where(a => a.SubLed_Name == FatherName).LastOrDefault().SubLed_Id;
                SubVM.SubLed_Date = Convert.ToString(DateTime.Now);
                SubVM.SubLed_Type = "نشاط لقسم";
                SubLedger.createSubLedger(SubVM);

            }
        }
        /// <summary>
        /// make all department for all branch in system
        /// </summary>
        /// <param name="dep"></param>
        public static void makeAllBussinesi(BranchDepartment dep)
        {
            var db = new AccountingContainer();
            var BuDep = new DepartmentBussinesUnit();
            var SubVM = new SubLedgerVM();
            var BranchId = db.DepartmentBussinesUnits.FirstOrDefault().BranchDepartmentDept_Id;
            var allBussines = db.DepartmentBussinesUnits.Where(a => a.BranchDepartmentDept_Id == BranchId).ToList();
            var Branch = db.BranchDepartments.Where(a => a.Dept_Name == dep.Dept_Name);
            foreach (var bran in Branch)
            {
                BuDep.BranchDepartmentDept_Id = bran.Dept_Id;
                foreach (var bus in allBussines)
                {
                    BuDep.BussinesUnit_Name = bus.BussinesUnit_Name;
                    BuDep.BussinesUnit_Description = bus.BussinesUnit_Description;
                    DepartmentBussinesUnit.makeMultiBussiness(BuDep);



                }



            }
            var Sub = db.SubLedgers.Where(a => a.SubLed_Name == dep.Dept_Name);
            var Father = db.SubLedgers.Where(a => a.SubLed_Type == "قسم لفرع").FirstOrDefault().SubLed_Id;
            var HisChild = db.SubLedgers.Where(a => a.SubLedgerSubLed_Id == Father);
            foreach (var parent in Sub)
            {
                SubVM.ParentSubledger_Id = parent.SubLed_Id;
                foreach (var Child in HisChild)
                {
                    SubVM.SubLed_Name = Child.SubLed_Name;

                    SubVM.SubLed_Date = Convert.ToString(DateTime.Now);
                    SubVM.SubLed_Type = "نشاط لقسم";
                    SubLedger.createSubLedger(SubVM);



                }
            }


        }
        public static string DeleteTransfer(int id,int ?level)
        {

            var db = new AccountingContainer();
            var deleteobject = db.BranchDepartments.Find(id);
            var branchsubledger = db.SubLedgers.Where(a => a.SubLed_Name == deleteobject.Dept_Name&&a.SubLedger1.SubLed_Name==deleteobject.Branch.Branch_Name && a.SubLed_Type == "قسم لفرع").FirstOrDefault();

            foreach (var branchDepartment in deleteobject.DepartmentBussinesUnits)
            {

                DepartmentBussinesUnit.DeleteTransfer(branchDepartment.BussinessUnit_Id,2);

            }
            if (level > 0)
            {

            }
            else
            {
                SubLedger.DeleteTransfer(branchsubledger.SubLed_Id,2);
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