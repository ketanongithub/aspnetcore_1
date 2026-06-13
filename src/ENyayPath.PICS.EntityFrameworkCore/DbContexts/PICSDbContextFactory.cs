using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.EntityFrameworkCore.DbContexts
{
    //public class PICSDbContextFactory : IDesignTimeDbContextFactory<PICSDbContext>
    //{
    //    public PICSDbContext CreateDbContext(string[] args)
    //    {
    //        var optionsBuilder = new DbContextOptionsBuilder<PICSDbContext>();
    //        optionsBuilder.UseSqlServer("Server=.;Database=ENyayPathPICS;Trusted_Connection=True;");
    //        return new PICSDbContext(optionsBuilder.Options);
    //    }
    //}

    public class PICSDbContextFactory : IDesignTimeDbContextFactory<PICSDbContext>
    {
        public PICSDbContext CreateDbContext(string[] args)
        {
            // Load configuration from appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var connectionString = config.GetConnectionString("Default");

            var optionsBuilder = new DbContextOptionsBuilder<PICSDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new PICSDbContext(optionsBuilder.Options);
        }
    }
}
