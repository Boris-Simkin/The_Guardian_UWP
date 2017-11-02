using System.Collections.ObjectModel;
using MvvmCross.Core.ViewModels;
using TheGuardian.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheGuardianProject.Core.Models;
using System.Linq;

namespace TheGuardian.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {

        private readonly HttpService _httpService;

        public MainViewModel(HttpService httpService)
        {
            _httpService = httpService;

            string[] Keys = _sections.SectionsDict.Keys.ToArray();

            var FirstKey = _sections.SectionsDict.ToList()[0];

        }
        Sections _sections = new Sections();
        public Dictionary<string, string> Sections => _sections.SectionsDict;

        private ObservableCollection<StoryHeader> _items;

        public ObservableCollection<StoryHeader> Items
        {
            get { return _items; }
            private set { SetProperty(ref _items, value); }
        }

        public async void Init()
        {
            await GetHeadersAsync(_sections.SectionsDict.Values.First());
        }

        public async Task GetHeadersAsync(string section)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("api-key", Constants.API_KEY);
            param.Add(Constants.SHOW_FIELDS_PARAM, "thumbnail,trailText,headline");
            SearchResult storyHeader = await _httpService.GetAsync<SearchResult>(Constants.BASE_API_URL + section, param);
            Items = new ObservableCollection<StoryHeader>(storyHeader.SearchResponse.StoryHeaders);

        }

        private MvxCommand<string> _readArticleCommand;
        public MvxCommand<string> ReadArticleCommand
        {
            get
            {
                return _readArticleCommand ?? (_readArticleCommand = new MvxCommand<string>((articleId) =>
                    {
                        ShowViewModel<ArticleViewModel>(new StoryHeader { Id = articleId });
                    }));
            }
        }

        private MvxAsyncCommand<string> _loadSectionTitlesCommand;
        public MvxAsyncCommand<string> LoadSectionTitlesCommand
        {
            get
            {
                return _loadSectionTitlesCommand ?? (_loadSectionTitlesCommand = new MvxAsyncCommand<string>(async (selectedSection) =>
                {
                    await GetHeadersAsync(selectedSection);
                }));
            }
        }


    }
}