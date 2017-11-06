using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGuardianProject.Core
{
    public class SucceedEventArgs : EventArgs
    {
        public SucceedEventArgs(bool succeed)
        {
            Succeed = succeed;
        }
        public bool Succeed { get; set; }
    }
}
