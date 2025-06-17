using Microsoft.AspNetCore.Http.HttpResults;
using WorkOrderService.Interfaces;

namespace WorkOrderService.Repositories
{
    public class ReportClient : IReportClient
    {
        private readonly HttpClient _httpClient;

        public ReportClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> CreateCSV(ImportResult importResult)
        {
            try
            {
                var result = await _httpClient.PostAsJsonAsync("api/report/log", importResult);
                return result;
            }
            catch (Exception ex)
            {
                var response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                {
                    Content = new StringContent($"Report failed: {ex.Message}")
                };
                return response;
            }

        }
    }
}
