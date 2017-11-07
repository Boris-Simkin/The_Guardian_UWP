using MvvmCross.Platform;
using MvvmCross.Platform.IoC;

namespace TheGuardian.Core
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            Mvx.LazyConstructAndRegisterSingleton<HttpService, HttpService>();

            RegisterAppStart<ViewModels.MainViewModel>();
        }
    }
}
