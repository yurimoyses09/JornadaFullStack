using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Responses;
using System.Net.Http.Json;

namespace Fina.Web.Handlers;

public class CategoryHandler(IHttpClientFactory httpClientFactory) : ICategoryHandler
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(WebConfiguration.HttpClientName);

    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
    {
        try
        {
            var result = await _httpClient.PostAsJsonAsync("v1/categories",  request);

            return await result.Content.ReadFromJsonAsync<Response<Category?>>() ?? new Response<Category?>();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
    {
        try
        {
            var result = await _httpClient.DeleteAsync($"v1/categories/{request.Id}");

            return await result.Content.ReadFromJsonAsync<Response<Category?>>() ?? new Response<Category?>();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<PagedResponse<List<Category>?>> GetAllAsync(GetAllCategoryRequest request)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<PagedResponse<List<Category>?>>($"v1/categories/");
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Response<Category?>> GetById(GetCategoryByIdRequest request)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<Response<Category?>>($"v1/categories/{ request.Id}");
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        try
        {
            var result = await _httpClient.PutAsJsonAsync($"v1/categories/{request.Id}", request);

            return await result.Content.ReadFromJsonAsync<Response<Category?>>() ?? new Response<Category?>();
        }
        catch (Exception)
        {
            throw;
        }
    }
}
