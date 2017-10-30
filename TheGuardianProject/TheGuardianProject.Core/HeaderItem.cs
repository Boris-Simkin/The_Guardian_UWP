using System.ComponentModel;
using System.Runtime.CompilerServices;
using TheGuardian.Core.Annotations;

namespace TheGuardian.Core
{
    //public class HeaderItem : INotifyPropertyChanged
    //{
    //    private string _description;
    //    //private bool _completed;

    //    public string Description
    //    {
    //        get { return _description; }
    //        set
    //        {
    //            if (value == _description) return;
    //            _description = value;
    //            OnPropertyChanged();
    //        }
    //    }

    //    //public bool Completed
    //    //{
    //    //    get { return _completed; }
    //    //    set
    //    //    {
    //    //        if (value == _completed) return;
    //    //        _completed = value;
    //    //        OnPropertyChanged();
    //    //    }
    //    //}

    //    public event PropertyChangedEventHandler PropertyChanged;

    //    [NotifyPropertyChangedInvocator]
    //    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    //    {
    //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    //    }
    //}
}