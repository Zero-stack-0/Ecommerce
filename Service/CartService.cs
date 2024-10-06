using AutoMapper;
using Data.Repository.Interface;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Service.Dto.Request.Cart;
using Service.Dto.Response;
using Service.Helper;
using Service.Interface;
using static Entities.Constants;

namespace Service
{
    public class CartService : ICartService
    {
        private readonly ICommonService commonService;
        private readonly IProductRepository productRepository;
        private readonly ICartRepository cartRepository;
        private readonly IMapper mapper;
        public CartService(ICommonService commonService, IProductRepository productRepository, ICartRepository cartRepository, IMapper mapper)
        {
            this.commonService = commonService;
            this.cartRepository = cartRepository;
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task<ApiResponse> Add(AddRequest dto)
        {
            try
            {
                if (dto.Requestor is null)
                {
                    return ResponseMessage.RequestorDoesNotExists();
                }

                var product = await productRepository.GetById(dto.ProductId);
                if (product is null)
                {
                    return ResponseMessage.BadRequest(PRODUCT.PRODUCT_DOES_NOT_EXISTS);
                }

                if (dto.Quantity > product.MaxOrderQuantity)
                {
                    return ResponseMessage.BadRequest(CART.ITEM_QUANTITY_SHOULD_NOT_BE_GREATER_THAN_MAX_ORDER_QUANTITY);
                }

                var cartAlreadyExists = await cartRepository.GetByProdcutIdAndAddedById(product.Id, dto.Requestor.Id);
                if (cartAlreadyExists is not null)
                {
                    if (cartAlreadyExists.Quantity + dto.Quantity > product.Quantity)
                    {
                        return ResponseMessage.BadRequest(CART.ITEM_QUANTITY_SHOULD_NOT_BE_GREATER_THAN_PRODUCT_QUANTITY);
                    }

                    else
                    {
                        cartAlreadyExists.Quantity += dto.Quantity;
                        cartRepository.Update(cartAlreadyExists);
                        await cartRepository.SaveAsync();

                        return ResponseMessage.Sucess(cartAlreadyExists, CART.UPDATED_SUCESSFULLY, null);
                    }
                }

                var cart = new Cart(product.Id, dto.Requestor.Id, dto.Quantity, product.Price, product.Discount);

                cartRepository.Add(cart);

                await cartRepository.SaveAsync();

                return ResponseMessage.Sucess(cart, CART.ADDED_SUCESSFULLY, null);
            }
            catch (Exception ex)
            {
                await commonService.RegisterException(ex);
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }

        public async Task<ApiResponse> GetList(GetListRequest dto)
        {
            try
            {
                if (dto.Requestor is null)
                {
                    return ResponseMessage.RequestorDoesNotExists();
                }

                var (carts, totalCount) = await cartRepository.GetByAddedById(dto.Requestor.Id, dto.SearchTerm, dto.PageNo, dto.PageSize);

                var cartResponses = carts.Select(mapper.Map<CartResponse>).ToList();

                foreach (var cart in cartResponses)
                {
                    cart.FinalPriceAfterDiscount = cart.Product.Price - (cart.Product.Price * cart.Product.Discount / 100);
                }

                return ResponseMessage.Sucess(cartResponses, Keys.CART, new PagedData(dto.PageNo, dto.PageSize, totalCount, (int)Math.Ceiling((double)totalCount / dto.PageSize)));
            }
            catch (Exception ex)
            {
                await commonService.RegisterException(ex);
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }
    }
}