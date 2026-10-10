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
    public class Emp_EvaluationController : Controller
    {
        // GET: Emp_Evaluation
        public ActionResult Index()
        {
            var models = Emp_Evaluation.getAllData();
            return View(models);
        }

        // GET: Emp_Evaluation/Details/5
        public ActionResult Details(int id)
        {
            var model = Emp_Evaluation.GetEvaluationById(id);
            return View(model);
        }

        // GET: Emp_Evaluation/Create
        public ActionResult Create()
        {
            var model = Emp_Evaluation.CreationForEvaluation();
            return View(model);
        }

        // POST: Emp_Evaluation/Create
        [HttpPost]
        public ActionResult Create(Emp_EvaluationVM empEvalVM)
        {
            try
            {
                // TODO: Add insert logic here

                var model = Emp_Evaluation.CreateEmpEvaluation(empEvalVM);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                var msg = ex.Message;
                return View(empEvalVM);
            }
        }

        // GET: Emp_Evaluation/Edit/5
        public ActionResult Edit(int id)
        {
            var model = Emp_Evaluation.FillDataForEditing(id);
            return View(model);
        }

        // POST: Emp_Evaluation/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Emp_EvaluationVM empEvalVM)
        {
            try
            {
                // TODO: Add update logic here
                var model = Emp_Evaluation.EditEmployeeEvaluation(empEvalVM);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return View();
            }
        }

        // GET: Emp_Evaluation/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Emp_Evaluation/Delete/5
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
