using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using TheGuardian.Core.Models;
using HtmlAgilityPack;
using System.Net.Http;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace TheGuardian.Core.ViewModels
{
    public class ArticleViewModel : MvxViewModel
    {
        private readonly HttpService _httpService;
        private string _headerId;
        public ArticleViewModel(HttpService httpService)
        {
            _httpService = httpService;
        }

        public async void Init(StoryHeader parameter)
        {
            ArticleContent = string.Empty;
            _headerId = parameter.Id;
            await TryGetPageAsync(_headerId);
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

        private bool _noConnection;
        public bool NoConnection
        {
            get { return _noConnection; }
            set { SetProperty(ref _noConnection, value); }
        }

        private string _articleContent;

        public string ArticleContent
        {
            get { return _articleContent; }
            private set { SetProperty(ref _articleContent, value); }
        }

        private bool _pageLoading;
        public bool PageLoading
        {
            get { return _pageLoading; }
            private set { SetProperty(ref _pageLoading, value); }
        }

        private MvxCommand _goBackCommand;
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

        private MvxAsyncCommand _reloadCommand;
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

        private MvxAsyncCommand _shareCommand;
        public MvxAsyncCommand ShareCommand
        {
            get
            {
                return _shareCommand ?? (_shareCommand = new MvxAsyncCommand(async () =>
                {
                    
                }));
            }
        }

        
    }
}