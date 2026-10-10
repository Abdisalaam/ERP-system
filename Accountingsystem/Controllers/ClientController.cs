using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        // GET: Client
        public ActionResult Index()
        {
            var List = Client.getAllClient();
            return View(List);
        }

        // GET: Client/Details/5
        public ActionResult Details(int id)
        {
            var client = Client.getClientById(id);
            return View();
        }

        // GET: Client/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Client/Create
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

        // GET: Client/Edit/5
        public ActionResult Edit(int id)
        {
            var client = Client.fillForEdit(id);
            return View(client);
        }

        // POST: Client/Edit/5
        [HttpPost]
        public ActionResult Edit(ClientVM vm)
        {
            try
            {
                var client = Client.EditForClient(vm);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Client/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Client/Delete/5
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
        public ActionResult RenewPackageForClient(int id)
        {
            var client = Client.renewPackage(id);

            return RedirectToAction("Index");

        }
    }
}
