using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class PaymentMethodVM
    {
        public int PaymentMethod_Id { get; set; }
        public int? Emp_Id { get; set; }
        public SelectList EmpList { get; set; }

        public string Payment_Type { get; set; }
        
        public Nullable<int> Bank_Account_Id { get; set; }
    
        public decimal Percentage { get; set; }
        public SelectList MyChartOfAccountList { get; set; }

        public Nullable<int> Expense_Id { get; set; }
        public SelectList ExpenseList { get; set; }
        public Decimal Operation_Cost { get; set; }
        public SelectList EmPloyeeMemberList { get; set; }
        public List<string> EmployeesMember { get; set; }
    }
}