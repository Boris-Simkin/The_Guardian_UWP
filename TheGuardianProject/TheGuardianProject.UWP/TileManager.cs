using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGuardianProject.Core;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace TheGuardianProject.UWP
{
    public class TileManager : ITileManager
    {
        public TileManager()
        {
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
        }

        public void SetImageTile(string url)
        {
            var tileXml =
                TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150Image);

            var tileAttributes = tileXml.GetElementsByTagName("image");

            ((XmlElement)tileAttributes[0]).SetAttribute("src", url);
            var tileNotification = new TileNotification(tileXml);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }

        public void SetTextTile(string header, string title)
        {
            var tileXml =
                TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150Text02);

            var tileAttributes = tileXml.GetElementsByTagName("text");

            tileAttributes[0].AppendChild(tileXml.CreateTextNode(header));
            tileAttributes[1].AppendChild(tileXml.CreateTextNode(title));
            var tileNotification = new TileNotification(tileXml);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }
    }
}
