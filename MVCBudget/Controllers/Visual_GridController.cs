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

        private readonly Service.Service _s;

        public Visual_GridController(Service.Service srv)
        {
            _s = srv;
        }

        [HttpPost]
        public async Task<ActionResult> DeleteAsync(IFormCollection collection)
        {
            //var model = new Visual_Grid();
            string entryId = collection["item.Entry_id"].ToString();
            int Id = 0;

            bool conv = false;

            conv = int.TryParse(entryId, out Id);

            if (conv)
            {
                await _s.DeleteEntry(Id);
                //CostandIncomeService.DeleteCost(Id);
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
            
            return Json(resultSet);
        }

        [HttpGet]
        public ActionResult DetailList(int ID)
        {
            // Retrieve model based on the selected ID
            List<string> resultSet = new();
            
            return View(resultSet);
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


        [HttpGet]
        public async Task<JsonResult> GetPossibles(int id)
        {
            try
            {

                var s = new Service.Service();
                var possibles = await s.GetEntryPossibles(id);
                return Json(possibles);
            }
            catch (Exception ex)
            {
                // Log the error
                return Json(new List<string>());
            }
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
        //public JsonResult Make([FromBody] string d)

        public ActionResult<EntryResponse> Make([FromBody] UpdateRequest request)

        {

            try
            {

                if (request == null)
                {
                    return BadRequest(new EntryResponse
                    {
                        Success = false,
                        Message = "Invalid request data"
                    });
                }

                if (request.Amount < 0)
                {
                    return BadRequest(new EntryResponse
                    {
                        Success = false,
                        Message = "Amount cannot be negative."
                    });
                }



                CostandIncomeService.Amend_Cost(request.EntryId, request.Amount);
                //_costService.UpdateCost(request.EntryId, request.Amount);
                var financialData = _s.MakeRetrunDataKVP(request.Id).Result;

                return Ok(new EntryResponse
                {
                    Success = true,
                    Message = "Entry saved successfully",
                    Income = financialData.Key,
                    Costs = financialData.Value
                });


                //using JsonDocument doc = JsonDocument.Parse(d);
                //var data = doc.RootElement;

                //if (!data.TryGetProperty("entryId", out var entryIdElement))
                //    throw new ArgumentException("Missing entryId property");

                //if (!data.TryGetProperty("amount", out var amountElement))
                //    throw new ArgumentException("Missing amount property");

                //if (!data.TryGetProperty("id", out var idElement))
                //    throw new ArgumentException("Missing id property");

                //// Parse entryId
                //int entryId = entryIdElement.ValueKind switch
                //{
                //    JsonValueKind.String => int.Parse(entryIdElement.GetString()),
                //    JsonValueKind.Number => entryIdElement.GetInt32(),
                //    _ => throw new FormatException("Invalid entryId format")
                //};

                //// Parse amount
                //decimal amount = amountElement.ValueKind switch
                //{
                //    JsonValueKind.String => decimal.Parse(amountElement.GetString()),
                //    JsonValueKind.Number => amountElement.GetDecimal(),
                //    _ => throw new FormatException("Invalid amount format")
                //};

                //// Parse id
                //int id = idElement.ValueKind switch
                //{
                //    JsonValueKind.String => int.Parse(idElement.GetString()),
                //    JsonValueKind.Number => idElement.GetInt32(),
                //    _ => throw new FormatException("Invalid id format")
                //};

                //// Use the parsed values
                //Console.WriteLine($"Entry: {entryId}, Amount: {amount}, ID: {id}");




                //if (amount < 0)
                //{
                //    return Json(new { success = false, message = "Amount cannot be negative." });
                //}

                //// Call the Amend_Cost method
                ////CostandIncomeService.Amend_Cost(entryId, amount);




                //KeyValuePair<decimal, decimal> dataBack = fnCalcNetIncome(id);

                //return Json(new { Income = dataBack.Key, Costs = dataBack.Value });

            }
            catch
            {
                return StatusCode(500, new EntryResponse
                {
                    Success = false,
                    Message = "An error occurred while processing your request"
                });
            }
        }
        public class IncomeAmendRequest
        {
            private string _entry_Id;
            private string _amount;
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

                    CostandIncomeService.AmendIncome(Id, Income);
                }
                else
                {

                }
                KeyValuePair<decimal, decimal> dataBack = _s.MakeRetrunDataKVP(Id).Result;

                return Json(new { Income = Income, Costs = dataBack.Value });
            }
            catch (Exception e)        
            {
                return Json( e.ToJson() );
            }
        }


        [HttpPost]
        public JsonResult DeleteEntry([FromBody] JsonDocument d)
        { // Your delete logic here // Return a JSON response
            try
            {
                var data = d.RootElement;

                // Safely handle both string and number entryId
                int entryId;
                if (data.TryGetProperty("entryId", out JsonElement idElement))
                {
                    entryId = idElement.ValueKind == JsonValueKind.String
                        ? int.Parse(idElement.GetString())
                        : idElement.GetInt32();
                }
                else
                {
                    return Json(new { success = false, message = "Missing entryId" });
                }

                CostandIncomeService.DeleteCost(entryId);
                return Json(new { success = true, message = "Entry deleted successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
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
                    CostandIncomeService.InsertEntryWithIntermediate(entry);
                }

                KeyValuePair<decimal, decimal> dataBack = _s.MakeRetrunDataKVP(Id).Result;

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
                CostandIncomeService.InsertEntryWithIntermediate(model);
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
                    CostandIncomeService.Amend_Cost(Id, cost);
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
