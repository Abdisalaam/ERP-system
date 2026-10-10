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
    public partial class MeasuringUnit
    {/// <summary>
    /// make measure unite unique code
    /// </summary>
    /// <param name="Serial"></param>
    /// <returns></returns>
        public static bool containCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.MeasuringUnits.ToList();
            if (!Serial.StartsWith("UNI-"))
            {
                return true;
            }
            else
            {
                foreach (var item in allinData)
                {
                    if (item.MUnit_Code == Serial || !Serial.StartsWith("UNI-"))
                    {
                        return true;
                    }

                    else
                    {

                    }

                }
            }

            return false;
        }
        /// <summary>
        /// get all meature unit in system
        /// </summary>
        /// <returns></returns>
        public static List<MeasuringUnit> GetMeasruingUnit()
        {
            var db = new AccountingContainer();
            var measruingunit = db.MeasuringUnits.ToList();

            return measruingunit;
        }

        /// <summary>
        /// get detail of meature unit by using its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static MeasuringUnit GetMeasruingUnitById(int id)
        {
            var db = new AccountingContainer();
            var measruingunit = db.MeasuringUnits.Find(id);
            return measruingunit;
        }

        /// <summary>
        /// get measure unite by its code
        /// </summary>
        /// <param name="measrungunitCode"></param>
        /// <returns></returns>
        public static MeasuringUnit GetmeasrungunitByCode(string measrungunitCode)
        {
            var db = new AccountingContainer();
            var measrungunit = db.MeasuringUnits.Where(c => c.MUnit_Code == measrungunitCode);
            var localmeasrungunit = new MeasuringUnit();
            if (measrungunit != null)
            {
                localmeasrungunit = measrungunit.FirstOrDefault();
                return localmeasrungunit;
            }
            return null;
        }
        /// <summary>
        /// fill for cratation meature unit and make unique code for it
        /// </summary>
        /// <returns></returns>
        public static MeasureVM CreateVMForCreation()
        {
            var db = new AccountingContainer();
            var measruingunitvm = new MeasureVM();

            var lastSerial = db.MeasuringUnits.OrderByDescending(c => c.MUnit_ID).FirstOrDefault();
            if (lastSerial == null)
            {
                measruingunitvm.MUnit_Code = "UNI-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                measruingunitvm.MUnit_Code = "UNI-" + (Convert.ToInt32(lastSerial.MUnit_Code.Substring(4, lastSerial.MUnit_Code.Length - 4)) + 1).ToString("D3");
            }
            return measruingunitvm;
        }
        /// <summary>
        /// create meature unit and save it in data base 
        /// </summary>
        /// <param name="measuVM"></param>
        /// <returns></returns>
        public static bool CreateMeasruingUnit(MeasureVM measuVM)
        {
            var db = new AccountingContainer();
            var Measur = new MeasuringUnit();

            var lastSerial = db.MeasuringUnits.OrderByDescending(c => c.MUnit_ID).FirstOrDefault();
            if (lastSerial == null)
            {
                Measur.MUnit_Code = "UNI-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                Measur.MUnit_Code = "UNI-" + (Convert.ToInt32(lastSerial.MUnit_Code.Substring(4, lastSerial.MUnit_Code.Length - 4)) + 1).ToString("D3");
            }


            Measur.MUnit_Descrip = measuVM.MUnit_Descrip;


            db.MeasuringUnits.Add(Measur);
            db.SaveChanges();
            db.Dispose();
            return true;
        }
        /// <summary>
        /// prepare form for editing in measure unit
        /// </summary>
        /// <param name="id"></param>
        /// <returns>MeasureVM</returns>

        public static MeasureVM FillTheViewModel(int id)
        {
            var db = new AccountingContainer();
            var measruingunit = db.MeasuringUnits.Find(id);
            var measurVM = new MeasureVM();

            measurVM.MUnit_ID = measruingunit.MUnit_ID;
            measurVM.MUnit_Code = measruingunit.MUnit_Code;
            measurVM.MUnit_Descrip = measruingunit.MUnit_Descrip;

            return measurVM;

        }
        /// <summary>
        /// save change in Edit and keep it in data base
        /// </summary>
        /// <param name="measurVM"></param>
        /// <returns></returns>
        public static MeasuringUnit Editemeasruingunitvm(MeasureVM measurVM)
        {
            var db = new AccountingContainer();
            var Measur = db.MeasuringUnits.Find(measurVM.MUnit_ID);
            if (Measur != null)
            {
                //Measur.MUnit_Code = measurVM.MUnit_Code;
                Measur.MUnit_Descrip = measurVM.MUnit_Descrip;

                db.Entry(Measur).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                return Measur;
            }
            return null;
        }
        /// <summary>
        /// delete masure unit by using its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int DeleteMeasruingUnit(int id)
        {

            var db = new AccountingContainer();
            MeasuringUnit Measur = db.MeasuringUnits.Find(id);
            if (Measur != null)
            {

                db.MeasuringUnits.Remove(Measur);
                db.SaveChanges();
                db.Dispose();
                return 1;
            }

            return 0;

        }
        /// <summary>
        /// Create multi measure unit by excel sheet
        /// </summary>
        /// <param name="excelfile"></param>
        /// <returns></returns>

        public static String Import(HttpPostedFileBase excelfile)
        {
            var data = getdataFillter(excelfile);
            List<MeasuringUnit> listMeasure = new List<MeasuringUnit>();
            if (data.Item2.Count() == 0)
            {
                foreach (var item in data.Item1)
                {
                    var tre = CreateMeasruingUnitFromFile(item);
                    listMeasure.Add(tre);

                }


                return "تم حفظ الملف بنجاح";

            }
            string combindedString = string.Join(" ,", data.Item2.ToArray());
            return "يوجد اخطاء فى الاسطر التالية" + combindedString;
        }
        /// <summary>
        /// make filteration of Excel sheet and discove error on it
        /// </summary>
        /// <param name="excelfile"></param>
        /// <returns></returns>
        public static Tuple<List<MeasureVM>, List<String>> getdataFillter(HttpPostedFileBase excelfile)
        {
            var MeasureList = new List<MeasureVM> { };
            var messageList = new List<String> { };
            var db = new AccountingContainer();
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                return Tuple.Create(MeasureList, messageList);

            }

            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + excelfile.FileName);

                    if (System.IO.File.Exists(path))
                    {
                        excelfile.SaveAs(path + "1");

                        System.IO.File.Delete(path);


                    }
                    else
                    {
                        excelfile.SaveAs(path);
                    }
                    // read data from excel file

                    Application application = new Application();
                    Workbook workbook = application.Workbooks.Open(path);
                    Worksheet worksheet = workbook.ActiveSheet;
                    Range range = worksheet.UsedRange;
                    for (int row = 2; row <= range.Rows.Count; row++)
                    {
                        MeasureVM measureVM = new MeasureVM();
                        // cust.Customer_Id = int.Parse(((Range)range.Cells[row, 1]).Text);
                        measureVM.MUnit_Descrip = ((Range)range.Cells[row, 1]).Text;
                        if (measureVM.MUnit_Descrip == "")
                        {
                            messageList.Add(row.ToString());
                            continue;
                        }
                        MeasureList.Add(measureVM);

                    }
                    //workbook.Save();
                    //workbook.Close(path);
                    //File.Delete(path);

                }
            }
            return Tuple.Create(MeasureList, messageList);
        }
        /// <summary>
        /// method that used in creation meature unit by excel sheet
        /// </summary>
        /// <param name="measuVM"></param>
        /// <returns></returns>
        public static MeasuringUnit CreateMeasruingUnitFromFile(MeasureVM measuVM)
        {
            var db = new AccountingContainer();
            var Measur = new MeasuringUnit();

            var lastSerial = db.MeasuringUnits.OrderByDescending(c => c.MUnit_ID).FirstOrDefault();
            if (lastSerial == null)
            {
                Measur.MUnit_Code = "UNI-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                Measur.MUnit_Code = "UNI-" + (Convert.ToInt32(lastSerial.MUnit_Code.Substring(4, lastSerial.MUnit_Code.Length - 4)) + 1).ToString("D3");
            }


            Measur.MUnit_Descrip = measuVM.MUnit_Descrip;


            db.MeasuringUnits.Add(Measur);
            db.SaveChanges();
            db.Dispose();
            return Measur;
        }
    }
}