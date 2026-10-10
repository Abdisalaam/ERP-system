using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Accountingsystem.Models
{
    public partial class HRNotification
    {
        public static List<HRNotification> GetAllHRNotification()
        {
            var db = new AccountingContainer();
            var HR = db.HRNotifications.ToList();

            return HR;
        }

        // use this method to   Get one element CategoryById from Database.  
        public static HRNotification GetHRNotificationById(int id)
        {
            var db = new AccountingContainer();
            var HR = db.HRNotifications.Find(id);
            return HR;
        }

        // use this method to   Get one element CategoryByCode from Database.  
       

        // use this method For Creation new Catagery and get Data from user.
        public static HRNotificationVM CreateVMForCreation()
        {
            var db = new AccountingContainer();
            var HRNotificationVM = new HRNotificationVM();


            return HRNotificationVM;
        }

        // use this method to  Create New Catagery and save it in DataBase.
        public static bool CreateHRNotification(HRNotificationVM vm)
        {
            var db = new AccountingContainer();

            var hr = new HRNotification();
           

            hr.NoOfhealtyInsurance = vm.NoOfhealtyInsurance;
            hr.NoOfResidence= vm.NoOfResidence;
           
            db.HRNotifications.Add(hr);
            db.SaveChanges();
            db.Dispose();
            return true;
        }

        // use this method to load Details of Data for Category.
        public static HRNotificationVM FillTheViewModel(int id)
        {
            var db = new AccountingContainer();
            var hr = db.HRNotifications.Find(id);
            var hrVM = new HRNotificationVM();

            hrVM.NoOfhealtyInsurance = hr.NoOfhealtyInsurance;
            hrVM.NoOfResidence = hr.NoOfResidence;
           
            return hrVM;

        }

        // use this method to Edite Catgery and save it in DataBase.
        public static HRNotification EditHRNotification(HRNotificationVM vm)
        {
            var db = new AccountingContainer();
            var HR = db.HRNotifications.Find(vm.Id);
            HR.NoOfhealtyInsurance = vm.NoOfhealtyInsurance;
            HR.NoOfResidence = vm.NoOfResidence;
            db.Entry(HR).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                return HR;
          
        }

        // use this method to Delete Category and save DataBase.
        public static int DeleteCategory(int id)
        {

            var db = new AccountingContainer();
            var HR = db.HRNotifications.Find(id);
            if (HR != null)
            {

                db.HRNotifications.Remove(HR);
                db.SaveChanges();
                db.Dispose();
                return 1;
            }

            return 0;

        }
    }
}