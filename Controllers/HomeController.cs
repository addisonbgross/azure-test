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

        private dynamic convertResponse(dynamic response) 
        {
            var content = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject(content);
        }

        public IActionResult Index(int id)
        {
            // get ALL types
            var response = Client.GetAsync("http://pokeapi.co/api/v2/type").Result;
            if (response.IsSuccessStatusCode)
            {
                dynamic jsonContent = convertResponse(response);

                workList.Clear();
                foreach (dynamic element in jsonContent["results"])
                {
                    workList.Add(element["name"].ToString());
                }
                ViewData["allTypes"] = workList.ToArray();
            }

            // get all Ghost type information
            response = Client.GetAsync("http://pokeapi.co/api/v2/type/" + id + "/").Result;
            if (response.IsSuccessStatusCode)
            {
                dynamic jsonContent = convertResponse(response);

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
            }

            return View();
        }
    }
}
