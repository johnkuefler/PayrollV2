using Payroll.Data.Models;
using PayrollProcessor.Models;

namespace PayrollProcessor
{
    public class PayrollCalculator : IPayrollCalculator
    {
        public PayrollCalculator()
        {

        }

        public EmployeePayroll Calculate(Employee employee, TaxBracket taxBracket, InsuranceAmounts insuranceAmounts, TimeCard timeCard)
        {
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

            return  new EmployeePayroll
            {
                NetPay =  netPay,
                BasePay =  basePay
            };
        }
    }
}