using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication4.Controllers
{
    [Authorize]
    public class UploadFileController : Controller
    {
        // GET: UploadFile
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult UploadFile()
        {
            var httpPostedFileBase = Request.Files["FileName"];
            if (httpPostedFileBase != null && httpPostedFileBase.ContentLength > 0)
            {
                string extension = System.IO.Path.GetExtension(httpPostedFileBase.FileName);
                string path1 = string.Format("{0}/{1}", Server.MapPath("~/SavedFiles"), extension);
                if (System.IO.File.Exists(path1))
                    System.IO.File.Delete(path1);

                httpPostedFileBase.SaveAs(path1);
            }
            ViewData["Status"] = "Success";

           // return RedirectToAction();
            return View("Index");
        }



        // GET: UploadFile/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UploadFile/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UploadFile/Create
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

        // GET: UploadFile/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UploadFile/Edit/5
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

        // GET: UploadFile/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UploadFile/Delete/5
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
