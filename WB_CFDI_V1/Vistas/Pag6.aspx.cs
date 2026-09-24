using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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
    public partial class Pag6 : Page
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

        protected static string getFecha1()
        {
            return DateTime.Now.AddDays(-DateTime.Now.Day + 1).ToString("yyyy,MM,dd");
        }





        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string GetProveedores()
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            try
            {

                using (IDbConnection con = new SqlConnection(Extensions.GetBD_ICG()))
                {
                    var sql = @"
                    --LISTA DE PROVEEDORES
                    SELECT CODPROVEEDOR, NOMPROVEEDOR, NIF20 RFC
                    FROM PROVEEDORES WITH(NOLOCK)
                    WHERE DESCATALOGADO ='F'
                    ORDER BY CODPROVEEDOR
                    ";

                    var rs = con.Query<PROVEEDORES2>(sql).ToList();
                    //rs.Insert(0, new CFDI_XML { ID = 0, RFC_EMISOR = "Todas" });
                    return JsonConvert.SerializeObject(rs);
                }
            }
            catch
            {
                return JsonConvert.SerializeObject(new ArrayList());
            }
        }



        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string GetResumenCompras(Req req)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;


            try
            {

                using (IDbConnection con = new SqlConnection(Extensions.GetBD_ICG()))
                {
                    string sql =
                        @"
-- RESUMEN DE COMPRAS POR PERIODO
--DECLARE @CODPROVEEDOR INT = 0
--DECLARE @FECHAINICIAL DATETIME = '01/12/2010'
--DECLARE @FECHAFINAL DATETIME = '31/12/2019'

--SET @FECHAINICIAL = @FECHAINICIAL + '00:00:00'
--SET @FECHAFINAL = @FECHAFINAL + '23:59:59'

SELECT 
FC.CODPROVEEDOR, P.NOMPROVEEDOR, P.NIF20 RFC,
SUM(TOTALBRUTO) 'SUBTOTAL', 
SUM(FC.TOTALIMPUESTOS) 'IMPUESTOS',
SUM(TOTALNETO) TOTAL
FROM
(
SELECT NUMSERIEFAC, NUMFAC, FECHA_PROCESO
FROM IT_RELFACTURAS_COMPRAS WITH(NOLOCK)
WHERE FECHA_PROCESO BETWEEN @FECHAINICIAL + ' 00:00:00' AND @FECHAFINAL + ' 23:59:59'
GROUP BY NUMSERIEFAC, NUMFAC, FECHA_PROCESO
) F
INNER JOIN FACTURASCOMPRA FC WITH(NOLOCK) ON F.NUMSERIEFAC COLLATE Latin1_General_CS_AI = FC.NUMSERIE AND F.NUMFAC = FC.NUMFACTURA
INNER JOIN PROVEEDORES P WITH(NOLOCK) ON FC.CODPROVEEDOR = P.CODPROVEEDOR
WHERE FC.CODPROVEEDOR = @CODPROVEEDOR OR @CODPROVEEDOR = 0
GROUP BY FC.CODPROVEEDOR, P.NOMPROVEEDOR, NIF20
ORDER BY FC.CODPROVEEDOR
";


                    if (Environment.MachineName=="CTL-PC")
                    {
                        sql = "SELECT * FROM RESUMEN_COMPRAS WITH (NOLOCK)";
                    }

                    var rs = con.Query<RESUMEN_COMPRAS>(sql, req).ToList();


                    return JsonConvert.SerializeObject(rs);
                }
            }
            catch
            {
                return JsonConvert.SerializeObject(new ArrayList());
            }
        }



        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string GetDetalleCompras(Req req)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;


            try
            {

                using (IDbConnection con = new SqlConnection(Extensions.GetBD_ICG()))
                {
                    string sql =
                        @"
-- DETALLE DE COMPRAS POR PERIODO POR PROVEEDOR
--DECLARE @CODPROVEEDORD INT = 0
--DECLARE @FECHAINICIALD DATETIME = '01/12/2019'
--DECLARE @FECHAFINALD DATETIME = '31/12/2019'

--SET @FECHAINICIALD = @FECHAINICIALD + '00:00:00'
--SET @FECHAFINALD = @FECHAFINALD + '23:59:59'

SELECT 
AC.CODPROVEEDOR, P.NOMPROVEEDOR, P.NIF20 RFC, F.FECHA_PROCESO, AC.TOTALBRUTO SUBTOTAL, AC.TOTALIMPUESTOS IMPUESTOS, AC.TOTALNETO TOTAL,
AC.NUMSERIEFAC, AC.NUMFAC, A.FECHA 'FECHA FACTURA',
AC.NUMSERIE, AC.NUMALBARAN, AC.FECHAALBARAN 'FECHA ALBARAN'
FROM
(
SELECT NUMSERIE, NUMALBARAN, FECHA_PROCESO
FROM IT_RELFACTURAS_COMPRAS WITH(NOLOCK)
WHERE FECHA_PROCESO BETWEEN @FECHAINICIALD AND @FECHAFINALD
GROUP BY NUMSERIE, NUMALBARAN, FECHA_PROCESO
) F
INNER JOIN ALBCOMPRACAB AC WITH(NOLOCK) ON F.NUMSERIE COLLATE Latin1_General_CS_AI = AC.NUMSERIE AND F.NUMALBARAN = AC.NUMALBARAN
INNER JOIN PROVEEDORES P WITH(NOLOCK) ON AC.CODPROVEEDOR = P.CODPROVEEDOR
INNER JOIN FACTURASCOMPRA A WITH(NOLOCK) ON AC.NUMSERIEFAC = A.NUMSERIE AND AC.NUMFAC = A.NUMFACTURA AND AC.NFAC = A.N
WHERE AC.CODPROVEEDOR = @CODPROVEEDORD OR @CODPROVEEDORD = 0
ORDER BY AC.CODPROVEEDOR
";


                    if (Environment.MachineName == "CTL-PC")
                    {
                        sql = "SELECT * FROM DETALLE_COMPRAS WITH (NOLOCK)";
                    }

                    Extensions.AddMap<DETALLE_COMPRAS>();

                    var rs = con.Query<DETALLE_COMPRAS>(sql, req).ToList();


                    return JsonConvert.SerializeObject(rs);
                }
            }
            catch
            {
                return JsonConvert.SerializeObject(new ArrayList());
            }
        }

        public class Req
        {
            public DateTime FECHAINICIAL { get; set; }
            public DateTime FECHAFINAL   { get; set; }
            public string   CODPROVEEDOR { get; set; }

            public DateTime FECHAINICIALD { get; set; }
            public DateTime FECHAFINALD   { get; set; }
            public string   CODPROVEEDORD { get; set; }
        }

    }


}