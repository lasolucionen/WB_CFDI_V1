using System;
using System.Web;
using System.Web.Security;

namespace WB_CFDI_V1.Vistas
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetExpires(DateTime.UtcNow.AddYears(-2));
            Response.Cache.SetNoStore();
            Response.Cache.SetValidUntilExpires(false);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ExpiresAbsolute = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0));
            Response.Expires         = 0;
            Response.CacheControl    = "no-cache";
            Response.AppendHeader("Pragma", "no-cache");

            if (Context.User != null && Context.User.Identity.IsAuthenticated)
            {
                FormsIdentity             id     = (FormsIdentity)Context.User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;
                string                    role   = ticket.UserData;
                //USER_NAME = ticket.Name;

                if (role == "1")
                {
                    Response.Redirect("~/Vistas/Pag1.aspx");
                }

                if (role == "2")
                {
                    Response.Redirect("~/Vistas/Pag2.aspx");
                }

                if (role == "3")
                {
                    Response.Redirect("~/Vistas/Pag4.aspx");
                }

                if (role == "4")
                {
                    Response.Redirect("~/Vistas/Pag5.aspx");
                }
            }

            Response.Redirect("~/Vistas/Login.aspx");


        }
    }
}