using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;
using Final.Api.Common.Api;

namespace Final.Api.Endpoints.Transactions;

public class UpdateTransactionEndpoint : IEndPoint
{
    public static void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPut("/{id}", HandleAsync)
            .WithName("Update: Transaction")
            .WithSummary("Atualiza uma transação")
            .WithDescription("Atualiza uma transação")
            .WithOrder(3)
            .Produces<Response<Transaction>?>();
    }

    private static async Task<IResult> HandleAsync(ITransactionHandler handler, int id)
    {
        var request = new UpdateTransactionRequest() { Id = id, UserId = ApiConfiguration.UserId };

        var response = await handler.UpdateAsync(request);

        return response.IsSuccess ? TypedResults.Ok(response) : TypedResults.BadRequest(response);
    }
}
