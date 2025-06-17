using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;

namespace Bookify.Web.Core.ViewModels
{
    public class BookFormViewModel
    {
        public int Id { get; set; }
        [MaxLength(500,ErrorMessage =Errors.MaxLength)]
        public string Title { get; set; } = null!;

        [Display(Name ="Author")]
        public string AuthorId { get; set; }
        public IEnumerable<SelectListItem>? Authors { get; set; }

        [MaxLength(500, ErrorMessage = Errors.MaxLength)]
        public string Publisher { get; set; } = null!;

        [Display(Name = "Publishing Date")]
        public DateTime PublishingDate { get; set; }=DateTime.Now;
        public IFormFile? Image { get; set; }
        [MaxLength(100, ErrorMessage = Errors.MaxLength)]
        public string Hall { get; set; } = null!;

        [Display(Name = "Is Available for Rental")]
        public bool ISAvailableForRental { get; set; }

        public string Description { get; set; } = null!;

        public IList<int> SelectedCategories { get; set; } = new List<int>();
        public IEnumerable<SelectListItem>? Categories { get; set; }
    }
}
