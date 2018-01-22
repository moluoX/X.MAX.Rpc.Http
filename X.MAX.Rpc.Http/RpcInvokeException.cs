using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace X.MAX.Rpc.Http
{
    public class RpcInvokeException : Exception
    {
        public int? Code { get; set; }

        public RpcInvokeException()
        {
        }

        public RpcInvokeException(string message) : base(message)
        {
        }

        public RpcInvokeException(string message, int? code) : base(message)
        {
            Code = code;
        }

        public RpcInvokeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RpcInvokeException(string message, int? code, Exception innerException) : base(message, innerException)
        {
            Code = code;
        }

        protected RpcInvokeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public RpcInvokeException ExceptionInfo { get; set; }
    }
}
