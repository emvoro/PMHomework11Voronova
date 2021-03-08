using CurrencyConverterTestApp.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CurrencyConverterTestApp.Tests
{
    public class Testing
    {
        private readonly HttpClient _httpClient;

        public Testing(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task TestCurrencies(string fromCurrency, string toCurrency, int amount, int expectedCode)
        {
            string request = $"rates/{fromCurrency}/{toCurrency}?amount={amount}";
            var response = await _httpClient.GetAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            content = expectedCode == 200 ? content + " " + toCurrency : content;
            ConsoleWriteResponse(request, expectedCode, (int)response.StatusCode, content);
        }

        public async Task TestRegistration()
        {
            string request = $"auth";
            var response = await _httpClient.PostAsync(request, new StringContent(JsonSerializer.Serialize(new User("emilia", "voronova")), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            ConsoleWriteResponse(request, 200, (int)response.StatusCode, content);
        }

        public void ConsoleWriteResponse(string request, int expectedStatusCode, int actualStatusCode, string content)
        {
            Console.WriteLine($"\n Request              : {request}");
            Console.WriteLine($" Expected Status Code : {expectedStatusCode}");
            Console.WriteLine($" Actual Status Code   : {actualStatusCode}");
            Console.WriteLine($" Response             : {content} ");

            WriteResult(expectedStatusCode == actualStatusCode);
        }

        public void WriteResult(bool isSucceeded)
        {
            Console.ForegroundColor = isSucceeded ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine(isSucceeded ? " SUCCEEDED" : " FAILED");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
