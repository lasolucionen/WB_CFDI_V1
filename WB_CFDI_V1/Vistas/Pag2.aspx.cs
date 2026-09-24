using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Services;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
//using AngleSharp.Network.Default;
using Dapper;
using EO.Internal;
using Newtonsoft.Json;
using WB_CFDI_V1.TagHelpers;
using XML3;
using Comprobante = XML3.Comprobante;


// ReSharper disable InconsistentNaming

namespace WB_CFDI_V1.Vistas
{
    public partial class Pag2 : Page
    {

        public string BuscarPorArticulo2
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


            string USER_NAME = "";
            if (User.Identity.IsAuthenticated)
            {
                FormsIdentity id = (FormsIdentity)User.Identity;
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
                        new {USER_NAME}).SingleOrDefault();

                  //***************************************************
               }
               catch(Exception ex)
               {
                  USER_ID = -1;
               }
               finally
               {
                  con.Close();
               }
            }

            BuscarPorArticulo2 = TemplateHelper.RenderPartialToString("~/Vistas/Pag2/BuscarPorArticulo2.ascx", "");

        }
        //new Date(2020, 6, 31)
        protected static string getFecha1()
        {
            
            if (Environment.MachineName == "CTL-PC")
            {
                return new DateTime(2010, 01, 01).ToString("yyyy,MM,dd");
            }
            return DateTime.Now.AddDays(-DateTime.Now.Day + 1).ToString("yyyy,MM,dd");
        }


        protected static string getFecha2()
        {
            var minDate = DateTime.Now.AddDays((-DateTime.Now.Day) ); //.ToString("yyyy,MM,dd");

            return "new Date(" + minDate.Year + "," + (minDate.Month-1) + "," + minDate.Day + ")";
        }



        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string tAjuste()
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
                        SELECT CODARTICULO, REFPROVEEDOR, DESCRIPCION 
                        FROM ARTICULOS WITH(NOLOCK)
                        WHERE DPTO IN (23, 24)";


                    var rs = con.Query(sql).ToList();
                    
                    return JsonConvert.SerializeObject(rs);
                }
            }
            catch
            {
                return JsonConvert.SerializeObject(new ArrayList());
            }
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

                    var sql = @"

                    SELECT
                      a.RFC_EMISOR
                     ,MAX(a.RAZON_SOCIAL_EMISOR) AS RAZON_SOCIAL_EMISOR

                     ,CASE
                        WHEN b.DIF_TOTAL = 0 THEN (SELECT
                              a.DIF_TOTAL
                            FROM TOLERANCIA_FAC_ALB a WITH (NOLOCK) WHERE a.ID = 1)
                        ELSE b.DIF_TOTAL
                      END DIF_TOTAL

                     ,CASE
                        WHEN b.DIF_IMPUESTO = 0 THEN (SELECT
                              a.DIF_IMPUESTO
                            FROM TOLERANCIA_FAC_ALB a WITH (NOLOCK) WHERE a.ID = 1)
                        ELSE b.DIF_IMPUESTO
                      END DIF_IMPUESTO

                    FROM dbo.CFDI_XML_PSI a WITH (NOLOCK)
                    INNER JOIN dbo.CFDI_ACCOUNT b WITH (NOLOCK)
                      ON a.USER_ID = b.ID
                    GROUP BY a.RFC_EMISOR
                            ,b.DIF_TOTAL
                            ,b.DIF_IMPUESTO";


                    var rs = con.Query<CFDI_XML>(sql).ToList();
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
        public static string GetSeries()
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
SELECT SERIE, DESCRIPCION FROM SERIES WITH (NOLOCK) WHERE LEN(SERIE) = 2 
AND SERIE NOT LIKE 'M%' AND SERIE NOT LIKE '00%' AND SERIE NOT LIKE 'A2%' AND SERIE NOT LIKE 'A0%' AND SERIE NOT LIKE 'Y%' AND SERIE NOT LIKE 'P%' 
AND SERIE NOT LIKE 'X%'
UNION ALL
SELECT SERIE, DESCRIPCION FROM SERIES WITH (NOLOCK) WHERE LEN(SERIE) = 3 
AND (SERIE LIKE 'P%' or  SERIE LIKE 'X1%' or  SERIE LIKE 'X2%' ) AND SERIE <> 'X10'
ORDER BY DESCRIPCION
";

                    var rs = con.Query<SERIES>(sql).ToList();
                    rs.Insert(0, new SERIES { SERIE = "", DESCRIPCION = "-----------------------" });
                    return JsonConvert.SerializeObject(rs);
                }
            }
            catch
            {
                return JsonConvert.SerializeObject(new ArrayList());
            }
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string GetCompras(string rfc, DateTime fecha1, DateTime fecha2, string serie)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            //Log.Write("fechaINI " + fecha1.ToString("dd/MM/yyyy") + " " + fecha1.ToString());
            //Log.Write("fechaFIN " + fecha2.ToString("dd/MM/yyyy") + " " + fecha2.ToString());

            try
            {

                using (IDbConnection con = new SqlConnection(Extensions.GetBD_ICG()))
                {
                    /*

                    SELECT

                          case 
                              when CAST(AC.FECHAALBARAN AS DATE) <= @FECHA_BLOQ and @IG = 'false' then 'true'

                              ELSE 'false'
                          end BLOQ,



                            AC.NUMSERIE, AC.NUMALBARAN, AC.FECHAALBARAN,SUALBARAN, 
                            CASE WHEN SUM(AT.BRUTO) IS NULL THEN 0 ELSE SUM(AT.BRUTO) END SUBTOTAL,
                            CASE WHEN SUM(AT.TOTIVA) IS NULL THEN 0 ELSE SUM(AT.TOTIVA) END IVA,
                            CASE WHEN SUM(AT.TOTREQ) IS NULL THEN 0 ELSE SUM(AT.TOTREQ) END IEPS,
                            CASE WHEN SUM(AT.TOTAL)  IS NULL THEN 0 ELSE SUM(AT.TOTAL) END TOTAL, P.NIF20,
                            CASE WHEN ROUND(SUM(AT.TOTAL),2) - ROUND(IMPORTE,2) = 0 THEN 0 ELSE 1 END ST
                            FROM ALBCOMPRACAB AC WITH (NOLOCK)
                            LEFT JOIN ALBCOMPRATOT AT WITH (NOLOCK) ON AC.NUMSERIE = AT.SERIE AND AC.NUMALBARAN = AT.NUMERO AND AC.N = AT.N
                            INNER JOIN PROVEEDORES P WITH (NOLOCK) ON AC.CODPROVEEDOR = P.CODPROVEEDOR
                            INNER JOIN 
	                            (SELECT SERIE, NUMERO, N, SUM(IMPORTE) IMPORTE FROM TESORERIA T WITH(NOLOCK)
	                            WHERE TIPODOCUMENTO = 'A' AND ORIGEN = 'P' AND CODFORMAPAGO <> 1
	                            GROUP BY SERIE, NUMERO, N
	                            ) T
                            ON AC.NUMSERIE = T.SERIE AND AC.NUMALBARAN = T.NUMERO AND AC.N = T.N
                            LEFT JOIN IT_RELFACTURAS_COMPRAS R WITH (NOLOCK) ON AC.NUMSERIE COLLATE Modern_Spanish_CI_AS = R.NUMSERIE AND AC.NUMALBARAN = R.NUMALBARAN
                            WHERE
                            FACTURADO = 'F' AND R.NUMFAC IS NULL AND UPPER(P.NIF20) = UPPER(@RFC)
                            AND AC.TOTALNETO <> 0
                            AND AC.NUMSERIE LIKE @SERIE + '%'
                            AND AC.FECHAALBARAN BETWEEN CAST((@FECHAINI + ' 00:00:00') AS DATETIME) AND CAST((@FECHAFIN + ' 23:59:59') AS DATETIME)
                            AND AC.NUMSERIE + CONVERT(NVARCHAR(MAX), AC.NUMALBARAN) NOT IN
	                            (SELECT
	                            AC.NUMSERIE + CONVERT(NVARCHAR(MAX), AC.NUMALBARAN)
	                            FROM ALBCOMPRACAB AC WITH (NOLOCK)
	                            INNER JOIN TESORERIA AT WITH (NOLOCK) ON AC.NUMSERIE = AT.SERIE AND AC.NUMALBARAN = AT.NUMERO AND AC.N = AT.N
	                            INNER JOIN PROVEEDORES P WITH (NOLOCK) ON AC.CODPROVEEDOR = P.CODPROVEEDOR
	                            LEFT JOIN IT_RELFACTURAS_COMPRAS R WITH (NOLOCK) ON AC.NUMSERIE COLLATE Modern_Spanish_CI_AS = R.NUMSERIE AND AC.NUMALBARAN = R.NUMALBARAN
	                            WHERE FACTURADO = 'F'
	                            AND R.NUMFAC IS NULL
	                            AND UPPER(P.NIF20) = UPPER(@RFC)
	                            AND AC.NUMSERIE LIKE @SERIE + '%'
	                            AND AC.FECHAALBARAN BETWEEN CAST((@FECHAINI + ' 00:00:00') AS DATETIME) AND CAST((@FECHAFIN + ' 23:59:59') AS DATETIME)
	                            AND AT.CODFORMAPAGO = 1
	                            GROUP BY AC.NUMSERIE,AC.NUMALBARAN)
                            GROUP BY AC.NUMSERIE, AC.NUMALBARAN, AC.FECHAALBARAN, SUALBARAN, P.NIF20, IMPORTE
                            ORDER BY SUALBARAN, NUMSERIE, NUMALBARAN, AC.FECHAALBARAN
                     
                    */

                    /*
                    string sql = @"
                    SELECT

                          case 
                              when CAST(AC.FECHAALBARAN AS DATE) <= @FECHA_BLOQ and @IG = 'false' then 'true'

                              ELSE 'false'
                          end BLOQ,



                            AC.NUMSERIE, AC.NUMALBARAN, AC.FECHAALBARAN,SUALBARAN, 
                            CASE WHEN SUM(AT.BRUTO) IS NULL THEN 0 ELSE SUM(AT.BRUTO) END SUBTOTAL,
                            CASE WHEN SUM(AT.TOTIVA) IS NULL THEN 0 ELSE SUM(AT.TOTIVA) END IVA,
                            CASE WHEN SUM(AT.TOTREQ) IS NULL THEN 0 ELSE SUM(AT.TOTREQ) END IEPS,
                            CASE WHEN SUM(AT.TOTAL)  IS NULL THEN 0 ELSE SUM(AT.TOTAL) END TOTAL, P.NIF20,
                            CASE WHEN ROUND(SUM(AT.TOTAL),2) - ROUND(IMPORTE,2) = 0 THEN 0 ELSE 1 END ST
                            FROM ALBCOMPRACAB AC WITH (NOLOCK)
                            LEFT JOIN ALBCOMPRATOT AT WITH (NOLOCK) ON AC.NUMSERIE = AT.SERIE AND AC.NUMALBARAN = AT.NUMERO AND AC.N = AT.N
                            INNER JOIN PROVEEDORES P WITH (NOLOCK) ON AC.CODPROVEEDOR = P.CODPROVEEDOR
                            INNER JOIN 
                                   (SELECT SERIE, NUMERO, N, SUM(IMPORTE) IMPORTE FROM TESORERIA T WITH(NOLOCK)
                                   WHERE TIPODOCUMENTO = 'A' AND ORIGEN = 'P' AND CODFORMAPAGO <> 1
                                   GROUP BY SERIE, NUMERO, N
                                   ) T
                            ON AC.NUMSERIE = T.SERIE AND AC.NUMALBARAN = T.NUMERO AND AC.N = T.N
                            LEFT JOIN IT_RELFACTURAS_COMPRAS R WITH (NOLOCK) ON AC.NUMSERIE COLLATE Modern_Spanish_CI_AS = R.NUMSERIE AND AC.NUMALBARAN = R.NUMALBARAN
                            WHERE
                            AC.NUMFAC = -1 AND R.NUMFAC IS NULL AND UPPER(P.NIF20) = UPPER(@RFC) AND AC.NUMSERIEFAC <> 'AUTO'
                            AND AC.TOTALNETO <> 0
                            AND AC.NUMSERIE LIKE @SERIE + '%'
                            AND AC.FECHAALBARAN BETWEEN CAST((@FECHAINI + ' 00:00:00') AS DATETIME) AND CAST((@FECHAFIN + ' 23:59:59') AS DATETIME)
                            AND AC.NUMSERIE + CONVERT(NVARCHAR(MAX), AC.NUMALBARAN) NOT IN
                                   (SELECT
                                   AC.NUMSERIE + CONVERT(NVARCHAR(MAX), AC.NUMALBARAN)
                                   FROM ALBCOMPRACAB AC WITH (NOLOCK)
                                   INNER JOIN TESORERIA AT WITH (NOLOCK) ON AC.NUMSERIE = AT.SERIE AND AC.NUMALBARAN = AT.NUMERO AND AC.N = AT.N
                                   INNER JOIN PROVEEDORES P WITH (NOLOCK) ON AC.CODPROVEEDOR = P.CODPROVEEDOR
                                   LEFT JOIN IT_RELFACTURAS_COMPRAS R WITH (NOLOCK) ON AC.NUMSERIE COLLATE Modern_Spanish_CI_AS = R.NUMSERIE AND AC.NUMALBARAN = R.NUMALBARAN
                                   WHERE AC.NUMFAC = -1
                                   AND R.NUMFAC IS NULL
                                   AND UPPER(P.NIF20) = UPPER(@RFC)
                                   AND AC.NUMSERIE LIKE @SERIE + '%'
                                   AND AC.FECHAALBARAN BETWEEN CAST((@FECHAINI + ' 00:00:00') AS DATETIME) AND CAST((@FECHAFIN + ' 23:59:59') AS DATETIME)
                                   AND AT.CODFORMAPAGO = 1
                                   GROUP BY AC.NUMSERIE,AC.NUMALBARAN)
                            GROUP BY AC.NUMSERIE, AC.NUMALBARAN, AC.FECHAALBARAN, SUALBARAN, P.NIF20, IMPORTE
                            ORDER BY SUALBARAN, NUMSERIE, NUMALBARAN, AC.FECHAALBARAN

                    ";*/


                    string sql = @"
WITH TesoreriaSum AS (
    SELECT SERIE, NUMERO, N, 
           SUM(CASE WHEN CODFORMAPAGO <> 1 THEN IMPORTE ELSE 0 END) as IMPORTE,
           MAX(CASE WHEN CODFORMAPAGO = 1 THEN 1 ELSE 0 END) as TIENE_PAGO_1
    FROM TESORERIA WITH(NOLOCK)
    WHERE TIPODOCUMENTO = 'A' AND ORIGEN = 'P'
    GROUP BY SERIE, NUMERO, N
)
SELECT 
    CASE 
        WHEN AC.FECHAALBARAN <= @FECHA_BLOQ AND @IG = 'false' THEN 'true' 
        ELSE 'false' 
    END AS BLOQ,
    AC.NUMSERIE, 
    AC.NUMALBARAN, 
    AC.FECHAALBARAN,
    AC.SUALBARAN, 
    ISNULL(SUM(AT.BRUTO), 0) AS SUBTOTAL,
    ISNULL(SUM(AT.TOTIVA), 0) AS IVA,
    ISNULL(SUM(AT.TOTREQ), 0) AS IEPS,
    ISNULL(SUM(AT.TOTAL), 0) AS TOTAL, 
    P.NIF20,
    CASE WHEN ROUND(ISNULL(SUM(AT.TOTAL), 0), 2) - ROUND(T.IMPORTE, 2) = 0 THEN 0 ELSE 1 END AS ST
FROM ALBCOMPRACAB AC WITH (NOLOCK)
INNER JOIN PROVEEDORES P WITH (NOLOCK) ON AC.CODPROVEEDOR = P.CODPROVEEDOR
INNER JOIN TesoreriaSum T ON AC.NUMSERIE = T.SERIE AND AC.NUMALBARAN = T.NUMERO AND AC.N = T.N
LEFT JOIN ALBCOMPRATOT AT WITH (NOLOCK) ON AC.NUMSERIE = AT.SERIE AND AC.NUMALBARAN = AT.NUMERO AND AC.N = AT.N
LEFT JOIN IT_RELFACTURAS_COMPRAS R WITH (NOLOCK) 
    ON AC.NUMSERIE = R.NUMSERIE COLLATE Modern_Spanish_CI_AS
    AND AC.NUMALBARAN = R.NUMALBARAN
WHERE 
    AC.NUMFAC = -1 
    AND R.NUMFAC IS NULL 
    AND P.NIF20 = UPPER(@RFC)
    AND AC.NUMSERIEFAC <> 'AUTO'
    AND AC.TOTALNETO <> 0
    AND AC.NUMSERIE LIKE @SERIE + '%'
    AND AC.FECHAALBARAN >= CAST(@FECHAINI AS DATETIME)
  AND AC.FECHAALBARAN <= CAST(@FECHAFIN AS DATETIME) + '23:59:59'
    AND T.TIENE_PAGO_1 = 0
GROUP BY 
    AC.NUMSERIE, AC.NUMALBARAN, AC.FECHAALBARAN, AC.SUALBARAN, P.NIF20, T.IMPORTE
ORDER BY 
    AC.SUALBARAN, AC.NUMSERIE, AC.NUMALBARAN, AC.FECHAALBARAN;

                   ";

                    //Log.Write("Serie: " + serie + " FECHAINI: " + fecha1.ToString("dd/MM/yyyy") + " FECHAFIN: " + fecha2.ToString("dd/MM/yyyy") + " RFC: " + rfc);


                    /*

                          case 
                              when CAST(AC.FECHAALBARAN AS DATE) < CAST(DATEADD(dd,-@FECHA_BLOQ,GETDATE()) AS DATE) and @IG = 'false' then 'true'

                              ELSE 'false'
                          end BLOQ,
                     
                          Convert.ToInt32(GetFechaBloqueo2())
                     *
                     *
                          case 
                              when CAST(xml.FECHA_FACTURA AS DATE) <= @FECHA_BLOQ then 'true'

                              ELSE 'false'
                          end BLOQ,
                     */



                    var rs1 = con.Query<COMPRAS>(
                            sql,
                            new {
                                    SERIE      = serie,
                                    FECHAINI   = fecha1.ToString("dd/MM/yyyy"),
                                    FECHAFIN   = fecha2.ToString("dd/MM/yyyy"),
                                    RFC        = rfc,
                                    FECHA_BLOQ = GetFechaBloqueo2(),
                                    IG         = ShowBtn().IGNORAR_BLOQUEO2
                                },commandTimeout:2000).ToList();




                    var sql1 = @"
DELETE FROM IT_NUM_ALB_TEMP WHERE USUARIO = @USUARIO;
";

                    con.Execute(sql1,new
                    {
                        USUARIO=Extensions.GetUserName()

                    });



                    var sql2 = @"
INSERT INTO dbo.IT_NUM_ALB_TEMP (NUMSERIE, NUMALBARAN, USUARIO) VALUES
(@NUMSERIE, @NUMALBARAN, @USUARIO);
";

                    foreach (var cm in rs1)
                    {
                        con.Execute(sql2, new
                        {
                            USUARIO    = Extensions.GetUserName(),
                            NUMSERIE   =cm.NUMSERIE,
                            NUMALBARAN = cm.NUMALBARAN,
                        });
                    }
                    


                    //if (Debugger.IsAttached)
                    //{
                    //    rs.Add(new COMPRAS {
                    //                           BLOQ = false,
                    //                           FECHAALBARAN = DateTime.Now,
                    //                           IEPS = 20.67,
                    //                           NUMALBARAN = 13877,
                    //                           NUMSERIE = "142A",
                    //                           SUALBARAN = "31256",
                    //                           TOTAL = 279.00,
                    //                           SUBTOTAL = 258.33
                    //                       });
                    //}


                    return JsonConvert.SerializeObject(new{rs1});
                }
            }
            catch(Exception ex)
            {
               //Log.Write("Error: " + ex.Message);
                return JsonConvert.SerializeObject(new ArrayList());
            }
        }




        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string GetCompras2(DateTime fecha1, DateTime fecha2, string NUMSERIE, string NUMALBARAN, string SUALBARAN)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            //Log.Write("fechaINI " + fecha1.ToString("dd/MM/yyyy") + " " + fecha1.ToString());
            //Log.Write("fechaFIN " + fecha2.ToString("dd/MM/yyyy") + " " + fecha2.ToString());


            if (string.IsNullOrEmpty(NUMSERIE))
            {
                NUMSERIE = "";
            }

            if (string.IsNullOrEmpty(NUMALBARAN))
            {
                NUMALBARAN = "";
            }

            if (string.IsNullOrEmpty(SUALBARAN))
            {
                SUALBARAN = "";
            }

            try
            {

                using (IDbConnection con = new SqlConnection(Extensions.GetBD_ICG()))
                {



                    string sql = @"
SELECT
AC.NUMSERIE, AC.NUMALBARAN, AC.FECHAALBARAN,SUALBARAN, P.NIF20,
CASE WHEN SUM(AT.BRUTO) IS NULL THEN 0 ELSE SUM(AT.BRUTO) END SUBTOTAL,
CASE WHEN SUM(AT.TOTIVA) IS NULL THEN 0 ELSE SUM(AT.TOTIVA) END IVA,
CASE WHEN SUM(AT.TOTREQ) IS NULL THEN 0 ELSE SUM(AT.TOTREQ) END IEPS,
CASE WHEN SUM(AT.TOTAL)  IS NULL THEN 0 ELSE SUM(AT.TOTAL) END TOTAL, P.NIF20,
CASE WHEN ROUND(SUM(AT.TOTAL),2) - ROUND(IMPORTE,2) = 0 THEN 0 ELSE 1 END ST, 'SIN VALIDAR' ESTADO 
FROM ALBCOMPRACAB AC WITH (NOLOCK)
LEFT JOIN ALBCOMPRATOT AT WITH (NOLOCK) ON AC.NUMSERIE = AT.SERIE AND AC.NUMALBARAN = AT.NUMERO AND AC.N = AT.N
INNER JOIN PROVEEDORES P WITH (NOLOCK) ON AC.CODPROVEEDOR = P.CODPROVEEDOR
INNER JOIN
        (SELECT SERIE, NUMERO, N, SUM(IMPORTE) IMPORTE FROM TESORERIA T WITH(NOLOCK)
        WHERE TIPODOCUMENTO = 'A' AND ORIGEN = 'P' AND CODFORMAPAGO <> 1
        GROUP BY SERIE, NUMERO, N
        ) T
ON AC.NUMSERIE = T.SERIE AND AC.NUMALBARAN = T.NUMERO AND AC.N = T.N
LEFT JOIN IT_RELFACTURAS_COMPRAS R WITH (NOLOCK) ON AC.NUMSERIE COLLATE Modern_Spanish_CI_AS = R.NUMSERIE AND AC.NUMALBARAN = R.NUMALBARAN
WHERE 
AC.NUMFAC = -1 AND R.NUMFAC IS NULL AND 
(@NUMSERIE = '' OR UPPER(AC.NUMSERIE) LIKE '%' + UPPER(@NUMSERIE) + '%')
AND (@NUMALBARAN = '' OR UPPER(AC.NUMALBARAN) LIKE '%' + UPPER(@NUMALBARAN) + '%')
AND (@SUALBARAN = '' OR UPPER(SUALBARAN) LIKE '%' + UPPER(@SUALBARAN) + '%') AND 
AC.TOTALNETO <> 0
AND AC.FECHAALBARAN BETWEEN CAST((@FECHAINI + ' 00:00:00') AS DATETIME) AND CAST((@FECHAFIN + ' 23:59:59') AS DATETIME)
AND AC.NUMSERIE + CONVERT(NVARCHAR(MAX), AC.NUMALBARAN) NOT IN
        (SELECT
        AC.NUMSERIE + CONVERT(NVARCHAR(MAX), AC.NUMALBARAN)
        FROM ALBCOMPRACAB AC WITH (NOLOCK)
        INNER JOIN TESORERIA AT WITH (NOLOCK) ON AC.NUMSERIE = AT.SERIE AND AC.NUMALBARAN = AT.NUMERO AND AC.N = AT.N
        INNER JOIN PROVEEDORES P WITH (NOLOCK) ON AC.CODPROVEEDOR = P.CODPROVEEDOR
        LEFT JOIN IT_RELFACTURAS_COMPRAS R WITH (NOLOCK) ON AC.NUMSERIE COLLATE Modern_Spanish_CI_AS = R.NUMSERIE AND AC.NUMALBARAN = R.NUMALBARAN
        WHERE AC.NUMFAC = -1
        AND R.NUMFAC IS NULL
        AND (@NUMSERIE = '' OR UPPER(AC.NUMSERIE) LIKE '%' + UPPER(@NUMSERIE) + '%')
        AND (@NUMALBARAN = '' OR UPPER(AC.NUMALBARAN) LIKE '%' + UPPER(@NUMALBARAN) + '%')
        AND (@SUALBARAN = '' OR UPPER(SUALBARAN) LIKE '%' + UPPER(@SUALBARAN) + '%')
        AND AC.FECHAALBARAN BETWEEN CAST((@FECHAINI + ' 00:00:00') AS DATETIME) AND CAST((@FECHAFIN + ' 23:59:59') AS DATETIME)
        AND AT.CODFORMAPAGO = 1
        GROUP BY AC.NUMSERIE,AC.NUMALBARAN)
GROUP BY AC.NUMSERIE, AC.NUMALBARAN, AC.FECHAALBARAN, SUALBARAN, P.NIF20, IMPORTE
UNION ALL
SELECT
AC.NUMSERIE, AC.NUMALBARAN, AC.FECHAALBARAN,SUALBARAN, P.NIF20,
CASE WHEN SUM(AT.BRUTO) IS NULL THEN 0 ELSE SUM(AT.BRUTO) END SUBTOTAL,
CASE WHEN SUM(AT.TOTIVA) IS NULL THEN 0 ELSE SUM(AT.TOTIVA) END IVA,
CASE WHEN SUM(AT.TOTREQ) IS NULL THEN 0 ELSE SUM(AT.TOTREQ) END IEPS,
CASE WHEN SUM(AT.TOTAL)  IS NULL THEN 0 ELSE SUM(AT.TOTAL) END TOTAL, P.NIF20,
CASE WHEN ROUND(SUM(AT.TOTAL),2) - ROUND(IMPORTE,2) = 0 THEN 0 ELSE 1 END ST, 'FACTURADO- VALIDADO' ESTADO 
FROM ALBCOMPRACAB AC WITH (NOLOCK)
LEFT JOIN ALBCOMPRATOT AT WITH (NOLOCK) ON AC.NUMSERIE = AT.SERIE AND AC.NUMALBARAN = AT.NUMERO AND AC.N = AT.N
INNER JOIN PROVEEDORES P WITH (NOLOCK) ON AC.CODPROVEEDOR = P.CODPROVEEDOR
INNER JOIN
        (SELECT SERIE, NUMERO, N, SUM(IMPORTE) IMPORTE FROM TESORERIA T WITH(NOLOCK)
        WHERE TIPODOCUMENTO = 'A' AND ORIGEN = 'P' AND CODFORMAPAGO <> 1
        GROUP BY SERIE, NUMERO, N
        ) T
ON AC.NUMSERIEFAC = T.SERIE AND AC.NUMFAC = T.NUMERO AND AC.N = T.N
INNER JOIN IT_RELFACTURAS_COMPRAS R WITH (NOLOCK) ON AC.NUMSERIE COLLATE Modern_Spanish_CI_AS = R.NUMSERIE AND AC.NUMALBARAN = R.NUMALBARAN
WHERE
AC.FACTURADO = 'T' AND
(@NUMSERIE = '' OR UPPER(AC.NUMSERIE) LIKE '%' + UPPER(@NUMSERIE) + '%')
AND (@NUMALBARAN = '' OR UPPER(AC.NUMALBARAN) LIKE '%' + UPPER(@NUMALBARAN) + '%')
AND (@SUALBARAN = '' OR UPPER(SUALBARAN) LIKE '%' + UPPER(@SUALBARAN) + '%') AND 
AC.TOTALNETO <> 0
AND AC.FECHAALBARAN BETWEEN CAST((@FECHAINI + ' 00:00:00') AS DATETIME) AND CAST((@FECHAFIN + ' 23:59:59') AS DATETIME)
GROUP BY AC.NUMSERIE, AC.NUMALBARAN, AC.FECHAALBARAN, SUALBARAN, P.NIF20, IMPORTE
UNION ALL
SELECT
AC.NUMSERIE, AC.NUMALBARAN, AC.FECHAALBARAN,SUALBARAN, P.NIF20,
CASE WHEN SUM(AT.BRUTO) IS NULL THEN 0 ELSE SUM(AT.BRUTO) END SUBTOTAL,
CASE WHEN SUM(AT.TOTIVA) IS NULL THEN 0 ELSE SUM(AT.TOTIVA) END IVA,
CASE WHEN SUM(AT.TOTREQ) IS NULL THEN 0 ELSE SUM(AT.TOTREQ) END IEPS,
CASE WHEN SUM(AT.TOTAL)  IS NULL THEN 0 ELSE SUM(AT.TOTAL) END TOTAL, P.NIF20,
CASE WHEN ROUND(SUM(AT.TOTAL),2) - ROUND(IMPORTE,2) = 0 THEN 0 ELSE 1 END ST, 'FACTURADO- NO VALIDADO' ESTADO
FROM ALBCOMPRACAB AC WITH (NOLOCK)
LEFT JOIN ALBCOMPRATOT AT WITH (NOLOCK) ON AC.NUMSERIE = AT.SERIE AND AC.NUMALBARAN = AT.NUMERO AND AC.N = AT.N
INNER JOIN PROVEEDORES P WITH (NOLOCK) ON AC.CODPROVEEDOR = P.CODPROVEEDOR
INNER JOIN
        (SELECT SERIE, NUMERO, N, SUM(IMPORTE) IMPORTE FROM TESORERIA T WITH(NOLOCK)
        WHERE TIPODOCUMENTO = 'A' AND ORIGEN = 'P' AND CODFORMAPAGO <> 1
        GROUP BY SERIE, NUMERO, N
        ) T
ON AC.NUMSERIE = T.SERIE AND AC.NUMALBARAN = T.NUMERO AND AC.N = T.N
LEFT JOIN IT_RELFACTURAS_COMPRAS R WITH (NOLOCK) ON AC.NUMSERIE COLLATE Modern_Spanish_CI_AS = R.NUMSERIE AND AC.NUMALBARAN = R.NUMALBARAN
WHERE AC.FACTURADO = 'T' AND AC.NUMFAC <> -1
AND R.NUMFAC IS NULL AND 
(@NUMSERIE = '' OR UPPER(AC.NUMSERIE) LIKE '%' + UPPER(@NUMSERIE) + '%')
AND (@NUMALBARAN = '' OR UPPER(AC.NUMALBARAN) LIKE '%' + UPPER(@NUMALBARAN) + '%')
AND (@SUALBARAN = '' OR UPPER(SUALBARAN) LIKE '%' + UPPER(@SUALBARAN) + '%') AND 
AC.TOTALNETO <> 0
AND AC.FECHAALBARAN BETWEEN CAST((@FECHAINI + ' 00:00:00') AS DATETIME) AND CAST((@FECHAFIN + ' 23:59:59') AS DATETIME)
GROUP BY AC.NUMSERIE, AC.NUMALBARAN, AC.FECHAALBARAN, SUALBARAN, P.NIF20, IMPORTE
ORDER BY ESTADO, NUMSERIE, NUMALBARAN, AC.FECHAALBARAN

                    ";





                    var rs = con.Query<COMPRAS>(
                            sql,
                            new
                            {
                                FECHAINI = fecha1.ToString("dd/MM/yyyy"),
                                FECHAFIN = fecha2.ToString("dd/MM/yyyy"),
                                NUMSERIE,
                                NUMALBARAN,
                                SUALBARAN
                            }).ToList();



                    return JsonConvert.SerializeObject(rs);
                }
            }
            catch (Exception ex)
            {
                //Log.Write("Error: " + ex.Message);
                return JsonConvert.SerializeObject(new ArrayList());
            }
        }


        public class DETALLE
        {
            public string DESCRIPCION { get; set; }
            public string CANTIDAD    { get; set; }
            public string UNIDAD      { get; set; }
            public string CLAVE       { get; set; }
            public string PU          { get; set; }
            public string IMPORTE     { get; set; }
            public string DESCUENTO   { get; set; }
        }


        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string GetAlbaranes(string uuid)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            try
            {
                using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                {
                    var emp = Extensions.cEmpresa();

                    var it = con.Query<ALBARANES>(
                        @"SELECT NUMSERIE, NUMALBARAN FROM " + emp.ICG_BD + @".dbo.IT_RELFACTURAS_COMPRAS WITH (NOLOCK) WHERE FOLIOFISCAL = @UUID",
                        new { uuid }
                    ).ToList();


                    return JsonConvert.SerializeObject(new { code = 0, data = it, msg = "" });
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { code = -1, data = "[]", msg = ex.Message });
            }
        }




        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string GetArticulosAlb(string NUMSERIE, string NUMALBARAN, string REF, string DESCRIPCION)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            try
            {
                Extensions.AddMap<ALBCOMPRALIN>();

                using (IDbConnection con = new SqlConnection(Extensions.GetBD_ICG()))
                {

                    if (string.IsNullOrEmpty(NUMSERIE))
                    {
                        NUMSERIE = "";
                    }


                    if (string.IsNullOrEmpty(NUMALBARAN))
                    {
                        NUMALBARAN = "";
                    }

                    if (string.IsNullOrEmpty(REF))
                    {
                        REF = "";
                    }

                    if (string.IsNullOrEmpty(DESCRIPCION))
                    {
                        DESCRIPCION = "";
                    }


                    var sql = @"
SELECT (SELECT FECHAALBARAN FROM ALBCOMPRACAB WHERE NUMSERIE = AL.NUMSERIE AND NUMALBARAN =  AL.NUMALBARAN) 'FECHA ALBARAN',
inat.USUARIO, AL.NUMSERIE,AL.NUMALBARAN,REFERENCIA, LTRIM(RTRIM(DESCRIPCION)) DESCRIPCION, UNIDADESTOTAL, PRECIO, DTO, TOTAL, TIPOIMPUESTO, AL.CODALMACEN, 
CASE WHEN CC.COSTOACTUALIZAR IS NULL THEN CA.ULTIMOCOSTE ELSE CC.COSTOACTUALIZAR END 'ULTIMO COSTO ACTUALIZADO',
CASE WHEN CC.COSTOACTUALIZAR IS NULL THEN '' ELSE CC.FECHAACTUALIZACION END 'FECHA ACTUALIZACION',
CASE WHEN CC.COSTOACTUALIZAR IS NULL THEN 'NO' ELSE 'SI' END 'CENTRALIZADOR DE COSTOS', AL.IVA, AL.REQ
FROM ALBCOMPRALIN AL WITH(NOLOCK)
LEFT JOIN COSTESPORALMACEN CA WITH(NOLOCK) ON AL.CODARTICULO = CA.CODARTICULO AND AL.CODALMACEN = CA.CODALMACEN
LEFT JOIN
(
SELECT
ROW_NUMBER() OVER (PARTITION BY CODARTICULO, CODALMACEN ORDER BY FECHAACTUALIZACION DESC) POS,
ID.CODARTICULO, ID.CODALMACEN, IA.FECHAACTUALIZACION, ID.COSTOACTUALIZAR
FROM
IT_ACTCOSTOS IA WITH(NOLOCK)
INNER JOIN IT_ACTCOSTOS_DET ID WITH(NOLOCK) ON IA.IDCARGA = ID.IDCARGA
WHERE STAPLICACION = 1
) CC ON CC.CODARTICULO = AL.CODARTICULO AND CC.CODALMACEN COLLATE Latin1_General_CS_AI = AL.CODALMACEN AND CC.POS = 1
INNER JOIN IT_NUM_ALB_TEMP inat ON inat.NUMSERIE COLLATE Latin1_General_CS_AI = AL.NUMSERIE AND inat.NUMALBARAN = AL.NUMALBARAN
WHERE inat.USUARIO = @USUARIO AND (@NUMSERIE='' OR UPPER(AL.NUMSERIE) = UPPER(@NUMSERIE) ) AND (@NUMALBARAN='' OR UPPER(AL.NUMALBARAN) = UPPER(@NUMALBARAN) )
AND (@REFERENCIA='' OR UPPER(REFERENCIA) LIKE '%' + UPPER(@REFERENCIA) + '%' )
AND (@DESCRIPCION='' OR UPPER(DESCRIPCION) LIKE '%' + UPPER(@DESCRIPCION) + '%' )
ORDER BY AL.NUMSERIE, AL.NUMALBARAN
";


                    var it = con.Query<ALBCOMPRALIN>(sql,
                            new
                            {
                                USUARIO = Extensions.GetUserName(),
                                NUMSERIE,
                                NUMALBARAN,
                                REFERENCIA = REF,
                                DESCRIPCION
                            })
                        .ToList();


                    it.ForEach(f =>
                    {

                        if (f.PRECIO != null && Math.Abs(f.PRECIO.Value - f.ULTIMO_COSTO_ACTUALIZADO) > 0.01)
                        {
                            f.COLOR = 1;
                        }


                        if (f.CENTRALIZADOR_DE_COSTOS == "NO")
                        {
                            f.FECHA_ACTUALIZACION_STR = "";
                        }
                        else
                        {

                            f.FECHA_ACTUALIZACION_STR = f.FECHA_ACTUALIZACION.ToString("dd/MM/yyyy");
                        }

                        if (f.DTO != null && f.DTO.Value > 0)
                        {
                            f.COLOR2 = 1;
                        }

                        //f.TOTAL = 423.01; //*******

                    });


                    return JsonConvert.SerializeObject(new { code = 0, data = it, msg = "" });
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { code = -1, data = "[]", msg = ex.Message });
            }
        }


        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string GetDetalle2(string id)
        {
             HttpContext current = HttpContext.Current;
             current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
             HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
             HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

             try
             {
                  using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                  {


                       var it = con.Query<CFDI_COMPRAS>(
                                 @"SELECT * FROM CFDI_COMPRAS WITH (NOLOCK) WHERE ID_XML = @ID_XML",
                                 new { ID_XML = id }
                                 ).ToList();


                       return JsonConvert.SerializeObject(new { code = 0, data = it, msg = "" });
                  }
             }
             catch (Exception ex)
             {
                  return JsonConvert.SerializeObject(new { code = -1, data = "[]", msg = ex.Message });
             }
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string GetDetalle(string id)
        {
           HttpContext current = HttpContext.Current;
           current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
           HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
           HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

           try
           {
              using( IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()) )
              {
                  string sql = @"SELECT XML,REVISADO FROM dbo.CFDI_XML_PSI WITH (NOLOCK) WHERE ID = @ID";

                 var rs = con.Query<CFDI_XML>(sql, new {ID = id}).SingleOrDefault();

                 XML3.Comprobante c33 = new XML3.Comprobante();

                 c33 = c33.DeserializeStr(rs.XML);

                 List<DETALLE> ds = new List<DETALLE>();

                 string RFiscal    = "";
                 string UsoCFDI    = "";
                 string MetodoPago = "";
                 string FormaPago  = "";
                 string Moneda     = "";
                 double Descuento  = 0;

                 if( c33.Version != null )
                 {
                    foreach( var cp in c33.Conceptos.Concepto )
                    {


                       ds.Add(
                             new DETALLE
                                {
                                   CANTIDAD    = cp.Cantidad.Dbl().ToString(),
                                   UNIDAD      = cp.ClaveUnidad,
                                   CLAVE       = cp.ClaveProdServ,
                                   DESCRIPCION = cp.Descripcion,
                                   PU          = cp.ValorUnitario.Dbl().ToString("N2"),
                                   IMPORTE = cp.Importe.Dbl().ToString("N2"),  //"622.23".Dbl().ToString("N2"),
                                   DESCUENTO = cp.Descuento.Dbl().ToString("N2"), //"80.88".Dbl().ToString("N2")
                                });
                       //Descuento += cp.Descuento.Dbl();
                    }

                    //"3.5".Dbl().ToString("N2") 


                    if (c33.TipoDeComprobante != null && (c33.TipoDeComprobante.ToLower() == "e" || c33.TipoDeComprobante.ToLower() == "egreso"))
                    {
                        
                        if (c33.Descuento != null) Descuento = c33.Descuento.Dbl() * -1;

                    }
                    else
                    {
                        if (c33.Descuento != null) Descuento = c33.Descuento.Dbl();
                    }

                    //c33.Receptor.UsoCFDI = "D09";
                    //c33.Emisor.RegimenFiscal = "620";


                    RFiscal    = c33.Emisor.RegimenFiscal.Descr();
                    UsoCFDI    = c33.Receptor.UsoCFDI.Descr();
                    MetodoPago = c33.MetodoPago.Descr();
                    FormaPago  = c33.FormaPago.Descr();
                    Moneda     = c33.Moneda;

                    /*
                    foreach (var tr in c33.Impuestos.Traslados.Traslado)
                    {
                        switch (tr.Impuesto)
                        {
                            case "002":
                                ds.Add(new DETALLE { Impuesto = "IVA", TasaOCuota = tr.TasaOCuota, Importe = tr.Importe });
                                break;
                            case "003":
                                ds.Add(new DETALLE { Impuesto = "IEPS", TasaOCuota = tr.TasaOCuota, Importe = tr.Importe });
                                break;
                        }
                    }*/
                 }

                 var list = new DATA3
                               {
                                  DATA       = ds,
                                  RFiscal    = RFiscal,
                                  UsoCFDI    = UsoCFDI,
                                  MetodoPago = MetodoPago,
                                  FormaPago  = FormaPago,
                                  Moneda     = Moneda,
                                  Revisado   = rs.REVISADO,
                                  Descuento  = Descuento.ToString("C2")
                               };

                 return JsonConvert.SerializeObject(list);
              }
           }
           catch
           {
              return JsonConvert.SerializeObject(new DATA3());
           }
        }


/*        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string GetDetalle(string id)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            try
            {

                using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                {
                    string sql = @"SELECT XML FROM dbo.CFDI_XML_PSI WHERE ID = @ID".BD();

                    var rs = con.Query<CFDI_XML>(sql, new { ID = id }).SingleOrDefault();

                    XML3.Comprobante c33 = new XML3.Comprobante();
                    XML2.Comprobante c32 = new XML2.Comprobante();


                    c33 = c33.DeserializeStr(rs.XML);
                    c32 = c32.DeserializeStr(rs.XML);

                    List<DETALLE> ds = new List<DETALLE>();

                    if (c33.Version != null)
                    {
                        foreach (var tr in c33.Impuestos.Traslados.Traslado)
                        {
                            switch (tr.Impuesto)
                            {
                                case "002":
                                    ds.Add(new DETALLE { Impuesto = "IVA", TasaOCuota = tr.TasaOCuota, Importe = tr.Importe });
                                    break;
                                case "003":
                                    ds.Add(new DETALLE { Impuesto = "IEPS", TasaOCuota = tr.TasaOCuota, Importe = tr.Importe });
                                    break;
                            }
                        }
                    }

                    if (c32.Version != null)
                    {
                        foreach (var tr in c32.Impuestos.Traslados.Traslado)
                        {
                            switch (tr.Impuesto)
                            {
                                case "002":
                                    ds.Add(new DETALLE { Impuesto = "IVA", TasaOCuota = tr.Tasa, Importe = tr.Importe });
                                    break;
                                case "003":
                                    ds.Add(new DETALLE { Impuesto = "IEPS", TasaOCuota = tr.Tasa, Importe = tr.Importe });
                                    break;
                            }
                        }
                    }

                    return JsonConvert.SerializeObject(ds);
                }
            }
            catch
            {
                return JsonConvert.SerializeObject(new ArrayList());
            }
        }*/


        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string GetRel(string uuid)
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
                                XML
                            FROM dbo.CFDI_XML_PSI WITH (NOLOCK) WHERE UUID = @UUID";

                    var it = con.Query<CFDI_XML>(sql, new {uuid}).FirstOrDefault();

                    Comprobante c33 = new Comprobante();
                    c33 = c33.DeserializeStr(it.XML);


                    var emp = Extensions.cEmpresa();
                    if (c33.CfdiRelacionados != null && c33.CfdiRelacionados.CfdiRelacionado != null)
                    {
                        sql = @"
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
                          xml.REVISADO,
                          xml.NOMBRE_ARCHIVO,
                          xml.ESTATUS,
                          es.ESTATUS AS ESTATUS2,
                          xml.TIPO_COMPROBANTE,
                          ic.FECHA_PROCESO
                        FROM dbo.CFDI_XML_PSI xml WITH (NOLOCK)
                        INNER JOIN dbo.CFDI_ESTATUS es  ON xml.ESTATUS = es.ID
                        LEFT OUTER JOIN
                                 (SELECT DISTINCT 
                                    CAST(FECHA_PROCESO AS DATE) FECHA_PROCESO,FOLIOFISCAL
                                    FROM " + emp.ICG_BD + @".dbo.IT_RELFACTURAS_COMPRAS) ic
                                        ON xml.UUID = ic.FOLIOFISCAL
                        WHERE UUID IN @UUID
                        ORDER BY xml.CONTRA_RECIBO_ID DESC";

                        var rs = con.Query<CFDI_XML>(sql, new { uuid = c33.CfdiRelacionados.CfdiRelacionado.Select(s=>s.UUID ).ToArray() }).ToList();


                        foreach (var xml in rs)
                        {
                            if (xml.TIPO_COMPROBANTE != null && (xml.TIPO_COMPROBANTE.ToLower() == "e" || xml.TIPO_COMPROBANTE.ToLower() == "egreso"))
                            {
                                if (xml.TOTAL != null) xml.TOTAL = xml.TOTAL * -1;
                                if (xml.SUBTOTAL != null) xml.SUBTOTAL = xml.SUBTOTAL * -1;
                                if (xml.IMPUESTOS != null) xml.IMPUESTOS = xml.IMPUESTOS * -1;
                                if (xml.DESCUENTO != null) xml.DESCUENTO = xml.DESCUENTO * -1;
                            }

                            c33 = c33.DeserializeStr(xml.XML);



                                if (c33.Impuestos != null && c33.Impuestos.Traslados != null && c33.Impuestos.Traslados.Traslado != null)
                                {
                                    xml.IVA = c33.Impuestos.Traslados.Traslado.Where(w => w.Impuesto == "002")
                                        .Sum(s => s.Importe.Dbl());

                                    xml.IEPS = c33.Impuestos.Traslados.Traslado.Where(w => w.Impuesto == "003")
                                        .Sum(s => s.Importe.Dbl());

                                }


                                if (xml.TIPO_COMPROBANTE != null && (xml.TIPO_COMPROBANTE.ToLower() == "e" || xml.TIPO_COMPROBANTE.ToLower() == "egreso"))
                                {
                                    if (xml.IVA != null) xml.IVA = xml.IVA * -1;
                                    if (xml.IEPS != null) xml.IEPS = xml.IEPS * -1;
                                }


                        }

                        return JsonConvert.SerializeObject(
                            rs.Select(
                                s => new
                                {
                                    s.BLOQ,
                                    s.ID,
                                    s.RFC_EMISOR,
                                    s.CONTRA_RECIBO_ID,
                                    s.FECHA_FACTURA,
                                    s.FECHA_RECEPCION,
                                    s.FECHA_PROCESO,
                                    s.RAZON_SOCIAL_EMISOR,
                                    s.TOTAL,
                                    s.SUBTOTAL,
                                    s.IMPUESTOS,
                                    s.IVA,
                                    s.IEPS,
                                    s.UUID,
                                    s.TIPO_REL,
                                    s.SERIE,
                                    s.FOLIO,
                                    REVISADO = s.REVISADO ? "OK" : "",
                                    s.TIENDA,
                                    s.NO_COMPRA,
                                    s.FECHA_COMPRA,
                                    ESTATUS = s.ESTATUS2,
                                    ESTATUS_ID = s.ESTATUS
                                }).ToList());
                    }



                }

                return JsonConvert.SerializeObject(new ArrayList());
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new ArrayList());
            }

        }

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
                    var emp = Extensions.cEmpresa();

                    string sql="";


                    /*
                                 (SELECT DISTINCT 
                                    CAST(FECHA_PROCESO AS DATE) FECHA_PROCESO,FOLIOFISCAL
                                    FROM " + emp.ICG_BD + @".dbo.IT_RELFACTURAS_COMPRAS) ic
                     */

                    if ( req.OPT == 0 )
                    {
                        sql = @"
                        SELECT
                          case 
                              when CAST(xml.FECHA_FACTURA AS DATE) <= @FECHA_BLOQ then 'true'

                              ELSE 'false'
                          end BLOQ,

                          xml.ID,
                          xml.USER_ID,
                          xml.CONTRA_RECIBO_ID,
                          xml.OBSERVACION1,
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
                          --xml.TIENDA,
                          --xml.NO_COMPRA,
                          (
                                    SELECT STRING_AGG (T.ALBARAN,', ')  FROM
                                    (
                                         SELECT cc.SERIE + '-' + cc.FOLIO ALBARAN FROM CFDI_COMPRAS cc WHERE ID_XML = xml.ID 
                                         UNION ALL
                                          SELECT cx.TIENDA + '-' + cx.NO_COMPRA ALBARAN FROM CFDI_XML_PSI cx WHERE cx.ID=xml.ID
                                          AND NOT EXISTS (SELECT 1 FROM CFDI_COMPRAS cc2 WHERE cc2.SERIE = cx.TIENDA AND cc2.FOLIO = cx.NO_COMPRA)
                                    ) T
                          ) ALBARAN,

                          xml.FECHA_COMPRA,
                          xml.REVISADO,
                          xml.NOMBRE_ARCHIVO,
                          xml.ESTATUS,
                          es.ESTATUS AS ESTATUS2,
                          xml.TIPO_COMPROBANTE,
                          xml.INCIDENCIA,
                          xml.INCIDENCIA_MSG,
                          ic.FECHA_PROCESO,
                          b.CHECK_ID,
                          b.CHECK_NUMBER,
                          b.CHECK_DATE,
                          --ir.RETENCIONES
                          --xml.TOTAL_IMPUESTOS_RETENIDOS RETENCIONES


                          case 
                              when xml.TIPO_COMPROBANTE IS NULL then xml.TOTAL_IMPUESTOS_RETENIDOS
                              when LOWER(xml.TIPO_COMPROBANTE) = 'e' then xml.TOTAL_IMPUESTOS_RETENIDOS*-1
                              when LOWER(xml.TIPO_COMPROBANTE) = 'egreso' then xml.TOTAL_IMPUESTOS_RETENIDOS*-1
                              ELSE xml.TOTAL_IMPUESTOS_RETENIDOS
                          end RETENCIONES



                        FROM dbo.CFDI_XML_PSI xml WITH (NOLOCK)
                        INNER JOIN dbo.CFDI_ESTATUS es WITH (NOLOCK) ON xml.ESTATUS = es.ID
                        LEFT OUTER JOIN
                                 (SELECT DISTINCT 
                                    MAX(FECHA_PROCESO) FECHA_PROCESO,FOLIOFISCAL
                                    FROM " + emp.ICG_BD + @".dbo.IT_RELFACTURAS_COMPRAS WITH (NOLOCK) group by FOLIOFISCAL) ic
                                        ON xml.UUID = ic.FOLIOFISCAL

                        LEFT OUTER JOIN CFDI_GENERAL.dbo.XXROD_AP_PAYMENTS_BY_INVOICE b WITH (NOLOCK) ON xml.UUID = b.UUID


                        /*
                        LEFT OUTER JOIN
                                 (SELECT SUM(cr.IMPORTE) RETENCIONES, cr.ID
                                    FROM CFDI_RETENCIONES cr WITH (NOLOCK) WHERE cr.IMPUESTO='001' GROUP BY cr.ID) ir
                                        ON xml.ID = ir.ID*/

                        WHERE (@UUID<>'' OR FECHA_RECEPCION
                        BETWEEN CAST((@fecha1 + ' 00:00:00') AS DATETIME) AND CAST((@fecha2 + ' 23:59:59') AS DATETIME))
                        --AND xml.USER_ID = @USER_ID
                        AND (@RFC_EMISOR IS NULL OR xml.RFC_EMISOR = @RFC_EMISOR)

                        AND (@ESTATUS = 0 OR (@ESTATUS = 7 AND b.CHECK_ID IS NOT NULL) OR xml.ESTATUS = @ESTATUS)


                        AND (@FOLIO = '' OR UPPER(xml.FOLIO) LIKE '%' + UPPER(@FOLIO) + '%')
                        AND (@UUID = '' OR UPPER(xml.UUID) LIKE '%' + UPPER(@UUID) + '%')
                        AND (@SERIE = '' OR UPPER(xml.SERIE) LIKE '%' + UPPER(@SERIE) + '%')
                        AND (@CONTR = '' OR UPPER(xml.CONTRA_RECIBO_ID) = @CONTR)
                        AND (@A IS NULL OR @A = '1' AND NOT xml.CONTRA_RECIBO_ID IS NULL
                                        OR @A = '2' AND xml.CONTRA_RECIBO_ID IS NULL
                                        OR @A = '3' AND xml.INCIDENCIA = 1)
                        ORDER BY xml.CONTRA_RECIBO_ID DESC";
                    }
                    else
                    {
                        sql = @"
                        SELECT
                          case 
                              when CAST(xml.FECHA_FACTURA AS DATE) <= @FECHA_BLOQ then 'true'

                              ELSE 'false'
                          end BLOQ,

                          xml.ID,
                          xml.USER_ID,
                          xml.CONTRA_RECIBO_ID,
                          xml.OBSERVACION1,
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
                          xml.REVISADO,
                          --xml.TIENDA,
                          --xml.NO_COMPRA,
                          (
                                    SELECT STRING_AGG (T.ALBARAN,', ')  FROM
                                    (
                                         SELECT cc.SERIE + '-' + cc.FOLIO ALBARAN FROM CFDI_COMPRAS cc WHERE ID_XML = xml.ID 
                                         UNION ALL
                                          SELECT cx.TIENDA + '-' + cx.NO_COMPRA ALBARAN FROM CFDI_XML_PSI cx WHERE cx.ID=xml.ID
                                          AND NOT EXISTS (SELECT 1 FROM CFDI_COMPRAS cc2 WHERE cc2.SERIE = cx.TIENDA AND cc2.FOLIO = cx.NO_COMPRA)
                                    ) T
                          ) ALBARAN,
                          xml.FECHA_COMPRA,
                          xml.NOMBRE_ARCHIVO,
                          xml.ESTATUS,
                          es.ESTATUS AS ESTATUS2,
                          xml.TIPO_COMPROBANTE,
                          xml.INCIDENCIA,
                          xml.INCIDENCIA_MSG,
                          ic.FECHA_PROCESO,
                          b.CHECK_ID,
                          b.CHECK_NUMBER,
                          b.CHECK_DATE,
                          --ir.RETENCIONES
                          --xml.TOTAL_IMPUESTOS_RETENIDOS RETENCIONES

                          case 
                              when xml.TIPO_COMPROBANTE IS NULL then xml.TOTAL_IMPUESTOS_RETENIDOS
                              when LOWER(xml.TIPO_COMPROBANTE) = 'e' then xml.TOTAL_IMPUESTOS_RETENIDOS*-1
                              when LOWER(xml.TIPO_COMPROBANTE) = 'egreso' then xml.TOTAL_IMPUESTOS_RETENIDOS*-1
                              ELSE xml.TOTAL_IMPUESTOS_RETENIDOS
                          end RETENCIONES

                        FROM dbo.CFDI_XML_PSI xml WITH (NOLOCK)
                        INNER JOIN dbo.CFDI_ESTATUS es WITH (NOLOCK) ON xml.ESTATUS = es.ID
                        INNER JOIN
                                 (SELECT DISTINCT 
                                    MAX(FECHA_PROCESO) FECHA_PROCESO,FOLIOFISCAL
                                    FROM " + emp.ICG_BD + @".dbo.IT_RELFACTURAS_COMPRAS WITH (NOLOCK) group by FOLIOFISCAL) ic
                                        ON xml.UUID = ic.FOLIOFISCAL

                        LEFT OUTER JOIN CFDI_GENERAL.dbo.XXROD_AP_PAYMENTS_BY_INVOICE b WITH (NOLOCK) ON xml.UUID = b.UUID


                        /*
                        LEFT OUTER JOIN
                                 (SELECT SUM(cr.IMPORTE) RETENCIONES, cr.ID
                                    FROM CFDI_RETENCIONES cr WITH (NOLOCK) WHERE cr.IMPUESTO='001' GROUP BY cr.ID) ir
                                        ON xml.ID = ir.ID*/

                        WHERE (@UUID<>'' OR ic.FECHA_PROCESO
                        BETWEEN CAST((@fecha1 + ' 00:00:00') AS DATETIME) AND CAST((@fecha2 + ' 23:59:59') AS DATETIME) )
                        --AND xml.USER_ID = @USER_ID
                        AND (@RFC_EMISOR IS NULL OR xml.RFC_EMISOR = @RFC_EMISOR)

                        AND (@ESTATUS = 0 OR (@ESTATUS = 7 AND b.CHECK_ID IS NOT NULL) OR xml.ESTATUS = @ESTATUS)

                        AND (@FOLIO = '' OR UPPER(xml.FOLIO) LIKE '%' + UPPER(@FOLIO) + '%')
                        AND (@UUID = '' OR UPPER(xml.UUID) LIKE '%' + UPPER(@UUID) + '%')
                        AND (@SERIE = '' OR UPPER(xml.SERIE) LIKE '%' + UPPER(@SERIE) + '%')
                        AND (@CONTR = '' OR UPPER(xml.CONTRA_RECIBO_ID) = @CONTR)
                        AND (@A IS NULL OR @A = '1' AND NOT xml.CONTRA_RECIBO_ID IS NULL
                                        OR @A = '2' AND xml.CONTRA_RECIBO_ID IS NULL
                                        OR @A = '3' AND xml.INCIDENCIA = 1)
                        ORDER BY xml.CONTRA_RECIBO_ID DESC";
                    }

                    
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

                    if (req.ESTATUS == 8)
                    {
                        req.A       = "3";
                        req.ESTATUS = 1;
                    }


                    req.FECHA_BLOQ = GetFechaBloqueo();
                    var rs = con.Query<CFDI_XML>(sql, req).ToList();

                    foreach (var xml in rs)
                    {
                        if (xml.TIPO_COMPROBANTE != null && (xml.TIPO_COMPROBANTE.ToLower() == "e" || xml.TIPO_COMPROBANTE.ToLower() == "egreso"))
                        {
                            if (xml.TOTAL!=null)xml.TOTAL = xml.TOTAL * -1;
                            if (xml.SUBTOTAL != null) xml.SUBTOTAL = xml.SUBTOTAL * -1;
                            if (xml.IMPUESTOS != null) xml.IMPUESTOS = xml.IMPUESTOS * -1;

                        }

                    }


                    foreach (var xm in rs)
                    {
                        try
                        {
                            XML3.Comprobante c33 = new XML3.Comprobante();
                            //XML2.Comprobante c32 = new XML2.Comprobante();


                            c33 = c33.DeserializeStr(xm.XML);
                            //c32 = c32.DeserializeStr(xm.XML);

                            if (c33.Version != null)
                            {

                                if (c33.Impuestos != null && c33.Impuestos.Traslados != null && c33.Impuestos.Traslados.Traslado != null)
                                {
                                    xm.IVA = c33.Impuestos.Traslados.Traslado.Where(w => w.Impuesto == "002")
                                            .Sum(s => s.Importe.Dbl());

                                    xm.IEPS = c33.Impuestos.Traslados.Traslado.Where(w => w.Impuesto == "003")
                                            .Sum(s => s.Importe.Dbl());

                                }
                                /*else if (c33.Complemento != null && c33.Complemento.ImpuestosLocales != null && c33.Complemento.ImpuestosLocales.TrasladosLocales != null)
                                {
                                    var cImp = c33.Complemento.ImpuestosLocales.TrasladosLocales;

                                    if (cImp.ImpLocTrasladado.ToLower() == "ieps")
                                    {
                                        xm.IEPS = cImp.Importe.Dbl();
                                    }
                                }*/

                                if (c33.Descuento != null)
                                {
                                    xm.DESCUENTO = c33.Descuento.Dbl();
                                }

                                if (xm.TIPO_COMPROBANTE != null && (xm.TIPO_COMPROBANTE.ToLower() == "e" || xm.TIPO_COMPROBANTE.ToLower() == "egreso"))
                                {
                                    if (xm.IVA != null) xm.IVA = xm.IVA * -1;
                                    if (xm.IEPS != null) xm.IEPS = xm.IEPS * -1;
                                    if (xm.DESCUENTO !=0) xm.DESCUENTO = xm.DESCUENTO * -1;
                                }

                                if (c33.CfdiRelacionados != null)
                                {
                                    xm.TIPO_REL = c33.CfdiRelacionados.TipoRelacion;
                                }

                                



/*                                using (IDbConnection con2 = new SqlConnection(Extensions.GetBD()))
                                {

                                    IDbTransaction tran = null;
                                    

                                    try
                                    {
                                        con2.Open();
                                        tran = con2.BeginTransaction();


                                        sql = @"UPDATE CFDI_XML SET TIPO_COMPROBANTE = @TIPO_COMPROBANTE WHERE ID = @ID";


                                            con2.Execute(
                                                sql,
                                                new { ID = xm.ID, TIPO_COMPROBANTE = c33.TipoDeComprobante },
                                                tran);
                                        

                                        tran.Commit();

                                    }
                                    catch (Exception ex)
                                    {
                                        if (tran != null)
                                        {
                                            tran.Rollback();
                                        }
                                    }
                                }*/

                            }


                        }
                        catch (Exception e)
                        {
                            
                        }
                    }



                    //******************************************
                    //rs[0].IVA = 0;
                    //rs[0].IEPS = 0;
                    //rs[0].TOTAL = 0;
                    //rs[0].SUBTOTAL = 0;
                    //******************************************


                    return JsonConvert.SerializeObject(
                          rs.Select(
                                s => new
                                        {
                                           s.BLOQ,
                                           s.ID,
                                           s.OBSERVACION1,
                                           s.RFC_EMISOR,
                                           s.CONTRA_RECIBO_ID,
                                           s.FECHA_FACTURA,
                                           s.FECHA_RECEPCION,
                                           s.FECHA_PROCESO,
                                           s.RAZON_SOCIAL_EMISOR,
                                           s.TOTAL,
                                           s.SUBTOTAL,
                                           s.IMPUESTOS,
                                           s.IVA,
                                           s.IEPS,
                                           s.RETENCIONES,
                                           s.DESCUENTO,
                                           s.UUID,
                                           s.TIPO_REL,
                                           s.SERIE,
                                           s.FOLIO,
                                           REVISADO = s.REVISADO ? "OK" : "",
                                           //s.TIENDA,
                                           //s.NO_COMPRA,
                                           s.ALBARAN,
                                           s.FECHA_COMPRA,
                                           ESTATUS    = s.ESTATUS2,
                                           ESTATUS_ID = s.ESTATUS,
                                           s.CHECK_ID,
                                           s.CHECK_NUMBER,
                                           s.CHECK_DATE,
                                           INCIDENCIA = s.INCIDENCIA?"SI":"",
                                           INCIDENCIA_MSG = s.INCIDENCIA_MSG.Str()
                                        }).ToList());
                }
            }
            catch(Exception ex)
            {
                return JsonConvert.SerializeObject(new ArrayList());
            }
        }


        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string Detalle(COMPRAS data)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            try
            {

                Extensions.AddMap<ALBCOMPRALIN>();

                /*
                            SELECT REFERENCIA, DESCRIPCION, UNIDADESTOTAL,
                                   PRECIO, DTO, TOTAL, TIPOIMPUESTO, CODALMACEN
                            FROM ALBCOMPRALIN WITH(NOLOCK)
                            WHERE NUMSERIE = @NUMSERIE AND NUMALBARAN = @NUMALBARAN              
                 */

                using (IDbConnection con = new SqlConnection(Extensions.GetBD_ICG()))
                {
                    //dbConnection.Open(); , AC.CODCLIENTE, C.NOMBRECLIENTE
                    string sql = @"
--DECLARE @NUMSERIE NVARCHAR(4) = '142A'
--DECLARE @NUMALBARAN INT = 38655

DECLARE @FECHA DATETIME = (SELECT FECHAALBARAN FROM ALBCOMPRACAB WHERE NUMSERIE = @NUMSERIE AND NUMALBARAN = @NUMALBARAN)
SELECT REFERENCIA, DESCRIPCION, UNIDADESTOTAL, PRECIO, DTO, TOTAL, TIPOIMPUESTO, AL.CODALMACEN, @FECHA 'FECHA ALBARAN',
CASE WHEN CC.COSTOACTUALIZAR IS NULL THEN CA.ULTIMOCOSTE ELSE CC.COSTOACTUALIZAR END 'ULTIMO COSTO ACTUALIZADO',
CASE WHEN CC.COSTOACTUALIZAR IS NULL THEN '' ELSE CC.FECHAACTUALIZACION END 'FECHA ACTUALIZACION',
CASE WHEN CC.COSTOACTUALIZAR IS NULL THEN 'NO' ELSE 'SI' END 'CENTRALIZADOR DE COSTOS', AL.IVA, AL.REQ
FROM ALBCOMPRALIN AL WITH(NOLOCK)
LEFT JOIN COSTESPORALMACEN CA WITH(NOLOCK) ON AL.CODARTICULO = CA.CODARTICULO AND AL.CODALMACEN = CA.CODALMACEN
LEFT JOIN
(
SELECT
ROW_NUMBER() OVER (PARTITION BY CODARTICULO, CODALMACEN ORDER BY FECHAACTUALIZACION DESC) POS,
ID.CODARTICULO, ID.CODALMACEN, IA.FECHAACTUALIZACION, ID.COSTOACTUALIZAR
FROM
IT_ACTCOSTOS IA WITH(NOLOCK)
INNER JOIN IT_ACTCOSTOS_DET ID WITH(NOLOCK) ON IA.IDCARGA = ID.IDCARGA
WHERE STAPLICACION = 1 AND FECHAACTUALIZACION <= @FECHA
) CC ON CC.CODARTICULO = AL.CODARTICULO AND CC.CODALMACEN COLLATE Latin1_General_CS_AI = AL.CODALMACEN AND CC.POS = 1
WHERE NUMSERIE = @NUMSERIE AND NUMALBARAN = @NUMALBARAN

";

                    var rs = con.Query<ALBCOMPRALIN>(sql, new {
                                                                 data.NUMSERIE,
                                                                 data.NUMALBARAN }).ToList();

                    rs.ForEach(f =>
                    {

                        if (f.PRECIO != null && Math.Abs(f.PRECIO.Value - f.ULTIMO_COSTO_ACTUALIZADO) > 0.01)
                        {
                            f.COLOR = 1;
                        }


                        if (f.CENTRALIZADOR_DE_COSTOS == "NO")
                        {
                            f.FECHA_ACTUALIZACION_STR = "";
                        }
                        else
                        {

                            f.FECHA_ACTUALIZACION_STR = f.FECHA_ACTUALIZACION.ToString("dd/MM/yyyy");
                        }

                        if (f.DTO != null && f.DTO.Value > 0)
                        {
                            f.COLOR2 = 1;
                        }

                        //f.TOTAL = 423.01; //*******

                    });


                    //var dd = new DateTime();
                    //dd.ToString("HH:mm:ss tt zz")
                    return JsonConvert.SerializeObject(rs);
                }
            }
            catch
            {
                return JsonConvert.SerializeObject(new ArrayList());
            }
        }

        //        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        //        public static string Procesar(List<REQ1> facturas, List<REQ2> compras,  String fechaProd ) //double dif
        //        {
        //            HttpContext current = HttpContext.Current;
        //            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
        //            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
        //            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

        //            if( fechaProd != "" )
        //            {
        //               fechaProd = Convert.ToDateTime(fechaProd).ToString("yyyy/MM/dd");
        //            }


        //            var fechaProceso = "";
        //            var chk = GetChkFechaValidacion();
        //            var fecha = GetFechaValidacion();

        //            if (chk != null && chk.Value && fecha != null)
        //            {
        //                fechaProceso = fecha.Value.ToString("dd/MM/yyyy 17:30:00");
        //            }


        //            var msg = new Dictionary<string, string>();
        //            string json = "";



        //            string USER_NAME = "";
        //            if (current.User != null && current.User.Identity.IsAuthenticated)
        //            {
        //                FormsIdentity id = (FormsIdentity)current.User.Identity;
        //                FormsAuthenticationTicket ticket = id.Ticket;
        //                //role = ticket.UserData;
        //                USER_NAME = ticket.Name;
        //            }
        //            else
        //            {
        //                 msg.Add("code", "-1");
        //                 msg.Add("msg", "Inicie sesión nuevamente.");
        //                 return JsonConvert.SerializeObject(msg);
        //            }



        ///*            using (IDbConnection con = new SqlConnection(Extensions.GetBD()))
        //            {
        //                foreach (var factura in facturas)
        //                {
        //                    var rs = con.Query<CFDI_XML>(
        //                        "SELECT ESTATUS FROM CFDI_XML WHERE ID = @ID AND ESTATUS = 2",
        //                        new {ID = factura.ID}).ToList();

        //                    if (rs.Count > 0)
        //                    {
        //                        msg.Add("code", "-1");
        //                        msg.Add("msg", "No se pudo procesar ya que hay alguna factura que ya han sido asociada.");

        //                        return JsonConvert.SerializeObject(msg);
        //                    }
        //                }
        //            }*/

        //            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
        //            {

        //                foreach (var factura in facturas)
        //                {
        //                    var rs = con.Query<CFDI_XML>(
        //                        "SELECT ESTATUS FROM CFDI_XML_PSI WITH (NOLOCK) WHERE ID = @ID AND ESTATUS <> 1",
        //                        new { factura.ID }, commandTimeout: 6000).ToList();

        //                    if (rs.Count > 0)
        //                    {
        //                        msg.Add("code", "-1");
        //                        msg.Add("msg", "Solo pueden ser procesadas las facturas con estatus de Recibidas.");

        //                        return JsonConvert.SerializeObject(msg);
        //                    }
        //                }
        //            }

        //            /*
        //            var fB = GetFechaBloqueo();

        //            if (fB != null)
        //            {
        //                var bloq = facturas.Where(w => w.FECHA_FACTURA.Date <= fB.Value.Date).ToList();

        //                if (bloq.Count > 0)
        //                {
        //                    msg.Add("code", "-1");
        //                    msg.Add("msg",  "No se puede procesar, hay facturas bloqueadas.");

        //                    return JsonConvert.SerializeObject(msg);
        //                }
        //            }*/


        //            double dif = 0;
        //            try
        //            {
        //                double importe1 = Math.Round(facturas.Sum(s => s.TOTAL.Value), 2);
        //                double importe2 = Math.Round(compras.Sum(s => s.TOTAL.Value), 2);
        //                dif = Math.Round(importe1 - importe2, 2);
        //            }
        //            catch (Exception ex)
        //            {
        //                Log.Date();
        //                Log.Write("Error:" + ex.Message);

        //                foreach (var factura in facturas)
        //                {
        //                    //Log.Write("Factutas Total: " + factura.TOTAL.Value);
        //                }

        //                foreach (var compra in compras)
        //                {
        //                    //Log.Write("Compras Total: " + compra.TOTAL.Value);
        //                }

        //                msg.Add("code", "-1");
        //                msg.Add("msg", ex.Message);
        //                return JsonConvert.SerializeObject(msg);
        //            }


        //            /*
        //            foreach (var factura in facturas)
        //            {
        //                if (factura.FOLIO.Any(c => !char.IsDigit(c)))
        //                {
        //                    factura.SERIE += factura.FOLIO;
        //                    factura.FOLIO = null;
        //                }
        //            }*/



        //            /*msg.Add("code", "-1");
        //            msg.Add("msg", "Registros procesados correctamente.");

        //            return JsonConvert.SerializeObject(msg);*/




        //                using (IDbConnection con = new SqlConnection(Extensions.GetBD_ICG()))
        //                {
        //                    IDbTransaction tran = null;
        //                    IDbTransaction tran2 = null;
        //                    try
        //                    {
        //                        //var rs = con.Query<SERIES>("SELECT SERIE, DESCRIPCION FROM SERIES WHERE LEN(SERIE) = 2").ToList();
        //                        con.Open();

        //                        string sql = @"
        //                                    DELETE FROM IT_RELFACTURAS_COMPRAS WITH (Rowlock)
        //                                    WHERE 
        //                                        FECHA_PROCESO IS NULL
        //                                    AND
        //                                        @USUARIO IS NOT NULL
        //                                    AND
        //                                        @USUARIO <> ''
        //                                    AND
        //                                        USUARIO = @USUARIO";

        //                        con.Execute(sql, new { USUARIO = USER_NAME }, commandTimeout: 6000);

        //                        sql =
        //                            @"INSERT INTO IT_RELFACTURAS_COMPRAS WITH(ROWLOCK)
        //                              VALUES (@FOLIOFISCAL, @SERIEINTERNA, @FOLIOINTERNO, @LIN_FAC, @LIN, @NUMSERIE, @NUMALBARAN, NULL, 0, NULL, @USUARIO)";

        //                        if (facturas.Count == 1 && compras.Count == 1)
        //                        {

        //                            tran = con.BeginTransaction();
        //                            var rs = con.Execute(
        //                                sql,
        //                                new IT_RELFACTURAS_COMPRAS(
        //                                    facturas[0].UUID,
        //                                    facturas[0].SERIE,
        //                                    facturas[0].FOLIO,
        //                                    1,
        //                                    1,
        //                                    compras[0].NUMSERIE,
        //                                    compras[0].NUMALBARAN,
        //                                    USER_NAME),
        //                                tran, commandTimeout: 6000);

        //                            tran.Commit();
        //                            tran = null;

        //                            //--------------------------------------------------------//
        //                            var contr = facturas[0].CONTRA_RECIBO_ID == null
        //                                ? ""
        //                                : Extensions.GetSerie() + "-" + facturas[0].CONTRA_RECIBO_ID;

        //                            int brinco = 0;
        //                            brincar:

        //                            var err = con.ExecuteScalar<string>(
        //                                "Sp_IT_FacturasCompras",
        //                                new
        //                                {
        //                                    COMMIT = 0,
        //                                    EMPRESA = Extensions.GetEmpresa(),
        //                                    USUARIO = USER_NAME,
        //                                    CONTRARECIBO = contr,
        //                                    DIFERENCIA = dif,
        //                                    FECHA = fechaProceso
        //                                },
        //                                commandType: CommandType.StoredProcedure,
        //                                commandTimeout: 6000);


        //                            Thread.Sleep(5000);

        //                            if (err != null && (err.ToLower().Contains("interbloqueo") || err.ToLower().Contains("primary key") || err.ToLower().Contains("la subconsulta ha devuelto más de un valor")) && brinco < 2)
        //                            {
        //                                brinco ++;

        //                                //Thread.Sleep(1000);
        //                                goto brincar;
        //                                //throw new Exception(err);
        //                                //Log.Write("1:1 SP: " + err + "\r\n", false);
        //                            }
        //                            //--------------------------------------------------------//



        //                            sql = @"
        //                                    SELECT * FROM IT_RELFACTURAS_COMPRAS WITH (NOLOCK)
        //                                    WHERE 
        //                                        FOLIOFISCAL  = @FOLIOFISCAL 
        //                                    AND USUARIO      = @USUARIO";

        //                            var rs1 = con.Query<IT_RELFACTURAS_COMPRAS>(
        //                                sql,
        //                                new
        //                                {
        //                                    FOLIOFISCAL = facturas[0].UUID,
        //                                    USUARIO = USER_NAME
        //                                }, commandTimeout: 6000).ToList();

        //                            if (rs1.Count == 0 || rs1.All(a => a.FECHA_PROCESO == null))
        //                            {
        //                                if (brinco < 2)
        //                                {
        //                                    brinco++;
        //                                    goto brincar;
        //                                }


        //                                json = JsonConvert.SerializeObject(rs1);
        //                                throw new Exception("No se pudo procesar. " + err);
        //                            }

        //                            //--------------------------------------------------------//

        //                            using (IDbConnection con2 = new SqlConnection(Extensions.GetBD_CFDI()))
        //                            {
        //                                con2.Open();

        //                                tran2 = con2.BeginTransaction();
        //                                con2.Execute(
        //                                    "UPDATE CFDI_XML_PSI WITH (Rowlock) SET ESTATUS = 2 WHERE ID = @ID",
        //                                    new
        //                                    {
        //                                        facturas[0].ID
        //                                    },tran2, commandTimeout: 6000);
        //                                tran2.Commit();
        //                                tran2 = null;
        //                            }


        //                            using (IDbConnection con2 = new SqlConnection(Extensions.GetBD_ICG()))
        //                            {
        //                                var err2 = con2.ExecuteScalar<string>(
        //                                    "ITO_LOC_COMPRAS_VALIDADOR",
        //                                    new
        //                                    {
        //                                        UUID = facturas[0].UUID

        //                                    },
        //                                    commandType: CommandType.StoredProcedure,
        //                                    commandTimeout: 6000);
        //                            }

        //                            msg.Add("code", "0");
        //                            msg.Add("msg", "Registros procesados correctamente.");

        //                            return JsonConvert.SerializeObject(msg);
        //                        }

        //                        if (facturas.Count > 1 && compras.Count == 1)
        //                        {
        //                            int LIN_FAC = 1;
        //                            int LIN = 1;

        //                            tran = con.BeginTransaction();
        //                            foreach (var factura in facturas)
        //                            {
        //                                var rs = con.Execute(
        //                                    sql,
        //                                    new IT_RELFACTURAS_COMPRAS(
        //                                        factura.UUID,
        //                                        factura.SERIE,
        //                                        factura.FOLIO,
        //                                        LIN_FAC,
        //                                        LIN,
        //                                        compras[0].NUMSERIE,
        //                                        compras[0].NUMALBARAN,
        //                                        USER_NAME),
        //                                    tran, commandTimeout: 6000);
        //                                LIN_FAC++;
        //                                LIN = 0;
        //                            }

        //                            tran.Commit();
        //                            tran = null;

        //                            //--------------------------------------------------------//

        //                            var ctr = facturas.FirstOrDefault(f => f.CONTRA_RECIBO_ID != null);
        //                            var contr = "";
        //                            if (ctr != null)
        //                            {
        //                                if (facturas.All(f => f.CONTRA_RECIBO_ID == ctr.CONTRA_RECIBO_ID))
        //                                {
        //                                    contr = Extensions.GetSerie() + "-" + ctr.CONTRA_RECIBO_ID.Value.ToString();
        //                                }
        //                            }

        //                            //--------------------------------------------------------//

        //                            int brinco = 0;
        //                            brincar:

        //                            var err = con.ExecuteScalar<string>(
        //                                "Sp_IT_FacturasCompras",
        //                                new
        //                                {
        //                                    COMMIT = 0,
        //                                    EMPRESA = Extensions.GetEmpresa(),
        //                                    USUARIO = USER_NAME,
        //                                    CONTRARECIBO = contr,
        //                                    DIFERENCIA = dif,
        //                                    FECHA = fechaProceso
        //                                },
        //                                commandType: CommandType.StoredProcedure,
        //                                commandTimeout: 6000);


        //                            Thread.Sleep(5000);

        //                            if (err != null && (err.ToLower().Contains("interbloqueo") || err.ToLower().Contains("primary key") || err.ToLower().Contains("la subconsulta ha devuelto más de un valor")) && brinco < 2)
        //                            {
        //                                brinco ++;
        //                                //Thread.Sleep(1000);
        //                                goto brincar;
        //                                //throw new Exception(err);
        //                                //Log.Write("1:1 SP: " + err + "\r\n", false);
        //                            }
        //                            //--------------------------------------------------------//

        //                            sql = @"
        //                                    SELECT * FROM IT_RELFACTURAS_COMPRAS WITH (NOLOCK)
        //                                    WHERE 
        //                                        FOLIOFISCAL  = @FOLIOFISCAL 
        //                                    AND USUARIO      = @USUARIO";

        //                            foreach (var factura in facturas)
        //                            {
        //                                 var rs1 = con.Query<IT_RELFACTURAS_COMPRAS>(
        //                                           sql,
        //                                           new
        //                                           {
        //                                                FOLIOFISCAL = factura.UUID,
        //                                                USUARIO     = USER_NAME
        //                                           }, commandTimeout: 6000).ToList();

        //                                 if (rs1.Count == 0 || rs1.All(a => a.FECHA_PROCESO == null))
        //                                 {

        //                                     if (brinco < 2)
        //                                     {
        //                                         brinco++;
        //                                         goto brincar;
        //                                     }

        //                                     json = JsonConvert.SerializeObject(rs1);
        //                                     throw new Exception("No se pudo procesar. " + err);
        //                                 }
        //                            }

        //                            /* sql = @"
        //                                    SELECT * FROM IT_RELFACTURAS_COMPRAS
        //                                    WHERE 
        //                                        FOLIOFISCAL  = @FOLIOFISCAL 
        //                                    AND USUARIO      = @USUARIO";

        //                            bool procesado = false;

        //                            foreach( var factura in facturas )
        //                            {
        //                                 var rs1 = con.Query<IT_RELFACTURAS_COMPRAS>(
        //                                           sql,
        //                                           new{
        //                                                   FOLIOFISCAL  = factura.UUID,
        //                                                   USUARIO      = USER_NAME
        //                                              }).FirstOrDefault();


        //                                 if (rs1 != null && rs1.FECHA_PROCESO != null)
        //                                 {
        //                                      procesado = true;
        //                                 }

        //                            }

        //                            if (!procesado)
        //                            {
        //                                 Log.Write("T1: " + procesado + " Err", false);

        //                                 //throw new Exception("No se pudo procesar.");

        //                                 foreach (var factura in facturas)
        //                                 {
        //                                     var rs1 = con.Query<IT_RELFACTURAS_COMPRAS>(
        //                                               sql,
        //                                               new
        //                                               {
        //                                                   FOLIOFISCAL = factura.UUID,
        //                                                   USUARIO = USER_NAME
        //                                               }).FirstOrDefault();

        //                                     if (rs1 != null)
        //                                     {
        //                                          Log.Write(
        //                                                    "T1: FOLIOFISCAL: "
        //                                                    + rs1.FOLIOFISCAL
        //                                                    + " SERIEINTERNA: "
        //                                                    + rs1.SERIEINTERNA
        //                                                    + " FOLIOINTERNO: "
        //                                                    + rs1.FOLIOINTERNO
        //                                                    + " LIN_FAC: "
        //                                                    + LIN_FAC
        //                                                    + " LIN: "
        //                                                    + rs1.LIN
        //                                                    + " NUMSERIE: "
        //                                                    + rs1.NUMSERIE
        //                                                    + " NUMALBARAN: "
        //                                                    + rs1.NUMALBARAN
        //                                                    + " FECHAPROCESO:"
        //                                                    + rs1.FECHA_PROCESO,
        //                                                    false);
        //                                     }

        //                                 }
        //                            }*/
        //                            //--------------------------------------------------------//


        //                            using (IDbConnection con2 = new SqlConnection(Extensions.GetBD_ICG()))
        //                            {

        //                                foreach (var factura in facturas)
        //                                {
        //                                    var err2 = con2.ExecuteScalar<string>(
        //                                        "ITO_LOC_COMPRAS_VALIDADOR",
        //                                        new
        //                                        {
        //                                            UUID = factura.UUID

        //                                        },
        //                                        commandType: CommandType.StoredProcedure,
        //                                        commandTimeout: 6000);
        //                                }
        //                            }


        //                            using (IDbConnection con2 = new SqlConnection(Extensions.GetBD_CFDI()))
        //                            {
        //                                con2.Open();
        //                                tran2 = con2.BeginTransaction();
        //                                foreach (var factura in facturas)
        //                                {
        //                                    con2.Execute(
        //                                        "UPDATE CFDI_XML_PSI WITH (Rowlock) SET ESTATUS = 2 WHERE ID = @ID",
        //                                        new {
        //                                            factura.ID
        //                                        },tran2, commandTimeout: 6000);
        //                                }
        //                                tran2.Commit();
        //                                tran2 = null;
        //                            }


        //                            msg.Add("code", "0");
        //                            msg.Add("msg", "Registros procesados correctamente.");

        //                            return JsonConvert.SerializeObject(msg);
        //                        }

        //                        if (facturas.Count == 1 && compras.Count > 1)
        //                        {
        //                            int LIN_FAC = 1;
        //                            int LIN = 1;

        //                            tran = con.BeginTransaction();
        //                            foreach (var compra in compras)
        //                            {
        //                                var rs = con.Execute(
        //                                    sql,
        //                                    new IT_RELFACTURAS_COMPRAS(
        //                                        facturas[0].UUID,
        //                                        facturas[0].SERIE,
        //                                        facturas[0].FOLIO,
        //                                        LIN_FAC,
        //                                        LIN,
        //                                        compra.NUMSERIE,
        //                                        compra.NUMALBARAN,
        //                                        USER_NAME),
        //                                    tran, commandTimeout: 6000);
        //                                LIN++;
        //                            }

        //                            tran.Commit();
        //                            tran = null;

        //                            //--------------------------------------------------------//
        //                            var contr = facturas[0].CONTRA_RECIBO_ID == null
        //                                ? ""
        //                                : Extensions.GetSerie() + "-" + facturas[0].CONTRA_RECIBO_ID;


        //                            int brinco = 0;
        //                            brincar:

        //                            var err = con.ExecuteScalar<string>(
        //                                "Sp_IT_FacturasCompras",
        //                                new
        //                                {
        //                                    COMMIT = 0,
        //                                    EMPRESA = Extensions.GetEmpresa(),
        //                                    USUARIO = USER_NAME,
        //                                    CONTRARECIBO = contr,
        //                                    DIFERENCIA = dif,
        //                                    FECHA = fechaProceso
        //                                },
        //                                commandType: CommandType.StoredProcedure,
        //                                commandTimeout: 6000);


        //                            Thread.Sleep(5000);

        //                            if (err != null && (err.ToLower().Contains("interbloqueo") || err.ToLower().Contains("primary key") || err.ToLower().Contains("la subconsulta ha devuelto más de un valor")) && brinco < 2)
        //                            {
        //                                brinco ++;

        //                                //Thread.Sleep(1000);
        //                                goto brincar;
        //                                //throw new Exception(err);
        //                                //Log.Write("1:1 SP: " + err + "\r\n", false);
        //                            }
        //                            //--------------------------------------------------------//

        //                            sql = @"
        //                                    SELECT * FROM IT_RELFACTURAS_COMPRAS WITH (NOLOCK)
        //                                    WHERE 
        //                                        FOLIOFISCAL  = @FOLIOFISCAL 
        //                                    AND USUARIO      = @USUARIO";

        //                            var rs1 = con.Query<IT_RELFACTURAS_COMPRAS>(
        //                                      sql,
        //                                      new
        //                                      {
        //                                           FOLIOFISCAL = facturas[0].UUID,
        //                                           USUARIO     = USER_NAME
        //                                      }, commandTimeout: 6000).ToList();

        //                            if (rs1.Count == 0 || rs1.All(a => a.FECHA_PROCESO == null))
        //                            {

        //                                if (brinco < 2)
        //                                {
        //                                    brinco++;
        //                                    goto brincar;
        //                                }

        //                                json = JsonConvert.SerializeObject(rs1);
        //                                throw new Exception("No se pudo procesar. " + err);
        //                            }

        //                            /*
        //                            sql = @"
        //                                    SELECT * FROM IT_RELFACTURAS_COMPRAS
        //                                    WHERE 
        //                                        FOLIOFISCAL  = @FOLIOFISCAL 
        //                                    AND USUARIO      = @USUARIO";

        //                            bool procesado = false;

        //                            var rs1 = con.Query<IT_RELFACTURAS_COMPRAS>(
        //                                      sql,
        //                                      new {
        //                                               FOLIOFISCAL  = facturas[0].UUID,
        //                                               USUARIO      = USER_NAME
        //                                          }).ToList();

        //                            if (rs1.Count > 0)
        //                            {
        //                                 foreach( var it in rs1 )
        //                                 {
        //                                      if (it.FECHA_PROCESO != null)
        //                                      {
        //                                           procesado = true;
        //                                      }
        //                                 }
        //                            }

        //                            if (!procesado)
        //                            {
        //                                 Log.Write("T2: " + procesado + " Err", false);

        //                                 //throw new Exception("No se pudo procesar.");
        //                                 if (rs1.Count > 0)
        //                                 {
        //                                      foreach( var it in rs1 )
        //                                      {
        //                                           Log.Write(
        //                                                     "T2: FOLIOFISCAL: "
        //                                                     + it.FOLIOFISCAL
        //                                                     + " SERIEINTERNA: "
        //                                                     + it.SERIEINTERNA
        //                                                     + " FOLIOINTERNO: "
        //                                                     + it.FOLIOINTERNO
        //                                                     + " LIN_FAC: "
        //                                                     + LIN_FAC
        //                                                     + " LIN: "
        //                                                     + it.LIN
        //                                                     + " NUMSERIE: "
        //                                                     + it.NUMSERIE
        //                                                     + " NUMALBARAN: "
        //                                                     + it.NUMALBARAN
        //                                                     + " FECHAPROCESO:"
        //                                                     + it.FECHA_PROCESO,
        //                                                     false);
        //                                      }
        //                                 }
        //                            }*/
        //                            //--------------------------------------------------------//


        //                            using (IDbConnection con2 = new SqlConnection(Extensions.GetBD_CFDI()))
        //                            {
        //                                con2.Open();
        //                                tran2 = con2.BeginTransaction();
        //                                con2.Execute(
        //                                    "UPDATE CFDI_XML_PSI WITH (Rowlock) SET ESTATUS = 2 WHERE ID = @ID",
        //                                    new {
        //                                        facturas[0].ID
        //                                    },tran2, commandTimeout: 6000);
        //                                tran2.Commit();
        //                                tran2 = null;
        //                            }


        //                            using (IDbConnection con2 = new SqlConnection(Extensions.GetBD_ICG()))
        //                            {

        //                                var err2 = con2.ExecuteScalar<string>(
        //                                    "ITO_LOC_COMPRAS_VALIDADOR",
        //                                    new
        //                                    {
        //                                        UUID = facturas[0].UUID

        //                                    },
        //                                    commandType: CommandType.StoredProcedure,
        //                                    commandTimeout: 6000);
        //                            }

        //                            msg.Add("code", "0");
        //                            msg.Add("msg", "Registros procesados correctamente.");

        //                            return JsonConvert.SerializeObject(msg);
        //                        }

        //                        msg.Add("code", "-1");
        //                        msg.Add("msg", "No hay registros para procesar.");
        //                        return JsonConvert.SerializeObject(msg);
        //                    }
        //                    catch (Exception ex)
        //                    {

        //                        if (tran != null)
        //                        {
        //                            //tran.Rollback();
        //                        }

        //                        if (tran2 != null)
        //                        {
        //                            //tran2.Rollback();
        //                        }

        //                        try
        //                        {
        //                            /*
        //                            using (IDbConnection con2 = new SqlConnection(Extensions.GetBD_ICG()))
        //                            {
        //                                foreach (var factura in facturas)
        //                                {
        //                                    string sql = @"
        //                                    DELETE FROM IT_RELFACTURAS_COMPRAS
        //                                    WHERE 
        //                                        FOLIOFISCAL  = @FOLIOFISCAL 
        //                                    AND USUARIO      = @USUARIO
        //                                    AND SERIEINTERNA = @SERIEINTERNA
        //                                    AND FOLIOINTERNO = @FOLIOINTERNO";

        //                                    var rs = con2.Execute(
        //                                              sql,
        //                                              new {
        //                                                       FOLIOFISCAL  = factura.UUID,
        //                                                       USUARIO      = USER_NAME,
        //                                                       SERIEINTERNA = factura.SERIE,
        //                                                       FOLIOINTERNO = factura.FOLIO,
        //                                                  });
        //                                }
        //                            }
        //                            */

        //                            Log.Date();


        //                            if (facturas.Count > 1 && compras.Count == 1)
        //                            {
        //                                int LIN_FAC = 1;
        //                                int LIN = 1;

        //                                foreach (var factura in facturas)
        //                                {
        //                                    Log.Write(
        //                                        "FOLIOFISCAL: " + factura.UUID + " SERIEINTERNA: " + factura.SERIE +
        //                                        " FOLIOINTERNO: " + factura.FOLIO + " LIN_FAC: " + LIN_FAC +
        //                                        " LIN: " + LIN + " NUMSERIE: " + compras[0].NUMSERIE + " NUMALBARAN: " +
        //                                        compras[0].NUMALBARAN, false);

        //                                    LIN_FAC++;
        //                                    LIN = 0;
        //                                }

        //                                var contr = facturas[0].CONTRA_RECIBO_ID == null
        //                                    ? ""
        //                                    : Extensions.GetSerie() + "-" + facturas[0].CONTRA_RECIBO_ID;

        //                                Log.Write("\r\nCONTRARECIBO: " + contr + " DIF: " + dif + " USUARIO: " + USER_NAME + " EMPRESA: " + Extensions.GetEmpresa() + " RFC_EMISOR: " + facturas[0].RFC_EMISOR, false);

        //                                Log.Write("ERROR: " + ex.Message + "\r\n\r\n" + json + "\r\n", false);

        //                            }



        //                            if (facturas.Count == 1 && compras.Count == 1)
        //                            {

        //                                var contr = facturas[0].CONTRA_RECIBO_ID == null
        //                                    ? ""
        //                                    : Extensions.GetSerie() + "-" + facturas[0].CONTRA_RECIBO_ID;


        //                                Log.Write(
        //                                    "FOLIOFISCAL: " + facturas[0].UUID + " SERIEINTERNA: " + facturas[0].SERIE +
        //                                    " FOLIOINTERNO: " + facturas[0].FOLIO + " LIN_FAC: 1 LIN: 1 NUMSERIE: " +
        //                                    compras[0].NUMSERIE + " NUMALBARAN: " + compras[0].NUMALBARAN,
        //                                    false);

        //                                Log.Write("\r\nCONTRARECIBO: " + contr + " DIF: " + dif + " USUARIO: " + USER_NAME + " EMPRESA: " + Extensions.GetEmpresa() + " RFC_EMISOR: " + facturas[0].RFC_EMISOR, false);

        //                                Log.Write("ERROR: " + ex.Message + "\r\n\r\n" + json + "\r\n", false);

        //                            }


        //                            if (facturas.Count == 1 && compras.Count > 1)
        //                            {
        //                                int LIN_FAC = 1;
        //                                int LIN = 1;

        //                                foreach (var compra in compras)
        //                                {
        //                                    Log.Write(
        //                                        "FOLIOFISCAL: " + facturas[0].UUID + " SERIEINTERNA: " + facturas[0].SERIE +
        //                                        " FOLIOINTERNO: " + facturas[0].FOLIO + " LIN_FAC: " + LIN_FAC +
        //                                        " LIN: " + LIN + " NUMSERIE: " + compra.NUMSERIE + " NUMALBARAN: " +
        //                                        compra.NUMALBARAN, false);


        //                                    LIN++;
        //                                }

        //                                var contr = facturas[0].CONTRA_RECIBO_ID == null
        //                                    ? ""
        //                                    : Extensions.GetSerie() + "-" + facturas[0].CONTRA_RECIBO_ID;


        //                                Log.Write("\r\nCONTRARECIBO: " + contr + " DIF: " + dif + " USUARIO: " + USER_NAME + " EMPRESA: " + Extensions.GetEmpresa() + " RFC_EMISOR: " + facturas[0].RFC_EMISOR, false);

        //                                Log.Write("ERROR: " + ex.Message + "\r\n\r\n" + json + "\r\n", false);
        //                            }

        //                        }
        //                        catch (Exception e)
        //                        {

        //                        }


        //                        msg.Add("code", "-1");
        //                        msg.Add("msg", ex.Message);

        //                        return JsonConvert.SerializeObject(msg);
        //                    }
        //                    finally
        //                    {
        //                        if (con != null)
        //                        {
        //                            con.Close();
        //                        }
        //                    }
        //                }

        //        }




        // ─── Método auxiliar: verifica/reabre la conexión y renueva la transacción si es necesario ──
        private static IDbTransaction EnsureConnectionOpen(IDbConnection con, IDbTransaction tran = null)
        {
            // Si la conexión está bien y hay transacción activa, no hacer nada
            if (con.State == ConnectionState.Open && tran != null)
                return tran;

            // Si la conexión está rota o cerrada, intentar recuperarla
            if (con.State == ConnectionState.Broken || con.State == ConnectionState.Closed)
            {
                // Descartar la transacción anterior (ya no es válida en SQL Server)
                try { tran?.Dispose(); } catch { /* ignorar */ }

                // Cerrar si está en estado Broken
                if (con.State != ConnectionState.Closed)
                    con.Close();

                // Reabrir
                con.Open();

                // Iniciar nueva transacción solo si había una antes
                return tran != null ? con.BeginTransaction() : null;
            }

            // Conexión abierta pero sin transacción: solo abrir si estaba cerrada
            if (con.State == ConnectionState.Closed)
                con.Open();

            return tran;
        }


        // ─── Método auxiliar de reintento ────────────────────────────────────────────
        private static T ExecuteWithRetry<T>(Func<T> operation, string operationName, string userName, int maxRetries = 5)
        {
            int attempt = 0;

            while (true)
            {
                try
                {
                    return operation();
                }
                catch (Exception ex) when (EsErrorTransitorio(ex))
                {
                    attempt++;

                    Log.Write($"[REINTENTO {attempt}/{maxRetries}] Operación: {operationName} | Error: {ex.Message}", USER_NAME: userName);

                    if (attempt >= maxRetries)
                    {
                        Log.Write($"[AGOTADOS {maxRetries} REINTENTOS] Operación: {operationName} | Error final: {ex.Message}", USER_NAME: userName);
                        throw;
                    }

                    Thread.Sleep(2000 * attempt); // backoff incremental
                }
            }
        }

        private static bool EsErrorTransitorio(Exception ex)
        {
            if (ex is SqlException sqlEx)
            {
                if (sqlEx.Number == -2 || sqlEx.Number == 121)
                    return true;
            }

            var msg = ex.Message.ToLower();
            return msg.Contains("timeout") ||
                   msg.Contains("semáforo") ||
                   msg.Contains("tiempo de espera") ||
                   msg.Contains("se ha terminado la conexión") ||
                   msg.Contains("connection") ||
                   msg.Contains("transport");
        }

        private static void ExecuteWithRetry(Action operation, string operationName, string userName, int maxRetries = 5)
        {
            ExecuteWithRetry<object>(() => { operation(); return null; }, operationName, userName, maxRetries);
        }
        // ─────────────────────────────────────────────────────────────────────────────


        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string Procesar(List<REQ1> facturas, List<REQ2> compras, String fechaProd)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            if (fechaProd != "")
                fechaProd = Convert.ToDateTime(fechaProd).ToString("yyyy/MM/dd");

            var fechaProceso = "";
            var chk = GetChkFechaValidacion();
            var fecha = GetFechaValidacion();

            if (chk != null && chk.Value && fecha != null)
                fechaProceso = fecha.Value.ToString("dd/MM/yyyy 17:30:00");

            var msg = new Dictionary<string, string>();
            string json = "";
            string USER_NAME = "";

            // ── Autenticación ──────────────────────────────────────────────────────────
            if (current.User != null && current.User.Identity.IsAuthenticated)
            {
                FormsIdentity id = (FormsIdentity)current.User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;
                USER_NAME = ticket.Name;
                Log.Write("", USER_NAME: USER_NAME);
                Log.Write("========================== Inicio de Procesar() ==========================", USER_NAME: USER_NAME);
            }
            else
            {
                Log.Write("Usuario no autenticado intentó ejecutar Procesar()");
                msg.Add("code", "-1");
                msg.Add("msg", "Inicie sesión nuevamente.");
                return JsonConvert.SerializeObject(msg);
            }

            // ── Validar estatus de facturas (solo ESTATUS = 1 permitido) ───────────────
            Log.Write($"Validando estatus de {facturas.Count} factura(s).", USER_NAME: USER_NAME);
            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {
                foreach (var factura in facturas)
                {
                    var rs = ExecuteWithRetry(
                        () => con.Query<CFDI_XML>(
                            "SELECT ESTATUS FROM CFDI_XML_PSI WITH (NOLOCK) WHERE ID = @ID AND ESTATUS <> 1",
                            new { factura.ID }, commandTimeout: 6000).ToList(),
                        $"ValidarEstatusFactura ID={factura.ID}",
                        USER_NAME);

                    if (rs.Count > 0)
                    {
                        Log.Write($"Factura ID={factura.ID} tiene estatus inválido ({rs[0].ESTATUS}). Se cancela el proceso.", USER_NAME: USER_NAME);
                        msg.Add("code", "-1");
                        msg.Add("msg", "Solo pueden ser procesadas las facturas con estatus de Recibidas.");
                        return JsonConvert.SerializeObject(msg);
                    }
                }
            }

            // ── Cálculo de diferencia ──────────────────────────────────────────────────
            double dif = 0;
            try
            {
                double importe1 = Math.Round(facturas.Sum(s => s.TOTAL.Value), 2);
                double importe2 = Math.Round(compras.Sum(s => s.TOTAL.Value), 2);
                dif = Math.Round(importe1 - importe2, 2);
                Log.Write($"Diferencia calculada: {dif} (Facturas={importe1}, Compras={importe2})", USER_NAME: USER_NAME);
            }
            catch (Exception ex)
            {
                Log.Write($"Error calculando diferencia: {ex.Message}", USER_NAME: USER_NAME);
                msg.Add("code", "-1");
                msg.Add("msg", ex.Message);
                return JsonConvert.SerializeObject(msg);
            }

            // ── Proceso principal ──────────────────────────────────────────────────────
            using (IDbConnection con = new SqlConnection(Extensions.GetBD_ICG()))
            {
                IDbTransaction tran = null;
                try
                {
                    con.Open();
                    Log.Write($"Conexión ICG abierta. Facturas={facturas.Count}, Compras={compras.Count}", USER_NAME: USER_NAME);

                    // Limpiar registros sin fecha de proceso del usuario
                    string sqlDelete = @"
DELETE FROM IT_RELFACTURAS_COMPRAS WITH (Rowlock)
WHERE 
    FECHA_PROCESO IS NULL
AND @USUARIO IS NOT NULL
AND @USUARIO <> ''
AND USUARIO = @USUARIO";

                    ExecuteWithRetry(
                        () =>
                        {
                            EnsureConnectionOpen(con);
                            return con.Execute(sqlDelete, new { USUARIO = USER_NAME }, commandTimeout: 6000);
                        },
                        "DeleteRegistrosSinFechaProceso",
                        USER_NAME);

                    Log.Write("Registros previos sin fecha de proceso eliminados.", USER_NAME: USER_NAME);

                    string sqlInsert = @"
INSERT INTO IT_RELFACTURAS_COMPRAS WITH(ROWLOCK)
VALUES (@FOLIOFISCAL, @SERIEINTERNA, @FOLIOINTERNO, @LIN_FAC, @LIN,
        @NUMSERIE, @NUMALBARAN, NULL, 0, NULL, @USUARIO)";

                    // ════════════════════════════════════════════════════════════════════
                    // CASO 1 : 1 factura ↔ 1 compra
                    // ════════════════════════════════════════════════════════════════════
                    if (facturas.Count == 1 && compras.Count == 1)
                    {
                        Log.Write($"Caso 1:1 | UUID={facturas[0].UUID}", USER_NAME: USER_NAME);

                        tran = con.BeginTransaction();
                        ExecuteWithRetry(
                            () =>
                            {
                                tran = EnsureConnectionOpen(con, tran);
                                return con.Execute(
                                    sqlInsert,
                                    new IT_RELFACTURAS_COMPRAS(
                                        facturas[0].UUID, facturas[0].SERIE, facturas[0].FOLIO,
                                        1, 1,
                                        compras[0].NUMSERIE, compras[0].NUMALBARAN,
                                        USER_NAME),
                                    tran, commandTimeout: 6000);
                            },
                            "InsertRelFacturasCompras 1:1",
                            USER_NAME);
                        tran.Commit();
                        tran = null;
                        Log.Write($"Insert IT_RELFACTURAS_COMPRAS => FOLIOFISCAL={facturas[0].UUID} | SERIE={facturas[0].SERIE} | FOLIO={facturas[0].FOLIO} | LIN_FAC=1 | LIN=1 | NUMSERIE={compras[0].NUMSERIE} | NUMALBARAN={compras[0].NUMALBARAN} | USUARIO={USER_NAME}", USER_NAME: USER_NAME);
                        Log.Write("Insert 1:1 confirmado.", USER_NAME: USER_NAME);

                        var contr = facturas[0].CONTRA_RECIBO_ID == null
                            ? ""
                            : Extensions.GetSerie() + "-" + facturas[0].CONTRA_RECIBO_ID;

                        int brinco = 0;
                        string err;
                    brincar_1_1:
                        Log.Write($"Ejecutando Sp_IT_FacturasCompras (intento {brinco + 1}) | CONTRARECIBO={contr}, DIF={dif}, FECHA_PROCESO={fechaProceso ?? "(vacío)"}", USER_NAME: USER_NAME);

                        err = ExecuteWithRetry(
                            () =>
                            {
                                EnsureConnectionOpen(con);
                                return con.ExecuteScalar<string>(
                                    "Sp_IT_FacturasCompras",
                                    new
                                    {
                                        COMMIT = 0,
                                        EMPRESA = Extensions.GetEmpresa(),
                                        USUARIO = USER_NAME,
                                        CONTRARECIBO = contr,
                                        DIFERENCIA = dif,
                                        FECHA = fechaProceso
                                    },
                                    commandType: CommandType.StoredProcedure, commandTimeout: 6000);
                            },
                            "Sp_IT_FacturasCompras 1:1",
                            USER_NAME);

                        Log.Write($"Sp_IT_FacturasCompras 1:1 resultado raw: {err ?? "NULL"}", USER_NAME: USER_NAME);

                        Thread.Sleep(5000);

                        if (err != null &&
                            (err.ToLower().Contains("interbloqueo") ||
                             err.ToLower().Contains("primary key") ||
                             err.ToLower().Contains("la subconsulta ha devuelto más de un valor")) &&
                            brinco < 4)
                        {
                            Log.Write($"SP devolvió error recuperable (intento {brinco + 1}): {err}", USER_NAME: USER_NAME);
                            brinco++;
                            goto brincar_1_1;
                        }

                        if (err != null)
                            Log.Write($"Sp_IT_FacturasCompras resultado: {err}", USER_NAME: USER_NAME);

                        var rs1 = ExecuteWithRetry(
                            () =>
                            {
                                EnsureConnectionOpen(con);
                                return con.Query<IT_RELFACTURAS_COMPRAS>(@"
                    SELECT * FROM IT_RELFACTURAS_COMPRAS WITH (NOLOCK)
                    WHERE FOLIOFISCAL = @FOLIOFISCAL AND USUARIO = @USUARIO",
                                    new { FOLIOFISCAL = facturas[0].UUID, USUARIO = USER_NAME },
                                    commandTimeout: 6000).ToList();
                            },
                            "SelectRelFacturasCompras 1:1",
                            USER_NAME);

                        if (rs1.Count == 0 || rs1.All(a => a.FECHA_PROCESO == null))
                        {
                            if (brinco < 4)
                            {
                                Log.Write($"Sin fecha de proceso, reintentando SP (brinco={brinco + 1}).", USER_NAME: USER_NAME);
                                brinco++;
                                goto brincar_1_1;
                            }
                            json = JsonConvert.SerializeObject(rs1);
                            Log.Write($"No se pudo procesar 1:1. SP error: {err} | JSON: {json}", USER_NAME: USER_NAME);
                            throw new Exception("No se pudo procesar. " + err);
                        }

                        // Actualizar CFDI - CON RETRY Y SIN TRANSACCIÓN EXPLÍCITA
                        using (IDbConnection con2 = new SqlConnection(Extensions.GetBD_CFDI()))
                        {
                            ExecuteWithRetry(
                                () =>
                                {
                                    EnsureConnectionOpen(con2);
                                    return con2.Execute(
                                        "UPDATE CFDI_XML_PSI WITH (Rowlock, READPAST) SET ESTATUS = 2 WHERE ID = @ID",
                                        new { facturas[0].ID }, commandTimeout: 6000);
                                },
                                $"UpdateEstatusCFDI ID={facturas[0].ID}",
                                USER_NAME);
                            Log.Write($"CFDI ID={facturas[0].ID} actualizado a ESTATUS=2.", USER_NAME: USER_NAME);
                        }

                        // Validador
                        using (IDbConnection con2 = new SqlConnection(Extensions.GetBD_ICG()))
                        {
                            ExecuteWithRetry(
                                () =>
                                {
                                    EnsureConnectionOpen(con2);
                                    return con2.ExecuteScalar<string>(
                                        "ITO_LOC_COMPRAS_VALIDADOR",
                                        new { UUID = facturas[0].UUID },
                                        commandType: CommandType.StoredProcedure, commandTimeout: 6000);
                                },
                                $"ITO_LOC_COMPRAS_VALIDADOR UUID={facturas[0].UUID}",
                                USER_NAME);
                            Log.Write($"Ejecutando ITO_LOC_COMPRAS_VALIDADOR para UUID={facturas[0].UUID}.", USER_NAME: USER_NAME);
                        }

                        Log.Write("Proceso 1:1 completado correctamente.", USER_NAME: USER_NAME);
                        msg.Add("code", "0");
                        msg.Add("msg", "Registros procesados correctamente.");
                        return JsonConvert.SerializeObject(msg);
                    }

                    // ════════════════════════════════════════════════════════════════════
                    // CASO N : 1  (varias facturas, una compra)
                    // ════════════════════════════════════════════════════════════════════
                    if (facturas.Count > 1 && compras.Count == 1)
                    {
                        Log.Write($"Caso N:1 | Facturas={facturas.Count}, UUID_0={facturas[0].UUID}", USER_NAME: USER_NAME);

                        int LIN_FAC = 1;
                        tran = con.BeginTransaction();
                        foreach (var factura in facturas)
                        {
                            var f = factura;
                            int linFacLocal = LIN_FAC;
                            ExecuteWithRetry(
                                () =>
                                {
                                    tran = EnsureConnectionOpen(con, tran);
                                    return con.Execute(
                                        sqlInsert,
                                        new IT_RELFACTURAS_COMPRAS(
                                            f.UUID, f.SERIE, f.FOLIO,
                                            linFacLocal, linFacLocal == 1 ? 1 : 0,
                                            compras[0].NUMSERIE, compras[0].NUMALBARAN,
                                            USER_NAME),
                                        tran, commandTimeout: 6000);
                                },
                                $"InsertRelFacturasCompras N:1 UUID={f.UUID}",
                                USER_NAME);
                            Log.Write($"Insert IT_RELFACTURAS_COMPRAS => FOLIOFISCAL={f.UUID} | SERIE={f.SERIE} | FOLIO={f.FOLIO} | LIN_FAC={linFacLocal} | LIN={(linFacLocal == 1 ? 1 : 0)} | NUMSERIE={compras[0].NUMSERIE} | NUMALBARAN={compras[0].NUMALBARAN} | USUARIO={USER_NAME}", USER_NAME: USER_NAME);
                            LIN_FAC++;
                        }
                        tran.Commit();
                        tran = null;
                        Log.Write("Inserts N:1 confirmados.", USER_NAME: USER_NAME);

                        var ctr = facturas.FirstOrDefault(f => f.CONTRA_RECIBO_ID != null);
                        var contr = "";
                        if (ctr != null && facturas.All(f => f.CONTRA_RECIBO_ID == ctr.CONTRA_RECIBO_ID))
                            contr = Extensions.GetSerie() + "-" + ctr.CONTRA_RECIBO_ID.Value;

                        int brinco = 0;
                        string err;
                    brincar_N_1:
                        Log.Write($"Ejecutando Sp_IT_FacturasCompras N:1 (intento {brinco + 1}) | CONTRARECIBO={contr}, DIF={dif}, FECHA_PROCESO={fechaProceso ?? "(vacío)"}", USER_NAME: USER_NAME);

                        err = ExecuteWithRetry(
                            () =>
                            {
                                EnsureConnectionOpen(con);
                                return con.ExecuteScalar<string>(
                                    "Sp_IT_FacturasCompras",
                                    new
                                    {
                                        COMMIT = 0,
                                        EMPRESA = Extensions.GetEmpresa(),
                                        USUARIO = USER_NAME,
                                        CONTRARECIBO = contr,
                                        DIFERENCIA = dif,
                                        FECHA = fechaProceso
                                    },
                                    commandType: CommandType.StoredProcedure, commandTimeout: 6000);
                            },
                            "Sp_IT_FacturasCompras N:1",
                            USER_NAME);

                        Log.Write($"Sp_IT_FacturasCompras N:1 resultado raw: {err ?? "NULL"}", USER_NAME: USER_NAME);

                        Thread.Sleep(5000);

                        if (err != null &&
                            (err.ToLower().Contains("interbloqueo") ||
                             err.ToLower().Contains("primary key") ||
                             err.ToLower().Contains("la subconsulta ha devuelto más de un valor")) &&
                            brinco < 4)
                        {
                            Log.Write($"SP N:1 error recuperable (intento {brinco + 1}): {err}", USER_NAME: USER_NAME);
                            brinco++;
                            goto brincar_N_1;
                        }

                        if (err != null)
                            Log.Write($"Sp_IT_FacturasCompras N:1 resultado: {err}", USER_NAME: USER_NAME);

                        foreach (var factura in facturas)
                        {
                            var rs1 = ExecuteWithRetry(
                                () =>
                                {
                                    EnsureConnectionOpen(con);
                                    return con.Query<IT_RELFACTURAS_COMPRAS>(@"
                        SELECT * FROM IT_RELFACTURAS_COMPRAS WITH (NOLOCK)
                        WHERE FOLIOFISCAL = @FOLIOFISCAL AND USUARIO = @USUARIO",
                                        new { FOLIOFISCAL = factura.UUID, USUARIO = USER_NAME },
                                        commandTimeout: 6000).ToList();
                                },
                                $"SelectRelFacturasCompras N:1 UUID={factura.UUID}",
                                USER_NAME);

                            if (rs1.Count == 0 || rs1.All(a => a.FECHA_PROCESO == null))
                            {
                                if (brinco < 4)
                                {
                                    Log.Write($"UUID={factura.UUID} sin fecha de proceso, reintentando (brinco={brinco + 1}).", USER_NAME: USER_NAME);
                                    brinco++;
                                    goto brincar_N_1;
                                }
                                json = JsonConvert.SerializeObject(rs1);
                                Log.Write($"No se pudo procesar N:1 UUID={factura.UUID}. SP error: {err} | JSON: {json}", USER_NAME: USER_NAME);
                                throw new Exception("No se pudo procesar. " + err);
                            }
                        }

                        // Validador
                        using (IDbConnection con2 = new SqlConnection(Extensions.GetBD_ICG()))
                        {
                            foreach (var factura in facturas)
                            {
                                var f = factura;
                                ExecuteWithRetry(
                                    () =>
                                    {
                                        EnsureConnectionOpen(con2);
                                        return con2.ExecuteScalar<string>(
                                            "ITO_LOC_COMPRAS_VALIDADOR",
                                            new { UUID = f.UUID },
                                            commandType: CommandType.StoredProcedure, commandTimeout: 6000);
                                    },
                                    $"ITO_LOC_COMPRAS_VALIDADOR UUID={f.UUID}",
                                    USER_NAME);
                                Log.Write($"Ejecutando ITO_LOC_COMPRAS_VALIDADOR para UUID={f.UUID}.", USER_NAME: USER_NAME);
                            }
                        }

                        // Actualizar CFDI - CON RETRY Y SIN TRANSACCIÓN EXPLÍCITA
                        using (IDbConnection con2 = new SqlConnection(Extensions.GetBD_CFDI()))
                        {
                            foreach (var factura in facturas)
                            {
                                var f = factura;
                                ExecuteWithRetry(
                                    () =>
                                    {
                                        EnsureConnectionOpen(con2);
                                        return con2.Execute(
                                            "UPDATE CFDI_XML_PSI WITH (Rowlock, READPAST) SET ESTATUS = 2 WHERE ID = @ID",
                                            new { f.ID }, commandTimeout: 6000);
                                    },
                                    $"UpdateEstatusCFDI N:1 ID={f.ID}",
                                    USER_NAME);
                                Log.Write($"CFDI ID={f.ID} actualizado a ESTATUS=2.", USER_NAME: USER_NAME);
                            }
                        }

                        Log.Write("Proceso N:1 completado correctamente.", USER_NAME: USER_NAME);
                        msg.Add("code", "0");
                        msg.Add("msg", "Registros procesados correctamente.");
                        return JsonConvert.SerializeObject(msg);
                    }

                    // ════════════════════════════════════════════════════════════════════
                    // CASO 1 : N  (una factura, varias compras)
                    // ════════════════════════════════════════════════════════════════════
                    if (facturas.Count == 1 && compras.Count > 1)
                    {
                        Log.Write($"Caso 1:N | UUID={facturas[0].UUID}, Compras={compras.Count}", USER_NAME: USER_NAME);

                        int LIN = 1;
                        tran = con.BeginTransaction();
                        foreach (var compra in compras)
                        {
                            var c = compra;
                            int linLocal = LIN;
                            ExecuteWithRetry(
                                () =>
                                {
                                    tran = EnsureConnectionOpen(con, tran);
                                    return con.Execute(
                                        sqlInsert,
                                        new IT_RELFACTURAS_COMPRAS(
                                            facturas[0].UUID, facturas[0].SERIE, facturas[0].FOLIO,
                                            1, linLocal,
                                            c.NUMSERIE, c.NUMALBARAN,
                                            USER_NAME),
                                        tran, commandTimeout: 6000);
                                },
                                $"InsertRelFacturasCompras 1:N NUMALBARAN={c.NUMALBARAN}",
                                USER_NAME);
                            Log.Write($"Insert IT_RELFACTURAS_COMPRAS => FOLIOFISCAL={facturas[0].UUID} | SERIE={facturas[0].SERIE} | FOLIO={facturas[0].FOLIO} | LIN_FAC=1 | LIN={linLocal} | NUMSERIE={c.NUMSERIE} | NUMALBARAN={c.NUMALBARAN} | USUARIO={USER_NAME}", USER_NAME: USER_NAME);
                            LIN++;
                        }
                        tran.Commit();
                        tran = null;
                        Log.Write("Inserts 1:N confirmados.", USER_NAME: USER_NAME);

                        var contr = facturas[0].CONTRA_RECIBO_ID == null
                            ? ""
                            : Extensions.GetSerie() + "-" + facturas[0].CONTRA_RECIBO_ID;

                        int brinco = 0;
                        string err;
                    brincar_1_N:
                        Log.Write($"Ejecutando Sp_IT_FacturasCompras 1:N (intento {brinco + 1}) | CONTRARECIBO={contr}, DIF={dif}, FECHA_PROCESO={fechaProceso ?? "(vacío)"}", USER_NAME: USER_NAME);

                        err = ExecuteWithRetry(
                            () =>
                            {
                                EnsureConnectionOpen(con);
                                return con.ExecuteScalar<string>(
                                    "Sp_IT_FacturasCompras",
                                    new
                                    {
                                        COMMIT = 0,
                                        EMPRESA = Extensions.GetEmpresa(),
                                        USUARIO = USER_NAME,
                                        CONTRARECIBO = contr,
                                        DIFERENCIA = dif,
                                        FECHA = fechaProceso
                                    },
                                    commandType: CommandType.StoredProcedure, commandTimeout: 6000);
                            },
                            "Sp_IT_FacturasCompras 1:N",
                            USER_NAME);

                        Log.Write($"Sp_IT_FacturasCompras 1:N resultado raw: {err ?? "NULL"}", USER_NAME: USER_NAME);

                        Thread.Sleep(5000);

                        if (err != null &&
                            (err.ToLower().Contains("interbloqueo") ||
                             err.ToLower().Contains("primary key") ||
                             err.ToLower().Contains("la subconsulta ha devuelto más de un valor")) &&
                            brinco < 4)
                        {
                            Log.Write($"SP 1:N error recuperable (intento {brinco + 1}): {err}", USER_NAME: USER_NAME);
                            brinco++;
                            goto brincar_1_N;
                        }

                        if (err != null)
                            Log.Write($"Sp_IT_FacturasCompras 1:N resultado: {err}", USER_NAME: USER_NAME);

                        var rs1_1N = ExecuteWithRetry(
                            () =>
                            {
                                EnsureConnectionOpen(con);
                                return con.Query<IT_RELFACTURAS_COMPRAS>(@"
                    SELECT * FROM IT_RELFACTURAS_COMPRAS WITH (NOLOCK)
                    WHERE FOLIOFISCAL = @FOLIOFISCAL AND USUARIO = @USUARIO",
                                    new { FOLIOFISCAL = facturas[0].UUID, USUARIO = USER_NAME },
                                    commandTimeout: 6000).ToList();
                            },
                            "SelectRelFacturasCompras 1:N",
                            USER_NAME);

                        if (rs1_1N.Count == 0 || rs1_1N.All(a => a.FECHA_PROCESO == null))
                        {
                            if (brinco < 4)
                            {
                                Log.Write($"Sin fecha de proceso 1:N, reintentando (brinco={brinco + 1}).", USER_NAME: USER_NAME);
                                brinco++;
                                goto brincar_1_N;
                            }
                            json = JsonConvert.SerializeObject(rs1_1N);
                            Log.Write($"No se pudo procesar 1:N. SP error: {err} | JSON: {json}", USER_NAME: USER_NAME);
                            throw new Exception("No se pudo procesar. " + err);
                        }

                        // Actualizar CFDI - CON RETRY Y SIN TRANSACCIÓN EXPLÍCITA
                        using (IDbConnection con2 = new SqlConnection(Extensions.GetBD_CFDI()))
                        {
                            ExecuteWithRetry(
                                () =>
                                {
                                    EnsureConnectionOpen(con2);
                                    return con2.Execute(
                                        "UPDATE CFDI_XML_PSI WITH (Rowlock, READPAST) SET ESTATUS = 2 WHERE ID = @ID",
                                        new { facturas[0].ID }, commandTimeout: 6000);
                                },
                                $"UpdateEstatusCFDI 1:N ID={facturas[0].ID}",
                                USER_NAME);
                            Log.Write($"CFDI ID={facturas[0].ID} actualizado a ESTATUS=2.", USER_NAME: USER_NAME);
                        }

                        // Validador
                        using (IDbConnection con2 = new SqlConnection(Extensions.GetBD_ICG()))
                        {
                            ExecuteWithRetry(
                                () =>
                                {
                                    EnsureConnectionOpen(con2);
                                    return con2.ExecuteScalar<string>(
                                        "ITO_LOC_COMPRAS_VALIDADOR",
                                        new { UUID = facturas[0].UUID },
                                        commandType: CommandType.StoredProcedure, commandTimeout: 6000);
                                },
                                $"ITO_LOC_COMPRAS_VALIDADOR 1:N UUID={facturas[0].UUID}",
                                USER_NAME);
                            Log.Write($"Ejecutando ITO_LOC_COMPRAS_VALIDADOR para UUID={facturas[0].UUID}.", USER_NAME: USER_NAME);
                        }

                        Log.Write("Proceso 1:N completado correctamente.", USER_NAME: USER_NAME);
                        msg.Add("code", "0");
                        msg.Add("msg", "Registros procesados correctamente.");
                        return JsonConvert.SerializeObject(msg);
                    }

                    // Sin registros
                    Log.Write($"Sin caso válido. Facturas={facturas.Count}, Compras={compras.Count}", USER_NAME: USER_NAME);
                    msg.Add("code", "-1");
                    msg.Add("msg", "No hay registros para procesar.");
                    return JsonConvert.SerializeObject(msg);
                }
                catch (Exception ex)
                {
                    // Rollback explícito de transacciones pendientes
                    try { if (tran != null && tran.Connection != null) tran.Rollback(); } catch { }

                    try
                    {
                        Log.Date();

                        // Operador ?. en todos los accesos a propiedades
                        if (facturas?.Count > 1 && compras?.Count == 1)
                        {
                            int lf = 1, l = 1;
                            foreach (var factura in facturas)
                            {
                                Log.Write($"FOLIOFISCAL={factura?.UUID} SERIE={factura?.SERIE} FOLIO={factura?.FOLIO} LIN_FAC={lf} LIN={l} NUMSERIE={compras[0]?.NUMSERIE} NUMALBARAN={compras[0]?.NUMALBARAN}", false, USER_NAME);
                                lf++; l = 0;
                            }
                            var contr = facturas[0]?.CONTRA_RECIBO_ID == null
                                ? ""
                                : (Extensions.GetSerie() ?? "") + "-" + facturas[0].CONTRA_RECIBO_ID;
                            Log.Write($"CONTRARECIBO={contr} DIF={dif} EMPRESA={Extensions.GetEmpresa()} RFC_EMISOR={facturas[0]?.RFC_EMISOR ?? "NULL"}", false, USER_NAME);
                        }
                        else if (facturas?.Count == 1 && compras?.Count == 1)
                        {
                            var contr = facturas[0]?.CONTRA_RECIBO_ID == null
                                ? ""
                                : (Extensions.GetSerie() ?? "") + "-" + facturas[0].CONTRA_RECIBO_ID;
                            Log.Write($"FOLIOFISCAL={facturas[0]?.UUID} SERIE={facturas[0]?.SERIE} FOLIO={facturas[0]?.FOLIO} LIN_FAC=1 LIN=1 NUMSERIE={compras[0]?.NUMSERIE} NUMALBARAN={compras[0]?.NUMALBARAN}", false, USER_NAME);
                            Log.Write($"CONTRARECIBO={contr} DIF={dif} EMPRESA={Extensions.GetEmpresa()} RFC_EMISOR={facturas[0]?.RFC_EMISOR ?? "NULL"}", false, USER_NAME);
                        }
                        else if (facturas?.Count == 1 && compras?.Count > 1)
                        {
                            int l = 1;
                            foreach (var compra in compras)
                            {
                                Log.Write($"FOLIOFISCAL={facturas[0]?.UUID} SERIE={facturas[0]?.SERIE} FOLIO={facturas[0]?.FOLIO} LIN_FAC=1 LIN={l} NUMSERIE={compra?.NUMSERIE} NUMALBARAN={compra?.NUMALBARAN}", false, USER_NAME);
                                l++;
                            }
                            var contr = facturas[0]?.CONTRA_RECIBO_ID == null
                                ? ""
                                : (Extensions.GetSerie() ?? "") + "-" + facturas[0].CONTRA_RECIBO_ID;
                            Log.Write($"CONTRARECIBO={contr} DIF={dif} EMPRESA={Extensions.GetEmpresa()} RFC_EMISOR={facturas[0]?.RFC_EMISOR ?? "NULL"}", false, USER_NAME);
                        }

                        // Incluir StackTrace para diagnóstico preciso
                        Log.Write($"ERROR: {ex.Message}\r\n\r\nSTACKTRACE:\r\n{ex.StackTrace}\r\n\r\nJSON:\r\n{json}", false, USER_NAME);
                    }
                    catch (Exception logEx)
                    {
                        // Capturar errores del propio bloque de log
                        try { Log.Write($"ERROR EN BLOQUE CATCH (log fallido): {logEx.Message}\r\n{logEx.StackTrace}"); } catch { }
                    }

                    msg.Add("code", "-1");
                    msg.Add("msg", ex.Message);
                    return JsonConvert.SerializeObject(msg);
                }
                finally
                {
                    try { con?.Close(); } catch { }
                    Log.Write("Conexión ICG cerrada.", USER_NAME: USER_NAME);
                }
            }
        }



        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string SetDesvalidarContr(string contrarecibo)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            var msg = new Dictionary<string, string>();

            string USER_NAME = "";
            if (current.User != null && current.User.Identity.IsAuthenticated)
            {
                FormsIdentity             id     = (FormsIdentity) current.User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;
                //role = ticket.UserData;
                USER_NAME = ticket.Name;
            }
            else
            {
                msg.Add("code", "-1");
                msg.Add("msg", "Inicie sesión nuevamente.");
                return JsonConvert.SerializeObject(msg);
            }


            using (IDbConnection con = new SqlConnection(Extensions.GetBD_ICG()))
            {
                //var rs = con.Query<SERIES>("SELECT SERIE, DESCRIPCION FROM SERIES WHERE LEN(SERIE) = 2").ToList();

                IDbTransaction tran = null;
                int            rs   = 0;

                try
                {

                    if (contrarecibo == null || contrarecibo.Trim() == "")
                    {
                        throw new Exception("Contrarecibo no valido.");
                    }

                    //var timer = new Stopwatch();
                    //timer.Start();


                    //con.Open();
                    //using (SqlCommand cmd = new SqlCommand("sp_recompile", (SqlConnection)con))
                    //{
                    //    cmd.CommandType                                               = CommandType.StoredProcedure;
                    //    cmd.Parameters.Add("@objname", SqlDbType.NVarChar, 776).Value = "Sp_IT_FacturasCompras_Des";
                    //    cmd.ExecuteNonQuery();
                    //}

                    ////con.Execute("EXEC sp_recompile 'Sp_IT_FacturasCompras_Des'  ");
                    //con.Execute("SET ARITHABORT ON");




                    var err = con.ExecuteScalar<string>(
                            "Sp_IT_FacturasCompras_Des",
                            new
                            {
                                COMMIT = 0,
                                USUARIO = USER_NAME,
                                CONTRARECIBO = contrarecibo,
                                UUID = ""
                            },
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 6000);



                    using (IDbConnection con2 = new SqlConnection(Extensions.GetBD_CFDI()))
                    {
                        var sql = @"

                        INSERT INTO LOG_CARTA_PAGO WITH (Rowlock) (FECHA_MODIFICACION, HORA_MODIFICACION, USUARIO, ESTATUS, CONTRARECIBO)
                          VALUES(@FECHA_MODIFICACION, @HORA_MODIFICACION, @USUARIO, @ESTATUS, @CONTRARECIBO)";

                        var fecha = DateTime.Now;
                        con2.Execute(sql, new
                        {
                            FECHA_MODIFICACION = fecha.Date,
                            HORA_MODIFICACION  = fecha.ToString("HH:mm:ss"),
                            USUARIO            = USER_NAME,
                            ESTATUS            = "3",
                            CONTRARECIBO       = contrarecibo
                        });
                    }


                    //timer.Stop();

                    //TimeSpan timeTaken = timer.Elapsed;
                    //string   foo       = "Time taken: " + DateTime.Now.ToString("dd/MM/yyyy ") +  timeTaken.ToString(@"m\:ss\.fff");

                    //string dirPath = HttpContext.Current.Server.MapPath("~/log");
                    //string path    = dirPath + "\\Log_Ctr.txt";

                    //string       line = "[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] : " + msg + "\r\n";
                    //StreamWriter file = new StreamWriter(path, true, Encoding.Default);
                    //file.WriteLine(foo);
                    //file.Close();

                    return JsonConvert.SerializeObject(new {code = 0, msg = "El contrarecibo se ha desvalidado."});

                }
                catch (Exception ex)
                {
                    if (tran != null)
                    {
                        tran.Rollback();
                    }

                    return JsonConvert.SerializeObject(new {code = -1, msg = ex.Message});
                }
            }
        }



        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string SetDesvalidarUUID(string contrarecibo, string uuid)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            var msg = new Dictionary<string, string>();

            string USER_NAME = "";
            if (current.User != null && current.User.Identity.IsAuthenticated)
            {
                FormsIdentity             id     = (FormsIdentity) current.User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;
                //role = ticket.UserData;
                USER_NAME = ticket.Name;
            }
            else
            {
                msg.Add("code", "-1");
                msg.Add("msg", "Inicie sesión nuevamente.");
                return JsonConvert.SerializeObject(msg);
            }


            using (IDbConnection con = new SqlConnection(Extensions.GetBD_ICG()))
            {
                //var rs = con.Query<SERIES>("SELECT SERIE, DESCRIPCION FROM SERIES WHERE LEN(SERIE) = 2").ToList();

                IDbTransaction tran = null;
                int            rs   = 0;

                try
                {

                    if (contrarecibo == null || contrarecibo.Trim() == "")
                    {
                        throw new Exception("Contrarecibo no valido.");
                    }

                    if (uuid == null || uuid.Trim() == "")
                    {
                        throw new Exception("UUID no valido.");
                    }


                    //string dirPath = HttpContext.Current.Server.MapPath("~/log");
                    //string path    = dirPath + "\\Log_UUID.txt";



                    //StreamWriter file2 = new StreamWriter(path, true, Encoding.Default);
                    //file2.WriteLine("A:1");
                    //file2.Close();



                    //var timer = new Stopwatch();
                    //timer.Start();



                    var err = con.ExecuteScalar<string>(
                            "Sp_IT_FacturasCompras_Des",
                            new
                            {
                                COMMIT = 0,
                                USUARIO = USER_NAME,
                                CONTRARECIBO = contrarecibo,
                                UUID = uuid
                            },
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 6000);




                    using (IDbConnection con2 = new SqlConnection(Extensions.GetBD_CFDI()))
                    {
                        var sql = @"

                    INSERT INTO LOG_CARTA_PAGO WITH (Rowlock) (FECHA_MODIFICACION, HORA_MODIFICACION, USUARIO, ESTATUS, UUID)
                      VALUES(@FECHA_MODIFICACION, @HORA_MODIFICACION, @USUARIO, @ESTATUS, @UUID)";

                        var fecha = DateTime.Now;
                        con2.Execute(sql, new
                        {
                            FECHA_MODIFICACION = fecha.Date,
                            HORA_MODIFICACION = fecha.ToString("HH:mm:ss"),
                            USUARIO = USER_NAME,
                            ESTATUS = "4",
                            UUID = uuid
                        });
                    }

                    return JsonConvert.SerializeObject(new { code = 0, msg = "La factura se ha desvalidado." });

                    //var req1 = new Http<Response>()
                    //{
                    //    ObjectJSON = new
                    //    {
                    //        COMMIT       = 0,
                    //        USUARIO      = USER_NAME,
                    //        CONTRARECIBO = contrarecibo,
                    //        UUID         = uuid,
                    //    },
                    //    requestURI = "http://localhost/WbDesvalidadorV1/api/desvalidaFactura",
                    //};
                    //req1.Post();




                    //timer.Stop();

                    //TimeSpan timeTaken = timer.Elapsed;
                    //string foo = "Time taken: " + DateTime.Now.ToString("dd/MM/yyyy ") + timeTaken.ToString(@"m\:ss\.fff");


                    //StreamWriter file = new StreamWriter(path, true, Encoding.Default);
                    //file.WriteLine(foo);
                    //file.Close();


                    //if (req1.IsSuccess && req1.Response.Code == 0)
                    //{
                    //    //using (IDbConnection con2 = new SqlConnection(Extensions.GetBD_CFDI()))
                    //    //{
                    //    //    var sql = @"

                    //    //INSERT INTO LOG_CARTA_PAGO WITH (Rowlock) (FECHA_MODIFICACION, HORA_MODIFICACION, USUARIO, ESTATUS, UUID)
                    //    //  VALUES(@FECHA_MODIFICACION, @HORA_MODIFICACION, @USUARIO, @ESTATUS, @UUID)";

                    //    //    var fecha = DateTime.Now;
                    //    //    con2.Execute(sql, new
                    //    //    {
                    //    //        FECHA_MODIFICACION = fecha.Date,
                    //    //        HORA_MODIFICACION  = fecha.ToString("HH:mm:ss"),
                    //    //        USUARIO            = USER_NAME,
                    //    //        ESTATUS            = "4",
                    //    //        UUID               = uuid
                    //    //    });
                    //    //}


                    //    return JsonConvert.SerializeObject(new { code = 0, msg = "La factura se ha desvalidado." });
                    //}
                    //else
                    //{
                    //    throw new Exception(req1.Response.Msg);
                    //}


                }
                catch (Exception ex)
                {

                    //string dirPath = HttpContext.Current.Server.MapPath("~/log");
                    //string path    = dirPath + "\\Log_UUID.txt";



                    //StreamWriter file2 = new StreamWriter(path, true, Encoding.Default);
                    //file2.WriteLine("Error:" + ex.Message);
                    //file2.Close();



                    if (tran != null)
                    {
                        tran.Rollback();
                    }

                    return JsonConvert.SerializeObject(new {code = -1, msg = ex.Message});
                }
            }
        }


        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string SetChkFacDup(bool chk)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            string USER_NAME = "";
            if (current.User != null && current.User.Identity.IsAuthenticated)
            {
                FormsIdentity id = (FormsIdentity)current.User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;
                //role = ticket.UserData;
                USER_NAME = ticket.Name;
            }

            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {


                try
                {
                    string sql = @"UPDATE FACTURA_DUPLICADOS WITH (Rowlock) SET CHK = @CHK WHERE ID = 1";
                    con.Execute(sql, new { chk });


                    return JsonConvert.SerializeObject(new { code = 0, msg = "" });

                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { code = -1, msg = ex.Message });
                }
            }
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string SetChkFechaValidacion(bool chk)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            string USER_NAME = "";
            if (current.User != null && current.User.Identity.IsAuthenticated)
            {
                FormsIdentity id = (FormsIdentity)current.User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;
                //role = ticket.UserData;
                USER_NAME = ticket.Name;
            }

            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {


                try
                {
                    string sql = @"UPDATE FECHA_VALIDACION WITH (Rowlock) SET CHK = @CHK WHERE ID = 1";
                    con.Execute(sql, new { chk });


                    
                    if (chk == false)
                    {
                        sql = @"UPDATE FECHA_VALIDACION WITH (Rowlock) SET FECHA = @FECHA WHERE ID = 1";
                        con.Execute(sql, new { fecha = DateTime.Now.Date });
                    }


                    sql = @"
                           INSERT INTO LOG_FECHA_VALIDACION WITH (Rowlock) ( FECHA_MODIFICACION,  USUARIO,  CHECK_FECHA_VALIDACION,  FECHA_VALIDACION) 
                                                     VALUES(@FECHA_MODIFICACION, @USUARIO, @CHECK_FECHA_VALIDACION, @FECHA_VALIDACION)";
                    con.Execute(sql, new
                    {
                        FECHA_MODIFICACION = DateTime.Now,
                        USUARIO = USER_NAME,
                        CHECK_FECHA_VALIDACION = chk,
                        FECHA_VALIDACION=GetFechaValidacion()
                    });


                    return JsonConvert.SerializeObject(new { code = 0, msg = "" });

                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { code = -1, msg = ex.Message });
                }
            }
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string SetChkFechaPago(bool chk)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            string USER_NAME = "";
            if (current.User != null && current.User.Identity.IsAuthenticated)
            {
                FormsIdentity id = (FormsIdentity)current.User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;
                //role = ticket.UserData;
                USER_NAME = ticket.Name;
            }

            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {


                try
                {
                    string sql = @"UPDATE FECHA_PAGO_BLOQUEO WITH (Rowlock) SET CHK = @CHK WHERE ID = 1";
                    con.Execute(sql, new { chk });


                    /*
                    if (chk == false)
                    {
                        sql = @"UPDATE FECHA_VALIDACION WITH (Rowlock) SET FECHA = @FECHA WHERE ID = 1";
                        con.Execute(sql, new { fecha = DateTime.Now.Date });
                    }
                    */
                    /*
                    sql = @"
                           INSERT INTO LOG_FECHA_VALIDACION WITH (Rowlock) ( FECHA_MODIFICACION,  USUARIO,  CHECK_FECHA_VALIDACION,  FECHA_VALIDACION) 
                                                     VALUES(@FECHA_MODIFICACION, @USUARIO, @CHECK_FECHA_VALIDACION, @FECHA_VALIDACION)";
                    con.Execute(sql, new
                    {
                        FECHA_MODIFICACION = DateTime.Now,
                        USUARIO = USER_NAME,
                        CHECK_FECHA_VALIDACION = chk,
                        FECHA_VALIDACION=GetFechaValidacion()
                    });
                    */

                    return JsonConvert.SerializeObject(new { code = 0, msg = "" });

                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { code = -1, msg = ex.Message });
                }
            }
        }


        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string SetChkFechaLimite(bool chk)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            string USER_NAME = "";
            if (current.User != null && current.User.Identity.IsAuthenticated)
            {
                FormsIdentity id = (FormsIdentity)current.User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;
                //role = ticket.UserData;
                USER_NAME = ticket.Name;
            }

            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {


                try
                {
                    string sql = @"UPDATE FECHA_REC_FACTURA WITH (Rowlock) SET CHK = @CHK WHERE ID = 1";
                    con.Execute(sql, new { chk });


                    /*
                    if (chk == false)
                    {
                        sql = @"UPDATE FECHA_VALIDACION WITH (Rowlock) SET FECHA = @FECHA WHERE ID = 1";
                        con.Execute(sql, new { fecha = DateTime.Now.Date });
                    }
                    */
                    /*
                    sql = @"
                           INSERT INTO LOG_FECHA_VALIDACION WITH (Rowlock) ( FECHA_MODIFICACION,  USUARIO,  CHECK_FECHA_VALIDACION,  FECHA_VALIDACION) 
                                                     VALUES(@FECHA_MODIFICACION, @USUARIO, @CHECK_FECHA_VALIDACION, @FECHA_VALIDACION)";
                    con.Execute(sql, new
                    {
                        FECHA_MODIFICACION = DateTime.Now,
                        USUARIO = USER_NAME,
                        CHECK_FECHA_VALIDACION = chk,
                        FECHA_VALIDACION=GetFechaValidacion()
                    });
                    */

                    return JsonConvert.SerializeObject(new { code = 0, msg = "" });

                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { code = -1, msg = ex.Message });
                }
            }
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string SetFechaValidacion(DateTime fecha)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            string USER_NAME = "";
            if (current.User != null && current.User.Identity.IsAuthenticated)
            {
                FormsIdentity id = (FormsIdentity)current.User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;
                //role = ticket.UserData;
                USER_NAME = ticket.Name;
            }

            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {


                try
                {
                    string sql = @"UPDATE FECHA_VALIDACION WITH (Rowlock) SET FECHA = @FECHA WHERE ID = 1";
                    con.Execute(sql, new { fecha });

                    sql = @"
                           INSERT INTO LOG_FECHA_VALIDACION WITH (Rowlock) ( FECHA_MODIFICACION,  USUARIO,  CHECK_FECHA_VALIDACION,  FECHA_VALIDACION) 
                                                     VALUES(@FECHA_MODIFICACION, @USUARIO, @CHECK_FECHA_VALIDACION, @FECHA_VALIDACION)";
                    con.Execute(sql, new
                    {
                        FECHA_MODIFICACION = DateTime.Now,
                        USUARIO = USER_NAME,
                        CHECK_FECHA_VALIDACION = GetChkFechaValidacion(),
                        FECHA_VALIDACION = fecha
                    });

                    return JsonConvert.SerializeObject(new { code = 0, msg = "" });

                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { code = -1, msg = ex.Message });
                }
            }
        }


        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string SetFechaLimite(int dia)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            string USER_NAME = "";
            if (current.User != null && current.User.Identity.IsAuthenticated)
            {
                FormsIdentity id = (FormsIdentity)current.User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;
                //role = ticket.UserData;
                USER_NAME = ticket.Name;
            }

            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {


                try
                {
                    string sql = @"UPDATE FECHA_REC_FACTURA WITH (Rowlock) SET FECHA = @dia WHERE ID = 1";
                    con.Execute(sql, new { dia });

//                    sql = @"
//                           INSERT INTO LOG_FECHA_VALIDACION WITH (Rowlock) ( FECHA_MODIFICACION,  USUARIO,  CHECK_FECHA_VALIDACION,  FECHA_VALIDACION) 
//                                                     VALUES(@FECHA_MODIFICACION, @USUARIO, @CHECK_FECHA_VALIDACION, @FECHA_VALIDACION)";
//                    con.Execute(sql, new
//                    {
//                        FECHA_MODIFICACION = DateTime.Now,
//                        USUARIO = USER_NAME,
//                        CHECK_FECHA_VALIDACION = GetChkFechaValidacion(),
//                        FECHA_VALIDACION = fecha
//                    });

                    return JsonConvert.SerializeObject(new { code = 0, msg = "" });

                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { code = -1, msg = ex.Message });
                }
            }
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string SetFechaBloqueo(DateTime fecha)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {


                try
                {
                    string sql = @"UPDATE FECHA_BLOQUEO WITH (Rowlock) SET FECHA = @FECHA WHERE ID = 1";
                    con.Execute(sql,new { fecha });

                    return JsonConvert.SerializeObject(new { code = 0, msg = "" });

                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { code = -1, msg = ex.Message });
                }
            }
        }


        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string SetFechaPagoBloqueo(string dias)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {


                try
                {
                    string sql = @"UPDATE FECHA_PAGO_BLOQUEO WITH (Rowlock) SET DIAS = @DIAS WHERE ID = 1";
                    con.Execute(sql, new { dias });

                    return JsonConvert.SerializeObject(new { code = 0, msg = "" });

                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { code = -1, msg = ex.Message });
                }
            }
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string SetFechaBloqueo2(DateTime fecha)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {


                try
                {
                    string sql = @"UPDATE FECHA_ALBARAN_BLOQUEO WITH (Rowlock) SET FECHA = @FECHA WHERE ID = 1";
                    con.Execute(sql, new { fecha });

                    return JsonConvert.SerializeObject(new { code = 0, msg = "" });

                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { code = -1, msg = ex.Message });
                }
            }
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static DateTime? GetFechaBloqueo()
        {
            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {

                try
                {
                    string sql = @"SELECT FECHA FROM FECHA_BLOQUEO WITH (NOLOCK) WHERE ID = 1";
                    return con.QueryFirstOrDefault<DateTime?>(sql);

                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static bool? GetChkFechaLimite()
        {
            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {

                try
                {
                    string sql = @"SELECT CHK FROM FECHA_REC_FACTURA WITH (NOLOCK) WHERE ID = 1";
                    return con.QueryFirstOrDefault<bool?>(sql);

                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        //[ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        //public static bool? GetChkFechaValidacion()
        //{
        //    using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
        //    {

        //        try
        //        {
        //            string sql = @"SELECT CHK FROM FECHA_VALIDACION WITH (NOLOCK) WHERE ID = 1";
        //            return con.QueryFirstOrDefault<bool?>(sql);

        //        }
        //        catch (Exception ex)
        //        {
        //            return null;
        //        }
        //    }
        //}

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static bool? GetChkFechaValidacion()
        {
            try
            {
                return ExecuteWithRetry(
                    () =>
                    {
                        using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                        {
                            string sql = @"SELECT CHK FROM FECHA_VALIDACION WITH (NOLOCK) WHERE ID = 1";
                            var resultado = con.QueryFirstOrDefault<bool?>(sql, commandTimeout: 6000);

                            // Si no retorna valor, forzar reintento lanzando excepción
                            if (!resultado.HasValue)
                            {
                                throw new InvalidOperationException("La consulta no devolvió ningún valor.");
                            }

                            return resultado;
                        }
                    },
                    "GetChkFechaValidacion",
                    Extensions.GetUserName() ?? "Sistema",
                    maxRetries: 3
                );
            }
            catch (Exception ex)
            {
                Log.Write($"GetChkFechaValidacion: Error después de todos los reintentos: {ex.Message}", USER_NAME: Extensions.GetUserName());
                return null;
            }
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static bool? GetValidaFacturaCierre(List<REQ1> facturas)
        {
            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {
                try
                {

                    var chk = con.ExecuteScalar<bool>(
                        "SP_VALIDA_FACTURA_CIERRE",
                        new
                        {
                            FECHATIMBRADO = facturas[0].FECHA_FACTURA.ToString("dd/MM/yyyy"),

                        },
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: 6000);

                    return chk;

                    //***************************************************
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }


        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static bool? GetChkFechaPago()
        {
            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {

                try
                {
                    string sql = @"SELECT CHK FROM FECHA_PAGO_BLOQUEO WITH (NOLOCK) WHERE ID = 1";
                    return con.QueryFirstOrDefault<bool?>(sql);

                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        //[ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        //public static int? GetFechaPagoBloqueo()
        //{
        //    using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
        //    {

        //        try
        //        {
        //            string sql = @"SELECT DIAS FROM FECHA_PAGO_BLOQUEO WITH (NOLOCK) WHERE ID = 1";
        //            return con.QueryFirstOrDefault<int>(sql);

        //        }
        //        catch (Exception ex)
        //        {
        //            return null;
        //        }
        //    }
        //}

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static int? GetFechaPagoBloqueo()
        {
            try
            {
                return ExecuteWithRetry(
                    () =>
                    {
                        using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                        {
                            string sql = @"SELECT DIAS FROM FECHA_PAGO_BLOQUEO WITH (NOLOCK) WHERE ID = 1";
                            var resultado = con.QueryFirstOrDefault<int?>(sql, commandTimeout: 30);

                            // Si no retorna valor, forzar reintento lanzando excepción
                            if (!resultado.HasValue)
                            {
                                throw new InvalidOperationException("La consulta no devolvió ningún valor.");
                            }

                            return resultado;
                        }
                    },
                    "GetFechaPagoBloqueo",
                    Extensions.GetUserName() ?? "Sistema",
                    maxRetries: 3
                );
            }
            catch (Exception ex)
            {
                Log.Write($"GetFechaPagoBloqueo: Error después de todos los reintentos: {ex.Message}", USER_NAME: Extensions.GetUserName());
                return null;
            }
        }


        //[ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        //public static bool? GetChkFacDup()
        //{
        //    using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
        //    {

        //        try
        //        {
        //            string sql = @"SELECT CHK FROM FACTURA_DUPLICADOS WITH (NOLOCK) WHERE ID = 1";
        //            return con.QueryFirstOrDefault<bool?>(sql);

        //        }
        //        catch (Exception ex)
        //        {
        //            return null;
        //        }
        //    }
        //}

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static bool? GetChkFacDup()
        {
            try
            {
                return ExecuteWithRetry(
                    () =>
                    {
                        using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                        {
                            string sql = @"SELECT CHK FROM FACTURA_DUPLICADOS WITH (NOLOCK) WHERE ID = 1";
                            var resultado = con.QueryFirstOrDefault<bool?>(sql, commandTimeout: 30);

                            // Si no retorna valor, forzar reintento lanzando excepción
                            if (!resultado.HasValue)
                            {
                                throw new InvalidOperationException("La consulta no devolvió ningún valor.");
                            }

                            return resultado;
                        }
                    },
                    "GetChkFacDup",
                    Extensions.GetUserName() ?? "Sistema",
                    maxRetries: 3
                );
            }
            catch (Exception ex)
            {
                Log.Write($"GetChkFacDup: Error después de todos los reintentos: {ex.Message}", USER_NAME: Extensions.GetUserName());
                return null;
            }
        }


        //[ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        //public static int? GetFechaLimite()
        //{
        //    using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
        //    {

        //        try
        //        {
        //            string sql = @"SELECT FECHA FROM FECHA_REC_FACTURA WITH (NOLOCK) WHERE ID = 1";
        //            return con.QueryFirstOrDefault<int?>(sql);

        //        }
        //        catch (Exception ex)
        //        {
        //            return null;
        //        }
        //    }
        //}

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static int? GetFechaLimite()
        {
            try
            {
                return ExecuteWithRetry(
                    () =>
                    {
                        using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                        {
                            string sql = @"SELECT FECHA FROM FECHA_REC_FACTURA WITH (NOLOCK) WHERE ID = 1";
                            var resultado = con.QueryFirstOrDefault<int?>(sql, commandTimeout: 30);

                            // Si no retorna valor, forzar reintento lanzando excepción
                            if (!resultado.HasValue)
                            {
                                throw new InvalidOperationException("La consulta no devolvió ningún valor.");
                            }

                            return resultado;
                        }
                    },
                    "GetFechaLimite",
                    Extensions.GetUserName() ?? "Sistema",
                    maxRetries: 3
                );
            }
            catch (Exception ex)
            {
                Log.Write($"GetFechaLimite: Error después de todos los reintentos: {ex.Message}", USER_NAME: Extensions.GetUserName());
                return null;
            }
        }

        //[ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        //public static DateTime? GetFechaValidacion()
        //{
        //    using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
        //    {

        //        try
        //        {
        //            string sql = @"SELECT FECHA FROM FECHA_VALIDACION WITH (NOLOCK) WHERE ID = 1";
        //            return con.QueryFirstOrDefault<DateTime?>(sql);

        //        }
        //        catch (Exception ex)
        //        {
        //            return null;
        //        }
        //    }
        //}

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static DateTime? GetFechaValidacion()
        {
            try
            {
                return ExecuteWithRetry(
                    () =>
                    {
                        using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                        {
                            string sql = @"SELECT FECHA FROM FECHA_VALIDACION WITH (NOLOCK) WHERE ID = 1";
                            var resultado = con.QueryFirstOrDefault<DateTime?>(sql, commandTimeout: 30);

                            // Si no retorna valor, forzar reintento lanzando excepción
                            if (!resultado.HasValue)
                            {
                                throw new InvalidOperationException("La consulta no devolvió ningún valor.");
                            }

                            return resultado;
                        }
                    },
                    "GetFechaValidacion",
                    Extensions.GetUserName() ?? "Sistema",
                    maxRetries: 3
                );
            }
            catch (Exception ex)
            {
                Log.Write($"GetFechaValidacion: Error después de todos los reintentos: {ex.Message}", USER_NAME: Extensions.GetUserName());
                return null;
            }
        }


        //[ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        //public static DateTime? GetFechaBloqueo2()
        //{
        //    using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
        //    {

        //        try
        //        {
        //            string sql = @"SELECT FECHA FROM FECHA_ALBARAN_BLOQUEO WITH (NOLOCK) WHERE ID = 1";
        //            return con.QueryFirstOrDefault<DateTime?>(sql);

        //        }
        //        catch (Exception ex)
        //        {
        //            return null;
        //        }
        //    }
        //}

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static DateTime? GetFechaBloqueo2()
        {
            try
            {
                return ExecuteWithRetry(
                    () =>
                    {
                        using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                        {
                            string sql = @"SELECT FECHA FROM FECHA_ALBARAN_BLOQUEO WITH (NOLOCK) WHERE ID = 1";
                            var resultado = con.QueryFirstOrDefault<DateTime?>(sql, commandTimeout: 30);

                            // Si no retorna valor, forzar reintento lanzando excepción
                            if (!resultado.HasValue)
                            {
                                throw new InvalidOperationException("La consulta no devolvió ningún valor.");
                            }

                            return resultado;
                        }
                    },
                    "GetFechaBloqueo2",
                    Extensions.GetUserName() ?? "Sistema",
                    maxRetries: 3
                );
            }
            catch (Exception ex)
            {
                Log.Write($"GetFechaBloqueo2: Error después de todos los reintentos: {ex.Message}", USER_NAME: Extensions.GetUserName());
                return null;
            }
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string GetFechaBloqueoStr()
        {

                var it = GetFechaBloqueo();

                if (it != null)
                {
                    return it.Value.ToString("dd/MM/yyyy");
                }
                else
                {
                    return "";
                }

        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string GetFechaValidacionStr()
        {

            var it = GetFechaValidacion();

            if (it != null)
            {
                return it.Value.ToString("dd/MM/yyyy");
            }
            else
            {
                return "";
            }

        }



        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string GetFechaBloqueo2Str()
        {
                var it = GetFechaBloqueo2();

                if (it != null)
                {
                    return it.Value.ToString("dd/MM/yyyy");
                }
                else
                {
                    return "";
                }
        }



        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string Ajustes(List<IT_REL_ALBCOMPRAMODIF> data)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            using (IDbConnection con = new SqlConnection(Extensions.GetBD_ICG()))
            {

                IDbTransaction tran = null;


                try
                {

                    con.Open();
                    tran = con.BeginTransaction();


                        //string sql = @"SELECT * FROM IT_REL_ALBCOMPRAMODIF";



                        //var ls=con.Query<IT_REL_ALBCOMPRAMODIF>(sql, new {  }, tran).ToList();


                        var sql = @"

                        INSERT INTO dbo.IT_REL_ALBCOMPRAMODIF 
                        ( UUID,  NUMSERIEO,  NUMALBARANO,  NUMLIN,  CODARTICULO,  UNIDADESTOTAL,  PRECIO,  TIPOIMPUESTO,  IVA,  REQ,  NUMSERIE,  NUMALBARAN,  ESTADO,  FECHA_PROCESO,  USUARIO) VALUES
                        (@UUID, @NUMSERIEO, @NUMALBARANO, @NUMLIN, @CODARTICULO, @UNIDADESTOTAL, @PRECIO, @TIPOIMPUESTO, @IVA, @REQ, @NUMSERIE, @NUMALBARAN, @ESTADO, null, @USUARIO)
                        ";


                        var dd = data.GroupBy(g => g.NUMALBARANO).ToDictionary(x => x.Key, y => y.ToList());

                        foreach (var ky in dd)
                        {
                            int Lin = 0;
                            foreach (var it in ky.Value)
                            {
                                Lin++;

                                it.NUMLIN     = Lin;
                                it.USUARIO    = Extensions.GetUserName();
                                it.NUMSERIE   = it.NUMSERIEO.SSubstring(0, it.NUMSERIEO.Length - 1) + "G";
                                it.NUMALBARAN = 0;
                                it.ESTADO     = 0;

                                if (it.PRECIO >= 0)
                                {
                                    it.UNIDADESTOTAL = 1;
                                }
                                else
                                {
                                    it.UNIDADESTOTAL = -1;
                                }
                            }

                            con.Execute(sql, ky.Value, tran);

                        }


                    tran.Commit();

                    
                    using (IDbConnection con2 = new SqlConnection(Extensions.GetBD_ICG()))
                    {


                        foreach (var ky in dd)
                        {
                            var tt = ky.Value.FirstOrDefault();

                            var err = con2.ExecuteScalar<string>(
                                "Sp_IT_AlbCompras_Ajuste",
                                new
                                {
                                    COMMIT      = 0,
                                    USUARIOL    = Extensions.GetUserName(),
                                    UUID        = tt.UUID,
                                    NUMSERIEO   = tt.NUMSERIEO,
                                    NUMALBARANO = tt.NUMALBARANO
                                },
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 6000);

                            if (err !=null && err.Trim() != "")
                            {
                                return JsonConvert.SerializeObject(new { code = -1, msg = err });
                            }
                        }
                    }


                    return JsonConvert.SerializeObject(new { code = 0, msg = "" });

                }
                catch (Exception ex)
                {
                    if (tran != null)
                    {
                        tran.Rollback();
                    }

                    return JsonConvert.SerializeObject(new { code = -1, msg = ex.Message });
                }
            }
        }


        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string SetAccion(List<CFDI_XML> data, string estatus, string observacion1)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {
                //var rs = con.Query<SERIES>("SELECT SERIE, DESCRIPCION FROM SERIES WHERE LEN(SERIE) = 2").ToList();

                IDbTransaction tran = null;
                int rs = 0;

                try
                {

                    if (data.Any(a => a.ESTATUS == 2 || a.ESTATUS == 6))
                    {
                        throw new Exception("Desmarque las facturas con estatus de asociadas o pagadas.");
                    }



                    con.Open();
                    tran = con.BeginTransaction();
                    //RFC_EMISOR
                    //INSERT INTO CONTRA_RECIBOS DEFAULT VALUES;

                    string sql = @"UPDATE CFDI_XML_PSI WITH (Rowlock) SET ESTATUS = @ESTATUS, OBSERVACION1 = @OBSERVACION1 WHERE ID = @ID";

                    foreach (var xml in data)
                    {

                        con.Execute(
                            sql,
                            new { xml.ID, estatus, observacion1 },
                            tran);

                    }

                    tran.Commit();
                    return JsonConvert.SerializeObject(new { code = 0, msg = "" });

                }
                catch (Exception ex)
                {
                    if (tran != null)
                    {
                        tran.Rollback();
                    }

                    return JsonConvert.SerializeObject(new { code = -1, msg = ex.Message });
                }
            }
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string Valido(string id)
        {
           HttpContext current = HttpContext.Current;
           current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
           HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
           HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;


           using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
           {
              //var rs = con.Query<SERIES>("SELECT SERIE, DESCRIPCION FROM SERIES WHERE LEN(SERIE) = 2").ToList();

              IDbTransaction tran = null;
              int            rs   = 0;

              try
              {
                 con.Open();
                 tran = con.BeginTransaction();
                 //RFC_EMISOR
                 //INSERT INTO CONTRA_RECIBOS DEFAULT VALUES;

                 string sql = @"UPDATE CFDI_XML_PSI WITH (Rowlock) SET REVISADO = 1 WHERE ID = @ID";

                 con.Execute(
                       sql,
                       new { id },
                       tran);

                 tran.Commit();
                 return JsonConvert.SerializeObject(new { code = 0, msg = "" });

              }
              catch (Exception ex)
              {
                 if (tran != null)
                 {
                    tran.Rollback();
                 }
                 return JsonConvert.SerializeObject(new { code = -1, msg = "Error: " + ex.Message });
              }
           }
        }


        public static CFDI_ACCOUNT ShowBtn()
        {

            try
            {

                using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                {
                    var rs2 = con.Query<CFDI_ACCOUNT>("SELECT * FROM CFDI_ACCOUNT WITH (NOLOCK) WHERE ID = @USER_ID", new { USER_ID = Extensions.GetUserID() }).FirstOrDefault();

                    return rs2;

                }
            }
            catch
            {
                return null;
            }
            
        }

        public class REQ1
        {
            public int? CONTRA_RECIBO_ID { get; set; }
            public string UUID { get; set; }
            public string SERIE { get; set; }
            public string FOLIO { get; set; }
            public int? ESTATUS_ID { get; set; }
            public int? ID { get; set; }
            public double? TOTAL { get; set; }
            public string RFC_EMISOR { get; set; }
            public DateTime FECHA_FACTURA { get; set; }
        }

        public class REQ2
        {
            public string NUMSERIE { get; set; } 
            public int NUMALBARAN { get; set; }
            public double? TOTAL { get; set; }
        }


        public class Req
        {
            public DateTime? FECHA_BLOQ { get; set; }
            public DateTime FECHA1 { get; set; }
            public DateTime FECHA2 { get; set; }
            public int ESTATUS { get; set; }
            public int USER_ID { get; set; }
            public string RFC_EMISOR { get; set; }
            public string UUID { get; set; }
            public string FOLIO { get; set; }
            public string SERIE { get; set; }
            public string CONTR { get; set; }
            public string A { get; set; }
            public int OPT { get; set; }
        }

        public class DATA3
        {
           public string        RFiscal;
           public string        UsoCFDI;
           public string        MetodoPago;
           public string        FormaPago;
           public string        Moneda;
           public bool          Revisado;
           public List<DETALLE> DATA { get; set; }
           public string        Descuento { get; set; }
        }

    }


}