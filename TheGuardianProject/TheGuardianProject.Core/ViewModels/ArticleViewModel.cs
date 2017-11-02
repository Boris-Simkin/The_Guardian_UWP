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
        public ArticleViewModel()
        {
        }

        public void Init(StoryHeader parameter)
        {
            ArticleUri = "https://www.theguardian.com/" + parameter.Id;
            // + " & api-key=" + Constants.API_KEY;
            HtmlDocument doc = new HtmlDocument();

            //string url = "https://www.digikala.com/";  
            string result = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync(ArticleUri).Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        result = content.ReadAsStringAsync().Result;
                    }
                }
            }
            doc.LoadHtml(result);

            //Classes and Id's you want to remove from the html content
            Regex classes = new Regex("meta__extras|submeta|l-footer u-cf|content-footer");
            Regex ids = new Regex("bannerandheader");

            var toRemove = doc.DocumentNode.Descendants()
                  .Where(x => (x.Attributes.Contains("class")
                  && classes.IsMatch(x.Attributes["class"].Value)) ||
                  ids.IsMatch(x.Id)
                  ).ToList();

            //x.Id.Contains("bannerandheader")
            foreach (var node in toRemove)
                node.Remove();

            ArticleContent = doc.DocumentNode.InnerHtml;
        }

        public string ArticleUri { get; set; }

        public string ArticleContent { get; set; }


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