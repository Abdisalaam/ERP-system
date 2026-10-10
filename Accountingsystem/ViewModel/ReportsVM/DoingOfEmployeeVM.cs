using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class DoingOfEmployeeVM
    {
        public string OperationName { get; set; }
        public string Operation_Code { get; set; }
        public int OperationId { get; set; }
        public string Start_Date { get; set; }
        public string End_Date { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public string Emp_Name { get; set; }
        public string Emp_Code { get; set; }
        public int Emp_Id { get; set; }
        public Company company { get; set; }
        public List<TransactionBatch> TransactionBatches { get; set; }
    }
}