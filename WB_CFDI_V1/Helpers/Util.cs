using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.Web.Mvc
{
    public static class Util
    {
        public static MvcHtmlString GetFecha(this HtmlHelper html)
         {
              if (Environment.MachineName != "CTL-PC")
              {
                    //    new DateTime(2010, 01, 01).ToString("new Date(yyyy, MM, dd)");
                   return new MvcHtmlString("new Date('2010/01/01')"); 
              }
              else
              {
                   var dt = DateTime.Now.AddDays(-DateTime.Now.Day + 1);

                   var yyyy = dt.Year;
                   var mm   = dt.Month;
                   var dd   = dt.Day;


                   return new MvcHtmlString(string.Format("new Date('{0}/{1}/{2}')", yyyy, mm, dd)); 
              }
         }
    }
}