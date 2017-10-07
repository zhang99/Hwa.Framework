using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hwa.Framework.Mvc
{
    /// <summary>
    /// 全局格式枚举类
    /// </summary>
    public enum GlobalFormatType
    {
        Text = 0, //文本
        Date = 1, //日期
        DateTime = 2,//时间精确到时分秒
        Quantity = 3, //数量
        Money = 4, //金额
        Price = 5, //价格
        Percent = 6, //百分比，一般用于报表显示
        Rate = 7,//比率格式,默认为两个小数点的统一显示处理。
        Integer= 8 //整型
    }   
}
