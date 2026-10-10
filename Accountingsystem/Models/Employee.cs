using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using Microsoft.Office.Interop.Excel;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Accountingsystem.Models
{

    public partial class Employee
    {
        private ApplicationUserManager _userManager;

        //GET details
        /// <summary>
        /// return 1 Employee specified by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Emp</returns>
        /// 
        public static bool containCode(string Emp_Code)
        {
            var db = new AccountingContainer();
            var allinData = db.Employees.ToList();
            foreach (var item in allinData)
            {
                if (item.Emp_Code == Emp_Code || !Emp_Code.StartsWith("EMP-"))
                {
                    return true;
                }
                else
                {

                }

            }
            return false;
        }
        /// <summary>
        /// get that Employee By its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Employee getEmpById(int id)
        {
            var db = new AccountingContainer();
            var Emp = db.Employees.Find(id);
            return Emp;
        }
        /// <summary>
        /// get the file belong to Employee by Id
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public static EmployeeFileDetail getFileById(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.EmployeeFileDetails.Find(fileId);
            return file;
        }
        public static int deleteFile(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.EmployeeFileDetails.Find(fileId);
            db.EmployeeFileDetails.Remove(file);
            db.SaveChanges();
            return file.EmployeeEmp_Id;
        }
        public static int deleteImage(int Emp_Id)
        {
            var db = new AccountingContainer();
            var employee = db.Employees.Find(Emp_Id);
            employee.Emp_Image = null;
            db.Entry(employee).State = EntityState.Modified;
            db.SaveChanges();
         
            return Emp_Id;
        }
        /// <summary>
        /// get all file that belong that Employee by using his Id
        /// </summary>
        /// <param name="empId"></param>
        /// <returns>List<EmployeeFileDetail></returns>
        public static List<EmployeeFileDetail> GetAllFiles(int empId)
        {
            var db = new AccountingContainer();
            var files = db.EmployeeFileDetails.Where(emp => emp.EmployeeEmp_Id == empId).ToList();
            return files;
        }
        /// <summary>
        /// get VM of salary Compnent to make user create 
        /// </summary>
        /// <returns>Salary_ComponentsVM</returns>
        public static Salary_ComponentsVM prepareSalaryComponant()
        {
            var db = new AccountingContainer();
            var salaryVM = new Salary_ComponentsVM();
           
            salaryVM.AccountList = new SelectList(db.TypeOfDetailSalaries.Where(a=>a.ComeFromVacation!=true), "Id", "SalaryDetail_Name");


            return salaryVM;


        }
        /// <summary>
        /// Get All Employee that Status is Active
        /// </summary>
        /// <returns>List<Employee></returns>
        public static List<Employee> getEmployees()
        {
            var db = new AccountingContainer();
            var Employees = db.Employees.Where(a=>a.Active==true).ToList();
            foreach (var emp in Employees)
            {
                var thisteam = db.EmployeeTeams.Find(emp.Team_ID);
                if (thisteam == null)
                { }
                else
                {
                    emp.Team_Name = thisteam.Team_Name;
                    db.Entry(emp).State = EntityState.Modified;
                    db.SaveChanges();
                  
                }

            }
            return Employees;
        }
        /// <summary>
        /// Get All Employee that Status is Not Active
        /// </summary>
        /// <returns>List<Employee></returns>
        public static List<Employee> getEmployeesNotActive()
        {
            var db = new AccountingContainer();
            var EmployeeNotActive = db.Employees.Where(a => a.Active == false).ToList();
            return EmployeeNotActive;


        }
        public static List<Employee> getEmployeesALL()
        {
            var db = new AccountingContainer();
            var EmployeeNotActive = db.Employees.ToList();
            return EmployeeNotActive;


        }

        /// <summary>
        /// change status of Employee by using its id
        /// </summary>
        /// <param name="Emp_Id"></param>
        /// <returns>bool</returns>
        public static bool ChangeStatusOfEmployeeToActiveOrNot(int Emp_Id)
        {
            var db = new AccountingContainer();
            var Emp = db.Employees.Find(Emp_Id);
            var account = db.Accounts.Where(a => a.Account_Code == Emp.Emp_Code).FirstOrDefault();
            if (Emp.Active==true) {

                Emp.Active = false;
                account.OpenBalance_Code = "1";
                db.Entry(account).State = EntityState.Modified;
                db.Entry(Emp).State = EntityState.Modified;
               
                db.SaveChanges();
                db.Dispose();
                return true;
            }
            else
            {
                Emp.Active = true;
                account.OpenBalance_Code = null;
                db.Entry(account).State = EntityState.Modified;
                db.Entry(Emp).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                return false;
            }
            
        }
        //GET Create
        /// <summary>
        /// Prepare form to make Create for Employee and make unique code for that Employee And prepare Temp to create Employee
        /// </summary>
        /// <returns></returns>
        public static EmpVM getViewForCreation()
        {
            var db = new AccountingContainer();
            var empVM = new EmpVM();

            empVM.BranchSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.isActive != false && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
            empVM.DepartSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.isActive != false && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
            empVM.BussinessList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.isActive != false && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            empVM.SubledgerList = new SelectList(db.SubLedgers.Where(a =>  a.isActive != false&&(a.SubLed_Type == "مركز تكلفة يدوى" || a.SubLed_Type == "مشروع")), "SubLed_Id", "SubLed_Name");
            empVM.compList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");

            var lastSerial = db.Employees.OrderByDescending(c => c.Emp_Id).FirstOrDefault();
            if (lastSerial == null)
            {
                empVM.Emp_Code = "EMP-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                empVM.Emp_Code = "EMP-" + (Convert.ToInt32(lastSerial.Emp_Code.Substring(4, lastSerial.Emp_Code.Length - 4)) + 1).ToString("D3");
            }
            empVM.Emp_Subledger = false;
            //var lista = new List<ChartOfAccounting> { };
            //foreach (var k in db.ChartOfAccountings)
            //{
            //    var parent = db.ChartOfAccountings.Where(a => a.ChartOfAccountingAccount_Id == k.Account_Id);
            //    var o = parent.FirstOrDefault();
            //    if (o == null)
            //    {
            //        lista.Add(db.ChartOfAccountings.Find(k.Account_Id));

            //    }


            //}
            //empVM.MyChartOfAccountList = new SelectList(lista.Where(accCode => accCode.Account_Code.StartsWith("05")), "Account_Id", "Account_Name");

            var EmpTemp = new EmployeTemp();
            var id = (from s in db.EmployeTemps
                      select s).ToList().LastOrDefault();

        
            if (id != null)
            {
             
              
                EmpTemp.EmpTemp_Id =id.EmpTemp_Id+1;
                db.EmployeTemps.Add(EmpTemp);

                
                empVM.Emp_Id = (int)EmpTemp.EmpTemp_Id;
            }
            else
            {
                

                    EmpTemp.EmpTemp_Id = 1;
                }
              
               
           
                db.EmployeTemps.Add(EmpTemp);

                db.SaveChanges();
                empVM.Emp_Id = (int)EmpTemp.EmpTemp_Id;
         




            return empVM;
        }
        /// <summary>
        /// add To Temp Salary Commponent  and Check Validation
        /// </summary>
        /// <param name="salVM"></param>
        /// <returns>string</returns>
        public static string addSalaryDetailTemp(Salary_ComponentsVM salVM)
        {
            if (salVM.Account_Id == 0 || salVM.Account_Id == null)
            {
                return Resources.validate.chooseComponentOfSalary;
            }
            if (salVM.Account_Amount == 0 || salVM.Account_Amount == null)
            {
                return Resources.validate.EnterAmount;
            }
            var db = new AccountingContainer();
            var EmpTemp = db.EmployeTemps.Find(salVM.Employee_Id);

            var details = EmpTemp.Salary_ComponentsTemp;
            var duplecatTemp = details.Where(s => s.EmployeeTemp_Id == EmpTemp.EmpTemp_Id && s.Account_Id == salVM.Account_Id).ToList();
            if (duplecatTemp.FirstOrDefault() != null)
            {
                return Resources.validate.thisCompnentIsExist;
            }
            else
            {
                if (salVM.Employee_Id == 0)
                {
                    var k = db.Salary_Components.Find(salVM.Salary_Components_Id).Employee_Id;
                    EmpTemp = db.EmployeTemps.Find(k);

                }
                else
                {
                    EmpTemp = db.EmployeTemps.Find(salVM.Employee_Id);
                }
                var salaryTemp = new Salary_ComponentsTemp();
                salaryTemp.Account_Id = salVM.Account_Id;
                salaryTemp.Account_Name = db.TypeOfDetailSalaries.Find(salVM.Account_Id).SalaryDetail_Name;
                salaryTemp.Account_Amount = salVM.Account_Amount;
                salaryTemp.EmployeeTemp_Id = salVM.Employee_Id;
                EmpTemp.Salary_ComponentsTemp.Add(salaryTemp);
                db.Entry(EmpTemp).State = EntityState.Modified;
                db.SaveChanges();
                return Resources.validate.SuccessfullAdd;
            }
            
        }

        /// <summary>
        /// get all Salary Compnent by using master Id
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns>List<Salary_ComponentsTemp></returns>
        public static List<Salary_ComponentsTemp> GetAllTempSalaryDetails(int masterVmId)
        {
            var db = new AccountingContainer();

            var SalaryDetailsTemp = db.Salary_ComponentsTemp.Where(invoice => invoice.EmployeeTemp_Id == masterVmId).ToList();
            return SalaryDetailsTemp;
        }
        /// <summary>
        /// Take data from user and Post that data to database and check the data isvalued or not and 
        /// Create Subledger for Employee If that check on it and add Salary Component to that Empolyee 
        /// </summary>
        /// <param name="empVM"></param>
        /// <returns>bool</returns>
        public static int CreateEmployee(EmpVM empVM)
        {
            var db = new AccountingContainer();
            var employee = new Employee();
            var EmpTemp = db.EmployeTemps.Find(empVM.Emp_Id);
            Decimal allTotalIncreaseSalary = 0;
            decimal totalDecreaseSalary = 0;
            employee.Vacations_Number = 0;
            if (empVM.SystemRole_ID != 0)
            {
                employee.SystemRoleId = empVM.SystemRole_ID;
            }
            try
            {
                var client = db.Clients.Where(a => a.Package != null).FirstOrDefault();
                if (client!=null)
                {
                    employee.Client_Id = client.Id;
                }
                employee.Emp_Name = empVM.Emp_Name;
                employee.Emp_Mail = empVM.Emp_Mail;
                employee.Emp_Title = empVM.Emp_Title;
                employee.Emp_Phone = empVM.Emp_Phone;
                employee.Emp_Address = empVM.Emp_Address;
                employee.Emp_DOB = empVM.Emp_DOB;
                employee.Residency_number = empVM.Residency_number;
                employee.Passport_number = empVM.Passport_number;
                employee.Start_Date_Passp = empVM.Start_Date_Passp;
                employee.End_Date_Passp = empVM.End_Date_Passp;
                employee.Start_Date_Residency = empVM.Start_Date_Residency;
                employee.End_Date_Residency = empVM.End_Date_Residency;
                employee.ViewAllCustomer = empVM.ViewAllCustomer;
                //employee.Vacations_Number = empVM.vacation_Number;
                employee.Fixed_Salary = empVM.fixed_sallary;
                employee.Reg_ID = empVM.Regist_Id;
              
                //if (db.Companies.FirstOrDefault()==null)
                //{
                //    return 0;
                //}
                employee.Status =Models.Status.صرف;
                var lastSerial = db.Employees.OrderByDescending(c => c.Emp_Id).FirstOrDefault();
                if (lastSerial == null)
                {
                    employee.Emp_Code = "EMP-001";
                }
                else
                {
                    //using string substring method to get the number of the last inserted
                    employee.Emp_Code = "EMP-" + (Convert.ToInt32(lastSerial.Emp_Code.Substring(4, lastSerial.Emp_Code.Length - 4)) + 1).ToString("D3");
                }

                
                employee.BranchSubledger_Id = empVM.BranchSubledger_Id;
                employee.DepartSubledger_Id= empVM.DepartSubledger_Id;
                employee.BussinesSubledger_Id= empVM.BussinesSubledger_Id;
                employee.User = empVM.User;
                
                employee.Company_ID = empVM.Company_ID;
                employee.Social_status = empVM.Social_status;
                employee.Nationality = empVM.Nationality;
                employee.Contract_Begin = empVM.Contract_Begin;
                employee.Contract_End = empVM.Contract_End;
                employee.Work_Actual_Start = empVM.Work_Actual_Start;
                employee.Type_OF_Work = empVM.Type_OF_Work;
                employee.Social_insurance_number = empVM.Social_insurance_number;
                employee.Medical_Insurance_Number = empVM.Medical_Insurance_Number;
                employee.End_medical_Insurance = empVM.End_medical_Insurance;


                employee.BankCode = empVM.BankCode;
                employee.BankName = empVM.BankName;
                employee.EbanNumb = empVM.EbanNumb;
                employee.AccountNum = empVM.AccountNum;



                employee.Sex = empVM.Sex;
                if (empVM.Emp_Image != null)
                {
                    //var data = new byte[catgorVM.Cat_Img.];
                    //Categr.Cat_Img = data;
                    var length = empVM.Emp_Image.InputStream.Length; //Length: 103050706
                    MemoryStream target = new MemoryStream();
                    empVM.Emp_Image.InputStream.CopyTo(target); // generates problem in this line
                    byte[] data = target.ToArray();
                    employee.Emp_Image = data;

                }
                //employee.Vacations_Number = empVM.vacation_Number;
                employee.Vacation_Pervious = empVM.Vacation_Pervious;
                //employee.TotalVacation = employee.Vacations_Number+ employee.Vacation_Pervious;


                if (empVM.Subledger_Id != null && empVM.Subledger_Id != 0)
                {

                    employee.Subldger_Id = empVM.Subledger_Id;

                }
                else if (empVM.BussinesSubledger_Id != null && empVM.BussinesSubledger_Id != 0)
                {

                    employee.Subldger_Id = empVM.BussinesSubledger_Id;

                }
                else if (empVM.DepartSubledger_Id != null&& empVM.DepartSubledger_Id !=0)
                {

                    employee.Subldger_Id = empVM.DepartSubledger_Id;

                }
                else if (empVM.BranchSubledger_Id != null)
                {

                    employee.Subldger_Id = empVM.BranchSubledger_Id;

                }
                else { employee.Subldger_Id = 1; }
                if (empVM.Files != null)
                {
                    if (empVM.Files.FirstOrDefault() != null)
                    {

                        foreach (var file in empVM.Files)
                        {
                            var filedetails = new EmployeeFileDetail();
                            var data = new byte[file.ContentLength];
                            filedetails.FileName = file.FileName;
                            filedetails.ContentLength = file.ContentLength;
                            file.InputStream.Read(data, 0, file.ContentLength);
                            filedetails.ProImg = data;
                            filedetails.EmployeeEmp_Id = employee.Emp_Id;
                            if (data.Length < 1000000)
                            {
                                employee.EmployeeFileDetails.Add(filedetails);
                            }
                            
                            
                        }
                    }
                }

                if (EmpTemp != null)
                {
                    foreach (var item in EmpTemp.Salary_ComponentsTemp.ToList())
                    {
                        var salary = new Salary_Components();
                        salary.Account_Amount = item.Account_Amount;
                        salary.Account_Id = item.Account_Id;
                        salary.Account_Name = item.Account_Name;

                        employee.Salary_Components.Add(salary);
                        db.Salary_ComponentsTemp.Remove(item);
                        var AccountType = db.TypeOfDetailSalaries.Find(item.Account_Id).Sal_Type;
                        if (AccountType == SalaryTypeValue.يزيد_من_قيمة_اجمالى_الراتب)
                        {
                            allTotalIncreaseSalary += item.Account_Amount;

                        }
                        else { totalDecreaseSalary += item.Account_Amount; }



                    }
                }
                if (EmpTemp != null)
                {
                    foreach (var item in EmpTemp.EmpVacationTemps.ToList())
                    {
                        var vacation = new EmpVacation();
                        vacation.Vacation_TypesId = item.Vacation_TypesId;
                        vacation.DaysInyear = item.DaysInyear;
                        vacation.TotalAfterAbsent = vacation.DaysInyear;

                        employee.EmpVacations.Add(vacation);
                        db.EmpVacationTemps.Remove(item);

                        employee.Vacations_Number += vacation.DaysInyear;



                    }
                }
                if (empVM.Emp_Subledger == true)
                {
                    var sub = new SubLedgerVM();
                    sub.ParentSubledger_Id = employee.Subldger_Id;
                    sub.SubLed_Name = empVM.Emp_Name;
                    sub.SubLed_Date = Convert.ToString(DateTime.Now);
                    sub.SubLed_Type = "مركز تكلفة يدوى";
                   var Sub= SubLedger.createSubLedger(sub);
                    employee.Subldger_Id = Sub.SubLed_Id;
                }
                employee.Total_Salary = allTotalIncreaseSalary- totalDecreaseSalary;
                employee.TotalSalaryBeforPenality = employee.Total_Salary;
                employee.Emp_SubLedger = empVM.Emp_Subledger;
                var ChartOfAccount = db.ChartOfAccountings.Find(39);
                var ChartOfAccountParent = db.ChartOfAccountings.Find(ChartOfAccount.ChartOfAccountingAccount_Id);

                var accounts = new Accounts();
                accounts.Account_Id = ChartOfAccount.Account_Id.ToString();
                accounts.Account_Code = employee.Emp_Code;
                var date = DateTime.Now.ToString();
                accounts.Account_Date = DateTime.Now.Date;
                accounts.Account_Name = employee.Emp_Name;
                accounts.Account_Type = ChartOfAccount.Account_Name;
                accounts.Account_Desc = ChartOfAccountParent.Account_Name;
                accounts.Account_Balance = 0;
                employee.Active = true;
                db.Employees.Add(employee);
                if (EmpTemp != null)
                {
                    db.EmployeTemps.Remove(EmpTemp);
                }
                db.Accounts.Add(accounts);
                db.SaveChanges();
                db.Dispose();
                return employee.Emp_Id;

            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                var result = ex.Data;
                var tt = result;
                throw;
            }
        }
        /// <summary>
        ///Take data from data base and but it in vm to make user edit and get salary Component for that saved in Data base
        /// </summary>
        /// <param name="id"></param>
        /// <returns>EmployeeVM</returns>
        public static EmpVM FillViewForEditting(int id)
        {
            var db = new AccountingContainer();
            var employee = getEmpById(id);
            var empVM = new EmpVM();
            if (employee.ViewAllCustomer == true)
            {
                empVM.ViewAllCustomer = true;
            }
            else
            {
                empVM.ViewAllCustomer = false;
            }
          
            var lista = new List<ChartOfAccounting> { };
            foreach (var k in db.ChartOfAccountings.Where(a=>a.isActive!=false))
            {
                var parent = db.ChartOfAccountings.Where(a => a.ChartOfAccountingAccount_Id == k.Account_Id);
                var o = parent.FirstOrDefault();
                if (o == null)
                {
                    lista.Add(db.ChartOfAccountings.Find(k.Account_Id));

                }
            }

            empVM.Emp_Id = employee.Emp_Id;
            empVM.Emp_Name = employee.Emp_Name;
            empVM.Emp_Code = employee.Emp_Code;
            empVM.Emp_Mail = employee.Emp_Mail;
            empVM.Emp_Title = employee.Emp_Title;
            empVM.Emp_Phone = employee.Emp_Phone;
            empVM.Emp_Address = employee.Emp_Address;
            empVM.Emp_DOB = employee.Emp_DOB;
            empVM.Residency_number = employee.Residency_number;
            empVM.Passport_number = employee.Passport_number;
            empVM.BankCode = employee.BankCode;
            empVM.BankName = employee.BankName;
            empVM.EbanNumb = employee.EbanNumb;
            empVM.AccountNum = employee.AccountNum;
            if (empVM.Start_Date_Passp != null)
            {
                empVM.Start_Date_Passp = Convert.ToString(employee.Start_Date_Passp);
            }
            if (empVM.End_Date_Passp != null)
            {
                empVM.End_Date_Passp = Convert.ToString(employee.End_Date_Passp);
            }

            if (empVM.Start_Date_Residency != null)
            {
                empVM.Start_Date_Residency = Convert.ToString(employee.Start_Date_Residency);
            }
            if (empVM.End_Date_Residency != null)
            {
                empVM.End_Date_Residency = Convert.ToString(employee.End_Date_Residency);
            }

            //if (empVM.vacation_Number != 0)
            //{
            //    empVM.vacation_Number = employee.Vacations_Number.Value;
            //}
            empVM.fixed_sallary = employee.Fixed_Salary;
            empVM.Active = employee.Active;
            empVM.BranchSubledger_Id = employee.BranchSubledger_Id;
            empVM.DepartSubledger_Id = employee.DepartSubledger_Id;
            empVM.BussinesSubledger_Id = employee.BussinesSubledger_Id; 
            empVM.User =(bool) employee.User;
           


            empVM.Company_ID = employee.Company_ID.Value;
            empVM.Social_status = employee.Social_status;
            empVM.Nationality = employee.Nationality;

            if (empVM.Contract_Begin != null)
            {
                empVM.Contract_Begin = employee.Contract_Begin.ToString();
            }
            if (empVM.Contract_End != null)
            { empVM.Contract_End = employee.Contract_End.ToString(); }

            if (empVM.Work_Actual_Start != null)
            { empVM.Work_Actual_Start = employee.Work_Actual_Start.ToString(); }
            if (empVM.End_medical_Insurance != null)
            { empVM.End_medical_Insurance = employee.End_medical_Insurance.ToString(); }
            empVM.Type_OF_Work = employee.Type_OF_Work;
            empVM.Social_insurance_number = employee.Social_insurance_number;
            empVM.Medical_Insurance_Number = employee.Medical_Insurance_Number;
            empVM.Start_Date_Passp = employee.Start_Date_Passp;
            empVM.End_Date_Passp = employee.End_Date_Passp;
            empVM.Start_Date_Residency = employee.Start_Date_Residency;
            empVM.End_Date_Residency = employee.End_Date_Residency;
            //empVM.vacation_Number =(int) employee.Vacations_Number;
            empVM.Emp_Subledger =(bool) employee.Emp_SubLedger;
            empVM.Contract_Begin = employee.Contract_Begin;
            empVM.Contract_End = employee.Contract_End;
            empVM.Work_Actual_Start = employee.Work_Actual_Start;
            empVM.User = employee.User;
            empVM.End_medical_Insurance = employee.End_medical_Insurance;

            empVM.Sex = employee.Sex;
            empVM.Emp_Subledger =(bool) employee.Emp_SubLedger;

            empVM.BranchSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.isActive != false && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
            if (empVM.BranchSubledger_Id > 0)
            {
                empVM.DepartSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.isActive != false && a.SubLed_Type == "قسم لفرع"&&a.SubLedgerSubLed_Id== empVM.BranchSubledger_Id), "SubLed_Id", "SubLed_Name");
            }
            else
            {
                empVM.DepartSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.isActive != false && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
            }
            if (empVM.DepartSubledger_Id > 0)
            {
                empVM.BussinessList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.isActive != false && a.SubLed_Type == "نشاط لقسم"&&a.SubLedgerSubLed_Id== empVM.DepartSubledger_Id), "SubLed_Id", "SubLed_Name");
            }
            else
            {
                empVM.BussinessList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.isActive != false && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            }

            
            empVM.SubledgerList = new SelectList(db.SubLedgers.Where(a => a.isActive != false&&(a.SubLed_Type == "مركز تكلفة يدوى"||a.SubLed_Type=="مشروع")), "SubLed_Id", "SubLed_Name");
            empVM.compList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            var EmpTemp = new EmployeTemp();
            var EmpTempId = db.EmployeTemps.Find(employee.Emp_Id);           

            if (EmpTempId == null)
            {
                EmpTemp.EmpTemp_Id = employee.Emp_Id;
                db.EmployeTemps.Add(EmpTemp);
            }
         
            db.SaveChanges();

            return empVM;
        }
        public static EmpVM FillViewForEdittingForAdmin(string id)
        {
            var db = new AccountingContainer();
            var employee = db.Employees.Where(a=>a.Emp_Code=="EMP-001").FirstOrDefault();
            var empVM = new EmpVM();
            if (employee.ViewAllCustomer == true)
            {
                empVM.ViewAllCustomer = true;
            }
            else
            {
                empVM.ViewAllCustomer = false;
            }
            empVM.Emp_Id = employee.Emp_Id;
            var lista = new List<ChartOfAccounting> { };
            foreach (var k in db.ChartOfAccountings.Where(a=>a.isActive!=false))
            {
                var parent = db.ChartOfAccountings.Where(a => a.ChartOfAccountingAccount_Id == k.Account_Id);
                var o = parent.FirstOrDefault();
                if (o == null)
                {
                    lista.Add(db.ChartOfAccountings.Find(k.Account_Id));

                }
            }

            empVM.Emp_Id = employee.Emp_Id;
            empVM.Emp_Name = employee.Emp_Name;
            empVM.Emp_Code = employee.Emp_Code;
            empVM.Emp_Mail = employee.Emp_Mail;
            empVM.Emp_Title = employee.Emp_Title;
            empVM.Emp_Phone = employee.Emp_Phone;
            empVM.Emp_Address = employee.Emp_Address;
            empVM.Emp_DOB = employee.Emp_DOB;
            empVM.Residency_number = employee.Residency_number;
            empVM.Passport_number = employee.Passport_number;
            empVM.BankCode = employee.BankCode;
            empVM.BankName = employee.BankName;
            empVM.EbanNumb = employee.EbanNumb;
            empVM.AccountNum = employee.AccountNum;
            if (empVM.Start_Date_Passp != null)
            {
                empVM.Start_Date_Passp = Convert.ToString(employee.Start_Date_Passp);
            }
            if (empVM.End_Date_Passp != null)
            {
                empVM.End_Date_Passp = Convert.ToString(employee.End_Date_Passp);
            }

            if (empVM.Start_Date_Residency != null)
            {
                empVM.Start_Date_Residency = Convert.ToString(employee.Start_Date_Residency);
            }
            if (empVM.End_Date_Residency != null)
            {
                empVM.End_Date_Residency = Convert.ToString(employee.End_Date_Residency);
            }

            //if (empVM.vacation_Number != 0)
            //{
            //    empVM.vacation_Number = employee.Vacations_Number.Value;
            //}
            empVM.fixed_sallary = employee.Fixed_Salary;
            empVM.Active = employee.Active;
            empVM.BranchSubledger_Id = employee.BranchSubledger_Id;
            empVM.DepartSubledger_Id = employee.DepartSubledger_Id;
            empVM.BussinesSubledger_Id = employee.BussinesSubledger_Id;
            empVM.User = (bool)employee.User;



            empVM.Company_ID = employee.Company_ID.Value;
            empVM.Social_status = employee.Social_status;
            empVM.Nationality = employee.Nationality;

            if (empVM.Contract_Begin != null)
            {
                empVM.Contract_Begin = employee.Contract_Begin.ToString();
            }
            if (empVM.Contract_End != null)
            { empVM.Contract_End = employee.Contract_End.ToString(); }

            if (empVM.Work_Actual_Start != null)
            { empVM.Work_Actual_Start = employee.Work_Actual_Start.ToString(); }
            if (empVM.End_medical_Insurance != null)
            { empVM.End_medical_Insurance = employee.End_medical_Insurance.ToString(); }
            empVM.Type_OF_Work = employee.Type_OF_Work;
            empVM.Social_insurance_number = employee.Social_insurance_number;
            empVM.Medical_Insurance_Number = employee.Medical_Insurance_Number;
            empVM.Start_Date_Passp = employee.Start_Date_Passp;
            empVM.End_Date_Passp = employee.End_Date_Passp;
            empVM.Start_Date_Residency = employee.Start_Date_Residency;
            empVM.End_Date_Residency = employee.End_Date_Residency;
            //empVM.vacation_Number =(int) employee.Vacations_Number;
            empVM.Emp_Subledger = (bool)employee.Emp_SubLedger;
            empVM.Contract_Begin = employee.Contract_Begin;
            empVM.Contract_End = employee.Contract_End;
            empVM.Work_Actual_Start = employee.Work_Actual_Start;
            empVM.User = employee.User;
            empVM.End_medical_Insurance = employee.End_medical_Insurance;

            empVM.Sex = employee.Sex;
            empVM.Emp_Subledger = (bool)employee.Emp_SubLedger;

            empVM.BranchSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.isActive != false && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
            if (empVM.BranchSubledger_Id > 0)
            {
                empVM.DepartSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.isActive != false && a.SubLed_Type == "قسم لفرع" && a.SubLedgerSubLed_Id == empVM.BranchSubledger_Id), "SubLed_Id", "SubLed_Name");
            }
            else
            {
                empVM.DepartSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.isActive != false && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
            }
            if (empVM.DepartSubledger_Id > 0)
            {
                empVM.BussinessList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.isActive != false && a.SubLed_Type == "نشاط لقسم" && a.SubLedgerSubLed_Id == empVM.DepartSubledger_Id), "SubLed_Id", "SubLed_Name");
            }
            else
            {
                empVM.BussinessList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.isActive != false && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            }


            empVM.SubledgerList = new SelectList(db.SubLedgers.Where(a =>  a.isActive != false&&(a.SubLed_Type == "مركز تكلفة يدوى" || a.SubLed_Type == "مشروع")), "SubLed_Id", "SubLed_Name");
            empVM.compList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            var EmpTemp = new EmployeTemp();
            var EmpTempId = db.EmployeTemps.Find(employee.Emp_Id);

            if (EmpTempId == null)
            {
                EmpTemp.EmpTemp_Id = employee.Emp_Id;
                db.EmployeTemps.Add(EmpTemp);
            }

            db.SaveChanges();

            return empVM;
        }
        /// <summary>
        /// prepare Component salary by the id of salary and get vM to change on it
        /// </summary>
        /// <param name="SalCom"></param>
        /// <returns></returns>
        public static Salary_ComponentsVM PrepareSalaryComponent(int SalCom)
        {
            var db = new AccountingContainer();
            var salComonent = new Salary_ComponentsVM();
            var ThisCompon = db.Salary_ComponentsTemp.Find(SalCom);
            salComonent.Account_Id = ThisCompon.Account_Id;
            salComonent.Account_Amount = ThisCompon.Account_Amount;
            salComonent.AccountList = new SelectList(db.TypeOfDetailSalaries.Where(a=>a.ComeFromVacation!=true), "Id", "SalaryDetail_Name");
            return salComonent;
        }

        /// <summary>
        /// get salary component by using id of detail of salary
        /// </summary>
        /// <param name="DetailEmp"></param>
        /// <returns></returns>
        public static Salary_ComponentsVM perpareSalaruEmp(int DetailEmp)
        {
            var db = new AccountingContainer();
            var EmpVm = new Salary_ComponentsVM();
            var EmpTemp = db.Salary_Components.Find(DetailEmp);
            EmpVm.Account_Id = EmpTemp.Account_Id;
            EmpVm.Account_Amount = EmpTemp.Account_Amount;                  
            EmpVm.AccountList = new SelectList(db.TypeOfDetailSalaries, "Id", "SalaryDetail_Name");
            return EmpVm;


        }
        /// <summary>
        /// that method delete all salary component and put thier in Salary component for making Edit 
        /// by Removing All Salary Component Temp and insert the Real Compnent in Temp
        /// </summary>
        /// <param name="EmpId"></param>
        /// <returns>List<Salary_ComponentsTemp> </returns>
        public static List<Salary_ComponentsTemp> GetallEmpDetailsTemp(int EmpId)
        {
            var db = new AccountingContainer();
            List<Salary_ComponentsTemp> ListTemp = new List<Salary_ComponentsTemp> { };
            var DetailsTemp = db.Salary_ComponentsTemp.Where(C => C.EmployeeTemp_Id == EmpId).ToList();
            if (DetailsTemp.FirstOrDefault() != null)
            {
                db.Salary_ComponentsTemp.RemoveRange(DetailsTemp);
                db.SaveChanges();
            }
            else { }  
                var salarycompnent = db.Salary_Components.Where(a => a.Employee_Id == EmpId);
               
                foreach (var item in salarycompnent)
                {
                    var tempComp = new Salary_ComponentsTemp();
                    tempComp.Account_Id = item.Account_Id;
                    tempComp.Account_Name = item.Account_Name;
                    tempComp.Account_Amount = item.Account_Amount;
                    tempComp.EmployeeTemp_Id = EmpId;
                    ListTemp.Add(tempComp);


                }
                db.Salary_ComponentsTemp.AddRange(ListTemp);
                db.SaveChanges();


           
            return ListTemp;
        }

        /// <summary>
        /// get All Sallary Component That Depend on That Employee 
        /// </summary>
        /// <param name="EmpId"></param>
        /// <returns>List<Salary_Components></returns>
        public static List<Salary_Components> GetAllEmpSalaryDetails(int EmpId)
        {
            var db = new AccountingContainer();

            var Details = db.Salary_Components.Where(invoice => invoice.Employee_Id == EmpId).ToList();
         
            return Details;
        }   
        /// <summary>
        /// Edit Employee and fill Employee and Recalculate Total Salary and put file is it Exisit
        /// </summary>
        /// <param name="empVM"></param>
        /// <returns>Employee</returns>
        public static Employee Edit(EmpVM empVM)
        {
            var db = new AccountingContainer();
            var employee = db.Employees.Find(empVM.Emp_Id);
            var subEmp = db.SubLedgers.Where(a => a.SubLed_Name == employee.Emp_Name).FirstOrDefault();
            var component = db.Salary_Components.Where(c => c.Employee_Id == employee.Emp_Id).ToList();
            var EmpTemp = db.EmployeTemps.Find(empVM.Emp_Id);
            if (subEmp != null) {

                subEmp.SubLed_Name = empVM.Emp_Name;

                db.Entry(subEmp).State = EntityState.Modified;
            }
            employee.ViewAllCustomer = empVM.ViewAllCustomer;
            if (employee != null)
            {
                if (empVM.Subledger_Id != null && empVM.Subledger_Id != 0)
                {

                    employee.Subldger_Id = empVM.Subledger_Id;

                }
                else if (empVM.BussinesSubledger_Id != null&& empVM.BussinesSubledger_Id!=0)
                {

                    employee.Subldger_Id = empVM.BussinesSubledger_Id;

                }
                else if (empVM.DepartSubledger_Id != null&& empVM.DepartSubledger_Id != 0)
                {

                    employee.Subldger_Id = empVM.DepartSubledger_Id;

                }
                else if (empVM.BranchSubledger_Id != null&& empVM.BranchSubledger_Id != 0)
                {

                    employee.Subldger_Id = empVM.BranchSubledger_Id;

                }
                else { employee.Subldger_Id = 1; }
                
                employee.Emp_Name = empVM.Emp_Name;
                employee.Emp_Code = employee.Emp_Code;
                employee.Emp_Mail = empVM.Emp_Mail;
                employee.Emp_Title = empVM.Emp_Title;
                employee.Emp_Phone = empVM.Emp_Phone;
                employee.Emp_Address = empVM.Emp_Address;
                employee.Emp_DOB = empVM.Emp_DOB;
                employee.Residency_number = empVM.Residency_number;
                employee.Passport_number = empVM.Passport_number;
                employee.Start_Date_Passp = empVM.Start_Date_Passp;
                employee.End_Date_Passp = empVM.End_Date_Passp;
                employee.Start_Date_Residency = empVM.Start_Date_Residency;
                employee.End_Date_Residency = empVM.End_Date_Residency;
                employee.Emp_SubLedger = empVM.Emp_Subledger;
                //employee.Vacations_Number = empVM.vacation_Number;

                employee.BankCode = empVM.BankCode;
                employee.BankName = empVM.BankName;
                employee.EbanNumb = empVM.EbanNumb;
                employee.AccountNum = empVM.AccountNum;


                employee.Fixed_Salary = empVM.fixed_sallary;
                employee.BranchSubledger_Id = empVM.BranchSubledger_Id;
                employee.DepartSubledger_Id = empVM.DepartSubledger_Id;
                employee.BussinesSubledger_Id = empVM.BussinesSubledger_Id;
                employee.User = empVM.User;
                employee.Active = empVM.Active;
                employee.Company_ID = empVM.Company_ID;
                employee.Social_status = empVM.Social_status;
                employee.Nationality = empVM.Nationality;
                employee.Contract_Begin = empVM.Contract_Begin;
                employee.Contract_End = empVM.Contract_End;
                employee.Work_Actual_Start = empVM.Work_Actual_Start;
                employee.Type_OF_Work = empVM.Type_OF_Work;
                employee.Social_insurance_number = empVM.Social_insurance_number;
                employee.Medical_Insurance_Number = empVM.Medical_Insurance_Number;
                employee.End_medical_Insurance = empVM.End_medical_Insurance;
                employee.Sex = empVM.Sex;
                var vacList = new List<EmpVacation> { };
                var vacListtemp = new List<EmpVacationTemp> { };
                if (EmpTemp.Salary_ComponentsTemp.FirstOrDefault()!=null) {
                    List<Salary_ComponentsTemp> compList = new List<Salary_ComponentsTemp> { };
                  
                    Decimal allTotalIncreaseSalary = 0;
                    decimal totalDecreaseSalary = 0;
                    foreach (var comp in EmpTemp.Salary_ComponentsTemp)
                    {
                        var salaryCom = new Salary_Components();
                        salaryCom.Account_Id = comp.Account_Id;
                        salaryCom.Account_Name = comp.Account_Name;
                        salaryCom.Account_Amount = comp.Account_Amount;
                        employee.Salary_Components.Add(salaryCom);
                        var AccountType = db.TypeOfDetailSalaries.Find(comp.Account_Id).Sal_Type;
                        if (AccountType == SalaryTypeValue.يزيد_من_قيمة_اجمالى_الراتب)
                        {
                            allTotalIncreaseSalary += comp.Account_Amount;

                        }
                        else { totalDecreaseSalary += comp.Account_Amount; }

                        compList.Add(comp);

                    }
                   
                    
                    employee.Total_Salary = allTotalIncreaseSalary- totalDecreaseSalary;
                    employee.TotalSalaryBeforPenality = employee.Total_Salary;
                   
                    db.EmpVacations.RemoveRange(employee.EmpVacations);
                    db.EmpVacationTemps.RemoveRange(vacListtemp);
                    db.Salary_ComponentsTemp.RemoveRange(compList);
                    db.Salary_Components.RemoveRange(component);
                    

                    if (empVM.Emp_Image != null)
                    {
                        //var data = new byte[catgorVM.Cat_Img.];
                        //Categr.Cat_Img = data;
                        var length = empVM.Emp_Image.InputStream.Length; //Length: 103050706
                        MemoryStream target = new MemoryStream();
                        empVM.Emp_Image.InputStream.CopyTo(target); // generates problem in this line
                        byte[] data = target.ToArray();
                        employee.Emp_Image = data;

                    }
                   



                }
                else {

                    Decimal allTotalIncreaseSalary = 0;
                    decimal totalDecreaseSalary = 0;
                    foreach (var comp in component)
                    {
                        var AccountType = db.TypeOfDetailSalaries.Find(comp.Account_Id).Sal_Type;
                        if (AccountType == SalaryTypeValue.يزيد_من_قيمة_اجمالى_الراتب)
                        {
                            allTotalIncreaseSalary += comp.Account_Amount;

                        }
                        else { totalDecreaseSalary += comp.Account_Amount; }
                       
                    }

                    employee.Total_Salary = allTotalIncreaseSalary - totalDecreaseSalary;

                }
                if (EmpTemp.EmpVacationTemps.FirstOrDefault() != null)
                {


                    foreach (var item in EmpTemp.EmpVacationTemps.ToList())
                    {
                        var vacation = new EmpVacation();
                        vacation.Vacation_TypesId = item.Vacation_TypesId;
                        vacation.DaysInyear = item.DaysInyear;

                        var oldempvacation = db.EmpVacations.Where(a => a.Vacation_TypesId == item.Vacation_TypesId).FirstOrDefault();
                        if (oldempvacation == null || employee.Vacations.Where(a => a.Status == true).FirstOrDefault() == null)
                        {
                            if (db.VacationEmgrations.Where(a => a.Employee_Id == employee.Emp_Id).FirstOrDefault() == null)
                            {
                                vacation.TotalAfterAbsent = item.DaysInyear;
                            }
                            else
                            {
                                vacation.TotalAfterAbsent = oldempvacation.TotalAfterAbsent;
                            }

                        }

                        else
                        {
                            vacation.TotalAfterAbsent = oldempvacation.TotalAfterAbsent;
                        }

                        vacList.Add(vacation);
                        vacListtemp.Add(item);
                        
                        employee.Vacations_Number += vacation.DaysInyear;



                    }
                }
                db.EmpVacationTemps.RemoveRange(EmpTemp.EmpVacationTemps);
                db.EmployeTemps.Remove(EmpTemp);
                if (empVM.Files.FirstOrDefault() != null)
                {

                    foreach (var file in empVM.Files)
                    {
                        var filedetails = new EmployeeFileDetail();
                        var data = new byte[file.ContentLength];
                        filedetails.FileName = file.FileName;
                        filedetails.ContentLength = file.ContentLength;
                        file.InputStream.Read(data, 0, file.ContentLength);
                        filedetails.ProImg = data;
                        filedetails.EmployeeEmp_Id = employee.Emp_Id;
                        if (data.Length < 1000000)
                        {
                            employee.EmployeeFileDetails.Add(filedetails);
                        }
                        
                    }
                }
                if (empVM.Emp_Image != null)
                {
                    //var data = new byte[catgorVM.Cat_Img.];
                    //Categr.Cat_Img = data;
                    var length = empVM.Emp_Image.InputStream.Length; //Length: 103050706
                    MemoryStream target = new MemoryStream();
                    empVM.Emp_Image.InputStream.CopyTo(target); // generates problem in this line
                    byte[] data = target.ToArray();
                    employee.Emp_Image = data;

                }
                //employee.Vacations_Number = empVM.vacation_Number;

                //employee.TotalVacation = employee.Vacations_Number + employee.Vacation_Pervious;
                if(empVM.Emp_Subledger==true)
                {
                    var subEmpl = db.SubLedgers.Where(a => a.SubLed_Name == employee.Emp_Name).FirstOrDefault();
                    if (subEmpl != null)
                    {
                        subEmpl.SubLed_Name = empVM.Emp_Name;
                        db.Entry(subEmpl).State = EntityState.Modified;
                        db.SaveChanges();
                        employee.Subldger_Id = subEmpl.SubLed_Id;
                    }
                    else
                    {

                        var sub = new SubLedgerVM();
                        sub.ParentSubledger_Id = 1;
                        sub.SubLed_Name = empVM.Emp_Name;
                        sub.SubLed_Date = Convert.ToString(DateTime.Now);
                        sub.SubLed_Type = "مركز تكلفة يدوى";
                        var Sub = SubLedger.createSubLedger(sub);
                        employee.Subldger_Id = Sub.SubLed_Id;

                    }


                 
                }

                employee.Active = true;
                var Employee= db.Accounts.Where(c=> c.Account_Code == employee.Emp_Code).FirstOrDefault();
                Employee.Account_Name = employee.Emp_Name;
                employee.EmpVacations = vacList;
                db.Entry(Employee).State = EntityState.Modified;
             
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                return employee;
            }
            return null;
        }

        public static string EditAdmin(EmpVM empVM)
        {
            try
            {
                var db = new AccountingContainer();
                var employee = db.Employees.Find(empVM.Emp_Id);
              
                var client = db.Clients.FirstOrDefault();

                employee.Emp_Name = empVM.Emp_Name;
                client.Name = empVM.Emp_Name;
                

                var Employee = db.Accounts.Where(c => c.Account_Code == employee.Emp_Code).FirstOrDefault();
                Employee.Account_Name = employee.Emp_Name;

                db.Entry(Employee).State = EntityState.Modified;
                db.Entry(client).State = EntityState.Modified;
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                return Resources.validate.Editsucc;
            }
            catch
            {
                return null;
            }
  
        }

        /// <summary>
        /// Delete Salary Component from Temp by using Component Id
        /// </summary>
        /// <param name="salId"></param>
        /// <returns></returns>
        public static Salary_ComponentsTemp DeleteEmployeeFromTemp(int salId)
        {
            var db = new AccountingContainer();
            Salary_ComponentsTemp vm = db.Salary_ComponentsTemp.Find(salId);
            db.Salary_ComponentsTemp.Remove(vm);
            db.SaveChanges();
            db.Dispose();
            return vm;
        }
        /// <summary>
        /// edit Employee Status to صرف or لم يصرف by using EmployeeId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool UpdateEmployeeSalary(int id)
        {
            var db = new AccountingContainer();

            var EmployeeStatus = db.Employees.Find(id);
            if (EmployeeStatus.Status == Models.Status.لم_يصرف)
            {


                // Create GL for the invoice 
                EmployeeStatus.Status = Models.Status.صرف;
            }
            else { EmployeeStatus.Status = Models.Status.لم_يصرف; }
                db.Entry(EmployeeStatus).State = EntityState.Modified;
                db.SaveChanges();
            db.Dispose();
            return true;
        }
        /// <summary>
        /// عمل قيد تجهيز المرتبات 
        /// اتباع الخطوات التالية
        /// بحث فى القيود على قيد تجهيز مرتبات  
        /// اذا وجد لن يتم عمل القيد 
        /// يتم عمل القيد بجمع مرتبات الموظفين الذين لهم نفس مركز التكلفة وعملها في قيد منفصل 
        /// </summary>
        /// <returns></returns>
        public static SalGLDate  FillForMakeGLForSalary() {


            SalGLDate sal = new SalGLDate();
            sal.date=DateTime.Today.ToString("dd-MM-yyyy   HH:mm:ss");
            return sal;

        }
        public static bool Checkdate(string date)
        {
            var datetime = Convert.ToDateTime(date);
            var db = new AccountingContainer();
           
            
                if (datetime > DateTime.Now)
                {
                    return false;
                }


            

            return true;


        }
        public static SalGLDate GetlListOfEmployeeSalary(SalGLDate salvm)
        {
            var db = new AccountingContainer();
            var date = Convert.ToDateTime(salvm.date);
            var gla = db.GLMasters.Where(a => a.GLType_Id.StartsWith("مرتبات")).ToList();
            var empList = db.Employees.Where(a => a.Active != false&&a.Status==Models.Status.صرف).ToList();
            var time = gla.Where(a => a.GL_Date.Year == date.Year);
            var timeMonth = time.Where(a => a.GL_Date.Month == date.Month);
            var nullmounth = new GLMaster();
            var RemoveSalaryComponantVacation = new List<Salary_Components> { };
            salvm.EmployeeList = new List<Employee>();

            try
            {
                nullmounth = timeMonth.ToList().Last();
                if (nullmounth.GLState == TransactionState.Void)
                {
                   nullmounth = null;
                
                }


            }

            catch
            {
                nullmounth = null;
              

            }
            if (nullmounth == null)
            {

                foreach (var emp in empList.Where(a=>a.Shifts.Count()>0))
                {
                    AttendingAndLeaving.PrepareTheSalaryOfEmployee(emp.Emp_Id, date);

                }
                foreach(var emp in empList.Where(a => a.Work_Actual_Start != null&&a.Work_Actual_Start!=""&&a.Shifts.Count()==0).ToList())
                {
                  
                        var StartDate = Convert.ToDateTime(emp.Work_Actual_Start);
                        var getNumberOfDayOfMounth =AttendingAndLeaving.GetDates(date.Year, date.Month).Count();
                        var DayWork = AttendingAndLeaving.GetDates(date.Year, date.Month).Where(a => a.Date >= StartDate).Count();
                    emp.Total_Salary = ((emp.TotalSalaryBeforPenality / getNumberOfDayOfMounth) * DayWork) ;
                   
                    db.Entry(emp).State = EntityState.Modified;
                    db.SaveChanges();

                }


            }
            
            salvm.EmployeeList = getEmployees().Where(a => a.Active != false && a.Status == Models.Status.صرف).ToList();
            return salvm;

        }
        public static string MakeGLForSalary( SalGLDate salvm)
        {
            var db = new AccountingContainer();
            var date = Convert.ToDateTime(salvm.date);
            var gla = db.GLMasters.Where(a => a.GLType_Id.StartsWith("مرتبات")).ToList(); 
            var time = gla.Where(a => a.GL_Date.Year == date.Year);
            var timeMonth = time.Where(a => a.GL_Date.Month == date.Month);
            var nullmounth = new GLMaster();
            var RemoveSalaryComponantVacation = new List<Salary_Components> { };


            try
            {
              nullmounth = timeMonth.ToList().Last();
                if (nullmounth.GLState==TransactionState.Void)
                {
                    nullmounth = null;
                }


            }

            catch {
                nullmounth = null;


            }



            if (nullmounth == null)
            {

                var list = db.Employees.Where(a => a.Status == Models.Status.صرف&&a.Active==true).ToList();


                var disticedSubLedger = list.Select(x => x.Subldger_Id).Distinct().ToList();
                foreach (var subLedger in disticedSubLedger.ToList())
                {
                    GLMaster glMaster = new GLMaster();
                    var lastSerial = db.GLMasters.Where(c => c.GLCode.StartsWith("EMPR-"));
                    if (lastSerial.FirstOrDefault() == null)
                    {
                        glMaster.GLCode = "EMPR-001";
                    }
                    else
                    {
                        var last = lastSerial.OrderByDescending(c => c.GLMaster_Id).FirstOrDefault();
                        //using string substring method to get the number of the last inserted
                        glMaster.GLCode = "EMPR-" + (Convert.ToInt32(last.GLCode.Substring(5, last.GLCode.Length - 5)) + 1).ToString("D3");
                    }
                    glMaster.GL_Date = date.AddDays(30);
                    var LastDayInMounth = DateTime.DaysInMonth(date.Year, date.Month);
                   glMaster.GL_Date = new DateTime(date.Year, date.Month, LastDayInMounth).AddHours(23).AddMinutes(59).AddSeconds(59);
                    glMaster.GL_Desc = " استحقاق راتب شهر" + glMaster.GL_Date.Month + "/  " + glMaster.GL_Date.Year;
                        
                    glMaster.Company_Id = 1;
                    if (subLedger.Value == 0)
                    {
                        glMaster.GLType_Id = "مرتبات" + db.SubLedgers.Find(1).SubLed_Code;
                    }
                    else
                    {
                        glMaster.GLType_Id = "مرتبات" + db.SubLedgers.Find(subLedger.Value).SubLed_Code;
                    }
                    glMaster.Currency_Id = "الريال السعودى";
                    glMaster.Auto = true;
                    glMaster.GLState = TransactionState.Pending;
                   
                    var listOfSalary = new List<Salary_Components>();
                    var ListaSub = list.Where(a => a.Subldger_Id == subLedger.Value).ToList();

                  
                        decimal sall = 0;
                    
                        foreach (var item in ListaSub)
                        {
                        GLDetail GLTotal = new GLDetail();


                        GLTotal.Account_Id = 50;
                        GLTotal.Account_Name = db.ChartOfAccountings.Find(50).Account_Name;
                        GLTotal.Bu_Id = 0;
                        GLTotal.Account_Child_ID = db.Accounts.Where(a=>a.Account_Code==item.Emp_Code).FirstOrDefault().Accounts_Id;
                        GLTotal.GLDetail_Credit = item.Total_Salary.Value;
                        glMaster.GLDetails.Add(GLTotal);
                        sall += item.Total_Salary.Value;
                        }
                       

                    var LibirotyForEmployee = new GLDetail();
                    LibirotyForEmployee.Account_Id = 111;
                        LibirotyForEmployee.Account_Name = db.ChartOfAccountings.Find(111).Account_Name;
                        LibirotyForEmployee.Bu_Id = 0;
                        LibirotyForEmployee.SubLedger_Id = subLedger;
                        LibirotyForEmployee.GLDetail_Debit = sall;

                       
                        glMaster.GLDetails.Add(LibirotyForEmployee);
                       


                        savegl(glMaster);
                    var gldetilLoan = glMaster.GLDetails.Where(a => a.Account_Id != 50).ToList();
                    var glmasterWithoutLibality = new GLMaster();
                    glmasterWithoutLibality.GLDetails = gldetilLoan;
                    GLMaster.GetBalanceForAccount(glMaster);
                        GLMaster.GetBalanceForSubLedger(glmasterWithoutLibality);

                        foreach (var emp in ListaSub)
                        {

                            var salMonth = new EmployeeSalaryMonth();
                            salMonth.Emp_Code = emp.Emp_Code;
                            salMonth.Emp_Name = emp.Emp_Name;
                            salMonth.Date = glMaster.GL_Date;
                            salMonth.TotalSalary = emp.Total_Salary.Value;
                            salMonth.Status = صرف_المرتبات_الشهرى.لم_يصرف;
                        if (emp.Shifts.Count() > 0)
                        {
                            salMonth.HourValueForEmployee = AttendingAndLeaving.PrepareTheSalaryOfEmployee(emp.Emp_Id, salMonth.Date);
                        }
                            foreach (var account in emp.Salary_Components)
                            {
                                var Emp = new EmpSal_Detail();
                                Emp.Account_Id = account.Account_Id;
                               // Emp.AcountValue = account.Account_Amount;
                            var Percentage = account.Account_Amount / account.Employee.TotalSalaryBeforPenality;
                           Emp.AcountValue = Percentage.Value * emp.Total_Salary.Value;
                                Emp.VacationId = account.VacationId;
                                salMonth.EmpSal_Detail.Add(Emp);
                            if (account.VacationId != null)
                            {

                                RemoveSalaryComponantVacation.Add(account);
                            }

                            }
                       var SalId=  CreateSalaryMounth(salMonth);
                        foreach (var penality in emp.AttendingAndLeavings.Where(a=>a.Date< date).SelectMany(a => a.RewardsAndPenalties).Where(a => a.DateOfTaken == null))
                        {
                            penality.DateOfTaken = salMonth.Date;
                            penality.IsTaken = true;
                            penality.EmployeeSalaryMonthId = SalId;
                            db.Entry(penality).State = EntityState.Modified;
                        }
                           

                        emp.Total_Salary = emp.TotalSalaryBeforPenality;
                        db.Entry(emp).State = EntityState.Modified;
                        
                    }
                       

                    }

                     db.Salary_Components.RemoveRange(RemoveSalaryComponantVacation);
               
                db.SaveChanges();
                    db.Dispose();
                    return Resources.validate.GLDone;
                }
               
           
            return Resources.validate.Thismonthssalary;
        }

        public static int getGlForSalary(DateTime date,string code)
        {
            var db = new AccountingContainer();
            var account = db.Accounts.Where(a => a.Account_Code == code).FirstOrDefault();
            var gl = db.GLDetails.Where(a => a.GLMaster.GLCode.StartsWith("EMPR-") && a.GLMaster.GL_Date.Day == date.Day&&a.GLMaster.GL_Date.Month==date.Month&&a.GLMaster.GL_Date.Year==date.Year&&a.Account_Child_ID==account.Accounts_Id).ToList();
            gl.Reverse();

            return gl.FirstOrDefault().GLMasterGLMaster_Id;

        }
        /// <summary>
        /// تحويل جميع المرتبات المستحقة الي بنك او حساب معين وذلك بحساب الرقم الاجمالي لمرتبات المستحقة هذا الشهر
        /// </summary>
        /// <returns></returns>
        public static TransferSalaryToAccountVM transferToBank()
        {
            var db = new AccountingContainer();
            
            var transVM = new TransferSalaryToAccountVM ();
            var gla = db.GLMasters.Where(a => a.GLType_Id .StartsWith("مرتبات")).ToList();
            var time = gla.Where(a => a.GL_Date.Year == DateTime.Today.Year);
            var timeMonth = time.Where(a => a.GL_Date.Month == DateTime.Today.Month).ToList();
            if (timeMonth.FirstOrDefault() != null)
            {
                foreach (var gl in timeMonth)
                {
                    var Total = gl.GLDetails.Where(a => a.GLDetail_Debit != 0).ToList();
                    foreach (var subTotal in Total)
                    {
                        transVM.Total_Sallary += subTotal.GLDetail_Debit;
                    }
                 
                }
            }
           


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

            transVM.AccountList =  new SelectList(lista.Where(accCode => accCode.Account_Code.StartsWith("010120002") || accCode.Account_Code.StartsWith("010120001")), "Account_Id", "Account_Name");
            return transVM;
        }
        /// <summary>
        ///  تحويل جميع المرتبات المستحقة الي بنك او حساب معين وعمل قيد بهم
        /// </summary>
        /// <param name="TransVM"></param>
        /// <returns></returns>
        public static String MakeGLForTransferToBank(TransferSalaryToAccountVM TransVM)
        {
            var db = new AccountingContainer();
            var gla = db.GLMasters.Where(a => a.GLType_Id == "فيد تحويل مرتبات الى حساب"|| a.GLType_Id== "فيد تحويل مرتبات معينة الى حساب").ToList();
            var time = gla.Where(a => a.GL_Date.Year == DateTime.Today.Year);
            var timeMonth = time.Where(a => a.GL_Date.Month == DateTime.Today.Month).FirstOrDefault();
            if (TransVM.Total_Sallary == 0)
            {
                return "خطأ القيد يساوي صفر";
            }
           else if (timeMonth == null) {
                GLMaster glMaster = new GLMaster();
                glMaster.GLCode = "MakeGLForTransferSalary";
                glMaster.GL_Date = DateTime.Now;
                glMaster.GL_Desc = "سند صرف راتب";
                glMaster.Company_Id = 1;
                glMaster.GLType_Id = "صرف راتب";
                glMaster.Currency_Id = "الريال السعودى";
                glMaster.GLState = TransactionState.Pending;
                glMaster.Auto = true;
                GLDetail bankL = new GLDetail(),
                     TotalSalary = new GLDetail();
                
                bankL.Bu_Id = 0;
                TotalSalary.Bu_Id = 0; ;
                var ChartOfAccounRecive = db.ChartOfAccountings.Find(TransVM.Account);
                bankL.Account_Id = ChartOfAccounRecive.Account_Id;
                bankL.Account_Name = ChartOfAccounRecive.Account_Name;
                TotalSalary.Account_Id = 50;
                TotalSalary.Account_Name = db.ChartOfAccountings.Find(50).Account_Name;
                bankL.SubLedger_Id = 
                TotalSalary.SubLedger_Id = 0;
                bankL.GLDetail_Credit =
               TotalSalary.GLDetail_Debit = TransVM.Total_Sallary;
                bankL.GLDetail_Debit =0;
                glMaster.GLDetails.Add(TotalSalary);
                glMaster.GLDetails.Add(bankL);
                db.GLMasters.Add(glMaster);
                var list = db.Employees.Where(a => a.Status == Models.Status.صرف);
              
                var salYear = db.EmployeeSalaryMonths.Where(k => k.Date.Year == DateTime.Today.Year);
                var salmon = salYear.Where(z => z.Date.Month == DateTime.Today.Month).ToList();
                foreach(var emp in salmon)
                {
                    emp.Status = صرف_المرتبات_الشهرى.لم_يصرف;
                    emp.AcountSal = TransVM.Account;
                    db.Entry(emp).State = EntityState.Modified;
                }

                db.SaveChanges();
                GLMaster.GetBalanceForAccount(glMaster);
                db.Dispose();
                return "تم عمل قيد تحويل الرواتب  بنجاح";
            }

            return "تم عمل قيد تحويل المرتبات لهذا الشهر من قبل ";




        }


        public static void deleteAllSalGl()
        {
            var db = new AccountingContainer();
            db.EmpSal_Detail.RemoveRange(db.EmpSal_Detail);
            db.EmployeeSalaryMonths.RemoveRange(db.EmployeeSalaryMonths);
            db.GLDetails.RemoveRange(db.GLDetails.Where(s => s.GLMaster.GLCode.StartsWith("EMP")));
            db.GLMasters.RemoveRange(db.GLMasters.Where(s => s.GLCode.StartsWith("EMP")));
            db.SaveChanges();
        }
       /// <summary>
       /// تقوم بفلترة الاقسام تبعأ للفرع المختار
       /// </summary>
       /// <param name="Branch_Id"></param>
       /// <returns></returns>
 

        public static IEnumerable<SelectListItem> GetDepartment(int BranshID)
        {
            var db = new AccountingContainer();
            IEnumerable<SelectListItem> dep = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == BranshID && p.SubLed_Type == "قسم لفرع"&&p.isActive!=false), "SubLed_Id", "SubLed_Name");
            // = db.Provinces.Where(p => p.CountryId == countryId).ToList();
            return dep;
        }
        public static IEnumerable<SelectListItem> GetBussines(int DepID)
        {
            var db = new AccountingContainer();
            IEnumerable<SelectListItem> dep = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == DepID && p.Subled_Level == 3 && p.SubLed_Type == "نشاط لقسم" && p.isActive != false), "SubLed_Id", "SubLed_Name");
            // = db.Provinces.Where(p => p.CountryId == countryId).ToList();
            return dep;
        }
        /// <summary>
        /// تقوم بحفظ القيد اليومي فى قاعدة البيانات
        /// </summary>
        /// <param name="gl"></param>

        public static void savegl(GLMaster gl) {

            var db = new AccountingContainer();
            db.GLMasters.Add(gl);
            db.SaveChanges();

        }
        //////////////////////////////////////////////////Adding Sales_Settings///////////////////////////////////////
        public static Sales_SettingsVM SalessettingforCreation(int id)
        {
            var db = new AccountingContainer();
            var salSettingVm = new Sales_SettingsVM();
            salSettingVm.Employee_ID = id;
            salSettingVm.SlicesList = new SelectList(db.Slices, "Slice_Id", "Slice_Name");
            return salSettingVm;
        }
        /// <summary>
        /// Prepare SalesSetting for Editing 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Sales_SettingsVM SalessettingforEdditing(int id)
        {
            var db = new AccountingContainer();
            var thissalessettings = db.Sales_Settings.Where(e => e.Employee_ID == id).ToList().FirstOrDefault();
            var salSettingVm = new Sales_SettingsVM();
            if (thissalessettings != null)
            {
              
                salSettingVm.Employee_ID = id;
                salSettingVm.SalesMan = thissalessettings.SalesMan;
                salSettingVm.Collector = thissalessettings.Collector;
                salSettingVm.Cant_Controle = thissalessettings.Cant_Controle;
                salSettingVm.Can_Increase_Price = thissalessettings.Can_Increase_Price;
                salSettingVm.Can_Decrease_Price = thissalessettings.Can_Decrease_Price;
                salSettingVm.Can_Sell_InLow = thissalessettings.Can_Sell_InLow;
                salSettingVm.SlicesList = new SelectList(db.Slices, "Slice_Id", "Slice_Name");
                return salSettingVm;
            }
            else {
                salSettingVm.Employee_ID = id;
                salSettingVm.SlicesList = new SelectList(db.Slices, "Slice_Id", "Slice_Name");
                return salSettingVm;
            }
  
          
           
        }
        /// <summary>
        /// Post the Editing Of SalesSetting in data Base
        /// </summary>
        /// <param name="SVM"></param>
        /// <returns></returns>
        public static string EditSettings(Sales_SettingsVM SVM)
        {
            var db = new AccountingContainer();
            var thissalessettings = db.Sales_Settings.Where(e => e.Employee_ID == SVM.Employee_ID).ToList().FirstOrDefault();
            
            thissalessettings.Employee_ID = SVM.Employee_ID;
            thissalessettings.SalesMan = SVM.SalesMan;
            thissalessettings.Collector = SVM.Collector;
            thissalessettings.Cant_Controle = SVM.Cant_Controle;
            thissalessettings.Can_Increase_Price = SVM.Can_Increase_Price;
            thissalessettings.Can_Decrease_Price = SVM.Can_Decrease_Price;
            thissalessettings.Can_Sell_InLow = SVM.Can_Sell_InLow;
            db.Entry(thissalessettings).State = EntityState.Modified;
            db.SaveChanges();
         
            return "تم التعديل";
        }
        /// <summary>
        /// that method for Adding new Seting For Employee 
        /// First Check that if that Employee is Exisit Or not second 
        /// </summary>
        /// <param name="sVM"></param>
        /// <returns></returns>
        public static string AddSettings(Sales_SettingsVM sVM)
        {
            var db = new AccountingContainer();
            var newSettings = new Sales_Settings();
            var sliceTemp = db.Sales_Setting_SliceTemp.ToList();
            var slicesetting = new Sales_Setting_Slice();
            var checksettings = db.Sales_Settings.Where(e => e.Employee_ID == sVM.Employee_ID).ToList().FirstOrDefault();
            if (checksettings == null)
            {
                newSettings.Employee_ID = sVM.Employee_ID;
                newSettings.SalesMan = sVM.SalesMan;
                newSettings.Collector = sVM.Collector;
                newSettings.Cant_Controle = sVM.Cant_Controle;
                newSettings.Can_Increase_Price = sVM.Can_Increase_Price;
                newSettings.Can_Decrease_Price = sVM.Can_Decrease_Price;
                newSettings.Can_Sell_InLow = sVM.Can_Sell_InLow;
                db.Sales_Settings.Add(newSettings);
                db.SaveChanges();
                foreach (var slice in sliceTemp)
                {
                    slicesetting.Slice_Name = slice.Slice_Name;
                    slicesetting.Slice_ID = slice.Slice_ID;
                    slicesetting.Sales_SettingsId = newSettings.Id;
                    db.Sales_Setting_Slice.Add(slicesetting);
                    db.SaveChanges();
                }
                db.Entry(newSettings).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                checksettings.Employee_ID = sVM.Employee_ID;
                checksettings.SalesMan = sVM.SalesMan;
                checksettings.Collector = sVM.Collector;
                checksettings.Cant_Controle = sVM.Cant_Controle;
                checksettings.Can_Increase_Price = sVM.Can_Increase_Price;
                checksettings.Can_Decrease_Price = sVM.Can_Decrease_Price;
                checksettings.Can_Sell_InLow = sVM.Can_Sell_InLow;
                db.Entry(checksettings).State = EntityState.Modified;
                db.SaveChanges();
                foreach (var slice in sliceTemp)
                {
                    slicesetting.Slice_Name = slice.Slice_Name;
                    slicesetting.Slice_ID = slice.Slice_ID;
                    slicesetting.Sales_SettingsId = checksettings.Id;
                    db.Sales_Setting_Slice.Add(slicesetting);
                    db.SaveChanges();
                }
                db.Entry(checksettings).State = EntityState.Modified;
                db.SaveChanges();
            }




            db.Sales_Setting_SliceTemp.RemoveRange(sliceTemp);
            db.SaveChanges();
            db.Dispose();
            return "تم حفظ الاعدادات";
        }
        /// <summary>
        /// لاضافة شريحة جديدة مختصة للموظف
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string addSlicetoTemp(int id)
        {
            var db = new AccountingContainer();
            var newtemp = new Sales_Setting_SliceTemp();
            var thisclise = db.Slices.Find(id);
            newtemp.Slice_Name = thisclise.Slice_Name;
            newtemp.Slice_ID = thisclise.Slice_Id;
            var alltemp = db.Sales_Setting_SliceTemp.Where(e => e.Slice_ID == newtemp.Slice_ID).ToList().FirstOrDefault();
            if (alltemp == null)
            {
                db.Sales_Setting_SliceTemp.Add(newtemp);
                db.SaveChanges();
                db.Dispose();
                return "تم اضافة الشرائح";
            }
            else
            {
                return "لقد تم اضافة الشريحة للموظف من قبل";
            }



        }
        public static string AddAllSlicestoTemp()
        {
            var db = new AccountingContainer();
            var newtemp = new Sales_Setting_SliceTemp();
            var thisclise = db.Slices.ToList();
            foreach (var slice in thisclise)
            {
                newtemp.Slice_Name = slice.Slice_Name;
                newtemp.Slice_ID = slice.Slice_Id;
                var checkSlice = db.Sales_Setting_SliceTemp.Where(e => e.Slice_ID == slice.Slice_Id).ToList().FirstOrDefault();
                if (checkSlice == null)
                {
                    db.Sales_Setting_SliceTemp.Add(newtemp);
                    db.SaveChanges();
                }
                else
                {
                    db.Sales_Setting_SliceTemp.Remove(newtemp);
                    db.SaveChanges();
                    db.Sales_Setting_SliceTemp.Add(newtemp);
                    db.SaveChanges();
                }

            }
            db.Dispose();
            return "تم اضافة  كل الشرائح";

        }

        /// <summary>
        /// الاتيان بجميع شرائح السعر 
        /// </summary>
        /// <returns></returns>
        public static List<Sales_Setting_SliceTemp> AlltempList()
        {
            var db = new AccountingContainer();
            var alltemp = db.Sales_Setting_SliceTemp.ToList();

            return alltemp;

        }
        /// <summary>
        /// تحضير القيد اليدوي للرواتب لعمله
        /// </summary>
        /// <returns></returns>
        public static EmployeeSalaryVM manualGLSalary()
        {
            var db = new AccountingContainer();
            EmployeeSalaryVM vm = new EmployeeSalaryVM();
            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0 && a.isActive != false);           
            vm.AccountList = new SelectList(lista.Where(accCode => accCode.Account_Code.StartsWith("010120002") || accCode.Account_Code.StartsWith("010120001")), "Account_Id", "Account_Name");
            PutLoanInEmpSal();


            return vm;

        }
        /// <summary>
        /// تحضير مرتبات الموظفين كل منه علي حدا وذلك بحساب سلفياتهم
        /// </summary>
        public static void PutLoanInEmpSal()
        {
            var db = new AccountingContainer();
            var empList = db.EmployeeSalaryMonths.Where(a=>a.AcountSal== null&&a.Status == صرف_المرتبات_الشهرى.لم_يصرف);
            if (empList.FirstOrDefault()!=null) {
               
                if (empList.ToList().Last().Trims_Pay == null) {
                   var codeConflict= empList.Select(a => a.Emp_Code).Distinct();
                    foreach (var Emp in codeConflict)
                    {
                        var Emploan = db.Accounts.Where(a => a.Account_Code == Emp).FirstOrDefault();
                       var empsal= empList.Where(a => a.Emp_Code == Emp).FirstOrDefault();
                        if (Emploan.Account_Balance < 0)
                        {
                            empsal.Trims_Pay = 0;
                        }
                        else
                        {
                            empsal.Trims_Pay = Emploan.Account_Balance;
                        }
                        
                        db.Entry(empsal).State = EntityState.Modified;


                    }
                    foreach (var empconflict in empList)
                    {
                        if (empconflict.Trims_Pay == null)
                        {
                            var Emploan = db.Accounts.Where(a => a.Account_Code == empconflict.Emp_Code).FirstOrDefault();


                            empconflict.Trims_Pay = 0;
                            db.Entry(Emploan).State = EntityState.Modified;

                        }
                    }

                    db.SaveChanges();


                }

            }
        }


        /// <summary>
        /// يتم عمل قيد المرتبات للموظفين الختارين وتغير حالتهم الى تم الصرف
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public static String PostManualGLSalary(EmployeeSalaryVM vm)
        {
            var db = new AccountingContainer();
            var ListaOfSall = db.EmployeeSalaryMonths.Where(a => a.Status == صرف_المرتبات_الشهرى.صرف).ToList();
            decimal totalOfTotalSal = 0;
            decimal netOfsalEmployeeTotal = 0;
            if (ListaOfSall.FirstOrDefault() != null)
            {
                foreach(var sal  in ListaOfSall)
                {
                    sal.AcountSal = vm.BankAccount;
                    sal.Status = صرف_المرتبات_الشهرى.تم_الصرف;
                    sal.Date_Take_Sal = Convert.ToDateTime(vm.Date);
                    totalOfTotalSal += sal.TotalSalary;
                    netOfsalEmployeeTotal += sal.Trims_Pay.Value;

                    db.Entry(sal).State = EntityState.Modified;
                   
                    db.SaveChanges();
                }
                GLMaster glMaster = new GLMaster();
                var lastSerial = db.GLMasters.Where(c => c.GLCode.StartsWith("EmpT-"));
                if (lastSerial.FirstOrDefault() == null)
                {
                    glMaster.GLCode = "EmpT-001";
                }
                else
                {
                    var last = lastSerial.OrderByDescending(c => c.GLMaster_Id).FirstOrDefault();
                    //using string substring method to get the number of the last inserted
                    glMaster.GLCode = "EmpT-" + (Convert.ToInt32(last.GLCode.Substring(5, last.GLCode.Length - 5)) + 1).ToString("D3");
                }
                glMaster.GL_Date =Convert.ToDateTime( vm.Date);
                glMaster.GL_Desc =vm.Description;
                glMaster.Company_Id = 1;
                glMaster.GLType_Id = "فيد تحويل مرتبات";
                glMaster.Currency_Id = "الريال السعودى";
               
                glMaster.GLState = TransactionState.Pending;
                glMaster.Auto = true;
                GLDetail bankL = new GLDetail();
                    

                bankL.Bu_Id = 0;
              
                var ChartOfAccounRecive = db.ChartOfAccountings.Find(vm.BankAccount);
                bankL.Account_Id = ChartOfAccounRecive.Account_Id;
                bankL.Account_Name = ChartOfAccounRecive.Account_Name;

                bankL.SubLedger_Id = 0;

                if (netOfsalEmployeeTotal > totalOfTotalSal)
                {
                    bankL.GLDetail_Debit = netOfsalEmployeeTotal - totalOfTotalSal;
                    bankL.GLDetail_Credit = 0;
                }
                else
                {
                    bankL.GLDetail_Credit = totalOfTotalSal - netOfsalEmployeeTotal;

                    bankL.GLDetail_Debit = 0;
                }
               
                foreach(var Loan in ListaOfSall)
                {
                    var TotalSalary = new GLDetail();
                    var loanGl = new GLDetail();
                    var account = db.Accounts.Where(a => a.Account_Code == Loan.Emp_Code).FirstOrDefault();
                    loanGl.Account_Child_ID = account.Accounts_Id;
                    loanGl.GLDetail_Debit = 0;
                    loanGl.GLDetail_Credit =Loan.Trims_Pay.Value;
                    loanGl.Account_Id = 39;
                    loanGl.Account_Name = db.ChartOfAccountings.Find(39).Account_Name;
                    if (loanGl.GLDetail_Credit!=0)
                    {
                        glMaster.GLDetails.Add(loanGl);
                    }
                    TotalSalary.Account_Id = 50;
                    TotalSalary.Account_Name = db.ChartOfAccountings.Find(50).Account_Name;
                    TotalSalary.Account_Child_ID = db.Accounts.Where(a => a.Account_Code == Loan.Emp_Code).FirstOrDefault().Accounts_Id;
                    TotalSalary.Bu_Id = 0; ;
                    TotalSalary.SubLedger_Id = 0;
                    TotalSalary.GLDetail_Debit = Loan.TotalSalary;
                    TotalSalary.GLDetail_Credit = 0;
                    glMaster.GLDetails.Add(TotalSalary);
                }



               
                glMaster.GLDetails.Add(bankL);
                db.GLMasters.Add(glMaster);

                db.SaveChanges();
                //var gla = db.GLMasters.Where(a => a.GLType_Id == "مرتبات").ToList();
                //var time = gla.Where(a => a.GL_Date.Year == DateTime.Today.Year);
                //var timeMonth = time.Where(a => a.GL_Date.Month == DateTime.Today.Month);
                //var AutoGL = timeMonth.Last();
                //AutoGL.GLState= TransactionState.Post;
                //db.Entry(AutoGL).State = EntityState.Modified;
                //db.SaveChanges();
                var gldetilLoan = glMaster.GLDetails.Where(a => a.Account_Id != 50).ToList();
                var glmasterWithoutLibality = new GLMaster();
                glmasterWithoutLibality.GLDetails = gldetilLoan;
                GLMaster.GetBalanceForAccount(glMaster);
                GLMaster.GetBalanceForSubLedger(glmasterWithoutLibality);
                db.Dispose();

            }


            return "Sucessful";
        }


        public static SalGLDate FillForMakeVoidOfGLSalary()
        {
            var vmdate = new SalGLDate();
            return vmdate;
                }

        /// <summary>
        /// عمل عكس للقيد الاستحقاق للمرتبات
        /// </summary>
        /// <returns></returns>
            public static string MakeVoidOfGLSalary(SalGLDate vmdate)
        {
            var db = new AccountingContainer();
            var date = Convert.ToDateTime(vmdate.date);
            var gla = db.GLMasters.Where(a => a.GLType_Id .StartsWith("مرتبات")).ToList();
            var time = gla.Where(a => a.GL_Date.Year == date.Year);
            var timeMonth = time.Where(a => a.GL_Date.Month == date.Month);
            var nullmounth = new GLMaster();


            try
            {
                nullmounth = timeMonth.ToList().Last();
                if (nullmounth.GLState == TransactionState.Post|| nullmounth.GLState == TransactionState.Void)
                {
                    nullmounth = null;
                }


            }

            catch
            {
                nullmounth = null;


            }
            if (nullmounth != null)
            {
                var EmpCheck = db.EmpSal_Detail.Where(a => a.EmployeeSalaryMonth.Date.Month == date.Month && a.EmployeeSalaryMonth.Date.Year == date.Year && a.EmployeeSalaryMonth.Status==صرف_المرتبات_الشهرى.تم_الصرف).ToList();
                if (EmpCheck.Count() != 0)
                {
                    return Resources.validate.CannotReverseGL;
                }


                foreach (var item in timeMonth.Where(a=>a.GLState==TransactionState.Pending).ToList())
                {
                    var msg = GLMaster.MakeVoidOfPostingGLForHR(item.GLMaster_Id);
                    item.GLState= TransactionState.Void;
                    db.Entry(item).State = EntityState.Modified;
                }
                
                var month = timeMonth.Last().GL_Date.Month;
                var year = timeMonth.Last().GL_Date.Year;
                var datetime = timeMonth.Last().GL_Date;
                var Emp = db.EmployeeSalaryMonths.Where(a => a.Date.Month == month&&a.Date.Year==a.Date.Year).ToList();
                var salCopList = new List<Salary_Components> { };
               
                foreach (var item in Emp)
                {
                    var emp = db.Employees.Where(a => a.Emp_Code == item.Emp_Code).FirstOrDefault();
                    foreach (var detail in item.EmpSal_Detail.Where(a=>a.VacationId!=null).ToList())
                    {
                        var com = new Salary_Components();
                        com.Account_Id = detail.Account_Id;
                        com.VacationId = detail.VacationId;
                        com.Account_Name = db.TypeOfDetailSalaries.Find(detail.Account_Id).SalaryDetail_Name;
                        com.Employee_Id = emp.Emp_Id;
                        com.Account_Amount = detail.AcountValue;
                        emp.Total_Salary = emp.Total_Salary - com.Account_Amount;
                        salCopList.Add(com);


                    }
                    var penalityList = db.RewardsAndPenalties.Where(a => a.DateOfTaken == datetime).ToList();
                    if (penalityList.Count() > 0)
                    {
                        foreach (var penality in penalityList)
                        {
                            penality.DateOfTaken = null;
                            penality.IsTaken = false;
                            penality.EmployeeSalaryMonthId = null;
                            db.Entry(penality).State = EntityState.Modified;
                        }
                    }
                    db.Entry(emp).State = EntityState.Modified;
                  
                    db.EmpSal_Detail.RemoveRange(item.EmpSal_Detail);
                    db.EmployeeSalaryMonths.Remove(item);
                    db.SaveChanges();

                }
                db.Salary_Components.AddRange(salCopList);
                db.SaveChanges();
                db.Dispose();
                return Resources.validate.GLDone;
            }

            return Resources.validate.CannotReverseGL1;

        }


        public static EmployeeSalaryVM EditInTypeTrims(int id)
        {
            var db = new AccountingContainer();
            var vm = new EmployeeSalaryVM();
            var empSal = db.EmployeeSalaryMonths.Find(id);
            vm.Employee_Code = empSal.Emp_Code;
            vm.Employee_Name = empSal.Emp_Name;
            vm.Trim_Pay = empSal.Trims_Pay.Value;
            return vm;

        }
        /// تعديل بيانات تفاصيل المرتب الشهري
        /// <summary>
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public static String EditInTypeTrimsPost(EmployeeSalaryVM vm)
        {
            var db = new AccountingContainer();
            var empSal = db.EmployeeSalaryMonths.Find(vm.id);
            empSal.Trims_Pay = vm.Trim_Pay;
            db.Entry(empSal).State = EntityState.Modified;
            db.SaveChanges();
            db.Dispose();
            return "تم تعديل سلف الموظف ";


        }
       public static int CreateSalaryMounth(EmployeeSalaryMonth empMon)
        {
            var db = new AccountingContainer();
            db.EmployeeSalaryMonths.Add(empMon);
            db.SaveChanges();
            return empMon.Id;


        }
        /// <summary>
        /// بتجيب كل المرتبات التى لم تصرف سواء من بنك او خزينة
        /// </summary>
        /// <returns></returns>
        public static List<EmployeeSalaryMonth> getAllsallaryForThatMounth()
        {
            var db = new AccountingContainer();
            var List = db.EmployeeSalaryMonths.Where(a => a.AcountSal == null || a.AcountSal == 0).ToList();

            return List;


        }
        /// <summary>
        /// تحويل المرتب من حاله الصرف الى لم يصرف
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool UpdateSalaryMounth(int id)
        {
            var db = new AccountingContainer();

            var EmployeeStatus = db.EmployeeSalaryMonths.Find(id);
            if (EmployeeStatus.Status == صرف_المرتبات_الشهرى.صرف)
            {


                // Create GL for the invoice 
                EmployeeStatus.Status = صرف_المرتبات_الشهرى.لم_يصرف;
            }
            else { EmployeeStatus.Status = صرف_المرتبات_الشهرى.صرف; }
            db.Entry(EmployeeStatus).State = EntityState.Modified;
            db.SaveChanges();
            db.Dispose();
            return true;
        }

        /// <summary>
        /// جعل جميع المرتبات بنفس الحالة سواء صرف او لم_يصرف
        /// </summary>
        /// <returns></returns>
        public static bool UpdateSalaryMounthForAllEmp()
        {
            var db = new AccountingContainer();

            var EmployeeStatus = db.EmployeeSalaryMonths.Where(a=>a.Status==صرف_المرتبات_الشهرى.لم_يصرف&&a.AcountSal==null).ToList();
            if (EmployeeStatus.FirstOrDefault() != null)
            {
                foreach (var EmployeeStatusa in EmployeeStatus)
                if (EmployeeStatusa.Status == صرف_المرتبات_الشهرى.لم_يصرف)
                {


                    // Create GL for the invoice 
                    EmployeeStatusa.Status = صرف_المرتبات_الشهرى.صرف;
                        db.Entry(EmployeeStatusa).State = EntityState.Modified;
                        db.SaveChanges();
                    }
              
              
            }
            db.Dispose();
            return true;
        }
        /// <summary>
        /// الاتيان بجميع المرتبات  للشهر الحالي
        /// </summary>
        /// <returns></returns>
        public static List<EmployeeSalaryMonth> getAllSalaryInThatMounth()
        {
            var db = new AccountingContainer();
          
            var time = db.EmployeeSalaryMonths.Where(a => a.Date.Year == DateTime.Today.Year);
            var timeMonth = time.Where(a => a.Date.Month == DateTime.Today.Month).ToList();
            return timeMonth;



        }
       /// <summary>
       /// Report for Employee get Recipts and invoice that Employee worked on it
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
        public static Employee_SalesVM FillEmplyeeSales(int id)
        {
            var db = new AccountingContainer();
            var EmpDetails = db.Employee_Sales.Where(e=>e.Employee_ID==id).ToList().FirstOrDefault();
            var allInvoices = db.InvoiceMasters.Where(s => s.Emp_Id == id && s.Invoice_State == TransactionState.Post).ToList();
            var thisEmployeeSales = db.Employee_Sales.Where(t => t.Employee_ID == id).FirstOrDefault();

            var EMPSv = new Employee_SalesVM();
            decimal totalinv = 0;
            decimal totalMatch = 0;
            if (allInvoices != null)
            {

                foreach (var inv in allInvoices)
                {
                    totalinv += inv.Total_Amount;
                    var allMatchs = db.Receipts.Where(r => r.Invoice_ID == inv.Master_Id && r.Rec_state == TransactionState.Post).ToList();
                    if (allMatchs != null)
                    {
                        foreach (var mat in allMatchs)
                        {
                            totalMatch += mat.TotalAmountAfMat;
                        }
                    }
                }
            }
            //var ESVM = new Employee_SalesVM();
            var EmployeeSettings = db.Sales_Settings.Where(e => e.Employee_ID == id).ToList().FirstOrDefault();
            if (EmployeeSettings != null)
            {
                if (EmployeeSettings.SalesMan == true && EmployeeSettings.Collector == true)
                {
                    EMPSv.Employee_ID = id;
                    EMPSv.Net_Sales = totalinv;
                    EMPSv.Collected = totalMatch;
                    if (thisEmployeeSales != null)
                    {
                        EMPSv.Target_Collected = thisEmployeeSales.Target_Collected;
                        EMPSv.Target_Sales = thisEmployeeSales.Target_Sales;
                    }

                }
                else if (EmployeeSettings.SalesMan == false && EmployeeSettings.Collector == true)
                {
                    EMPSv.Employee_ID = id;
                    EMPSv.Net_Sales = 0;
                    EMPSv.Collected = totalMatch;
                    if (thisEmployeeSales != null)
                    {
                        EMPSv.Target_Collected = thisEmployeeSales.Target_Collected;
                        EMPSv.Target_Sales = thisEmployeeSales.Target_Sales;
                    }
                }
                else if (EmployeeSettings.SalesMan == true && EmployeeSettings.Collector == false)
                {
                    EMPSv.Employee_ID = id;
                    EMPSv.Net_Sales = totalinv;
                    EMPSv.Collected = 0;
                    if (thisEmployeeSales != null)
                    {
                        EMPSv.Target_Collected = thisEmployeeSales.Target_Collected;
                        EMPSv.Target_Sales = thisEmployeeSales.Target_Sales;
                    }
                }
            }
            else {
                EMPSv.Target_Collected = thisEmployeeSales.Target_Collected;
                EMPSv.Target_Sales = thisEmployeeSales.Target_Sales;
                EMPSv.Employee_ID = thisEmployeeSales.Employee_ID;
                return EMPSv; }

            return EMPSv;

        }

        /// <summary>
        /// For making Report for number of Saling and Recipt that employee Worked For
        /// </summary>
        /// <param name="ESVM"></param>
        /// <returns></returns>
        public static string AddDetailsToEMployeeSales(Employee_SalesVM ESVM)
        {
            var db = new AccountingContainer();
            var thisdetail = db.Employee_Sales.Where(e => e.Employee_ID == ESVM.Employee_ID).ToList().FirstOrDefault();
            if (thisdetail == null)
            {
                var newdetail = new Employee_Sales();
                newdetail.Employee_ID = ESVM.Employee_ID;
                newdetail.Net_Sales = ESVM.Net_Sales;
                newdetail.Collected = ESVM.Collected;
                newdetail.Target_Collected = ESVM.Target_Collected;
                newdetail.Target_Sales = ESVM.Target_Sales;
                db.Employee_Sales.Add(newdetail);
                db.SaveChanges();
            }
            else
            {
                thisdetail.Employee_ID = ESVM.Employee_ID;
                thisdetail.Net_Sales = ESVM.Net_Sales;
                thisdetail.Collected = ESVM.Collected;
                thisdetail.Target_Collected = ESVM.Target_Collected;
                thisdetail.Target_Sales = ESVM.Target_Sales;
                db.Entry(thisdetail).State = EntityState.Modified;
                db.SaveChanges();
            }
            db.Dispose();
            return "";
        }

        /// <summary>
        /// make Registeration for Employee and save User id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Empid"></param>
        /// <returns></returns>
        public static string TakeIDString(string id, int Empid,string email,int RoleId)
        {
            var db = new AccountingContainer();
            var thisemp = db.Employees.Find(Empid);
            thisemp.Reg_ID = id;
            thisemp.Emp_Mail = email;
            thisemp.SystemRoleId = RoleId;
            var client = db.Clients.Where(a => a.PackageId != 0 && a.PackageId != null).FirstOrDefault();
                if (client != null)
            {
                thisemp.Client_Id = client.Id;
            }
            db.Entry(thisemp).State = EntityState.Modified;
            db.SaveChanges();
            db.Dispose();
            return "";

        }
        /// <summary>
        /// تعديل دور الموظف بالنظام
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static EmployeeRoleEditVM EditRoleForEditting(int id)
        {
            var db = new AccountingContainer();
            var vm = new EmployeeRoleEditVM();
        //    var allRoless = (new ApplicationDbContext()).Roles.OrderBy(r => r.Name).ToList();
        //    List<IdentityRole> RoleList = new List<IdentityRole>();

        //    foreach (var role in allRoless)
        //    {
        //        if (role.Name == "عميل")
        //        {
        //            role.Name = "Admin";

        //        }
        //        RoleList.Add(role);
        //    }



        //    var allRoles = RoleList.Select(rr =>

        //new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList().Where(a => a.Text != "مالك النظام");
            vm.selectrole = new SelectList(db.SystemRoles, "Id", "Name");
            var emp = db.Employees.Find(id);
            if (emp.SystemRoleId != null)
            {
                vm.roleIDMain = emp.SystemRoleId.ToString();
            }
            vm.EMployeeID = emp.Reg_ID;
           
            vm.EmployName = emp.Emp_Name;

            return vm;
        }

        public static int EditRoleForEditting(EmployeeRoleEditVM vm)
        {
            var db = new AccountingContainer();
            var emp = db.Employees.Where(a=>a.Reg_ID==vm.EMployeeID).FirstOrDefault();
            emp.SystemRoleId = Convert.ToInt32( vm.roleIDMain);
            db.Entry(emp).State = EntityState.Modified;
            db.SaveChanges();
            return emp.Emp_Id;
        }



        /// <summary>
        /// get List Of Employee Role in System 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static List<UserRoleVM> GetRoleANdUserNames(List<UserRoleVM> s)
        {
            var db = new AccountingContainer();
            var cont = new ApplicationDbContext();
            List<string> emploName = new List<string>();
            List<string> roleNames = new List<string>();
            List<Tuple<string, string, string, string,string>> res = new List<Tuple<string, string, string, string,string>>();
            string email = "";
            string EmpName = "";
            string roleID = "";
            string roleName = "";
            string UserID = "";
            if (s == null || s.Count == 0)
            { return s; }
            else
            {
                foreach (var EName in s)
                {
                    email = EName.fname;
                    var data = db.Employees.ToList();
                    if (data == null)
                        break;
                    foreach (var item in data)
                    {
                        if (item.Reg_ID == EName.UserID)
                        {
                            EmpName = db.Employees.Where(e => e.Reg_ID == EName.UserID).First().Emp_Name;
                            roleID = EName.rname;
                            roleName = cont.Roles.Where(e => e.Id == roleID).ToList().FirstOrDefault().Name;
                            UserID = EName.UserID;
                            res.Add(Tuple.Create(EmpName, roleName.ToString(), EName.fname2, EName.rname2,UserID));


                        }
                        else
                        {


                        }

                    }

                }
                var result = (from empl in res
                              select new UserRoleVM
                              {
                                  fname = empl.Item1,
                                  rname = empl.Item2,
                                  fname2 = empl.Item3,
                                  rname2 = empl.Item4,
                                  UserID=empl.Item5
                              }).Distinct().ToList();

                return result;
            }

        }



        /// <summary>
        /// Register Id by using Email
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public static string GetEmployeeRegID(string Email)
        {
            var db = new AccountingContainer();
            var thisEmployee = db.Employees.Where(e => e.Emp_Mail == Email).ToList().FirstOrDefault().Reg_ID;
           
            return thisEmployee;
        }

        /// <summary>
        /// Vm For Creting New Role
        /// </summary>
        /// <returns></returns>

        public static RoleCreateVM RoleCreateVM()
        {
            var cont = new ApplicationDbContext();
            var lasRole = cont.Roles.ToList();
            int max = 0;
            foreach (var last in lasRole)
            {
                if (Convert.ToInt32(last.Id) > max)
                {
                    max = Convert.ToInt32(last.Id);
                }
            }
            var intID = Convert.ToInt32(max) + 1;
            var rCVM = new RoleCreateVM();
            rCVM.ID = intID.ToString();
            return rCVM;

        }
        /// <summary>
        /// اضافة صلاحية جديدة لموظف معيين
        /// </summary>
        /// <param name="RCM"></param>
        /// <returns></returns>
        public static string addRole(RoleCreateVM RCM)
        {
            var cont = new ApplicationDbContext();
            var sendedRole = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
            sendedRole.Id = RCM.ID;
            sendedRole.Name = RCM.Name;
            cont.Roles.Add(sendedRole);
            cont.SaveChanges();
            cont.Dispose();
            return "تم اضافة الصلاحية";

        }
        /// <summary>
        /// Get all Role in system except العميل
        /// </summary>
        /// <returns></returns>
        public static List<Microsoft.AspNet.Identity.EntityFramework.IdentityRole> ListofRole()
        {
            var cont = new ApplicationDbContext();
            var allROles = cont.Roles.Where(n=>n.Name != "عميل").ToList();
           

            return allROles;
        }

        /// <summary>
        /// تقرير المرتبات حيث يتم ادخال التاريخ لاتيان بمرتبات هذا الشهر
        /// </summary>
        /// <returns></returns>
        public static EmployeeSalVM GetAllSallryDate()
        {
            var EmpSal = new EmployeeSalVM();
            return EmpSal;

        }

        /// <summary>
        /// تقرير المرتبات حيث يتم ادخال التاريخ لاتيان بمرتبات هذا الشهر
        /// </summary>
        /// <returns>List<EmployeeSalaryMonth></returns>
        public static List<EmployeeSalaryMonth> getAllSallaryforThatMounth(string vM)
        {
            var db = new AccountingContainer();
            var Date = Convert.ToDateTime(vM);
            var EmpSalMounth = db.EmployeeSalaryMonths.Where(e => e.Date.Month == Date.Month && e.Date.Year == Date.Year).ToList();

            return EmpSalMounth;


        }
        /// <summary>
        /// تفاصيل مرتب معين لهذا الشهر
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public static EmployeeSalaryMonth getDetailOfThatSallary(int id)
        {
            var db = new AccountingContainer();
            var emp = db.EmployeeSalaryMonths.Find(id);
            return emp;

        }
        /// <summary>
        /// معرفة سلفيات الموظف عن طريق كود الموظف
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static decimal getLoanOEmployee(String code)
        {
            var db = new AccountingContainer();
            decimal loan = db.Accounts.Where(a => a.Account_Code == code).FirstOrDefault().Account_Balance.Value;
            return loan;

        }
        /// <summary>
        /// الاتيان بالموظف عن طريق user Id
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public static Employee GetEmployeeRegIDID(string Email)
        {
            var db = new AccountingContainer();
            var thisEmployee = db.Employees.Where(e => e.Reg_ID == Email).ToList().FirstOrDefault();

            return thisEmployee;
        }
        public static string removeUserAccount(int id)
        {
            var db = new AccountingContainer();
            var Emp = db.Employees.Find(id);
            var Identityid = Emp.Reg_ID;
            if (Emp.Reg_ID != null)
            {
                Emp.Reg_ID = null;

                db.Entry(Emp).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
            }
            return Identityid;


        }

        public static string deleteEmplyee(int id)
        {
            var db = new AccountingContainer();
            var emp = db.Employees.Find(id);
            var account = db.Accounts.Where(a => a.Account_Code == emp.Emp_Code).FirstOrDefault();
            if (db.GLDetails.Where(a=>a.Account_Child_ID==account.Accounts_Id).Count()>0)
            {
                return Resources.validate.Error;
            }
            if (emp.Reg_ID != null && emp.Reg_ID != "")
            {
                return Resources.validate.Error;
            }
            if (db.TransactionBatches.Where(a=>a.UserId==emp.Reg_ID).Count()>0 )
            {
                return Resources.validate.Error;
            }
            try
            {
                db.EmployeeFileDetails.RemoveRange(emp.EmployeeFileDetails);
                db.Accounts.Remove(account);
                db.Employees.Remove(emp);
                db.SaveChanges();
            }
            catch 
            {
                return Resources.validate.Error;
            }

               return Resources.validate.DeleteSuccessful;
        }

        public static EmpVacationVM prepareVacation()
        {
            var db = new AccountingContainer();
            var salaryVM = new EmpVacationVM();

            salaryVM.VacationTypeList = new SelectList(db.Vacation_Types, "Id", "Vacation_Type");


            return salaryVM;


        }
        public static string addVacationTemp(EmpVacationVM vacEmpVM)
        {
            if (vacEmpVM.Vacation_TypesId == 0 || vacEmpVM.Vacation_TypesId == null)
            {
                return Resources.validate.chooseVactionType;
            }
            if (vacEmpVM.DaysInyear == 0 || vacEmpVM.DaysInyear == null)
            {
                return Resources.validate.EnterNumberOfDay;
            }
            var db = new AccountingContainer();
            var EmpTemp = db.EmployeTemps.Find(vacEmpVM.Employee_Id);

            var details = EmpTemp.EmpVacationTemps;
            var duplecatTemp = details.Where(s => s.EmpTemp_Id == EmpTemp.EmpTemp_Id && s.Vacation_TypesId == vacEmpVM.Vacation_TypesId).ToList();
            if (duplecatTemp.FirstOrDefault() != null)
            {
                  return Resources.validate.thisIsTyprOfVactionisExist; ;
            }
            else
            {
               
                var vacEmpTemp = new EmpVacationTemp();
                vacEmpTemp.Vacation_TypesId = vacEmpVM.Vacation_TypesId;
                vacEmpTemp.DaysInyear = vacEmpVM.DaysInyear;
                vacEmpTemp.VacationTypeName = db.Vacation_Types.Find(vacEmpTemp.Vacation_TypesId).Vacation_Type;
                EmpTemp.EmpVacationTemps.Add(vacEmpTemp);
                db.Entry(EmpTemp).State = EntityState.Modified;
                db.SaveChanges();
                     return Resources.validate.SuccessfullAdd; ;
            }
            
        }
        public static List<EmpVacationTemp> GetAllEmpVacationDetails(int EmpId)
        {
            var db = new AccountingContainer();

            var Details = db.EmpVacationTemps.Where(invoice => invoice.EmpTemp_Id == EmpId).ToList();

            return Details;
        }
        public static string DeleteEmpVacation(int vacId)
        {
            var db = new AccountingContainer();
            EmpVacationTemp vactemp = db.EmpVacationTemps.Find(vacId);
            db.EmpVacationTemps.Remove(vactemp);
            db.SaveChanges();
            db.Dispose();
             return Resources.validate.DeleteSuccessful; 
        }
        public static List<EmpVacationTemp> GetallEmpDetailsVacationTemp(int EmpId)
        {
            var db = new AccountingContainer();
            List<EmpVacationTemp> ListTemp = new List<EmpVacationTemp> { };
            var DetailsTemp = db.EmpVacationTemps.Where(C => C.EmpTemp_Id == EmpId).ToList();
            if (DetailsTemp.FirstOrDefault() != null)
            {
                db.EmpVacationTemps.RemoveRange(DetailsTemp);
                db.SaveChanges();
            }
            else { }
            var Vacation = db.EmpVacations.Where(a => a.EmployeeEmp_Id == EmpId);

            foreach (var item in Vacation)
            {
                var tempComp = new EmpVacationTemp();
                tempComp.Vacation_TypesId = item.Vacation_TypesId;
                tempComp.DaysInyear = item.DaysInyear;

                tempComp.VacationTypeName = db.Vacation_Types.Find(tempComp.Vacation_TypesId).Vacation_Type;
                tempComp.EmpTemp_Id = EmpId;
                ListTemp.Add(tempComp);


            }
            db.EmpVacationTemps.AddRange(ListTemp);
            db.SaveChanges();



            return ListTemp;
        }
        public static String Import(HttpPostedFileBase excelfile)
        {
            var data = getdataFillter(excelfile);
         
            if (data.Item2.Count() == 0)
            {
                foreach (var item in data.Item1)
                {
                    var Emp = CreateEmployee(item);
                  

                }

               
                return "تم حفظ الملف بنجاح";

            }
            string combindedString = string.Join(" ,", data.Item2.ToArray());
            return "يوجد اخطاء فى الاسطر التالية" + combindedString;
        }


        public static Tuple<List<EmpVM>, List<String>> getdataFillter(HttpPostedFileBase excelfile)
        {
            var EmpList = new List<EmpVM> { };
            var messageList = new List<String> { };
            var db = new AccountingContainer();
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                return Tuple.Create(EmpList, messageList);

            }

            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    // string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + excelfile.FileName);
                    var nameOfFile = "Employee" + DateTime.Now.ToBinary() + ".xlsx";
                    string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + nameOfFile);

                    excelfile.SaveAs(path);


                    Application application = new Application();
                    Workbook workbook = application.Workbooks.Open(path);
                    Worksheet worksheet = workbook.ActiveSheet;
                    Range range = worksheet.UsedRange;
                    for (int row = 3; row <= range.Rows.Count; row++)
                    {
                        EmpVM vm = new EmpVM();
                        // cust.Customer_Id = int.Parse(((Range)range.Cells[row, 1]).Text);
                        vm.Emp_Name = ((Range)range.Cells[row, 1]).Text;
                        if (vm.Emp_Name == "")
                        {
                            messageList.Add(row.ToString());
                            continue;
                        }
                        vm.Emp_Address = ((Range)range.Cells[row, 2]).Text;
                       

                        vm.Emp_Phone = ((Range)range.Cells[row, 3]).Text;
                        if (vm.Emp_Phone == "")
                        {
                            messageList.Add(row.ToString());
                            continue;
                        }
                        vm.Emp_Mail = ((Range)range.Cells[row, 4]).Text;
                        vm.Passport_number = ((Range)range.Cells[row, 3]).Text;
                        if (vm.Passport_number == "")
                        {
                            messageList.Add(row.ToString());
                            continue;
                        }

                        EmpList.Add(vm);







                    }
                    workbook.Save();
                    workbook.Close(path);



                    // File.Delete(path);

                }
            }
            return Tuple.Create(EmpList, messageList);
        }

        /// <summary>
        /// For validation of errors for treasury file
        /// </summary>
        /// <param name="treaVM"></param>
        /// <returns></returns>
        /// 
        public static bool getAllFunctionForEmployee(string userId,string functionName)
        {
            if (userId == "1449d4de-4cc3-43c5-b8a8-db3c13fcad9a")
            {
                return true;
            }
            var db = new AccountingContainer();
            var Employee = db.Employees.Where(a => a.Reg_ID == userId).FirstOrDefault();
           
            if (Employee.SystemRole != null)
            {
                if (Employee.SystemRole.Id == 6)
                {
                    return true;
                }
                var Allow = Employee.SystemRole.RoleFunctions.Where(a => a.SystemFunction.ArabName.Contains(functionName)).FirstOrDefault();
                if (Allow != null)
                {
                    return true;
                }

            }
            
            return false;
        }


        public static bool updateEmailEmpAndClient(ResetPasswordViewModel vm)
        {
            var db = new AccountingContainer();
            var Emp = db.Employees.Where(a => a.Emp_Code == "EMP-001").FirstOrDefault();
            Emp.Emp_Mail = vm.Email;
            var client = db.Clients.FirstOrDefault();
            client.E_Mail = vm.Email;
            db.Entry(Emp).State = EntityState.Modified;
            db.Entry(client).State = EntityState.Modified;
            db.SaveChanges();
            return true;
        }
    }
}
