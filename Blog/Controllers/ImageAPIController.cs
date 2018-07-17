using Blog.Models;
using BlogServices.DTO;
using BlogServices.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Blog.Controllers
{
    public class ImageAPIController : ApiController
    {
        private IImageService imageService;
        private IHashService hashService;
        private APIIModelDataMapper dataMapper;

        public ImageAPIController(IImageService imageService, IHashService hashService, APIIModelDataMapper dataMapper)
        {
            this.imageService = imageService;
            this.hashService = hashService;
            this.dataMapper = dataMapper;
        }

        [Route("api/Images")]
        public HttpResponseMessage GetImage(string name)
        {
            HttpContext httpContext = HttpContext.Current;
            string imageServerRoot = httpContext.Server.MapPath(APISettings.IMAGE_ROOT);
            string imagePath = Path.Combine(imageServerRoot, name);
            HttpResponseMessage httpResponse;

            try
            {
                httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
                FileStream fileStream = new FileStream(imagePath, FileMode.Open);
                httpResponse.Content = new StreamContent(fileStream);
                httpResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            }
            catch (Exception)
            {
                httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            return httpResponse;
        }

        [HttpPost]
        [Route("api/Images/New")]
        public async Task<IHttpActionResult> UploadImages()
        {
            object jsonObject;

            try
            {
                HttpContext httpContext = HttpContext.Current;
                string imageServerRoot = httpContext.Server.MapPath(APISettings.IMAGE_ROOT);
                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(imageServerRoot);
                List<APIImageModel> imageModels = new List<APIImageModel>();
                await Request.Content.ReadAsMultipartAsync(streamProvider);

                foreach(MultipartFileData file in streamProvider.FileData)
                {
                    FileInfo fileInfo = new FileInfo(file.Headers.ContentDisposition.FileName.Trim('"'));
                    string imageExtension = fileInfo.Extension;
                    string imageId = hashService.GenerateId();
                    string imageName = imageId + imageExtension;
                    string newImagePath = Path.Combine(imageServerRoot, imageName);

                    bool moveSuccessfully = imageService.MoveImage(oldImagePath: file.LocalFileName, newImagePath);

                    if (moveSuccessfully)
                    {
                        APIImageModel imageModel = new APIImageModel()
                        {
                            ImageId = imageId,
                            Extension = imageExtension
                        };

                        imageModels.Add(imageModel);
                    }
                }

                jsonObject = new {
                    status = 200,
                    data = imageModels
                };
            }
            catch (Exception)
            {
                jsonObject = new { status = 500 };
            }

            return Json(jsonObject);
        }
    }
}
