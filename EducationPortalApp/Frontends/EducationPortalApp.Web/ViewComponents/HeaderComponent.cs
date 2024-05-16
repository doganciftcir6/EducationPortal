using Microsoft.AspNetCore.Mvc;

namespace EducationPortalApp.Web.ViewComponents
{
    public class HeaderComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
