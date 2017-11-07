using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGuardianProject.Core
{
    public interface ITileManager
    {
        void SetTextTile(string sectionName, string title);
        void SetImageTile(string url);
        Task<bool> PinSecondaryTile(string tileId, string displayName, string arguments);
        Task<bool> UnpinSecondaryTileAsync(string tileId);
        bool IsPinned(string tileId);
        string TappedTileId { get; set; }
    }
}
