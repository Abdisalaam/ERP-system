using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class Emp_EvaluationVM
    {
        public int Evaluation_Id { get; set; }
        public Nullable<int> Evaluation_Days { get; set; }
        public Nullable<decimal> Evaluation_Amount { get; set; }
        public string Evaluation_Date { get; set; }
        public string Reference { get; set; }
        public int Employee_Id { get; set; }
        public SelectList EmpList { get; set; }
        public Nullable<Evaluation_State> Evaluation_State { get; set; }
    }

}