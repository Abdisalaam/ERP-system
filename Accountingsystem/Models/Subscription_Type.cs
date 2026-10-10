using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Accountingsystem.ViewModel;
using System.Data.Entity;


namespace Accountingsystem.Models
{
    public partial class Subscription_Type
    {
        /// <summary>
        /// check that code is unique
        /// </summary>
        /// <param name="Serial"></param>
        /// <returns>bool</returns>
        public static bool containCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.Subscription_Type.ToList();
            foreach (var item in allinData)
            {
                if (item.Subscrip_Code == Serial || !Serial.StartsWith("SUBT-"))
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
        /// get all sub type from db
        /// </summary>
        /// <returns>List<Subscription_Type></returns>
        public static List<Subscription_Type> GetAllSubscription_Type()
        {
            var db = new AccountingContainer();
            var SubscriptionType = db.Subscription_Type.ToList();
            return SubscriptionType;
        }
        /// <summary>
        /// get sub type by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Subscription_Type</returns>
        public static Subscription_Type GetallSubscription_TypeById(int id)
        {
            var db = new AccountingContainer();
            var Subsc = db.Subscription_Type.Find(id);
            return Subsc;
        }
        /// <summary>
        /// prepare form for create 
        /// </summary>
        /// <returns>Subscription_TypeVM</returns>
        public static Subscription_TypeVM CreateforCreation()
        {
            var db = new AccountingContainer();
            var SubscriptionVM = new Subscription_TypeVM();
            String StartCodeOfrevenue = "04";
            var lastSerial = db.Subscription_Type.OrderByDescending(c => c.Subscription_Type_ID).FirstOrDefault();
            if (lastSerial == null)
            {
                SubscriptionVM.Subscrip_Code = "SUBT-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                SubscriptionVM.Subscrip_Code = "SUBT-" + (Convert.ToInt32(lastSerial.Subscrip_Code.Substring(5, lastSerial.Subscrip_Code.Length - 5)) + 1).ToString("D3");
            }

            SubscriptionVM.MyCompList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            // SubscriptionVM.MyRvenuoList = new SelectList(db.ChartOfAccountings, "Account_Id", "Account_Name");
            var lista = db.ChartOfAccountings.Where(a=>a.ChartOfAccountings.Count()==0).ToList();

            SubscriptionVM.MyRvenuoList = new SelectList(lista.Where(a => a.Account_Code.StartsWith("040103001")), " Account_Id", "Account_Name");

            //SubscriptionVM.MyBuList = new SelectList(db.BussinesUnits, "BU_Id", "BU_Name");
            return SubscriptionVM;
        }
        /// <summary>
        /// add new sub type and save in db
        /// </summary>
        /// <param name="TypeVM"></param>
        /// <returns></returns>
        public static Subscription_Type CreateNewSubscription_Type(Subscription_TypeVM TypeVM)
        {
            var db = new AccountingContainer();
            var Subscription = new Subscription_Type();
            try
            {
                if (TypeVM != null)
                {


                    //var resultSub = db.BussinesUnits.Find(TypeVM.SelectedSubscrip_BU);
                    //if (resultSub != null)
                    //{

                    //    Subscription.Subscrip_BU = resultSub.BU_Id;
                    //    Subscription.Bussines_Unit_Name = resultSub.BU_Name;
                    //}

                    var resultCompa = db.Companies.FirstOrDefault();
                    if (resultCompa != null)
                    {
                        Subscription.Subscrip_Company = resultCompa.Comp_Id;
                    }


                    var resulEmpReven = db.ChartOfAccountings.Find(TypeVM.SelectedRvenuo_ID);
                    if (resulEmpReven != null)
                    {

                        Subscription.Rvenuo_ID = resulEmpReven.Account_Id;
                        Subscription.Account_Name = resulEmpReven.Account_Name;
                    }
                     Subscription.Subscription_Type_ID = TypeVM.Subscription_Type_ID;
                    var lastSerial = db.Subscription_Type.OrderByDescending(c => c.Subscription_Type_ID).FirstOrDefault();
                    if (lastSerial == null)
                    {
                        Subscription.Subscrip_Code = "SUBT-001";
                    }
                    else
                    {
                        //using string substring method to get the number of the last inserted
                        Subscription.Subscrip_Code = "SUBT-" + (Convert.ToInt32(lastSerial.Subscrip_Code.Substring(5, lastSerial.Subscrip_Code.Length - 5)) + 1).ToString("D3");
                    }
                    Subscription.Subscrip_Name = TypeVM.Subscrip_Name;
                    Subscription.Subscrip_Description = TypeVM.Subscrip_Description;
                    Subscription.Durationsubscription = TypeVM.Durationsubscription;
                }
                db.Subscription_Type.Add(Subscription);
                db.SaveChanges();
                db.Dispose();
                return Subscription;
            }
            catch (Exception s)
            {
                var Msg = s.Message;
                var msg = s.Message;
                var result = s.Data;
                var tt = result;
                throw;
            }
            return Subscription;
        }
        /// <summary>
        /// fill for Ediit by used its id
        /// </summary>
        /// <param name="subid"></param>
        /// <returns></returns>
        public static Subscription_TypeVM FillSubscription_TypeVM(int subid)
        {
            var db = new AccountingContainer();
            var SubTypeVM = new Subscription_TypeVM();
            String StartCodeOfrevenue = "04";
            var mySubscrip = db.Subscription_Type.Find(subid);

            //var Buid = mySubscrip.Subscrip_BU;
            var company = mySubscrip.Subscrip_Company;
            var revnu = mySubscrip.Rvenuo_ID;

            //int LastCode = (from s in db.Subscription_Type
            //                select s.Subscription_Type_ID).ToList().LastOrDefault();
            //mySubscrip.Subscrip_Code = (LastCode + 1).ToString();

            //var resultBussinesUnit = db.BussinesUnits.Where(B => B.BU_Id == Buid);
            //if (resultBussinesUnit.Any())
            //{
            //    SubTypeVM.SelectedSubscrip_BU = resultBussinesUnit.First().BU_Id;
            //}

            var resultrevnu = db.ChartOfAccountings.Where(c => c.Account_Id == revnu);
            if (resultrevnu.Any())
            {
                SubTypeVM.SelectedRvenuo_ID = resultrevnu.First().Account_Id;
            }

            var resultCompan = db.Companies.ToList();
            if (resultCompan.Any())
            {
                SubTypeVM.SelectedSubscrip_Comp = resultCompan.First().Comp_Id;
            }

            SubTypeVM.Subscrip_Description = mySubscrip.Subscrip_Description;
            SubTypeVM.Subscrip_Name = mySubscrip.Subscrip_Name;
            SubTypeVM.Subscrip_Code = mySubscrip.Subscrip_Code;
            SubTypeVM.Durationsubscription = mySubscrip.Durationsubscription;
            SubTypeVM.Subscription_Type_ID = mySubscrip.Subscription_Type_ID;

            SubTypeVM.MyCompList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            ////SubTypeVM.MyRvenuoList = new SelectList(db.ChartOfAccountings, "Account_Id", "Account_Name");
            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0).ToList();


            SubTypeVM.MyRvenuoList = new SelectList(lista.Where(a => a.Account_Code.StartsWith("040103001")), " Account_Id", "Account_Name");

            SubTypeVM.MyBuList = new SelectList(db.BussinesUnits, "BU_Id", "BU_Name");
            return SubTypeVM;
        }
        /// <summary>
        /// save change in Edit of type sub
        /// </summary>
        /// <param name="SubscTypeVM"></param>
        /// <returns></returns>
        public static Subscription_Type EditSubscriptionType(Subscription_TypeVM SubscTypeVM)
        {
            var db = new AccountingContainer();
            var SubscriptType = db.Subscription_Type.Find(SubscTypeVM.Subscription_Type_ID);

            if(SubscriptType != null)
            {
                var resultcom = db.Companies.FirstOrDefault();
                if(resultcom != null)
                {
                    SubscriptType.Subscrip_Company = resultcom.Comp_Id;
                }

                //var resultBesUnit = db.BussinesUnits.Find(SubscTypeVM.SelectedSubscrip_BU);
                //if(SubscriptType != null)
                //{
                //    SubscriptType.Subscrip_BU = resultBesUnit.BU_Id;
                //}

                var resultRev = db.ChartOfAccountings.Find(SubscTypeVM.SelectedRvenuo_ID);
                if(SubscriptType != null)
                {
                    SubscriptType.Rvenuo_ID = resultRev.Account_Id;
                }


                SubscriptType.Subscription_Type_ID = SubscTypeVM.Subscription_Type_ID;
              
                SubscriptType.Subscrip_Name = SubscTypeVM.Subscrip_Name;
                SubscriptType.Subscrip_Description = SubscTypeVM.Subscrip_Description;
                SubscriptType.Durationsubscription = SubscTypeVM.Durationsubscription;

                db.Entry(SubscriptType).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                return SubscriptType;
            }
            return null;
        }
        /// <summary>
        /// get sub_tupe by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public static Subscription_Type GetSuptypetById(int id)
        {
            var db = new AccountingContainer();
            var subb = db.Subscription_Type.Find(id);
            return subb;
        }
        /// <summary>
        /// delete sub _type by its id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int DeleteSubscriptionType(int id)
        {
            var db = new AccountingContainer();
            var subtype = db.Subscription_Type.Find(id);
            if(subtype != null)
            {
                db.Subscription_Type.Remove(subtype);
                db.SaveChanges();
                db.Dispose();
                return 1;
            }
            return 0;
        }
    }
}