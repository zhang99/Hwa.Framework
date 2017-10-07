using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hwa
{
    public static class StringExtension
    {
        /// <summary>
        /// IsNullOrEmpty
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        /// <summary>
        /// IsNullOrWhiteSpace
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        /// <summary>
        /// HasValue
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool HasValue(this string s)
        {
            return !string.IsNullOrEmpty(s);
        }

        /// <summary>
        /// ToInt
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int ToInt(this string s)
        {
            return Int32.Parse(s);
        }

        /// <summary>
        /// ToNullable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return default(T);

            try
            {
                return (T)Convert.ChangeType(s, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// ToBase64
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToBase64(this string s)
        {
            byte[] toEncodeAsBytes = Encoding.UTF8.GetBytes(s);
            return System.Convert.ToBase64String(toEncodeAsBytes);
        }

        /// <summary>
        /// DecodeFromBase64
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string DecodeFromBase64(this string s)
        {
            try
            {
                byte[] encodedDataAsBytes = System.Convert.FromBase64String(s);
                return Encoding.UTF8.GetString(encodedDataAsBytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 反转字符串
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static string ToReverseString(this string original)
        {
            char[] c = original.ToCharArray();
            Array.Reverse(c);
            return new string(c);
        }

        /// <summary>
        /// 返回单词的复数形式
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static string ToWordPlural(this string original)
        {
            Regex plural1 = new Regex("(?<keep>[^aeiou])y$");
            Regex plural2 = new Regex("(?<keep>[aeiou]y)$");
            Regex plural3 = new Regex("(?<keep>[sxzh])$");
            Regex plural4 = new Regex("(?<keep>[^sxzhy])$");

            if (plural1.IsMatch(original))
                return plural1.Replace(original, "${keep}ies");
            else if (plural2.IsMatch(original))
                return plural2.Replace(original, "${keep}s");
            else if (plural3.IsMatch(original))
                return plural3.Replace(original, "${keep}es");
            else if (plural4.IsMatch(original))
                return plural4.Replace(original, "${keep}s");

            return original;
        }

        /// <summary>
        /// 返回单词的单数形式
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ToWordSingle(this string original)
        {
            Regex plural1 = new Regex("(?<keep>[^aeiou])ies$");
            Regex plural2 = new Regex("(?<keep>[aeiou]y)s$");
            Regex plural3 = new Regex("(?<keep>[sxzh])es$");
            Regex plural4 = new Regex("(?<keep>[^sxzhyu])s$");

            if (plural1.IsMatch(original))
                return plural1.Replace(original, "${keep}y");
            else if (plural2.IsMatch(original))
                return plural2.Replace(original, "${keep}");
            else if (plural3.IsMatch(original))
                return plural3.Replace(original, "${keep}");
            else if (plural4.IsMatch(original))
                return plural4.Replace(original, "${keep}");

            return original;
        }

        /// <summary>
        /// 全角转半角
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToDBC(this string str)
        {
            char[] c = str.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }

        /// <summary>
        /// 全角转半角
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static char ToDBCChar(this char input)
        {
            if (input == 12288)
            {
                input = (char)32;
            }
            if (input > 65280 && input < 65375)
                input = (char)(input - 65248);
            return input;
        }

        /// <summary>
        /// 获取字符串字节长度(中文2个字符)
        /// </summary>
        /// <param name="strContent"></param>
        /// <returns></returns>
        public static int ByteLength(this string str)
        {
            return System.Text.Encoding.Default.GetByteCount(str);
        }

        /// <summary>
        /// 在指定标识前插入字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="insertFlag"></param>
        /// <param name="appendStr"></param>
        /// <returns></returns>
        public static string InsertBeforeFlag(this string str, string insertFlag, string appendStr)
        {
            if (str.IndexOf(insertFlag) < 0) return str;

            return str.Substring(0, str.IndexOf(insertFlag)) + appendStr + str.Substring(str.IndexOf(insertFlag));
        }

        /// <summary>
        /// 拼接或往SringBuilder指定位置插入字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="left"></param>
        /// <param name="rightFlag"></param>
        /// <param name="insertStr"></param>
        public static void InsertJoinStrInMessage(this StringBuilder str, string left, string rightFlag, string insertStr, string join = ",")
        {
            if (str.ToString().IndexOf(rightFlag) >= 0)
            {
                rightFlag = str.ToString().InsertBeforeFlag(rightFlag, join + insertStr);
                str.Clear();
                str.Append(rightFlag);
            }
            else
            {
                str.Append(left + insertStr + rightFlag);
            }
        }

        /// <summary>
        /// 获取补位字符串
        /// </summary>
        /// <param name="str">传入的字符串</param>
        /// <param name="length">字符长度</param>
        /// <param name="c">补位的字符</param>
        /// <param name="cut">超过长度的部分是否截取掉</param>
        /// <returns></returns>
        public static string GetSupplementString(string str, int length, char c, bool cut = false)
        {
            if (!string.IsNullOrEmpty(str) && str.Length >= length)
                return cut ? str.Substring(0, length) : str;
            int supLen = length - (string.IsNullOrEmpty(str) ? 0 : str.Length);
            string supStr = ((int)Math.Pow(10, supLen)).ToString().Substring(1).Replace('0', c);
            return str + supStr;
        }  
    }
}
