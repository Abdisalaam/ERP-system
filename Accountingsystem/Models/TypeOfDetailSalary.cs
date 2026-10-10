using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Accountingsystem.Models
{
    public partial class TypeOfDetailSalary
    {
        /// <summary>
        /// get all type of detail of sallary
        /// </summary>
        /// <returns>List<TypeOfDetailSalary></returns>
        public static List<TypeOfDetailSalary> getDetailOfTypeSalary() {

            var db = new AccountingContainer();
            var list = db.TypeOfDetailSalaries.ToList();
            return list;




        }
        /// <summary>
        /// get that type of salary by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>TypeOfDetailSalary</returns>
        public static TypeOfDetailSalary getDetailOfTypeSalaryById(int id)
        {

            var db = new AccountingContainer();
            var list = db.TypeOfDetailSalaries.Find(id);
            return list;


        }
        /// <summary>
        /// fill for Crate anew datail of salary
        /// </summary>
        /// <returns>TypeOfDetailSalaryVM</returns>
        public static TypeOfDetailSalaryVM fillForCreate() {

            var VM = new TypeOfDetailSalaryVM();
            VM.Sal_Type = SalaryTypeValue.يزيد_من_قيمة_اجمالى_الراتب;
            return VM;



        }
        /// <summary>
        /// post the detail of salary and save it in data base
        /// </summary>
        /// <param name="VM"></param>
        /// <returns>bool</returns>
        public static int createTypeOfDetailSalary(TypeOfDetailSalaryVM VM)
        {
           
                var db = new AccountingContainer();
                var Type = new TypeOfDetailSalary();
                Type.SalaryDetail_Name = VM.SalaryDetail_Name;
                Type.Sal_Type = VM.Sal_Type;
                db.TypeOfDetailSalaries.Add(Type);
                db.SaveChanges();
                db.Dispose();
            
            return Type.Id;
        }
        /// <summary>
        /// fill for edit by id of detial of salary
        /// </summary>
        /// <param name="id"></param>
        /// <returns>TypeOfDetailSalaryVM</returns>
        public static TypeOfDetailSalaryVM fillForEditing(int id)
        {
            var db = new AccountingContainer();
            var TypeVM = new TypeOfDetailSalaryVM();
            var Type = getDetailOfTypeSalaryById(id);
            TypeVM.Id = Type.Id;
            TypeVM.SalaryDetail_Name = Type.SalaryDetail_Name;
            TypeVM.Sal_Type = Type.Sal_Type;
            return TypeVM;


        }
        /// <summary>
        /// save change in db after editing
        /// </summary>
        /// <param name="VM"></param>
        /// <returns>bool</returns>
        public static int EditingTypeOfSalary(TypeOfDetailSalaryVM VM)
        {
           
                var db = new AccountingContainer();
                var Type = getDetailOfTypeSalaryById(VM.Id);
                Type.SalaryDetail_Name = VM.SalaryDetail_Name;
                Type.Sal_Type = VM.Sal_Type;
                db.Entry(Type).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
              
          

            return Type.Id;
        }
        /// <summary>
        /// delete the detail by using its id
        /// </summary>
        /// <param name="id"></param>
        public static void delete(int id)
        {
            var db = new AccountingContainer();
            var Type = db.TypeOfDetailSalaries.Find(id);
            db.TypeOfDetailSalaries.Remove(Type);
            db.SaveChanges();
            db.Dispose();

        }


    }
}