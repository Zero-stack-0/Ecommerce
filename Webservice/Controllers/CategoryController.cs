using System.Security.Claims;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Dto.Request.Admin.Category;
using Service.Interface;
using Webservice.Helper;

namespace Webservice.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly CookieUserDetailsHandler cookieUserDetailsHandler;
        public CategoryController(ICategoryService categoryService, CookieUserDetailsHandler cookieUserDetailsHandler)
        {
            this.categoryService = categoryService;
            this.cookieUserDetailsHandler = cookieUserDetailsHandler;
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCategoryRequest dto)
        {
            dto.Requestor = await cookieUserDetailsHandler.GetUserDetail(User.Identity as ClaimsIdentity);
            var data = await categoryService.Add(dto);

            TempData["apiResponseMessage"] = data.Message;
            TempData["apiResponseStatusCode"] = data.StatusCodes.ToString();

            return RedirectToAction("List");
        }

        public async Task<IActionResult> Delete(DeleteRequest dto)
        {
            dto.Requestor = await cookieUserDetailsHandler.GetUserDetail(User.Identity as ClaimsIdentity);
            var data = await categoryService.Delete(dto);

            TempData["apiResponseMessage"] = data.Message;
            TempData["apiResponseStatusCode"] = data.StatusCodes.ToString();

            return RedirectToAction("List");
        }

        public async Task<IActionResult> Update(long categoryId)
        {
            var requestor = await cookieUserDetailsHandler.GetUserDetail(User.Identity as ClaimsIdentity);
            var data = await categoryService.GetById(categoryId, requestor);
            return View((Category)data.Result);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateRequest dto)
        {
            dto.Requestor = await cookieUserDetailsHandler.GetUserDetail(User.Identity as ClaimsIdentity);
            var data = await categoryService.Update(dto);

            TempData["apiResponseMessage"] = data.Message;
            TempData["apiResponseStatusCode"] = data.StatusCodes.ToString();

            return RedirectToAction("List");
        }

        public async Task<JsonResult> GetCategoryList(GetCategoryListRequest dto)
        {
            dto.Requestor = await cookieUserDetailsHandler.GetUserDetail(User.Identity as ClaimsIdentity);
            if (dto.Requestor is null)
            {
                return Json(null);
            }

            var result = await categoryService.GetList(dto);

            var json = Json(result);
            return Json(json);
        }

        public async Task<JsonResult> GetCategoryForUser()
        {
            var result = await categoryService.GetListForOwner();

            var json = Json(result);
            return Json(json);
        }
        public IActionResult List()
        {
            return View();
        }
    }
}