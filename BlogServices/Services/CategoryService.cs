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
        private IHashService hashService;

        public CategoryService(ICategoryRepository categoryRepository, IServiceDataMapper dataMapper, IHashService hashService)
        {
            this.categoryRepository = categoryRepository;
            this.dataMapper = dataMapper;
            this.hashService = hashService;
        }

        public bool Add(CategoryDTO categoryDTO)
        {
            categoryDTO.CategoryId = hashService.GenerateId();

            CategoryEntity categoryEntity = dataMapper.MapCategoryDTOToEntity(categoryDTO);
            bool addSuccessfully = categoryRepository.Add(categoryEntity);

            return addSuccessfully;
        }

        public List<CategoryDTO> GetCategories()
        {
            List<CategoryEntity> categoryEntities = categoryRepository.GetEntities();
            List<CategoryDTO> categoryDTOs = dataMapper.MapCategoryEntitiesToDTOs(categoryEntities);

            return categoryDTOs;
        }

        public bool Update(CategoryDTO categoryDTO)
        {
            CategoryEntity categoryEntity = dataMapper.MapCategoryDTOToEntity(categoryDTO);
            bool updateSuccessfully = categoryRepository.Update(categoryEntity);

            return updateSuccessfully;
        }
    }
}
