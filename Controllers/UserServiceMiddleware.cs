using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobeazyCore.Services;
using System.Threading.Tasks;

namespace RobeazyCore.Controllers
{
    public class UserServiceMiddleware
    {
        private readonly RequestDelegate _next;

        public UserServiceMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, Controller controller, IUserService userService)
        {
             userService.SetUser(controller.User.Identity.GetUserId());
            await _next(context);
        }
    }
}