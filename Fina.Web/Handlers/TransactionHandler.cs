using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;
using System.Net.Http.Json;

namespace Fina.Web.Handlers
{
    public class TransactionHandler(IHttpClientFactory httpClientFactory) : ITransactionHandler
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient(WebConfiguration.HttpClientName);
        public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
        {
            try
            {
                var result = await _httpClient.PostAsJsonAsync("v1/transaction", request);

                return await result.Content.ReadFromJsonAsync<Response<Transaction?>>() ?? new Response<Transaction?>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Response<Transaction?>> DeleteAsync(DeleteTransacrionRequest request)
        {
            try
            {
                var result = await _httpClient.DeleteAsync($"v1/transaction/{request.Id}");

                return await result.Content.ReadFromJsonAsync<Response<Transaction?>>() ?? new Response<Transaction?>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Response<Transaction?>> GetById(GetTransactionByIdRequest request)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Response<Transaction?>>($"v1/transaction/{request.Id}");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransactionByPeriodRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
        {
            try
            {
                var result = await _httpClient.PutAsJsonAsync($"v1/transaction{request.Id}", request);

                return await result.Content.ReadFromJsonAsync<Response<Transaction?>>() ?? new Response<Transaction?>();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
