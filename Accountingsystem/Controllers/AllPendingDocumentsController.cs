using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class AllPendingDocumentsController : Controller
    {
        // GET: AllPendingDocuments
        [HttpGet]
        public ActionResult IndexPendingPurchSection(string Message)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "اعتماد المستندات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var models = AllPendingDocumanets.GetAllPendings();
            ViewBag.Message = Message;
           return View(models);

        }


        [HttpPost]
        public ActionResult IndexPendingPurchSection(FormCollection formCollection)
        {

            var counter = 0;
            var models = AllPendingDocumanets.GetAllPendings().ToList();
            List<AllSectionPendingVM> AVM = new List<AllSectionPendingVM>();
            string a = "";
            foreach (var item in models)
            {
                List<bool> bools = new List<bool>();
                try
                {
                     a = formCollection[item.Serial].ToString();
                }
                catch
                {
                    a = "false";
                }
                //if (item.Serial.Contains(","))
                //{
                //     bools = formCollection[item.Serial].Split(',').Select(n => string.Compare(n, "true", true) == 0 ? true : false).ToList();
                //}
               
             

                    if (a!="false")
                    {
                        AllSectionPendingVM avm = new AllSectionPendingVM();                        
                        avm.MasterID = item.MasterID;
                        avm.Serial = item.Serial;
                        avm.Checked = true;
                        avm.Description = item.Description;
                        AVM.Add(avm);
                        counter++;

                    }
                }

            //if (counter == 0)
            //{
            //    foreach (var items in models)
            //    {
            //        AllSectionPendingVM avm = new AllSectionPendingVM();                  
            //        avm.MasterID = items.MasterID;
            //        avm.Serial = items.Serial;
            //        avm.Checked = true;
            //        avm.Description = items.Description;
            //        AVM.Add(avm);

            //    }
            //}
            var msg = "";
            if (AVM.Count() != 0)
            {

                var modeltoPost = AllPendingDocumanets.PostAllCHeckedDocuments(AVM);
                msg = Resources.validate.AllowedWasApproved;
            }
            else
            {
                msg = Resources.validate.youdidnotchoosebonds;
            }
            return RedirectToAction("IndexPendingPurchSection",new {Message= msg });
        }
       
        // GET: AllPendingDocuments/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AllPendingDocuments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AllPendingDocuments/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AllPendingDocuments/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AllPendingDocuments/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AllPendingDocuments/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AllPendingDocuments/Delete/5
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
    }
}
