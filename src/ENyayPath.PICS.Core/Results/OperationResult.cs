using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.Results
{
    public class OperationResult<T>
    {
        public bool Success { get; private set; }
        public string? ErrorMessage { get; private set; }
        public T? Data { get; private set; }

        private OperationResult(bool success, T? data, string? errorMessage)
        {
            Success = success;
            Data = data;
            ErrorMessage = errorMessage;
        }

        public static OperationResult<T> Ok(T data) => new(true, data, null);
        public static OperationResult<T> Fail(string errorMessage) => new(false, default, errorMessage);
    }
}
