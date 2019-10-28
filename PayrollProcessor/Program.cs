using Payroll.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payroll.Data.Models;
using PayrollProcessor.Models;
using Newtonsoft.Json;

namespace PayrollProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            #region setup context config
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true);

            IConfigurationRoot configuration = builder.Build();

            DbContextOptionsBuilder<PayrollDbContext> optionsBuilder = new DbContextOptionsBuilder<PayrollDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            #endregion

            PayrollDbContext context = new PayrollDbContext(optionsBuilder.Options);

            HttpClient httpClient = new HttpClient();

            DateTime startDateTime = Convert.ToDateTime("10/6/2019");
            DateTime endDateTime = Convert.ToDateTime("10/20/2019");

            IPayrollService payrollService = new SqlPayrollService(context);
            IHRService hrService = new OracleHRService(httpClient);

            PayrollProcessor processor = new PayrollProcessor(payrollService, hrService);

            processor.ProcessPayroll(startDateTime, endDateTime);
        }
    }
}
