using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGuardianProject.Core
{
    public interface ILocalSettings
    {
        void Save<T>(string key, T value);
        T Load<T>(string key);
    }
}
