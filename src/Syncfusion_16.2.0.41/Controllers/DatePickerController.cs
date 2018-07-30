using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace DotnetFiddle.Controllers
{
    public partial class DatePickerController : Controller
    {
        //
        // GET: /Default/
        public IActionResult DatePickerFeatures()
        {
            return View("DatePickerFeatures");
        }
    }
}
