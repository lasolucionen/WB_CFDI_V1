using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using Dapper;
using WB_CFDI_V1.Properties;
using XML3;
using Comprobante = XML3.Comprobante;

namespace WB_CFDI_V1.Vistas
{
    public partial class reportes : System.Web.UI.Page
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

            Req req = (Req)HttpContext.Current.Session["req"];

            HttpContext.Current.Session["req"] = null;

            if (req != null)
            {

                using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                {
                    try
                    {
                        
                        string sql = "";
                        /*
                            SELECT 
                              DISTINCT
                              xml.UUID,
                              xml.CONTRA_RECIBO_ID,
                              --xml.XML,
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
                              xml.TIPO_COMPROBANTE
                            FROM dbo.CFDI_XML_PSI xml
                            WHERE
                              xml.FECHA_RECEPCION BETWEEN CAST(@fecha1 + ' 00:00:00' AS DATETIME) AND CAST(@fecha2 + ' 23:59:59' AS DATETIME)
                            AND ESTATUS=2
                         */

                        //BD1 ICGFRONTDOS

                        var emp = Extensions.cEmpresa();

                        if( req.rep == 1 )
                        {
                            sql = @"

                            SELECT
                              DISTINCT
                              xml.UUID,
                              xml.CONTRA_RECIBO_ID,
                              --xml.XML,
                              xml.RAZON_SOCIAL_EMISOR,
                              xml.RFC_EMISOR,
                                --xml.TOTAL,
                              case 
                                  when xml.TIPO_COMPROBANTE IS NULL then xml.TOTAL
                                  when LOWER(xml.TIPO_COMPROBANTE) = 'e' then xml.TOTAL*-1
                                  when LOWER(xml.TIPO_COMPROBANTE) = 'egreso' then xml.TOTAL*-1
                                  ELSE xml.TOTAL
                              end TOTAL,
                                --xml.SUBTOTAL,
                              case 
                                  when xml.TIPO_COMPROBANTE IS NULL then xml.SUBTOTAL
                                  when LOWER(xml.TIPO_COMPROBANTE) = 'e' then xml.SUBTOTAL*-1
                                  when LOWER(xml.TIPO_COMPROBANTE) = 'egreso' then xml.SUBTOTAL*-1
                                  ELSE xml.SUBTOTAL
                              end SUBTOTAL,
                                --xml.IMPUESTOS,
                              case 
                                  when xml.TIPO_COMPROBANTE IS NULL then xml.IMPUESTOS
                                  when LOWER(xml.TIPO_COMPROBANTE) = 'e' then xml.IMPUESTOS*-1
                                  when LOWER(xml.TIPO_COMPROBANTE) = 'egreso' then xml.IMPUESTOS*-1
                                  ELSE xml.IMPUESTOS
                              end IMPUESTOS,
                              xml.FECHA_RECEPCION,
                              xml.FECHA_FACTURA,
                              xml.SERIE,
                              xml.FOLIO,
                              xml.TIENDA,
                              xml.ESTATUS,
                              xml.TIPO_COMPROBANTE,
                              CAST(it.FECHA_PROCESO AS DATE) FECHA_PROCESO
                            FROM dbo.CFDI_XML_PSI xml WITH (NOLOCK)
                            INNER JOIN " + emp.ICG_BD + @".dbo.IT_RELFACTURAS_COMPRAS it WITH (NOLOCK)
                              ON xml.UUID = it.FOLIOFISCAL
                            WHERE it.FECHA_PROCESO BETWEEN CAST(@fecha1 + ' 00:00:00' AS DATETIME) AND CAST(@fecha2 + ' 23:59:59' AS DATETIME)
                              AND (xml.ESTATUS=2 OR xml.ESTATUS=6)";
                        }

                        if (req.rep == 2)
                        {
                            /*
                              DISTINCT
                              xml.UUID,
                              xml.CONTRA_RECIBO_ID,
                              --xml.XML,
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
                              xml.TIPO_COMPROBANTE,
                              rel.NUMSERIEFAC,
                              rel.NUMFAC
                             */
                            

                            sql = @"
                            SELECT 
                                DISTINCT
                                    xml.UUID,
                                    xml.CONTRA_RECIBO_ID,
                                    xml.RAZON_SOCIAL_EMISOR,
                                    xml.RFC_EMISOR,
                                    --xml.TOTAL,
                                  case 
                                      when xml.TIPO_COMPROBANTE IS NULL then xml.TOTAL
                                      when LOWER(xml.TIPO_COMPROBANTE) = 'e' then xml.TOTAL*-1
                                      when LOWER(xml.TIPO_COMPROBANTE) = 'egreso' then xml.TOTAL*-1
                                      ELSE xml.TOTAL
                                  end TOTAL,
                                    --xml.SUBTOTAL,
                                  case 
                                      when xml.TIPO_COMPROBANTE IS NULL then xml.SUBTOTAL
                                      when LOWER(xml.TIPO_COMPROBANTE) = 'e' then xml.SUBTOTAL*-1
                                      when LOWER(xml.TIPO_COMPROBANTE) = 'egreso' then xml.SUBTOTAL*-1
                                      ELSE xml.SUBTOTAL
                                  end SUBTOTAL,
                                    --xml.IMPUESTOS,
                                  case 
                                      when xml.TIPO_COMPROBANTE IS NULL then xml.IMPUESTOS
                                      when LOWER(xml.TIPO_COMPROBANTE) = 'e' then xml.IMPUESTOS*-1
                                      when LOWER(xml.TIPO_COMPROBANTE) = 'egreso' then xml.IMPUESTOS*-1
                                      ELSE xml.IMPUESTOS
                                  end IMPUESTOS,
                                    xml.FECHA_RECEPCION,
                                    xml.FECHA_FACTURA,
                                    xml.SERIE,
                                    xml.FOLIO,
                                    xml.TIENDA,
                                    xml.ESTATUS,
                                    xml.TIPO_COMPROBANTE,
                                    it.FECHA_PROCESO,
                                    it.NUMSERIEFAC,
                                    it.NUMFAC
                            FROM dbo.CFDI_XML_PSI xml WITH (NOLOCK)
                            --INNER JOIN " + emp.ICG_BD + @".dbo.IT_RELFACTURAS_COMPRAS it WITH (NOLOCK) ON xml.UUID = it.FOLIOFISCAL
                            INNER JOIN
                                     (SELECT DISTINCT 
                                        CAST(FECHA_PROCESO AS DATE) FECHA_PROCESO,FOLIOFISCAL,NUMSERIEFAC,NUMFAC
                                        FROM " + emp.ICG_BD + @".dbo.IT_RELFACTURAS_COMPRAS WITH (NOLOCK)) it ON xml.UUID = it.FOLIOFISCAL

                            WHERE
                              it.FECHA_PROCESO BETWEEN CAST(@fecha1 + ' 00:00:00' AS DATETIME) AND CAST(@fecha2 + ' 23:59:59' AS DATETIME)
                            AND (xml.ESTATUS=2 OR xml.ESTATUS=6)
                            ORDER BY it.NUMSERIEFAC";
                        }

                        List<CFDI_XML> ls = new List<CFDI_XML>();

                        if (req.rep == 3)
                        {

                            sql = @"
                            SELECT
                              xml.UUID,
                              xml.TIPO_COMPROBANTE,
                              xml.RAZON_SOCIAL_EMISOR,
                              xml.RFC_EMISOR,
                                  case 
                                      when xml.TIPO_COMPROBANTE IS NULL then xml.TOTAL
                                      when LOWER(xml.TIPO_COMPROBANTE) = 'e' then xml.TOTAL*-1
                                      when LOWER(xml.TIPO_COMPROBANTE) = 'egreso' then xml.TOTAL*-1
                                      ELSE xml.TOTAL
                                  end TOTAL,
                                  case 
                                      when xml.TIPO_COMPROBANTE IS NULL then xml.SUBTOTAL
                                      when LOWER(xml.TIPO_COMPROBANTE) = 'e' then xml.SUBTOTAL*-1
                                      when LOWER(xml.TIPO_COMPROBANTE) = 'egreso' then xml.SUBTOTAL*-1
                                      ELSE xml.SUBTOTAL
                                  end SUBTOTAL
                            FROM dbo.CFDI_XML_PSI xml WITH (NOLOCK)
                            
                            INNER JOIN
                                     (SELECT DISTINCT 
                                        CAST(FECHA_PROCESO AS DATE) FECHA_PROCESO,FOLIOFISCAL,NUMSERIEFAC
                                        FROM " + emp.ICG_BD + @".dbo.IT_RELFACTURAS_COMPRAS WITH (NOLOCK)) it ON xml.UUID = it.FOLIOFISCAL

                            WHERE
                              it.FECHA_PROCESO BETWEEN CAST(@fecha1 + ' 00:00:00' AS DATETIME) AND CAST(@fecha2 + ' 23:59:59' AS DATETIME)
                            AND (xml.ESTATUS=2 OR xml.ESTATUS=6)
                            ORDER BY it.NUMSERIEFAC";


                            Dictionary<sKey,CFDI_XML> sxml=new Dictionary<sKey, CFDI_XML>();
                            
                            var xmls = con.Query<CFDI_XML>(sql, new { req.fecha1, req.fecha2 }).ToList();
                            foreach (var rs in xmls)
                            {
                                 try
                                 {
                                      Comprobante c33 = new Comprobante();


                                      var xml = con.QueryFirst<string>("SELECT XML FROM dbo.CFDI_XML_PSI WITH (NOLOCK) WHERE UUID = @UUID", new { rs.UUID });

                                      c33 = c33.DeserializeStr(xml);

                                      if (c33.Version != null)
                                      {

                                           if (c33.Impuestos != null)
                                           {
                                                rs.IVA = c33.Impuestos.Traslados.Traslado.Where(w => w.Impuesto == "002")
                                                            .Sum(s => s.Importe.Dbl());
                                           }
                                           else
                                           {
                                                rs.IVA = 0;
                                           }

                                           if (c33.Impuestos != null)
                                           {

                                                rs.IEPS = c33.Impuestos.Traslados.Traslado.Where(w => w.Impuesto == "003")
                                                             .Sum(s => s.Importe.Dbl());
                                           }
                                           else
                                           {
                                                rs.IEPS = 0;
                                           }

                                           if (rs.TIPO_COMPROBANTE != null && (rs.TIPO_COMPROBANTE.ToLower() == "e" || rs.TIPO_COMPROBANTE.ToLower() == "egreso"))
                                           {
                                                rs.IVA  = rs.IVA  * -1;
                                                rs.IEPS = rs.IEPS * -1;
                                           }

                                      }

                                 }
                                 catch (Exception ex)
                                 {

                                 }

                                 if (rs != null)
                                 {

                                      string TIPO_COMPROBANTE = "";

                                      if (rs.TIPO_COMPROBANTE != null)
                                      {
                                          TIPO_COMPROBANTE = rs.TIPO_COMPROBANTE.ToLower();
                                      }


                                      if (!sxml.ContainsKey(new sKey(rs.RFC_EMISOR, TIPO_COMPROBANTE)))
                                      {
                                          sxml.Add(new sKey(rs.RFC_EMISOR, TIPO_COMPROBANTE),rs);
                                      }
                                      else
                                      {
                                           var hkey=sxml[new sKey(rs.RFC_EMISOR, TIPO_COMPROBANTE)];

                                           hkey.TOTAL    += rs.TOTAL;
                                           hkey.SUBTOTAL += rs.SUBTOTAL;
                                           hkey.IEPS     += rs.IEPS;
                                           hkey.IVA      += rs.IVA;

                                      }


                                      //ls.Add(rs);
                                 }
                            }

                            ls = sxml.OrderByDescending(o => o.Key.TIPO_COMPROBANTE).Select(p => p.Value).ToList(); //.ToDictionary(p => p.Key,p=>p.Value);
                        }


                        if (req.rep == 4)
                        {
                            sql = @"

                            SELECT
                              xml.UUID,
                              xml.CONTRA_RECIBO_ID,
                              --xml.XML,
                              xml.RAZON_SOCIAL_EMISOR,
                              xml.RFC_EMISOR,
                                --xml.TOTAL,
                              case 
                                  when xml.TIPO_COMPROBANTE IS NULL then xml.TOTAL
                                  when LOWER(xml.TIPO_COMPROBANTE) = 'e' then xml.TOTAL*-1
                                  when LOWER(xml.TIPO_COMPROBANTE) = 'egreso' then xml.TOTAL*-1
                                  ELSE xml.TOTAL
                              end TOTAL,
                                --xml.SUBTOTAL,
                              case 
                                  when xml.TIPO_COMPROBANTE IS NULL then xml.SUBTOTAL
                                  when LOWER(xml.TIPO_COMPROBANTE) = 'e' then xml.SUBTOTAL*-1
                                  when LOWER(xml.TIPO_COMPROBANTE) = 'egreso' then xml.SUBTOTAL*-1
                                  ELSE xml.SUBTOTAL
                              end SUBTOTAL,
                                --xml.IMPUESTOS,
                              case 
                                  when xml.TIPO_COMPROBANTE IS NULL then xml.IMPUESTOS
                                  when LOWER(xml.TIPO_COMPROBANTE) = 'e' then xml.IMPUESTOS*-1
                                  when LOWER(xml.TIPO_COMPROBANTE) = 'egreso' then xml.IMPUESTOS*-1
                                  ELSE xml.IMPUESTOS
                              end IMPUESTOS,
                              xml.FECHA_RECEPCION,
                              xml.FECHA_FACTURA,
                              xml.SERIE,
                              xml.FOLIO,
                              xml.TIENDA,
                              xml.NO_COMPRA,
                              xml.FECHA_COMPRA,
                              xml.ESTATUS,
                              xml.TIPO_COMPROBANTE
                            FROM dbo.CFDI_XML_PSI xml WITH (NOLOCK)
                            WHERE xml.FECHA_RECEPCION BETWEEN CAST(@fecha1 + ' 00:00:00' AS DATETIME) AND CAST(@fecha2 + ' 23:59:59' AS DATETIME)
                            AND xml.FECHA_COMPRA IS NOT NULL
                            ORDER BY xml.RFC_EMISOR
                              ";
                        }


                        if (req.rep == 5)
                        {
                            sql = @"

                            SELECT
                              xml.UUID,
                              xml.CONTRA_RECIBO_ID,
                              --xml.XML,
                              xml.RAZON_SOCIAL_EMISOR,
                              xml.RFC_EMISOR,
                                --xml.TOTAL,
                              case 
                                  when xml.TIPO_COMPROBANTE IS NULL then xml.TOTAL
                                  when LOWER(xml.TIPO_COMPROBANTE) = 'e' then xml.TOTAL*-1
                                  when LOWER(xml.TIPO_COMPROBANTE) = 'egreso' then xml.TOTAL*-1
                                  ELSE xml.TOTAL
                              end TOTAL,
                                --xml.SUBTOTAL,
                              case 
                                  when xml.TIPO_COMPROBANTE IS NULL then xml.SUBTOTAL
                                  when LOWER(xml.TIPO_COMPROBANTE) = 'e' then xml.SUBTOTAL*-1
                                  when LOWER(xml.TIPO_COMPROBANTE) = 'egreso' then xml.SUBTOTAL*-1
                                  ELSE xml.SUBTOTAL
                              end SUBTOTAL,
                                --xml.IMPUESTOS,
                              case 
                                  when xml.TIPO_COMPROBANTE IS NULL then xml.IMPUESTOS
                                  when LOWER(xml.TIPO_COMPROBANTE) = 'e' then xml.IMPUESTOS*-1
                                  when LOWER(xml.TIPO_COMPROBANTE) = 'egreso' then xml.IMPUESTOS*-1
                                  ELSE xml.IMPUESTOS
                              end IMPUESTOS,
                              xml.FECHA_RECEPCION,
                              xml.FECHA_FACTURA,
                              xml.SERIE,
                              xml.FOLIO,
                              xml.TIENDA,
                              xml.NO_COMPRA,
                              xml.FECHA_COMPRA,
                              xml.ESTATUS,
                              xml.TIPO_COMPROBANTE
                            FROM dbo.CFDI_XML_PSI xml WITH (NOLOCK)
                            WHERE xml.FECHA_RECEPCION BETWEEN CAST(@fecha1 + ' 00:00:00' AS DATETIME) AND CAST(@fecha2 + ' 23:59:59' AS DATETIME)
                            AND xml.ESTATUS = 1
                            ORDER BY xml.RFC_EMISOR
                              ";


                            ls = con.Query<CFDI_XML>(sql, new { req.fecha1, req.fecha2 }).ToList();
                        }


                        if (req.rep == 6)
                        {
                            sql = @"

                            SELECT
                              xml.UUID,
                              xml.CONTRA_RECIBO_ID,
                              --xml.XML,
                              xml.RAZON_SOCIAL_EMISOR,
                              xml.RFC_EMISOR,
                                --xml.TOTAL,
                              case 
                                  when xml.TIPO_COMPROBANTE IS NULL then xml.TOTAL
                                  when LOWER(xml.TIPO_COMPROBANTE) = 'e' then xml.TOTAL*-1
                                  when LOWER(xml.TIPO_COMPROBANTE) = 'egreso' then xml.TOTAL*-1
                                  ELSE xml.TOTAL
                              end TOTAL,
                                --xml.SUBTOTAL,
                              case 
                                  when xml.TIPO_COMPROBANTE IS NULL then xml.SUBTOTAL
                                  when LOWER(xml.TIPO_COMPROBANTE) = 'e' then xml.SUBTOTAL*-1
                                  when LOWER(xml.TIPO_COMPROBANTE) = 'egreso' then xml.SUBTOTAL*-1
                                  ELSE xml.SUBTOTAL
                              end SUBTOTAL,
                                --xml.IMPUESTOS,
                              case 
                                  when xml.TIPO_COMPROBANTE IS NULL then xml.IMPUESTOS
                                  when LOWER(xml.TIPO_COMPROBANTE) = 'e' then xml.IMPUESTOS*-1
                                  when LOWER(xml.TIPO_COMPROBANTE) = 'egreso' then xml.IMPUESTOS*-1
                                  ELSE xml.IMPUESTOS
                              end IMPUESTOS,
                              xml.FECHA_RECEPCION,
                              xml.FECHA_FACTURA,
                              xml.SERIE,
                              xml.FOLIO,
                              xml.TIENDA,
                              xml.NO_COMPRA,
                              xml.FECHA_COMPRA,
                              xml.ESTATUS,
                              xml.TIPO_COMPROBANTE
                            FROM dbo.CFDI_XML_PSI xml WITH (NOLOCK)
                            WHERE xml.FECHA_RECEPCION BETWEEN CAST(@fecha1 + ' 00:00:00' AS DATETIME) AND CAST(@fecha2 + ' 23:59:59' AS DATETIME)
                            AND xml.ESTATUS = 3
                            ORDER BY xml.RFC_EMISOR
                              ";


                            ls = con.Query<CFDI_XML>(sql, new { req.fecha1, req.fecha2 }).ToList();
                        }



                        if (req.rep != 3 && req.rep != 5 && req.rep != 6)
                        {
                            var xmls = con.Query<CFDI_XML>(sql, new { req.fecha1, req.fecha2 }).ToList();

                            foreach (var rs in xmls)
                            {
                                try
                                {
                                    Comprobante c33 = new Comprobante();


                                    var xml = con.QueryFirst<string>("SELECT XML FROM dbo.CFDI_XML_PSI WITH (NOLOCK) WHERE UUID = @UUID", new { rs.UUID });

                                    c33 = c33.DeserializeStr(xml);

                                    if (c33.Version != null)
                                    {

                                        rs.IVA = c33.Impuestos.Traslados.Traslado.Where(w => w.Impuesto == "002")
                                                    .Sum(s => s.Importe.Dbl());

                                        rs.IEPS = c33.Impuestos.Traslados.Traslado.Where(w => w.Impuesto == "003")
                                                     .Sum(s => s.Importe.Dbl());

                                        if (rs.TIPO_COMPROBANTE != null && (rs.TIPO_COMPROBANTE.ToLower() == "e" || rs.TIPO_COMPROBANTE.ToLower() == "egreso"))
                                        {
                                            rs.IVA = rs.IVA * -1;
                                            rs.IEPS = rs.IEPS * -1;
                                        }

                                        /*
                                    if (rs.TIPO_COMPROBANTE != null
                                     && (rs.TIPO_COMPROBANTE.ToLower() == "e"
                                      || rs.TIPO_COMPROBANTE.ToLower() == "egreso"))
                                    {
                                        rs.IVA = rs.IVA * -1;
                                        rs.IEPS = rs.IEPS * -1;
                                    }
                                    */

                                    }

                                }
                                catch (Exception ex)
                                {

                                }



                                if (rs != null)
                                {

                                    ls.Add(rs);
                                }
                            }
                        }


                        /*
                            if (rs.TIPO_COMPROBANTE != null
                             && (rs.TIPO_COMPROBANTE.ToLower() == "e" || rs.TIPO_COMPROBANTE.ToLower() == "egreso"))
                            {
                                if (rs.TOTAL != null) rs.TOTAL = rs.TOTAL * -1;
                                if (rs.SUBTOTAL != null) rs.SUBTOTAL = rs.SUBTOTAL * -1;
                                if (rs.IMPUESTOS != null) rs.IMPUESTOS = rs.IMPUESTOS * -1;
                            }
                        */





                        var wb = new XLWorkbook();
                        var ws = wb.Worksheets.Add("Reporte");

                        int row = 2;
                        int column = 2;

                        for (int j = 1; j < column; j++)
                        {
                            ws.Column(j).Width = 2.5;
                        }

                        if (ls.Count != 0)
                        {
                            //ls.Add(new CFDI_XML());


                            DataTable lst = null;

                            if( req.rep == 1 )
                            {
                                lst = ls.Select(
                                        s => new
                                             {
                                                 s.UUID,
                                                 s.RAZON_SOCIAL_EMISOR,
                                                 s.RFC_EMISOR,
                                                 s.CONTRA_RECIBO_ID,
                                                 s.SERIE,
                                                 s.FOLIO,
                                                 s.TOTAL,
                                                 s.SUBTOTAL,
                                                 s.IMPUESTOS,
                                                 s.IVA,
                                                 s.IEPS,
                                                 s.TIENDA,
                                                 s.TIPO_COMPROBANTE,
                                                 s.ESTATUS,
                                                 FECHA_FACTURA =
                                                         s.FECHA_FACTURA.Value.ToString("dd/MM/yyyy hh:mm:ss tt"),
                                                 FECHA_RECEPCION =
                                                         s.FECHA_RECEPCION.Value.ToString("dd/MM/yyyy"),
                                                 FECHA_PROCESO =
                                                         s.FECHA_PROCESO.Value.ToString("dd/MM/yyyy")
                                             }).ToDataTable(true);
                            }

                            if (req.rep == 2)
                            {
                                lst = ls.Select(
                                        s => new
                                             {
                                                 s.UUID,
                                                 s.RAZON_SOCIAL_EMISOR,
                                                 s.RFC_EMISOR,
                                                 s.NUMSERIEFAC,
                                                 s.NUMFAC,
                                                 s.CONTRA_RECIBO_ID,
                                                 s.SERIE,
                                                 s.FOLIO,
                                                 s.TOTAL,
                                                 s.SUBTOTAL,
                                                 s.IMPUESTOS,
                                                 s.IVA,
                                                 s.IEPS,
                                                 s.TIENDA,
                                                 s.TIPO_COMPROBANTE,
                                                 s.ESTATUS,
                                                 FECHA_FACTURA =
                                                         s.FECHA_FACTURA.Value.ToString("dd/MM/yyyy hh:mm:ss tt"),
                                                 FECHA_RECEPCION =
                                                         s.FECHA_RECEPCION.Value.ToString("dd/MM/yyyy"),
                                                 FECHA_PROCESO =
                                                         s.FECHA_PROCESO.Value.ToString("dd/MM/yyyy")
                                             }).ToDataTable(true);
                            }

                            if (req.rep == 3)
                            {

                                IXLCell cellx = ws.Cell(row, column);
                                cellx.Value = "FECHA: " + req.fecha1.ToShortDateString() + " - " + req.fecha2.ToShortDateString();

                                row++;

                                lst = ls.Select(
                                        s => new
                                             {
                                                 s.RFC_EMISOR,
                                                 s.RAZON_SOCIAL_EMISOR,
                                                 s.SUBTOTAL,
                                                 s.IEPS,
                                                 s.IVA,
                                                 s.TOTAL,
                                             }).ToDataTable(true);
                            }


                            if (req.rep == 4)
                            {
                                 lst = ls.Select(
                                           s => new
                                                {
                                                     s.UUID,
                                                     s.RAZON_SOCIAL_EMISOR,
                                                     s.RFC_EMISOR,
                                                     s.CONTRA_RECIBO_ID,
                                                     s.SERIE,
                                                     s.FOLIO,
                                                     s.TOTAL,
                                                     s.SUBTOTAL,
                                                     s.IMPUESTOS,
                                                     s.IVA,
                                                     s.IEPS,
                                                     s.TIENDA,
                                                     s.NO_COMPRA,
                                                     FECHA_COMPRA = s.FECHA_COMPRA.Value.ToString("dd/MM/yyyy"),
                                                     s.TIPO_COMPROBANTE,
                                                     s.ESTATUS,
                                                     FECHA_FACTURA =
                                                               s.FECHA_FACTURA.Value.ToString("dd/MM/yyyy hh:mm:ss tt"),
                                                     FECHA_RECEPCION =
                                                               s.FECHA_RECEPCION.Value.ToString("dd/MM/yyyy")
                                                }).ToDataTable(true);
                            }

                            if (req.rep == 5)
                            {
                                lst = ls.Select(
                                    s => new
                                    {
                                        s.UUID,
                                        s.RAZON_SOCIAL_EMISOR,
                                        s.RFC_EMISOR,
                                        s.CONTRA_RECIBO_ID,
                                        s.SERIE,
                                        s.FOLIO,
                                        s.TOTAL,
                                        s.SUBTOTAL,
                                        s.IMPUESTOS,
                                        s.TIENDA,
                                        s.TIPO_COMPROBANTE,
                                        s.ESTATUS,
                                        FECHA_FACTURA =
                                            s.FECHA_FACTURA.Value.ToString("dd/MM/yyyy hh:mm:ss tt"),
                                        FECHA_RECEPCION =
                                            s.FECHA_RECEPCION.Value.ToString("dd/MM/yyyy")
                                    }).ToDataTable(false);

                                lst.Columns["CONTRA_RECIBO_ID"].Caption = "CONTRA_RECIBO";
                            }

                            if (req.rep == 6)
                            {
                                lst = ls.Select(
                                    s => new
                                    {
                                        s.UUID,
                                        s.RAZON_SOCIAL_EMISOR,
                                        s.RFC_EMISOR,
                                        s.CONTRA_RECIBO_ID,
                                        s.SERIE,
                                        s.FOLIO,
                                        s.TOTAL,
                                        s.SUBTOTAL,
                                        s.IMPUESTOS,
                                        s.TIENDA,
                                        s.TIPO_COMPROBANTE,
                                        s.ESTATUS,
                                        FECHA_FACTURA =
                                            s.FECHA_FACTURA.Value.ToString("dd/MM/yyyy hh:mm:ss tt"),
                                        FECHA_RECEPCION =
                                            s.FECHA_RECEPCION.Value.ToString("dd/MM/yyyy")
                                    }).ToDataTable(false);

                                lst.Columns["CONTRA_RECIBO_ID"].Caption = "CONTRA_RECIBO";
                            }

                            IXLCell cell = ws.Cell(row, column);
                            IXLRange ran = null;

                            var tbl = cell.InsertTable(lst, true);
                            tbl.Theme = XLTableTheme.TableStyleMedium2;

                            ran = ws.Range(
                                    tbl.FirstCell().CellBelow(),
                                    tbl.LastCell());



                            if (ran != null)
                            {
                                if (req.rep == 1)
                                {
                                    ran.Column(tbl.Field("TOTAL").Index + 1).Style.NumberFormat.Format = "$#,##0.00";
                                    ran.Column(tbl.Field("SUBTOTAL").Index + 1).Style.NumberFormat.Format = "$#,##0.00";
                                    ran.Column(tbl.Field("IMPUESTOS").Index + 1).Style.NumberFormat.Format = "$#,##0.00";
                                    ran.Column(tbl.Field("IVA").Index + 1).Style.NumberFormat.Format = "$#,##0.00";
                                    ran.Column(tbl.Field("IEPS").Index + 1).Style.NumberFormat.Format = "$#,##0.00";

                                    ran.Column(tbl.Field("CONTRA_RECIBO").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ran.Column(tbl.Field("SERIE").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ran.Column(tbl.Field("FOLIO").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ran.Column(tbl.Field("TIENDA").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ran.Column(tbl.Field("TIPO_COMPROBANTE").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                                    ran.Column(tbl.Field("FECHA_FACTURA").Index + 1).DataType = XLCellValues.DateTime;
                                    ran.Column(tbl.Field("FECHA_RECEPCION").Index + 1).DataType = XLCellValues.DateTime;
                                    ran.Column(tbl.Field("FECHA_PROCESO").Index + 1).DataType = XLCellValues.DateTime;

                                    ran.Column(tbl.Field("FECHA_FACTURA").Index + 1).Style.DateFormat.Format = "dd/mm/yyyy hh:mm AM/PM";
                                    ran.Column(tbl.Field("FECHA_RECEPCION").Index + 1).Style.DateFormat.Format = "dd/mm/yyyy";
                                    ran.Column(tbl.Field("FECHA_PROCESO").Index + 1).Style.DateFormat.Format = "dd/mm/yyyy";
                                }

                                if (req.rep == 2)
                                {
                                    ran.Column(tbl.Field("NUMSERIEFAC").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ran.Column(tbl.Field("NUMFAC").Index + 1).Style.Alignment.Horizontal      = XLAlignmentHorizontalValues.Center;
                                }

                                if (req.rep == 3)
                                {
                                    ran.Column(tbl.Field("TOTAL").Index + 1).Style.NumberFormat.Format = "$#,##0.00";
                                    ran.Column(tbl.Field("IVA").Index + 1).Style.NumberFormat.Format = "$#,##0.00";
                                    ran.Column(tbl.Field("IEPS").Index + 1).Style.NumberFormat.Format = "$#,##0.00";
                                    ran.Column(tbl.Field("SUBTOTAL").Index + 1).Style.NumberFormat.Format = "$#,##0.00";
                                }

                                if (req.rep == 4)
                                {
                                    ran.Column(tbl.Field("TOTAL").Index + 1).Style.NumberFormat.Format = "$#,##0.00";
                                    ran.Column(tbl.Field("SUBTOTAL").Index + 1).Style.NumberFormat.Format = "$#,##0.00";
                                    ran.Column(tbl.Field("IMPUESTOS").Index + 1).Style.NumberFormat.Format = "$#,##0.00";
                                    ran.Column(tbl.Field("IVA").Index + 1).Style.NumberFormat.Format = "$#,##0.00";
                                    ran.Column(tbl.Field("IEPS").Index + 1).Style.NumberFormat.Format = "$#,##0.00";

                                    ran.Column(tbl.Field("CONTRA_RECIBO").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ran.Column(tbl.Field("SERIE").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ran.Column(tbl.Field("FOLIO").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ran.Column(tbl.Field("SERIE_COMPRA").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ran.Column(tbl.Field("FOLIO_COMPRA").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ran.Column(tbl.Field("TIPO_COMPROBANTE").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                                    ran.Column(tbl.Field("FECHA_FACTURA").Index + 1).DataType = XLCellValues.DateTime;
                                    ran.Column(tbl.Field("FECHA_RECEPCION").Index + 1).DataType = XLCellValues.DateTime;
                                    ran.Column(tbl.Field("FECHA_COMPRA").Index + 1).DataType = XLCellValues.DateTime;

                                    ran.Column(tbl.Field("FECHA_FACTURA").Index + 1).Style.DateFormat.Format = "dd/mm/yyyy hh:mm AM/PM";
                                    ran.Column(tbl.Field("FECHA_RECEPCION").Index + 1).Style.DateFormat.Format = "dd/mm/yyyy";
                                    ran.Column(tbl.Field("FECHA_COMPRA").Index + 1).Style.DateFormat.Format = "dd/mm/yyyy";
                                }

                                if (req.rep == 5)
                                {
                                    ran.Column(tbl.Field("TOTAL").Index + 1).Style.NumberFormat.Format = "$#,##0.00";
                                    ran.Column(tbl.Field("SUBTOTAL").Index + 1).Style.NumberFormat.Format = "$#,##0.00";
                                    ran.Column(tbl.Field("IMPUESTOS").Index + 1).Style.NumberFormat.Format = "$#,##0.00";

                                    ran.Column(tbl.Field("CONTRA_RECIBO").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ran.Column(tbl.Field("SERIE").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ran.Column(tbl.Field("FOLIO").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ran.Column(tbl.Field("TIPO_COMPROBANTE").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ran.Column(tbl.Field("TIENDA").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                                    ran.Column(tbl.Field("FECHA_FACTURA").Index + 1).DataType = XLCellValues.DateTime;
                                    ran.Column(tbl.Field("FECHA_RECEPCION").Index + 1).DataType = XLCellValues.DateTime;

                                    ran.Column(tbl.Field("FECHA_FACTURA").Index + 1).Style.DateFormat.Format = "dd/mm/yyyy hh:mm AM/PM";
                                    ran.Column(tbl.Field("FECHA_RECEPCION").Index + 1).Style.DateFormat.Format = "dd/mm/yyyy";

                                }

                                if (req.rep == 6)
                                {
                                    ran.Column(tbl.Field("TOTAL").Index + 1).Style.NumberFormat.Format = "$#,##0.00";
                                    ran.Column(tbl.Field("SUBTOTAL").Index + 1).Style.NumberFormat.Format = "$#,##0.00";
                                    ran.Column(tbl.Field("IMPUESTOS").Index + 1).Style.NumberFormat.Format = "$#,##0.00";

                                    ran.Column(tbl.Field("CONTRA_RECIBO").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ran.Column(tbl.Field("SERIE").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ran.Column(tbl.Field("FOLIO").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ran.Column(tbl.Field("TIPO_COMPROBANTE").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ran.Column(tbl.Field("TIENDA").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                                    ran.Column(tbl.Field("FECHA_FACTURA").Index + 1).DataType = XLCellValues.DateTime;
                                    ran.Column(tbl.Field("FECHA_RECEPCION").Index + 1).DataType = XLCellValues.DateTime;

                                    ran.Column(tbl.Field("FECHA_FACTURA").Index + 1).Style.DateFormat.Format = "dd/mm/yyyy hh:mm AM/PM";
                                    ran.Column(tbl.Field("FECHA_RECEPCION").Index + 1).Style.DateFormat.Format = "dd/mm/yyyy";

                                }

                                ran.AdjustToContents();
                            }

                        }

                        Response.Clear();
                        Response.ContentType =
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                        if( req.rep == 1 )
                        {
                            Response.AddHeader(
                                    "content-disposition",
                                    "attachment;filename=\"CFDI_XML_PSI_1.xlsx\"");
                        }

                        if (req.rep == 2)
                        {
                            Response.AddHeader(
                                    "content-disposition",
                                    "attachment;filename=\"CFDI_XML_PSI_2.xlsx\"");
                        }

                        if (req.rep == 3)
                        {
                            Response.AddHeader(
                                    "content-disposition",
                                    "attachment;filename=\"CFDI_XML_PSI_3.xlsx\"");
                        }

                        if (req.rep == 4)
                        {
                            Response.AddHeader(
                                "content-disposition",
                                "attachment;filename=\"CFDI_XML_PSI_4.xlsx\"");
                        }

                        if (req.rep == 5)
                        {
                            Response.AddHeader(
                                "content-disposition",
                                "attachment;filename=\"CFDI_XML_PSI_5.xlsx\"");
                        }

                        if (req.rep == 6)
                        {
                            Response.AddHeader(
                                "content-disposition",
                                "attachment;filename=\"CFDI_XML_PSI_6.xlsx\"");
                        }

                        using (var memoryStream = new MemoryStream())
                        {

                            wb.SaveAs(memoryStream);
                            memoryStream.WriteTo(Response.OutputStream);
                        }

                        Response.End();


                    }
                    catch(Exception ex)
                    {

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


        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static void SetID(Req req)
        {
            HttpContext.Current.Session["req"] = req;
        }
    }

    public struct sKey
    {
        public string RFC_EMISOR { get; set; }
        public string TIPO_COMPROBANTE { get; set; }

        public sKey(string rfcEmisor, string tipoComprobante) : this()
        {
             RFC_EMISOR = rfcEmisor;
             TIPO_COMPROBANTE = tipoComprobante;
        }
    }

    public class Req
    {
        public DateTime fecha1 { get; set; }
        public DateTime fecha2 { get; set; }
        public int      rep { get; set; }

    }
}