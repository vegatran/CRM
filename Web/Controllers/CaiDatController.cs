using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;

namespace Web.Controllers
{
    public class CaiDatController : Controller
    {
        public CaiDatController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult HeThong()
        {
            return View();
        }

        public IActionResult NguoiDung()
        {
            return View();
        }

        public IActionResult Backup()
        {
            return View();
        }
    }
}
