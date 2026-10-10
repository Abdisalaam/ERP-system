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
    public class CategoryController : Controller
    {


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
              
                worksheet.Cells[1, 1] = "اسم التصنيف";
                //worksheet.Cells[1, 2] = "رقم التصنيف";
                int row = 2;
                foreach (Category item in Category.GetAllCategory())
                {
                    //worksheet.Cells[row, 1] = item.Customer_Id;
                    worksheet.Cells[row, 1] = item.Cat_Descrip;
                   // worksheet.Cells[row, 2] = item.Cat_Code;
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

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase excelfile)
        {
            var msg = Category.Import(excelfile);
            return RedirectToAction("Index", new { msg = msg });
        }
        public JsonResult CheckCode(string Cat_Code)
        {
            if (Category.containCode(Cat_Code))
            {
                var msg = "CAT- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        // GET: Category
        public ActionResult Index()
        {
            var models = Category.GetAllCategory();
            return View(models);
        }

        // GET: Category/Details/5
        public ActionResult Details(int id)
        {
            var model = Category.GetCategoryById(id);
            return View(model);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء تصنيف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Category.CreateVMForCreation();
            return View(model);
        }

        // POST: Category/Create
        [HttpPost]
        public ActionResult Create(CategroyVM categroyVM)
        {
            try
            {
                // TODO: Add insert logic here
                var model = Category.CreateCatgery(categroyVM);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل تصنيف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Category.FillTheViewModel(id);
            return View(model);
        }

        // POST: Category/Edit/5
        [HttpPost]
        public ActionResult Edit( CategroyVM categroyVM)
        {
            try
            {
                var model = Category.EditeCatgery(categroyVM);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Category/Delete/5
        public ActionResult Delete(int id)
        {
            var model = Category.GetCategoryById(id);
            return View(model);
        }

        // POST: Category/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var model = Category.DeleteCategory(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}