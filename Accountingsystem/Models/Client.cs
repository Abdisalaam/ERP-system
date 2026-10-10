using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class Client
    {/// <summary>
    /// make new Client For the System And Status is is not Active
    /// </summary>
    /// <param name="userId"></param>
    /// 
    /// <param name="reg"></param>
        public static void addClientByRegisterMethod(String userId,RegisterViewModel reg)
        {
            var db = new AccountingContainer();
            var client = new Client();
            client.Name = reg.Client_Name;
            client.E_Mail = reg.Email;
            client.status = Client_status.مفعل;
            client.User_Id = userId;
            client.Start_sub = DateTime.Today;
            var day = db.Packages.FirstOrDefault().Package_duration;
            if (day > 10000)
            {
                client.End_sub = DateTime.Today.AddYears(20);
            }
            else
            {
                client.End_sub = DateTime.Today.AddDays(day);
            }
            client.PackageId = reg.Package_Id;
            db.Clients.Add(client);
            db.SaveChanges();
            var emp = new EmpVM();
            // that default data for admin to become Employee
            emp.Emp_Name= reg.Client_Name;
            emp.Emp_Phone = "000000";
            emp.Passport_number = "0000000";
            emp.Subledger_Id = 1;
            emp.Regist_Id = userId;           
            emp.Active = true;
            emp.Company_ID = 1;
            emp.SystemRole_ID = db.SystemRoles.FirstOrDefault().Id;
           var a= Employee.CreateEmployee(emp);
            if (a == 1)
            {
                db.SaveChanges();
            }
            var POS = new PointOfSale();
            POS.Company_Id = 1;
            POS.Adress = " ";
            POS.Employee_Id = db.Employees.FirstOrDefault().Emp_Id;
            POS.POS_Name = "نقطة بيع ";
            POS.Store_Id = db.Stores.FirstOrDefault().Store_ID;
            var POSDetail = new POSDetailPayment();
            var paymentMethod = db.PaymentMethods.FirstOrDefault();

            POSDetail.PaymentMethod_Id = paymentMethod.PaymentMethod_Id;
            POSDetail.PaymentMethod_Name = paymentMethod.Payment_Type;
            POS.POSDetailPayments.Add(POSDetail);
            db.PointOfSales.Add(POS);
            db.SaveChanges();




        }
        /// <summary>
        /// get AllClient For the System
        /// </summary>
        /// <returns></returns>
        public static List<Client>getAllClient()
        {
            var db = new AccountingContainer();
            var List = db.Clients.ToList();
            return List;
        }
        /// <summary>
        /// Check Client validation and status for Entering the System
        /// </summary>
        /// <param name="logModel"></param>
        /// <returns></returns>
        public static String CheckClientLogin(LoginViewModel logModel)
        {
            var db = new AccountingContainer();
            String msg = "";

            if (logModel.Email == "sutumsuperadmin@sutum.org")
            {
                return msg;
            }
            var Account = db.Clients.FirstOrDefault();
            if (Account != null)
            {
                if (Account.Start_sub == null)
                {
                    Account.Start_sub = DateTime.Now;
                    if (Account.Package != null)
                    {
                        Account.End_sub = Account.Start_sub.Value.AddDays(Account.Package.Package_duration);
                    }
                    Account.status = Client_status.مفعل;
                    db.Entry(Account).State = EntityState.Modified;
                    db.SaveChanges();

                }
                if (Account.End_sub < DateTime.Today)
                {

                    return "تم انتهاء مدة الاشتراك";


                }


            }

            return msg;

        }
        /// <summary>
        /// Check is status is Expire Or Active
        /// </summary>
        public static void ChecedTheEmailForAllClient()
        {
            var db = new AccountingContainer();
            var ClientList = db.Clients.Where(a => a.status == Client_status.مفعل).ToList();
            foreach ( var client in ClientList )
            {
                if (client.End_sub<DateTime.Now)
                {
                    client.status = Client_status.منتهى;
                    db.Entry(client).State = EntityState.Modified;
                    db.SaveChanges();
                }

            }



        }
        /// <summary>
        /// get client by its id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Client</returns>
        public static Client getClientById(int id)
        {
            var db = new AccountingContainer();
            var List = db.Clients.Find(id);
            return List;


        }
        /// <summary>
        /// renew the package for the client
        /// </summary>
        /// <param name="id"></param>
        /// <returns>string</returns>
        public static string renewPackage(int id )
        {
            var db = new AccountingContainer();
            var client = db.Clients.Find(id);
            var pack = client.Package.Package_duration;
         
            if(DateTime.Now > client.End_sub)
            {

                client.End_sub = DateTime.Now.AddDays(pack);
            }
            else
            {
                client.End_sub = client.End_sub.Value.AddDays(pack);
            }

            db.Entry(client).State = EntityState.Modified;
            client.status = Client_status.مفعل;
            db.SaveChanges();

            return "تم تجديد الباقة بنجاح";

        }

        /// <summary>
        /// fill for Editing client and can make change the package
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ClientVM</returns>
        public static ClientVM fillForEdit(int id)
        {
            var db = new AccountingContainer();
            var client = db.Clients.Find(id);
            var vm = new ClientVM();
            vm.PackageList = new SelectList(Package.getAllPackage(), "Id", "Package_Name");
            vm.Client_Name = client.Name;
            vm.Client_Id = client.Id;
            vm.Package_Id = client.PackageId;
            if (client.End_sub != null)
            {
                vm.EndDate = client.End_sub.Value.ToString("dd-MM-yyyy   HH:mm:ss"); ;
            }
            return vm;

        }
        /// <summary>
        /// save the change of Editing in db
        /// </summary>
        /// <param name="vm"></param>
        /// <returns>string</returns>
        public static string EditForClient(ClientVM vm)
        {
            var db = new AccountingContainer();
            var client = db.Clients.Find(vm.Client_Id);
  
            client.End_sub = Convert.ToDateTime(vm.EndDate);
            db.Entry(client).State = EntityState.Modified;
            db.SaveChanges();
            return "تم التعديل بنجاح";




        }

    }
}