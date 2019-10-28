using System.Net.Http;
using Newtonsoft.Json;
using PayrollProcessor.Models;

namespace PayrollProcessor
{
    public class OracleHrService : IHrService
    {
        private HttpClient _httpClient;

        public OracleHrService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public MealDeduction GetDiningDeductionForEmployee(string employeeId)
        {
            string mealJson =
                _httpClient.GetStringAsync(
                    $"https://payroll.getsandbox.com/api/v1/meal-deduction?id={employeeId}").Result;

            MealDeduction mealDeduction = JsonConvert.DeserializeObject<MealDeduction>(mealJson);

            return mealDeduction;
        }
    }
}