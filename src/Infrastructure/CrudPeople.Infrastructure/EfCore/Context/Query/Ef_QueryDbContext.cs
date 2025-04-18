﻿
using CrudPeople.CoreDomain.Entities;
using CrudPeople.CoreDomain.Entities.People.Query;
using CrudPeople.Infrastructure.EfCore.Context.ModelCreating;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;

namespace CrudPeople.Infrastructure.EfCore.Context.Query
{
    public class Ef_QueryDbContext(DbContextOptions<Ef_QueryDbContext> options, IConfiguration configuration, ILoggerFactory logger) : DbContext(options)
    {
        private readonly string _connectionString = configuration.GetSection("Connections:SqlQuery").Value.ToString();
        private readonly IConfiguration _configuration = configuration;
        public TransactionStatus TransactionSatatus { get; set; }
        public Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction Transaction { get; set; }
        private readonly ILoggerFactory _logger = logger;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.MainQueryConfig(_configuration);

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       => optionsBuilder.UseSqlServer(_connectionString);
         
        public DbSet<PeopleQueryEntity> People { get; set; }
    }
}
