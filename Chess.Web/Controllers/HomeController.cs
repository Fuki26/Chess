using Chess.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chess.Controllers
{
    public class HomeController : Microsoft.AspNetCore.Mvc.Controller
    {
        public static Board boardModel = new Board();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Chat()
        {
            return View();
        }

    }
}
