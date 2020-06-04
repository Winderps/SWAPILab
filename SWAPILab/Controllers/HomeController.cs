using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SWAPILab.Models;

namespace SWAPILab.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetPersonById([FromQuery] int selectedPerson)
        {
            People person = await People.GetPerson(selectedPerson);
            return View("SinglePersonView", person);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetPlanetById([FromQuery] int selectedPlanet)
        {
            Planet p = (await Utilities.GetApiResponse<Planet>("api", "planets", "https://swapi.dev",
                selectedPlanet.ToString()))?.First();
            return View("SinglePlanetView", p);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetPersonByName([FromQuery] string searchQuery)
        {
            List<People> people = (await Utilities.GetApiResponse<ListRootObject<People>>("api", "people", "https://swapi.dev", "",
                "search", Uri.EscapeDataString(searchQuery))).First().Results.ToList();
            return View("MultiPersonView", people);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetPlanetByName([FromQuery] string searchQuery)
        {
            List<Planet> planets = (await Utilities.GetApiResponse<ListRootObject<Planet>>("api", "planets", "https://swapi.dev", "",
                "search", Uri.EscapeDataString(searchQuery))).First().Results.ToList();
            return View("MultiPlanetView", planets);
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}