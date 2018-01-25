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
        public int TimeoutMillisecond { get; set; } = 60000;

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            //请求
            string url = AnalyzeUri(targetMethod);
            var request = WebRequest.CreateHttp(url);
            request.Timeout = TimeoutMillisecond;
            request.Method = "POST";
            request.Accept = "application/json";

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

            if (code < 200 || code >= 300)
                throw new RpcInvokeException(content, code);

            if (string.IsNullOrWhiteSpace(content))
                throw new RpcInvokeException("no response content", 99);

            var resObj = JsonConvert.DeserializeObject(content, targetMethod.ReturnType);
            return resObj;
        }

        private string AnalyzeUri(MethodInfo targetMethod)
        {
            if (string.IsNullOrWhiteSpace(ServiceUrl))
                throw new ArgumentNullException("ServiceUrl");
            var fullName = targetMethod.DeclaringType.FullName.Replace('.', '-') + ('-') + targetMethod.Name;
            return ServiceUrl.TrimEnd('/') + '/' + fullName;
        }
    }
}
