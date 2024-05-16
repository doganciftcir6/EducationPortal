using Microsoft.AspNetCore.Mvc;

namespace EducationPortalApp.Web.ViewComponents
{
    public class FooterComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
