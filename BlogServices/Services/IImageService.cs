using BlogServices.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlogServices.Services
{
    public interface IImageService
    {
        bool MoveImage(string oldImagePath, string newImagePath);
    }
}
