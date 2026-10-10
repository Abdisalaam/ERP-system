using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class AnswerVM
    {
        public int Id { get; set; }
        public string PathImage { get; set; }
        public string Desc { get; set; }
        public bool IsActive { get; set; }
        public int QuestionId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public HttpPostedFileBase Image { get; set; }
    }
}