using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hwa.Framework.Mvc.Annotations.RegularExpressions
{
    public static class RegularExpression
    {
        /// <summary>
        /// 邮箱验证的正则表达式
        /// </summary>
        public const string EMAIL_REGULAR = @"^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$";
        /// <summary>
        /// 邮箱验证的错误信息
        /// </summary>
        public const string EMAIL_ERROR_MESSAGE = "请输入正确的电子邮件格式\n示例：abc@123.com";

        /// <summary>
        /// 电话号码、手机
        /// </summary>
        public const string PHONE_OR_MOBILEPHONE = @"(" + FAX_OR_PHONE + ")|(" + MOBILEPHONE + ")";
        /// <summary>
        /// 错误信息
        /// </summary>
        public const string PHONE_OR_MOBILEPHONE_ERROR_MESSAGE = "请输入正确的手机号码或电话号码!";

        /// <summary>
        /// 手机号码正则表达式
        /// </summary>
        public const string MOBILEPHONE = @"^(13[0-9]|14[5|7]|15[0-9]|17[0|6|7|8]|18[0-9])\d{8}$";
        /// <summary>
        /// 错误信息
        /// </summary>
        public const string MOBILEPHONE_ERROR_MESSAGE = "请输入正确的手机号码!";

        /// <summary>
        /// 传真/电话号码
        /// </summary>
        public const string FAX_OR_PHONE = @"(^[0-9]{3,4}\-[0-9]{3,8}$)|(^([0-9]{3,4})[0-9]{3,8}$)|(^[0-9]{3,8}$)";
        /// <summary>
        /// 错误信息
        /// </summary>
        public const string FAX_OR_PHONE_ERROR_MESSAGE = "请输入正确的传真/电话号码!";

        /// <summary>
        /// 折扣：大于等于1小于等于100
        /// </summary>
        public const string DISCOUNT = @"^(100|[1-9]\d{1}|[1-9])$";
        /// <summary>
        /// 折扣错误信息
        /// </summary>
        public const string DISCOUNT_ERROR_MESSAGE = "只能为1~100之间的整数";

        /// <summary>
        /// 编码正则表达式
        /// </summary>
        public const string CODE_REGULAR = "^[A-Za-z0-9]+$";
        /// <summary>
        /// 错误信息
        /// </summary>
        public const string CODE_ERROR_MESSAGE = "只能填写数字或字母";

        /// <summary>
        /// 企业代码正则表达式
        /// </summary>
        public const string TENANTCODE_REGULAR = @"(^([a-zA-Z0-9]|[_])*$)|(" + EMAIL_REGULAR + ")";
        /// <summary>
        /// 错误信息
        /// </summary>
        public const string TENANTCODE_REGULAR_ERROR_MESSAGE = "只能输入字母数字及\"_\"，或输入的邮箱格式不正确！";

        /// <summary>
        /// 用户编码正则表达式
        /// </summary>
        public const string USERNAME_REGULAR = @"(^([a-zA-Z0-9]|[_])*$)|(" + EMAIL_REGULAR + ")";
        //public const string USERNAME_REGULAR = "^.*$";//通配所有
        /// <summary>
        /// 错误信息
        /// </summary>
        public const string USERNAME_ERROR_MESSAGE = "只能输入字母数字及\"_\"，或输入的邮箱格式不正确！";

        /// <summary>
        /// 货号正则表达式
        /// </summary>
        public const string ITEMCODE_REGULAR = "^(?!_|-)(\\w|-)+$";
        /// <summary>
        /// 错误信息
        /// </summary>
        public const string ITEMCODE_REGULAR_ERROR_MESSAGE = "不能有\"+*%/\"等特殊字符，不能以\"_\"或\"-\"开头！";

        /// <summary>
        /// 短时间格式（HH:mm）
        /// </summary>
        public const string SHORTTIME_HHMM_REGULAR = @"^((([0-1][0-9])|(2[0-3])):[0-5]\d)$";
        /// <summary>
        /// 错误信息
        /// </summary>
        public const string SHORTTIME_HHMM_REGULAR_ERROR_MESSAGE = "请输入正确的时间格式(HH:mm)";

        /// <summary>
        /// 短时间格式（HH:mm）
        /// </summary>
        public const string SHORTTIME_HHMMSS_REGULAR = @"^((([0-1][0-9])|(2[0-3])):[0-5]\d:[0-5]\d)$";
        /// <summary>
        /// 错误信息
        /// </summary>
        public const string SHORTTIME_HHMMSS_REGULAR_ERROR_MESSAGE = "请输入正确的时间格式(HH:mm:ss)";

        /// <summary>
        /// 系统中的数量最小范围值
        /// </summary>
        public const double RANG_MIN_QTY = 0;
        /// <summary>
        /// 系统中的数量最大范围值
        /// </summary>
        public const double RANG_MAX_QTY = 99999999;
        /// <summary>
        /// 系统中的数量超过范围的错误信息
        /// </summary>
        public const string RANG_QTY_ERROR_MESSAGE = "只能在0~99999999的范围之间";
        /// <summary>
        /// 系统中的金额最小范围值
        /// </summary>
        public const double RANG_MIN_AMOUNT = 0;
        /// <summary>
        /// 系统中的金额最大范围值
        /// </summary>
        public const double RANG_MAX_AMOUNT = 99999999;
        /// <summary>
        /// 系统中的金额超过范围的错误信息
        /// </summary>
        public const string RANG_AMOUNT_ERROR_MESSAGE = "只能在0~99999999的范围之间";
    }
}
