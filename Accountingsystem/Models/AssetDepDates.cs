using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class AssetDepDates
    {/// <summary>
     /// get all assDep by asset id
     /// </summary>
     /// <param name="id"></param>
     /// <returns>List<AssetDepDates></returns>
        public static List<AssetDepDates> getAssetDepByID(int id)
        {
            var db = new AccountingContainer();
            var thisAssetDep = db.AssetDepDates.Where(e=>e.Asset_ID==id).ToList();
            return thisAssetDep;
        }
    }
    public partial class DeprTaskStatus
    {/// <summary>
     /// get status of asset by its id
     /// </summary>
     /// <param name="id"></param>
     /// <returns>DeprTaskStatus</returns>
        public static DeprTaskStatus getStatusByID(int id)
        {
            var db = new AccountingContainer();
            var thisAssetDep = db.DeprTaskStatus.Where(s=>s.Asset_id==id).ToList().FirstOrDefault(); 
            return thisAssetDep;
        }
    }
}