using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGuardian.Core;
using TheGuardian.Core.Models;

namespace TheGuardianProject.Core.Models
{
    public class Headers : IEnumerable<StoryHeader>
    {
        private readonly HttpService _httpService;
        public Headers(HttpService httpService)
        {
            _httpService = httpService;
        }

        private List<StoryHeader> _headersList = new List<StoryHeader>();

        public event EventHandler HeadersLoading;
        public event EventHandler<SucceedEventArgs> HeadersLoaded;

        public async Task GetHeadersAsync(Section section)
        {
            HeadersLoading?.Invoke(this, EventArgs.Empty);
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("api-key", Constants.API_KEY);
            param.Add(Constants.SHOW_FIELDS_PARAM, "thumbnail,trailText,headline");
            try
            {
                SearchResult storyHeader = await _httpService.GetAsync<SearchResult>(Constants.BASE_API_URL + section.Address, param);
                _headersList = new List<StoryHeader>(storyHeader.SearchResponse.StoryHeaders);

                HeadersLoaded?.Invoke(this, new SucceedEventArgs(true));
            }
            catch (Exception)
            {
                HeadersLoaded?.Invoke(this, new SucceedEventArgs(false));
            }
        }

        public IEnumerator<StoryHeader> GetEnumerator()
        {
            foreach (var header in _headersList)
                yield return header;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
