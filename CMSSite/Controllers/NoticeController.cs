using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSite.Controllers
{
    [Authorize]
    public class NoticeController : Controller
    {
        //public NoticeController(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        //{
        //}

        public IActionResult ForbiddenAccess()
        {
            return View("Unauthorization");
        }
    }
}
