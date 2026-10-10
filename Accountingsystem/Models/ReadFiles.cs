using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.Models
{
    public class ReadFiles
    {
        //private HttpPostedFileBase _myfile; 
        //public ReadFiles(HttpPostedFileBase file)
        //{
        //    _myfile = file;
        //}
      
        public static string[] MyWords = new string[2];
        private List<string> Mylines = new List<string>();
        public Dictionary<string, string> WordsList = new Dictionary<string, string>();
        private List<string> MyColumNames = new List<string>();

        private string[] MySerprators = new string[2];
        private char[] sepratoers = {' ',',' ,'#'};
        /// <summary>
        /// making arrary to return null        /// </summary>
        /// <returns></returns>
        public static  string[] Getarrayreturnarray()
        {
           
            return null;
        }


            

    }
}