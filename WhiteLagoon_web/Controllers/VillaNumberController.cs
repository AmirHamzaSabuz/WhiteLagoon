using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon_Domain.Entities;
using WhiteLagoon_Infrastructure.Data;

namespace WhiteLagoon_web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _db;

        public VillaNumberController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var villaNumbers = _db.VillaNumbers.ToList();
            return View(villaNumbers);
        }

        public IActionResult Create()
        {
            IEnumerable<SelectListItem> list = _db.Villas.ToList().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            ViewData["VillaList"] = list;
            return View();
        }

        [HttpPost]
        public IActionResult Create(VillaNumber obj)
        {
            //ModelState.Remove("Villa");
            if (ModelState.IsValid)
            {
                _db.VillaNumbers.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "The villa Number has been created successfully.";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Update(int villa_NumberId)
        {
            VillaNumber? obj = _db.VillaNumbers.FirstOrDefault(u => u.Villa_Number == villa_NumberId);
            
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }


        [HttpPost]
        public IActionResult Update(VillaNumber obj)
        {
            if (ModelState.IsValid && obj.Villa_Number > 0)
            {
                _db.VillaNumbers.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "The villa number has been updated successfully.";
                return RedirectToAction("Index");
            }
            return View();
        }



        public IActionResult Delete(int villa_NumberId)
        {
            VillaNumber? obj = _db.VillaNumbers.FirstOrDefault(u => u.Villa_Number == villa_NumberId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }


        [HttpPost]
        public IActionResult Delete(VillaNumber obj)
        {
            VillaNumber? objFromDb = _db.VillaNumbers.FirstOrDefault(u => u.Villa_Number == obj.Villa_Number);
            if (objFromDb is not null)
            {
                _db.VillaNumbers.Remove(objFromDb);
                _db.SaveChanges();
                TempData["success"] = "The villa number has been deleted successfully.";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The villa could not be deleted.";
            return View();
        }
    }
}
