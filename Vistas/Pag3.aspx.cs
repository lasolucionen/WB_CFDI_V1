using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Web;
using System.Web.Script.Services;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using AngleSharp;

using Dapper;
using EO.Internal;
using Newtonsoft.Json;
using WB_CFDI_V1.Properties;
using Calendar = System.Globalization.Calendar;

using XML2;
using XML3;

namespace WB_CFDI_V1.Vistas
{
    public partial class Pag3 : Page
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
                        "SELECT ID FROM CFDI_ACCOUNT WHERE USER_NAME = @USER_NAME",
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
        public static string Quitar(List<CFDI_XML> data)
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
                    con.Open();
                    tran = con.BeginTransaction();
                    //RFC_EMISOR
                    //INSERT INTO CONTRA_RECIBOS DEFAULT VALUES;

                    string sql = @"UPDATE CFDI_XML_PSI SET CONTRA_RECIBO_ID = null WHERE ID = @ID";

                    foreach (var xml in data)
                    {
                        con.Execute(
                            sql,
                            new { ID = xml.ID },
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
                    return JsonConvert.SerializeObject(new { code = -1, msg = "Error: " + ex.Message });
                }
            }
        }


        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string Crear(List<CFDI_XML> data, /*DateTime fecha1,*/ DateTime fecha2, string observacion)
        {
             HttpContext current = HttpContext.Current;
             current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
             HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
             HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

             if (data.All(a => a.CONTRA_RECIBO_ID != null))
             {
                  return JsonConvert.SerializeObject(
                            new{code = -1, msg = "No se puede agregar, alguna factura seleccionada ya tiene contrarecibo."});
             }

             if (data.Count == 0)
             {
                  return JsonConvert.SerializeObject(new{code = -1, msg = "Agregue facturas para crear el contrarecibo."});
             }

             if (data.Any(a => a.ESTATUS2 == "Rechazadas"))
             {
                  return JsonConvert.SerializeObject(
                            new{code = -1, msg = "No se puede agregar, alguna factura seleccionada esta rechazada."});
             }

             using( IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()) )
             {
                  //var rs = con.Query<SERIES>("SELECT SERIE, DESCRIPCION FROM SERIES WHERE LEN(SERIE) = 2").ToList();

                  IDbTransaction tran = null;
                  int            id   = 0;

                  try
                  {
                       con.Open();
                       tran = con.BeginTransaction();
                       //RFC_EMISOR
                       //INSERT INTO CONTRA_RECIBOS DEFAULT VALUES;  @@IDENTITY;

                       string sql = @"

                        DECLARE @ID INT;
                        SET @ID = (SELECT MAX(p.ID) FROM CONTRA_RECIBOS_PSI p);
                        SET @ID = ISNULL( @ID , 0 ) + 1;

                        INSERT INTO CONTRA_RECIBOS_PSI(ID,  RFC_EMISOR,  FECHA,  FECHA_RECEPCION,  FECHA_PAGO, OBSERVACION) 
                                               VALUES(@ID, @RFC_EMISOR, @FECHA, @FECHA_RECEPCION, @FECHA_PAGO, @OBSERVACION);
                        SELECT @ID";

                       var fecha = DateTime.Now;

                       id = con.QueryFirst<int>(
                                 sql,
                                 new{
                                         RFC_EMISOR      = data[0].RFC_EMISOR,
                                         FECHA           = fecha,
                                         FECHA_RECEPCION = fecha,
                                         FECHA_PAGO      = fecha2,
                                         OBSERVACION     = observacion.Trim()
                                    },
                                 transaction: tran);
                      
                       sql = @"UPDATE CFDI_XML_PSI SET CONTRA_RECIBO_ID = @CONTRA_RECIBO_ID WHERE ID = @ID";

                       foreach( var xml in data )
                       {
                            con.Execute(
                                      sql,
                                      new{CONTRA_RECIBO_ID = id, ID = xml.ID},
                                      tran);
                       }

                       tran.Commit();

                       HttpContext.Current.Session["pdf_id"] = id;
                       return JsonConvert.SerializeObject(new{code = 0, msg = "", contr = Extensions.GetSerie() + "-" + id});

                  }
                  catch(Exception ex)
                  {
                       if (tran != null)
                       {
                            tran.Rollback();
                       }
                       return JsonConvert.SerializeObject(new{code = -1, msg = "Error: " + ex.Message});
                  }
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
                    var rs = con.Query<CFDI_XML>("SELECT RFC_EMISOR, MAX(RAZON_SOCIAL_EMISOR) RAZON_SOCIAL_EMISOR FROM CFDI_XML_PSI GROUP BY RFC_EMISOR").ToList();
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
        public static string GetContr(string rfc, DateTime fecha1, DateTime fecha2, string Contr)
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
                    SELECT
                        *
                    FROM dbo.CONTRA_RECIBOS_PSI a
                    WHERE FECHA BETWEEN CAST((@FECHAINI + ' 00:00:00') AS DATETIME) AND CAST((@FECHAFIN + ' 23:59:59') AS DATETIME)
                    AND RFC_EMISOR = @RFC_EMISOR
                    AND (@ID = '' OR ID LIKE '%' + @ID + '%')
                    ORDER BY ID DESC";


                    var rs = con.Query<CONTRA_RECIBOS>(
                        sql,
                        new { FECHAINI = fecha1, FECHAFIN = fecha2, RFC_EMISOR = rfc, ID = Contr}).ToList();
                    
                    
                    return JsonConvert.SerializeObject(rs);
                }
            }
            catch
            {
                return JsonConvert.SerializeObject(new ArrayList());
            }
        }

        public class DETALLE
        {
            public string Impuesto { get; set; }
            public string TasaOCuota { get; set; }
            public string Importe { get; set; }
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
                    string sql = @"SELECT XML FROM dbo.CFDI_XML_PSI WHERE ID = @ID";

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
                    string sql =
                        @"
                    SELECT
                      xml.ID,
                      xml.USER_ID,
                      xml.UUID,
                      xml.XML,
                      xml.RAZON_SOCIAL_EMISOR,
                      xml.RFC_EMISOR,
                      xml.TOTAL,
                      xml.SUBTOTAL,
                      xml.IMPUESTOS,
                      xml.CONTRA_RECIBO_ID,
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
                      xml.ESTATUS,
                      es.ESTATUS AS ESTATUS2,
                      xml.TIPO_COMPROBANTE
                    FROM dbo.CFDI_XML_PSI xml
                    INNER JOIN dbo.CFDI_ESTATUS es ON xml.ESTATUS = es.ID
                    WHERE FECHA_RECEPCION
                    BETWEEN CAST((@fecha1 + ' 00:00:00') AS DATETIME) AND CAST((@fecha2 + ' 23:59:59') AS DATETIME)
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
                    


                    return JsonConvert.SerializeObject(rs.Select(
                        s => new
                        {
                            s.ID,
                            s.CONTRA_RECIBO_ID,
                            s.FECHA_FACTURA,
                            s.FECHA_RECEPCION,
                            s.VERSION,
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
                            ESTATUS_ID = s.ESTATUS
                        }).ToList());
                }
            }
            catch
            {
                return JsonConvert.SerializeObject(new ArrayList());
            }
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string Detalle(string id)
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
                      xml.UUID,
                      xml.XML,
                      xml.RAZON_SOCIAL_EMISOR,
                      xml.RFC_EMISOR,
                      xml.TOTAL,
                      xml.SUBTOTAL,
                      xml.IMPUESTOS,
                      xml.CONTRA_RECIBO_ID,
                      xml.VERSION,
                      xml.FECHA_RECEPCION,
                      xml.FECHA_FACTURA,
                      xml.SERIE,
                      xml.FOLIO,
                      xml.TIENDA,
                      xml.NO_COMPRA,
                      xml.FECHA_COMPRA,
                      xml.NOMBRE_ARCHIVO,
                      xml.ESTATUS,
                      es.ESTATUS AS ESTATUS2,
                      xml.TIPO_COMPROBANTE
                    FROM dbo.CFDI_XML_PSI xml
                    INNER JOIN dbo.CFDI_ESTATUS es ON xml.ESTATUS = es.ID
                    WHERE xml.CONTRA_RECIBO_ID = @ID";

                    var rs = con.Query<CFDI_XML>(sql, new {ID = id});

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





                    return JsonConvert.SerializeObject(rs.Select(
                        s => new
                        {
                            s.ID,
                            s.CONTRA_RECIBO_ID,
                            s.FECHA_FACTURA,
                            s.FECHA_RECEPCION,
                            s.VERSION,
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
                            ESTATUS = s.ESTATUS2,
                            ESTATUS_ID = s.ESTATUS
                        }).ToList());
                }
            }
            catch
            {
                return JsonConvert.SerializeObject(new ArrayList());
            }
        }



        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string ProcesoPago()
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            var msg = new Dictionary<string, string>();

            try
            {

                using (IDbConnection con = new SqlConnection(Extensions.GetBD_ICG()))
                {
                    var err = con.ExecuteScalar<string>(
                            "Sp_IT_ActualizaFoliosF",
                            new
                            {
                                COMMIT = 0
                            },
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 3000);

                    msg.Add("code", "0");
                    msg.Add("msg",  "Proceso terminado.");

                    return JsonConvert.SerializeObject(msg);
                }
            }
            catch(Exception ex)
            {

                msg.Add("code", "-1");
                msg.Add("msg",  ex.Message);

                return JsonConvert.SerializeObject(msg);
            }
        }


        public class REQ1
        {
            public string UUID { get; set; } // varchar(40), not null
            public string SERIE { get; set; } // varchar(20), not null
            public int? FOLIO { get; set; } // varchar(20), not null
            public int? ESTATUS_ID { get; set; } // varchar(20), not null
            public int? ID { get; set; } // varchar(20), not null
        }

        public class REQ2
        {
            public string NUMSERIE { get; set; } // nvarchar(4), not null
            public int NUMALBARAN { get; set; } // int, not null
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
        }

    }


}