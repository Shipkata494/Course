using Course.Models;
using Course.Services.Interfaces;
using Course.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Course.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStudentService studentService;

        public HomeController(IStudentService _studentService)
        {
            studentService = _studentService;
        }

        public IActionResult Index()
        {   
           
            return View(); 
        }
        [HttpPost]
        public IActionResult Index(ServiceModel model)
        {
            return RedirectToAction("Result", model);
        }
        public async Task<IActionResult> Result(ServiceModel serviceModel)
        {
            var viewModel = await studentService.GetStudentsAsync(serviceModel);
            return View(viewModel);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
