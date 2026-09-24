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
      public interface IDropDownList
     {
          
          //DropDownList AutoDropDownHeight(string value);
          DropDownList Source(string value);
          DropDownList Create();
          MvcHtmlString GetScript();
          void ToScript();
          DropDownList Theme(string value, bool quotes = true);
          DropDownList Width(string value, bool quotes = true);
          DropDownList Height(string value, bool quotes = true);
          DropDownList Style(string value);
          DropDownList Class(string value);
          DropDownList PromptText(string value, bool quotes = true);
          DropDownList DisplayMember(string value, bool quotes = true);
          DropDownList ValueMember(string value, bool quotes = true);
          DropDownList DropDownHeight(string value, bool quotes = true);
          DropDownList SelectedIndex(string value);
          MvcHtmlString Value(string value);
          MvcHtmlString SelectedIndex();
          MvcHtmlString GetItem(string value);

          //void SetSource(string url);
          //void SetSource(string url, string index);

          //MvcHtmlString GetSource(string url);
          //MvcHtmlString GetSource(string url, string index);
     }

    public class DropDownList : IDropDownList
    {
        private HtmlHelper HtmlHelper { get; set; }
        private String id { get; set; }
        private Settings op =new Settings();
        private string style { get; set; }
        private string _class { get; set; }

        public DropDownList()
        {
        }


        public DropDownList(HtmlHelper helper, string id)
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

        public DropDownList AddScript(HelperResult script)
        {
             var src = script.Str();
             src.Send(HtmlHelper,"script");

             return this;
        }

        public DropDownList DropDownHeight(string value, bool quotes = true)
        {
             op.dropDownHeight = value.Quotes(quotes);
             return this;
        }

        public DropDownList DisplayMember(string value, bool quotes = true)
        {
             op.displayMember= value.Quotes(quotes);
             return this;
        }

        public DropDownList SelectedIndex(string value)
        {
             op.selectedIndex = value;
             return this;
        }


        public MvcHtmlString Value(string value)
        {
            var script = string.Format(
                @"$(""#{0}"").jqxDropDownList('val', {1});" + Environment.NewLine,
                id,value);


            return new MvcHtmlString(script);
        }

        public MvcHtmlString SelectedIndex()
        {
            var script = string.Format(
                      @"$(""#{0}"").jqxDropDownList('selectedIndex');" + Environment.NewLine,
                      id);


            return new MvcHtmlString(script);
        }

        public MvcHtmlString GetItem(string value)
        {
            var script = string.Format(
                      @"$(""#{0}"").jqxDropDownList('getItem',{1});" + Environment.NewLine,
                      id,
                      value);


            return new MvcHtmlString(script);
        }

        public DropDownList ValueMember(string value, bool quotes = true)
        {
             op.valueMember= value.Quotes(quotes);
             return this;
        }

        public DropDownList Style(string value)
        {
             this.style=value;
             return this;
        }

        public DropDownList Class(string value)
        {
             this._class=value;
             return this;
        }

       /* public DropDownList AutoDropDownHeight(string value)
        {
             op.autoDropDownHeight=value;
             return this;
        }*/

        public DropDownList Source(string value)
        {
             op.source=value;
             return this;
        }

        public DropDownList Create()
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
                       @"$(""#{0}"").jqxDropDownList({1});" + Environment.NewLine,
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

        public DropDownList Theme(string value, bool quotes = true)
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
                       @"$(""#{0}"").jqxDropDownList({1});" + Environment.NewLine, 
                       id,
                       op.Json());

             return new MvcHtmlString(script1 + script);
        }

        public DropDownList Width(string value, bool quotes = true)
        {
            op.width=value.Quotes(quotes);
            return this;
        }

        public DropDownList Height(string value, bool quotes = true)
        {
            op.height= value.Quotes(quotes);
            return this;
        }

        public DropDownList PromptText(string value, bool quotes = true)
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
        }


    }


    public static class jqxDropDownList
    {
         public static IDropDownList JqxDropDownList(this HtmlHelper helper, string id)
         {
              return new DropDownList(helper, id);
         }
    }
}