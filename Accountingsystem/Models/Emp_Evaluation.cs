using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Accountingsystem.ViewModel;
using System.Web.Mvc;
using System.Data.Entity;

namespace Accountingsystem.Models
{
    public partial class Emp_Evaluation
    {
        public static List<Emp_Evaluation> getAllData()
        {
            var db = new AccountingContainer();
            var empEvaluations = db.Emp_Evaluation.ToList();
            return empEvaluations;
        }

        public static Emp_Evaluation GetEvaluationById(int id)
        {
            var db = new AccountingContainer();
            var empEvalu = db.Emp_Evaluation.Find(id);
            return empEvalu;
        }

        public static Emp_EvaluationVM CreationForEvaluation()
        {
            var db = new AccountingContainer();
            var empEvalVM = new Emp_EvaluationVM();
            empEvalVM.Evaluation_Date = DateTime.Now.ToString("dd-MM-yyyy   HH:mm:ss");
            empEvalVM.EmpList = new SelectList(db.Employees, "Emp_Id", "Emp_Name");
            return empEvalVM;
        }

        public static string CreateEmpEvaluation(Emp_EvaluationVM empEvaluaVM)
        {
            var db = new AccountingContainer();
            var empEval = new Emp_Evaluation();
            
            var resultEmployee = db.Employees.Find(empEvaluaVM.Employee_Id);
            if (resultEmployee != null)
            {
                empEval.Employee_Id = resultEmployee.Emp_Id;
            }

            
            empEval.Evaluation_Date = empEvaluaVM.Evaluation_Date;
            empEval.Evaluation_Days = empEvaluaVM.Evaluation_Days;           
            empEval.Reference = empEvaluaVM.Reference;
            empEval.Evaluation_State = empEvaluaVM.Evaluation_State;

            var today = DateTime.Today.ToString("dd");
            var MyYear = DateTime.Now.Year.ToString();
            var Mymonth = DateTime.Now.Month.ToString();
            //now i  have  last  day  on  the  month  i am  in
            var MyLastDay = new DateTime(Convert.ToInt32(MyYear), Convert.ToInt32(Mymonth), DateTime.DaysInMonth(Convert.ToInt32(MyYear), Convert.ToInt32(Mymonth))).ToString("dd");
            double amountForOneDay =Convert.ToDouble((decimal)(resultEmployee.Total_Salary) / Convert.ToInt32(MyLastDay));

            empEval.Evaluation_Amount = Convert.ToDecimal(amountForOneDay) * empEvaluaVM.Evaluation_Days;
            if (empEval.Evaluation_State == Models.Evaluation_State.مكافئة)
            {
                resultEmployee.Total_Salary += empEval.Evaluation_Amount;
            }
            else
            {
                resultEmployee.Total_Salary -= empEval.Evaluation_Amount;
            }

            db.Entry(resultEmployee).State = EntityState.Modified;

            db.Emp_Evaluation.Add(empEval);
            db.SaveChanges();
            db.Dispose();
            return "Evaluation is added";
        }

        public static Emp_EvaluationVM FillDataForEditing(int id)
        {
            var db = new AccountingContainer();
            var empEvalVM = new Emp_EvaluationVM();
            var empEvaluation = db.Emp_Evaluation.Find(id);
            var empId = empEvaluation.Employee_Id;


            empEvalVM.Evaluation_Id = empEvaluation.Evaluation_Id;
            empEvalVM.Evaluation_Date = empEvaluation.Evaluation_Date;
            empEvalVM.Evaluation_Days = empEvaluation.Evaluation_Days;
            //empEvalVM.Evaluation_Amount = empEvaluation.Evaluation_Amount;
            empEvalVM.Reference = empEvaluation.Reference;

            var employee = db.Emp_Evaluation.Where(e => e.Employee_Id == empId);
            if (employee.Any())
            {
                empEvalVM.Employee_Id = employee.FirstOrDefault().Employee_Id;
            }

            empEvalVM.Evaluation_State = empEvaluation.Evaluation_State;
         
            empEvalVM.EmpList = new SelectList(db.Employees, "Emp_Id", "Emp_Name");

            return empEvalVM;

        }

        public static string EditEmployeeEvaluation(Emp_EvaluationVM empEvaluaVM)
        {
            var db = new AccountingContainer();
            var empEval = db.Emp_Evaluation.Find(empEvaluaVM.Evaluation_Id);
            if (empEval != null)
            {
                var resultEmployee = db.Employees.Find(empEvaluaVM.Employee_Id);
                if (resultEmployee != null)
                {
                    empEval.Employee_Id = resultEmployee.Emp_Id;
                }

                empEval.Evaluation_Date = empEvaluaVM.Evaluation_Date;
                empEval.Evaluation_Days = empEvaluaVM.Evaluation_Days;
               
                empEval.Reference = empEvaluaVM.Reference;
                empEval.Evaluation_State = empEvaluaVM.Evaluation_State;

                var today = DateTime.Today.ToString("dd");
                var MyYear = DateTime.Now.Year.ToString();
                var Mymonth = DateTime.Now.Month.ToString();
                //now i  have  last  day  on  the  month  i am  in
                var MyLastDay = new DateTime(Convert.ToInt32(MyYear), Convert.ToInt32(Mymonth), DateTime.DaysInMonth(Convert.ToInt32(MyYear), Convert.ToInt32(Mymonth))).ToString("dd");
                double amountForOneDay = Convert.ToDouble((decimal)(resultEmployee.Total_Salary) / Convert.ToInt32(MyLastDay));

                empEval.Evaluation_Amount = Convert.ToDecimal(amountForOneDay) * empEvaluaVM.Evaluation_Days;
                if (empEval.Evaluation_State == Models.Evaluation_State.مكافئة)
                {
                    resultEmployee.Total_Salary += empEval.Evaluation_Amount;
                }
                else
                {
                    resultEmployee.Total_Salary -= empEval.Evaluation_Amount;
                }
                db.Entry(resultEmployee).State = EntityState.Modified;
                db.Entry(empEval).State = EntityState.Modified;

                db.SaveChanges();
                db.Dispose();
                return "Success";
            }
            return "Fail";

        }

        //public static decimal CalculateAmountOfEvaluation()
        //{
        //    var db = new AccountingContainer();

        //}
    }
}