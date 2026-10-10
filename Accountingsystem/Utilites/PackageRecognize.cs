using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Accountingsystem.Utilites
{
    public class PackageRecognize
    {
        public static Package getThePackage(string userid)
        {
            var db = new AccountingContainer();
            //var Client = db.Clients.Where(a => a.User_Id == userid).FirstOrDefault();
            //if (Client != null)
            //{
            //    if (Client.PackageId==null)
            //    {
            //        return null;
            //    }
            //    else
            //    {
            //        return Client.Package;
            //    }
            //}
            //var emp = db.Employees.Where(a => a.Reg_ID == userid).FirstOrDefault();
            //if (emp!=null)
            //{
            //    if (emp.Client_Id==0||emp.Client_Id==null)
            //    {
            //        return null;
            //    }
            //    else
            //    {
            //        var ClientEmp = db.Clients.Find(emp.Client_Id);

            //            if (ClientEmp.PackageId == null)
            //            {
            //                return null;
            //            }
            //            else
            //            {
            //                return ClientEmp.Package;
            //            }


            //    }



            //}
            //else
            //{
            //    return null;
            //}


            return db.Packages.FirstOrDefault();


        }
        public static Tuple<Package,bool> checkValiditeOfAsset(string userid)
        {
            var pack = getThePackage(userid);
            if (pack != null)
            {
                if (pack.Asset == true)
                {
                    return Tuple.Create(pack, true);
                }
                else
                {
                    return Tuple.Create(pack, false);
                }
            }
            else
            {
                return Tuple.Create(pack, true);
            }




        }
        public static Tuple<Package, bool> checkValiditeOfExpenses(string userid)
        {
            var pack = getThePackage(userid);
            if (pack != null)
            {
                if (pack.Expenses == true||pack.Expenses==null)
                {
                    return Tuple.Create(pack, true);
                }
                else
                {
                    return Tuple.Create(pack, false);
                }
            }
            else
            {
                return Tuple.Create(pack, true);
            }




        }
        public static Tuple<Package, bool> checkValiditeOfHR(string userid)
        {
            var pack = getThePackage(userid);
            if (pack != null)
            {
                if (pack.HR == true)
                {
                    return Tuple.Create(pack, true);
                }
                else
                {
                    return Tuple.Create(pack, false);
                }
            }
            else
            {
                return Tuple.Create(pack, true);
            }




        }
        public static Tuple<Package, bool> checkValiditeOfSubscribtion(string userid)
        {
            var pack = getThePackage(userid);
            if (pack != null)
            {
                if (pack.Subscribtion == true)
                {
                    return Tuple.Create(pack, true);
                }
                else
                {
                    return Tuple.Create(pack, false);
                }
            }
            else
            {
                return Tuple.Create(pack, true);
            }




        }
        public static Tuple<Package, bool> checkValiditeOfPoject(string userid)
        {
            var pack = getThePackage(userid);
            if (pack != null)
            {
                if (pack.Project == true)
                {
                    return Tuple.Create(pack, true);
                }
                else
                {
                    return Tuple.Create(pack, false);
                }
            }
            else
            {
                return Tuple.Create(pack, true);
            }




        }

        public static Tuple<Package, bool> checkValiditeOfGLOff(string userid)
        {
            var pack = getThePackage(userid);
            if (pack != null)
            {
                if (pack.OffGL == true)
                {
                    return Tuple.Create(pack, true);
                }
                else
                {
                    return Tuple.Create(pack, false);
                }
            }
            else
            {
                return Tuple.Create(pack, true);
            }




        }
        public static Tuple<Package, bool> checkValiditeOfBranch(string userid)
        {
            var db = new AccountingContainer();
            var pack = getThePackage(userid);
            if (pack != null)
            {
                var branch = db.Branches.Count();
                if (pack.Number_Branch > branch)
                {
                    return Tuple.Create(pack, true);
                }
                else
                {
                    return Tuple.Create(pack, false);
                }
            }
            else
            {
                return Tuple.Create(pack, true);
            }




        }
        public static Tuple<Package, bool> checkValiditeOfDepartment(string userid)
        {
            var db = new AccountingContainer();
            var pack = getThePackage(userid);
            if (pack != null)
            {
                var Department = db.CompanyDepartments.Count();
                if (pack.Number_Branch > Department)
                {
                    return Tuple.Create(pack, true);
                }
                else
                {
                    return Tuple.Create(pack, false);
                }
            }
            else
            {
                return Tuple.Create(pack, true);
            }




        }
        public static Tuple<Package, bool> checkValiditeOfPointOfSale(string userid)
        {
            var db = new AccountingContainer();
            var pack = getThePackage(userid);
            if (pack != null)
            {
                var pos = db.PointOfSales.Count();
                if (pack.Number_POS > pos)
                {
                    return Tuple.Create(pack, true);
                }
                else
                {
                    return Tuple.Create(pack, false);
                }
            }
            else
            {
                return Tuple.Create(pack, true);
            }




        }
        public static Tuple<Package, bool> checkValiditeOfStore(string userid)
        {
            var db = new AccountingContainer();
            var pack = getThePackage(userid);
            if (pack != null)
            {
                var store = db.Stores.Count();
                if (pack.Number_store > store)
                {
                    return Tuple.Create(pack, true);
                }
                else
                {
                    return Tuple.Create(pack, false);
                }
            }
            else
            {
                return Tuple.Create(pack, true);
            }




        }
        public static Tuple<Package, bool> checkValiditeOfUser(string userid)
        {
            var db = new AccountingContainer();
            var pack = getThePackage(userid);
            if (pack != null)
            {
                var emp = db.Employees.Where(a => a.Reg_ID != null).Count();
                if (pack.Number_User+1 > emp)
                {
                    return Tuple.Create(pack, true);
                }
                else
                {
                    return Tuple.Create(pack, false);
                }
            }
            else
            {
                return Tuple.Create(pack, true);
            }




        }
        public static Tuple<Package, bool> checkValiditeOfSubLedger(string userid)
        {
            var db = new AccountingContainer();
            var pack = getThePackage(userid);
            if (pack != null)
            {
                var sub = db.SubLedgers.Where(a => a.SubLed_Type == "مركز تكلفة يدوى").Count();
                if (pack.Number_subLedgger > sub)
                {
                    return Tuple.Create(pack, true);
                }
                else
                {
                    return Tuple.Create(pack, false);
                }
            }
            else
            {
                return Tuple.Create(pack, true);
            }




        }
        public static Tuple<Package, bool> checkValiditeOfEmployee(string userid)
        {
            var db = new AccountingContainer();
            var pack = getThePackage(userid);
            if (pack != null)
            {
                var sub = db.Employees.Count();
                if (pack.Number_Employee+1 > sub)
                {
                    return Tuple.Create(pack, true);
                }
                else
                {
                    return Tuple.Create(pack, false);
                }
            }
            else
            {
                return Tuple.Create(pack, true);
            }




        }
        public static Tuple<Package, bool> checkValiditeOfTrans(string userid)
        {
            var db = new AccountingContainer();
            var pack = getThePackage(userid);
            if (pack != null)
            {
                var sub = pack.Number_store;
                if (sub< 2)
                {
                    return Tuple.Create(pack, false);
                }
                else
                {
                    return Tuple.Create(pack, true);
                }
            }
            else
            {
                return Tuple.Create(pack, true);
            }




        }

        public static string alertForRenew()
        {
            var db = new AccountingContainer();
            
            var client = db.Clients.Where(a => a.PackageId != null && a.PackageId != 0).FirstOrDefault();
            if (client == null)
            {
                return null;
            }
            else
            {
                var date = DateTime.Now.AddDays(15);
                if (date>=client.End_sub)
                {
                    var time = (int)(  client.End_sub- DateTime.Now).Value.TotalDays;
                    var msg = " متبقي من الباقة " + time + "يوم برجاء تجديد الباقة   "  ;
                    return msg;

                }
                else
                {
                    return null;
                }
            }
        }

        public static string NoOfResidence()
        {
            var db = new AccountingContainer();

            var HRSetting = db.HRNotifications.FirstOrDefault();
            var package = db.Packages.FirstOrDefault();
            if (HRSetting == null && (package == null || package.HR == false))
            {
                return null;
            }
            if (HRSetting == null)
            {
                return null;
            }
            else
            {
                var date = DateTime.Now.AddDays(HRSetting.NoOfResidence);
                var msg = "";
                foreach(var emp in db.Employees.Where(a => a.End_Date_Residency != null && a.Active != false))
                {

                    if (date >=Convert.ToDateTime( emp.End_Date_Residency))
                    {
                        
                         msg = "اقامات علي وشك الانتهاء";
                        
                        
                    }
                    
                }
                if (msg != "")
                {
                    return msg;
                }
                else
                {
                    return null;
                }

                
            }
        }
        public static string NoOfEnd_medical_Insurance( )
        {
            var db = new AccountingContainer();

            var HRSetting = db.HRNotifications.FirstOrDefault();
            var package = db.Packages.FirstOrDefault();
            if (HRSetting == null&&(package==null||package.HR==false))
            {
                return null;
            }
            else
            {
                var date = DateTime.Now.AddDays(HRSetting.NoOfhealtyInsurance);
                var msg = "";
                foreach (var emp in db.Employees.Where(a => a.End_medical_Insurance != null && a.Active != false))
                {

                    if (date >= Convert.ToDateTime(emp.End_medical_Insurance))
                    {

                        msg = "تأمين طبي علي وشك الانتهاء";


                    }

                }
                if (msg != "")
                {
                    return msg;
                }
                else
                {
                    return null;
                }


            }
        }
        public static bool checkLanguage(string text)
        {
            if (text == null || text == "")
            {
                text = "t";
            }
            Regex regex = new Regex("[\u0600-\u06ff]|[\u0750-\u077f]|[\ufb50-\ufc3f]|[\ufe70-\ufefc]");
            return regex.IsMatch(text);
        }


    }
}