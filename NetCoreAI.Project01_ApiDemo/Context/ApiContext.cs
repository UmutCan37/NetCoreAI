using Microsoft.EntityFrameworkCore;
using NetCoreAI.Project1_ApiDemo.Entities;

namespace NetCoreAI.Project1_ApiDemo.Context
{
    public class ApiContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=UMUT\\MSSQLSERVER01;initial Catalog=ApiAIDb;integrated Security=true;trust server certificate=true");
        }

        public DbSet<Customer> Customers { get; set; }
    }
}
