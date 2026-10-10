
using System.Collections.Generic;
using System.Linq;


namespace Accountingsystem.Models
{
    public partial class GLDetail
    {//تقوم بارجاع جميع تفاصيل القيد
        public static List<GLDetail> getAllGLDetail()
        {
            var db = new AccountingContainer();
            var glDetail = db.GLDetails.ToList();
            return glDetail;
        }
    }
}