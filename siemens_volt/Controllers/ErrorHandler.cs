using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace siemens_volt.Controllers
{
    public class ErrorHandler : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
