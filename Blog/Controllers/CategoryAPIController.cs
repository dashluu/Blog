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
        private IModelDataMapper dataMapper;

        public CategoryAPIController(ICategoryService categoryService, IModelDataMapper dataMapper)
        {
            this.categoryService = categoryService;
            this.dataMapper = dataMapper;
        }

        public IHttpActionResult GetCategories()
        {
            object jsonObject;

            List<CategoryDTO> categoryDTOs = categoryService.GetCategoryDTOs();

            if (categoryDTOs == null)
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            jsonObject = new
            {
                status = 200,
                data = categoryDTOs
            };

            return Json(jsonObject);
        }

        [HttpPost]
        public IHttpActionResult AddCategory([FromBody]EditedCategoryModel editedCategoryModel)
        {
            object jsonObject;

            if (string.IsNullOrWhiteSpace(editedCategoryModel.Name) 
                || string.IsNullOrWhiteSpace(editedCategoryModel.Description))
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            EditedCategoryDTO editedCategoryDTO = dataMapper.MapEditedCategoryModelToDTO(editedCategoryModel);

            bool addSuccessfully = categoryService.AddEditedCategoryDTO(editedCategoryDTO);

            if (!addSuccessfully)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                jsonObject = new { status = 200 };
            }

            return Json(jsonObject);
        }

        [HttpPost]
        public IHttpActionResult UpdateCategory([FromBody]EditedCategoryModel editedCategoryModel)
        {
            object jsonObject;

            if (string.IsNullOrWhiteSpace(editedCategoryModel.Name)
                || string.IsNullOrWhiteSpace(editedCategoryModel.Description))
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            EditedCategoryDTO editedCategoryDTO = dataMapper.MapEditedCategoryModelToDTO(editedCategoryModel);

            bool updateSuccessfully = categoryService.UpdateEditedCategoryDTO(editedCategoryDTO);

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
