using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGuardianProject.Core;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;

namespace TheGuardianProject.UWP
{
    public class TileManager : ITileManager
    {
        public string TappedTileId { get; set; }
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

        public async Task<bool> PinSecondaryTile(string tileId, string displayName, string arguments)
        {
            // Initialize the tile with required arguments
            SecondaryTile tile = new SecondaryTile(
                tileId,
                displayName,
                arguments,
                // The tile is set with an image corresponding to the section
                new Uri($"ms-appx:///Assets/{displayName}.png"),
                TileSize.Default);

            // Pin the tile
            return await tile.RequestCreateAsync();
        }

        public async Task<bool> UnpinSecondaryTileAsync(string tileId)
        {
            // Initialize a secondary tile with the same tile ID you want removed
            SecondaryTile toBeDeleted = new SecondaryTile(tileId);
            try
            {
                return await toBeDeleted.RequestDeleteAsync();
            }
            catch (Exception)
            {
                return true;
            }
        }

        public bool IsPinned(string tileId)
        {
            // Check if the secondary tile is pinned
            return SecondaryTile.Exists(tileId);
        }
    }
}
