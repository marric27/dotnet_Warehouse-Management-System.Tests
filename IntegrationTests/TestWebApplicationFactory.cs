using dotnet_Warehouse_Management_System.Common;
using dotnet_Warehouse_Management_System.Data;
using dotnet_Warehouse_Management_System.GoodsIn.Entities;
using dotnet_Warehouse_Management_System.Products.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace dotnet_Warehouse_Management_System.Tests.IntegrationTests
{
    public class TestWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.Remove(
                    services.SingleOrDefault(d => d.ServiceType == typeof(IDbContextOptionsConfiguration<ApplicationDBContext>))
                );

                // Aggiungi DbContext in-memory
                services.AddDbContext<ApplicationDBContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                // Opzionale: popola il DB con dati di test
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
                    db.Database.EnsureCreated();

                    db.Products.Add(new Product { Code = "TestCode", Name = "Test Product", Category = Common.Category.STANDARD });
                    db.Grns.Add(new Grn { Code = "GRN-001", Supplier = "Test Supplier", State = State.OPEN, ReceivingDate = DateTime.Now, Items = [] });
                    db.SaveChanges();
                }
            });
        }
    }
}