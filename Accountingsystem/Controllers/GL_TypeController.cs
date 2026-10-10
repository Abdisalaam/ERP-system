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
    public class GL_TypeController : Controller
    {
        // GET: GL_Type
        public ActionResult Index()
        {
            var glType = GL_Type.getAllData();
            return View(glType);
        }

        // GET: GL_Type/Details/5
        public ActionResult Details(int id)
        {
            var glType = GL_Type.getDetailOfTypeOfGL(id);
            return View(glType);
        }

        // GET: GL_Type/Create
        public ActionResult Create()
        {
            var glTypevm = new GL_TypeVM();
            return View(glTypevm);
        }

        // POST: GL_Type/Create
        [HttpPost]
        public ActionResult Create(GL_TypeVM glTypeVM)
        {
            try
            {
                var glType = GL_Type.createNewGLType(glTypeVM);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: GL_Type/Edit/5
        public ActionResult Edit(int id)
        {
            var glType = GL_Type.fillDataForEditing(id);
            return View(glType);
        }

        // POST: GL_Type/Edit/5
        [HttpPost]
        public ActionResult Edit(GL_TypeVM glTypeVM)
        {
            try
            {
                var gl = GL_Type.editTheData(glTypeVM);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: GL_Type/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: GL_Type/Delete/5
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