using Data.Repository.Interface;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Service.Dto.Request.Product;
using Service.Helper;
using Service.Interface;
using static Entities.Constants;

namespace Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly ISellerStoreInfoRepository sellerStoreInfoRepository;
        public ProductService(IProductRepository productRepository, ISellerStoreInfoRepository sellerStoreInfoRepository)
        {
            this.productRepository = productRepository;
            this.sellerStoreInfoRepository = sellerStoreInfoRepository;
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

                var product = new Product(dto.Name, dto.Description, dto.SKU, dto.Quantity, dto.Price, dto.Discount, dto.MaxOrderQuantity, dto.CategoryId, dto.Requestor.Id, sellerStoreInfo.Id, dto.ImageUrl);

                productRepository.Add(product);

                await productRepository.SaveAsync();

                return new ApiResponse(null, StatusCodes.Status200OK, PRODUCT.PRODUCT_ADDED_SUCESSFULLY, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }
    }
}