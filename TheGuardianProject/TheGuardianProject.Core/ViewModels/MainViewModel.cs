using System.Collections.ObjectModel;
using MvvmCross.Core.ViewModels;
using TheGuardian.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheGuardianProject.Core.Models;
using System.Linq;
using System;
using TheGuardianProject.Core;

namespace TheGuardian.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        private readonly HttpService _httpService;
        private readonly ILocalSettings _localSettings;
        private readonly ITileManager _tileManager;
        public MainViewModel(HttpService httpService, ILocalSettings localSettings, ITileManager tileManager)
        {
            _httpService = httpService;
            _localSettings = localSettings;
            _tileManager = tileManager;

            Sections = new Sections();
        }

        public Sections Sections { get; private set; }

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
            CurrentSection = Sections.ByName(_localSettings.Load<string>("LastVisitedSection"));
            await LoadSectionAsync(CurrentSection.Address);

            if (!NoConnection)
            {
                //Loading some titles to the live tile from the last visited section
                Random r = new Random();
                for (int i = 0; i < 3; i++)
                {
                    int itemId = r.Next(Items.Count);
                    _tileManager.SetTextTile(CurrentSection.Name, Items[itemId].WebTitle);
                    _tileManager.SetImageTile(Items[itemId].StoryHeaderAdditionalFields.Thumbnail);
                }
            }
        }

        public Section CurrentSection
        {
            get { return Sections.Current; }
            set { Sections.Current = value; RaisePropertyChanged("CurrentSection"); }
        }

        public async Task LoadSectionAsync(string section)
        {
            PageLoading = true;
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("api-key", Constants.API_KEY);
            param.Add(Constants.SHOW_FIELDS_PARAM, "thumbnail,trailText,headline");
            try
            {
                SearchResult storyHeader = await _httpService.GetAsync<SearchResult>(Constants.BASE_API_URL + section, param);
                Items = new ObservableCollection<StoryHeader>(storyHeader.SearchResponse.StoryHeaders);
                IsSectionPinned = _tileManager.IsPinned(CurrentSection.Name);
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
                    if (selectedSection != null)
                        CurrentSection = Sections.ByName(selectedSection);
                    _localSettings.Save("LastVisitedSection", CurrentSection.Name);
                    await LoadSectionAsync(CurrentSection.Address);
                }));
            }
        }

        private MvxAsyncCommand _reloadCommand;
        public MvxAsyncCommand ReloadCommand
        {
            get
            {
                return _reloadCommand ?? (_reloadCommand = new MvxAsyncCommand(async () =>
                {
                    await LoadSectionAsync(CurrentSection.Address);
                }));
            }
        }

        private bool _isSectionPinned;

        public bool IsSectionPinned
        {
            get { return _isSectionPinned; }
            set { SetProperty(ref _isSectionPinned, value); }
        }

        private MvxAsyncCommand<bool> _togglePinSectionCommand;
        public MvxAsyncCommand<bool> TogglePinSectionCommand
        {
            get
            {
                return _togglePinSectionCommand ?? (_togglePinSectionCommand = new MvxAsyncCommand<bool>(async (isChecked) =>
                {
                    if (isChecked)
                        IsSectionPinned = await _tileManager.PinSecondaryTile(CurrentSection.Name, CurrentSection.Name, CurrentSection.Name);
                    else
                        IsSectionPinned = !await _tileManager.UnpinSecondaryTileAsync(CurrentSection.Name);

                }));
            }
        }
    }
}