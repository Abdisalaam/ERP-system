using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class TrialBalncePostVM
    {
        public string Start_Date { get; set; }
        public string End_Date { get; set; }
        public Boolean WithOutOFFGLs { get; set; }
        public List<GLDetailVM> StartBalance { get; set; }
        public List<GLDetailVM> Movement{ get; set; }
        public List<GLDetailVM> TotalBalance{ get; set; }
        public List<BalanceTrialVMAttribute> ballanceAttrribute { get; set; }
        public decimal TotalOpenBalance { get; set; }
        public decimal TotalEndBalance { get; set; }
        public decimal Total { get; set; }
      
        public decimal startBallanceCreditSUM { get; set; }
        public decimal startBallanceDebitSUM { get; set; }
        public decimal MovmentBallanceCreditSUM { get; set; }
        public decimal MovmentBallanceDebitSUM { get; set; }
        public decimal pureCreditSUM { get; set; }
        public decimal puredepitSUM { get; set; }
        public decimal EndCreditSUM { get; set; }
        public decimal EnddepitSUM { get; set; }
        public decimal PureEndCreditSUM { get; set; }
        public decimal PureEnddepitSUM { get; set; }
        public Company company { get; set; }

    }
}