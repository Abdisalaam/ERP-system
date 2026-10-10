using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class ShiftCoding
    {
        public static List<ShiftCoding> GetAllShiftCoding()
        {
            var db = new AccountingContainer();
            var ShiftCodingList = db.ShiftCodings.Where(a=>a.Active==true).ToList();

            return ShiftCodingList;
        }

        // use this method to   Get one element CategoryById from Database.  
        public static ShiftCoding GetShiftCodingsById(int id)
        {
            var db = new AccountingContainer();
            var ShiftCoding = db.ShiftCodings.Find(id);
            return ShiftCoding;
        }

        // use this method to   Get one element CategoryByCode from Database.  
        

        // use this method For Creation new Catagery and get Data from user.
        public static ShiftCodingVM CreateVMForCreation()
        {
            var db = new AccountingContainer();
            var vm = new ShiftCodingVM();
      

            vm.WeekHolidayList = new SelectList(db.VacationWeeklyCodings.ToList(), "Id", "Name");
            var EmployeeList = db.Employees.Where(a => a.Active != false);
            vm.EmployeeList = new List<EmployeeShiftCheckVM>();

            foreach (var Emp in EmployeeList)
            {
                var EmpShift = new EmployeeShiftCheckVM();
                EmpShift.Emp = Emp;             
                EmpShift.IsActive = false;
                vm.EmployeeList.Add(EmpShift);
            }


            return vm;
        }

        // use this method to  Create New Catagery and save it in DataBase.
        public static int CreateShiftCoding(ShiftCodingVM vm)
        {
            var db = new AccountingContainer();

            var shiftCoding = new ShiftCoding();

            shiftCoding.From = TimeSpan.Parse(vm.From);
            shiftCoding.To = TimeSpan.Parse(vm.To);
            shiftCoding.Name = vm.Name;
            
           var Difference = shiftCoding.To-shiftCoding.From;
            if (Difference.Hours > 0)
            {
                shiftCoding.Hours = Difference.Hours;
            }
           
            else{
                shiftCoding.Hours = Difference.Hours*-1;
            }
            shiftCoding.Minute = Difference.Minutes;
            if (Difference.Minutes > 0)
            {
                shiftCoding.Minute = Difference.Minutes;
            }
            else
            {
                shiftCoding.Minute =Difference.Minutes*-1;
            }
            shiftCoding.PercentageOfAdditionHours = vm.PercentageOfAdditionHours;
            if (vm.WeekHollidays != null)
            {
                foreach (var vacatinonCoding in vm.WeekHollidays)
                {
                    var vacationWeekly = new VacationWeekly();
                    vacationWeekly.VacationWeeklyCodingId =Convert.ToInt32(vacatinonCoding);
                    shiftCoding.VacationWeeklies.Add(vacationWeekly);

                }
            }
            if (vm.EmployeeList != null)
            {
                if (vm.EmployeeList.Where(a => a.IsActive == true) != null)
                {
                    foreach (var emp in vm.EmployeeList.Where(a => a.IsActive == true))
                    {
                        var shift = new Shift();
                        shift.EmployeeEmp_Id = emp.EmployeeId;

                       
                        var empo = db.Employees.Find(emp.EmployeeId);
                        empo.NumberOfHour = vm.NumberOfHour;
                        db.Entry(empo).State = EntityState.Modified;
                        shiftCoding.Shifts.Add(shift);

                    }
                }
            }
            shiftCoding.Active = true;
            db.ShiftCodings.Add(shiftCoding);
            db.SaveChanges();
            db.Dispose();
            return shiftCoding.Id;
        }

        // use this method to load Details of Data for Category.
        public static ShiftCodingVM FillTheViewModel(int id)
        {
            var db = new AccountingContainer();
            var shift = db.ShiftCodings.Find(id);
            ShiftCodingVM vm = new ShiftCodingVM();
            vm.From = new DateTime(shift.From.Ticks).ToString("HH:mm");
            vm.To = new DateTime(shift.To.Ticks).ToString("HH:mm");
            vm.WeekHollidays = shift.VacationWeeklies.Select(a => a.VacationWeeklyCodingId.ToString()).ToList();
            vm.Name = shift.Name;
            vm.PercentageOfAdditionHours = shift.PercentageOfAdditionHours;
            vm.WeekHolidayList = new SelectList(db.VacationWeeklyCodings.ToList(), "Id", "Name");
            vm.EmployeeList = new List<EmployeeShiftCheckVM>();

            foreach (var employee in db.Employees.Where(a=>a.Active!=false).ToList())
            {
                var EmpShift = new EmployeeShiftCheckVM();
                EmpShift.Emp = employee;
                if (shift.Shifts.Where(a => a.EmployeeEmp_Id == employee.Emp_Id).FirstOrDefault() == null)
                {
                    EmpShift.IsActive = false;
                }
                else
                {
                    EmpShift.IsActive = true;
                }

                vm.EmployeeList.Add(EmpShift);


            }

            return vm;
           

        }

        // use this method to Edite Catgery and save it in DataBase.
        public static ShiftCoding EditeCatgery(ShiftCodingVM vm)
        {
            var db = new AccountingContainer();

            var shiftCoding = db.ShiftCodings.Find(vm.Id);

            shiftCoding.From = TimeSpan.Parse(vm.From);
            shiftCoding.To = TimeSpan.Parse(vm.To);
            shiftCoding.Name = vm.Name;

            var Difference = shiftCoding.To - shiftCoding.From;
            if (Difference.Hours > 0)
            {
                shiftCoding.Hours = Difference.Hours;
            }

            else
            {
                shiftCoding.Hours = Difference.Hours * -1;
            }
            shiftCoding.Minute = Difference.Minutes;
            if (Difference.Minutes > 0)
            {
                shiftCoding.Minute = Difference.Minutes;
            }
            else
            {
                shiftCoding.Minute = Difference.Minutes * -1;
            }
            shiftCoding.PercentageOfAdditionHours = vm.PercentageOfAdditionHours;
            var vacationList = new List<VacationWeekly>();
            shiftCoding.VacationWeeklies = null;
            shiftCoding.VacationWeeklies = new List<VacationWeekly>();
            if (vm.WeekHollidays != null)
            {
                foreach (var vacatinonCoding in vm.WeekHollidays)
                {
                    var vacationWeekly = new VacationWeekly();
                    vacationWeekly.VacationWeeklyCodingId = Convert.ToInt32(vacatinonCoding);




                    shiftCoding.VacationWeeklies.Add(vacationWeekly);
                
                }
            }
            shiftCoding.Shifts = null;
            shiftCoding.Shifts = new List<Shift>();
            if (vm.EmployeeList != null)
            {
                foreach (var emp in vm.EmployeeList.Where(a=>a.IsActive==true))
                {
                    var shift = new Shift();
                    shift.EmployeeEmp_Id = emp.EmployeeId;




                    shiftCoding.Shifts.Add(shift);

                }
            }
            db.Shifts.RemoveRange(shiftCoding.Shifts.Where(a => a.Id > 0).ToList());






            db.VacationWeeklies.RemoveRange(shiftCoding.VacationWeeklies.Where(a=>a.Id>0).ToList());
          
            db.Entry(shiftCoding).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                return shiftCoding;
            
        }

        public static EmployeeShiftVM GetEmployeeShift(int id)
        {
            var db = new AccountingContainer();
            var EmployeeList = db.Employees.Where(a=>a.Active!=false);
            EmployeeShiftVM ShiftVM = new EmployeeShiftVM();
            ShiftVM.shiftCodeing = db.ShiftCodings.Find(id);
            ShiftVM.EmployeeList = new List<EmployeeShiftCheckVM>();

            foreach (var Emp in EmployeeList)
            {
                var EmpShift = new EmployeeShiftCheckVM();
                EmpShift.Emp = Emp;
                if(Emp.Shifts.Where(a => a.ShiftCodingId == id).FirstOrDefault() == null)
                {
                    EmpShift.IsActive = false;
                }
                else
                {
                    EmpShift.IsActive = true;
                }

                ShiftVM.EmployeeList.Add(EmpShift);


            }

            return ShiftVM;


        }
        public static EmployeeShiftVM GetEmployeeShiftAdding(EmployeeShiftVM vm)
        {
            var db = new AccountingContainer();
            var EmployeeList = db.Employees.Where(a => a.Active != false);
            EmployeeShiftVM ShiftVM = new EmployeeShiftVM();
            //ShiftVM.shiftCodeing = db.ShiftCodings.Find(id);
            //ShiftVM.EmployeeList = new List<EmployeeShiftCheckVM>();

            //foreach (var Emp in EmployeeList)
            //{
            //    var EmpShift = new EmployeeShiftCheckVM();
            //    EmpShift.Emp = Emp;
            //    if (Emp.Shifts.Where(a => a.ShiftCodingId == id).FirstOrDefault() == null)
            //    {
            //        EmpShift.Check = false;
            //    }
            //    else
            //    {
            //        EmpShift.Check = true;
            //    }

            //    ShiftVM.EmployeeList.Add(EmpShift);


            //}

            return ShiftVM;


        }
        public static string DeleteShiftTransfer(int id)
        {

            var db = new AccountingContainer();
            var shift = db.ShiftCodings.Find(id);
            if (shift.Active != false)
            {
                shift.Active = false;

            }
            else
            {
                shift.Active = true;
            }

            db.Entry(shift).State = EntityState.Modified;
            db.SaveChanges();


            return Resources.ProductDetails.TransferSucces;

        }
    }
}