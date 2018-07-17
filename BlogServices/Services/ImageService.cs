using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BlogServices.DTO;

namespace BlogServices.Services
{
    public class ImageService : IImageService
    {
        public bool MoveImage(string oldImagePath, string newImagePath)
        {
            try
            {
                File.Move(oldImagePath, newImagePath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
