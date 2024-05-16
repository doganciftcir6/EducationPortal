using Microsoft.AspNetCore.Mvc;

namespace EducationPortalApp.Web.ViewComponents
{
    public class AdminSidebarComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
