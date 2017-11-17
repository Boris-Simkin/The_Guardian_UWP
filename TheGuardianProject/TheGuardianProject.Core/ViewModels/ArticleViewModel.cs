using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using TheGuardian.Core.Models;
using HtmlAgilityPack;
using System.Net.Http;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using TheGuardianProject.Core;

namespace TheGuardian.Core.ViewModels
{
    public class ArticleViewModel : MvxViewModel
    {
        #region fields
        private readonly HttpService _httpService;
        private readonly IShareManager _shareManager;
        private string _headerId;

        private bool _noConnection;
        private string _articleContent;
        private bool _pageLoading;
        private MvxCommand _goBackCommand;
        private MvxAsyncCommand _reloadCommand;
        private MvxCommand _shareCommand;
        #endregion
        #region properties
        public bool NoConnection
        {
            get { return _noConnection; }
            set { SetProperty(ref _noConnection, value); }
        }

        public string ArticleContent
        {
            get { return _articleContent; }
            private set { SetProperty(ref _articleContent, value); }
        }

        public bool PageLoading
        {
            get { return _pageLoading; }
            private set { SetProperty(ref _pageLoading, value); }
        }
        #endregion
        #region command properties
        public MvxCommand GoBackCommand
        {
            get
            {
                return _goBackCommand ?? (_goBackCommand = new MvxCommand(() =>
                {
                    Close(this);
                }));
            }
        }
        public MvxAsyncCommand ReloadCommand
        {
            get
            {
                return _reloadCommand ?? (_reloadCommand = new MvxAsyncCommand(async () =>
                {
                    await TryGetPageAsync(_headerId);
                }));
            }
        }
        public MvxCommand ShareCommand
        {
            get
            {
                return _shareCommand ?? (_shareCommand = new MvxCommand(() =>
                {
                    _shareManager.Share(Constants.BASE_WEB_URL + _headerId);
                }));
            }
        }
        #endregion

        public ArticleViewModel(HttpService httpService, IShareManager shareManager)
        {
            _httpService = httpService;
            _shareManager = shareManager;
        }

        public async void Init(StoryHeader parameter)
        {
            ArticleContent = string.Empty;
            _headerId = parameter.Id;
            await TryGetPageAsync(_headerId);
        }

        protected override void InitFromBundle(IMvxBundle parameters)
        {
            _shareManager.Register();
            base.InitFromBundle(parameters);
        }

        protected override void SaveStateToBundle(IMvxBundle bundle)
        {
            _shareManager.Unregister();
            base.SaveStateToBundle(bundle);
        }

        public async Task TryGetPageAsync(string headerId)
        {
            PageLoading = true;
            //Classes and Id's you want to remove from the html content
            Regex classes = new Regex("meta__extras|submeta|l-footer|content-footer");
            Regex ids = new Regex("bannerandheader");
            try
            {
                ArticleContent = await _httpService
                    .GetHtmlContentAsync(Constants.BASE_WEB_URL + headerId, classes, ids);
                NoConnection = false;
            }
            catch (System.Exception)
            {
                NoConnection = true;
            }
            PageLoading = false;
        }
    }
}