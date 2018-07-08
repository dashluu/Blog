﻿using Blog.Models;
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

        [Route("api/Categories")]
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
        [Route("api/Categories")]
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
        [Route("api/Categories/{categoryId}")]
        public IHttpActionResult UpdateCategory(string categoryId, [FromBody]EditedCategoryDTO editedCategoryDTO)
        {
            object jsonObject;

            if (string.IsNullOrWhiteSpace(categoryId))
            {
                jsonObject = new { status = 500 };
                return Json(jsonObject);
            }

            editedCategoryDTO.CategoryId = categoryId;

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
