using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.WebPages;

using WB_CFDI_V1;

namespace System.Web.Mvc
{
    public static class Wait
    {
/*         private static int time;
         static Wait()
         {
              
         }

         class DisposableHelper : IDisposable
         {
              private Action end;

              // When the object is created, write "begin" function
              public DisposableHelper(Action begin, Action end)
              {
                   this.end = end;
                   begin();
              }

              // When the object is disposed (end of using block), write "end" function
              public void Dispose()
              {
                   end();
              }
         }

         public static IDisposable WaitScript(this HtmlHelper htmlHelper, Func<object, HelperResult> script, int time = 30)
         {
              Wait.time = time;
              return new DisposableHelper(
                        () => htmlHelper.Begin(),
                        () => htmlHelper.End());
         }

         private static void Begin(this HtmlHelper htmlHelper)
         {
             htmlHelper.ViewContext.Writer.WriteLine("setTimeout(function () {");
         }


         private static void End(this HtmlHelper htmlHelper)
         {
             htmlHelper.ViewContext.Writer.WriteLine("}, " + time + ");");
         }*/

        public static MvcHtmlString wait(this HtmlHelper htmlHelper, Func<object, HelperResult> script, int time = 30)
        {

            StringBuilder str=new StringBuilder();

            str.AppendLine("$(\"#init\").removeClass('no-show');")
               .AppendLine("setTimeout(function () {")
               .AppendLine(script.Str())
               .AppendLine("$(\"#init\").addClass('no-show');")
               .AppendLine("}, " + time + ");");

            return new MvcHtmlString(str.ToString());

         }

        public static MvcHtmlString StartWait(this HtmlHelper htmlHelper)
        {
             StringBuilder str = new StringBuilder();

             str.AppendLine("$(\"#init\").removeClass('no-show');")
                .AppendLine("setTimeout(function () {");


             return new MvcHtmlString(str.ToString());
        }

        public static MvcHtmlString EndWait(this HtmlHelper htmlHelper, int time = 30)
        {
             StringBuilder str = new StringBuilder();

             str.AppendLine("$(\"#init\").addClass('no-show');")
                .AppendLine("}, " + time + ");");

            return new MvcHtmlString(str.ToString());
        }

        public static MvcHtmlString StartLoading(this HtmlHelper htmlHelper)
        {
            return new MvcHtmlString("$(\"#init\").removeClass('no-show');");
        }
        public static MvcHtmlString EndLoading(this HtmlHelper htmlHelper)
        {
            return new MvcHtmlString("$(\"#init\").addClass('no-show');");
        }
    }
}