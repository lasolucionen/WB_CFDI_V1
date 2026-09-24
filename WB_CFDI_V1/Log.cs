using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;


namespace WB_CFDI_V1
{
    static class Log
    {
        public static void Write(string msg, bool date = true, string USER_NAME = "")
        {
            try
            {
                try
                {
                    CleanOldLogs();
                }
                catch
                {
                    // Ignorar errores de limpieza
                }

                string dirPath = HttpContext.Current.Server.MapPath("~/log");

                // Si USER_NAME tiene valor, usar subcarpeta
                if (!string.IsNullOrEmpty(USER_NAME))
                {
                    dirPath = Path.Combine(dirPath, USER_NAME);
                }

                // Crear el directorio si no existe
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                string path = dirPath + "\\Log_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

                string line = "[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] : " + msg;

                // Abrir con FileShare.ReadWrite para no bloquear el archivo
                using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter file = new StreamWriter(fs, Encoding.Default))
                {
                    file.WriteLine(date ? line : msg);
                }
            }
            catch
            {
                // Ignorar cualquier error al escribir logs
                // No propagar excepciones para no afectar la ejecución de la aplicación
            }
        }

        public static void Date(string USER_NAME = "")
        {
            try
            {
                string dirPath = HttpContext.Current.Server.MapPath("~/log");

                // Si USER_NAME tiene valor, usar subcarpeta
                if (!string.IsNullOrEmpty(USER_NAME))
                {
                    dirPath = Path.Combine(dirPath, USER_NAME);
                }

                // Crear el directorio si no existe
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                string path = dirPath + "\\Log_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

                string line = "[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "]\r\n";

                // Abrir con FileShare.ReadWrite para no bloquear el archivo
                using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter file = new StreamWriter(fs, Encoding.Default))
                {
                    file.WriteLine(line);
                }
            }
            catch
            {
                // Ignorar cualquier error al escribir logs
                // No propagar excepciones para no afectar la ejecución de la aplicación
            }
        }


        public static void CleanOldLogs(int months = 2)
        {
            try
            {
                if (HttpContext.Current == null) return;

                string dirPath = HttpContext.Current.Server.MapPath("~/log");
                if (!Directory.Exists(dirPath)) return;

                DateTime threshold = DateTime.Now.AddMonths(-months);

                // Limpiar logs en el directorio principal
                CleanLogsInDirectory(dirPath, threshold);

                // Limpiar logs en subdirectorios
                try
                {
                    var subDirs = Directory.GetDirectories(dirPath);
                    foreach (var subDir in subDirs)
                    {
                        CleanLogsInDirectory(subDir, threshold);
                    }
                }
                catch
                {
                    // No propagar excepciones de limpieza de subdirectorios
                }
            }
            catch
            {
                // Ignorar cualquier error al limpiar logs
            }
        }

        private static void CleanLogsInDirectory(string dirPath, DateTime threshold)
        {
            try
            {
                var files = Directory.GetFiles(dirPath, "Log_*.txt");

                foreach (var file in files)
                {
                    try
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file); // "Log_dd-MM-yyyy"
                        if (fileName.Length <= 4) continue;

                        string datePart = fileName.Substring(4); // obtiene "dd-MM-yyyy"
                        DateTime fileDate;
                        if (DateTime.TryParseExact(datePart, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fileDate))
                        {
                            if (fileDate < threshold)
                            {
                                try { File.Delete(file); } catch { /* ignorar errores de borrado */ }
                            }
                        }
                        else
                        {
                            // Si el nombre no tiene el formato, fallback por fecha de creación
                            try
                            {
                                var fi = new FileInfo(file);
                                if (fi.CreationTime < threshold)
                                {
                                    try { File.Delete(file); } catch { }
                                }
                            }
                            catch { }
                        }
                    }
                    catch
                    {
                        // No propagar excepciones de limpieza de archivos individuales
                    }
                }
            }
            catch
            {
                // Ignorar cualquier error al obtener archivos del directorio
            }
        }

    }
}