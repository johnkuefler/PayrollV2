using System.Net.Http;
using Newtonsoft.Json;
using Payroll.Data.Models;
using PayrollProcessor.Models;

namespace PayrollProcessor
{
    public class OracleHRService : IHRService
    {
        private HttpClient _httpClient;
        public OracleHRService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public InsuranceAmounts GetInsuranceAmounts(Employee employee)
        {
            string insuranceJson =
                _httpClient.GetStringAsync(
                    $"https://test.mocktopus.dev-squared.com/payroll/api/benefits?id={employee.Id}").Result;

            InsuranceAmounts insuranceAmounts = JsonConvert.DeserializeObject<InsuranceAmounts>(insuranceJson);
            return insuranceAmounts;
        }

        public  TaxBracket GetTaxBracket(double salary, Employee employee)
        {
            string taxRateJson =
                _httpClient.GetStringAsync(
                        $"https://test.mocktopus.dev-squared.com/payroll/api/taxrate?salary={salary}&zip={employee.ZipCode}")
                    .Result;

            TaxBracket taxBracket = JsonConvert.DeserializeObject<TaxBracket>(taxRateJson);
            return taxBracket;
        }
    }
}