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
using WB_CFDI_V1.TagHelpers;
using Calendar = System.Globalization.Calendar;
using System.Web.Mvc.Html;

namespace WB_CFDI_V1.Vistas
{
    public partial class Pag9 : Page
    {


        public string Reporte1
        {
            get;
            set;
        }

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


            Reporte1 = TemplateHelper.RenderPartialToString("~/Vistas/Pag9/Reporte1.ascx", "");
        }

        protected static string getFecha1()
        {
            //return DateTime.Now.AddDays(-DateTime.Now.Day + 1).ToString("yyyy,MM,dd");
            return new DateTime(2022,1,1).ToString("yyyy,MM,dd");
        }

        protected static string getFecha2()
        {
            return DateTime.Now.AddDays(-1).ToString("yyyy,MM,dd");
        }



        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string GetEstatus()
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            try
            {

                using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                {
                    var rs = con.Query<CFDI_ESTATUS>("SELECT * FROM CFDI_ESTATUS WITH (NOLOCK) ORDER BY ID").ToList();
                    rs.Insert(0, new CFDI_ESTATUS{ID = 0,ESTATUS = "Todas"});
                    return JsonConvert.SerializeObject(rs);
                }
            }
            catch
            {
                return JsonConvert.SerializeObject(new ArrayList());
            }
        }


        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string GetRFCs()
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            try
            {

                using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                {
                    var rs = con.Query<CFDI_XML>("SELECT RFC_EMISOR, MAX(RAZON_SOCIAL_EMISOR) RAZON_SOCIAL_EMISOR FROM CFDI_XML_PSI WITH (NOLOCK) GROUP BY RFC_EMISOR").ToList();
                    //rs.Insert(0, new CFDI_XML { ID = 0, RFC_EMISOR = "Todas" });
                    return JsonConvert.SerializeObject(rs);
                }
            }
            catch
            {
                return JsonConvert.SerializeObject(new ArrayList());
            }
        }

/*        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string GetRegistros(Req req)
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
                      xml.ID,
                      xml.USER_ID,
                      xml.CONTRA_RECIBO_ID,
                      xml.UUID,
                      xml.XML,
                      xml.RAZON_SOCIAL_EMISOR,
                      xml.RFC_EMISOR,
                      xml.TOTAL,
                      xml.SUBTOTAL,
                      xml.IMPUESTOS,
                      xml.VERSION,
                      xml.FECHA_RECEPCION,
                      xml.FECHA_FACTURA,
                      xml.SERIE,
                      xml.FOLIO,
                      xml.NOMBRE_ARCHIVO,
                      es.ESTATUS AS ESTATUS2,
                      xml.TIPO_COMPROBANTE
                    FROM dbo.CFDI_XML_{0} xml
                    INNER JOIN dbo.CFDI_ESTATUS es ON xml.ESTATUS = es.ID
                    WHERE FECHA_RECEPCION
                    BETWEEN CAST((@fecha1 + ' 00:00:00') AS DATETIME) AND CAST((@fecha2 + ' 23:59:59') AS DATETIME)
                    AND xml.USER_ID = @USER_ID
                    AND (@ESTATUS = 0 OR xml.ESTATUS = @ESTATUS)
                    AND UPPER(xml.FOLIO) LIKE '%' + UPPER(@FOLIO) + '%'
                    AND UPPER(xml.SERIE) LIKE '%' + UPPER(@SERIE) + '%'
                    AND (@CONTR = '' OR UPPER(xml.CONTRA_RECIBO_ID) = @CONTR)
                    AND (@A IS NULL OR @A = '1' AND NOT xml.CONTRA_RECIBO_ID IS NULL
                                    OR @A = '2' AND xml.CONTRA_RECIBO_ID IS NULL)
                    ORDER BY xml.CONTRA_RECIBO_ID DESC".XML_BD();
                    
                    req.USER_ID = USER_ID;

                    if (req.ESTATUS == 4)
                    {
                        //req.CONTR = "";
                        req.A = "1";
                        req.ESTATUS = 1;
                    }

                    if (req.ESTATUS == 5)
                    {
                        req.CONTR = "";
                        req.A = "2";
                        req.ESTATUS = 1;
                    }

                    var rs = con.Query<CFDI_XML>(sql, req);

                    foreach (var xml in rs)
                    {
                        if (xml.TIPO_COMPROBANTE != null && (xml.TIPO_COMPROBANTE.ToLower() == "e" || xml.TIPO_COMPROBANTE.ToLower() == "egreso"))
                        {
                            if (xml.TOTAL != null) xml.TOTAL = xml.TOTAL * -1;
                            if (xml.SUBTOTAL != null) xml.SUBTOTAL = xml.SUBTOTAL * -1;
                            if (xml.IMPUESTOS != null) xml.IMPUESTOS = xml.IMPUESTOS * -1;
                        }
                    }

                    return JsonConvert.SerializeObject(rs.Select(s => new
                    {
                        s.ID,
                        s.FECHA_FACTURA,
                        s.FECHA_RECEPCION,
                        s.VERSION,
                        s.CONTRA_RECIBO_ID,
                        s.RFC_EMISOR,
                        s.TOTAL,
                        s.UUID,
                        s.SERIE,
                        s.FOLIO,
                        ESTATUS = s.ESTATUS2
                    }).ToList());
                }
            }
            catch
            {
                return JsonConvert.SerializeObject(new ArrayList());
            }
        }*/


        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string GetRegistros(Req req)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            req.FOLIO = req.FOLIO.Trim();
            req.SERIE = req.SERIE.Trim();

            try
            {

                using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                {
                    string sql =
                        @"
                    SELECT
                      xml.ID,
                      xml.USER_ID,
                      xml.CONTRA_RECIBO_ID,
                      xml.UUID,
                      xml.XML,
                      xml.RAZON_SOCIAL_EMISOR,
                      xml.RFC_EMISOR,
                      xml.TOTAL,
                      xml.SUBTOTAL,
                      xml.IMPUESTOS,
                      xml.FECHA_RECEPCION,
                      xml.FECHA_FACTURA,
                      xml.SERIE,
                      xml.FOLIO,
                      xml.TIENDA,
                      xml.NO_COMPRA,
                      xml.FECHA_COMPRA,
                      --xml.REVISADO,
                      xml.NOMBRE_ARCHIVO,
                      xml.ESTATUS,
                      es.ESTATUS AS ESTATUS2,
                      xml.TIPO_COMPROBANTE,
                      xml.NUMEFECTO,
                      xml.FECHA_SALDADO
                    FROM dbo.CFDI_XML_PSI xml WITH (NOLOCK)
                    INNER JOIN dbo.CFDI_ESTATUS es WITH (NOLOCK) ON xml.ESTATUS = es.ID
                    WHERE 

                    (@OPT = 0 AND FECHA_RECEPCION BETWEEN CAST((@fecha1 + ' 00:00:00') AS DATETIME) AND CAST((@fecha2 + ' 23:59:59') AS DATETIME)
                    OR
                    @OPT = 1 AND FECHA_SALDADO BETWEEN CAST((@fecha1 + ' 00:00:00') AS DATETIME) AND CAST((@fecha2 + ' 23:59:59') AS DATETIME))

                    --AND xml.USER_ID = @USER_ID
                    AND (@RFC_EMISOR IS NULL OR xml.RFC_EMISOR = @RFC_EMISOR)
                    AND (@ESTATUS = 0 OR xml.ESTATUS = @ESTATUS)
                    AND (@FOLIO = '' OR UPPER(xml.FOLIO) LIKE '%' + UPPER(@FOLIO) + '%')
                    AND (@SERIE = '' OR UPPER(xml.SERIE) LIKE '%' + UPPER(@SERIE) + '%')
                    AND (@CONTR = '' OR UPPER(xml.CONTRA_RECIBO_ID) = @CONTR)
                    AND (@A IS NULL OR @A = '1' AND NOT xml.CONTRA_RECIBO_ID IS NULL
                                    OR @A = '2' AND xml.CONTRA_RECIBO_ID IS NULL)
                    ORDER BY xml.CONTRA_RECIBO_ID DESC";

                    req.USER_ID = USER_ID;


                    if (req.ESTATUS == 4)
                    {
                        //req.CONTR = "";
                        req.A = "1";
                        req.ESTATUS = 0;
                    }

                    if (req.ESTATUS == 5)
                    {
                        req.CONTR = "";
                        req.A = "2";
                        req.ESTATUS = 0;
                    }

                    var rs = con.Query<CFDI_XML>(sql, req);

                    foreach (var xml in rs)
                    {
                        if (xml.TIPO_COMPROBANTE != null && (xml.TIPO_COMPROBANTE.ToLower() == "e" || xml.TIPO_COMPROBANTE.ToLower() == "egreso"))
                        {
                            if (xml.TOTAL != null) xml.TOTAL = xml.TOTAL * -1;
                            if (xml.SUBTOTAL != null) xml.SUBTOTAL = xml.SUBTOTAL * -1;
                            if (xml.IMPUESTOS != null) xml.IMPUESTOS = xml.IMPUESTOS * -1;
                        }
                    }


                    return JsonConvert.SerializeObject(rs.Select(
                        s => new
                        {
                            s.ID,
                            s.CONTRA_RECIBO_ID,
                            //s.RFC_EMISOR,
                            s.FECHA_FACTURA,
                            s.FECHA_RECEPCION,
                            //s.VERSION,
                            s.RAZON_SOCIAL_EMISOR,
                            s.TOTAL,
                            s.SUBTOTAL,
                            s.IMPUESTOS,
                            s.IVA,
                            s.IEPS,
                            s.UUID,
                            s.SERIE,
                            s.FOLIO,
                            s.TIENDA,
                            s.NO_COMPRA,
                            s.FECHA_COMPRA,
                            //REVISADO = s.REVISADO ? "OK" : "",
                            ESTATUS = s.ESTATUS2,
                            ESTATUS_ID = s.ESTATUS,
                            s.NUMEFECTO,
                            s.FECHA_SALDADO
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
            public string RFC_EMISOR { get; set; }
            public string FOLIO { get; set; }
            public string SERIE { get; set; }
            public string CONTR { get; set; }
            public string A { get; set; }
            public int OPT { get; set; }
        }

    }

}