using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
     
        public ActionResult Index()
        {
            var Account = Accounts.getAllAccount();

            return View(Account);
        }

        public ActionResult getAccountsJsondrobDown()
        {
            //List<Accounts> accounts = Accounts.getAllAccountsFordrobDownList();
            List<dynamic> result = new List<dynamic>();
            //foreach (var accou in accounts)
            //{
            //    var data = new
            //    {
            //        // the value of each select item
            //        id = accou.Accounts_Id,
            //        // showing both code and name in the drop-down list 
            //        text = accou.Account_Name + "\t" + accou.Account_Code
            //    };
            //    result.Add(data);
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}