using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class BussinesUnit
    {
        /// <summary>
        ///that method get all data that belonge to bussiness  unit from data base
        /// </summary>
        /// <returns></returns>
        public static List<BussinesUnit> getAllData()
        {
            var db = new AccountingContainer();
            var BU = db.BussinesUnits.ToList();
            return BU;


        }

        //   send view model to user to fill it
        public static BussinesUnitVM fillVMForCreate()
        {
            var db = new AccountingContainer();
            var bussinesVM = new BussinesUnitVM();
            int lastCode = (from s in db.BussinesUnits
                            select s.BU_Id).ToList().LastOrDefault();
            bussinesVM.BU_Code = (lastCode + 1).ToString();
            bussinesVM.compList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            return bussinesVM;


        }

        /// <summary>
        /// that method create a new bussines unit and save it in data base
        /// </summary>
        /// <param name="buvm"></param>
        /// <returns></returns>
        public static BussinesUnit createBussinesUnit(BussinesUnitVM buvm)
        {
            var db = new AccountingContainer();
            var bu = new BussinesUnit();

            bu.BU_Name = buvm.BU_Name;
            bu.BU_Description = buvm.BU_Description;
            bu.BU_Type = buvm.BU_Type;

            int lastCode = (from s in db.BussinesUnits
                            select s.BU_Id).ToList().LastOrDefault();
            bu.BU_Code = (lastCode + 1).ToString();

            var comResult = db.Companies.Find(buvm.CompanyComp_Id);
            if (buvm != null)
            {
                if (comResult != null)
                {

                    bu.CompanyComp_Id = comResult.Comp_Id;

                }


                db.BussinesUnits.Add(bu);
                db.SaveChanges();
                db.Dispose();
                return bu;
            }
            return null;
        }

        //يقوم بارجاع تفاصل النشاط المحدد
        public static BussinesUnit getDetail(int id)
        {
            var db = new AccountingContainer();
            var bu = db.BussinesUnits.Find(id);
            return bu;

        }
        //نقوم بارجاع النشاط المراد لعمل عليه التعديلات الازمة
        public static BussinesUnitVM fillDataForEditing(int id)
        {
            var bu = getDetail(id);
            var buvm = new BussinesUnitVM();
            buvm.BU_Id = bu.BU_Id;
            buvm.CompanyComp_Id = bu.CompanyComp_Id;
            buvm.BU_Code = bu.BU_Code;
            buvm.BU_Name = bu.BU_Name;
            buvm.BU_Description = bu.BU_Description;
            buvm.BU_Type = bu.BU_Type;
            return buvm;

        }
        //نقوم بحفظ النشاط المعدل 
        public static BussinesUnit editTheData(BussinesUnitVM buvm)
        {
            var db = new AccountingContainer();
            var bu = db.BussinesUnits.Find(buvm.BU_Id);
            if (bu != null)
            {
                bu.BU_Id = buvm.BU_Id;

                bu.BU_Code = buvm.BU_Code;
                bu.BU_Name = buvm.BU_Name;
                bu.BU_Description = buvm.BU_Description;
                bu.BU_Type = buvm.BU_Type;
                db.Entry(bu).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                return bu;
            }
            return null;
        }

    }
}