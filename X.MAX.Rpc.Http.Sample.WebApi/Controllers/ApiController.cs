using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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
        [HttpPost]
        public object Post([FromBody]RpcRequest request)
        {
            return ServerManager.Invoke(request);
        }
    }
}
