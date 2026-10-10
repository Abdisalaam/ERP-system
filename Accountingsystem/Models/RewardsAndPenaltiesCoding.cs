using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class RewardsAndPenaltiesCoding
    {
        public static List<RewardsAndPenaltiesCoding> getListOfRewardAndPenalties()
        {
            var db = new AccountingContainer();
            var List = db.RewardsAndPenaltiesCodings.Where(a=>a.Active==true).ToList();
            return List;


        }
        public static RewardsAndPenaltiesCoding getReward(int id)
        {
            var db = new AccountingContainer();
            var Reward = db.RewardsAndPenaltiesCodings.Find(id);
            return Reward;


        }
        public static RewardsAndPenaltiesCodingVM CreateVMForCreation()
        {
            var db = new AccountingContainer();
            var VM = new RewardsAndPenaltiesCodingVM();
            VM.TypeOfDetailSalaryList = new SelectList(db.TypeOfDetailSalaries, "Id", "SalaryDetail_Name");
            VM.RewardsAndPenaltiesCodingCategoryList = new SelectList(db.RewardsAndPenaltiesCodingCategories, "Id", "Name");

            return VM;
        }

        // use this method to  Create New Catagery and save it in DataBase.
        public static RewardsAndPenaltiesCoding CreateRewardAndPenalties(RewardsAndPenaltiesCodingVM vm)
        {
            var db = new AccountingContainer();
            var rewardsAndPenaltiesCoding = new RewardsAndPenaltiesCoding();
            rewardsAndPenaltiesCoding.Name = vm.Name;
            rewardsAndPenaltiesCoding.RewardsAndPenaltiesCodingCategoryId = vm.RewardsAndPenaltiesCodingCategoryId;
            rewardsAndPenaltiesCoding.RewordInDay = vm.RewordInDay;
            rewardsAndPenaltiesCoding.RewordinHour = vm.RewordinHour;
            rewardsAndPenaltiesCoding.DurationInDay = vm.DurationInDay;
            rewardsAndPenaltiesCoding.DurationInhour = vm.DurationInhour;
            rewardsAndPenaltiesCoding.DurationInMinute = vm.DurationInMinute;
            rewardsAndPenaltiesCoding.TypeOfDetailSalaryId = vm.TypeOfDetailSalaryId;
            rewardsAndPenaltiesCoding.Active = true;
            if (rewardsAndPenaltiesCoding.RewardsAndPenaltiesCodingCategoryId != 3)
            {
                rewardsAndPenaltiesCoding.TransformToMinute = rewardsAndPenaltiesCoding.DurationInhour * 60 + rewardsAndPenaltiesCoding.DurationInMinute;
            }
            if (rewardsAndPenaltiesCoding.RewardsAndPenaltiesCodingCategoryId > 3)
            {
                rewardsAndPenaltiesCoding.InCreaseSalary = true;
            }
            else
            {
                rewardsAndPenaltiesCoding.InCreaseSalary = false;
            }
            rewardsAndPenaltiesCoding.IsMax = false;
            var unlimte = db.RewardsAndPenaltiesCodings.Where(a => a.RewardsAndPenaltiesCodingCategoryId == vm.RewardsAndPenaltiesCodingCategoryId && a.IsMax == true).FirstOrDefault();
            var Absent= db.RewardsAndPenaltiesCodings.Where(a => a.RewardsAndPenaltiesCodingCategoryId == 3&& a.IsMax == true).FirstOrDefault();
            db.RewardsAndPenaltiesCodings.Add(rewardsAndPenaltiesCoding);
          
            if (unlimte == null&& rewardsAndPenaltiesCoding.RewardsAndPenaltiesCodingCategoryId != 3)
            {
                var rewardsAndPenaltiesCoding1 = new RewardsAndPenaltiesCoding();
                if (rewardsAndPenaltiesCoding.RewardsAndPenaltiesCodingCategoryId == 1)
                {
                    rewardsAndPenaltiesCoding1.Name = " حد اقصي تأخير";
                }
                else
                {
                    rewardsAndPenaltiesCoding1.Name = " حد اقصي انصراف مبكر";
                }
            
                rewardsAndPenaltiesCoding1.RewardsAndPenaltiesCodingCategoryId = vm.RewardsAndPenaltiesCodingCategoryId;
                rewardsAndPenaltiesCoding1.RewordInDay = 1;
                rewardsAndPenaltiesCoding1.RewordinHour =0;
                rewardsAndPenaltiesCoding1.DurationInDay = 0;
                rewardsAndPenaltiesCoding1.DurationInhour = 0;
                rewardsAndPenaltiesCoding1.DurationInMinute = 0;
                rewardsAndPenaltiesCoding1.TypeOfDetailSalaryId = vm.TypeOfDetailSalaryId;
                rewardsAndPenaltiesCoding1.Active = true;
                rewardsAndPenaltiesCoding1.IsMax = true;
                db.RewardsAndPenaltiesCodings.Add(rewardsAndPenaltiesCoding1);
            }
            if (Absent == null)
            {
                var rewardsAndPenaltiesCoding1 = new RewardsAndPenaltiesCoding();
                rewardsAndPenaltiesCoding1.Name = "غياب";
                rewardsAndPenaltiesCoding1.RewardsAndPenaltiesCodingCategoryId =3;
                rewardsAndPenaltiesCoding1.RewordInDay = 1;
                rewardsAndPenaltiesCoding1.RewordinHour = 0;
                rewardsAndPenaltiesCoding1.DurationInDay = 0;
                rewardsAndPenaltiesCoding1.DurationInhour = 0;
                rewardsAndPenaltiesCoding1.DurationInMinute = 0;
                rewardsAndPenaltiesCoding1.TypeOfDetailSalaryId = vm.TypeOfDetailSalaryId;
                rewardsAndPenaltiesCoding1.Active = true;
                rewardsAndPenaltiesCoding1.IsMax = true;
                db.RewardsAndPenaltiesCodings.Add(rewardsAndPenaltiesCoding1);
            }
            db.SaveChanges();
            db.Dispose();
            return rewardsAndPenaltiesCoding;
        }

        // use this method to load Details of Data for Category.
        public static RewardsAndPenaltiesCodingVM FillTheViewModel(int id)
        {
            var db = new AccountingContainer();
            var rewardsAndPenaltiesCoding = db.RewardsAndPenaltiesCodings.Find(id);
            var vm = new RewardsAndPenaltiesCodingVM();
            vm.Name = rewardsAndPenaltiesCoding.Name;
            vm.RewardsAndPenaltiesCodingCategoryId = rewardsAndPenaltiesCoding.RewardsAndPenaltiesCodingCategoryId;
            vm.RewordInDay = rewardsAndPenaltiesCoding.RewordInDay;
            vm.RewordinHour = rewardsAndPenaltiesCoding.RewordinHour;
            vm.DurationInDay = rewardsAndPenaltiesCoding.DurationInDay;
            vm.DurationInhour = rewardsAndPenaltiesCoding.DurationInhour;
            vm.DurationInMinute = rewardsAndPenaltiesCoding.DurationInMinute;
            vm.TypeOfDetailSalaryId = rewardsAndPenaltiesCoding.TypeOfDetailSalaryId;
            vm.rewardsAndPenaltiesCodingCategory = rewardsAndPenaltiesCoding.RewardsAndPenaltiesCodingCategory;
            vm.typeOfDetailSalary = rewardsAndPenaltiesCoding.TypeOfDetailSalary;
            vm.TypeOfDetailSalaryList = new SelectList(db.TypeOfDetailSalaries, "Id", "SalaryDetail_Name");
            vm.RewardsAndPenaltiesCodingCategoryList = new SelectList(db.RewardsAndPenaltiesCodingCategories, "Id", "Name");

            return vm;

        }

   
        public static RewardsAndPenaltiesCoding EditForRewardsAndPenaltiesCoding(RewardsAndPenaltiesCodingVM vm)
        {
            var db = new AccountingContainer();
            var rewardsAndPenaltiesCoding = db.RewardsAndPenaltiesCodings.Find(vm.Id);
            if (rewardsAndPenaltiesCoding != null)
            {


                rewardsAndPenaltiesCoding.Name = vm.Name;
            
                rewardsAndPenaltiesCoding.RewordInDay = vm.RewordInDay;
                rewardsAndPenaltiesCoding.RewordinHour = vm.RewordinHour;
                //rewardsAndPenaltiesCoding.DurationInDay = vm.DurationInDay;
                //rewardsAndPenaltiesCoding.DurationInhour = vm.DurationInhour;
                //rewardsAndPenaltiesCoding.DurationInMinute = vm.DurationInMinute;
                if (rewardsAndPenaltiesCoding.RewardsAndPenaltiesCodingCategoryId != 3)
                {
                    rewardsAndPenaltiesCoding.TransformToMinute = rewardsAndPenaltiesCoding.DurationInhour * 60 + rewardsAndPenaltiesCoding.DurationInMinute;
                }

                db.Entry(rewardsAndPenaltiesCoding).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();


                return rewardsAndPenaltiesCoding;
            }
            return null;
        }
        public static IEnumerable<SelectListItem> GetAllTypeOfSalaryDependToThat(int CategoryPenalityId)
        {
            var db = new AccountingContainer();
            var AllTypeDetailSalary = db.TypeOfDetailSalaries.ToList();
            IEnumerable<SelectListItem> Department;
            if (CategoryPenalityId > 3) {
                IEnumerable<SelectListItem> DepartmentIncreaseSal = new SelectList(AllTypeDetailSalary.Where(p => p.Sal_Type == SalaryTypeValue.يزيد_من_قيمة_اجمالى_الراتب), "Id", "SalaryDetail_Name");
                Department = DepartmentIncreaseSal;
            }
            else {
                IEnumerable<SelectListItem> DepartmentDecreaseSal = new SelectList(AllTypeDetailSalary.Where(p => p.Sal_Type == SalaryTypeValue.يقلل_من_اجمالى_الراتب), "Id", "SalaryDetail_Name");
                Department = DepartmentDecreaseSal;
            }
       
           
            // = db.Provinces.Where(p => p.CountryId == countryId).ToList();
            return Department;
        }
        public static int DeleteReward(int id)
        //after finding tax by id . you will delete it & than save it .
        {
            var db = new AccountingContainer();
            var reward = db.RewardsAndPenaltiesCodings.Find(id);
            if (reward != null)
            {
                if (reward.Active != false)
                {

                    reward.Active = false;
                

                }
                else
                {
                    reward.Active = true;
                 
                }
                db.Entry(reward).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                return 1;
            }
            return 0;
        }

    }
}