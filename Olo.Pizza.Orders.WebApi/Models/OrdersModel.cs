using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Olo.Pizza.Orders.WebApi.Models
{
    public class OrdersModel
    {
        public Pizza[] Pizzas { get; set; }

        public class Pizza
        {
            public string[] toppings { get; set; }

            public override string ToString()
            {
                //NOTE: this string will uniquely identify specific pizza orders

                var orderedToppings = toppings.OrderBy(topping => topping);

                return string.Join("-", orderedToppings) ?? base.ToString();
            }
        }
    }
}
