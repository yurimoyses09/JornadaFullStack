using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Responses;
using Final.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Final.Api.Handlers;

public class CategoryHandler(AppDbContext context) : ICategoryHandler
{
    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
    {

        var category = new Category()
        {
            UserId = request.UserId,
            Description = request.Description,
            Title = request.Title
        };

        try
        {
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return new Response<Category?>(category, 201, "Categoria Criada com sucesso");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new Response<Category?>(null, 500, e.Message);
        }
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x =>
                x.Id == request.Id &&
                x.UserId == request.UserId
            );

            if (category == null)
                return new Response<Category?>(category, 404, "Categoria nao encontrada");

            context.Categories.Remove(category);
            await context.SaveChangesAsync();

            return new Response<Category?>(category, message: "Categoria deletada com sucesso");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new Response<Category?>(null, 500, e.Message);
        }
    }

    public async Task<PagedResponse<List<Category>?>> GetAllAsync(GetAllCategoryRequest request)
    {
        try
        {
            var query = context
                .Categories
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .OrderBy(x => x.Title);

            var categories = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponse<List<Category>?>(
                categories,
                count,
                request.PageNumber,
                request.PageSize);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new PagedResponse<List<Category>?>(
                null,
                0,
                request.PageNumber,
                request.PageSize);
        }
    }

    public async Task<Response<Category?>> GetById(GetCategoryByIdRequest request)
    {
        try
        {
            var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x =>
                x.Id == request.Id
            );

            if (category == null)
                return new Response<Category?>(null, 404, "Categoria nao encontrada");

            return new Response<Category?>(category, message: "Categoria encontrada com sucesso");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new Response<Category?>(null, 500, e.Message);
        }
    }

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => 
                x.Id == request.Id &&
                x.UserId == request.UserId
            );

            if (category == null)
                return new Response<Category?>(category, 404, "Categoria nao encontrada");

            category.Title = request.Title;
            category.Description = request.Description;

            context.Categories.Update(category);
            await context.SaveChangesAsync();

            return new Response<Category?>(category, message: "Categoria atualizada com sucesso");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new Response<Category?>(null, 500, e.Message);
        }
    }
}
