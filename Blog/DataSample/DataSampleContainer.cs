using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.DataSample
{
    public class DataSampleContainer
    {
        public List<PostModel> PostList { get; set; }
        public List<PostCardModel> PostCardList { get; set; }

        private string MapImagePath (string imageName)
        {
            return "/Image/Index?imageName=" + imageName;
        }

        public DataSampleContainer()
        {

            PostList = new List<PostModel>()
            {
                new PostModel()
                {
                    PostId = "123",
                    Title = "Title",
                    Author = "Author",
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
                    CreatedDate = DateTime.Now.ToShortDateString(),
                    UpdatedDate = "1 min ago"
                },
                new PostModel()
                {
                    PostId = "456",
                    Title = "Title",
                    Author = "Author",
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
                    CreatedDate = DateTime.Now.ToShortDateString(),
                    UpdatedDate = "2 min ago"
                },
                new PostModel()
                {
                    PostId = "789",
                    Title = "Title",
                    Author = "Author",
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
                    ThumbnailImageSrc = MapImagePath("wallpaper4.jpg"),
                    CreatedDate = DateTime.Now.ToShortDateString(),
                    UpdatedDate = "3 min ago"
                },
                new PostModel()
                {
                    PostId = "ABC",
                    Title = "Title",
                    Author = "Author",
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
                    ThumbnailImageSrc = MapImagePath("wallpaper5.png"),
                    CreatedDate = DateTime.Now.ToShortDateString(),
                    UpdatedDate = "4 min ago"
                }
            };

            PostCardList = new List<PostCardModel>();

            for (int i = 0, postListCount = PostList.Count; 
                i < postListCount;
                i++)
            {
                PostCardList.Add(new PostCardModel()
                {
                    PostId = PostList[i].PostId,
                    Title = PostList[i].Title,
                    Author = PostList[i].Author,
                    CreatedDate = PostList[i].CreatedDate,
                    UpdatedDate = PostList[i].UpdatedDate,
                    ShortDescription = PostList[i].ShortDescription,
                    ThumbnailImageSrc = PostList[i].ThumbnailImageSrc
                });
            }
        }
    }
}