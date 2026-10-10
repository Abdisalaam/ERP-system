using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class AttendingAndLeaving
    {
        public static AttendingAndLeavingVM PrepareattendingAndLeavingVM()
        {
            var db = new AccountingContainer();
            AttendingAndLeavingVM vm = new AttendingAndLeavingVM();
            vm.ShiftCodingList= new SelectList(db.ShiftCodings.Where(a=>a.Active==true).ToList(), "Id", "Name");
            vm.BranchList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.isActive != false && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
            return vm;


        }
        public static bool checkGLOfSallaryIsExcect(DateTime date)
        {
            var db = new AccountingContainer();
         
            var gla = db.GLMasters.Where(a => a.GLType_Id.StartsWith("مرتبات")).ToList();
            var time = gla.Where(a => a.GL_Date.Year == date.Year);
            var timeMonth = time.Where(a => a.GL_Date.Month == date.Month);
            var nullmounth = new GLMaster();
      

            try
            {
                nullmounth = timeMonth.ToList().Last();
                if (nullmounth.GLState == TransactionState.Void)
                {
                    return false;
                }


            }

            catch
            {
                return false;


            }
            return true;
        }
        public static AttendingAndLeavingVM GetAllEmpolyeeForThatDay(AttendingAndLeavingVM vm)
        {
            var db = new AccountingContainer();
            vm.AttendingAndLeavingDetailsList = new List<AttendingAndLeavingDetailsVM>();
            var date = Convert.ToDateTime(vm.Date);

                var shiftCode = db.ShiftCodings.Find(vm.ShiftCodingId);
            var check = GetDateIfItContain(shiftCode,date);
            if (check == false)
            {
                return null;
            }
            var oldAttending = db.AttendingAndLeavings.Where(a => a.ShiftCodingId == vm.ShiftCodingId && a.Date == date).ToList();
                if (vm.BranchId > 0)
                {
                    oldAttending = oldAttending.Where(a => a.Employee.BranchSubledger_Id == vm.BranchId).ToList();
                    vm.BranchName = db.SubLedgers.Where(a => a.SubLed_Id == vm.BranchId).FirstOrDefault().SubLed_Name;
                }
                vm.shiftCoding = shiftCode;
                if (oldAttending.Count() == 0)
                {
                    var AttendingAndLeavingD = new List<AttendingAndLeaving>();
                    if (vm.BranchId > 0)
                    {

                    foreach (var Emp in shiftCode.Shifts.Where(a => a.Employee.BranchSubledger_Id == vm.BranchId && a.Employee.Active == true))
                    {
                     


                            var AttendingAndLeavingDetails = new AttendingAndLeavingDetailsVM();
                            var AttendingAndLeaving = new AttendingAndLeaving();
                            AttendingAndLeaving.EmployeeEmp_Id = Emp.EmployeeEmp_Id;
                            AttendingAndLeaving.Date = date;
                            AttendingAndLeavingDetails.EmployeeEmp_Id = Emp.EmployeeEmp_Id;
                            AttendingAndLeavingDetails.Employee = Emp.Employee;
                            //AttendingAndLeavingDetails.From = null;
                            //AttendingAndLeavingDetails.To = null;

                            AttendingAndLeaving.ShiftCodingId = shiftCode.Id;
                            db.AttendingAndLeavings.Add(AttendingAndLeaving);
                            db.SaveChanges();
                            AttendingAndLeavingDetails.Attending = AttendingAndLeaving;
                            AttendingAndLeavingDetails.Id = AttendingAndLeaving.Id;
                            vm.AttendingAndLeavingDetailsList.Add(AttendingAndLeavingDetails);
                        }
                    }
                    

                    else
                    {
                        foreach (var Emp in shiftCode.Shifts.Where(a=>a.Employee.Active==true))
                    {
                       
                            var AttendingAndLeavingDetails = new AttendingAndLeavingDetailsVM();
                            var AttendingAndLeaving = new AttendingAndLeaving();
                            AttendingAndLeaving.EmployeeEmp_Id = Emp.EmployeeEmp_Id;
                            AttendingAndLeaving.Date = date;
                            AttendingAndLeavingDetails.EmployeeEmp_Id = Emp.EmployeeEmp_Id;
                            AttendingAndLeavingDetails.Employee = Emp.Employee;
                            //AttendingAndLeavingDetails.From = null;
                            //AttendingAndLeavingDetails.To = null;

                            AttendingAndLeaving.ShiftCodingId = shiftCode.Id;
                            db.AttendingAndLeavings.Add(AttendingAndLeaving);

                            db.SaveChanges();
                            AttendingAndLeavingDetails.Attending = AttendingAndLeaving;
                            AttendingAndLeavingDetails.Id = AttendingAndLeaving.Id;
                            vm.AttendingAndLeavingDetailsList.Add(AttendingAndLeavingDetails);
                        }
                    }
                    

                }
                else
                {
                    foreach (var old in oldAttending.ToList())
                    {
                        var AttendingAndLeavingDetails = new AttendingAndLeavingDetailsVM();

                        AttendingAndLeavingDetails.EmployeeEmp_Id = old.EmployeeEmp_Id;
                        AttendingAndLeavingDetails.Employee = old.Employee;
                        if (old.From != null)
                        {
                            AttendingAndLeavingDetails.From = new DateTime(old.From.Value.Ticks).ToString("HH:mm");
                        }
                        if (old.To != null)
                        {
                            AttendingAndLeavingDetails.To = new DateTime(old.To.Value.Ticks).ToString("HH:mm");
                        }
                        AttendingAndLeavingDetails.Attending = old;
                        AttendingAndLeavingDetails.Id = old.Id;
                        vm.AttendingAndLeavingDetailsList.Add(AttendingAndLeavingDetails);
                    }

                }
            
         
            return vm;
        }

        public static int PostNewAttendanceEdit(dynamic json )
        {
            var db = new AccountingContainer();
            var shifCode = 0;
            var RewardAndPenalties = db.RewardsAndPenaltiesCodings.Where(a => a.Active == true).ToList();
        
            var RewardAndPenaltiesAbsent = RewardAndPenalties.Where(a => a.RewardsAndPenaltiesCodingCategoryId == 3).FirstOrDefault();
            var ComeLate= RewardAndPenalties.Where(a => a.RewardsAndPenaltiesCodingCategoryId == 1).OrderBy(a=>a.TransformToMinute).ToList();
         
            var leaveearly= RewardAndPenalties.Where(a => a.RewardsAndPenaltiesCodingCategoryId == 2).OrderBy(a => a.TransformToMinute).ToList();
            var comelateMax = ComeLate.Where(a => a.IsMax == true).FirstOrDefault();
            var leaveearlymax = leaveearly.Where(a => a.IsMax == true).FirstOrDefault();
            foreach (var Attending in json)
            {
                AttendingAndLeaving Attend = db.AttendingAndLeavings.Find(Convert.ToInt32(Attending.Id));
                var penality = Attend.RewardsAndPenalties.Where(a => a.Id > 0&&a.DateOfTaken==null);
                bool Act = Attending.Active;
                if (Act == true)
                {
                    Attending.From =  Attend.ShiftCoding.From.ToString();
                    Attending.To = Attend.ShiftCoding.To.ToString();
                }
                if (Attending.From!=null&& Attending.From!="")
                {
                    if (Attend.RewardsAndPenalties.Count() > 0)
                    {
                        db.RewardsAndPenalties.RemoveRange(Attend.RewardsAndPenalties.Where(a => a.RewardsAndPenaltiesCoding.RewardsAndPenaltiesCodingCategoryId == 1||a.RewardsAndPenaltiesCoding.RewardsAndPenaltiesCodingCategoryId==3));
                        db.SaveChanges();
                    }
                    Attend.From = Attending.From;
                    Attend.StatisticFrom = (Attend.From.Value - Attend.ShiftCoding.From).Hours*60+ (Attend.From.Value - Attend.ShiftCoding.From).Minutes;
                    if (Attend.StatisticFrom.Value > 0&& ComeLate.FirstOrDefault()!=null)
                    {
                        //عقوبة تأخر عن العمل
                        var Penality = new RewardsAndPenalties();
                        Penality.DateOfTaken = null;
                        Penality.IsTaken = false;
                        var codeOfPunsh = AddPunishmentForGetLate(ComeLate, Attend.StatisticFrom.Value);
                        Penality.NumberOfHour = codeOfPunsh.RewordinHour + codeOfPunsh.RewordInDay * Attend.ShiftCoding.Hours;
                        Penality.RewardsAndPenaltiesCodingId = codeOfPunsh.Id;
                        Attend.RewardsAndPenalties.Add(Penality);
                    }
                   
                }
                if (Attending.To != null && Attending.To != "")
                {
                    if (penality.Count() > 0)
                    {
                        var k = penality.Where(a=>a.RewardsAndPenaltiesCoding.RewardsAndPenaltiesCodingCategoryId == 2 || a.RewardsAndPenaltiesCoding.RewardsAndPenaltiesCodingCategoryId == 3).ToList();
                        db.RewardsAndPenalties.RemoveRange(k);
                        db.SaveChanges();
                    }
                    Attend.To = Attending.To;
                    Attend.StatisticTo = (Attend.ShiftCoding.To- Attend.To.Value).Hours * 60 + (Attend.ShiftCoding.To - Attend.To.Value ).Minutes;
                    if (Attend.StatisticTo.Value > 0&& leaveearly.FirstOrDefault()!=null)
                    {
                        //عقوبة انصراف مبكر عن العمل
                        var Penality = new RewardsAndPenalties();
                        Penality.DateOfTaken = null;
                        Penality.IsTaken = false;
                        var codeOfPunsh = LeaveWorkEarly(leaveearly, Attend.StatisticTo.Value);
                        Penality.NumberOfHour = codeOfPunsh.RewordinHour + codeOfPunsh.RewordInDay * Attend.ShiftCoding.Hours;
                        Penality.RewardsAndPenaltiesCodingId = codeOfPunsh.Id;
                        Attend.RewardsAndPenalties.Add(Penality);
                    }
                }
                //غياب عن العمل 
             if(Attend.To == null && Attend.From != null && leaveearlymax != null)
                {
                    if (penality.Count() > 0)
                    {
                        var k = penality.Where(a => a.Id != 0).ToList();
                        db.RewardsAndPenalties.RemoveRange(k);
                        db.SaveChanges();
                    }
                    var Penality = new RewardsAndPenalties();
                    Penality.DateOfTaken = null;
                    Penality.IsTaken = false;
                    Penality.RewardsAndPenaltiesCodingId = leaveearlymax.Id;
                    Penality.NumberOfHour = leaveearlymax.RewordinHour + leaveearlymax.RewordInDay * Attend.ShiftCoding.Hours;                  
                    Attend.RewardsAndPenalties.Add(Penality);
                }
                if (Attend.To != null && Attend.From == null&& comelateMax!=null)
                {
                    if (penality.Count() > 0)
                    {
                        var k = penality.Where(a => a.Id != 0).ToList();
                        db.RewardsAndPenalties.RemoveRange(k);
                        db.SaveChanges();
                    }
                    var Penality = new RewardsAndPenalties();
                    Penality.DateOfTaken = null;
                    Penality.IsTaken = false;
                    Penality.RewardsAndPenaltiesCodingId = comelateMax.Id;
                    Penality.NumberOfHour = comelateMax.RewordinHour + comelateMax.RewordInDay * Attend.ShiftCoding.Hours;
                    Attend.RewardsAndPenalties.Add(Penality);
                }
                if (Attend.To != null&& Attend.From != null)
                {

                    Attend.Hours = (Attend.To - Attend.From).Value.Hours;
                    if (Attend.Hours < 0)
                    {
                        Attend.Hours = Attend.Hours * -1;
                    }
                    Attend.Minute = (Attend.To - Attend.From).Value.Minutes;
                    if (Attend.Minute < 0)
                    {
                        Attend.Minute = Attend.Hours * -1;
                    }




                }
                if (Attend.To == null && Attend.From == null&& RewardAndPenaltiesAbsent!=null)
                {
                    if (penality.Count() > 0)
                    {
                        var k = penality.Where(a => a.Id != 0).ToList();
                        db.RewardsAndPenalties.RemoveRange(k);
                        db.SaveChanges();
                    }
                    var Penality = new RewardsAndPenalties();
                    Penality.DateOfTaken = null;
                    Penality.IsTaken = false;
                    Penality.RewardsAndPenaltiesCodingId = RewardAndPenaltiesAbsent.Id;
                    Penality.NumberOfHour = RewardAndPenaltiesAbsent.RewordinHour + RewardAndPenaltiesAbsent.RewordInDay * Attend.ShiftCoding.Hours;
                    Attend.RewardsAndPenalties.Add(Penality);
                }


                db.Entry(Attend).State = EntityState.Modified;
                db.SaveChanges();
                shifCode = Attend.Id;
            }
            return shifCode;




        }

       
        public static RewardsAndPenaltiesCoding AddPunishmentForGetLate(List<RewardsAndPenaltiesCoding>codeList ,int differentTime)
        {
            foreach(var code in codeList)
            {
                if (differentTime <= code.TransformToMinute)
                {
                    return code;
                }
                else {
                    continue;
                        }
            }
            return codeList.Where(a=>a.IsMax==true).FirstOrDefault();
        }
        public static RewardsAndPenaltiesCoding LeaveWorkEarly(List<RewardsAndPenaltiesCoding> codeList, int differentTime)
        {
            foreach (var code in codeList)
            {
                if (differentTime <= code.TransformToMinute)
                {
                    return code;
                }
                else
                {
                    continue;
                }
            }
            return codeList.Where(a => a.IsMax == true).FirstOrDefault();
        }

        public static EmployeePenalitiesVM prepareAttending(int EmployeeId)
        {
            var db = new AccountingContainer();
            var EmpPenalitiesvm = new EmployeePenalitiesVM();
            EmpPenalitiesvm.employee = db.Employees.Find(EmployeeId);
            EmpPenalitiesvm.employee_Id = EmployeeId;
            return EmpPenalitiesvm;

        }
        public static EmployeePenalitiesVM prepareAttending(EmployeePenalitiesVM vm)
        {
            var db = new AccountingContainer();
            var Date = Convert.ToDateTime(vm.DateOfMounth);
       
                vm.PenaltiesList = new List<RewardAndPenaltiesVM>();
                vm.employee = db.Employees.Find(vm.employee_Id);
                var PenaltiesList = db.AttendingAndLeavings.Where(a => a.EmployeeEmp_Id == vm.employee_Id && a.Date.Value.Year == Date.Year && a.Date.Value.Month == Date.Month).SelectMany(a => a.RewardsAndPenalties).ToList();
                foreach (var penalty in PenaltiesList)
                {
                    RewardAndPenaltiesVM vmdetail = new RewardAndPenaltiesVM();
                    vmdetail.reward = penalty;
                    vmdetail.Description = penalty.Description;
                    vm.PenaltiesList.Add(vmdetail);
                }
                return vm;
       
       
        }
        public static int PostEditPenaltiy(dynamic json,int EmpId)
        {
            var db = new AccountingContainer();
         
            foreach (var Penalitiy in json)
            {
                RewardsAndPenalties penali = db.RewardsAndPenalties.Find(Convert.ToInt32(Penalitiy.Id));
                if (Penalitiy.NumberOfHour != null)
                {
                    penali.NumberOfHour = Penalitiy.NumberOfHour;
                }
                if (Penalitiy.Description != null&& Penalitiy.Description != "")
                {
                    penali.Description = Penalitiy.Description;
                }
                db.Entry(penali).State = EntityState.Modified;
         
                db.SaveChanges();

            }
            return EmpId;




        }
        public static decimal getSalaryForEmployeeStartActualyWork(decimal Salary,DateTime date)
        {
            var db = new AccountingContainer();
            var numberOfMounth = GetDates(date.Year, date.Month);
            var numberOfDayActualyNotWork = numberOfMounth.Where(a => a.Date < date).Count();
            var salary = (Salary / numberOfMounth.Count()) * numberOfDayActualyNotWork;
            return salary;
        }
        public static List<DateTime> GetDates(int year, int month)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month))  // Days: 1, 2 ... 31 etc.
                             .Select(day => new DateTime(year, month, day)) // Map each day to a date
                             .ToList(); // Load dates into a list
        }
        //بيرجع عدد الساعات التي يجب عملها في هذا الشهر 
        // مع حذف الاجازات الاسبوعية
        public static int GetDateOfShiftInMounth(ShiftCoding shift,DateTime Date,Employee emp)
        {
            var db = new AccountingContainer();
            var mounth = Date.Month;
            var Year = Date.Year;
            var Dayes = GetDates(Year, mounth);
           
            //if (shift.VacationWeeklies.Where(a => a.VacationWeeklyCodingId == 1).FirstOrDefault() != null) {
            //  Dayes= Dayes.Where(a => a.DayOfWeek != DayOfWeek.Friday).ToList();
            //    }
            //if (shift.VacationWeeklies.Where(a => a.VacationWeeklyCodingId == 2).FirstOrDefault() != null)
            //{
            //    Dayes= Dayes.Where(a => a.DayOfWeek != DayOfWeek.Saturday).ToList();
            //}
            //if (shift.VacationWeeklies.Where(a => a.VacationWeeklyCodingId == 3).FirstOrDefault() != null)
            //{
            //    Dayes= Dayes.Where(a => a.DayOfWeek != DayOfWeek.Sunday).ToList();
            //}
            //if (shift.VacationWeeklies.Where(a => a.VacationWeeklyCodingId == 4).FirstOrDefault() != null)
            //{
            //    Dayes=Dayes.Where(a => a.DayOfWeek != DayOfWeek.Monday).ToList();
            //}
            //if (shift.VacationWeeklies.Where(a => a.VacationWeeklyCodingId == 5).FirstOrDefault() != null)
            //{
            //    Dayes=Dayes.Where(a => a.DayOfWeek != DayOfWeek.Tuesday).ToList();
            //}
            //if (shift.VacationWeeklies.Where(a => a.VacationWeeklyCodingId == 6).FirstOrDefault() != null)
            //{
            //    Dayes= Dayes.Where(a => a.DayOfWeek != DayOfWeek.Wednesday).ToList();
            //}
            //if (shift.VacationWeeklies.Where(a => a.VacationWeeklyCodingId == 7).FirstOrDefault() != null)
            //{
            //    Dayes= Dayes.Where(a => a.DayOfWeek != DayOfWeek.Thursday).ToList();
            //}
            
         

            return Dayes.Count() * emp.NumberOfHour.Value;
        }

        public static bool GetDateIfItContain(ShiftCoding shift, DateTime Date)
        {
            var db = new AccountingContainer();
            var mounth = Date.Month;
            var Year = Date.Year;
            var Dayes = GetDates(Year, mounth);
            var RealWorked = 0;
            if (shift.VacationWeeklies.Where(a => a.VacationWeeklyCodingId == 1).FirstOrDefault() != null)
            {
                Dayes = Dayes.Where(a => a.DayOfWeek != DayOfWeek.Friday).ToList();
            }
            if (shift.VacationWeeklies.Where(a => a.VacationWeeklyCodingId == 2).FirstOrDefault() != null)
            {
                Dayes = Dayes.Where(a => a.DayOfWeek != DayOfWeek.Saturday).ToList();
            }
            if (shift.VacationWeeklies.Where(a => a.VacationWeeklyCodingId == 3).FirstOrDefault() != null)
            {
                Dayes = Dayes.Where(a => a.DayOfWeek != DayOfWeek.Sunday).ToList();
            }
            if (shift.VacationWeeklies.Where(a => a.VacationWeeklyCodingId == 4).FirstOrDefault() != null)
            {
                Dayes = Dayes.Where(a => a.DayOfWeek != DayOfWeek.Monday).ToList();
            }
            if (shift.VacationWeeklies.Where(a => a.VacationWeeklyCodingId == 5).FirstOrDefault() != null)
            {
                Dayes = Dayes.Where(a => a.DayOfWeek != DayOfWeek.Tuesday).ToList();
            }
            if (shift.VacationWeeklies.Where(a => a.VacationWeeklyCodingId == 6).FirstOrDefault() != null)
            {
                Dayes = Dayes.Where(a => a.DayOfWeek != DayOfWeek.Wednesday).ToList();
            }
            if (shift.VacationWeeklies.Where(a => a.VacationWeeklyCodingId == 7).FirstOrDefault() != null)
            {
                Dayes = Dayes.Where(a => a.DayOfWeek != DayOfWeek.Thursday).ToList();
            }
            if (Dayes.Where(a => a.Date== Date).Count() == 0)
            {
                return false;
            }


            return true ;
        }

        public static decimal PrepareTheSalaryOfEmployee(int Emp,DateTime Date)
        {
            var db = new AccountingContainer();
            var Employee = db.Employees.Find(Emp);
            int numberOfHourMustWork = 0;
            int numberOfHourRealWorked = 0;        
            foreach(var shift in Employee.Shifts)
            {
                var GetShift = GetDateOfShiftInMounth(shift.ShiftCoding, Date, Employee);
                numberOfHourMustWork += GetShift;
          

            }
            decimal ThePriceOfHourInThatMounth = Employee.TotalSalaryBeforPenality.Value / numberOfHourMustWork;
            Employee.Total_Salary = (ThePriceOfHourInThatMounth * numberOfHourMustWork) - ((Employee.AttendingAndLeavings.Where(a=>a.Date<Date).SelectMany(a=>a.RewardsAndPenalties).Where(a=>a.IsTaken!=true).Sum(a=>a.NumberOfHour)* ThePriceOfHourInThatMounth));
           
            if (Employee.Work_Actual_Start != null && Employee.Work_Actual_Start != "")
            {
                var StartDate = Convert.ToDateTime(Employee.Work_Actual_Start);
                var getNumberOfDayOfMounth = GetDates(Date.Year, Date.Month).Count();
               var DayWork= GetDates(Date.Year, Date.Month).Where(a => a.Date >= StartDate).Count(); 
                Employee.Total_Salary=((Employee.TotalSalaryBeforPenality/ getNumberOfDayOfMounth)* DayWork) - ((Employee.AttendingAndLeavings.Where(a => a.Date < Date).SelectMany(a => a.RewardsAndPenalties).Where(a => a.IsTaken != true).Sum(a => a.NumberOfHour) * ThePriceOfHourInThatMounth));
            }
            db.Entry(Employee).State = EntityState.Modified;
            db.SaveChanges();
            db.Dispose();
            return ThePriceOfHourInThatMounth;
        }
     


    }
}