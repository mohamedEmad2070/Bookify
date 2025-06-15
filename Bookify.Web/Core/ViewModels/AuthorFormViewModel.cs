namespace Bookify.Web.Core.ViewModels
{
    public class AuthorFormViewModel
    {
        public int Id { get; set; }
        [MaxLength(100, ErrorMessage = "Max Length is 100 chr.")]
        [Remote("AllowItem", null, AdditionalFields = "Id", ErrorMessage = "This Category Name Is Exists")]
        public string Name { get; set; } = null!;
    }
}
