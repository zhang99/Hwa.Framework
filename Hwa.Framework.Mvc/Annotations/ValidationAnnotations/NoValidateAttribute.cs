using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Hwa.Framework.Mvc.Annotations.ValidationAnnotations
{
    /// <summary>
    /// 无需Model验证(标记在Id字段上，此Id对应的SiUControlFor不生成验证，如Item.BrandId)
    /// </summary>
    public class NoValidateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return true;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {            
            return null;
        }
    }
}
