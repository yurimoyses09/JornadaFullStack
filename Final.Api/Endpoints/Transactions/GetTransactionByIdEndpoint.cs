using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;
using Final.Api.Common.Api;

namespace Final.Api.Endpoints.Transactions;

public class GetTransactionByIdEndpoint : IEndPoint
{
    public static void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/{id}", HandleAsync)
            .WithName("Transaction: Get by Id")
            .WithSummary("Recupera transação por id")
            .WithDescription("Recupera transação por id")
            .WithOrder(4)
            .Produces<Response<Transaction?>>();
    }

    private static async Task<IResult> HandleAsync(ITransactionHandler handler, int id)
    {
        var request = new GetTransactionByIdRequest() { Id = id, UserId = ApiConfiguration.UserId };

        var response = await handler.GetById(request);

        return response.IsSuccess ? TypedResults.Ok(response) : TypedResults.BadRequest(response);
    }
}
