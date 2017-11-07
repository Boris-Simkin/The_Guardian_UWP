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
        #region fields
        private readonly ILocalSettings _localSettings;
        private readonly ITileManager _tileManager;
        private readonly Headers _headers;

        private List<StoryHeader> _items;
        private bool _noConnection;
        private bool _pageLoading;
        private MvxCommand<string> _readArticleCommand;
        private MvxAsyncCommand<string> _loadSectionTitlesCommand;
        private MvxAsyncCommand _reloadCommand;
        private bool _isSectionPinned;
        private MvxAsyncCommand<bool> _togglePinSectionCommand;
        #endregion
        #region properties
        public Sections Sections { get; private set; }
        public List<StoryHeader> Items
        {
            get { return _items; }
            private set { SetProperty(ref _items, value); }
        }
        public Section CurrentSection
        {
            get { return Sections.Current; }
            set { Sections.Current = value; RaisePropertyChanged("CurrentSection"); }
        }
        public bool NoConnection
        {
            get { return _noConnection; }
            set { SetProperty(ref _noConnection, value); }
        }
        public bool PageLoading
        {
            get { return _pageLoading; }
            private set { SetProperty(ref _pageLoading, value); }
        }
        public bool IsSectionPinned
        {
            get { return _isSectionPinned; }
            set { SetProperty(ref _isSectionPinned, value); }
        }
        #endregion
        #region command properties
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
        public MvxAsyncCommand ReloadCommand
        {
            get
            {
                return _reloadCommand ?? (_reloadCommand = new MvxAsyncCommand(async () =>
                {
                    await _headers.GetHeadersAsync(CurrentSection);
                }));
            }
        }
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
        public MvxAsyncCommand<string> LoadSectionTitlesCommand
        {
            get
            {
                return _loadSectionTitlesCommand ?? (_loadSectionTitlesCommand = new MvxAsyncCommand<string>(async (selectedSection) =>
                {
                    if (selectedSection != null)
                        CurrentSection = Sections.ByName(selectedSection);
                    if (!NoConnection)
                    {
                        _localSettings.Save("LastVisitedSection", CurrentSection.Name);
                        await _headers.GetHeadersAsync(CurrentSection);
                    }
                }));
            }
        }
        #endregion

        public MainViewModel(HttpService httpService, ILocalSettings localSettings, ITileManager tileManager)
        {
            _localSettings = localSettings;
            _tileManager = tileManager;

            _headers = new Headers(httpService);
            Sections = new Sections();
        }

        public async void Init()
        {
            _headers.HeadersLoading += OnHeadersLoading;
            _headers.HeadersLoaded += OnHeadersLoaded;
            //  Set the current section to the last visited or to the secondary tile
            //  if the app was launched by secondary tile
            if (_tileManager.TappedTileId == "App")
                CurrentSection = Sections.ByName(_localSettings.Load<string>("LastVisitedSection"));
            else CurrentSection = Sections.ByName(_tileManager.TappedTileId);

            await _headers.GetHeadersAsync(CurrentSection);
        }

        private void OnHeadersLoading(object sender, EventArgs e)
        {
            PageLoading = true;
        }

        private void OnHeadersLoaded(object sender, SucceedEventArgs e)
        {
            PageLoading = false;
            if (e.Succeed)
            {
                NoConnection = false;
                Items = _headers.ToList();
                IsSectionPinned = _tileManager.IsPinned(CurrentSection.Name);
                LoadTiles();
            }
            else
                NoConnection = true;
        }

        private void LoadTiles()
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
}