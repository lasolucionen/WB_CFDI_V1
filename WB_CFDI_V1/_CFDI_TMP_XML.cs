using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;

namespace WB_CFDI_V1
{
    public class _CFDI_TMP_XML
    {
        public string USER_NAME { get; set; }
        public string DATA { get; set; }


        public static void AddData(string data)
        {
            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {
                var sql = @"

                    DELETE FROM CFDI_TMP_XML WHERE USER_NAME = @USER_NAME

                    INSERT INTO CFDI_TMP_XML (USER_NAME, DATA) VALUES
                    (@USER_NAME, @DATA)
                
                ";


                    con.Execute(sql, new { USER_NAME = Extensions.GetUserName(), data });
                

            }
        }


        public static string GetData()
        {
            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {
                var sql = @"
                    SELECT data FROM CFDI_TMP_XML WHERE USER_NAME = @USER_NAME
           
                ";


                var str = con.ExecuteScalar<string>(sql, new { USER_NAME = Extensions.GetUserName() });
                return str;

            }
        }
    }
}