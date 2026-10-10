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
    public partial class Category
    {
        /// <summary>
        /// make code of catigore unique
        /// </summary>
        /// <param name="Serial"></param>
        /// <returns></returns>

        public static bool containCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.Categories.ToList();
            if (!Serial.StartsWith("CAT-"))
            {
                return true;
            }
            else
            {
                foreach (var item in allinData)
                {
                    if (item.Cat_Code == Serial || !Serial.StartsWith("CAT-"))
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
        // use this method to  Get List Category from Data base . 
        public static List<Category> GetAllCategory()
        {
            var db = new AccountingContainer();
            var categor = db.Categories.ToList();

            return categor;
        }

        // use this method to   Get one element CategoryById from Database.  
        public static Category GetCategoryById(int id)
        {
            var db = new AccountingContainer();
            var categor = db.Categories.Find(id);
            return categor;
        }

        // use this method to   Get one element CategoryByCode from Database.  
        public static Category GetCategoryByCode(string CategoryCode)
        {
            var db = new AccountingContainer();
            var categor = db.Categories.Where(c => c.Cat_Code == CategoryCode);
            var localcategor = new Category();
            if (categor != null)
            {
                localcategor = categor.FirstOrDefault();
                return localcategor;
            }
            return null;
        }

        // use this method For Creation new Catagery and get Data from user.
        public static CategroyVM CreateVMForCreation()
        {
            var db = new AccountingContainer();
            var categoryvm = new CategroyVM();


            var lastSerial = db.Categories.OrderByDescending(c => c.Cat_ID).FirstOrDefault();
            if (lastSerial == null)
            {
                categoryvm.Cat_Code = "CAT-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                categoryvm.Cat_Code = "CAT-" + (Convert.ToInt32(lastSerial.Cat_Code.Substring(4, lastSerial.Cat_Code.Length - 4)) + 1).ToString("D3");
            }
            return categoryvm;
        }

        // use this method to  Create New Catagery and save it in DataBase.
        public static bool CreateCatgery(CategroyVM catgorVM)
        {
            var db = new AccountingContainer();

            var Categr = new Category();
            var lastSerial = db.Categories.OrderByDescending(c => c.Cat_ID).FirstOrDefault();
            if (lastSerial == null)
            {
                Categr.Cat_Code = "CAT-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                Categr.Cat_Code = "CAT-" + (Convert.ToInt32(lastSerial.Cat_Code.Substring(4, lastSerial.Cat_Code.Length - 4)) + 1).ToString("D3");
            }

            Categr.Cat_Descrip = catgorVM.Cat_Descrip;
            if (catgorVM.Cat_Img != null)
            {
                //var data = new byte[catgorVM.Cat_Img.];
                //Categr.Cat_Img = data;
                var length = catgorVM.Cat_Img.InputStream.Length; //Length: 103050706
                MemoryStream target = new MemoryStream();
                catgorVM.Cat_Img.InputStream.CopyTo(target); // generates problem in this line
                byte[] data = target.ToArray();
                Categr.Cat_Img = data;

            }
            if (catgorVM.Cat_Img2 != null)
            {
                for (int i = 0; i < catgorVM.Cat_Img2.Count; i++)
                {


                    
                    var length = catgorVM.Cat_Img2[i].InputStream.Length; //Length: 103050706
                    MemoryStream target = new MemoryStream();
                    catgorVM.Cat_Img2[i].InputStream.CopyTo(target); // generates problem in this line
                    byte[] data = target.ToArray();
                    
                    if (data.Length < 1000000)
                    {
                        Categr.Cat_Img = data;
                    }

                }
            }

            db.Categories.Add(Categr);
            db.SaveChanges();
            db.Dispose();
            return true;
        }

        // use this method to load Details of Data for Category.
        public static CategroyVM FillTheViewModel(int id)
        {
            var db = new AccountingContainer();
            var category = db.Categories.Find(id);
            var CatVM = new CategroyVM();

            CatVM.Cat_Code = category.Cat_Code;
            CatVM.Cat_ID = category.Cat_ID;
            CatVM.Cat_Descrip = category.Cat_Descrip;
            return CatVM;

        }

        // use this method to Edite Catgery and save it in DataBase.
        public static Category EditeCatgery(CategroyVM catgorVM)
        {
            var db = new AccountingContainer();
            var Categr = db.Categories.Find(catgorVM.Cat_ID);
            if (Categr != null)
            {
            
             
                Categr.Cat_Descrip = catgorVM.Cat_Descrip;
                if (catgorVM.Cat_Img != null)
                {
                    //var data = new byte[catgorVM.Cat_Img.];
                    //Categr.Cat_Img = data;
                    var length = catgorVM.Cat_Img.InputStream.Length; //Length: 103050706
                    MemoryStream target = new MemoryStream();
                    catgorVM.Cat_Img.InputStream.CopyTo(target); // generates problem in this line
                    byte[] data = target.ToArray();
                    Categr.Cat_Img = data;

                }
                db.Entry(Categr).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                return Categr;
            }
            return null;
        }

        // use this method to Delete Category and save DataBase.
        public static int DeleteCategory(int id)
        {

            var db = new AccountingContainer();
            Category Cater = db.Categories.Find(id);
            if (Cater != null)
            {

                db.Categories.Remove(Cater);
                db.SaveChanges();
                db.Dispose();
                return 1;
            }

            return 0;

        }
        /// <summary>
        /// for adding catigory from Excel Sheet
        /// </summary>
        /// <param name="excelfile"></param>
        /// <returns></returns>
        public static String Import(HttpPostedFileBase excelfile)
        {
            var data = getdataFillter(excelfile);
            List<Category> categoriesList = new List<Category>();
            if (data.Item2.Count() == 0)
            {
                foreach (var item in data.Item1)
                {
                    var cat = CreateCatgeryFromFile(item);
                    categoriesList.Add(cat);

                }


                return "تم حفظ الملف بنجاح";

            }
            string combindedString = string.Join(" ,", data.Item2.ToArray());
            return "يوجد اخطاء فى الاسطر التالية" + combindedString;

        }
        /// <summary>
        /// get validation of Importing file
        /// </summary>
        /// <param name="excelfile"></param>
        /// <returns></returns>
        public static Tuple<List<CategroyVM>, List<String>> getdataFillter(HttpPostedFileBase excelfile)
        {
            var catgoryVMList = new List<CategroyVM> { };
            var messageList = new List<String> { };
            var db = new AccountingContainer();
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                return Tuple.Create(catgoryVMList, messageList);

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
                        CategroyVM catgVM = new CategroyVM();
                        // cust.Customer_Id = int.Parse(((Range)range.Cells[row, 1]).Text);
                        catgVM.Cat_Descrip = ((Range)range.Cells[row, 1]).Text;
                        if (catgVM.Cat_Descrip == "")
                        {
                            messageList.Add(row.ToString());
                            continue;
                        }
                        catgoryVMList.Add(catgVM);

                    }
                    //workbook.Save();
                    //workbook.Close(path);
                    //File.Delete(path);

                }
            }
            return Tuple.Create(catgoryVMList, messageList);
        }
        /// <summary>
        /// adding Catigory from Excel File
        /// </summary>
        /// <param name="catgorVM"></param>
        /// <returns></returns>
        public static Category CreateCatgeryFromFile(CategroyVM catgorVM)
        {
            var db = new AccountingContainer();

            var Categr = new Category();
            var lastSerial = db.Categories.OrderByDescending(c => c.Cat_ID).FirstOrDefault();
            if (lastSerial == null)
            {
                Categr.Cat_Code = "CAT-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                Categr.Cat_Code = "CAT-" + (Convert.ToInt32(lastSerial.Cat_Code.Substring(4, lastSerial.Cat_Code.Length - 4)) + 1).ToString("D3");
            }

            Categr.Cat_Descrip = catgorVM.Cat_Descrip;


            db.Categories.Add(Categr);
            db.SaveChanges();
            db.Dispose();
            return Categr;
        }
    }
}