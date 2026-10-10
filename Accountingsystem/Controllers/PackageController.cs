using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using System.Web.Mvc;

namespace Accountingsystem.Controllers
{
    [Authorize(Roles = "مالك النظام")]
    public class PackageController : Controller
    {
        // GET: Package
        public ActionResult Index()
        {
            var pack = Package.getAllPackage();
            return View(pack);
        }

        // GET: Package/Details/5
        public ActionResult Details(int id)
        {
            var pack = Package.getThatPackageById(id);
            return View(pack);
        }

        // GET: Package/Create
        public ActionResult Create()
        {
            var pack = Package.createVM();
            return View(pack);
          
        }

        // POST: Package/Create
        [HttpPost]
        public ActionResult Create(PackageVM vm)
        {
            try
            {
                var pack = Package.CreatePackagePost(vm);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Package/Edit/5
        public ActionResult Edit(int id)
        {
            var pack = Package.fillForEditing(id);
            return View(pack);
        }

        // POST: Package/Edit/5
        [HttpPost]
        public ActionResult Edit(PackageVM vm)
        {
            try
            {
                var pack = Package.EditPost(vm);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Package/Delete/5
        public ActionResult Delete(int id)
        {
            var pack = Package.getThatPackageById(id);
            return View(pack);
        }

        // POST: Package/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var pack = Package.deletePackage(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
