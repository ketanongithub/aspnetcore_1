using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.Exceptions
{
    public class BusinessException : Exception
    {
        public string Code { get; }

        public BusinessException(string message, string code = "BUSINESS_ERROR")
            : base(message)
        {
            Code = code;
        }
    }
}
