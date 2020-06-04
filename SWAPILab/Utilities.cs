using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SWAPILab.Models;

namespace SWAPILab
{
    public class Utilities
    {
        /// <summary>
        /// Builds a set of key value pairs from a list of strings
        ///
        /// String size must be divisible by 2
        /// </summary>
        /// <param name="pairs">A list in the format of "[key]", "[value]" repeating</param>
        /// <returns>A list that can be passed to GetApiResponse, or null if an odd number of arguments is passed</returns>
        public static List<KeyValuePair<string, string>> BuildApiArguments(params string[] pairs)
        {
            if (pairs.Length % 2 == 1)
            {
                return null;
            }

            var ret = new List<KeyValuePair<string, string>>();

            for (int i = 0; i < pairs.Length; i+=2)
            {
                ret.Add(new KeyValuePair<string, string>(pairs[i], pairs[i+1]));
            }

            return ret;
        }
        public static async Task<List<T>> GetApiResponse<T>(string controller, string action, string baseUrl, string id = "",
            params KeyValuePair<string, string>[] options) where T : new()
        {
            string url = $"{baseUrl}/" +
                         $"{controller}/" +
                         $"{action}/"+
                         $"{(!string.IsNullOrEmpty(id) ? $"{id}/" : "")}";

            bool first = true;
            foreach (KeyValuePair<string, string> argument in options)
            {
                url += first ? "?" : "&";
                url += $"{argument.Key}={Uri.EscapeDataString(argument.Value)}";
                first = false;
            }

            HttpWebRequest request = WebRequest.CreateHttp(url);
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse) request.GetResponse();
            }
            catch (WebException)
            {
                return null;
            }

            Stream s = response.GetResponseStream();
            if (s == null)
            {
                return null;
            }
            
            StreamReader rd = new StreamReader(s);

            string output = await rd.ReadToEndAsync();
            List<T> ret;
            try
            {
                ret = JsonConvert.DeserializeObject<List<T>>(output);
            }
            catch (JsonSerializationException) // most likely we only got one object back
            {
                ret = new List<T> {JsonConvert.DeserializeObject<T>(output)};
            }

            return ret;
        }

        public static async Task<List<T>> GetApiResponse<T>(string controller, string action, string baseUrl, string id = "", params string[] options)
            where T : new()
        {
            var keyValuePairs = Utilities.BuildApiArguments(options);
            if (keyValuePairs == null && options.Length > 0)
            {
                return new List<T>();
            }

            return await GetApiResponse<T>(controller, action, baseUrl, id,
                (keyValuePairs ?? new List<KeyValuePair<string, string>>()).ToArray());
        }
        public static async Task<List<T>> GetApiResponse<T>(string controller, string action, string baseUrl, string id = "")
            where T : new()
        {
            return await GetApiResponse<T>(controller, action, baseUrl, id,
                (new List<KeyValuePair<string, string>>()).ToArray());
        }

        public static string CreateCollapse(string type, string name, string[] values, bool links)
        {
            if (values.Length == 0)
                return "";
            string str =
                $"<a class=\"btn btn-primary\" style=\"margin: .25em;\" data-toggle=\"collapse\" href=\"#collapse{type}\" role=\"button\" aria-expanded=\"false\" aria-controls=\"collapse{type}\">\n" +
                $"Show/Hide {name}{(name.EndsWith("s") ? "\'" : "\'s")} {type} information\n" +
                "</a>\n" +
                $"<div class=\"collapse\" id=\"collapse{type}\" >\n" +
                "<div class=\"card card-body\">\n";
            values.ToList().ForEach(x => str += 
                (!x.Contains("people") 
                    ? ($"<a href=\"{x}\">{x}</a>") 
                    : ($"<a href=\"/Home/GetPersonById?selectedPerson={x.Split('/')[^2]}\">{People.GetName(int.Parse(x.Split('/')[^2]))}</a>")));
                
                str += "</div>\n" +
                "</div>"+
                "<br/>";
                
            return str;
        }
    }
}