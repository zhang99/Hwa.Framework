using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hwa.Framework.Cryptography
{
    public static class MD5CryptoHelper
    {
        private const string MD5_SALT = "GVYURdyryu7#4567@A$I(O$^hgh";
        private const string MD5_PLATFORM_SALT = "aD$kTPOS31cnn#$%WMAC^h89ii*K";
        private const string MD5_SUPPLYCHAIN_SALT = "74jfursSJF@&~KAJD&u572K_";

        public static string CreateMD5(string fileName)
        {
            string hashStr = string.Empty;
            try
            {
                FileStream fs = new FileStream(
                    fileName,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read);
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                byte[] hash = md5.ComputeHash(fs);
                hashStr = ByteArrayToHexString(hash);
                fs.Close();
                fs.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return hashStr;
        }

        public static string CreateMD5(Stream stream)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(stream);
            return ByteArrayToHexString(hash);
        }

        public static string CreateStringMD5(String str)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(str + MD5_SALT);
            return CreateMD5(buffer, 0, buffer.Length);
        }

        public static string CreatePlatformStringMD5(String str)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(str + MD5_PLATFORM_SALT);
            return CreateMD5(buffer, 0, buffer.Length);
        }

        public static string CreateSupplyChainStringMD5(String str)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(str + MD5_SUPPLYCHAIN_SALT);
            return CreateMD5(buffer, 0, buffer.Length);
        }

        public static string CreateMD5(byte[] buffer, int offset, int count)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(buffer, offset, count);
            return ByteArrayToHexString(hash);
        }

        private static string ByteArrayToHexString(byte[] values)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte value in values)
            {
                sb.AppendFormat("{0:X2}", value);
            }
            return sb.ToString();
        }

    }
}
