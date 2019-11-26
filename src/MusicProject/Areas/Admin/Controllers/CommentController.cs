using DNTBreadCrumb.Core;
using DNTCommon.Web.Core;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicProject.DataLayer.Context;
using MusicProject.Entities.Comment;
using MusicProject.Services.Contracts.Content;
using MusicProject.Services.Identity;
using MusicProject.ViewModels.Comment;
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Security.Claims;

namespace MusicProject.Areas.Admin.Controllers
{

    [Area(AreaConstants.AdminArea)]
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [BreadCrumb(UseDefaultRouteUrl = true, Order = 0)]

    [DisplayName("مدیریت نظرات")]
    public class CommentController : Controller
    {
        private readonly ClaimsPrincipal _claimsPrincipal;
        private const string RoleNotFound = "رکورد درخواستی یافت نشد.";
        private readonly ICategoryCommentService _CategoryComment;
        private readonly ICommentService _commentService;
        private readonly IContentListCommentService _contentListComment;
        private readonly IUnitOfWork _uow;
        private const int DefaultPageSize = 20;

        public CommentController(
          ICommentService commentService,
          IUnitOfWork uow,
            ICategoryCommentService categoryComment,
            IContentListCommentService contentListComment
          )
        {
            _claimsPrincipal = new ClaimsPrincipal();
            _CategoryComment = categoryComment;
            _commentService = commentService;
            _contentListComment = contentListComment;
            _uow = uow;
        }

        [BreadCrumb(Title = "/ایندکس", Order = 1)]
        [DisplayName("لیست نظرات")]
        public async Task<IActionResult> Index(
            int pageNumber = 1,
            int pageSize = -1,
            string sort = "desc",
            string title = "",
            string orderBy = "last",
            bool? ispublished = null,
            bool? isSelected = null,
            string name = null,
            string body = null,
            string fromTime = null, string toTime = null,
            int TypeId = 0
        )
        {
            ViewBag.Search = name + body + fromTime + toTime + TypeId;
            ViewBag.NumOfRow = (pageNumber - 1) * 10 + 1;
            ViewBag.CurrentDate = DateTime.Now.ToShortPersianDateString();
            var itemsPerPage = 10;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }
            var model = await _commentService.GetPagedCommentListAsync(
                pageNumber,
                itemsPerPage,
                sort == "desc" ? SortOrder.Descending : SortOrder.Ascending,
                TypeId,
                name,
                body,
                ispublished,
                fromTime, toTime
                ).ConfigureAwait(false);

            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            return View(model);
        }

        [HttpPost]
        [AjaxOnly]
        [DisplayName("انتشار نظر")]
        public async Task<JsonResult> UpdatePusblished([FromBody] PagedCommentListViewModel model)
        {
            int Id = model.Id;
            bool state = model.State;

            await _commentService.UpdatePublishedAsync(Id, state);

            return new JsonResult("");
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await _commentService.GetCommentDetailsAsync(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisplayName("حذف نظرات")]

        public async Task<IActionResult> Delete(int id, int pageNumber)
        {
            await _commentService.DeleteCommentAsync(id);

            return RedirectToAction("Index", new { pageNumber = pageNumber == 0 ? 1 : pageNumber });
        }

        [HttpPost]
        [AjaxOnly]
        [DisplayName("پاسخ نظرات")]

        public async Task<IActionResult> AddReplyCommentAsync(int id, string comment, int type)
        {
            Comment cmt = new Comment
            {
                Body = comment,
                ParentId = id,
                UserCommentId = 1,
            };
            if (type == 1)
                await _CategoryComment.AddNewCommentAsync(cmt);
            if (type == 2)
                await _contentListComment.AddNewCommentAsync(cmt);
            return new JsonResult("");
        }
    }
}
