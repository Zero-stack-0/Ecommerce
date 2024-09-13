using Data.Repository.Interface;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Service.Dto;
using Service.Dto.Request.Admin;
using Service.Dto.Request.Admin.Category;
using Service.Helper;
using Service.Interface;
using static Entities.Constants;

namespace Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<ApiResponse> Add(AddCategoryRequest dto)
        {
            try
            {
                if (dto.Requestor is null)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, Keys.REQUESTOR_DOES_NOT_EXISTS, null);
                }

                if (dto.Requestor.Role.Name != Keys.ADMIN)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, Keys.FORBIDDEN, null);
                }

                var doesCategoryAlreadyExists = await categoryRepository.GetByName(dto.Name);
                if (doesCategoryAlreadyExists is not null)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, CATEGORY.CATEGORY_ALREADY_EXISTS, null);
                }

                var category = new Category(dto.Name, dto.Description);

                categoryRepository.Add(category);

                await categoryRepository.SaveAsync();

                return new ApiResponse(category, StatusCodes.Status200OK, CATEGORY.ADDED_SUCESSFULLY, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }

        public async Task<ApiResponse> GetList(GetCategoryListRequest dto)
        {
            try
            {
                if (dto.Requestor is null)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, Keys.REQUESTOR_DOES_NOT_EXISTS, null);
                }

                if (dto.Requestor.Role.Name != Keys.ADMIN)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, Keys.FORBIDDEN, null);
                }

                if (dto.PageNo <= 0 || dto.PageSize <= 0)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, Keys.INVALID_PAGINATION, null);
                }

                var (categories, totalCount) = await categoryRepository.GetList(dto.PageNo, dto.PageSize, dto.SearchTerm);

                return new ApiResponse(categories, StatusCodes.Status200OK, Keys.CATEGORY, new PagedData(dto.PageNo, dto.PageSize, totalCount, (int)Math.Ceiling((double)totalCount / dto.PageSize)));
            }
            catch (Exception ex)
            {
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }

        public async Task<ApiResponse> Delete(DeleteRequest dto)
        {
            try
            {
                if (dto.Requestor is null)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, Keys.REQUESTOR_DOES_NOT_EXISTS, null);
                }

                if (dto.Requestor.Role.Name != Keys.ADMIN)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, Keys.FORBIDDEN, null);
                }

                var category = await categoryRepository.GetById(dto.CategoryId);
                if (category is null)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, string.Format(Keys.DOES_NOT_EXISTS, Keys.CATEGORY), null);
                }

                category.Delete();

                categoryRepository.Update(category);

                await categoryRepository.SaveAsync();

                return new ApiResponse(category, StatusCodes.Status200OK, CATEGORY.DELETED_SUCESSFULLY, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }

        public async Task<ApiResponse> Update(UpdateRequest dto)
        {
            try
            {
                if (dto.Requestor is null)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, Keys.REQUESTOR_DOES_NOT_EXISTS, null);
                }

                if (dto.Requestor.Role.Name != Keys.ADMIN)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, Keys.FORBIDDEN, null);
                }

                var category = await categoryRepository.GetById(dto.Id);
                if (category is null)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, string.Format(Keys.DOES_NOT_EXISTS, Keys.CATEGORY), null);
                }

                category.Update(dto.Name, dto.Description);

                categoryRepository.Update(category);

                await categoryRepository.SaveAsync();

                return new ApiResponse(category, StatusCodes.Status200OK, CATEGORY.UPDATED_SUCESSFULLY, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }

        public async Task<ApiResponse> GetById(long id, UserResponse requestor)
        {
            try
            {
                if (requestor is null)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, Keys.REQUESTOR_DOES_NOT_EXISTS, null);
                }

                var category = await categoryRepository.GetById(id);
                if (category is null)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, string.Format(Keys.DOES_NOT_EXISTS, Keys.CATEGORY), null);
                }

                return new ApiResponse(category, StatusCodes.Status200OK, Keys.CATEGORY, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }

        public async Task<ApiResponse> GetListForOwner()
        {
            try
            {
                var category = await categoryRepository.GetListForUser();

                return new ApiResponse(category, StatusCodes.Status200OK, Keys.CATEGORY, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }
    }
}