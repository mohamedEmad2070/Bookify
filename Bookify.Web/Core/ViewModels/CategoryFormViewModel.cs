using Microsoft.AspNetCore.Mvc;

namespace Bookify.Web.Core.ViewModels
{
    public class CategoryFormViewModel
    {
        public int Id { get; set; }
        [MaxLength(100, ErrorMessage = "Max Length is 100 chr.")]
        [Remote("AllowItem", controller: "Category", ErrorMessage = "This Category Name Is Exists")]
        public string Name { get; set; } = null!;
    }
}
