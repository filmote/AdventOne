using System.Web.Mvc;

namespace AdventOne.Controllers {
    public class UnauthorisedController : Controller
    {
        // GET: Unauthorised
        public ActionResult Index()
        {
            return View();
        }
    }
}