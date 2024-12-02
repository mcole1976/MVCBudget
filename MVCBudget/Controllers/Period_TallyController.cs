using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCBudget.Models;
using MVCBudget.Service;

namespace MVCBudget.Controllers
{
    public class Period_TallyController : Controller
    {
        // GET: Period_TallyController1
        public ActionResult Index()
        {
            var model = new Period_Tally
            {
                Date = MYSQLAccess.GetDictionaryDateData(),

                Period_Data = fnSetData()



            };



            return View(model);
        }

        private Dictionary<string, decimal> fnSetData()
        {
           Dictionary<string,decimal> res = new Dictionary<string,decimal>();
            res.Add("Add Data", 0);
            return res;
        }

        // GET: Period_TallyController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Period_TallyController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Period_TallyController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Period_Tally model, IFormCollection collection)
        {
            try
            {
                var stringKeys = collection["StringKeys"]; 
                var decimalValues = collection["DecimalValues"]; 
                Dictionary<string, decimal> periodData = new Dictionary<string, decimal>(); 
                for (int i = 0; i < stringKeys.Count; i++) { 
                    if (decimal.TryParse(decimalValues[i], out decimal decimalValue)) 
                    { periodData[stringKeys[i]] = decimalValue; } }

                model.Period_Data = periodData;
                MYSQLAccess.InsertEntryWithIntermediate(model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Period_TallyController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Period_TallyController1/Edit/5
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

        // GET: Period_TallyController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Period_TallyController1/Delete/5
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
