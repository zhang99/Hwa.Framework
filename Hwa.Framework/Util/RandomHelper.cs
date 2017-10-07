using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hwa.Framework.Util
{
    public class RandomHelper
    {
        /// <summary>
        /// 生成指定长度的随机字符
        /// </summary>
        /// <param name="iLength">字符长度</param>
        /// <returns></returns>
        public static string GetRandomString(int iLength)
        {
            return GetRandom(iLength, "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_");
        }

        /// <summary>
        /// 生成指定长度的随机数字
        /// </summary>
        /// <param name="iLength"></param>
        /// <returns></returns>
        public static string GetRandomNumber(int iLength)
        {
            return GetRandom(iLength, "0123456789");
        }

        /// <summary>
        /// 在buffer范围内获取随机字符
        /// </summary>
        /// <param name="iLength"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private static string GetRandom(int iLength, string buffer)
        {
            StringBuilder sb = new StringBuilder();
            Random r = new Random();
            int range = buffer.Length;
            for (int i = 0; i < iLength; i++)
            {
                sb.Append(buffer.Substring(r.Next(range), 1));
            }
            return sb.ToString();
        }
    }
}
