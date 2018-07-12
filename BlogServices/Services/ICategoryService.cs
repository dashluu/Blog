using BlogServices.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogServices.Services
{
    public interface ICategoryService
    {
        List<CategoryDTO> GetCategoryDTOs();

        bool AddCategory(CategoryDTO categoryDTO);

        bool UpdateCategory(CategoryDTO categoryDTO);
    }
}
