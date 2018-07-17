using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotnetFiddle.Models;
using Microsoft.AspNetCore.Http;

namespace DotnetFiddle.Controllers
{
    public partial class TimePickerController : Controller
    {
        //
        // GET: /Default/
        public IActionResult TimePickerFeatures()
        {
            return View();
        }
        public IActionResult dummy()
        {
            return View();
        }

        public IActionResult dummy_2()
        {
            return View("dummy");
        }
    }
}
