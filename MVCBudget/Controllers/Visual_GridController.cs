using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCBudget.Models;
using System.Diagnostics;
using System.Reflection;

namespace MVCBudget.Controllers
{
    public class Visual_GridController : Controller
    {
        // GET: Visual_GridController
        
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var model = new Visual_Grid();

            return View(model);
        }

       
        public ActionResult Index()
        {
            var model = new Visual_Grid();

            return View(model);
        }
        [HttpPost]
        public ActionResult Index(Visual_Grid grid)
        {
            // Retrieve model based on the selected ID

            Visual_Grid v = new Visual_Grid();
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
        [ValidateAntiForgeryToken]
        public ActionResult Create(Visual_Grid model, IFormCollection collection)
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

        // GET: Visual_GridController/Edit/5


        // POST: Visual_GridController/Edit/5


        // GET: Visual_GridController/Delete/5


        // POST: Visual_GridController/Delete/5


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( IFormCollection collection)
        {
            try
            {
                var entryId = collection["item.Entry_id"].ToString(); 
                var descriptionTime = collection["item.Description_time"].ToString(); 
                var entryName = collection["item.Entry_name"].ToString(); 
                var amountString = collection["item.Amount"].ToString();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }



    }
}
