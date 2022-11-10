using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Test.Models;

namespace Test.Controllers
{
    public class TestController : Controller
    {
        private readonly ILogger<TestController> _logger;
        private readonly IWebHostEnvironment _appEnvironment;
        //private readonly ApplicationContext _context;

        public TestController(ILogger<TestController> logger, IWebHostEnvironment appEnvironment)
        {
            _logger = logger;
            _appEnvironment = appEnvironment;
        }

        public IActionResult Test()
        {
            return View();
        }

        public string RetString()
        {
            return "Hello world";
        }

        public IActionResult RetHTML()
        {
            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = "<html><body>Hello</body></html>"
            };
        }
        public IActionResult RetEmpty()
        {
            return new EmptyResult();
        }
        public IActionResult RetEmptyContent()
        {
            return new NoContentResult();
        }
        //public IActionResult RetFile()
        //{
        //    return new FileResult(); // FileContentResult VirtualFileResult FileStringResult
        //}
        public IActionResult RetObject()
        {
            return new ObjectResult(new Person("Sasha", 30));
        }
        record class Person(string Name, int Age);
        public JsonResult GetJson()
        {
            return Json("Sasha");
        }
        public IActionResult GetJsonAction()
        {
            Person person = new Person("Sasha", 30);
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
            };
            //string json = JsonSerializer.Serialize<Person>(person, jsonOptions);
            return Json(person, jsonOptions);
        }
        public IActionResult GetContent()
        {
            return Content("Hello");
        }
        public IActionResult GetStatus404()
        {
            return StatusCode(400);
        }
        public IActionResult GetStatusError()
        {
            return NotFound("Ресурс в приложении не найден");
            //return NotFoundResult();  
        }

        public IActionResult GetAuth(int age)
        {
            //if (age < 18)
            //    return Unauthorized();
            //return Content("Проверка пройдена");
            if (age < 18)
                return Unauthorized(new Error { Message = "параметр age содержит недействительное значение" });
            return Content("Проверка пройдена");
        }
        class Error
        {
            public string Message { get; set; }
        }

        public IActionResult GetBad(string? name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest("Не указаны параметры запроса");
            return Content($"Name: {name}");
        }
        public IActionResult GetFile()
        {
                //_appEnvironment.ContentRootPath
            // Путь к файлу
            string file_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files/file.pdf");
            // Тип файла - content-type
            string file_type = "application/pdf";
            // Имя файла - необязательно
            string file_name = "file.pdf";
            return PhysicalFile(file_path, file_type, file_name);
        }
        // Отправка массива байтов
        public FileResult GetBytes()
        {
            string path = Path.Combine(_appEnvironment.ContentRootPath, "Files/file.pdf");
            byte[] mas = System.IO.File.ReadAllBytes(path);
            string file_type = "application/pdf";
            string file_name = "file.pdf";
            return File(mas, file_type, file_name);
        }
        // Отправка потока
        public FileResult GetStream()
        {
            string path = Path.Combine(_appEnvironment.ContentRootPath, "Files/file.pdf");
            FileStream fs = new FileStream(path, FileMode.Open);
            string file_type = "application/pdf";
            string file_name = "file.pdf";
            return File(fs, file_type, file_name);
        }
        public VirtualFileResult GetVirtualFile()
        {
            var filepath = Path.Combine("~/Files", "text.txt");
            return File(filepath, "text/plain", "text.txt");
        }

        public IActionResult GetRedirect() => Content("GetFile");
    }
}
