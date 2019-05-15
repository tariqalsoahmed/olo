using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Olo.Pizza.Orders.WebApi.Models;

namespace Olo.Pizza.Orders.WebApi.Services
{
    public class PopularityService : IPopularityService
    {
        public static Uri PizzasEndpoint = new Uri("http://files.olo.com/pizzas.json");

        public async Task<PopularityModel[]> Query(int top)
        {
            var orders = await this.GetOrders();

            var popularity = this.Rank(orders);

            return popularity.Take(top).ToArray();
        }

        private PopularityModel[] Rank(OrdersModel orders)
        {
            var popularity = new Dictionary<string, PopularityModel>();

            this.Rank(orders, ref popularity);

            return popularity.Values.OrderBy(p => p.Rank).ToArray();
        }

        private void Rank(OrdersModel orders, ref Dictionary<string, PopularityModel> popularity)
        {
            var rank = 1;

            foreach(var pizza in orders.Pizzas)
            {
                var pizzaKey = pizza.ToString();

                this.Build(pizza, pizzaKey, ref popularity);
            }

            foreach(var model in popularity.Values.OrderByDescending(p => p.OrderCount))
            {
                model.Rank = rank++;
            }
        }

        private void Build(OrdersModel.Pizza pizza, string pizzaKey, ref Dictionary<string, PopularityModel> popularity)
        {
            if (popularity.ContainsKey(pizzaKey))
            {
                popularity[pizzaKey].OrderCount += 1;
            }
            else
            {
                var popularityModel = new PopularityModel()
                {
                    Toppings = pizza.toppings,
                    OrderCount = 1
                };

                popularity[pizzaKey] = popularityModel;
            }
        }

        private async Task<OrdersModel> GetOrders()
        {
            using(var httpClient = new HttpClient())
            {
                var ordersModel = new OrdersModel();

                var ordersResponse = await httpClient.GetAsync(PizzasEndpoint);

                var ordersString = await ordersResponse.Content.ReadAsStringAsync();

                ordersModel.Pizzas = JsonConvert.DeserializeObject<OrdersModel.Pizza[]>(ordersString);

                return ordersModel;
            }
        }
    }

    public interface IPopularityService
    {
        Task<PopularityModel[]> Query(int top);
    }
}
