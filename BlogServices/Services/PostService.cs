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
        private IServiceDataMapper dataMapper;

        public PostService(IPostRepository postRepository, IServiceDataMapper dataMapper)
        {
            this.postRepository = postRepository;
            this.dataMapper = dataMapper;
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

            if (postEntities == null)
            {
                return null;
            }

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

            if (postEntities == null)
            {
                return null;
            }

            List<PostCardDTO> postCardDTOs = new List<PostCardDTO>();

            foreach (PostEntity postEntity in postEntities)
            {
                PostCardDTO postCardDTO = dataMapper.MapPostCardEntityToDTO(postEntity);
                postCardDTOs.Add(postCardDTO);
            }

            return postCardDTOs;
        }

        public (PostDTO postDTO, bool end) GetPostDTOWithCommentPagination(string id)
        {
            (PostEntity postEntity, bool end) postEntityWithCommentPagination = postRepository.GetPostEntityWithCommentPagination(id);
            PostDTO postDTO = dataMapper.MapPostEntityToDTO(postEntityWithCommentPagination.postEntity);
            bool end = postEntityWithCommentPagination.end;

            return (postDTO, end);
        }
    }
}
