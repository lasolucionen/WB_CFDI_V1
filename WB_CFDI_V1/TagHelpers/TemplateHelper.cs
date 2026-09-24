using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace WB_CFDI_V1.TagHelpers
{
    public class TemplateHelper
    {
        /// <summary>
        /// Render a Partial View (MVC User Control, .ascx) to a string using the given ViewData.
        /// http://www.joeyb.org/blog/2010/01/23/aspnet-mvc-2-render-template-to-string
        /// </summary>
        /// <param name="controlName"></param>
        /// <param name="viewData"></param>
        /// <returns></returns>
        public static string RenderPartialToString(string controlName, object viewData)
        {
            ViewDataDictionary vd = new ViewDataDictionary(viewData);
            ViewPage vp = new ViewPage { ViewData = vd };
            Control control = vp.LoadControl(controlName);

            vp.Controls.Add(control);

            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                using (HtmlTextWriter tw = new HtmlTextWriter(sw))
                {
                    vp.RenderControl(tw);
                }
            }

            return sb.ToString();
        }
    }
}