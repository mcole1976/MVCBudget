using Microsoft.AspNetCore.Mvc;
using MVCBudget.Models;
using MVCBudget.Service;

namespace MVCBudget.Controllers
{
    public class EntryDateController : Controller
    {
        // GET: EntryDateController1
        public ActionResult Index()
        {
            var previousEntries = new List<IncomeTotals>();
            previousEntries = MYSQLAccess.GetDictionaryDataDateDesc();

            var orderedEntries = previousEntries.OrderBy(entry => entry.Description_time).ToList();

            var model = new EntryDate
            {
                DateOnly = DateOnly.FromDateTime(DateTime.Today),
                Period = MYSQLAccess.GetDictionaryData(),
                PreviousEntries = orderedEntries
            };
            return View(model);
        }

        // GET: EntryDateController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EntryDateController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EntryDateController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EntryDate model, IFormCollection collection)
        {
            try
            {


                var entryDate = new EntryDate();
                Service.MYSQLAccess.InsertPeriaod_and_Date(model);


            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index");
        }

        // GET: EntryDateController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EntryDateController1/Edit/5
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

        // GET: EntryDateController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EntryDateController1/Delete/5
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
