using Microsoft.AspNet.Identity;
using RobeazyCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RobeazyCore.Models;

namespace RobeazyCore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ViewUserAccount(string userName)
        {
            var viewModel = _userService.GetUserViewAccountModel(userName);
            if (viewModel == null)
            {
                return Json(new { success = false, responseText = "User was not found in the database" });
            }
            return View("~/Views/Account/ViewAccount.cshtml", viewModel);
            //return Json(new { success = true, responseText = "User account info found", accountInfo = viewModel }, JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        [Authorize]
        [AllowAnonymous]
        public ActionResult ViewMyAccount()
        {
            var viewModel = _userService.GetUserViewAccountModel();
            if (viewModel == null)
            {
                return RedirectToAction("Index", "Home");
                //return Json(new { success = false, responseText = "User was not found in the database" }, JsonRequestBehavior.DenyGet);
            }
            return View("~/Views/Account/MyAccount.cshtml", viewModel);
            //return Json(new { success = true, responseText = "User account info found", accountInfo = viewModel }, JsonRequestBehavior.DenyGet);
        }

        [HttpPut]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateMyAccount(EditAccountModel account)
        {
            if (_userService.User == null)
            {
                return Json(new { success = false, responseText = "User was not found in the database" });
            }
            if (_userService.User.UserName != account.UserName)
            {
                return Json(new { success = false, responseText = "Only the account holder can edit their profile" });
            }

            _userService.UpdateUserAccount(account);

            return Json(new { success = true, responseText = "User account info updated", accountInfo = account });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //if (_userManager != null)
                //{
                //    _userManager.Dispose();
                //    _userManager = null;
                //}

                //if (_signInManager != null)
                //{
                //    _signInManager.Dispose();
                //    _signInManager = null;
                //}
            }

            base.Dispose(disposing);
        }
    }
}