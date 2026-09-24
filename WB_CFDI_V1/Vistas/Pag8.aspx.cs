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
using EO.Internal;
using Newtonsoft.Json;
using WB_CFDI_V1.Properties;
using Calendar = System.Globalization.Calendar;
using XML3;

namespace WB_CFDI_V1.Vistas
{
    public partial class Pag8 : Page
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
                    var rs = con.Query<CFDI_XML>("SELECT RFC_EMISOR, MAX(RAZON_SOCIAL_EMISOR) RAZON_SOCIAL_EMISOR FROM CFDI_XML_COMPLEMENTO_PAGO WITH (NOLOCK) GROUP BY RFC_EMISOR").ToList();
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

                    string uuid = "";


                    if (!String.IsNullOrEmpty(req.UUID))
                    {
                        uuid = @"
                            INNER JOIN 
                              (
                              
                                SELECT
                                  MIN(d.ID_DOCUMENTO) ID_DOCUMENTO, MIN(d.NUM_PARCIALIDAD) NUM_PARCIALIDAD,d.COMPLEMENTO_ID 
                                FROM dbo.CFDI_XML_COMPLEMENTO_DETALLE d WITH (NOLOCK)
                                INNER JOIN dbo.CFDI_XML_COMPLEMENTO_PAGO c WITH (NOLOCK)
                                  ON d.COMPLEMENTO_ID = c.ID
                                  WHERE (@UUID = '' OR UPPER(d.ID_DOCUMENTO) LIKE '%' + UPPER(@UUID) + '%')
                                 GROUP BY d.ID_DOCUMENTO, d.COMPLEMENTO_ID
                              
                              ) t

                             ON  t.COMPLEMENTO_ID = xml.ID
                            ";
                    }


                    string sql =
                        @"
                    SELECT
                      xml.ID,
                      xml.USER_ID,
                      xml.UUID,
                      xml.XML,
                      xml.RAZON_SOCIAL_EMISOR,
                      xml.RFC_EMISOR,
                      xml.FECHA_RECEPCION,
                      xml.FECHA_FACTURA,
                      xml.SERIE,
                      xml.FOLIO,
                      xml.NOMBRE_ARCHIVO,
                      xml.FECHA_PAGO,
                      xml.FORMA_DE_PAGO_P,
                      xml.MONEDA_P,
                      xml.MONTO,
                      xml.NUM_OPERACION,
                      xml.RFC_EMISOR_CTA_ORD,
                      xml.NOM_BANCO_ORD_EXT,                        
                      xml.CTA_ORDENANTE,                        
                      xml.RFC_EMISOR_CTA_BEN,                        
                      xml.CTA_BENEFICIARIO
                    FROM dbo.CFDI_XML_COMPLEMENTO_PAGO xml WITH (NOLOCK)
                    " + uuid + @"

                    WHERE 

                    
                    (@UUID<>'' OR @OPT = 0 AND FECHA_RECEPCION BETWEEN CAST((@fecha1 + ' 00:00:00') AS DATETIME) AND CAST((@fecha2 + ' 23:59:59') AS DATETIME) )

                    --AND xml.USER_ID = @USER_ID
                    AND (@RFC_EMISOR IS NULL OR xml.RFC_EMISOR = @RFC_EMISOR)
                    AND (@FOLIO = '' OR UPPER(xml.FOLIO) LIKE '%' + UPPER(@FOLIO) + '%')
                    AND (@SERIE = '' OR UPPER(xml.SERIE) LIKE '%' + UPPER(@SERIE) + '%')
                    --AND (@UUID = '' OR UPPER(xml.UUID) LIKE '%' + UPPER(@UUID) + '%')
                    
                    ORDER BY xml.FECHA_RECEPCION DESC";

                    req.USER_ID = USER_ID;


                    
                    var rs = con.Query<CFDI_XML_COMPLEMENTO_PAGO>(sql, req).ToList();

                    /*
                    if (req.ESTATUS != 0)
                    {

                        sql = @"SELECT * FROM dbo.CFDI_XML_COMPLEMENTO_DETALLE WHERE COMPLEMENTO_ID = @ID";



                        for (var i = rs.Count - 1; i >= 0; i--)
                        {
                            var rs1 = con.Query<CFDI_XML_COMPLEMENTO_DETALLE>(sql, new {ID = rs[i].ID}).ToList();

                            var gp = rs1.GroupBy(g => g.ID_DOCUMENTO).ToDictionary(k => k.Key, v => v.FirstOrDefault());




                                bool existe = true;

                                foreach (var pair in gp)
                                {

                                    var sql2 = @"SELECT * FROM dbo.CFDI_XML_PSI WHERE UUID = @ID";
                                    var rs2 = con.Query<CFDI_XML>(sql2, new { ID = pair.Value.ID_DOCUMENTO })
                                        .FirstOrDefault();

                                    if (rs2 == null)
                                    {
                                        //xm.EXISTE = true;
                                        existe = false;
                                    }
                                }

                                if (req.ESTATUS == 1)
                                {
                                    //con complemento
                                    if (!existe)
                                    {
                                        rs.RemoveAt(i);
                                    }

                                }

                                if (req.ESTATUS == 2)
                                {
                                    //sin complemento
                                    if (existe)
                                    {
                                        rs.RemoveAt(i);
                                    }

                                }
                        }

                    }

                    */




                    return JsonConvert.SerializeObject(rs.Select(
                        s => new
                        {
                            s.ID,
                            s.USER_ID,
                            s.FECHA_FACTURA,
                            s.FECHA_RECEPCION,
                            s.RFC_EMISOR,
                            s.RAZON_SOCIAL_EMISOR,
                            s.UUID,
                            s.SERIE,
                            s.FOLIO,
                            s.CTA_BENEFICIARIO,
                            s.CTA_ORDENANTE,
                            s.FECHA_PAGO,
                            s.FORMA_DE_PAGO_P,
                            s.MONEDA_P,
                            s.MONTO,
                            s.NOM_BANCO_ORD_EXT,
                            s.NUM_OPERACION,
                            s.RFC_EMISOR_CTA_BEN,
                            s.RFC_EMISOR_CTA_ORD,
                        }).ToList());
                }
            }
            catch
            {
                return JsonConvert.SerializeObject(new ArrayList());
            }
        }


   

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string Save(string id, string observaciones)
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
                        UPDATE dbo.CFDI_XML_PSI WITH (Rowlock) SET 
                            OBSERVACIONES = @OBSERVACIONES
                        WHERE ID = @ID";

                 var rs = con.Execute(
                       sql,
                       new { ID = id, OBSERVACIONES = observaciones.Trim() });


                 return "0";
              }
           }
           catch
           {
              return "-1";
           }
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string Valido(string id, string tipo)
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

                 string sql = @"UPDATE CFDI_XML_PSI WITH (Rowlock) SET REVISADO = 1 , TIPO_FACTURA = @TIPO WHERE ID = @ID";

                 con.Execute(
                       sql,
                       new {id, tipo},
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
                   string sql = @"SELECT * FROM dbo.CFDI_XML_COMPLEMENTO_DETALLE WITH (NOLOCK) WHERE COMPLEMENTO_ID = @ID";

                   var rs1 = con.Query<CFDI_XML_COMPLEMENTO_DETALLE>(sql, new { ID = id }).ToList();

                   var rs=new List<CFDI_XML_COMPLEMENTO_DETALLE>();

                   //var gp = rs1.GroupBy(g => g.ID_DOCUMENTO).ToDictionary(k=>k.Key,v=>v.ToList());
                   var gp = rs1.GroupBy(g => g.ID_DOCUMENTO).ToDictionary(k=>k.Key,v=>v.FirstOrDefault());

                   

                   var d = new List<CFDI_XML_COMPLEMENTO_DETALLE>();

                   foreach (var pair in gp)
                   {
                       d.Add(pair.Value);
                   }

                   using (IDbConnection con2 = new SqlConnection(Extensions.GetBD_CFDI()))
                   {
                       sql = @"SELECT * FROM dbo.CFDI_XML_PSI WITH (NOLOCK) WHERE UUID = @ID";

                       /*
                       int last = gp.Count - 1;
                       int index = 0;

                       foreach (var key in gp)
                       {
                           foreach (var xm in key.Value)
                           {
                               

                               var rs2 = con.Query<CFDI_XML>(sql, new { ID = xm.ID_DOCUMENTO }).FirstOrDefault();

                               if (rs2 != null)
                               {
                                   xm.EXISTE = true;
                               }

                               rs.Add(xm);
                           }

                           if (index != last)
                           {
                               rs.Add(new CFDI_XML_COMPLEMENTO_DETALLE
                               {
                                   EXISTE = true
                               });
                           }
                           index++;


                       }*/


                           foreach (var xm in d)
                           {


                               var rs2 = con.Query<CFDI_XML>(sql, new { ID = xm.ID_DOCUMENTO }).FirstOrDefault();

                               if (rs2 != null)
                               {
                                   xm.EXISTE = true;
                               }

                               //rs.Add(xm);
                           }

                       


                       /*
                       foreach (var xm in rs1)
                       {
                           var rs2 = con.Query<CFDI_XML>(sql, new { ID = xm.ID_DOCUMENTO }).FirstOrDefault();

                           if (rs2 != null)
                           {
                               xm.EXISTE = true;
                           }
                       }*/
                   }


                   return JsonConvert.SerializeObject(d);
               }
           }
           catch
           {
              return JsonConvert.SerializeObject("[]");
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
                    var emp = Extensions.cEmpresa();
                    string sql = @"
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
                        INNER JOIN dbo.CFDI_ESTATUS es WITH (NOLOCK) ON xml.ESTATUS = es.ID
                        LEFT OUTER JOIN
                                 (SELECT DISTINCT 
                                    CAST(FECHA_PROCESO AS DATE) FECHA_PROCESO,FOLIOFISCAL
                                    FROM " + emp.ICG_BD + @".dbo.IT_RELFACTURAS_COMPRAS WITH (NOLOCK)) ic
                                        ON xml.UUID = ic.FOLIOFISCAL
WHERE UUID = @ID";

                    var rs = con.Query<CFDI_XML>(sql, new { ID = id }).ToList();

                    foreach (var xml in rs)
                    {
                        if (xml.TIPO_COMPROBANTE != null && (xml.TIPO_COMPROBANTE.ToLower() == "e" || xml.TIPO_COMPROBANTE.ToLower() == "egreso"))
                        {
                            if (xml.TOTAL != null) xml.TOTAL         = xml.TOTAL * -1;
                            if (xml.SUBTOTAL != null) xml.SUBTOTAL   = xml.SUBTOTAL * -1;
                            if (xml.IMPUESTOS != null) xml.IMPUESTOS = xml.IMPUESTOS * -1;

                        }

                    }


                    foreach (var xm in rs)
                    {
                        try
                        {
                            XML3.Comprobante c33 = new XML3.Comprobante();


                            c33 = c33.DeserializeStr(xm.XML);

                            if (c33.Version != null)
                            {

                                if (c33.Impuestos != null && c33.Impuestos.Traslados != null &&
                                    c33.Impuestos.Traslados.Traslado != null)
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

                                if (xm.TIPO_COMPROBANTE != null &&
                                    (xm.TIPO_COMPROBANTE.ToLower() == "e" || xm.TIPO_COMPROBANTE.ToLower() == "egreso"))
                                {
                                    if (xm.IVA != null) xm.IVA          = xm.IVA * -1;
                                    if (xm.IEPS != null) xm.IEPS        = xm.IEPS * -1;
                                    if (xm.DESCUENTO != 0) xm.DESCUENTO = xm.DESCUENTO * -1;
                                }

                                if (c33.CfdiRelacionados != null)
                                {
                                    xm.TIPO_REL = c33.CfdiRelacionados.TipoRelacion;
                                }




                            }


                        }
                        catch (Exception e)
                        {

                        }
                    }


                    return JsonConvert.SerializeObject(
                        rs.Select(
                            s => new
                            {
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
                                s.DESCUENTO,
                                s.UUID,
                                s.TIPO_REL,
                                s.SERIE,
                                s.FOLIO,
                                REVISADO = s.REVISADO ? "OK" : "",
                                s.TIENDA,
                                s.NO_COMPRA,
                                s.FECHA_COMPRA,
                                ESTATUS    = s.ESTATUS2,
                                ESTATUS_ID = s.ESTATUS
                            }).ToList());
                }
            }
            catch
            {
                return JsonConvert.SerializeObject("[]");
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
            public string UUID  { get; set; }
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
           public string        Tipo;
           public List<DETALLE> DATA { get; set; }
        }

        public class DETALLE
        {
           public string DESCRIPCION { get; set; }
           public string CANTIDAD    { get; set; }
           public string UNIDAD      { get; set; }
           public string CLAVE       { get; set; }
           public string PU          { get; set; }
           public string IMPORTE     { get; set; }
        }

    }


}