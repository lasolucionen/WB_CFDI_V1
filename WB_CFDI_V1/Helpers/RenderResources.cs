using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.WebPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using WB_CFDI_V1;

namespace System.Web.Mvc
{

     public static class renderResources
     {

          public static void Script(this HtmlHelper HtmlHelper, Func<object, HelperResult> Template)
          {
               
               Resource(HtmlHelper, o => new HelperResult(w => w.WriteLine(Template.Str())), "script");
          }

          public static IHtmlString Resource(this HtmlHelper HtmlHelper, Func<object, HelperResult> Template, string Type)
          {
               if (HtmlHelper.ViewContext.HttpContext.Items[Type] != null)
               {
                    ((List<Func<object, HelperResult>>)HtmlHelper.ViewContext.HttpContext.Items[Type]).Add(Template);
               }
               else
               {
                    HtmlHelper.ViewContext.HttpContext.Items[Type] = new List<Func<object, HelperResult>>{Template};
               }

               return new HtmlString(String.Empty);
          }

          public static IHtmlString RenderResources(this HtmlHelper HtmlHelper, string Type)
          {
               if (HtmlHelper.ViewContext.HttpContext.Items[Type] == null) return new HtmlString(String.Empty);

               var Resources = (List<Func<object, HelperResult>>)HtmlHelper.ViewContext.HttpContext.Items[Type];

               foreach( var Resource in Resources.Where(Resource => Resource != null) )
               {
                    HtmlHelper.ViewContext.Writer.Write(Resource(null));
               }

               return new HtmlString(String.Empty);
          }
     }
}