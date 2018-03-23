using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;

namespace X.MAX.Rpc.Http.Sample.WebApi.Controllers
{
    [Route("[controller]")]
    public class ApiController : Controller
    {
        // GET api
        [HttpGet]
        public string Get()
        {
            return "ok";
        }

        // POST api
        [HttpPost("{uri}")]
        public string Post(string uri)
        {
            string arg;
            using (StreamReader sr = new StreamReader(Request.Body, Encoding.UTF8))
            {
                arg = sr.ReadToEnd();
            }
            return RpcServer.Invoke(uri, arg);
        }
    }
}
