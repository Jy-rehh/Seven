using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Interfaces
{
    public interface ICategoryService
    {
        void AddCategory(CategoryViewModel model, string userId);
        List<CategoryViewModel> GetAllCategory();
        CategoryViewModel RetrieveCategory(int id);
        void UpdateCategory(CategoryViewModel model, string userId);
        void DeleteCategory(int id);
        string GetDuplicateCategoryName(string categoryName, int? excludeCategoryId = null);
    }
}
