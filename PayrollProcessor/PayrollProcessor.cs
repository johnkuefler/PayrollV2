using System;
using System.Net.Http;
using System.Text;

namespace PayrollProcessor
{
    public class PayrollProcessor
    {
        private IPayrollService _payrollService;
        private IHRService _hrService;
        private IPayrollCalculator _payrollCalculator;

        public PayrollProcessor(IPayrollService payrollService, IHRService hrService, IPayrollCalculator payrollCalculator)
        {
            _payrollService = payrollService;
            _hrService = hrService;
            _payrollCalculator = payrollCalculator;
        }

        public void ProcessPayroll(DateTime startDateTime, DateTime endDateTime)
        {
            string payFileOutput = "Employee Id|Total Hours|Health Amount|Life Amount|Tax Rate|Base Pay|Net Pay\r\n";

            var timeCards = _payrollService.GetTimeCardsByDate(startDateTime, endDateTime);

            foreach (var timeCard in timeCards)
            {
                var employee = _payrollService.GetEmployeeByTimeCard(timeCard);

                var insuranceAmounts = _hrService.GetInsuranceAmounts(employee);

                var taxBracket = _hrService.GetTaxBracket(employee);

                var payroll = _payrollCalculator.Calculate(employee, taxBracket, insuranceAmounts, timeCard);

                payFileOutput +=
                    $"{employee.Id}|{timeCard.TotalHours}|${insuranceAmounts.Health}|${insuranceAmounts.Life}|{taxBracket.TaxRate}|${payroll.BasePay}|${payroll.NetPay}\r\n";
            }

            Console.WriteLine(payFileOutput);

            // send 

            Console.ReadLine();
        }
    }
}
