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

        public PaginationDTO<PostCardDTO> GetPostCardPaginationDTOWithCategory(string category, int pageNumber, int pageSize)
        {
            PaginationEntity<PostEntity> postPaginationEntity = postRepository.GetPostPaginationEntityWithCategory(category, pageNumber, pageSize);
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
    }
}
