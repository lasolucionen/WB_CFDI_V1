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
using WB_CFDI_V1.Properties;
using WB_CFDI_V1.WS_CFDI;
using XML2;
using XML3;



namespace WB_CFDI_V1.Vistas
{
    /// <summary>
    /// Descripción breve de fileUploader1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio Web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class fileUploader1 : System.Web.Services.WebService, System.Web.SessionState.IRequiresSessionState
    {
        private CFDI_ACCOUNT User = null;
        //Smtp client = null;
        //Htmlmailer client = null;

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

        public fileUploader1()
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
                        "SELECT ID, USER_NAME,ROLE, EMAIL FROM CFDI_ACCOUNT WHERE USER_NAME = @USER_NAME",
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



        public class Result
        {
            public List<Data> result;
            public int code { set; get; }
            public string file { set; get; }
            public string msg { set; get; }

            public Result()
            {
                code = -1;
            }
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

        public void Write(object ob)
        {
            string strResponse = JsonConvert.SerializeObject(
                ob,
                Formatting.None,
                new JsonSerializerSettings() {StringEscapeHandling = StringEscapeHandling.EscapeHtml});

            //string strResponse = JsonConvert.SerializeObject(ob);
            
            byte[] data = Encoding.UTF8.GetBytes(strResponse);
            string len = data.Length.ToString();

            //Context.Response.Clear();
            Context.Response.Charset = "UTF-8";
            Context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            Context.Response.ContentType = "application/json";
            Context.Response.AddHeader("content-length", len);
            Context.Response.Flush();
            Context.Response.Write(strResponse);
            //context.Response.BinaryWrite(data);


            /*
            Context.Response.AddHeader("content-length", ln);
            var serializer = new JsonSerializer();
            serializer.Serialize(context.Response.Output, ob);*/
            
        }

        public static bool IsNumeric(string str, NumberStyles style = NumberStyles.Number, CultureInfo culture = null)
        {
            if( culture == null )
            {
                culture = CultureInfo.InvariantCulture;
            }
            double num;
            return double.TryParse(str, style, culture, out num) && !string.IsNullOrWhiteSpace(str);
        }


        private bool del = true;
        string name = "";
        //private string strFile = "";
        //private string fileName = "";

        
        [WebMethod(EnableSession = true)]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Upload()
        {
            Result rs=new Result();

            //001 f1dbc6f7-9386-46af-9348-cbd1c3dcbc7c.pdf

            try
            {

                ArrayList arr = new ArrayList();

                if (!Array.Exists(Context.Request.Files.AllKeys, el => el == "myfile1"))
                {
                    arr.Add("<strong>Archivo XML</strong>");
                }

                if (!Array.Exists(Context.Request.Files.AllKeys, el => el == "myfile2"))
                {
                    //arr.Add("<strong>Archivo PDF</strong>");
                }


                if (arr.Count > 0)
                {
                    rs.msg = "<div style='text-align: left;'>Ingrese el valor del campo:<br />"
                             + string.Join(", ", arr.ToArray()) + "</div>";
                    Write(rs);
                    return;
                }



                var file1 = Context.Request.Files["myfile1"];
                var file2 = Context.Request.Files["myfile2"];

                string f1 = Path.GetFileNameWithoutExtension(file1.FileName);
                string f2 = "";

                if (file2 != null)
                {
                    f2 = Path.GetFileNameWithoutExtension(file2.FileName);
                }

                if (f2 != "" && f1.ToLower() != f2.ToLower())
                {
                    rs.msg = "<div style='text-align: left;min-width:250px;'>El nombre del archivo xml: <br />" +
                             "<strong>" + file1.FileName + "</strong><br />" +
                             "no coincide con el nombre del archivo pdf:<br />" +
                             "<strong>" + file2.FileName + "</strong></div>";

                    Write(rs);
                    return;
                }

                arr.Clear();

                string ext1 = Path.GetExtension(file1.FileName).ToLower();
                string ext2 = "";

                if (file2 != null)
                {
                    ext2 = Path.GetExtension(file2.FileName).ToLower();
                }

                if (ext1 != ".xml")
                {
                    rs.msg = "<div style='text-align: left;min-width:250px;'>Archivo XML no valido:<br /><strong>"
                             + file1.FileName + "</strong></div>";

                    Write(rs);
                    return;
                }

                if (ext2 != "" && ext2 != ".pdf")
                {
                    rs.msg = "<div style='text-align: left;min-width:250px;'>Archivo PDF no valido:<br /><strong>"
                             + file2.FileName + "</strong></div>";

                    Write(rs);
                    return;
                }

                List<Data> ls = new List<Data>();
                var data = "";
                if (ext1 == ".xml")
                {
                    data = file1.InputStream.GetStr();

                    XML3.Comprobante c33 = new XML3.Comprobante();

                    try
                    {
                        c33 = c33.DeserializeStr(data);
                    }
                    catch (Exception e)
                    {

                        throw new Exception(
                            "El archivo: <strong>" + f1 + ".xml</strong><br />no es valido.");

                    }

                    byte[] filePDF = null;

                    if (file2 != null)
                    {
                        filePDF = file2.InputStream.ToByteArray();
                    }


                    string emisorRFC    = null;
                    string receptorRFC  = null;
                    string uuid         = null;
                    string emisorNombre = null;
                    string ImpTras      = null;

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

                            throw new Exception(
                                "RFC RECEPTOR no valido: " + receptorRFC + "<br/><br/><strong>" + f1 +
                                ".xml</strong>");
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

                            throw new Exception(
                                "RFC EMISOR no valido: " + emisorRFC + "<br/><br/><strong>" + f1 +
                                ".xml</strong>");
                        }

                        var User2 = GetUser(emisorRFC);
                        if (User2 == null)
                        {

                            throw new Exception(
                                "El RFC: <strong>" + emisorRFC + "</strong> del archivo: <strong>" + f1 + ".xml</strong> no esta registrado.");
                        }

                        if (ExistUUID(uuid, receptorRFC,User2))
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

                            throw new Exception(
                                "No se puede importar, el documento ya existe.");
                        }


                        var xml = XMLVal(data);

                        if (xml.Estatus == "RFC Receptor no valido.")
                        {
                            throw new Exception(
                                    "No se puede importar.<br />EdiFact: El rfc receptor no está dado de alta.<br/><br/><strong>" + f1 +
                                    ".xml</strong>");
                        }

                        if (xml.Observacion.ToLower().Contains("error "))
                        {
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

                            throw new Exception(
                                "No se puede importar, " + xml.Observacion + "<br/><br/><strong>" + f1 +
                                ".xml</strong>");
                        }

                        if (xml.Codigo == "0")
                        {
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

                            throw new Exception(
                                "No se puede importar, " + xml.Mensaje + "<br/><br/><strong>" + f1 +
                                ".xml</strong>");
                        }

                        AddXML(
                            data,
                            uuid,
                            emisorNombre,
                            emisorRFC,
                            c33.Total,
                            c33.SubTotal,
                            ImpTras,
                            3.3,
                            c33.Fecha.ToDateTime(),
                            filePDF,
                            c33.Serie,
                            c33.Folio,
                            f1,
                            1,
                            c33.TipoDeComprobante,
                            receptorRFC,
                            User2);
                        
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

                    }



                    if (c33.Version == null)
                    {
                        rs.msg = "<div style='text-align: left;'>Archivo xml no valido:<br /><strong>"
                                 + file1.FileName + "</strong></div>";

                        Write(rs);
                        return;
                    }
                }

                //rs.result = ls;
                rs.code = 0;
                rs.msg = "<div style='text-align: left;margin:5px;'>Archivo importado correctamente:<br /><strong>"
                         + file1.FileName + "</strong></div>";

                Write(rs);
                return;

                //**************************************************
                string dirRaiz = Directory.CreateDirectory(Context.Server.MapPath("~/") + "Files/").FullName;
                string dirPath = Directory
                    .CreateDirectory(dirRaiz + name + DateTime.Now.ToString(".dd.hh.mm.ss.ffff") + "/").FullName;
            }
            catch (Exception ex)
            {
                rs.msg = "<div style='text-align: left;margin:5px;'>" + ex.Message + "</div>";
                Write(rs);
            }
            finally
            {
                try
                {
                    //File.WriteAllText(Path.Combine(Settings.Default.Tigger_Email_Path, "email.ok"), "");
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
                        "SELECT ID, USER_NAME, EMAIL FROM CFDI_ACCOUNT WHERE USER_NAME = @USER_NAME AND ROLE = 1",
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

        private void SaveMail(string emisorRFC, string receptorRFC, string serie, string folio, string uuid, int tipo, string observacion, string mensaje)
        {
            if (User.EMAIL == null || User.EMAIL.Trim() == "")
            {
                return;
            }


            using (IDbConnection con = new SqlConnection(Extensions.GetBD()))
            {
                IDbTransaction tran = null;

                try
                {
                    con.Open();
                    tran = con.BeginTransaction();

                    string sql = @"INSERT INTO CFDI_EMAIL_MSG (UUID, EMAIL_TO, SERIE, FOLIO, RFC_EMISOR, RFC_RECEPTOR, RFC_USER, TIPO, OBSERVACION, MENSAJE) VALUES (@UUID, @EMAIL_TO, @SERIE, @FOLIO, @RFC_EMISOR, @RFC_RECEPTOR, @RFC_USER, @TIPO, @OBSERVACION, @MENSAJE)";
                    var rs = con.Execute(
                        sql,
                        new CFDI_EMAIL_MSG(
                            uuid,
                            User.EMAIL,
                            serie,
                            folio,
                            emisorRFC,
                            receptorRFC,
                            User.USER_NAME,
                            tipo,
                            observacion,
                            mensaje),
                        tran);
                    tran.Commit();

                    //***************************************************
                }
                catch (Exception ex)
                {
                    if (tran != null)
                    {
                        tran.Rollback();
                    }
                }
                finally
                {
                    if (con != null)
                    {
                        con.Close();
                    }
                }
            }


            /*if (User.EMAIL != null)
            {
                SendMail(User.EMAIL, msg, subject);
            }*/
        }



        [WebMethod(EnableSession = true)]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Upload2()
        {
            Result2 rs2=new Result2();

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
                                        DETALLE ="El archivo no es valido.",
                                        ERROR = true
                                    });
                                continue;
                            }


                            string emisorRFC    = null;
                            string receptorRFC  = null;
                            string uuid         = null;
                            string emisorNombre = null;
                            string ImpTras      = null;

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
                                            UUID         = uuid,
                                            SERIE        = c33.Serie,
                                            FOLIO        = c33.Folio,
                                            RFC_RECEPTOR = receptorRFC,
                                            RFC_EMISOR   = emisorRFC,
                                            ERROR        = true
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

                                if (ExistUUID(uuid, receptorRFC,User2))
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
                                            ARCHIVO      = data.Key + ".xml",
                                            DETALLE      = "El documento ya existe.",
                                            UUID         = uuid,
                                            SERIE        = c33.Serie,
                                            FOLIO        = c33.Folio,
                                            RFC_RECEPTOR = receptorRFC,
                                            RFC_EMISOR   = emisorRFC,
                                            ERROR        = true
                                        });
                                    continue;
                                }

                                var xml = XMLVal(data.Value.XML);

                                if (xml.Estatus == "RFC Receptor no valido.")
                                {
                                    rs1.Add(
                                            new Data2
                                            {
                                                ARCHIVO      = data.Key + ".xml",
                                                DETALLE      = "No se puede importar.",
                                                UUID         = uuid,
                                                SERIE        = c33.Serie,
                                                FOLIO        = c33.Folio,
                                                RFC_RECEPTOR = receptorRFC,
                                                RFC_EMISOR   = emisorRFC,
                                                MENSAJE      = "EdiFact: El rfc receptor no está dado de alta.",
                                                ERROR        = true
                                            });
                                    continue;
                                }

                                if (xml.Observacion.ToLower().Contains("error "))
                                {
                                    rs1.Add(
                                        new Data2
                                        {
                                            ARCHIVO = data.Key + ".xml",
                                            DETALLE = "No se puede importar, " + xml.Observacion,
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

                                if (xml.Codigo == "0")
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
                                    3.3,
                                    c33.Fecha.ToDateTime(),
                                    data.Value.PDF,
                                    c33.Serie,
                                    c33.Folio,
                                    data.Key,
                                    1,
                                    c33.TipoDeComprobante,
                                    receptorRFC,
                                    User2);

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

                //**************************************************
                string dirRaiz = Directory.CreateDirectory(Context.Server.MapPath("~/") + "Files/").FullName;
                string dirPath = Directory
                    .CreateDirectory(dirRaiz + name + DateTime.Now.ToString(".dd.hh.mm.ss.ffff") + "/").FullName;
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

        //private Htmlmailer mail = null;
        private bool SaveMail(string email, string msg, string subject)
        {
            
            try
            {
                //MailMessage mail=new MailMessage();

                /*SmtpClient smp = new SmtpClient(Settings.Default.Correo_Servidor, Settings.Default.Correo_Port);
                smp.Credentials = new NetworkCredential(Settings.Default.Correo_Usuario, Settings.Default.Correo_Password);
                smp.Send(new System.Net.Mail.MailMessage
                {
                    IsBodyHtml = true,
                    From = new MailAddress(Settings.Default.Correo_Usuario),
                    To = {email},
                    Subject = subject,
                    Body = msg

                });*/


                /*client.ResetMessage();
                client.Message.From.Email = Settings.Default.Correo_Usuario;
                client.Message.Subject = subject;
                client.Message.To = new EmailAddressCollection(email);
                client.Message.BodyHtmlText = msg;
                var snd = client.Send();*/
                
                //System.Threading.Thread.Sleep(Settings.Default.Correo_Sleep);

                /*MailMessage mail = new MailMessage();
                mail.To          = email;
                mail.From        = Settings.Default.Correo_Usuario;
                mail.Subject     = subject;
                mail.BodyHtml    = msg;*/

                //client.Connect(Settings.Default.Correo_IMAP, 587);
                //client.Login(Settings.Default.Correo_Usuario, Settings.Default.Correo_Password);

                //client.Send(mail);
                
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
/*                if (client.IsConnected)
                {
                    //client.Disconnect();
                }*/
            }
        }



        private bool ExistUUID(string uuid, string receptorRfc, CFDI_ACCOUNT User2)
        {
            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {
                try
                {
                    string sql = "SELECT UUID FROM CFDI_XML_PSI WHERE UUID = @UUID AND USER_ID = @USER_ID";


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

        private void AddXML(string xml, string uuid, string razonSocial, string rfc, string total, string subtotal, string impuestos, double version, DateTime fechaFactura, byte[] file, string serie, string folio, string nombreArchivo, int estatus, string tipoComprobante, string receptorRFC, CFDI_ACCOUNT User2)
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
                    SET @XID = (SELECT MAX(p.ID) FROM CFDI_XML_PSI p);
                    SET @XID = ISNULL( @XID , 0 ) + 1;

                    INSERT INTO CFDI_XML_PSI (ID, USER_ID, UUID, XML, RAZON_SOCIAL_EMISOR, RFC_EMISOR, TOTAL, SUBTOTAL, IMPUESTOS, VERSION, FECHA_FACTURA, ARCHIVO, SERIE, FOLIO, NOMBRE_ARCHIVO, ESTATUS, TIPO_COMPROBANTE)
                                     VALUES (@XID, @USER_ID, @UUID, @XML, @RAZON_SOCIAL_EMISOR, @RFC_EMISOR, @TOTAL, @SUBTOTAL, @IMPUESTOS, @VERSION, @FECHA_FACTURA, @ARCHIVO, @SERIE, @FOLIO, @NOMBRE_ARCHIVO, @ESTATUS, @TIPO_COMPROBANTE)";

                    razonSocial = Regex.Replace(razonSocial, @"\s+", " ").Trim();
                    var rs = con.Execute(
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
                            tipoComprobante),
                        tran);

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

        private static Data XMLVal(string b64)
        {
            Validador cfdi = new Validador();
            var rs1 = cfdi.ValidaAll(b64.ToBase64(Encoding.UTF8)); //b64.ToB64()

            var estatus = "";
            var mensaje = "";
            if (rs1.estatus != null && rs1.estatus.Contains("Su suscripcion no registrado"))
            {
                estatus = "RFC Receptor no valido.";
            }
            else
            {
                estatus = rs1.estatus;
                mensaje = rs1.mensaje;
            }

            return new Data
            {
                Codigo      = rs1.codigo,
                Estatus     = estatus,
                NPAC        = rs1.npac,
                Mensaje     = mensaje,
                Observacion = rs1.observacion != null
                    ? rs1.observacion.SSubstring(14, rs1.observacion.Length).Trim()
                    : null
            };
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

    public class Data
    {
        //public string Archivo{get ; set; }
        public string Codigo { get; set; }
        public string Estatus { get; set; }
        public string Mensaje { get; set; }
        public string NPAC { get; set; }
        public string Observacion { get; set; }
    }

    public class Data1
    {
        public string XML { get; set; }
        public byte[] PDF { get; set; }
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
    }
}
