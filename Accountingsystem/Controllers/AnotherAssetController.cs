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
    public class AnotherAssetController : Controller
    {
        // GET: AnotherAsset
        public ActionResult Index()
        {            
            var searchWithNo = AnotherAsset.getAllAnotherAssets();
            return View(searchWithNo);
        }

        // GET: AnotherAsset/Details/5
        public ActionResult Details(int id)
        {
            var findAsset = AnotherAsset.GetAnotherAssetByID(id);
            return View(findAsset);
        }

        // GET: AnotherAsset/Create
        public ActionResult Create()
        {
            var AnotherAssets = new AnotherAssetVM();
            return View(AnotherAssets);
        }

        // POST: AnotherAsset/Create
        [HttpPost]
        public ActionResult Create(AnotherAssetVM vm)
        {
            try
            {
                AnotherAsset.AddNewAnotherAsset(vm);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(vm);
            }
        }

        // GET: AnotherAsset/Edit/5
        public ActionResult Edit(int id)
        {
            var editAnAsst = AnotherAsset.FillTheViewModel(id);
            if (editAnAsst.AnothrAsst_IDs != id)
            {
                string msg = "No Asset  With this Code";
                return RedirectToAction("Index", new { msg = msg });
            }

            return View(editAnAsst);
        }

        // POST: AnotherAsset/Edit/5
        [HttpPost]
        public ActionResult Edit(AnotherAssetVM vm)
        {
            try
            {
                var AnAsset = AnotherAsset.EditAnotherAsset(vm);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(vm);
            }
        }

        // GET: AnotherAsset/Delete/5
        public ActionResult Delete(int id)
        {
            var myAsset = AnotherAsset.GetAnotherAssetByID(id);
            return View(myAsset);

        }

        // POST: AnotherAsset/Delete/5
        [HttpPost]
        public ActionResult Delete(int ID, AnotherAssetVM col)
        {
            try
            {
                // TODO: Add delete logic here
                int deprt = AnotherAsset.DeleteAnotherAsset(ID);
                if (deprt == 1)
                {
                    return RedirectToAction("Index");

                }
                return View(col);
            }
            catch
            {
                return View();
            }
        }
    }
}