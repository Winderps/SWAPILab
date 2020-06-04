using System;
using Newtonsoft.Json;

namespace SWAPILab.Models
{
    public class Planet
    {
        public string Name { get; set; }
        public string Diameter { get; set; }
        [JsonProperty("rotation_period")] public string RotationPeriod { get; set; }
        [JsonProperty("orbital_period")] public string OrbitalPeriod { get; set; }
        public string Gravity { get; set; }
        public string Population { get; set; }
        public string Climate { get; set; }
        public string Terrain { get; set; }
        [JsonProperty("surface_water")] public string SurfaceWater { get; set; }
        public string[] Residents { get; set; }
        public string[] Films { get; set; }
        public string Url { get; set; }
        public DateTime Created { get; set; }
        public DateTime Edited { get; set; }
    }
}