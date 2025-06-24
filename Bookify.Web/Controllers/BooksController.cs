using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookify.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private List<string> _allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".gif" };
        private int _maxFileSize = 3 * 1024 * 1024; // 3 MB
        public BooksController(ApplicationDbContext context, IMapper mapper, 
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View("Form", PopulateViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", PopulateViewModel(model));

            var book = _mapper.Map<Book>(model);

            if (model.Image is not null)
            {
                var extension = Path.GetExtension(model.Image.FileName);
                if (!_allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(nameof(model.Image),Errors.notAllowedExtension);
                    return View("Form", PopulateViewModel(model));
                }
                if (model.Image.Length > _maxFileSize)
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.maxFileSize);
                    return View("Form", PopulateViewModel(model));
                }
                var imageName = $"{Guid.NewGuid()}{extension}";
                var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/Images/Books", imageName);
                using var stream = System.IO.File.Create(path);
                model.Image.CopyTo(stream);
                book.ImageUrl = imageName;
            }

            foreach (var category in model.SelectedCategories)
                book.Categories.Add(new BookCategory { CategoryID = category });
            _context.Books.Add(book);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            var book = _context.Books.Include(b => b.Categories).SingleOrDefault(b => b.Id == id);


            if (book is null)
                return NotFound();
            
            var model = _mapper.Map<BookFormViewModel>(book);
            var viewModel = PopulateViewModel(model);
            viewModel.SelectedCategories = book.Categories.Select(c => c.CategoryID).ToList();
            return View("Form", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BookFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", PopulateViewModel(model));

            var book = _context.Books.Include(b => b.Categories).SingleOrDefault(b => b.Id == model.Id);
            
            if (book is null)
                return NotFound();

            if (model.Image is not null)
            {
                if (!string.IsNullOrEmpty(book.ImageUrl))
                {
                    var oldImagePath = Path.Combine($"{_webHostEnvironment.WebRootPath}/Images/Books", book.ImageUrl);
                    
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }
                var extension = Path.GetExtension(model.Image.FileName);
                if (!_allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.notAllowedExtension);
                    return View("Form", PopulateViewModel(model));
                }
                if (model.Image.Length > _maxFileSize)
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.maxFileSize);
                    return View("Form", PopulateViewModel(model));
                }
                var imageName = $"{Guid.NewGuid()}{extension}";
                var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/Images/Books", imageName);
                using var stream = System.IO.File.Create(path);
                model.Image.CopyTo(stream);
                model.ImageUrl = imageName;
            }

            else if (model.Image is null && !string.IsNullOrEmpty(book.ImageUrl))
                model.ImageUrl = book.ImageUrl;

            book = _mapper.Map(model, book);
            book.LastUpdatedOn = DateTime.Now;


            foreach (var category in model.SelectedCategories)
                book.Categories.Add(new BookCategory { CategoryID = category });


            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        private BookFormViewModel PopulateViewModel(BookFormViewModel? model = null)
        {
            BookFormViewModel viewModel = model is null ? new BookFormViewModel() : model;
            var authors = _context.Authors.Where(a => !a.IsDeleted).OrderBy(a => a.Name).ToList();
            var categories = _context.Categories.Where(a => !a.IsDeleted).OrderBy(a => a.Name).ToList();

            viewModel.Authors = _mapper.Map<IEnumerable<SelectListItem>>(authors);
            viewModel.Categories = _mapper.Map<IEnumerable<SelectListItem>>(categories);
            return viewModel;

        }
    }
}
