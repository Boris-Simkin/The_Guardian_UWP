using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGuardianProject.Core;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using Microsoft.Toolkit.Uwp.Notifications;

namespace TheGuardianProject.UWP
{
    public class TileManager : ITileManager
    {
        public string TappedTileId { get; set; }
        public TileManager()
        {
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
        }

        public void SetTile(string header, string title, string url)
        {
            //I want my image tile without text
            var imageTile = new TileBinding()
            {
                Content = new TileBindingContentAdaptive()
                {
                    BackgroundImage = new TileBackgroundImage()
                    {
                        Source = url
                    }
                }
            };

            var textTile = new TileBinding()
            {
                Content = new TileBindingContentAdaptive()
                {
                    Children =
                    {
                        new AdaptiveText()
                        {
                            Text = header,
                            HintStyle = AdaptiveTextStyle.Body,
                            HintWrap = true
                        },

                        new AdaptiveText()
                        {
                            Text = title,
                            HintStyle = AdaptiveTextStyle.BodySubtle,
                            HintWrap = true
                        }
                    }
                }
            };

            // Construct the tile content
            TileContent content = new TileContent()
            {
                Visual = new TileVisual()
                {
                    TileMedium = textTile,
                    TileWide = textTile,
                }
            };
            // Create the tile notification
            var notification = new TileNotification(content.GetXml());
            // Set the notification
            TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);

            // Construct the tile content
            content = new TileContent()
            {
                Visual = new TileVisual()
                {
                    TileMedium = imageTile,
                    TileWide = imageTile,
                }
            };
            // Create the tile notification
            notification = new TileNotification(content.GetXml());
            // Set the notification
            TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);

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
