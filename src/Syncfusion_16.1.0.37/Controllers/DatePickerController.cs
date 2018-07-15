using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotnetFiddle.Models;
using Microsoft.AspNetCore.Http;

namespace DotnetFiddle.Controllers
{
    public partial class DatePickerController : Controller
    {
        //
        // GET: /Default/
        public IActionResult DatePickerFeatures()
        {
            return View();
        }

        public IActionResult dummy() {
            return View("dummy_view");
        }

        public IActionResult dummy_1()
        {
            return View("dummy_view");
        }

        public IActionResult new_dummy()
        {
            return View("dummy_view");
        }

        [HttpPost]
        public IActionResult FormSubmit(IFormCollection collection)
        {
            ViewData["Date"] = collection["DatePick"];
            ViewData["Time"] = collection["TimePick"];
            ViewData["DateTime"] = collection["DateTime"];
            return View();
        }


        public IActionResult FormSubmit() {

            return View();
        }

        [HttpPost]
        public IActionResult ForControl(DPModel dpmodel)
        {
            ViewData["Date"] = dpmodel.Date;
            ViewData["Time"] = dpmodel.Time;
            ViewData["DateTime"] = dpmodel.DateTime;
            return View();
        }
        public IActionResult ForControl()
        {
            DPModel date = new DPModel();
            date.Date = date.DateTime = date.Time = DateTime.Now;
            return View(date);
        }
    }
}
