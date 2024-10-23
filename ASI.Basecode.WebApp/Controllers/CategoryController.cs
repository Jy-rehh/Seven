using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace ASI.Basecode.WebApp.Controllers
{
    public class CategoryController : ControllerBase<CategoryController>
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(
                                IHttpContextAccessor httpContextAccessor,
                                ILoggerFactory loggerFactory,
                                IConfiguration configuration,
                                ICategoryService categoryService,
                                IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _categoryService = categoryService;
        }
        public IActionResult Index(int Id)
        {
            var data = _categoryService.GetAllCategory(Id);
            return View(data);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var data = _categoryService.GetAllCategory(Id);
            return View(data);
        }
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var data = _categoryService.GetAllCategory(Id);
            return View(data);
        }

        [HttpPost]
        public IActionResult Create(CategoryViewModel model)
        {
            _categoryService.AddCategory(model, UserId);
            return RedirectToAction("Create");
        }
        [HttpPost]
        public IActionResult Edit(CategoryViewModel model)
        {
            _categoryService.UpdateCategory(model, UserId);
            return RedirectToAction("Create");
        }
        [HttpPost]
        public IActionResult PostDelete(int Id)
        {
            _categoryService.DeleteCategory(Id);
            return RedirectToAction("Create");
        }
    }
}
