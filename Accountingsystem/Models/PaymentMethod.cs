using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Accountingsystem.ViewModel;
using System.Data.Entity;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class PaymentMethod
    {/// <summary>
     /// get All Paymentmethod in db
     /// </summary>
     /// <returns>List<PaymentMethod></returns>
        public static List<PaymentMethod> GetAllPaymentMethods()
        {
            var db = new AccountingContainer();
            var allPaymentMethods = db.PaymentMethods.Where(a => a.IsActive != false).ToList();
            return allPaymentMethods;
        }
        public static List<PaymentMethod> GetAllNotActive()
        {
            var db = new AccountingContainer();
            var payment = db.PaymentMethods.Where(a => a.IsActive == false).ToList();
            return payment;
        }
        /// <summary>
        /// get that paymentMethod by its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>PaymentMethod</returns>
        public static PaymentMethod GetPaymentMethodById(int id)
        {
            var db = new AccountingContainer();
            var PaymentMethod = db.PaymentMethods.Find(id);
           
            return PaymentMethod;
        }
        /// <summary>
        /// fill For Creation PaymentMethod
        /// </summary>
        /// <returns>PaymentMethodVM</returns>

        public static PaymentMethodVM getListForCreation()
        {
            var db = new AccountingContainer();
            var paymentVM = new PaymentMethodVM();
            int lastCode = (from s in db.PaymentMethods
                            select s.PaymentMethod_Id).ToList().LastOrDefault();
            paymentVM.PaymentMethod_Id = (lastCode + 1);
            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0);
            
            paymentVM.MyChartOfAccountList = new SelectList(lista.Where(accCode => accCode.Account_Code.StartsWith("010120")&&accCode.isActive!=false ), "Account_Id", "Account_Name");
            paymentVM.ExpenseList = new SelectList(lista.Where(accCode => accCode.Account_Code.StartsWith("05") && accCode.isActive != false), "Account_Id", "Account_Name");
            paymentVM.EmpList = new SelectList(db.Employees, "Emp_Id", "Emp_Name");
            paymentVM.EmPloyeeMemberList = new SelectList(db.Employees, "Emp_Id", "Emp_Name");
            return paymentVM;
        }
        /// <summary>
        /// create payment method and save it in db
        /// </summary>
        /// <param name="PaymentMethodvm"></param>
        /// <returns>bool</returns>
        public static PaymentMethod createNewPaymentMethod(PaymentMethodVM PaymentMethodvm)
        {
            var newPaymentMethod = new PaymentMethod();

            if (PaymentMethodvm.Operation_Cost == 0 && PaymentMethodvm.Percentage == 0)
            {
                newPaymentMethod.PaymentMethod_Id = PaymentMethodvm.PaymentMethod_Id;
                newPaymentMethod.Payment_Type = PaymentMethodvm.Payment_Type;
                newPaymentMethod.Percentage = PaymentMethodvm.Percentage;
                newPaymentMethod.Emp_Id = PaymentMethodvm.Emp_Id;
                //newPaymentMethod.MyBankAccountList = PaymentMethodvm.MyBankAccountList;
                newPaymentMethod.Bank_Account_Id = PaymentMethodvm.Bank_Account_Id;
                newPaymentMethod.Expense_Account_Id = null;
                newPaymentMethod.Operatin_Cost = PaymentMethodvm.Operation_Cost;
                //newPaymentMethod.MyChartOfAccountList = new SelectList(db.ChartOfAccountings.Where(accCode => accCode.Account_Code.StartsWith("010115")), "Account_Id", "Account_Name");
                //newPaymentMethod.MyBankAccountList = new SelectList(db.Banks, "PaymentMethod_Id", "Payment_Type");


                var db = new AccountingContainer();
                newPaymentMethod.Bank_Account_Name = db.ChartOfAccountings.Find(newPaymentMethod.Bank_Account_Id).Account_Name;
                newPaymentMethod.PaymentMethodEmployees = new List<PaymentMethodEmployee>();
                if (PaymentMethodvm.EmployeesMember != null)
                {
                    foreach (var item in PaymentMethodvm.EmployeesMember.ToList())
                    {
                        var empstore = new PaymentMethodEmployee();
                        empstore.Employee_Id = Convert.ToInt32(item);

                        newPaymentMethod.PaymentMethodEmployees.Add(empstore);


                    }
                }

                db.PaymentMethods.Add(newPaymentMethod);
                db.SaveChanges();
                db.Dispose();
            }
            else 
            {
                newPaymentMethod.PaymentMethod_Id = PaymentMethodvm.PaymentMethod_Id;
                newPaymentMethod.Payment_Type = PaymentMethodvm.Payment_Type;
                newPaymentMethod.Percentage = PaymentMethodvm.Percentage;
                //newPaymentMethod.MyBankAccountList = PaymentMethodvm.MyBankAccountList;
                newPaymentMethod.Bank_Account_Id = PaymentMethodvm.Bank_Account_Id;
                if (PaymentMethodvm.Expense_Id == 0 || PaymentMethodvm.Expense_Id == null)
                {
                    newPaymentMethod.Expense_Account_Id = 143;
                    
                }
                else
                {
                    newPaymentMethod.Expense_Account_Id = PaymentMethodvm.Expense_Id;
                }
               
                newPaymentMethod.Operatin_Cost = PaymentMethodvm.Operation_Cost;
                //newPaymentMethod.MyChartOfAccountList = new SelectList(db.ChartOfAccountings.Where(accCode => accCode.Account_Code.StartsWith("010115")), "Account_Id", "Account_Name");
                //newPaymentMethod.MyBankAccountList = new SelectList(db.Banks, "PaymentMethod_Id", "Payment_Type");


                var db = new AccountingContainer();
                newPaymentMethod.Bank_Account_Name = db.ChartOfAccountings.Find(newPaymentMethod.Bank_Account_Id).Account_Name;

                newPaymentMethod.PaymentMethodEmployees = new List<PaymentMethodEmployee>();
                if (PaymentMethodvm.EmployeesMember != null)
                {
                    foreach (var item in PaymentMethodvm.EmployeesMember.ToList())
                    {
                        var empstore = new PaymentMethodEmployee();
                        empstore.Employee_Id = Convert.ToInt32(item);

                        newPaymentMethod.PaymentMethodEmployees.Add(empstore);


                    }
                }

                db.PaymentMethods.Add(newPaymentMethod);
                db.SaveChanges();
                db.Dispose();
            }

      

            return newPaymentMethod;
        }


        /// <summary>
        /// fill for Editing payment method  
        /// </summary>
        /// <param name="id"></param>
        /// <returns>PaymentMethodVM</returns>
        public static PaymentMethodVM fillThePayment(int id)
        {
            var db = new AccountingContainer();
            var findPaymentMethod = GetPaymentMethodById(id);

            if (findPaymentMethod == null)
            {
                return null;
            }

            var fillPaymentMethodvm = new PaymentMethodVM();
            fillPaymentMethodvm.PaymentMethod_Id = findPaymentMethod.PaymentMethod_Id;
            fillPaymentMethodvm.Payment_Type = findPaymentMethod.Payment_Type;
            fillPaymentMethodvm.Percentage = findPaymentMethod.Percentage.Value;
            fillPaymentMethodvm.Operation_Cost = findPaymentMethod.Operatin_Cost.Value;
            if (findPaymentMethod.Expense_Account_Id != null && findPaymentMethod.Expense_Account_Id != 0)
            {
                fillPaymentMethodvm.Expense_Id = findPaymentMethod.Expense_Account_Id.Value;
            }
            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0 && a.isActive != false);
            
            fillPaymentMethodvm.MyChartOfAccountList = new SelectList(lista.Where(accCode => accCode.Account_Code.StartsWith("010120001") || accCode.Account_Code.StartsWith("010120002")), "Account_Id", "Account_Name");
            fillPaymentMethodvm.ExpenseList = new SelectList(lista.Where(accCode => accCode.Account_Code.StartsWith("05")), "Account_Id", "Account_Name");
            fillPaymentMethodvm.EmpList = new SelectList(db.Employees, "Emp_Id", "Emp_Name");
            fillPaymentMethodvm.Emp_Id = findPaymentMethod.Emp_Id;
            //fillPaymentMethodvm.Bank_Account_Id = findPaymentMethod.Bank_Account_Id;
            fillPaymentMethodvm.EmPloyeeMemberList = new SelectList(db.Employees, "Emp_Id", "Emp_Name");
            fillPaymentMethodvm.EmployeesMember = new List<string>();
            foreach (var item in findPaymentMethod.PaymentMethodEmployees)
            {
                string empId = Convert.ToString(item.Employee_Id);
                fillPaymentMethodvm.EmployeesMember.Add(empId);

            }
            return fillPaymentMethodvm;
        }
        /// <summary>
        /// edit in Paymentmethod and save change in db 
        /// </summary>
        /// <param name="PaymentMethodvm"></param>
        /// <returns></returns>
        public static PaymentMethod editPaymentMethod(PaymentMethodVM PaymentMethodvm)
        {
            var db = new AccountingContainer();
            var paymentMethod = db.PaymentMethods.Find(PaymentMethodvm.PaymentMethod_Id);

            if (paymentMethod != null)
            {
                //  select Country from   entity Contry.
                var thistreasury = db.Treasuries.Where(e => e.Trea_Name == paymentMethod.Payment_Type).FirstOrDefault();
                var thisBank = db.Banks.Where(e => e.Bank_Name == paymentMethod.Payment_Type).FirstOrDefault();
                if (thistreasury != null)
                {
                    thistreasury.Trea_Name = PaymentMethodvm.Payment_Type;
                }
                else if(thisBank != null)
                {
                    thisBank.Bank_Name = PaymentMethodvm.Payment_Type;
                }
               
                var resultpayment = db.ChartOfAccountings.Find(PaymentMethodvm.Bank_Account_Id);
                if (resultpayment != null)
                {
                    paymentMethod.Bank_Account_Id = resultpayment.ChartOfAccountingAccount_Id;
                }
                var resultExpense = db.ChartOfAccountings.Find(PaymentMethodvm.Expense_Id);
                if (resultExpense != null)
                {
                    paymentMethod.Expense_Account_Id = resultExpense.ChartOfAccountingAccount_Id.Value;
                }
                paymentMethod.PaymentMethod_Id = PaymentMethodvm.PaymentMethod_Id;
                paymentMethod.Payment_Type = PaymentMethodvm.Payment_Type;
                paymentMethod.Emp_Id = PaymentMethodvm.Emp_Id;
                if (paymentMethod.Expense_Account_Id == 0 || paymentMethod.Expense_Account_Id == null)
                {
                    paymentMethod.Percentage = 0;
                    paymentMethod.Operatin_Cost = 0;
                }
                else
                {
                    paymentMethod.Percentage = PaymentMethodvm.Percentage;
                    paymentMethod.Operatin_Cost = PaymentMethodvm.Operation_Cost;
                }
             
                //paymentMethod.Bank_Account_Id = PaymentMethodvm.Bank_Account_Id;
                paymentMethod.Bank_Account_Name = db.ChartOfAccountings.Find(paymentMethod.Bank_Account_Id).Account_Name;
                if (paymentMethod.PaymentMethodEmployees == null || paymentMethod.PaymentMethodEmployees.ToList().Count() == 0)
                {

                }
                else
                {
                    db.PaymentMethodEmployees.RemoveRange(paymentMethod.PaymentMethodEmployees);
                    db.SaveChanges();
                }
                paymentMethod.PaymentMethodEmployees = new List<PaymentMethodEmployee>();
                if (PaymentMethodvm.EmployeesMember != null)
                {
                    foreach (var empId in PaymentMethodvm.EmployeesMember)
                    {
                        var empo = new PaymentMethodEmployee();

                        empo.Employee_Id = Convert.ToInt32(empId);
                        paymentMethod.PaymentMethodEmployees.Add(empo);
                    }
                }
                db.Entry(paymentMethod).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();

            }
            return paymentMethod;
        }
        /// <summary>
        /// delete payment method from db by using its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static PaymentMethod deletePaymentMethod(int id)
        {
            var db = new AccountingContainer();
            var paymentMethod = db.PaymentMethods.Find(id);
            if (paymentMethod != null)
            {
                db.PaymentMethods.Remove(paymentMethod);
                db.SaveChanges();
                db.Dispose();
            }

            return paymentMethod;
        }
        /// <summary>
        /// get the bank dropdownList that get all bank or treasury in system from chart of Accounting
        /// </summary>
        /// <returns></returns>
        public static PaymentMethodVM GetBankList()
        {
            var db = new AccountingContainer();
            PaymentMethodVM pvm = new PaymentMethodVM();
            // BankVM vm = new BankVM();
            var lista = new List<ChartOfAccounting> { };
            foreach (var k in db.ChartOfAccountings)
            {
                var parent = db.ChartOfAccountings.Where(a => a.ChartOfAccountingAccount_Id == k.Account_Id);
                var o = parent.FirstOrDefault();
                if (o == null)
                {
                    lista.Add(db.ChartOfAccountings.Find(k.Account_Id));

                }


            }
            pvm.MyChartOfAccountList = new SelectList(lista.Where(accCode => accCode.Account_Code.StartsWith("010120001") || accCode.Account_Code.StartsWith("010120002")), "Account_Id", "Account_Name");

            // vm.MyChartOfAccountList = new SelectList(db.ChartOfAccountings.Where(accCode => accCode.Account_Code.StartsWith("010102")), "Account_Id", "Account_Name");
           // pvm.MyChartOfAccountList = new SelectList(db.ChartOfAccountings.Where(accCode => accCode.Account_Code.StartsWith("010102")), "Account_Id", "Account_Name");

            return pvm;
        }
        /// <summary>
        /// تحدث هذه الدالة عند انشاء حساب على الشجرة يمون تحت بنوك اوخزائن او اوراق مالية 
        /// </summary>
        /// <param name="Account"></param>
        public static void CreatePaymentMethod(ChartOfAccounting Account)
        {
            var db = new AccountingContainer();
            var payment = new PaymentMethod();
            payment.Payment_Type = Account.Account_Name;
            payment.Bank_Account_Id = Account.Account_Id;
            payment.Percentage = 0;
            payment.Operatin_Cost = 0;
            db.PaymentMethods.Add(payment);
            db.SaveChanges();
            db.Dispose();
            

        }
        public static string DeleteTransfer(int id)
        {

            var db = new AccountingContainer();
            var deleteobject = db.PaymentMethods.Find(id);
           
            if (deleteobject.IsActive != false)
            {
                deleteobject.IsActive = false;
                

            }
            else
            {
                deleteobject.IsActive = true;
               
            }
           
            db.Entry(deleteobject).State = EntityState.Modified;
        
            db.SaveChanges();


            return Resources.ProductDetails.TransferSucces;

        }
    }
}
