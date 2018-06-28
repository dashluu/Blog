using BlogServices.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogServices.Services
{
    public interface ICategoryService
    {
        List<CategoryDTO> GetCategoryDTOs();
        bool AddEditedCategoryDTO(EditedCategoryDTO editedCategoryDTO);
        bool RemoveEditedCategoryDTO(EditedCategoryDTO editedCategoryDTO);
        bool UpdateEditedCategoryDTO(EditedCategoryDTO editedCategoryDTO);
    }
}
