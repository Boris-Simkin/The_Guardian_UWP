using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGuardianProject.Core
{
    public interface IShareManager
    {
        void Share(string link);
        void Register();
        void Unregister();
    }
}
