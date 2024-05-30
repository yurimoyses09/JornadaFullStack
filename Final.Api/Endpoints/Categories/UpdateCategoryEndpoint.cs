using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Responses;
using Final.Api.Common.Api;

namespace Final.Api.Endpoints.Categories
{
    public class UpdateCategoryEndpoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder builder)
        {
            builder.MapPut("/{id}", HandleAsync)
                .WithName("Update: Category")
                .WithSummary("Atualiza uma categoria")
                .WithDescription("Atualiza uma categoria")
                .WithOrder(2)
                .Produces<Response<Category>?>();
        }

        private static async Task<IResult> HandleAsync(ICategoryHandler handler, int id)
        {
            var request = new UpdateCategoryRequest() { Id = id, UserId = ApiConfiguration.UserId };

            var response = await handler.UpdateAsync(request);

            return response.IsSuccess ? TypedResults.Ok(response) : TypedResults.BadRequest(response);
        }
    }
}
