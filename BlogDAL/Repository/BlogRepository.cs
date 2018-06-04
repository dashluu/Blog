using System;
using System.Collections.Generic;
using System.Text;
using BlogDAL.Entity;

namespace BlogDAL.Repository
{
    public class BlogRepository : IBlogRepository
    {
        private List<PostEntity> postSampleData;
        private List<CommentEntity> commentSampleData;

        public BlogRepository()
        {
            commentSampleData = new List<CommentEntity>()
            {
                new CommentEntity()
                {
                    CommentId = "abc",
                    Username = "Quang",
                    Content = "This is Quang's comment."
                },
                new CommentEntity()
                {
                    CommentId = "def",
                    Username = "Hao",
                    Content = "This is Hao's comment."
                }
            };
            commentSampleData[0].ChildCommentEntities = new List<CommentEntity>()
            {
                commentSampleData[1]
            };
            postSampleData = new List<PostEntity>()
            {
                new PostEntity()
                {
                    PostId = "123",
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
                    UpdatedDate = new DateTime(2018, 5, 1),
                    CommentEntities = new List<CommentEntity>()
                    {
                        commentSampleData[0]
                    }
                },
                new PostEntity()
                {
                    PostId = "456",
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
                    UpdatedDate = new DateTime(2018, 5, 28),
                    CommentEntities = new List<CommentEntity>()
                    {
                        commentSampleData[1]
                    }
                },
                new PostEntity()
                {
                    PostId = "789",
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
                    PostId = "ABC",
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

        public PostEntity GetPost(string id)
        {
            foreach (PostEntity postEntity in postSampleData)
            {
                if (postEntity.PostId.Equals(id))
                {
                    return postEntity;
                }
            }
            return null;
        }

        public List<PostEntity> GetPostEntities()
        {
            return postSampleData;
        }

        public List<PostEntity> GetPostEntitiesWithCategory(string category)
        {
            List<PostEntity> postEntities = new List<PostEntity>();
            foreach (PostEntity postEntity in postSampleData)
            {
                if (postEntity.PostCategory.Equals(category))
                {
                    postEntities.Add(postEntity);
                }
            }
            return postEntities;
        }

        public List<CommentEntity> GetCommentEntities(PostEntity postEntity)
        {
            throw new NotImplementedException();
        }

        public List<CommentEntity> GetCommentEntitiesWithRoot(CommentEntity commentEntity)
        {
            throw new NotImplementedException();
        }
    }
}
