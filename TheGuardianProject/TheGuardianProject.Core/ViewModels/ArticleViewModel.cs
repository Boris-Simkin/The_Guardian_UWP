using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using TheGuardian.Core.Models;

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
        }

        public string ArticleUri { get; set; }


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