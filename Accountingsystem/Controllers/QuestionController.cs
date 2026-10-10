using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Controllers
{
    public class QuestionController : Controller
    {
        // GET: Question
        public ActionResult Index()
        {
            var quest = Question.GetAllQuestions();
            return View(quest);
        }

        // GET: Question/Details/5
        public ActionResult Details(int id)
        {
            var quest = Question.GetQuestionById(id);
            return View(quest);
        }

        // GET: Question/Create
        public ActionResult Create()
        {
            var quest = Question.CreateVMForCreation();
            return View(quest);
        }

        // POST: Question/Create
        [HttpPost]
        public ActionResult Create(QuestionVM vm)
        {
            try
            {
                // TODO: Add insert logic here
                var result = Question.Create(vm);
                return RedirectToAction("Details", new { id = result });
                
              
            }
            catch
            {
                return View();
            }
        }

        // GET: Question/Edit/5
        public ActionResult Edit(int id)
        {
            var quest = Question.FillTheViewModel(id);
            return View(quest);
        }

        // POST: Question/Edit/5
        [HttpPost]
        public ActionResult Edit( QuestionVM vm)
        {
            try
            {
                var result = Question.EditeQuestion(vm);
                return RedirectToAction("Details", new { id = result.Id });
            }
            catch
            {
                return View();
            }
        }

        // GET: Question/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Question/Delete/5
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
        public ActionResult ADDAnswer(int questId)
        {
            var quest = Question.AddAnswer(questId);
            return View(quest);
        }

        // POST: Question/Create
        [HttpPost]
        public ActionResult ADDAnswer(AnswerVM vm)
        {
            try
            {
                // TODO: Add insert logic here
                var result = Question.AddAnswer(vm);
                return RedirectToAction("Details", new { id = result.QuestionId });


            }
            catch(Exception ex)
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult EditAnswer(int id)
        {
            var quest = Question.EditAnswer(id);
            return View(quest);
        }

        // POST: Question/Create
        [HttpPost]
        public ActionResult EditAnswer(AnswerVM vm)
        {
            try
            {
                // TODO: Add insert logic here
                var result = Question.EditAnswer(vm);
                return RedirectToAction("Details", new { id = result.QuestionId });


            }
            catch (Exception ex)
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult DeleteQuestion(int id)
        {
            var quest = Question.DeleteQuestions(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult DeleteAnswer(int id)
        {
            var quest = Question.DeleteAnswers(id);
            return RedirectToAction("Details", new { id = quest });
        }
    }
}
