﻿using Bookify.Web.Core.Models;
using Bookify.Web.Core.ViewModels;
using Bookify.Web.Filters;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var categories = _context.Categories.AsNoTracking().ToList();
            return View(categories);
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

            var category = new Category
            {
                Name = model.Name,           
            };

            _context.Categories.Add(category);
            _context.SaveChanges();
            
            return PartialView("_CategoryRow",category);
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
            var model = new CategoryFormViewModel
            {
                Id = category.Id,
                Name = category.Name,
            };
            return PartialView("_Form",model);
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
            category.Name = model.Name;
            category.LastUpdatedOn = DateTime.Now;       
            _context.SaveChanges();

            return PartialView("_CategoryRow", category);
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
            category.LastUpdatedOn =DateTime.Now;
            _context.SaveChanges();
            return Ok(category.LastUpdatedOn.ToString());
        }
    }
}
