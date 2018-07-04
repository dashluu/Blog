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
    [Route("api/Categories")]
    public class CategoryAPIController : ApiController
    {
        private ICategoryService categoryService;

        public CategoryAPIController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public IHttpActionResult GetCategories()
        {
            object jsonObject;

            List<CategoryDTO> categoryDTOs = categoryService.GetCategoryDTOs();

            if (categoryDTOs == null)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                jsonObject = new
                {
                    status = 200,
                    data = categoryDTOs
                };
            }

            return Json(jsonObject);
        }

        [HttpPost]
        public IHttpActionResult AddCategory([FromBody]EditedCategoryDTO editedCategoryDTO)
        {
            object jsonObject;

            if (string.IsNullOrWhiteSpace(editedCategoryDTO.Name) 
                || string.IsNullOrWhiteSpace(editedCategoryDTO.Description))
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            bool addSuccessfully = categoryService.AddEditedCategoryDTO(editedCategoryDTO);

            if (!addSuccessfully)
            {
                jsonObject = new { status = 500 };
            }
            else
            {
                jsonObject = new {
                    status = 200,
                    data = editedCategoryDTO.CategoryId
                };
            }

            return Json(jsonObject);
        }

        [HttpPost]
        public IHttpActionResult UpdateCategory([FromBody]EditedCategoryDTO editedCategoryDTO)
        {
            object jsonObject;

            if (string.IsNullOrWhiteSpace(editedCategoryDTO.CategoryId)
                || string.IsNullOrWhiteSpace(editedCategoryDTO.Name)
                || string.IsNullOrWhiteSpace(editedCategoryDTO.Description))
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

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
