using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecTech.Domain.Result
{
    public class BaseResult
    {
        public Boolean IsSuccess => ErrorMessage == null;
        public int? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }

    }

    public class BaseResult<T> : BaseResult
    {
        public BaseResult()
        {
        }

        public BaseResult(string errorMessage, int errorCode, T data)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            Data = data;
        }
        public T Data { get; set; }
    }
}
