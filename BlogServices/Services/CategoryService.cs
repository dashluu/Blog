using System;
using System.Collections.Generic;
using System.Text;
using BlogDAL.Entity;
using BlogDAL.Repository;
using BlogServices.DTO;

namespace BlogServices.Services
{
    public class CategoryService : ICategoryService
    {
        private ICategoryRepository categoryRepository;
        private IServiceDataMapper dataMapper;

        public CategoryService(ICategoryRepository categoryRepository, IServiceDataMapper dataMapper)
        {
            this.categoryRepository = categoryRepository;
            this.dataMapper = dataMapper;
        }

        public List<CategoryDTO> GetCategoryDTOs()
        {
            List<CategoryEntity> categoryEntities = categoryRepository.GetAll();

            if (categoryEntities == null)
            {
                return null;
            }

            List<CategoryDTO> categoryDTOs = new List<CategoryDTO>();

            foreach (CategoryEntity categoryEntity in categoryEntities)
            {
                CategoryDTO categoryDTO = dataMapper.MapCategoryEntityToDTO(categoryEntity);
                categoryDTOs.Add(categoryDTO);
            }

            return categoryDTOs;
        }
    }
}
