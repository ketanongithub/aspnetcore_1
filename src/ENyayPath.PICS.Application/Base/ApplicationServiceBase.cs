using ENyayPath.PICS.Core.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Application.Base
{
    public abstract class ApplicationServiceBase
    {
        protected OperationResult<T> Success<T>(T data) => OperationResult<T>.Ok(data);
        protected OperationResult<T> Failure<T>(string message) => OperationResult<T>.Fail(message);
    }
}
