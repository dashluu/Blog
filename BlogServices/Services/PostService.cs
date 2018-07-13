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
        private IHashService hashService;

        public PostService(IPostRepository postRepository, IServiceDataMapper dataMapper, IHashService hashService)
        {
            this.postRepository = postRepository;
            this.dataMapper = dataMapper;
            this.hashService = hashService;
        }

        public bool AddPost(PostDTO postDTO)
        {
            postDTO.PostId = hashService.GenerateId();
            postDTO.CreatedDate = DateTime.Now;
            postDTO.UpdatedDate = DateTime.Now;

            PostEntity postEntity = dataMapper.MapPostDTOToEntity(postDTO);
            postEntity.CategoryId = postEntity.PostCategory.CategoryId;

            bool addSuccessfully = postRepository.Add(postEntity);

            return addSuccessfully;
        }

        public PaginationDTO<PostCardDTO> GetPostCardPaginationDTO(int pageNumber, int pageSize, string category = null)
        {
            PaginationEntity<PostEntity> postPaginationEntity = postRepository.GetPostPaginationEntity(pageNumber, pageSize, category);
            PaginationDTO<PostCardDTO> postCardPaginationDTO = dataMapper.MapPostCardPaginationEntityToDTO(postPaginationEntity);

            return postCardPaginationDTO;
        }

        public PostDTOWithPaginatedComments GetPostDTOWithPaginatedComments(string id, int pageSize)
        {
            PostEntityWithPaginatedComments postEntityWithPaginatedComments = postRepository.GetPostEntityWithPaginatedComments(id, pageSize);
            PostDTOWithPaginatedComments postDTOWithPaginatedComments = dataMapper.MapPostEntityToDTOWithPaginatedComments(postEntityWithPaginatedComments);

            return postDTOWithPaginatedComments;
        }

        public List<PaginationDTO<PostCardDTO>> GetPostCardPaginationDTOs(int pageSize)
        {
            List<PaginationEntity<PostEntity>> postPaginationEntities = postRepository.GetPostPaginationEntities(pageSize);
            List<PaginationDTO<PostCardDTO>> postCardPaginationDTOs = dataMapper.MapPostCardPaginationEntitiesToDTOs(postPaginationEntities);

            return postCardPaginationDTOs;
        }

        public PostDTO GetPostDTO(string id)
        {
            PostEntity postEntity = postRepository.GetPostEntity(id);
            PostDTO postDTO = dataMapper.MapPostEntityToDTO(postEntity);

            return postDTO;
        }

        public bool UpdatePost(PostDTO postDTO)
        {
            PostEntity postEntity = dataMapper.MapPostDTOToEntity(postDTO);
            postEntity.CategoryId = postEntity.PostCategory.CategoryId;
            postEntity.UpdatedDate = DateTime.Now;

            bool updateSuccessfully = postRepository.Update(postEntity);

            return updateSuccessfully;
        }

        public bool RemovePost(string postId)
        {
            bool removeSuccessfully = postRepository.Remove(postId);
            return removeSuccessfully;
        }
    }
}
