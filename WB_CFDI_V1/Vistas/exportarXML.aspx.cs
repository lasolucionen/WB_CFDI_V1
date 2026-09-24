using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dapper;

namespace WB_CFDI_V1.Vistas
{
    public partial class exportarXML : System.Web.UI.Page
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


            Req req = (Req) HttpContext.Current.Session["req"];

            if (req != null)
            {
                using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                {

                    try
                    {
                        string sql = @"
                        SELECT xml.ID FROM dbo.CFDI_XML_PSI xml WITH (NOLOCK)
                        WHERE 
                        FECHA_FACTURA BETWEEN CAST((@fecha1 + ' 00:00:00') AS DATETIME) AND CAST((@fecha2 + ' 23:59:59') AS DATETIME)";

                        var ids = con.Query<CFDI_XML>(sql, req).ToList();


                        sql = @"
                        SELECT
                          xml.XML,
                          xml.NOMBRE_ARCHIVO,
                          xml.FECHA_FACTURA
                        FROM dbo.CFDI_XML_PSI xml WITH (NOLOCK)
                        WHERE xml.ID = @ID";


                        MemoryStream st = new MemoryStream();
                        using (ZipStorer zip = ZipStorer.Create(st, ""))
                        {
                            foreach (var it in ids)
                            {
                                var rs = con.Query<CFDI_XML>(sql, new {ID = it.ID}).FirstOrDefault();

                                if (rs != null)
                                {
                                    zip.AddStream(
                                            ZipStorer.Compression.Deflate,
                                            rs.NOMBRE_ARCHIVO + ".xml",
                                            rs.XML.ToStream(),
                                            rs.FECHA_FACTURA.Value,
                                            "");

                                }

                            }

                        }

                        Response.Expires = 0;
                        Response.Buffer  = true;
                        Response.ClearContent();
                        Response.AddHeader(
                                "content-disposition",
                                "attachment; filename=\"" + Extensions.GetAlias() + "_XML [" + req.FECHA1.ToString("dd MMMM yyyy") +
                                "] AL [" + req.FECHA2.ToString("dd MMMM yyyy") + "].zip\"");
                        Response.ContentType = "application/x-compressed";
                        Response.BinaryWrite(st.ToArray());
                        Response.End();

                    }
                    catch (Exception ex)
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

            HttpContext.Current.Session["req"] = null;
        }


        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod(EnableSession = true)]
        public static void SetFecha(Req req)
        {
            HttpContext.Current.Session["req"] = req;
        }

        public class Req
        {
            public DateTime FECHA1     { get; set; }
            public DateTime FECHA2     { get; set; }
        }
    }
}