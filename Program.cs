using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using shoppinglist.Models;
using shoppinglist.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shoppinglist
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateDb();
            CreateHostBuilder(args).Build().Run();
           



        }


        public static void CreateDb()
        {

            using (var context = new ShoppingListContext())
            {
                context.Database.EnsureCreated();
                //var person1 = new Person() { Name = "ali", Age = 20 };

                //context.persons.Add(person1);
                //context.SaveChanges();

            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
