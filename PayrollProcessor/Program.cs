using Payroll.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Payroll.Data.Models;
using PayrollProcessor.Models;
using Newtonsoft.Json;

namespace PayrollProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            string payFileOutput = "";

            DateTime startDateTime = Convert.ToDateTime("10/7/2019");
            DateTime endDateTime = Convert.ToDateTime("10/11/2019");

            PayrollDbContext context = new PayrollDbContext();

            List<TimeCard> timeCards = new List<TimeCard>();
            timeCards = context.TimeCards
                .Where(x => x.StartDateTime >= startDateTime && x.EndDateDateTime <= endDateTime).ToList();

            foreach (var timeCard in timeCards)
            {
                Employee employee = context.Employees.FirstOrDefault(x => x.Id == timeCard.EmployeeId);

                double salary = employee.HourlyRate * 40 * 52;

                FakeHttpClient httpClient = new FakeHttpClient();

                string taxRateJson =
                    httpClient.GetAsJson(
                        $"https://fakeapi.com/api/tax?salary={salary}&zip={employee.ZipCode}");

                TaxBracket taxBracket = JsonConvert.DeserializeObject<TaxBracket>(taxRateJson);

                string insuranceJson =
                    httpClient.GetAsJson(
                        $"https://fakeapi.com/api/insurance?id={employee.Id}");

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
                    basePay += insuranceAmounts.Health;
                    basePay += insuranceAmounts.Life;
                }

                netPay = basePay * (1 - taxBracket.TaxRate);

                if (!employee.InsuranceBeforeTaxes)
                {
                    netPay += insuranceAmounts.Health;
                    netPay += insuranceAmounts.Life;
                }

                payFileOutput +=
                    $"{employee.Id}|${timeCard.TotalHours}|${basePay}|${netPay}|${insuranceAmounts.Health}|${insuranceAmounts.Life}|{taxBracket.TaxRate}\r\n";
            }


            Console.WriteLine(payFileOutput);
        }
    }
}
