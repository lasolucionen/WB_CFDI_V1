using System;
using System.Collections;
using System.Collections.Generic;
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
using XML2;
using XML3;
// ReSharper disable InconsistentNaming

namespace WB_CFDI_V1.Vistas
{
    public partial class Pag2 : Page
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
                        "SELECT ID FROM CFDI_ACCOUNT WHERE USER_NAME = @USER_NAME",
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
                    var rs = con.Query<SERIES>("SELECT SERIE, DESCRIPCION FROM SERIES WHERE LEN(SERIE) = 2").ToList();
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

            try
            {

                using (IDbConnection con = new SqlConnection(Extensions.GetBD_ICG()))
                {
/*                    string sql =
                        @"
                        SELECT
                          AC.NUMSERIE,
                          AC.NUMALBARAN,
                          AC.FECHAALBARAN,
                          SUALBARAN,
                          TOTALNETO,
                          P.NIF20
                        FROM ALBCOMPRACAB AC WITH (NOLOCK)
                        INNER JOIN PROVEEDORES P WITH (NOLOCK) ON AC.CODPROVEEDOR = P.CODPROVEEDOR
                        LEFT JOIN IT_RELFACTURAS_COMPRAS R WITH (NOLOCK) ON AC.NUMSERIE COLLATE Modern_Spanish_CI_AS = R.NUMSERIE
                              AND AC.NUMALBARAN = R.NUMALBARAN
                        WHERE
                          FACTURADO = 'F'
                          AND R.NUMFAC IS NULL
                          AND UPPER(P.NIF20) = UPPER(@RFC)
                          AND AC.NUMSERIE LIKE @SERIE + '%'
                          AND AC.FECHAALBARAN BETWEEN CAST((@FECHAINI + ' 00:00:00') AS DATETIME) AND CAST((@FECHAFIN + ' 23:59:59') AS DATETIME)
                        ORDER BY SUALBARAN,
                                 NUMSERIE,
                                 NUMALBARAN,
                                 AC.FECHAALBARAN";*/


                    /*string sql = @"
                    SELECT
                      AC.NUMSERIE,
                      AC.NUMALBARAN,
                      AC.FECHAALBARAN,
                      SUALBARAN,
                      SUM(AT.BRUTO)  SUBTOTAL,
                      SUM(AT.TOTIVA) IVA,
                      SUM(AT.TOTREQ) IEPS,
                      SUM(AT.TOTAL)  TOTAL,
                      P.NIF20
                    FROM ALBCOMPRACAB AC WITH (NOLOCK)
                    INNER JOIN ALBCOMPRATOT AT WITH (NOLOCK) ON AC.NUMSERIE = AT.SERIE
                          AND AC.NUMALBARAN = AT.NUMERO
                          AND AC.n = AT.n
                    INNER JOIN PROVEEDORES P WITH (NOLOCK) ON AC.CODPROVEEDOR = P.CODPROVEEDOR
                    LEFT JOIN IT_RELFACTURAS_COMPRAS R WITH (NOLOCK) ON AC.NUMSERIE COLLATE Modern_Spanish_CI_AS = R.NUMSERIE
                          AND AC.NUMALBARAN = R.NUMALBARAN
                    WHERE
                      FACTURADO = 'F'
                      AND R.NUMFAC IS NULL
                      AND UPPER(P.NIF20) = UPPER(@RFC)
                      AND AC.NUMSERIE LIKE @SERIE + '%'
                      AND AC.FECHAALBARAN BETWEEN CAST((@FECHAINI + ' 00:00:00') AS DATETIME) AND CAST((@FECHAFIN + ' 23:59:59') AS DATETIME)
                    GROUP BY AC.NUMSERIE,
                             AC.NUMALBARAN,
                             AC.FECHAALBARAN,
                             SUALBARAN,
                             P.NIF20
                    ORDER BY SUALBARAN,
                             NUMSERIE,
                             NUMALBARAN,
                             AC.FECHAALBARAN";*/

                    string sql = @"
                    SELECT
                        AC.NUMSERIE,
                        AC.NUMALBARAN,
                        AC.FECHAALBARAN,
                        SUALBARAN,
                        SUM(AT.BRUTO)  SUBTOTAL,
                        SUM(AT.TOTIVA) IVA,
                        SUM(AT.TOTREQ) IEPS,
                        SUM(AT.TOTAL)  TOTAL,
                        P.NIF20
                    FROM ALBCOMPRACAB AC WITH (NOLOCK)
                    INNER JOIN ALBCOMPRATOT AT WITH (NOLOCK) ON AC.NUMSERIE = AT.SERIE
                            AND AC.NUMALBARAN = AT.NUMERO
                            AND AC.N = AT.N
                    INNER JOIN PROVEEDORES P WITH (NOLOCK) ON AC.CODPROVEEDOR = P.CODPROVEEDOR
                    LEFT JOIN IT_RELFACTURAS_COMPRAS R WITH (NOLOCK) ON AC.NUMSERIE COLLATE Modern_Spanish_CI_AS = R.NUMSERIE
                            AND AC.NUMALBARAN = R.NUMALBARAN
                    WHERE
                        FACTURADO = 'F'
                        AND R.NUMFAC IS NULL
                        AND UPPER(P.NIF20) = UPPER(@RFC)
                        AND AC.NUMSERIE LIKE @SERIE + '%'
                        AND AC.FECHAALBARAN BETWEEN CAST((@FECHAINI + ' 00:00:00') AS DATETIME) AND CAST((@FECHAFIN + ' 23:59:59') AS DATETIME)
                        AND AC.NUMSERIE + CONVERT(NVARCHAR(MAX), AC.NUMALBARAN) NOT IN (SELECT
                            AC.NUMSERIE + CONVERT(NVARCHAR(MAX), AC.NUMALBARAN)
                        FROM ALBCOMPRACAB AC WITH (NOLOCK)
                        INNER JOIN TESORERIA AT WITH (NOLOCK)
                            ON AC.NUMSERIE = AT.SERIE
                            AND AC.NUMALBARAN = AT.NUMERO
                            AND AC.N = AT.N
                        INNER JOIN PROVEEDORES P WITH (NOLOCK)
                            ON AC.CODPROVEEDOR = P.CODPROVEEDOR
                        LEFT JOIN IT_RELFACTURAS_COMPRAS R WITH (NOLOCK)
                            ON AC.NUMSERIE COLLATE Modern_Spanish_CI_AS = R.NUMSERIE
                            AND AC.NUMALBARAN = R.NUMALBARAN
                        WHERE FACTURADO = 'F'
                        AND R.NUMFAC IS NULL
                        AND UPPER(P.NIF20) = UPPER(@RFC)
                        AND AC.NUMSERIE LIKE @SERIE + '%'
                        AND AC.FECHAALBARAN BETWEEN CAST((@FECHAINI + ' 00:00:00') AS DATETIME) AND CAST((@FECHAFIN + ' 23:59:59') AS DATETIME)
                        AND AT.CODFORMAPAGO = 1
                        GROUP BY AC.NUMSERIE,
                                    AC.NUMALBARAN)
                    GROUP BY AC.NUMSERIE,
                                AC.NUMALBARAN,
                                AC.FECHAALBARAN,
                                SUALBARAN,
                                P.NIF20
                    ORDER BY SUALBARAN,
                                NUMSERIE,
                                NUMALBARAN,
                                AC.FECHAALBARAN";

                    //Log.Write("Serie: " + serie + " FECHAINI: " + fecha1.ToString("dd/MM/yyyy") + " FECHAFIN: " + fecha2.ToString("dd/MM/yyyy") + " RFC: " + rfc);

                    var rs = con.Query<COMPRAS>(
                        sql,
                        new { SERIE = serie, FECHAINI = fecha1.ToString("dd/MM/yyyy"), FECHAFIN = fecha2.ToString("dd/MM/yyyy"), RFC = rfc }).ToList();

                    //Log.Write("Count: " + rs.Count);
                    
                    return JsonConvert.SerializeObject(rs);
                }
            }
            catch(Exception ex)
            {
               //Log.Write("Error: " + ex.Message);
                return JsonConvert.SerializeObject(new ArrayList());
            }
        }


        public class DETALLE
        {
            public string DESCRIPCION { get; set; }
            public string CANTIDAD { get; set; }
            public string UNIDAD { get; set; }
            public string CLAVE { get; set; }
            public string PU { get; set; }
            public string IMPORTE { get; set; }
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
                                 @"SELECT * FROM CFDI_COMPRAS WHERE ID_XML = @ID_XML",
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
                  string sql = @"SELECT XML,REVISADO FROM dbo.CFDI_XML_PSI WHERE ID = @ID";

                 var rs = con.Query<CFDI_XML>(sql, new {ID = id}).SingleOrDefault();

                 XML3.Comprobante c33 = new XML3.Comprobante();

                 c33 = c33.DeserializeStr(rs.XML);

                 List<DETALLE> ds = new List<DETALLE>();

                 string RFiscal    = "";
                 string UsoCFDI    = "";
                 string MetodoPago = "";
                 string FormaPago  = "";
                 string Moneda     = "";

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
                                   IMPORTE     = cp.Importe.Dbl().ToString("N2")
                                });
                    }

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
                                  Revisado   = rs.REVISADO
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


                    if( req.OPT == 0 )
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
                        FROM dbo.CFDI_XML_PSI xml
                        INNER JOIN dbo.CFDI_ESTATUS es ON xml.ESTATUS = es.ID
                        LEFT OUTER JOIN
                                 (SELECT DISTINCT 
                                    CAST(FECHA_PROCESO AS DATE) FECHA_PROCESO,FOLIOFISCAL
                                    FROM " + emp.ICG_BD + @".dbo.IT_RELFACTURAS_COMPRAS) ic
                                        ON xml.UUID = ic.FOLIOFISCAL
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
                    }
                    else
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
                          xml.REVISADO,
                          xml.TIENDA,
                          xml.NO_COMPRA,
                          xml.FECHA_COMPRA,
                          xml.NOMBRE_ARCHIVO,
                          xml.ESTATUS,
                          es.ESTATUS AS ESTATUS2,
                          xml.TIPO_COMPROBANTE,
                          ic.FECHA_PROCESO
                        FROM dbo.CFDI_XML_PSI xml
                        INNER JOIN dbo.CFDI_ESTATUS es ON xml.ESTATUS = es.ID
                        INNER JOIN
                                 (SELECT DISTINCT 
                                    CAST(FECHA_PROCESO AS DATE) FECHA_PROCESO,FOLIOFISCAL
                                    FROM " + emp.ICG_BD + @".dbo.IT_RELFACTURAS_COMPRAS) ic
                                        ON xml.UUID = ic.FOLIOFISCAL
                        WHERE ic.FECHA_PROCESO
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

                    var rs = con.Query<CFDI_XML>(sql, req);

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
                                            new { ID = xm.ID, TIPO_COMPROBANTE = c32.TipoDeComprobante },
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
                                           s.UUID,
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

                /*
                    string sql = @"
                            SELECT NUMSERIE, NUMALBARAN, NUMLIN, REFERENCIA, DESCRIPCION, UNIDADESTOTAL
                            FROM ALBCOMPRALIN WITH(NOLOCK)
                            WHERE NUMSERIE = @NUMSERIE AND NUMALBARAN = @NUMALBARAN";
                 
SELECT REFERENCIA, DESCRIPCION, UNIDADESTOTAL CANTIDAD,
PRECIO, DTO, TOTAL, TIPOIMPUESTO, CODALMACEN ALM                
                 */

                using (IDbConnection con = new SqlConnection(Extensions.GetBD_ICG()))
                {
                    //dbConnection.Open(); , AC.CODCLIENTE, C.NOMBRECLIENTE
                    string sql = @"
                            SELECT REFERENCIA, DESCRIPCION, UNIDADESTOTAL,
                                   PRECIO, DTO, TOTAL, TIPOIMPUESTO, CODALMACEN
                            FROM ALBCOMPRALIN WITH(NOLOCK)
                            WHERE NUMSERIE = @NUMSERIE AND NUMALBARAN = @NUMALBARAN";

                    var rs = con.Query<ALBCOMPRALIN>(sql, new {
                                                                 data.NUMSERIE,
                                                                 data.NUMALBARAN }).ToList();
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

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string Procesar(List<REQ1> facturas, List<REQ2> compras,  String fechaProd ) //double dif
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            if( fechaProd != "" )
            {
               fechaProd = Convert.ToDateTime(fechaProd).ToString("yyyy/MM/dd");
            }

            var msg = new Dictionary<string, string>();

            string USER_NAME = "";
            if (current.User != null && current.User.Identity.IsAuthenticated)
            {
                FormsIdentity id = (FormsIdentity)current.User.Identity;
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



/*            using (IDbConnection con = new SqlConnection(Extensions.GetBD()))
            {
                foreach (var factura in facturas)
                {
                    var rs = con.Query<CFDI_XML>(
                        "SELECT ESTATUS FROM CFDI_XML WHERE ID = @ID AND ESTATUS = 2",
                        new {ID = factura.ID}).ToList();

                    if (rs.Count > 0)
                    {
                        msg.Add("code", "-1");
                        msg.Add("msg", "No se pudo procesar ya que hay alguna factura que ya han sido asociada.");

                        return JsonConvert.SerializeObject(msg);
                    }
                }
            }*/

            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {
                foreach (var factura in facturas)
                {
                    var rs = con.Query<CFDI_XML>(
                        "SELECT ESTATUS FROM CFDI_XML_PSI WHERE ID = @ID AND ESTATUS <> 1",
                        new {
                               factura.ID }).ToList();

                    if (rs.Count > 0)
                    {
                        msg.Add("code", "-1");
                        msg.Add("msg", "Solo pueden ser procesadas las facturas con estatus de Recibidas.");

                        return JsonConvert.SerializeObject(msg);
                    }
                }
            }


            double dif = 0;
            try
            {
                double importe1 = Math.Round(facturas.Sum(s => s.TOTAL.Value), 2);
                double importe2 = Math.Round(compras.Sum(s => s.TOTAL.Value), 2);
                dif = Math.Round(importe1 - importe2, 2);
            }
            catch (Exception ex)
            {
                Log.Date();
                Log.Write("Error:" + ex.Message);

                foreach (var factura in facturas)
                {
                    //Log.Write("Factutas Total: " + factura.TOTAL.Value);
                }

                foreach (var compra in compras)
                {
                    //Log.Write("Compras Total: " + compra.TOTAL.Value);
                }

                msg.Add("code", "-1");
                msg.Add("msg", ex.Message);
                return JsonConvert.SerializeObject(msg);
            }

            /*msg.Add("code", "-1");
            msg.Add("msg", "Registros procesados correctamente.");

            return JsonConvert.SerializeObject(msg);*/




                using (IDbConnection con = new SqlConnection(Extensions.GetBD_ICG()))
                {
                    IDbTransaction tran = null;
                    try
                    {
                        //var rs = con.Query<SERIES>("SELECT SERIE, DESCRIPCION FROM SERIES WHERE LEN(SERIE) = 2").ToList();
                        con.Open();

                        string sql = @"
                                    DELETE FROM IT_RELFACTURAS_COMPRAS
                                    WHERE 
                                        FECHA_PROCESO IS NULL
                                    AND
                                        @USUARIO IS NOT NULL
                                    AND
                                        @USUARIO <> ''
                                    AND
                                        USUARIO = @USUARIO";

                        con.Execute(sql, new {USUARIO = USER_NAME});

                        tran = con.BeginTransaction();

                        
                        sql =
                            @"INSERT INTO IT_RELFACTURAS_COMPRAS WITH(ROWLOCK)
                              VALUES (@FOLIOFISCAL, @SERIEINTERNA, @FOLIOINTERNO, @LIN_FAC, @LIN, @NUMSERIE, @NUMALBARAN, NULL, 0, NULL, @USUARIO)";

                        if (facturas.Count == 1 && compras.Count == 1)
                        {

                            var rs = con.Execute(
                                sql,
                                new IT_RELFACTURAS_COMPRAS(
                                    facturas[0].UUID,
                                    facturas[0].SERIE,
                                    facturas[0].FOLIO,
                                    1,
                                    1,
                                    compras[0].NUMSERIE,
                                    compras[0].NUMALBARAN,
                                    USER_NAME),
                                tran);

                            tran.Commit();
                            tran = null;

                            //--------------------------------------------------------//
                            var contr = facturas[0].CONTRA_RECIBO_ID == null
                                ? ""
                                : Extensions.GetSerie() + "-" + facturas[0].CONTRA_RECIBO_ID;
                            
                            var err = con.ExecuteScalar<string>(
                                "Sp_IT_FacturasCompras",
                                new
                                {
                                    COMMIT = 0,
                                    EMPRESA = Extensions.GetEmpresa(),
                                    USUARIO = USER_NAME,
                                    CONTRARECIBO = contr,
                                    DIFERENCIA = dif
                                },
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 3000);
                            
                            if (err != null && err.ToLower().Contains("error"))
                            {
                                //throw new Exception(err);
                                //Log.Write("1:1 SP: " + err + "\r\n", false);
                            }
                            //--------------------------------------------------------//

                            sql = @"
                                    SELECT * FROM IT_RELFACTURAS_COMPRAS
                                    WHERE 
                                        FOLIOFISCAL  = @FOLIOFISCAL 
                                    AND USUARIO      = @USUARIO";

                            var rs1 = con.Query<IT_RELFACTURAS_COMPRAS>(
                                      sql,
                                      new
                                      {
                                           FOLIOFISCAL = facturas[0].UUID,
                                           USUARIO     = USER_NAME
                                      }).ToList();

                            if (rs1.Count == 0 || rs1.All(a => a.FECHA_PROCESO == null))
                            {
                                 throw new Exception("No se pudo procesar. " + err);
                            }

                            //--------------------------------------------------------//

                            using (IDbConnection con2 = new SqlConnection(Extensions.GetBD_CFDI()))
                            {
                                con2.Execute(
                                    "UPDATE CFDI_XML_PSI SET ESTATUS = 2 WHERE ID = @ID",
                                    new {
                                           facturas[0].ID});
                            }

                            msg.Add("code", "0");
                            msg.Add("msg", "Registros procesados correctamente.");

                            return JsonConvert.SerializeObject(msg);
                        }

                        if (facturas.Count > 1 && compras.Count == 1)
                        {
                            int LIN_FAC = 1;
                            int LIN = 1;

                            foreach (var factura in facturas)
                            {
                                var rs = con.Execute(
                                    sql,
                                    new IT_RELFACTURAS_COMPRAS(
                                        factura.UUID,
                                        factura.SERIE,
                                        factura.FOLIO,
                                        LIN_FAC,
                                        LIN,
                                        compras[0].NUMSERIE,
                                        compras[0].NUMALBARAN,
                                        USER_NAME),
                                    tran);
                                LIN_FAC++;
                                LIN = 0;
                            }

                            tran.Commit();
                            tran = null;

                            //--------------------------------------------------------//
                            var err = con.ExecuteScalar<string>(
                                "Sp_IT_FacturasCompras",
                                new
                                {
                                    COMMIT = 0,
                                    EMPRESA = Extensions.GetEmpresa(),
                                    USUARIO = USER_NAME,
                                    CONTRARECIBO = "",
                                    DIFERENCIA = dif
                                },
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 3000);

                            if (err != null && err.ToLower().Contains("error"))
                            {
                                //throw new Exception(err);
                                //Log.Write("N:1 SP: " + err + "\r\n", false);
                            }
                            //--------------------------------------------------------//

                            sql = @"
                                    SELECT * FROM IT_RELFACTURAS_COMPRAS
                                    WHERE 
                                        FOLIOFISCAL  = @FOLIOFISCAL 
                                    AND USUARIO      = @USUARIO";

                            foreach (var factura in facturas)
                            {
                                 var rs1 = con.Query<IT_RELFACTURAS_COMPRAS>(
                                           sql,
                                           new
                                           {
                                                FOLIOFISCAL = factura.UUID,
                                                USUARIO     = USER_NAME
                                           }).ToList();

                                 if (rs1.Count == 0 || rs1.All(a => a.FECHA_PROCESO == null))
                                 {
                                      throw new Exception("No se pudo procesar. " + err);
                                 }
                            }

                            /* sql = @"
                                    SELECT * FROM IT_RELFACTURAS_COMPRAS
                                    WHERE 
                                        FOLIOFISCAL  = @FOLIOFISCAL 
                                    AND USUARIO      = @USUARIO";

                            bool procesado = false;

                            foreach( var factura in facturas )
                            {
                                 var rs1 = con.Query<IT_RELFACTURAS_COMPRAS>(
                                           sql,
                                           new{
                                                   FOLIOFISCAL  = factura.UUID,
                                                   USUARIO      = USER_NAME
                                              }).FirstOrDefault();


                                 if (rs1 != null && rs1.FECHA_PROCESO != null)
                                 {
                                      procesado = true;
                                 }

                            }

                            if (!procesado)
                            {
                                 Log.Write("T1: " + procesado + " Err", false);

                                 //throw new Exception("No se pudo procesar.");

                                 foreach (var factura in facturas)
                                 {
                                     var rs1 = con.Query<IT_RELFACTURAS_COMPRAS>(
                                               sql,
                                               new
                                               {
                                                   FOLIOFISCAL = factura.UUID,
                                                   USUARIO = USER_NAME
                                               }).FirstOrDefault();

                                     if (rs1 != null)
                                     {
                                          Log.Write(
                                                    "T1: FOLIOFISCAL: "
                                                    + rs1.FOLIOFISCAL
                                                    + " SERIEINTERNA: "
                                                    + rs1.SERIEINTERNA
                                                    + " FOLIOINTERNO: "
                                                    + rs1.FOLIOINTERNO
                                                    + " LIN_FAC: "
                                                    + LIN_FAC
                                                    + " LIN: "
                                                    + rs1.LIN
                                                    + " NUMSERIE: "
                                                    + rs1.NUMSERIE
                                                    + " NUMALBARAN: "
                                                    + rs1.NUMALBARAN
                                                    + " FECHAPROCESO:"
                                                    + rs1.FECHA_PROCESO,
                                                    false);
                                     }

                                 }
                            }*/
                            //--------------------------------------------------------//
                            
                            using (IDbConnection con2 = new SqlConnection(Extensions.GetBD_CFDI()))
                            {
                                foreach (var factura in facturas)
                                {
                                    con2.Execute(
                                        "UPDATE CFDI_XML_PSI SET ESTATUS = 2 WHERE ID = @ID",
                                        new {
                                               factura.ID});
                                }
                            }


                            msg.Add("code", "0");
                            msg.Add("msg", "Registros procesados correctamente.");

                            return JsonConvert.SerializeObject(msg);
                        }

                        if (facturas.Count == 1 && compras.Count > 1)
                        {
                            int LIN_FAC = 1;
                            int LIN = 1;

                            foreach (var compra in compras)
                            {
                                var rs = con.Execute(
                                    sql,
                                    new IT_RELFACTURAS_COMPRAS(
                                        facturas[0].UUID,
                                        facturas[0].SERIE,
                                        facturas[0].FOLIO,
                                        LIN_FAC,
                                        LIN,
                                        compra.NUMSERIE,
                                        compra.NUMALBARAN,
                                        USER_NAME),
                                    tran);
                                LIN++;
                            }

                            tran.Commit();
                            tran = null;

                            //--------------------------------------------------------//
                            var contr = facturas[0].CONTRA_RECIBO_ID == null
                                ? ""
                                : Extensions.GetSerie() + "-" + facturas[0].CONTRA_RECIBO_ID;
                            
                            var err = con.ExecuteScalar<string>(
                                "Sp_IT_FacturasCompras",
                                new
                                {
                                    COMMIT = 0,
                                    EMPRESA = Extensions.GetEmpresa(),
                                    USUARIO = USER_NAME,
                                    CONTRARECIBO = contr,
                                    DIFERENCIA = dif
                                },
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 3000);

                            if (err != null && err.ToLower().Contains("error"))
                            {
                                //throw new Exception(err);
                                //Log.Write("1:N SP: " + err + "\r\n", false);
                            }
                            //--------------------------------------------------------//

                            sql = @"
                                    SELECT * FROM IT_RELFACTURAS_COMPRAS
                                    WHERE 
                                        FOLIOFISCAL  = @FOLIOFISCAL 
                                    AND USUARIO      = @USUARIO";

                            var rs1 = con.Query<IT_RELFACTURAS_COMPRAS>(
                                      sql,
                                      new
                                      {
                                           FOLIOFISCAL = facturas[0].UUID,
                                           USUARIO     = USER_NAME
                                      }).ToList();

                            if (rs1.Count == 0 || rs1.All(a => a.FECHA_PROCESO == null))
                            {
                                 throw new Exception("No se pudo procesar. " + err);
                            }

                            /*
                            sql = @"
                                    SELECT * FROM IT_RELFACTURAS_COMPRAS
                                    WHERE 
                                        FOLIOFISCAL  = @FOLIOFISCAL 
                                    AND USUARIO      = @USUARIO";

                            bool procesado = false;

                            var rs1 = con.Query<IT_RELFACTURAS_COMPRAS>(
                                      sql,
                                      new {
                                               FOLIOFISCAL  = facturas[0].UUID,
                                               USUARIO      = USER_NAME
                                          }).ToList();

                            if (rs1.Count > 0)
                            {
                                 foreach( var it in rs1 )
                                 {
                                      if (it.FECHA_PROCESO != null)
                                      {
                                           procesado = true;
                                      }
                                 }
                            }

                            if (!procesado)
                            {
                                 Log.Write("T2: " + procesado + " Err", false);

                                 //throw new Exception("No se pudo procesar.");
                                 if (rs1.Count > 0)
                                 {
                                      foreach( var it in rs1 )
                                      {
                                           Log.Write(
                                                     "T2: FOLIOFISCAL: "
                                                     + it.FOLIOFISCAL
                                                     + " SERIEINTERNA: "
                                                     + it.SERIEINTERNA
                                                     + " FOLIOINTERNO: "
                                                     + it.FOLIOINTERNO
                                                     + " LIN_FAC: "
                                                     + LIN_FAC
                                                     + " LIN: "
                                                     + it.LIN
                                                     + " NUMSERIE: "
                                                     + it.NUMSERIE
                                                     + " NUMALBARAN: "
                                                     + it.NUMALBARAN
                                                     + " FECHAPROCESO:"
                                                     + it.FECHA_PROCESO,
                                                     false);
                                      }
                                 }
                            }*/
                            //--------------------------------------------------------//
                            
                            using (IDbConnection con2 = new SqlConnection(Extensions.GetBD_CFDI()))
                            {
                                con2.Execute(
                                    "UPDATE CFDI_XML_PSI SET ESTATUS = 2 WHERE ID = @ID",
                                    new {
                                           facturas[0].ID});
                            }

                            msg.Add("code", "0");
                            msg.Add("msg", "Registros procesados correctamente.");

                            return JsonConvert.SerializeObject(msg);
                        }

                        msg.Add("code", "-1");
                        msg.Add("msg", "No hay registros para procesar.");
                        return JsonConvert.SerializeObject(msg);
                    }
                    catch (Exception ex)
                    {

                        if (tran != null)
                        {
                            tran.Rollback();
                        }



                        try
                        {
                            /*
                            using (IDbConnection con2 = new SqlConnection(Extensions.GetBD_ICG()))
                            {
                                foreach (var factura in facturas)
                                {
                                    string sql = @"
                                    DELETE FROM IT_RELFACTURAS_COMPRAS
                                    WHERE 
                                        FOLIOFISCAL  = @FOLIOFISCAL 
                                    AND USUARIO      = @USUARIO
                                    AND SERIEINTERNA = @SERIEINTERNA
                                    AND FOLIOINTERNO = @FOLIOINTERNO";

                                    var rs = con2.Execute(
                                              sql,
                                              new {
                                                       FOLIOFISCAL  = factura.UUID,
                                                       USUARIO      = USER_NAME,
                                                       SERIEINTERNA = factura.SERIE,
                                                       FOLIOINTERNO = factura.FOLIO,
                                                  });
                                }
                            }
                            */

                            Log.Date();


                            if (facturas.Count > 1 && compras.Count == 1)
                            {
                                int LIN_FAC = 1;
                                int LIN = 1;

                                foreach (var factura in facturas)
                                {
                                    Log.Write(
                                        "FOLIOFISCAL: " + factura.UUID + " SERIEINTERNA: " + factura.SERIE +
                                        " FOLIOINTERNO: " + factura.FOLIO + " LIN_FAC: " + LIN_FAC +
                                        " LIN: " + LIN + " NUMSERIE: " + compras[0].NUMSERIE + " NUMALBARAN: " +
                                        compras[0].NUMALBARAN, false);

                                    LIN_FAC++;
                                    LIN = 0;
                                }

                                var contr = facturas[0].CONTRA_RECIBO_ID == null
                                    ? ""
                                    : Extensions.GetSerie() + "-" + facturas[0].CONTRA_RECIBO_ID;

                                Log.Write("\r\nCONTRARECIBO: " + contr + " DIF: " + dif + " USUARIO: " + USER_NAME + " EMPRESA: " + Extensions.GetEmpresa() + " RFC_EMISOR: " + facturas[0].RFC_EMISOR, false);

                                Log.Write("ERROR: " + ex.Message + "\r\n", false);

                            }



                            if (facturas.Count == 1 && compras.Count == 1)
                            {

                                var contr = facturas[0].CONTRA_RECIBO_ID == null
                                    ? ""
                                    : Extensions.GetSerie() + "-" + facturas[0].CONTRA_RECIBO_ID;


                                Log.Write(
                                    "FOLIOFISCAL: " + facturas[0].UUID + " SERIEINTERNA: " + facturas[0].SERIE +
                                    " FOLIOINTERNO: " + facturas[0].FOLIO + " LIN_FAC: 1 LIN: 1 NUMSERIE: " +
                                    compras[0].NUMSERIE + " NUMALBARAN: " + compras[0].NUMALBARAN,
                                    false);

                                Log.Write("\r\nCONTRARECIBO: " + contr + " DIF: " + dif + " USUARIO: " + USER_NAME + " EMPRESA: " + Extensions.GetEmpresa() + " RFC_EMISOR: " + facturas[0].RFC_EMISOR, false);

                                Log.Write("ERROR: " + ex.Message + "\r\n", false);

                            }


                            if (facturas.Count == 1 && compras.Count > 1)
                            {
                                int LIN_FAC = 1;
                                int LIN = 1;

                                foreach (var compra in compras)
                                {
                                    Log.Write(
                                        "FOLIOFISCAL: " + facturas[0].UUID + " SERIEINTERNA: " + facturas[0].SERIE +
                                        " FOLIOINTERNO: " + facturas[0].FOLIO + " LIN_FAC: " + LIN_FAC +
                                        " LIN: " + LIN + " NUMSERIE: " + compra.NUMSERIE + " NUMALBARAN: " +
                                        compra.NUMALBARAN, false);


                                    LIN++;
                                }

                                var contr = facturas[0].CONTRA_RECIBO_ID == null
                                    ? ""
                                    : Extensions.GetSerie() + "-" + facturas[0].CONTRA_RECIBO_ID;


                                Log.Write("\r\nCONTRARECIBO: " + contr + " DIF: " + dif + " USUARIO: " + USER_NAME + " EMPRESA: " + Extensions.GetEmpresa() + " RFC_EMISOR: " + facturas[0].RFC_EMISOR, false);

                                Log.Write("ERROR: " + ex.Message + "\r\n", false);
                            }

                        }
                        catch (Exception e)
                        {

                        }


                        msg.Add("code", "-1");
                        msg.Add("msg", ex.Message);

                        return JsonConvert.SerializeObject(msg);
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
        public static string SetAccion(List<CFDI_XML> data,string estatus)
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

                   if (data.Any(a => a.ESTATUS == 2 || a.ESTATUS == 6))
                   {
                       throw new Exception("Desmarque las facturas con estatus de asociadas o pagadas.");
                   }



                    con.Open();
                    tran = con.BeginTransaction();
                    //RFC_EMISOR
                    //INSERT INTO CONTRA_RECIBOS DEFAULT VALUES;

                    string sql = @"UPDATE CFDI_XML_PSI SET ESTATUS = @ESTATUS WHERE ID = @ID";
                    
                    foreach (var xml in data)
                    {
                        con.Execute(
                                sql,
                                new { xml.ID, estatus },
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

                    return JsonConvert.SerializeObject(new {code = -1, msg = ex.Message});
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

                 string sql = @"UPDATE CFDI_XML_PSI SET REVISADO = 1 WHERE ID = @ID";

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
        }

        public class REQ2
        {
            public string NUMSERIE { get; set; } 
            public int NUMALBARAN { get; set; }
            public double? TOTAL { get; set; }
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

        public class DATA3
        {
           public string        RFiscal;
           public string        UsoCFDI;
           public string        MetodoPago;
           public string        FormaPago;
           public string        Moneda;
           public bool          Revisado;
           public List<DETALLE> DATA { get; set; }
        }
    }


}