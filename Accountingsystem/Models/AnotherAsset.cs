using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Accountingsystem.Models
{
    public partial class AnotherAsset
    {/// <summary>
     /// get all another asset from db
     /// </summary>
     /// <returns>List<AnotherAsset></returns>
        public static List<AnotherAsset> getAllAnotherAssets12325()
        {
            var db = new AccountingContainer();
            var LAssets = db.AnotherAssets.ToList();
            return LAssets;
        }
        /// <summary>
        ///get all another asset from db 
        /// </summary>
        /// <returns>List<AnotherAsset></returns>
        public static List<AnotherAsset> getAllAnotherAssets()
        {
            var db = new AccountingContainer();
            var LAssets = db.AnotherAssets.ToList();
            return LAssets;
        }
        /// <summary>
        /// get asset information by its id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static AnotherAsset GetAnotherAssetByID(int ID)
        {
            var db = new AccountingContainer();
            AnotherAsset anasset = db.AnotherAssets.Find(ID);

            return anasset;
        }
        /// <summary>
        /// adding  new AnotherAsset TO system  but it  will not make anything else Just Adding
        /// </summary>
        /// <param name="FVM"></param>
        /// <returns>AnotherAsset</returns>

        public static AnotherAsset AddNewAnotherAsset(AnotherAssetVM FVM)
        {
            var db = new AccountingContainer();
            var AnAsset = new AnotherAsset();
            try
            {
                AnAsset.AnothrAsst_Desc = FVM.AnothrAsst_Descs;
                AnAsset.AnothrAsst_Price = FVM.AnothrAsst_Prices;
                db.AnotherAssets.Add(AnAsset);
                db.SaveChanges();
                db.Dispose();
                return AnAsset;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                throw;
            }
        }

        /// <summary>
        /// edit another asset and save changes in db
        /// </summary>
        /// <param name="FVM"></param>
        /// <returns>AnotherAsset</returns>
        public static AnotherAsset EditAnotherAsset(AnotherAssetVM FVM)
        {
            var db = new AccountingContainer();
            AnotherAsset MyAsset = db.AnotherAssets.Find(FVM.AnothrAsst_IDs);
            if (MyAsset != null)
            {
                MyAsset.AnothrAsst_Desc = FVM.AnothrAsst_Descs;
                MyAsset.AnothrAsst_Price = FVM.AnothrAsst_Prices;
                // LinQ  
                db.Entry(MyAsset).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                return MyAsset;
            }


            return null;
        }
        /// <summary>
        /// fill for create anew another asset
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>AnotherAssetVM</returns>
        public static AnotherAssetVM FillTheViewModel(int ID)
        {
            var editModel = GetAnotherAssetByID(ID);
            AnotherAssetVM FVM = new AnotherAssetVM();
            FVM.AnothrAsst_Descs = editModel.AnothrAsst_Desc;
            FVM.AnothrAsst_Prices = editModel.AnothrAsst_Price;
            FVM.AnothrAsst_IDs = editModel.AnothrAsst_ID;
            return FVM;

        }
        /// <summary>
        /// delete another asset by its id
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
    }
}