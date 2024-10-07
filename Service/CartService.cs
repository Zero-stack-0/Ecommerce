using AutoMapper;
using Data.Repository.Interface;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Service.Dto;
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

                if (product.CreatedById == dto.Requestor.Id)
                {
                    return ResponseMessage.BadRequest(Keys.FORBIDDEN);
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
                    //price after deduction of discount
                    cart.FinalPriceAfterDiscount = cart.Quantity * (cart.Product.Price - (cart.Product.Price * cart.Product.Discount / 100));
                }

                return ResponseMessage.Sucess(cartResponses, Keys.CART, new PagedData(dto.PageNo, dto.PageSize, totalCount, (int)Math.Ceiling((double)totalCount / dto.PageSize)));
            }
            catch (Exception ex)
            {
                await commonService.RegisterException(ex);
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }

        //View Review on product on view detail screen pending
        public async Task<ApiResponse> GetDetail(long cartId, UserResponse? requestor)
        {
            try
            {
                if (requestor is null)
                {
                    return ResponseMessage.RequestorDoesNotExists();
                }

                var cart = await cartRepository.GetById(cartId);
                if (cart is null)
                {
                    return ResponseMessage.BadRequest(string.Format(Keys.DOES_NOT_EXISTS, CART.CART_ITEM));
                }

                if (cart.AddedById != requestor.Id)
                {
                    return ResponseMessage.BadRequest(Keys.FORBIDDEN);
                }

                return ResponseMessage.Sucess(mapper.Map<CartResponse>(cart), CART.CART_ITEM, null);
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

                var cart = await cartRepository.GetById(dto.CartId);
                if (cart is null)
                {
                    return ResponseMessage.BadRequest(string.Format(Keys.DOES_NOT_EXISTS, CART.CART_ITEM));
                }

                if (cart.AddedById != dto.Requestor.Id)
                {
                    return ResponseMessage.BadRequest(Keys.FORBIDDEN);
                }

                if (dto.Quantity > cart.Product.Quantity)
                {
                    return ResponseMessage.BadRequest(CART.ITEM_QUANTITY_SHOULD_NOT_BE_GREATER_THAN_PRODUCT_QUANTITY);
                }

                cart.Update(dto.Quantity);

                cartRepository.Update(cart);

                await cartRepository.SaveAsync();

                return ResponseMessage.Sucess(cart, CART.UPDATED_SUCESSFULLY, null);
            }
            catch (Exception ex)
            {
                await commonService.RegisterException(ex);
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }

        //Pop up pending on Clicking Delete
        public async Task<ApiResponse> Delete(long cartId, UserResponse? requestor)
        {
            try
            {
                if (requestor is null)
                {
                    return ResponseMessage.RequestorDoesNotExists();
                }

                var cart = await cartRepository.GetById(cartId);
                if (cart is null)
                {
                    return ResponseMessage.BadRequest(string.Format(Keys.DOES_NOT_EXISTS, CART.CART_ITEM));
                }

                if (cart.AddedById != requestor.Id)
                {
                    return ResponseMessage.BadRequest(Keys.FORBIDDEN);
                }

                cart.Delete();

                await cartRepository.SaveAsync();

                return ResponseMessage.Sucess(null, CART.DELETED_SUCESSFULLY, null);
            }

            catch (Exception ex)
            {
                await commonService.RegisterException(ex);
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }
    }
}