using Microsoft.International.Converters.PinYinConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Hwa.Framework
{
    public static class MathsHelper
    {
        /// <summary>
        /// 从字符串公式中计算结果
        /// </summary>
        /// <param name="formula">字符串公式</param>
        /// <param name="parms">字符串包含的参数</param>
        /// <returns></returns>
        public static object FormulaResult(string formula, params object[] parms)
        {
            string formulate = string.Format(formula, parms);
            System.Data.DataTable dt = new System.Data.DataTable();
            return dt.Compute(formulate, "");
        }
       
    }
}
