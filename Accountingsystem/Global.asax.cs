using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Threading;
using System.Globalization;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace Accountingsystem
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Client.ChecedTheEmailForAllClient();

            //FixedAsset.startAllDep();
            // Category cat = new Category();
            //cat.InsertDataToDB();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Language"];
            if (cookie != null && cookie.Value != null)
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cookie.Value);
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(cookie.Value);
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ar");
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ar");
            }

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            newCulture.DateTimeFormat.ShortDatePattern = "dd-MMM-yyyy";
            newCulture.DateTimeFormat.DateSeparator = "-";
            Thread.CurrentThread.CurrentCulture = newCulture;
        }
        //void Session_Start(object sender, EventArgs e)
        //{
        //    Session.Timeout = 1;
        //}
        //void Session_End(object sender, EventArgs e)
        //{
        //    ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
        //    Session["test"].ToString();
        //    var tt = 0;
        //}
        //void Session_End(object sender, EventArgs e)
        //{
        //    // Code that runs when a session ends. 
        //    // Note: The Session_End event is raised only when the sessionstate mode
        //    // is set to InProc in the Web.config file. If session mode is set to StateServer 
        //    // or SQLServer, the event is not raised.
        //    ApplicationUser UserMan = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
        //    if (Application[UserMan.Id] != null)
        //    {
        //        if (Convert.ToString(Application[UserMan.Id]) == UserMan.Id)
        //        {
        //            Application.Contents.Remove(UserMan.Id);
        //        }
        //    }
        //}

    }
}
