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
using XML2;
using XML3;
using Calendar = System.Globalization.Calendar;

namespace WB_CFDI_V1.Vistas
{
    public partial class Pag1 : Page
    {
        //private static int USER_ID = -1;
        //static string USER_NAME = "";
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

        }

        protected static string getFecha1()
        {
            return DateTime.Now.AddDays(-DateTime.Now.Day + 1).ToString("yyyy,MM,dd");
        }


        protected static string getRazonSocial()
        {
            
            try
            {

                using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                {
                    var rs = con.Query<string>("SELECT MAX(RAZON_SOCIAL_EMISOR) RAZON_SOCIAL_EMISOR FROM CFDI_XML_PSI WHERE RFC_EMISOR = @USER_NAME GROUP BY RFC_EMISOR",
                            new { USER_NAME = Extensions.GetUserName() }).SingleOrDefault();
                    
                    return rs;
                }
            }
            catch
            {
                return "";
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
                    var rs = con.Query<CFDI_ESTATUS>("SELECT * FROM CFDI_ESTATUS ORDER BY ID").ToList();
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
                      xml.TIENDA,
                      xml.NO_COMPRA,
                      xml.FECHA_COMPRA,
                      --xml.REVISADO,
                      xml.NOMBRE_ARCHIVO,
                      es.ESTATUS AS ESTATUS2,
                      xml.TIPO_COMPROBANTE,
                      xml.NUMEFECTO,
                      xml.FECHA_SALDADO,
                      ct.FECHA_PAGO
                    FROM dbo.CFDI_XML_PSI xml
                    INNER JOIN dbo.CFDI_ESTATUS es ON xml.ESTATUS = es.ID
                    LEFT OUTER JOIN dbo.CONTRA_RECIBOS_PSI ct ON xml.CONTRA_RECIBO_ID = ct.ID
                    WHERE 
                    
                    (@OPT = 0 AND xml.FECHA_RECEPCION BETWEEN CAST((@fecha1 + ' 00:00:00') AS DATETIME) AND CAST((@fecha2 + ' 23:59:59') AS DATETIME)
                    OR
                    @OPT = 1 AND xml.FECHA_SALDADO BETWEEN CAST((@fecha1 + ' 00:00:00') AS DATETIME) AND CAST((@fecha2 + ' 23:59:59') AS DATETIME))

                    AND xml.USER_ID = @USER_ID
                    AND (@ESTATUS = 0 OR xml.ESTATUS = @ESTATUS)
                    AND (@FOLIO = '' OR UPPER(xml.FOLIO) LIKE '%' + UPPER(@FOLIO) + '%')
                    AND (@SERIE = '' OR UPPER(xml.SERIE) LIKE '%' + UPPER(@SERIE) + '%')
                    AND (@CONTR = '' OR UPPER(xml.CONTRA_RECIBO_ID) = @CONTR)
                    AND (@A IS NULL OR @A = '1' AND NOT xml.CONTRA_RECIBO_ID IS NULL
                                    OR @A = '2' AND xml.CONTRA_RECIBO_ID IS NULL)
                    ORDER BY xml.CONTRA_RECIBO_ID DESC";
                    
                    req.USER_ID = Extensions.GetUserID();

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





                    foreach (var xm in rs)
                    {
                        try
                        {
                            XML3.Comprobante c33 = new XML3.Comprobante();
                            XML2.Comprobante c32 = new XML2.Comprobante();


                            c33 = c33.DeserializeStr(xm.XML);
                            c32 = c32.DeserializeStr(xm.XML);

                            if (c33.Version != null)
                            {

                                xm.IVA = c33.Impuestos.Traslados.Traslado.Where(w => w.Impuesto == "002")
                                    .Sum(s => s.Importe.Dbl());

                                xm.IEPS = c33.Impuestos.Traslados.Traslado.Where(w => w.Impuesto == "003")
                                    .Sum(s => s.Importe.Dbl());

                                if (xm.TIPO_COMPROBANTE != null && (xm.TIPO_COMPROBANTE.ToLower() == "e" || xm.TIPO_COMPROBANTE.ToLower() == "egreso"))
                                {
                                    xm.IVA = xm.IVA * -1;
                                    xm.IEPS = xm.IEPS * -1;
                                }


                            }

                            if (c32.Version != null)
                            {
                                xm.IVA = c32.Impuestos.Traslados.Traslado.Where(w => w.Impuesto == "002")
                                    .Sum(s => s.Importe.Dbl());

                                xm.IEPS = c32.Impuestos.Traslados.Traslado.Where(w => w.Impuesto == "003")
                                    .Sum(s => s.Importe.Dbl());

                                if (xm.TIPO_COMPROBANTE != null && (xm.TIPO_COMPROBANTE.ToLower() == "e" || xm.TIPO_COMPROBANTE.ToLower() == "egreso"))
                                {
                                    xm.IVA = xm.IVA * -1;
                                    xm.IEPS = xm.IEPS * -1;
                                }
                            }
                        }
                        catch (Exception e)
                        {

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
                        s.SUBTOTAL,
                        s.IMPUESTOS,
                        s.IVA,
                        s.IEPS,
                        s.SERIE,
                        s.FOLIO,
                        s.TIENDA,
                        s.NO_COMPRA,
                        s.FECHA_COMPRA,
                        //REVISADO = s.REVISADO ? "OK" : "",
                        ESTATUS = s.ESTATUS2,
                        s.NUMEFECTO,
                        s.FECHA_SALDADO,
                        s.FECHA_PAGO
                    }).ToList());
                }
            }
            catch
            {
                return JsonConvert.SerializeObject(new ArrayList());
            }
        }


        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string Save(string id, string tienda, string compra)
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
                        UPDATE dbo.CFDI_XML_PSI SET 
                            TIENDA = @TIENDA,
                            NO_COMPRA = @COMPRA,
                            FECHA_COMPRA = @FECHA
                        WHERE ID = @ID";

                    DateTime fecha = DateTime.Now;

                    var rs = con.Execute(
                            sql,
                            new { ID = id, TIENDA = tienda.Trim(), COMPRA = compra.Trim(), FECHA = DateTime.Now });


                    return JsonConvert.SerializeObject(new { code = 0, msg = fecha }); ;
                }
            }
            catch(Exception ex)
            {
                 return JsonConvert.SerializeObject(new{code = -1, msg = ex.Message});
            }
        }


        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string Save2(string id, List<Data3> data)
        {
             HttpContext current = HttpContext.Current;
             current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
             HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
             HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

             IDbTransaction tran = null;

             try
             {

                  using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                  {

                       /*
                       string sql = @"
                        UPDATE dbo.CFDI_XML_PSI SET 
                            TIENDA = @TIENDA,
                            NO_COMPRA = @COMPRA,
                            FECHA_COMPRA = @FECHA
                        WHERE ID = @ID";

                       DateTime fecha = DateTime.Now;

                       var rs = con.Execute(
                                 sql,
                                 new { ID = id, TIENDA = tienda.Trim(), COMPRA = compra.Trim(), FECHA = DateTime.Now });
                      */

                       con.Open();
                       tran = con.BeginTransaction();
                       //RFC_EMISOR
                       //INSERT INTO CONTRA_RECIBOS DEFAULT VALUES;  @@IDENTITY;

                       data.ForEach(f => { f.ID_XML = id; });

                       foreach( var it1 in data )
                       {
                            var it2 = con.Query<CFDI_COMPRAS>(
                                      @"SELECT * FROM CFDI_COMPRAS
                              WHERE ID_XML = @ID_XML AND SERIE = @SERIE AND FOLIO = @FOLIO",
                                      it1,
                                      tran
                                      ).ToList();

                           if (it2.Count > 0)
                           {
                               return JsonConvert.SerializeObject(new { code = -1, msg = "El registro ya existe: <br /><b>SERIE:</b> " + it1.SERIE + "<br /><b>FOLIO:</b> " + it1.FOLIO});
                           }
                       }



                       string sql = @"

                        INSERT INTO CFDI_COMPRAS(ID_XML, SERIE, FOLIO) 
                                               VALUES(@ID_XML, @SERIE, @FOLIO);";

                       con.Execute(
                                 sql,
                                 data,
                                 transaction: tran);




                       sql = @"UPDATE dbo.CFDI_XML_PSI SET FECHA_COMPRA = @FECHA WHERE ID = @ID";

                       DateTime fecha = DateTime.Now;

                       con.Execute(
                                 sql,
                                 new{ID = id, FECHA = DateTime.Now},
                                 tran);


                       tran.Commit();

                       return JsonConvert.SerializeObject(new { code = 0, msg = fecha });
                  }
             }
             catch (Exception ex)
             {
                  return JsonConvert.SerializeObject(new { code = -1, msg = ex.Message });
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
                using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                {


                     var it = con.Query<CFDI_COMPRAS>(
                               @"SELECT * FROM CFDI_COMPRAS WHERE ID_XML = @ID_XML",
                               new { ID_XML = id }
                               ).ToList();


                     return JsonConvert.SerializeObject(new { code = 0, data = it, msg = "" });
                }
            }
            catch(Exception ex)
            {
                 return JsonConvert.SerializeObject(new{code = -1, data = "[]", msg = ex.Message});
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
            public int OPT { get; set; }
        }

    }

    public class Data3
    {
         public string ID_XML { get; set; }
         public string SERIE  { get; set; }
         public string FOLIO  { get; set; }
    }
}