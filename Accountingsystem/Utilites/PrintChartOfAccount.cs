using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using Accountingsystem.ViewModel.ReportsNewVM;
using Accountingsystem.ViewModel.ReportsVM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace Accountingsystem.Utilites
{
    public class PrintChartOfAccount
    {
        List<ChartOfAccountingVM> chartList = new List<ChartOfAccountingVM>();
        List<ChartOfAccounting> ChartModelList = new List<ChartOfAccounting> { };
        List<TrialBalanceDetail> BalanceList = new List<TrialBalanceDetail> { };
        List<RoleFunctionVM> FunctionList = new List<RoleFunctionVM> { };
        public List<ChartOfAccountingVM> getAllBalanceSheet(String code)
        {
            var db = new AccountingContainer();
            var account = db.ChartOfAccountings.Where(a => a.Account_Code == code).FirstOrDefault();
            foreach (var accou in account.ChartOfAccountings)
            {
                ChartOfAccountingVM chart = new ChartOfAccountingVM();
                chart.Account_Name = accou.Account_Name;
                chart.Level_Expression = new String('-', accou.Account_Level);
                chart.Account_Level = accou.Account_Level;
                chart.Account_Code = accou.Account_Code;
                chart.ChartOfAccountingAccount_Id = accou.ChartOfAccountingAccount_Id;
                //var chartHasChild = db.ChartOfAccountings.Where(z => z.ChartOfAccountingAccount_Id == accou.Account_Id).ToList();
            
                    chart.Account_Balance = accou.BalanceSheetValue.Value;
                

                chartList.Add(chart);
                var a = getAllBalanceSheet(chart.Account_Code);


            }


            return chartList;



        }

        public List<ChartOfAccounting> getAllBalanceSheetModel(String code)
        {
            var db = new AccountingContainer();
            var account = db.ChartOfAccountings.Where(a => a.Account_Code == code).FirstOrDefault();
            foreach (var accou in account.ChartOfAccountings)
            {
                ChartOfAccounting chart = new ChartOfAccounting();
                chart.Account_Name = accou.Account_Name;
                chart.Account_Level = accou.Account_Level;
                chart.Account_Code = accou.Account_Code;
                chart.Account_Id = accou.Account_Id;
                chart.ISFinalChild = accou.ISFinalChild;
                chart.isActive = accou.isActive;
                chart.BalanceSheetValue = accou.BalanceSheetValue;
                chart.ChartOfAccountingAccount_Id = accou.ChartOfAccountingAccount_Id;


                ChartModelList.Add(chart);
                var a = getAllBalanceSheetModel(chart.Account_Code);


            }


            return ChartModelList;



        }


        public List<TrialBalanceDetail> AddToFatherAccout(string code, List<TrialBalanceDetail> AllDetail)
        {
            var db = new AccountingContainer();
            var account = db.ChartOfAccountings.Where(a => a.Account_Code == code).FirstOrDefault();
            foreach (var accou in account.ChartOfAccountings)
            {

                var DetailOfBalance = AllDetail.Where(a => a.Account.Account_Code.StartsWith(accou.Account_Code)&&a.ChildAccountd==null&&a.Asset==null).ToList();
                if (DetailOfBalance.Count == 0)
                {

                }
                else
                {
                    TrialBalanceDetail detail = new TrialBalanceDetail();
                    var SamBalance = AllDetail.Where(a => a.Account.Account_Id==accou.Account_Id).ToList();
                    if (SamBalance.Count() > 0)
                    {
                        detail.IshasAmount = true;
                    }
                    detail.Account = accou;
                    detail.AmountBefor = DetailOfBalance.Select(a => a.AmountBefor).Sum();
                    detail.AmountOfPeriod = DetailOfBalance.Select(a => a.AmountOfPeriod).Sum();
                    detail.CeditMov = DetailOfBalance.Select(a => a.CeditMov).Sum();
                    detail.LevelExpresion=new String('-', detail.Account.Account_Level);
                    detail.DepitMov = DetailOfBalance.Select(a => a.DepitMov).Sum();
                    detail.FinalAmount = DetailOfBalance.Select(a => a.FinalAmount).Sum();
                    BalanceList.Add(detail);
                    if (accou.Account_Id == 17 || accou.Account_Id == 37)
                    {

                        var a = 9;
                    }
                   var ChildList= AllDetail.Where(a => a.Account.Account_Id == accou.Account_Id&&(a.Asset!=null||a.ChildAccountd!=null) ).ToList();
                    if (ChildList.Count() > 0)
                    {
                        BalanceList.AddRange(ChildList);
                    }
                }
                AddToFatherAccout(accou.Account_Code, AllDetail);
            }

            return BalanceList;

        }

        public List<RoleFunctionVM> AddToFatherAccout(string code, List<SystemFunction> AllDetail)
        {
            var db = new AccountingContainer();
            var systemFunction = db.SystemFunctions.Where(a => a.Code == code).FirstOrDefault();
            foreach (var accou in systemFunction.SystemFunctions)
            {
                var rolefun = new RoleFunctionVM();

                var DetailOfBalance = AllDetail.Where(a => a.Code.StartsWith(accou.Code)).ToList();
                if (DetailOfBalance.Count != 0)
                {
                    rolefun.SystemFunction = accou;
                    FunctionList.Add(rolefun);
                }
                
                AddToFatherAccout(accou.Code, AllDetail);
            }

            return FunctionList;

        }

        public List<RoleFunctionVM> AddToFatherAccoutForEdit(string code, List<SystemFunction> AllDetail,SystemRole role)
        {
            var db = new AccountingContainer();
            var systemFunction = db.SystemFunctions.Where(a => a.Code == code).FirstOrDefault();
            foreach (var accou in systemFunction.SystemFunctions)
            {
                var rolefun = new RoleFunctionVM();

                var DetailOfBalance = AllDetail.Where(a => a.Code.StartsWith(accou.Code)).ToList();


                if (DetailOfBalance.Count != 0)
                {
                    rolefun.SystemFunction = accou;
                    var existrole = role.RoleFunctions.Where(a => a.SystemFunctionId == accou.Id).FirstOrDefault();
                    if (existrole != null)
                    {
                        rolefun.isExist = true;
                    }
                    else
                    {

                        rolefun.isExist = false;
                    }

                    FunctionList.Add(rolefun);
                }

                AddToFatherAccoutForEdit(accou.Code, AllDetail,role);
            }

            return FunctionList;

        }

    }
}