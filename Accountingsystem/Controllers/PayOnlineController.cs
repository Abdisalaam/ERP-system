using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Controllers
{
    public class PayOnlineController : Controller
    {
   
        public ActionResult GetInvoiceById(int id)
        {
            var invoice = InvoiceMaster.GetInvoiceDetailsById(id);
            return View();
        }
        public ActionResult GetSubscribtionInvoiceById(int id)
        {
            var invoice = Subscription.GetThisINvoiceDetail(id);
            return View();
        }
        public ActionResult GetProjectInvoiceById(int id)
        {
            var invoice = Project.GetProjectInvoice(id);
            return View();
        }
    }
}