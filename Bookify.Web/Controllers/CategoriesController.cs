﻿
namespace Bookify.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoriesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpGet]
        public IActionResult Index()
        {
            var categories = _context.Categories.AsNoTracking().ToList();
            var viewModel = _mapper.Map<IEnumerable<CategoryViewModel>>(categories);

            return View(viewModel);
        }
        [HttpGet]
        [AjaxOnly]
        public IActionResult Create()
        {
            return PartialView("_Form");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var category = _mapper.Map<Category>(model);

            _context.Categories.Add(category);
            _context.SaveChanges();
            var viewModel = _mapper.Map<CategoryViewModel>(category);
            return PartialView("_CategoryRow", viewModel);
        }
        [HttpGet]
        [AjaxOnly]
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<CategoryFormViewModel>(category);
            return PartialView("_Form", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var category = _context.Categories.Find(model.Id);
            if (category == null)
            {
                return NotFound();
            }
            category = _mapper.Map(model, category);
            category.LastUpdatedOn = DateTime.Now;
            _context.SaveChanges();
            var viewModel = _mapper.Map<CategoryViewModel>(category);
            return PartialView("_CategoryRow", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(int id)
        {


            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            category.IsDeleted = !category.IsDeleted;
            category.LastUpdatedOn = DateTime.Now;
            _context.SaveChanges();
            return Ok(category.LastUpdatedOn.ToString());
        }
        public IActionResult AllowItem(CategoryFormViewModel model)

        {
            var caregory = _context.Categories.SingleOrDefault(c => c.Name == model.Name);
            var isAllowed = caregory == null || caregory.Id.Equals(model.Id); // Allow if the name is not in use or if it's the same category being edited
            return Json(isAllowed); // Return true if the name is available, false if it exists

        }
    }
}
