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

        public bool Add(PostDTO postDTO)
        {
            postDTO.PostId = hashService.GenerateId();
            postDTO.CreatedDate = DateTime.Now;
            postDTO.UpdatedDate = DateTime.Now;

            PostEntity postEntity = dataMapper.MapPostDTOToEntity(postDTO);
            postEntity.CategoryId = postEntity.PostCategory.CategoryId;

            bool addSuccessfully = postRepository.Add(postEntity);

            return addSuccessfully;
        }

        public PaginationDTO<PostCardDTO> GetPostCardPagination(int pageNumber, int pageSize, string category = null, string searchQuery = null)
        {
            PaginationEntity<PostEntity> postPaginationEntity = postRepository.GetPostPagination(pageNumber, pageSize, category, searchQuery);
            PaginationDTO<PostCardDTO> postCardPaginationDTO = dataMapper.MapPostCardPaginationEntityToDTO(postPaginationEntity);

            return postCardPaginationDTO;
        }

        public PostDTOWithPaginatedComments GetPostWithPaginatedComments(string id, int pageSize)
        {
            PostEntityWithPaginatedComments postEntityWithPaginatedComments = postRepository.GetPostWithPaginatedComments(id, pageSize);
            PostDTOWithPaginatedComments postDTOWithPaginatedComments = dataMapper.MapPostEntityToDTOWithPaginatedComments(postEntityWithPaginatedComments);

            return postDTOWithPaginatedComments;
        }

        public List<PaginationDTO<PostCardDTO>> GetPostCardPaginationList(int pageSize, string searchQuery = null)
        {
            List<PaginationEntity<PostEntity>> postPaginationEntities = postRepository.GetPostPaginationList(pageSize, searchQuery);
            List<PaginationDTO<PostCardDTO>> postCardPaginationDTOs = dataMapper.MapPostCardPaginationEntitiesToDTOs(postPaginationEntities);

            return postCardPaginationDTOs;
        }

        public PostDTO GetPost(string id)
        {
            PostEntity postEntity = postRepository.GetPost(id);
            PostDTO postDTO = dataMapper.MapPostEntityToDTO(postEntity);

            return postDTO;
        }

        public bool Update(PostDTO postDTO)
        {
            PostEntity postEntity = dataMapper.MapPostDTOToEntity(postDTO);
            postEntity.CategoryId = postEntity.PostCategory.CategoryId;
            postEntity.UpdatedDate = DateTime.Now;

            bool updateSuccessfully = postRepository.Update(postEntity);

            return updateSuccessfully;
        }

        public bool Remove(string postId)
        {
            bool removeSuccessfully = postRepository.Remove(postId);
            return removeSuccessfully;
        }
    }
}
