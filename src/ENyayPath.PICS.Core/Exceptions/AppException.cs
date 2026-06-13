using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ENyayPath.PICS.Core.Exceptions
{
    [Serializable]
    public class AppException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="AppException"/> object.
        /// </summary>
        public AppException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="AppException"/> object.
        /// </summary>
        public AppException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="AppException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public AppException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new <see cref="AppException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public AppException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
