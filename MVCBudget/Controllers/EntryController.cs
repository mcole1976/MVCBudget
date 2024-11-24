using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCBudget.Models;
using MVCBudget.Service;

namespace MVCBudget.Controllers
{ 
  
    public class EntryController : Controller
    {

        private readonly Service.Service _entryService; 
       // public EntryController(Service.Service entryService) { _entryService = entryService; }



        // GET: EntryController1
        public ActionResult Index()
        {
            return View();
        }

        // GET: EntryController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EntryController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EntryController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Entry model, IFormCollection collection)
        {
            try
            {

                if (ModelState.IsValid) 
                { var entry = new Entry 
                { Description = model.Description, Amount = model.Amount, };
                    Service.MYSQLAccess.InsertEntryWithIntermediate(entry);
               }

            }
            catch
            {
                return View();
            }
            return View();
        }

        // GET: EntryController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EntryController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EntryController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EntryController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
