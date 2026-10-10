using Accountingsystem.Models;
using Accountingsystem.ViewModel.ReportsNewVM;
using Accountingsystem.ViewModel.ReportsVM;
using Accountingsystem.ViewModel.ReportsVM.SubscriptionReportVM;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Accountingsystem.Utilites
{
    public class ExcelSheetTemplate
    {
        int rowindex = 1;
        ExcelRange cell;
        ExcelFill fill;
        Border border;
        public byte[] GenerateExcel(int storeId)
        {
            using (var excelpackage = new ExcelPackage())
            {
                var db = new AccountingContainer();
                var productList = db.Products.Where(a => a.Prod_Type == Prod_Type.منتج).ToList();
                excelpackage.Workbook.Properties.Author = "Thump.IKR";
                excelpackage.Workbook.Properties.Title = "Thump.IKR";
                var sheet = excelpackage.Workbook.Worksheets.Add("Product Excel");
                sheet.Name = "قيم افتتاحية لمخزن";
                sheet.Column(2).Width = 10;
                sheet.Column(3).Width = 30;
                sheet.Column(4).Width = 90;
                sheet.Column(5).Width = 20;

                //sheet.Cells[rowindex, 2].Merge = true;
                cell = sheet.Cells[rowindex, 2];
                cell.Value = db.Stores.Find(storeId).Store_Name;
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 30;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowindex = rowindex + 1;

                //sheet.Cells[rowindex, 3,rowindex,9].Merge = true;
                //cell = sheet.Cells[rowindex, 2];
                //cell.Value = db.Stores.Find(storeId).Store_Code;
                //cell.Style.Font.Bold = true;
                //cell.Style.Font.Size = 30;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //rowindex = rowindex + 2;


                cell = sheet.Cells[rowindex, 2];
                cell.Value = db.Stores.Find(storeId).Store_Code;
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 30;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowindex = rowindex + 2;



                cell = sheet.Cells[rowindex, 2];
                cell.Value = "الرقم";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                cell = sheet.Cells[rowindex, 3];
                cell.Value = "رقم المنتج";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                cell = sheet.Cells[rowindex, 4];
                cell.Value = "اسم المنتج";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 5];
                cell.Value = "الكمية";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowindex = rowindex + 1;
                int serialNumber = 1;

                if (productList.Count > 0)
                {
                    foreach (var Product in productList)
                    {
                        cell = sheet.Cells[rowindex, 2];
                        cell.Value = serialNumber++.ToString();
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowindex, 3];
                        cell.Value = Product.Prod_Code;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowindex, 4];
                        cell.Value = Product.Prod_Name;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;

                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 5];
                        cell.Value = null;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowindex = rowindex + 1;

                    }



                }
                return excelpackage.GetAsByteArray();
            }


        }
        public byte[] GenerateExcelForEmployee()
        {
            using (var excelpackage = new ExcelPackage())
            {
                var db = new AccountingContainer();
                var employees = db.Employees.ToList();
                excelpackage.Workbook.Properties.Author = "Thump.IKR";
                excelpackage.Workbook.Properties.Title = "Thump.IKR";
                var sheet = excelpackage.Workbook.Worksheets.Add("Employee Excel");
                sheet.Name = "بيانات موظفين";
                sheet.Column(2).Width = 10;
                sheet.Column(3).Width = 20;
                sheet.Column(4).Width = 90;
                sheet.Column(5).Width = 50;
                sheet.Column(6).Width = 50;
                sheet.Column(7).Width = 90;
                sheet.Column(8).Width = 20;
                sheet.Column(9).Width = 20;
                sheet.Column(10).Width = 20;
                sheet.Column(11).Width = 50;
                sheet.Column(12).Width = 20;
                sheet.Column(13).Width = 30;
                sheet.Column(14).Width = 20;
                sheet.Column(15).Width = 20;
                sheet.Column(15).Width = 20;
                sheet.Column(16).Width = 20;
                sheet.Column(17).Width = 20;
                sheet.Column(18).Width = 20;
                sheet.Column(19).Width = 20;
                sheet.Column(20).Width = 20;
                sheet.Column(21).Width = 20;
                sheet.Column(22).Width = 20;
                sheet.Column(23).Width = 20;
                sheet.Column(24).Width = 20;
                sheet.Column(25).Width = 20;





                cell = sheet.Cells[rowindex, 2];
                cell.Value = "الرقم";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                cell = sheet.Cells[rowindex, 3];
                cell.Value = "الكود";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                cell = sheet.Cells[rowindex, 4];
                cell.Value = "الاسم ";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 5];
                cell.Value = "العنوان";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 6];
                cell.Value = "المحمول ";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                cell = sheet.Cells[rowindex, 7];
                cell.Value = "البريد الالكتروني";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                cell = sheet.Cells[rowindex, 8];
                cell.Value = "الوظيفة  ";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 9];
                cell.Value = "تاريخ الميلاد";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 10];
                cell.Value = "الحالة الاجتماعية";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                cell = sheet.Cells[rowindex, 11];
                cell.Value = "مركز تكلفة الموظف";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                cell = sheet.Cells[rowindex, 12];
                cell.Value = "كود مركز التكلفة ";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 13];
                cell.Value = "رقم الهوية";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 14];
                cell.Value = "تاريخ سريان الجواز";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                cell = sheet.Cells[rowindex, 15];
                cell.Value = "تاريخ انتهاء الجواز";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                cell = sheet.Cells[rowindex, 16];
                cell.Value = "تاريخ سريان الاقامة ";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 17];
                cell.Value = "تاريخ انتهاء الاقامة";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;








                cell = sheet.Cells[rowindex, 18];
                cell.Value = "بداية العقد";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                cell = sheet.Cells[rowindex, 19];
                cell.Value = "تاريخ انتهاء العقد ";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 20];
                cell.Value = "تاريخ بدء العمل الفعلي";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowindex, 21];


                cell.Value = "نوع الدوام";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowindex, 22];
                cell.Value = "رقم تأمين اجتماعي";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowindex, 23];
                cell.Value = "رقم التأمين الصحي";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowindex, 24];
                cell.Value = "انتهاء التأمين الصحي ";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowindex, 25];
                cell.Value = "الجنسية ";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowindex, 26];
                cell.Value = "النوع  ";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                rowindex = rowindex + 1;
                int serialNumber = 1;

                if (employees.Count > 0)
                {
                    foreach (var emp in employees)
                    {
                        cell = sheet.Cells[rowindex, 2];
                        cell.Value = serialNumber++.ToString();
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowindex, 3];
                        cell.Value = emp.Emp_Code;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowindex, 4];
                        cell.Value = emp.Emp_Name;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;

                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 5];
                        cell.Value = emp.Emp_Address;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 6];
                        cell.Value = emp.Emp_Phone;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowindex, 7];
                        cell.Value = emp.Emp_Mail;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowindex, 8];
                        cell.Value = emp.Emp_Title;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 9];
                        cell.Value = emp.Emp_DOB;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 10];
                        cell.Value = emp.Social_status;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 11];
                        var sub = db.SubLedgers.Find(emp.Subldger_Id);
                        if (sub == null)
                        {
                            cell.Value = db.SubLedgers.FirstOrDefault().SubLed_Name;

                        }
                        else
                        {
                            cell.Value = sub.SubLed_Name;
                        }
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowindex, 12];
                        if (sub == null)
                        {
                            cell.Value = db.SubLedgers.FirstOrDefault().SubLed_Code;

                        }
                        else
                        {
                            cell.Value = sub.SubLed_Code;
                        }
                        //cell.Value = db.SubLedgers.Find(emp.Subldger_Id).SubLed_Code;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 13];
                        cell.Value = emp.Passport_number;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 14];
                        cell.Value = emp.Start_Date_Passp;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 15];
                        cell.Value = emp.End_Date_Passp;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 16];
                        cell.Value = emp.Start_Date_Residency;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 17];
                        cell.Value = emp.End_Date_Residency;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;






                        cell = sheet.Cells[rowindex, 18];
                        cell.Value = emp.Contract_Begin;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 19];
                        cell.Value = emp.Contract_End;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 20];
                        cell.Value = emp.Work_Actual_Start;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 21];
                        if (emp.Type_OF_Work != null)
                        {
                            cell.Value = emp.Type_OF_Work.ToString();
                        }
                        else
                        {
                            cell.Value = null;
                        }
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 22];
                        cell.Value = emp.Social_insurance_number;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 23];
                        cell.Value = emp.Medical_Insurance_Number
;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 24];
                        cell.Value = emp.End_medical_Insurance
;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 25];
                        cell.Value = emp.Nationality;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowindex, 26];
                        if (emp.Sex != null)
                        {
                            cell.Value = emp.Sex.ToString();
                        }
                        else
                        {
                            cell.Value = null;
                        }
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;








                        rowindex = rowindex + 1;

                    }



                }
                return excelpackage.GetAsByteArray();
            }


        }

        public byte[] TrialBalance(TrialBalanceNew VM)
        {
            using (var excelpackage = new ExcelPackage())
            {
                var db = new AccountingContainer();
                //var employees = db.Employees.ToList();
                excelpackage.Workbook.Properties.Author = "Thump.IKR";
                excelpackage.Workbook.Properties.Title = "Thump.IKR";
                var sheet = excelpackage.Workbook.Worksheets.Add("Trial balnce Excel");
                sheet.Name = "ميزان المراجعة";
                sheet.Column(1).Width = 10;
                sheet.Column(2).Width = 90;
                sheet.Column(3).Width = 20;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 20;
                sheet.Column(6).Width = 20;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 20;
                sheet.Column(9).Width = 20;

                //cell = sheet.Cells[rowindex, 1];
                //cell.Value = "من";
                //cell.Style.Font.Bold = true;
                //cell.Style.Font.Size = 20;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                //fill = cell.Style.Fill;
                //fill.PatternType = ExcelFillStyle.Solid;
                //fill.BackgroundColor.SetColor(Color.LightGray);
                //border = cell.Style.Border;
                //border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;





                //cell = sheet.Cells[rowindex, 2];
                //cell.Value =VM.from;
                //cell.Style.Font.Bold = true;
                //cell.Style.Font.Size = 20;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                //fill = cell.Style.Fill;
                //fill.PatternType = ExcelFillStyle.Solid;
                //fill.BackgroundColor.SetColor(Color.LightGray);
                //border = cell.Style.Border;
                //border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                //cell = sheet.Cells[rowindex, 3];
                //cell.Value = "الي";
                //cell.Style.Font.Bold = true;
                //cell.Style.Font.Size = 20;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                //fill = cell.Style.Fill;
                //fill.PatternType = ExcelFillStyle.Solid;
                //fill.BackgroundColor.SetColor(Color.LightGray);
                //border = cell.Style.Border;
                //border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                //cell = sheet.Cells[rowindex, 4];
                //cell.Value = VM.to;
                //cell.Style.Font.Bold = true;
                //cell.Style.Font.Size = 20;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                //fill = cell.Style.Fill;
                //fill.PatternType = ExcelFillStyle.Solid;
                //fill.BackgroundColor.SetColor(Color.LightGray);
                //border = cell.Style.Border;
                //border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;






                rowindex = rowindex + 1;



















                cell = sheet.Cells[rowindex, 1];
                cell.Value = "الرقم";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;





                cell = sheet.Cells[rowindex, 2];
                cell.Value = "اسم الحساب";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                cell = sheet.Cells[rowindex, 3];
                cell.Value = "رقم الحساب";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                cell = sheet.Cells[rowindex, 4];
                cell.Value = "رصيد قبل المدة";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                cell = sheet.Cells[rowindex, 5];
                cell.Value = "الحركات ";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 6];
                cell.Value = "";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 7];
                cell.Value = "";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;






                sheet.Cells["E2:G2"].Merge = true;

                cell = sheet.Cells[rowindex, 8];
                cell.Value = "رصيد اخر المدة ";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowindex = rowindex + 1;

                cell = sheet.Cells[rowindex, 1];
                cell.Value = "";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowindex, 2];
                cell.Value = "";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                cell = sheet.Cells[rowindex, 3];
                cell.Value = "";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 4];
                cell.Value = "";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                cell = sheet.Cells[rowindex, 5];
                cell.Value = "مدين ";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowindex, 6];
                cell.Value = "دائن ";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowindex, 7];
                cell.Value = "الصافي ";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowindex, 8];
                cell.Value = "";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;








                rowindex = rowindex + 1;
                int serialNumber = 1;

                if (VM.Detail.Count > 0)
                {
                    foreach (var detail in VM.Detail)
                    {
                        cell = sheet.Cells[rowindex, 1];
                        cell.Value = serialNumber++.ToString();
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowindex, 2];
                        if (detail.ChildAccountd != null)
                        {
                            cell.Value = detail.ChildAccountd.Account_Name;
                        }
                        else if (detail.Asset != null)
                        {
                            cell.Value = detail.Asset.FAsset_Description;
                        }
                        else
                        {
                            cell.Value = detail.Account.Account_Name + detail.LevelExpresion;
                        }
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowindex, 3];
                        if (detail.ChildAccountd != null)
                        {
                            cell.Value = detail.ChildAccountd.Account_Code;
                        }
                        else if (detail.Asset != null)
                        {
                            cell.Value = detail.Asset.FAsset_Code;
                        }
                        else
                        {
                            cell.Value = detail.Account.Account_Code;
                        }

                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;





                        cell = sheet.Cells[rowindex, 4];
                        cell.Value = Math.Round(detail.AmountBefor, 2, MidpointRounding.AwayFromZero);
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;

                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 5];
                        cell.Value = Math.Round(detail.DepitMov, 2, MidpointRounding.AwayFromZero);
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 6];
                        cell.Value = Math.Round(detail.CeditMov, 2, MidpointRounding.AwayFromZero);
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowindex, 7];
                        cell.Value = Math.Round(detail.AmountOfPeriod, 2, MidpointRounding.AwayFromZero);
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowindex, 8];
                        cell.Value = Math.Round(detail.FinalAmount, 2, MidpointRounding.AwayFromZero);
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        rowindex = rowindex + 1;

                    }





                    cell = sheet.Cells[rowindex, 1];
                    cell.Value = "الاجمالي";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                    cell = sheet.Cells[rowindex, 2];
                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowindex, 3];
                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowindex, 4];
                    cell.Value = Math.Round(VM.AmountBefor, 2, MidpointRounding.AwayFromZero);
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;

                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowindex, 5];

                    cell.Value = Math.Round(VM.DepitMov, 2, MidpointRounding.AwayFromZero);
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowindex, 6];

                    cell.Value = Math.Round(VM.CeditMov, 2, MidpointRounding.AwayFromZero);
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowindex, 7];
                    cell.Value = Math.Round(VM.AmountOfPeriod, 2, MidpointRounding.AwayFromZero);

                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowindex, 8];
                    cell.Value = Math.Round(VM.FinalAmount, 2, MidpointRounding.AwayFromZero);

                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;









                    rowindex = rowindex + 1;



                }
                return excelpackage.GetAsByteArray();
            }


        }

        public byte[] GetListLedger(MRGL VM)
        {
            using (var excelpackage = new ExcelPackage())
            {
                var db = new AccountingContainer();
                decimal Balance = VM.AmountBefor;
                //var employees = db.Employees.ToList();
                excelpackage.Workbook.Properties.Author = "Thump.IKR";
                excelpackage.Workbook.Properties.Title = "Thump.IKR";
                var sheet = excelpackage.Workbook.Worksheets.Add("LedgerList Excel");
                sheet.Name = "حساب الاستاذ";
                sheet.Column(1).Width = 10;
                sheet.Column(2).Width = 30;
                sheet.Column(3).Width = 30;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 50;
                sheet.Column(6).Width = 50;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 20;
                sheet.Column(9).Width = 20;






                rowindex = rowindex + 1;



















                cell = sheet.Cells[rowindex, 1];
                cell.Value = "الرقم";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;





                cell = sheet.Cells[rowindex, 2];
                cell.Value = "التاريخ";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 3];
                cell.Value = "الحساب";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                cell = sheet.Cells[rowindex, 4];
                cell.Value = "الرقم";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                cell = sheet.Cells[rowindex, 5];
                cell.Value = "الاسم ";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 6];
                cell.Value = "الوصف";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 7];
                cell.Value = "القيمة";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 8];
                cell.Value = "الرصيد";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;











                rowindex = rowindex + 1;


                cell = sheet.Cells[rowindex, 1];
                cell.Value = 1;
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightYellow);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;





                cell = sheet.Cells[rowindex, 2];
                cell.Value = "رصيد قبل المدة";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightYellow);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                cell = sheet.Cells[rowindex, 3];
                cell.Value = "";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightYellow);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 4];
                cell.Value = "";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightYellow);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                cell = sheet.Cells[rowindex, 5];
                cell.Value = "";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightYellow);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 6];
                cell.Value = "";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightYellow);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 7];
                cell.Value = "";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightYellow);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 8];
                cell.Value = Math.Round(VM.AmountBefor, 2, MidpointRounding.AwayFromZero);
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightYellow);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowindex = rowindex + 1;
                decimal totalOfAmount = 0;
                int serialNumber = 2;

                if (VM.details.Count > 0)
                {
                    foreach (var detail in VM.details)
                    {
                        totalOfAmount += detail.Amount;
                        Balance += detail.Amount;
                        cell = sheet.Cells[rowindex, 1];
                        cell.Value = serialNumber++.ToString();
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowindex, 2];
                        cell.Value = detail.date.ToString("yyyy/MM/dd ");
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[rowindex, 3];
                        cell.Value = detail.ChartAccount.Account_Name;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;

                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowindex, 4];
                        cell.Value = detail.DocumentNumber;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;

                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 5];
                        cell.Value = detail.AccountName;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 6];
                        cell.Value = detail.Description;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowindex, 7];
                        cell.Value = Math.Round(detail.Amount, 2, MidpointRounding.AwayFromZero);
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowindex, 8];
                        cell.Value = Math.Round(Balance, 2, MidpointRounding.AwayFromZero);
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        rowindex = rowindex + 1;

                    }





                    cell = sheet.Cells[rowindex, 1];
                    cell.Value = "الاجمالي";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowindex, 2];
                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowindex, 3];
                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowindex, 4];
                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;

                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowindex, 5];

                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowindex, 6];

                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowindex, 7];
                    cell.Value = Math.Round(totalOfAmount, 2, MidpointRounding.AwayFromZero);

                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowindex, 8];
                    cell.Value = "";

                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;









                    rowindex = rowindex + 1;



                }
                return excelpackage.GetAsByteArray();
            }


        }


        public byte[] GetListSubscribtion(AllSubscriptionVM VM)
        {
            using (var excelpackage = new ExcelPackage())
            {
                var db = new AccountingContainer();

                //var employees = db.Employees.ToList();
                excelpackage.Workbook.Properties.Author = "Thump.IKR";
                excelpackage.Workbook.Properties.Title = "Thump.IKR";
                var sheet = excelpackage.Workbook.Worksheets.Add("LedgerList Excel");
                sheet.Name = "حساب الاستاذ";
                sheet.Column(1).Width = 10;
                sheet.Column(2).Width = 30;
                sheet.Column(3).Width = 30;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 50;
                sheet.Column(6).Width = 50;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 20;
                sheet.Column(9).Width = 20;






                rowindex = rowindex + 1;



















                cell = sheet.Cells[rowindex, 1];
                cell.Value = "اسم العميل/ ClientName";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;





                cell = sheet.Cells[rowindex, 2];
                cell.Value = "الرقم المميز / The Identification Number";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 3];
                cell.Value = "  رقم السجل التجاري / Commercial Registration Number";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                cell = sheet.Cells[rowindex, 4];
                cell.Value = "رقم الفاتورة/Invoice Number";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;








                cell = sheet.Cells[rowindex, 5];
                cell.Value = "تاريخ اصدار الفاتورة/ Invoice Issuance Number";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 6];
                cell.Value = " اجمالى الفاتورة شامل ضريبة القيمة المضافة / Invoice Total VAT Inclusive";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 7];
                cell.Value = "  وصف الخدمة/ services Description";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 8];
                cell.Value = "ملاحظات/ Notes";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                int count = 0;
                decimal Total = 0;
                decimal totalAllTax = 0;


                rowindex += 1;

                if (VM.Sublist.Count > 0)
                {
                    foreach (var detail in VM.Sublist)
                    {

                        count = count + 1;
                        var discount = (decimal)(detail.Estimate_Amount * (detail.Discount.Value / 100));
                        decimal totalAfterdiscount = detail.Estimate_Amount - discount;
                        decimal totaltax = 0;
                        if (detail.Tax_ID > 0)
                        {
                            var thistax = Tax.GetTaxesbyId(detail.Tax_ID);

                            totaltax = (decimal)totalAfterdiscount * (thistax.Tax_Percentage / 100);
                        }
                        var customer = db.Customers.Find(detail.Customer_Id);
                        cell = sheet.Cells[rowindex, 1];
                        cell.Value = customer.Customer_Name; ;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowindex, 2];
                        cell.Value = customer.Customer_TaxNumber;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[rowindex, 3];
                        cell.Value = customer.CommercialRegistrationNo;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;

                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowindex, 4];
                        cell.Value = detail.Subscrip_Code;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;

                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 5];
                        cell.Value = Convert.ToDateTime(detail.Sub_Date).ToString("yyyy/MM/dd ");
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 6];
                        cell.Value = Math.Round(detail.AmountAfterTax, 2, MidpointRounding.AwayFromZero);
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowindex, 7];
                        cell.Value = Subscription_Type.GetSuptypetById(detail.Sub_Type_ID).Subscrip_Name;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowindex, 8];
                        cell.Value = "  ";
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;








                        rowindex = rowindex + 1;
                        Total += detail.AmountAfterTax;
                        totalAllTax += totaltax;

                    }





                    cell = sheet.Cells[rowindex, 1];
                    cell.Value = "الاجمالي / Total";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowindex, 2];
                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowindex, 3];
                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowindex, 4];
                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;

                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowindex, 5];

                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowindex, 6];

                    cell.Value = Math.Round(Total, 2, MidpointRounding.AwayFromZero);
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowindex, 7];
                    cell.Value = "";

                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowindex, 8];
                    cell.Value = "";

                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                    rowindex = rowindex + 1;



                }
                return excelpackage.GetAsByteArray();
            }


        }


        public byte[] GetListSalesClassify(SalesClassffied VM)
        {
            using (var excelpackage = new ExcelPackage())
            {
                var db = new AccountingContainer();

                //var employees = db.Employees.ToList();
                excelpackage.Workbook.Properties.Author = "Thump.IKR";
                excelpackage.Workbook.Properties.Title = "Thump.IKR";
                var sheet = excelpackage.Workbook.Worksheets.Add("SalesList Excel");
                sheet.Name = "تصنيف المبيعات";
                sheet.Column(1).Width = 10;
                sheet.Column(2).Width = 30;
                sheet.Column(3).Width = 30;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 50;
                sheet.Column(6).Width = 50;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 20;
                sheet.Column(9).Width = 20;






                rowindex = rowindex + 1;



















                cell = sheet.Cells[rowindex, 1];
                cell.Value = "رقم العميل/ Customer NO";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 10;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;





                cell = sheet.Cells[rowindex, 2];
                cell.Value = "العميل /Customer";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 10;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 3];
                cell.Value = "رقم الفاتورة / Invoice NO";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 10;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                cell = sheet.Cells[rowindex, 4];
                cell.Value = "التاريخ/Date";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 10;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;








                cell = sheet.Cells[rowindex, 5];
                cell.Value = "الكمية/ QY";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 10;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 6];
                cell.Value = "الفرع / Branch";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 10;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 7];
                cell.Value = " طريقة الدفع / PaymentMethod";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 10;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 8];
                cell.Value = "القيمة/ Amount";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 10;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowindex, 9];
                cell.Value = "الضريبة/ Tax";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 10;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowindex, 10];
                cell.Value = "قيمة الفاتورة/ Invoice Amount";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 10;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                int count = 0;
                decimal Total = 0;
                decimal totalAllTax = 0;


                rowindex += 1;

                if (VM.classvmList.Count > 0)
                {
                    foreach (var detail in VM.classvmList)
                    {

                        count = count + 1;



                        cell = sheet.Cells[rowindex, 1];
                        cell.Value = detail.customerCode;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowindex, 2];
                        cell.Value = detail.customerName+" "+detail.CustomerTaxNumber;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[rowindex, 3];
                        cell.Value = detail.InvoiceNo;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;

                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowindex, 4];
                        cell.Value = Convert.ToDateTime(detail.InvoiceDate).ToString("yyyy/MM/dd ");
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;

                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 5];
                        cell.Value = Math.Round(detail.Quantity, 2, MidpointRounding.AwayFromZero);
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 6];
                        cell.Value = detail.BranchName;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowindex, 7];
                        cell.Value = detail.paymentMethodName;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowindex, 8];
                        cell.Value = Math.Round(detail.totalAmount - detail.totalTax, 2, MidpointRounding.AwayFromZero);
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 9];
                        cell.Value = Math.Round(detail.totalTax, 2, MidpointRounding.AwayFromZero);
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 10];
                        cell.Value = Math.Round(detail.totalAmount, 2, MidpointRounding.AwayFromZero);
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        rowindex += 1;

                        if (detail.totalReverse > 0)
                        {
                            count = count + 1;



                            cell = sheet.Cells[rowindex, 1];
                            cell.Value = detail.customerCode;
                            cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 10;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            fill = cell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border;
                            border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                            cell = sheet.Cells[rowindex, 2];
                            cell.Value = detail.customerName;
                            cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 10;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            fill = cell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border;
                            border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                            cell = sheet.Cells[rowindex, 3];
                            cell.Value = "";
                            cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 10;

                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            fill = cell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border;
                            border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                            cell = sheet.Cells[rowindex, 4];
                            cell.Value = "";
                            cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 10;

                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            fill = cell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border;
                            border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[rowindex, 5];
                            cell.Value = Math.Round(detail.totalReverseQTY, 2, MidpointRounding.AwayFromZero);
                            cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 10;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            fill = cell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border;
                            border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[rowindex, 6];
                            cell.Value = detail.BranchName;
                            cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 10;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            fill = cell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border;
                            border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowindex, 7];
                            cell.Value = detail.paymentMethodName;
                            cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 10;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            fill = cell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border;
                            border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowindex, 8];
                            cell.Value = Math.Round(detail.totalReverseWithoutTax, 2, MidpointRounding.AwayFromZero);
                            cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 10;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            fill = cell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border;
                            border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[rowindex, 9];
                            cell.Value = Math.Round(detail.totalReverseTax, 2, MidpointRounding.AwayFromZero);
                            cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 10;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            fill = cell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border;
                            border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[rowindex, 10];
                            cell.Value = Math.Round(detail.totalReverse, 2, MidpointRounding.AwayFromZero);
                            cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 10;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            fill = cell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border;
                            border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                            rowindex += 1;
                        }


                    }





                    cell = sheet.Cells[rowindex, 1];
                    cell.Value = "الاجمالي / Total";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowindex, 2];
                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowindex, 3];
                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowindex, 4];
                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;

                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowindex, 5];

                    cell.Value = Math.Round(VM.classvmList.Select(a => a.Quantity - a.totalReverseQTY).Sum(), 2, MidpointRounding.AwayFromZero);
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowindex, 6];

                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowindex, 7];
                    cell.Value = "";

                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowindex, 8];
                    cell.Value = Math.Round(VM.classvmList.Select(a => a.totalAmount - (a.totalTax + a.totalReverseWithoutTax)).Sum(), 2, MidpointRounding.AwayFromZero);

                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowindex, 9];
                    cell.Value = cell.Value = Math.Round(VM.classvmList.Select(a => a.totalTax - a.totalReverseTax).Sum(), 2, MidpointRounding.AwayFromZero);

                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowindex, 10];
                    cell.Value = Math.Round(VM.classvmList.Select(a => a.totalAmount - a.totalReverse).Sum(), 2, MidpointRounding.AwayFromZero);

                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    rowindex = rowindex + 1;



                }
                return excelpackage.GetAsByteArray();
            }


        }
        public byte[] GetListTotalSubledger(SubLedgerReportVM VM)
        {
            using (var excelpackage = new ExcelPackage())
            {
                var db = new AccountingContainer();

                //var employees = db.Employees.ToList();
                excelpackage.Workbook.Properties.Author = "Thump.IKR";
                excelpackage.Workbook.Properties.Title = "Thump.IKR";
                var sheet = excelpackage.Workbook.Worksheets.Add("LedgerList Excel");
                sheet.Name = "مركز التكلفة الاجمالي";
                sheet.Column(1).Width = 10;
                sheet.Column(2).Width = 30;
                sheet.Column(3).Width = 30;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 50;
                sheet.Column(6).Width = 50;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 20;
                sheet.Column(9).Width = 20;






                rowindex = rowindex + 1;
















                cell = sheet.Cells[rowindex, 1];
                cell.Value = "الرقم";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                cell = sheet.Cells[rowindex, 2];
                cell.Value = "مركز التكلفة";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;





                cell = sheet.Cells[rowindex, 3];
                cell.Value = "المدين";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 4];
                cell.Value = "الدائن";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                cell = sheet.Cells[rowindex, 5];
                cell.Value = "الرصيد";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                int serialNumber = 1;

                if (VM.Account_Sudledger.Count > 0)
                {
                    foreach (var detail in VM.Account_Sudledger)
                    {
                        rowindex++;
                        cell = sheet.Cells[rowindex, 1];
                        cell.Value = serialNumber++.ToString();
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowindex, 2];
                        cell.Value = detail.Subledger_Name;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[rowindex, 3];
                        cell.Value = Math.Round(detail.GLDetail_Debit, 2, MidpointRounding.AwayFromZero);
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;

                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowindex, 4];
                        cell.Value = Math.Round(detail.GLDetail_Credit, 2, MidpointRounding.AwayFromZero);
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;

                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 5];
                        cell.Value = Math.Round(detail.GLDetail_Debit - detail.GLDetail_Credit, 2, MidpointRounding.AwayFromZero);
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                    }





                    cell = sheet.Cells[rowindex, 1];
                    cell.Value = "الاجمالي";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowindex, 2];
                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowindex, 3];
                    cell.Value = Math.Round(VM.TotalDepit, 2, MidpointRounding.AwayFromZero);
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowindex, 4];
                    cell.Value = Math.Round(VM.TotalCredit, 2, MidpointRounding.AwayFromZero);
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;

                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowindex, 5];

                    cell.Value = Math.Round(VM.Difference, 2, MidpointRounding.AwayFromZero);
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;





                    rowindex = rowindex + 1;



                }
                return excelpackage.GetAsByteArray();
            }


        }
        public byte[] GetListDetailSubleger(SubledgerVM VM)
        {
            using (var excelpackage = new ExcelPackage())
            {
                var db = new AccountingContainer();
                decimal Balance = 0;
                //var employees = db.Employees.ToList();
                excelpackage.Workbook.Properties.Author = "Thump.IKR";
                excelpackage.Workbook.Properties.Title = "Thump.IKR";
                var sheet = excelpackage.Workbook.Worksheets.Add("LedgerList Excel");
                sheet.Name = "مركز تكلفة تفصيلي";
                sheet.Column(1).Width = 10;
                sheet.Column(2).Width = 30;
                sheet.Column(3).Width = 30;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 50;
                sheet.Column(6).Width = 50;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 20;
                sheet.Column(9).Width = 20;






                rowindex = rowindex + 1;



















                cell = sheet.Cells[rowindex, 1];
                cell.Value = "الرقم";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;





                cell = sheet.Cells[rowindex, 2];
                cell.Value = "التاريخ";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 3];
                cell.Value = "رقم المستند";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                cell = sheet.Cells[rowindex, 4];
                cell.Value = "الوصف";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                cell = sheet.Cells[rowindex, 5];
                cell.Value = "الحساب ";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 6];
                cell.Value = "المدين";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 7];
                cell.Value = "الدائن";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 8];
                cell.Value = "الرصيد";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;









                rowindex = rowindex + 1;
                decimal totalOfAmount = 0;
                int serialNumber = 1;

                if (VM.DetailList.Count > 0)
                {
                    foreach (var detail in VM.DetailList)
                    {
                        totalOfAmount += detail.Amount;
                        Balance += detail.Amount;
                        cell = sheet.Cells[rowindex, 1];
                        cell.Value = serialNumber++.ToString();
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowindex, 2];
                        cell.Value = detail.GLdate.ToString("yyyy/MM/dd ");
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[rowindex, 3];
                        cell.Value = detail.serialCode;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12;

                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowindex, 4];
                        cell.Value = detail.Description;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;

                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 5];
                        cell.Value = detail.Account_Name;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 6];
                        cell.Value = Math.Round(detail.Depite, 2, MidpointRounding.AwayFromZero);
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowindex, 7];
                        cell.Value = Math.Round(detail.Credit, 2, MidpointRounding.AwayFromZero);
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowindex, 8];
                        cell.Value = Math.Round(Balance, 2, MidpointRounding.AwayFromZero);
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        rowindex = rowindex + 1;

                    }





                    cell = sheet.Cells[rowindex, 1];
                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowindex, 2];
                    cell.Value = "الاجمالي";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowindex, 3];
                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowindex, 4];
                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;

                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowindex, 5];

                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowindex, 6];

                    cell.Value = VM.DetailList.Select(a => a.Depite).Sum();
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowindex, 7];
                    cell.Value = Math.Round(VM.DetailList.Select(a => a.Credit).Sum(), 2, MidpointRounding.AwayFromZero);

                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowindex, 8];
                    cell.Value = Math.Round(totalOfAmount, 2, MidpointRounding.AwayFromZero); ;

                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;









                    rowindex = rowindex + 1;



                }
                return excelpackage.GetAsByteArray();
            }


        }
        public byte[] GetListOfAllProduct(ProductListVM VM)
        {
            using (var excelpackage = new ExcelPackage())
            {
                var db = new AccountingContainer();
                decimal Balance = 0;
                //var employees = db.Employees.ToList();
                excelpackage.Workbook.Properties.Author = "Thump.IKR";
                excelpackage.Workbook.Properties.Title = "Thump.IKR";
                var sheet = excelpackage.Workbook.Worksheets.Add("ProductList Excel");
                sheet.Name = "تقرير الاصناف";
                sheet.Column(1).Width = 10;
                sheet.Column(2).Width = 30;
                sheet.Column(3).Width = 30;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 50;
                sheet.Column(6).Width = 50;







                rowindex = rowindex + 1;



















                cell = sheet.Cells[rowindex, 1];
                cell.Value = "الرقم";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;





                cell = sheet.Cells[rowindex, 2];
                cell.Value = "الصنف";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 3];
                cell.Value = "الكمية";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                cell = sheet.Cells[rowindex, 4];
                cell.Value = "سعر التكلفة";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                cell = sheet.Cells[rowindex, 5];
                cell.Value = "سعر البيع ";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 6];
                cell.Value = "التكلفة التقديرية";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;






                rowindex = rowindex + 1;


                if (VM.Products.Count > 0)
                {
                    if (VM.all > 0)
                    {
                        foreach (var item in VM.Products)
                        {

                            var pro = Models.Product.getTotalQuantityandCost(item.Prod_ID);
                            var probatch = item.ProductBatches.Where(a => a.Store_ID == 1).FirstOrDefault();
                            cell = sheet.Cells[rowindex, 1];
                            cell.Value = item.Prod_Code;
                            cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 12;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            fill = cell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border;
                            border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                            cell = sheet.Cells[rowindex, 2];
                            cell.Value = item.Prod_Name;
                            cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 12;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            fill = cell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border;
                            border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                            cell = sheet.Cells[rowindex, 3];
                            cell.Value = Math.Round(pro.Sale_Price.Value, 2, MidpointRounding.AwayFromZero);
                            cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 12;

                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            fill = cell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border;
                            border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                            cell = sheet.Cells[rowindex, 4];
                            if (pro.Sale_Price == 0)
                            {
                                cell.Value = Math.Round(pro.Prod_Cost_Price.Value, 2, MidpointRounding.AwayFromZero);
                            }
                            else
                            {
                                cell.Value = Math.Round(pro.Prod_Cost_Price.Value / pro.Sale_Price.Value, 2, MidpointRounding.AwayFromZero);
                            }
                            cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 12;

                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            fill = cell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border;
                            border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[rowindex, 5];
                            if (probatch == null || probatch.SalesPrice == null)
                            {
                                cell.Value = Math.Round(item.Sale_Price.Value, 2, MidpointRounding.AwayFromZero);

                            }
                            else
                            {
                                cell.Value = Math.Round(probatch.SalesPrice.Value, 2, MidpointRounding.AwayFromZero);
                            }


                            cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 12;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            fill = cell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border;
                            border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[rowindex, 6];
                            cell.Value = Math.Round(item.Prod_Cost_Price.Value, 2, MidpointRounding.AwayFromZero);
                            cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 12;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            fill = cell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border;
                            border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            rowindex = rowindex + 1;

                        }

                    }


                    else
                    {


                        foreach (var item in VM.Products)
                        {
                            var pro = Models.Product.getTotalQuantityandCost(item.Prod_ID);
                            var probatch = item.ProductBatches.Where(a => a.Store_ID == 1).FirstOrDefault();
                            if (Math.Round(pro.Sale_Price.Value, 2, MidpointRounding.AwayFromZero) > 0)
                            {

                                cell = sheet.Cells[rowindex, 1];
                                cell.Value = item.Prod_Code;
                                cell.Style.Font.Bold = true;
                                cell.Style.Font.Size = 12;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                fill = cell.Style.Fill;
                                fill.PatternType = ExcelFillStyle.Solid;
                                fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border;
                                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                                cell = sheet.Cells[rowindex, 2];
                                cell.Value = item.Prod_Name;
                                cell.Style.Font.Bold = true;
                                cell.Style.Font.Size = 12;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                fill = cell.Style.Fill;
                                fill.PatternType = ExcelFillStyle.Solid;
                                fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border;
                                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                                cell = sheet.Cells[rowindex, 3];
                                cell.Value = Math.Round(pro.Sale_Price.Value, 2, MidpointRounding.AwayFromZero);
                                cell.Style.Font.Bold = true;
                                cell.Style.Font.Size = 12;

                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                fill = cell.Style.Fill;
                                fill.PatternType = ExcelFillStyle.Solid;
                                fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border;
                                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                                cell = sheet.Cells[rowindex, 4];
                                if (pro.Sale_Price == 0)
                                {
                                    cell.Value = Math.Round(pro.Prod_Cost_Price.Value, 2, MidpointRounding.AwayFromZero);
                                }
                                else
                                {
                                    cell.Value = Math.Round(pro.Prod_Cost_Price.Value / pro.Sale_Price.Value, 2, MidpointRounding.AwayFromZero);
                                }
                                cell.Style.Font.Bold = true;
                                cell.Style.Font.Size = 12;

                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                fill = cell.Style.Fill;
                                fill.PatternType = ExcelFillStyle.Solid;
                                fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border;
                                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                cell = sheet.Cells[rowindex, 5];
                                if (probatch == null || probatch.SalesPrice == null)
                                {
                                    cell.Value = Math.Round(item.Sale_Price.Value, 2, MidpointRounding.AwayFromZero);

                                }
                                else
                                {
                                    cell.Value = Math.Round(probatch.SalesPrice.Value, 2, MidpointRounding.AwayFromZero);
                                }


                                cell.Style.Font.Bold = true;
                                cell.Style.Font.Size = 12;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                fill = cell.Style.Fill;
                                fill.PatternType = ExcelFillStyle.Solid;
                                fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border;
                                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                cell = sheet.Cells[rowindex, 6];
                                cell.Value = Math.Round(item.Prod_Cost_Price.Value, 2, MidpointRounding.AwayFromZero);
                                cell.Style.Font.Bold = true;
                                cell.Style.Font.Size = 12;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                fill = cell.Style.Fill;
                                fill.PatternType = ExcelFillStyle.Solid;
                                fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border;
                                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                rowindex = rowindex + 1;

                            }
                        }

                    }
                }




                return excelpackage.GetAsByteArray();


            }



        }
        public byte[] GetListOfLedgerExcel(ChildAccountGL VM)
        {
            using (var excelpackage = new ExcelPackage())
            {
                var db = new AccountingContainer();
                decimal Balance = 0;
                decimal totalAmount = 0;
                //var employees = db.Employees.ToList();
                excelpackage.Workbook.Properties.Author = "Thump.IKR";
                excelpackage.Workbook.Properties.Title = "Thump.IKR";
                var sheet = excelpackage.Workbook.Worksheets.Add("LedgerList Excel");
                sheet.Name = "تقرير حساب";
                sheet.Column(1).Width = 10;
                sheet.Column(2).Width = 30;
                sheet.Column(3).Width = 30;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 50;
                sheet.Column(6).Width = 50;



                //sheet.Cells[rowindex, 2].Merge = true;
                cell = sheet.Cells[rowindex, 2];
                cell.Value = VM.Account.Account_Name;
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 30;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowindex = rowindex + 1;




                cell = sheet.Cells[rowindex, 2];
                cell.Value = VM.Account.Account_Code;
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 30;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowindex = rowindex + 2;



                rowindex = rowindex + 1;



















                cell = sheet.Cells[rowindex, 1];
                cell.Value = "التاريخ";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;





                cell = sheet.Cells[rowindex, 2];
                cell.Value = "الاستحقاق";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 3];
                cell.Value = "الرقم";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                cell = sheet.Cells[rowindex, 4];
                cell.Value = "الوصف";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                cell = sheet.Cells[rowindex, 5];
                cell.Value = "القيمة ";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 6];
                cell.Value = "الرصيد";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;






                rowindex = rowindex + 1;


                cell = sheet.Cells[rowindex, 1];
                cell.Value = "رصيد قبل المدة";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                cell = sheet.Cells[rowindex, 2];
                cell.Value = "";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                cell = sheet.Cells[rowindex, 3];
                cell.Value = "";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12;

                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                cell = sheet.Cells[rowindex, 4];

                cell.Value = "";

                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12;

                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 5];
                cell.Value = "";


                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 6];
                cell.Value = Math.Round(VM.AmountBefor, 2, MidpointRounding.AwayFromZero);
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowindex = rowindex + 1;




                if (VM.details.Count > 0)
                {

                    foreach (var item in VM.details)
                    {

                        Balance += item.Amount;
                        totalAmount += item.Amount;
                        cell = sheet.Cells[rowindex, 1];
                        cell.Value = item.date.ToString("yyyy/MM/dd ");
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowindex, 2];
                        if (item.dueDate != null)
                        {
                            cell.Value = item.dueDate.Value.ToString("yyyy/MM/dd ");
                        }
                        else
                        {
                            cell.Value = "";
                        }

                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[rowindex, 3];
                        cell.Value = item.DocumentNumber;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12;

                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowindex, 4];
                        cell.Value = item.Description;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12;

                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 5];
                        cell.Value = Math.Round(item.Amount, 2, MidpointRounding.AwayFromZero);


                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 6];
                        cell.Value = Math.Round(Balance, 2, MidpointRounding.AwayFromZero);
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        rowindex = rowindex + 1;

                    }


                    cell = sheet.Cells[rowindex, 1];
                    cell.Value = "الاجمالي";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                    cell = sheet.Cells[rowindex, 2];
                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                    cell = sheet.Cells[rowindex, 3];
                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12;

                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                    cell = sheet.Cells[rowindex, 4];

                    cell.Value = "";

                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12;

                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowindex, 5];
                    cell.Value = "";


                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowindex, 6];
                    cell.Value = Math.Round(totalAmount, 2, MidpointRounding.AwayFromZero);
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                }




                return excelpackage.GetAsByteArray();


            }



        }
        public byte[] GetListOfIncomeExcel(IncomeStatementVM VM)
        {
            using (var excelpackage = new ExcelPackage())
            {
                var db = new AccountingContainer();
              
                //var employees = db.Employees.ToList();
                excelpackage.Workbook.Properties.Author = "Thump.IKR";
                excelpackage.Workbook.Properties.Title = "Thump.IKR";
                var sheet = excelpackage.Workbook.Worksheets.Add("LedgerList Excel");
                sheet.Name = "تقرير قائمة الدخل";
                sheet.Column(1).Width = 50;
                sheet.Column(2).Width = 50;




                //sheet.Cells[rowindex, 2].Merge = true;





                cell = sheet.Cells[rowindex, 2];
                cell.Value = "قائمة الدخل";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 30;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowindex = rowindex + 2;







                cell = sheet.Cells[rowindex, 1];
                cell.Value = "الحساب";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;





                cell = sheet.Cells[rowindex, 2];
                cell.Value = "القيمة";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                rowindex = rowindex + 1;


                cell = sheet.Cells[rowindex, 1];
                cell.Value = "الايرادات";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                cell = sheet.Cells[rowindex, 2];
                cell.Value = "";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                rowindex = rowindex + 1;




                if (VM.Account_Revenue.Count > 0)
                {

                    foreach (var item in VM.Account_Revenue)
                    {


                        cell = sheet.Cells[rowindex, 1];
                        cell.Value = item.Account_Name;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowindex, 2];

                        cell.Value = item.difference;


                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        rowindex = rowindex + 1;

                    }


                    cell = sheet.Cells[rowindex, 1];
                    cell.Value = "الاجمالي";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 13;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                    cell = sheet.Cells[rowindex, 2];
                    cell.Value = VM.Total_Revenue;
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 13;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                    rowindex = rowindex + 1;


                    cell = sheet.Cells[rowindex, 1];
                    cell.Value = " تكلفة المبيعات";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                    cell = sheet.Cells[rowindex, 2];
                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                    rowindex = rowindex + 1;




                    if (VM.ExpenseSubLedger.Count > 0)
                    {

                        foreach (var item in VM.ExpenseSubLedger)
                        {


                            cell = sheet.Cells[rowindex, 1];
                            cell.Value = item.Account_Name;
                            cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 12;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            fill = cell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border;
                            border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                            cell = sheet.Cells[rowindex, 2];

                            cell.Value = item.difference;


                            cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 12;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            fill = cell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border;
                            border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                            rowindex = rowindex + 1;

                        }

                    }
                        cell = sheet.Cells[rowindex, 1];
                        cell.Value = "الاجمالي";
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 13;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowindex, 2];
                        cell.Value = VM.salesCost;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 13;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        rowindex = rowindex + 1;


                        cell = sheet.Cells[rowindex, 1];
                        cell.Value = " المصروفات";
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowindex, 2];
                        cell.Value = "";
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        rowindex = rowindex + 1;




                    if (VM.Account_Expense.Count > 0)
                    {

                        foreach (var item in VM.Account_Expense)
                        {


                            cell = sheet.Cells[rowindex, 1];
                            cell.Value = item.Account_Name;
                            cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 12;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            fill = cell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border;
                            border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                            cell = sheet.Cells[rowindex, 2];

                            cell.Value = item.difference;


                            cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 12;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            fill = cell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border;
                            border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                            rowindex = rowindex + 1;

                        }
                    }

                            cell = sheet.Cells[rowindex, 1];
                            cell.Value = "الاجمالي";
                            cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 13;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            fill = cell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border;
                            border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                            cell = sheet.Cells[rowindex, 2];
                            cell.Value = VM.Total_Expense;
                            cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 13;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            fill = cell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border;
                            border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowindex = rowindex + 1;

                    cell = sheet.Cells[rowindex, 1];
                    cell.Value = "الملخص";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 13;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                    cell = sheet.Cells[rowindex, 2];
                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 13;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowindex = rowindex + 1;

                    cell = sheet.Cells[rowindex, 1];
                    cell.Value = "الايرادات";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 13;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                    cell = sheet.Cells[rowindex, 2];
                    cell.Value = VM.Total_Revenue;
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 13;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                    rowindex = rowindex + 1;

                    cell = sheet.Cells[rowindex, 1];
                    cell.Value = " تكلفة المبيعات";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 13;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                    cell = sheet.Cells[rowindex, 2];
                    cell.Value = VM.salesCost;
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 13;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    rowindex = rowindex + 1;

                    cell = sheet.Cells[rowindex, 1];
                    cell.Value = "المصروفات";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 13;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                    cell = sheet.Cells[rowindex, 2];
                    cell.Value = VM.Total_Expense;
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 13;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    rowindex = rowindex + 1;

                    cell = sheet.Cells[rowindex, 1];
                    cell.Value = "صافي الربح والخسارة";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 13;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                    cell = sheet.Cells[rowindex, 2];
                    cell.Value = VM.Difference;
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 13;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                }




                return excelpackage.GetAsByteArray();


                    }



                }
        public byte[] GetListOfGeneralReportExcel(ReportMasterGeneralBudgetVM VM)
        {
            using (var excelpackage = new ExcelPackage())
            {
                var db = new AccountingContainer();

                //var employees = db.Employees.ToList();
                excelpackage.Workbook.Properties.Author = "Thump.IKR";
                excelpackage.Workbook.Properties.Title = "Thump.IKR";
                var sheet = excelpackage.Workbook.Worksheets.Add("GeneralList Excel");
                sheet.Name = "تقريرالمركز المالي ";
                sheet.Column(1).Width = 50;
                sheet.Column(2).Width = 50;




                //sheet.Cells[rowindex, 2].Merge = true;





                cell = sheet.Cells[rowindex, 2];
                cell.Value = "تقريرالمركز المالي";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 30;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowindex = rowindex + 2;







                cell = sheet.Cells[rowindex, 1];
                cell.Value = "الحساب";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;





                cell = sheet.Cells[rowindex, 2];
                cell.Value = "القيمة";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




              




                rowindex = rowindex + 1;




                if (VM.AssetList.Count > 0)
                {

                    foreach (var item in VM.AssetList)
                    {


                        cell = sheet.Cells[rowindex, 1];
                        cell.Value = item.Level_Expression + item.Account_Name;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowindex, 2];

                        cell.Value =Math.Round( item.Account_Balance,2,MidpointRounding.AwayFromZero);


                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        rowindex = rowindex + 1;

                    }




                }
                if (VM.Libality.Count > 0)
                {

                    foreach (var item in VM.Libality)
                    {


                        cell = sheet.Cells[rowindex, 1];
                        cell.Value = item.Level_Expression+item.Account_Name;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowindex, 2];

                        cell.Value = Math.Round(item.Account_Balance, 2, MidpointRounding.AwayFromZero);


                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        rowindex = rowindex + 1;

                    }




                }

                if (VM.PropertyRights.Count > 0)
                {

                    foreach (var item in VM.PropertyRights)
                    {


                        cell = sheet.Cells[rowindex, 1];
                        cell.Value = item.Level_Expression + item.Account_Name;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowindex, 2];

                        cell.Value = Math.Round(item.Account_Balance, 2, MidpointRounding.AwayFromZero);


                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        rowindex = rowindex + 1;

                    }


                    cell = sheet.Cells[rowindex, 1];
                    cell.Value = "صافي الربح والخسارة";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                    cell = sheet.Cells[rowindex, 2];

                    cell.Value = Math.Round(VM.Profit, 2, MidpointRounding.AwayFromZero);


                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                }


                return excelpackage.GetAsByteArray();


            }



        }

        public byte[] GetListPurchase(PurchaseDatesVM VM)
        {
            using (var excelpackage = new ExcelPackage())
            {
                var db = new AccountingContainer();

                //var employees = db.Employees.ToList();
                excelpackage.Workbook.Properties.Author = "Thump.IKR";
                excelpackage.Workbook.Properties.Title = "Thump.IKR";
                var sheet = excelpackage.Workbook.Worksheets.Add("PurchaseList Excel");
                sheet.Name = "تقرير المشتريات";
                sheet.Column(1).Width = 10;
                sheet.Column(2).Width = 30;
                sheet.Column(3).Width = 30;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 50;
                sheet.Column(6).Width = 50;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 20;
                sheet.Column(9).Width = 20;






                rowindex = rowindex + 1;



















                cell = sheet.Cells[rowindex, 1];
                cell.Value = "رقم المورد/ Supplier NO";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 10;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;





                cell = sheet.Cells[rowindex, 2];
                cell.Value = "المورد /Supplier";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 10;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 3];
                cell.Value = "الرقم  / Invoice NO";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 10;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                cell = sheet.Cells[rowindex, 4];
                cell.Value = "التاريخ/Date";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 10;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;








                cell = sheet.Cells[rowindex, 5];
                cell.Value = "الاستحقاق/ Due Date";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 10;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


              

              

                cell = sheet.Cells[rowindex, 6];
                cell.Value = "القيمة/ Amount";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 10;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowindex, 7];
                cell.Value = "الضريبة/ Tax";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 10;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowindex,8];
                cell.Value = " قيمة المردود / Recall Amount";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 10;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 9];
                cell.Value = "قيمة الفاتورة/ Invoice Amount";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 10;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                int count = 0;
                decimal totalamount = 0;
                decimal totarecall = 0;
                decimal totaldef = 0;
                decimal totalAllTax = 0;


                rowindex += 1;

                if (VM.List.Count > 0)
                {
                    foreach (var detail in VM.List)
                    {
                        totalamount = totalamount + detail.invoiceTotal;
                        totarecall = totarecall + detail.RecallInvoicetotal;
                        totaldef = totaldef + detail.TotalOFInvoicesBefor;
                        totalAllTax = totalAllTax + detail.totaltax;
                        count = count + 1;



                        cell = sheet.Cells[rowindex, 1];
                        cell.Value = detail.SupplierCode;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowindex, 2];
                        cell.Value = detail.supplierName+" "+detail.SupplierTaxNumber;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[rowindex, 3];
                        cell.Value = detail.InvoiceCode;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;

                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowindex, 4];
                        cell.Value = Convert.ToDateTime(detail.InvoiceDate).ToString("yyyy/MM/dd ");
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;

                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 5];
                        cell.Value = Convert.ToDateTime(detail.invoiceDeadLine).ToString("yyyy/MM/dd ");
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 6];
                        cell.Value = Math.Round(detail.invoiceTotal, 2, MidpointRounding.AwayFromZero);
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowindex, 7];
                        cell.Value = Math.Round(detail.totaltax, 2, MidpointRounding.AwayFromZero);
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowindex, 8];
                        cell.Value = Math.Round(detail.RecallInvoicetotal, 2, MidpointRounding.AwayFromZero);
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowindex, 9];
                        cell.Value = Math.Round(detail.TotalOFInvoicesBefor, 2, MidpointRounding.AwayFromZero);
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        rowindex += 1;

                    


                    }





                    cell = sheet.Cells[rowindex, 1];
                    cell.Value = "الاجمالي / Total";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowindex, 2];
                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowindex, 3];
                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowindex, 4];
                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;

                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowindex, 5];

                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowindex, 6];

                    cell.Value = Math.Round(totalamount, 2, MidpointRounding.AwayFromZero);
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowindex, 7];
                    cell.Value = Math.Round(totalAllTax, 2, MidpointRounding.AwayFromZero);

                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowindex, 8];
                    cell.Value = Math.Round(totarecall, 2, MidpointRounding.AwayFromZero);

                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowindex, 9];
                    cell.Value = Math.Round(totaldef, 2, MidpointRounding.AwayFromZero);

                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    

                    rowindex = rowindex + 1;



                }
                return excelpackage.GetAsByteArray();
            }


        }
        public byte[] GetTypeSubscribtion(SubscriptionStopVM VM)
        {
            using (var excelpackage = new ExcelPackage())
            {
                var db = new AccountingContainer();

                //var employees = db.Employees.ToList();
                excelpackage.Workbook.Properties.Author = "Thump.IKR";
                excelpackage.Workbook.Properties.Title = "Thump.IKR";
                var sheet = excelpackage.Workbook.Worksheets.Add("StatusSubscribtionList Excel");
                sheet.Name = "تقرير حالات الاشتراكات";
                sheet.Column(1).Width = 10;
                sheet.Column(2).Width = 30;
                sheet.Column(3).Width = 30;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 50;
                sheet.Column(6).Width = 50;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 20;
                sheet.Column(9).Width = 20;






                rowindex = rowindex + 1;



















                cell = sheet.Cells[rowindex, 1];
                cell.Value = "الرقم/ NO";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;





                cell = sheet.Cells[rowindex, 2];
                cell.Value = " رقم الاشتراك /  Sub NO";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 3];
                cell.Value = " تاريخ البدايه / Start Date";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                cell = sheet.Cells[rowindex, 4];
                cell.Value = " تاريخ الانتهاء/ End Date";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;








                cell = sheet.Cells[rowindex, 5];
                cell.Value = "العميل/Customer";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 6];
                cell.Value = " جوال العميل / Phone";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 7];
                cell.Value = " ايميل/ Email";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowindex, 8];
                cell.Value = "  الموقع الالكتروني/ website";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowindex, 9];
                cell.Value = "  الصافي/ Net";
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightGray);
                border = cell.Style.Border;
                border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                int count = 0;
                decimal Total = 0;
                decimal totalAllTax = 0;


                rowindex += 1;

                if (VM.StopSubscription.Count > 0)
                {
                    foreach (var detail in VM.StopSubscription)
                    {
                        var Cust = Accountingsystem.Models.Customer.GetCustomerById(Convert.ToInt32(detail.Customer_Id));
                        Total += detail.AmountAfterTax;

                        count = count + 1;
                        
                        cell = sheet.Cells[rowindex, 1];
                        cell.Value = count;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowindex, 2];
                        cell.Value = detail.Subscrip_Code;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[rowindex, 3];
                        cell.Value = Convert.ToDateTime(detail.StartTime).ToString("yyyy/MM/dd ");
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;

                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                        cell = sheet.Cells[rowindex, 4];
                        cell.Value = Convert.ToDateTime(detail.DeadLine).ToString("yyyy/MM/dd ");
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;

                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 5];
                        cell.Value = Cust.Customer_Name;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 6];
                        cell.Value = detail.tel_customer;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowindex, 7];
                        cell.Value = Cust.Customer_Mail;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowindex, 8];
                        cell.Value = Cust.Customer_Website;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowindex, 9];
                        cell.Value = Math.Round(detail.AmountAfterTax, 2, MidpointRounding.AwayFromZero);
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 10;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;





                        rowindex = rowindex + 1;
                   
                      

                    }





                    cell = sheet.Cells[rowindex, 1];
                    cell.Value = "الاجمالي / Total";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowindex, 2];
                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowindex, 3];
                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowindex, 4];
                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;

                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowindex, 5];

                    cell.Value = "";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowindex, 6];

                    cell.Value ="";
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowindex, 7];
                    cell.Value = "";

                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowindex, 8];
                    cell.Value = "";

                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowindex, 9];

                    cell.Value = Math.Round(Total, 2, MidpointRounding.AwayFromZero);
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 10;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowindex = rowindex + 1;



                }
                return excelpackage.GetAsByteArray();
            }


        }
    }
        }
    

    
