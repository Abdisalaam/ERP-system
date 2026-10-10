using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class CategroyVM
    {
        public int Cat_ID { get; set; }

        
        public HttpPostedFileBase Cat_Img { get; set; }
        public HttpFileCollectionBase Cat_Img2 { get; set; }

        public string Cat_Code { get; set; }

  
        public string Cat_Descrip { get; set; }
        public Decimal? CatQuantity { get; set; }
        public Decimal? CatSales { get; set; }
    }
}