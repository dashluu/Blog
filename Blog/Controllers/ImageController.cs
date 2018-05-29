using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class ImageController : Controller
    {
        // GET: Image
        public ActionResult Index(string imageName)
        {
            string imageDir = Server.MapPath("~/Images");
            string imagePath = Path.Combine(imageDir, imageName);
            return base.File(imagePath, MimeMapping.GetMimeMapping(imagePath));
        }
    }
}