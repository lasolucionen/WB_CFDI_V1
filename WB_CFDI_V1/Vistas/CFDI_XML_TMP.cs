using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using Dapper;

namespace WB_CFDI_V1.Vistas
{
    public class CFDI_XML_TMP
    {
        public int Id { get; set; }
        public string User_Name { get; set; }
        public string Xml { get; set; }
        public string UUID { get; set; }
        public string Nombre_Archivo { get; set; }
        public byte[] Archivo { get; set; }



        public void AddData(CFDI_XML_TMP data)
        {
            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {
                var sql = @"
                INSERT INTO CFDI_BD.dbo.CFDI_XML_TMP (ID, USER_NAME, XML, UUID, NOMBRE_ARCHIVO, ARCHIVO)
                VALUES (@Id, @User_Name, @Xml, @UUID, @Nombre_Archivo, @Archivo)
                ";


                data.User_Name = Extensions.GetUserName();

                con.Execute(sql, data);
            }
        }

        public static void DeleteData()
        {
            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {
                var sql = @"
                DELETE FROM CFDI_XML_TMP WHERE USER_NAME = @UserName
                ";
               
                con.Execute(sql, new {UserName= Extensions.GetUserName() });
            }
        }

        public static List<CFDI_XML_TMP> GetData()
        {
            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {
                var sql = @"
                    SELECT * FROM CFDI_XML_TMP WHERE USER_NAME = @USER_NAME
           
                ";


                return con.Query<CFDI_XML_TMP>(sql, new { USER_NAME = Extensions.GetUserName() }).ToList();

            }
        }
    }
}