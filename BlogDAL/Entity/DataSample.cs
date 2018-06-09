using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDAL.Entity
{
    public class DataSample
    {
        public List<PostEntity> PostSampleData { get; set; }

        public DataSample()
        {
            PostSampleData = new List<PostEntity>()
            {
                new PostEntity()
                {
                    PostId = GenerateId(),
                    Title = "Title",
                    PostCategory = "Life",
                    ShortDescription = "Lorem ipsum dolor sit amet, dico " +
                    "prima graece sea an, te diam praesent nec. Duo no eros " +
                    "graece, ad iriure maiorum omnesque ius, vix oblique " +
                    "eripuit oportere eu.",
                    Content = "Lorem ipsum dolor sit amet, dico " +
                    "prima graece sea an, te diam praesent nec. Duo no eros " +
                    "graece, ad iriure maiorum omnesque ius, vix oblique " +
                    "eripuit oportere eu. Per tempor apeirian antiopam in, " +
                    "vix vidit dictas ex. Ne feugait honestatis per, dicam " +
                    "placerat sea in. Te vim zril neglegentur, meis.",
                    ThumbnailImageSrc = MapImagePath("wallpaper1.jpg"),
                    CreatedDate = DateTime.Now,
                    UpdatedDate = new DateTime(2018, 5, 1)
                },
                new PostEntity()
                {
                    PostId = GenerateId(),
                    Title = "Title",
                    PostCategory = "Travel",
                    ShortDescription = "Lorem ipsum dolor sit amet, dico " +
                    "prima graece sea an, te diam praesent nec. Duo no eros " +
                    "graece, ad iriure maiorum omnesque ius, vix oblique " +
                    "eripuit oportere eu.",
                    Content = "Lorem ipsum dolor sit amet, dico " +
                    "prima graece sea an, te diam praesent nec. Duo no eros " +
                    "graece, ad iriure maiorum omnesque ius, vix oblique " +
                    "eripuit oportere eu. Per tempor apeirian antiopam in, " +
                    "vix vidit dictas ex. Ne feugait honestatis per, dicam " +
                    "placerat sea in. Te vim zril neglegentur, meis.",
                    ThumbnailImageSrc = MapImagePath("wallpaper3.jpg"),
                    CreatedDate = DateTime.Now,
                    UpdatedDate = new DateTime(2018, 5, 28)
                },
                new PostEntity()
                {
                    PostId = GenerateId(),
                    Title = "Title",
                    PostCategory = "Tech",
                    ShortDescription = "Lorem ipsum dolor sit amet, dico " +
                    "prima graece sea an, te diam praesent nec. Duo no eros " +
                    "graece, ad iriure maiorum omnesque ius, vix oblique " +
                    "eripuit oportere eu.",
                    Content = "Lorem ipsum dolor sit amet, dico " +
                    "prima graece sea an, te diam praesent nec. Duo no eros " +
                    "graece, ad iriure maiorum omnesque ius, vix oblique " +
                    "eripuit oportere eu. Per tempor apeirian antiopam in, " +
                    "vix vidit dictas ex. Ne feugait honestatis per, dicam " +
                    "placerat sea in. Te vim zril neglegentur, meis.",
                    ThumbnailImageSrc = MapImagePath("wallpaper4.jpg"),
                    CreatedDate = DateTime.Now,
                    UpdatedDate = new DateTime(2018, 5, 30)
                },
                new PostEntity()
                {
                    PostId = GenerateId(),
                    Title = "Title",
                    PostCategory = "Health",
                    ShortDescription = "Lorem ipsum dolor sit amet, dico " +
                    "prima graece sea an, te diam praesent nec. Duo no eros " +
                    "graece, ad iriure maiorum omnesque ius, vix oblique " +
                    "eripuit oportere eu.",
                    Content = "Lorem ipsum dolor sit amet, dico " +
                    "prima graece sea an, te diam praesent nec. Duo no eros " +
                    "graece, ad iriure maiorum omnesque ius, vix oblique " +
                    "eripuit oportere eu. Per tempor apeirian antiopam in, " +
                    "vix vidit dictas ex. Ne feugait honestatis per, dicam " +
                    "placerat sea in. Te vim zril neglegentur, meis.",
                    ThumbnailImageSrc = MapImagePath("wallpaper5.png"),
                    CreatedDate = DateTime.Now,
                    UpdatedDate = new DateTime(2018, 5, 30)
                }
            };
        }

        private string MapImagePath(string imageName)
        {
            return "/Image/Index?imageName=" + imageName;
        }

        private string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
