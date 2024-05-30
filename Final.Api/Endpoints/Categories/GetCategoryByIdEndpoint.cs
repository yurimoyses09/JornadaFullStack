using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Responses;
using Final.Api.Common.Api;

namespace Final.Api.Endpoints.Categories
{
    public class GetCategoryByIdEndpoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder builder)
        {
            builder.MapGet("/{id}", HandleAsync)
                .WithName("Category: Get by Id")
                .WithSummary("Recupera categoria por id")
                .WithDescription("Recupera categoria por id")
                .WithOrder(4)
                .Produces<Response<Category>?>();
        }

        private static async Task<IResult> HandleAsync(ICategoryHandler handler, int id)
        {
            var request = new GetCategoryByIdRequest() { Id = id, UserId = ApiConfiguration.UserId };

            var response = await handler.GetById(request);

            return response.IsSuccess ? TypedResults.Ok(response) : TypedResults.BadRequest(response);
        }
    }
}