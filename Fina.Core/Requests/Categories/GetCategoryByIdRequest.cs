using System.ComponentModel.DataAnnotations;

namespace Fina.Core.Requests.Categories;

public class GetCategoryByIdRequest : Request
{
    [Required(ErrorMessage = "Id deve ser passado como parametro")]
    public long Id { get; set; }
}
