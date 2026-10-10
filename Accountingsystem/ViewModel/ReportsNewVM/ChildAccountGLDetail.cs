using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class ChildAccountGLDetail
    {
        public DateTime date { get; set; }
        public DateTime? dueDate { get; set; }
        public string DocumentNumber { get; set; }
        public string Number { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }
}