using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.WebPages;
using System.Xml;
using WB_CFDI_V1;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace System.Web.Mvc
{
    public interface IComboBox
     {
          
          //DropDownList AutoDropDownHeight(string value);
          ComboBox Source(string value);
          ComboBox Create();
          MvcHtmlString GetScript();
          void ToScript();
          ComboBox Theme(string value, bool quotes = true);
          ComboBox Width(string value, bool quotes = true);
          ComboBox Height(string value, bool quotes = true);
          ComboBox Style(string value);
          ComboBox Class(string value);
          ComboBox PromptText(string value, bool quotes = true);
          ComboBox DisplayMember(string value, bool quotes = true);
          ComboBox ValueMember(string value, bool quotes = true);
          ComboBox DropDownHeight(string value, bool quotes = true);
          ComboBox SelectedIndex(string value);
          ComboBox SearchMode(string value, bool quotes = true);

          //void SetSource(string url);
          //void SetSource(string url, string index);

          //MvcHtmlString GetSource(string url);
          //MvcHtmlString GetSource(string url, string index);
     }

    public class ComboBox : IComboBox
    {
        private HtmlHelper HtmlHelper { get; set; }
        private String id { get; set; }
        private Settings op =new Settings();
        private string style { get; set; }
        private string _class { get; set; }

        public ComboBox()
        {
        }


        public ComboBox(HtmlHelper helper, string id)
        {
            this.HtmlHelper = helper;
            this.id = id;
            op.height = "'27px'";
            op.autoDropDownHeight = "auto";
        }


        /*
        public void SetSource(string url)
        {
             GetSource(url).Send(HtmlHelper, "script");
        }

        public void SetSource(string url, string index)
        {
             GetSource(url, index).Send(HtmlHelper, "script");
        }

        public MvcHtmlString GetSource(string url)
        {

             string frm = @"
getData(""{0}"", {{ }}, function(data){{

     $(""#{1}"").jqxDropDownList({{ source:data }});

}});
";

             frm = String.Format(frm, url, id);

             //var ss=HtmlHelper.Partial("~/Views/Shared/_CellsRenderer1.cshtml").Str();


             return new MvcHtmlString(frm);
        }

        public MvcHtmlString GetSource(string url, string index)
        {

             string frm = @"
getData(""{0}"", {{ }}, function(data){{

     $(""#{1}"").jqxDropDownList({{ source:data, selectedIndex:{2}  }});

}});
";

             frm = String.Format(frm, url, id, index);

             //var ss=HtmlHelper.Partial("~/Views/Shared/_CellsRenderer1.cshtml").Str();

             
             return new MvcHtmlString(frm);
        }

        */

        public ComboBox AddScript(HelperResult script)
        {
             var src = script.Str();
             src.Send(HtmlHelper,"script");

             return this;
        }

        public ComboBox SearchMode(string value, bool quotes = true)
        {
            op.searchMode = value.Quotes(quotes);
             return this;
        }

        public ComboBox DropDownHeight(string value, bool quotes = true)
        {
             op.dropDownHeight = value.Quotes(quotes);
             return this;
        }

        public ComboBox DisplayMember(string value, bool quotes = true)
        {
             op.displayMember= value.Quotes(quotes);
             return this;
        }

        public ComboBox SelectedIndex(string value)
        {
             op.selectedIndex = value;
             return this;
        }

        public ComboBox ValueMember(string value, bool quotes = true)
        {
             op.valueMember= value.Quotes(quotes);
             return this;
        }

        public ComboBox Style(string value)
        {
             this.style=value;
             return this;
        }

        public ComboBox Class(string value)
        {
             this._class=value;
             return this;
        }

       /* public DropDownList AutoDropDownHeight(string value)
        {
             op.autoDropDownHeight=value;
             return this;
        }*/

        public ComboBox Source(string value)
        {
             op.source=value;
             return this;
        }

        public ComboBox Create()
        {
             var htm = new TagBuilder("div");
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

             string script1 = "";

             if (!string.IsNullOrEmpty(op.source))
             {
                  script1 =  @"auto = true;"                       + Environment.NewLine;
                  script1 += "if(" + op.source + ".length > 10) {" + Environment.NewLine;
                  script1 += "auto = false;"                       + Environment.NewLine;
                  script1 += "}"                                   + Environment.NewLine;
             }
                    
           
             HtmlHelper.ViewContext.Writer.WriteLine(htm.ToString(TagRenderMode.Normal));

             var script = string.Format(
                       @"$(""#{0}"").jqxComboBox({1});" + Environment.NewLine,
                       id,
                       op.Json());

             (script1 + script).Send(HtmlHelper, "script");

             return this;
        }

        public void ToScript()
        {
            GetScript().Send(HtmlHelper, "script");

             //return this;
        }

        public ComboBox Theme(string value, bool quotes = true)
        {
             op.theme=value.Quotes(quotes);
             return this;
        }

        public MvcHtmlString GetScript()
        {
             string script1 = "";

             if (!string.IsNullOrEmpty(op.source))
             {
                  script1 = @"auto = true;" + Environment.NewLine;
                  script1 += "if(" + op.source + ".length > 10) {" + Environment.NewLine;
                  script1 += "auto = false;" + Environment.NewLine;
                  script1 += "}" + Environment.NewLine;
             }


             var script=string.Format(
                       @"$(""#{0}"").jqxComboBox({1});" + Environment.NewLine, 
                       id,
                       op.Json());

             return new MvcHtmlString(script1 + script);
        }

        public ComboBox Width(string value, bool quotes = true)
        {
            op.width=value.Quotes(quotes);
            return this;
        }

        public ComboBox Height(string value, bool quotes = true)
        {
            op.height= value.Quotes(quotes);
            return this;
        }

        public ComboBox PromptText(string value, bool quotes = true)
        {
            op.promptText= value.Quotes(quotes);
            return this;
        }

        //[JsonObject(MemberSerialization.OptIn)]
        public partial class Settings
        {
             //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

             [JsonConverter(typeof(PlainJson))]
             public string autoDropDownHeight { get; set; }

             [JsonConverter(typeof(PlainJson))]
             public string source { get; set; }

             [JsonConverter(typeof(PlainJson))]
             public string theme { get; set; }

             [JsonConverter(typeof(PlainJson))]
             public string width { get; set; }

             [JsonConverter(typeof(PlainJson))]
             public string height { get; set; }

             [JsonConverter(typeof(PlainJson))]
             public string promptText { get; set; }

             [JsonConverter(typeof(PlainJson))]
             public string displayMember { get; set; }

             [JsonConverter(typeof(PlainJson))]
             public string valueMember { get; set; }

             [JsonConverter(typeof(PlainJson))]
             public string dropDownHeight { get; set; }

             [JsonConverter(typeof(PlainJson))]
             public string selectedIndex { get; set; }

             [JsonConverter(typeof(PlainJson))]
             public string searchMode { get; set; }
        }


    }


    public static class jqxComboBox
    {
        public static IComboBox JqxComboBox(this HtmlHelper helper, string id)
         {
             return new ComboBox(helper, id);
         }
    }
}