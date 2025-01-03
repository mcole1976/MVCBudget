using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCBudget.Service;

namespace MVCBudget.Controllers
{
    public class AuxController : Controller
    {
        // GET: AuxController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AuxController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AuxController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuxController/Create
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

        // GET: AuxController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AuxController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {

                string entryId = collection["entry.ID"].ToString();
                string incomeAmount = collection["entry.Income"].ToString();

                int entry_ID = 0;
                decimal income_total = 0;
                bool conv = int.TryParse(entryId, out entry_ID);
                if (conv)
                {

                    conv = decimal.TryParse(incomeAmount, out income_total);

                }
                else
                {
                    return RedirectToAction("Index", "Income");
                }
                if (conv)
                {
                    MYSQLAccess.AmendIncome(entry_ID, income_total);
                    return RedirectToAction("Index", "EntryDate");
                }



                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AuxController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AuxController/Delete/5
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
