using ClosedXML.Excel;
using Dapper;
using FastMember;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.WebPages;
//using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using WB_CFDI_V1.Properties;
using XML3;
using Formatting = Newtonsoft.Json.Formatting;

namespace WB_CFDI_V1
{

    public static class StringExtensions
    {

        public static string Capitalize(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return "";
            }

            char[] a = s.ToLower().ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        public static string ToBase64(this string text)
        {
            return ToBase64(text, Encoding.UTF8);
        }

        public static string ToBase64(this string text, Encoding encoding)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            byte[] textAsBytes = encoding.GetBytes(text);
            return Convert.ToBase64String(textAsBytes);
        }

        public static bool TryParseBase64(this string text, out string decodedText)
        {
            return TryParseBase64(text, Encoding.UTF8, out decodedText);
        }

        public static bool TryParseBase64(this string text, Encoding encoding, out string decodedText)
        {
            if (string.IsNullOrEmpty(text))
            {
                decodedText = text;
                return false;
            }

            try
            {
                byte[] textAsBytes = Convert.FromBase64String(text);
                decodedText = encoding.GetString(textAsBytes);
                return true;
            }
            catch (Exception)
            {
                decodedText = null;
                return false;
            }
        }
    }


    public static class Extensions
    {

        public static byte[] Object2Bytes(this object obj)
        {
            string json             = JsonConvert.SerializeObject(obj);
            byte[] serializedResult = Encoding.UTF8.GetBytes(json);
            return serializedResult;
        }
        public static T Bytes2Object<T>(this byte[] buff)
        {
            string json = System.Text.Encoding.UTF8.GetString(buff);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static object Bytes2Object(this byte[] buff)
        {
            string json = System.Text.Encoding.UTF8.GetString(buff);
            return JsonConvert.DeserializeObject<object>(json);
        }



        static public string Beautify(this XmlDocument doc)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent             = true,
                OmitXmlDeclaration = true
            };

            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                doc.Save(writer);
            }

            return sb.ToString();
        }

        public static void Send(this MvcHtmlString value, HtmlHelper helper, string type)
        {

            helper.Resource(o => new HelperResult(w => w.Write(value)), type);
        }

        public static void Send(this string value, HtmlHelper helper, string type)
        {

            helper.Resource(o => new HelperResult(w => w.Write(value)), type);
        }

        public static void Send(this StringBuilder value, HtmlHelper helper, string type)
        {

            helper.Resource(o => new HelperResult(w => w.Write(value)), type);
        }

        public static string Quotes(this string str)
        {
            return str.Quotes(true);
        }

        public static string Quotes(this string str, bool quotes)
        {

            if (quotes)
            {
                return "'" + str + "'";
            }
            else
            {
                return str;
            }
        }


        public static string Json(this object obj, bool NullValue = true)
        {

            JsonSerializer serializer = null;

            if (NullValue)
            {
                serializer = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore };
            }
            else
            {
                serializer = new JsonSerializer();
            }


            var stringWriter = new StringWriter();

            using (var writer = new JsonTextWriter(stringWriter))
            {
                writer.QuoteName = false;
                //writer.QuoteChar = '\'';

                writer.Formatting = Formatting.Indented;
                serializer.Serialize(writer, obj);

            }


            return stringWriter.ToString();
        }

        public class UpperCaseUTF8Encoding : UTF8Encoding
        {
            // Code from a blog http://www.distribucon.com/blog/CategoryView,category,XML.aspx
            //
            // Dan Miser - Thoughts from Dan Miser
            // Tuesday, January 29, 2008 
            // He used the Reflector to understand the heirarchy of the encoding class
            //
            //      Back to Reflector, and I notice that the Encoding.WebName is the property used to
            //      write out the encoding string. I now create a descendant class of UTF8Encoding.
            //      The class is listed below. Now I just call XmlTextWriter, passing in
            //      UpperCaseUTF8Encoding.UpperCaseUTF8 for the Encoding type, and everything works
            //      perfectly. - Dan Miser

            public override string WebName
            {
                get { return base.WebName.ToUpper(); }
            }

            public static UpperCaseUTF8Encoding UpperCaseUTF8
            {
                get
                {
                    if (upperCaseUtf8Encoding == null)
                    {
                        upperCaseUtf8Encoding = new UpperCaseUTF8Encoding();
                    }
                    return upperCaseUtf8Encoding;
                }
            }

            private static UpperCaseUTF8Encoding upperCaseUtf8Encoding = null;

        }


        private class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding
            {
                get { return new UpperCaseUTF8Encoding(); }
            }
        }


        public static String PrettyPrintXML(String XML)
        {
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                //settings.OmitXmlDeclaration = true;
                IndentChars = "\t",
                Indent      = true
            };


            XmlDocument document = new XmlDocument();
            document.Load(new StringReader(XML));


            using (var strWriter = new Utf8StringWriter())
            using (XmlWriter writer = XmlWriter.Create(strWriter, settings))
            {
                document.Save(writer);

                return strWriter.ToString();
            }

        }


        public static string GetUserName()
        {

            HttpContext context = HttpContext.Current;
            if (context.User != null && context.User.Identity.IsAuthenticated)
            {
                FormsIdentity id = (FormsIdentity) context.User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;
                //role = ticket.UserData;
                return ticket.Name;
            }
            else
            {
                return "";
            }
        }

        public static int GetUserID()
        {

            using (IDbConnection con = new SqlConnection(GetBD_CFDI()))
            {

                try
                {
                    con.Open();
                    return con.Query<int>(
                        "SELECT ID FROM CFDI_ACCOUNT WITH (NOLOCK) WHERE USER_NAME = @USER_NAME",
                        new {USER_NAME = GetUserName()}).SingleOrDefault();

                    //***************************************************
                }
                catch (Exception ex)
                {
                    return -1;
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

        /*
        public static PropertyDescriptor PropertyDescriptor(this PropertyInfo propertyInfo)
        {
            return TypeDescriptor.GetProperties(propertyInfo.DeclaringType)[propertyInfo.Name];
        }
        */

        public static string Descr(this string str)
        {
            string des = "";
            var st = str.ToUpper();

            // Método de Pago
            switch (st)
            {
                case "PUE":
                    des = ".Pago en una sola exhibición";
                    break;
                case "PIP":
                    des = ".Pago inicial y parcialidades";
                    break;
                case "PPD":
                    des = ".Pago en parcialidades o diferido";
                    break;
            }

            //UsoCFDI
            switch (st)
            {
                case "G01":
                    des = ".Adquisición de mercancias";
                    break;
                case "G02":
                    des = ".Devoluciones, descuentos o bonificaciones";
                    break;
                case "G03":
                    des = ".Gastos en general";
                    break;
                case "I01":
                    des = ".Construcciones";
                    break;
                case "I02":
                    des = ".Mobilario y equipo de oficina por inversiones";
                    break;
                case "I03":
                    des = ".Equipo de transporte";
                    break;
                case "I04":
                    des = ".Equipo de computo y accesorios";
                    break;
                case "I05":
                    des = ".Dados, troqueles, moldes, matrices y herramental";
                    break;
                case "I06":
                    des = ".Comunicaciones telefónicas";
                    break;
                case "I07":
                    des = ".Comunicaciones satelitales";
                    break;
                case "I08":
                    des = ".Otra maquinaria y equipo";
                    break;
                case "D01":
                    des = ".Honorarios médicos, dentales y gastos hospitalarios.";
                    break;
                case "D02":
                    des = ".Gastos médicos por incapacidad o discapacidad";
                    break;
                case "D03":
                    des = ".Gastos funerales.";
                    break;
                case "D04":
                    des = ".Donativos.";
                    break;
                case "D05":
                    des = ".Intereses reales efectivamente pagados por créditos hipotecarios (casa habitación).";
                    break;
                case "D06":
                    des = ".Aportaciones voluntarias al SAR.";
                    break;
                case "D07":
                    des = ".Primas por seguros de gastos médicos.";
                    break;
                case "D08":
                    des = ".Gastos de transportación escolar obligatoria.";
                    break;
                case "D09":
                    des = ".Depósitos en cuentas para el ahorro, primas que tengan como base planes de pensiones.";
                    break;
                case "D10":
                    des = ".Pagos por servicios educativos (colegiaturas)";
                    break;
                case "P01":
                    des = ".Por definir";
                    break;
            }

            //Forma Pago

            switch (st)
            {
                case "01":
                    des = ".Efectivo";
                    break;
                case "02":
                    des = ".Cheque nominativo";
                    break;
                case "03":
                    des = ".Transferencia electrónica de fondos";
                    break;
                case "04":
                    des = ".Tarjeta de crédito";
                    break;
                case "05":
                    des = ".Monedero electrónico";
                    break;
                case "06":
                    des = ".Dinero electrónico";
                    break;
                case "08":
                    des = ".Vales de despensa";
                    break;
                case "12":
                    des = ".Dación en pago";
                    break;
                case "13":
                    des = ".Pago por subrogación";
                    break;
                case "14":
                    des = ".Pago por consignación";
                    break;
                case "15":
                    des = ".Condonación";
                    break;
                case "17":
                    des = ".Compensación";
                    break;
                case "23":
                    des = ".Novación";
                    break;
                case "24":
                    des = ".Confusión";
                    break;
                case "25":
                    des = ".Remisión de deuda";
                    break;
                case "26":
                    des = ".Prescripción o caducidad";
                    break;
                case "27":
                    des = ".A satisfacción del acreedor";
                    break;
                case "28":
                    des = ".Tarjeta de débito";
                    break;
                case "29":
                    des = ".Tarjeta de servicios";
                    break;
                case "99":
                    des = ".Por definir";
                    break;

            }

            //Regimen Fiscal
            switch (st)
            {
                case "601":
                    des = ".General de Ley Personas Morales";
                    break;
                case "603":
                    des = ".Personas Morales con Fines no Lucrativos";
                    break;
                case "605":
                    des = ".Sueldos y Salarios e Ingresos Asimilados a Salarios";
                    break;
                case "606":
                    des = ".Arrendamiento";
                    break;
                case "608":
                    des = ".Demás ingresos";
                    break;
                case "609":
                    des = ".Consolidación";
                    break;
                case "610":
                    des = ".Residentes en el Extranjero sin Establecimiento Permanente en México";
                    break;
                case "611":
                    des = ".Ingresos por Dividendos (socios y accionistas)";
                    break;
                case "612":
                    des = ".Personas Físicas con Actividades Empresariales y Profesionales";
                    break;
                case "614":
                    des = ".Ingresos por intereses";
                    break;
                case "616":
                    des = ".Sin obligaciones fiscales";
                    break;
                case "620":
                    des = ".Sociedades Cooperativas de Producción que optan por diferir sus ingresos";
                    break;
                case "621":
                    des = ".Incorporación Fiscal";
                    break;
                case "622":
                    des = ".Actividades Agrícolas, Ganaderas, Silvícolas y Pesqueras";
                    break;
                case "623":
                    des = ".Opcional para Grupos de Sociedades";
                    break;
                case "624":
                    des = ".Coordinados";
                    break;
                case "628":
                    des = ".Hidrocarburos";
                    break;
                case "607":
                    des = ".Régimen de Enajenación o Adquisición de Bienes";
                    break;
                case "629":
                    des = ".De los Regímenes Fiscales Preferentes y de las Empresas Multinacionales";
                    break;
                case "630":
                    des = ".Enajenación de acciones en bolsa de valores";
                    break;
                case "615":
                    des = ".Régimen de los ingresos por obtención de premios";
                    break;
            }

            return str + des;
        }


        public static void SetEmpresaID(string id)
        {

            HttpCookie myCookie = new HttpCookie(Settings.Default.Cookie1);
            DateTime now = DateTime.Now;

            myCookie.Value = id;
            myCookie.Expires = now.AddYears(50);
            HttpContext.Current.Response.Cookies.Add(myCookie);

        }

        public static string GetEmpresaID()
        {
            HttpCookie myCookie = HttpContext.Current.Request.Cookies[Settings.Default.Cookie1];

            string id = "1";
            if (myCookie != null && myCookie.Value != null && myCookie.Value != "")
            {
                id = myCookie.Value;
            }

            return id;
        }

        public static void SetEmpID(string id)
        {

            HttpCookie myCookie = new HttpCookie(Settings.Default.Cookie2);
            DateTime now = DateTime.Now;

            myCookie.Value = id;
            myCookie.Expires = now.AddYears(50);
            HttpContext.Current.Response.Cookies.Add(myCookie);
        }

        public static string GetEmpID()
        {
            HttpCookie myCookie = HttpContext.Current.Request.Cookies[Settings.Default.Cookie2];

            string id = null;
            if (myCookie != null && myCookie.Value != null && myCookie.Value != "")
            {
                id = myCookie.Value;
            }

            return id;
        }


        public static string GetAlias()
        {

            var emp = cEmpresa();

            return emp.ALIAS;
        }



        private static string conexion(string BD)
        {
            return String.Format(
                @"
                   Data Source={0};
                   Initial Catalog={1};
                   Integrated Security=False;
                   User ID={2};
                   Password={3};
                   Connect Timeout=6000;",
                Settings.Default.BD_HOST,
                BD,
                Settings.Default.BD_USER,
                pwd());
        }

        private static string pwd()
        {
            try
            {
                using (var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                {
                    using (var subKey = baseKey.OpenSubKey(@"SOFTWARE\LaSolucionEnIT"))
                    {
                        var value = subKey.GetValue("pwd2");
                        string pwd = StringSecurity.Security.DecryptString(value.ToString(), "LaSolucionEnIT2026#3487$");
                        return pwd;
                    }
                }
            }
            catch
            {
                return "";
            }
        }
        public static CFDI_EMPRESA cEmpresa()
        {
            using (IDbConnection con = new SqlConnection(conexion(Settings.Default.BD)))
            {
                try
                {
                    var empID = GetEmpID();

                    return con.Query<CFDI_EMPRESA>(
                        "SELECT * FROM CFDI_EMPRESA WITH (NOLOCK) WHERE ID = @ID",
                        new
                        {
                            ID = empID
                        }).SingleOrDefault();
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

            return null;
        }

        public static CFDI_EMPRESA cEmpresa(string ID)
        {
            using (IDbConnection con = new SqlConnection(conexion(Settings.Default.BD)))
            {
                try
                {
                    return con.Query<CFDI_EMPRESA>(
                        "SELECT * FROM CFDI_EMPRESA WITH (NOLOCK) WHERE ID = @ID",
                        new
                        {
                            ID
                        }).SingleOrDefault();
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

            return null;
        }

        public static List<CFDI_EMPRESA> cEmpresas()
        {
            using (IDbConnection con = new SqlConnection(conexion(Settings.Default.BD)))
            {
                try
                {
                    return con.Query<CFDI_EMPRESA>(
                        @"
                            SELECT
                              e.*,
                              t.DESCRIPCION AS TIPO_VALIDADOR2
                            FROM dbo.CFDI_EMPRESA e WITH (NOLOCK)
                            INNER JOIN dbo.TIPO_VALIDADOR t WITH (NOLOCK) ON e.TIPO_VALIDADOR = t.ID").ToList();
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

            return null;
        }

        public static string GetBD_CFDI()
        {

            const int maxReintentos = 3;
            const int delayMs = 1000;

            for (int intento = 1; intento <= maxReintentos; intento++)
            {
                var emp = cEmpresa();

                if (emp != null)
                {
                    return conexion(emp.EMPRESA_BD);
                }

                Log.Write($"GetBD_CFDI: cEmpresa() devolvió NULL (intento {intento}/{maxReintentos})");

                if (intento < maxReintentos)
                {
                    System.Threading.Thread.Sleep(delayMs);
                }
            }

            var errorMsg = $"No se pudo obtener la información de la empresa después de {maxReintentos} intentos. " +
                           "Verifique que la sesión del usuario esté activa.";
            Log.Write($"GetBD_CFDI ERROR: {errorMsg}");
            return null;
        }

        public static string GetBD()
        {

            return conexion(Settings.Default.BD);
        }

        public static string GetBD_CFDI(string id)
        {

            const int maxReintentos = 3;
            const int delayMs = 1000;

            for (int intento = 1; intento <= maxReintentos; intento++)
            {
                var emp = cEmpresa(id);

                if (emp != null)
                {
                    return conexion(emp.EMPRESA_BD);
                }

                Log.Write($"GetBD_CFDI: cEmpresa() devolvió NULL (intento {intento}/{maxReintentos})");

                if (intento < maxReintentos)
                {
                    System.Threading.Thread.Sleep(delayMs);
                }
            }

            var errorMsg = $"No se pudo obtener la información de la empresa después de {maxReintentos} intentos. " +
                           "Verifique que la sesión del usuario esté activa.";
            Log.Write($"GetBD_CFDI ERROR: {errorMsg}");
            return null;
        }

        /*
        public static string BD(this string sql)
        {
            if (GetBD_NAME() == "BD_PSI")
            {
                return sql.Replace("{BD}", "PSI");
            }

            return sql.Replace("{BD}", "PSO");
        }

        public static string BD(this string sql, string receptorRfc)
        {
            if (Settings.Default.ReceptorRFC_PSI.ToUpper() == receptorRfc.ToUpper())
            {
                return sql.Replace("{BD}", "PSI");
            }

            if (Settings.Default.ReceptorRFC_PSO.ToUpper() == receptorRfc.ToUpper())
            {
                //return sql.Replace("{BD}", "PSO");
            }

            return sql.Replace("{BD}", "");
        }*/

        public static string GetBD_ICG()
        {
            var emp = cEmpresa();

            /*
            if (GetBD_NAME() == "BD_PSI")
            {
                return Settings.Default.BD_PSI_ICG;
            }*/
            return conexion(emp.ICG_BD);
        }

        public static string GetRazon_Social()
        {
            var emp = cEmpresa();

            /*
            if (GetBD_NAME() == "BD_PSI")
            {
                return Settings.Default.RAZON_SOCIAL_PSI;
            }
            return Settings.Default.RAZON_SOCIAL_PSO;*/

            return emp.RAZON_SOCIAL;
        }

        public static string GetDireccion1()
        {
            /*
            if (GetBD_NAME() == "BD_PSI")
            {
                return Settings.Default.DIRECCION1_PSI;
            }
            return Settings.Default.DIRECCION1_PSO;
            */

            var emp = cEmpresa();
            return emp.DIRECCION1;
        }

        public static string GetCodPostal()
        {
            /*
            if (GetBD_NAME() == "BD_PSI")
            {
                return Settings.Default.CODPOSTAL_PSI;
            }
            return Settings.Default.CODPOSTAL_PSO;*/

            var emp = cEmpresa();
            return emp.CODPOSTAL;
        }

        public static string GetPoblacion()
        {
            /*
            if (GetBD_NAME() == "BD_PSI")
            {
                return Settings.Default.POBLACION_PSI;
            }
            return Settings.Default.POBLACION_PSO;
            */

            var emp = cEmpresa();
            return emp.POBLACION;
        }

        public static string GetProvincia()
        {
            var emp = cEmpresa();
            return emp.PROVINCIA;

            /*
            if (GetBD_NAME() == "BD_PSI")
            {
                return Settings.Default.PROVINCIA_PSI;
            }
            return Settings.Default.PROVINCIA_PSO;*/
        }

        public static string GetSerie()
        {
            /*
            if (GetBD_NAME() == "BD_PSI")
            {
                return Settings.Default.SERIE_PSI;
            }
            
            return Settings.Default.SERIE_PS0;*/

            var emp = cEmpresa();
            return emp.SERIE;
        }

        public static int GetEmpresa()
        {
            /*
            if (GetBD_NAME() == "BD_PSI")
            {
                return Settings.Default.EMPRESA_PSI;
            }
            return Settings.Default.EMPRESA_PS0;*/

            var emp = cEmpresa();
            return emp.EMPRESA.Value;
        }

        public static string GetReceptorRFC()
        {
            /*
            if (GetBD_NAME() == "BD_PSI")
            {
                return Settings.Default.EMPRESA_RFC;
            }
            return Settings.Default.ReceptorRFC_PSO;
            */

            var emp = cEmpresa();
            return emp.EMPRESA_RFC;
        }

        public static DateTime ToDateTime(this string date)
        {
            return Convert.ToDateTime(date);
        }

        public static string ToB64(this Stream inputStream)
        {
            return Convert.ToBase64String(inputStream.ToByteArray());
        }

        public static byte[] ToByteArray(this Stream inputStream)
        {
            //File.WriteAllBytes()
            inputStream.Position = 0;
            byte[] bytes = new byte[16384];
            using (MemoryStream memoryStream = new MemoryStream())
            {
                int count;
                while ((count = inputStream.Read(bytes, 0, bytes.Length)) > 0)
                {
                    memoryStream.Write(bytes, 0, count);
                }

                return memoryStream.ToArray();
            }
        }

        public static Stream ToStream(this string str)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(str);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static string GetStr(this Stream inputStream)
        {
            inputStream.Position = 0;
            /* convert stream to string*/
            StreamReader reader = new StreamReader(inputStream);
            //string text = reader.ReadToEnd();

            return reader.ReadToEnd(); //Encoding.UTF8.GetString(inputStream.ToByteArray());
        }

        public static string Str(this object obj)
        {
            return Convert.ToString(obj);
        }

        public static string Str(this string obj)
        {
            return Convert.ToString(obj);
        }

        public static Int32 Int32(this object obj)
        {
            return Convert.ToInt32(obj);
        }

        public static double Dbl(this object obj)
        {
            return Convert.ToDouble(obj); //Truncate(obj.Str(),2)
        }

        public static string Truncate(string value, int precision)
        {
            if (precision < 0)
            {
                throw new ArgumentOutOfRangeException("Precision cannot be less than zero");
            }

            string result = value.ToString();

            int dot = result.IndexOf('.');
            if (dot < 0)
            {
                return result;
            }

            int newLength = dot + precision + 1;

            if (newLength == dot + 1)
            {
                newLength--;
            }

            if (newLength > result.Length)
            {
                newLength = result.Length;
            }

            return result.Substring(0, newLength);
        }

        public static decimal Dec(this object obj)
        {
            return Convert.ToDecimal(obj);
        }

        public static object ParseDbl(this object obj)
        {
            double num;
            if (double.TryParse(obj.Str(), out num))
            {
                return num;
            }

            return obj;
            //return Convert.ToString(obj);
        }

        public static string[] Columns<T>(this IEnumerable<T> cols, bool renameColumns = true)
        {

            Type type = cols.GetType().GetGenericArguments()[0];
            var info = TypeDescriptor.GetProperties(type).Cast<PropertyDescriptor>()
                .ToDictionary(p => p.Name.ToLower(), p => p.DisplayName);

            var order = typeof(T).GetProperties().ToDictionary(x => x.Name, x => info[x.Name.ToLower()]);

            if (renameColumns)
            {
                return order.Values.ToArray();
            }

            return order.Keys.ToArray();
        }

        public static void RenameColumns<T>(this IXLTable tbl)
        {
            var info = TypeDescriptor.GetProperties(typeof(T)).Cast<PropertyDescriptor>()
                .ToDictionary(p => p.Name, p => p.DisplayName);

            foreach (var field in tbl.Fields)
            {
                if (info.ContainsKey(field.Name))
                {
                    field.Name = info[field.Name];
                }
            }
        }

        public static void RenameColumns<T>(this IXLRange tbl)
        {
            var info = TypeDescriptor.GetProperties(typeof(T)).Cast<PropertyDescriptor>()
                .ToDictionary(p => p.Name, p => p.DisplayName);

            foreach (var cell in tbl.FirstRow().Cells())
            {
                if (info.ContainsKey(cell.Value.ToString()))
                {
                    cell.Value = info[cell.Value.ToString()];
                }
            }
        }

        public static void AdjustToContents(this IXLRange range)
        {
            range.Worksheet.Columns(
                range.FirstColumn().ColumnLetter(),
                range.LastColumn().ColumnLetter()
            ).AdjustToContents();
        }

        public static void AdjustToContents(this IXLRange range, string firstColumn, string lastColumn)
        {
            range.Worksheet.Columns(firstColumn, lastColumn).AdjustToContents();
        }

        public static void AdjustToContents(this IXLRange range, string columns)
        {
            range.Worksheet.Columns(columns).AdjustToContents();
        }

        public static void AdjustToContents(this IXLRange range, int firstColumn, int lastColumn)
        {
            range.Worksheet.Columns(firstColumn, lastColumn).AdjustToContents();
        }

        public static string Description(this PropertyInfo propertyInfo)
        {
            var ob = TypeDescriptor.GetProperties(propertyInfo.DeclaringType)[propertyInfo.Name];
            return ob.Description == "" ? ob.Name : ob.Description;
        }

        public static string DisplayName(this PropertyInfo propertyInfo)
        {
            var ob = TypeDescriptor.GetProperties(propertyInfo.DeclaringType)[propertyInfo.Name];
            return ob.DisplayName;
        }

        public static void AddMap<T>()
        {
            Dapper.SqlMapper.SetTypeMap(typeof(T), new CustomPropertyTypeMap(typeof(T),
                (t, s) =>
                {
                    return t.GetProperties()
                        .FirstOrDefault(p => p.Description().ToLower() == s.ToLower());
                }));
        }

        public static T GetClass<T>(this string str)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(str);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public static T GetClass<T>(this StringBuilder str)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(str.ToString());
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public static string fDB(this string str)
        {
            if (str.Trim() != "")
            {
                str = str.Replace("\n", "")
                    .Replace("\r", "")
                    .Trim()
                    .dbRemoveSpace();

                var cad = "";
                foreach (var c in str.Split(";"))
                {
                    if (c.Trim() != "")
                    {
                        cad += "       " + c + ";\r\n";
                    }
                }

                return "\r\n" + cad + "  ";
            }

            return str;
        }

        public static Color GetColor(this string color)
        {
            var seg = color.Split(',');

            if (seg.Length == 4)
            {
                try
                {
                    return Color.FromArgb(
                        Convert.ToInt32(seg[0]),
                        Convert.ToInt32(seg[1]),
                        Convert.ToInt32(seg[2]),
                        Convert.ToInt32(seg[3]));
                }
                catch (Exception e)
                {
                    return Color.FromArgb(0);
                }
            }
            else
            {
                return Color.FromArgb(0);
            }
        }

        public static JObject json<T>(this T obj)
        {
            try
            {
                return JObject.Parse(JsonConvert.SerializeObject(obj));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static JObject json(this XmlDocument obj, string file)
        {
            try
            {
                obj.LoadXml(File.ReadAllText(file, Encoding.Default));
                return JObject.Parse(JsonConvert.SerializeXmlNode(obj, Formatting.None, true));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static void Save<T>(this T obj, string file)
        {
            try
            {
                var cf =
                    XDocument.Parse(
                        JsonConvert.DeserializeXmlNode(
                            JsonConvert.SerializeObject(obj),
                            "CONFIG").OuterXml).ToString();
                File.WriteAllText(file, cf, Encoding.Default);
            }
            catch (Exception ex)
            {

            }
        }

        public static T Load<T>(this T obj, string file)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(File.ReadAllText(file, Encoding.Default));
                string jsonText =
                    JObject.Parse(JsonConvert.SerializeXmlNode(doc, Formatting.None, true)).ToString();

                return JsonConvert.DeserializeObject<T>(jsonText);
            }
            catch (Exception ex)
            {
                //T ob = default(T);
                obj.Save(file);
                return obj;
            }
        }

        public static T? ConvertTo<T>(object x) where T : struct
        {
            return x == null ? null : (T?) Convert.ChangeType(x, typeof(T));
        }


        public static DataTable ToDataTable<T>(this IEnumerable<T> source)
        {
            return source.ToDataTable("");
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> source, bool renameColumns = true)
        {
            return source.ToDataTable("", renameColumns);
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> source, string tblName, bool renameColumns = true)
        {
            DataTable tbl = new DataTable(tblName);

            /*
            var info = TypeDescriptor.GetProperties(typeof(T)).Cast<PropertyDescriptor>()
                .ToDictionary(p => p.Name, p => p.DisplayName);
            */

            Type type = source.GetType().GetGenericArguments()[0];

            var info = TypeDescriptor.GetProperties(type).Cast<PropertyDescriptor>()
                .ToDictionary(p => p.Name.ToLower(), p => p.DisplayName);


            var order = typeof(T).GetProperties().ToDictionary(x => x.Name, x => info[x.Name.ToLower()]);

            //var order = info.Keys.ToArray(); //.Select(s => s.Value).ToArray();

            using (var reader = ObjectReader.Create(source, order.Keys.ToArray()))
            {
                tbl.Load(reader);
            }

            for (var i = 0; i < order.Count; i++)
            {
                try
                {
                    tbl.Columns[i].ColumnName = renameColumns ? order.ElementAt(i).Value : order.ElementAt(i).Key;
                }
                catch (Exception)
                {

                }
            }

            return tbl;
        }

        public static string SSubstring(this string value, int startIndex, int length)
        {
            return new string((value ?? string.Empty).Skip(startIndex).Take(length).ToArray());
        }

        public static string Repeat(this string value, int count)
        {
            return new StringBuilder(value.Length * count).Insert(0, value, count).ToString();
        }


        public static string dbRemoveSpace(this string str)
        {
            var ts = "";
            foreach (var s1 in str.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries))
            {

                if (s1.Contains("="))
                {
                    var s2 = s1.Split('=');

                    List<string> arr = new List<string>();
                    for (var i = 1; i < s2.Length; i++)
                    {
                        arr.Add(s2[i]);
                    }

                    ts += Regex.Replace(s2[0].Trim(), @"\s+", " ") + "=" + string.Join("=", arr.ToArray()) + ";";
                }
                else
                {
                    ts += s1.Trim();
                }
            }

            return ts;
        }

        //public static string[] Trunk(this string str, string formato)
        //{
        //    string cadena = "";
        //    List<string> rs = new List<string>();

        //    foreach (var s in formato.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries))
        //    {
        //        rs.Add(str.SSubstring(cadena.Length, s.Length).Trim());
        //        cadena += s + " ";
        //    }
        //    return rs.ToArray();

        //}

        public static string Args(this string str, params object[] arg)
        {
            return string.Format(str, arg);
        }



        public static string Join(this string[] str, string separator)
        {
            return string.Join(separator, str);
        }

        public static string[] Trunk(this string str, string formato, bool space = true)
        {
            List<string> list = new List<string>();
            int i = 0;
            foreach (var itm in formato.Split(" "))
            {
                list.Add(str.SSubstring(i, itm.Length));
                i += itm.Length;
                if (space) i++;
            }

            return list.ToArray();
        }

        public static string[] TrimEnd(this string[] str)
        {
            return str.Select(s => s.TrimEnd()).ToArray();
        }

        public static string[] Trim(this string[] str)
        {
            return str.Select(s => s.Trim()).ToArray();
        }

        public static string[] TrimStart(this string[] str)
        {
            return str.Select(s => s.TrimStart()).ToArray();
        }

        public enum TrimType
        {
            Trim,
            TrimEnd,
            TrimStart
        }

        public static void TrimAll<TSelf>(this TSelf obj, TrimType trimType = TrimType.Trim)
        {
            if (obj == null)
                return;

            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                 BindingFlags.FlattenHierarchy;

            foreach (PropertyInfo p in obj.GetType().GetProperties(flags))
            {
                Type currentNodeType = p.PropertyType;
                if (currentNodeType == typeof(String))
                {
                    string currentValue = (string) p.GetValue(obj, null);
                    if (currentValue != null)
                    {
                        switch (trimType)
                        {
                            case TrimType.Trim:
                                p.SetValue(obj, currentValue.Trim(), null);
                                break;
                            case TrimType.TrimStart:
                                p.SetValue(obj, currentValue.TrimStart(), null);
                                break;
                            case TrimType.TrimEnd:
                                p.SetValue(obj, currentValue.TrimEnd(), null);
                                break;
                        }

                    }
                }
                // see http://stackoverflow.com/questions/4444908/detecting-native-objects-with-reflection
                else if (currentNodeType != typeof(object) && Type.GetTypeCode(currentNodeType) == TypeCode.Object)
                {
                    if (p.GetIndexParameters().Length == 0)
                    {
                        p.GetValue(obj, null).TrimAll();
                    }
                    else
                    {
                        p.GetValue(obj, new Object[] {0}).TrimAll();
                    }
                }
            }
        }


        public static object getNumber(this string str)
        {
            return new String(str.Where(c => Char.IsDigit(c) || c == '.' || c == '-').ToArray());
        }



        public static T[] Trunk<T>(this string str, string formato, bool space = true)
        {
            List<T> list = new List<T>();
            int i = 0;
            foreach (var itm in formato.Split(" "))
            {
                var it = str.SSubstring(i, itm.Length).Trim();
                try
                {
                    list.Add((T) Convert.ChangeType(it, typeof(T)));
                }
                catch (Exception)
                {
                    try
                    {
                        list.Add((T) Convert.ChangeType(it.getNumber(), typeof(T)));
                    }
                    catch (Exception)
                    {
                        list.Add((T) Convert.ChangeType(0, typeof(T)));
                    }
                }

                i += itm.Length;
                if (space)
                    i++;
            }

            return list.ToArray();
        }

        public static bool isValid(this string str, string formato, bool forceLength = true)
        {
            if (forceLength && str.Length != formato.Length)
            {
                return false;
            }

            int i = 0;
            int count = 1;
            var arr = formato.Split(" ");

            var _char = "";
            foreach (var itm in arr)
            {
                i += itm.Length;

                _char = str.SSubstring(i, 1);
                if (_char == " ")
                {
                    count++;
                }

                i++;
            }

            if (_char != "")
            {
                count = -1;
            }

            return count == arr.Length;
        }



        public static string[] Split(this string str, string separator, bool removeEmpty = true)
        {
            if (removeEmpty)
            {
                return str.Split(new[] {separator}, StringSplitOptions.RemoveEmptyEntries);
            }

            return str.Split(new[] {separator}, StringSplitOptions.None);
        }


        public static string Str(this IEnumerable<char> str)
        {
            return new string(str.ToArray());
        }

        public static string Split(this string str, string separator1, string separator2)
        {
            try
            {
                return str.Split(separator1, false)[1].Split(separator2, false)[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool Contains(this string str, params string[] values)
        {
            int count = 0;
            foreach (var value in values)
            {
                if (str.Contains(value))
                    count++;

            }

            return count == values.Length;
        }

        public static string WordWrap(this string the_string, int width = 70)
        {
            if (the_string == null)
                return null;

            const string _newline = "<br />";
            int pos, next;
            StringBuilder sb = new StringBuilder();

            // Lucidity check
            if (width < 1)
                return the_string;

            // Parse each line of text
            for (pos = 0; pos < the_string.Length; pos = next)
            {
                // Find end of line
                int eol = the_string.IndexOf(_newline, pos);

                if (eol == -1)
                    next = eol = the_string.Length;
                else
                    next = eol + _newline.Length;

                // Copy this line of text, breaking into smaller lines as needed
                if (eol > pos)
                {
                    do
                    {
                        int len = eol - pos;

                        if (len > width)
                            len = BreakLine(the_string, pos, width);

                        sb.Append(the_string, pos, len);
                        sb.Append(_newline);

                        // Trim whitespace following break
                        pos += len;

                        while (pos < eol && Char.IsWhiteSpace(the_string[pos]))
                            pos++;

                    } while (eol > pos);
                }
                else sb.Append(_newline); // Empty line
            }

            return sb.ToString();
        }

        public static int BreakLine(string text, int pos, int max)
        {
            // Find last whitespace in line
            int i = max - 1;
            while (i >= 0 && !Char.IsWhiteSpace(text[pos + i]))
                i--;
            if (i < 0)
                return max; // No whitespace found; break at maximum length
            // Find start of whitespace
            while (i >= 0 && Char.IsWhiteSpace(text[pos + i]))
                i--;
            // Return length of text before whitespace
            return i + 1;
        }


        public static List<CFDI_XML> GetFact(string emisorRfc, string serie, string folio, string TipoDeComprobante)
        {
            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {
                string sql =
                    @"
                    SELECT
                      xml.ID,
                      xml.USER_ID,
                      xml.CONTRA_RECIBO_ID,
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
                      xml.NO_COMPRA,
                      xml.FECHA_COMPRA,
                      xml.NOMBRE_ARCHIVO,
                      es.ESTATUS AS ESTATUS2,
                      xml.TIPO_COMPROBANTE,
                      xml.NUMEFECTO,
                      xml.FECHA_SALDADO,
                      ct.FECHA_PAGO
                    FROM dbo.CFDI_XML_PSI xml WITH (NOLOCK)
                    INNER JOIN dbo.CFDI_ESTATUS es WITH (NOLOCK) ON xml.ESTATUS = es.ID
                    LEFT OUTER JOIN dbo.CONTRA_RECIBOS_PSI ct WITH (NOLOCK) ON xml.CONTRA_RECIBO_ID = ct.ID
                    WHERE 

                    --xml.RFC_EMISOR = @RFC_EMISOR AND (@FOLIO IS NOT NULL OR UPPER(xml.FOLIO) = UPPER(@FOLIO)) AND (@SERIE IS NOT NULL OR UPPER(xml.SERIE) = UPPER(@SERIE))
                    xml.RFC_EMISOR = @RFC_EMISOR AND UPPER(ISNULL(xml.FOLIO,'')) = UPPER(@FOLIO) AND UPPER(ISNULL(xml.SERIE,'')) = UPPER(@SERIE)

                    ORDER BY xml.CONTRA_RECIBO_ID DESC";


                if (string.IsNullOrEmpty(serie))
                {
                    serie = "";
                }

                if (string.IsNullOrEmpty(folio))
                {
                    folio = "";
                }

                var rs = con.Query<CFDI_XML>(sql, new { RFC_EMISOR = emisorRfc,serie,folio }).ToList();

                foreach (var xml in rs)
                {
                    if (xml.TIPO_COMPROBANTE != null && (xml.TIPO_COMPROBANTE.ToLower() == "e" ||
                                                         xml.TIPO_COMPROBANTE.ToLower() == "egreso"))
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

                            if (xm.TIPO_COMPROBANTE != null && (xm.TIPO_COMPROBANTE.ToLower() == "e" ||
                                                                xm.TIPO_COMPROBANTE.ToLower() == "egreso"))
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

                var rsx = rs.Where(w => 
                    w.TIPO_COMPROBANTE.ToLower() == TipoDeComprobante.ToLower()
                    ).ToList();

                return rsx;
            }


        }
    }
}