using System;
using System.Net.Http;
using System.Text;
using PayrollProcessor.Models;

namespace PayrollProcessor
{
    public class PayrollProcessor
    {
        private IPayrollService _payrollService;
        private IHRService _hrService;

        public PayrollProcessor(IPayrollService payrollService, IHRService hrService)
        {
            _payrollService = payrollService;
            _hrService = hrService;
        }

        public void ProcessPayroll(DateTime startDateTime, DateTime endDateTime)
        {
            string payFileOutput = "Employee Id|Total Hours|Health Amount|Life Amount|Tax Rate|Base Pay|Net Pay\r\n";

            var timeCards = _payrollService.GetTimeCardsByDate(startDateTime, endDateTime);

            foreach (var timeCard in timeCards)
            {
                var employee = _payrollService.GetEmployeeByTimeCard(timeCard);

                double salary = employee.HourlyRate * 40 * 52;

                var taxBracket = _hrService.GetTaxBracket(salary, employee);

                var insuranceAmounts = _hrService.GetInsuranceAmounts(employee);

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


                payFileOutput +=
                    $"{employee.Id}|{timeCard.TotalHours}|${insuranceAmounts.Health}|${insuranceAmounts.Life}|{taxBracket.TaxRate}|${basePay}|${netPay}\r\n";
            }


            Console.WriteLine(payFileOutput);

            // send 

            Console.ReadLine();
        }

    
    }
}
