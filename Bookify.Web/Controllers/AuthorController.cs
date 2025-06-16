﻿namespace Bookify.Web.Controllers
{
    public class AuthorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public AuthorController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var authors = _context.Authors.AsNoTracking().ToList();
            var viewModel = _mapper.Map<IEnumerable<AuthorViewModel>>(authors);
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
        public IActionResult Create(AuthorFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var author = _mapper.Map<Author>(model);

            _context.Authors.Add(author);
            _context.SaveChanges();
            var viewModel = _mapper.Map<AuthorViewModel>(author);
            return PartialView("_AuthorRow", viewModel);
        }

        [HttpGet]
        [AjaxOnly]
        public IActionResult Edit(int id)
        {
            var author = _context.Authors.Find(id);
            if (author == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<AuthorFormViewModel>(author);
            return PartialView("_Form", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AuthorFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var author = _context.Authors.Find(model.Id);
            if (author == null)
            {
                return NotFound();
            }
            author = _mapper.Map(model, author);
            author.LastUpdatedOn = DateTime.Now;
            _context.SaveChanges();
            var viewModel = _mapper.Map<AuthorViewModel>(author);
            return PartialView("_AuthorRow", viewModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(int id)
        {


            var auther = _context.Authors.Find(id);
            if (auther == null)
            {
                return NotFound();
            }
            auther.IsDeleted = !auther.IsDeleted;
            auther.LastUpdatedOn = DateTime.Now;
            _context.SaveChanges();
            return Ok(auther.LastUpdatedOn.ToString());
        }
        public IActionResult AllowItem(AuthorFormViewModel model)

        {
            var author = _context.Authors.SingleOrDefault(c => c.Name == model.Name);
            var isAllowed = author == null || author.Id.Equals(model.Id); // Allow if the name is not in use or if it's the same category being edited
            return Json(isAllowed); // Return true if the name is available, false if it exists

        }


    }
}
