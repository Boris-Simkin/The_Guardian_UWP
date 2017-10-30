using Newtonsoft.Json;

namespace TheGuardian.Core.Models
{
    public class SearchResult
    {
        [JsonProperty(PropertyName = "response")]
        public SearchResponse SearchResponse { get; set; }
    }
}