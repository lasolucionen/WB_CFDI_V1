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
     public interface IInput
     {
          Input KeyPress(MvcHtmlString value);
          Input ButtonClick(string value);
          Input Create();
          MvcHtmlString GetScript();
          MvcHtmlString Clear();
          MvcHtmlString Trim();
          MvcHtmlString Val();
          MvcHtmlString Val(string value);
          void ToScript();
          Input Theme(string value, bool quotes = true);
          Input Width(string value, bool quotes = true);
          Input Height(string value, bool quotes = true);
          Input Style(string value);
          Input Class(string value);
          Input AutoComplete(string value);
          Input ReadOnly(string value);
          Input Type(string value);
          Input Value(string value);
          Input TextAlign(string value);
          Input Color(string value);
     }

     public class Input : IInput
     {
          private HtmlHelper HtmlHelper { get; set; }
          private String     id         { get; set; }
          private Settings   op = new Settings();
          private string style          { get; set; }
          private string _type          { get; set; }
          private string _class         { get; set; }
          private string _autoComplete  { get; set; }
          private string _readOnly      { get; set; }
          private string _value         { get; set; }

          public Input()
          {
          }

          public Input(HtmlHelper helper, string id)
          {
               this.HtmlHelper = helper;
               this.id         = id;
               op.height       = "'27px'";
               _type           = "text";
          }

          public Input KeyPress(MvcHtmlString value)
          {
               var script = string.Format(
                         @"$(""#{0}"").keypress(function(e){{ 
                         {1}
                         }});"
                         + Environment.NewLine,
                         id,
                         value.Str());

               script.Send(HtmlHelper, "script");
               return this;
          }

          public Input ButtonClick(string value)
          {

               var script = string.Format(
                         @"$(""#{0}"").keypress(function(e){{ 
                              if (e.keyCode == 13) {{
                                  $(""#{1}"").click()
                              }}
                         }});"
                         + Environment.NewLine,
                         id,
                         value);


               script.Send(HtmlHelper, "script");
               return this;
          }

          public Input TextAlign(string value)
          {
               //textAlign = "text-align:" + value + ";";

               this.style += "text-align:" + value + ";";
               return this;
          }

          public Input Color(string value)
          {
               //color = "color:" + value + ";";
              this.style += "color:" + value + ";";
               return this;
          }

          public Input Create()
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

               if (!String.IsNullOrEmpty(_autoComplete))
               {
                    htm.MergeAttribute("autocomplete", _autoComplete);
               }

               if (!String.IsNullOrEmpty(_readOnly))
               {
                    htm.MergeAttribute("readonly", _readOnly);
               }


               HtmlHelper.ViewContext.Writer.WriteLine(htm.ToString(TagRenderMode.SelfClosing));

               var script = string.Format(
                         @"$(""#{0}"").jqxInput({1});" + Environment.NewLine,
                         id,
                         op.Json());

               script.Send(HtmlHelper, "script");
               return this;
          }

          public MvcHtmlString Clear()
          {
               var script = string.Format(
                         @"$(""#{0}"").val('');" + Environment.NewLine,
                         id);

               return new MvcHtmlString(script);
          }

          public MvcHtmlString Trim()
          {
               var script = string.Format(
                         @"$(""#{0}"").val().trim();" + Environment.NewLine,
                         id);

               return new MvcHtmlString(script);
          }

          public MvcHtmlString Val()
          {
               var script = string.Format(
                         @"$(""#{0}"").val();" + Environment.NewLine,
                         id);

               return new MvcHtmlString(script);
          }

          public MvcHtmlString Val(string value)
          {
               var script = string.Format(
                         @"$(""#{0}"").val({1});" + Environment.NewLine,
                         id,
                         value);

               return new MvcHtmlString(script);
          }

          public MvcHtmlString GetScript()
          {
               var script = string.Format(
                         @"$(""#{0}"").jqxInput({1});" + Environment.NewLine,
                         id,
                         op.Json());

               return new MvcHtmlString(script);
          }

          public void ToScript()
          {
              GetScript().Send(HtmlHelper, "script");
          }

          public Input Theme(string value, bool quotes = true)
          {
              op.theme = value.Quotes(quotes);
              return this;
          }

          public Input Width(string value, bool quotes = true)
          {
               op.width = value.Quotes(quotes);
               return this;
          }

          public Input Height(string value, bool quotes = true)
          {
              op.height = value.Quotes(quotes);
              return this;
          }

          public Input Style(string value)
          {
              this.style = value;
              return this;
          }

          public Input Type(string value)
          {
              this._type = value;
              return this;
          }

          public Input AutoComplete(string value)
          {
              this._autoComplete = value;
              return this;
          }

          public Input ReadOnly(string value)
          {
              this._readOnly = value;
              return this;
          }

          public Input Value(string value)
          {
              this._value = value;
              return this;
          }

          public Input Class(string value)
          {
              this._class = value;
              return this;
          }

          public partial class Settings
          {
               [JsonConverter(typeof(PlainJson))]
               public string theme { get; set; }

               [JsonConverter(typeof(PlainJson))]
               public string width { get; set; }

               [JsonConverter(typeof(PlainJson))]
               public string height { get; set; }
          }

     }

     public static class jqxInput
     {
          public static IInput JqxInput(this HtmlHelper helper, string id)
          {
               return new Input(helper, id);
          }
     }
}