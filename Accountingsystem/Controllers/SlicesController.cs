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
    public class SlicesController : Controller
    {
        public ActionResult Getexcel()
        {
            return File("~/FiLeDownLoad/Slice.xlsx", "application/xlsx", Server.UrlEncode("~/FiLeDownLoad/Slice.xlsx"));
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
                worksheet.Cells[1, 1] = "اسم الشريحة";
              
                int row = 2;
                foreach (Slices item in Slices.GetAllSlices())
                {
                    //worksheet.Cells[row, 1] = item.Customer_Id;
                    worksheet.Cells[row, 1] = item.Slice_Name;
                  
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
            try
            {
                var msg = Slices.Import(excelfile);
                return RedirectToAction("Index", new { msg = msg });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { msg = ex.Message });

            }

        }



        public JsonResult CheckCode(string Slice_Code)
        {
            if (Slices.containCode(Slice_Code))
            {
                var msg = "SLI- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        // GET: Slices
        public ActionResult Index()
        {
            var models = Slices.GetAllSlices();
            return View(models);
        }

        // GET: Slices/Details/5
        public ActionResult Details(int id)
        {
            var model = Slices.GetSliceById(id);
            return View(model);
        }

        // GET: Slices/Create
        public ActionResult Create()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء شريحة سعر");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Slices.AddSlicesForCreation();
            return View(model);
        }

        // POST: Slices/Create
        [HttpPost]
        public ActionResult Create(SlicesVM slicesVM)
        {
            try
            {
                // TODO: Add insert logic here

                Slices.CreateSlice(slicesVM);
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                var msg = e.Message;
                return View();
            }
        }

        // GET: Slices/Edit/5
        public ActionResult Edit(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل شريحة سعر");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Slices.FillForEdit(id);
            return View(model);
        }

        // POST: Slices/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, SlicesVM slicesVM)
        {
            try
            {
                // TODO: Add update logic here
                Slices.EditForSlice(slicesVM);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                var msg = e.Message;
                return View();
            }
        }

        // GET: Slices/Delete/5
        public ActionResult Delete(int id)
        {
            var model = Slices.GetSliceById(id);
            return View(model);
        }

        // POST: Slices/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                Slices.DeleteSlice(id);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                var msg = e.Message;
                return View();
            }
        }
    }
}
