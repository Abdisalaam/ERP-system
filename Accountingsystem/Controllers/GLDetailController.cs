using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class GLDetailController : Controller
    {
        // GET: GLDetail
        public ActionResult Index()
        {
            var gldetail = GLDetail.getAllGLDetail();
            return View(gldetail);
        }

        // GET: GLDetail/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: GLDetail/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GLDetail/Create
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

        // GET: GLDetail/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: GLDetail/Edit/5
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

        // GET: GLDetail/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: GLDetail/Delete/5
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