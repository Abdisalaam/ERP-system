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
    public class CurrencyController : Controller
    {
        // GET: Currency
        public ActionResult Index(string sasa, string msg)
        {
            ViewBag.saber = sasa;
            ViewBag.Message = msg;
            var models = Currency.GetAllcurrency();
            return View(models);
        }

        // GET: Currency/Details/5
        public ActionResult Details(int id)
        {
            var model = Currency.GetCurrencyById(id);
            return View(model);
        }

        // GET: Currency/Create
        public ActionResult Create()
        {
            var model = new CurrencyVM();
            return View(model);
        }

        // POST: Currency/Create
        [HttpPost]
        public ActionResult Create(CurrencyVM cuuVM)
        {
            try
            {
                // TODO: Add insert logic here
                var NewCurrency = Currency.CreateCurrency(cuuVM);
                if (NewCurrency != null)
                {
                    return RedirectToAction("Details", new { id = NewCurrency.Currency_Id });
                }
                else
                {
                    string msg = Resources.validate.Error;
                    return RedirectToAction("index", new { msg = msg });
                }
               
              
            }
            catch
            {
                return View();
            }
        }

        // GET: Currency/Edit/5
        public ActionResult Edit(int id)
        {
            if (id < 1)
            {
                string msg = Resources.validate.Error;
                return RedirectToAction("Index", new { msg = msg });
            }
            var currency = Currency.FillCurrncy(id);
            return View(currency);
        }

        // POST: Currency/Edit/5
        [HttpPost]
        public ActionResult Edit(CurrencyVM cuuVM)
        {
            try
            {
                // TODO: Add update logic here
                var currency = Currency.EidtCurrncy(cuuVM);
                if (currency != null)
                {
                    return RedirectToAction("Details", new { id = currency.Currency_Id });
                }
                else
                {
                    string msg = Resources.validate.Error;
                    return RedirectToAction("index", new { msg = msg });
                }
               
            }
            catch
            {
                return View();
            }
        }

        // GET: Currency/Delete/5
        public ActionResult Delete(int id)
        {
            var currency = Currency.FindCurrencyById(id);
            return View(currency);
        }

        // POST: Currency/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                int currency = Currency.DelCurrency(id);
                if (currency == 0)
                {
                    var msg = "لا يمكنك حذف العملة,لانها مستخدمة في النظام";
                    return RedirectToAction("Index",new { msg=msg});
                }
                else
                {
                    var msg = "تم حذف العملة";
                    return RedirectToAction("Index", new { msg = msg });
                }
             
               
                
                
            }
            catch
            {
                return View(collection);
            }
        }
    }
}