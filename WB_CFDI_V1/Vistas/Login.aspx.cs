using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using Dapper;
using Newtonsoft.Json;
using WB_CFDI_V1.Properties;

namespace WB_CFDI_V1.Vistas
{
    public partial class Login : Page
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


            var u = Request.Form["_u"];
            var p = Request.Form["_p"];
            var param1 = Request.Form["_param1"];

            if (u != null && p != null)
            {
                SetEmpresaID(param1);


                CFDI_ACCOUNT acc = null;
                try
                {
                    using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI(param1)))
                    {
                        string sql = @"
                                    SELECT *
                                    FROM CFDI_ACCOUNT u WITH (NOLOCK)
                                    WHERE
                                      u.user_name = @UserName
                                      AND u.Password = @Password
                                    ";

                        acc = con.Query<CFDI_ACCOUNT>(sql, new { UserName = u, Password = p })
                            .SingleOrDefault();

                    }
                }
                catch
                {
                }



                if (acc != null)
                {

                    Extensions.SetEmpID(param1);

                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        1,
                        acc.USER_NAME,
                        DateTime.Now,
                        DateTime.Now.AddMinutes(80),
                        false,
                        acc.ROLE.Str(),
                        FormsAuthentication.FormsCookiePath);
                    string hash = FormsAuthentication.Encrypt(ticket);
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);

                    if (ticket.IsPersistent)
                    {
                        cookie.Expires = ticket.Expiration;
                    }
                    Response.Cookies.Add(cookie);

                    if (acc.ROLE == 1)
                    {
                        Response.Redirect("~/Vistas/Pag1.aspx");
                    }

                    if (acc.ROLE == 2 || acc.ROLE == 5)
                    {
                        Response.Redirect("~/Vistas/Pag2.aspx");
                    }

                    if (acc.ROLE == 3)
                    {
                        Response.Redirect("~/Vistas/Pag4.aspx");
                    }

                    if (acc.ROLE == 4)
                    {
                        Response.Redirect("~/Vistas/Pag5.aspx");
                    }

                    Response.Redirect(FormsAuthentication.GetRedirectUrl(acc.USER_NAME, false));


                    //string s=FormsAuthentication.HashPasswordForStoringInConfigFile("JOSE_F", "md5");

                    //FormsAuthentication.RedirectFromLoginPage(acc.USER_NAME, true);
                }
                else
                {
                    lbMsg.Text = "Usuario y o contraseña incorrecto";
                }


            }



            var empID = Extensions.GetEmpresaID();

            var path = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/" + GetPath(Extensions.cEmpresa(empID));

            if( path != GetPath() )
            {
                Response.Redirect(path + "/Vistas/");
            }

        }


        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod(EnableSession = true)]
        public static void SetEmpresaID(string ID)
        {
            Extensions.SetEmpresaID(ID);
        }


        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod(EnableSession = true)]
        public static string GetEmpresaID()
        {
            return Extensions.GetEmpresaID();
        }


        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string GetEmpresas()
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            try
            {
                var rs = Extensions.cEmpresas().Select(
                        s => new {
                                     s.ID,
                                     DESCRIPCION = s.ALIAS + " - " + s.TIPO_VALIDADOR2,
                                     PATH = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/" + GetPath(s)
                                 });

                return JsonConvert.SerializeObject(rs);

            }
            catch
            {
                return JsonConvert.SerializeObject(new ArrayList());
            }
        }

        public static string GetPath(CFDI_EMPRESA tipo)
        {
            switch (tipo.TIPO_VALIDADOR)
            {
                case 1:
                    return Settings.Default.PATH1;
                case 2:
                    return Settings.Default.PATH2;
            }

            /*
            if (tipo.TIPO_VALIDADOR == 1 && tipo.ALIAS.ToLower() == "psi")
            {
                return Settings.Default.PATH1;
            }

            if (tipo.TIPO_VALIDADOR == 2 && tipo.ALIAS.ToLower() == "psi")
            {
                return Settings.Default.PATH2;
            }

            if (tipo.TIPO_VALIDADOR == 2 && tipo.ALIAS.ToLower() == "corona")
            {
                return Settings.Default.PATH2;
            }

            if (tipo.TIPO_VALIDADOR == 1 && tipo.ALIAS.ToLower() == "belen")
            {
                return Settings.Default.PATH1;
            }


            //---------------------------------------------------------------
            if (tipo.TIPO_VALIDADOR == 1 && tipo.ALIAS.ToLower() == "pso")
            {
                return Settings.Default.PATH3;
            }

            if (tipo.TIPO_VALIDADOR == 2 && tipo.ALIAS.ToLower() == "pso")
            {
                return Settings.Default.PATH4;
            }
            */
            return "";
        }

        protected void btEntrar_Click(object sender, EventArgs e)
        {
/*            string _connectionString =
        @"Data Source=(LocalDB)\v11.0;Initial Catalog=dbAccount.mdf;"
        + @"Integrated Security=True;AttachDbFilename=|DataDirectory|\dbAccount.mdf;User Instance=True;
            Async=true;Persist Security Info=True;User ID=sa;Password=adminbd";*/

            string bd = (string)HttpContext.Current.Session["bd"];

            

            CFDI_ACCOUNT acc = null;
            try
            {
                using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI(txEmpresa.Value)))
                {
                    string sql = @"
                                    SELECT *
                                    FROM CFDI_ACCOUNT u WITH (NOLOCK)
                                    WHERE
                                      u.user_name = @UserName
                                      AND u.Password = @Password
                                    ";

                    acc = con.Query<CFDI_ACCOUNT>(sql, new { UserName = txUser.Text, Password = txPass.Text })
                        .SingleOrDefault();

                }
            } catch
            {
            }



            if( acc!=null )
            {

                Extensions.SetEmpID(txEmpresa.Value);
                
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,
                    acc.USER_NAME,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(80),
                    false,
                    acc.ROLE.Str(),
                    FormsAuthentication.FormsCookiePath);
                string hash = FormsAuthentication.Encrypt(ticket);
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);

                if (ticket.IsPersistent)
                {
                    cookie.Expires = ticket.Expiration;
                }
                Response.Cookies.Add(cookie);

                if (acc.ROLE == 1)
                {
                    Response.Redirect("~/Vistas/Pag1.aspx");
                }

                if (acc.ROLE == 2 || acc.ROLE == 5)
                {
                    Response.Redirect("~/Vistas/Pag2.aspx");
                }

                if (acc.ROLE == 3)
                {
                    Response.Redirect("~/Vistas/Pag4.aspx");
                }

                if (acc.ROLE == 4)
                {
                    Response.Redirect("~/Vistas/Pag5.aspx");
                }
                
                Response.Redirect(FormsAuthentication.GetRedirectUrl(acc.USER_NAME, false));
                

                //string s=FormsAuthentication.HashPasswordForStoringInConfigFile("JOSE_F", "md5");

                //FormsAuthentication.RedirectFromLoginPage(acc.USER_NAME, true);
            }
            else
            {
                lbMsg.Text = "Usuario y o contraseña incorrecto";
            }
        }

        protected string GetPath()
        {
            return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/" + Settings.Default.PATH1;
        }
    }
}