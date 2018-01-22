using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

namespace X.MAX.Rpc.Http
{
    public class RpcProxy : DispatchProxy
    {
        public string ServiceUrl { get; set; }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            //请求
            string url = AnalyzeUri(targetMethod);
            var request = WebRequest.CreateHttp(url);
            request.Timeout = 60000;
            request.Method = "POST";
            request.Accept = "application/json; charset=utf-8";

            //body
            var body = JsonConvert.SerializeObject(args);
            var bodyBytes = Encoding.UTF8.GetBytes(body);
            request.ContentType = "application/json; charset=utf-8";
            request.ContentLength = bodyBytes.LongLength;
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(bodyBytes, 0, bodyBytes.Length);
                requestStream.Close();
            }

            //回应
            string content;
            int code;
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                code = (int)response.StatusCode;
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    content = sr.ReadToEnd();
                }
            }

            //RESTFulInvokeExceptionInfo exceptionInfo;
            //if (code < 200 || code >= 300)
            //{
            //    if (string.IsNullOrWhiteSpace(content))
            //        exceptionInfo = new RESTFulInvokeExceptionInfo { type = "100001", message = "操作失败" };
            //    exceptionInfo = Serializer.Deserialize<RESTFulInvokeExceptionInfo>(content);
            //    if (exceptionInfo == null)
            //        exceptionInfo = new RESTFulInvokeExceptionInfo { type = "100002", message = "操作失败", messageDetail = content };
            //    exceptionInfo.url = url;
            //    exceptionInfo.code = code;

            //    throw new RESTFulInvokeException(exceptionInfo);
            //}

            if (string.IsNullOrWhiteSpace(content))
                return null;
            
            var resObj = JsonConvert.DeserializeObject(content, targetMethod.ReturnType);
            return resObj;
        }

        private string AnalyzeUri(MethodInfo targetMethod)
        {
            return "http://localhost:31762/api/X-MAX-Rpc-Http-Sample-Service-IFooService-Add";
        }
    }
}
