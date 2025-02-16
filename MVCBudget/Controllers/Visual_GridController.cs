using Microsoft.AspNetCore.Mvc;
using MVCBudget.Models;
using MVCBudget.Service;
using NuGet.Protocol;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MVCBudget.Controllers
{
    public class Visual_GridController : Controller
    {
        // GET: Visual_GridController

        [HttpPost]
        public ActionResult Delete(IFormCollection collection)
        {
            //var model = new Visual_Grid();
            string entryId = collection["item.Entry_id"].ToString();
            int Id = 0;

            bool conv = false;

            conv = int.TryParse(entryId, out Id);

            if (conv)
            {
                MYSQLAccess.DeleteCost(Id);
            }

            return RedirectToAction(nameof(Index));
        }


        public ActionResult Index()
        {
            var model = new Visual_Grid();

            return View(model);
        }
        [HttpGet]
        public JsonResult Change(int Selected)
        {
            // Retrieve model based on the selected ID

            Visual_Grid v = new();
            List<Income_Lots> resultSet = new();
            if (Selected != 0)
            {
                v.Selected = Selected;
                v.SetSelected(v.Selected);
                resultSet = v.Income;

            }
            if (resultSet.Count == 0)
            { // Return a message indicating no data found
                return Json(new { Message = "No data found" });
            }
            return Json(resultSet);
        }




        [HttpPost]
        public ActionResult Index(Visual_Grid grid)
        {
            // Retrieve model based on the selected ID

            Visual_Grid v = new();
            v.Selected = grid.Selected;
            v.SetSelected(v.Selected);
            return View("Index", v);
        }

        // GET: Visual_GridController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Visual_GridController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Visual_GridController/Create
        [HttpPost]
        public JsonResult Make([FromBody] string d)
        {

            try
            {
                using JsonDocument doc = JsonDocument.Parse(d);
                var data = doc.RootElement;
                int entryId = data.GetProperty("entryId").GetInt32();
                decimal amount = data.GetProperty("amount").GetDecimal();
                int id = data.GetProperty("id").GetInt32();

                bool conv = false;
                decimal Income = 0;
                int Id = 0;


                if (amount < 0)
                {
                    return Json(new { success = false, message = "Amount cannot be negative." });
                }

                // Call the Amend_Cost method
                MYSQLAccess.Amend_Cost(entryId, amount);




                KeyValuePair<decimal, decimal> dataBack = fnCalcNetIncome(id);

                return Json(new { Income = dataBack.Key, Costs = dataBack.Value });

            }
            catch
            {
                return Json(new { success = true, message = "Entry saved successfully" });
            }
        }
        public class IncomeAmendRequest
        {
            public string EntryId { get; set; }
            public string Amount { get; set; }
        }

        [HttpPost]
        public JsonResult Income_Amend([FromBody] IncomeAmendRequest request)
        {

            try
            {
                //using JsonDocument doc = JsonDocument.Parse(d);
                //var data = doc.RootElement;
                var entryId = request.EntryId;
                //data.GetProperty("entryId").ToString();
                var amount = request.Amount;
                //data.GetProperty("amount").ToString();


                bool conv = false;
                decimal Income = 0;
                int Id = 0;

                conv = int.TryParse(entryId, out Id);
                if (conv)
                {
                    conv = decimal.TryParse(amount, out Income);
                }
                if (Income > -1)
                {

                    MYSQLAccess.AmendIncome(Id, Income);
                }
                else
                {

                }
                KeyValuePair<decimal, decimal> dataBack = fnCalcNetIncome(Id);

                return Json(new { Income = Income, Costs = dataBack.Value });
            }
            catch (Exception e)        
            {
                return Json( e.ToJson() );
            }
        }

        private KeyValuePair<decimal, decimal> fnCalcNetIncome(int id)
        {
            KeyValuePair<decimal, decimal> r = MYSQLAccess.MakeRetrunDataKVP(id); ;
            return r;
        }

        [HttpPost]
        public JsonResult DeleteEntry([FromBody] JsonDocument d)
        { // Your delete logic here // Return a JSON response
            var data = d.RootElement;
            var entryId = data.GetProperty("entryId").GetInt32();
            MYSQLAccess.DeleteCost(entryId);

            return Json(new { success = true, message = "Entry deleted successfully" });
        }


        [HttpPost]
        public JsonResult ExtraData([FromBody] JsonDocument d)
        {
            try
            {
                var data = d.RootElement;
                var entryId = data.GetProperty("entryId").ToString();
                var amount = data.GetProperty("amount").ToString();
                var desc = data.GetProperty("description").ToString();

                int Id = 0;
                decimal  cost = 0;

                bool conv = false;

                conv = int.TryParse(entryId, out Id);
                if (conv)
                {
                    conv = decimal.TryParse(amount, out  cost);
                }

                if (conv)
                {

                    var entry = Tuple.Create(Id, desc, cost);
                    MYSQLAccess.InsertEntryWithIntermediate(entry);
                }

                KeyValuePair<decimal, decimal> dataBack = fnCalcNetIncome(Id);

                return Json(new { Income = dataBack.Key, Costs = dataBack.Value });
            }
            catch
            {
                return Json(new { success = true, message = "Entry saved successfully" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Period_Tally model, IFormCollection collection)
        {
            try
            {
                var stringKeys = collection["StringKeys"];
                var decimalValues = collection["DecimalValues"];
                Dictionary<string, decimal> periodData = new();
                for (int i = 0; i < stringKeys.Count; i++)
                {
                    if (decimal.TryParse(decimalValues[i], out decimal decimalValue))
                    {
                        periodData[stringKeys[i]] = decimalValue;
                    }
                }

                model.Period_Data = periodData;
                MYSQLAccess.InsertEntryWithIntermediate(model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IFormCollection collection)
        {
            try
            {
                string entryId = collection["item.Entry_id"].ToString();
                //var descriptionTime = collection["item.Description_time"].ToString(); 
                //var entryName = collection["item.Entry_name"].ToString(); 
                string amountString = collection["item.Amount"].ToString();


                int Id = 0;
                decimal cost = 0;
                bool conv = false;

                conv = true; int.TryParse(entryId, out Id);
                if (conv)
                {
                    conv = decimal.TryParse(amountString, out cost);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
                if (conv && cost > -1)
                {
                    MYSQLAccess.Amend_Cost(Id, cost);
                }
                else
                {

                }


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }



    }
}
