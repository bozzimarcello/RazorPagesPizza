using RazorPagesPizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace RazorPagesPizza.Services
{
    public static class PizzaService
    {
        static List<Pizza> Pizzas { get; }
        static int nextId = 3;
        static PizzaService()
        {
            /*
            Pizzas = new List<Pizza>
            {
                new Pizza { Id = 1, Name = "Classic Italian", Price=20.00M, Size=PizzaSize.Large, IsGlutenFree = false },
                new Pizza { Id = 2, Name = "Veggie", Price=15.00M, Size=PizzaSize.Small, IsGlutenFree = true }
            };
            */
            Pizzas = new List<Pizza>();

            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DBPizzeria;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                String sql = "SELECT * FROM TBPizza";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Pizzas.Add(new Pizza { Id = reader.GetInt32(0),
                                Name = reader.GetString(1), 
                                Size = (PizzaSize) reader.GetInt16(2),
                                IsGlutenFree = reader.GetBoolean(3), 
                                Price = reader.GetDecimal(4)
                            });
                        }
                    }
                }
            }
        }

        public static List<Pizza> GetAll() => Pizzas;

        public static Pizza Get(int id) => Pizzas.FirstOrDefault(p => p.Id == id);

        public static void Add(Pizza pizza)
        {
            pizza.Id = nextId++;
            Pizzas.Add(pizza);
        }

        public static void Delete(int id)
        {
            var pizza = Get(id);
            if (pizza is null)
                return;

            Pizzas.Remove(pizza);
        }

        public static void Update(Pizza pizza)
        {
            var index = Pizzas.FindIndex(p => p.Id == pizza.Id);
            if (index == -1)
                return;

            Pizzas[index] = pizza;

        }
    }
}