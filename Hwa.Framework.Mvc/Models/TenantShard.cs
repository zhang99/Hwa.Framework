using Hwa.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hwa.Framework.Mvc.Model
{
    [Serializable]
    public class TenantShard
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
       
        //public ProductType ProductType { get; set; }

        //public DbConnectionType DbType { get; set; }

        public string DbHost { get; set; }

        public string DbName { get; set; }
    }
}
