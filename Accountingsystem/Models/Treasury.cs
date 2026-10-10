using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Office.Interop.Excel;
using System.Web.Mvc;

using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Data.Entity.Validation;
using System.Data.Entity;

namespace Accountingsystem.Models
{
    public partial class Treasury
    {
        //GET Detail
        public static Treasury getTreaById(int id)
        {
            var db = new AccountingContainer();
            var trea = db.Treasuries.Find(id);
            return trea;
        }
        // GET index
        public static List<Treasury> getTreasuries()
        {
            var db = new AccountingContainer();
            var Treasuries = db.Treasuries.Where(a => a.IsActive != false).ToList();
            return Treasuries;
        }
        public static List<Treasury> GetAllNotActive()
        {
            var db = new AccountingContainer();
            var Treasuries = db.Treasuries.Where(a => a.IsActive == false).ToList();
            return Treasuries;
        }

        // GET Create 
        public static TreaVM Create()
        {
            var db = new AccountingContainer();
            var treaVM = new TreaVM();
            //var lastCode = (from s in db.Treasuries
            //                select s.Trea_Id).ToList().LastOrDefault();
            //treaVM.Trea_Code = (lastCode + 1).ToString();
            List<object> newList = new List<object>();
            var members = db.SubLedgers.Where(a=>a.isActive!=false).Where(a =>  a.SubLed_Type == "قسم لفرع"||  a.SubLed_Type== "قسم رئيسى لشركة");
            foreach (var member in members)
                newList.Add(new
                {
                    Id = member.SubLed_Id,
                    Name = member.SubLed_Name + "   " + member.SubLed_Code 
                });
            treaVM.DepartmentList = new SelectList(newList, "Id", "Name");
            treaVM.EmployeeList = new SelectList(db.Employees, "Emp_Id", "Emp_Name");
            treaVM.MyChartOfAccountList = new SelectList(db.ChartOfAccountings.Where(accCode => accCode.Account_Code=="010120001" && accCode.isActive != false), "Account_Id", "Account_Name");
            return treaVM;
        }

        //POST Create
        public static Treasury CreateTreasury(TreaVM treaVM)
        {
            var db = new AccountingContainer();
            var Treasury = new Treasury();

            var resultemp = db.Employees.Find(treaVM.selectedEmp_Id);
            
            
            if (resultemp != null)
            {
                Treasury.Emp_Id = resultemp.Emp_Id;
                Treasury.EmpName = (resultemp.Emp_Name).ToString();

            }

            ChartOfAccountingVM accountVM = new ChartOfAccountingVM();
            accountVM.ChartOfAccountingAccount_Id = 89;
            accountVM.Account_Name = treaVM.Trea_Name;
            accountVM.Account_Curenncy = 1;
            Treasury.Trea_Code = ChartOfAccounting.CreateAccount(accountVM);

            Treasury.Trea_Name = treaVM.Trea_Name;
            Treasury.Trea_Desc = treaVM.Trea_Desc;
           
            db.Treasuries.Add(Treasury);
            try
            { db.SaveChanges(); }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        var x = validationError.PropertyName;
                        var y = validationError.ErrorMessage;
                    }
                }
            }
            db.Dispose();
            return Treasury;


        }
        //GET Edit
        public static TreaVM FillTheViewModel(int id)
        {
            var db = new AccountingContainer();
            var Treasury = getTreaById(id);
            var treaVM = new TreaVM();
            //var dept = db.CompanyDepartments.Where(d => d.Id = Treasury.Dep_Id);
            var employ = db.Employees.Where(d => (d.Emp_Id) == Treasury.Emp_Id);
            treaVM.selectedDep_Id = Convert.ToInt32(Treasury.Dep_Id);
            if (employ.Any())
            {
                treaVM.selectedEmp_Id = employ.FirstOrDefault().Emp_Id;
            }

            List<object> newList = new List<object>();
            var members = db.SubLedgers.Where(a => a.SubLed_Type == "قسم لفرع" || a.SubLed_Type == "قسم رئيسى لشركة");
            foreach (var member in members)
                newList.Add(new
                {
                    Id = member.SubLed_Id,
                    Name = member.SubLed_Name + "   " + member.SubLed_Code 
                });
            treaVM.DepartmentList = new SelectList(newList, "Id", "Name");
            treaVM.EmployeeList = new SelectList(db.Employees, "Emp_Id", "Emp_Name");
            treaVM.MyChartOfAccountList = new SelectList(db.ChartOfAccountings.Where(accCode => accCode.Account_Code.StartsWith("010101001")), "Account_Id", "Account_Name");
            treaVM.Trea_Id = Treasury.Trea_Id;
            treaVM.Trea_Code = Treasury.Trea_Code;
            treaVM.Trea_Name = Treasury.Trea_Name;
            treaVM.Trea_Desc = Treasury.Trea_Desc;

            return treaVM;
        }
        //POST Edit
        public static Treasury EditTrea(TreaVM treaVM)
        {
            var db = new AccountingContainer();
            var Treasury = db.Treasuries.Find(treaVM.Trea_Id);
            if (Treasury != null)
            {
                var resultemp = db.Employees.Find(treaVM.selectedEmp_Id);
                var resultDep = db.CompanyDepartments.Find(treaVM.selectedDep_Id);
                var resultaccount = db.ChartOfAccountings.Find(treaVM.SelectedChartOfAccountId);
                var payment = db.PaymentMethods.Where(a => a.Payment_Type == Treasury.Trea_Name).FirstOrDefault();

                if (resultemp != null)
                {
                    Treasury.Emp_Id = (resultemp.Emp_Id);
                    Treasury.EmpName = resultemp.Emp_Name;
                }
                Treasury.Dep_Id = treaVM.selectedDep_Id;
                Treasury.Trea_Id = treaVM.Trea_Id;
                Treasury.Trea_Code = treaVM.Trea_Code;
                Treasury.Trea_Desc = treaVM.Trea_Desc;
                Treasury.Trea_Name = treaVM.Trea_Name;
                payment.Payment_Type= treaVM.Trea_Name;
                var account = db.ChartOfAccountings.Where(a => a.Account_Code == treaVM.Trea_Code).FirstOrDefault();
                account.Account_Name = treaVM.Trea_Name; ;
                db.Entry(account).State = EntityState.Modified;
                db.Entry(payment).State = EntityState.Modified;
                db.Entry(Treasury).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                return Treasury;
            }
            return null;
        }


        public static String Import(HttpPostedFileBase excelfile)
        {
            var data = getdataFillter(excelfile);
            List<Treasury> listTreasry= new List<Treasury>();
            if (data.Item2.Count() == 0)
            {
                foreach (var item in data.Item1)
                {
                    var tre = CreateTreasuryForFile(item);
                    listTreasry.Add(tre);

                }
             
                var listTreaseryempty = listTreasry.Where(a => a.Balance_Amount != 0).ToList();

                if (listTreaseryempty.Count == 0)
                {

                }
                else
                {
                    Utilites.OpenBalanceFile.MakeOpenBallanceForGroupOFTreasury(listTreasry);
                }
                return "تم حفظ الملف بنجاح";

            }
            string combindedString = string.Join(" ,", data.Item2.ToArray());
            return "يوجد اخطاء فى الاسطر التالية" + combindedString;
        }


        public static Tuple<List<TreaVM>, List<String>> getdataFillter(HttpPostedFileBase excelfile)
        {
            var treasuryList = new List<TreaVM> { };
            var messageList = new List<String> { };
            var db = new AccountingContainer();
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                return Tuple.Create(treasuryList, messageList);

            }

            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                   // string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + excelfile.FileName);
                    var nameOfFile = "Treasury" + DateTime.Now.ToBinary() + ".xlsx";
                    string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + nameOfFile);

                    excelfile.SaveAs(path);
                   

                    Application application = new Application();
                    Workbook workbook = application.Workbooks.Open(path);
                    Worksheet worksheet = workbook.ActiveSheet;
                    Range range = worksheet.UsedRange;
                    for (int row = 3; row <= range.Rows.Count; row++)
                    {
                        TreaVM tre = new TreaVM();
                        // cust.Customer_Id = int.Parse(((Range)range.Cells[row, 1]).Text);
                        tre.Trea_Name = ((Range)range.Cells[row, 1]).Text;
                        if (tre.Trea_Name == "")
                        {
                            messageList.Add(row.ToString());
                            continue;
                        }
                     
                        try
                        {
                            var cusbal = ((Range)range.Cells[row, 3]).Text;
                            if (cusbal == "")
                            {
                                tre.treasuryBallance = 0;
                            }
                            else
                            {
                                tre.treasuryBallance = Convert.ToDecimal(((Range)range.Cells[row, 3]).Text);
                            }


                           
                           
                        }
                        catch
                        {

                            messageList.Add(row.ToString());
                            continue;


                        }
                        if (tre.treasuryBallance < 0)
                        {
                            messageList.Add(row.ToString());
                            continue;
                        }





                        tre.Trea_Desc = ((Range)range.Cells[row, 2]).Text;
                       
                        treasuryList.Add(tre);





                    }
                    workbook.Save();
                    workbook.Close(path);



                   // File.Delete(path);

                }
            }
            return Tuple.Create(treasuryList, messageList);
        }

        /// <summary>
        /// For validation of errors for treasury file
        /// </summary>
        /// <param name="treaVM"></param>
        /// <returns></returns>
        public static Treasury CreateTreasuryForFile(TreaVM treaVM)
        {
            var db = new AccountingContainer();
            var Treasury = new Treasury();

          

            ChartOfAccountingVM accountVM = new ChartOfAccountingVM();
            accountVM.ChartOfAccountingAccount_Id = 89;
            accountVM.Account_Name = treaVM.Trea_Name;
            accountVM.Account_Curenncy = 1;
            Treasury.Trea_Code = ChartOfAccounting.CreateAccount(accountVM);
            Treasury.Balance_Amount = treaVM.treasuryBallance;
            Treasury.Trea_Name = treaVM.Trea_Name;
            Treasury.Trea_Desc = treaVM.Trea_Desc;

            db.Treasuries.Add(Treasury);
            try
            { db.SaveChanges(); }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        var x = validationError.PropertyName;
                        var y = validationError.ErrorMessage;
                    }
                }
            }
            db.Dispose();
            return Treasury;


        }
        public static string DeleteTransfer(int id)
        {

            var db = new AccountingContainer();
            var deleteobject = db.Treasuries.Find(id);
            var treasAccount = db.ChartOfAccountings.Where(a => a.Account_Code == deleteobject.Trea_Code).FirstOrDefault();
            if (deleteobject.IsActive != false)
            {
                deleteobject.IsActive = false;
                treasAccount.isActive = false;

            }
            else
            {
                deleteobject.IsActive = true;
                treasAccount.isActive = true;
            }
            var paymentmethod = db.PaymentMethods.Where(a => a.Bank_Account_Id == treasAccount.Account_Id).ToList();
            if (paymentmethod.Count() > 0)
            {
                foreach (var pay in paymentmethod)
                {
                    pay.IsActive = deleteobject.IsActive;

                    db.Entry(pay).State = EntityState.Modified;

                }
            }
            db.Entry(deleteobject).State = EntityState.Modified;
            db.Entry(treasAccount).State = EntityState.Modified;
            db.SaveChanges();


            return Resources.ProductDetails.TransferSucces;

        }
    }
}