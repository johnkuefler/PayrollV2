﻿using Payroll.Data;
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

            string payFileOutput = "Employee Id|Total Hours|Health Amount|Life Amount|Tax Rate|Base Pay|Net Pay\r\n";

            DateTime startDateTime = Convert.ToDateTime("10/6/2019");
            DateTime endDateTime = Convert.ToDateTime("10/20/2019");

            // get employee timecards from database
            List<TimeCard> timeCards = new List<TimeCard>();
            timeCards = context.TimeCards
                .Where(x => x.StartDateTime >= startDateTime && x.EndDateDateTime <= endDateTime).ToList();

            foreach (var timeCard in timeCards)
            {
                // fetch details of employee for timecard from database
                Employee employee = context.Employees.FirstOrDefault(x => x.Id == timeCard.EmployeeId);

                double salary = employee.HourlyRate * 40 * 52;

                HttpClient httpClient = new HttpClient();

                // get the employee's taxrate from HR system API
                string taxRateJson =
                    httpClient.GetStringAsync(
                        $"https://test.mocktopus.dev-squared.com/payroll/api/taxrate?salary={salary}&zip={employee.ZipCode}").Result;

                TaxBracket taxBracket = JsonConvert.DeserializeObject<TaxBracket>(taxRateJson);

                // get the employee's insurance deductions from HR system API
                string insuranceJson =
                    httpClient.GetStringAsync(
                        $"https://test.mocktopus.dev-squared.com/payroll/api/benefits?id={employee.Id}").Result;

                InsuranceAmounts insuranceAmounts = JsonConvert.DeserializeObject<InsuranceAmounts>(insuranceJson);

                // calculate net payroll amount
                double netPay = 0;
                double basePay = timeCard.TotalHours * employee.HourlyRate;
                if (employee.Seniority)
                {
                    basePay += 100;
                }

                if (employee.InsuranceBeforeTaxes)
                {
                    netPay = basePay;
                    netPay -= insuranceAmounts.Health;
                    netPay -= insuranceAmounts.Life;
                    netPay = netPay * (1 - taxBracket.TaxRate);
                }
                else
                {
                    netPay = basePay * (1 - taxBracket.TaxRate);
                    netPay -= insuranceAmounts.Health;
                    netPay -= insuranceAmounts.Life;
                }

                // output to desired format
                payFileOutput +=
                    $"{employee.Id}|{timeCard.TotalHours}|${insuranceAmounts.Health}|${insuranceAmounts.Life}|{taxBracket.TaxRate}|${basePay}|${netPay}\r\n";
            }

            
            Console.WriteLine(payFileOutput);

            // send output file wherever needed

            Console.ReadLine();
        }
    }
}
