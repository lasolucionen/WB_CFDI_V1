using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Dapper;
using WB_CFDI_V1.Properties;

namespace WB_CFDI_V1.Vistas
{
    public partial class archivoXML : System.Web.UI.Page
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


            int? xml_id = (int?)HttpContext.Current.Session["xml_id"];
            CFDI_XML xml = null;
            if (xml_id != null)
            {
                //id.Value

                try
                {

                    using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                    {
                        string sql =
                            @"
                    SELECT XML, NOMBRE_ARCHIVO
                    FROM dbo.CFDI_XML_PSI WITH (NOLOCK)
                    WHERE ID = @ID";

                        xml = con.Query<CFDI_XML>(sql, new {ID = xml_id.Value}).FirstOrDefault();
                    }


                    Response.Clear();
                    Response.StatusCode = 200;
                    Response.ContentType = "application-download";
                    Response.AddHeader("content-disposition", "attachment; filename=\"" + xml.NOMBRE_ARCHIVO + ".xml\"");
                    Response.AddHeader("Content-Transfer-Encoding", "binary");
                    //response.AddHeader("Content-Length", _Buffer.Length.ToString());



                    Response.Write(Extensions.PrettyPrintXML(xml.XML));
                    Response.End();

                }
                catch
                {

                }
                finally
                {
                    HttpContext.Current.Session["xml_id"] = null;
                }


            }


            
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod(EnableSession = true)]
        public static void SetID(int id)
        {
            HttpContext.Current.Session["xml_id"] = id;
        }





        public static String PrettyPrint(String XML)
        {
            String Result = "";

            MemoryStream MS = new MemoryStream();
            XmlTextWriter W = new XmlTextWriter(MS, Encoding.Unicode);
            XmlDocument D = new XmlDocument();

            try
            {
                // Load the XmlDocument with the XML.
                D.LoadXml(XML);

                W.Formatting = Formatting.Indented;

                // Write the XML into a formatting XmlTextWriter
                D.WriteContentTo(W);
                W.Flush();
                MS.Flush();

                // Have to rewind the MemoryStream in order to read
                // its contents.
                MS.Position = 0;

                // Read MemoryStream contents into a StreamReader.
                StreamReader SR = new StreamReader(MS);

                // Extract the text from the StreamReader.
                String FormattedXML = SR.ReadToEnd();

                Result = FormattedXML;
            }
            catch (XmlException)
            {
            }

            MS.Close();
            W.Close();

            return Result;
        }



    }
}