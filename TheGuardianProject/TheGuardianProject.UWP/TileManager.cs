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
            // Construct a unique tile ID, which you will need to use later for updating the tile
            //string tileId = "City";

            //// Use a display name you like
            //string displayName = "Secondary tile test";

            //// Provide all the required info in arguments so that when user
            //// clicks your tile, you can navigate them to the correct content
            //string arguments = "action=viewCity&zipCode=";

            // Initialize the tile with required arguments
            SecondaryTile tile = new SecondaryTile(
                tileId,
                displayName,
                arguments,
                new Uri("ms-appx:///Assets/Square150x150Logo.png"),
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
