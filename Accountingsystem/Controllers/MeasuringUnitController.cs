using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Office.Interop.Excel;
using System.Web.Mvc;

using System.IO;
using System.Runtime.InteropServices;
using Rotativa;
using System.Text;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web.UI;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class MeasuringUnitController : Controller
    {
        public ActionResult Getexcel()
        {
            return File("~/FiLeDownLoad/MeasureUnit.xlsx", "application/xlsx", Server.UrlEncode("~/FiLeDownLoad/MeasureUnit.xlsx"));
        }

        public ActionResult Export()
        {
            HttpPostedFileBase file = Request.Files["goods_in_data.xls"];
            string directory = @"Uploads\";
            try
            {
                Application application = new Application();
                Workbook workbook = application.Workbooks.Add(System.Reflection.Missing.Value);
                Worksheet worksheet = workbook.ActiveSheet;
                Customer cust = new Customer();
                //worksheet.Cells[1, 1] = "Customer_Id";
                worksheet.Cells[1, 1] = "اسم وحده القياس";              

                int row = 2;
                foreach (MeasuringUnit item in MeasuringUnit.GetMeasruingUnit())
                {
                    //worksheet.Cells[row, 1] = item.Customer_Id;
                    worksheet.Cells[row, 1] = item.MUnit_Descrip;                  
                    row++;
                }

                //workbook.get_Range("A1", "O1").EntireColumn.AutoFite();
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    file.SaveAs(Path.Combine(directory, fileName));
                }
                //workbook.SaveAs("~\\Files excel\\myCustomersList.xls");
                workbook.Close();
                Marshal.ReleaseComObject(workbook);
                application.Quit();
                Marshal.FinalReleaseComObject(workbook);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }

            return RedirectToAction("Index");
        }


        public ActionResult Import(HttpPostedFileBase excelfile)
        {
            var msg = MeasuringUnit.Import(excelfile);
            return RedirectToAction("Index", new { msg = msg });
        }


        public JsonResult CheckCode(string MUnit_Code)
        {
            if (MeasuringUnit.containCode(MUnit_Code))
            {
                var msg = "UNI- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        // GET: MeasuringUnit
        public ActionResult Index()
        {
            var model = MeasuringUnit.GetMeasruingUnit();
            return View(model);
        }

        // GET: MeasuringUnit/Details/5
        public ActionResult Details(int id)
        {
            var model = MeasuringUnit.GetMeasruingUnitById(id);
            return View(model);
        }

        // GET: MeasuringUnit/Create
        public ActionResult Create()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء وحدة قياس");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = MeasuringUnit.CreateVMForCreation();
            return View(model);
        }

        // POST: MeasuringUnit/Create
        [HttpPost]
        public ActionResult Create(MeasureVM measurVM)
        {
            try
            {
                // TODO: Add insert logic here
                var model = MeasuringUnit.CreateMeasruingUnit(measurVM);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: MeasuringUnit/Edit/5
        public ActionResult Edit(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل وحدة قياس");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = MeasuringUnit.FillTheViewModel(id);
            return View(model);
        }

        // POST: MeasuringUnit/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, MeasureVM measurVM)
        {
            try
            {
                
                // TODO: Add update logic here
                var model = MeasuringUnit.Editemeasruingunitvm(measurVM);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: MeasuringUnit/Delete/5
        public ActionResult Delete(int id)
        {
            var model = MeasuringUnit.GetMeasruingUnitById(id);
            return View(model);
        }

        // POST: MeasuringUnit/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var model = MeasuringUnit.DeleteMeasruingUnit(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}