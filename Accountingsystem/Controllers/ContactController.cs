using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Accountingsystem.Models;
using Accountingsystem.ViewModel;


namespace Accountingsystem.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            var contact = Contacts.GetAllConatcts();
            return View(contact);
        }

        // GET: Contact/Details/5
        public ActionResult Details(int id)
        {
            var contact = Contacts.GetContactById(id);
            return View(contact);
        }

        // GET: Contact/Create
        public ActionResult Create()
        {
            var model = Contacts.CreationVM();
            return View(model);
        }

        // POST: Contact/Create
        [HttpPost]
        public ActionResult Create(ContactVM contactVM)
        {
            try
            {
                // TODO: Add insert logic here
                var result = Contacts.CreateNewContact(contactVM);

               
                    return RedirectToAction("Index");
               

               
            }
            catch
            {
                return View();
            }
        }

        // GET: Contact/Edit/5
        public ActionResult Edit(int id)
        {
            var contact = Contacts.FillConatct(id);
            return View(contact);
        }

        // POST: Contact/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ContactVM contactVM)
        {
            try
            {
                // TODO: Add update logic here
                var contact = Contacts.editContact(contactVM);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Contact/Delete/5
        public ActionResult Delete(int id)
        {
            var contact = Contacts.deleteContact(id);
            return View(contact);
        }

        // POST: Contact/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var contact = Contacts.deleteContact(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(collection);
            }
        }


        [HttpGet]
        public ActionResult PartialDialog()
        {
            var custvm = Customer.AddCustomerVM();
            return PartialView("CuStomerPartialView", custvm);
        }

        [HttpPost]
        public ActionResult AddNewCustomerPartial(CustomerVM masterVM)
        {

            var result = Customer.AddNewCustomer(masterVM);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [HttpGet]
        public ActionResult GetCustomerDropdownList()
        {
            var customer = Contacts.GetCustomerList();
            return PartialView("CuStomerDropDownList", customer);
        }
    }
}
