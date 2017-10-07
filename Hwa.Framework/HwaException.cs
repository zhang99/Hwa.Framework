using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hwa.Framework
{
    /// <summary>
    /// Base exception.
    /// </summary>
    [Serializable]
    public class HwaException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="HwaException"/> object.
        /// </summary>
        public HwaException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="HwaException"/> object.
        /// </summary>
        public HwaException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="HwaException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public HwaException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new <see cref="HwaException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public HwaException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
