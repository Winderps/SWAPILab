using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace SWAPILab.Models
{
    public class People
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("birth_year")]
        public string BirthYear { get; set; }
        [JsonProperty("eye_color")]
        public string EyeColor { get; set; }
        [JsonProperty("gender")]
        public string Gender { get; set; }
        [JsonProperty("hair_color")]
        public string HairColor { get; set; }
        [JsonProperty("height")]
        public string Height { get; set; }
        [JsonProperty("mass")]
        public string Mass { get; set; }
        [JsonProperty("skin_color")]
        public string SkinColor { get; set; }
        [JsonProperty("homeworld")]
        public string Homeworld { get; set; }
        [JsonProperty("films")]
        public string[] Films { get; set; }
        [JsonProperty("species")]
        public string[] Species  { get; set; }
        [JsonProperty("vehicles")]
        public string[] Vehicles { get; set; }
        [JsonProperty("starships")]
        public string[] StarShips { get; set; }
        
        [JsonProperty("created")]
        public DateTime Created { get; set; }
        [JsonProperty("edited")]
        public DateTime Edited { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }

        public static async Task<People> GetPerson(int id)
        {
            People person =
                (await Utilities.GetApiResponse<People>("api", "people", "https://swapi.dev", id.ToString()))
                .FirstOrDefault();
            return person;
        }

        public static string GetName(int id)
        {
            Task<People> p = GetPerson(id);
            p.Wait();
            return p.Result.Name;
        }
    }
}