using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGuardianProject.Core;
using Windows.Storage;

namespace TheGuardianProject.UWP
{
    public class LocalSettings : ILocalSettings
    {
        ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;

        public T Load<T>(string key)
        {
            return (T)_localSettings.Values[key];
        }

        public void Save<T>(string key, T value)
        {
            _localSettings.Values[key] = value;
        }

    }
}
