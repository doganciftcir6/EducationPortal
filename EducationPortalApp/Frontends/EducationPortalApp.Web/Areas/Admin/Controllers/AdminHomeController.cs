using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationPortalApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/AdminHome/{action}/{id?}")]
    [Authorize(Roles = "Admin")]
    public class AdminHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
