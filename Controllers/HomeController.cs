using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace playground.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return RedirectToAction("QueryPoke", new {pokeid = 1});
        }
        [HttpGet]
        [Route("pokemon/{pokeid}")]
        public IActionResult QueryPoke(int pokeid)
        {
            var PokeInfo = new Dictionary<string, object>();
            WebRequest.GetPokemonDataAsync(pokeid, ApiResponse =>
                {
                    PokeInfo = ApiResponse;
                }
            ).Wait();
            // Other code
            ViewBag.Pokemon = PokeInfo;
            ViewBag.Next = pokeid + 1;
            ViewBag.Previous = pokeid - 1;
            return View("Index");
        }
        [HttpPost]
        [Route("pokemon/search")]
        public IActionResult Search(string pokemonname)
        {
            pokemonname = pokemonname.ToLower();
            var PokeInfo = new Dictionary<string, object>();
            WebRequest.GetPokemonNameDataAsync(pokemonname, ApiResponse =>
                {
                    PokeInfo = ApiResponse;
                }
            ).Wait();
            return RedirectToAction("QueryPoke", new {pokeid = Convert.ToInt32(PokeInfo["id"])});
        }
        [HttpPost]
        [Route("ajaxsearch")]
        public JsonResult AJAXSearch(string pokemonname)
        {
            if(pokemonname == "random")
            {
                Random randomgenerator = new Random();
                pokemonname = randomgenerator.Next(1,722).ToString();
            }
            pokemonname = pokemonname.ToLower();
            var PokeInfo = new Dictionary<string, object>();
            WebRequest.GetPokemonNameDataAsync(pokemonname, ApiResponse =>
                {
                    PokeInfo = ApiResponse;
                }
            ).Wait();
            return Json(PokeInfo);
        }
    }
}
