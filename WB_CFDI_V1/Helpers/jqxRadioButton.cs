using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using WB_CFDI_V1;
using Newtonsoft.Json;

namespace System.Web.Mvc
{
    public interface IRadioButton
     {
          RadioButton Create();
          MvcHtmlString GetScript();
          void ToScript();
          RadioButton Theme(string value, bool quotes = true);
          RadioButton Width(string value, bool quotes = true);
          RadioButton Height(string value, bool quotes = true);
          RadioButton Style(string value, bool quotes = true);
          RadioButton Class(string value);
          RadioButton Checked(bool value);
          RadioButton Value(string value);
          RadioButton PaddingBottom(string value, bool quotes = true);
          RadioButton Change(MvcHtmlString value);
     }

     public class RadioButton : IRadioButton
     {
          private HtmlHelper HtmlHelper { get; set; }
          private String   id { get; set; }
          private Settings op = new Settings();
          private Css css = new Css();

          private string   style  { get; set; }
          private string   _class { get; set; }
          private string   _value { get; set; }

          public RadioButton()
          {
          }

          public RadioButton(HtmlHelper helper, string id)
          {
               this.HtmlHelper = helper;
               this.id         = id;
          }

          public RadioButton Change(MvcHtmlString value)
          {

              /*
                $("#jqxOpt1").on('change', function (event) {
                    var checked = event.args.checked;

                    if (checked) {
                        $("#lbFecha1, #lbFecha2").html("Recepción");
                        opt = 0;
                    }
                });
               */


              StringBuilder st = new StringBuilder();
              st.AppendLine("$(\"#" + id + "\").on('change', function(event) {");

              st.AppendLine("var checked = event.args.checked;");

              st.AppendLine("if (checked) {");
              st.AppendLine(value.Str());
              st.AppendLine("}");
              
              st.AppendLine("});");

              st.Send(HtmlHelper, "script");

              return this;
          }

          public RadioButton Create()
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

               if (!String.IsNullOrEmpty(_value))
               {
                    htm.InnerHtml = _value;
               }


               HtmlHelper.ViewContext.Writer.WriteLine(htm.ToString(TagRenderMode.Normal));

               var script1 = string.Format(
                         @"$(""#{0}"").jqxRadioButton({1});" + Environment.NewLine,
                         id,
                         op.Json());

               script1.Send(HtmlHelper, "script");

               var script2 = string.Format(
                         @"$(""#{0}"").css({1});" + Environment.NewLine,
                         id,
                         css.Json());

               script2.Send(HtmlHelper, "script");


               return this;
          }

          public MvcHtmlString GetScript()
          {
               var script = string.Format(
                         @"$(""#{0}"").jqxRadioButton({1});" + Environment.NewLine,
                         id,
                         op.Json());

               var script2 = string.Format(
                         @"$(""#{0}"").css({1});" + Environment.NewLine,
                         id,
                         css.Json());

               return new MvcHtmlString(script + script2);
          }

          public void ToScript()
          {
              GetScript().Send(HtmlHelper, "script");
          }

          public RadioButton PaddingBottom(string value, bool quotes = true)
          {
               css.paddingBottom = value.Quotes(quotes);
               return this;
          }


          public RadioButton Theme(string value, bool quotes = true)
          {
              op.theme = value.Quotes(quotes);
              return this;
          }

          public RadioButton Width(string value, bool quotes = true)
          {
               op.width = value.Quotes(quotes);
               return this;
          }

          public RadioButton Height(string value, bool quotes = true)
          {
              op.height = value.Quotes(quotes);
              return this;
          }

          public RadioButton Style(string value, bool quotes = true)
          {
              this.style = value.Quotes(quotes);
              return this;
          }

          public RadioButton Class(string value)
          {
              this._class = value;
              return this;
          }

          public RadioButton Value(string value)
          {
               this._value = value;
               return this;
          }


          public RadioButton Checked(bool value)
          {
               op._checked = value.ToString().ToLower();
               return this;
          }

          public partial class Settings
          {
               [JsonProperty("checked")]
               [JsonConverter(typeof(PlainJson))]
               public string _checked;

               [JsonConverter(typeof(PlainJson))]
               public string theme { get; set; }

               [JsonConverter(typeof(PlainJson))]
               public string width { get; set; }

               [JsonConverter(typeof(PlainJson))]
               public string height { get; set; }


          }

          public partial class Css
          {
               [JsonProperty("padding-bottom")]
               [JsonConverter(typeof(PlainJson))]
               public string paddingBottom { get; set; }

          }

     }

     public static class jqxRadioButton
     {
         public static IRadioButton JqxRadioButton(this HtmlHelper helper, string id)
         {
             return new RadioButton(helper, id);
          }
     }
}