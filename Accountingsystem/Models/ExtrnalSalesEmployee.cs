using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Accountingsystem.Models;
using Accountingsystem.ViewModel;

namespace Accountingsystem.Models
{
    public partial class ExtrnalSalesEmployee
    {
        public static List<ExtrnalSalesEmployee> GetAllEmployees()
        {
            var db = new AccountingContainer();
            var employeeList = db.ExtrnalSalesEmployees.ToList();
            return employeeList;

        }
        public static List<ExternalEmployeePayments> GetAllPayments()
        {
            var db = new AccountingContainer();
            var paymentsList = db.ExternalEmployeePayments.ToList();
            return paymentsList;

        }
        public static List<EmployeeSalesDetails> GetAllSales()
        {
            var db = new AccountingContainer();
            var salesList = db.EmployeeSalesDetails.ToList();
            return salesList;

        }
        public static List<ExtrnalSalesEmployee> GetEmployeeChilderen(int parentID)
        {
            var db = new AccountingContainer();
            var employeeChilderenList = db.ExtrnalSalesEmployees.Where(e => e.ParentID == parentID).ToList();
            return employeeChilderenList;

        }
        public static ExternalEmployeePaymentsVM PreparePaymentVMForCreation(int empID)
        {
            var db = new AccountingContainer();
            var employee = db.ExtrnalSalesEmployees.Find(empID);
            if (employee == null)
            {
                return null;
            }
            ExternalEmployeePaymentsVM externalEmployeePaymentsVM = new ExternalEmployeePaymentsVM
            {
                ExternalSalesEmployeeID = employee.Id
            };
            externalEmployeePaymentsVM.ExternalSalesEmployeeList = new SelectList(db.ExtrnalSalesEmployees, "Id", "Name");
            externalEmployeePaymentsVM.SelectedExternalSalesEmployeeID = employee.Id;
            externalEmployeePaymentsVM.ExternalSalesEmployeeID = employee.Id;
            return externalEmployeePaymentsVM;
        }
        public static ExternalEmployeePaymentsVM PreparePaymentVMForCreation()
        {
            var db = new AccountingContainer();
            ExternalEmployeePaymentsVM externalEmployeePaymentsVM = new ExternalEmployeePaymentsVM();           
            externalEmployeePaymentsVM.ExternalSalesEmployeeList = new SelectList(db.ExtrnalSalesEmployees, "Id", "Name");           
            return externalEmployeePaymentsVM;
        }
        public static ExternalEmployeePayments FindPaymentByID(int id)
        {
            var db = new AccountingContainer();
            var payment = db.ExternalEmployeePayments.Find(id);
            if (payment == null)
            {
                return null;
            }
           
            return payment;
        }
        public static ExtrnalSalesEmployee DeletePayment(int id)
        {
            var db = new AccountingContainer();
            var payment = db.ExternalEmployeePayments.Find(id);
            if (payment == null)
            {
                return null;
            }
            var employee = payment.ExtrnalSalesEmployee;
            db.ExternalEmployeePayments.Remove(payment);
            db.SaveChanges();
            employee.TotalReceivedAmount = employee.ExternalEmployeePayments.Sum(e => e.PaymentAmount);
            db.Entry(employee).State = EntityState.Modified;
            db.SaveChanges();
            return employee;
        }
        public static ExtrnalSalesEmployee DeleteSale(int id)
        {
            var db = new AccountingContainer();
            var sale = db.EmployeeSalesDetails.Find(id);
            if (sale == null)
            {
                return null;
            }
            var employee = sale.ExtrnalSalesEmployee;
            db.EmployeeSalesDetails.Remove(sale);
            db.SaveChanges();
            employee.TotalAmountBalance = employee.EmployeeSalesDetails.Sum(e => e.Amount);
            employee.TotalCommissionAmount = employee.EmployeeSalesDetails.Sum(e => e.CommissionAmount);
            employee.TotalCustommersCount = employee.EmployeeSalesDetails.Count();
            if (employee.ParentID > 0 && employee.Id != employee.ParentID)
            {
                var parent = db.ExtrnalSalesEmployees.Find(employee.ParentID);
                var allEmployees = db.ExtrnalSalesEmployees.Where(e => e.ParentID == parent.Id && e.Id != parent.Id);
                if (allEmployees.Any())
                {
                    parent.TotalCommissionAmountFromChild = 0;
                    parent.TotalChildsSalesAmountBalance = 0;
                    parent.TotalChildsSalesCount = 0;
                    foreach (var item in allEmployees)
                    {
                        parent.TotalCommissionAmountFromChild += item.EmployeeSalesDetails.Sum(e => e.ParentCommissionAmount);
                        parent.TotalChildsSalesAmountBalance += item.EmployeeSalesDetails.Sum(e => e.Amount);
                        parent.TotalChildsSalesCount += item.EmployeeSalesDetails.Count();
                    }
                    db.Entry(parent).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            db.Entry(employee).State = EntityState.Modified;
            db.SaveChanges();
            return employee;
        }
        public static EmployeeSalesDetailsVM PrepareEmployeeSalesDetailsVMForCreation(int empID)
        {
            var db = new AccountingContainer();
            var employee = db.ExtrnalSalesEmployees.Find(empID);
            if (employee == null)
            {
                return null;
            }
            EmployeeSalesDetailsVM employeeSalesDetailsVM = new EmployeeSalesDetailsVM();
            employeeSalesDetailsVM.ExtrnalEmployeeList = new SelectList(db.ExtrnalSalesEmployees, "Id", "Name");
            employeeSalesDetailsVM.SelectedExtrnalEmployeeID = employee.Id;
            employeeSalesDetailsVM.ExternalSalesEmployeeID = employee.Id;
            employeeSalesDetailsVM.ExtrnalSalesEmployee = employee;
            employeeSalesDetailsVM.CustomerList = new SelectList(db.Customers, "Customer_Id", "Customer_Name");

            return employeeSalesDetailsVM;
        }
        public static EmployeeSalesDetailsVM PrepareEmployeeSalesDetailsVMForCreation()
        {
            var db = new AccountingContainer();
           
            EmployeeSalesDetailsVM employeeSalesDetailsVM = new EmployeeSalesDetailsVM();
            employeeSalesDetailsVM.ExtrnalEmployeeList = new SelectList(db.ExtrnalSalesEmployees, "Id", "Name");           
            employeeSalesDetailsVM.CustomerList = new SelectList(db.Customers, "Customer_Id", "Customer_Name");

            return employeeSalesDetailsVM;
        }
        public static ExtrnalSalesEmployee AddEmployeeSalesDetailsToEmployee(EmployeeSalesDetailsVM employeeSalesDetailsVM)
        {
            var db = new AccountingContainer();
            var employee = db.ExtrnalSalesEmployees.Find(employeeSalesDetailsVM.SelectedExtrnalEmployeeID);
            if (employee == null)
            {
                return null;
            }
           
            EmployeeSalesDetails employeeSalesDetails = new EmployeeSalesDetails();
            employeeSalesDetails.Amount = employeeSalesDetailsVM.Amount;
            employeeSalesDetails.Comment = employeeSalesDetailsVM.Comment;
            employeeSalesDetails.CommissionAmount = employeeSalesDetailsVM.CommissionAmount;
            employeeSalesDetails.ParentCommissionAmount = employeeSalesDetailsVM.ParentCommissionAmount;
            employeeSalesDetails.SalesDate = DateTime.Now;            
            if (employeeSalesDetailsVM.SelectedCustomerID > 0)
            {
                var custommer = db.Customers.Find(employeeSalesDetailsVM.SelectedCustomerID);
                employeeSalesDetails.CustommerID = custommer.Customer_Id;
                employeeSalesDetails.CustommerName = custommer.Customer_Name;
                employeeSalesDetails.CustommerMobileNumber = custommer.Customer_Phone;
                employeeSalesDetails.CustommerEmail = custommer.Customer_Mail;
            }
            if (!string.IsNullOrEmpty(employeeSalesDetailsVM.CustomerName))
            {               
                employeeSalesDetails.CustommerName = employeeSalesDetailsVM.CustomerName;               
            }
            if (!string.IsNullOrEmpty(employeeSalesDetailsVM.CustomerEmail))
            {               
                employeeSalesDetails.CustommerEmail = employeeSalesDetailsVM.CustomerEmail;
            }
            if (!string.IsNullOrEmpty(employeeSalesDetailsVM.CustomerMobileNumber))
            {
                employeeSalesDetails.CustommerMobileNumber = employeeSalesDetailsVM.CustomerMobileNumber;
            }
            employee.EmployeeSalesDetails.Add(employeeSalesDetails);
            employee.TotalAmountBalance += employeeSalesDetails.Amount;
            employee.TotalCommissionAmount += employeeSalesDetails.CommissionAmount;
            employee.TotalCustommersCount += 1;   
            db.Entry(employee).State = EntityState.Modified;
            db.SaveChanges();
            if (employee.ParentID > 0 && employee.Id != employee.ParentID)
            {
                var parent = db.ExtrnalSalesEmployees.Find(employee.ParentID);
                var allEmployees = db.ExtrnalSalesEmployees.Where(e => e.ParentID == parent.Id && e.Id != parent.Id);
                if (allEmployees.Any())
                {
                    parent.TotalCommissionAmountFromChild = 0;
                    parent.TotalChildsSalesAmountBalance = 0;
                    parent.TotalChildsSalesCount = 0;
                    foreach (var item in allEmployees)
                    {
                        parent.TotalCommissionAmountFromChild += item.EmployeeSalesDetails.Sum(e => e.ParentCommissionAmount);
                        parent.TotalChildsSalesAmountBalance += item.EmployeeSalesDetails.Sum(e => e.Amount);
                        parent.TotalChildsSalesCount += item.EmployeeSalesDetails.Count();
                    }
                    db.Entry(parent).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return employee;
        }

        public static ExtrnalSalesEmployeeVM PrepareExtrnalEmployeeVMForCreation()
        {
            var db = new AccountingContainer();
           
            ExtrnalSalesEmployeeVM extrnalSalesEmployeeVM = new ExtrnalSalesEmployeeVM();
            extrnalSalesEmployeeVM.ParentList = new SelectList(db.ExtrnalSalesEmployees, "Id", "Name");
            
            
            return extrnalSalesEmployeeVM;
        }

        public static ExtrenlaEmployeeOfTheMonthVM GetEmployeeOtheMonth(DateTime startDate, DateTime endDate)
        {
            var db = new AccountingContainer();
            DateTime _endDate = endDate.AddDays(1);
            DateTime _startDate = startDate.AddDays(-1);
            var employeesSalesList = db.EmployeeSalesDetails.Where(e => e.SalesDate > _startDate && e.SalesDate < _endDate).ToList();
            List<ExtrnalSalesEmployee> _ExtrnalSalesEmployeeList = new List<ExtrnalSalesEmployee>();
            var employees = db.ExtrnalSalesEmployees.ToList();
            ExtrenlaEmployeeOfTheMonthVM extrenlaEmployeeOfTheMonthVM = new ExtrenlaEmployeeOfTheMonthVM();
            extrenlaEmployeeOfTheMonthVM.ExtrnalSalesEmployees = new List<ExtrnalSalesEmployee>();
            foreach (var employee in employees)
            {
                employee.TotalAmountBalance = 0;
                employee.TotalCustommersCount = 0;              
                bool soldSomthing = false;
                foreach (var sale in employeesSalesList)
                {
                    if (sale.ExtrnalSalesEmployee.Id == employee.Id)
                    {
                        employee.TotalAmountBalance += sale.Amount;
                        employee.TotalCustommersCount += 1;                       
                        soldSomthing = true;
                    }
                }
                if (soldSomthing)
                {
                    extrenlaEmployeeOfTheMonthVM.ExtrnalSalesEmployees.Add(employee);
                }
            }
             extrenlaEmployeeOfTheMonthVM.ExtrnalSalesEmployees.OrderBy(e => e.TotalAmountBalance).ToList();
            return extrenlaEmployeeOfTheMonthVM;
        }

        public static ExtrnalSalesEmployeeVM PrepareExtrnalEmployeeVMForEdit(int empID)
        {
            var db = new AccountingContainer();
            var employee = db.ExtrnalSalesEmployees.Find(empID);
            if (employee == null)
            {
                return null;
            }
            ExtrnalSalesEmployeeVM extrnalSalesEmployeeVM = new ExtrnalSalesEmployeeVM();
            extrnalSalesEmployeeVM.Id = employee.Id;
            extrnalSalesEmployeeVM.Name = employee.Name;
            extrnalSalesEmployeeVM.Email = employee.Email;
            extrnalSalesEmployeeVM.MobileNumber = employee.MobileNumber;
            extrnalSalesEmployeeVM.City = employee.City;
            extrnalSalesEmployeeVM.BankName = employee.BankName;
            extrnalSalesEmployeeVM.BankAccountNumber = employee.BankAccountNumber;
            extrnalSalesEmployeeVM.IBAN = employee.IBAN;
            extrnalSalesEmployeeVM.PromoCode = employee.PromoCode;
            extrnalSalesEmployeeVM.ParentList = new SelectList(db.ExtrnalSalesEmployees, "Id", "Name");
            if (employee.ParentID > 0)
            {
                if (employee.Id != employee.ParentID)
                {
                    var parent = db.ExtrnalSalesEmployees.Find(employee.ParentID);
                    extrnalSalesEmployeeVM.SelectedParentID = parent.Id;
                    extrnalSalesEmployeeVM.ParantName = parent.Name;
                    extrnalSalesEmployeeVM.ParentPromoCode = parent.PromoCode;
                }
            }
           
            extrnalSalesEmployeeVM.TotalAmountBalance = employee.TotalAmountBalance;
            extrnalSalesEmployeeVM.TotalCustommerCount = employee.TotalCustommersCount;
            extrnalSalesEmployeeVM.TotalReceivedAmount = employee.TotalReceivedAmount;
            extrnalSalesEmployeeVM.TotalChildsSalesAmountBalance = employee.TotalChildsSalesAmountBalance;
            extrnalSalesEmployeeVM.TotalChildsSalesCount = employee.TotalChildsSalesCount;
            extrnalSalesEmployeeVM.TotalCommissionsAmount = employee.TotalCommissionAmount;
            extrnalSalesEmployeeVM.TotalCommissionsAmountFromChild = employee.TotalCommissionAmountFromChild;
            extrnalSalesEmployeeVM.RegistedDate = employee.RegistedDate;
            extrnalSalesEmployeeVM.EmailConfirmed = employee.EmailConfirmed;
            extrnalSalesEmployeeVM.Comment = employee.Comment;
            return extrnalSalesEmployeeVM;
        }

        public static ExtrenlaEmployeeOfTheMonthVM PrepareEmployeeOtheMonth()
        {
            var db = new AccountingContainer();            
            ExtrenlaEmployeeOfTheMonthVM extrenlaEmployeeOfTheMonthVM = new ExtrenlaEmployeeOfTheMonthVM();           
            extrenlaEmployeeOfTheMonthVM.ExtrnalSalesEmployees = db.ExtrnalSalesEmployees.OrderBy(e => e.TotalAmountBalance).ToList();
            return extrenlaEmployeeOfTheMonthVM;
        }

        public static ExtrnalSalesEmployee UpdateExtrnalEmployee(ExtrnalSalesEmployeeVM extrnalSalesEmployeeVM)
        {
            var db = new AccountingContainer();
            var employee = db.ExtrnalSalesEmployees.Find(extrnalSalesEmployeeVM.Id);
            if (employee == null)
            {
                return null;
            }
            employee.Name = extrnalSalesEmployeeVM.Name;
            employee.MobileNumber = extrnalSalesEmployeeVM.MobileNumber;
            employee.City = extrnalSalesEmployeeVM.City;
            employee.BankName = extrnalSalesEmployeeVM.BankName;
            employee.BankAccountNumber = extrnalSalesEmployeeVM.BankAccountNumber;
            employee.IBAN = extrnalSalesEmployeeVM.IBAN;
            if (extrnalSalesEmployeeVM.SelectedParentID > 0)
            {
                if (employee.Id != extrnalSalesEmployeeVM.SelectedParentID)
                {
                    var parent = db.ExtrnalSalesEmployees.Find(extrnalSalesEmployeeVM.SelectedParentID);
                    employee.ParentID = parent.Id;
                    employee.ParentPromoCode = parent.PromoCode;
                    employee.ParentName = parent.Name;
                }
            }
            if (extrnalSalesEmployeeVM.SelectedParentID <=0 )
            {
                employee.ParentID = employee.Id;
                employee.ParentPromoCode = employee.PromoCode;
                employee.ParentName = employee.Name;
            }
            employee.Comment = extrnalSalesEmployeeVM.Comment;
            db.Entry(employee).State = EntityState.Modified;
            db.SaveChanges();
            return employee;
        }
        public static ExtrnalSalesEmployee AddNewExtrnalEmployee(ExtrnalSalesEmployeeVM extrnalSalesEmployeeVM)
        {
            var db = new AccountingContainer();
            ExtrnalSalesEmployee employee = new ExtrnalSalesEmployee();
            if (employee == null)
            {
                return null;
            }
            employee.Name = extrnalSalesEmployeeVM.Name;
            employee.Email = extrnalSalesEmployeeVM.Email;
            employee.MobileNumber = extrnalSalesEmployeeVM.MobileNumber;
            employee.City = extrnalSalesEmployeeVM.City;
            employee.BankName = extrnalSalesEmployeeVM.BankName;
            employee.BankAccountNumber = extrnalSalesEmployeeVM.BankAccountNumber;
            employee.IBAN = extrnalSalesEmployeeVM.IBAN;
            employee.RegistedDate = DateTime.Now;          
            employee.EmailConfirmed = true;
            employee.PromoCode = extrnalSalesEmployeeVM.PromoCode;
            if (extrnalSalesEmployeeVM.SelectedParentID > 0)
            {
                var parent = db.ExtrnalSalesEmployees.Find(extrnalSalesEmployeeVM.SelectedParentID);
                employee.ParentID = parent.Id;
                employee.ParentPromoCode = parent.PromoCode;
                employee.ParentName = parent.Name;
            }
           
            employee.TotalAmountBalance = 0;
            employee.TotalCustommersCount = 0;
            employee.TotalCommissionAmount = 0;
            employee.TotalReceivedAmount = 0;
            employee.TotalChildsSalesAmountBalance = 0;
            employee.TotalChildsSalesCount = 0;
            employee.TotalCommissionAmountFromChild = 0;
            employee.Comment = extrnalSalesEmployeeVM.Comment;
            db.ExtrnalSalesEmployees.Add(employee);
            db.SaveChanges();
            if (extrnalSalesEmployeeVM.SelectedParentID <= 0)
            {
                employee.ParentID = employee.Id;
                employee.ParentPromoCode = employee.PromoCode;
                employee.ParentName = employee.Name;
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
            }
            return employee;
        }
        public static ExtrnalSalesEmployee GetEmployeeByID(int id)
        {
            var db = new AccountingContainer();
            var employee = db.ExtrnalSalesEmployees.Find(id);
            if (employee == null)
            {
                return null;
            }
            var allEmployees = db.ExtrnalSalesEmployees.Where(e => e.ParentID == employee.Id && e.Id != employee.Id);
            employee.TotalAmountBalance = employee.EmployeeSalesDetails.Sum(e => e.Amount);
            employee.TotalCustommersCount = employee.EmployeeSalesDetails.Count();
            employee.TotalReceivedAmount = employee.ExternalEmployeePayments.Sum(e => e.PaymentAmount);
            employee.TotalCommissionAmount = employee.EmployeeSalesDetails.Sum(e => e.CommissionAmount);
            if (allEmployees.Any())
            {
                employee.TotalCommissionAmountFromChild = 0;
                employee.TotalChildsSalesAmountBalance = 0;
                employee.TotalChildsSalesCount = 0;
                foreach (var item in allEmployees)
                {
                    employee.TotalCommissionAmountFromChild += item.EmployeeSalesDetails.Sum(e => e.ParentCommissionAmount);
                    employee.TotalChildsSalesAmountBalance += item.EmployeeSalesDetails.Sum(e => e.Amount);
                    employee.TotalChildsSalesCount += item.EmployeeSalesDetails.Count();
                }               
            }
            return employee;
        }
        public static ExternalEmployeePayments GetPaymentByID(int id)
        {
            var db = new AccountingContainer();
            var payment = db.ExternalEmployeePayments.Find(id);
            if (payment == null)
            {
                return null;
            }            
            return payment;
        }
        public static EmployeeSalesDetails GetSaleByID(int id)
        {
            var db = new AccountingContainer();
            var sale = db.EmployeeSalesDetails.Find(id);
            if (sale == null)
            {
                return null;
            }
            return sale;
        }
        public static EmployeeSalesDetailsVM PrepareSaleForEdit(int id)
        {
            var db = new AccountingContainer();
            var sale = db.EmployeeSalesDetails.Find(id);
            if (sale == null)
            {
                return null;
            }
            EmployeeSalesDetailsVM employeeSalesDetailsVM = new EmployeeSalesDetailsVM();
            employeeSalesDetailsVM.Id = sale.Id;
            employeeSalesDetailsVM.Amount = sale.Amount;
            employeeSalesDetailsVM.CommissionAmount = sale.CommissionAmount;
            employeeSalesDetailsVM.ParentCommissionAmount = sale.ParentCommissionAmount.Value;
            employeeSalesDetailsVM.SalesDate = sale.SalesDate;
            employeeSalesDetailsVM.SelectedExtrnalEmployeeID = sale.ExtrnalSalesEmployeeId;
            employeeSalesDetailsVM.ExtrnalSalesEmployee = sale.ExtrnalSalesEmployee;
            employeeSalesDetailsVM.CustomerName = sale.CustommerName;
            employeeSalesDetailsVM.CustomerEmail = sale.CustommerEmail;
            employeeSalesDetailsVM.ExternalSalesEmployeeID = sale.ExtrnalSalesEmployeeId;
            employeeSalesDetailsVM.ExtrnalSalesEmployee = sale.ExtrnalSalesEmployee;
            employeeSalesDetailsVM.CustomerMobileNumber = sale.CustommerMobileNumber;
            if (sale.CustommerID > 0)
            {
                var customer = db.Customers.Find(sale.CustommerID);
                employeeSalesDetailsVM.SelectedCustomerID = customer.Customer_Id;
                employeeSalesDetailsVM.CustomerName = customer.Customer_Name;
                employeeSalesDetailsVM.CustomerEmail = customer.Customer_Mail;
                employeeSalesDetailsVM.CustomerMobileNumber = customer.Customer_Phone;
            }

            employeeSalesDetailsVM.ExtrnalEmployeeList = new SelectList(db.ExtrnalSalesEmployees, "Id", "Name");
            employeeSalesDetailsVM.CustomerList = new SelectList(db.Customers, "Customer_Id", "Customer_Name");
            return employeeSalesDetailsVM;
        }
        public static ExternalEmployeePaymentsVM PreparePaymentForEdit(int id)
        {
            var db = new AccountingContainer();
            var payment = db.ExternalEmployeePayments.Find(id);
            if (payment == null)
            {
                return null;
            }
            ExternalEmployeePaymentsVM externalEmployeePaymentsVM = new ExternalEmployeePaymentsVM();
            externalEmployeePaymentsVM.Id = payment.Id;
            externalEmployeePaymentsVM.PaymentAmount = payment.PaymentAmount;
            externalEmployeePaymentsVM.PaymentDate = payment.PaymentDate;
            externalEmployeePaymentsVM.Comment = payment.Comment;
            externalEmployeePaymentsVM.ExternalSalesEmployeeList = new SelectList(db.ExtrnalSalesEmployees, "Id", "Name");
            externalEmployeePaymentsVM.SelectedExternalSalesEmployeeID = payment.ExtrnalSalesEmployeeId;
            externalEmployeePaymentsVM.ExtrnalSalesEmployee = payment.ExtrnalSalesEmployee;
            return externalEmployeePaymentsVM;
        }
        public static EmployeeSalesDetails UpdateSale(EmployeeSalesDetailsVM employeeSalesDetailsVM)
        {
            var db = new AccountingContainer();
            var employee = db.ExtrnalSalesEmployees.Find(employeeSalesDetailsVM.SelectedExtrnalEmployeeID);
            if (employee == null)
            {
                return null;
            }

            EmployeeSalesDetails employeeSalesDetails = db.EmployeeSalesDetails.Find(employeeSalesDetailsVM.Id) ;
            employeeSalesDetails.Amount = employeeSalesDetailsVM.Amount;
            employeeSalesDetails.Comment = employeeSalesDetailsVM.Comment;
            employeeSalesDetails.CommissionAmount = employeeSalesDetailsVM.CommissionAmount;
            employeeSalesDetails.ParentCommissionAmount = employeeSalesDetailsVM.ParentCommissionAmount;            
            if (employeeSalesDetailsVM.SelectedCustomerID > 0)
            {
                var custommer = db.Customers.Find(employeeSalesDetailsVM.SelectedCustomerID);
                employeeSalesDetails.CustommerID = custommer.Customer_Id;
                employeeSalesDetails.CustommerName = custommer.Customer_Name;
                employeeSalesDetails.CustommerMobileNumber = custommer.Customer_Phone;
                employeeSalesDetails.CustommerEmail = custommer.Customer_Mail;
            }
            if (!string.IsNullOrEmpty(employeeSalesDetailsVM.CustomerName))
            {
                employeeSalesDetails.CustommerName = employeeSalesDetailsVM.CustomerName;
            }
            if (!string.IsNullOrEmpty(employeeSalesDetailsVM.CustomerEmail))
            {
                employeeSalesDetails.CustommerEmail = employeeSalesDetailsVM.CustomerEmail;
            }
            if (!string.IsNullOrEmpty(employeeSalesDetailsVM.CustomerMobileNumber))
            {
                employeeSalesDetails.CustommerMobileNumber = employeeSalesDetailsVM.CustomerMobileNumber;
            }
            employeeSalesDetails.ExtrnalSalesEmployeeId = employee.Id;
            employeeSalesDetails.ExtrnalSalesEmployee = employee;
           
            db.Entry(employeeSalesDetails).State = EntityState.Modified;
            db.SaveChanges();
            employee.TotalAmountBalance = employee.EmployeeSalesDetails.Sum(e=> e.Amount);
            employee.TotalCommissionAmount = employee.EmployeeSalesDetails.Sum(e => e.CommissionAmount);
            employee.TotalCustommersCount = employee.EmployeeSalesDetails.Count();
            if (employee.ParentID > 0 && employee.Id != employee.ParentID)
            {
                var parent = db.ExtrnalSalesEmployees.Find(employee.ParentID);
                var allEmployees = db.ExtrnalSalesEmployees.Where(e => e.ParentID == parent.Id && e.Id != parent.Id);
                if (allEmployees.Any())
                {
                    parent.TotalCommissionAmountFromChild = 0;
                    parent.TotalChildsSalesAmountBalance = 0;
                    parent.TotalChildsSalesCount = 0;
                    foreach (var item in allEmployees)
                    {
                        parent.TotalCommissionAmountFromChild += item.EmployeeSalesDetails.Sum(e => e.ParentCommissionAmount);
                        parent.TotalChildsSalesAmountBalance += item.EmployeeSalesDetails.Sum(e => e.Amount);
                        parent.TotalChildsSalesCount += item.EmployeeSalesDetails.Count(); 
                    }
                    db.Entry(parent).State = EntityState.Modified;
                    db.SaveChanges();
                }          
            }
            db.Entry(employee).State = EntityState.Modified;
            db.SaveChanges();
            return employeeSalesDetails;
        }
        public static ExternalEmployeePayments UpdatePayment(ExternalEmployeePaymentsVM externalEmployeePaymentsVM)
        {
            var db = new AccountingContainer();
            var employee = db.ExtrnalSalesEmployees.Find(externalEmployeePaymentsVM.SelectedExternalSalesEmployeeID);
            if (employee == null)
            {
                return null;
            }
            ExternalEmployeePayments _externalEmployeePayments = db.ExternalEmployeePayments.Find(externalEmployeePaymentsVM.Id);
            if (_externalEmployeePayments == null)
            {
                return null;
            }
            _externalEmployeePayments.PaymentAmount = externalEmployeePaymentsVM.PaymentAmount;            
            _externalEmployeePayments.Comment = externalEmployeePaymentsVM.Comment;
            _externalEmployeePayments.ExtrnalSalesEmployee = employee;
            _externalEmployeePayments.ExtrnalSalesEmployeeId = employee.Id;

            db.Entry(_externalEmployeePayments).State = EntityState.Modified;
            db.SaveChanges();
            employee.TotalReceivedAmount = employee.ExternalEmployeePayments.Sum(e => e.PaymentAmount);
            db.Entry(employee).State = EntityState.Modified;
            db.SaveChanges();
            return _externalEmployeePayments;
        }
        public static ExtrnalSalesEmployee AddPaymentToEmployee(ExternalEmployeePaymentsVM externalEmployeePaymentsVM)
        {
            var db = new AccountingContainer();
            var employee = db.ExtrnalSalesEmployees.Find(externalEmployeePaymentsVM.SelectedExternalSalesEmployeeID);
            if (employee == null)
            {
                return null;
            }
            ExternalEmployeePayments _externalEmployeePayments = new ExternalEmployeePayments();
            _externalEmployeePayments.PaymentAmount = externalEmployeePaymentsVM.PaymentAmount;
            _externalEmployeePayments.PaymentDate =  DateTime.Now;
            _externalEmployeePayments.Comment = externalEmployeePaymentsVM.Comment;
            employee.ExternalEmployeePayments.Add(_externalEmployeePayments);
           
            db.Entry(employee).State = EntityState.Modified;
            db.SaveChanges();
            employee.TotalReceivedAmount = employee.ExternalEmployeePayments.Sum(e=> e.PaymentAmount);
            db.Entry(employee).State = EntityState.Modified;
            db.SaveChanges();
            return employee;
        }
        public static ExtrnalSalesEmployee RegisterFirstFase(ExtrnalSalesEmployeeVM extrnalSalesEmployeeVM)
        {
            try
            {
                var db = new AccountingContainer();
                ExtrnalSalesEmployee extrnalSalesEmployee = new ExtrnalSalesEmployee();
                extrnalSalesEmployee.Name = extrnalSalesEmployeeVM.Name;
                extrnalSalesEmployee.Email = extrnalSalesEmployeeVM.Email;
                extrnalSalesEmployee.City = extrnalSalesEmployeeVM.City;
                extrnalSalesEmployee.MobileNumber = extrnalSalesEmployeeVM.MobileNumber;
                extrnalSalesEmployee.BankName = extrnalSalesEmployeeVM.BankName;
                extrnalSalesEmployee.BankAccountNumber = extrnalSalesEmployeeVM.BankAccountNumber;
                extrnalSalesEmployee.IBAN = extrnalSalesEmployeeVM.IBAN;
                extrnalSalesEmployee.PromoCode = extrnalSalesEmployeeVM.PromoCode;
                extrnalSalesEmployee.RegistedDate = DateTime.Now;
                extrnalSalesEmployee.TotalAmountBalance = 0;
                extrnalSalesEmployee.TotalCustommersCount = 0;
                extrnalSalesEmployee.TotalReceivedAmount = 0;
                extrnalSalesEmployee.TotalCommissionAmount = 0; 
                extrnalSalesEmployee.TotalCommissionAmountFromChild = 0;
                extrnalSalesEmployee.TotalChildsSalesAmountBalance = 0;
                extrnalSalesEmployee.TotalChildsSalesCount = 0;
                extrnalSalesEmployee.EmailConfirmed = false;                            
                db.ExtrnalSalesEmployees.Add(extrnalSalesEmployee);
                db.SaveChanges();
                return extrnalSalesEmployee;

            }
            catch (Exception)
            {

                return null;
            }
           
        }
        public static bool CheckIfPromoCodeExist(string promoCode)
        {
            try
            {
                var db = new AccountingContainer();
                var extrnalSalesEmployees = db.ExtrnalSalesEmployees.Where(p => p.PromoCode == promoCode).ToList();
                if (extrnalSalesEmployees.Any())
                {
                    return true;
                }
                return false;

            }
            catch (Exception)
            {

                return false;
            }

        }
        public static bool CheckIfEmailExist(string email)
        {
            try
            {
                var db = new AccountingContainer();
                var extrnalSalesEmployees = db.ExtrnalSalesEmployees.Where(e => e.Email == email).ToList();
                if (extrnalSalesEmployees.Any())
                {
                    return true;
                }
                return false;

            }
            catch (Exception)
            {

                return false;
            }

        }
        public static bool CheckIfParentPromoCodeExist(string parentPromoCode)
        {
            try
            {
                var db = new AccountingContainer();
                var extrnalSalesEmployees = db.ExtrnalSalesEmployees.Where(p => p.PromoCode == parentPromoCode).ToList();
                if (extrnalSalesEmployees.Any())
                {
                    return true;
                }
                return false;

            }
            catch (Exception)
            {
                return false;
            }

        }
        public static ExtrnalSalesEmployeeVM PrepareVMToCompleetRegistration(int id)
        {
            var db = new AccountingContainer();
            var model = db.ExtrnalSalesEmployees.Find(id);
            if (model != null)
            {
                ExtrnalSalesEmployeeVM extrnalSalesEmployeeVM = new ExtrnalSalesEmployeeVM();
                extrnalSalesEmployeeVM.Id = model.Id;
                extrnalSalesEmployeeVM.Name = model.Name;
                extrnalSalesEmployeeVM.Email = model.Email;
                extrnalSalesEmployeeVM.MobileNumber = model.MobileNumber;
                extrnalSalesEmployeeVM.City = model.City;
                extrnalSalesEmployeeVM.BankName = model.BankName;
                extrnalSalesEmployeeVM.BankAccountNumber = model.BankAccountNumber;
                extrnalSalesEmployeeVM.IBAN = model.IBAN;
                extrnalSalesEmployeeVM.PromoCode = model.PromoCode;
                return extrnalSalesEmployeeVM;
            }
            return null;
        }
        public static ExtrnalSalesEmployeeVM CompleteEmployeeRegistration(ExtrnalSalesEmployeeVM extrnalSalesEmployeeVM)
        {
            var db = new AccountingContainer();
            var model = db.ExtrnalSalesEmployees.Find(extrnalSalesEmployeeVM.Id);
            if (model != null)
            {
                model.Name = extrnalSalesEmployeeVM.Name;
                model.MobileNumber = extrnalSalesEmployeeVM.MobileNumber;
                model.City = extrnalSalesEmployeeVM.City;
                model.BankName = extrnalSalesEmployeeVM.BankName;
                model.BankAccountNumber = extrnalSalesEmployeeVM.BankAccountNumber;
                model.IBAN = extrnalSalesEmployeeVM.IBAN;                
                model.EmailConfirmed = true;
                if (extrnalSalesEmployeeVM.SelectedFoundAboutUs > 1)
                {
                    var parentList = db.ExtrnalSalesEmployees.Where(e=> e.PromoCode == extrnalSalesEmployeeVM.ParentPromoCode).ToList();
                    if (parentList.Any())
                    {
                        var parentModel = parentList.FirstOrDefault();
                        model.ParentPromoCode = parentModel.PromoCode;
                        model.ParentID = parentModel.Id;
                        model.ParentName = parentModel.Name;
                    }
                    else
                    {
                        model.ParentPromoCode = model.PromoCode;
                        model.ParentID = model.Id;
                        model.ParentName = model.Name;
                    }                   
                }
                else
                {
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    model.ParentPromoCode = model.PromoCode;
                    model.ParentID = model.Id;
                    model.ParentName = model.Name;

                }
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return extrnalSalesEmployeeVM;
            }
            return null;
        }
    }
}