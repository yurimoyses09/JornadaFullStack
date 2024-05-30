using Fina.Core;
using Fina.Core.Handlers;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;
using Final.Api.Common.Api;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace Final.Api.Endpoints.Transactions;

public class GetTransactionsByPeriodEndpoint : IEndPoint
{
    public static void Map(IEndpointRouteBuilder builder)
    { 
        builder.MapGet("/", HandleAsync)
            .WithName("Transaction: Get all")
            .WithSummary("Recupera todas as transações")
            .WithDescription("Recupera todas as transações")
            .WithOrder(5)
            .Produces<PagedResponse<List<Transaction?>>>();
    }

    private static async Task<IResult> HandleAsync(
        ITransactionHandler handler, 
        [FromQuery] DateTime? startDate = null, 
        [FromQuery] DateTime? endDate = null,
        [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
        [FromQuery] int pageSize = Configuration.DefaultPageSize)
    {
        var request = new GetTransactionByPeriodRequest()
        {
            EndDate = endDate,
            PageNumber = pageNumber,
            PageSize = pageSize,
            StartDate = startDate,
            UserId = ApiConfiguration.UserId
        };

        var response = await handler.GetByPeriodAsync(request);

        return response.IsSuccess ? TypedResults.Ok(response) : TypedResults.BadRequest(response);
    }
}
