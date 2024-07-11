using AutoMapper;
using Data.Repository.Interface;
using Entities;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Service.Dto;
using Service.Dto.Request;
using Service.Dto.Response;
using Service.Helper;
using Service.Interface;
using static Entities.Constants;

namespace Service
{
    public class SellerService : ISellerService
    {
        private readonly ISellerRequestRepository sellerRequestRepository;
        private readonly IMapper mapper;
        public SellerService(ISellerRequestRepository sellerRequestRepository, IMapper mapper)
        {
            this.sellerRequestRepository = sellerRequestRepository;
            this.mapper = mapper;
        }

        public async Task<ApiResponse> Request(SellerRequestDto dto)
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

                var doessellerRequestExists = await sellerRequestRepository.GetByUserId(dto.Requestor.Id);
                if (doessellerRequestExists is not null)
                {
                    return new ApiResponse(mapper.Map<SellerRequestResponse>(doessellerRequestExists), StatusCodes.Status400BadRequest, SELLER.REQUEST_ALREADY_EXISTS, null);
                }

                var sellerRequest = new SellerRequest(dto.Requestor.Id, dto.StoreName, dto.StoreAddress, dto.StoreContactNumber, dto.PaymentMethod);

                sellerRequestRepository.Add(sellerRequest);

                await sellerRequestRepository.SaveAsync();

                return new ApiResponse(mapper.Map<SellerRequestResponse>(sellerRequest), StatusCodes.Status200OK, SELLER.REQEUST_SUBMITTED, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }

        public async Task<ApiResponse> Get(UserResponse? requestor)
        {
            if (requestor is null)
            {
                return new ApiResponse(null, StatusCodes.Status400BadRequest, Keys.REQUESTOR_DOES_NOT_EXISTS, null);
            }

            if (!requestor.IsEmailVerified)
            {
                return new ApiResponse(null, StatusCodes.Status400BadRequest, Account.VERIFY_EMAIL_ID, null);
            }

            var doessellerRequestExists = await sellerRequestRepository.GetByUserId(requestor.Id);
            if (doessellerRequestExists is null)
            {
                return new ApiResponse(null, StatusCodes.Status400BadRequest, string.Format(Keys.DOES_NOT_EXISTS, SELLER.SELLET_REQUEST), null);
            }

            return new ApiResponse(mapper.Map<SellerRequestResponse>(doessellerRequestExists), StatusCodes.Status200OK, SELLER.SELLET_REQUEST, null);
        }
    }
}