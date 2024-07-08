using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PrimeiroProjetoASPNet.Models;

namespace PrimeiroProjetoASPNet.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index(int? id)
        {
            return View(id);
        }

    }
}
