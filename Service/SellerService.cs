using AutoMapper;
using Azure;
using Data.Repository.Interface;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Service.Dto;
using Service.Dto.Request;
using Service.Dto.Request.Admin;
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
        private readonly SendInBlueEmailNotificationService sendInBlueEmailNotificationService;
        private readonly ISellerStoreInfoRepository sellerStoreInfoRepository;
        private readonly ICommonService commonService;
        public SellerService(ISellerRequestRepository sellerRequestRepository, IMapper mapper, SendInBlueEmailNotificationService sendInBlueEmailNotificationService, ISellerStoreInfoRepository sellerStoreInfoRepository, ICommonService commonService)
        {
            this.sellerRequestRepository = sellerRequestRepository;
            this.mapper = mapper;
            this.sendInBlueEmailNotificationService = sendInBlueEmailNotificationService;
            this.sellerStoreInfoRepository = sellerStoreInfoRepository;
            this.commonService = commonService;
        }

        public async Task<ApiResponse> Request(SellerRequestDto dto)
        {
            try
            {
                if (dto.Requestor is null)
                {
                    return ResponseMessage.RequestorDoesNotExists();
                }

                if (!dto.Requestor.IsEmailVerified)
                {
                    return ResponseMessage.BadRequest(Account.VERIFY_EMAIL_ID);
                }

                var doessellerRequestExists = await sellerRequestRepository.GetByUserId(dto.Requestor.Id);
                if (doessellerRequestExists is not null)
                {
                    return ResponseMessage.BadRequest(SELLER.REQUEST_ALREADY_EXISTS);
                }

                var sellerRequest = new SellerRequest(dto.Requestor.Id, dto.StoreName, dto.StoreAddress, dto.StoreContactNumber, dto.PaymentMethod);

                sellerRequestRepository.Add(sellerRequest);

                await sellerRequestRepository.SaveAsync();

                return ResponseMessage.Sucess(mapper.Map<SellerRequestResponse>(sellerRequest), SELLER.REQEUST_SUBMITTED, null);
            }
            catch (Exception ex)
            {
                await commonService.RegisterException(ex);
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }

        public async Task<ApiResponse> Get(UserResponse? requestor)
        {
            try
            {
                if (requestor is null)
                {
                    return ResponseMessage.RequestorDoesNotExists();
                }

                if (!requestor.IsEmailVerified)
                {
                    return ResponseMessage.BadRequest(Account.VERIFY_EMAIL_ID);
                }

                var doessellerRequestExists = await sellerRequestRepository.GetByUserId(requestor.Id);
                if (doessellerRequestExists is null)
                {
                    return ResponseMessage.BadRequest(string.Format(Keys.DOES_NOT_EXISTS, SELLER.SELLET_REQUEST));
                }

                return ResponseMessage.Sucess(mapper.Map<SellerRequestResponse>(doessellerRequestExists), SELLER.SELLET_REQUEST, null);
            }

            catch (Exception ex)
            {
                await commonService.RegisterException(ex);
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }

        }

        public async Task<ApiResponse> UpdateRequestStatus(UpdateSellerRequestStatus dto)
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

                var sellerRequest = await sellerRequestRepository.GetById(dto.SellerRequestId);
                if (sellerRequest is null)
                {
                    return ResponseMessage.BadRequest(string.Format(Keys.DOES_NOT_EXISTS, SELLER.SELLET_REQUEST));
                }

                if (sellerRequest.Status != SellerReqeustStatus.Pending)
                {
                    return ResponseMessage.BadRequest(SELLER.ONLY_STATUS_OF_PENDING_REQUEST_CAN_CHANGE);
                }

                sellerRequest.UpdateStatus(dto.Status);

                if (dto.Status == SellerReqeustStatus.Accepted)
                {
                    sellerRequest.User.UpdateSeller();
                    var sellerStoreInfo = new SellerStoreInfo(sellerRequest.StoreName, sellerRequest.StoreAddress, sellerRequest.StoreContactNumber, sellerRequest.PaymentMethod, sellerRequest.UserId);
                    sellerStoreInfoRepository.Add(sellerStoreInfo);
                }

                await sellerRequestRepository.SaveAsync();

                SendUpdatedStatusEmail(sellerRequest.User, dto.Status.ToString());

                return ResponseMessage.Sucess(sellerRequest, SELLER.SELLET_REQUEST_UPDATED_SUCESSFULLY, null);
            }

            catch (Exception ex)
            {
                await commonService.RegisterException(ex);
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }

        public async Task<ApiResponse> GetRequestList(GetSellerRequestList dto)
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

                var (sellerRequests, totalCount) = await sellerRequestRepository.GetRequests(dto.PageNo, dto.PageSize, dto.SearchTerm, dto.Status);

                return ResponseMessage.Sucess(sellerRequests.Select(mapper.Map<SellerRequestResponse>).ToList(), Keys.SELLER_REQUEST, new PagedData(dto.PageNo, dto.PageSize, totalCount, (int)Math.Ceiling((double)totalCount / dto.PageSize)));
            }
            catch (Exception ex)
            {
                await commonService.RegisterException(ex);
                return new ApiResponse(null, StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }

        private void SendUpdatedStatusEmail(Users user, string status)
        {
            var subject = $"Hello {user.FirstName}!";

            string htmlContent;
            if (status == "Accepted")
            {
                htmlContent = $"<p>Hello {user.FirstName}, your seller reqeust has been {status}";
            }
            else
            {
                htmlContent = $"<p>Hello {user.FirstName}, We regret to inform you that your seller reqeust has {status}";
            }


            sendInBlueEmailNotificationService.SendEmail(user.EmailId, user.FirstName, subject, htmlContent);
        }
    }
}