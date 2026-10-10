using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using Newtonsoft.Json;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Controllers
{
    public class SewingController : Controller
    {
        string protocol = "http";
        // GET: Sewing
        public ActionResult Index(int ? all)
        {
            var list = Sewing.GetAllsewing(all);
            return View(list);
        }

        // GET: Sewing/Details/5
        public ActionResult Details(int id)
        {
            var sewing = Sewing.GetsewingById(id);
            return View(sewing);
        }

        // GET: Sewing/Create
        public ActionResult Create()
        {
          var sewing = Sewing.FillForCreate();
            return View(sewing);
        }

        // POST: Sewing/Create
        [HttpPost]
        public ActionResult Create(string detail,string Date,string DueDate, string CustomerId, string ProductId, string NumberOfSewing, string order,string Amount,string quantity)
        {
            try
            {
                dynamic batchList = JsonConvert.DeserializeObject<dynamic>(detail);
                int msg = Sewing.CreatePost(batchList,Date,DueDate,CustomerId, ProductId, NumberOfSewing,order,Amount, quantity);
                var fullUrl = this.Url.Action("Details", "Sewing", new { id = msg }, this.Request.Url.Scheme);
                return Json(fullUrl, JsonRequestBehavior.AllowGet);

             
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: Sewing/Edit/5
        public ActionResult Edit(int id,int?copy )
        {
            var sewing = Sewing.FillForEdit(id,copy);
            return View(sewing);
        }

        // POST: Sewing/Edit/5
        [HttpPost]
        public ActionResult Edit(string detail, string Date, string DueDate, string CustomerId, string ProductId, string NumberOfSewing, string iscopy, string Amount,string Id,string quantity)
        {
            try
            {
                dynamic batchList = JsonConvert.DeserializeObject<dynamic>(detail);
                int msg = 0;
                if (iscopy == "1")
                {
                    msg = Sewing.CreatePost(batchList, Date, DueDate, CustomerId, ProductId, NumberOfSewing, iscopy, Amount, quantity);
                }
                else
                {
                   
                    msg = Sewing.EditPost(batchList, Date, DueDate, CustomerId, ProductId, NumberOfSewing, iscopy, Amount, Id, quantity);
                }
                var fullUrl = this.Url.Action("Details", "Sewing", new { id = msg }, this.Request.Url.Scheme);
                return Json(fullUrl, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Sewing/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Sewing/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult EditStatus(int id, int status)
        {
            var sewing = Sewing.Editstaus(id, status);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult PartialDialog()
        {
            var custvm = Customer.AddCustomerVM();
            return PartialView("CustomerPartialView", custvm);
        }

        public ActionResult AddNewCustomerPartial(CustomerVM masterVM)
        {
            HttpFileCollectionBase files = Request.Files;
            if (files.Count > 0)
            {
                masterVM.customerFiles = files;
            }
            var result = Customer.AddNewCustomer(masterVM);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [HttpGet]
        public ActionResult GetCustomerDropdownList()
        {
            var customer = InvoiceMaster.GetCustomerList();
            return new JsonResult { Data = customer, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult PrintSewing(int id)
        {
           
                var model = Sewing.GetsewingById(id);
                string customSwitchesheader = string.Format(" --print-media-type --page-offset 2 --header-center [page] --allow {0} --header-html {0} --header-spacing 5 ",
    Url.Action("HeaderReport", "InvoiceQRExternalUseres", new { Branch_Id = 0 }, protocol));

                return new ViewAsPdf(model)
                {

                    
                    CustomSwitches = customSwitchesheader
                }
                ;
              
            
        }
    }
}
