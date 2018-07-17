using Blog.Models;
using BlogServices.DTO;
using BlogServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Blog.Controllers
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class CategoryAPIController : ApiController
    {
        private ICategoryService categoryService;
        private APIIModelDataMapper dataMapper;

        public CategoryAPIController(ICategoryService categoryService, APIIModelDataMapper dataMapper)
        {
            this.categoryService = categoryService;
            this.dataMapper = dataMapper;
        }

        [Route("api/Categories")]
        public IHttpActionResult GetCategories()
        {
            object jsonObject;

            List<CategoryDTO> categoryDTOs = categoryService.GetCategories();
            List<APICategoryModel> categoryModels = dataMapper.MapCategoryDTOsToModels(categoryDTOs);

            if (categoryDTOs == null)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                jsonObject = new
                {
                    status = 200,
                    data = categoryModels
                };
            }

            return Json(jsonObject);
        }

        [HttpPost]
        [Route("api/Categories")]
        public IHttpActionResult AddCategory([FromBody]APIEditedCategoryModel editedCategoryModel)
        {
            object jsonObject;

            if (!ModelState.IsValid)
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            CategoryDTO categoryDTO = dataMapper.MapEditedCategoryModelToDTO(editedCategoryModel);
            bool addSuccessfully = categoryService.Add(categoryDTO);

            if (!addSuccessfully)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                APICategoryModel categoryModel = dataMapper.MapCategoryDTOToModel(categoryDTO);

                jsonObject = new {
                    status = 200,
                    data = categoryModel
                };
            }

            return Json(jsonObject);
        }

        [HttpPost]
        [Route("api/Categories/{categoryId}")]
        public IHttpActionResult UpdateCategory(string categoryId, [FromBody]APIEditedCategoryModel editedCategoryModel)
        {
            object jsonObject;

            if (string.IsNullOrWhiteSpace(categoryId) || !ModelState.IsValid)
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            CategoryDTO categoryDTO = dataMapper.MapEditedCategoryModelToDTO(editedCategoryModel);
            categoryDTO.CategoryId = categoryId;

            bool updateSuccessfully = categoryService.Update(categoryDTO);

            if (!updateSuccessfully)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                jsonObject = new { status = 200 };
            }

            return Json(jsonObject);
        }
    }
}
