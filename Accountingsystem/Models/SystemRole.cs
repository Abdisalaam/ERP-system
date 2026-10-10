using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Accountingsystem.Models
{
    public partial class SystemRole
    {

        public static SystemRoleVM prepareForeCreation()
        {
            var db = new AccountingContainer();
            var systemrole = new SystemRoleVM();
            var print = new Utilites.PrintChartOfAccount();
            systemrole.RoleFunctions = print.AddToFatherAccout("s",db.SystemFunctions.ToList());
            var package = db.Packages.FirstOrDefault();
            if (package != null)
            {
              
                if (package.Commission == false|| package.Commission==null)
                {
                    systemrole.RoleFunctions = systemrole.RoleFunctions.Where(a =>!a.SystemFunction.Code.StartsWith("s-cm")).ToList();
                  
                }
                if (package.Number_POS == 0)
                {
                    systemrole.RoleFunctions = systemrole.RoleFunctions.Where(a => !a.SystemFunction.Code.StartsWith("s-ps")).ToList();
                }
                if (package.Subscribtion != true)
                {
                    systemrole.RoleFunctions = systemrole.RoleFunctions.Where(a => !a.SystemFunction.Code.StartsWith("s-sb")).ToList();
                }

                if (package.Asset != true)
                {
                    systemrole.RoleFunctions = systemrole.RoleFunctions.Where(a => !a.SystemFunction.Code.StartsWith("s-st")).ToList();
                }
                if (package.Project != true)
                {
                    systemrole.RoleFunctions = systemrole.RoleFunctions.Where(a => !a.SystemFunction.Code.StartsWith("s-pg")).ToList();
                }
                if (package.Casher != true)
                {
                    systemrole.RoleFunctions = systemrole.RoleFunctions.Where(a => !a.SystemFunction.Code.StartsWith("s-ch")).ToList();
                }
                if (package.Maintenance != true)
                {
                    systemrole.RoleFunctions = systemrole.RoleFunctions.Where(a => !a.SystemFunction.Code.StartsWith("s-ms")).ToList();
                }
                if (package.OffGL != true)
                {
                    systemrole.RoleFunctions = systemrole.RoleFunctions.Where(a => !a.SystemFunction.Code.StartsWith("s-sv-cy-glf")).ToList();
                }
                if (package.Expenses != true)
                {
                    systemrole.RoleFunctions = systemrole.RoleFunctions.Where(a => !a.SystemFunction.Code.StartsWith("s-ex")).ToList();
                }
                if (package.Accounting == false )
                {
                    systemrole.RoleFunctions = systemrole.RoleFunctions.Where(a => !a.SystemFunction.Code.StartsWith("s-ac")).ToList();

                }
                if (package.Suppliers == false )
                {
                    systemrole.RoleFunctions = systemrole.RoleFunctions.Where(a => !a.SystemFunction.Code.StartsWith("s-sp")).ToList();

                }
                if (package.ReportsAndPending == false )
                {
                    systemrole.RoleFunctions = systemrole.RoleFunctions.Where(a => !a.SystemFunction.Code.StartsWith("s-rp")).ToList();

                }
                if (package.Purchase == false)
                {
                    systemrole.RoleFunctions = systemrole.RoleFunctions.Where(a => !a.SystemFunction.Code.StartsWith("s-pu")).ToList();

                }
                if (package.Sales == false)
                {
                    systemrole.RoleFunctions = systemrole.RoleFunctions.Where(a => !a.SystemFunction.Code.StartsWith("s-sl")).ToList();

                }
            }



            return systemrole;



        }
        public static int Create(dynamic json,string name)
        {
            var db = new AccountingContainer();
            var systemrole = new SystemRole();
            systemrole.Name = name;
            systemrole.RoleFunctions = new List<RoleFunction>();
            foreach (var function in json)
            {

                var roleFunction = new RoleFunction();

                roleFunction.SystemFunctionId = Convert.ToInt32(function.Id);
                roleFunction.isExist = Convert.ToBoolean(function.isExist);
                //db.Entry(batch).State = EntityState.Modified;
                SystemFunction functionmain = db.SystemFunctions.Find(roleFunction.SystemFunctionId);

                var a = functionmain.SystemFunctionId;

                if (roleFunction.isExist == true)
                {
                   
                    while (a != null&& systemrole.RoleFunctions.Where(z => z.SystemFunctionId == a).Count() == 0)
                    {
                        
                            var roleFunctionfather = new RoleFunction();

                            roleFunctionfather.SystemFunctionId = a.Value;
                            roleFunctionfather.isExist = true;
                            systemrole.RoleFunctions.Add(roleFunctionfather);
                            var grand= db.SystemFunctions.Find(a.Value);

                            a = grand.SystemFunctionId;
                        
                    }
                    systemrole.RoleFunctions.Add(roleFunction);
                }
            }
            systemrole.RoleFunctions = systemrole.RoleFunctions.OrderBy(v => v.SystemFunctionId).ToList();
            db.SystemRoles.Add(systemrole);
            db.SaveChanges();
            return systemrole.Id;




        }


        public static SystemRoleVM prepareForEdit(int id)
        {
            var db = new AccountingContainer();
            var systemRole =db.SystemRoles.Find(id);
            var systemrolevm = new SystemRoleVM();
            systemrolevm.Id = systemRole.Id;
            systemrolevm.Name = systemRole.Name;
            var print = new Utilites.PrintChartOfAccount();
            systemrolevm.RoleFunctions = print.AddToFatherAccoutForEdit("s", db.SystemFunctions.ToList(), systemRole);
            var package = db.Packages.FirstOrDefault();
            if (package != null)
            {

                if (package.Commission == false || package.Commission == null)
                {
                    systemrolevm.RoleFunctions = systemrolevm.RoleFunctions.Where(a => !a.SystemFunction.Code.StartsWith("s-cm")).ToList();

                }
                if (package.Number_POS == 0)
                {
                    systemrolevm.RoleFunctions = systemrolevm.RoleFunctions.Where(a => !a.SystemFunction.Code.StartsWith("s-ps")).ToList();
                }
                if (package.Subscribtion != true)
                {
                    systemrolevm.RoleFunctions = systemrolevm.RoleFunctions.Where(a => !a.SystemFunction.Code.StartsWith("s-sb")).ToList();
                }

                if (package.Asset != true)
                {
                    systemrolevm.RoleFunctions = systemrolevm.RoleFunctions.Where(a => !a.SystemFunction.Code.StartsWith("s-st")).ToList();
                }
                if (package.Project != true)
                {
                    systemrolevm.RoleFunctions = systemrolevm.RoleFunctions.Where(a => !a.SystemFunction.Code.StartsWith("s-pg")).ToList();
                }
                if (package.Casher != true)
                {
                    systemrolevm.RoleFunctions = systemrolevm.RoleFunctions.Where(a => !a.SystemFunction.Code.StartsWith("s-ch")).ToList();
                }
                if (package.Maintenance != true)
                {
                    systemrolevm.RoleFunctions = systemrolevm.RoleFunctions.Where(a => !a.SystemFunction.Code.StartsWith("s-ms")).ToList();
                }
                if (package.OffGL != true)
                {
                    systemrolevm.RoleFunctions = systemrolevm.RoleFunctions.Where(a => !a.SystemFunction.Code.StartsWith("s-sv-cy-glf")).ToList();
                }
                if (package.Expenses != true)
                {
                    systemrolevm.RoleFunctions = systemrolevm.RoleFunctions.Where(a => !a.SystemFunction.Code.StartsWith("s-ex")).ToList();
                }
                if (package.Accounting == false)
                {
                    systemrolevm.RoleFunctions = systemrolevm.RoleFunctions.Where(a => !a.SystemFunction.Code.StartsWith("s-ac")).ToList();

                }
                if (package.Suppliers == false)
                {
                    systemrolevm.RoleFunctions = systemrolevm.RoleFunctions.Where(a => !a.SystemFunction.Code.StartsWith("s-sp")).ToList();

                }
                if (package.ReportsAndPending == false)
                {
                    systemrolevm.RoleFunctions = systemrolevm.RoleFunctions.Where(a => !a.SystemFunction.Code.StartsWith("s-rp")).ToList();

                }
                if (package.Purchase == false)
                {
                    systemrolevm.RoleFunctions = systemrolevm.RoleFunctions.Where(a => !a.SystemFunction.Code.StartsWith("s-pu")).ToList();

                }
                if (package.Sales == false)
                {
                    systemrolevm.RoleFunctions = systemrolevm.RoleFunctions.Where(a => !a.SystemFunction.Code.StartsWith("s-sl")).ToList();

                }
            }

            return systemrolevm;



        }

        public static int Edit(dynamic json, string name,int id)
        {
            var db = new AccountingContainer();
            var systemrole = db.SystemRoles.Find(id);
            systemrole.Name = name;

            db.RoleFunctions.RemoveRange(systemrole.RoleFunctions);
            db.SaveChanges();

            systemrole.RoleFunctions = new List<RoleFunction>();

            foreach (var function in json)
            {

                var roleFunction = new RoleFunction();

                roleFunction.SystemFunctionId = Convert.ToInt32(function.Id);
                roleFunction.isExist = Convert.ToBoolean(function.isExist);
             
                SystemFunction functionmain = db.SystemFunctions.Find(roleFunction.SystemFunctionId);

                var a = functionmain.SystemFunctionId;

                if (roleFunction.isExist == true)
                {

                    while (a != null && systemrole.RoleFunctions.Where(z => z.SystemFunctionId == a).Count() == 0)
                    {

                        var roleFunctionfather = new RoleFunction();

                        roleFunctionfather.SystemFunctionId = a.Value;
                        roleFunctionfather.isExist = true;
                        systemrole.RoleFunctions.Add(roleFunctionfather);
                        var grand = db.SystemFunctions.Find(a.Value);

                        a = grand.SystemFunctionId;

                    }
                    systemrole.RoleFunctions.Add(roleFunction);
                }
            }
            systemrole.RoleFunctions = systemrole.RoleFunctions.OrderBy(v => v.SystemFunctionId).ToList();
         
            db.Entry(systemrole).State = EntityState.Modified;
            db.SaveChanges();
            return systemrole.Id;




        }






        public static List<SystemRole> getAllRole()
        {
            var db = new AccountingContainer();
            var systemRole = db.SystemRoles.ToList();

            return systemRole;

        }
        public static List<Employee> getAllUser()
        {
            var db = new AccountingContainer();
            var systemRoleEmp = db.Employees.Where(a=>a.SystemRoleId>0&&a.Reg_ID!=null).ToList();

            return systemRoleEmp;

        }

        public static SystemRoleVM GetSystemRolById(int id)
        {
            var db = new AccountingContainer();
            var systemRole = db.SystemRoles.Find(id);
            var systemrolevm = new SystemRoleVM();
            systemrolevm.Name = systemRole.Name;
            systemrolevm.Id = systemRole.Id;
            var print = new Utilites.PrintChartOfAccount();
            systemrolevm.RoleFunctions = print.AddToFatherAccoutForEdit("s", db.SystemFunctions.ToList(), systemRole).Where(a=>a.isExist==true).ToList();

            return systemrolevm;



        }

        public static string getDepartment(int id)
        {
            var db = new AccountingContainer();
            var systemfuction = db.SystemFunctions.Find(id);
            var level = systemfuction.Level;
            while (level > 1)
            {
                systemfuction = db.SystemFunctions.Find(systemfuction.SystemFunctionId);

                level = systemfuction.Level;
            }
            return systemfuction.ArabName;

        }
        public static List<string> getsystemFunctionByUser(string userId)
        {
            var db = new AccountingContainer();
           
            var emp = db.Employees.Where(a => a.Reg_ID == userId).FirstOrDefault();
            if (emp == null || emp.Emp_Name == "Admin" || emp.SystemRoleId == null || emp.SystemRoleId == 6)
            {
                return db.SystemFunctions.Select(a => a.Code).ToList();
            }
            else {
                var Listfunction = db.SystemRoles.Find(emp.SystemRoleId).RoleFunctions.Select(a => a.SystemFunction).Select(a=>a.Code).ToList();
                return Listfunction;
            }

           

        }

        public static bool changeRoleId(string userId,int roleid)
        {
            var db = new AccountingContainer();

            var emp = db.Employees.Where(a => a.Reg_ID == userId).FirstOrDefault();
            if (emp != null&&emp.SystemRoleId==null)
            {
                emp.SystemRoleId = roleid;
                db.Entry(emp).State = EntityState.Modified;
                db.SaveChanges();
            }
            return true;
        }
    }
}