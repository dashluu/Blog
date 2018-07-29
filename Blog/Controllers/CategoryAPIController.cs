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
    [Authorize(Roles = "admin")]
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

            if (categoryDTOs == null)
            {
                jsonObject = new
                {
                    status = HttpStatusCode.InternalServerError
                };
            }
            else
            {
                List<APICategoryModel> categoryModels = dataMapper.MapCategoryDTOsToModels(categoryDTOs);

                jsonObject = new
                {
                    status = HttpStatusCode.OK,
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
                jsonObject = new
                {
                    status = HttpStatusCode.BadRequest
                };

                return Json(jsonObject);
            }

            CategoryDTO categoryDTO = dataMapper.MapEditedCategoryModelToDTO(editedCategoryModel);
            bool addSuccessfully = categoryService.Add(categoryDTO);

            if (!addSuccessfully)
            {
                jsonObject = new
                {
                    status = HttpStatusCode.InternalServerError
                };
            }
            else
            {
                APICategoryModel categoryModel = dataMapper.MapCategoryDTOToModel(categoryDTO);

                jsonObject = new
                {
                    status = HttpStatusCode.OK,
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

            if (string.IsNullOrWhiteSpace(categoryId) 
                || !ModelState.IsValid)
            {
                jsonObject = new
                {
                    status = HttpStatusCode.BadRequest
                };

                return Json(jsonObject);
            }

            CategoryDTO categoryDTO = dataMapper.MapEditedCategoryModelToDTO(editedCategoryModel);
            categoryDTO.CategoryId = categoryId;

            bool updateSuccessfully = categoryService.Update(categoryDTO);

            if (!updateSuccessfully)
            {
                jsonObject = new
                {
                    status = HttpStatusCode.InternalServerError
                };
            }
            else
            {
                jsonObject = new
                {
                    status = HttpStatusCode.OK
                };
            }

            return Json(jsonObject);
        }
    }
}
