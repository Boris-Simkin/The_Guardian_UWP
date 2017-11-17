using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGuardianProject.Core;
using Windows.ApplicationModel.DataTransfer;

namespace TheGuardianProject.UWP
{
    public class ShareManager : IShareManager
    {
        DataTransferManager _dataTransferManager;
        private string _link;
        public ShareManager()
        {
            _dataTransferManager = DataTransferManager.GetForCurrentView();
        }
        public void Register()
        {
            _dataTransferManager.DataRequested += DataTransferManager_DataRequested;
        }

        public void Unregister()
        {
            _dataTransferManager.DataRequested -= DataTransferManager_DataRequested;
        }
        public void Share(string link)
        {
            _link = link;
            DataTransferManager.ShowShareUI();
        }

        private void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            request.Data.Properties.Title = "Share the article";
            request.Data.SetWebLink(new Uri(_link));
        }
    }
}
