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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class PartnerController : Controller
    {

        public ActionResult Export(int compid)
        {
            HttpPostedFileBase file = Request.Files["goods_in_data.xls"];
            string directory = @"Uploads\";
            try
            {
                Application application = new Application();
                Workbook workbook = application.Workbooks.Add(System.Reflection.Missing.Value);
                Worksheet worksheet = workbook.ActiveSheet;
               
                worksheet.Cells[1, 1] = "اسم الشريكك";
                worksheet.Cells[1, 2] = "هاتف الشريك";
                worksheet.Cells[1, 3] = " رصيد الشريك";
                worksheet.Cells[1, 4] = "ايميل الشريك";
                worksheet.Cells[1, 5] = "رقم الشركة ";
                worksheet.Cells[1, 6] = " جنسية الشريك ";
                //worksheet.Cells[1, 7] = " تاريخ ميلاد الشريك ";
                //worksheet.Cells[1, 8] = " تاريخ اضافة الشريك ";

                int row = 2;
                foreach (Partner item in Partner.GetPartnerById(compid))
                {
                    worksheet.Cells[row, 1] = item.Part_Name;
                    worksheet.Cells[row, 2] = item.Part_Phone;
                    worksheet.Cells[row, 3] = item.Balance_Amount;
                    worksheet.Cells[row, 4] = item.Part_Email;
                    worksheet.Cells[row, 5] = item.CompanyComp_Id;
                    worksheet.Cells[row, 6] = item.Part_Nationality;
                    //worksheet.Cells[row, 7] = item.Part_Birthday;
                    //worksheet.Cells[row, 8] = item.Part_AddedDate;
                 

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

            return RedirectToAction("Index", "Partner", new { id = compid });
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase excelfile , int compid)
        {
            var msg = Partner.Import(excelfile);
            return RedirectToAction("Index", "Partner", new { id = compid });
        }


        // GET: Partner
        public ActionResult Index(int compid,string msg)
        {

            var allpartner = Partner.GetPartnerById(compid);
            ViewBag.Message = msg;
            if (allpartner.Count != 0)
            {
                ViewBag.compid = compid;
                return View(allpartner);

            }
            else
            {
                ViewBag.compid = compid;
                return RedirectToAction("Create", "Partner", new { id = compid});


            }


            /* var allpartners = Partner.GetPartners();
             return View(allpartners);*/
        }

        // GET: Partner/Details/5
        public ActionResult Details(int id)
        {
            var partnerdetail = Partner.GetPartById(id);

            return View(partnerdetail);
        }

        // GET: Partner/Create
        public ActionResult Create(int id,string msg)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء شريك");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var partvm = Partner.CreateVMForCreation(id);
            ViewBag.Message = msg;
            return View(partvm);
        }

        // POST: Partner/Create
        [HttpPost]
        public ActionResult Create(PartnerVM partVM)
        {
            var ComID = 0;
            try
            {
                // TODO: Add insert logic here
                var createpart = Partner.CreatePartner(partVM);
                ComID = createpart.CompanyComp_Id;
               var msg = "تم انشاء الشريك";
               
                // return RedirectToAction("Index");
                return RedirectToAction("Details", "Partner", new { id = createpart.Part_Id});

            }
            catch(Exception ex)
            {
                var msg = "لم يتم انشاء الشريك";
                if (ComID == 0)
                {
                    ComID = 1;
                }
                return RedirectToAction("Index", "Partner", new { compid = ComID, msg = msg });
            }
        }

        // GET: Partner/Edit/5
        public ActionResult Edit(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل شريك");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var partneredit = Partner.FillEditPartner(id);
            return View(partneredit);
        }

        // POST: Partner/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, PartnerVM part)
        {
            try
            {
                // TODO: Add update logic here
                var editpart = Partner.EditPartner(part);
                return RedirectToAction("Details", "Partner", new { id = editpart.Part_Id });
            }
            catch
            {
                return View();
            }
        }

        // GET: Partner/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Partner/Delete/5
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




        public FileResult ReturnpartFile(int fileId)
        {

            var document = Partner.getFileById(fileId);
            var cd = new System.Net.Mime.ContentDisposition
            {
                // for example foo.bak
                FileName = document.FileName,
                // always prompt the user for downloading, set to true if you want 
                // the browser to try to show the file inline
                Inline = false,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(document.ProImg, "txt");
        }
        public ActionResult DeleteFile(int fileId)
        {

            var a = Company.DeleteFile(fileId);
            return RedirectToAction("Details", new { id = a });
        }
        public ActionResult GetAllFiles(int partid)
        {
            var model = Partner.GetAllFiles(partid);
            return View(model);
        }







    }
}