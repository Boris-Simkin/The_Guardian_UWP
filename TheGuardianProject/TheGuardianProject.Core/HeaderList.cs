using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TheGuardian.Core.Annotations;
using TheGuardian.Core.Models;

namespace TheGuardian.Core
{
    public class HeaderList : INotifyPropertyChanged
    {
       // private ObservableCollection<StoryHeader> _items;

        public HeaderList()
        {
            //_items = new ObservableCollection<StoryHeader>();
           // Items = new ObservableCollection<StoryHeader>(_items);
        }
        public ObservableCollection<StoryHeader> Items { get; set; }

        //public void AddItem(string description)
        //{
        //    _items.Add(new StoryHeader { Description = description});
        //}

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
