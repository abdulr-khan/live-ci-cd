using AnimalCountingDatabase.Api;
using AnimalCountingDatabase.Api.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AnimalCountingDatabase.Test
{
    public class DemoTest
    {
        [Fact]
        public void Test1()
        {
            Assert.True(1 == 1);
        }

        [Fact]
        public async Task CustomerIntegrationTest()
        {
            //Create DB Context
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables() //This call will make sure we can overwrite config settings from json file
                                           //Explicit call here, it is the default behavior in WebApi projects
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<CustomerContext>();
            optionsBuilder
                // Uncomment the following line if you want to print generated
                // SQL statements on the console.
                // .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                .UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);

            var context = new CustomerContext(optionsBuilder.Options);


            //Just to make sure: Delete all existing records in Customer Table
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            //The two lines above will make sure that we always start fresh
            //so we can remove it
            //context.Customers.RemoveRange(await context.Customers.ToArrayAsync());
            //await context.SaveChangesAsync();

            //Create Controller
            var controller = new CustomerController(context);

            //Add Customer
            await controller.Add(new Customer() { Name = "FooTest" });


            //checked: Does GetAll return the added customer
            var result = (await controller.GetAll()).ToArray();
            Assert.Single(result);
            Assert.Equal("FooTest", result[0].Name);
        }
    }
}
