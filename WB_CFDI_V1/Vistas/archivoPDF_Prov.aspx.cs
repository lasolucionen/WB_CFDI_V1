using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dapper;
using Newtonsoft.Json;
using WB_CFDI_V1.Properties;

namespace WB_CFDI_V1.Vistas
{
    public partial class archivoPDF_Prov : System.Web.UI.Page
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

            int? pdf_id = (int?)HttpContext.Current.Session["pdf_id"];
            CFDI_XML pdf = null;
            if (pdf_id != null)
            {
                //id.Value

                try
                {

                    using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                    {
                        string sql =
                            @"
                    SELECT ARCHIVO, NOMBRE_ARCHIVO
                    FROM dbo.CFDI_XML_PSI WITH (NOLOCK)
                    WHERE ID = @ID";

                        pdf = con.Query<CFDI_XML>(sql, new { ID = pdf_id.Value }).FirstOrDefault();
                    }


                    Response.Expires = 0;
                    Response.Buffer = true;
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment; filename=\"" + pdf.NOMBRE_ARCHIVO + ".pdf\"");
                    Response.ContentType = "application/pdf";
                    Response.BinaryWrite(pdf.ARCHIVO);
                    Response.End();

                }
                catch
                {

                }
                finally
                {
                    HttpContext.Current.Session["pdf_id"] = null;
                }


            }
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod(EnableSession = true)]
        public static string SetID(int id)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Filter = new GZipStream(current.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;

            //HttpContext.Current.Session["pdf_id"] = id;
            //return JsonConvert.SerializeObject(id.Str());

            //Log.Write("SetID:" + id);
            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {
                try
                {
                    var rs = con.Query<CFDI_XML>("SELECT USER_ID,ARCHIVO FROM CFDI_XML_PSI WITH (NOLOCK) WHERE ID = @ID", new { ID = id })
                        .FirstOrDefault();

                    if (rs != null && rs.ARCHIVO != null)
                    {
                        HttpContext.Current.Session["pdf_id"] = id;
                        return JsonConvert.SerializeObject(id.Str());
                    }
                    else
                    {
                        return null;
                    }

                    
                }
                catch (Exception ex)
                {
                    //Log.Write("Err SetID:" + ex.Message);
                    return null;
                }
            }
        }
    }
}