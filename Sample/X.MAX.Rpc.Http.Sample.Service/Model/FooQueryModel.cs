using System;
using System.Collections.Generic;
using System.Text;

namespace X.MAX.Rpc.Http.Sample.Service.Model
{
    public class FooQueryModel
    {
        public string Name { get; set; }
        public decimal? MoneyMin { get; set; }
        public decimal? MoneyMax { get; set; }
    }
}
