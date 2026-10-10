using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class SewingDetailVM
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public int SewingId { get; set; }
        public string Description { get; set; }
    }
}