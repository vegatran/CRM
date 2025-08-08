using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;

namespace Web.Controllers
{
    public class BaoCaoController : Controller
    {
        public BaoCaoController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ThongKeBanHang()
        {
            return View();
        }

        public IActionResult ThongKeNhapKho()
        {
            return View();
        }

        public IActionResult ThongKeTonKho()
        {
            return View();
        }

        public IActionResult ThongKeDoanhThu()
        {
            return View();
        }
    }
}
