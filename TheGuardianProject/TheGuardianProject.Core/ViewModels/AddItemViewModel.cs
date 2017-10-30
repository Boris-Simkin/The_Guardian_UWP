using MvvmCross.Core.ViewModels;

namespace TheGuardian.Core.ViewModels
{
    public class AddItemViewModel : MvxViewModel
    {
        private readonly HeaderList _todoList;
        private MvxCommand _cancelCommand;
        private MvxCommand _saveCommand;

        public AddItemViewModel(HeaderList todoList)
        {
            _todoList = todoList;
        }
        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public MvxCommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new MvxCommand(() =>
                {
                  //  _todoList.AddItem(Description);
                    Close(this);
                }));
            }
        }

        public MvxCommand CancelCommand
        {
            get { return _cancelCommand ?? (_cancelCommand = new MvxCommand(() => { Close(this); })); }
        }
    }
}