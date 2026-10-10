using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Accountingsystem.Models
{
    public partial class GL_Type
    {/// <summary>
     /// ارجاع جميع انواع القيود اليومية
     /// </summary>
     /// <returns>List<GL_Type></returns>
        public static List<GL_Type> getAllData()
        {
            var db = new AccountingContainer();
            var gl_type = db.GL_Type.ToList();
            return gl_type;


        }
        public static GL_Type createNewGLType(GL_TypeVM glTypeVm)
        {//ارسال واجهه لأضافة نوع قيد جديد
            var db = new AccountingContainer();
            var glType = new GL_Type();
            glType.GL_Type_Description = glTypeVm.GL_Type_Description;
            glType.GL_Type_Name = glTypeVm.GL_Type_Name;

            db.GL_Type.Add(glType);
            db.SaveChanges();
            db.Dispose();
            return glType;


        }
        public static GL_Type getDetailOfTypeOfGL(int id)
        {//يقوم بارجاع البيانات للنوع القيد المخصص
            var db = new AccountingContainer();
            var glType = db.GL_Type.Find(id);
            return glType;

        }
        public static GL_TypeVM fillDataForEditing(int id)
        {//عرض البيانات نوع القيد المراد تعديلة
            var GlType = getDetailOfTypeOfGL(id);
            var GLTypeVm = new GL_TypeVM();
            GLTypeVm.GL_Type_Id = GlType.GL_Type_Id;
            GLTypeVm.GL_Type_Name = GlType.GL_Type_Name;
            GLTypeVm.GL_Type_Description = GlType.GL_Type_Description;

            return GLTypeVm;


        }
        public static GL_Type editTheData(GL_TypeVM glTypevm)
        {//حفظ التعديلات على نوع القيد 
            var db = new AccountingContainer();
            var glType = db.GL_Type.Find(glTypevm.GL_Type_Id);
            if (glType != null)
            {
                glType.GL_Type_Id = glTypevm.GL_Type_Id;
                glType.GL_Type_Name = glTypevm.GL_Type_Name;
                glType.GL_Type_Description = glTypevm.GL_Type_Description;
                db.Entry(glType).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                return glType;



            }
            return null;

        }

    }
}