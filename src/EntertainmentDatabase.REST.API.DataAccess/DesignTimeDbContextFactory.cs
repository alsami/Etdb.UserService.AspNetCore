//using Microsoft.AspNetCore.Hosting;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace EntertainmentDatabase.REST.API.DataAccess
//{
//    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EntertainmentDatabaseContext>
//    {
//        private readonly IConfigurationRoot configurationRoot;
//        private readonly IHostingEnvironment hostingEnvironment;

//        public DesignTimeDbContextFactory(IConfigurationRoot configurationRoot, IHostingEnvironment hostingEnvironment)
//        {
//            this.configurationRoot = configurationRoot;
//            this.hostingEnvironment = hostingEnvironment;
//        }

//        public EntertainmentDatabaseContext CreateDbContext(string[] args)
//        {
//            var builder = new DbContextOptionsBuilder<EntertainmentDatabaseContext>();
//            builder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EntertainmentDatabaseDEV;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

//            return new EntertainmentDatabaseContext(builder.Options);
//        }
//    }
//}
