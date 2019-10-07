using System;
using Microsoft.EntityFrameworkCore;
using Payroll.Data.Models;

namespace Payroll.Data
{
    public class PayrollDbContext : DbContext
    {
        private readonly string _connectionString;

        public PayrollDbContext()
        {
            _connectionString = "some connection string";
        }

        public PayrollDbContext(DbContextOptions<PayrollDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrEmpty(_connectionString))
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }

            if (optionsBuilder.IsConfigured == false)
            {
                base.OnConfiguring(optionsBuilder);
            }
        }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<TimeCard> TimeCards { get; set; }
    }
}
