using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Hwa.Framework.Mvc;
using Hwa.Framework.Mvc.Annotations.GridAnnotations;

namespace Hwa.Framework.Mvc.Model
{
    public abstract class EntityBase
    {
        /// <summary>
        /// Id唯一标识
        /// </summary>
        [Key]
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Sortable(false), Queryable(false)]
        [System.ComponentModel.DataAnnotations.Schema.Column(TypeName = "bigint")]
        public virtual long Id { get; set; }
    }
}
