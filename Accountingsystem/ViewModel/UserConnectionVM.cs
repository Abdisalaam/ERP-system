using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class UserConnectionVM
    {
        public int Id { get; set; }
        public string Email { get; set; }      
        public string SessionId { get; set; }
        public Nullable<System.DateTime> Expires { get; set; }
        public string Token { get; set; }
    }
}