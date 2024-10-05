using AutoMapper;
using Data.Repository.Interface;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Service.Dto;
using Service.Dto.Request.Product;
using Service.Dto.Response;
using Service.Helper;
using Service.Interface;
using static Entities.Constants;

namespace Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly ISellerStoreInfoRepository sellerStoreInfoRepository;
        private readonly IMapper mapper;
        private readonly ICommonService commonService;
        private readonly ICategoryRepository categoryRepository;
        public ProductService(IProductRepository productRepository, ISellerStoreInfoRepository sellerStoreInfoRepository, IMapper mapper, ICommonService commonService, ICategoryRepository categoryRepository)
        {
            this.productRepository = productRepository;
            this.sellerStoreInfoRepository = sellerStoreInfoRepository;
            this.mapper = mapper;
            this.commonService = commonService;
            this.categoryRepository = categoryRepository;
        }

        public async Task<ApiResponse> Add(AddRequest dto)
        {
            try
            {
                if (dto.Requestor is null)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, Keys.REQUESTOR_DOES_NOT_EXISTS, null);
                }

                if (!dto.Requestor.IsEmailVerified)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, Account.VERIFY_EMAIL_ID, null);
                }

                if (!dto.Requestor.IsSeller)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, Keys.FORBIDDEN, null);
                }

                var sellerStoreInfo = await sellerStoreInfoRepository.GetByUserId(dto.Requestor.Id);
                if (sellerStoreInfo is null)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, Keys.FORBIDDEN, null);
                }

                var doesProductAlreadyExists = await productRepository.GetByUserIdAndSku(dto.Requestor.Id, dto.SKU);
                if (doesProductAlreadyExists is not null)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, PRODUCT.PRODUCT_ALREADY_EXISTS_WITH_SKU, null);
                }

                if (dto.MaxOrderQuantity > dto.Quantity)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, PRODUCT.QUANTITY_SHOULD_BE_GREATER_THAN_MAX_ORDER_QUANTITY, null);
                }

                var category = await categoryRepository.GetById(dto.CategoryId);
                if (category is null)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, CATEGORY.DOES_NOT_EXISTS, null);
                }

                var product = new Product(dto.Name, dto.Description, dto.SKU, dto.Quantity, dto.Price, dto.Discount, dto.MaxOrderQuantity, dto.CategoryId, dto.Requestor.Id, sellerStoreInfo.Id, dto.ImageUrl);

                productRepository.Add(product);

                await productRepository.SaveAsync();

                return new ApiResponse(null, StatusCodes.Status200OK, PRODUCT.PRODUCT_ADDED_SUCESSFULLY, null);
            }
            catch (Exception ex)
            {
                await commonService.RegisterException(ex);
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }

        public async Task<ApiResponse> GetListByCreatedById(string searchTerm, int categoryId, UserResponse requestor)
        {
            try
            {
                if (requestor is null)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, Keys.REQUESTOR_DOES_NOT_EXISTS, null);
                }

                if (!requestor.IsEmailVerified)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, Account.VERIFY_EMAIL_ID, null);
                }

                if (!requestor.IsSeller)
                {
                    return new ApiResponse(null, StatusCodes.Status400BadRequest, Keys.FORBIDDEN, null);
                }

                var products = await productRepository.GetListByCreatedById(searchTerm, categoryId, requestor.Id);

                return new ApiResponse(products.Select(mapper.Map<ProductResponse>).ToList(), StatusCodes.Status200OK, PRODUCT.PRODUCT_ADDED_SUCESSFULLY, null);
            }
            catch (Exception ex)
            {
                await commonService.RegisterException(ex);
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }

        public async Task<ICollection<ProductResponse>> GetOpenList(string searchTerm, int categoryId)
        {
            try
            {
                var products = await productRepository.GetList(searchTerm, categoryId);
                return products.Select(mapper.Map<ProductResponse>).ToList();
            }

            catch (Exception ex)
            {
                await commonService.RegisterException(ex);
                return new List<ProductResponse>();
            }
        }

        public async Task<ApiResponse> GetById(long id)
        {
            try
            {
                return new ApiResponse(mapper.Map<ProductResponse>(await productRepository.GetById(id)), StatusCodes.Status200OK, Keys.PRODUCT, null);
            }
            catch (Exception ex)
            {
                await commonService.RegisterException(ex);
                return new ApiResponse(new ProductResponse(), StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }
    }
}