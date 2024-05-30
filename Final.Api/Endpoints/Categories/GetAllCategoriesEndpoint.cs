using Fina.Core;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Responses;
using Final.Api.Common.Api;
using Microsoft.AspNetCore.Mvc;

namespace Final.Api.Endpoints.Categories
{
    public class GetAllCategoriesEndpoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder builder)
        {
            builder.MapGet("/", HandleAsync)
                .WithName("GetAll: Get All Categories")
                .WithSummary("Recupera todas categorias")
                .WithDescription("Recupera todas categorias")
                .WithOrder(5)
                .Produces<PagedResponse<List<Category?>>>();
        }

        private static async Task<IResult> HandleAsync(
            ICategoryHandler handler, 
            [FromQuery] int pageNumber = Configuration.DefaultPageNumber, 
            [FromQuery] int pageSize = Configuration.DefaultPageSize)
        {
            var request = new GetAllCategoryRequest(){ PageNumber = pageNumber, PageSize = pageSize };

            var response = await handler.GetAllAsync(request);

            return response.IsSuccess ? TypedResults.Ok(response) : TypedResults.BadRequest(response);
        }
    }
}
