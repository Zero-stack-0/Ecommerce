using Data.Repository.Interface;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Service.Dto;
using Service.Dto.Request.Admin.Category;
using Service.Helper;
using Service.Interface;
using static Entities.Constants;

namespace Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly ICommonService commonService;
        public CategoryService(ICategoryRepository categoryRepository, ICommonService commonService)
        {
            this.categoryRepository = categoryRepository;
            this.commonService = commonService;
        }

        public async Task<ApiResponse> Add(AddCategoryRequest dto)
        {
            try
            {
                if (dto.Requestor is null)
                {
                    return ResponseMessage.RequestorDoesNotExists();
                }

                if (dto.Requestor.Role.Name != Keys.ADMIN)
                {
                    return ResponseMessage.BadRequest(Keys.FORBIDDEN);
                }

                var doesCategoryAlreadyExists = await categoryRepository.GetByName(dto.Name);
                if (doesCategoryAlreadyExists is not null)
                {
                    return ResponseMessage.BadRequest(CATEGORY.CATEGORY_ALREADY_EXISTS);
                }

                var category = new Category(dto.Name, dto.Description);

                categoryRepository.Add(category);

                await categoryRepository.SaveAsync();

                return ResponseMessage.Sucess(category, CATEGORY.ADDED_SUCESSFULLY, null);
            }
            catch (Exception ex)
            {
                await commonService.RegisterException(ex);
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }

        public async Task<ApiResponse> GetList(GetCategoryListRequest dto)
        {
            try
            {
                if (dto.Requestor is null)
                {
                    return ResponseMessage.RequestorDoesNotExists();
                }

                if (dto.Requestor.Role.Name != Keys.ADMIN)
                {
                    return ResponseMessage.BadRequest(Keys.FORBIDDEN);
                }

                if (dto.PageNo <= 0 || dto.PageSize <= 0)
                {
                    return ResponseMessage.BadRequest(Keys.INVALID_PAGINATION);
                }

                var (categories, totalCount) = await categoryRepository.GetList(dto.PageNo, dto.PageSize, dto.SearchTerm);

                return ResponseMessage.Sucess(categories, Keys.CATEGORY, new PagedData(dto.PageNo, dto.PageSize, totalCount, (int)Math.Ceiling((double)totalCount / dto.PageSize)));
            }
            catch (Exception ex)
            {
                await commonService.RegisterException(ex);
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }

        public async Task<ApiResponse> Delete(DeleteRequest dto)
        {
            try
            {
                if (dto.Requestor is null)
                {
                    return ResponseMessage.RequestorDoesNotExists();
                }

                if (dto.Requestor.Role.Name != Keys.ADMIN)
                {
                    return ResponseMessage.BadRequest(Keys.FORBIDDEN);
                }

                var category = await categoryRepository.GetById(dto.CategoryId);
                if (category is null)
                {
                    return ResponseMessage.BadRequest(string.Format(Keys.DOES_NOT_EXISTS, Keys.CATEGORY));
                }

                category.Delete();

                categoryRepository.Update(category);

                await categoryRepository.SaveAsync();

                return ResponseMessage.Sucess(category, CATEGORY.DELETED_SUCESSFULLY, null);
            }
            catch (Exception ex)
            {
                await commonService.RegisterException(ex);
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }

        public async Task<ApiResponse> Update(UpdateRequest dto)
        {
            try
            {
                if (dto.Requestor is null)
                {
                    return ResponseMessage.RequestorDoesNotExists();
                }

                if (dto.Requestor.Role.Name != Keys.ADMIN)
                {
                    return ResponseMessage.BadRequest(Keys.FORBIDDEN);
                }

                var category = await categoryRepository.GetById(dto.Id);
                if (category is null)
                {
                    return ResponseMessage.BadRequest(string.Format(Keys.DOES_NOT_EXISTS, Keys.CATEGORY));
                }

                category.Update(dto.Name, dto.Description);

                categoryRepository.Update(category);

                await categoryRepository.SaveAsync();

                return ResponseMessage.Sucess(category, CATEGORY.UPDATED_SUCESSFULLY, null);
            }
            catch (Exception ex)
            {
                await commonService.RegisterException(ex);
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }

        public async Task<ApiResponse> GetById(long id, UserResponse requestor)
        {
            try
            {
                if (requestor is null)
                {
                    return ResponseMessage.RequestorDoesNotExists();
                }

                var category = await categoryRepository.GetById(id);
                if (category is null)
                {
                    return ResponseMessage.BadRequest(string.Format(Keys.DOES_NOT_EXISTS, Keys.CATEGORY));
                }

                return ResponseMessage.Sucess(category, Keys.CATEGORY, null);
            }
            catch (Exception ex)
            {
                await commonService.RegisterException(ex);
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }

        public async Task<ApiResponse> GetListForOwner()
        {
            try
            {
                var category = await categoryRepository.GetListForUser();

                return ResponseMessage.Sucess(category, Keys.CATEGORY, null);
            }
            catch (Exception ex)
            {
                await commonService.RegisterException(ex);
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }
    }
}