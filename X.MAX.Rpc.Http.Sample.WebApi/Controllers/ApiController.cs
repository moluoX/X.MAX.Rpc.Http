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
        // POST api/values
        [HttpPost]
        public object Post(RpcRequest request)
        {
            return ServerManager.Invoke(request);
        }
    }
}
