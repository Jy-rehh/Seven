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
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public void AddCategory(CategoryViewModel model, string userId)
        {
            var newCategory = new Category();
            newCategory.Name = model.Name;
            newCategory.CreatedBy = userId;
            newCategory.DateCreated = DateTime.Now;
            newCategory.DateUpdated = DateTime.Now;
            newCategory.Description = model.Description;
            _categoryRepository.AddCategory(newCategory);

        }
        public List<CategoryViewModel> GetAllCategory(int Id)
        {
            var data = _categoryRepository.GetAllCategory().Select(s => new CategoryViewModel
            {
                CategoryId = s.CategoryId,
                Name = s.Name,
                CreatedBy = s.CreatedBy,
                DateCreated = s.DateCreated,
                DateUpdated = s.DateUpdated,
                Description = s.Description,
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
                Description = s.Description,

            }).FirstOrDefault();
            return category;
        }
        public void UpdateCategory(CategoryViewModel model, string userId)
        {
            var category = _categoryRepository.GetAllCategory().Where(x => x.CategoryId.Equals(model.CategoryId)).FirstOrDefault();
            if (category != null)
            {
                _mapper.Map(model, category);
                category.DateUpdated = DateTime.Now;
                category.CreatedBy = userId;

                _categoryRepository.UpdateCategory(category);
            }
        }
        public void DeleteCategory(int Id)
        {
            var category = _categoryRepository.GetAllCategory().Where(x => x.CategoryId.Equals(Id)).FirstOrDefault();
            if(category != null)
            {
                _categoryRepository.DeleteCategory(category);
            }
        }
    }
}
    