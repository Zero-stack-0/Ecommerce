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
    public class OrderService : IOrderService
    {
        private readonly ICommonService commonService;
        public OrderService(ICommonService commonService)
        {
            this.commonService = commonService;
        }
    }
}