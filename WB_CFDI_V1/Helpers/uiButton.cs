using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;
using WB_CFDI_V1;
using Newtonsoft.Json;

namespace System.Web.Mvc
{



    public interface IuiButton
    {
        UiButton Create();
        MvcHtmlString GetScript();
        void ToScript();
        UiButton Width(string value, bool quotes);
        UiButton Height(string value, bool quotes);
        UiButton PaddingTop(string value, bool quotes);
        UiButton FontSize(string value, bool quotes);
        UiButton Style(string value);
        UiButton Class(string value);
        UiButton Value(string value);
        UiButton Type(string value);
        MvcHtmlString Click();
        UiButton Click(MvcHtmlString value);
        UiButton Click(Func<object, HelperResult> value);
        UiButton MarginRight(string value, bool quotes = true);
        UiButton Disabled(bool value);
        MvcHtmlString GetDisabled(bool value);
    }

    public class UiButton : IuiButton
    {
        private HtmlHelper HtmlHelper { get; set; }
        private String id { get; set; }
        private Settings op = new Settings();
        private string style { get; set; }
        private string _type { get; set; }
        private string _class { get; set; }
        private string _value { get; set; }
        private string _submit { get; set; }
        private string _disabled { get; set; }

        public UiButton()
        {
        }

        public UiButton(HtmlHelper helper, string id)
        {
            this.HtmlHelper = helper;
            this.id = id;
            op.height = "'30px'";
            op.paddingTop = "'5px'";

            op.fontSize = "'1em'";
            _type = "button";

/*            var path=HttpContext.Current.Server.MapPath("~/Views/Shared/Ajax.js");
            string data = File.ReadAllText(path);

            var ajax = data.Split(new string[] { "//ajax\r\n" },2,StringSplitOptions.None)[1];
                ajax = ajax.Split(new string[] { "\r\n///ajax" },StringSplitOptions.None)[0];

           var error = data.Split(new string[] { "//error\r\n" }, 2, StringSplitOptions.None)[1];
               error = error.Split(new string[] { "\r\n///error" }, StringSplitOptions.None)[0];

               data = ajax.Replace("[error]", "function(){ }");*/

        }

        public UiButton Create()
        {
            var htm = new TagBuilder("input");
            htm.MergeAttribute("id", id);
            htm.MergeAttribute("name", id);

            if (!String.IsNullOrEmpty(style))
            {
                htm.MergeAttribute("style", style);
            }

            if (!String.IsNullOrEmpty(_class))
            {
                htm.MergeAttribute("class", _class);
            }


            if (!String.IsNullOrEmpty(_type))
            {
                htm.MergeAttribute("type", _type);
            }

            if (!String.IsNullOrEmpty(_value))
            {
                htm.MergeAttribute("value", _value);
            }

            var disable = "";

            if (!String.IsNullOrEmpty(_disabled))
            {
                 disable = "{ disabled: " + _disabled + "}";
            }

            HtmlHelper.ViewContext.Writer.WriteLine(htm.ToString(TagRenderMode.SelfClosing));


            var script1 = string.Format(
                      @"$(""#{0}"").button({1});" + Environment.NewLine,
                      id,
                      disable);

            script1.Send(HtmlHelper, "script");
            

            var script2 = string.Format(
                      @"$(""#{0}"").css({1});" + Environment.NewLine,
                      id,
                      op.Json());

            script2.Send(HtmlHelper, "script");

            return this;
        }

        public MvcHtmlString GetScript()
        {
            var script2 = string.Format(
                      @"$(""#{0}"").css({1});" + Environment.NewLine,
                      id,
                      op.Json());

            
            return new MvcHtmlString(script2);
        }

        public void ToScript()
        {
            GetScript().Send(HtmlHelper, "script");
        }

        public UiButton Disabled(bool value)
        {
             _disabled = value.ToString().ToLower();
             return this;
        }

        public MvcHtmlString GetDisabled(bool value)
        {
            var script1 = string.Format(
                      @"$(""#{0}"").button({1});" + Environment.NewLine,
                      id,
                      "{ disabled: " + value.ToString().ToLower() + "}");

            return new MvcHtmlString(script1);
        }

        public MvcHtmlString Click()
        {

             var script= "$(\"#" + id + "\").click();";
             return new MvcHtmlString(script);
        }

        public UiButton Click(MvcHtmlString value)
        {
            StringBuilder st=new StringBuilder();
            st.AppendLine("$(\"#" + id + "\").click(function() {");
            st.AppendLine(value.Str());
            st.AppendLine("});");

            st.Send(HtmlHelper, "script");

            return this;
        }

        public UiButton Click(Func<object, HelperResult> value)
        {
             StringBuilder st = new StringBuilder();
             st.AppendLine("$(\"#" + id + "\").click(function() {");
             st.AppendLine(value.Str());
             st.AppendLine("});");

             st.Send(HtmlHelper, "script");

             return this;
        }

        public UiButton Value(string value)
        {
             _value = value;
             return this;
        }

        public UiButton Width(string value, bool quotes = true)
        {
            op.width = value.Quotes(quotes);
            return this;
        }

        public UiButton PaddingTop(string value, bool quotes = true)
        {
            op.paddingTop = value.Quotes(quotes);
            return this;
        }

        public UiButton MarginRight(string value, bool quotes = true)
        {
            op.marginRight = value.Quotes(quotes);
            return this;
        }

        public UiButton FontSize(string value, bool quotes = true)
        {
            op.fontSize = value.Quotes(quotes);
            return this;
        }

        public UiButton Height(string value, bool quotes = true)
        {
            op.height = value.Quotes(quotes);
            return this;
        }

        public UiButton Style(string value)
        {
            this.style = value;
            return this;
        }

        public UiButton Type(string value)
        {
            this._type = value;
            return this;
        }

        public UiButton Class(string value)
        {
            this._class = value;
            return this;
        }

        public partial class Settings
        {
            [JsonConverter(typeof(PlainJson))]
            public string width { get; set; }

            [JsonConverter(typeof(PlainJson))]
            public string height { get; set; }

            [JsonProperty("padding-top")]
            [JsonConverter(typeof(PlainJson))]
            public string paddingTop { get; set; }

            [JsonProperty("margin-right")]
            [JsonConverter(typeof(PlainJson))]
            public string marginRight { get; set; }

            [JsonProperty("font-size")]
            [JsonConverter(typeof(PlainJson))]
            public string fontSize { get; set; }

        }

    }

     public static class uiButton
     {
         public static IuiButton UiButton(this HtmlHelper helper, string id)
         {
               return new UiButton(helper, id);
          }
     }
}