using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;
using System.Xml;
using WB_CFDI_V1;
using Newtonsoft.Json;

namespace System.Web.Mvc
{

    public interface ITabInternal
     {

          string content { get; set; }
          string title { get; set; }
          string tabStyle { get; set; }

     }

     public interface ITab
     {
         ITab Content(MvcHtmlString value);
         ITab TabPadding(string value);
     }

    public class Tabs: ITab,ITabInternal
    {

         CTabs tabs { get; set; }
        public Tabs(CTabs tabs)
         {
              this.tabs = tabs;
         }


        public ITab Content(MvcHtmlString value)
        {
            this.content = value.ToHtmlString();
             return this;
        }

        public ITab TabPadding(string value)
        {
             this.tabStyle += "padding: " + value + ";";
             return this;
        }

        public string content { get; set; }
        public string title { get; set; }
        public string tabStyle { get; set; }


    }

    public class Tab
    {
        private CTabs tabs { get; set; }

        public Tab(CTabs value)
         {
             tabs = value;
         }

         public ITab Add(string title)
         {
              return tabs.Add(title);
         }

    }

    public interface ITabs
    {
        CTabs Create();
        MvcHtmlString GetScript();
        void ToScript();
        CTabs Width(string value);
        CTabs Height(string value);
        CTabs AutoOpen(bool value);
        CTabs Resizable(bool value);
        CTabs Modal(bool value);
        CTabs DialogClass(string value, bool quotes = true);
        CTabs Close(string value);
        CTabs Style(string value);
        CTabs Class(string value);
        CTabs Tabs(Action<Tab> value);
        //IList<ITabInternal> GButtons(Action<Tab> value);
        MvcHtmlString Html(string value);
        CTabs TabMarging(string value);
    }

    public class CTabs : ITabs
    {
        private HtmlHelper HtmlHelper { get; set; }
        private String id { get; set; }
        private Settings op = new Settings();
        private Css css = new Css();
        private string style { get; set; }
        private string tabMargin { get; set; }
        private string _class { get; set; }

        
        public CTabs Tabs(Action<Tab> value)
        {
             Tab bt=new Tab(this);
             value(bt);
             return this;
        }

        /*
        public IList<ITabInternal> GButtons(Action<Tab> value)
        {
            CTabs tmp = new CTabs(HtmlHelper,id);

             Tab bt = new Tab(tmp);
             value(bt);
             return tmp.tabs;
        }*/
        
        internal IList<ITabInternal> tabs { get; set; }
        internal ITab Add(string title)
        {
             Tabs bt = new Tabs(this);
             bt.title = title;
             this.tabs.Add(bt);
             return bt;
        }

        public CTabs()
        {
        }

        public CTabs(HtmlHelper helper, string id)
        {
            this.HtmlHelper = helper;
            this.id = id;
            //op.height = "'30px'";
            //op.paddingTop = "'5px'";
            //op.fontSize = "'1.15em'";
            //op.modal = true;
            //op.dialogClass = "'mWin'";
            //op.autoOpen  = false;
            //op.resizable = false;

            this.tabs = new List<ITabInternal>();
        }

        public CTabs TabMarging(string value)
        {
             this.tabMargin += "margin-left:" + value + ";";
             return this;
        }

        public MvcHtmlString Html(string value)
        {
             var script1 = string.Format(
                       @"$(""#{0}"").html({1});" + Environment.NewLine,
                       id,
                       value);

             return new MvcHtmlString(script1);
        }



        public CTabs Create()
        {



             XmlDocument html  = new XmlDocument();
             XmlElement content = html.CreateElement("div");
             
             html.AppendChild(content);

             content.SetAttribute("id", id);
            
             if (!String.IsNullOrEmpty(style))
             {
                  content.SetAttribute("style", style);
             }

             XmlElement ul = html.CreateElement("ul");
             content.AppendChild(ul);

             bool init = true;
             foreach (ITabInternal tc in this.tabs)
             {
                 XmlElement li = html.CreateElement("li");
                 li.InnerText = tc.title;
                 ul.AppendChild(li);

                 if (!String.IsNullOrEmpty(tabMargin) && init)
                 {
                      li.SetAttribute("style", tabMargin);
                      init = false;
                 }
             }

             foreach (ITabInternal tc in this.tabs)
             {
                  XmlElement div = html.CreateElement("div");

                  if (!String.IsNullOrEmpty(tc.tabStyle))
                  {
                       div.SetAttribute("style",tc.tabStyle );
                  }
                  div.InnerXml     =  tc.content;
                  content.AppendChild(div);
             }


            /*
            var content = new TagBuilder("div");
            content.MergeAttribute("id", id);


            if (!String.IsNullOrEmpty(style))
            {
                content.MergeAttribute("style", style);
            }

            if (!String.IsNullOrEmpty(_class))
            {
                content.MergeAttribute("class", _class);
            }

*/

             HtmlHelper.ViewContext.Writer.WriteLine(html.Beautify());

            /*
            var script1 = string.Format(
                      @"$(""#{0}"").button();" + Environment.NewLine,
                      id);


            HtmlHelper.Resource(o => new HelperResult(w => w.Write(script1)), "script");
            */
            var script2 = string.Format(
                      @"$(""#{0}"").jqxTabs({1});" + Environment.NewLine,
                      id,
                      op.Json());

            script2.Send(HtmlHelper, "script");

            return this;
        }



        public MvcHtmlString GetScript()
        {

             var script1 = string.Format(
                       @"$(""#{0}"").jqxTabs({1});" + Environment.NewLine,
                       id,
                       op.Json());

             return new MvcHtmlString(script1);
        }


        public void ToScript()
        {
            GetScript().Send(HtmlHelper, "script");
        }

        public CTabs Width(string value)
        {
            op.width = value;
            return this;
        }

        public CTabs Height(string value)
        {
             op.height = value;
             return this;
        }

        public CTabs AutoOpen(bool value)
        {
            op.autoOpen = value.ToString().ToLower();
            return this;
        }

        public CTabs Resizable(bool value)
        {
            op.resizable = value.ToString().ToLower();
             return this;
        }

        public CTabs Modal(bool value)
        {
            op.modal = value.ToString().ToLower();
             return this;
        }

        public CTabs DialogClass(string value, bool quotes = true)
        {
            op.dialogClass = value.Quotes(quotes);
             return this;
        }


        public CTabs Close(string value)
        {
            op.close = value;
             return this;
        }

        public CTabs Style(string value)
        {
            this.style = value;
            return this;
        }

        public CTabs Class(string value)
        {
            this._class = value;
            return this;
        }

        public partial class Settings
        {
            [JsonConverter(typeof(PlainJson))]
            public string autoOpen { get; set; }

            [JsonConverter(typeof(PlainJson))]
            public string resizable { get; set; }

            //[JsonProperty("padding-top")]
            [JsonConverter(typeof(PlainJson))]
            public string width { get; set; }

            [JsonConverter(typeof(PlainJson))]
            public string height { get; set; }

            [JsonConverter(typeof(PlainJson))]
            public string modal { get; set; }

            [JsonConverter(typeof(PlainJson))]
            public string dialogClass { get; set; }
            
            [JsonConverter(typeof(PlainJson))]
            public string close { get; set; }
        }

        public partial class Css
        {
             [JsonProperty("padding-top")]
             [JsonConverter(typeof(PlainJson))]
             public string paddingTop { get; set; }

        }


    }

    public static class jqxTabs
     {
        public static ITabs JqxTabs(this HtmlHelper helper, string id)
         {
             return new CTabs(helper, id);
          }
     }
}