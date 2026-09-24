using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using WB_CFDI_V1;
using Newtonsoft.Json;

namespace System.Web.Mvc
{
    public interface ICheckBox
     {
          CheckBox Create();
          MvcHtmlString GetScript();
          void ToScript();
          CheckBox Theme(string value, bool quotes = true);
          CheckBox Width(string value, bool quotes = true);
          CheckBox Height(string value, bool quotes = true);
          CheckBox Style(string value, bool quotes = true);
          CheckBox Class(string value);
          CheckBox Checked(bool value);
          CheckBox Value(string value);
          CheckBox PaddingBottom(string value, bool quotes = true);
     }

     public class CheckBox : ICheckBox
     {
          private HtmlHelper HtmlHelper { get; set; }
          private String   id { get; set; }
          private Settings op = new Settings();
          private Css css = new Css();

          private string   style  { get; set; }
          private string   _class { get; set; }
          private string   _value { get; set; }

          public CheckBox()
          {
          }

          public CheckBox(HtmlHelper helper, string id)
          {
               this.HtmlHelper = helper;
               this.id         = id;
          }

          public CheckBox Create()
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
                         @"$(""#{0}"").jqxCheckBox({1});" + Environment.NewLine,
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
                         @"$(""#{0}"").jqxCheckBox({1});" + Environment.NewLine,
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

          public CheckBox PaddingBottom(string value, bool quotes = true)
          {
               css.paddingBottom = value.Quotes(quotes);
               return this;
          }


          public CheckBox Theme(string value, bool quotes = true)
          {
              op.theme = value.Quotes(quotes);
              return this;
          }

          public CheckBox Width(string value, bool quotes = true)
          {
               op.width = value.Quotes(quotes);
               return this;
          }

          public CheckBox Height(string value, bool quotes = true)
          {
              op.height = value.Quotes(quotes);
              return this;
          }

          public CheckBox Style(string value, bool quotes = true)
          {
              this.style = value.Quotes(quotes);
              return this;
          }

          public CheckBox Class(string value)
          {
              this._class = value;
              return this;
          }

          public CheckBox Value(string value)
          {
               this._value = value;
               return this;
          }


          public CheckBox Checked(bool value)
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

     public static class jqxCheckBox
     {
         public static ICheckBox JqxCheckBox(this HtmlHelper helper, string id)
         {
             return new CheckBox(helper, id);
          }
     }
}