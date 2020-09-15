using Microsoft.AspNetCore.Mvc;

namespace TradingViewUdfProvider.Example.Controllers
{
    public class GuiController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
