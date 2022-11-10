using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;
using Test.Models;

namespace Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }
        //public async Task<IActionResult> Index(Guid id)
        //{
        //    var personal = await _context.Personal.FirstOrDefaultAsync(m => m.Id == id);

        //    return View();
        //}
        public IActionResult Index()
        {
            List<PersonalInformations> person = (from m in _context.Personal select m).ToList();
            return View(person);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreatePersonalInformation()
        {
            return View();
        }
        // crud
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePersonalInformation([Bind("Login,Password,FirstName,LastName,Gender,YearOfBirth")] PersonalInformations information)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(information);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }

            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " + "Try again or call system admin");
            }

            return View(information);
        }

        

        public async Task<IActionResult> TableOfInformations(string searched)
        {
            //string searched
            var searchPerson = from m in _context.Personal select m; //LINQ 

            if (!String.IsNullOrEmpty(searched))
            {
                searchPerson = searchPerson.Where(s => s.FirstName.Contains(searched));
            }

            return View(await searchPerson.ToListAsync());
            //return View(await _context.Personal.ToListAsync());
        }

        public IActionResult GetJsonAction()
        {
            List<PersonalInformations> person = (from m in _context.Personal select m).ToList();
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
            };
            return Json(person, jsonOptions);
        }

        //Edit or Update
        [HttpPost, ActionName("EditPersonalInformation")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPostPersonalInformation(Guid? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var personalInformations = await _context.Personal.FirstOrDefaultAsync(s => s.Id == Id);


            if (await TryUpdateModelAsync<PersonalInformations>(
                personalInformations, "", s => s.Login, s => s.Password, s => s.FirstName, s => s.LastName, s => s.Gender, s => s.YearOfBirth))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again or call system admin");
                }
            }

            return View(personalInformations);
        }


        public async Task<IActionResult> EditPersonalInformation(Guid Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var person = await _context.Personal.FirstOrDefaultAsync(m => m.Id == Id);

            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        //Details
        public async Task<IActionResult> DetailsOfPerson(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Personal.FirstOrDefaultAsync(m => m.Id == id);

            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        


        //Delete 
        public async Task<IActionResult> DeletePerson(Guid id, bool? Savechangeserror = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Personal.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (person == null)
            {
                return NotFound();
            }

            if (Savechangeserror.GetValueOrDefault())
            {
                ViewData["DeleteError"] = "Delete failed, please try again later ... ";
            }

            return View(person);
        }

        //Delete continue
        [HttpPost, ActionName("DeletePerson")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDeletePerson(Guid id)
        {
            var person = await _context.Personal.FindAsync(id);

            if (person == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Personal.Remove(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(DeletePerson), new { id = id, Savechangeserror = true });
            }

            //return View(person);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Partial View
        public async Task<IActionResult> PartialView()
        {
            return PartialView("PartialView");
        }
    }
}