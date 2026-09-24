using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Script.Services;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using AngleSharp;

using Dapper;
using Newtonsoft.Json;
using WB_CFDI_V1.Properties;
using Calendar = System.Globalization.Calendar;

namespace WB_CFDI_V1.Vistas
{
    public partial class Pag5 : Page
    {
        private static int USER_ID = -1;
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

            //string bd = (string)HttpContext.Current.Session["bd"];

            string USER_NAME = "";
            HttpContext context = HttpContext.Current;
            if (context.User != null && context.User.Identity.IsAuthenticated)
            {
                FormsIdentity id = (FormsIdentity)context.User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;
                //role = ticket.UserData;
                USER_NAME = ticket.Name;
            }

            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {

                try
                {
                    con.Open();
                    USER_ID = con.Query<int>(
                        "SELECT ID FROM CFDI_ACCOUNT WITH (NOLOCK) WHERE USER_NAME = @USER_NAME",
                        new { USER_NAME = USER_NAME }).SingleOrDefault();

                    //***************************************************
                }
                catch (Exception ex)
                {
                    USER_ID = -1;
                }
                finally
                {
                    if (con != null)
                    {
                        con.Close();
                    }
                }
            }
        }



        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string GetFlag(CFDI_ACCOUNT ac)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            try
            {

                using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                {
                    string sql1 = @"SELECT TOP 1 USER_ID FROM CFDI_XML_PSI WITH (NOLOCK) WHERE USER_ID = @USER_ID";

                    var rs1 = con.Query<CFDI_XML>(sql1, new { USER_ID = ac.ID}).FirstOrDefault();
                    var rs2 = con.Query<CFDI_ACCOUNT>("SELECT * FROM CFDI_ACCOUNT WITH (NOLOCK) WHERE ID = @USER_ID", new { USER_ID = ac.ID }).FirstOrDefault();

                    if (rs1 == null)
                    {
                        return JsonConvert.SerializeObject(new
                        {
                            f = "0", 
                            rs2.MOSTRAR_BLOQUEO, 
                            rs2.MOSTRAR_BLOQUEO2,
                            rs2.IGNORAR_BLOQUEO2,
                            rs2.MOSTRAR_RECHAZAR, 
                            rs2.MOSTRAR_DESVALIDAR, 
                            rs2.MOSTRAR_FECHA_VALIDACION,
                            rs2.MOSTRAR_ABRIR_CARTA_PAGO,
                            rs2.MOSTRAR_FACTURA_DUPLICADOS,
                        });
                    }

                    return JsonConvert.SerializeObject(new
                    {
                        f = "1", 
                        rs2.MOSTRAR_BLOQUEO, 
                        rs2.MOSTRAR_BLOQUEO2,
                        rs2.IGNORAR_BLOQUEO2,
                        rs2.MOSTRAR_RECHAZAR,
                        rs2.MOSTRAR_DESVALIDAR,
                        rs2.MOSTRAR_FECHA_VALIDACION,
                        rs2.MOSTRAR_ABRIR_CARTA_PAGO,
                        rs2.MOSTRAR_FACTURA_DUPLICADOS,
                    });
                }
            }
            catch
            {
                return JsonConvert.SerializeObject(
                    new {f = "-1", MOSTRAR_BLOQUEO = "false", MOSTRAR_RECHAZAR = "false"});
            }
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string Save(CFDI_ACCOUNT ac)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            try
            {
                if (ac.ROLE != 2)
                {
                    ac.MOSTRAR_BLOQUEO = false;
                    ac.MOSTRAR_RECHAZAR = false;
                }


                if (ac.ROLE != 1)
                {
                    ac.DIF_IMPUESTO = "0";
                    ac.DIF_TOTAL = "0";
                }

                using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                {
                    string sql = @"
                        UPDATE CFDI_ACCOUNT WITH (Rowlock) SET 
                            USER_NAME                  = @USER_NAME,
                            PASSWORD                   = @PASSWORD,
                            ROLE                       = @ROLE,
                            MOSTRAR_BLOQUEO            = @MOSTRAR_BLOQUEO,
                            MOSTRAR_BLOQUEO2           = @MOSTRAR_BLOQUEO2,
                            IGNORAR_BLOQUEO2           = @IGNORAR_BLOQUEO2,
                            MOSTRAR_RECHAZAR           = @MOSTRAR_RECHAZAR,
                            MOSTRAR_DESVALIDAR         = @MOSTRAR_DESVALIDAR,
                            DIF_TOTAL                  = @DIF_TOTAL,
                            DIF_IMPUESTO               = @DIF_IMPUESTO,
                            MOSTRAR_FECHA_VALIDACION   = @MOSTRAR_FECHA_VALIDACION,
                            MOSTRAR_ABRIR_CARTA_PAGO   = @MOSTRAR_ABRIR_CARTA_PAGO,
                            MOSTRAR_FACTURA_DUPLICADOS = @MOSTRAR_FACTURA_DUPLICADOS
                        WHERE ID = @ID";

                    var rs = con.Execute(sql,ac);
                    return "0";
                }
            }
            catch
            {
                return "-1";
            }
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string Eliminar(CFDI_ACCOUNT ac)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            var msg = new Dictionary<string, string>();

            try
            {

                using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                {
                    string sql = @"
                        DELETE FROM CFDI_ACCOUNT WITH (Rowlock)
                        WHERE ID = @ID";

                    var rs = con.Execute(
                        sql,
                        new { ID = ac.ID });


                    msg.Add("code", "0");
                    msg.Add("msg", "");

                    return JsonConvert.SerializeObject(msg);
                }
            }
            catch(Exception ex)
            {
                msg.Add("code", "-1");
                msg.Add("msg", "Error: " + ex.Message);

                return JsonConvert.SerializeObject(msg);
            }
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string Agregar(CFDI_ACCOUNT ac)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            var msg = new Dictionary<string, string>();

            try
            {

                if (ac.ROLE != 2)
                {
                    ac.MOSTRAR_BLOQUEO = false;
                    ac.MOSTRAR_RECHAZAR = false;
                    ac.MOSTRAR_DESVALIDAR = false;
                }

                ArrayList arr = new ArrayList();

                if (ac.USER_NAME.Trim() == "")
                {
                    arr.Add("<strong>USUARIO</strong>");
                }

                if (ac.PASSWORD.Trim() == "")
                {
                    arr.Add("<strong>PASSWORD</strong>");
                }

                if (arr.Count > 0)
                {
                    msg.Add("code", "-1");
                    msg.Add("msg", "<div style='text-align: left;'>Ingrese el valor del campo:<br />"
                                   + string.Join(", ", arr.ToArray()) + "</div>");

                    return JsonConvert.SerializeObject(msg);
                }


                using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                {


                    string sql1 = @"SELECT ID FROM CFDI_ACCOUNT WITH (NOLOCK) WHERE USER_NAME = @USER_NAME";

                    var rs1 = con.Query<CFDI_ACCOUNT>(sql1, new { USER_NAME = ac.USER_NAME }).SingleOrDefault();

                    if (rs1 != null)
                    {
                        msg.Add("code", "0");
                        msg.Add("msg", "No se pudo agregar, la cuenta: <strong>" + ac.USER_NAME + "</strong> ya existe.");

                        return JsonConvert.SerializeObject(msg);
                    }


                    string sql = @"

                    DECLARE @XID INT;
                    SET @XID = (SELECT MAX(p.ID) FROM CFDI_ACCOUNT p WITH (NOLOCK));
                    SET @XID = ISNULL( @XID , 0 ) + 1;

                    INSERT CFDI_ACCOUNT WITH (Rowlock) (  ID,  USER_NAME,  PASSWORD,  ROLE,  MOSTRAR_BLOQUEO,  MOSTRAR_BLOQUEO2,  IGNORAR_BLOQUEO2,  MOSTRAR_RECHAZAR,  MOSTRAR_DESVALIDAR,  DIF_TOTAL,  DIF_IMPUESTO,  MOSTRAR_FECHA_VALIDACION,  MOSTRAR_ABRIR_CARTA_PAGO,  MOSTRAR_FACTURA_DUPLICADOS)
                                 VALUES (@XID, @USER_NAME, @PASSWORD, @ROLE, @MOSTRAR_BLOQUEO, @MOSTRAR_BLOQUEO2, @IGNORAR_BLOQUEO2, @MOSTRAR_RECHAZAR, @MOSTRAR_DESVALIDAR, @DIF_TOTAL, @DIF_IMPUESTO, @MOSTRAR_FECHA_VALIDACION, @MOSTRAR_ABRIR_CARTA_PAGO, @MOSTRAR_FACTURA_DUPLICADOS)";


                    var rs = con.Execute(sql,ac);

                    /*var rs = con.Execute(
                        sql,
                        new { ID = ac.ID, USER_NAME = ac.USER_NAME, PASSWORD = ac.PASSWORD, ROLE = ac.ROLE });*/


                    msg.Add("code", "0");
                    msg.Add("msg", "Registro agregado correctamente.");

                    return JsonConvert.SerializeObject(msg);
                }
            }
            catch(Exception ex)
            {
                
                msg.Add("code", "-1");
                msg.Add("msg", "Error: " + ex.Message);

                return JsonConvert.SerializeObject(msg);
            }
        }


        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string SaveDifGlobal(string total, string impuesto)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            try
            {
                using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                {
                    string sql = @"
                        UPDATE TOLERANCIA_FAC_ALB WITH (Rowlock) SET 
                            DIF_TOTAL    = @DIF_TOTAL,
                            DIF_IMPUESTO = @DIF_IMPUESTO
                        WHERE ID = 1";

                    var rs = con.Execute(sql, new { DIF_TOTAL = total, DIF_IMPUESTO = impuesto });
                    return "0";
                }
            }
            catch
            {
                return "-1";
            }
        }


        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string GetDifGlobal()
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            try
            {

                using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                {
                    var rs = con.Query<TOLERANCIA_FAC_ALB>("SELECT * FROM TOLERANCIA_FAC_ALB WITH (NOLOCK)").FirstOrDefault();

                    return JsonConvert.SerializeObject(rs);
                }
            }
            catch
            {
                return JsonConvert.SerializeObject(new ArrayList());
            }
        }


        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string GetRole(bool Todos)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            try
            {

                using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                {
                    var rs = con.Query<CFDI_ROLE>("SELECT * FROM CFDI_ROLE WITH (NOLOCK) ORDER BY ID").ToList();

                    if (Todos)
                    {
                        rs.Insert(0, new CFDI_ROLE {ID = null, ROLE_NAME = "Todos"});
                    }

                    return JsonConvert.SerializeObject(rs);
                }
            }
            catch
            {
                return JsonConvert.SerializeObject(new ArrayList());
            }
        }


        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string GetRegistros(string role, string usuario)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;
            
            try
            {

                using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                {
                    string sql =
                        @"
                    SELECT
                      a.ID,
                      a.USER_NAME,
                      a.PASSWORD,
                      a.EMAIL,
                      a.DIF_TOTAL,
                      a.DIF_IMPUESTO,
                      a.ROLE,
                      b.ROLE_NAME
                    FROM dbo.CFDI_ACCOUNT a WITH (NOLOCK)
                    INNER JOIN dbo.CFDI_ROLE b WITH (NOLOCK) ON a.ROLE = b.ID
                    WHERE 
                        (@USER_NAME = '' OR UPPER(a.USER_NAME) LIKE '%' + UPPER(@USER_NAME) + '%')
                    AND
                        (@ROLE IS NULL OR a.ROLE = @ROLE)
                    ";




                    var rs = con.Query<CFDI_ACCOUNT>(sql, new {ROLE = role, USER_NAME = usuario});


                    return JsonConvert.SerializeObject(rs.Select(s => new
                    {
                        s.ID,
                        s.USER_NAME,
                        s.PASSWORD,
                        s.ROLE,
                        s.ROLE_NAME,
                        s.EMAIL,
                        s.DIF_TOTAL,
                        s.DIF_IMPUESTO
                    }).ToList());
                }
            }
            catch
            {
                return JsonConvert.SerializeObject(new ArrayList());
            }
        }


        public class Req
        {
            public DateTime FECHA1 { get; set; }
            public DateTime FECHA2 { get; set; }
            public int ESTATUS { get; set; }
            public int USER_ID { get; set; }
            public string FOLIO { get; set; }
            public string SERIE { get; set; }
            public string CONTR { get; set; }
            public string A { get; set; }
        }

    }


}