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
        public ArticleViewModel(HttpService httpService)
        {
            _httpService = httpService;
        }

        public async void Init(StoryHeader parameter)
        {
            ArticleContent = string.Empty;
            PageLoading = true;
            //Classes and Id's you want to remove from the html content
            Regex classes = new Regex("meta__extras|submeta|l-footer|content-footer");
            Regex ids = new Regex("bannerandheader");

            ArticleContent = await _httpService
                .GetHtmlContentAsync(Constants.BASE_WEB_URL + parameter.Id, classes, ids);
            PageLoading = false;
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

    }
}