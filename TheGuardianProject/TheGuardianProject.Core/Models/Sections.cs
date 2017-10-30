using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGuardianProject.Core.Models
{
    public class Sections
    {
        private Dictionary<string, string> _sectionsDict = new Dictionary<string, string>()
        {
            { "Main", "search" },
            { "Technology", "technology" },
            { "Opinion", "commentisfree" },
            { "Culture", "culture" },
            { "Travel", "travel" },
            { "World", "world" },
            { "Business", "business" },
        };
        public Dictionary<string, string> SectionsDict => _sectionsDict;
    }
}
