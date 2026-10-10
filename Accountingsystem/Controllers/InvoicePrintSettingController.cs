using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Controllers
{
    public class InvoicePrintSettingController : Controller
    {
        // GET: InvoicePrintSetting
        public ActionResult Index()
        {
            var model = InvoicePrintSetting.getAllInvoicePrint();
            return View(model);
        }

        // GET: InvoicePrintSetting/Details/5
        public ActionResult Details(int id)
        {
            var model = InvoicePrintSetting.GetCategoryById(id);
            return View(model);
        }

        // GET: InvoicePrintSetting/Create
        public ActionResult Create()
        {
            var model = InvoicePrintSetting.CreateVMForCreation();
            return View(model);
        }

        // POST: InvoicePrintSetting/Create
        [HttpPost]
        public ActionResult Create(InvoicePrintSettingVM vm)
        {
            try
            {
                var model = InvoicePrintSetting.CreateInvoicePrint(vm);
                return RedirectToAction("Index");


            }
            catch
            {
                return View();
            }
        }

        // GET: InvoicePrintSetting/Edit/5
        public ActionResult Edit(int id)
        {
            var model = InvoicePrintSetting.FillTheViewModel(id);
            return View(model);
           
        }

        // POST: InvoicePrintSetting/Edit/5
        [HttpPost]
        public ActionResult Edit(InvoicePrintSettingVM vm)
        {
            var model = InvoicePrintSetting.Edit(vm);
            return RedirectToAction("Index");
        }

    

    }
}
