using AutoMapper;
using Data.Repository.Interface;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Service.Dto.Request.Order;
using Service.Dto.Response;
using Service.Helper;
using Service.Interface;
using static Entities.Constants;

namespace Service
{
    public class OrderService : IOrderService
    {
        private readonly ICommonService commonService;
        private readonly ICartRepository cartRepository;
        private readonly PaymentService paymentService;
        private readonly IMapper mapper;
        public OrderService(ICommonService commonService, ICartRepository cartRepository, PaymentService paymentService, IMapper mapper)
        {
            this.commonService = commonService;
            this.cartRepository = cartRepository;
            this.paymentService = paymentService;
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

                var cart = await cartRepository.GetById(dto.CartId);
                if (cart is null)
                {
                    return ResponseMessage.BadRequest(string.Format(Keys.DOES_NOT_EXISTS, Keys.CART));
                }

                if (cart.AddedById != dto.Requestor.Id)
                {
                    return ResponseMessage.BadRequest(Keys.FORBIDDEN);
                }

                if (cart.Product.Quantity < cart.Quantity)
                {
                    return ResponseMessage.BadRequest(ORDER.OUT_OF_STOCK);
                }

                var order = new Order(dto.Requestor.Id, cart.Product.Id, cart.Product.Name, cart.Product.Description, cart.Product.Price, cart.Product.Discount, cart.Quantity, OrderStatus.Pending, dto.ShippingAddress, dto.PaymentMethod);

                order.PaymentIntentId = paymentService.CreatePaymentIntendId(order.TotalAmount);

                cart.Delete();

                cartRepository.Add(order);

                await cartRepository.SaveAsync();

                return ResponseMessage.Sucess(order, "Order complemeted sucessfully", null);
            }
            catch (Exception ex)
            {
                await commonService.RegisterException(ex);
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }

        public async Task<ApiResponse> GetPaymentIntentId(CreatePaymentIntentRequest dto)
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
                    return ResponseMessage.BadRequest(string.Format(Keys.DOES_NOT_EXISTS, Keys.CART));
                }

                if (cart.AddedById != dto.Requestor.Id)
                {
                    return ResponseMessage.BadRequest(Keys.FORBIDDEN);
                }

                if (cart.Product.Quantity < cart.Quantity)
                {
                    return ResponseMessage.BadRequest(ORDER.OUT_OF_STOCK);
                }

                var paymentIntent = paymentService.CreatePaymentIntendId(cart.Quantity * (cart.Product.Price - (cart.Product.Price * cart.Product.Discount / 100)));

                return ResponseMessage.Sucess(paymentIntent, Keys.PAYMENT_INTENT, null);
            }

            catch (Exception ex)
            {
                await commonService.RegisterException(ex);
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }
    }
}