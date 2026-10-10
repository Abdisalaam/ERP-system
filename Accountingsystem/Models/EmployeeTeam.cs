using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Accountingsystem.ViewModel;
using Accountingsystem.Models;
using System.Web.Mvc;
using System.Data.Entity;

namespace Accountingsystem.Models
{
    public partial class EmployeeTeam
    {
        /// <summary>
        /// make vm for Creation new team
        /// </summary>
        /// <returns></returns>
        public static EmployeeTeamVM AddNewForCreation()
        {
            var db = new AccountingContainer();
            var masterVM = new EmployeeTeamVM();
      
            masterVM.EmPloyeeList = new SelectList(db.Employees.Where(a=>a.Team_ID==0||a.Team_ID==null), "Emp_Id", "Emp_Name");
           
            return masterVM;
        }
        /// <summary>
        /// make VM for Creation New Member in that Team
        /// </summary>
        /// <returns></returns>
        public static EmpVM AddNewForCreationteamMeMber()
        {
            var db = new AccountingContainer();
            var masterVM = new EmpVM();
    
            masterVM.Employlist = new SelectList(db.Employees.Where(a=>a.Team_ID==null||a.Team_ID==0), "Emp_Id", "Emp_Name");
            return masterVM;
        }

        /// <summary>
        /// make post For that team and save it in data base
        /// </summary>
        /// <param name="EVM"></param>
        /// <returns></returns>
        public static int AddTeam(EmployeeTeamVM EVM)
        {
            var db = new AccountingContainer();
            var EmployeeT = new EmployeeTeam();
            
         
            var TeamLeader = db.Employees.Find(EVM.Team_Leader_ID);
         
                EmployeeT.Team_Leader_ID = EVM.Team_Leader_ID;
                EmployeeT.Team_Leader_Name = TeamLeader.Emp_Name;
            EmployeeT.Team_Name = EVM.Team_Name;


            var allEmployeesinteam = db.EmployeeTeamTemps.ToList();
           
            db.EmployeeTeams.Add(EmployeeT);
            db.SaveChanges();
            foreach (var item in EVM.EmployeesMember)
            {
                int empid = Convert.ToInt32(item);
                var thisEmployee = db.Employees.Where(s => s.Emp_Id == empid).FirstOrDefault();
                
                thisEmployee.Team_ID = EmployeeT.Team_ID;
                thisEmployee.Team_Name = EmployeeT.Team_Name;
                db.Entry(thisEmployee).State = EntityState.Modified;
          
                db.SaveChanges();
            }
            db.Dispose();
            return EmployeeT.Team_ID;
        }
        /// <summary>
        /// add employee in EmployeeTemp for make Creation after that
        /// </summary>
        /// <param name="TeamID"></param>
        /// <returns></returns>
        public static string AddEmployeetoTemp(EmployeeTeamVM TeamID)
        {
            var EmpTemp = new EmployeeTeamTemp();
            var db = new AccountingContainer();
            var thisEmployee = db.Employees.Find(TeamID.Team_Leader_ID);
            if (thisEmployee.Team_ID == null||thisEmployee.Team_ID==0)
            {
                EmpTemp.EmployeeID = TeamID.Team_Leader_ID;
                EmpTemp.Employee_Name = thisEmployee.Emp_Name;
                EmpTemp.TeamID = TeamID.Team_ID;
                EmpTemp.Team_Name = TeamID.Team_Name;
                db.EmployeeTeamTemps.Add(EmpTemp);
                db.SaveChanges();
                db.Dispose();
                return "تم اضافة الموظف";
            }
            else
            {
                return thisEmployee.Team_Name + "هذا الموظف موجود في  فريق";
            }


        }
        /// <summary>
        /// delete Employee From Temp 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static string DeleteEmployeeFromTemp(EmployeeTeamVM ID)
        {
            
            var db = new AccountingContainer();
            var EmpTemp = db.EmployeeTeamTemps.Where(s => s.EmployeeID == ID.Team_Leader_ID).ToList().FirstOrDefault();
            db.EmployeeTeamTemps.Remove(EmpTemp);
            db.SaveChanges();
            db.Dispose();
            return "تم حذف الموظف من القائمة";
        }
        /// <summary>
        /// get List From Employee Temp
        /// </summary>
        /// <returns>List<EmployeeTeamTemp></returns>
        public static List<EmployeeTeamTemp> EmployeeTempList()
        {
           
            var db = new AccountingContainer();
            var Temp = db.EmployeeTeamTemps.ToList();
            return Temp;
        }
        /// <summary>
        /// add information for Employee When Cratinh Team
        /// </summary>
        /// <returns></returns>
        public static string UpdateEmployees()
        {

            var db = new AccountingContainer();
            var allEmployeesinteam = db.EmployeeTeamTemps.ToList();
            foreach (var item in allEmployeesinteam)
            {
                var thisEmployee = db.Employees.Where(s => s.Emp_Id == item.EmployeeID).FirstOrDefault();
                var tempdata = db.EmployeeTeamTemps.Where(e => e.TeamID == item.TeamID&&e.Team_Name==item.Team_Name&&e.EmployeeID==item.EmployeeID).FirstOrDefault();
                thisEmployee.Team_ID = tempdata.TeamID;
                thisEmployee.Team_Name = tempdata.Team_Name;
                db.Entry(thisEmployee).State = EntityState.Modified;
                db.EmployeeTeamTemps.Remove(tempdata);
                db.SaveChanges();
            }      
                            
            db.Dispose();
            return "تم اضافة الموظفين الي الفريق";
        }
        /// <summary>
        /// get All Employee Team from DB
        /// </summary>
        /// <returns>List<EmployeeTeam></returns>
        public static List<EmployeeTeam> EMployeeList()
        {
            var db = new AccountingContainer();
            var allEMployees = db.EmployeeTeams.ToList();
            return allEMployees;
        }
        /// <summary>
        /// Prepare VM For Edit EmployeeTeam
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>EmployeeTeamVM</returns>
        public static EmployeeTeamVM FillViewEmployeeTeam(int ID)
        {
            var db = new AccountingContainer();
            var thisTeam = db.EmployeeTeams.Find(ID);
            var ETV = new EmployeeTeamVM();
            ETV.Team_ID = ID;
            ETV.Team_Name = thisTeam.Team_Name;
            ETV.Team_Leader_Name = thisTeam.Team_Leader_Name;
            ETV.Team_Leader_ID = (int)thisTeam.Team_Leader_ID;
            var ttt = db.Employees.Where(z => z.Team_ID == null || z.Team_ID == 0 || z.Team_ID == ID).ToList();
            ETV.EmPloyeeList = new SelectList(db.Employees.Where(a => a.Team_ID == null || a.Team_ID == 0||a.Team_ID== ID), "Emp_Id", "Emp_Name");
            ETV.EmployeesMember = new List<string>();
            foreach(var item in db.Employees.Where(a=>a.Team_ID== ID))
            {
                string empId = Convert.ToString(item.Emp_Id);
                ETV.EmployeesMember.Add(empId);

            }
            return ETV;
        }
        /// <summary>
        /// Edit Employee Team in Employee Team post and save changes in db
        /// </summary>
        /// <param name="ETV"></param>
        /// <returns>EmployeeTeam</returns>
        public static int  EditTeam(EmployeeTeamVM ETV)
        {
            var db = new AccountingContainer();
            var thisteam = db.EmployeeTeams.Find(ETV.Team_ID);
            thisteam.Team_Name = ETV.Team_Name;
            var cur = db.Employees.Where(c => c.Emp_Id == ETV.Team_Leader_ID);
           
                thisteam.Team_Leader_ID = cur.FirstOrDefault().Emp_Id;
                thisteam.Team_Leader_Name = cur.FirstOrDefault().Emp_Name;
            
           
            db.Entry(thisteam).State = EntityState.Modified;
            db.SaveChanges();
            var EmpNotMember = db.Employees.Where(a => a.Team_ID == thisteam.Team_ID);
            foreach(var item in EmpNotMember)
            {
              
                item.Team_ID = null;
                item.Team_Name = "";
                var emo = item;
                db.Entry(emo).State = EntityState.Modified;

               

            }
            db.SaveChanges();
            foreach (var item in ETV.EmployeesMember)
            {
                int empid = Convert.ToInt32(item);
                var thisEmployee = db.Employees.Where(s => s.Emp_Id == empid).FirstOrDefault();

                thisEmployee.Team_ID = thisteam.Team_ID;
                thisEmployee.Team_Name = thisteam.Team_Name;
                db.Entry(thisEmployee).State = EntityState.Modified;

                db.SaveChanges();
            }
            db.Dispose();
            return thisteam.Team_ID;
        }


        /// <summary>
        /// Rempove Employee Team and Edit in Employee Team
        /// </summary>
        /// <param name="ETV"></param>
        /// <returns></returns>
        public static string DeleteTeamscript(EmployeeTeamVM ETV)
        {
            var db = new AccountingContainer();
            var thisteam = db.EmployeeTeams.Find(ETV.Team_ID);

            var cur = db.Employees.Where(c => c.Team_ID == ETV.Team_ID).ToList();

            if (cur.Count!=0)
            {
                foreach (var emp in cur)
                {
                    emp.Team_ID = 0;
                    emp.Team_Name = "";
                    db.Entry(emp).State = EntityState.Modified;
                }
            }
            db.EmployeeTeams.Remove(thisteam);
            db.SaveChanges();
            db.Dispose();
            return "تم حذف الفريق";
            

        }
        /// <summary>
        /// Get Employe by Id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>EmployeeTeam</returns>
        public static EmployeeTeam getEmployeeBYID(int ID)
        {
            var db = new AccountingContainer();
            EmployeeTeam team = db.EmployeeTeams.Find(ID);

            return team;
        }
        /// <summary>
        /// Delete Employee Team and Edit in 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static EmployeeTeam DeleteTeam(int ID)
        {
            var db = new AccountingContainer();
            var thisteam = db.EmployeeTeams.Find(ID);

            var cur = db.Employees.Where(c => c.Team_ID == ID).ToList();
            if (cur.Count!=0)
            {
                foreach (var emp in cur)
                {
                    emp.Team_ID = 0;
                    emp.Team_Name = "";
                    db.Entry(emp).State = EntityState.Modified;
                    db.SaveChanges();
                }
               
                db.EmployeeTeams.Remove(thisteam);
                db.SaveChanges();
                db.Dispose();
                return thisteam;
            }
            else {
                db.EmployeeTeams.Remove(thisteam);
                db.SaveChanges();
                db.Dispose();
                return thisteam;
            }
          

           

          
        }
        /// <summary>
        /// getList of Employe has the same team
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<Employee> EmployeeTeamDetails(int id)
        {
            var db = new AccountingContainer();
            var cur = db.Employees.Where(c => c.Team_ID == id).ToList();        
            return cur;
            }
        }
    
}