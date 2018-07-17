using BlogServices.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogServices.Services
{
    public interface ICategoryService
    {
        List<CategoryDTO> GetCategories();

        bool Add(CategoryDTO categoryDTO);

        bool Update(CategoryDTO categoryDTO);
    }
}
