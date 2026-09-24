using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using WB_CFDI_V1;

namespace System.Web.Mvc
{
    public static class ActionLink
    {
       public static string GetMsg(this HtmlHelper html)
       {
          return "ok";
       }


       public static MvcHtmlString MenuEmpresa(this HtmlHelper helper)
       {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {

                 return new MvcHtmlString(
                           "<li style=\"border:0px solid transparent !important;background-color:transparent !important;color:black !important;cursor:default !important;\">"
                           + "<b>Empresa:</b> <span id=\"razonSocialEmp\"></span>"
                           + "</li>");
            }
            else
            {
                 return new MvcHtmlString("");
            }
            
       }


       public static MvcHtmlString Login(this HtmlHelper helper, string text, string action, string controller)
       {
          if (HttpContext.Current.User.Identity.IsAuthenticated)
          {
             return MvcHtmlString.Empty;
          }

          return MenuLink(helper, text, action, controller);
       }

       public static MvcHtmlString Logout(this HtmlHelper helper, string text, string action, string controller)
       {
          if (!HttpContext.Current.User.Identity.IsAuthenticated)
          {
             return MvcHtmlString.Empty;
          }

          return MenuLink(helper, text, action, controller);
       }

       public static MvcHtmlString MenuInRole(this HtmlHelper helper, string text, string action, string controller, string roles)
       {
            //List<string> roles=new List<string>();

            //roles.AddRange(new []{"1", "2"});

            foreach( var role in roles.Split(',') )
            {
                 if (HttpContext.Current.User.IsInRole(role))
                 {
                      return MenuLink(helper, text, action, controller);
                 }
            }

            return MvcHtmlString.Empty;
       }


       public static MvcHtmlString MenuLink(this HtmlHelper helper, string text, string action, string controller)
       {
          var routeData         = helper.ViewContext.RouteData.Values;
          var currentController = routeData["controller"];
          var currentAction     = routeData["action"];

          if (String.Equals(action,       currentAction as string,     StringComparison.OrdinalIgnoreCase)
             && String.Equals(controller, currentController as string, StringComparison.OrdinalIgnoreCase))
          {
              return new MvcHtmlString("<li style=\"border: 1px solid #fd9595 !important\">" + helper.ActionLink(text, action, controller, null, new { style = "" }) + "</li>");
          }
          return new MvcHtmlString("<li>" + helper.ActionLink(text, action, controller) + "</li>");
       }
    }
}