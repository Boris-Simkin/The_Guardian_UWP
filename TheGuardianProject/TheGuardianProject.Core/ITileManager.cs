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
    }
}
