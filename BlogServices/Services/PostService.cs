using System;
using System.Collections.Generic;
using BlogServices.DTO;
using BlogDAL.Repository;
using BlogDAL.Entity;

namespace BlogServices.Services
{
    public class PostService : IPostService
    {
        private IPostRepository postRepository;
        private DataMapper dataMapper;

        public PostService()
        {
            postRepository = new PostRepository();
            dataMapper = new DataMapper();
        }

        public bool AddEditedPostDTO(EditedPostDTO editedPostDTO)
        {
            PostEntity postEntity = dataMapper.MapEditedPostDTOToEntity(editedPostDTO);
            bool addSuccessfully = postRepository.Add(postEntity);
            return addSuccessfully;
        }

        public List<PostCardDTO> GetPostCardDTOs()
        {
            List<PostEntity> postEntities = postRepository.GetAll();
            List<PostCardDTO> postCardDTOs = new List<PostCardDTO>();

            foreach (PostEntity postEntity in postEntities)
            {
                PostCardDTO postCardDTO = dataMapper.MapPostCardEntityToDTO(postEntity);
                postCardDTOs.Add(postCardDTO);
            }

            return postCardDTOs;
        }

        public List<PostCardDTO> GetPostCardDTOsWithCategory(string category)
        {
            List<PostEntity> postEntities = postRepository.GetPostEntitiesWithCategory(category);
            List<PostCardDTO> postCardDTOs = new List<PostCardDTO>();

            foreach (PostEntity postEntity in postEntities)
            {
                PostCardDTO postCardDTO = dataMapper.MapPostCardEntityToDTO(postEntity);
                postCardDTOs.Add(postCardDTO);
            }

            return postCardDTOs;
        }

        public PostDTO GetPostDTO(string id)
        {
            PostEntity postEntity = postRepository.GetPostEntity(id);
            PostDTO postDTO = dataMapper.MapPostEntityToDTO(postEntity);
            return postDTO;
        }
    }
}
