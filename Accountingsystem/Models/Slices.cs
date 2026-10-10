using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Office.Interop.Excel;
using System.Web.Mvc;

using System.IO;
using System.Runtime.InteropServices;
using Rotativa;
using System.Text;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Accountingsystem.Models
{
    public partial class Slices
    {/// <summary>
     /// check the code of slice s unique
     /// </summary>
     /// <param name="Slice_Code"></param>
     /// <returns>bool</returns>
        public static bool containCode(string Slice_Code)
        {
            var db = new AccountingContainer();
            var allinData = db.Slices.ToList();
            foreach (var item in allinData)
            {
                if (item.Slice_Code == Slice_Code || !Slice_Code.StartsWith("SLI-"))
                {
                    return true;
                }
                else
                {

                }

            }
            return false;
        }
        /// <summary>
        /// get all slice in db
        /// </summary>
        /// <returns>List<Slices> </returns>
        public static List<Slices> GetAllSlices()
        {
            var db = new AccountingContainer();
            var slices = db.Slices.ToList();
            return slices;
        }
        /// <summary>
        /// get slice by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Slices GetSliceById(int id)
        {
            var db = new AccountingContainer();
            var slices = db.Slices.Find(id);
            return slices;
        }
        /// <summary>
        /// fill for create new slice
        /// </summary>
        /// <returns>SlicesVM</returns>
        public static SlicesVM AddSlicesForCreation()
        {
            var db = new AccountingContainer();
            var sliceVM = new SlicesVM();

            var lastSerial = db.Slices.OrderByDescending(c => c.Slice_Id).FirstOrDefault();
            if (lastSerial == null)
            {
                sliceVM.Slice_Code = "SLI-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                sliceVM.Slice_Code = "SLI-" + (Convert.ToInt32(lastSerial.Slice_Code.Substring(4, lastSerial.Slice_Code.Length - 4)) + 1).ToString("D3");
            }

            return sliceVM;
        }

        /// <summary>
        /// create new slice and save it in db
        /// </summary>
        /// <param name="slicesVM"></param>
        /// <returns>bool</returns>
        public static int CreateSlice(SlicesVM slicesVM)
        {
            var db = new AccountingContainer();
            var _Slice = new Slices();

            var lastSerial = db.Slices.OrderByDescending(c => c.Slice_Id).FirstOrDefault();
            if (lastSerial == null)
            {
                _Slice.Slice_Code = "SLI-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                _Slice.Slice_Code = "SLI-" + (Convert.ToInt32(lastSerial.Slice_Code.Substring(4, lastSerial.Slice_Code.Length - 4)) + 1).ToString("D3");
            }

            _Slice.Slice_Name = slicesVM.Slice_Name;
           

            db.Slices.Add(_Slice);
            db.SaveChanges();
            db.Dispose();

            return _Slice.Slice_Id;
        }
        /// <summary>
        /// fill For Editing slice 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>SlicesVM</returns>
        public static SlicesVM FillForEdit(int id)
        {
            var db = new AccountingContainer();
            var _Slice = db.Slices.Find(id);

            var sliceVM = new SlicesVM();

            sliceVM.Slice_Id = _Slice.Slice_Id;
            sliceVM.Slice_Name = _Slice.Slice_Name;
            sliceVM.Slice_Code = _Slice.Slice_Code;
      
            return sliceVM;
        }
        /// <summary>
        /// Edite slice and save change in db
        /// </summary>
        /// <param name="sliceVM"></param>
        /// <returns></returns>

        public static bool EditForSlice(SlicesVM sliceVM)
        {
            var db = new AccountingContainer();
            var _Slice = db.Slices.Find(sliceVM.Slice_Id);
            if (_Slice == null)
            {
                return false;
            }

            _Slice.Slice_Name = sliceVM.Slice_Name;
         
          
         

            db.Entry(_Slice).State = EntityState.Modified;
            db.SaveChanges();
            db.Dispose();

            return true;
        }
        /// <summary>
        /// delete slice by its id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Slices DeleteSlice(int id)
        {
            var db = new AccountingContainer();
            var _Slice = db.Slices.Find(id);
            if (_Slice == null)
            {
                return _Slice;
            }

            db.Slices.Remove(_Slice);
            db.SaveChanges();
            db.Dispose();

            return _Slice;
        }
        /// <summary>
        /// import slice from file excel and check avalidation of Excel
        /// </summary>
        /// <param name="excelfile"></param>
        /// <returns>String</returns>
        public static String Import(HttpPostedFileBase excelfile)
        {
            var data = getdataFillter(excelfile);
            List<Slices> listSlice = new List<Slices>();
            if (data.Item2.Count() == 0)
            {
                foreach (var item in data.Item1)
                {
                    var Slices = CreateSliceFromFile(item);
                    listSlice.Add(Slices);

                }


                return "تم حفظ الملف بنجاح";

            }
            string combindedString = string.Join(" ,", data.Item2.ToArray());
            return "يوجد اخطاء فى الاسطر التالية" + combindedString;

        }
        /// <summary>
        /// filter the excel from wrong date and return the number of line that in Error
        /// </summary>
        /// <param name="excelfile"></param>
        /// <returns></returns>
        public static Tuple<List<SlicesVM>, List<String>> getdataFillter(HttpPostedFileBase excelfile)
        {
            var SliceList = new List<SlicesVM> { };
            var messageList = new List<String> { };
            var db = new AccountingContainer();
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                return Tuple.Create(SliceList, messageList);

            }

            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    var nameOfFile = "Slice"+DateTime.Now.ToBinary() + ".xlsx";
                    string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + nameOfFile);


                    excelfile.SaveAs(path);
                    // read data from excel file

                    Application application = new Application();
                    Workbook workbook = application.Workbooks.Open(path);
                    Worksheet worksheet = workbook.ActiveSheet;
                    Range range = worksheet.UsedRange;
                    for (int row = 2; row <= range.Rows.Count; row++)
                    {
                        SlicesVM sliceVM = new SlicesVM();
                        // cust.Customer_Id = int.Parse(((Range)range.Cells[row, 1]).Text);
                        sliceVM.Slice_Name = ((Range)range.Cells[row, 1]).Text;
                        if (sliceVM.Slice_Name=="")
                        {
                            messageList.Add(row.ToString());
                            continue;
                        }
                        SliceList.Add(sliceVM);
                        

                    }
                    workbook.Save();
                    workbook.Close(path);
                    //File.Delete(path);

                }
            }
            return Tuple.Create(SliceList, messageList);
        }
        /// <summary>
        /// Create new slice by file 
        /// </summary>
        /// <param name="slicesVM"></param>
        /// <returns>Slices</returns>
        public static Slices CreateSliceFromFile(SlicesVM slicesVM)
        {
            var db = new AccountingContainer();
            var _Slice = new Slices();

            var lastSerial = db.Slices.OrderByDescending(c => c.Slice_Id).FirstOrDefault();
            if (lastSerial == null)
            {
                _Slice.Slice_Code = "SLI-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                _Slice.Slice_Code = "SLI-" + (Convert.ToInt32(lastSerial.Slice_Code.Substring(4, lastSerial.Slice_Code.Length - 4)) + 1).ToString("D3");
            }

            _Slice.Slice_Name = slicesVM.Slice_Name;


            db.Slices.Add(_Slice);
            db.SaveChanges();
            db.Dispose();

            return _Slice;
        }
        
    }
}