using Fina.Core.Common;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;
using Final.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Final.Api.Handlers;

public class TransactionHandler(AppDbContext context) : ITransactionHandler
{
    public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        try
        {
            if (request is { Type: Fina.Core.Enums.ETransactionType.Withdraw, Amount: >= 0 })
                request.Amount *= -1;

            var transaction = new Transaction
            {
                Amount = request.Amount,
                Type = request.Type,
                CategoryId = request.CategoryId,
                CreateAt = DateTime.Now,
                PaidOrReceivedAt = request.PaidOrReceiveAt,
                Title = request.Title,
                UserId = request.UserId
            };

            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();

            return new Response<Transaction?>(transaction, 201, "Transação criada com sucesso");
        }
        catch (Exception ex)
        {
            return new Response<Transaction?>(null, 500, ex.Message);
        }
    }

    public async Task<Response<Transaction?>> DeleteAsync(DeleteTransacrionRequest request)
    {
        try
        {
            var transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (transaction is null)
                return new Response<Transaction?>(null, 404, "Transação nao foi encontrada");

            context.Transactions.Remove(transaction);
            await context.SaveChangesAsync();

            return new Response<Transaction?>(transaction);
        }
        catch (Exception ex)
        {
            return new Response<Transaction?>(null, 500, ex.Message);
        }
    }

    public async Task<Response<Transaction?>> GetById(GetTransactionByIdRequest request)
    {
        try
        {
            var category = await context.Transactions.AsNoTracking().FirstOrDefaultAsync(x =>
                x.Id == request.Id
            );

            if (category == null)
                return new Response<Transaction?>(null, 404, "Categoria nao encontrada");

            return new Response<Transaction?>(category, message: "Categoria encontrada com sucesso");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new Response<Transaction?>(null, 500, e.Message);
        }
    }

    public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransactionByPeriodRequest request)
    {
        try
        {
            request.StartDate ??= DateTime.Now.GetFirstDay(null, null);
            request.EndDate ??= DateTime.Now.GetLastDay(null, null);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new PagedResponse<List<Transaction>?>(
                null,
                500,
                message: "Nao foi possivel determinar a data de inicio e fim");
        }

        try
        {
            var query = context
                .Transactions
                .AsNoTracking()
                .Where(x => 
                    x.UserId == request.UserId &&
                    x.PaidOrReceivedAt >= request.StartDate &&
                    x.PaidOrReceivedAt <= request.EndDate)
                .OrderBy(x => x.Title);

            var transactions = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponse<List<Transaction>?>(
                transactions,
                count,
                request.PageNumber,
                request.PageSize);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new PagedResponse<List<Transaction>?>(
                null,
                0,
                request.PageNumber,
                request.PageSize);
        }
    }

    public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        try
        {
            if (request is { Type: Fina.Core.Enums.ETransactionType.Withdraw, Amount: >= 0 })
                request.Amount *= -1;

            var transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (transaction is null)
                return new Response<Transaction?>(null, 404, "Transação nao foi encontrada");

            transaction.Amount = request.Amount;
            transaction.Type = request.Type;
            transaction.CategoryId = request.CategoryId;
            transaction.CreateAt = DateTime.Now;
            transaction.PaidOrReceivedAt = request.PaidOrReceiveAt;
            transaction.Title = request.Title;
            transaction.UserId = request.UserId;

            context.Transactions.Update(transaction);
            await context.SaveChangesAsync();

            return new Response<Transaction?>(transaction);
        }
        catch (Exception ex)
        {
            return new Response<Transaction?>(null, 500, ex.Message);
        }
    }
}
