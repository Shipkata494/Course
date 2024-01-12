namespace Course.Controllers
{
    using Course.Models;
    using Course.Services;
    using Course.Services.Interfaces;
    using Course.ViewModel;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
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
            return RedirectToAction("SaveToCsv", model);
        }
        public async Task<IActionResult> Result(ServiceModel serviceModel)
        {
            var viewModel = await studentService.GetStudentsAsync(serviceModel);
            return View(viewModel);
        }
        public async Task<IActionResult> SaveToCsv(ServiceModel serviceModel)
        {
            ICollection<StudentViewModel> studentViewModels = await studentService.GetStudentsAsync(serviceModel);
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            string relativePath = "App_Data";
            string directoryPath = Path.Combine(rootPath, relativePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }            
            string fileName = "report.csv";
            string filePath = Path.Combine(directoryPath, fileName);
            studentService.SaveToCsv(filePath, studentViewModels);

            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
