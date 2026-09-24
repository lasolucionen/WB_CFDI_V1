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
using WB_CFDI_V1;
using Newtonsoft.Json;

namespace System.Web.Mvc
{

    public interface IDialogButtonInternal
     {

          string click { get; set; }
          string id { get; set; }
          string text { get; set; }

     }

     public interface IDialogButton
     {
         IDialogButton Click(Func<object, HelperResult> value);
         IDialogButton Click(HelperResult value);
     }

    public class DialogButton: IDialogButton,IDialogButtonInternal
    {

         UiDialog uiDialog { get; set; }
        public DialogButton(UiDialog uiDialog)
         {
              this.uiDialog = uiDialog;
         }

        public IDialogButton Click(Func<object, HelperResult> value)
        {

            StringBuilder st = new StringBuilder();
            st.AppendLine("function() {");
            st.AppendLine(value.Str());
            st.AppendLine("}");

            this.click = st.ToString();

             return this;
        }

        public IDialogButton Click(HelperResult value)
        {
             StringBuilder st = new StringBuilder();
             st.AppendLine("function() {");
             st.AppendLine(value.Str());
             st.AppendLine("}");

             this.click = st.ToString();

             return this;
        }

        public string click { get; set; }
        public string id { get; set; }
        public string text { get; set; }


    }

    public class Button
    {
        private UiDialog uiDialog { get; set; }

        public Button(UiDialog value)
         {
             uiDialog = value;
         }

         public IDialogButton Add(string id, string text)
         {
              return uiDialog.Add(id, text);
         }

         public IDialogButton Add(string text)
         {
              return uiDialog.Add(text);
         }

    }

    public interface IuiDialog
    {
        UiDialog Create();
        MvcHtmlString GetScript();
        MvcHtmlString GetScript(IList<IDialogButtonInternal> opt);
        void ToScript();
        UiDialog Width(string value, bool quotes);
        UiDialog AutoOpen(bool value);
        UiDialog Resizable(bool value);
        UiDialog Modal(bool value);
        UiDialog DialogClass(string value, bool quotes = true);
       // UiDialog Open(string value);
        MvcHtmlString Open();
        UiDialog Close(string value);
        UiDialog Style(string value);
        UiDialog Class(string value);
        UiDialog Title(string value);
        UiDialog Content(MvcHtmlString value);
        UiDialog Buttons(Action<Button> value);
        IList<IDialogButtonInternal> GButtons(Action<Button> value);
        MvcHtmlString Html(string value);
        MvcHtmlString Open(IList<IDialogButtonInternal> opt);
        UiDialog PaddingTop(string value);
        UiDialog PaddingBottom(string value);
        UiDialog PaddingLeft(string value);
        UiDialog PaddingRight(string value);
    }

    public class UiDialog : IuiDialog
    {
        private HtmlHelper HtmlHelper { get; set; }
        private String id { get; set; }
        private Settings op = new Settings();
        private Css css = new Css();
        private string style { get; set; }
        private string _class { get; set; }
        private string title { get; set; }
        private string content { get; set; }

        
        public UiDialog Buttons(Action<Button> value)
        {
             Button bt=new Button(this);
             value(bt);
             return this;
        }

        public IList<IDialogButtonInternal> GButtons(Action<Button> value)
        {
            UiDialog tmp = new UiDialog(HtmlHelper,id);

             Button bt = new Button(tmp);
             value(bt);
             return tmp.DialogButtons;
        }
        
        internal IList<IDialogButtonInternal> DialogButtons { get; set; }
        internal IDialogButton Add(string id, string text)
        {
             DialogButton bt = new DialogButton(this);
             //bt.title = value;
             bt.id = id;
             bt.text = text;
             this.DialogButtons.Add(bt);
             return bt;
        }

        internal IDialogButton Add(string text)
        {
             DialogButton bt = new DialogButton(this);
             bt.text = text;
             this.DialogButtons.Add(bt);
             return bt;
        }

        public UiDialog()
        {
        }

        public UiDialog(HtmlHelper helper, string id)
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

            this.DialogButtons = new List<IDialogButtonInternal>();
        }

        public UiDialog PaddingTop(string value)
        {
             this.style += "padding-top:" + value + " !important;";
             return this;
        }

        public UiDialog PaddingBottom(string value)
        {
             this.style += "padding-bottom:" + value + " !important;";
             return this;
        }

        public UiDialog PaddingLeft(string value)
        {
             this.style += "padding-left:" + value + " !important;";
             return this;
        }

        public UiDialog PaddingRight(string value)
        {
             this.style += "padding-right:" + value + " !important;";
             return this;
        }

        public UiDialog Content(MvcHtmlString value)
        {
             content = value.ToHtmlString();
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
        
        public UiDialog Create()
        {

             foreach (IDialogButtonInternal tc in this.DialogButtons)
             {
                 var btns=new buttons();

                 btns.text = tc.text.Quotes();
                 btns.click = tc.click;

                 if (!String.IsNullOrEmpty(tc.id))
                 {
                      btns.id = tc.id.Quotes();
                 }

                 op.buttons.Add(btns);
             }

             op.create = @"function( e, ui ) { 
                         //$("".mWin"").css('z-index', 999);
                           $('.ui-dialog').css('z-index',999);
                       //$('.ui-widget-overlay').css('z-index',999);
                       }";

            var htm = new TagBuilder("div");
            htm.MergeAttribute("id", id);


            if (!String.IsNullOrEmpty(style))
            {
                htm.MergeAttribute("style", style);
            }

            if (!String.IsNullOrEmpty(_class))
            {
                htm.MergeAttribute("class", _class);
            }


            if (!String.IsNullOrEmpty(title))
            {
                htm.MergeAttribute("title", title);
            }


            htm.InnerHtml = content;

            HtmlHelper.ViewContext.Writer.WriteLine(htm.ToString(TagRenderMode.Normal));

            /*
            var script1 = string.Format(
                      @"$(""#{0}"").button();" + Environment.NewLine,
                      id);


            HtmlHelper.Resource(o => new HelperResult(w => w.Write(script1)), "script");
            */
            var script2 = string.Format(
                      @"$(""#{0}"").dialog({1});" + Environment.NewLine,
                      id,
                      op.Json());

            script2.Send(HtmlHelper, "script");

            return this;
        }

        public MvcHtmlString GetScript()
        {

            return GetScript(null);
        }

        public MvcHtmlString GetScript(IList<IDialogButtonInternal> opt)
        {

             if (opt == null)
             {
                  foreach (IDialogButtonInternal tc in this.DialogButtons)
                  {
                       var btns = new buttons();

                       btns.text  = tc.text.Quotes();
                       btns.click = tc.click;

                       if (!String.IsNullOrEmpty(tc.id))
                       {
                            btns.id = tc.id.Quotes();
                       }

                       op.buttons.Add(btns);
                  }
             }
             else
             {
                  foreach (IDialogButtonInternal tc in opt)
                  {
                       var btns = new buttons();

                       btns.text  = tc.text.Quotes();
                       btns.click = tc.click;

                       if (!String.IsNullOrEmpty(tc.id))
                       {
                            btns.id = tc.id.Quotes();
                       }

                       op.buttons.Add(btns);
                  }
             }

             var script1 = string.Format(
                       @"$(""#{0}"").dialog({1});" + Environment.NewLine,
                       id,
                       op.Json());

             return new MvcHtmlString(script1);
        }

        public MvcHtmlString Open()
        {
             return Open(null);
        }

        public MvcHtmlString Open(IList<IDialogButtonInternal> opt)
        {
             op.buttons = new List<buttons>();


             if (opt == null)
             {
                  foreach (IDialogButtonInternal tc in this.DialogButtons)
                  {
                       var btns = new buttons();

                       btns.text  = tc.text.Quotes();
                       btns.click = tc.click;

                       if (!String.IsNullOrEmpty(tc.id))
                       {
                            btns.id = tc.id.Quotes();
                       }

                       op.buttons.Add(btns);
                  }
             }
             else
             {
                  foreach( IDialogButtonInternal tc in opt )
                  {
                       var btns = new buttons();

                       btns.text  = tc.text.Quotes();
                       btns.click = tc.click;

                       if (!String.IsNullOrEmpty(tc.id))
                       {
                            btns.id = tc.id.Quotes();
                       }

                       op.buttons.Add(btns);
                  }
             }

             var script1 = string.Format(
                       @"$(""#{0}"").dialog({1});" + Environment.NewLine,
                       id,
                       op.Json());

             var script2 = string.Format(
                       @"$(""#{0}"").dialog('open');" + Environment.NewLine,
                       id);

             var script3 = string.Format(
                       @"$(""#{0}"").dialog('widget').position({{ my: 'center', at: 'center', of: window }});" + Environment.NewLine,
                       id);

             return new MvcHtmlString(script1 + script2 + script3);
        }

        public void ToScript()
        {
            GetScript().Send(HtmlHelper, "script");
        }



        public UiDialog Title(string value)
        {
             title = value;
             return this;
        }

        public UiDialog Width(string value, bool quotes = true)
        {
            op.width = value.Quotes(quotes);
            return this;
        }

        public UiDialog AutoOpen(bool value)
        {
            op.autoOpen = value.ToString().ToLower();
            return this;
        }

        public UiDialog Resizable(bool value)
        {
            op.resizable = value.ToString().ToLower();
             return this;
        }

        public UiDialog Modal(bool value)
        {
            op.modal = value.ToString().ToLower();
             return this;
        }

        public UiDialog DialogClass(string value, bool quotes = true)
        {
            op.dialogClass = value.Quotes(quotes);
             return this;
        }


        public UiDialog Close(string value)
        {
            op.close = value;
             return this;
        }

        public UiDialog Style(string value)
        {
            this.style = value;
            return this;
        }

        public UiDialog Class(string value)
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
            public string modal { get; set; }

            [JsonConverter(typeof(PlainJson))]
            public string dialogClass { get; set; }
            
            [JsonConverter(typeof(PlainJson))]
            public string close { get; set; }

            [JsonConverter(typeof(PlainJson))]
            public string create { get; set; }

            public List<buttons> buttons = new List<buttons>();
           
        }

        public partial class Css
        {
             [JsonProperty("padding-top")]
             [JsonConverter(typeof(PlainJson))]
             public string paddingTop { get; set; }

        }

        public partial class buttons
        {
            [JsonConverter(typeof(PlainJson))]
            public string id { get; set; }

            [JsonConverter(typeof(PlainJson))]
            public string text { get; set; }

            [JsonConverter(typeof(PlainJson))]
            public string click { get; set; }
        }

    }

     public static class uiDialog
     {
         public static IuiDialog UiDialog(this HtmlHelper helper, string id)
         {
              return new UiDialog(helper, id);
          }
     }
}