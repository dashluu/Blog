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
        [Route("Images")]
        public ActionResult Index(string name)
        {
            string imageDir = Server.MapPath("~/Images");
            string imagePath = Path.Combine(imageDir, name);
            return File(imagePath, MimeMapping.GetMimeMapping(imagePath));
        }
    }
}