using Final.Api.Common.Api;
using Final.Api.Endpoints.Categories;
using Final.Api.Endpoints.Transactions;

namespace Final.Api.Endpoints
{
    public static class Enpoint
    {
        public static void MapEnpoints(this WebApplication web)
        {
            var endpoints = web.MapGroup("");

            endpoints.MapGroup("/")
                .WithTags("Healh Check")
                .MapGet("/", () => new { message = "OK" });

            endpoints.MapGroup("/v1/categories")
                .WithTags("Categories")
                .MapEndpoint<CreateCategoryEndpoint>()
                .MapEndpoint<DeleteCategoryEndpoint>()
                .MapEndpoint<GetAllCategoriesEndpoint>()
                .MapEndpoint<GetCategoryByIdEndpoint>()
                .MapEndpoint<UpdateCategoryEndpoint>()
                ;

            endpoints.MapGroup("/v1/transactions")
                .WithTags("Transactions")
                .MapEndpoint<CreateTransactionEndpoint>()
                .MapEndpoint<DeleteTransactionEndpoint>()
                .MapEndpoint<GetTransactionsByPeriodEndpoint>()
                .MapEndpoint<GetTransactionByIdEndpoint>()
                .MapEndpoint<UpdateTransactionEndpoint>()
                ;
        }

        private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app) where TEndpoint : IEndPoint
        {
            TEndpoint.Map(app);

            return app;
        }
    }
}
