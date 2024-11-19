using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MVCBudget.Controllers
{
    public class EntryController : Controller
    {
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
        public ActionResult Create(IFormCollection collection)
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
