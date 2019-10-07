using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayrollProcessor
{
    public class HttpClient
    {
        public string GetAsJson(string url)
        {
            if (url.Contains("tax"))
            {
                return "{'TaxRate':0.2}";
            }
            else
            {
                return "{'Health':289.50, 'Life':'24.00'}";
            }
        }
    }
}
