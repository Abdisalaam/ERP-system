using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class SystemRoleVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public  List<RoleFunctionVM> RoleFunctions { get; set; }
        public List<SystemFunction> systemFunctions { get; set; }
    }
}