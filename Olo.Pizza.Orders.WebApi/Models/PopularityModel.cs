using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Olo.Pizza.Orders.WebApi.Models
{
    public class PopularityModel
    {
        public int Rank { get; set; }

        public int OrderCount { get; set; }

        public string[] Toppings { get; set; }
    }
}
