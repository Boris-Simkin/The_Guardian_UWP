using Windows.UI.Xaml.Controls;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using MvvmCross.Uwp.Platform;
using MvvmCross.Platform;
using TheGuardianProject.Core;
using TheGuardianProject.UWP;

namespace TheGuardian.UWP
{
    public class Setup : MvxWindowsSetup
    {
        public Setup(Frame rootFrame) : base(rootFrame)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            Mvx.LazyConstructAndRegisterSingleton<ILocalSettings, LocalSettings>();
            return new Core.App();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }
    }
}
