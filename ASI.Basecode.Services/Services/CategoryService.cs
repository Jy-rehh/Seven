using ASI.Basecode.Data;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, IConfiguration configuration)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _config = configuration;
        }
        public void AddCategory(CategoryViewModel model, string userId)
        {
            var newCategory = new Category();
            newCategory.Name = model.Name;
            newCategory.CreatedBy = userId;
            newCategory.DateCreated = DateTime.Now;
            newCategory.DateUpdated = DateTime.Now;
            newCategory.Status = false;
            newCategory.TotalAmount = 0.00;
            _categoryRepository.AddCategory(newCategory);

        }
        public List<CategoryViewModel> GetAllCategory()
        {
            var serverUrl = _config.GetValue<string>("ServerUrl");
            var data = _categoryRepository.GetAllCategory().Where(s => s.Status == false).Select(s => new CategoryViewModel
            {
                CategoryId = s.CategoryId,
                Name = s.Name,
                CreatedBy = s.CreatedBy,
                DateCreated = s.DateCreated,
                DateUpdated = s.DateUpdated,

            }).ToList();
            return data;
        }
        public CategoryViewModel RetrieveCategory(int Id)
        {
            var category = _categoryRepository.GetAllCategory().Where(x => x.CategoryId.Equals(Id)).Select(s => new CategoryViewModel
            {
                CategoryId = s.CategoryId,
                Name = s.Name,
                CreatedBy = s.CreatedBy,
                DateCreated = s.DateCreated,
                DateUpdated = s.DateUpdated,

            }).FirstOrDefault();
            return category;
        }
        public void UpdateCategory(CategoryViewModel model, string userId)
        {
            var category = _categoryRepository.GetAllCategory().Where(x => x.CategoryId.Equals(model.CategoryId)).FirstOrDefault();
            model.DateCreated = category.DateCreated;
            if (category != null)
            {
                _mapper.Map(model, category);
                category.DateUpdated = DateTime.Now;
                category.CreatedBy = userId;
                category.TotalAmount = 0.00;

                _categoryRepository.UpdateCategory(category);
            }
        }
        public void DeleteCategory(int Id)
        {
            var category = _categoryRepository.GetAllCategory().FirstOrDefault(x => x.CategoryId == Id);
            if (category != null)
            {
                category.Status = true;
                _categoryRepository.UpdateCategory(category);
            }
        }
        public bool CategoryExists(string categoryName)
        {
            return _categoryRepository.GetAllCategory().Any(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
