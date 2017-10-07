using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hwa.Framework.Cryptography
{
    public class RsaKeyInfo
    {
        public string Modn { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
    }
}
