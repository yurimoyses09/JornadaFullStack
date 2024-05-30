using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Responses;
using Final.Api.Common.Api;

namespace Final.Api.Endpoints.Categories
{
    public class DeleteCategoryEndpoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder builder)
        {
            builder.MapDelete("/{id}", HandleAsync)
                .WithName("Delete: Category")
                .WithSummary("Exclui uma categoria")
                .WithDescription("Exclui uma categoria")
                .WithOrder(3)
                .Produces<Response<Category?>>();
        }

        private static async Task<IResult> HandleAsync(ICategoryHandler handler, long id)
        {
            var request = new DeleteCategoryRequest()
            {
                Id = id,
                UserId = ApiConfiguration.UserId
            };

            var response = await handler.DeleteAsync(request);

            return response.IsSuccess ? TypedResults.Ok(response) : TypedResults.BadRequest(response);
        }
    }
}
