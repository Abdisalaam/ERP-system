using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using System.Web.Mvc;
using System.IO;

namespace Accountingsystem.Models
{
    public partial class Vacations
    {
        /// <summary>
        /// get all type of vacation in db
        /// </summary>
        /// <returns>List<Vacation_Types> </returns>
        public static List<Vacations> getAllVacForEmployees()
        {
            var db = new AccountingContainer();
            var VacList = db.Vacations.ToList();
            return VacList;
        }
        public static List<Vacations> getAllVacInpenddingStatus()
        {
            var db = new AccountingContainer();
            var VacList = db.Vacations.Where(a=>a.Status==null).ToList();
            return VacList;
        }
        public static List<Vacations> getAllVacForThatEmployee(int id)
        {
            var db = new AccountingContainer();
            var VacList = db.Vacations.Where(a=>a.Employee_Id==id).ToList();
            return VacList;
        }
        /// <summary>
        /// get that type by its id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>Vacation_Types</returns>
        public static Vacations GetVacationByID(int ID)
        {
            var db = new AccountingContainer();
            Vacations vac = db.Vacations.Find(ID);

            return vac;
        }
        /// <summary>
        /// prepare for create anew vacation by id of Employee
        /// </summary>
        /// <param name="id"></param>
        /// <returns>VacationsVM</returns>
        public static VacationsVM PrepareVacForcreation(int id)
        {
            var db = new AccountingContainer();
            var VacVM = new VacationsVM();
            var emp = db.Employees.Find(id);
            List<object> newList2 = new List<object>();
            var members2 = emp.EmpVacations.ToList();
            foreach (var member1 in members2)
                newList2.Add(new
                {
                    Id = member1.Vacation_TypesId,
                    Name = member1.TotalAfterAbsent +member1.Vacation_Types.Vacation_Type+ " عدد الايام  "
                });
            VacVM.VacationTypeList = new SelectList(newList2, "Id", "Name");
           
          
            
            VacVM.Employee_Id = id;
            return VacVM;
        }
        /// <summary>
        /// save anew vaction in db 
        /// </summary>
        /// <param name="VVM"></param>
        /// <returns></returns>
        public static string AddNewVacationForEmployee(VacationsVM VVM)
        {
            var db = new AccountingContainer();
            var vac = new Vacations();
            try
            {
                
                var emp = db.Employees.Find(VVM.Employee_Id);
                var vactyp = db.Vacation_Types.Find(VVM.Vacation_Type_Id);
                vac.Employee_Id = VVM.Employee_Id;
                vac.End_Date_Vacations =Convert.ToDateTime( VVM.End_Date_Vacations);
                vac.Start_Date_Vacations = Convert.ToDateTime(VVM.Start_Date_Vacations);
                 var issuait=checkDateIsSuitable(VVM.Employee_Id, VVM.Start_Date_Vacations, VVM.End_Date_Vacations);
                if (issuait ==0)
                {
                    return Resources.validate.DateVacationWrong;
                }
                vac.Vacation_Type_Id = VVM.Vacation_Type_Id ;
                var today = DateTime.Today.ToString("dd");
                var MyYear = DateTime.Now.Year.ToString();
                var Mymonth = DateTime.Now.Month.ToString();
                //now i  have  last  day  on  the  month  i am  in
                vac.NumberOfDay = (vac.End_Date_Vacations - vac.Start_Date_Vacations).Days+1;
                vac.Reason = VVM.Reason;
                if (VVM.Attachment != null)
                {
                    //var data = new byte[prodVM.Pro_Img.ContentLength];
                    //product.Pro_Img = data;
                    var length = VVM.Attachment.InputStream.Length; //Length: 103050706
                    MemoryStream target = new MemoryStream();
                    vac.Attachname = VVM.Attachment.FileName;
                    VVM.Attachment.InputStream.CopyTo(target); // generates problem in this line
                    byte[] data = target.ToArray();
                    vac.Attachment = data;
                }
                vac.Status = null;
               
                db.Vacations.Add(vac);
                db.SaveChanges();
                db.Dispose();
                return Resources.validate.CreateSucc;

            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                throw;
            }
        }
        /// <summary>
        /// fill for Editing by using the id of vacation
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static VacationsVM FillTheViewModel(int ID)
        {
            var editModel = GetVacationByID(ID);
            var db = new AccountingContainer();
            VacationsVM VVM = new VacationsVM();
            VVM.Start_Date_Vacations = editModel.Start_Date_Vacations.ToString("dd-MM-yyyy   HH:mm:ss"); ;
            VVM.End_Date_Vacations = editModel.End_Date_Vacations.ToString("dd-MM-yyyy   HH:mm:ss"); ;
            VVM.Vacation_Type_Id = editModel.Vacation_Type_Id;
            var emp = db.Employees.Find(editModel.Employee_Id);
            List<object> newList2 = new List<object>();
            var members2 = emp.EmpVacations.ToList();
            foreach (var member1 in members2)
                newList2.Add(new
                {
                    Id = member1.Vacation_TypesId,
                    Name = member1.Vacation_Types.Vacation_Type
                });
            VVM.VacationTypeList = new SelectList(newList2, "Id", "Name");

            VVM.Employee_Id = editModel.Employee_Id;
            VVM.Vacation_Id = editModel.Vacation_Id;
            return VVM;

        }
        /// <summary>
        /// save change of vacation Edit in db
        /// </summary>
        /// <param name="VVM"></param>
        /// <returns>Vacations</returns>
        public static string EditVacation(VacationsVM VVM)
        {
            var db = new AccountingContainer();
            var emp = db.Employees.Find(VVM.Employee_Id);
            var vactyp = db.Vacation_Types.Find(VVM.Vacation_Type_Id);
            var vac = db.Vacations.Find(VVM.Vacation_Id);
            
            if (vac.Status == null)
            {
                vac.Employee_Id = VVM.Employee_Id;
                vac.End_Date_Vacations = Convert.ToDateTime(VVM.End_Date_Vacations);
                vac.Start_Date_Vacations = Convert.ToDateTime(VVM.Start_Date_Vacations);
                vac.Vacation_Type_Id = VVM.Vacation_Type_Id;
                var today = DateTime.Today.ToString("dd");
                var MyYear = DateTime.Now.Year.ToString();
                var Mymonth = DateTime.Now.Month.ToString();
                //now i  have  last  day  on  the  month  i am  in
                vac.NumberOfDay = (vac.End_Date_Vacations - vac.Start_Date_Vacations).Days + 1;
                vac.Reason = VVM.Reason;
                if (VVM.Attachment != null)
                {
                    //var data = new byte[prodVM.Pro_Img.ContentLength];
                    //product.Pro_Img = data;
                    var length = VVM.Attachment.InputStream.Length; //Length: 103050706
                    MemoryStream target = new MemoryStream();
                    VVM.Attachment.InputStream.CopyTo(target); // generates problem in this line
                    byte[] data = target.ToArray();
                    vac.Attachment = data;
                }
                vac.Status = null;


                db.Entry(vac).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                return "تم تعديل البيانات بنجاح";
            }
            else
            {
                return "لايمكنك التعديل فى هذه الاجازه";
            }
        }
        /// <summary>
        /// delete avation from data base
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>int</returns>
        public static int DeleteAnotherAsset(int Id)
        {
            var db = new AccountingContainer();
            AnotherAsset MyAsset = db.AnotherAssets.Find(Id);
            if (MyAsset != null)
            {

                db.AnotherAssets.Remove(MyAsset);
                db.SaveChanges();
                db.Dispose();
                return 1;
            }

            return 0;
        }

        public static int checkDateIsSuitable(int EmpId,string startDate,string endDate)
        {
            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;
            try
            {
                 start = Convert.ToDateTime(startDate);
                 end = Convert.ToDateTime(endDate);
            }
            catch
            {
                return 1;
            }
            if (end < start)
            {
                return 0;
            }
            var needVacation= new List<DateTime> { };
            while (start != end)
            {
                needVacation.Add(start);

                start=start.AddDays(1);

            }
            needVacation.Add(end);
            var db = new AccountingContainer();
            var vacationinthatyear = db.Vacations.Where(a => a.Employee_Id == EmpId&&a.Status==true).ToList();
            var readdayVacation = new List<DateTime> { };
            foreach(var vac in vacationinthatyear)
            {
                while (vac.Start_Date_Vacations != vac.End_Date_Vacations)
                {
                    needVacation.Add(vac.Start_Date_Vacations);

                    vac.Start_Date_Vacations = vac.Start_Date_Vacations.AddDays(1);

                }
                needVacation.Add(vac.End_Date_Vacations);

            }
            var commonlist = needVacation.Distinct().ToList();
            if (commonlist.Count() == needVacation.Count())
            {
                return 1;
            }

            return 0;

        }

        public static VacationsVM FillTheViewModelForAproveManger(int ID)
        {
            var editModel = GetVacationByID(ID);
            var db = new AccountingContainer();
            VacationsVM VVM = new VacationsVM();
            VVM.Start_Date_Vacations = editModel.Start_Date_Vacations.ToString("dd-MM-yyyy   HH:mm:ss"); ;
            VVM.End_Date_Vacations = editModel.End_Date_Vacations.ToString("dd-MM-yyyy   HH:mm:ss"); ;
            VVM.Vacation_Type_Id = editModel.Vacation_Type_Id;
            var emp = db.Employees.Find(editModel.Employee_Id);
            List<object> newList2 = new List<object>();
            var members2 = emp.EmpVacations.ToList();
            foreach (var member1 in members2)
                newList2.Add(new
                {
                    Id = member1.Vacation_TypesId,
                    Name = member1.TotalAfterAbsent + member1.Vacation_Types.Vacation_Type + " عدد الايام  "
                });
            VVM.VacationTypeList = new SelectList(newList2, "Id", "Name");
            List<object> newList4 = new List<object>();
            newList4.Add(new
            {
                Id = true,
                Name ="موافقة"

            });
            newList4.Add(new
            {
                Id = false,
                Name = "رفض"

            });
            VVM.StatusList= new SelectList(newList4, "Id", "Name");
      
            VVM.Employee_Id = editModel.Employee_Id;
            VVM.Vacation_Id = editModel.Vacation_Id;
            VVM.AttachmentByte = editModel.Attachment;
            VVM.Reason = editModel.Reason;
            return VVM;

        }
        /// <summary>
        /// save change of vacation Edit in db
        /// </summary>
        /// <param name="VVM"></param>
        /// <returns>Vacations</returns>
        public static string ApprovornotMangerVacation(VacationsVM VVM)
        {
            var db = new AccountingContainer();
            var emp = db.Employees.Find(VVM.Employee_Id);
            var vactyp = db.Vacation_Types.Find(VVM.Vacation_Type_Id);
            var vac = db.Vacations.Find(VVM.Vacation_Id);

            if (vac.Status == null)
            {
                vac.Employee_Id = VVM.Employee_Id;
                vac.End_Date_Vacations = Convert.ToDateTime(VVM.End_Date_Vacations);
                vac.Start_Date_Vacations = Convert.ToDateTime(VVM.Start_Date_Vacations);
                var issuait = checkDateIsSuitable(VVM.Employee_Id, VVM.Start_Date_Vacations, VVM.End_Date_Vacations);
                if (issuait == 0)
                {
                    return Resources.validate.DateVacationWrong;
                }
                vac.Vacation_Type_Id = VVM.Vacation_Type_Id;
                vac.Status = VVM.Status;
                var empvac = vac.Employee.EmpVacations.Where(a => a.Vacation_TypesId == vac.Vacation_Types.Id).FirstOrDefault();
                if (VVM.Status == true)
                {
                    decimal AllPenality = 0;
                    decimal exctraPenality = 0;
                    var salary = new Salary_Components();
                    var typeOfSal = db.TypeOfDetailSalaries.Where(a => a.SalaryDetail_Name == vac.Vacation_Types.Vacation_Type).FirstOrDefault();
                    salary.Account_Id = typeOfSal.Id;
                    salary.Account_Name = typeOfSal.SalaryDetail_Name;

                    var different = empvac.TotalAfterAbsent - vac.NumberOfDay;
                    var salinday = vac.Employee.TotalSalaryBeforPenality / 30;
                    if (different >= 0)
                    {

                        salinday = vac.Employee.TotalSalaryBeforPenality / 30;
                        AllPenality = (salinday.Value * vac.NumberOfDay * vac.Vacation_Types.PenalityDay) + vac.NumberOfDay * (vac.Vacation_Types.penalityPercentage / 100 * vac.Employee.TotalSalaryBeforPenality).Value;

                        empvac.TotalAfterAbsent -= vac.NumberOfDay;
                        vac.Employee.Total_Salary -= AllPenality + exctraPenality;
                    }
                    else
                    {


                        AllPenality = (salinday.Value * empvac.TotalAfterAbsent * vac.Vacation_Types.PenalityDay) + (empvac.TotalAfterAbsent * vac.Vacation_Types.penalityPercentage / 100 * vac.Employee.TotalSalaryBeforPenality).Value;
                        empvac.TotalAfterAbsent = 0;
                        var exctravacation = different * -1;
                        exctraPenality = (salinday.Value * exctravacation * vac.Vacation_Types.PenalityDayAfter) + (exctravacation * vac.Vacation_Types.PenalityPercentageAfter / 100 * vac.Employee.TotalSalaryBeforPenality).Value;
                        vac.Employee.Total_Salary -= AllPenality + exctraPenality;

                    }
                    salary.Employee_Id = vac.Employee_Id;
                    salary.Account_Amount = exctraPenality + AllPenality;
                    salary.VacationId = vac.Vacation_Id;
                    if (salary.Account_Amount != 0)
                    {
                        db.Salary_Components.Add(salary);
                    }
                }
                //now i  have  last  day  on  the  month  i am  in
                vac.NumberOfDay = (vac.End_Date_Vacations - vac.Start_Date_Vacations).Days + 1;
                vac.Reason = VVM.Reason;



                vac.MangerDescription = VVM.MangerDescription;
                db.Entry(vac).State = System.Data.Entity.EntityState.Modified;

                db.Entry(empvac).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                if (VVM.Status == true)
                {
                    return Resources.validate.vactionaccept;
                }
                else
                {
                    return Resources.validate.vacationrefused;
                }


            }
            else
            {
                return Resources.validate.vacationpendding;
            }

        }

        public static EmpVM makevmforEmployee()
        {
            var vm = new EmpVM();
            List<Employee> empList = new List<Employee>();
            var stat1 = new Employee();
            stat1.Emp_Id = 1;
            stat1.Emp_Name = "طلب اجازه";
            empList.Add(stat1);
            var stat2 = new Employee();
            stat2.Emp_Id = 2;
            stat2.Emp_Name = "طلب سلفه";
            empList.Add(stat2);

            vm.Branch_List = new SelectList(empList, "Emp_Id", "Emp_Name");
            return vm;

        }


        public static int IsEmployeeOrNot(EmpVM vm)
        {
            var db = new AccountingContainer();
            var emp = db.Employees.Where(a => a.Passport_number == vm.Passport_number).FirstOrDefault();
            if (emp == null)
            {
                return 0;
            }
            else
            {
                return emp.Emp_Id;
            }

        }
        public static string makeEmgrationOfVacation()
        {
            var db = new AccountingContainer();
            var EmpVactionList = db.EmpVacations.Where(a => a.Vacation_Types.TransferToNewYear == true).ToList();
            if (db.VacationEmgrations.Where(a => a.DateOfTransaction.Year == DateTime.Now.Year && a.DateOfTransaction.Month == DateTime.Now.Month).FirstOrDefault() != null)
            {
                return Resources.validate.DeportationExist;
            }
            var empvacList = new List <VacationEmgration>();
            foreach(var vac in EmpVactionList)
            {
                var empvac = new VacationEmgration();
                empvac.NumberOfDay = vac.TotalAfterAbsent;
                empvac.DateOfTransaction = DateTime.Now;
                empvac.Employee_Id = vac.EmployeeEmp_Id;
                empvac.Vacation_Type_id = vac.Vacation_TypesId;
                empvacList.Add(empvac);
               
                vac.TotalAfterAbsent = vac.DaysInyear+vac.TotalAfterAbsent;

                db.Entry(vac).State = System.Data.Entity.EntityState.Modified;
            }

            db.VacationEmgrations.AddRange(empvacList);
            db.SaveChanges();
            db.Dispose();

            return Resources.validate.DeportationSuccess;

        }
      

        
    }
}