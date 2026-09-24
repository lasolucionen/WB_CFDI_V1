using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
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
using XML3;
using Calendar = System.Globalization.Calendar;

namespace WB_CFDI_V1.Vistas
{
    public partial class Pag1 : Page
    {


        //----------------------------------------------------------------------------------
        public string Detalle2
        {
            get;
            set;
        }

        public string CargaZIP
        {
            get;
            set;
        }
        public string CargaXML
        {
            get;
            set;
        }

        public string VistaPrevia1
        {
            get;
            set;
        }
        //----------------------------------------------------------------------------------



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

            Detalle2     = TemplateHelper.RenderPartialToString("~/Vistas/Pag1/Detalle2.ascx", "");
            CargaZIP     = TemplateHelper.RenderPartialToString("~/Vistas/Pag1/CargaZIP.ascx", "");
            CargaXML     = TemplateHelper.RenderPartialToString("~/Vistas/Pag1/CargaXML.ascx", "");
            VistaPrevia1 = TemplateHelper.RenderPartialToString("~/Vistas/Pag1/VistaPrevia1.ascx", "");
        }

        protected static string getFecha1()
        {
            return DateTime.Now.AddDays(-DateTime.Now.Day + 1).ToString("yyyy,MM,dd");
        }



        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string GetFechaLimite()
        {
            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {

                try
                {
                    string sql = @"SELECT CHK FROM FECHA_REC_FACTURA WITH (NOLOCK) WHERE ID = 1";
                    var chk = con.QueryFirstOrDefault<bool?>(sql);

                    if (chk.Value)
                    {

                        sql = @"SELECT FECHA FROM FECHA_REC_FACTURA WITH (NOLOCK) WHERE ID = 1";
                        var dia = con.QueryFirstOrDefault<int?>(sql);

                        var maxDate = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

                        if (dia > maxDate)
                        {
                            dia = maxDate;
                        }

                        if (DateTime.Now.Day > dia)
                        {
                            return JsonConvert.SerializeObject(new
                                {code = 0, chk = 1, fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, dia.Value).ToString("dd/MM/yyyy")});
                        }
                        else
                        {
                            return JsonConvert.SerializeObject(new { code = 0, chk = 0 });
                        }

                        
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { code = 0, chk = 0 });
                    }

                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { code = -1, msg = "Error: " + ex.Message });
                }
            }
        }


        protected static string getRazonSocial()
        {
            
            try
            {

                using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                {
                    var rs = con.Query<string>("SELECT MAX(RAZON_SOCIAL_EMISOR) RAZON_SOCIAL_EMISOR FROM CFDI_XML_PSI WITH (NOLOCK) WHERE RFC_EMISOR = @USER_NAME GROUP BY RFC_EMISOR",
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
        public static string GetPSerieFolio()
        {

            try
            {

                using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                {
                    var rs = con.ExecuteScalar<bool>("SELECT PEDIR_SERIE_FOLIO FROM CFDI_ACCOUNT WHERE USER_NAME = @USER_NAME",
                        new { USER_NAME = Extensions.GetUserName() });

                    if (rs)
                    {
                        return "1";
                    }
                    else
                    {
                        return "0";
                    }


                    
                }
            }
            catch(Exception ex)
            {
                return "-1";
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
                      xml.OBSERVACION1,
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
                          (
                                    SELECT STRING_AGG (T.ALBARAN,', ')  FROM
                                    (
                                         SELECT cc.SERIE + '-' + cc.FOLIO ALBARAN FROM CFDI_COMPRAS cc WHERE ID_XML = xml.ID 
                                         UNION ALL
                                          SELECT cx.TIENDA + '-' + cx.NO_COMPRA ALBARAN FROM CFDI_XML_PSI cx WHERE cx.ID=xml.ID
                                          AND NOT EXISTS (SELECT 1 FROM CFDI_COMPRAS cc2 WHERE cc2.SERIE = cx.TIENDA AND cc2.FOLIO = cx.NO_COMPRA)
                                    ) T
                          ) ALBARAN,

                      xml.NO_COMPRA,
                      xml.FECHA_COMPRA,
                      --xml.REVISADO,
                      xml.NOMBRE_ARCHIVO,
                      es.ESTATUS AS ESTATUS2,
                      xml.TIPO_COMPROBANTE,
                      xml.NUMEFECTO,
                      xml.FECHA_SALDADO,
                      ct.FECHA_PAGO,
                      b.CHECK_ID,
                      b.CHECK_NUMBER,
                      b.CHECK_DATE
                    FROM dbo.CFDI_XML_PSI xml WITH (NOLOCK)
                    INNER JOIN dbo.CFDI_ESTATUS es WITH (NOLOCK) ON xml.ESTATUS = es.ID
                    LEFT OUTER JOIN dbo.CONTRA_RECIBOS_PSI ct WITH (NOLOCK) ON xml.CONTRA_RECIBO_ID = ct.ID

                    LEFT OUTER JOIN CFDI_GENERAL.dbo.XXROD_AP_PAYMENTS_BY_INVOICE b WITH (NOLOCK) ON xml.UUID = b.UUID

                    WHERE 
                    
                    (@OPT = 0 AND xml.FECHA_RECEPCION BETWEEN CAST((@fecha1 + ' 00:00:00') AS DATETIME) AND CAST((@fecha2 + ' 23:59:59') AS DATETIME)
                    OR
                    @OPT = 1 AND xml.FECHA_SALDADO BETWEEN CAST((@fecha1 + ' 00:00:00') AS DATETIME) AND CAST((@fecha2 + ' 23:59:59') AS DATETIME))

                    AND xml.USER_ID = @USER_ID

                    AND (@ESTATUS = 0 OR (@ESTATUS = 7 AND b.CHECK_ID IS NOT NULL) OR xml.ESTATUS = @ESTATUS)

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


                            c33 = c33.DeserializeStr(xm.XML);

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
                        s.OBSERVACION1,
                        s.RFC_EMISOR,
                        s.TOTAL,
                        s.SUBTOTAL,
                        s.IMPUESTOS,
                        s.ALBARAN,
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
                        s.FECHA_PAGO,
                        s.CHECK_ID,
                        s.CHECK_NUMBER,
                        s.CHECK_DATE
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
                        UPDATE dbo.CFDI_XML_PSI WITH (Rowlock) SET 
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

                       /*
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
                      */

                       string sql = "";

                       sql = @"

                        DELETE FROM CFDI_COMPRAS WITH (Rowlock) WHERE ID_XML = @id;";

                       con.Execute(
                           sql,
                           new{id},
                           transaction: tran);


                        sql = @"

                        INSERT INTO CFDI_COMPRAS WITH (Rowlock) (ID_XML, SERIE, FOLIO) 
                                               VALUES(@ID_XML, @SERIE, @FOLIO);";

                       con.Execute(
                                 sql,
                                 data,
                                 transaction: tran);




                       sql = @"UPDATE dbo.CFDI_XML_PSI WITH (Rowlock) SET FECHA_COMPRA = @FECHA WHERE ID = @ID";

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
                               @"

IF NOT EXISTS(

SELECT 1 FROM CFDI_XML_PSI cc
INNER JOIN CFDI_COMPRAS cx ON cc.ID = cx.ID_XML
WHERE cc.ID=@ID_XML AND cx.SERIE = cc.TIENDA AND cx.FOLIO = cc.NO_COMPRA

) AND EXISTS (SELECT 1 FROM CFDI_XML_PSI WHERE ISNULL(TIENDA,'') <> '' AND ID=@ID_XML) BEGIN  

  INSERT INTO CFDI_COMPRAS (ID_XML, SERIE, FOLIO)
  VALUES (@ID_XML, (SELECT TIENDA FROM CFDI_XML_PSI WHERE ID=@ID_XML), (SELECT NO_COMPRA FROM CFDI_XML_PSI WHERE ID=@ID_XML) );
END

                                SELECT * FROM CFDI_COMPRAS WITH (NOLOCK) WHERE ID_XML = @ID_XML",
                               new { ID_XML = id }
                               ).ToList();


                     var it2 = con.Query<string>(
                         @"
                                    SELECT STRING_AGG (T.ALBARAN,', ')  FROM
                                    (
                                         SELECT cc.SERIE + '-' + cc.FOLIO ALBARAN FROM CFDI_COMPRAS cc WHERE ID_XML = @ID_XML 
                                         --UNION ALL
                                          --SELECT cx.TIENDA + '-' + cx.NO_COMPRA ALBARAN FROM CFDI_XML_PSI cx WHERE cx.ID=@ID_XML
                                          --AND NOT EXISTS (SELECT 1 FROM CFDI_COMPRAS cc2 WHERE cc2.SERIE = cx.TIENDA AND cc2.FOLIO = cx.NO_COMPRA)
                                    ) T

",
                         new { ID_XML = id }
                     );


                    return JsonConvert.SerializeObject(new { code = 0, data = it, data2 = it2, msg = "" });
                }
            }
            catch(Exception ex)
            {
                 return JsonConvert.SerializeObject(new{code = -1, data = "[]", msg = ex.Message});
            }
        }



        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string InfoPass(string pass1)
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
                                    SELECT *
                                    FROM CFDI_ACCOUNT u WITH (NOLOCK)
                                    WHERE
                                      u.user_name = @UserName
                                      AND u.Password = @Password
                                    ";

                    var acc = con.Query<CFDI_ACCOUNT>(sql, new { UserName = Extensions.GetUserName(), Password = pass1 })
                        .FirstOrDefault();

                    if (acc == null)
                    {
                        return JsonConvert.SerializeObject(new { f = "-1" });
                    }

                    return JsonConvert.SerializeObject(new { f = "0" });
                }
            }
            catch
            {
                return JsonConvert.SerializeObject(new { f = "-1" });
            }
        }


        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static string NewPass(string pass1)
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
                                    UPDATE CFDI_ACCOUNT WITH (Rowlock) SET PASSWORD = @Password
                                    WHERE
                                      user_name = @UserName
                                    ";

                    con.Execute(sql, new { UserName = Extensions.GetUserName(), Password = pass1 });



                    return JsonConvert.SerializeObject(new { code = "0" });
                }
            }
            catch(Exception ex)
            {
                return JsonConvert.SerializeObject(new { code = "-1", msg=ex.Message });
            }
        }


        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod(EnableSession = true)]
        public static string CheckSF(string SERIE, string FOLIO)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            try
            {
                //return JsonConvert.SerializeObject(new { code = "0" });
                using (IDbConnection con = new SqlConnection(Extensions.GetBD_ICG()))
                {
                    string sql1 = @"
                                    SELECT * FROM PROVEEDORES
                                    WHERE
                                      NIF20 = @UserName
                                    ";

                    var it1 = con.Query<PROVEEDORES>(sql1, new { UserName = Extensions.GetUserName()}).FirstOrDefault();


                    if (it1 == null)
                    {
                        throw new Exception("No se encontro el proveedor: " + Extensions.GetUserName());
                    }


                    string sql2 = @"
                                    SELECT * FROM ALBCOMPRACAB
                                    WHERE
                                      CODPROVEEDOR = @CODPROVEEDOR AND
                                      UPPER(NUMSERIE) = UPPER(@SERIE) AND
                                      UPPER(NUMALBARAN) = UPPER(@FOLIO)
                                    ";

                    var it2 = con.Query<ALBCOMPRACAB>(sql2, new { it1.CODPROVEEDOR, SERIE, FOLIO }).FirstOrDefault();


                    if (it2 == null)
                    {
                        throw new Exception("La serie: <strong>" + SERIE + "</strong> y Folio: <strong>" + FOLIO + "</strong> no son validos.");
                    }


                    return JsonConvert.SerializeObject(new { code = "0" });
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { code = "-1", msg = ex.Message });
            }
        }


        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod(EnableSession = true)]
        public static string SetTmpXML(List<Data2> zipForm)
        {

            var tmpXml = CFDI_XML_TMP.GetData();

            try
            {
                  //HttpContext.Current.Session["tmpXML"];

                //(List<TmpXML>)
                if (tmpXml.Count==0)
                {
                    throw new Exception("Error 001");
                }

                //var tXML = JsonConvert.DeserializeObject<List<TmpXML>>(tmpXml.Str());

                DateTime fecha = DateTime.Now;


                using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                {
                    con.Open();
                    IDbTransaction tran = con.BeginTransaction();



                    foreach (var data in tmpXml)
                    {

                        XML3.Comprobante c33 = new XML3.Comprobante();

                        c33 = c33.DeserializeStr(data.Xml);

                        string emisorRFC = null;
                        string receptorRFC = null;
                        string uuid = null;
                        string emisorNombre = null;
                        string ImpTras = null;
                        string ImpRet = null;
                        List<CFDI_RETENCIONES> LstRet = new List<CFDI_RETENCIONES>();

                        if (c33.Emisor != null)
                        {
                            emisorRFC = c33.Emisor.Rfc;
                        }

                        if (c33.Receptor != null)
                        {
                            receptorRFC = c33.Receptor.Rfc;
                        }

                        if (c33.Complemento != null && c33.Complemento.TimbreFiscalDigital != null)
                        {
                            uuid = c33.Complemento.TimbreFiscalDigital.UUID;
                        }

                        if (c33.Emisor != null)
                        {
                            emisorNombre = c33.Emisor.Nombre;
                        }

                        if (c33.Impuestos != null)
                        {
                            ImpTras = c33.Impuestos.TotalImpuestosTrasladados;

                            if (c33.Impuestos.TotalImpuestosRetenidos != null)
                            {
                                ImpRet = c33.Impuestos.TotalImpuestosRetenidos;


                                if (c33.Conceptos != null && c33.Conceptos.Concepto != null)
                                {

                                    int linea = 1;

                                    foreach (var concepto in c33.Conceptos.Concepto)
                                    {

                                        if (concepto.Impuestos != null &&
                                            concepto.Impuestos.Retenciones != null &&
                                            concepto.Impuestos.Retenciones.Retencion != null)
                                        {


                                            foreach (var retencion in concepto.Impuestos.Retenciones.Retencion)
                                            {
                                                LstRet.Add(new CFDI_RETENCIONES
                                                {
                                                    LINEA = linea,
                                                    IMPUESTO = retencion.Impuesto,
                                                    IMPORTE = retencion.Importe,
                                                    TASAOCUOTA = retencion.TasaOCuota
                                                });
                                            }

                                            linea++;
                                        }


                                    }
                                }

                                if (LstRet.Count == 0)
                                {
                                    ImpRet = null;
                                }

                            }
                        }

                        var User2 = GetUser(emisorRFC);
                        AddXML(
                            data.Xml,
                            uuid,
                            emisorNombre,
                            emisorRFC,
                            c33.Total,
                            c33.SubTotal,
                            ImpTras,
                            c33.Version,
                            c33.Fecha.ToDateTime(),
                            data.Archivo,
                            c33.Serie,
                            c33.Folio,
                            data.Nombre_Archivo,
                            1,
                            c33.TipoDeComprobante,
                            receptorRFC,
                            User2,
                            c33.MetodoPago,
                            c33.FormaPago,
                            ImpRet,
                            LstRet,
                            zipForm);


                        var sql =
                            @"UPDATE dbo.CFDI_XML_PSI WITH (Rowlock) SET FECHA_COMPRA = @FECHA WHERE UUID = @UUID";
                        var FECHA = DateTime.Now;


                        try
                        {
                            

                            con.Execute(
                                sql,
                                new { UUID = uuid, FECHA },
                                tran);

                        }
                        catch
                        {
                        }
                    }

                    tran.Commit();
                }

                return JsonConvert.SerializeObject(new { code = "0", msg="Facturas agregadas correctamente." });
            }
            catch (Exception ex)
            {
                Log.Write(ex.Message + " " + Extensions.GetUserName() + " " + tmpXml.Str());
                return JsonConvert.SerializeObject(new { code = "-1", msg = ex.Message });
            }
        }


        private static CFDI_ACCOUNT GetUser(string emisorRfc)
        {

            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {
                try
                {
                    return con.Query<CFDI_ACCOUNT>(
                        "SELECT ID, USER_NAME, EMAIL, PEDIR_SERIE_FOLIO FROM CFDI_ACCOUNT WITH (NOLOCK) WHERE USER_NAME = @USER_NAME AND ROLE = 1",
                        new { USER_NAME = emisorRfc }).SingleOrDefault();

                    //var ls = con.Query<CFDI_XML>("select * from CFDI_XML", new { }, tran).ToList();

                    //***************************************************
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    if (con != null)
                    {
                        con.Close();
                    }
                }
            }
            return null;
        }

        private static void AddXML(string xml, string uuid, string razonSocial, string rfc, string total,
            string subtotal,
            string impuestos, string version, DateTime fechaFactura, byte[] file, string serie, string folio,
            string nombreArchivo, int estatus, string tipoComprobante, string receptorRFC, CFDI_ACCOUNT User2,
            string metodoPago, string formaPago, string totalImpuestosRetenidos,
            List<CFDI_RETENCIONES> cfdiRetenciones, List<Data2> zipForm)
        {
            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {
                IDbTransaction tran = null;

                try
                {
                    con.Open();
                    tran = con.BeginTransaction();

                    string sql = @"

                    DECLARE @XID INT;
                    SET @XID = (SELECT MAX(p.ID) FROM CFDI_XML_PSI p WITH (NOLOCK));
                    SET @XID = ISNULL( @XID , 0 ) + 1;

                    INSERT INTO CFDI_XML_PSI WITH (Rowlock) ( ID,  USER_ID,  UUID,  XML,  RAZON_SOCIAL_EMISOR,  RFC_EMISOR,  TOTAL,  SUBTOTAL,  IMPUESTOS,  VERSION,  FECHA_FACTURA,  ARCHIVO,  SERIE,  FOLIO,  NOMBRE_ARCHIVO,  ESTATUS,  TIPO_COMPROBANTE,  METODO_PAGO,  FORMA_PAGO, TOTAL_IMPUESTOS_RETENIDOS)
                                     VALUES (@XID, @USER_ID, @UUID, @XML, @RAZON_SOCIAL_EMISOR, @RFC_EMISOR, @TOTAL, @SUBTOTAL, @IMPUESTOS, @VERSION, @FECHA_FACTURA, @ARCHIVO, @SERIE, @FOLIO, @NOMBRE_ARCHIVO, @ESTATUS, @TIPO_COMPROBANTE, @METODO_PAGO, @FORMA_PAGO, @TOTAL_IMPUESTOS_RETENIDOS)

                    SELECT @XID;";

                    razonSocial = Regex.Replace(razonSocial, @"\s+", " ").Trim();
                    var id = con.ExecuteScalar<int>(
                        sql, new CFDI_XML(
                            User2.ID,
                            uuid,
                            xml,
                            razonSocial,
                            rfc,
                            total.Dbl(),
                            subtotal.Dbl(),
                            impuestos.Dbl(),
                            version,
                            fechaFactura,
                            file,
                            serie,
                            folio,
                            nombreArchivo,
                            estatus,
                            tipoComprobante,
                            metodoPago,
                            formaPago,
                            totalImpuestosRetenidos.Dbl()),
                        tran);

                    if (cfdiRetenciones.Count > 0)
                    {
                        cfdiRetenciones.ForEach(f =>
                        {
                            f.ID = id;
                        });


                        sql = @"


                        INSERT INTO dbo.CFDI_RETENCIONES WITH (Rowlock) (ID, LINEA, IMPUESTO, TASAOCUOTA, IMPORTE) VALUES
                        (@ID, @LINEA, @IMPUESTO, @TASAOCUOTA, @IMPORTE)";

                        con.Execute(sql, cfdiRetenciones, tran);
                    }



                    var zp = zipForm.FirstOrDefault(f => f.UUID.ToLower() == uuid.ToLower());




                    sql = @"

                        INSERT INTO CFDI_COMPRAS WITH (Rowlock) (ID_XML, SERIE, FOLIO) 
                                               VALUES(@ID_XML, @SERIE, @FOLIO);";

                    foreach (var sf in zp.SF)
                    {
                        con.Execute(
                            sql,
                            new
                            {
                                ID_XML=id,
                                sf.SERIE,
                                sf.FOLIO
                            },
                            transaction: tran);
                    }


                    tran.Commit();

                    //***************************************************
                }
                catch (Exception ex)
                {
                    if (tran != null)
                    {
                        tran.Rollback();
                    }

                    throw;
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


        public class SF
        {
            public string SERIE { get; set; }
            public string FOLIO { get; set; }
        }

        public class Data2
        {
            public string ARCHIVO { get; set; }
            public string PDF { get; set; }
            public string DETALLE { get; set; }
            public string RFC_EMISOR { get; set; }
            public string RFC_RECEPTOR { get; set; }
            public string SERIE { get; set; }
            public string FOLIO { get; set; }
            public string UUID { get; set; }
            public bool ERROR { get; set; }
            public string MENSAJE { get; set; }
            public string SERIE_FOLIO { get; set; }
            public List<SF> SF = new List<SF>();
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