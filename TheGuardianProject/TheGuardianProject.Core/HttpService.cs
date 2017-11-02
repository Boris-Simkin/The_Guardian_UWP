using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class HttpService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="baseUrl"></param>
    /// <param name="classes">Classes to remove</param>
    /// <param name="ids">Id's to remove</param>
    /// <returns></returns>
    public async Task<string> GetHtmlContentAsync(string baseUrl, Regex classes, Regex ids)
    {
        HtmlDocument doc = new HtmlDocument();

        string result = string.Empty;
        using (HttpClient client = new HttpClient())
        {
            using (HttpResponseMessage response = await client.GetAsync(baseUrl))
            {
                using (HttpContent content = response.Content)
                {
                     result = await content.ReadAsStringAsync();
                }
            }
        }
        doc.LoadHtml(result);

        var toRemove = doc.DocumentNode.Descendants()
              .Where(x => (x.Attributes.Contains("class")
              && classes.IsMatch(x.Attributes["class"].Value)) ||
              ids.IsMatch(x.Id)
              ).ToList();

        foreach (var node in toRemove)
            node.Remove();

        return doc.DocumentNode.InnerHtml;
    }
    public async Task<T> GetAsync<T>(string baseUrl, Dictionary<string, string> parameters)
    {
        using (var client = new HttpClient())
        {
            var finalUrl = baseUrl + GetParametersString(parameters);

            var response = await client.GetAsync(finalUrl);
            response.EnsureSuccessStatusCode();

            var resultJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(resultJson);
        }
    }

    private string GetParametersString(Dictionary<string, string> parameters)
    {
        if (parameters?.Any() != true)
            return string.Empty;

        var parametersString = new StringBuilder("?");
        foreach (var parameter in parameters)
        {
            parametersString.Append($"{parameter.Key}={parameter.Value}&");
        }

        parametersString.Remove(parametersString.Length - 1, 1);
        
        return parametersString.ToString();
    }
}