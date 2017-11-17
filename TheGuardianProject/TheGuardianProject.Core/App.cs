using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using TheGuardianProject.Core.Models;

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
            Mvx.LazyConstructAndRegisterSingleton<Headers, Headers>();
            Mvx.LazyConstructAndRegisterSingleton<Sections, Sections>();
            RegisterAppStart<ViewModels.MainViewModel>();
        }
    }
}
