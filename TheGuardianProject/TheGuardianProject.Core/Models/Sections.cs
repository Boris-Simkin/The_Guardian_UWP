using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGuardianProject.Core.Models
{
    public class Sections : IEnumerable<Section>
    {

        public Sections()
        {
            _current = _list.First();
        }

        List<Section> _list = new List<Section>
        {
                new Section( "Main", "search" ),
                new Section( "Technology", "technology" ),
                new Section( "Opinion", "commentisfree" ),
                new Section( "Culture", "culture" ),
                new Section( "Travel", "travel" ),
                new Section( "World", "world" ),
                new Section( "Business", "business" ),
        };

        private Section _current;
        public Section Current
        {
            get { return _current; }
            set { if (value != null) _current = value; }
        }

        public Section ByName(string name)
        {
            return _list.Where(s => s.Name == name).FirstOrDefault();
        }

        public Section ByAddress(string address)
        {
            return _list.Where(s => s.Address == address).FirstOrDefault();
        }

        public IEnumerator<Section> GetEnumerator()
        {
            foreach (var section in _list)
                yield return section;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
