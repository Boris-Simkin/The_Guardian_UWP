using System.Collections.ObjectModel;
using MvvmCross.Core.ViewModels;
using TheGuardian.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheGuardianProject.Core.Models;
using System.Linq;
using System;

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

        private bool _noConnection;
        public bool NoConnection
        {
            get { return _noConnection; }
            set { SetProperty(ref _noConnection, value); }
        }

        private bool _pageLoading;
        public bool PageLoading
        {
            get { return _pageLoading; }
            private set { SetProperty(ref _pageLoading, value); }
        }

        public async void Init()
        {
            await TryLoadingAsync();
        }

        public async Task TryLoadingAsync()
        {
            await TryGetHeadersAsync(_sections.SectionsDict.Values.First());
        }

        public async Task TryGetHeadersAsync(string section)
        {
            PageLoading = true;
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("api-key", Constants.API_KEY);
            param.Add(Constants.SHOW_FIELDS_PARAM, "thumbnail,trailText,headline");
            try
            {
                SearchResult storyHeader = await _httpService.GetAsync<SearchResult>(Constants.BASE_API_URL + section, param);
                Items = new ObservableCollection<StoryHeader>(storyHeader.SearchResponse.StoryHeaders);
                NoConnection = false;
            }
            catch (Exception)
            {
                NoConnection = true;
            }
            PageLoading = false;
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
                    await TryGetHeadersAsync(selectedSection);
                }));
            }
        }

        private MvxAsyncCommand _reload;
        public MvxAsyncCommand Reload
        {
            get
            {
                return _reload ?? (_reload = new MvxAsyncCommand(async () =>
                {
                    await TryLoadingAsync();
                }));
            }
        }

    }
}