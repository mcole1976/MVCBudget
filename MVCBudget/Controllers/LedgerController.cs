using Microsoft.AspNetCore.Mvc;
using MVCBudget.Models;

namespace MVCBudget.Controllers
{
    public class LedgerController : Controller
    {
        // GET: LedgerController
        public ActionResult Index()
        {
            return View();
        }

        // GET: LedgerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LedgerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LedgerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Ledger model, IFormCollection collection)
        {

            try
            {

                if (ModelState.IsValid)
                {

                    Service.MYSQLAccess.InsertLedger(model);
                }

            }
            catch
            {
                return View();
            }
            return RedirectToAction(nameof(Index));


        }

        // GET: LedgerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LedgerController/Edit/5
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

        // GET: LedgerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LedgerController/Delete/5
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
