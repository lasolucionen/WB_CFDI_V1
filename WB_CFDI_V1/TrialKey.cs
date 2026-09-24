/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rebex.Security.Cryptography;

namespace WB_CFDI_V1
{
    static class TrialKey
    {
        /// <summary>
        /// Set the key to start your evaluation period.
        /// You can get your trial key at http://www.rebex.net/support/trial-key.aspx
        /// No key is needed for the full version.
        /// </summary>
        //public const string Key = "==AU/NSblRVvucV4k27ZdqI2/0ZEizoVL0kDdxPPeraeZI==";
        //public const string Key = "==AfEuB75WgnKrZmIlOv2NrTkLMrQ2xENKgLSxuF1Jedts==";
        public static string Key { get; set; }
        static TrialKey()
        {
            var forBuild = 65535;
            var validTillDate = DateTime.Now.AddDays(64).Subtract(new DateTime(2010, 12, 31)).TotalDays;

            var forgedBytes = new byte[16];
            forgedBytes[0] = (byte)(validTillDate / 256);
            forgedBytes[1] = (byte)(validTillDate % 256);
            forgedBytes[2] = (byte)(forBuild / 256);
            forgedBytes[3] = (byte)(forBuild % 256);

            var output = new byte[32];
            output[0] = 7;
            var rnd = new Random();
            for (var x = 1; x < 13; x++)
                output[x] = (byte)rnd.Next(1, 256);

            Array.Copy(forgedBytes, 0, output, 14, forgedBytes.Length);

            var rsaManaged2 = new RSAManaged();
            rsaManaged2.FromXmlString("<RSAKeyValue><Modulus>thycVKzZzdxBD6Rl8RoS9MEs1rrLY5qDhse+a+ljfpM=</Modulus><Exponent>AQAB</Exponent><P>xJXNbvuhJEpA647ZChJHMQ==</P><Q>7Sb4m1/8WXGGL/2Zw075Aw==</Q><DP>VtattvbkyfkbEHM7oN1OIQ==</DP><DQ>0kQaatCpErjYDBbjTUro9w==</DQ><InverseQ>AVeR8pKZ4H05p7NRb02kNw==</InverseQ><D>ENshFS1Sk51ZYEtFLEXPjzPUmZbbIak0S+dyUK5o/sE=</D></RSAKeyValue>");

            var decrypted = rsaManaged2.DecryptValue(output);

            Key = "==A" + Convert.ToBase64String(decrypted) + "=";
            //Key = "==AKYbfG5RfZbvvuVhvJaiO7g522AntYxH4wiUDG43G5fs==";
        }
    }
}*/