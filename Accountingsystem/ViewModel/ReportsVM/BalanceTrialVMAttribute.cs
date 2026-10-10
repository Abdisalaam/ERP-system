using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class BalanceTrialVMAttribute
    {
        public ChartOfAccounting Account { get; set; }
        public String Account_Name { get; set; }
        public String Account_Code { get; set; }
        public int Account_Id { get; set; }
        public decimal startBallance { get; set; }
        public decimal startBallanceCredit{ get; set; }
        public decimal startBallanceDebit { get; set; }
        public decimal MovmentBallanceCredit { get; set; }
        public decimal MovmentBallanceDebit{ get; set; }
        public decimal pureCredit { get; set; }
        public decimal puredepit { get; set; }
        public decimal EndCredit { get; set; }
        public decimal Enddepit { get; set; }
        public String Start_Date { get; set; }
        public String End_Date { get; set; }
        public decimal pureFinalCredit { get; set; }
        public decimal pureFinaldepit { get; set; }




    }
}