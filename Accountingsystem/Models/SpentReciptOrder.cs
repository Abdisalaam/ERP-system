using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class SpentReciptOrder
    {
        public static List<SpentReciptOrder> getAllSpentReciptOrders()
        {
            var db = new AccountingContainer();
            var spentList = db.SpentReciptOrders.ToList();
            return spentList;
        }
        public static List<SpentReciptOrder> getAllSpentReciptOrdersByType(int Type)
        {
            var db = new AccountingContainer();
            var spentList = db.SpentReciptOrders.Where(a=>a.SpentReciptOrderTypeId== Type).ToList();
            return spentList;
        }
        public static SpentReciptOrder getSpentReciptOrderByItsId(int SpentReciptOrderTypeId)
        {
            var db = new AccountingContainer();
            var spent = db.SpentReciptOrders.Find(SpentReciptOrderTypeId);
            return spent;
        }
        public static SpentReciptOrderVM FillForCreate(int ?id)
        {
            var vm = new SpentReciptOrderVM();
            var db = new AccountingContainer();
            if (id > 0)
            {
                vm.EmpList = new SelectList(db.Employees.Where(a=>a.Emp_Id==id), "Emp_Id", "Emp_Name");
                vm.Employee_Id = id.Value;
                return vm;
            }
            vm.EmpList = new SelectList(db.Employees, "Emp_Id", "Emp_Name");
            return vm;
        }
        public static int Create(SpentReciptOrderVM vm)
        {
            var spentReciptOrder = new SpentReciptOrder();
            var db = new AccountingContainer();
            spentReciptOrder.Date = Convert.ToDateTime(vm.Date);
            spentReciptOrder.Description = vm.Description;
            spentReciptOrder.Employee_Id = vm.Employee_Id;
            spentReciptOrder.SpentReciptOrderTypeId = 1;
            spentReciptOrder.Value = vm.Value;
            db.SpentReciptOrders.Add(spentReciptOrder);
            db.SaveChanges();

            return spentReciptOrder.Id;
        }

        public static SpentReciptOrderVM managerAcceptForFill(int  id)
        {
            var vm = new SpentReciptOrderVM();
       
            var db = new AccountingContainer();
            var spentReciptOrder = getSpentReciptOrderByItsId(id);
            vm.EmpList = new SelectList(db.Employees, "Emp_Id", "Emp_Name");
            vm.Date = spentReciptOrder.Date.ToString();
           vm.Description = spentReciptOrder.Description ;
            vm.Employee_Id= spentReciptOrder.Employee_Id ;
            spentReciptOrder.DateAcceptOrRefused = DateTime.Now;
             vm.Value= spentReciptOrder.Value ;
        

            return vm;
        }
        public static int mangerApprovePost (SpentReciptOrderVM vm)
        {
            var db = new AccountingContainer();
            var spentReciptOrder = db.SpentReciptOrders.Find(vm.Id);
         
            spentReciptOrder.Date = Convert.ToDateTime(vm.Date);
            spentReciptOrder.Description = vm.Description;
            spentReciptOrder.Employee_Id = vm.Employee_Id;
            spentReciptOrder.SpentReciptOrderTypeId = vm.SpentReciptOrderTypeId;
            spentReciptOrder.Value = vm.Value;
            db.Entry(spentReciptOrder).State = EntityState.Modified;
            db.SaveChanges();

            return spentReciptOrder.Id;
        }
    }
}