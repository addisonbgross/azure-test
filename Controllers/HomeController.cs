using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace azure_test.Controllers
{
    public class HomeController : Controller
    {
        private static HttpClient Client = new HttpClient();
        // for general pokemon information
        private static List<String> workList = new List<String>();
        // list of all relevant pokemon sprite urls
        private static List<String> spriteList = new List<String>();

        private Array extract(dynamic content, String id)
        {
            workList.Clear();
            foreach (dynamic element in content)
            {
                workList.Add(element[id].ToString());
            }
            return workList.ToArray();
        }

        public IActionResult Index()
        {
            // get all Ghost type information
            var response = Client.GetAsync("http://pokeapi.co/api/v2/type/8/").Result;
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                dynamic jsonContent = JsonConvert.DeserializeObject(content);

                // get type
                ViewData["type"] = jsonContent["name"];

                // get move list for type
                ViewData["moves"] = extract(jsonContent["moves"], "name");

                // get pokemon list for type
                workList.Clear();
                foreach (dynamic pokemon in jsonContent["pokemon"])
                {
                    workList.Add(pokemon["pokemon"]["name"].ToString());

                    // get sprite url
                    Uri rawUrl = new Uri(pokemon["pokemon"]["url"].ToString());
                    String pokemonSpriteId = rawUrl.Segments.Last().TrimEnd('/');
                    spriteList.Add("https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/" + pokemonSpriteId + ".png");
                }
                ViewData["pokemon"] = workList.ToArray();
                ViewData["sprites"] = spriteList.ToArray();

                // get damage relations
                workList.Clear();
                foreach (dynamic type in jsonContent["damage_relations"]["half_damage_from"])
                {
                    workList.Add(type["name"].ToString());
                }
                ViewData["weak"] = workList.ToArray();

                workList.Clear();
                foreach (dynamic type in jsonContent["damage_relations"]["no_damage_from"])
                {
                    workList.Add(type["name"].ToString());
                }
                ViewData["noDamage"] = workList.ToArray();

            }
            return View();
        }
    }
}
