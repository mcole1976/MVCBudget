using Microsoft.AspNetCore.Mvc;
using MVCBudget.Models;
using MVCBudget.Service;

namespace MVCBudget.Controllers
{
    public class IncomeController : Controller
    {
        // GET: IncomeController
        public ActionResult Index()
        {
            //return View();
            var model = new Income
            {
                Dates = CostandIncomeService.GetDictionaryIncomeDateData(),
            };



            return View(model);




        }

        // GET: IncomeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: IncomeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: IncomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Income model, IFormCollection collection)
        {
            try
            {
                Service.CostandIncomeService.InsertIncome(model);
                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return View(nameof(Index));
            }
        }

        // GET: IncomeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }





        // POST: IncomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IFormCollection collection)
        {
            try
            {
                string entryId = collection["Id"].ToString();
                string incomeAmount = collection["Income_amount"].ToString();
                int entry_ID = 0;
                decimal income_total = 0;
                bool conv = int.TryParse(entryId, out entry_ID);
                if (conv)
                {

                    conv = decimal.TryParse(incomeAmount, out income_total);

                }
                else
                {
                    return RedirectToAction("Index", "Visual_Grid");
                }
                if (conv)
                {
                    CostandIncomeService.AmendIncome(entry_ID, income_total);
                    return RedirectToAction("Index", "Visual_Grid");
                }

            }
            catch
            {
                return View();
            }
            return View();
        }

        // GET: IncomeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: IncomeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                string entryId = collection["entry.ID"].ToString();


                int entry_ID = 0;
                bool conv = int.TryParse(entryId, out entry_ID);
                if (conv)
                {
                    CostandIncomeService.DeleteIncome(entry_ID);
                }
                return RedirectToAction("Index", "EntryDate");
            }
            catch
            {
                return View();
            }
        }
    }
}
