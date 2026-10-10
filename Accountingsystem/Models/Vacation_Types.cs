using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Accountingsystem.ViewModel;
using Accountingsystem.Models;
using System.Data.Entity;

namespace Accountingsystem.Models
{
    public partial class Vacation_Types
    {/// <summary>
     /// get all type of vacation in db
     /// </summary>
     /// <returns>List<Vacation_Types> </returns>
        public static List<Vacation_Types> getAllVacationTypes()
        {
            var db = new AccountingContainer();
            var Vacations = db.Vacation_Types.ToList();
            return Vacations;
        }
        /// <summary>
        /// get that type by its id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>Vacation_Types</returns>
        public static Vacation_Types GetVacationTypeByID(int ID)
        {
            var db = new AccountingContainer();
            Vacation_Types vac = db.Vacation_Types.Find(ID);

            return vac;
        }
        public static Vacation_TypesVM FillForCreate()
        {
            var vm  = new Vacation_TypesVM();
            

            return vm;
        }
        /// <summary>
        ///  create anew type Vacation and save it db
        /// </summary>
        /// <param name="VVM"></param>
        /// <returns>Vacation_Types</returns>
        public static Vacation_Types AddNewVacationType(Vacation_TypesVM VVM)
        {
            var db = new AccountingContainer();
            var vacT = new Vacation_Types();
            try
            {
                vacT.Vacation_Type = VVM.Vacation_Type;
                vacT.penalityPercentage = VVM.penalityPercentage;
                vacT.PenalityPercentageAfter = VVM.PenalityPercentageAfter;
                vacT.PenalityDay = VVM.PenalityDay;
                vacT.PenalityDayAfter = VVM.PenalityDayAfter;
                vacT.TransferToNewYear = VVM.TransferToNewYear;
               
                var sal = new TypeOfDetailSalary();
                sal.SalaryDetail_Name = vacT.Vacation_Type;
                sal.ComeFromVacation = true;
             
                sal.Sal_Type = SalaryTypeValue.يقلل_من_اجمالى_الراتب;
                db.TypeOfDetailSalaries.Add(sal);
                
                db.Vacation_Types.Add(vacT);
                db.SaveChanges();
                db.Dispose();
                return vacT;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                throw;
            }
        }
        /// <summary>
        /// fill for Create anew Vaction
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>Vacation_TypesVM</returns>

        public static Vacation_TypesVM FillTheViewModel(int ID)
        {
            var editModel = GetVacationTypeByID(ID);
            Vacation_TypesVM VVM = new Vacation_TypesVM();
            VVM.Id = editModel.Id;
            VVM.TransferToNewYear = editModel.TransferToNewYear;
            VVM.Vacation_Type = editModel.Vacation_Type;
             VVM.penalityPercentage = editModel.penalityPercentage;
            VVM.PenalityPercentageAfter = editModel.PenalityPercentageAfter;
            VVM.PenalityDay = editModel.PenalityDay;
            VVM.PenalityDayAfter = editModel.PenalityDayAfter;
            VVM.Id = editModel.Id;
            return VVM;

        }
        /// <summary>
        /// save change of edit for vaction by its id
        /// </summary>
        /// <param name="VVM"></param>
        /// <returns>Vacation_Types</returns>
        public static Vacation_Types EditVacationType(Vacation_TypesVM VVM)
        {
            var db = new AccountingContainer();
            Vacation_Types vacT = db.Vacation_Types.Find(VVM.Id);
            if (vacT != null)
            {
                vacT.Vacation_Type = VVM.Vacation_Type;
                vacT.penalityPercentage = VVM.penalityPercentage;
                vacT.PenalityPercentageAfter = VVM.PenalityPercentageAfter;
                vacT.PenalityDay = VVM.PenalityDay;
                vacT.PenalityDayAfter = VVM.PenalityDayAfter;
                vacT.TransferToNewYear = VVM.TransferToNewYear;
                var sal = db.TypeOfDetailSalaries.Where(a => a.SalaryDetail_Name == vacT.Vacation_Type).FirstOrDefault();
                sal.SalaryDetail_Name = vacT.Vacation_Type;
               
                db.Entry(sal).State = EntityState.Modified;
              
               
                db.Entry(vacT).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                return vacT;
            }


            return null;
        }
       
        /// <summary>
        /// delete a type of vacation by its id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static int DeleteVacationType(int Id)
        {
            var db = new AccountingContainer();
            Vacation_Types myVacation = db.Vacation_Types.Find(Id);
            if (myVacation != null)
            {
                var Empvac = myVacation.EmpVacations;
                var vac = myVacation.Vacations;
                if (Empvac.Count() ==0&& vac.Count() == 0)
                {
                    db.Vacation_Types.Remove(myVacation);
                    db.SaveChanges();
                    db.Dispose();
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

            return 0;
        }
    }
}