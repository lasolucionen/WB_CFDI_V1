using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WB_CFDI_V1
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*
            Response.Buffer = true;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1.0);
            Response.Expires = -1;
            Response.CacheControl = "no-cache";
            */

            Response.Cache.SetExpires(DateTime.UtcNow.AddYears(-2));
            Response.Cache.SetNoStore();
            Response.Cache.SetValidUntilExpires(false);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ExpiresAbsolute = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0));
            Response.Expires         = 0;
            Response.CacheControl    = "no-cache";
            Response.AppendHeader("Pragma", "no-cache");

            bool flag = HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated;
            if (flag)
            {
                try
                {
                }
                catch
                {
                }
            }

            /*
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                FormsIdentity id = (FormsIdentity) HttpContext.Current.User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;
                string userData = ticket.UserData;
            }*/

            try
            {
                //String url = HttpContext.Current.Request.Url.Segments[2];
                String url = HttpContext.Current.Request.Url.Segments.SingleOrDefault(s => s.Contains("aspx"));

                if (url == "Pag1.aspx")
                {
                    CargaArchivos.Style.Add("border", "1px solid gray !important");
                    //CargaArchivos.Style.Add("border", "1px solid #fd9595 !important");
                    //CargaArchivos.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#e8e8e8 !important");
                }

                if (url == "Pag2.aspx")
                {
                    Admin.Style.Add("border", "1px solid gray !important");
                    //Admin.Style.Add("border", "1px solid #fd9595 !important");
                    //CargaArchivos.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#e8e8e8 !important");
                }

                if (url == "Pag3.aspx")
                {
                    Contr.Style.Add("border", "1px solid gray !important");
                    //Contr.Style.Add("border", "1px solid #fd9595 !important");
                    //CargaArchivos.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#e8e8e8 !important");
                }

                if (url == "Pag4.aspx")
                {
                    Consulta.Style.Add("border", "1px solid gray !important");
                    //Consulta.Style.Add("border", "1px solid #fd9595 !important");
                    //CargaArchivos.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#e8e8e8 !important");
                }

                if (url == "Pag5.aspx")
                {
                    CUser.Style.Add("border", "1px solid gray !important");
                    //CUser.Style.Add("border", "1px solid #fd9595 !important");
                    //CargaArchivos.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#e8e8e8 !important");
                }

                if (url == "Pag6.aspx")
                {
                    Reporte1.Style.Add("border", "1px solid gray !important");
                    //Reporte1.Style.Add("border", "1px solid #fd9595 !important");
                    //CargaArchivos.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#e8e8e8 !important");
                }

                if (url == "Pag7.aspx")
                {
                    Reporte2.Style.Add("border", "1px solid gray !important");
                    //Reporte2.Style.Add("border", "1px solid #fd9595 !important");
                    //CargaArchivos.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#e8e8e8 !important");
                }

                if (url == "Pag8.aspx")
                {
                    Compl.Style.Add("border", "1px solid gray !important");
                    
                    //Reporte2.Style.Add("border", "1px solid #fd9595 !important");
                    //CargaArchivos.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#e8e8e8 !important");
                }


                if (url == "Pag9.aspx")
                {
                    Reporte3.Style.Add("border", "1px solid gray !important");

                    //Reporte2.Style.Add("border", "1px solid #fd9595 !important");
                    //CargaArchivos.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#e8e8e8 !important");
                }

                var empID = Extensions.GetEmpresaID();
                var it = Extensions.cEmpresas().Find(f => f.ID.ToString() == empID);


                Empresa.InnerHtml = "<span style='font-weight: bold;'>Empresa: </span>" + it.ALIAS + " - " + it.TIPO_VALIDADOR2;
                Empresa.Style.Add("border", "0px solid transparent !important");
                Empresa.Style.Add("background-color", "transparent !important");
                Empresa.Style.Add("color", "black !important");
                Empresa.Style.Add("cursor", "default !important");

            }
            catch (Exception ex)
            {

            }

            CargaArchivos.Visible = false;
            Admin.Visible = false;
            Contr.Visible = false;
            Consulta.Visible = false;
            CUser.Visible = false;
            Empresa.Visible = false;
            Reporte1.Visible = false;
            Reporte2.Visible = false;
            Reporte3.Visible = false;
            Compl.Visible = false;
            Contraseña.Visible = false;

            if (!flag)
            {
                Cerrar.Visible = false;
            }
            else
            {
                Iniciar.Visible = false;
            }

            if (Context.User != null && Context.User.Identity.IsAuthenticated)
            {
                FormsIdentity id = (FormsIdentity)Context.User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;
                string role = ticket.UserData;
                //USER_NAME = ticket.Name;
                if (role == "1")
                {
                    CargaArchivos.Visible = true;
                    Contraseña.Visible = true;
                }

                if (role == "2" || role == "5")
                {
                    Admin.Visible = true;
                    Compl.Visible = true;
                    Contr.Visible = true;
                    Reporte1.Visible = true;
                    Reporte2.Visible = true;
                    Reporte3.Visible = true;
                }

                if (role == "3")
                {
                    Consulta.Visible = true;
                    Contr.Visible = true;
                    Reporte1.Visible = true;
                    Reporte2.Visible = true;
                    //Reporte3.Visible = true;
                }

                if (role == "4")
                {
                    CUser.Visible = true;
                }

                Empresa.Visible = true;
            }
        }

        protected string GetBD()
        {
            HttpCookie myCookie = HttpContext.Current.Request.Cookies["WB_CFDI_V1"];

            string bd = "[BD_PSI]";
            if (myCookie != null && myCookie.Value != null && myCookie.Value != "")
            {
                bd ="[" + myCookie.Value + "]";
            }

            String url = HttpContext.Current.Request.Url.Segments.SingleOrDefault(s => s.Contains("aspx"));
            if (url == "Login.aspx")
            {
                bd = "";
            }

            return bd;
        }
    }
}