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


        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IStudentService _studentService)
        {
            _logger = logger;
            studentService = _studentService;
        }

        public IActionResult Index()
        {   
           
            return View(); 
        }
        [HttpPost]
        public IActionResult Index(ServiceModel model)
        {
            return RedirectToAction("Res", model);
        }
        public async Task<IActionResult> Res(ServiceModel serviceModel)
        {
            var a = await studentService.GetStudentsAsync(serviceModel);
            return RedirectToAction("Result", a);
        }
        public IActionResult Result(ICollection<StudentViewModel> viewModels)
        {

            return View(viewModels);
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
