namespace Bookify.Web.Core.ViewModels
{
    public class CategoryFormViewModel
    {
        public int Id { get; set; }
        [MaxLength(100,ErrorMessage ="Max Length is 100 chr.")]
        public string Name { get; set; } = null!;
    }
}
