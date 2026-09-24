using System;
using System.Activities.Expressions;
using System.Collections;
using System.Collections.Generic;
using System.Data;
//using System.Data.CData.Excel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using Dapper;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.IO.Compression;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Services;
using System.Xml;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Drawing;
using WB_CFDI_V1.Properties;
using WB_CFDI_V1.WS_CFDI;
using XML3;
using Formatting = Newtonsoft.Json.Formatting;
using Path = System.IO.Path;

namespace WB_CFDI_V1.Vistas
{
    /// <summary>
    /// Descripción breve de fileUploader3
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class fileUploader3 : System.Web.Services.WebService
    {
        private CFDI_ACCOUNT User = null;
        private bool del = true;
        string name = "";


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                /*if (client.IsConnected)
                {
                    client.Disconnect();
                }*/
            }
            base.Dispose(disposing);
        }

        public fileUploader3()
        {
            /*            MailBee.Global.LicenseKey = "MN110-1C20DF387EA9952F661A118648FA-468F";

                        client = new Smtp();

                        client.SmtpServers.Add(
                            Settings.Default.Correo_Servidor,
                            Settings.Default.Correo_Port,
                            0,
                            4000,
                            true,
                            AuthenticationMethods.Auto,
                            Settings.Default.Correo_Usuario,
                            Settings.Default.Correo_Password,
                            true,
                            "",
                            ExtendedSmtpOptions.Default);*/


            /*try
            {
                client.Connect();
                client.Hello();
                client.Login();
            }
            catch (Exception e)
            {
                
            }*/

            /*client.SmtpServers.Add(
                "smtp.uservers.net",
                587,
                0,
                5000,
                false,
                AuthenticationMethods.Auto,
                "lorenzo@lasolucionen.com",
                "99mayo16",
                true,
                "",
                ExtendedSmtpOptions.Default);*/

            //client.Message.From.Email = "lorenzo@lasolucionen.com";


            /*client = new nsoftware.IPWorks.Htmlmailer();
            client.MailServer = Settings.Default.Correo_IMAP;
            client.User = Settings.Default.Correo_Usuario;
            client.Password = Settings.Default.Correo_Password;
            client.MailPort = 26;*/

            if (Context.User != null && Context.User.Identity.IsAuthenticated)
            {
                FormsIdentity id = (FormsIdentity)Context.User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;
                //role = ticket.UserData;
                name = ticket.Name;
            }

            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {
                try
                {
                    User = con.Query<CFDI_ACCOUNT>(
                        "SELECT ID, USER_NAME,ROLE, EMAIL FROM CFDI_ACCOUNT WITH (NOLOCK) WHERE USER_NAME = @USER_NAME",
                        new { USER_NAME = name }).SingleOrDefault();

                    //var ls = con.Query<CFDI_XML>("select * from CFDI_XML", new { }, tran).ToList();

                    //***************************************************
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





        [WebMethod(EnableSession = true)]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Upload2()
        {
            Result2 rs2 = new Result2();

            //001 f1dbc6f7-9386-46af-9348-cbd1c3dcbc7c.pdf

            try
            {

                ArrayList arr = new ArrayList();

                if (!Array.Exists(Context.Request.Files.AllKeys, el => el == "myfile1"))
                {
                    arr.Add("<strong>Archivo ZIP</strong>");
                }

                if (!Array.Exists(Context.Request.Files.AllKeys, el => el == "myfile2"))
                {
                    //arr.Add("<strong>Archivo PDF</strong>");
                }

                if (arr.Count > 0)
                {
                    rs2.msg = "<div style='text-align: left;'>Ingrese el valor del campo:<br />"
                              + string.Join(", ", arr.ToArray()) + "</div>";
                    Write(rs2);
                    return;
                }

                var file1 = Context.Request.Files["myfile1"];
                string f1 = Path.GetFileNameWithoutExtension(file1.FileName);


                arr.Clear();

                string ext1 = Path.GetExtension(file1.FileName).ToLower();

                if (ext1 != ".zip")
                {
                    rs2.msg = "<div style='text-align: left;min-width:250px;'>Archivo ZIP no valido:<br /><strong>"
                              + file1.FileName + "</strong></div>";

                    Write(rs2);
                    return;
                }


                List<Data2> rs1 = new List<Data2>();

                Dictionary<string, Data1> raw = new Dictionary<string, Data1>();
                if (ext1 == ".zip")
                {

                    ZipStorer zip = ZipStorer.Open(file1.InputStream, FileAccess.Read);
                    foreach (ZipStorer.ZipFileEntry entry in zip.ReadCentralDir())
                    {
                        var znam = Path.GetFileNameWithoutExtension(entry.FilenameInZip);
                        var zext = Path.GetExtension(entry.FilenameInZip).ToLower();

                        if (zext == ".xml")
                        {
                            if (!raw.ContainsKey(znam))
                            {
                                raw.Add(znam, new Data1());
                            }

                            MemoryStream zmem = new MemoryStream();
                            zip.ExtractFile(entry, zmem);
                            raw[znam].XML = zmem.GetStr();
                        }

                        if (zext == ".pdf")
                        {
                            if (!raw.ContainsKey(znam))
                            {
                                raw.Add(znam, new Data1());
                            }

                            MemoryStream zmem = new MemoryStream();
                            zip.ExtractFile(entry, zmem);
                            raw[znam].PDF = zmem.ToByteArray();
                        }
                    }


                    foreach (var data in raw)
                    {
                        if (data.Value.XML != null && data.Value.XML.Trim() != "")
                        {
                            XML3.Comprobante c33 = new XML3.Comprobante();

                            try
                            {
                                c33 = c33.DeserializeStr(data.Value.XML);
                            }
                            catch (Exception e)
                            {

                                rs1.Add(
                                    new Data2
                                    {
                                        ARCHIVO = data.Key + ".xml",
                                        DETALLE = "El archivo no es valido.",
                                        ERROR = true
                                    });
                                continue;
                            }


                            string emisorRFC = null;
                            string receptorRFC = null;
                            string uuid = null;
                            string emisorNombre = null;
                            string ImpTras = null;
                            string ImpRet = null;
                            List<CFDI_RETENCIONES> LstRet = new List<CFDI_RETENCIONES>();

                            if (c33.Version != null)
                            {
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

                                //if (receptorRFC == null || receptorRFC.ToUpper() != Settings.Default.ReceptorRFC_PSI.ToUpper() && receptorRFC.ToUpper() != Settings.Default.ReceptorRFC_PSO.ToUpper())
                                if (receptorRFC == null || receptorRFC.ToUpper() != Extensions.GetReceptorRFC().ToUpper())
                                {

                                    /*
                                    SaveMail(
                                        emisorRFC,
                                        receptorRFC,
                                        c33.Serie,
                                        c33.Folio,
                                        uuid,
                                        1,
                                        null,
                                        null);*/

                                    rs1.Add(
                                        new Data2
                                        {
                                            ARCHIVO = data.Key + ".xml",
                                            DETALLE =
                                                "RFC RECEPTOR no valido: " +
                                                receptorRFC,
                                            UUID = c33.Complemento.TimbreFiscalDigital.UUID,
                                            SERIE = c33.Serie,
                                            FOLIO = c33.Folio,
                                            RFC_RECEPTOR = c33.Receptor.Rfc,
                                            RFC_EMISOR = c33.Emisor.Rfc,
                                            ERROR = true
                                        });
                                    continue;
                                }

                                if (User.USER_NAME.ToUpper() != emisorRFC.ToUpper() && User.ROLE == 1)
                                {
                                    /*
                                    SaveMail(
                                        emisorRFC,
                                        receptorRFC,
                                        c33.Serie,
                                        c33.Folio,
                                        uuid,
                                        2,
                                        null,
                                        null);*/

                                    rs1.Add(
                                        new Data2
                                        {
                                            ARCHIVO = data.Key + ".xml",
                                            DETALLE =
                                                "El RFC EMISOR del documento no coincide con la cuenta de usuario: " +
                                                User.USER_NAME,
                                            UUID = uuid,
                                            SERIE = c33.Serie,
                                            FOLIO = c33.Folio,
                                            RFC_RECEPTOR = receptorRFC,
                                            RFC_EMISOR = emisorRFC,
                                            ERROR = true
                                        });
                                    continue;
                                }

                                var User2 = GetUser(emisorRFC);
                                if (User2 == null)
                                {

                                    rs1.Add(
                                        new Data2
                                        {
                                            ARCHIVO = data.Key + ".xml",
                                            DETALLE =
                                                "El RFC: <strong>" + emisorRFC + "</strong> no esta registrado.",
                                            UUID = "",
                                            SERIE = "",
                                            FOLIO = "",
                                            RFC_RECEPTOR = "",
                                            RFC_EMISOR = "",
                                            ERROR = true
                                        });
                                    continue;

                                }

                                if (ExistUUID(uuid, receptorRFC, User2))
                                {
                                    /*
                                    SaveMail(
                                        emisorRFC,
                                        receptorRFC,
                                        c33.Serie,
                                        c33.Folio,
                                        uuid,
                                        3,
                                        null,
                                        null);*/

                                    rs1.Add(
                                        new Data2
                                        {
                                            ARCHIVO = data.Key + ".xml",
                                            DETALLE = "El documento ya existe.",
                                            UUID = uuid,
                                            SERIE = c33.Serie,
                                            FOLIO = c33.Folio,
                                            RFC_RECEPTOR = receptorRFC,
                                            RFC_EMISOR = emisorRFC,
                                            ERROR = true
                                        });
                                    continue;
                                }





                                var chk = Pag2.GetChkFacDup();
                                if (chk != null && !chk.Value)
                                {
                                    var obj = Extensions.GetFact(emisorRFC, c33.Serie, c33.Folio, c33.TipoDeComprobante);

                                    if (obj.Count > 0)
                                    {

                                        /*
                                        throw new Exception(
                                            "No se puede importar la factura: <strong>" + f1 +
                                            ".xml</strong> <br /><br />por que ya existe una factura con estos datos: <br />" +
                                            "<strong>RFC:</strong> " + obj[0].RFC_EMISOR + "<br />" +
                                            "<strong>Serie:</strong> " + obj[0].SERIE + "<br />" +
                                            "<strong>Folio:</strong> " + obj[0].FOLIO + "<br />" +
                                            "<strong>Total:</strong> " + obj[0].TOTAL + "<br />" +
                                            "<strong>Fecha Factura:</strong> " + obj[0].FECHA_FACTURA
                                        );
                                        */

                                        rs1.Add(
                                            new Data2
                                            {
                                                ARCHIVO = data.Key + ".xml",
                                                DETALLE = "No se puede importar.",
                                                UUID = uuid,
                                                SERIE = c33.Serie,
                                                FOLIO = c33.Folio,
                                                RFC_RECEPTOR = receptorRFC,
                                                RFC_EMISOR = emisorRFC,
                                                MENSAJE = "ya existe una factura con estos datos: <br />" +
                                                "<strong>RFC:</strong> " + obj[0].RFC_EMISOR + " <strong>Total:</strong> " + obj[0].TOTAL + "<br />" +
                                                "<strong>Serie:</strong> " + obj[0].SERIE + " <strong>Folio:</strong> " + obj[0].FOLIO + "<br />" +
                                                "<strong>Fecha Factura:</strong> " + obj[0].FECHA_FACTURA
                                                ,
                                                ERROR = true
                                            });


                                        continue;
                                    }
                                }


                                var xml = XMLVal(data.Value.XML);
                                if (xml.Codigo != "201")
                                {
                                    rs1.Add(
                                        new Data2
                                        {
                                            ARCHIVO = data.Key + ".xml",
                                            DETALLE = "No se puede importar.",
                                            UUID = uuid,
                                            SERIE = c33.Serie,
                                            FOLIO = c33.Folio,
                                            RFC_RECEPTOR = receptorRFC,
                                            RFC_EMISOR = emisorRFC,
                                            MENSAJE = xml.Mensaje,
                                            ERROR = true
                                        });

                                    /*
                                    SaveMail(
                                        emisorRFC,
                                        receptorRFC,
                                        c33.Serie,
                                        c33.Folio,
                                        uuid,
                                        5,
                                        xml.Observacion,
                                        xml.Mensaje);*/
                                    continue;
                                }


                                //Xml Valido!
                                //ok


                                AddXML(
                                    data.Value.XML,
                                    uuid,
                                    emisorNombre,
                                    emisorRFC,
                                    c33.Total,
                                    c33.SubTotal,
                                    ImpTras,
                                    c33.Version,
                                    c33.Fecha.ToDateTime(),
                                    data.Value.PDF,
                                    c33.Serie,
                                    c33.Folio,
                                    data.Key,
                                    1,
                                    c33.TipoDeComprobante,
                                    receptorRFC,
                                    User2,
                                    c33.MetodoPago,
                                    c33.FormaPago,
                                    ImpRet,
                                    LstRet);

                                /*
                                SaveMail(
                                    emisorRFC,
                                    receptorRFC,
                                    c33.Serie,
                                    c33.Folio,
                                    uuid,
                                    4,
                                    null,
                                    null);*/

                                string zpdf = "Agregado";
                                if (data.Value.PDF == null)
                                {
                                    zpdf = "No Encontrado";
                                }

                                rs1.Add(
                                    new Data2
                                    {
                                        ARCHIVO = data.Key + ".xml",
                                        DETALLE = "Agregado correctamente.",
                                        PDF = zpdf,
                                        UUID = uuid,
                                        SERIE = c33.Serie,
                                        FOLIO = c33.Folio,
                                        RFC_RECEPTOR = receptorRFC,
                                        RFC_EMISOR = emisorRFC,
                                        MENSAJE = xml.Mensaje
                                    });

                            }

                        }
                    }

                }

                rs2.result = rs1;
                rs2.code = 0;
                /*rs.msg = "<div style='text-align: left;'>Archivo importado correctamente:<br /><strong>"
                         + file1.FileName + "</strong></div>";*/

                Write(rs2);
                return;


            }
            catch (Exception ex)
            {
                rs2.msg = "<div style='width:300px;text-align: left;'>" + ex.Message + "</div>";
                Write(rs2);
            }
            finally
            {
                try
                {
                    //File.WriteAllText(Path.Combine(Settings.Default.Tigger_Email_Path,"email.ok"),"");
                }
                catch (Exception e)
                {

                }
            }
        }

        private CFDI_ACCOUNT GetUser(string emisorRfc)
        {

            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {
                try
                {
                    return con.Query<CFDI_ACCOUNT>(
                        "SELECT ID, USER_NAME, EMAIL FROM CFDI_ACCOUNT WITH (NOLOCK) WHERE USER_NAME = @USER_NAME AND ROLE = 1",
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


        private bool ExistUUID(string uuid, string receptorRfc, CFDI_ACCOUNT User2)
        {
            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {
                try
                {
                    string sql = "SELECT UUID FROM CFDI_XML_PSI WITH (NOLOCK) WHERE UUID = @UUID AND USER_ID = @USER_ID";


                    string e_uuid = con.Query<string>(sql,
                        new { UUID = uuid, USER_ID = User2.ID }).SingleOrDefault();

                    if (e_uuid != null)
                    {
                        return true;
                    }

                    return false;
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
        }

        private void AddXML(string xml, string uuid, string razonSocial, string rfc, string total, string subtotal,
            string impuestos, string version, DateTime fechaFactura, byte[] file, string serie, string folio,
            string nombreArchivo, int estatus, string tipoComprobante, string receptorRFC, CFDI_ACCOUNT User2,
            string metodoPago, string formaPago, string totalImpuestosRetenidos,
            List<CFDI_RETENCIONES> cfdiRetenciones)
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


        public void Write(object ob)
        {
            string strResponse = JsonConvert.SerializeObject(
                ob,
                Formatting.None,
                new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.EscapeHtml });

            //string strResponse = JsonConvert.SerializeObject(ob);

            byte[] data = Encoding.UTF8.GetBytes(strResponse);
            string len  = data.Length.ToString();

            //Context.Response.Clear();
            Context.Response.Charset         = "UTF-8";
            Context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            Context.Response.ContentType     = "application/json";
            Context.Response.AddHeader("content-length", len);
            Context.Response.Flush();
            Context.Response.Write(strResponse);
            //context.Response.BinaryWrite(data);


            /*
            Context.Response.AddHeader("content-length", ln);
            var serializer = new JsonSerializer();
            serializer.Serialize(context.Response.Output, ob);*/

        }

        private static Data XMLVal(string xml)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
            CFDI2.validarComprobantesCfdi cfd = new CFDI2.validarComprobantesCfdi();

            var user = "";
            var pass = "";


            XML3.Comprobante c33 = new XML3.Comprobante();
            c33 = c33.DeserializeStr(xml);

            if (c33.Receptor.Rfc.ToUpper() == "CCO171114EW7")
            {
                user = "reccco171114ew7";
                pass = "%U$Y0fez";

            }
            else if (c33.Receptor.Rfc.ToUpper() == "PSI0401308Y2")
            {
                user = "recpsi0401308y2";
                pass = "kW#DLAC1";
            }

            var rs = cfd.validarCfdi(user, pass, xml, "", 0);


            var estatus = "";
            var mensaje = "";


            return new Data
            {
                Codigo = rs.codigo,
                Estatus = "",
                NPAC = "",
                Mensaje = rs.mensaje,
                Observacion = ""
            };
        }



        public class Result2
        {
            public List<Data2> result;
            public int code { set; get; }
            public string file { set; get; }
            public string msg { set; get; }

            public Result2()
            {
                code = -1;
            }
        }

        private void clearFolder(string FolderName)
        {
            DirectoryInfo dir = new DirectoryInfo(FolderName);

            foreach (FileInfo fi in dir.GetFiles())
            {
                fi.Delete();
            }

            foreach (DirectoryInfo di in dir.GetDirectories())
            {
                clearFolder(di.FullName);
                di.Delete();
            }
        }
    }



}

