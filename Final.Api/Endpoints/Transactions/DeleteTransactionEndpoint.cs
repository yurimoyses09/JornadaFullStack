using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;
using Final.Api.Common.Api;

namespace Final.Api.Endpoints.Transactions;

public class DeleteTransactionEndpoint : IEndPoint
{
    public static void Map(IEndpointRouteBuilder builder)
    {
        builder.MapDelete("/{id}", HandleAsync)
            .WithName("Delete: Transaction")
            .WithSummary("Exclui uma transação")
            .WithDescription("Exclui uma transação")
            .WithOrder(2)
            .Produces<Response<Transaction?>>();
    }

    private static async Task<IResult> HandleAsync(ITransactionHandler handler, long id)
    {
        var request = new DeleteTransacrionRequest()
        {
            Id = id,
            UserId = ApiConfiguration.UserId
        };

        var response = await handler.DeleteAsync(request);

        return response.IsSuccess ? TypedResults.Ok(response) : TypedResults.BadRequest(response);
    }
}
